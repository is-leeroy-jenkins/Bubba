﻿// ******************************************************************************************
//     Assembly:                Badger
//     Author:                  Terry D. Eppler
//     Created:                 10-26-2024
// 
//     Last Modified By:        Terry D. Eppler
//     Last Modified On:        10-26-2024
// ******************************************************************************************
// <copyright file="WebControl.cs" company="Terry D. Eppler">
//   An open source data analysis application for EPA Analysts developed
//   in C-Sharp using WPF and released under the MIT license
// 
//    Copyright ©  2020-2024 Terry D. Eppler
// 
//    Permission is hereby granted, free of charge, to any person obtaining a copy
//    of this software and associated documentation files (the “Software”),
//    to deal in the Software without restriction,
//    including without limitation the rights to use,
//    copy, modify, merge, publish, distribute, sublicense,
//    and/or sell copies of the Software,
//    and to permit persons to whom the Software is furnished to do so,
//    subject to the following conditions:
// 
//    The above copyright notice and this permission notice shall be included in all
//    copies or substantial portions of the Software.
// 
//    THE SOFTWARE IS PROVIDED “AS IS”, WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED,
//    INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NON-INFRINGEMENT.
//    IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
//    DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER
//    DEALINGS IN THE SOFTWARE.
// 
//    You can contact me at:  terryeppler@gmail.com or eppler.terry@epa.gov
// </copyright>
// <summary>
//   WebControl.cs
// </summary>
// ******************************************************************************************

namespace Bubba
{
    using System;
    using System.ComponentModel;
    using System.Diagnostics.CodeAnalysis;
    using System.Drawing;
    using System.Windows;
    using System.Windows.Forms;
    using Point = System.Drawing.Point;

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    /// <seealso cref="T:Badger.MetroTabControl" />
    /// <seealso cref="T:System.ComponentModel.ISupportInitialize" />
    [ SuppressMessage( "ReSharper", "FieldCanBeMadeReadOnly.Global" ) ]
    [ SuppressMessage( "ReSharper", "FieldCanBeMadeReadOnly.Local" ) ]
    [ SuppressMessage( "ReSharper", "UnusedParameter.Global" ) ]
    [ SuppressMessage( "ReSharper", "UnusedVariable" ) ]
    [ SuppressMessage( "ReSharper", "ConvertIfStatementToSwitchStatement" ) ]
    [ SuppressMessage( "ReSharper", "MemberCanBeInternal" ) ]
    [ DefaultEvent( "TabStripItemSelectionChanged" ) ]
    [ DefaultProperty( "Items" ) ]
    [ ToolboxItem( true ) ]
    [ SuppressMessage( "ReSharper", "LocalVariableHidesMember" ) ]
    [ SuppressMessage( "ReSharper", "IntroduceOptionalParameters.Global" ) ]
    [ SuppressMessage( "ReSharper", "UnusedType.Global" ) ]
    [ SuppressMessage( "ReSharper", "MemberCanBePrivate.Global" ) ]
    [ SuppressMessage( "ReSharper", "InconsistentNaming" ) ]
    [ SuppressMessage( "ReSharper", "ConvertIfStatementToNullCoalescingExpression" ) ]
    [ SuppressMessage( "ReSharper", "RedundantAssignment" ) ]
    [ SuppressMessage( "ReSharper", "AssignNullToNotNullAttribute" ) ]
    [ SuppressMessage( "ReSharper", "MergeIntoPattern" ) ]
    [ SuppressMessage( "ReSharper", "PossibleNullReferenceException" ) ]
    [ SuppressMessage( "ReSharper", "RedundantExtendsListEntry" ) ]
    public class WebControl : MetroTabControl, ISupportInitialize, IDisposable
    {
        /// <summary>
        /// The text left margin
        /// </summary>
        private const int TEXT_LEFT_MARGIN = 15;

        /// <summary>
        /// The text right margin
        /// </summary>
        private const int TEXT_RIGHT_MARGIN = 10;

        /// <summary>
        /// The definition header height
        /// </summary>
        private const int _DEF_HEADER_HEIGHT = 28;

        /// <summary>
        /// The definition button height
        /// </summary>
        private const int _DEF_BUTTON_HEIGHT = 28;

        /// <summary>
        /// The definition glyph width
        /// </summary>
        private const int _DEF_GLYPH_WIDTH = 40;

        /// <summary>
        /// The start position
        /// </summary>
        private int _startPosition = 10;

        /// <summary>
        /// The format string
        /// </summary>
        private StringFormat _formatString;

        /// <summary>
        /// The initializing
        /// </summary>
        private bool _initializing;

        /// <summary>
        /// The menu
        /// </summary>
        private protected readonly ContextMenuStrip _menu;

        /// <summary>
        /// The open
        /// </summary>
        private protected bool _open;

        /// <summary>
        /// The selected item
        /// </summary>
        private BrowserTabItem _selectedItem;

        /// <summary>
        /// The rectangle
        /// </summary>
        private Rectangle _rectangle = Rectangle.Empty;

        /// <summary>
        /// The add button width
        /// </summary>
        public new int AddButtonWidth = 40;

        /// <summary>
        /// The maximum tab size
        /// </summary>
        public new int MaxTabSize = 200;

        /// <inheritdoc />
        /// <summary>
        /// Occurs when [tab strip item closing].
        /// </summary>
        public override event TabItemClosing TabStripItemClosing;

        /// <inheritdoc />
        /// <summary>
        /// Occurs when [tab strip item selection changed].
        /// </summary>
        public override event TabItemChange TabStripItemSelectionChanged;

        /// <inheritdoc />
        /// <summary>
        /// Occurs when [menu items loading].
        /// </summary>
        public override event HandledEventHandler MenuItemsLoading;

        /// <inheritdoc />
        /// <summary>
        /// Occurs when [menu items loaded].
        /// </summary>
        public override event EventHandler MenuItemsLoaded;

        /// <inheritdoc />
        /// <summary>
        /// Occurs when [tab strip item closed].
        /// </summary>
        public override event EventHandler TabStripItemClosed;

        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="T:Badger.MetroTabControl" /> class.
        /// </summary>
        /// <returns></returns>
        /// <inheritdoc />
        public WebControl( )
        {
            BeginInit( );
            Items = new BrowserTabCollection( );
            Items.CollectionChanged += OnCollectionChanged;
            _formatString = new StringFormat( );
            EndInit( );
            UpdateFormat( );
        }

        /// <summary>
        /// Gets or sets the first item in the current selection
        /// or returns null if the selection is empty.
        /// </summary>
        public new BrowserTabItem SelectedItem
        {
            get
            {
                return _selectedItem;
            }
            set
            {
                if( _selectedItem == value )
                {
                    return;
                }

                if( value == null
                    && Items.Count > 0 )
                {
                    var _fATabStripItem = Items[ 0 ];
                    if( _fATabStripItem.IsVisible )
                    {
                        _selectedItem = _fATabStripItem;
                        _selectedItem.Selected = true;
                    }
                }
                else
                {
                    _selectedItem = value;
                }

                foreach( BrowserTabItem _item in Items )
                {
                    if( _item == _selectedItem )
                    {
                        SelectItem( _item );
                        _item.IsVisible = true;
                    }
                    else
                    {
                        UnSelectItem( _item );
                        _item.IsVisible = false;
                    }
                }

                SelectItem( _selectedItem );
                if( !_selectedItem.IsLoaded )
                {
                    Items.MoveTo( 0, _selectedItem );
                }

                OnBrowserTabItemChanged( new BrowserTabChangedEventArgs( _selectedItem,
                    ChangeType.SelectionChanged ) );
            }
        }

        /// <summary>
        /// Gets the collection used to generate the content of the
        /// <see cref="T:System.Windows.Controls.ItemsControl" />.
        /// </summary>
        public new BrowserTabCollection Items { get; set; }

        /// <inheritdoc />
        /// <summary>
        /// Adds the tab.
        /// </summary>
        /// <param name="tabItem">The tab item.</param>
        public override void AddTab( BrowserTabItem tabItem )
        {
            AddTab( tabItem, false );
        }

        /// <inheritdoc />
        /// <summary>
        /// Adds the tab.
        /// </summary>
        /// <param name="tabItem">The tab item.</param>
        /// <param name="autoSelect">if set to <c>true</c> [automatic select].</param>
        public override void AddTab( BrowserTabItem tabItem, bool autoSelect )
        {
            Items.Add( tabItem );
            if( ( autoSelect && tabItem.IsVisible )
                || ( tabItem.IsVisible && Items.DrawnCount < 1 ) )
            {
                SelectedItem = tabItem;
                SelectItem( tabItem );
            }
        }

        /// <inheritdoc />
        /// <summary>
        /// Removes the tab.
        /// </summary>
        /// <param name="tabItem">The tab item.</param>
        public override void RemoveTab( BrowserTabItem tabItem )
        {
            var _num = Items.IndexOf( tabItem );
            if( _num >= 0 )
            {
                UnSelectItem( tabItem );
                Items.Remove( tabItem );
            }

            if( Items.Count > 0 )
            {
                if( Items[ _num - 1 ] != null )
                {
                    SelectedItem = Items[ _num - 1 ];
                }
                else
                {
                    SelectedItem = Items.FirstVisible;
                }
            }
        }

        /// <inheritdoc />
        /// <summary>
        /// Gets the tab item by point.
        /// </summary>
        /// <param name="pt">The pt.</param>
        /// <returns></returns>
        public override BrowserTabItem GetTabItemByPoint( Point pt )
        {
            BrowserTabItem _result = null;
            var _flag = false;
            for( var _i = 0; _i < Items.Count; _i++ )
            {
                var _fATabStripItem = Items[ _i ];
                if( _fATabStripItem.IsVisible
                    && _fATabStripItem.IsLoaded )
                {
                    _result = _fATabStripItem;
                    _flag = true;
                }

                if( _flag )
                {
                    break;
                }
            }

            return _result;
        }

        /// <summary>
        /// Ensures that all visual child elements of this
        /// element are properly updated for layout.
        /// </summary>
        protected virtual void UpdateFormat( )
        {
            if( _formatString != null )
            {
                _formatString.Trimming = StringTrimming.EllipsisCharacter;
                _formatString.FormatFlags |= StringFormatFlags.NoWrap;
                _formatString.FormatFlags &= StringFormatFlags.DirectionRightToLeft;
            }
        }

        /// <inheritdoc />
        /// <summary>
        /// Sets the default selection.
        /// </summary>
        protected override void SetDefaultSelection( )
        {
            if( _selectedItem == null
                && Items.Count > 0 )
            {
                _selectedItem = Items[ 0 ];
            }

            for( var _i = 0; _i < Items.Count; _i++ )
            {
                var _fATabStripItem = Items[ _i ];
            }
        }

        /// <inheritdoc />
        /// <![CDATA[The 'member' start tag on line 2 position 2 does not match the end tag of 'param'. Line 5, position 38.]]>
        protected override void OnTabStripItemClosing( TabClosingEventArgs e )
        {
            TabStripItemClosing?.Invoke( e );
        }

        /// <inheritdoc />
        /// <summary>
        /// Raises the <see cref="E:TabStripItemClosed" /> event.
        /// </summary>
        /// <param name="e">The <see cref="T:System.Windows.RoutedEventArgs" />
        /// instance containing the event data.</param>
        protected override void OnTabStripItemClosed( RoutedEventArgs e )
        {
            _selectedItem = null;
            TabStripItemClosed?.Invoke( this, e );
        }

        /// <inheritdoc />
        /// <summary>
        /// Raises the <see cref="E:MenuItemsLoading" /> event.
        /// </summary>
        /// <param name="e">The <see cref="T:System.ComponentModel.HandledEventArgs" />
        /// instance containing the event data.</param>
        protected override void OnMenuItemsLoading( HandledEventArgs e )
        {
            MenuItemsLoading?.Invoke( this, e );
        }

        /// <inheritdoc />
        /// <summary>
        /// Raises the <see cref="E:MenuItemsLoaded" /> event.
        /// </summary>
        /// <param name="e">The <see cref="T:System.Windows.RoutedEventArgs" />
        /// instance containing the event data.</param>
        protected override void OnMenuItemsLoaded( RoutedEventArgs e )
        {
            MenuItemsLoaded?.Invoke( this, e );
        }

        /// <inheritdoc />
        /// <summary>
        /// Raises the <see cref="E:BrowserTabItemChanged" /> event.
        /// </summary>
        /// <param name="e">The <see cref="T:Bubba.BrowserTabChangedEventArgs" />
        /// instance containing the event data.</param>
        protected override void OnBrowserTabItemChanged( BrowserTabChangedEventArgs e )
        {
            TabStripItemSelectionChanged?.Invoke( e );
        }

        /// <inheritdoc />
        /// <summary>
        /// Raises the <see cref="E:RightToLeftChanged" /> event.
        /// </summary>
        /// <param name="e">The <see cref="T:System.Windows.RoutedEventArgs" />
        /// instance containing the event data.</param>
        protected override void OnRightToLeftChanged( RoutedEventArgs e )
        {
            UpdateFormat( );
        }

        /// <inheritdoc />
        /// <summary>
        /// Raises the <see cref="E:SizeChanged" /> event.
        /// </summary>
        /// <param name="e">The <see cref="T:System.Windows.RoutedEventArgs" />
        /// instance containing the event data.</param>
        protected override void OnSizeChanged( RoutedEventArgs e )
        {
            if( !_initializing )
            {
                UpdateFormat( );
            }
        }

        /// <inheritdoc />
        /// <summary>
        /// Called when [collection changed].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="T:System.ComponentModel.CollectionChangeEventArgs" />
        /// instance containing the event data.</param>
        protected override void OnCollectionChanged( object sender, CollectionChangeEventArgs e )
        {
            var Tab = ( BrowserTabItem )e.Element;
            if( e.Action == CollectionChangeAction.Add )
            {
                var _args = new BrowserTabChangedEventArgs( Tab, ChangeType.Added );
                OnBrowserTabItemChanged( _args );
            }
            else if( e.Action == CollectionChangeAction.Remove )
            {
                var _args = new BrowserTabChangedEventArgs( Tab, ChangeType.Removed );
                OnBrowserTabItemChanged( _args );
            }
            else
            {
                var _args = new BrowserTabChangedEventArgs( Tab, ChangeType.Changed );
                OnBrowserTabItemChanged( _args );
            }

            UpdateFormat( );
        }

        /// <inheritdoc />
        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources;
        /// <c>false</c> to release only unmanaged resources.</param>
        protected override void Dispose( bool disposing )
        {
            if( disposing )
            {
                Items.CollectionChanged -= OnCollectionChanged;
                for( var _i = 0; _i < Items.Count; _i++ )
                {
                    var _item = Items[ _i ];
                    _item?.Dispose( );
                }

                if( _menu != null
                    && !_menu.IsDisposed )
                {
                    _menu.Dispose( );
                }

                _formatString?.Dispose( );
            }
        }
    }
}