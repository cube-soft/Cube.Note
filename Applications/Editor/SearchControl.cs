﻿/* ------------------------------------------------------------------------- */
///
/// SearchControl.cs
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
using System.Collections.Generic;
using System.Windows.Forms;

namespace Cube.Note.App.Editor
{
    /* --------------------------------------------------------------------- */
    ///
    /// SearchControl
    /// 
    /// <summary>
    /// 検索画面を表示するためのクラスです。
    /// </summary>
    /// 
    /* --------------------------------------------------------------------- */
    public partial class SearchControl : Cube.Forms.UserControl
    {
        #region Constructors

        /* ----------------------------------------------------------------- */
        ///
        /// SearchControl
        /// 
        /// <summary>
        /// オブジェクトを初期化します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public SearchControl()
        {
            InitializeComponent();
            Dock = DockStyle.Fill;
            RemoveButton.Click += (s, e) => Pop();
        }

        #endregion

        #region Methods

        /* ----------------------------------------------------------------- */
        ///
        /// Stash
        /// 
        /// <summary>
        /// 指定されたコントロールから他の子要素を退避させ、自身を挿入します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public void Stash(Control parent)
        {
            if (Parent == parent) return;

            _controls.Clear();
            foreach (Control control in parent.Controls) _controls.Add(control);
            parent.Controls.Clear();
            parent.Controls.Add(this);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Pop
        /// 
        /// <summary>
        /// 退避させた子要素を元に戻します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public void Pop()
        {
            if (Parent == null) return;
            var parent = Parent;
            Parent.Controls.Remove(this);
            foreach (var control in _controls) parent.Controls.Add(control);
            _controls.Clear();
        }

        #endregion

        #region Fields
        private IList<Control> _controls = new List<Control>();
        #endregion
    }
}
