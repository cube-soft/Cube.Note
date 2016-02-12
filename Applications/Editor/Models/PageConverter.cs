﻿/* ------------------------------------------------------------------------- */
///
/// PageConverter.cs
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
using System.Linq;

namespace Cube.Note.App.Editor
{
    /* --------------------------------------------------------------------- */
    ///
    /// PageConverter
    /// 
    /// <summary>
    /// Page オブジェクトを ListViewItem オブジェクトに変換するための
    /// クラスです。
    /// </summary>
    /// 
    /* --------------------------------------------------------------------- */
    public class PageConverter : Cube.Forms.IListViewItemConverter
    {
        #region Methods

        /* ----------------------------------------------------------------- */
        ///
        /// Convert
        ///
        /// <summary>
        /// ListViewItem オブジェクトに変換します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public ListViewItem Convert<T>(T src)
        {
            var page = src as Page;
            if (page == null) return new ListViewItem(src.ToString());

            var dest = new List<string>();
            dest.Add(page.GetAbstract());
            dest.Add(page.Creation.ToString(Properties.Resources.CreationFormat));
            dest.Add(page.LastUpdate.ToString(Properties.Resources.LastUpdateFormat));
            dest.Add(string.Join(", ", page.Tags));
            return new ListViewItem(dest.ToArray());
        }

        #endregion
    }
}
