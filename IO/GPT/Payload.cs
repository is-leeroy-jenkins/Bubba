// ******************************************************************************************
//     Assembly:                Bubba
//     Author:                  Terry D. Eppler
//     Created:                 12-13-2024
// 
//     Last Modified By:        Terry D. Eppler
//     Last Modified On:        12-13-2024
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
    using JsonSerializer = System.Text.Json.JsonSerializer;

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    /// <seealso cref="T:OpenAI.Chat.ChatCompletionOptions" />
    /// <seealso cref="T:System.ComponentModel.INotifyPropertyChanged" />
    [ SuppressMessage( "ReSharper", "UnusedType.Global" ) ]
    [ SuppressMessage( "ReSharper", "MemberCanBePrivate.Global" ) ]
    [ SuppressMessage( "ReSharper", "FieldCanBeMadeReadOnly.Global" ) ]
    public class Payload : PayloadBase
    {
        /// <inheritdoc />
        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="T:Bubba.Payload" /> class.
        /// </summary>
        public Payload( )
        {
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
        /// <param name="prompt"> The ChatGPT prompt</param>
        /// <param name="id">The identifier.</param>
        /// <param name="frequency">The frequency.</param>
        /// <param name="presence">The presence.</param>
        /// <param name="temperature">The temperature.</param>
        /// <param name="maxTokens">The tokens.</param>
        public Payload( string prompt, int id = 1, double frequency = 0.0,
            double presence = 0.0, double temperature = 0.7, int maxTokens = 2048 ) 
            : this( )
        {
            _id = id;
            _temperature = temperature;
            _maxCompletionTokens = maxTokens;
            _frequency = frequency;
            _presence = presence;
            _prompt = prompt;
            _data.Add( "id", id );
            _data.Add( "max_completion_tokens", maxTokens );
            _data.Add( "frequency", frequency );
            _data.Add( "presence", presence );
            _data.Add( "content", prompt );
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
            _maxCompletionTokens = payload.MaxCompletionTokens;
            _frequency = payload.Frequency;
            _presence = payload.Presence;
            _prompt = payload.Prompt;
        }

        /// <summary>
        /// Deconstructs the specified user identifier.
        /// </summary>
        /// <param name = "prompt" > </param>
        /// <param name="userId">The user identifier.</param>
        /// <param name="frequency">The frequency.</param>
        /// <param name="presence">The presence.</param>
        /// <param name="temperature">The temperature.</param>
        /// <param name="maximumTokens">The maximum tokens.</param>
        public void Deconstruct( out string prompt, out int userId, out double frequency,
            out double presence, out double temperature, out int maximumTokens )
        {
            userId = _id;
            temperature = _temperature;
            frequency = _frequency;
            presence = _presence;
            maximumTokens = _maxCompletionTokens;
            prompt = _prompt;
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
                Prompt = _prompt,
                MaxCompletionTokens = _maxCompletionTokens,
                Temperature = _temperature,
                ResponseFormat = "url" 
            };
        }

        // A method to easily set up image generation specifics
        /// <summary>
        /// Fors the image generation.
        /// </summary>
        /// <param name="prompt">The prompt.</param>
        /// <param name="size">The size.</param>
        /// <param name="numberOfImages">The number of images.</param>
        /// <returns></returns>
        public static Payload ForImageGeneration( string prompt, string size = "512x512", int numberOfImages = 1 )
        {
            return new Payload
            {
                Prompt = prompt,
                ImageSize = size,
                Number = numberOfImages,
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
                                content = _prompt
                            }
                        }
                    } );
                }
                else
                {
                    return JsonSerializer.Serialize( new
                    {
                        model = _model,
                        _prompt,
                        max_completion_tokens = _maxCompletionTokens,
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
                                content = _prompt
                            }
                        }
                    } );
                }
                else
                {
                    return JsonSerializer.Serialize( new
                    {
                        model = _model,
                        _prompt,
                        max_completion_tokens = _maxCompletionTokens,
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