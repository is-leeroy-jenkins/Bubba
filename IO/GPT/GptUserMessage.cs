

namespace Bubba
{
    using System;
    using System.Diagnostics.CodeAnalysis;

    [ SuppressMessage( "ReSharper", "UnusedType.Global" ) ]
    [ SuppressMessage( "ReSharper", "MemberCanBePrivate.Global" ) ]
    [ SuppressMessage( "ReSharper", "FieldCanBeMadeReadOnly.Global" ) ]
    public class GptUserMessage
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
        /// <see cref="GptUserMessage"/> class.
        /// </summary>
        public GptUserMessage( )
        {
            _role = "user";
        }

        /// <inheritdoc />
        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="T:Bubba.GptUserMessage" /> class.
        /// </summary>
        /// <param name="prompt">The prompt.</param>
        public GptUserMessage( string prompt ) 
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
        public override string ToString()
        {
            return $" role : user, content: {_content}";
        }
    }
}
