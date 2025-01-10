// ******************************************************************************************
//     Assembly:                Bubba
//     Author:                  Terry D. Eppler
//     Created:                 01-10-2025
// 
//     Last Modified By:        Terry D. Eppler
//     Last Modified On:        01-10-2025
// ******************************************************************************************
// <copyright file="TranslationRequest.cs" company="Terry D. Eppler">
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
//   TranslationRequest.cs
// </summary>
// ******************************************************************************************

namespace Bubba
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.IO;
    using System.Linq;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Threading.Tasks;
    using Newtonsoft.Json;
    using Properties;

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    /// <seealso cref="T:Bubba.GptRequest" />
    [ SuppressMessage( "ReSharper", "UnusedType.Global" ) ]
    [ SuppressMessage( "ReSharper", "MemberCanBePrivate.Global" ) ]
    [ SuppressMessage( "ReSharper", "ClassCanBeSealed.Global" ) ]
    [ SuppressMessage( "ReSharper", "PreferConcreteValueOverDefault" ) ]
    public class TranslationRequest : GptRequest
    {
        /// <summary>
        /// The response format
        /// </summary>
        private protected string _responseFormat;

        /// <summary>
        /// The file path
        /// </summary>
        private protected string _file;

        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="TranslationRequest"/> class.
        /// </summary>
        /// <inheritdoc />
        public TranslationRequest( )
            : base( )
        {
            _entry = new object( );
            _httpClient = new HttpClient( );
            _model = "whisper-1";
            _endPoint = GptEndPoint.Translations;
            _responseFormat = "mp3";
            _temperature = 0.18;
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
        /// The path to the audio file object (not file name) to transcribe,
        /// in one of these formats: flac, mp3, mp4, mpeg, mpga, m4a, ogg, wav, or webm.
        /// </summary>
        /// <value>
        /// The file.
        /// </value>
        [ JsonProperty( "file" ) ]
        public string File
        {
            get
            {
                return _file;
            }
            set
            {
                if( _file != value )
                {
                    _file = value;
                    OnPropertyChanged( nameof( File ) );
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
        [ JsonProperty( "model" ) ]
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
        /// The format of the output, in one of these options:
        /// json, text, srt, verbose_json, or vtt.
        /// </summary>
        /// <value>
        /// The response format.
        /// </value>
        [ JsonProperty( "response_format" ) ]
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
        /// Gets or sets the input.
        /// </summary>
        /// <value>
        /// The input.
        /// </value>
        [ JsonProperty( "prompt" ) ]
        public string Prompt
        {
            get
            {
                return _prompt;
            }
            set
            {
                if( _prompt != value )
                {
                    _prompt = value;
                    OnPropertyChanged( nameof( Prompt ) );
                }
            }
        }

        /// <inheritdoc />
        /// <summary>
        /// The sampling temperature, between 0 and 1.
        /// Higher values like 0.8 will make the output more random,
        /// while lower values like 0.2 will make it more focused and deterministic.
        /// If set to 0, the model will use log probability to automatically increase
        /// the temperature until certain thresholds are hit.
        /// </summary>
        /// <value>
        /// The temperature.
        /// </value>
        [ JsonProperty( "temperature" ) ]
        public override double Temperature
        {
            get
            {
                return _temperature;
            }
        }

        /// <summary>
        /// Translates the audio asynchronous.
        /// </summary>
        /// <param name="path">The file path.</param>
        /// <returns></returns>
        public async Task<string> TranslateAudioAsync( string path )
        {
            try
            {
                ThrowIf.Empty( path, nameof( path ) );
                using var _client = new HttpClient( );
                _client.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue( "Bearer", _apiKey );

                using var _fileStream = new FileStream( path, FileMode.Open, FileAccess.Read );
                var _content = new StreamContent( _fileStream );
                _content.Headers.ContentType = new MediaTypeHeaderValue( "audio/mpeg" );
                var _form = new MultipartFormDataContent
                {
                    {
                        _content, "file", Path.GetFileName( path )
                    },
                    {
                        new StringContent( "tts-1-hd" ), "model"
                    }
                };

                _form.Add( new StringContent( "0.5" ), "temperature" );
                _form.Add( new StringContent( "en" ), "language" );
                var _response = await _client.PostAsync( _endPoint, _form );
                _response.EnsureSuccessStatusCode( );
                var _responseContent = await _response.Content.ReadAsStringAsync( );
                return _responseContent;
            }
            catch( Exception ex )
            {
                Fail( ex );
                return string.Empty;
            }
        }

        /// <inheritdoc />
        /// <summary>
        /// Gets the data.
        /// </summary>
        /// <returns>
        /// </returns>
        public override IDictionary<string, object> GetData( )
        {
            try
            {
                _data.Add( "model", _model );
                _data.Add( "endpoint", _endPoint );
                _data.Add( "number", _number );
                _data.Add( "max_completion_tokens", _maximumTokens );
                _data.Add( "store", _store );
                _data.Add( "stream", _stream );
                _data.Add( "temperature", _temperature );
                _data.Add( "frequency_penalty", _frequencyPenalty );
                _data.Add( "presence_penalty", _presencePenalty );
                _data.Add( "top_p", _topPercent );
                _data.Add( "response_format", _responseFormat );
                _data.Add( "endpoint", _endPoint );
                if( !string.IsNullOrEmpty( _file ) )
                {
                    _data.Add( "filepath", _file );
                }

                return _data?.Any( ) == true
                    ? _data
                    : default( IDictionary<string, object> );
            }
            catch( Exception ex )
            {
                Fail( ex );
                return default( IDictionary<string, object> );
            }
        }

        /// <summary>
        /// Converts to string.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString( )
        {
            try
            {
                return _data?.Any( ) == true
                    ? _data.ToJson( )
                    : string.Empty;
            }
            catch( Exception ex )
            {
                Fail( ex );
                return string.Empty;
            }
        }
    }
}