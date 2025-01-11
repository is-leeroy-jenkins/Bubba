// ******************************************************************************************
//     Assembly:                Bubba
//     Author:                  Terry D. Eppler
//     Created:                 01-11-2025
// 
//     Last Modified By:        Terry D. Eppler
//     Last Modified On:        01-11-2025
// ******************************************************************************************
// <copyright file="Payload.cs" company="Terry D. Eppler">
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
//   Payload.cs
// </summary>
// ******************************************************************************************

namespace Bubba
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using Newtonsoft.Json;
    using Exception = System.Exception;
    using JsonSerializer = System.Text.Json.JsonSerializer;
    using Properties;

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    /// <seealso cref="T:OpenAI.Chat.ChatCompletionOptions" />
    /// <seealso cref="T:System.ComponentModel.INotifyPropertyChanged" />
    [ SuppressMessage( "ReSharper", "UnusedType.Global" ) ]
    [ SuppressMessage( "ReSharper", "MemberCanBePrivate.Global" ) ]
    [ SuppressMessage( "ReSharper", "FieldCanBeMadeReadOnly.Global" ) ]
    [ SuppressMessage( "ReSharper", "RedundantExtendsListEntry" ) ]
    [ SuppressMessage( "ReSharper", "PreferConcreteValueOverDefault" ) ]
    [ SuppressMessage( "ReSharper", "ClassCanBeSealed.Global" ) ]
    public class GptPayload : PayloadBase, IGptPayload
    {
        /// <inheritdoc />
        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="T:Bubba.Payload" /> class.
        /// </summary>
        public GptPayload( )
        {
            _systemPrompt = OpenAI.BubbaPrompt;
            _temperature = 0.18;
            _topPercent = 0.11;
            _maximumTokens = 2048;
            _frequencyPenalty = 0.00;
            _presencePenalty = 0.00;
            _store = false;
            _stream = true;
            _stop = new List<string>( );
            _messages = new List<IGptMessage>( );
            _data = new Dictionary<string, object>( );
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GptPayload"/> class.
        /// </summary>
        /// <param name = "userPrompt" > </param>
        /// <param name="model">The model.</param>
        /// <param name="id">The identifier.</param>
        /// <param name="frequency">The frequency penalty.</param>
        /// <param name="presence">The presence penalty.</param>
        /// <param name="temperature">The temperature.</param>
        /// <param name="topPercent">The top percent.</param>
        /// <param name="maxTokens">The maximum tokens.</param>
        /// <param name="store">if set to <c>true</c> [store].</param>
        /// <param name="stream">if set to <c>true</c> [stream].</param>
        public GptPayload( string userPrompt, string model = "gpt-4o-mini", double frequency = 0.00,
            double presence = 0.00, double temperature = 0.18, double topPercent = 0.11,
            int maxTokens = 2048, bool store = false, bool stream = true )
        {
            _model = model;
            _userPrompt = userPrompt;
            _temperature = temperature;
            _maximumTokens = maxTokens;
            _frequencyPenalty = frequency;
            _presencePenalty = presence;
            _store = store;
            _stream = stream;
            _stop = new List<string>();
            _messages = new List<IGptMessage>();
            _data = new Dictionary<string, object>( );
        }

        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="GptPayload"/> class.
        /// </summary>
        /// <param name="userPrompt">The user prompt.</param>
        /// <param name="config">The configuration.</param>
        public GptPayload( string userPrompt, GptParameter config )
        {
            _model = config.Model;
            _userPrompt = userPrompt;
            _temperature = config.Temperature;
            _maximumTokens = config.MaximumTokens;
            _frequencyPenalty = config.FrequencyPenalty;
            _presencePenalty = config.PresencePenalty;
            _store = config.Store;
            _stream = config.Stream;
            _stop = new List<string>( );
            _data = new Dictionary<string, object>( );
        }

        /// <inheritdoc />
        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="T:Bubba.Payload" /> class.
        /// </summary>
        /// <param name="gptPayload">The payload.</param>
        public GptPayload( GptPayload gptPayload )
        {
            _model = gptPayload.Model;
            _userPrompt = gptPayload.UserPrompt;
            _temperature = gptPayload.Temperature;
            _maximumTokens = gptPayload.MaximumTokens;
            _frequencyPenalty = gptPayload.FrequencyPenalty;
            _presencePenalty = gptPayload.PresencePenalty;
            _systemPrompt = gptPayload.UserPrompt;
            _store = gptPayload.Store;
            _stream = gptPayload.Stream;
            _stop = gptPayload.Stop;
        }

        /// <summary>
        /// Deconstructs the specified prompt.
        /// </summary>
        /// <param name="prompt">The prompt.</param>
        /// <param name="userId">The user identifier.</param>
        /// <param name="model">The model.</param>
        /// <param name="frequency">The frequency.</param>
        /// <param name="presence">The presence.</param>
        /// <param name="temperature">The temperature.</param>
        /// <param name="maximumTokens">The maximum tokens.</param>
        /// <param name="store">if set to <c>true</c> [store].</param>
        /// <param name="stream">if set to <c>true</c> [stream].</param>
        public void Deconstruct( out string prompt, out string userId, out string model,
            out double frequency, out double presence, out double temperature,
            out int maximumTokens, out bool store, out bool stream )
        {
            prompt = _userPrompt;
            userId = _id;
            model = _model;
            temperature = _temperature;
            frequency = _frequencyPenalty;
            presence = _presencePenalty;
            maximumTokens = _maximumTokens;
            store = _store;
            stream = _stream;
        }

        /// <summary>
        /// Gets or sets a value indicating whether this
        /// <see cref="ParameterBase"/> is store.
        /// </summary>
        /// <value>
        ///   <c>true</c> if store; otherwise, <c>false</c>.
        /// </value>
        [ JsonProperty( "store" ) ]
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
        /// <see cref="ParameterBase"/> is stream.
        /// </summary>
        /// <value>
        ///   <c>true</c> if stream; otherwise, <c>false</c>.
        /// </value>
        [ JsonProperty( "stream" ) ]
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

        /// <summary>
        /// Fors the text generation.
        /// A method to construct other reusable default payloads if required
        /// </summary>
        /// <returns>
        /// Payload
        /// </returns>
        public GptPayload ForTextGeneration( )
        {
            return new GptPayload
            {
                Model = _model,
                UserPrompt = _systemPrompt,
                MaximumTokens = _maximumTokens,
                Temperature = _temperature,
                FrequencyPenalty = _frequencyPenalty,
                PresencePenalty = _presencePenalty,
                Store = _store,
                Stream = _stream,
                ResponseFormat = "text"
            };
        }

        // A method to easily set up image generation specifics
        /// <summary>
        /// Fors the image generation.
        /// </summary>
        /// <returns>
        /// </returns>
        public GptPayload ForImageGeneration( )
        {
            return new GptPayload
            {
                UserPrompt = _systemPrompt,
                Size = _size,
                Number = _number,
                ResponseFormat = "url"
            };
        }

        /// <summary>
        /// Serializes the specified prompt.
        /// </summary>
        /// <returns></returns>
        public string Serialize( )
        {
            try
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
                                content = _userPrompt
                            }
                        }
                    } );
                }
                else
                {
                    return JsonSerializer.Serialize( new
                    {
                        Model = _model,
                        UserPrompt = _userPrompt,
                        MaximumTokens = _maximumTokens,
                        Temperature = _temperature,
                        FrequencyPenalty = _frequencyPenalty,
                        PresencePenalty = _presencePenalty,
                        stop = new[ ]
                        {
                            "#",
                            ";"
                        }
                    } );
                }
            }
            catch( Exception ex )
            {
                Fail( ex );
                return string.Empty;
            }
        }

        /// <summary>
        /// Serializes the specified prompt.
        /// </summary>
        /// <param name="prompt">The prompt.</param>
        /// <returns></returns>
        public string Serialize( string prompt )
        {
            try
            {
                ThrowIf.Null( prompt, nameof( prompt ) );
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
                                content = _systemPrompt
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
                        max_completion_tokens = _maximumTokens,
                        user = _id,
                        _temperature,
                        frequency_penalty = _frequencyPenalty,
                        presence_penalty = _presencePenalty,
                        stop = new[ ]
                        {
                            "#",
                            ";"
                        }
                    } );
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
        /// Gets the data.
        /// </summary>
        /// <returns>
        /// </returns>
        public virtual IDictionary<string, object> GetData( )
        {
            try
            {
                _data.Add( "model", _model );
                _data.Add( "number", _number );
                _data.Add( "max_completion_tokens", _maximumTokens );
                _data.Add( "store", _store );
                _data.Add( "stream", _stream );
                _data.Add( "temperature", _temperature );
                _data.Add( "frequency_penalty", _frequencyPenalty );
                _data.Add( "presence_penalty", _presencePenalty );
                _data.Add( "top_p", _topPercent );
                _data.Add( "response_format", _responseFormat );
                _stop.Add( "#" );
                _stop.Add( ";" );
                _data.Add( "stop", _stop );
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
        /// A <see cref="T:System.String" />
        /// that represents this instance.
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

        /// <summary>
        /// Fails the specified ex.
        /// </summary>
        /// <param name="ex">The ex.</param>
        private protected void Fail( Exception ex )
        {
            var _error = new ErrorWindow( ex );
            _error?.SetText( );
            _error?.ShowDialog( );
        }
    }
}