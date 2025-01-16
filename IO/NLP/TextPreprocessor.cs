

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
    /// <seealso cref="Bubba.ILanguageProcessor" />
    public class TextPreprocessor : ILanguageProcessor
    {
        /// <summary>
        /// The stop words
        /// </summary>
        private static readonly HashSet<string> _stopWords = new HashSet<string>
        {
            "a", "an", "the", "and", "or", "but", "on", "in", "with", "to", "for", "is", "was"
        };

        /// <summary>
        /// Tokenizes the specified text.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <returns></returns>
        public string[] Tokenize(string text)
        {
            return text.Split(new[] { ' ', '.', ',', '!', '?' }, StringSplitOptions.RemoveEmptyEntries);
        }

        /// <summary>
        /// Normalizes the text.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <returns></returns>
        public string NormalizeText(string text)
        {
            return text.ToLowerInvariant().Trim();
        }

        /// <summary>
        /// Removes the stop words.
        /// </summary>
        /// <param name="tokens">The tokens.</param>
        /// <returns></returns>
        public string[] RemoveStopWords(string[] tokens)
        {
            return tokens.Where(token => !_stopWords.Contains(token)).ToArray();
        }

        /// <summary>
        /// Stems the word.
        /// </summary>
        /// <param name="word">The word.</param>
        /// <returns></returns>
        public string StemWord(string word)
        {
            // Basic stemming logic; replace with a robust stemming library as needed.
            if(word.EndsWith("ing"))
                return word.Substring(0, word.Length - 3);
            if(word.EndsWith("ed"))
                return word.Substring(0, word.Length - 2);
            return word;
        }
    }
}
