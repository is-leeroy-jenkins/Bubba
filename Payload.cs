
namespace Bubba
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    [ SuppressMessage( "ReSharper", "UnusedType.Global" ) ]
    public class Payload
    {
        /// <summary>
        /// The chat model
        /// </summary>
        private protected string _model;

        /// <summary>
        /// A number between -2.0 and 2.0  Positive value decrease the
        /// model's likelihood to repeat the same line verbatim.
        /// </summary>
        private protected string _temperature;

        /// <summary>
        /// The maximum tokens
        /// </summary>
        private protected string _maximumTokens;

        /// <summary>
        /// The maximum tokens
        /// </summary>
        private protected string _prompt;

        /// <summary>
        /// The messages
        /// </summary>
        private protected List<dynamic> _messages;

        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="Payload"/> class.
        /// </summary>
        public Payload( )
        {
        }
    }
}
