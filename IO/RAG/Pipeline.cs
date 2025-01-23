// ******************************************************************************************
//     Assembly:                Bubba
//     Author:                  Terry D. Eppler
//     Created:                 01-22-2025
// 
//     Last Modified By:        Terry D. Eppler
//     Last Modified On:        01-22-2025
// ******************************************************************************************
// <copyright file="Pipeline.cs" company="Terry D. Eppler">
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
//   Pipeline.cs
// </summary>
// ******************************************************************************************

namespace Bubba
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Threading;
    using System.Threading.Tasks;

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    /// <seealso cref="T:System.IDisposable" />
    [ SuppressMessage( "ReSharper", "MemberCanBePrivate.Global" ) ]
    [ SuppressMessage( "ReSharper", "UnusedType.Global" ) ]
    public class Pipeline : IDisposable
    {
        /// <summary>
        /// The busy
        /// </summary>
        private protected bool _busy;

        /// <summary>
        /// The entry
        /// </summary>
        private protected object _entry = new object( );

        /// <summary>
        /// The timer
        /// </summary>
        private protected Timer _timer;

        /// <summary>
        /// The status update
        /// </summary>
        private protected Action _statusUpdate;

        /// <summary>
        /// The timer callback
        /// </summary>
        private protected TimerCallback _timerCallback;

        /// <summary>
        /// The extractor
        /// </summary>
        private readonly Extractor _extractor;

        /// <summary>
        /// The chunker
        /// </summary>
        private readonly Chunker _chunker;

        /// <summary>
        /// The retriever
        /// </summary>
        private readonly Retriever _retriever;

        /// <summary>
        /// The file request
        /// </summary>
        private readonly GptFileRequest _request;

        /// <summary>
        /// Initializes a new instance of the <see cref="Pipeline"/> class.
        /// </summary>
        /// <param name="extractor">The extractor.</param>
        /// <param name="chunker">The chunker.</param>
        /// <param name="retriever">The retriever.</param>
        /// <param name="request">The file request.</param>
        public Pipeline( Extractor extractor, Chunker chunker, Retriever retriever,
            GptFileRequest request )
        {
            _extractor = extractor;
            _chunker = chunker;
            _retriever = retriever;
            _request = request;
        }

        /// <summary>
        /// Begins the initialize.
        /// </summary>
        private protected void Busy( )
        {
            try
            {
                lock( _entry )
                {
                    _busy = true;
                }
            }
            catch( Exception ex )
            {
                Fail( ex );
            }
        }

        /// <summary>
        /// Ends the initialize.
        /// </summary>
        private protected void Chill( )
        {
            try
            {
                lock( _entry )
                {
                    _busy = false;
                }
            }
            catch( Exception ex )
            {
                Fail( ex );
            }
        }

        /// <summary>
        /// Executes the asynchronous.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <param name="directory">The PDF directory.</param>
        /// <returns></returns>
        public async Task<string> ExecuteAsync( string query, string directory )
        {
            try
            {
                Busy( );
                ThrowIf.Empty( query, nameof( query ) );
                ThrowIf.Empty( directory, nameof( directory ) );
                var _texts = _extractor.GetTextFromFolder( directory );
                var _text = _chunker.ChunkText( _texts );
                var _chunks = _retriever.GetChunks( query, _text );
                var _response = await _request.GetResponseAsync( query, _chunks );
                Chill( );
                return !string.IsNullOrEmpty( _response )
                    ? _response
                    : string.Empty;
            }
            catch( Exception ex )
            {
                Fail( ex );
                return string.Empty;
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