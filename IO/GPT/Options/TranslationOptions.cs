// ******************************************************************************************
//     Assembly:                Bubba
//     Author:                  Terry D. Eppler
//     Created:                 02-17-2025
// 
//     Last Modified By:        Terry D. Eppler
//     Last Modified On:        02-17-2025
// ******************************************************************************************
// <copyright file="TranslationOptions.cs" company="Terry D. Eppler">
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
//   TranslationOptions.cs
// </summary>
// ******************************************************************************************
namespace Bubba
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Text.Json.Serialization;
    using Properties;

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    /// <seealso cref="T:Bubba.GptOptions" />
    [ SuppressMessage( "ReSharper", "UnusedType.Global" ) ]
    [ SuppressMessage( "ReSharper", "MemberCanBePrivate.Global" ) ]
    [ SuppressMessage( "ReSharper", "PreferConcreteValueOverDefault" ) ]
    [ SuppressMessage( "ReSharper", "FieldCanBeMadeReadOnly.Global" ) ]
    [ SuppressMessage( "ReSharper", "ClassCanBeSealed.Global" ) ]
    public class TranslationOptions : GptOptions
    {
        /// <summary>
        /// The input
        /// </summary>
        private protected string _prompt;

        /// <summary>
        /// The file path
        /// </summary>
        private protected object _file;

        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="TranscriptionOptions"/> class.
        /// </summary>
        /// <inheritdoc />
        public TranslationOptions( )
            : base( )
        {
            _model = "whisper-1";
            _endPoint = GptEndPoint.Translations;
            _store = true;
            _stream = true;
            _number = 1;
            _temperature = 0.8;
            _topPercent = 0.9;
            _frequencyPenalty = 0.00;
            _presencePenalty = 0.00;
            _maxCompletionTokens = 10000;
        }

        /// <summary>
        /// Gets or sets the input.
        /// </summary>
        /// <value>
        /// The input.
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
        /// Gets or sets the file path.
        /// </summary>
        /// <value>
        /// The file path.
        /// </value>
        public object File
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

        /// <inheritdoc />
        /// <summary>
        /// Gets or sets the modalities.
        /// </summary>
        /// <value>
        /// The modalities.
        /// </value>
        [ JsonPropertyName( "response_format" ) ]
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
        /// Gets the data.
        /// </summary>
        /// <returns></returns>
        public override IDictionary<string, object> GetData( )
        {
            try
            {
                _data.Add( "Number", _number );
                _data.Add( "MaxCompletionTokens", _maxCompletionTokens );
                _data.Add( "Endoint", _endPoint );
                _data.Add( "Store", _store );
                _data.Add( "Stream", _stream );
                _data.Add( "Temperature", _temperature );
                _data.Add( "TopPercent", _topPercent );
                _data.Add( "FrequencyPenalty", _frequencyPenalty );
                _data.Add( "PresencePenalty", _presencePenalty );
                _data.Add( "ResponseFormat", _responseFormat );
                _data.Add( "Stop", _stop );
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

        /// <summary>
        /// Gets the formats.
        /// </summary>
        /// <returns></returns>
        public IList<string> GetResponseFormats( )
        {
            try
            {
                var _options = new List<string>
                {
                    "json",
                    "text",
                    "srt",
                    "verbose_json",
                    "vtt"
                };

                return _options?.Any( ) == true
                    ? _options
                    : default( IList<string> );
            }
            catch( Exception ex )
            {
                Fail( ex );
                return default( IList<string> );
            }
        }

        /// <summary>
        /// Gets the formats.
        /// </summary>
        /// <returns></returns>
        public IList<string> GetFileFormats( )
        {
            try
            {
                var _options = new List<string>
                {
                    "flac",
                    "mp3",
                    "mp4",
                    "mpeg",
                    "mpga",
                    "m4a",
                    "ogg",
                    "wav",
                    "webm"
                };

                return _options?.Any( ) == true
                    ? _options
                    : default( IList<string> );
            }
            catch( Exception ex )
            {
                Fail( ex );
                return default( IList<string> );
            }
        }

        /// <summary>
        /// Gets the voice options.
        /// </summary>
        /// <returns></returns>
        public IList<string> GetVoiceOptions( )
        {
            try
            {
                var _options = new List<string>
                {
                    "alloy",
                    "ash",
                    "ballad",
                    "coral",
                    "echo",
                    "fable",
                    "onyx",
                    "nova",
                    "sage",
                    "shimmer"
                };

                return _options?.Any( ) == true
                    ? _options
                    : default( IList<string> );
            }
            catch( Exception ex )
            {
                Fail( ex );
                return default( IList<string> );
            }
        }

        /// <summary>
        /// Gets the model options.
        /// </summary>
        /// <returns></returns>
        public IList<string> GetModelOptions( )
        {
            try
            {
                var _options = new List<string>
                {
                    "tts-1",
                    "tts-1-hd",
                    "gpt-4o-mini-tts"
                };

                return _options?.Any( ) == true
                    ? _options
                    : default( IList<string> );
            }
            catch( Exception ex )
            {
                Fail( ex );
                return default( IList<string> );
            }
        }
    }
}