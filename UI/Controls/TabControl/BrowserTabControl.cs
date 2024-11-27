namespace Bubba
{
    using System;
    using System.ComponentModel;
    using System.Diagnostics.CodeAnalysis;
    using System.Drawing;
    using System.Drawing.Drawing2D;
    using System.Windows;
    using System.Windows.Forms;
    using System.Windows.Input;
    using Point = System.Drawing.Point;

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
    public class BrowserTabControl : MetroTabControl, ISupportInitialize

    {
        private const int _TEXT_LEFT_MARGIN = 15;

        private const int _TEXT_RIGHT_MARGIN = 10;

        private const int _DEF_HEADER_HEIGHT = 28;

        private const int _DEF_BUTTON_HEIGHT = 28;

        private const int _DEF_GLYPH_WIDTH = 40;

        private int _startPosition = 10;

        private StringFormat _formatString;

        private bool _initializing;

        private readonly ContextMenuStrip _menu;

        private bool _open;

        private BrowserTabItem _selectedItem;

        private Rectangle _rectangle = Rectangle.Empty;

        public int AddButtonWidth = 40;

        public int MaxTabSize = 200;

        public event TabItemClosing TabStripItemClosing;

        public event TabItemChange TabStripItemSelectionChanged;

        public event HandledEventHandler MenuItemsLoading;

        public event EventHandler MenuItemsLoaded;

        public event EventHandler TabStripItemClosed;

        public BrowserTabControl( )
        {
            BeginInit( );
            Items = new BrowserTabCollection( );
            Items.CollectionChanged += OnCollectionChanged;
            _formatString = new StringFormat( );
            EndInit( );
            UpdateLayout( );
        }

        public BrowserTabItem SelectedItem
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

        public BrowserTabCollection Items { get; set; }

        public void AddTab( BrowserTabItem tabItem )
        {
            AddTab( tabItem, false );
        }

        public void AddTab( BrowserTabItem tabItem, bool autoSelect )
        {
            Items.Add( tabItem );
            if( ( autoSelect && tabItem.IsVisible )
                || ( tabItem.IsVisible && Items.DrawnCount < 1 ) )
            {
                SelectedItem = tabItem;
                SelectItem( tabItem );
            }
        }

        public void RemoveTab( BrowserTabItem tabItem )
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

        public BrowserTabItem GetTabItemByPoint( Point pt )
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

        public void BeginInit( )
        {
            _initializing = true;
        }

        public void EndInit( )
        {
            _initializing = false;
        }

        protected virtual void UpdateLayout( )
        {
            if( _formatString != null )
            {
                _formatString.Trimming = StringTrimming.EllipsisCharacter;
                _formatString.FormatFlags |= StringFormatFlags.NoWrap;
                _formatString.FormatFlags &= StringFormatFlags.DirectionRightToLeft;
            }
        }

        public void SelectItem( BrowserTabItem tabItem )
        {
            tabItem.IsVisible = true;
            tabItem.Selected = true;
        }

        public void UnSelectItem( BrowserTabItem tabItem )
        {
            tabItem.Selected = false;
        }

        private void SetDefaultSelection( )
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

        protected virtual void OnTabStripItemClosing( TabClosingEventArgs e )
        {
            TabStripItemClosing?.Invoke( e );
        }

        protected virtual void OnTabStripItemClosed( RoutedEventArgs e )
        {
            _selectedItem = null;
            TabStripItemClosed?.Invoke( this, e );
        }

        protected virtual void OnMenuItemsLoading( HandledEventArgs e )
        {
            MenuItemsLoading?.Invoke( this, e );
        }

        protected virtual void OnMenuItemsLoaded( RoutedEventArgs e )
        {
            MenuItemsLoaded?.Invoke( this, e );
        }

        protected virtual void OnBrowserTabItemChanged( BrowserTabChangedEventArgs e )
        {
            TabStripItemSelectionChanged?.Invoke( e );
        }

        protected virtual void OnRightToLeftChanged( RoutedEventArgs e )
        {
            UpdateLayout( );
        }

        protected virtual void OnSizeChanged( RoutedEventArgs e )
        {
            if( !_initializing )
            {
                UpdateLayout( );
            }
        }

        private void OnCollectionChanged( object sender, CollectionChangeEventArgs e )
        {
            var _tab = ( BrowserTabItem )e.Element;
            if( e.Action == CollectionChangeAction.Add )
            {
                OnBrowserTabItemChanged( new BrowserTabChangedEventArgs( _tab, ChangeType.Added ) );
            }
            else if( e.Action == CollectionChangeAction.Remove )
            {
                OnBrowserTabItemChanged( new BrowserTabChangedEventArgs( _tab, ChangeType.Removed ) );
            }
            else
            {
                OnBrowserTabItemChanged( new BrowserTabChangedEventArgs( _tab, ChangeType.Changed ) );
            }

            UpdateLayout( );
        }

        protected virtual void Dispose( bool disposing )
        {
            if( disposing )
            {
                Items.CollectionChanged -= OnCollectionChanged;
                for( var _i = 0; _i < Items.Count; _i++ )
                {
                    var _item = Items[ _i ];
                    if( _item != null )
                    {
                        _item.Dispose( );
                    }
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