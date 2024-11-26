// ******************************************************************************************
//     Assembly:                Bubba
//     Author:                  Terry D. Eppler
//     Created:                 11-26-2024
// 
//     Last Modified By:        Terry D. Eppler
//     Last Modified On:        11-26-2024
// ******************************************************************************************
// <copyright file="GptConfig.cs" company="Terry D. Eppler">
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
//   GptConfig.cs
// </summary>
// ******************************************************************************************

namespace Bubba
{
    using System;
    using System.Diagnostics.CodeAnalysis;

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    /// <seealso cref="T:Bubba.PropertyChangedBase" />
    [ SuppressMessage( "ReSharper", "MemberCanBePrivate.Global" ) ]
    [ SuppressMessage( "ReSharper", "UnusedType.Global" ) ]
    public class GptConfig : PropertyChangedBase
    {
        /// <summary>
        /// The number responses to generate
        /// </summary>
        private protected int _number;

        /// <summary>
        /// Whether or not to store the responses
        /// </summary>
        private protected bool _store;

        /// <summary>
        /// The stream
        /// </summary>
        private protected bool _stream;

        /// <summary>
        /// An alternative to sampling with temperature,
        /// called nucleus sampling, where the model considers
        /// the results of the tokens with top_p probability mass.
        /// So 0.1 means only the tokens comprising the top 10% probability
        /// mass are considered. We generally recommend altering this
        /// or temperature but not both.
        /// </summary>
        private protected double _topPercent;

        /// <summary>
        /// A number between -2.0 and 2.0  Positive value decrease the
        /// model's likelihood to repeat the same line verbatim.
        /// </summary>
        private protected double _temperature;

        /// <summary>
        /// An upper bound for the number of tokens
        /// that can be generated for a completion
        /// </summary>
        private protected int _maximumTokens;

        /// <summary>
        /// A number between -2.0 and 2.0. Positive values penalize new
        /// tokens based on their existing frequency in the text so far,
        /// decreasing the model's likelihood to repeat the same line verbatim.
        /// </summary>
        private protected double _frequency;

        /// <summary>
        /// Number between -2.0 and 2.0. Positive values penalize new tokens
        /// based on whether they appear in the text so far,
        /// ncreasing the model's likelihood to talk about new topics.
        /// </summary>
        private protected double _presence;

        /// <summary>
        /// The system prompt
        /// </summary>
        private protected string _systemPrompt;

        /// <summary>
        /// The user prompt
        /// </summary>
        private protected string _userPrompt;

        /// <summary>
        /// The assistant prompt
        /// </summary>
        private protected string _assistantPrompt;

        /// <summary>
        /// The chat model
        /// </summary>
        private protected string _model;

        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="GptConfig"/> class.
        /// </summary>
        public GptConfig( )
        {
            _store = false;
            _stream = false;
            _number = 1;
            _maximumTokens = 2048;
            _temperature = 1.0;
            _frequency = 0.0;
            _presence = 0.0;
            _topPercent = 0.0;
        }

        /// <inheritdoc />
        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="T:Bubba.GptConfig" /> class.
        /// </summary>
        /// <param name="systemPrompt">The system prompt.</param>
        /// <param name="userPrompt">The user prompt.</param>
        public GptConfig( string systemPrompt, string userPrompt )
            : this( )
        {
            _systemPrompt = systemPrompt;
            _userPrompt = userPrompt;
        }

        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="GptConfig"/> class.
        /// </summary>
        /// <param name="config">The prompt model.</param>
        public GptConfig( GptConfig config )
        {
            _systemPrompt = config.SystemPrompt;
            _userPrompt = config.UserPrompt;
            _maximumTokens = config.MaximumTokens;
            _temperature = config.Temperature;
            _frequency = config.Frequency;
            _presence = config.Presence;
        }

        /// <summary>
        /// Decontructs the specified system prompt.
        /// </summary>
        /// <param name="systemPrompt">The system prompt.</param>
        /// <param name="userPrompt">The user prompt.</param>
        /// <param name="maxTokens">The maximum tokens.</param>
        /// <param name="temperature">The temperature.</param>
        /// <param name="frequency">The frequency.</param>
        /// <param name="presense">The presense.</param>
        public void Decontruct( out string systemPrompt, out string userPrompt, out int maxTokens,
            out double temperature, out double frequency, out double presense )
        {
            systemPrompt = _systemPrompt;
            userPrompt = _userPrompt;
            maxTokens = _maximumTokens;
            temperature = _temperature;
            frequency = _frequency;
            presense = _presence;
        }

        /// <summary>
        /// Gets or sets a value indicating whether this
        /// <see cref="GptConfig"/> is store.
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
        /// <see cref="GptConfig"/> is stream.
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
        /// An upper bound for the number of tokens
        /// that can be generated for a completion
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
            set
            {
                if( _maximumTokens != value )
                {
                    _maximumTokens = value;
                    OnPropertyChanged( nameof( MaximumTokens ) );
                }
            }
        }

        /// <summary>
        /// A number between -2.0 and 2.0  Positive value decrease the
        /// model's likelihood to repeat the same line verbatim.
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
        /// A number between -2.0 and 2.0. Positive values penalize new
        /// tokens based on their existing frequency in the text so far,
        /// decreasing the model's likelihood to repeat the same line verbatim.
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
        /// Number between -2.0 and 2.0. Positive values penalize new tokens
        /// based on whether they appear in the text so far,
        /// ncreasing the model's likelihood to talk about new topics.
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

        /// <summary>
        /// THe number 'n' returned by the response.
        /// </summary>
        /// <value>
        /// The user identifier.
        /// </value>
        public int Number
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

        /// <summary>
        /// Gets or sets the system prompt.
        /// </summary>
        /// <value>
        /// The system prompt.
        /// </value>
        public string SystemPrompt
        {
            get
            {
                return _systemPrompt;
            }
            set
            {
                if( _systemPrompt != value )
                {
                    _systemPrompt = value;
                    OnPropertyChanged( nameof( SystemPrompt ) );
                }
            }
        }

        /// <summary>
        /// Gets or sets the user prompt.
        /// </summary>
        /// <value>
        /// The user prompt.
        /// </value>
        public string UserPrompt
        {
            get
            {
                return _userPrompt;
            }
            set
            {
                if( _userPrompt != value )
                {
                    _userPrompt = value;
                    OnPropertyChanged( nameof( UserPrompt ) );
                }
            }
        }

        /// <summary>
        /// Gets or sets the assistant prompt.
        /// </summary>
        /// <value>
        /// The assistant prompt.
        /// </value>
        public string AssistantPrompt
        {
            get
            {
                return _assistantPrompt;
            }
            set
            {
                if( _assistantPrompt != value )
                {
                    _assistantPrompt = value;
                    OnPropertyChanged( nameof( AssistantPrompt ) );
                }
            }
        }
    }
}