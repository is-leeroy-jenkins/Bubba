

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
    /// <seealso cref="System.IDisposable" />
    public class RagPipe : IDisposable
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
        /// The extractor
        /// </summary>
        private readonly Extractor _extractor;

        /// <summary>
        /// The chunker
        /// </summary>
        private readonly Chunker _chunker;

        /// <summary>
        /// The retriever
        /// </summary>
        private readonly Retriever _retriever;

        /// <summary>
        /// The file request
        /// </summary>
        private readonly GptFileRequest _fileRequest;

        /// <summary>
        /// Initializes a new instance of the <see cref="RagPipe"/> class.
        /// </summary>
        /// <param name="extractor">The extractor.</param>
        /// <param name="chunker">The chunker.</param>
        /// <param name="retriever">The retriever.</param>
        /// <param name="fileRequest">The file request.</param>
        public RagPipe( Extractor extractor, Chunker chunker, 
            Retriever retriever, GptFileRequest fileRequest)
        {
            _extractor = extractor;
            _chunker = chunker;
            _retriever = retriever;
            _fileRequest = fileRequest;
        }

        /// <summary>
        /// Executes the asynchronous.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <param name="pdfDirectory">The PDF directory.</param>
        /// <returns></returns>
        public async Task<string> ExecuteAsync(string query, string pdfDirectory)
        {
            var _texts = _extractor.ExtractText(pdfDirectory);
            var _chunkText = _chunker.ChunkText(_texts);
            var _chunks = _retriever.GetChunks(query, _chunkText);
            return await _fileRequest.GetResponseAsync(query, _chunks);
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
