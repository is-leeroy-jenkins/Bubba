

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
    public class Chunker : IDisposable
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
        /// The status update
        /// </summary>
        private protected Action _statusUpdate;


        public Chunker( )
        {
        }

        /// <summary>
        /// Begins the initialize.
        /// </summary>
        private protected void Busy()
        {
            try
            {
                lock(_entry)
                {
                    _busy = true;
                }
            }
            catch(Exception ex)
            {
                Fail(ex);
            }
        }

        /// <summary>
        /// Ends the initialize.
        /// </summary>
        private protected void Chill()
        {
            try
            {
                lock(_entry)
                {
                    _busy = false;
                }
            }
            catch(Exception ex)
            {
                Fail( ex);
            }
        }

        /// <summary>
        /// Chunks the text.
        /// </summary>
        /// <param name="texts">The texts.</param>
        /// <param name="maxChunkSize">Maximum size of the chunk.</param>
        /// <returns></returns>
        public List<string> ChunkText( List<string> texts, int maxChunkSize = 500 )
        {
            var _chunks = new List<string>( );
            foreach( var _text in texts )
            {
                var _sentences = _text.Split( new[ ]
                {
                    '.',
                    '!',
                    '?'
                }, StringSplitOptions.RemoveEmptyEntries );

                var _currentChunk = "";
                foreach( var _sentence in _sentences )
                {
                    if( ( _currentChunk + _sentence ).Length > maxChunkSize )
                    {
                        _chunks.Add( _currentChunk );
                        _currentChunk = "";
                    }

                    _currentChunk += _sentence + ". ";
                }

                if( !string.IsNullOrEmpty( _currentChunk ) )
                    _chunks.Add( _currentChunk );
            }

            return _chunks;
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
