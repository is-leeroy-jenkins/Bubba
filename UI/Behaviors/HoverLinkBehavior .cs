// ******************************************************************************************
//     Assembly:                Bubba
//     Author:                  Terry D. Eppler
//     Created:                 ${CurrentDate.Month}-${CurrentDate.Day}-${CurrentDate.Year}
//
//     Last Modified By:        Terry D. Eppler
//     Last Modified On:        ${CurrentDate.Month}-${CurrentDate.Day}-${CurrentDate.Year}
// ******************************************************************************************
// <copyright file="${File.FileName}" company="Terry D. Eppler">
//     Badger is a budget execution & data analysis tool for EPA analysts 
//     based on WPF, Net 6, and written in C Sharp.
//     
//     Copyright �  2022 Terry D. Eppler
//
//    Permission is hereby granted, free of charge, to any person obtaining a copy
//    of this software and associated documentation files (the �Software�),
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
//    THE SOFTWARE IS PROVIDED �AS IS�, WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED,
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
//   ${File.FileName}
// </summary>
// ******************************************************************************************

namespace Bubba
{
    using System;
    using System.Globalization;
    using System.Windows;
    using System.Windows.Data;
    using CefSharp;
    using CefSharp.Wpf;
    using Microsoft.Xaml.Behaviors;

    /// <summary>
    /// 
    /// </summary>
    public class HoverLinkBehavior : Behavior<ChromiumWebBrowser>
    {
        // Using a DependencyProperty as the backing store for HoverLink.
        // This enables animation, styling, binding, etc...
        /// <summary>
        /// The hover link property
        /// </summary>
        public static readonly DependencyProperty HoverLinkProperty = DependencyProperty.Register( "HoverLink", typeof( string ), typeof( HoverLinkBehavior ), new PropertyMetadata( string.Empty ) );

        /// <summary>
        /// Gets or sets the hover link.
        /// </summary>
        /// <value>
        /// The hover link.
        /// </value>
        public string HoverLink
        {
            get
            {
                return ( string )GetValue( HoverLinkProperty );
            }
            set
            {
                SetValue( HoverLinkProperty, value );
            }
        }

        /// <summary>
        /// Called after the behavior is attached to an AssociatedObject.
        /// </summary>
        /// <remarks>
        /// Override this to hook up functionality to the AssociatedObject.
        /// </remarks>
        protected override void OnAttached( )
        {
            AssociatedObject.StatusMessage += OnStatusMessageChanged;
        }

        /// <summary>
        /// Called when the behavior is being detached from its AssociatedObject,
        /// but before it has actually occurred.
        /// </summary>
        /// <remarks>
        /// Override this to unhook functionality from the AssociatedObject.
        /// </remarks>
        protected override void OnDetaching( )
        {
            AssociatedObject.StatusMessage -= OnStatusMessageChanged;
        }

        /// <summary>
        /// Called when [status message changed].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="StatusMessageEventArgs"/>
        /// instance containing the event data.</param>
        private void OnStatusMessageChanged( object sender, StatusMessageEventArgs e )
        {
            var chromiumWebBrowser = sender as ChromiumWebBrowser;
            chromiumWebBrowser.Dispatcher.BeginInvoke( ( Action )( ( ) => HoverLink = e.Value ) );
        }
    }
}
