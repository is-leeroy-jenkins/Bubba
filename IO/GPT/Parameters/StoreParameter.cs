// ******************************************************************************************
//     Assembly:                Bubba
//     Author:                  Terry D. Eppler
//     Created:                 01-08-2025
// 
//     Last Modified By:        Terry D. Eppler
//     Last Modified On:        01-08-2025
// ******************************************************************************************
// <copyright file="StoreParameter.cs" company="Terry D. Eppler">
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
//   StoreParameter.cs
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
    [ SuppressMessage( "ReSharper", "PreferConcreteValueOverDefault" ) ]
    [ SuppressMessage( "ReSharper", "MemberCanBePrivate.Global" ) ]
    public class StoreParameter : GptParameter
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
        /// The meta data
        /// </summary>
        private protected IDictionary<string, object> _metaData;

        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="StoreParameter"/> class.
        /// </summary>
        /// <inheritdoc />
        public StoreParameter( )
            : base( )
        {
            _model = "gpt-4o";
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
                if(_filePath != value)
                {
                    _filePath = value;
                    OnPropertyChanged(nameof( FilePath ) );
                }
            }
        }

        /// <summary>
        /// Gets or sets the vector store identifier.
        /// </summary>
        /// <value>
        /// The vector store identifier.
        /// </value>
        public string VectorStoreId
        {
            get
            {
                return _vectorStoreId;
            }
            set
            {
                if(_vectorStoreId != value)
                {
                    _vectorStoreId = value;
                    OnPropertyChanged( nameof( VectorStoreId ) );
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
        /// Gets or sets a value indicating whether this
        /// <see cref="T:Bubba.ParameterBase" /> is store.
        /// </summary>
        /// <value>
        ///   <c>true</c> if store; otherwise, <c>false</c>.
        /// </value>
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

        /// <inheritdoc />
        /// <summary>
        /// Gets or sets a value indicating whether this
        /// <see cref="T:Bubba.ParameterBase" /> is stream.
        /// </summary>
        /// <value>
        ///   <c>true</c> if stream; otherwise, <c>false</c>.
        /// </value>
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
        /// Gets or sets the meta data.
        /// </summary>
        /// <value>
        /// The meta data.
        /// </value>
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
                _data.Add( "limit", _limit  );
                if( !string.IsNullOrEmpty( _filePath ) )
                {
                    _data.Add( "filepath", _filePath );
                }

                if( !string.IsNullOrEmpty( _name ) )
                {
                    _data.Add( "name", _name );
                }

                if( _fileIds?.Any( ) == true )
                {
                    _data.Add( "file_ids", _fileIds );
                }

                if( _metaData?.Any( ) == true )
                {
                    _data.Add( "file_ids", _metaData );
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