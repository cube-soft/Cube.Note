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
using System.Drawing;
using NUnit.Framework;
using IoEx = System.IO;


namespace Cube.Note.Tests
{
    /* --------------------------------------------------------------------- */
    ///
    /// SettingsFolderTest
    ///
    /// <summary>
    /// SettingsFolder のテスト用クラスです。
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    [Parallelizable]
    [TestFixture]
    class SettingsFolderTest : FileResource
    {
        #region SetUp and TearDown

        /* ----------------------------------------------------------------- */
        ///
        /// OneTimeSetUp
        ///
        /// <summary>
        /// 1 度だけ初期化します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            Settings = new SettingsFolder(Examples);
            Settings.Load();
        }

        #endregion

        #region Properties

        /* ----------------------------------------------------------------- */
        ///
        /// Settings
        ///
        /// <summary>
        /// SettingsValue オブジェクトを取得または設定します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private SettingsFolder Settings { get; set; }

        #endregion

        #region Tests

        /* ----------------------------------------------------------------- */
        ///
        /// Properties
        ///
        /// <summary>
        /// 各種プロパティのテストを行います。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        #region Properties

        [TestCase("Settings.json")]
        public void Path(string expected)
        {
            Assert.That(
                Settings.Path,
                Is.EqualTo(IoEx.Path.Combine(Examples, expected))
            );
        }

        [TestCase("MS Gothic UI", 9.0)]
        public void User_Font(string name, double size)
        {
            Assert.That(
                Settings.User.Font,
                Is.EqualTo(new Font(name, (float)size, FontStyle.Regular, GraphicsUnit.Point))
            );
        }

        [TestCase(1, 1, 1)]
        public void User_ForeColor(int red, int green, int blue)
        {
            Assert.That(
                Settings.User.ForeColor,
                Is.EqualTo(Color.FromArgb(red, green, blue))
            );
        }

        [TestCase("Window")]
        public void User_BackColor(string expected)
        {
            Assert.That(
                Settings.User.BackColor,
                Is.EqualTo(Color.FromName(expected))
            );
        }

        [TestCase("Highlight")]
        public void User_HighlightBackColor(string expected)
        {
            Assert.That(
                Settings.User.HighlightBackColor,
                Is.EqualTo(Color.FromName(expected))
            );
        }

        [TestCase("HighlightText")]
        public void User_HighlightForeColor(string expected)
        {
            Assert.That(
                Settings.User.HighlightForeColor,
                Is.EqualTo(Color.FromName(expected))
            );
        }

        [TestCase("Control")]
        public void User_LineNumberBackColor(string expected)
        {
            Assert.That(
                Settings.User.LineNumberBackColor,
                Is.EqualTo(Color.FromName(expected))
            );
        }

        [TestCase("ControlDark")]
        public void User_LineNumberForeColor(string expected)
        {
            Assert.That(
                Settings.User.LineNumberForeColor,
                Is.EqualTo(Color.FromName(expected))
            );
        }

        [TestCase("ControlDark")]
        public void User_SpecialCharsColor(string expected)
        {
            Assert.That(
                Settings.User.SpecialCharsColor,
                Is.EqualTo(Color.FromName(expected))
            );
        }

        [TestCase("ControlDark")]
        public void User_CurrentLineColor(string expected)
        {
            Assert.That(
                Settings.User.CurrentLineColor,
                Is.EqualTo(Color.FromName(expected))
            );
        }

        [TestCase(10)]
        public void User_AutoSaveTime(int expected)
        {
            Assert.That(
                Settings.User.AutoSaveTime,
                Is.EqualTo(TimeSpan.FromSeconds(expected))
            );
        }

        [TestCase(4)]
        public void User_TabWidth(int expected)
        {
            Assert.That(
                Settings.User.TabWidth,
                Is.EqualTo(expected)
            );
        }

        [TestCase(80)]
        public void User_WordWrapCount(int expected)
        {
            Assert.That(
                Settings.User.WordWrapCount,
                Is.EqualTo(80)
            );
        }

        [TestCase(false)]
        public void User_WordWrap(bool expected)
        {
            Assert.That(
                Settings.User.WordWrap,
                Is.EqualTo(expected)
            );
        }

        [TestCase(false)]
        public void User_WordWrapAsWindow(bool expected)
        {
            Assert.That(
                Settings.User.WordWrapAsWindow,
                Is.EqualTo(expected)
            );
        }

        [TestCase(false)]
        public void User_TabToSpace(bool expected)
        {
            Assert.That(
                Settings.User.TabToSpace,
                Is.EqualTo(expected)
            );
        }

        [TestCase(true)]
        public void User_LineNumberVisible(bool expected)
        {
            Assert.That(
                Settings.User.LineNumberVisible,
                Is.EqualTo(expected)
            );
        }

        [TestCase(true)]
        public void User_RulerVisible(bool expected)
        {
            Assert.That(
                Settings.User.RulerVisible,
                Is.EqualTo(expected)
            );
        }

        [TestCase(false)]
        public void User_SpecialCharsVisible(bool expected)
        {
            Assert.That(
                Settings.User.SpecialCharsVisible,
                Is.EqualTo(expected)
            );
        }

        [TestCase(true)]
        public void User_EolVisible(bool expected)
        {
            Assert.That(
                Settings.User.EolVisible,
                Is.EqualTo(expected)
            );
        }

        [TestCase(true)]
        public void User_TabVisible(bool expected)
        {
            Assert.That(
                Settings.User.TabVisible,
                Is.EqualTo(expected)
            );
        }

        [TestCase(false)]
        public void User_SpaceVisible(bool expected)
        {
            Assert.That(
                Settings.User.SpaceVisible,
                Is.EqualTo(expected)
            );
        }

        [TestCase(true)]
        public void User_FullSpaceVisible(bool expected)
        {
            Assert.That(
                Settings.User.FullSpaceVisible,
                Is.EqualTo(expected)
            );
        }

        [TestCase(false)]
        public void User_CurrentLineVisible(bool expected)
        {
            Assert.That(
                Settings.User.CurrentLineVisible,
                Is.EqualTo(expected)
            );
        }

        [TestCase(false)]
        public void User_ModifiedLineVisible(bool expected)
        {
            Assert.That(
                Settings.User.ModifiedLineVisible,
                Is.EqualTo(expected)
            );
        }

        [TestCase(true)]
        public void User_RemoveWarning(bool expected)
        {
            Assert.That(
                Settings.User.RemoveWarning,
                Is.EqualTo(expected)
            );
        }

        #endregion

        /* ----------------------------------------------------------------- */
        ///
        /// Save
        ///
        /// <summary>
        /// 定義ファイルの保存テストを行います。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [TestCase("NewSettings.json")]
        public void Save(string filename)
        {
            Settings.Save(IoEx.Path.Combine(Results, filename));
            Assert.That(
                IoEx.File.Exists(Settings.Path),
                Is.True
            );
        }

        #endregion
    }
}
