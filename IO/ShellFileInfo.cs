// ******************************************************************************************
//     Assembly:                Bubba
//     Author:                  Terry D. Eppler
//     Created:                 11-26-2024
// 
//     Last Modified By:        Terry D. Eppler
//     Last Modified On:        11-26-2024
// ******************************************************************************************
// <copyright file="ShellFileInfo.cs" company="Terry D. Eppler">
//    Bubba is a small windows (wpf) application for interacting with
//    Chat GPT that's developed in C-Sharp under the MIT license
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
//   ShellFileInfo.cs
// </summary>
// ******************************************************************************************

namespace Bubba
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Runtime.InteropServices;

    /// <summary>
    /// Contains information about a file object.
    /// </summary>
    [ SuppressMessage( "ReSharper", "MemberCanBeInternal" ) ]
    public struct ShellFileInfo
    {
        /// <summary>
        /// Array of values that indicates the attributes of the file object.
        /// For information about these values, see the IShellFolder::GetAttributesOf
        /// method.
        /// </summary>
        public uint Attributes;

        /// <summary>
        /// Handle to the icon that represents the file. You are responsible for
        /// destroying this handle with DestroyIcon when you no longer need it.
        /// </summary>
        public IntPtr IconHandle;

        /// <summary>
        /// Index of the icon image within the system image list.
        /// </summary>
        public IntPtr IconIndex;

        /// <summary>
        /// String that contains the name of the file as it appears in the Microsoft
        /// Windows Shell, or the path and file name of the file that contains the
        /// icon representing the file.
        /// </summary>
        [ MarshalAs( UnmanagedType.ByValTStr, SizeConst = 260 ) ]
        public string DisplayName;

        /// <summary>
        /// String that describes the type of file.
        /// </summary>
        [ MarshalAs( UnmanagedType.ByValTStr, SizeConst = 80 ) ]
        public string TypeName;
    };
}