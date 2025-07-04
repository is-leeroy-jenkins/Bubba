﻿// ******************************************************************************************
//     Assembly:                Bubba
//     Author:                  Terry D. Eppler
//     Created:                 01-07-2025
// 
//     Last Modified By:        Terry D. Eppler
//     Last Modified On:        01-07-2025
// ******************************************************************************************
// <copyright file="GoogleSearch.cs" company="Terry D. Eppler">
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
//   GoogleSearch.cs
// </summary>
// ******************************************************************************************

namespace Bubba
{
    using Google.Apis.CustomSearchAPI.v1;
    using Google.Apis.Services;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Threading.Tasks;
    using Properties;

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    [ SuppressMessage( "ReSharper", "PossibleNullReferenceException" ) ]
    [ SuppressMessage( "ReSharper", "UseObjectOrCollectionInitializer" ) ]
    [ SuppressMessage( "ReSharper", "MemberCanBePrivate.Global" ) ]
    [ SuppressMessage( "ReSharper", "ArrangeDefaultValueWhenTypeNotEvident" ) ]
    [ SuppressMessage( "ReSharper", "ReturnTypeCanBeEnumerable.Global" ) ]
    [ SuppressMessage( "ReSharper", "ClassCanBeSealed.Global" ) ]
    [ SuppressMessage( "ReSharper", "MemberCanBeInternal" ) ]
    [ SuppressMessage( "ReSharper", "AutoPropertyCanBeMadeGetOnly.Global" ) ]
    [ SuppressMessage( "ReSharper", "RedundantBaseConstructorCall" ) ]
    [ SuppressMessage( "ReSharper", "UnusedType.Global" ) ]
    [ SuppressMessage( "ReSharper", "PreferConcreteValueOverDefault" ) ]
    public class GoogleSearch : SearchBase
    {
        /// <summary>
        /// The key
        /// </summary>
        private readonly string _key;

        /// <summary>
        /// The engine
        /// </summary>
        private readonly string _engineId;

        /// <inheritdoc />
        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="T:Bubba.GoogleSearch" /> class.
        /// </summary>
        public GoogleSearch( )
            : base( )
        {
            _key = SearchEngine.KEY;
            _engineId = SearchEngine.ID;
            _projectId = SearchEngine.ProjectID;
            _projectNumber = SearchEngine.ProjectNumber;
            _url = SearchEngine.URL;
        }

        /// <inheritdoc />
        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="T:Bubba.GoogleSearch" /> class.
        /// </summary>
        /// <param name="keyWords">
        /// The keyWords.
        /// </param>
        public GoogleSearch( string keyWords )
            : this( )
        {
            _keyWords = keyWords;
        }

        /// <inheritdoc />
        /// <summary>
        /// Gets or sets the query.
        /// </summary>
        /// <value>
        /// The query.
        /// </value>
        public override string KeyWords
        {
            get
            {
                return _keyWords;
            }
            set
            {
                if( _keyWords != value )
                {
                    _keyWords = value;
                    OnPropertyChanged( nameof( KeyWords ) );
                }
            }
        }

        /// <summary>
        /// Gets or sets the link.
        /// </summary>
        /// <value>
        /// The link.
        /// </value>
        public override string Link
        {
            get
            {
                return _link;
            }
            set
            {
                if( _link != value )
                {
                    _link = value;
                    OnPropertyChanged( nameof( Link ) );
                }
            }
        }

        /// <summary>
        /// Gets or sets the project identifier.
        /// </summary>
        /// <value>
        /// The project identifier.
        /// </value>
        public override string ProjectId
        {
            get
            {
                return _projectId;
            }
            set
            {
                if( _link != value )
                {
                    _projectId = value;
                    OnPropertyChanged( nameof( ProjectId ) );
                }
            }
        }

        /// <summary>
        /// Gets or sets the project number.
        /// </summary>
        /// <value>
        /// The project number.
        /// </value>
        public override string ProjectNumber
        {
            get
            {
                return _projectNumber;
            }
            set
            {
                if( _projectNumber != value )
                {
                    _projectNumber = value;
                    OnPropertyChanged( nameof( ProjectNumber ) );
                }
            }
        }

        /// <summary>
        /// Gets or sets the URL.
        /// </summary>
        /// <value>
        /// The URL.
        /// </value>
        public override string Url
        {
            get
            {
                return _url;
            }
            set
            {
                if( _url != value )
                {
                    _url = value;
                    OnPropertyChanged( nameof( Url ) );
                }
            }
        }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public override string Name
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
        /// Gets or sets the content.
        /// </summary>
        /// <value>
        /// The content.
        /// </value>
        public override string Content
        {
            get
            {
                return _content;
            }
            set
            {
                if( _content != value )
                {
                    _content = value;
                    OnPropertyChanged( nameof( Content ) );
                }
            }
        }

        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>
        /// The title.
        /// </value>
        public override string Title
        {
            get
            {
                return _title;
            }
            set
            {
                if( _title != value )
                {
                    _title = value;
                    OnPropertyChanged( nameof( Title ) );
                }
            }
        }

        /// <summary>
        /// Gets the results.
        /// </summary>
        /// <returns>
        /// List(SearchResult)
        /// </returns>
        public IList<SearchResult> GetResults( )
        {
            try
            {
                var _count = 0;
                var _results = new List<SearchResult>( );
                var _initializer = new BaseClientService.Initializer( );
                _initializer.ApiKey = _key;
                var _customSearch = new CustomSearchAPIService( _initializer );
                var _searchRequest = _customSearch?.Cse?.List( );
                if( _searchRequest != null )
                {
                    _searchRequest.Q = _keyWords;
                    _searchRequest.Cx = _engineId;
                    _searchRequest.Start = _count;
                    var _list = _searchRequest.Execute( )
                        ?.Items
                        ?.ToList( );

                    if( _list?.Any( ) == true )
                    {
                        for( var _i = 0; _i < _list.Count; _i++ )
                        {
                            var _snippet = _list[ _i ].Snippet ?? string.Empty;
                            var _lines = _list[ _i ].Link ?? string.Empty;
                            var _titles = _list[ _i ].Title ?? string.Empty;
                            var _htmlTitle = _list[ _i ].HtmlTitle ?? string.Empty;
                            var _searchResults = new SearchResult( _snippet, _lines,
                                _titles, _htmlTitle );

                            _results.Add( _searchResults );
                        }

                        return _results?.Any( ) == true
                            ? _results
                            : default( IList<SearchResult> );
                    }
                    else
                    {
                        return default( IList<SearchResult> );
                    }
                }
                else
                {
                    return default( IList<SearchResult> );
                }
            }
            catch( Exception ex )
            {
                Fail( ex );
                return default( IList<SearchResult> );
            }
        }

        /// <summary>
        /// Gets the results.
        /// </summary>
        /// <returns>
        /// Task(IList(SearchResult))
        /// </returns>
        public Task<IList<SearchResult>> GetResultsAsync( )
        {
            var _async = new TaskCompletionSource<IList<SearchResult>>( );
            try
            {
                var _count = 0;
                var _results = new List<SearchResult>( );
                var _initializer = new BaseClientService.Initializer( );
                _initializer.ApiKey = _key;
                var _customSearch = new CustomSearchAPIService( _initializer );
                var _searchRequest = _customSearch?.Cse?.List( );
                if( _searchRequest != null )
                {
                    _searchRequest.Q = _keyWords;
                    _searchRequest.Cx = _engineId;
                    _searchRequest.Start = _count;
                    var _list = _searchRequest.Execute( )
                        ?.Items
                        ?.ToList( );

                    if( _list?.Any( ) == true )
                    {
                        for( var _i = 0; _i < _list.Count; _i++ )
                        {
                            var _snippet = _list[ _i ].Snippet ?? string.Empty;
                            var _line = _list[ _i ].Link ?? string.Empty;
                            var _titles = _list[ _i ].Title ?? string.Empty;
                            var _htmlTitle = _list[ _i ].HtmlTitle ?? string.Empty;
                            var _searchResults =
                                new SearchResult( _snippet, _line, _titles, _htmlTitle );

                            _results.Add( _searchResults );
                        }

                        _async.SetResult( _results );
                        return _results?.Any( ) == true
                            ? _async.Task
                            : default( Task<IList<SearchResult>> );
                    }
                    else
                    {
                        return default( Task<IList<SearchResult>> );
                    }
                }
                else
                {
                    return default( Task<IList<SearchResult>> );
                }
            }
            catch( Exception ex )
            {
                _async.SetException( ex );
                Fail( ex );
                return default( Task<IList<SearchResult>> );
            }
        }

        /// <inheritdoc />
        /// <summary>
        /// Notifies this instance.
        /// </summary>
        private protected override void SendNotification( string message )
        {
            try
            {
                ThrowIf.Null( message, nameof( message ) );
                var _notify = new SplashMessage( message );
                _notify.Show( );
            }
            catch( Exception ex )
            {
                Fail( ex );
            }
        }
    }
}