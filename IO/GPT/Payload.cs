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

    [ SuppressMessage( "ReSharper", "UnusedType.Global" ) ]
    [ SuppressMessage( "ReSharper", "MemberCanBePrivate.Global" ) ]
    public class Payload : PropertyChangedBase
    {
        /// <summary>
        /// The user identifier
        /// </summary>
        private protected int _id;

        /// <summary>
        /// The number of images
        /// </summary>
        private protected int _number;

        /// <summary>
        /// ID of the model to use.
        /// </summary>
        private protected string _model;

        /// <summary>
        /// A number between -2.0 and 2.0  Positive value decrease the
        /// model's likelihood to repeat the same line verbatim.
        /// </summary>
        private protected double _temperature;

        /// <summary>
        /// An upper bound for the number of tokens that can be generated for a completion
        /// </summary>
        private protected int _maxCompletionTokens;

        /// <summary>
        /// TNumber between -2.0 and 2.0. Positive values penalize new
        /// tokens based on their existing frequency in the text so far,
        /// decreasing the model's likelihood to repeat the same line verbatim.
        /// </summary>
        private protected double _frequency;

        /// <summary>
        /// The top percent
        /// </summary>
        private protected double _topPercent;

        /// <summary>
        /// The stop sequences
        /// </summary>
        private protected IList<string> _stopSequences;

        /// <summary>
        /// Number between -2.0 and 2.0. Positive values penalize new tokens
        /// based on whether they appear in the text so far,
        /// ncreasing the model's likelihood to talk about new topics.
        /// </summary>
        private protected double _presence;

        /// <summary>
        /// The size
        /// </summary>
        private protected string _size;

        /// <summary>
        /// The string provided to GPT
        /// </summary>
        private protected string _prompt;

        /// <summary>
        /// The response format
        /// </summary>
        private protected string _responseFormat;

        /// <summary>
        /// The messages
        /// </summary>
        private protected List<dynamic> _messages;

        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="Payload"/> class.
        /// </summary>
        public Payload( )
        {
            _stopSequences = new List<string>( );
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Payload"/> class.
        /// </summary>
        /// <param name = "prompt" > The ChatGPT prompt</param>
        /// <param name="id">The identifier.</param>
        /// <param name="frequency">The frequency.</param>
        /// <param name="presence">The presence.</param>
        /// <param name="temperature">The temperature.</param>
        /// <param name="completionTokens">The tokens.</param>
        public Payload( string prompt, int id = 1, double frequency = 0.0,
            double presence = 0.0, double temperature = 0.5, int completionTokens = 2048 )
        {
            _id = id;
            _temperature = temperature;
            _maxCompletionTokens = completionTokens;
            _frequency = frequency;
            _presence = presence;
            _prompt = prompt;
        }

        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="Payload"/> class.
        /// </summary>
        /// <param name="payload">The payload.</param>
        public Payload( Payload payload )
        {
            _id = payload.Id;
            _temperature = payload.Temperature;
            _maxCompletionTokens = payload.MaxCompletionTokens;
            _frequency = payload.Frequency;
            _presence = payload.Presence;
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
            set
            {
                if( _id != value )
                {
                    _id = value;
                    OnPropertyChanged( nameof( Id ) );
                }
            }
        }

        /// <summary>
        /// Gets or sets the stop sequences.
        /// </summary>
        /// <value>
        /// The stop sequences.
        /// </value>
        public IList<string> StopSequences
        {
            get
            {
                return _stopSequences;
            }
            set
            {
                if( _stopSequences != value )
                {
                    _stopSequences = value;
                    OnPropertyChanged( nameof( StopSequences ) );
                }
            }
        }

        /// <summary>
        /// Gets or sets the number of images.
        /// </summary>
        /// <value>
        /// The number of images.
        /// </value>
        public int NumberOfImages
        {
            get
            {
                return _maxCompletionTokens;
            }
            set
            {
                if( _maxCompletionTokens != value )
                {
                    _maxCompletionTokens = value;
                    OnPropertyChanged( nameof( MaxCompletionTokens ) );
                }
            }
        }

        /// <summary>
        /// Gets the maximum tokens.
        /// </summary>
        /// <value>
        /// The maximum tokens.
        /// </value>
        public int MaxCompletionTokens
        {
            get
            {
                return _maxCompletionTokens;
            }
            set
            {
                if( _maxCompletionTokens != value )
                {
                    _maxCompletionTokens = value;
                    OnPropertyChanged( nameof( MaxCompletionTokens ) );
                }
            }
        }

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
            set
            {
                if( _temperature != value )
                {
                    _temperature = value;
                    OnPropertyChanged( nameof( Temperature ) );
                }
            }
        }

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
            set
            {
                if( _frequency != value )
                {
                    _frequency = value;
                    OnPropertyChanged( nameof( Frequency ) );
                }
            }
        }

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
            set
            {
                if( _presence != value )
                {
                    _presence = value;
                    OnPropertyChanged( nameof( Presence ) );
                }
            }
        }

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
            set
            {
                if( _model != value )
                {
                    _model = value;
                    OnPropertyChanged( nameof( Model ) );
                }
            }
        }

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
            set
            {
                if( _prompt != value )
                {
                    _prompt = value;
                    OnPropertyChanged( nameof( Prompt ) );
                }
            }
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