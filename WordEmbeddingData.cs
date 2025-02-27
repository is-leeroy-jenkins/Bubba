
namespace Bubba
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class WordEmbeddingData
    {
        private protected string _text;

        private protected float[ ] _wordEmbedding;

        public WordEmbeddingData( )
        {
        }

        public string Text { get; set; }

        public float[] WordEmbedding { get; set; }
    }
}
