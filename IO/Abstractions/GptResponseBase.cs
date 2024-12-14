

namespace Bubba
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    /// <seealso cref="T:Bubba.GptBase" />
    [ SuppressMessage( "ReSharper", "UnusedType.Global" ) ]
    public class GptResponseBase : GptBase
    {
        /// <summary>
        /// The identifier
        /// </summary>
        private protected string _id;

        /// <summary>
        /// The object
        /// </summary>
        private protected string _object;

        /// <summary>
        /// The created
        /// </summary>
        private protected DateTime _created;

        /// <inheritdoc />
        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="T:Bubba.GptResponseBase" /> class.
        /// </summary>
        protected GptResponseBase( )
            : base( )
        {
        }
    }
}
