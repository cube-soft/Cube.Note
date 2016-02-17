﻿/* ------------------------------------------------------------------------- */
///
/// PageListView.cs
/// 
/// Copyright (c) 2010 CubeSoft, Inc.
/// 
/// Licensed under the Apache License, Version 2.0 (the "License");
/// you may not use this file except in compliance with the License.
/// You may obtain a copy of the License at
///
///  http://www.apache.org/licenses/LICENSE-2.0
///
/// Unless required by applicable law or agreed to in writing, software
/// distributed under the License is distributed on an "AS IS" BASIS,
/// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
/// See the License for the specific language governing permissions and
/// limitations under the License.
///
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
    public class PageListView : Cube.Forms.ListView
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
        /// Aggregator
        /// 
        /// <summary>
        /// イベントを集約したオブジェクトを取得または設定します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
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
        public ObservableCollection<Page> DataSource
        {
            get { return _source; }
            set
            {
                if (_source == value) return;

                if (_source != null)
                {
                    _source.CollectionChanged -= DS_CollectionChanged;
                    Detach(_source);
                }
                ClearItems();

                _source = value;
                if (_source == null) return;

                _source.CollectionChanged -= DS_CollectionChanged;
                _source.CollectionChanged += DS_CollectionChanged;
                foreach (var page in _source)
                {
                    Add(page);
                    page.PropertyChanged -= DS_PropertyChanged;
                    page.PropertyChanged += DS_PropertyChanged;
                }
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
            Replace(index, DataSource[index]);
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

            BorderStyle   = BorderStyle.None;
            Converter     = new PageConverter();
            FullRowSelect = true;
            HeaderStyle   = ColumnHeaderStyle.None;
            Margin        = new Padding(0);
            MultiSelect   = false;
            Theme         = Cube.Forms.WindowTheme.Explorer;
            TileSize      = new Size(Width, 115);
            View          = View.Tile;
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
        /// OnDoubleClick
        /// 
        /// <summary>
        /// マウスがダブルクリックされた時に実行されます。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        protected override void OnMouseDoubleClick(MouseEventArgs e)
        {
            base.OnMouseDoubleClick(e);

            if (DataSource == null || SelectedIndices.Count <= 0) return;
            var page = DataSource[SelectedIndices[0]];
            Aggregator?.Property.Raise(new ValueEventArgs<Page>(page));
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
                case NotifyCollectionChangedAction.Remove:
                    Detach(e.OldItems);
                    RemoveItems(new int[] { e.OldStartingIndex });
                    break;
                case NotifyCollectionChangedAction.Reset:
                    if (DataSource.Count > 0) break;
                    Detach(e.OldItems);
                    ClearItems();
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
            if (index < 0) return;

            switch (e.PropertyName)
            {
                case nameof(page.Abstract):
                    Items[index].Text = page.GetAbstract();
                    break;
                case nameof(page.Creation):
                case nameof(page.LastUpdate):
                    Replace(index, page);
                    break;
                default:
                    break;
            }
        }

        #endregion

        #region Others

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
                new ColumnHeader(), // CreationTime
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

            var height = Math.Max(TileSize.Height, 1);
            var width  = Height < height * Count ?
                         Math.Max(Width - SystemInformation.VerticalScrollBarWidth, 1) :
                         Math.Max(Width, 1);

            if (width == TileSize.Width && height == TileSize.Height) return;

            TileSize = new Size(width, height);
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
            foreach (Page page in pages) page.PropertyChanged -= DS_PropertyChanged;
        }

        #endregion

        #region Fields
        private ObservableCollection<Page> _source;
        #endregion
    }
}
