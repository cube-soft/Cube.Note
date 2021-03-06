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
using System.Drawing.Printing;
using System.Runtime.Serialization;

namespace Cube.Note
{
    /* --------------------------------------------------------------------- */
    ///
    /// SettingsValue
    ///
    /// <summary>
    /// 各種設定を保持するためのクラスです。
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    [DataContract]
    public class SettingsValue : Cube.ObservableProperty
    {
        #region Constructors

        /* ----------------------------------------------------------------- */
        ///
        /// SettingsValue
        ///
        /// <summary>
        /// オブジェクトを初期化します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public SettingsValue()
        {
            InitializeValues();
        }

        #endregion

        #region Properties

        /* ----------------------------------------------------------------- */
        ///
        /// X
        ///
        /// <summary>
        /// メイン画面の x 座標を取得または設定します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [DataMember]
        public int X
        {
            get { return _x; }
            set
            {
                if (_x == value) return;
                _x = value;
                RaisePropertyChanged(nameof(X));
            }
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Y
        ///
        /// <summary>
        /// メイン画面の y 座標を取得または設定します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [DataMember]
        public int Y
        {
            get { return _y; }
            set
            {
                if (_y == value) return;
                _y = value;
                RaisePropertyChanged(nameof(Y));
            }
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Width
        ///
        /// <summary>
        /// メイン画面の幅を取得または設定します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [DataMember]
        public int Width
        {
            get { return _width; }
            set
            {
                if (_width == value) return;
                _width = value;
                RaisePropertyChanged(nameof(Width));
            }
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Height
        ///
        /// <summary>
        /// メイン画面の高さを取得または設定します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [DataMember]
        public int Height
        {
            get { return _height; }
            set
            {
                if (_height == value) return;
                _height = value;
                RaisePropertyChanged(nameof(Height));
            }
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Page
        ///
        /// <summary>
        /// 選択ページのファイル名を取得または設定します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [DataMember]
        public string Page
        {
            get { return _page; }
            set
            {
                if (_page == value) return;
                _page = value;
                RaisePropertyChanged(nameof(Page));
            }
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Tag
        ///
        /// <summary>
        /// 選択中のタグ名を取得または設定します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [DataMember]
        public string Tag
        {
            get { return _tag; }
            set
            {
                if (_tag == value) return;
                _tag = value;
                RaisePropertyChanged(nameof(Tag));
            }
        }

        /* ----------------------------------------------------------------- */
        ///
        /// FontName
        ///
        /// <summary>
        /// フォント名を取得または設定します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [DataMember]
        public string FontName
        {
            get { return _fontName; }
            set
            {
                if (_fontName == value) return;
                _fontName = value;
                RaisePropertyChanged(nameof(FontName));
                RaisePropertyChanged(nameof(Font));
            }
        }

        /* ----------------------------------------------------------------- */
        ///
        /// FontSize
        ///
        /// <summary>
        /// フォントサイズを取得または設定します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [DataMember]
        public double FontSize
        {
            get { return _fontSize; }
            set
            {
                if (_fontSize == value) return;
                _fontSize = value;
                RaisePropertyChanged(nameof(FontSize));
                RaisePropertyChanged(nameof(Font));
            }
        }

        /* ----------------------------------------------------------------- */
        ///
        /// FontStyle
        ///
        /// <summary>
        /// フォントスタイルを取得または設定します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [DataMember]
        public FontStyle FontStyle
        {
            get { return _fontStyle; }
            set
            {
                if (_fontStyle == value) return;
                _fontStyle = value;
                RaisePropertyChanged(nameof(FontStyle));
                RaisePropertyChanged(nameof(Font));
            }
        }

        /* ----------------------------------------------------------------- */
        ///
        /// Font
        ///
        /// <summary>
        /// フォントオブジェクトを取得します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public Font Font => new Font(FontName, (float)FontSize, FontStyle, GraphicsUnit.Point);

        /* ----------------------------------------------------------------- */
        ///
        /// BackColor
        ///
        /// <summary>
        /// 背景色を取得または設定します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [DataMember]
        public Color BackColor
        {
            get { return _backColor; }
            set
            {
                if (_backColor == value) return;
                _backColor = value;
                RaisePropertyChanged(nameof(BackColor));
            }
        }

        /* ----------------------------------------------------------------- */
        ///
        /// ForeColor
        ///
        /// <summary>
        /// テキスト色を取得または設定します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [DataMember]
        public Color ForeColor
        {
            get { return _foreColor; }
            set
            {
                if (_foreColor == value) return;
                _foreColor = value;
                RaisePropertyChanged(nameof(ForeColor));
            }
        }

        /* ----------------------------------------------------------------- */
        ///
        /// UriColor
        ///
        /// <summary>
        /// URL 色を取得または設定します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [DataMember]
        public Color UriColor
        {
            get { return _uriColor; }
            set
            {
                if (_uriColor == value) return;
                _uriColor = value;
                RaisePropertyChanged(nameof(UriColor));
            }
        }

        /* ----------------------------------------------------------------- */
        ///
        /// HighlightBackColor
        ///
        /// <summary>
        /// 強調表示時の背景色を取得または設定します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [DataMember]
        public Color HighlightBackColor
        {
            get { return _highlightBackColor; }
            set
            {
                if (_highlightBackColor == value) return;
                _highlightBackColor = value;
                RaisePropertyChanged(nameof(HighlightBackColor));
            }
        }

        /* ----------------------------------------------------------------- */
        ///
        /// HighlightForeColor
        ///
        /// <summary>
        /// 強調表示時の前景色を取得または設定します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [DataMember]
        public Color HighlightForeColor
        {
            get { return _highlightForeColor; }
            set
            {
                if (_highlightForeColor == value) return;
                _highlightForeColor = value;
                RaisePropertyChanged(nameof(HighlightForeColor));
            }
        }

        /* ----------------------------------------------------------------- */
        ///
        /// SearchBackColor
        ///
        /// <summary>
        /// 検索結果表示時の背景色を取得または設定します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [DataMember]
        public Color SearchBackColor
        {
            get { return _searchBackColor; }
            set
            {
                if (_searchBackColor == value) return;
                _searchBackColor = value;
                RaisePropertyChanged(nameof(SearchBackColor));
            }
        }

        /* ----------------------------------------------------------------- */
        ///
        /// SearchForeColor
        ///
        /// <summary>
        /// 検索結果表示時の前景色を取得または設定します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [DataMember]
        public Color SearchForeColor
        {
            get { return _searchForeColor; }
            set
            {
                if (_searchForeColor == value) return;
                _searchForeColor = value;
                RaisePropertyChanged(nameof(SearchForeColor));
            }
        }

        /* ----------------------------------------------------------------- */
        ///
        /// LineNumberBackColor
        ///
        /// <summary>
        /// 行番号および水平ルーラー部の背景色を取得または設定します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [DataMember]
        public Color LineNumberBackColor
        {
            get { return _lineNumberBackColor; }
            set
            {
                if (_lineNumberBackColor == value) return;
                _lineNumberBackColor = value;
                RaisePropertyChanged(nameof(LineNumberBackColor));
            }
        }

        /* ----------------------------------------------------------------- */
        ///
        /// LineNumberForeColor
        ///
        /// <summary>
        /// 行番号および水平ルーラー部の前景色を取得または設定します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [DataMember]
        public Color LineNumberForeColor
        {
            get { return _lineNumberForeColor; }
            set
            {
                if (_lineNumberForeColor == value) return;
                _lineNumberForeColor = value;
                RaisePropertyChanged(nameof(LineNumberForeColor));
            }
        }

        /* ----------------------------------------------------------------- */
        ///
        /// SpecialCharsColor
        ///
        /// <summary>
        /// 特殊文字の表示色を取得または設定します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [DataMember]
        public Color SpecialCharsColor
        {
            get { return _specialCharsColor; }
            set
            {
                if (_specialCharsColor == value) return;
                _specialCharsColor = value;
                RaisePropertyChanged(nameof(SpecialCharsColor));
            }
        }

        /* ----------------------------------------------------------------- */
        ///
        /// CurrentLineColor
        ///
        /// <summary>
        /// 現在行の表示色を取得または設定します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [DataMember]
        public Color CurrentLineColor
        {
            get { return _currentLineColor; }
            set
            {
                if (_currentLineColor == value) return;
                _currentLineColor = value;
                RaisePropertyChanged(nameof(CurrentLineColor));
            }
        }

        /* ----------------------------------------------------------------- */
        ///
        /// AutoSaveTime
        ///
        /// <summary>
        /// 自動保存間隔を取得または設定します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [DataMember]
        public TimeSpan AutoSaveTime
        {
            get { return _autoSaveTime; }
            set
            {
                if (_autoSaveTime == value) return;
                _autoSaveTime = value;
                RaisePropertyChanged(nameof(AutoSaveTime));
            }
        }

        /* ----------------------------------------------------------------- */
        ///
        /// PrintMargin
        ///
        /// <summary>
        /// 印刷時の余白を mm 単位で取得または設定します。
        /// </summary>
        ///
        /// <remarks>
        /// Margins オブジェクトの各種プロパティは 1/100 インチ単位である事が
        /// 想定されています。したがって、印刷時には変換された値が使用されます。
        /// </remarks>
        ///
        /* ----------------------------------------------------------------- */
        [DataMember]
        public Margins PrintMargin
        {
            get { return _printMargin; }
            set
            {
                if (_printMargin == value) return;
                _printMargin = value;
                RaisePropertyChanged(nameof(PrintMargin));
            }
        }

        /* ----------------------------------------------------------------- */
        ///
        /// SearchQuery
        ///
        /// <summary>
        /// 検索クエリーを取得または設定します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [DataMember]
        public string SearchQuery
        {
            get { return _searchQuery; }
            set
            {
                if (_searchQuery == value) return;
                _searchQuery = value;
                RaisePropertyChanged(nameof(SearchQuery));
            }
        }

        /* ----------------------------------------------------------------- */
        ///
        /// TabWidth
        ///
        /// <summary>
        /// タブ幅を取得または設定します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [DataMember]
        public int TabWidth
        {
            get { return _tabWidth; }
            set
            {
                if (_tabWidth == value) return;
                _tabWidth = value;
                RaisePropertyChanged(nameof(TabWidth));
            }
        }

        /* ----------------------------------------------------------------- */
        ///
        /// TabToSpace
        ///
        /// <summary>
        /// タブの代わりにスペースを挿入するかどうかを示す値を取得または
        /// 設定します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [DataMember]
        public bool TabToSpace
        {
            get { return _tabToSpace; }
            set
            {
                if (_tabToSpace == value) return;
                _tabToSpace = value;
                RaisePropertyChanged(nameof(TabToSpace));
            }
        }

        /* ----------------------------------------------------------------- */
        ///
        /// WordWrap
        ///
        /// <summary>
        /// テキストを折り返し表示するかどうかを示す値を取得または
        /// 設定します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [DataMember]
        public bool WordWrap
        {
            get { return _wordWrap; }
            set
            {
                if (_wordWrap == value) return;
                _wordWrap = value;
                RaisePropertyChanged(nameof(WordWrap));
            }
        }

        /* ----------------------------------------------------------------- */
        ///
        /// WordWrapAsWindow
        ///
        /// <summary>
        /// ウィンドウ幅でテキストを折り返し表示するかどうかを示す値を
        /// 取得または設定します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [DataMember]
        public bool WordWrapAsWindow
        {
            get { return _wordWrapAsWindow; }
            set
            {
                if (_wordWrapAsWindow == value) return;
                _wordWrapAsWindow = value;
                RaisePropertyChanged(nameof(WordWrapAsWindow));
            }
        }

        /* ----------------------------------------------------------------- */
        ///
        /// WordWrapCount
        ///
        /// <summary>
        /// 折り返し表示する文字数を取得または設定します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [DataMember]
        public int WordWrapCount
        {
            get { return _wordWrapCount; }
            set
            {
                if (_wordWrapCount == value) return;
                _wordWrapCount = value;
                RaisePropertyChanged(nameof(WordWrapCount));
            }
        }

        /* ----------------------------------------------------------------- */
        ///
        /// LineNumberVisible
        ///
        /// <summary>
        /// 行番号を表示するかどうかを示す値を取得または設定します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [DataMember]
        public bool LineNumberVisible
        {
            get { return _lineNumberVisible; }
            set
            {
                if (_lineNumberVisible == value) return;
                _lineNumberVisible = value;
                RaisePropertyChanged(nameof(LineNumberVisible));
            }
        }

        /* ----------------------------------------------------------------- */
        ///
        /// RulerVisible
        ///
        /// <summary>
        /// 水平方向に目盛りを表示するかどうかを示す値を取得または
        /// 設定します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [DataMember]
        public bool RulerVisible
        {
            get { return _rulerVisible; }
            set
            {
                if (_rulerVisible == value) return;
                _rulerVisible = value;
                RaisePropertyChanged(nameof(RulerVisible));
            }
        }

        /* ----------------------------------------------------------------- */
        ///
        /// SpecialCharsVisible
        ///
        /// <summary>
        /// 特殊文字を表示するかどうかを示す値を取得または設定します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [DataMember]
        public bool SpecialCharsVisible
        {
            get { return _specialCharsVisible; }
            set
            {
                if (_specialCharsVisible == value) return;
                _specialCharsVisible = value;
                RaisePropertyChanged(nameof(SpecialCharsVisible));
            }
        }

        /* ----------------------------------------------------------------- */
        ///
        /// EolVisible
        ///
        /// <summary>
        /// 改行を表示するかどうかを示す値を取得または設定します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [DataMember]
        public bool EolVisible
        {
            get { return _eolVisible; }
            set
            {
                if (_eolVisible == value) return;
                _eolVisible = value;
                RaisePropertyChanged(nameof(EolVisible));
            }
        }

        /* ----------------------------------------------------------------- */
        ///
        /// TabVisible
        ///
        /// <summary>
        /// タブを表示するかどうかを示す値を取得または設定します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [DataMember]
        public bool TabVisible
        {
            get { return _tabVisible; }
            set
            {
                if (_tabVisible == value) return;
                _tabVisible = value;
                RaisePropertyChanged(nameof(TabVisible));
            }
        }

        /* ----------------------------------------------------------------- */
        ///
        /// SpaceVisible
        ///
        /// <summary>
        /// 半角スペースを表示するかどうかを示す値を取得または設定します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [DataMember]
        public bool SpaceVisible
        {
            get { return _spaceVisible; }
            set
            {
                if (_spaceVisible == value) return;
                _spaceVisible = value;
                RaisePropertyChanged(nameof(SpaceVisible));
            }
        }

        /* ----------------------------------------------------------------- */
        ///
        /// FullSpaceVisible
        ///
        /// <summary>
        /// 全角スペースを表示するかどうかを示す値を取得または設定します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [DataMember]
        public bool FullSpaceVisible
        {
            get { return _fullSpaceVisible; }
            set
            {
                if (_fullSpaceVisible == value) return;
                _fullSpaceVisible = value;
                RaisePropertyChanged(nameof(FullSpaceVisible));
            }
        }

        /* ----------------------------------------------------------------- */
        ///
        /// CurrentLineVisible
        ///
        /// <summary>
        /// 現在の行を強調表示するかどうかを示す値を取得または設定します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [DataMember]
        public bool CurrentLineVisible
        {
            get { return _currentLineVisible; }
            set
            {
                if (_currentLineVisible == value) return;
                _currentLineVisible = value;
                RaisePropertyChanged(nameof(CurrentLineVisible));
            }
        }

        /* ----------------------------------------------------------------- */
        ///
        /// ModifiedLineVisible
        ///
        /// <summary>
        /// 修正行を強調表示するかどうかを示す値を取得または設定します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [DataMember]
        public bool ModifiedLineVisible
        {
            get { return _modifiedLineVisible; }
            set
            {
                if (_modifiedLineVisible == value) return;
                _modifiedLineVisible = value;
                RaisePropertyChanged(nameof(ModifiedLineVisible));
            }
        }

        /* ----------------------------------------------------------------- */
        ///
        /// BracketVisible
        ///
        /// <summary>
        /// 対応する括弧を強調表示するかどうかを示す値を取得または設定します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [DataMember]
        public bool BracketVisible
        {
            get { return _bracketVisible; }
            set
            {
                if (_bracketVisible == value) return;
                _bracketVisible = value;
                RaisePropertyChanged(nameof(BracketVisible));
            }
        }

        /* ----------------------------------------------------------------- */
        ///
        /// RemoveWarning
        ///
        /// <summary>
        /// ページ削除時に警告メッセージを表示するかどうかを示す値を取得
        /// または設定します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [DataMember]
        public bool RemoveWarning
        {
            get { return _removeWarning; }
            set
            {
                if (_removeWarning == value) return;
                _removeWarning = value;
                RaisePropertyChanged(nameof(RemoveWarning));
            }
        }

        /* ----------------------------------------------------------------- */
        ///
        /// TagRemoveWarning
        ///
        /// <summary>
        /// タグ削除時に警告メッセージを表示するかどうかを示す値を取得
        /// または設定します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [DataMember]
        public bool TagRemoveWarning
        {
            get { return _tagRemoveWarning; }
            set
            {
                if (_tagRemoveWarning == value) return;
                _tagRemoveWarning = value;
                RaisePropertyChanged(nameof(TagRemoveWarning));
            }
        }

        /* ----------------------------------------------------------------- */
        ///
        /// OpenUri
        ///
        /// <summary>
        /// URL ダブルクリック時に既定ブラウザで開くかどうかを示す値を取得または
        /// 設定します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [DataMember]
        public bool OpenUri
        {
            get { return _openUri; }
            set
            {
                if (_openUri == value) return;
                _openUri = value;
                RaisePropertyChanged(nameof(OpenUri));
            }
        }

        /* ----------------------------------------------------------------- */
        ///
        /// IncludeLineCode
        ///
        /// <summary>
        /// 改行コードも文字数に含めるかどうかを示す値を取得または設定します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [DataMember]
        public bool IncludeLineCode
        {
            get { return _includeLineCode; }
            set
            {
                if (_includeLineCode == value) return;
                _includeLineCode = value;
                RaisePropertyChanged(nameof(IncludeLineCode));
            }
        }

        /* ----------------------------------------------------------------- */
        ///
        /// ShowUpdate
        ///
        /// <summary>
        /// アップデートを確認するかどうかを示す値を取得または設定します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [DataMember]
        public bool ShowUpdate
        {
            get { return _showUpdate; }
            set
            {
                if (_showUpdate == value) return;
                _showUpdate = value;
                RaisePropertyChanged(nameof(ShowUpdate));
            }
        }

        /* ----------------------------------------------------------------- */
        ///
        /// LastUpdate
        ///
        /// <summary>
        /// 最後にアップデートを確認した日時を取得または設定します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [DataMember]
        public DateTime LastUpdate
        {
            get { return _lastUpdate; }
            set
            {
                if (_lastUpdate == value) return;
                _lastUpdate = value;
                RaisePropertyChanged(nameof(LastUpdate));
            }
        }

        #endregion

        #region Others

        /* ----------------------------------------------------------------- */
        ///
        /// OnDeserializing
        ///
        /// <summary>
        /// デシリアライズ時に実行されます。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [OnDeserializing]
        private void OnDeserializing(StreamingContext context) => InitializeValues();

        /* ----------------------------------------------------------------- */
        ///
        /// InitializeValues
        ///
        /// <summary>
        /// 値を初期化します。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        private void InitializeValues()
        {
            X                   = -1;
            Y                   = -1;
            Width               = -1;
            Height              = -1;
            Page                = string.Empty;
            Tag                 = "NoTag";

            FontName            = "Meiryo";
            FontSize            = 11.25;
            FontStyle           = FontStyle.Regular;

            BackColor           = SystemColors.Window;
            ForeColor           = SystemColors.WindowText;
            UriColor            = Color.Navy;
            HighlightBackColor  = SystemColors.Highlight;
            HighlightForeColor  = SystemColors.HighlightText;
            SearchBackColor     = Color.Orange;
            SearchForeColor     = Color.Black;
            LineNumberBackColor = SystemColors.Control;
            LineNumberForeColor = SystemColors.ControlDark;
            SpecialCharsColor   = SystemColors.ControlDark;
            CurrentLineColor    = SystemColors.ControlDark;

            AutoSaveTime        = TimeSpan.FromSeconds(30);
            LastUpdate          = new DateTime(2016, 1, 1, 0, 0, 0, DateTimeKind.Local);
            PrintMargin         = new Margins(25, 25, 25, 25);
            SearchQuery         = "https://s.cube-soft.jp/search/?q=";
            TabWidth            = 8;
            WordWrapCount       = 80;

            TabToSpace          = false;
            WordWrap            = false;
            WordWrapAsWindow    = false;
            LineNumberVisible   = true;
            RulerVisible        = false;
            SpecialCharsVisible = true;
            EolVisible          = true;
            TabVisible          = true;
            SpaceVisible        = false;
            FullSpaceVisible    = true;
            CurrentLineVisible  = false;
            ModifiedLineVisible = false;
            BracketVisible      = false;
            RemoveWarning       = true;
            TagRemoveWarning    = true;
            OpenUri             = true;
            IncludeLineCode     = false;
            ShowUpdate          = true;
        }

        #endregion

        #region Fields
        private int _x;
        private int _y;
        private int _width;
        private int _height;
        private string _page;
        private string _tag;
        private Color _backColor;
        private Color _foreColor;
        private Color _uriColor;
        private Color _highlightBackColor;
        private Color _highlightForeColor;
        private Color _searchBackColor;
        private Color _searchForeColor;
        private Color _lineNumberBackColor;
        private Color _lineNumberForeColor;
        private Color _specialCharsColor;
        private Color _currentLineColor;
        private TimeSpan _autoSaveTime;
        private Margins _printMargin;
        private string _searchQuery;
        private string _fontName;
        private double _fontSize;
        private FontStyle _fontStyle;
        private int _tabWidth;
        private int _wordWrapCount;
        private bool _tabToSpace;
        private bool _wordWrap;
        private bool _wordWrapAsWindow;
        private bool _lineNumberVisible;
        private bool _rulerVisible;
        private bool _specialCharsVisible;
        private bool _eolVisible;
        private bool _tabVisible;
        private bool _spaceVisible;
        private bool _fullSpaceVisible;
        private bool _currentLineVisible;
        private bool _modifiedLineVisible;
        private bool _bracketVisible;
        private bool _removeWarning;
        private bool _tagRemoveWarning;
        private bool _openUri;
        private bool _includeLineCode;
        private bool _showUpdate;
        private DateTime _lastUpdate;
        #endregion
    }
}
