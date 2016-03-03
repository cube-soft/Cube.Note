﻿/* ------------------------------------------------------------------------- */
///
/// SearchPresenter.cs
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
using Cube.Note.Azuki;

namespace Cube.Note.App.Editor
{
    /* --------------------------------------------------------------------- */
    ///
    /// SearchPresenter
    /// 
    /// <summary>
    /// SearchControl とモデルを関連付けるためのクラスです。
    /// </summary>
    /// 
    /* --------------------------------------------------------------------- */
    public class SearchPresenter : 
        PresenterBase<SearchForm, SearchReplace>
    {
        #region Constructors

        /* ----------------------------------------------------------------- */
        ///
        /// SearchPresenter
        /// 
        /// <summary>
        /// オブジェクトを初期化します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public SearchPresenter(SearchForm view, PageCollection parent,
            SettingsFolder settings, EventAggregator events)
            : base(view, new SearchReplace(parent), settings, events)
        {
            Events.Search.Handle += SearchMode_Handle;
            Settings.Current.PageChanged += Settings_PageChanged;

            View.Showing += View_Showing;
            View.Hiding += View_Hiding;
            View.Search += View_Search;
            View.SearchNext += (s, e) => Model.Forward();
            View.SearchPrev += (s, e) => Model.Back();
            View.Pages.SelectedIndexChanged += View_SelectedIndexChanged;
            View.Pages.DataSource = Model.Results;
            View.Aggregator = Events;

            Model.CurrentChanged += Model_CurrentChanged;
        }

        #endregion

        #region Event handlers

        #region EventAggregator

        /* ----------------------------------------------------------------- */
        ///
        /// SearchMode_Handle
        /// 
        /// <summary>
        /// 検索画面を表示します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void SearchMode_Handle(object sender, ValueEventArgs<int> e)
        {
            Sync(() =>
            {
                View.Show();

                var count = View.SearchRange.Items.Count;
                if (count <= 0) return;

                var index = Math.Max(Math.Min(e.Value, count - 1), 0);
                View.SearchRange.SelectedIndex = index;
            });
        }

        #endregion

        #region Settings

        /* ----------------------------------------------------------------- */
        ///
        /// Settings_PageChanged
        /// 
        /// <summary>
        /// 現在のページが変化した時に実行されるハンドラです。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void Settings_PageChanged(object sender, ValueChangedEventArgs<Page> e)
        {
            if (e.NewValue == null) return;

            var index = Model.Results.IndexOf(e.NewValue);
            if (index < 0 || index >= Model.Results.Count) return;

            Sync(() => View.Pages.Select(index));
        }

        #endregion

        #region View

        /* ----------------------------------------------------------------- */
        ///
        /// View_Search
        /// 
        /// <summary>
        /// 検索を実行します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private async void View_Search(object sender, EventArgs e)
        {
            var keyword   = View.Keyword;
            var sensitive = View.CaseSensitive;
            var one       = View.SearchRange.SelectedIndex == 0;

            if (string.IsNullOrEmpty(keyword)) return;

            await Async(() =>
            {
                if (one) Model.Search(keyword, sensitive, Settings.Current.Page);
                else Model.Search(keyword, sensitive, Model.Pages.Everyone);
            });

            if (!one && Model.Results.Count > 0)
            {
                View.ShowPages = true;
                View.Pages.Select(0);
            }
            else View.ShowPages = false;
        }

        /* ----------------------------------------------------------------- */
        ///
        /// View_Showing
        /// 
        /// <summary>
        /// 表示時に実行されるハンドラです。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void View_Showing(object sender, CancelEventArgs e)
           => SyncWait(() => ResetSearchRange());

        /* ----------------------------------------------------------------- */
        ///
        /// View_Hiding
        /// 
        /// <summary>
        /// 非表示時に実行されるハンドラです。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private async void View_Hiding(object sender, CancelEventArgs e)
        {
            View.Found = -1;
            View.ShowPages = false;

            await Async(() =>
            {
                Model.Reset();
                Events.Refresh.Raise();
            });
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
            var page = SelectedPage();
            if (page == null) return;
            Settings.Current.Page = page;
            Events.Refresh.Raise();
        }

        #endregion

        #region Model

        /* ----------------------------------------------------------------- */
        ///
        /// Model_CurrentChanged
        /// 
        /// <summary>
        /// Current の内容が変化した時に実行されるハンドラです。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void Model_CurrentChanged(object sender,
            ValueChangedEventArgs<SearchReplace.Position> e)
        {
            if (e.OldValue != null)
            {
                // reset selection
                SetSelection(e.OldValue.Index, e.OldValue.End, e.OldValue.End);
            }
            SetPageAndSelection(e.NewValue);
        }

        #endregion

        #endregion

        #region Others

        /* ----------------------------------------------------------------- */
        ///
        /// ResetSearchRange
        /// 
        /// <summary>
        /// 検索範囲用の項目を設定します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void ResetSearchRange()
        {
            View.SearchRange.Items.Clear();
            View.SearchRange.Items.Add(Properties.Resources.CurrentNote);
            View.SearchRange.Items.Add(Model.Pages.Everyone);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// SetSelection
        /// 
        /// <summary>
        /// 選択範囲を設定します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void SetSelection(int index, int begin, int end)
        {
            if (index < 0 || index >= Model.Results.Count) return;

            var page = Model.Results[index];
            var doc = page?.CreateDocument(Model.Pages.Directory);
            if (doc == null) return;

            Sync(() =>
            {
                doc.SetSelection(begin, end);
                if (begin == end) Events.Refresh.Raise();
            });
        }

        /* ----------------------------------------------------------------- */
        ///
        /// SetPageAndSelection
        /// 
        /// <summary>
        /// 現在のページと選択範囲を設定します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void SetPageAndSelection(SearchReplace.Position pos)
        {
            if (pos == null) return;
            if (pos.Index < 0 || pos.Index >= Model.Results.Count) return;

            var page = Model.Results[pos.Index];
            Settings.Current.Page = page;

            SetSelection(pos.Index, pos.Begin, pos.End);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// SelectedPage
        /// 
        /// <summary>
        /// 選択ページを取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private Page SelectedPage()
        {
            if (!View.Pages.AnyItemsSelected) return null;

            var pages = Model.Results;
            var index = View.Pages.SelectedIndices[0];
            return (index >= 0 && index < pages.Count) ? pages[index] : null;
        }

        #endregion
    }
}
