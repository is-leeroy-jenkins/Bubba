// ******************************************************************************************
//     Assembly:                Bubba
//     Author:                  Terry D. Eppler
//     Created:                 01-08-2025
// 
//     Last Modified By:        Terry D. Eppler
//     Last Modified On:        01-08-2025
// ******************************************************************************************
// <copyright file="TranslationParameter.cs" company="Terry D. Eppler">
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
//   TranslationParameter.cs
// </summary>
// ******************************************************************************************

namespace Bubba
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using Properties;

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    /// <seealso cref="T:Bubba.GptParameter" />
    [ SuppressMessage( "ReSharper", "UnusedType.Global" ) ]
    [ SuppressMessage( "ReSharper", "MemberCanBePrivate.Global" ) ]
    [ SuppressMessage( "ReSharper", "PreferConcreteValueOverDefault" ) ]
    [ SuppressMessage( "ReSharper", "FieldCanBeMadeReadOnly.Global" ) ]
    public class TranslationParameter : GptParameter
    {
        /// <summary>
        /// The language
        /// </summary>
        private protected string _language;

        /// <summary>
        /// The input
        /// </summary>
        private protected string _input;

        /// <summary>
        /// The file path
        /// </summary>
        private protected string _file;

        /// <summary>
        /// The audio data
        /// </summary>
        private protected byte[ ] _audioData;

        /// <summary>
        /// The modalities
        /// </summary>
        private protected IList<string> _modalities;

        /// <summary>
        /// The voice
        /// </summary>
        private protected string _voice;

        /// <summary>
        /// The speed
        /// </summary>
        private protected int _speed;

        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="TranscriptionParameter"/> class.
        /// </summary>
        /// <inheritdoc />
        public TranslationParameter( )
            : base( )
        {
            _model = "tts-1";
            _endPoint = GptEndPoint.SpeechGeneration;
            _responseFormat = "mp3";
            _modalities = new List<string>( );
            _voice = "fable";
            _language = "en";
        }

        /// <summary>
        /// Gets or sets the language.
        /// </summary>
        /// <value>
        /// The language.
        /// </value>
        public string Language
        {
            get
            {
                return _language;
            }
            set
            {
                if( _language != value )
                {
                    _language = value;
                    OnPropertyChanged( nameof( Language ) );
                }
            }
        }

        /// <summary>
        /// Gets or sets the input.
        /// </summary>
        /// <value>
        /// The input.
        /// </value>
        public string Input
        {
            get
            {
                return _input;
            }
            set
            {
                if(_input != value)
                {
                    _input = value;
                    OnPropertyChanged(nameof(Input));
                }
            }
        }

        /// <summary>
        /// Gets or sets the voice.
        /// </summary>
        /// <value>
        /// The voice.
        /// </value>
        public string Voice
        {
            get
            {
                return _voice;
            }
            set
            {
                if( _voice != value )
                {
                    _voice = value;
                    OnPropertyChanged( nameof( Voice ) );
                }
            }
        }

        /// <summary>
        /// Gets or sets the speed.
        /// </summary>
        /// <value>
        /// The speed.
        /// </value>
        public int Speed
        {
            get
            {
                return _speed;
            }
            set
            {
                if( _speed != value )
                {
                    _speed = value;
                    OnPropertyChanged( nameof( Speed ) );
                }
            }
        }

        /// <summary>
        /// Gets or sets the file path.
        /// </summary>
        /// <value>
        /// The file path.
        /// </value>
        public string File
        {
            get
            {
                return _file;
            }
            set
            {
                if( _file != value )
                {
                    _file = value;
                    OnPropertyChanged( nameof( File ) );
                }
            }
        }

        /// <summary>
        /// Gets the chat model.
        /// </summary>
        /// <value>
        /// The chat model.
        /// </value>
        /// <inheritdoc />
        public override string Model
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
        /// Gets or sets the response format.
        /// </summary>
        /// <value>
        /// The response format.
        /// </value>
        public override string ResponseFormat
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
                _data.Add( "number", _number );
                _data.Add( "max_completion_tokens", _maximumTokens );
                _data.Add( "store", _store );
                _data.Add( "stream", _stream );
                _data.Add( "temperature", _temperature );
                _data.Add( "frequency_penalty", _frequencyPenalty );
                _data.Add( "presence_penalty", _presencePenalty );
                _data.Add( "top_p", _topPercent );
                _data.Add( "response_format", _responseFormat );
                _data.Add( "endpoint", _endPoint );
                _data.Add( "speed", _speed );
                _data.Add( "voice", _voice );
                _data.Add("language", _language);
                _modalities.Add("text");
                _modalities.Add("audio");
                _data.Add("modalities", _modalities);
                if( !string.IsNullOrEmpty( _file ) )
                {
                    _data.Add( "filepath", _file );
                }

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
        /// Converts to string.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
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