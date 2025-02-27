
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
    public class NlpResult
    {
        /// <summary>
        /// The tokens
        /// </summary>
        private protected string[ ] _tokens;

        /// <summary>
        /// The position tags
        /// </summary>
        private protected string[] _posTags;

        /// <summary>
        /// The named entities
        /// </summary>
        private protected string[] _namedEntities;

        /// <summary>
        /// The sentiment
        /// </summary>
        private protected float _sentiment;

        /// <summary>
        /// The word frequencies
        /// </summary>
        private protected IDictionary<string, int> _wordFrequencies;

        /// <summary>
        /// The inverse frequency scores
        /// </summary>
        private protected IDictionary<string, double> _inverseFrequencyScores;

        /// <summary>
        /// The topics
        /// </summary>
        private protected string[ ] _topics;

        /// <summary>
        /// The dependency parse
        /// </summary>
        private protected string[ ] _dependencyParse;

        /// <summary>
        /// The word embeddings
        /// </summary>
        private protected IDictionary<string, float[]> _wordEmbeddings;

        /// <summary>
        /// The data
        /// </summary>
        private protected IDictionary<string, object> _data;

        /// <summary>
        /// Initializes a new instance of the <see cref="NlpResult"/> class.
        /// </summary>
        public NlpResult( )
        {
        }

        public NlpResult(string[] tokens, string[] posTag,string[] dependencyParse, 
            IDictionary<string, float[]> wordEmbeddings, string[] topics )
        {
            _topics = topics;
            _dependencyParse = dependencyParse;
            _wordEmbeddings = wordEmbeddings;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NlpResult"/> class.
        /// </summary>
        /// <param name="result">The result.</param>
        public NlpResult( NlpResult result)
        {
            _tokens = result.Tokens;
            _topics = result.Topics;
        }

        /// <summary>
        /// Gets or sets the tokens.
        /// </summary>
        /// <value>
        /// The tokens.
        /// </value>
        public string[] Tokens { get; set; }

        /// <summary>
        /// Gets or sets the position tags.
        /// </summary>
        /// <value>
        /// The position tags.
        /// </value>
        public string[] PosTags { get; set; }

        /// <summary>
        /// Gets or sets the named entities.
        /// </summary>
        /// <value>
        /// The named entities.
        /// </value>
        public string[] NamedEntities { get; set; }

        /// <summary>
        /// Gets or sets the sentiment.
        /// </summary>
        /// <value>
        /// The sentiment.
        /// </value>
        public float Sentiment { get; set; }

        /// <summary>
        /// Gets or sets the word frequencies.
        /// </summary>
        /// <value>
        /// The word frequencies.
        /// </value>
        public IDictionary<string, int> WordFrequencies { get; set; }

        /// <summary>
        /// Gets or sets the inverse frequency scores.
        /// </summary>
        /// <value>
        /// The inverse frequency scores.
        /// </value>
        public IDictionary<string, double> InverseFrequencyScores { get; set; }

        /// <summary>
        /// Gets or sets the topics.
        /// </summary>
        /// <value>
        /// The topics.
        /// </value>
        public string[] Topics { get; set; }

        /// <summary>
        /// Gets or sets the dependency parse.
        /// </summary>
        /// <value>
        /// The dependency parse.
        /// </value>
        public string[] DependencyParse { get; set; }

        /// <summary>
        /// Gets or sets the word embeddings.
        /// </summary>
        /// <value>
        /// The word embeddings.
        /// </value>
        public IDictionary<string, float[]> WordEmbeddings { get; set; }
    }
}
