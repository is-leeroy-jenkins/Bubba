// ******************************************************************************************
//     Assembly:                Bubba
//     Author:                  Terry D. Eppler
//     Created:                 01-12-2025
// 
//     Last Modified By:        Terry D. Eppler
//     Last Modified On:        01-12-2025
// ******************************************************************************************
// <copyright file="GptRequest.cs" company="Terry D. Eppler">
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
//   GptRequest.cs
// </summary>
// ******************************************************************************************

namespace Bubba
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Net.Http;
    using System.Text;
    using System.Text.Json;
    using System.Text.Json.Serialization;
    using System.Threading.Tasks;
    using Newtonsoft.Json;
    using Properties;

    /// <inheritdoc />
    [ SuppressMessage( "ReSharper", "UnusedType.Global" ) ]
    [ SuppressMessage( "ReSharper", "MemberCanBePrivate.Global" ) ]
    [ SuppressMessage( "ReSharper", "FieldCanBeMadeReadOnly.Global" ) ]
    [ SuppressMessage( "ReSharper", "MemberCanBeProtected.Global" ) ]
    [ SuppressMessage( "ReSharper", "PreferConcreteValueOverDefault" ) ]
    [ SuppressMessage( "ReSharper", "PossibleNullReferenceException" ) ]
    [ SuppressMessage( "ReSharper", "VirtualMemberNeverOverridden.Global" ) ]
    public class GptRequest : GptRequestBase, IGptRequest
    {
        /// <summary>
        /// The HTTP client
        /// </summary>
        private protected HttpClient _httpClient;

        /// <summary>
        /// The stop sequence
        /// </summary>
        private protected IList<string> _stop;

        /// <summary>
        /// The modalities
        /// </summary>
        private protected string _modalities;

        /// <summary>
        /// The response format
        /// </summary>
        private protected string _responseFormat;

        /// <inheritdoc />
        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="T:Bubba.GptRequest" /> class.
        /// </summary>
        public GptRequest( )
        {
            _header = new GptHeader( );
            _apiKey = _header.ApiKey;
            _stop = new List<string>( );
            _store = false;
            _stream = true;
            _presencePenalty = 0.00;
            _frequencyPenalty = 0.00;
            _topPercent = 0.11;
            _temperature = 0.18;
            _maximumTokens = 2048;
            _number = 1;
            _responseFormat = "text";
            _modalities = "['text','audio']";
        }

        /// <inheritdoc />
        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="T:Bubba.GptRequest" /> class.
        /// </summary>
        /// <param name = "system" > </param>
        /// <param name = "user" > </param>
        /// <param name = "parameter" > </param>
        public GptRequest( string system, string user, IGptParameter parameter )
        {
            _header = new GptHeader( );
            _apiKey = _header.ApiKey;
            _stop = new List<string>( );
            _number = parameter.Number;
            _store = parameter.Store;
            _stream = parameter.Stream;
            _frequencyPenalty = parameter.FrequencyPenalty;
            _presencePenalty = parameter.PresencePenalty;
            _topPercent = parameter.TopPercent;
            _temperature = parameter.Temperature;
            _maximumTokens = parameter.MaximumTokens;
            _responseFormat = parameter.ResponseFormat;
        }

        /// <inheritdoc />
        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="T:Bubba.GptRequest" /> class.
        /// </summary>
        /// <param name="request">The GPT request.</param>
        public GptRequest( GptRequest request )
        {
            _apiKey = request.ApiKey;
            _number = request.Number;
            _stream = request.Stream;
            _store = request.Store;
            _frequencyPenalty = request.FrequencyPenalty;
            _presencePenalty = request.PresencePenalty;
            _topPercent = request.TopPercent;
            _temperature = request.Temperature;
            _maximumTokens = request.MaximumTokens;
            _stop = request.Stop;
            _body = request.Body;
        }

        /// <summary>
        /// Deconstructs the specified user identifier.
        /// </summary>
        /// <param name = "header" > </param>
        /// <param name = "endPoint" > </param>
        /// <param name = "user" > </param>
        /// <param name = "system" > </param>
        /// <param name = "store" > </param>
        /// <param name = "stream" > </param>
        /// <param name = "model" > </param>
        /// <param name="number">The user identifier.</param>
        /// <param name="presence">The presence.</param>
        /// <param name="frequency">The frequency.</param>
        /// <param name="temperature">The temperature.</param>
        /// <param name = "topPercent" > </param>
        /// <param name="tokens">The maximum tokens.</param>
        public void Deconstruct( out GptHeader header, out string endPoint, out string user,
            out string system, out bool store, out bool stream,
            out string model, out int number, out double presence,
            out double frequency, out double temperature, out double topPercent,
            out int tokens )
        {
            header = _header;
            system = _body.SystemMessage.Content;
            user = _body.UserMessage.Content;
            endPoint = _endPoint;
            store = _store;
            stream = _stream;
            model = _model;
            number = _number;
            presence = _presencePenalty;
            frequency = _frequencyPenalty;
            temperature = Temperature;
            topPercent = TopPercent;
            tokens = _maximumTokens;
        }

        /// <summary>
        /// Gets or sets the stop sequences.
        /// </summary>
        /// <value>
        /// The stop sequences.
        /// </value>
        [ JsonPropertyName( "stop" ) ]
        public virtual IList<string> Stop
        {
            get
            {
                return _stop;
            }
            set
            {
                if( _stop != value )
                {
                    _stop = value;
                    OnPropertyChanged( nameof( Stop ) );
                }
            }
        }

        /// <inheritdoc />
        /// <summary>
        /// Gets or sets a value indicating whether this
        /// <see cref="P:Bubba.GptRequest.HttpClient" /> is store.
        /// </summary>
        /// <value>
        ///   <c>true</c> if store; otherwise, <c>false</c>.
        /// </value>
        public virtual HttpClient HttpClient
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
        /// Gets or sets the modalities.
        /// </summary>
        /// <value>
        /// The modalities.
        /// </value>
        [ JsonPropertyName( "modalities" ) ]
        public virtual string Modalities
        {
            get
            {
                return _modalities;
            }
            set
            {
                if( _modalities != value )
                {
                    _modalities = value;
                    OnPropertyChanged( nameof( Modalities ) );
                }
            }
        }

        /// <summary>
        /// Gets or sets the response format.
        /// </summary>
        /// <value>
        /// The response format.
        /// </value>
        [ JsonPropertyName( "response_format" ) ]
        public virtual string ResponseFormat
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

        /// <inheritdoc />
        /// <summary>
        /// Gets the chat model.
        /// </summary>
        /// <value>
        /// The chat model.
        /// </value>
        public virtual GptBody Body
        {
            get
            {
                return _body;
            }
            set
            {
                if( _body != value )
                {
                    _body = value;
                    OnPropertyChanged( nameof( Body ) );
                }
            }
        }

        /// <summary>
        /// Posts the json asynchronous.
        /// </summary>
        /// <param name="endpoint">The endpoint.</param>
        /// <param name="payload">The payload.</param>
        /// <returns></returns>
        /// <exception cref="HttpRequestException">
        /// Error: {_response.StatusCode}, {_error}
        /// </exception>
        private protected virtual async Task<string> PostAsync( string endpoint,
            object payload )
        {
            try
            {
                ThrowIf.Empty( endpoint, nameof( endpoint ) );
                ThrowIf.Null( payload, nameof( payload ) );
                var _url = GptEndPoint.TextGeneration;
                var _gpt = new GptHeader( );
                _httpClient.DefaultRequestHeaders.Clear( );
                _httpClient.DefaultRequestHeaders.Add( "Authorization", $"Bearer {_gpt.ApiKey}" );
                var _json = JsonConvert.SerializeObject( payload );
                var _content = new StringContent( _json, Encoding.UTF8, "application/json" );
                var _response = await _httpClient.PostAsync( $"{_url}/{endpoint}", _content );
                if( !_response.IsSuccessStatusCode )
                {
                    var _error = await _response.Content.ReadAsStringAsync( );
                    throw new HttpRequestException( $"Error: {_response.StatusCode}, {_error}" );
                }

                return await _response.Content.ReadAsStringAsync( );
            }
            catch( Exception ex )
            {
                Fail( ex );
                return await default( Task<string> );
            }
        }

        /// <summary>
        /// Extracts the content of the response.
        /// </summary>
        /// <param name="jsonResponse">
        /// The json response.
        /// </param>
        /// <returns></returns>
        private protected string ExtractResponseContent( string jsonResponse )
        {
            try
            {
                ThrowIf.Empty( jsonResponse, nameof( jsonResponse ) );
                using var _jsonDocument = JsonDocument.Parse( jsonResponse );
                var _root = _jsonDocument.RootElement;
                if( _model.Contains( "gpt-3.5-turbo" ) )
                {
                    var _choices = _root.GetProperty( "choices" );
                    if( _choices.ValueKind == JsonValueKind.Array
                        && _choices.GetArrayLength( ) > 0 )
                    {
                        var _msg = _choices[ 0 ].GetProperty( "message" );
                        var _cnt = _msg.GetProperty( "content" );
                        var _txt = _cnt.GetString( );
                        return _txt;
                    }

                    var _message = _choices[ 0 ].GetProperty( "message" );
                    var _text = _message.GetString( );
                    return _text;
                }
                else
                {
                    var _choice = _root.GetProperty( "choices" )[ 0 ];
                    var _property = _choice.GetProperty( "text" );
                    var _text = _property.GetString( );
                    return _text;
                }
            }
            catch( Exception ex )
            {
                Fail( ex );
                return string.Empty;
            }
        }

        /// <summary>
        /// Gets the data.
        /// </summary>
        /// <returns></returns>
        public virtual IDictionary<string, object> GetData( )
        {
            try
            {
                _data.Add( "model", _model );
                _data.Add( "n", _number );
                _data.Add( "max_completion_tokens", _maximumTokens );
                _data.Add( "store", _store );
                _data.Add( "stream", _stream );
                _data.Add( "temperature", Temperature );
                _data.Add( "frequency_penalty", _frequencyPenalty );
                _data.Add( "presence_penalty", _presencePenalty );
                _data.Add( "top_p", TopPercent );
                _data.Add( "response_format", _responseFormat );
                _stop.Add( "#" );
                _stop.Add( ";" );
                _data.Add( "stop", _stop );
                _data.Add( "modalities", _modalities );
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
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="disposing">
        /// <c>true</c>
        /// to release both managed
        /// and unmanaged resources;
        /// <c>false</c> to release only unmanaged resources.
        /// </param>
        public virtual void Dispose( bool disposing )
        {
            if( disposing )
            {
                _httpClient?.Dispose( );
            }
        }

        /// <inheritdoc />
        /// <summary>
        /// Performs application-defined tasks
        /// associated with freeing, releasing,
        /// or resetting unmanaged resources.
        /// </summary>
        public void Dispose( )
        {
            Dispose( true );
            GC.SuppressFinalize( this );
        }
    }
}