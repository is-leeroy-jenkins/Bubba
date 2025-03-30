// ******************************************************************************************
//     Assembly:                Bubba
//     Author:                  Terry D. Eppler
//     Created:                 03-25-2025
// 
//     Last Modified By:        Terry D. Eppler
//     Last Modified On:        03-25-2025
// ******************************************************************************************
// <copyright file="TextConverter.cs" company="Terry D. Eppler">
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
//   TextConverter.cs
// </summary>
// ******************************************************************************************

namespace Bubba
{
    using System;
    using System.IO;
    using System.Text.Json;
    using System.Text.RegularExpressions;
    using System.Threading.Tasks;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;

    /// <summary>
    /// 
    /// </summary>
    [ SuppressMessage( "ReSharper", "MoveVariableDeclarationInsideLoopCondition" ) ]
    [ SuppressMessage( "ReSharper", "MemberCanBePrivate.Global" ) ]
    [ SuppressMessage( "ReSharper", "UseAwaitUsing" ) ]
    [ SuppressMessage( "ReSharper", "PreferConcreteValueOverDefault" ) ]
    public class TextConverter
    {
        /// <summary>
        /// The enable NLP tokenization
        /// </summary>
        private readonly bool _enable;

        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="TextConverter"/> class.
        /// </summary>
        public TextConverter( )
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TextConverter"/> class.
        /// </summary>
        /// <param name="enable">if set to <c>true</c>
        /// [enable NLP tokenization].</param>
        public TextConverter( bool enable = false ) 
            : this( )
        {
            _enable = enable;
        }

        /// <summary>
        /// Converts the asynchronous.
        /// </summary>
        /// <param name="input">The input file path.</param>
        /// <param name="output">The output file path.</param>
        /// <exception cref="FileNotFoundException">
        /// Input file not found: {input}</exception>
        public async Task ConvertAsync( string input, string output )
        {
            try
            {
                ThrowIf.Empty( input, nameof( input ) );
                ThrowIf.Empty( output, nameof( output ) );
                if( !File.Exists( input ) )
                {
                    throw new FileNotFoundException( $"Input file not found: {input}" );
                }

                var _cleaned = new List<string>( );
                using( var reader = new StreamReader( input ) )
                {
                    string line;
                    while( ( line = await reader.ReadLineAsync( ) ) != null )
                    {
                        var cleaned = CleanLine( line );
                        if( !string.IsNullOrWhiteSpace( cleaned ) )
                        {
                            _cleaned.Add( cleaned );
                        }
                    }
                }

                using var writer = new StreamWriter( output );
                foreach( var _line in _cleaned )
                {
                    string json;
                    if( _enable )
                    {
                        var tokens = Tokenize( _line );
                        json = JsonSerializer.Serialize( new
                        {
                            tokens
                        } );
                    }
                    else
                    {
                        json = JsonSerializer.Serialize( new
                        {
                            line = _line
                        } );
                    }

                    await writer.WriteLineAsync( json );
                }
            }
            catch( Exception ex )
            {
                Fail( ex );
            }
        }

        /// <summary>
        /// Cleans the line.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns></returns>
        private string CleanLine( string input )
        {
            try
            {
                ThrowIf.Empty( input, nameof( input ) );
                if( string.IsNullOrWhiteSpace( input ) )
                {
                    return string.Empty;
                }

                var cleaned = input.Trim( );
                cleaned = Regex.Replace( cleaned, "<.*?>", string.Empty );
                cleaned = cleaned.ToLowerInvariant( );
                cleaned = Regex.Replace( cleaned, @"[^\u0020-\u007E]", string.Empty );
                cleaned = Regex.Replace( cleaned, @"\d", string.Empty );
                cleaned = Regex.Replace( cleaned, @"[^a-zA-Z\s\.,!\?'\-]", string.Empty );
                cleaned = Regex.Replace( cleaned, @"\s+", " " );
                return cleaned;
            }
            catch( Exception ex )
            {
                Fail( ex );
                return string.Empty;
            }
        }

        /// <summary>
        /// Tokenizes the specified cleaned line.
        /// </summary>
        /// <param name="line">The cleaned line.</param>
        /// <returns></returns>
        private IList<string> Tokenize( string line )
        {
            try
            {
                ThrowIf.Empty( line, nameof( line ) );
                var tokens = new List<string>( );
                var words = line.Split( ' ', StringSplitOptions.RemoveEmptyEntries );

                foreach( var word in words )
                {
                    var token = Regex.Replace( word, @"[^\w'-]", "" );
                    if( !string.IsNullOrWhiteSpace( token ) )
                    {
                        tokens.Add( token );
                    }
                }

                return tokens;
            }
            catch( Exception ex )
            {
                Fail( ex );
                return default( IList<string> );
            }
        }

        /// <summary>
        /// Fails the specified ex.
        /// 
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