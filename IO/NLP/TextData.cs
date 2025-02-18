


namespace Bubba
{
    using System;
    using System.Text;
    using System.Threading.Tasks;
    using System.Collections.Generic;
    using System.Data.SQLite;
    using System.IO;
    using System.Linq;
    using Microsoft.ML;
    using Microsoft.ML.Data;

    public class TextData
    {
        private protected string _text;

        public TextData( )
        {
        }

        [ LoadColumn( 0 ) ]
        public string Text { get; set; }
    }
}
