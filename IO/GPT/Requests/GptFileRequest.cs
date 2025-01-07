// ******************************************************************************************
//     Assembly:                Bubba
//     Author:                  Terry D. Eppler
//     Created:                 01-07-2025
// 
//     Last Modified By:        Terry D. Eppler
//     Last Modified On:        01-07-2025
// ******************************************************************************************
// <copyright file="GptFileRequest.cs" company="Terry D. Eppler">
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
//   GptFileRequest.cs
// </summary>
// ******************************************************************************************

namespace Bubba
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.IO;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Threading.Tasks;
    using Properties;

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    /// <seealso cref="T:Bubba.GptRequest" />
    [ SuppressMessage( "ReSharper", "UnusedType.Global" ) ]
    public class GptFileRequest : GptRequest
    {
        /// <summary>
        /// The response format
        /// </summary>
        private protected string _responseFormat;

        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="GptFileRequest"/> class.
        /// </summary>
        /// <inheritdoc />
        public GptFileRequest( )
            : base( )
        {
            _entry = new object( );
            _httpClient = new HttpClient( );
            _presencePenalty = 0.00;
            _frequencyPenalty = 0.00;
            _topPercent = 0.11;
            _temperature = 0.18;
            _maximumTokens = 2048;
            _model = "gpt-4o-mini";
            _endPoint = GptEndPoint.Files;
            _number = 1;
        }

        /// <inheritdoc />
        /// <summary>
        /// Gets or sets a value indicating whether this
        /// <see cref="P:Bubba.GptRequest.HttpClient" /> is store.
        /// </summary>
        /// <value>
        ///   <c>true</c> if store; otherwise, <c>false</c>.
        /// </value>
        public override HttpClient HttpClient
        {
            get
            {
                return _httpClient;
            }
            set
            {
                if( _httpClient != value )
                {
                    _httpClient = value;
                    OnPropertyChanged( nameof( HttpClient ) );
                }
            }
        }

        /// <summary>
        /// Gets the chat model.
        /// </summary>
        /// <value>
        /// The chat model.
        /// </value>
        /// <inheritdoc />
        public override string Model
        {
            get
            {
                return _model;
            }
            set
            {
                if( _model != value )
                {
                    _model = value;
                    OnPropertyChanged( nameof( Model ) );
                }
            }
        }

        /// <inheritdoc />
        /// <summary>
        /// Gets or sets the messages.
        /// </summary>
        /// <value>
        /// The messages.
        /// </value>
        public IList<IGptMessage> Messages
        {
            get
            {
                return _messages;
            }
            set
            {
                if( _messages != value )
                {
                    _messages = value;
                    OnPropertyChanged( nameof( Messages ) );
                }
            }
        }

        /// <inheritdoc />
        /// <summary>
        /// Gets the end point.
        /// </summary>
        /// <value>
        /// The end point.
        /// </value>
        public override string EndPoint
        {
            get
            {
                return _endPoint;
            }
            set
            {
                if( _endPoint != value )
                {
                    _endPoint = value;
                    OnPropertyChanged( nameof( EndPoint ) );
                }
            }
        }

        /// <inheritdoc />
        /// <summary>
        /// THe number 'n' of responses returned by the API.
        /// </summary>
        /// <value>
        /// The user identifier.
        /// </value>
        public override int Number
        {
            get
            {
                return _number;
            }
            set
            {
                if( _number != value )
                {
                    _number = value;
                    OnPropertyChanged( nameof( Number ) );
                }
            }
        }

        /// <inheritdoc />
        /// <summary>
        /// Gets the maximum tokens.
        /// </summary>
        /// <value>
        /// The maximum tokens.
        /// </value>
        public override int MaximumTokens
        {
            get
            {
                return _maximumTokens;
            }
            set
            {
                if( _maximumTokens != value )
                {
                    _maximumTokens = value;
                    OnPropertyChanged( nameof( MaximumTokens ) );
                }
            }
        }

        /// <inheritdoc />
        /// <summary>
        /// Gets or sets a value indicating whether this
        /// <see cref="T:Bubba.GptRequest" /> is store.
        /// </summary>
        /// <value>
        ///   <c>true</c> if store; otherwise, <c>false</c>.
        /// </value>
        public override bool Store
        {
            get
            {
                return _store;
            }
            set
            {
                if( _store != value )
                {
                    _store = value;
                    OnPropertyChanged( nameof( Store ) );
                }
            }
        }

        /// <inheritdoc />
        /// <summary>
        /// Gets or sets a value indicating whether this
        /// <see cref="P:Bubba.ChatCompletionRequest.Stream" /> is stream.
        /// </summary>
        /// <value>
        ///   <c>true</c> if stream; otherwise, <c>false</c>.
        /// </value>
        public override bool Stream
        {
            get
            {
                return _stream;
            }
            set
            {
                if( _stream != value )
                {
                    _stream = value;
                    OnPropertyChanged( nameof( Stream ) );
                }
            }
        }

        /// <inheritdoc />
        /// <summary>
        /// Gets or sets the response format.
        /// </summary>
        /// <value>
        /// The response format.
        /// </value>
        public string ResponseFormat
        {
            get
            {
                return _responseFormat;
            }
            set
            {
                if( _responseFormat != value )
                {
                    _responseFormat = value;
                    OnPropertyChanged( nameof( ResponseFormat ) );
                }
            }
        }

        /// <summary>
        /// Uploads the file asynchronous.
        /// </summary>
        /// <param name="filePath">The file path.</param>
        /// <param name="purpose">The purpose.</param>
        /// <returns></returns>
        public async Task<string> UploadFileAsync( string filePath, string purpose = "fine-tune" )
        {
            using var _client = new HttpClient( );
            _client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue( "Bearer", _apiKey );

            using var _fileStream = new FileStream( filePath, FileMode.Open, FileAccess.Read );
            var _fileContent = new StreamContent( _fileStream );
            _fileContent.Headers.ContentType = new MediaTypeHeaderValue( "application/json" );
            var _formData = new MultipartFormDataContent
            {
                {
                    _fileContent, "file", Path.GetFileName( filePath )
                },
                {
                    new StringContent( purpose ), "purpose"
                }
            };

            // Send the POST request
            var _response = await _client.PostAsync( _endPoint, _formData );
            _response.EnsureSuccessStatusCode( );
            var _responseContent = await _response.Content.ReadAsStringAsync( );
            return _responseContent;
        }

        /// <summary>
        /// Lists the files asynchronous.
        /// </summary>
        /// <returns></returns>
        public async Task<string> ListFilesAsync( )
        {
            using var _client = new HttpClient( );
            _client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue( "Bearer", _apiKey );

            var _response = await _client.GetAsync( _endPoint );
            _response.EnsureSuccessStatusCode( );
            var _responseContent = await _response.Content.ReadAsStringAsync( );
            return _responseContent;
        }

        /// <summary>
        /// Retrieves the file asynchronous.
        /// </summary>
        /// <param name="fileId">The file identifier.</param>
        /// <returns></returns>
        public async Task<string> RetrieveFileAsync( string fileId )
        {
            using var _client = new HttpClient( );
            _client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue( "Bearer", _apiKey );

            var _response = await _client.GetAsync( $"{_endPoint}/{fileId}" );
            _response.EnsureSuccessStatusCode( );
            var _responseContent = await _response.Content.ReadAsStringAsync( );
            return _responseContent;
        }

        /// <summary>
        /// Deletes the file asynchronous.
        /// </summary>
        /// <param name="fileId">The file identifier.</param>
        /// <returns></returns>
        public async Task<string> DeleteFileAsync( string fileId )
        {
            using var _client = new HttpClient( );
            _client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue( "Bearer", _apiKey );

            var _response = await _client.DeleteAsync( $"{_endPoint}/{fileId}" );
            _response.EnsureSuccessStatusCode( );
            var _responseContent = await _response.Content.ReadAsStringAsync( );
            return _responseContent;
        }
    }
}