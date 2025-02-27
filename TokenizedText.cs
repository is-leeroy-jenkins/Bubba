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

    public class TokenizedText
    {
        private protected string[ ] _tokens;

        public TokenizedText( )
        {
        }

        public string[] Tokens { get; set; } 
    }
}
