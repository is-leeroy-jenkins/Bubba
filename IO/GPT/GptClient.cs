﻿// ******************************************************************************************
//     Assembly:                Bubba
//     Author:                  Terry D. Eppler
//     Created:                 11-19-2024
// 
//     Last Modified By:        Terry D. Eppler
//     Last Modified On:        11-19-2024
// ******************************************************************************************
// <copyright file="GptClient.cs" company="Terry D. Eppler">
//    Bubba is a small windows (wpf) application for interacting with
//    Chat GPT that's developed in C-Sharp under the MIT license
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
    using System.Threading.Tasks;
    using Newtonsoft.Json;
    using Exception = System.Exception;
    using JsonSerializer = System.Text.Json.JsonSerializer;

    [ SuppressMessage( "ReSharper", "UnusedType.Global" ) ]
    [ SuppressMessage( "ReSharper", "MemberCanBePrivate.Global" ) ]
    public class GptClient : GptBase, IGptClient
    {
        private const string KEY = "sk-proj-qW9o_PoT2CleBXOErbGxe2UlOeHtgJ9K-"
            + "rVFooUImScUvXn44e4R9ivYZtbYh5OIObWepnxCGET3BlbkFJykj4Dt9MDZT2GQg"
            + "NarXOifdSxGwmodYtevUniudDGt8vkUNmxurKO9DkULeAUVz3rdY9g_-OsA";

        /// <inheritdoc />
        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="T:Bubba.GptClient" /> class.
        /// </summary>
        public GptClient( )
            : base( )
        {
            _entry = new object( );
            _apiKey = KEY;
            _presence = 0.0;
            _frequency = 0.0;
            _temperature = 0.5;
            _maximumTokens = 2048;
            _model = "gpt-3.5-turbo";
            _endPoint = "https://api.openai.com/v1/chat/completions";
            _endPoints = GetEndPoints( );
            _models = GetModels( );
        }

        /// <inheritdoc />
        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="T:Bubba.GptClient" /> class.
        /// </summary>
        /// <param name="temperature">The temperature.</param>
        /// <param name="tokens">The tokens.</param>
        /// <param name="model">The chat model.</param>
        public GptClient( string model, double temperature = 0.5, int tokens = 2048 )
            : this( )
        {
            _model = model;
            _temperature = temperature;
            _maximumTokens = tokens;
            _endPoint = "https://api.openai.com/v1/chat/completions";
        }

        /// <inheritdoc />
        /// <summary>
        /// Gets the user identifier.
        /// </summary>
        /// <value>
        /// The user identifier.
        /// </value>
        public int Id
        {
            get
            {
                return _id;
            }
            private set
            {
                if( _id != value )
                {
                    _id = value;
                    OnPropertyChanged( nameof( Id ) );
                }
            }
        }

        /// <inheritdoc />
        /// <summary>
        /// Gets the frequency.
        /// </summary>
        /// <value>
        /// The frequency.
        /// </value>
        public double Frequency
        {
            get
            {
                return _frequency;
            }
            private set
            {
                if( _frequency != value )
                {
                    _frequency = value;
                    OnPropertyChanged( nameof( Frequency ) );
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
        public double Presence
        {
            get
            {
                return _presence;
            }
            private set
            {
                if( _presence != value )
                {
                    _presence = value;
                    OnPropertyChanged( nameof( Presence ) );
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
        public int MaximumTokens
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
                    OnPropertyChanged( nameof( MaximumTokens ) );
                }
            }
        }

        /// <summary>
        /// Gets the end point.
        /// </summary>
        /// <value>
        /// The end point.
        /// </value>
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

        /// <inheritdoc />
        /// <summary>
        /// Gets the prompt.
        /// </summary>
        /// <value>
        /// The prompt.
        /// </value>
        public string Prompt
        {
            get
            {
                return _prompt;
            }
            private set
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
        public async Task<string> GetResponseAsync( List<dynamic> messages, string model = "gpt-4",
            int maxTokens = 150, double temperature = 0.7 )
        {
            var _payload = new
            {
                model,
                messages,
                max_tokens = maxTokens,
                temperature
            };

            return await SendRequestAsync( $"{_endPoint}/chat/completions", _payload );
        }

        /// <inheritdoc />
        /// <summary>
        /// Handles POST requests and response parsing.
        /// </summary>
        public async Task<string> SendRequestAsync( string url, object payload )
        {
            var _serial = JsonConvert.SerializeObject( payload );
            var _content = new StringContent( _serial, Encoding.UTF8, "application/json" );
            _httpClient.DefaultRequestHeaders.Clear( );
            _httpClient.DefaultRequestHeaders.Add( "Authorization", $"Bearer {_apiKey}" );
            var _response = await _httpClient.PostAsync( url, _content );
            if( !_response.IsSuccessStatusCode )
            {
                var _message =
                    $"Error: {_response.StatusCode}, {await _response.Content.ReadAsStringAsync( )}";

                throw new Exception( _message );
            }

            var _json = await _response.Content.ReadAsStringAsync( );
            dynamic _result = JsonConvert.DeserializeObject( _json );
            if( url.Contains( "/completions" ) )
            {
                return _result?.choices[ 0 ]?.text ?? "No response.";
            }
            else if( url.Contains( "/chat/completions" ) )
            {
                return _result?.choices[ 0 ]?.message?.content ?? "No response.";
            }

            return "Unexpected response format.";
        }

        /// <inheritdoc />
        /// <summary>
        /// Sends the HTTP message.
        /// </summary>
        /// <param name="prompt">The question.</param>
        /// <returns></returns>
        public string WebGenerate( string prompt )
        {
            try
            {
                ThrowIf.Null( prompt, nameof( prompt ) );
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12
                    | SecurityProtocolType.Tls11;

                var _url = _model.Contains( "gpt-3.5-turbo" )
                    ? "https://api.openai.com/v1/chat/completions"
                    : "https://api.openai.com/v1/completions";

                // Validate randomness (temperature)
                var _payload = CreatePayload( prompt );
                var _request = WebRequest.Create( _url );
                _request.Method = "POST";
                _request.ContentType = "application/json";
                _request.Headers.Add( "Authorization", $"Bearer {App.KEY}" );
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
                return ExtractMessageFromResponse( _jsonResponse );
            }
            catch( Exception ex )
            {
                Fail( ex );
                return string.Empty;
            }
        }

        // Helper method to build the request payload
        /// <inheritdoc />
        /// <summary>
        /// Builds the request data.
        /// </summary>
        /// <param name="prompt">The question.</param>
        /// <returns></returns>
        public string CreatePayload( string prompt )
        {
            if( _model.Contains( "gpt-3.5-turbo" ) )
            {
                return JsonSerializer.Serialize( new
                {
                    model = _model,
                    messages = new[ ]
                    {
                        new
                        {
                            role = "user",
                            content = prompt
                        }
                    }
                } );
            }
            else
            {
                return JsonSerializer.Serialize( new
                {
                    model = _model,
                    prompt,
                    max_tokens = _maximumTokens,
                    user = _id,
                    _temperature,
                    frequency_penalty = 0.0,
                    presence_penalty = 0.0,
                    stop = new[ ]
                    {
                        "#",
                        ";"
                    }
                } );
            }
        }

        /// <summary>
        /// Extracts the message from response.
        /// Helper method to extract the message from the JSON response
        /// </summary>
        /// <param name="response">The json response.</param>
        /// <returns></returns>
        private string ExtractMessageFromResponse( string response )
        {
            try
            {
                ThrowIf.Empty( response, nameof( response ) );
                using var _document = JsonDocument.Parse( response );
                var _root = _document.RootElement;
                if( _model.Contains( "gpt-3.5-turbo" ) )
                {
                    var _choices = _root.GetProperty( "choices" );
                    if( _choices.ValueKind == JsonValueKind.Array
                        && _choices.GetArrayLength( ) > 0 )
                    {
                        var _element = _choices[ 0 ].GetProperty( "message" );
                        return _element.GetProperty( "content" ).GetString( );
                    }

                    return _choices[ 0 ].GetProperty( "message" ).GetString( );
                }
                else
                {
                    return _root.GetProperty( "choices" )[ 0 ].GetProperty( "text" ).GetString( );
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
        /// <param name="prompt">The prompt.</param>
        /// <returns></returns>
        public async Task<string> SendHttpMessageAsync( string prompt )
        {
            var _temp = _temperature;
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
                            role = "user",
                            content = PadQuotes( prompt )
                        }
                    }
                } );
            }
            else
            {
                _payload = JsonSerializer.Serialize( new
                {
                    model = _model,
                    prompt = PadQuotes( prompt ),
                    max_tokens = _maximumTokens,
                    temperature = _temp,
                    user = _id,
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
                    new AuthenticationHeaderValue( "Bearer", App.KEY );

                var _content = new StringContent( _payload, Encoding.UTF8, "application/json" );
                var _response = await _client.PostAsync( _url, _content );
                _response.EnsureSuccessStatusCode( );
                var _responseText = await _response.Content.ReadAsStringAsync( );
                if( _model.Contains( "gpt-3.5-turbo" ) )
                {
                    using var _doc = JsonDocument.Parse( _responseText );
                    var _root = _doc.RootElement;
                    var _choice = _root.GetProperty( "choices" )[ 0 ];
                    var _message = _choice.GetProperty( "message" )
                        .GetProperty( "content" )
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