

namespace Bubba
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    [ SuppressMessage( "ReSharper", "UnusedType.Global" ) ]
    [ SuppressMessage( "ReSharper", "MemberCanBePrivate.Global" ) ]
    public class GptHeader : PropertyChangedBase
    {
        /// <summary>
        /// The content type
        /// </summary>
        private protected string _contentType;

        /// <summary>
        /// The authorization
        /// </summary>
        private protected string _authorization;

        /// <summary>
        /// The data
        /// </summary>
        private protected IDictionary<string, string> _data; 

        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="GptHeader"/> class.
        /// </summary>
        public GptHeader( )
        {
            _contentType = "application/json";
            _data = new Dictionary<string, string>( );
        }

        /// <inheritdoc />
        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="T:Bubba.GptHeader" /> class.
        /// </summary>
        /// <param name="key">The key.</param>
        public GptHeader( string key ) 
            : this( )
        {
            _contentType = "application/json";
            _authorization = "Bearer " + key;
            _data.Add( "content-type", "application/json" );
            _data.Add( "authorization", _authorization );
        }

        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="GptHeader"/> class.
        /// </summary>
        /// <param name="header">The header.</param>
        public GptHeader( GptHeader header )
        {
            _contentType = header.ContentType;
            _authorization = header.Authorization;
        }

        /// <summary>
        /// Gets the type of the content.
        /// </summary>
        /// <value>
        /// The type of the content.
        /// </value>
        public string ContentType
        {
            get
            {
                return _contentType;
            }
        }

        /// <summary>
        /// Gets or sets the authorization.
        /// </summary>
        /// <value>
        /// The authorization.
        /// </value>
        public string Authorization
        {
            get
            {
                return _authorization;
            }
            set
            {
                _authorization = value;
            }
        }

        /// <summary>Returns a string that represents the current object.</summary>
        /// <returns>A string that represents the current object.</returns>
        public override string ToString( )
        {
            return $" content-type : {_contentType}, authorization : {_authorization}";
        }
    }
}
