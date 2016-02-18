﻿/* ------------------------------------------------------------------------- */
///
/// AutoSaver.cs
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
using System.Threading.Tasks;
using System.Timers;
using Cube.Note.Azuki;

namespace Cube.Note
{
    /* --------------------------------------------------------------------- */
    ///
    /// AutoSaver
    /// 
    /// <summary>
    /// PageCollection オブジェクトの自動保存を行うクラスです。
    /// </summary>
    /// 
    /// <remarks>
    /// 最初の読み込み処理もこのクラスが担います。
    /// </remarks>
    /// 
    /* --------------------------------------------------------------------- */
    public class AutoSaver : IDisposable
    {
        #region Constructors and destructors

        /* ----------------------------------------------------------------- */
        ///
        /// AutoSaver
        /// 
        /// <summary>
        /// オブジェクトを初期化します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public AutoSaver(PageCollection pages, SettingsFolder settings)
        {
            Pages    = pages;
            Settings = settings;
            Interval = Settings.User.AutoSaveTime;

            Settings.Current.PageChanged += Settings_PageChanged;
            Settings.User.PropertyChanged += Settings_PropertyChanged;
            _timer.Elapsed += Timer_Elapsed;

            Task.Run(() => InitializePages());
        }

        /* ----------------------------------------------------------------- */
        ///
        /// AutoSaver
        /// 
        /// <summary>
        /// オブジェクトを破棄します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        ~AutoSaver()
        {
            Dispose(false);
        }

        #endregion

        #region Properties

        /* ----------------------------------------------------------------- */
        ///
        /// Target
        /// 
        /// <summary>
        /// 自動保存の対象となるオブジェクトを取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public PageCollection Pages { get; }

        /* ----------------------------------------------------------------- */
        ///
        /// Settings
        /// 
        /// <summary>
        /// 設定オブジェクトを取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public SettingsFolder Settings { get; }

        /* ----------------------------------------------------------------- */
        ///
        /// Interval
        /// 
        /// <summary>
        /// 自動保存を実行する間隔を取得または設定します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public TimeSpan Interval
        {
            get { return TimeSpan.FromMilliseconds(_timer.Interval); }
            set { _timer.Interval = value.TotalMilliseconds; }
        }

        #endregion

        #region Methods

        /* ----------------------------------------------------------------- */
        ///
        /// Dispose
        /// 
        /// <summary>
        /// オブジェクトを破棄する際に必要な終了処理を実行します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion

        #region Event handlers

        /* ----------------------------------------------------------------- */
        ///
        /// Timer_Elapsed
        /// 
        /// <summary>
        /// 一定時間毎に実行されるハンドラです。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private async void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            await Task.Run(() =>
            {
                SaveDocument(Settings.Current.Page);
                SaveOrderFile();
            });
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Target_ActiveChanged
        /// 
        /// <summary>
        /// アクティブな Page オブジェクトが変更された時に実行される
        /// ハンドラです。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private async void Settings_PageChanged(object sender, ValueChangedEventArgs<Page> e)
        {
            await Task.Run(() => SaveDocument(e.OldValue));
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Settings_PropertyChanged
        /// 
        /// <summary>
        /// 設定内容が変更された時に実行されるハンドラです。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void Settings_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName != nameof(Settings.User.AutoSaveTime)) return;
            Interval = Settings.User.AutoSaveTime;
        }

        #endregion

        #region Virtual methods

        /* ----------------------------------------------------------------- */
        ///
        /// Dispose
        /// 
        /// <summary>
        /// オブジェクトを破棄する際に必要な終了処理を実行します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        protected virtual void Dispose(bool disposing)
        {
            if (_disposed) return;
            _disposed = true;

            _timer.Stop();
            Settings.Current.PageChanged -= Settings_PageChanged;
            SaveAllDocuments();
            SaveOrderFile();
            SaveSettingsFile();

            if (disposing) _timer.Dispose();
        }

        #endregion

        #region Other private methods

        /* ----------------------------------------------------------------- */
        ///
        /// InitializeTarget
        ///
        /// <summary>
        /// 対象とする PageCollection オブジェクトを初期化します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void InitializePages()
        {
            try
            {
                if (Pages == null) return;
                Pages.Load();
                if (Pages.Count <= 0) Pages.NewPage();
            }
            finally { _timer.Start(); }
        }

        /* ----------------------------------------------------------------- */
        ///
        /// SaveDocument
        ///
        /// <summary>
        /// Document オブジェクトを保存します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void SaveDocument(Page page) => page?.SaveDocument(Pages.Directory);

        /* ----------------------------------------------------------------- */
        ///
        /// SaveAllDocuments
        ///
        /// <summary>
        /// 全ての Document オブジェクトを保存します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void SaveAllDocuments()
        {
            foreach (var page in Pages) page?.SaveDocument(Pages.Directory);
        }

        /* ----------------------------------------------------------------- */
        ///
        /// SaveOrderFile
        ///
        /// <summary>
        /// 定義ファイルを保存します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void SaveOrderFile() => Pages?.Save();

        /* ----------------------------------------------------------------- */
        ///
        /// SaveSettingsFile
        ///
        /// <summary>
        /// 設定ファイルを保存します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void SaveSettingsFile() => Settings?.Save();

        #endregion

        #region Fields
        private bool _disposed = false;
        private Timer _timer = new Timer();
        #endregion
    }
}