﻿// ******************************************************************************************
//     Assembly:                Bubba
//     Author:                  Terry D. Eppler
//     Created:                 01-26-2025
// 
//     Last Modified By:        Terry D. Eppler
//     Last Modified On:        01-26-2025
// ******************************************************************************************
// <copyright file="MetroTextBox.cs" company="Terry D. Eppler">
//    Bubba is a small and simple windows (wpf) application for interacting with the OpenAI API
//    that's developed in C-Sharp under the MIT license.C#.
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
//   MetroTextBox.cs
// </summary>
// ******************************************************************************************

namespace Bubba
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Windows;
    using System.Windows.Media;
    using Syncfusion.Windows.Controls.Input;

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    /// <seealso cref="T:Syncfusion.Windows.Controls.Input.SfTextBoxExt" />
    [ SuppressMessage( "ReSharper", "UnusedType.Global" ) ]
    [ SuppressMessage( "ReSharper", "FieldCanBeMadeReadOnly.Local" ) ]
    [ SuppressMessage( "ReSharper", "MemberCanBePrivate.Global" ) ]
    [ SuppressMessage( "ReSharper", "InconsistentNaming" ) ]
    [ SuppressMessage( "ReSharper", "FieldCanBeMadeReadOnly.Global" ) ]
    [ SuppressMessage( "ReSharper", "MemberCanBeProtected.Global" ) ]
    [ SuppressMessage( "ReSharper", "MemberCanBeInternal" ) ]
    public class MetroTextBox : SfTextBoxExt
    {
        /// <summary>
        /// The theme
        /// </summary>
        private protected readonly DarkMode _theme = new DarkMode( );

        /// <summary>
        /// The input text
        /// </summary>
        private protected string _inputText;

        /// <summary>
        /// The temporary text
        /// </summary>
        private protected string _tempText;

        /// <inheritdoc />
        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="T:Bubba.MetroTextBox" /> class.
        /// </summary>
        public MetroTextBox( )
            : base( )
        {
            SetResourceReference( StyleProperty, typeof( SfTextBoxExt ) );
            Width = 100;
            Height = 24;
            FontFamily = _theme.FontFamily;
            FontSize = _theme.FontSize;
            _inputText = "";
            _tempText = "";

            // Event Wiring
            TextChanged += OnTextChanged;
            LostFocus += OnFocusLost;
        }

        /// <summary>
        /// Gets or sets the input text.
        /// </summary>
        /// <value>
        /// The input text.
        /// </value>
        public string InputText
        {
            get
            {
                return _inputText;
            }
            set
            {
                _inputText = value;
            }
        }

        /// <summary>
        /// Called when [mouse enter].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/>
        /// instance containing the event data.</param>
        private protected void OnMouseEnter( object sender, RoutedEventArgs e )
        {
            try
            {
                Background = _theme.DarkBlueBrush;
                BorderBrush = _theme.BlueBorderBrush;
                Foreground = _theme.WhiteForeground;
            }
            catch( Exception ex )
            {
                Fail( ex );
            }
        }

        /// <summary>
        /// Called when [mouse leave].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/>
        /// instance containing the event data.</param>
        private protected void OnMouseLeave( object sender, RoutedEventArgs e )
        {
            try
            {
                Background = _theme.ControlInterior;
                BorderBrush = _theme.BlueBorderBrush;
                Foreground = _theme.FormForeground;
            }
            catch( Exception ex )
            {
                Fail( ex );
            }
        }

        /// <summary>
        /// Called when [un focused].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/>
        /// instance containing the event data.
        /// </param>
        private protected void OnTextChanged( object sender, RoutedEventArgs e )
        {
            try
            {
                if( sender is MetroTextBox _textBox
                    && !string.IsNullOrEmpty( _textBox.Text ) )
                {
                    _tempText = _textBox.Text;
                }
            }
            catch( Exception ex )
            {
                Fail( ex );
            }
        }

        /// <summary>
        /// Called when [focus lost].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs" />
        /// instance containing the event data.</param>
        private protected void OnFocusLost( object sender, RoutedEventArgs e )
        {
            try
            {
                if( !string.IsNullOrEmpty( _tempText )
                    && !string.IsNullOrEmpty( _inputText )
                    && _tempText != _inputText )
                {
                    _tempText = _inputText;
                }
            }
            catch( Exception ex )
            {
                Fail( ex );
            }
        }

        /// <summary>
        /// Fails the specified ex.
        /// </summary>
        /// <param name="_ex">The ex.</param>
        private protected void Fail( Exception _ex )
        {
            var _error = new ErrorWindow( _ex );
            _error?.SetText( );
            _error?.ShowDialog( );
        }
    }
}