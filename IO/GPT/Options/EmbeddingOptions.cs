﻿// ******************************************************************************************
//     Assembly:                Bubba
//     Author:                  Terry D. Eppler
//     Created:                 02-17-2025
// 
//     Last Modified By:        Terry D. Eppler
//     Last Modified On:        02-17-2025
// ******************************************************************************************
// <copyright file="EmbeddingOptions.cs" company="Terry D. Eppler">
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
//   EmbeddingOptions.cs
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
    [ SuppressMessage( "ReSharper", "UnusedType.Global" ) ]
    [ SuppressMessage( "ReSharper", "MemberCanBePrivate.Global" ) ]
    [ SuppressMessage( "ReSharper", "FieldCanBeMadeReadOnly.Global" ) ]
    [ SuppressMessage( "ReSharper", "PreferConcreteValueOverDefault" ) ]
    [ SuppressMessage( "ReSharper", "ClassCanBeSealed.Global" ) ]
    public class EmbeddingOptions : GptOptions
    {
        /// <summary>
        /// The file path
        /// </summary>
        private protected string _filePath;

        /// <summary>
        /// The input
        /// </summary>
        private protected string _input;

        /// <summary>
        /// The encoding format
        /// </summary>
        private protected string _encodingFormat;

        /// <summary>
        /// The dimensions
        /// </summary>
        private protected int _dimensions;

        /// <inheritdoc />
        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="T:Bubba.EmbeddingOptions" /> class.
        /// </summary>
        public EmbeddingOptions( )
            : base( )
        {
            _endPoint = GptEndPoint.Embeddings;
            _store = true;
            _stream = true;
            _number = 1;
            _temperature = 0.80;
            _topPercent = 0.90;
            _frequencyPenalty = 0.00;
            _presencePenalty = 0.00;
            _maxCompletionTokens = 2048;
            _encodingFormat = "float";
        }

        /// <summary>
        /// Gets or sets the file path.
        /// </summary>
        /// <value>
        /// The file path.
        /// </value>
        public string FilePath
        {
            get
            {
                return _filePath;
            }
            set
            {
                if( _filePath != value )
                {
                    _filePath = value;
                    OnPropertyChanged( nameof( FilePath ) );
                }
            }
        }

        /// <summary>
        /// Gets or sets the encoding format.
        /// </summary>
        /// <value>
        /// The encoding format.
        /// </value>
        public string EncodingFormat
        {
            get
            {
                return _encodingFormat;
            }
            set
            {
                if( _encodingFormat != value )
                {
                    _encodingFormat = value;
                    OnPropertyChanged( nameof( EncodingFormat ) );
                }
            }
        }

        /// <inheritdoc />
        /// <summary>
        /// Gets or sets the end point.
        /// </summary>
        /// <value>
        /// The end point.
        /// </value>
        public override string EndPoint
        {
            get
            {
                return _endPoint;
            }
            set
            {
                if( _endPoint != value )
                {
                    _endPoint = value;
                    OnPropertyChanged( nameof( EndPoint ) );
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

        /// <summary>
        /// Input text to embed, encoded as a string or array of tokens.
        /// To embed multiple inputs in a single request, pass an array of
        /// strings or array of token arrays. The input must not exceed
        /// the max input tokens for the model (8192 tokens for all embedding models),
        /// cannot be an empty string, and any array must be 2048 dimensions or less.
        /// In addition to the per-input token limit, all embedding models enforce
        /// a maximum of 300,000 tokens summed across all inputs in a single request.
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
                if( _input != value )
                {
                    _input = value;
                    OnPropertyChanged( nameof( Input ) );
                }
            }
        }

        /// <summary>
        /// Gets or sets the dimensions.
        /// </summary>
        /// <value>
        /// The dimensions.
        /// </value>
        public int Dimensions
        {
            get
            {
                return _dimensions;
            }
            set
            {
                if( _dimensions != value )
                {
                    _dimensions = value;
                    OnPropertyChanged( nameof( Dimensions ) );
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
                _data.Add( "n", _number );
                _data.Add( "model", _model );
                _data.Add( "max_completion_tokens", _maxCompletionTokens );
                _data.Add( "store", _store );
                _data.Add( "stream", _stream );
                _data.Add( "temperature", _temperature );
                _data.Add( "frequency_penalty", _frequencyPenalty );
                _data.Add( "presence_penalty", _presencePenalty );
                _data.Add( "top_p", _topPercent );
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
    }
}