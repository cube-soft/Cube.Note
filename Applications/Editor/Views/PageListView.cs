﻿/* ------------------------------------------------------------------------- */
//
// Copyright (c) 2010 CubeSoft, Inc.
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//  http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
//
/* ------------------------------------------------------------------------- */
using System;
using System.ComponentModel;
using System.Collections;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Drawing;
using System.Windows.Forms;

namespace Cube.Note.App.Editor
{
    /* --------------------------------------------------------------------- */
    ///
    /// PageListView
    ///
    /// <summary>
    /// ページ一覧を表示するための ListView クラスです。
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public class PageListView : ListViewBase
    {
        #region Constructors

        /* ----------------------------------------------------------------- */
        ///
        /// PageListView
        ///
        /// <summary>
        /// オブジェクトを初期化します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public PageListView() : base() { }

        #endregion

        #region Properties

        /* ----------------------------------------------------------------- */
        ///
        /// AllowNoSelect
        ///
        /// <summary>
        /// 選択項目がゼロの状態を許可するかどうかを示す値を取得または
        /// 設定します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [Browsable(true)]
        [DefaultValue(true)]
        public bool AllowNoSelect { get; set; } = true;

        /* ----------------------------------------------------------------- */
        ///
        /// ShowRemoveButton
        ///
        /// <summary>
        /// 項目の削除ボタンを表示するかどうかを示す値を取得または設定します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [Browsable(true)]
        [DefaultValue(true)]
        public bool ShowRemoveButton { get; set; } = true;

        /* ----------------------------------------------------------------- */
        ///
        /// ShowPropertyButton
        ///
        /// <summary>
        /// プロパティ情報の編集用（タグ付け等）ボタンを表示するかどうかを
        /// 示す値を取得または設定します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [Browsable(true)]
        [DefaultValue(true)]
        public bool ShowPropertyButton { get; set; } = true;

        /* ----------------------------------------------------------------- */
        ///
        /// BaseDpi
        ///
        /// <summary>
        /// 基準となる DPI 値を取得または設定します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public double BaseDpi { get; set; } = 96.0;

        /* ----------------------------------------------------------------- */
        ///
        /// SelectedBackColor
        ///
        /// <summary>
        /// 選択項目の背景色を取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [Browsable(false)]
        public Color SelectedBackColor => Color.FromArgb(205, 232, 255);

        /* ----------------------------------------------------------------- */
        ///
        /// SelectedBorderColor
        ///
        /// <summary>
        /// 選択項目の枠色を取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [Browsable(false)]
        public Color SelectedBorderColor => Color.FromArgb(153, 209, 255);

        /* ----------------------------------------------------------------- */
        ///
        /// Aggregator
        ///
        /// <summary>
        /// イベントを集約したオブジェクトを取得または設定します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public EventAggregator Aggregator { get; set; }

        /* ----------------------------------------------------------------- */
        ///
        /// DataSource
        ///
        /// <summary>
        /// 同期するデータを取得または設定します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public ObservableCollection<Page> DataSource
        {
            get { return _source; }
            set
            {
                if (_source == value) return;
                Update(() =>
                {
                    if (_source != null)
                    {
                        _source.CollectionChanged -= DS_CollectionChanged;
                        Detach(_source);
                    }
                    ClearItems();

                    _source = value;
                    if (_source == null) return;

                    _source.CollectionChanged -= DS_CollectionChanged;
                    foreach (var page in _source)
                    {
                        page.PropertyChanged -= DS_PropertyChanged;
                        Add(page);
                        page.PropertyChanged += DS_PropertyChanged;
                    }
                    _source.CollectionChanged += DS_CollectionChanged;
                });
            }
        }

        #endregion

        #region Methods

        /* ----------------------------------------------------------------- */
        ///
        /// Update
        ///
        /// <summary>
        /// 指定された項目を更新します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public void Update(int index)
        {
            if (DataSource == null || index < 0 || index >= DataSource.Count) return;

            var src = Items[index];
            var cvt = Converter.Convert(DataSource[index]);

            Update(() =>
            {
                for (var i = 0; i < src.SubItems.Count; ++i)
                {
                    if (src.SubItems[i].Text == cvt.SubItems[i].Text) continue;
                    src.SubItems[i].Text = cvt.SubItems[i].Text;
                }
            });
        }

        #endregion

        #region Override methods

        /* ----------------------------------------------------------------- */
        ///
        /// OnAdded
        ///
        /// <summary>
        /// 項目が追加された時に実行されます。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        protected override void OnAdded(ValueEventArgs<int> e)
        {
            if (Columns.Count == 0) SetColumns();
            base.OnAdded(e);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// OnCreateControl
        ///
        /// <summary>
        /// コントロールが生成された時に実行されます。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        protected override void OnCreateControl()
        {
            base.OnCreateControl();

            var upper = Math.Max(Width - SystemInformation.VerticalScrollBarWidth, 1);

            BorderStyle    = BorderStyle.None;
            Converter      = new PageConverter();
            DoubleBuffered = true;
            FullRowSelect  = true;
            HeaderStyle    = ColumnHeaderStyle.None;
            LabelWrap      = false;
            Margin         = new Padding(0);
            MultiSelect    = false;
            OwnerDraw      = true;
            View           = View.Tile;

            SetTileSize();
        }

        /* ----------------------------------------------------------------- */
        ///
        /// OnDrawItem
        ///
        /// <summary>
        /// 項目を描画する際に実行されます。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        protected override void OnDrawItem(DrawListViewItemEventArgs e)
        {
            // base.OnDrawItem(e);
            e.DrawDefault = false;

            DrawBackground(e.Item, e.Graphics, e.Bounds);
            DrawText(e.Item, e.Graphics, e.Bounds);

            if (!SelectedIndices.Contains(e.ItemIndex)) return;
            if (ShowRemoveButton) DrawRemoveButton(e.Graphics, e.Bounds);
            if (ShowPropertyButton) DrawPropertyButton(e.Graphics, e.Bounds);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// OnMouseDown
        ///
        /// <summary>
        /// マウスがクリックされた時に実行されます。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            if (e.Button != MouseButtons.Left) return;

            var item = GetItemAt(e.Location.X, e.Location.Y);
            if (item == null) return;

            if (IsRemoveButton(e.Location, item.Bounds)) Aggregator?.Remove.Publish(EventAggregator.Selected);
            else if (IsPropertyButton(e.Location, item.Bounds)) Aggregator?.Property.Publish(EventAggregator.Selected);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// OnMouseUp
        ///
        /// <summary>
        /// マウスのボタンから離れた時に実行されます。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);

            if (AllowNoSelect || SelectedIndices.Count > 0) return;
            if (FocusedItem != null) FocusedItem.Selected = true;
        }

        /* ----------------------------------------------------------------- */
        ///
        /// OnMouseMove
        ///
        /// <summary>
        /// マウスが移動した時に実行されます。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            if (e.Button != MouseButtons.None)
            {
                if (e.Button == MouseButtons.Left) DoDragMove(e.Location);
                return;
            }

            var item = GetItemAt(e.Location.X, e.Location.Y);
            var bounds = item?.Bounds ?? Rectangle.Empty;

            Cursor = IsRemoveButton(e.Location, bounds) ||
                     IsPropertyButton(e.Location, bounds) ?
                     Cursors.Hand : Cursors.Default;
        }

        /* ----------------------------------------------------------------- */
        ///
        /// OnDragEnter
        ///
        /// <summary>
        /// 項目がドラッグ移動された時に実行されます。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        protected override void OnDragEnter(DragEventArgs e)
        {
            var prev = e.Effect;
            base.OnDragEnter(e);
            if (e.Effect != prev) return;

            e.Effect = e.Data.GetDataPresent(typeof(ListViewItem)) ?
                       DragDropEffects.Move :
                       DragDropEffects.None;
        }

        /* ----------------------------------------------------------------- */
        ///
        /// OnDragDrop
        ///
        /// <summary>
        /// 項目がドロップされた時に実行されます。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        protected override void OnDragDrop(DragEventArgs e)
        {
            base.OnDragDrop(e);

            var item = e.Data.GetData(typeof(ListViewItem)) as ListViewItem;
            if (item == null) return;

            var src = Items.IndexOf(item);
            if (src == -1) return;

            var point = PointToClient(new Point(e.X, e.Y));
            int dest = Items.IndexOf(GetItemAt(point.X, point.Y));
            if (dest == -1) dest = Items.Count - 1;

            Aggregator?.Move.Publish(ValueEventArgs.Create(dest - src));
        }

        /* ----------------------------------------------------------------- */
        ///
        /// OnRemoved
        ///
        /// <summary>
        /// 項目が削除された時に実行されます。
        /// </summary>
        ///
        /// <remarks>
        /// 追加時（スクロールバーが新たに表示されたタイミング）に関しては
        /// 必要に応じて Resize イベントが発生するようなので、OnResize で
        /// 一括対応。
        /// </remarks>
        ///
        /* ----------------------------------------------------------------- */
        protected override void OnRemoved(ValueEventArgs<int[]> e)
        {
            SetTileSize();
            base.OnRemoved(e);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// OnResize
        ///
        /// <summary>
        /// リサイズ時に実行されます。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        protected override void OnResize(EventArgs e)
        {
            SetTileSize();
            base.OnResize(e);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// OnSelectedIndexChanged
        ///
        /// <summary>
        /// 選択項目が変更された時に実行されます。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        protected override void OnSelectedIndexChanged(EventArgs e)
        {
            if (!AllowNoSelect && SelectedIndices.Count <= 0) return;
            base.OnSelectedIndexChanged(e);
        }

        #endregion

        #region DataSource event handlers

        /* ----------------------------------------------------------------- */
        ///
        /// DS_CollectionChanged
        ///
        /// <summary>
        /// コレクションの内容が変化した時に実行されるハンドラです。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void DS_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (InvokeRequired)
            {
                Invoke(new Action(() => DS_CollectionChanged(sender, e)));
                return;
            }

            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    Insert(e.NewStartingIndex, DataSource[e.NewStartingIndex]);
                    if (Count == 1 && !AllowNoSelect) Select(0);
                    Attach(e.NewItems);
                    break;
                case NotifyCollectionChangedAction.Move:
                    Update(() => MoveItem(e.OldStartingIndex, e.NewStartingIndex));
                    break;
                case NotifyCollectionChangedAction.Remove:
                    Detach(e.OldItems);
                    Update(() => RemoveItems(new int[] { e.OldStartingIndex }));
                    break;
                case NotifyCollectionChangedAction.Reset:
                    if (DataSource.Count > 0) break;
                    Detach(e.OldItems);
                    Update(() => ClearItems());
                    break;
            }
        }

        /* ----------------------------------------------------------------- */
        ///
        /// DS_CollectionChanged
        ///
        /// <summary>
        /// コレクションの内容が変化した時に実行されるハンドラです。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void DS_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (InvokeRequired)
            {
                Invoke(new Action(() => DS_PropertyChanged(sender, e)));
                return;
            }

            var page = sender as Page;
            if (page == null) return;

            var index = DataSource?.IndexOf(page) ?? -1;
            if (index < 0 || index >= Items.Count) return;

            Update(index);
        }

        #endregion

        #region Draw methods

        /* ----------------------------------------------------------------- */
        ///
        /// DrawBackground
        ///
        /// <summary>
        /// 各項目の背景を描画します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void DrawBackground(ListViewItem item, Graphics gs, Rectangle bounds)
        {
            bounds.X     += _left;
            bounds.Width -= _left;

            if (!item.Selected)
            {
                gs.FillRectangle(new SolidBrush(BackColor), bounds);
                return;
            }

            var frame = bounds;
            --frame.Width;
            --frame.Height;

            gs.FillRectangle(new SolidBrush(SelectedBackColor), bounds);
            gs.DrawRectangle(new Pen(SelectedBorderColor), frame);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// DrawText
        ///
        /// <summary>
        /// 各項目のテキストを描画します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void DrawText(ListViewItem item, Graphics gs, Rectangle bounds)
        {
            var format = new StringFormat(StringFormatFlags.NoWrap);
            format.Trimming = StringTrimming.EllipsisCharacter;

            bounds.Width -= (_left + _space);
            bounds.Height = Font.Height;
            bounds.X += (_left + _space);
            bounds.Y += ShowRemoveButton ? bounds.Height : _space;

            for (var i = 0; i < item.SubItems.Count; ++i)
            {
                var text = item.SubItems[i].Text;
                var color = (i == 0) ? Color.Black : Color.DimGray;
                gs.DrawString(text, Font, new SolidBrush(color), bounds, format);
                bounds.Y += bounds.Height;
            }
        }

        /* ----------------------------------------------------------------- */
        ///
        /// DrawRemoveButton
        ///
        /// <summary>
        /// 削除ボタンを描画します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void DrawRemoveButton(Graphics gs, Rectangle bounds)
        {
            var ratio = gs.DpiX / BaseDpi;
            var image = Images.Get("remove", ratio);
            var x = bounds.Right - image.Width - _space;
            var y = bounds.Top + _space;
            gs.DrawImage(image, x, y);

            _cacheRemove = new SizeF(image.Width, image.Height);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// DrawPropertyButton
        ///
        /// <summary>
        /// プロパティ情報編ボタンを描画します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void DrawPropertyButton(Graphics gs, Rectangle bounds)
        {
            var font = new Font(Font.FontFamily, 8, FontStyle.Regular);
            var size = GetPropertySize(gs, font);
            var height = size.Height;
            var ratio = gs.DpiX / BaseDpi;
            var image = Images.Get("property", ratio);

            var x0 = bounds.Left + _left + _space;
            var y0 = bounds.Bottom - (height + _space) + (height - image.Height) / 2.0 - 1.0;
            gs.DrawImage(image, x0, (float)y0);

            var text = Properties.Resources.ShowProperty;
            var brush = new SolidBrush(Color.Gray);

            var x1 = x0 + image.Width;
            var y1 = bounds.Bottom - (height + _space) + (height - size.Height) / 2.0;
            gs.DrawString(text, font, brush, x1, (float)y1);
        }

        #endregion

        #region Button methods

        /* ----------------------------------------------------------------- */
        ///
        /// IsSelectedArea
        ///
        /// <summary>
        /// 指定された座標が選択項目上にあるかどうかを判別します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private bool IsSelectedArea(Point point)
        {
            var item = GetItemAt(point.X, point.Y);
            if (item == null) return false;
            return SelectedIndices.Contains(item.Index);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// IsRemoveButton
        ///
        /// <summary>
        /// 削除ボタン上かどうかを判別します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private bool IsRemoveButton(Point point, Rectangle bounds)
        {
            if (!ShowRemoveButton || !IsSelectedArea(point)) return false;

            var x1 = bounds.Right - _space;
            var x0 = x1 - _cacheRemove.Width;
            var y0 = bounds.Top + _space;
            var y1 = y0 + _cacheRemove.Height;

            return point.X >= x0 && point.X <= x1 &&
                   point.Y >= y0 && point.Y <= y1;
        }

        /* ----------------------------------------------------------------- */
        ///
        /// IsPropertyButton
        ///
        /// <summary>
        /// プロパティ情報編集ボタンおよびテキスト上かどうかを判別します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private bool IsPropertyButton(Point point, Rectangle bounds)
        {
            var size = _cacheProperty;
            if (size == SizeF.Empty) return false;
            if (!ShowPropertyButton || !IsSelectedArea(point)) return false;

            var x0 = bounds.Left + _space;
            var x1 = x0 + size.Width;
            var y1 = bounds.Bottom - _space;
            var y0 = y1 - size.Height;

            return point.X >= x0 && point.X <= x1 &&
                   point.Y >= y0 && point.Y <= y1;
        }

        /* ----------------------------------------------------------------- */
        ///
        /// GetPropertySize
        ///
        /// <summary>
        /// プロパティボタンの描画領域を取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private SizeF GetPropertySize(Graphics gs, Font font)
        {
            if (_cacheProperty != SizeF.Empty) return _cacheProperty;

            var ratio = gs.DpiX / BaseDpi;
            var image  = Images.Get("property", ratio);
            var text   = Properties.Resources.ShowProperty;
            var size   = gs.MeasureString(text, font);
            var height = Math.Max(image.Height, size.Height);

            _cacheProperty = new SizeF(image.Width + size.Width, height);
            return _cacheProperty;
        }

        #endregion

        #region Others

        /* ----------------------------------------------------------------- */
        ///
        /// Update
        ///
        /// <summary>
        /// 更新処理を実行します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void Update(Action action)
        {
            try
            {
                BeginUpdate();
                action();
            }
            finally { EndUpdate(); }
        }

        /* ----------------------------------------------------------------- */
        ///
        /// MoveItem
        ///
        /// <summary>
        /// 項目を移動します。
        /// </summary>
        ///
        /// <remarks>
        /// TODO: 無条件で Selected, Focused の設定、EnsureVisible(int) を
        /// 実行しても良いか要検討。
        /// </remarks>
        ///
        /* ----------------------------------------------------------------- */
        private void MoveItem(int src, int dest)
        {
            MoveItems(new int[] { src }, dest - src);
            Items[dest].Selected = true;
            Items[dest].Focused = true;
            EnsureVisible(dest);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// SetColumns
        ///
        /// <summary>
        /// カラムを設定します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void SetColumns()
        {
            Columns.AddRange(new ColumnHeader[]
            {
                new ColumnHeader(), // Title
                new ColumnHeader(), // LastUpdateTime
                new ColumnHeader(), // Tags
            });
        }

        /* ----------------------------------------------------------------- */
        ///
        /// SetTileSize
        ///
        /// <summary>
        /// タイルサイズを設定します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void SetTileSize()
        {
            if (View != View.Tile) return;

            var count = Columns?.Count ?? 0;
            if (ShowRemoveButton)   ++count;
            if (ShowPropertyButton) ++count;

            var height = Math.Max(Font.Height * count + 8, 1);
            var width  = Height < height * Count ?
                         Math.Max(Width - SystemInformation.VerticalScrollBarWidth, 1) :
                         Math.Max(Width, 1);

            if (width == TileSize.Width && height == TileSize.Height) return;

            TileSize = new Size(width, height);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// DoDragMove
        ///
        /// <summary>
        /// ドラッグ移動を実行します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void DoDragMove(Point location)
        {
            var item = GetItemAt(location.X, location.Y);
            if (item == null) return;
            DoDragDrop(item, DragDropEffects.Move);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Attach
        ///
        /// <summary>
        /// イベントハンドラを関連付けます。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void Attach(IList pages)
        {
            if (pages == null) return;

            foreach (Page page in pages)
            {
                page.PropertyChanged -= DS_PropertyChanged;
                page.PropertyChanged += DS_PropertyChanged;
            }
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Detach
        ///
        /// <summary>
        /// イベントハンドラの関連付けを解除します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void Detach(IList pages)
        {
            if (pages == null) return;

            foreach (Page page in pages)
            {
                page.PropertyChanged -= DS_PropertyChanged;
            }
        }

        #endregion

        #region Fields
        private ObservableCollection<Page> _source;
        private SizeF _cacheProperty = SizeF.Empty;
        private SizeF _cacheRemove = SizeF.Empty;
        private static readonly int _left = 4;
        private static readonly int _space = 3;
        #endregion
    }
}
