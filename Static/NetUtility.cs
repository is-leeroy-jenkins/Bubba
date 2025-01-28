// ******************************************************************************************
//     Assembly:                Baby
//     Author:                  Terry D. Eppler
//     Created:                 09-09-2024
// 
//     Last Modified By:        Terry D. Eppler
//     Last Modified On:        09-09-2024
// ******************************************************************************************
// <copyright file="NetUtility.cs" company="Terry D. Eppler">
//     Baby is a light-weight, full-featured, web-browser built with .NET 6 and is written
//     in C#.  The baby browser is designed for budget execution and data analysis.
//     A tool for EPA analysts and a component that can be used for general browsing.
// 
//     Copyright ©  2020 Terry D. Eppler
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
//   NetUtility.cs
// </summary>
// ******************************************************************************************

namespace Bubba
{
    using System;
    using System.IO;
    using CefSharp;
    using System.Diagnostics.CodeAnalysis;
    using System.Windows;
    using System.Windows.Input;
    using System.Windows.Controls;
    using CefSharp.Wpf.Internals;

    /// <summary>
    /// 
    /// </summary>
    [ SuppressMessage( "ReSharper", "ReplaceSubstringWithRangeIndexer" ) ]
    [ SuppressMessage( "ReSharper", "ExpressionIsAlwaysNull" ) ]
    [ SuppressMessage( "ReSharper", "MemberCanBeInternal" ) ]
    [ SuppressMessage( "ReSharper", "UnusedParameter.Global" ) ]
    [ SuppressMessage( "ReSharper", "MemberCanBePrivate.Global" ) ]
    public static class NetUtility
    {
        /// <summary>
        /// Determines whether the specified tb is focused.
        /// </summary>
        /// <param name="textBox">The tb.</param>
        /// <returns>
        ///   <c>true</c> if the specified tb is focused; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsFocused( TextBox textBox )
        {
            return textBox.IsFocused;
        }

        /// <summary>
        /// Adds the hot key.
        /// </summary>
        /// <param name="form">The form.</param>
        /// <param name="function">The function.</param>
        /// <param name="ctrl">if set to <c>true</c> [control].</param>
        /// <param name="shift">if set to <c>true</c> [shift].</param>
        /// <param name="alt">if set to <c>true</c> [alt].</param>
        public static void AddHotKey( Window form, Action function, bool ctrl = false, 
            bool shift = false, bool alt = false )
        {
            form.KeyDown += delegate( object sender, KeyEventArgs e )
            {
                if( e.IsHotKey( ctrl, shift, alt ) )
                {
                    function( );
                }
            };
        }

        /// <summary>
        /// Determines whether [is fully selected] [the specified tb].
        /// </summary>
        /// <param name="textBox">The tb.</param>
        /// <returns>
        /// <c>true</c> if [is fully selected]
        /// [the specified tb]; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsFullySelected( TextBox textBox )
        {
            return textBox.SelectionLength == textBox.Text.Length;
        }

        /// <summary>
        /// Determines whether the specified tb has selection.
        /// </summary>
        /// <param name="textBox">The tb.</param>
        /// <returns>
        ///   <c>true</c>
        /// if the specified tb has selection;
        /// otherwise,
        /// <c>false</c>.
        /// </returns>
        public static bool HasSelection( TextBox textBox )
        {
            return textBox.SelectionLength > 0;
        }

        /// <summary>
        /// Converts to string.
        /// </summary>
        /// <param name="o">The o.</param>
        /// <returns></returns>
        public static string ConvertToString( this object o )
        {
            return o as string;
        }

        /// <summary>
        /// Determines whether [is hot key] [the specified key].
        /// </summary>
        /// <param name="eventData">The <see cref="KeyEventArgs"/>
        /// instance containing the event data.
        /// </param>
        /// <param name="ctrl">if set to <c>true</c> [control].</param>
        /// <param name="shift">if set to <c>true</c> [shift].</param>
        /// <param name="alt">if set to <c>true</c> [alt].</param>
        /// <returns>
        ///   <c>true</c> if [is hot key] [the specified key];
        /// otherwise, <c>false</c>.
        /// </returns>
        public static bool IsHotKey( this KeyEventArgs eventData, bool ctrl = false,
            bool shift = false, bool alt = false )
        {
            return eventData.IsDown == ctrl && eventData.Handled == shift
                && eventData.Key == Key.LeftShift == alt;
        }

        /// <summary>
        /// Converts to cefstate.
        /// </summary>
        /// <param name="value">if set to <c>true</c> [value].</param>
        /// <returns></returns>
        public static CefState ToCefState( this bool value )
        {
            return value
                ? CefState.Enabled
                : CefState.Disabled;
        }

        /// <summary>
        /// Determines whether [is bitMask on] [the specified bitMask].
        /// </summary>
        /// <param name="num">The number.</param>
        /// <param name="bitMask">The bitMask.</param>
        /// <returns>
        ///   <c>true</c> if [is bitMask on] [the specified bitMask]; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsBitMaskOn( this int num, int bitMask )
        {
            try
            {
                return ( num & bitMask ) != 0;
            }
            catch( Exception ex )
            {
                Fail( ex );
                return false;
            }
        }

        /// <summary>
        /// Get ErrorDialog Dialog.
        /// </summary>
        /// <param name="ex">
        /// The ex.
        /// </param>
        private static void Fail( Exception ex )
        {
            using var _error = new ErrorWindow( ex );
            _error?.SetText( );
            _error?.ShowDialog( );
        }
    }
}