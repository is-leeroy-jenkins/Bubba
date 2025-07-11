

namespace Bubba
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// 
    /// </summary>
    [ SuppressMessage( "ReSharper", "MemberCanBePrivate.Global" ) ]
    [ SuppressMessage( "ReSharper", "PossibleUnintendedReferenceComparison" ) ]
    public class GptContainer : PropertyChangedBase
    {
        /// <summary>
        /// The identifier
        /// </summary>
        private protected string _id;

        /// <summary>
        /// The status
        /// </summary>
        private protected string _status;

        /// <summary>
        /// The name
        /// </summary>
        private protected string _name;

        /// <summary>
        /// The expires after
        /// </summary>
        private protected int _expiresAfter;

        /// <summary>
        /// The file ids
        /// </summary>
        private protected IList<string> _fileIds;

        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="GptContainer"/> class.
        /// </summary>
        public GptContainer( )
        {
        }

        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="GptContainer"/> class.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="status">The status.</param>
        /// <param name="name">The name.</param>
        /// <param name="expires">The expires.</param>
        public GptContainer( int id, string status, string name, 
                             int expires )
        {
            _id = id.ToString( );
            _status = status;
            _name = name;
            _expiresAfter = expires;
            _fileIds = new List<string>( );
        }

        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
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
        /// Gets or sets the status.
        /// </summary>
        /// <value>
        /// The status.
        /// </value>
        public string Status
        {
            get
            {
                return _status;
            }
            set
            {
                if( _status != value )
                {
                    _status = value;
                    OnPropertyChanged( nameof( Status ) );
                }
            }
        }

        /// <summary>
        /// Gets or sets the file ids.
        /// </summary>
        /// <value>
        /// The file ids.
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
