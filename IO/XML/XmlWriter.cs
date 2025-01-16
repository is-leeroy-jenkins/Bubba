// ******************************************************************************************
//     Assembly:                Bubba
//     Author:                  Terry D. Eppler
//     Created:                 01-16-2025
// 
//     Last Modified By:        Terry D. Eppler
//     Last Modified On:        01-16-2025
// ******************************************************************************************
// <copyright file="XmlWriter.cs" company="Terry D. Eppler">
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
//   XmlWriter.cs
// </summary>
// ******************************************************************************************

namespace Bubba
{
    using System.Xml.Linq;
    using System;
    using System.Linq;
    using System.Threading;

    /// <summary>
    /// 
    /// </summary>
    public class XmlWriter
    {
        /// <summary>
        /// The timer
        /// </summary>
        private protected Timer _timer;

        /// <summary>
        /// Initializes a new instance of the <see cref="XmlWriter"/> class.
        /// </summary>
        public XmlWriter( )
        {
        }

        /// <summary>
        /// Writes the element value.
        /// </summary>
        /// <param name="filePath">The file path.</param>
        /// <param name="elementName">Name of the element.</param>
        /// <param name="value">The value.</param>
        public void WriteElementValue( string filePath, string elementName, string value )
        {
            var _document = XDocument.Load( filePath );
            var _element = _document.Descendants( elementName ).FirstOrDefault( );
            if( _element != null )
            {
                _element.Value = value;
                _document.Save( filePath );
            }
        }

        /// <summary>
        /// Adds the element.
        /// </summary>
        /// <param name="filePath">The file path.</param>
        /// <param name="elementToAdd">The element to add.</param>
        /// <param name="parentElementName">Name of the parent element.</param>
        public void AddElement( string filePath, XElement elementToAdd,
            string parentElementName = null )
        {
            var _document = XDocument.Load( filePath );
            if( string.IsNullOrEmpty( parentElementName ) )
            {
                _document.Root?.Add( elementToAdd );
            }
            else
            {
                var _parent = _document.Descendants( parentElementName ).FirstOrDefault( );
                _parent?.Add( elementToAdd );
            }

            _document.Save( filePath );
        }

        /// <summary>
        /// Updates the attribute.
        /// </summary>
        /// <param name="filePath">The file path.</param>
        /// <param name="elementName">Name of the element.</param>
        /// <param name="attributeName">Name of the attribute.</param>
        /// <param name="newValue">The new value.</param>
        public void UpdateAttribute( string filePath, string elementName, string attributeName,
            string newValue )
        {
            var _document = XDocument.Load( filePath );
            var _element = _document.Descendants( elementName ).FirstOrDefault( );
            if( _element != null )
            {
                _element.SetAttributeValue( attributeName, newValue );
                _document.Save( filePath );
            }
        }

        /// <inheritdoc />
        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        public void Dispose( )
        {
            Dispose( true );
            GC.SuppressFinalize( this );
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="disposing"><c>true</c>
        /// to release both managed and unmanaged resources;
        /// <c>false</c> to release only unmanaged resources.
        /// </param>
        protected virtual void Dispose( bool disposing )
        {
            if( disposing )
            {
                _timer?.Dispose( );
            }
        }

        /// <summary>
        /// Fails the specified ex.
        /// </summary>
        /// <param name="ex">The ex.</param>
        private protected void Fail( Exception ex )
        {
            using var _error = new ErrorWindow( ex );
            _error?.SetText( );
            _error?.ShowDialog( );
        }
    }
}