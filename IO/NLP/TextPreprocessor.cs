// ******************************************************************************************
//     Assembly:                Bubba
//     Author:                  Terry D. Eppler
//     Created:                 02-16-2025
// 
//     Last Modified By:        Terry D. Eppler
//     Last Modified On:        02-16-2025
// ******************************************************************************************
// <copyright file="TextPreprocessor.cs" company="Terry D. Eppler">
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
//   TextPreprocessor.cs
// </summary>
// ******************************************************************************************

namespace Bubba
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    /// <seealso cref="T:Bubba.ILanguageProcessor" />
    [ SuppressMessage( "ReSharper", "MemberCanBePrivate.Global" ) ]
    [ SuppressMessage( "ReSharper", "PreferConcreteValueOverDefault" ) ]
    [ SuppressMessage( "ReSharper", "UseCollectionExpression" ) ]
    public class TextPreprocessor : ILanguageProcessor
    {
        /// <summary>
        /// The stop words
        /// </summary>
        private protected static readonly HashSet<string> _stopWords = new HashSet<string>
        {
            "a",
            "an",
            "the",
            "and",
            "or",
            "but",
            "on",
            "in",
            "with",
            "to",
            "for",
            "is",
            "was"
        };

        /// <inheritdoc />
        /// <summary>
        /// Tokenizes the specified text.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <returns></returns>
        public string[ ] Tokenize( string text )
        {
            try
            {
                ThrowIf.Empty( text, nameof( text ) );
                return text.Split( new[ ]
                {
                    ' ',
                    '.',
                    ',',
                    '!',
                    '?'
                }, StringSplitOptions.RemoveEmptyEntries );
            }
            catch( Exception ex )
            {
                Fail( ex );
                return default( string[ ] );
            }
        }

        /// <inheritdoc />
        /// <summary>
        /// Normalizes the text.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <returns></returns>
        public string NormalizeText( string text )
        {
            try
            {
                ThrowIf.Negative( text, nameof( text ) );
                return text.ToLowerInvariant( ).Trim( );
            }
            catch( Exception ex )
            {
                Fail( ex );
                return string.Empty;
            }
        }

        /// <inheritdoc />
        /// <summary>
        /// Removes the stop words.
        /// </summary>
        /// <param name="tokens">The tokens.</param>
        /// <returns></returns>
        public IEnumerable<string> RemoveStopWords( string[ ] tokens )
        {
            try
            {
                ThrowIf.Null( tokens, nameof( tokens ) );
                var _stops = tokens
                    ?.Where( token => !_stopWords.Contains( token ) )
                    ?.ToArray( );

                return ( _stops?.Any( ) == true )
                    ? _stops
                    : default( string[ ] );
            }
            catch( Exception ex )
            {
                Fail( ex );
                return default( string[ ] );
            }
        }

        /// <inheritdoc />
        /// <summary>
        /// Stems the word.
        /// </summary>
        /// <param name="word">The word.</param>
        /// <returns></returns>
        public string StemWord( string word )
        {
            try
            {
                // Basic stemming logic; replace with a robust stemming library as needed.
                ThrowIf.Empty( word, nameof( word ) );
                if( word.EndsWith( "ing" ) )
                {
                    return word.Substring( 0, word.Length - 3 );
                }

                if( word.EndsWith( "ed" ) )
                {
                    return word.Substring( 0, word.Length - 2 );
                }

                return word;
            }
            catch( Exception ex )
            {
                Fail( ex );
                return string.Empty;
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