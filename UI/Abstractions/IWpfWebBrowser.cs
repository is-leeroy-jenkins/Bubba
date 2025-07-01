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
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Windows;
    using CefSharp;
    using CefSharp.Enums;
    using CefSharp.Internals;
    using CefSharp.Wpf;

    /// <summary>
    /// WPF specific implementation, has reference to some of the commands
    /// and properties the <see cref="ChromiumWebBrowser" /> exposes.
    /// </summary>
    /// <seealso cref="CefSharp.IWebBrowser" />
    public interface IWpfWebBrowser : IWpfChromiumWebBrowser, IInputElement
    {
        /// <summary>
        /// Raised every time <see cref="IRenderWebBrowser.OnPaint"/> is called. You can access the underlying buffer, though it's
        /// preferable to either override <see cref="ChromiumWebBrowser.OnPaint"/> or implement your own <see cref="IRenderHandler"/> as there is no outwardly
        /// accessible locking (locking is done within the default <see cref="IRenderHandler"/> implementations).
        /// It's important to note this event is fired on a CEF UI thread, which by default is not the same as your application UI thread
        /// </summary>
        event EventHandler<PaintEventArgs> Paint;

        /// <summary>
        /// Raised every time <see cref="IRenderWebBrowser.OnVirtualKeyboardRequested(IBrowser, TextInputMode)"/> is called. 
        /// It's important to note this event is fired on a CEF UI thread, which by default is not the same as your application UI thread
        /// </summary>
        event EventHandler<VirtualKeyboardRequestedEventArgs> VirtualKeyboardRequested;
    }
}
