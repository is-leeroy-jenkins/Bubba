

namespace Bubba
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Diagnostics.CodeAnalysis;
    using Microsoft.Xaml.Behaviors.Media;

    [ SuppressMessage( "ReSharper", "UnusedType.Global" ) ]
    [ SuppressMessage( "ReSharper", "MemberCanBePrivate.Global" ) ]
    [ SuppressMessage( "ReSharper", "FieldCanBeMadeReadOnly.Global" ) ]
    public class GptSystemMessage
    {
        /// <summary>
        /// The role
        /// </summary>
        private protected string _role;

        /// <summary>
        /// The content
        /// </summary>
        private protected string _content;

        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="GptSystemMessage"/> class.
        /// </summary>
        public GptSystemMessage( )
        {
            _role = "system";
        }

        public GptSystemMessage( string prompt ) 
            : this( )
        {
            _content = prompt;
        }

        /// <summary>
        /// Gets the role.
        /// </summary>
        /// <value>
        /// The role.
        /// </value>
        public string Role
        {
            get
            {
                return _role;
            }
        }

        /// <summary>
        /// Gets the content.
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
        }

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>
        /// A string that represents the current object.
        /// </returns>
        public override string ToString( )
        {
            return $" role : system, content: {_content}";
        }
    }
}
