// ******************************************************************************************
//     Assembly:                Bubba
//     Author:                  Terry D. Eppler
//     Created:                 01-10-2025
// 
//     Last Modified By:        Terry D. Eppler
//     Last Modified On:        01-10-2025
// ******************************************************************************************
// <copyright file="CompletionRequest.cs" company="Terry D. Eppler">
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
//   CompletionRequest.cs
// </summary>
// ******************************************************************************************

namespace Bubba
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Text;
    using System.Text.Json;
    using System.Text.Json.Serialization;
    using System.Threading.Tasks;
    using Properties;

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    [ SuppressMessage( "ReSharper", "UnusedType.Global" ) ]
    [ SuppressMessage( "ReSharper", "MemberCanBePrivate.Global" ) ]
    [ SuppressMessage( "ReSharper", "PreferConcreteValueOverDefault" ) ]
    [ SuppressMessage( "ReSharper", "ClassCanBeSealed.Global" ) ]
    [ SuppressMessage( "ReSharper", "PossibleUnintendedReferenceComparison" ) ]
    public class AssistantRequest : TextGenerationRequest
    {
        /// <summary>
        /// Developer-defined tags and values used for filtering completions
        /// </summary>
        private protected IDictionary<string, object> _metaData;

        /// <summary>
        /// The reasoning effort
        /// </summary>
        private protected string _reasoningEffort;

        /// <summary>
        /// The user prompt
        /// </summary>
        private protected string _userPrompt;

        /// <summary>
        /// The system prompt
        /// </summary>
        private protected string _systemPrompt;

        /// <inheritdoc />
        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="T:Bubba.CompletionRequest" /> class.
        /// </summary>
        public AssistantRequest( )
            : base( )
        {
            _entry = new object( );
            _header = new GptHeader( );
            _httpClient = new HttpClient( );
            _endPoint = GptEndPoint.Completions;
            _model = "gpt-4o-mini";
            _stop = new List<string>( );
            _seed = 1;
            _modalities = "['text', 'audio']";
        }

        /// <inheritdoc />
        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="T:Bubba.CompletionRequest" /> class.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="system">The system.</param>
        /// <param name = "config" > </param>
        public AssistantRequest( string user, string system, GptParameter config )
            : base( )
        {
            _header = new GptHeader( );
            _entry = new object( );
            _httpClient = new HttpClient( );
            _endPoint = GptEndPoint.Completions;
            _model = config.Model;
            _store = config.Store;
            _stream = config.Stream;
            _number = config.Number;
            _presencePenalty = config.PresencePenalty;
            _frequencyPenalty = config.FrequencyPenalty;
            _temperature = config.Temperature;
            _topPercent = config.TopPercent;
            _maximumTokens = config.MaximumTokens;
            _stop = config.Stop;
            _modalities = config.Modalities;
        }

        /// <inheritdoc />
        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="T:Bubba.CompletionRequest" /> class.
        /// </summary>
        /// <param name="request">The request.</param>
        public AssistantRequest( AssistantRequest request )
            : base( )
        {
            _header = new GptHeader( );
            _entry = new object( );
            _httpClient = new HttpClient( );
            _endPoint = request.EndPoint;
            _model = request.Model;
            _store = request.Store;
            _stream = request.Stream;
            _number = request.Number;
            _presencePenalty = request.PresencePenalty;
            _frequencyPenalty = request.FrequencyPenalty;
            _temperature = request.Temperature;
            _topPercent = request.TopPercent;
            _maximumTokens = request.MaximumTokens;
            _stop = request.Stop;
            _modalities = request.Modalities;
        }

        /// <summary>
        /// Decontructs the specified header.
        /// </summary>
        /// <param name="header">The header.</param>
        /// <param name="endPoint">The end point.</param>
        /// <param name="store">if set to <c>true</c> [store].</param>
        /// <param name="stream">if set to <c>true</c> [stream].</param>
        /// <param name="model">The model.</param>
        /// <param name="number">The number.</param>
        /// <param name="presence">The presence.</param>
        /// <param name="frequency">The frequency.</param>
        /// <param name="temperature">The temperature.</param>
        /// <param name="topPercent">The top percent.</param>
        /// <param name="tokens">The tokens.</param>
        public void Decontruct( out GptHeader header, out string endPoint, out bool store,
            out bool stream, out string model, out int number,
            out double presence, out double frequency, out double temperature,
            out double topPercent, out int tokens )
        {
            header = _header;
            endPoint = _endPoint;
            store = _store;
            stream = _stream;
            model = _model;
            number = _number;
            presence = _presencePenalty;
            frequency = _frequencyPenalty;
            temperature = _temperature;
            topPercent = _topPercent;
            tokens = _maximumTokens;
        }

        /// <inheritdoc />
        /// <summary>
        /// Gets the end point.
        /// </summary>
        /// <value>
        /// The end point.
        /// </value>
        [ JsonPropertyName( "endpoint" ) ]
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
        /// Gets or sets the modalities.
        /// </summary>
        /// <value>
        /// The modalities.
        /// </value>
        [ JsonPropertyName( "modalities" ) ]
        public override string Modalities
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
        /// Gets or sets the reasoning effort.
        /// </summary>
        /// <value>
        /// The reasoning effort.
        /// </value>
        [ JsonPropertyName( "reasoning_effort" ) ]
        public string ReasoningEffort
        {
            get
            {
                return _reasoningEffort;
            }
            set
            {
                if( _reasoningEffort != value )
                {
                    _reasoningEffort = value;
                    OnPropertyChanged( nameof( ReasoningEffort ) );
                }
            }
        }

        /// <summary>
        /// Gets or sets the meta data.
        /// </summary>
        /// <value>
        /// The meta data.
        /// </value>
        [ JsonPropertyName( "meta_data" ) ]
        public IDictionary<string, object> MetaData
        {
            get
            {
                return _metaData;
            }
            set
            {
                if( _metaData != value )
                {
                    _metaData = value;
                    OnPropertyChanged( nameof( MetaData ) );
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
        public override GptBody Body
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
        /// Gets the chat response asynchronous.
        /// </summary>
        /// <returns></returns>
        public async Task<string> GetResponseAsync( )
        {
            try
            {
                using var _client = new HttpClient( );
                _client.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue( "Bearer", _apiKey );

                var _payload = new AssistantPayload( )
                {
                    Model = _model,
                    Temperature = _temperature,
                    Store = _store,
                    Stream = _stream,
                    Stop = _stop,
                    TopPercent = _topPercent,
                    FrequencyPenalty = _frequencyPenalty,
                    PresencePenalty = _presencePenalty
                };

                var _json = _payload.Serialize( );
                var _content = new StringContent( _json, Encoding.UTF8, _header.ContentType );
                var _response = await _client.PostAsync( _endPoint, _content );
                _response.EnsureSuccessStatusCode( );
                var _chat = await _response.Content.ReadAsStringAsync( );
                var _chatResponse = ExtractResponse( _chat );
                return !string.IsNullOrEmpty( _chatResponse )
                    ? _chatResponse
                    : string.Empty;
            }
            catch( Exception ex )
            {
                Fail( ex );
                return string.Empty;
            }
        }

        /// <summary>
        /// Extracts the message from response.
        /// </summary>
        /// <param name="jsonResponse">The json response.</param>
        /// <returns></returns>
        private protected string ExtractResponse( string jsonResponse )
        {
            try
            {
                using var _document = JsonDocument.Parse( jsonResponse );
                var _root = _document.RootElement;
                var _choices = _root.GetProperty( "choices" );
                if( _choices.ValueKind == JsonValueKind.Array
                    && _choices.GetArrayLength( ) > 0 )
                {
                    var _message = _choices[ 0 ].GetProperty( "message" );
                    var _content = _message.GetProperty( "content" );
                    return _content.GetString( );
                }

                return string.Empty;
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
                _data.Add( "n", _number );
                _data.Add( "max_completion_tokens", _maximumTokens );
                _data.Add( "store", _store );
                _data.Add( "stream", _stream );
                _data.Add( "temperature", _temperature );
                _data.Add( "frequency_penalty", _frequencyPenalty );
                _data.Add( "presence_penalty", _presencePenalty );
                _data.Add( "top_p", _topPercent );
                _data.Add( "response_format", _responseFormat );
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

        /// <inheritdoc />
        /// <summary>
        /// Converts to string.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.String" /> that represents this instance.
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