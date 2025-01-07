// ******************************************************************************************
//     Assembly:                Bubba
//     Author:                  Terry D. Eppler
//     Created:                 01-07-2025
// 
//     Last Modified By:        Terry D. Eppler
//     Last Modified On:        01-07-2025
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
    using Exception = System.Exception;
    using JsonSerializer = System.Text.Json.JsonSerializer;

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    /// <seealso cref="T:OpenAI.Chat.ChatCompletionOptions" />
    /// <seealso cref="T:System.ComponentModel.INotifyPropertyChanged" />
    [ SuppressMessage( "ReSharper", "UnusedType.Global" ) ]
    [ SuppressMessage( "ReSharper", "MemberCanBePrivate.Global" ) ]
    [ SuppressMessage( "ReSharper", "FieldCanBeMadeReadOnly.Global" ) ]
    [ SuppressMessage( "ReSharper", "RedundantExtendsListEntry" ) ]
    public class Payload : PayloadBase, IPayload
    {
        /// <inheritdoc />
        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="T:Bubba.Payload" /> class.
        /// </summary>
        public Payload( )
        {
            _id = 1;
            _temperature = 0.7;
            _maximumTokens = 2048;
            _frequency = 0.0;
            _presence = 0.0;
            _store = false;
            _stream = false;
            _stopSequences = new List<string>( );
            _stopSequences.Add( "#" );
            _stopSequences.Add( ";" );
            _data = new Dictionary<string, object>( );
            _data.Add( "stop", _stopSequences );
        }

        /// <inheritdoc />
        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="T:Bubba.Payload" /> class.
        /// </summary>
        /// <param name="systemPrompt">The system prompt.</param>
        /// <param name="userPrompt">The user prompt.</param>
        /// <param name="model">The model.</param>
        /// <param name="id">The identifier.</param>
        /// <param name="frequency">The frequency.</param>
        /// <param name="presence">The presence.</param>
        /// <param name="temperature">The temperature.</param>
        /// <param name="maxTokens">The maximum tokens.</param>
        /// <param name="store">if set to <c>true</c> [store].</param>
        /// <param name="stream">if set to <c>true</c> [stream].</param>
        public Payload( string systemPrompt, string userPrompt, string model,
            int id = 1, double frequency = 0.0, double presence = 0.0,
            double temperature = 0.7, int maxTokens = 2048, bool store = false,
            bool stream = false )
            : this( )
        {
            _id = id;
            _model = "gpt-4o";
            _temperature = temperature;
            _maximumTokens = maxTokens;
            _frequency = frequency;
            _presence = presence;
            _systemPrompt = systemPrompt;
            _store = store;
            _stream = stream;
            _data.Add( "id", id );
            _data.Add( "max_completion_tokens", maxTokens );
            _data.Add( "frequency", frequency );
            _data.Add( "presence", presence );
            _data.Add( "content", systemPrompt );
        }

        /// <inheritdoc />
        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="T:Bubba.Payload" /> class.
        /// </summary>
        /// <param name="payload">The payload.</param>
        public Payload( Payload payload )
        {
            _id = payload.Id;
            _temperature = payload.Temperature;
            _maximumTokens = payload.MaximumTokens;
            _frequency = payload.Frequency;
            _presence = payload.Presence;
            _systemPrompt = payload.Prompt;
            _model = payload.Model;
            _store = payload.Store;
            _stream = payload.Stream;
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
        public void Deconstruct( out string prompt, out int userId, out string model,
            out double frequency, out double presence, out double temperature,
            out int maximumTokens, out bool store, out bool stream )
        {
            prompt = _systemPrompt;
            userId = _id;
            model = _model;
            temperature = _temperature;
            frequency = _frequency;
            presence = _presence;
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
        public Payload ForTextGeneration( )
        {
            return new Payload
            {
                Model = _model,
                Prompt = _systemPrompt,
                MaximumTokens = _maximumTokens,
                Temperature = _temperature,
                ResponseFormat = "text"
            };
        }

        // A method to easily set up image generation specifics
        /// <summary>
        /// Fors the image generation.
        /// </summary>
        /// <returns>
        /// </returns>
        public Payload ForImageGeneration( )
        {
            return new Payload
            {
                Prompt = _systemPrompt,
                ImageSize = _imageSize,
                Number = _number,
                ResponseFormat = _responseFormat
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
                        _prompt = _systemPrompt,
                        max_completion_tokens = _maximumTokens,
                        user = _id,
                        _temperature,
                        frequency_penalty = _frequency,
                        presence_penalty = _presence,
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
                        frequency_penalty = _frequency,
                        presence_penalty = _presence,
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