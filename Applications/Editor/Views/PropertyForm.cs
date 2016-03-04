﻿/* ------------------------------------------------------------------------- */
///
/// PropertyForm.cs
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
using System.Collections.Generic;
using System.Windows.Forms;

namespace Cube.Note.App.Editor
{
    /* --------------------------------------------------------------------- */
    ///
    /// PropertyForm
    /// 
    /// <summary>
    /// ページのプロパティを表示するためのクラスです。
    /// </summary>
    /// 
    /* --------------------------------------------------------------------- */
    public partial class PropertyForm : FormBase
    {
        #region Constructors

        /* ----------------------------------------------------------------- */
        ///
        /// PropertyForm
        ///
        /// <summary>
        /// オブジェクトを初期化します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public PropertyForm()
        {
            InitializeComponent();
            InitializeEvents();

            Caption = TitleControl;
        }

        /* ----------------------------------------------------------------- */
        ///
        /// PropertyForm
        ///
        /// <summary>
        /// オブジェクトを初期化します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public PropertyForm(Page src, IEnumerable<Tag> tags) : this()
        {
            InitializeLayout(src, tags);
        }

        #endregion

        #region Properties

        /* ----------------------------------------------------------------- */
        ///
        /// Tags
        ///
        /// <summary>
        /// ページに関連付けられたタグ一覧を取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public IList<string> Tags { get; } = new List<string>();

        #endregion

        #region Initialize methods

        /* ----------------------------------------------------------------- */
        ///
        /// InitializeEvents
        ///
        /// <summary>
        /// 各種イベントを初期化します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void InitializeEvents()
        {
            ApplyButton.Click += ApplyButton_Click;
            NewTagButton.Click += NewTagButton_Click;
        }

        /* ----------------------------------------------------------------- */
        ///
        /// InitializeLayout
        ///
        /// <summary>
        /// レイアウトを初期化します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void InitializeLayout(Page src, IEnumerable<Tag> tags)
        {
            AbstractLabel.Text = src.GetAbstract();
            CreationLabel.Text = src.Creation.ToString(Properties.Resources.CreationFormat);
            LastUpdateLabel.Text = src.LastUpdate.ToString(Properties.Resources.LastUpdateFormat);

            var margin = Math.Max((NewTagWrapper.Height - NewTagTextBox.Height) / 2 - 1, 0);
            NewTagWrapper.Padding = new Padding(4, margin, 0, 0);

            foreach (var tag in tags)
            {
                var button = new TagButton(tag);
                button.Name = tag.Name;
                if (src.Tags.Contains(tag.Name)) button.Checked = true;
                TagsPanel.Controls.Add(button);
            }
        }

        #endregion

        #region Event handlers

        /* ----------------------------------------------------------------- */
        ///
        /// ApplyButton_Click
        ///
        /// <summary>
        /// OK ボタンが押下される時に実行されるハンドラです。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void ApplyButton_Click(object sender, EventArgs e)
        {
            Tags.Clear();
            foreach (TagButton button in TagsPanel.Controls)
            {
                if (button.Checked) Tags.Add(button.Text);
            }
            Close();
        }

        /* ----------------------------------------------------------------- */
        ///
        /// NewTagButton_Click
        ///
        /// <summary>
        /// 新しいタグが追加される時に実行されるハンドラです。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void NewTagButton_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(NewTagTextBox.Text)) return;

                var tag = NewTagTextBox.Text.Trim();
                if (string.IsNullOrEmpty(tag)) return;

                var contains = TagsPanel.Controls.ContainsKey(tag);
                var button   = contains ?
                               TagsPanel.Controls[tag] as TagButton :
                               new TagButton(tag);
                button.Name = tag;
                button.Checked = true;

                if (!contains) TagsPanel.Controls.Add(button);
            }
            finally { NewTagTextBox.Text = string.Empty; }
        }

        #endregion
    }
}