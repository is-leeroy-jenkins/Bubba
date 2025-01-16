

namespace Bubba
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// 
    /// </summary>
    public class HtmlWriter
    {
        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="HtmlWriter"/> class.
        /// </summary>
        public HtmlWriter( )
        {
        }

        /// <summary>
        /// Writes the specified document.
        /// </summary>
        /// <param name="document">The document.</param>
        /// <returns></returns>
        public static string Write( HtmlDocument document )
        {
            return document.ToString( );
        }
    }
}
