

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
    public class GptContainerFile : PropertyChangedBase
    {
        /// <summary>
        /// The container identifier
        /// </summary>
        private protected string _containerId;

        /// <summary>
        /// The file
        /// </summary>
        private protected GptFile _file;

        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="GptContainerFile"/> class.
        /// </summary>
        public GptContainerFile( )
        {
        }

        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="GptContainerFile"/> class.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="file">The file.</param>
        public GptContainerFile( string id, GptFile file )
        {
            _containerId = id;
            _file = file;
        }

        /// <summary>
        /// Gets or sets the container identifier.
        /// </summary>
        /// <value>
        /// The container identifier.
        /// </value>
        public string ContainerId
        {
            get
            {
                return _containerId;
            }
            set
            {
                if( _containerId != value )
                {
                    _containerId = value;
                    OnPropertyChanged( nameof( ContainerId ) );
                }
            }
        }

        /// <summary>
        /// Gets or sets the file.
        /// </summary>
        /// <value>
        /// The file.
        /// </value>
        public GptFile File
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
