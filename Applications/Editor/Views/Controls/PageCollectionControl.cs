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
using System.Windows.Forms;

namespace Cube.Note.App.Editor
{
    /* --------------------------------------------------------------------- */
    ///
    /// PageCollectionControl
    ///
    /// <summary>
    /// ページ一覧を表示するためのクラスです。
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public partial class PageCollectionControl : Cube.Forms.ControlBase, IDpiAwarable
    {
        #region Constructors

        /* ----------------------------------------------------------------- */
        ///
        /// ItemListControl
        ///
        /// <summary>
        /// オブジェクトを初期化します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public PageCollectionControl()
        {
            InitializeComponent();

            NewPageButton.Click += (s, e)
                => Aggregator.Get()?.NewPage.Publish(ValueEventArgs.Create(0));

            Pages.AllowDrop = true;
            Pages.KeyDown += (s, e) => OnKeyDown(e);
        }

        #endregion

        #region Properties

        /* ----------------------------------------------------------------- */
        ///
        /// Pages
        ///
        /// <summary>
        /// ページ一覧を表示する ListView オブジェクトを取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public PageListView Pages => PageListView;

        /* ----------------------------------------------------------------- */
        ///
        /// Tags
        ///
        /// <summary>
        /// タグ一覧を表示する ComboBox オブジェクトを取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public ComboBox Tags => TagComboBox;

        #endregion

        #region Methods

        /* ----------------------------------------------------------------- */
        ///
        /// UpdateLayout
        ///
        /// <summary>
        /// オブジェクトを初期化します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public void UpdateLayout(double ratio)
        {
            var top    = (int)(32 * ratio);
            var bottom = (int)(28 * ratio);
            var margin = (top - TagComboBox.Height) / 2;

            LayoutPanel.RowStyles[0].Height = top;
            LayoutPanel.RowStyles[4].Height = bottom;
            TagComboBox.Margin = new Padding(4, margin, 4, 0);
            NewPageButton.Image = Images.Get("add", ratio);
        }

        #endregion

        #region Override methods

        /* ----------------------------------------------------------------- */
        ///
        /// OnCreateControl
        ///
        /// <summary>
        /// コントロール生成時に実行されます。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        protected override void OnCreateControl()
        {
            base.OnCreateControl();
            ActiveControl = Pages;
        }

        /* ----------------------------------------------------------------- */
        ///
        /// OnKeyDown
        ///
        /// <summary>
        /// キーが押下された時に実行されます。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        protected override void OnKeyDown(KeyEventArgs e)
        {
            try
            {
                var result = true;
                switch (e.KeyCode)
                {
                    case Keys.Delete:
                        Aggregator.Get()?.Remove.Publish(EventAggregator.Selected);
                        break;
                    default:
                        result = false;
                        break;
                }
                e.Handled = result;
            }
            finally { base.OnKeyDown(e); }
        }

        #endregion
    }
}
