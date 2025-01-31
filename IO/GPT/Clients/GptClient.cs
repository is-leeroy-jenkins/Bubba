// ******************************************************************************************
//     Assembly:                Bubba
//     Author:                  Terry D. Eppler
//     Created:                 01-22-2025
// 
//     Last Modified By:        Terry D. Eppler
//     Last Modified On:        01-22-2025
// ******************************************************************************************
// <copyright file="GptClient.cs" company="Terry D. Eppler">
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
//   GptClient.cs
// </summary>
// ******************************************************************************************

namespace Bubba
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.IO;
    using System.Net;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Text;
    using System.Text.Json;
    using System.Text.Json.Serialization;
    using System.Threading.Tasks;
    using Newtonsoft.Json;
    using Exception = System.Exception;
    using Properties;
    using JsonSerializer = System.Text.Json.JsonSerializer;

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    /// <seealso cref="T:Bubba.GptBase" />
    /// <seealso cref="T:Bubba.IGptClient" />
    [ SuppressMessage( "ReSharper", "UnusedType.Global" ) ]
    [ SuppressMessage( "ReSharper", "MemberCanBePrivate.Global" ) ]
    [ SuppressMessage( "ReSharper", "InternalOrPrivateMemberNotDocumented" ) ]
    [ SuppressMessage( "ReSharper", "PossibleUnintendedReferenceComparison" ) ]
    public class GptClient : GptBase, IGptClient
    {
        /// <summary>
        /// The header
        /// </summary>
        private protected GptHeader _header;

        /// <summary>
        /// The HTTP client
        /// </summary>
        private protected HttpClient _httpClient;

        /// <summary>
        /// The messages
        /// </summary>
        private protected IList<IGptMessage> _messages;

        /// <inheritdoc />
        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="T:Bubba.GptClient" /> class.
        /// </summary>
        public GptClient( )
            : base( )
        {
            _entry = new object( );
            _apiKey = App.OpenAiKey;
            _header = new GptHeader( );
        }

        /// <inheritdoc />
        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="T:Bubba.GptClient" /> class.
        /// </summary>
        public GptClient( GptParameter config )
            : this( )
        {
            _model = config.Model;
            _temperature = config.Temperature;
            _maximumTokens = config.MaximumTokens;
        }

        /// <summary>
        /// Gets or sets a value indicating whether this
        /// <see cref="Store"/> is store.
        /// </summary>
        /// <value>
        ///   <c>true</c> if store; otherwise, <c>false</c>.
        /// </value>
        [ JsonPropertyName( "store" ) ]
        public bool Store
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

        /// <summary>
        /// Gets or sets a value indicating whether this
        /// <see cref="Stream"/> is stream.
        /// </summary>
        /// <value>
        ///   <c>true</c> if stream; otherwise, <c>false</c>.
        /// </value>
        [ JsonPropertyName( "stream" ) ]
        public bool Stream
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
        /// Number between -2.0 and 2.0. Positive values penalize new tokens
        /// based on their existing frequency in the text so far,
        /// decreasing the model's likelihood to repeat the same line verbatim.
        /// </summary>
        /// <value>
        /// The frequency.
        /// </value>
        [ JsonPropertyName( "frequency_penalty" ) ]
        public double FrequencyPenalty
        {
            get
            {
                return _frequencyPenalty;
            }
            private set
            {
                if( _frequencyPenalty != value )
                {
                    _frequencyPenalty = value;
                    OnPropertyChanged( nameof( FrequencyPenalty ) );
                }
            }
        }

        /// <inheritdoc />
        /// <summary>
        /// Gets the temperature.
        /// </summary>
        /// <value>
        /// The temperature.
        /// </value>
        [ JsonPropertyName( "temperature" ) ]
        public double Temperature
        {
            get
            {
                return _temperature;
            }
            private set
            {
                if( _temperature != value )
                {
                    _temperature = value;
                    OnPropertyChanged( nameof( Temperature ) );
                }
            }
        }

        /// <inheritdoc />
        /// <summary>
        /// Gets the presence.
        /// </summary>
        /// <value>
        /// The presence.
        /// </value>
        [ JsonPropertyName( "presence_penalty" ) ]
        public double PresencePenalty
        {
            get
            {
                return _presencePenalty;
            }
            private set
            {
                if( _presencePenalty != value )
                {
                    _presencePenalty = value;
                    OnPropertyChanged( nameof( PresencePenalty ) );
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
        [ JsonPropertyName( "max_completion_tokens" ) ]
        public int MaxTokens
        {
            get
            {
                return _maximumTokens;
            }
            private set
            {
                if( _maximumTokens != value )
                {
                    _maximumTokens = value;
                    OnPropertyChanged( nameof( MaxTokens ) );
                }
            }
        }

        /// <summary>
        /// An alternative to sampling with temperature,
        /// called nucleus sampling, where the model considers
        /// the results of the tokens with top_p probability mass.
        /// So 0.1 means only the tokens comprising the top 10% probability
        /// mass are considered. We generally recommend altering this
        /// or temperature but not both.
        /// </summary>
        /// <value>
        /// The top percent.
        /// </value>
        [ JsonPropertyName( "top_p" ) ]
        public double TopPercent
        {
            get
            {
                return _topPercent;
            }
            set
            {
                if( _topPercent != value )
                {
                    _topPercent = value;
                    OnPropertyChanged( nameof( TopPercent ) );
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
        [ JsonPropertyName( "endpoint" ) ]
        public string EndPoint
        {
            get
            {
                return _endPoint;
            }
            private set
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
        /// Gets the chat model.
        /// </summary>
        /// <value>
        /// The chat model.
        /// </value>
        [ JsonPropertyName( "model" ) ]
        public string Model
        {
            get
            {
                return _model;
            }
            private set
            {
                if( _model != value )
                {
                    _model = value;
                    OnPropertyChanged( nameof( Model ) );
                }
            }
        }

        /// <summary>
        /// Gets the messages.
        /// </summary>
        /// <value>
        /// The messages.
        /// </value>
        [ JsonPropertyName( "messages" ) ]
        public IList<IGptMessage> Messages
        {
            get
            {
                return _messages;
            }
            private set
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
        /// Gets the prompt.
        /// </summary>
        /// <value>
        /// The prompt.
        /// </value>
        [ JsonPropertyName( "prompt" ) ]
        public override string Prompt
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
        /// Sends a request to the Chat (Assistant) API.
        /// </summary>
        public async Task<string> GetResponseAsync( string prompt )
        {
            try
            {
                var _systemPrompt = OpenAI.BubbaPrompt;
                var _payload = new
                {
                    model = "gpt-4",
                    messages = new[ ]
                    {
                        new
                        {
                            role = "system",
                            content = _systemPrompt
                        },
                        new
                        {
                            role = "user",
                            content = prompt
                        }
                    },
                    max_completion_tokens = 2048
                };

                _httpClient = new HttpClient( );
                _httpClient.DefaultRequestHeaders.Add( "Authorization", $"Bearer {_apiKey}" );
                var _serial = JsonSerializer.Serialize( _payload );
                var _content = new StringContent( _serial, Encoding.UTF8, "application/json" );
                var _async = await _httpClient.PostAsync( _endPoint, _content );
                _async.EnsureSuccessStatusCode( );
                var _response = await _async.Content.ReadAsStringAsync( );
                var _choices = JsonSerializer.Deserialize<JsonElement>( _response );
                var _message = _choices.GetProperty( "choices" )[ 0 ]
                    .GetProperty( "message" )
                    .GetProperty( "content" )
                    .GetString( );

                return !string.IsNullOrEmpty( _message )
                    ? _message
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
        /// Handles POST requests and response parsing.
        /// </summary>
        public async Task<string> SendRequestAsync( GptPayload payload )
        {
            try
            {
                var _serial = payload.Serialize( );
                var _content = new StringContent( _serial, Encoding.UTF8, "application/json" );
                _httpClient.DefaultRequestHeaders.Clear( );
                _httpClient.DefaultRequestHeaders.Add( "Authorization", $"Bearer {_apiKey}" );
                var _response = await _httpClient.PostAsync( _endPoint, _content );
                if( !_response.IsSuccessStatusCode )
                {
                    var _message =
                        $"Error: {_response.StatusCode}, {await _response.Content.ReadAsStringAsync( )}";

                    throw new Exception( _message );
                }

                var _json = await _response.Content.ReadAsStringAsync( );
                dynamic _result = JsonConvert.DeserializeObject( _json );
                if( _endPoint.Contains( "/completions" ) )
                {
                    return _result?.choices[ 0 ]?.text ?? "No response.";
                }
                else if( _endPoint.Contains( "/chat/completions" ) )
                {
                    return _result?.choices[ 0 ]?.message?.content ?? "No response.";
                }

                return "Unexpected response format.";
            }
            catch( Exception ex )
            {
                Fail( ex );
                return string.Empty;
            }
        }

        /// <inheritdoc />
        /// <summary>
        /// Sends the HTTP message.
        /// </summary>
        /// <param name="prompt">The question.</param>
        /// <returns></returns>
        public string SendRequest( string prompt )
        {
            try
            {
                ThrowIf.Null( prompt, nameof( prompt ) );
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12
                    | SecurityProtocolType.Tls11;

                var _url = _model.Contains( "gpt-3.5-turbo" )
                    ? "https://api.openai.com/v1/chat/completions"
                    : "https://api.openai.com/v1/completions";

                var _payload = new GptPayload
                {
                    Prompt = prompt
                };

                var _request = WebRequest.Create( _url );
                _request.Method = "POST";
                _request.ContentType = "application/json";
                _request.Headers.Add( "Authorization", $"Bearer {_apiKey}" );
                using var _requestStream = _request.GetRequestStream( );
                using var _writer = new StreamWriter( _requestStream );
                _writer.Write( _payload );
                using var _response = _request.GetResponse( );
                using var _responseStream = _response.GetResponseStream( );
                if( _responseStream == null )
                {
                    return string.Empty;
                }

                using var _reader = new StreamReader( _responseStream );
                var _jsonResponse = _reader.ReadToEnd( );
                return ExtractResponseContent( _jsonResponse );
            }
            catch( Exception ex )
            {
                Fail( ex );
                return string.Empty;
            }
        }

        /// <summary>
        /// Extracts the message from jsonResponse.
        /// Helper method to extract the message from the JSON jsonResponse
        /// </summary>
        /// <param name="jsonResponse">The json jsonResponse.</param>
        /// <returns></returns>
        private string ExtractResponseContent( string jsonResponse )
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

        /// <inheritdoc />
        /// <summary>
        /// Sends the HTTP message asynchronous.
        /// </summary>
        /// <param name="userPrompt">The prompt.</param>
        /// <returns></returns>
        public async Task<string> SendHttpMessageAsync( string userPrompt )
        {
            var _temp = Temperature;
            var _url = _model.Contains( "gpt-3.5-turbo" )
                ? "https://api.openai.com/v1/chat/completions"
                : "https://api.openai.com/v1/completions";

            // Prepare request payload
            string _payload;
            if( _model.Contains( "gpt-3.5-turbo" ) )
            {
                _payload = JsonSerializer.Serialize( new
                {
                    model = _model,
                    messages = new[ ]
                    {
                        new
                        {
                            role = _role,
                            content = ProcessQuotes( userPrompt )
                        }
                    }
                } );
            }
            else
            {
                _payload = JsonSerializer.Serialize( new
                {
                    model = _model,
                    prompt = ProcessQuotes( userPrompt ),
                    maxTokens = _maximumTokens,
                    temperature = _temp,
                    user = _user,
                    frequency_penalty = 0.0,
                    presence_penalty = 0.0,
                    stop = new[ ]
                    {
                        "#",
                        ";"
                    }
                } );
            }

            try
            {
                using var _client = new HttpClient( );
                _client.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue( "Bearer", OpenAI.BubbaKey );

                var _content = new StringContent( _payload, Encoding.UTF8, "application/json" );
                var _response = await _client.PostAsync( _url, _content );
                _response.EnsureSuccessStatusCode( );
                var _responseText = await _response.Content.ReadAsStringAsync( );
                if( _model.Contains( "gpt-3.5-turbo" ) )
                {
                    using var _doc = JsonDocument.Parse( _responseText );
                    var _root = _doc.RootElement;
                    var _choice = _root.GetProperty( "choices" )[ 0 ];
                    var _message = _choice.GetProperty( "message" ).GetProperty( "content" )
                        .GetString( );

                    return _message ?? string.Empty;
                }
                else
                {
                    using var _doc = JsonDocument.Parse( _responseText );
                    var _root = _doc.RootElement;
                    var _choice = _root.GetProperty( "choices" )[ 0 ];
                    var _text = _choice.GetProperty( "text" ).GetString( );
                    return _text ?? string.Empty;
                }
            }
            catch( HttpRequestException ex )
            {
                Fail( ex );
                return string.Empty;
            }
            catch( Exception ex )
            {
                Fail( ex );
                return string.Empty;
            }
        }
    }
}