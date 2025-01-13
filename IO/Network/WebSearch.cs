﻿// ******************************************************************************************
//     Assembly:                Bubba
//     Author:                  Terry D. Eppler
//     Created:                 01-12-2025
// 
//     Last Modified By:        Terry D. Eppler
//     Last Modified On:        01-12-2025
// ******************************************************************************************
// <copyright file="WebSearch.cs" company="Terry D. Eppler">
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
//   WebSearch.cs
// </summary>
// ******************************************************************************************

namespace Bubba
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Net.NetworkInformation;
    using System.Text;

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    [ SuppressMessage( "ReSharper", "InconsistentNaming" ) ]
    [ SuppressMessage( "ReSharper", "MemberCanBePrivate.Global" ) ]
    public abstract class WebSearch : PropertyChangedBase
    {
        /// <summary>
        /// The locked object
        /// </summary>
        private protected object _entry;

        /// <summary>
        /// The busy
        /// </summary>
        private protected bool _busy;

        /// <summary>
        /// The query
        /// </summary>
        private protected string _keyWords;

        /// <summary>
        /// The link
        /// </summary>
        private protected string _link;

        /// <summary>
        /// The name
        /// </summary>
        private protected string _name;

        /// <summary>
        /// The content
        /// </summary>
        private protected string _content;

        /// <summary>
        /// The title
        /// </summary>
        private protected string _title;

        /// <summary>
        /// The project identifier
        /// </summary>
        private protected string _projectId;

        /// <summary>
        /// The project number
        /// </summary>
        private protected string _projectNumber;

        /// <summary>
        /// The URL
        /// </summary>
        private protected string _url;

        /// <summary>
        /// Gets a value indicating whether this instance is busy.
        /// </summary>
        /// <value>
        /// <c> true </c>
        /// if this instance is busy; otherwise,
        /// <c> false </c>
        /// </value>
        public bool IsBusy
        {
            get
            {
                lock( _entry )
                {
                    return _busy;
                }
            }
        }

        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="WebSearch"/> class.
        /// </summary>
        protected WebSearch( )
        {
        }

        /// <summary>
        /// Gets or sets the query.
        /// </summary>
        /// <value>
        /// The query.
        /// </value>
        public virtual string KeyWords
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
        public string Link
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
        public string ProjectId
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
        public string ProjectNumber
        {
            get
            {
                return _projectNumber;
            }
            set
            {
                if(_projectNumber != value)
                {
                    _projectNumber = value;
                    OnPropertyChanged(nameof(ProjectNumber));
                }
            }
        }

        /// <summary>
        /// Gets or sets the URL.
        /// </summary>
        /// <value>
        /// The URL.
        /// </value>
        public string Url
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
        /// Gets or sets the content.
        /// </summary>
        /// <value>
        /// The content.
        /// </value>
        public string Content
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
        public string Title
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
        /// Pings the network.
        /// </summary>
        /// <param name="ipAddress">
        /// The host name or address.
        /// </param>
        /// <returns>
        /// bool
        /// </returns>
        private protected virtual bool PingNetwork( string ipAddress )
        {
            var _status = false;
            try
            {
                ThrowIf.Null( ipAddress, nameof( ipAddress ) );
                using var _ping = new Ping( );
                var _buffer = "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa"u8.ToArray( );
                var _timeout = 5000;// 5sg
                var _reply = _ping.Send( ipAddress, _timeout, _buffer );
                if( _reply != null )
                {
                    _status = _reply.Status == IPStatus.Success;
                }
            }
            catch( Exception ex )
            {
                _status = false;
                Fail( ex );
            }

            return _status;
        }

        /// <summary>
        /// Notifies this instance.
        /// </summary>
        private protected virtual void SendNotification( string message )
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

        /// <summary>
        /// Begins the initialize.
        /// </summary>
        private protected void Busy( )
        {
            try
            {
                lock( _entry )
                {
                    _busy = true;
                }
            }
            catch( Exception ex )
            {
                Fail( ex );
            }
        }

        /// <summary>
        /// Ends the initialize.
        /// </summary>
        private protected void Chill( )
        {
            try
            {
                lock( _entry )
                {
                    _busy = false;
                }
            }
            catch( Exception ex )
            {
                Fail( ex );
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