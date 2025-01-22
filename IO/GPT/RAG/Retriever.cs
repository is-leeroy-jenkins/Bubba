

namespace Bubba
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary>
    /// 
    /// </summary>
    public class Retriever : IDisposable
    {
        /// <summary>
        /// The busy
        /// </summary>
        private protected bool _busy;

        /// <summary>
        /// The entry
        /// </summary>
        private protected object _entry = new object();

        /// <summary>
        /// The timer
        /// </summary>
        private protected Timer _timer;

        /// <summary>
        /// The timer callback
        /// </summary>
        private protected TimerCallback _timerCallback;

        /// <summary>
        /// Initializes a new instance of the <see cref="Retriever"/> class.
        /// </summary>
        public Retriever( )
        {
        }

        /// <summary>
        /// Gets the chunks.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <param name="documentChunks">The document chunks.</param>
        /// <param name="topN">The top n.</param>
        /// <returns></returns>
        public List<string> GetChunks(string query, List<string> documentChunks, int topN = 3)
        {
            var _queryTokens = query.Split(' ', System.StringSplitOptions.RemoveEmptyEntries);
            var _scoredChunks = new List<(string Chunk, double Score)>();
            foreach(var _chunk in documentChunks)
            {
                var _chunkTokens = _chunk.Split(' ', System.StringSplitOptions.RemoveEmptyEntries);
                var _score = CalculateCosineSimilarity(_queryTokens, _chunkTokens);
                _scoredChunks.Add((_chunk, _score));
            }

            return _scoredChunks
                .OrderByDescending(c => c.Score)
                .Take(topN)
                .Select(c => c.Chunk)
                .ToList();
        }

        /// <summary>
        /// Calculates the cosine similarity.
        /// </summary>
        /// <param name="tokensA">The tokens a.</param>
        /// <param name="tokensB">The tokens b.</param>
        /// <returns></returns>
        private double CalculateCosineSimilarity(string[] tokensA, string[] tokensB)
        {
            var _setA = tokensA.Distinct().ToArray();
            var _setB = tokensB.Distinct().ToArray();
            var _intersection = _setA.Intersect(_setB).Count();
            var _magnitudeA = System.Math.Sqrt(_setA.Length);
            var _magnitudeB = System.Math.Sqrt(_setB.Length);
            return _intersection / (_magnitudeA * _magnitudeB);
        }

        /// <inheritdoc />
        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="disposing"><c>true</c>
        /// to release both managed and unmanaged resources;
        /// <c>false</c> to release only unmanaged resources.
        /// </param>
        protected virtual void Dispose(bool disposing)
        {
            if(disposing)
            {
                _timer?.Dispose();
            }
        }

        /// <summary>
        /// Fails the specified ex.
        /// </summary>
        /// <param name="ex">The ex.</param>
        private protected void Fail(Exception ex)
        {
            using var _error = new ErrorWindow(ex);
            _error?.SetText();
            _error?.ShowDialog();
        }
    }
}
