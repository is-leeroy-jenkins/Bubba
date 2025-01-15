// ******************************************************************************************
//     Assembly:                Bubba
//     Author:                  Terry D. Eppler
//     Created:                 01-15-2025
// 
//     Last Modified By:        Terry D. Eppler
//     Last Modified On:        01-15-2025
// ******************************************************************************************
// <copyright file="QuickResponse.cs" company="Terry D. Eppler">
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
//   QuickResponse.cs
// </summary>
// ******************************************************************************************

namespace Bubba
{
    using System;
    using System.Drawing;
    using System.Drawing.Imaging;
    using System.IO;
    using System.Diagnostics.CodeAnalysis;
    using QRCoder;

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    [ SuppressMessage( "ReSharper", "MemberCanBePrivate.Global" ) ]
    [ SuppressMessage( "ReSharper", "PreferConcreteValueOverDefault" ) ]
    [ SuppressMessage( "ReSharper", "UnusedType.Global" ) ]
    public class QuickResponse : IDisposable
    {
        /// <summary>
        /// The code generator
        /// </summary>
        private readonly QRCodeGenerator _codeGenerator;

        /// <summary>
        /// Initializes a new instance of the <see cref="QuickResponse"/> class.
        /// </summary>
        public QuickResponse( )
        {
            _codeGenerator = new QRCodeGenerator( );
        }

        /// <summary>
        /// Generates the code.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <param name="pixelsPerModule">The pixels per module.</param>
        /// <param name="darkColor">Color of the dark.</param>
        /// <param name="lightColor">Color of the light.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentException">Text cannot be null or empty. - text</exception>
        public Bitmap GenerateCode( string text, int pixelsPerModule = 20, Color? darkColor = null,
            Color? lightColor = null )
        {
            if( string.IsNullOrWhiteSpace( text ) )
            {
                var _message = "Text cannot be null or empty.";
                throw new ArgumentException( _message, nameof( text ) );
            }

            var _data = _codeGenerator.CreateQrCode( text, QRCodeGenerator.ECCLevel.Q );
            var _code = new QRCode( _data );
            darkColor ??= Color.Black;
            lightColor ??= Color.White;
            return _code.GetGraphic( pixelsPerModule, darkColor.Value, lightColor.Value, true );
        }

        /// <summary>
        /// Saves to file.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <param name="filePath">The file path.</param>
        /// <param name="format">The image format.</param>
        /// <param name="pixelsPerModule">The pixels per module.</param>
        /// <param name="darkColor">Color of the dark.</param>
        /// <param name="lightColor">Color of the light.</param>
        public void SaveToFile( string text, string filePath, ImageFormat format = null,
            int pixelsPerModule = 20, Color? darkColor = null, Color? lightColor = null )
        {
            try
            {
                format ??= ImageFormat.Png;
                using var _code = GenerateCode( text, pixelsPerModule, darkColor, lightColor );
                _code.Save( filePath, format );
            }
            catch( Exception ex )
            {
                Fail( ex );
            }
        }

        /// <summary>
        /// Creates the byte array.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <param name="format">The image format.</param>
        /// <param name="pixelsPerModule">The pixels per module.</param>
        /// <param name="darkColor">Color of the dark.</param>
        /// <param name="lightColor">Color of the light.</param>
        /// <returns></returns>
        public byte[ ] CreateByteArray( string text, ImageFormat format = null,
            int pixelsPerModule = 20, Color? darkColor = null, Color? lightColor = null )
        {
            try
            {
                format ??= ImageFormat.Png;
                using var _bitmap = GenerateCode( text, pixelsPerModule, darkColor, lightColor );
                using var _stream = new MemoryStream( );
                _bitmap.Save( _stream, format );
                return _stream.ToArray( );
            }
            catch( Exception ex )
            {
                Fail( ex );
                return default( byte[ ] );
            }
        }

        /// <summary>
        /// Creates the stream.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <param name="format">The image format.</param>
        /// <param name="pixelsPerModule">The pixels per module.</param>
        /// <param name="darkColor">Color of the dark.</param>
        /// <param name="lightColor">Color of the light.</param>
        /// <returns></returns>
        public MemoryStream CreateStream( string text, ImageFormat format = null,
            int pixelsPerModule = 20, Color? darkColor = null, Color? lightColor = null )
        {
            try
            {
                format ??= ImageFormat.Png;
                var _stream = new MemoryStream( );
                using var _bitmap = GenerateCode( text, pixelsPerModule, darkColor, lightColor );
                _bitmap.Save( _stream, format );
                _stream.Position = 0;
                return _stream;
            }
            catch( Exception ex )
            {
                Fail( ex );
                return default( MemoryStream );
            }
        }

        /// <summary>
        /// Creates the base64.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <param name="imageFormat">The image format.</param>
        /// <param name="pixelsPerModule">The pixels per module.</param>
        /// <param name="darkColor">Color of the dark.</param>
        /// <param name="lightColor">Color of the light.</param>
        /// <returns></returns>
        public string CreateBase64( string text, ImageFormat imageFormat = null,
            int pixelsPerModule = 20, Color? darkColor = null, Color? lightColor = null )
        {
            try
            {
                var _array = CreateByteArray( text, imageFormat, pixelsPerModule, darkColor,
                    lightColor );

                return Convert.ToBase64String( _array );
            }
            catch( Exception ex )
            {
                Fail( ex );
                return string.Empty;
            }
        }

        /// <inheritdoc />
        /// <summary>
        /// Performs application-defined tasks associated with freeing,
        /// releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose( )
        {
            _codeGenerator?.Dispose( );
        }

        /// <summary>
        /// Fails the specified ex.
        /// </summary>
        /// <param name="ex">The ex.</param>
        private protected static void Fail( Exception ex )
        {
            using var _error = new ErrorWindow( ex );
            _error?.SetText( );
            _error?.ShowDialog( );
        }
    }
}