// ******************************************************************************************
//     Assembly:                Bubba
//     Author:                  Terry D. Eppler
//     Created:                 01-10-2025
// 
//     Last Modified By:        Terry D. Eppler
//     Last Modified On:        01-10-2025
// ******************************************************************************************
// <copyright file="PayloadBase.cs" company="Terry D. Eppler">
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
//   PayloadBase.cs
// </summary>
// ******************************************************************************************

namespace Bubba
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Diagnostics.CodeAnalysis;
    using System.Runtime.CompilerServices;
    using Newtonsoft.Json;

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    [ SuppressMessage( "ReSharper", "MemberCanBePrivate.Global" ) ]
    [ SuppressMessage( "ReSharper", "MemberCanBeProtected.Global" ) ]
    [ SuppressMessage( "ReSharper", "PossibleUnintendedReferenceComparison" ) ]
    [ SuppressMessage( "ReSharper", "BadParensSpaces" ) ]
    public abstract class PayloadBase : INotifyPropertyChanged, IPayload
    {
        /// <summary>
        /// The user identifier
        /// </summary>
        private protected string _id;

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
        /// An upper bound for the number of tokens
        /// that can be generated for a completion
        /// </summary>
        private protected int _maximumTokens;

        /// <summary>
        /// TNumber between -2.0 and 2.0. Positive values penalize new
        /// tokens based on their existing frequency in the text so far,
        /// decreasing the model's likelihood to repeat the same line verbatim.
        /// </summary>
        private protected double _frequencyPenalty;

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
        private protected double _presencePenalty;

        /// <summary>
        /// Whether or not to store the responses in the Chat Log
        /// </summary>
        private protected bool _store;

        /// <summary>
        /// Stream the response in chuncks.
        /// </summary>
        private protected bool _stream;

        /// <summary>
        /// The size
        /// </summary>
        private protected string _imageSize;

        /// <summary>
        /// The string provided to GPT
        /// </summary>
        private protected string _systemPrompt;

        /// <summary>
        /// The user prompt
        /// </summary>
        private protected string _userPrompt;

        /// <summary>
        /// The response format
        /// </summary>
        private protected string _responseFormat;

        /// <summary>
        /// The messages
        /// </summary>
        private protected List<IGptMessage> _messages;

        /// <summary>
        /// The data
        /// </summary>
        private protected IDictionary<string, object> _data;

        /// <inheritdoc />
        /// <summary>
        /// Occurs when a property value changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Called when [property changed].
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        public void OnPropertyChanged( [ CallerMemberName ] string propertyName = null )
        {
            PropertyChanged?.Invoke( this, new PropertyChangedEventArgs( propertyName ) );
        }

        /// <summary>
        /// Gets the user identifier.
        /// </summary>
        /// <value>
        /// The user identifier.
        /// </value>
        [ JsonProperty( "id" ) ]
        public string Id
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

        /// <inheritdoc />
        /// <summary>
        /// Gets the maximum tokens.
        /// </summary>
        /// <value>
        /// The maximum tokens.
        /// </value>
        [ JsonProperty( "max_completion_tokens" ) ]
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

        /// <inheritdoc />
        /// <summary>
        /// Gets or sets the temperature.
        /// </summary>
        /// <value>
        /// The temperature.
        /// </value>
        [ JsonProperty( "temperature" ) ]
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

        /// <inheritdoc />
        /// <summary>
        /// Gets the frequency.
        /// </summary>
        /// <value>
        /// The frequency.
        /// </value>
        [ JsonProperty( "frequency_penalty" ) ]
        public double FrequencyPenalty
        {
            get
            {
                return _frequencyPenalty;
            }
            set
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
        /// Gets or sets the number.
        /// </summary>
        /// <value>
        /// The number.
        /// </value>
        [ JsonProperty( "n" ) ]
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

        /// <inheritdoc />
        /// <summary>
        /// Gets the presence.
        /// </summary>
        /// <value>
        /// The presence.
        /// </value>
        [ JsonProperty( "presence_penalty" ) ]
        public double PresencePenalty
        {
            get
            {
                return _presencePenalty;
            }
            set
            {
                if( _presencePenalty != value )
                {
                    _presencePenalty = value;
                    OnPropertyChanged( nameof( PresencePenalty ) );
                }
            }
        }

        /// <summary>
        /// Gets or sets the top percent.
        /// </summary>
        /// <value>
        /// The top percent.
        /// </value>
        [ JsonProperty( "top_p" ) ]
        public double TopPercent
        {
            get
            {
                return _topPercent;
            }
            set
            {
                if(_topPercent != value)
                {
                    _topPercent = value;
                    OnPropertyChanged(nameof(TopPercent));
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
        [ JsonProperty( "models" ) ]
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

        /// <inheritdoc />
        /// <summary>
        /// Gets or sets the size of the image.
        /// </summary>
        /// <value>
        /// The size of the image.
        /// </value>
        [ JsonProperty( "size" ) ]
        public string ImageSize
        {
            get
            {
                return _imageSize;
            }
            set
            {
                if( _imageSize != value )
                {
                    _imageSize = value;
                    OnPropertyChanged( nameof( ImageSize ) );
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
        [ JsonProperty( "prompt" ) ]
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
                if(_systemPrompt != value)
                {
                    _systemPrompt = value;
                    OnPropertyChanged(nameof(UserPrompt));
                }
            }
        }

        /// <inheritdoc />
        /// <summary>
        /// Gets or sets the response format.
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
        /// Gets or sets the stop sequences.
        /// </summary>
        /// <value>
        /// The stop sequences.
        /// </value>
        [ JsonProperty( "stop" ) ]
        public IList<string> StopSequences
        {
            get
            {
                return _stopSequences;
            }
            set
            {
                if(_stopSequences != value)
                {
                    _stopSequences = value;
                    OnPropertyChanged(nameof(StopSequences));
                }
            }
        }

        /// <inheritdoc />
        /// <summary>
        /// Gets or sets the data.
        /// </summary>
        /// <value>
        /// The data.
        /// </value>
        public IDictionary<string, object> Data
        {
            get
            {
                return _data;
            }
            set
            {
                if( _data != value )
                {
                    _data = value;
                    OnPropertyChanged( nameof( Data ) );
                }
            }
        }
    }
}