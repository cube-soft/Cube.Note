﻿/* ------------------------------------------------------------------------- */
///
/// PageCollectionPresenter.cs
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
using System.Collections.Specialized;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Cube.Note.App.Editor
{
    /* --------------------------------------------------------------------- */
    ///
    /// PageCollectionPresenter
    /// 
    /// <summary>
    /// PageCollectionControl とモデルを関連付けるためのクラスです。
    /// </summary>
    /// 
    /* --------------------------------------------------------------------- */
    public class PageCollectionPresenter : Cube.Forms.PresenterBase<PageCollectionControl, PageCollection>
    {
        #region Constructors

        /* ----------------------------------------------------------------- */
        ///
        /// PageCollectionPresenter
        /// 
        /// <summary>
        /// オブジェクトを初期化します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public PageCollectionPresenter(PageCollectionControl view, PageCollection model)
            : base(view, model)
        {
            View.FindForm().FormClosing += View_Closing;
            View.SelectedIndexChanged   += View_SelectedIndexChanged;
            View.NewPageRequired        += View_NewPageRequired;
            View.Removed                += View_Removed;

            Model.CollectionChanged += Model_CollectionChanged;

            InitializeModel();
        }

        #endregion

        #region Event handlers

        #region View

        /* ----------------------------------------------------------------- */
        ///
        /// View_Closing
        /// 
        /// <summary>
        /// フォームを閉じる時に実行されるハンドラです。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void View_Closing(object sender, FormClosingEventArgs e)
        {
            if (Model.Active != null) Model.Active.SaveDocument(Model.Directory);
            Model.Save(Properties.Resources.OrderFileName);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// View_SelectedIndexChanged
        /// 
        /// <summary>
        /// 選択項目が変更された時に実行されるハンドラです。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void View_SelectedIndexChanged(object sender, EventArgs e)
        {
            var index = View.SelectedIndex;
            if (index < 0 || index >= Model.Count) return;
            Model.Active = Model[index];
        }

        /* ----------------------------------------------------------------- */
        ///
        /// View_NewPageRequired
        /// 
        /// <summary>
        /// 新しいページの作成要求が発生した時に実行されるハンドラです。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void View_NewPageRequired(object sender, EventArgs e)
        {
            Model.NewPage();
            Post(() => View.Select(0));
        }

        /* ----------------------------------------------------------------- */
        ///
        /// View_Removed
        /// 
        /// <summary>
        /// ページが削除された時に実行されるハンドラです。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void View_Removed(object sender, DataEventArgs<int> e)
        {
            var index = e.Value;
            if (index < 0 || index >= Model.Count) return;

            Model[index].PropertyChanged -= Model_PropertyChanged;
            Model.RemoveAt(index);
        }

        #endregion

        #region Model

        /* ----------------------------------------------------------------- */
        ///
        /// Model_CollectionChanged
        /// 
        /// <summary>
        /// コレクションの内容に変更があった時に実行されるハンドラです。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void Model_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    var index = e.NewStartingIndex;
                    Model[index].PropertyChanged -= Model_PropertyChanged;
                    Model[index].PropertyChanged += Model_PropertyChanged;
                    Send(() => View.Insert(index, Model[index]));
                    break;
                case NotifyCollectionChangedAction.Remove:
                    if (Model.Count <= 0) Model.NewPage();
                    break;
            }
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Model_PropertyChanged
        /// 
        /// <summary>
        /// プロパティの値が変化した時に実行されるハンドラです。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void Model_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName != "Abstract") return;

            var page = sender as Page;
            if (page == null) return;

            var index = Model.IndexOf(page);
            if (index == -1) return;

            Post(() => View.UpdateText(index, page.GetAbstract()));
        }

        #endregion

        #endregion

        #region Other private methods

        /* ----------------------------------------------------------------- */
        ///
        /// InitializeModel
        /// 
        /// <summary>
        /// モデルを初期化します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void InitializeModel()
        {
            Task.Run(() =>
            {
                Model.Load(Properties.Resources.OrderFileName);
                if (Model.Count <= 0) Model.NewPage();
            });
        }

        #endregion
    }
}
