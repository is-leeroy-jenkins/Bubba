// ******************************************************************************************
//     Assembly:                Bubba
//     Author:                  Terry D. Eppler
//     Created:                 01-12-2025
// 
//     Last Modified By:        Terry D. Eppler
//     Last Modified On:        01-12-2025
// ******************************************************************************************
// <copyright file="VectorPayload.cs" company="Terry D. Eppler">
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
//   VectorPayload.cs
// </summary>
// ******************************************************************************************

namespace Bubba
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Text.Json.Serialization;
    using Newtonsoft.Json;
    using Properties;

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    [ SuppressMessage( "ReSharper", "UnusedType.Global" ) ]
    [ SuppressMessage( "ReSharper", "PreferConcreteValueOverDefault" ) ]
    [ SuppressMessage( "ReSharper", "MemberCanBePrivate.Global" ) ]
    public class VectorPayload : GptPayload
    {
        /// <summary>
        /// The file path
        /// </summary>
        private protected string _filePath;

        /// <summary>
        /// The vector store identifier
        /// </summary>
        private protected string _vectorStoreId;

        /// <summary>
        /// The file ids
        /// </summary>
        private protected IList<string> _fileIds;

        /// <summary>
        /// The name
        /// </summary>
        private protected string _name;

        /// <summary>
        /// The limit
        /// </summary>
        private protected int _limit;

        /// <summary>
        /// The before
        /// </summary>
        private protected string _before;

        /// <summary>
        /// The after
        /// </summary>
        private protected string _after;

        /// <summary>
        /// The order
        /// </summary>
        private protected string _order;

        /// <summary>
        /// The end point
        /// </summary>
        private protected string _endPoint;

        /// <summary>
        /// The meta data
        /// </summary>
        private protected IDictionary<string, object> _metaData;

        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="VectorPayload"/> class.
        /// </summary>
        /// <inheritdoc />
        public VectorPayload( )
            : base( )
        {
            _model = "gpt-4o-mini";
            _endPoint = GptEndPoint.VectorStores;
            _limit = 20;
            _responseFormat = "text";
            _fileIds = new List<string>( );
            _metaData = new Dictionary<string, object>( );
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
        /// Gets or sets the vector store identifier.
        /// </summary>
        /// <value>
        /// The vector store identifier.
        /// </value>
        [ JsonPropertyName( "vector_store_id" ) ]
        public string VectorStoreId
        {
            get
            {
                return _vectorStoreId;
            }
            set
            {
                if( _vectorStoreId != value )
                {
                    _vectorStoreId = value;
                    OnPropertyChanged( nameof( VectorStoreId ) );
                }
            }
        }

        /// <summary>
        /// Gets or sets the before.
        /// </summary>
        /// <value>
        /// The before.
        /// </value>
        [ JsonPropertyName( "before" ) ]
        public string Before
        {
            get
            {
                return _before;
            }
            set
            {
                if( _before != value )
                {
                    _before = value;
                    OnPropertyChanged( nameof( Before ) );
                }
            }
        }

        /// <summary>
        /// Gets or sets the after.
        /// </summary>
        /// <value>
        /// The after.
        /// </value>
        [ JsonPropertyName( "after" ) ]
        public string After
        {
            get
            {
                return _after;
            }
            set
            {
                if( _after != value )
                {
                    _after = value;
                    OnPropertyChanged( nameof( After ) );
                }
            }
        }

        /// <summary>
        /// Gets or sets the order.
        /// </summary>
        /// <value>
        /// The order.
        /// </value>
        [ JsonPropertyName( "order" ) ]
        public string Order
        {
            get
            {
                return _order;
            }
            set
            {
                if( _order != value )
                {
                    _order = value;
                    OnPropertyChanged( nameof( Order ) );
                }
            }
        }

        /// <inheritdoc />
        /// <summary>
        /// THe number 'n' of responses generatred.
        /// </summary>
        /// <value>
        /// The user identifier.
        /// </value>
        [ JsonPropertyName( "limit" ) ]
        public int Limit
        {
            get
            {
                return _limit;
            }
            set
            {
                if( _limit != value )
                {
                    _limit = value;
                    OnPropertyChanged( nameof( Limit ) );
                }
            }
        }

        /// <summary>
        /// Gets or sets the file ids.
        /// </summary>
        /// <value>
        /// The file ids.
        /// </value>
        [ JsonPropertyName( "file_ids" ) ]
        public IList<string> FileIds
        {
            get
            {
                return _fileIds;
            }
            set
            {
                if( _fileIds != value )
                {
                    _fileIds = value;
                    OnPropertyChanged( nameof( FileIds ) );
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
        [ JsonPropertyName( "model" ) ]
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
        /// Gets or sets a value indicating whether this
        /// <see cref="T:Bubba.ParameterBase" /> is stream.
        /// </summary>
        /// <value>
        ///   <c>true</c> if stream; otherwise, <c>false</c>.
        /// </value>
        [ JsonPropertyName( "name" ) ]
        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                if( _name != value )
                {
                    _name = value;
                    OnPropertyChanged( nameof( Name ) );
                }
            }
        }

        /// <summary>
        /// Set of 16 key-value pairs that can be attached to an object.
        /// This can be useful for storing additional information about the
        /// object in a structured format. Keys can be a maximum of 64 characters
        /// long and values can be a maximum of 512 characters long
        /// </summary>
        /// <value>
        /// The meta data.
        /// </value>
        [ JsonPropertyName( "meta_data" ) ]
        public IDictionary<string, object> MetaData
        {
            get
            {
                return _metaData;
            }
            set
            {
                if( _metaData != value )
                {
                    _metaData = value;
                    OnPropertyChanged( nameof( MetaData ) );
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
                _data.Add( "n", _number );
                _data.Add( "max_completion_tokens", _maximumTokens );
                _data.Add( "store", _store );
                _data.Add( "stream", _stream );
                _data.Add( "temperature", _temperature );
                _data.Add( "frequency_penalty", _frequencyPenalty );
                _data.Add( "presence_penalty", _presencePenalty );
                _data.Add( "top_p", _topPercent );
                _data.Add( "limit", _limit );
                _data.Add( "response_format", _responseFormat );
                _data.Add( "modalitites", _modalities );
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
    }
}