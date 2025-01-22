

namespace Bubba
{
    using PdfSharp.Pdf.IO;
    using System.Text;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Threading;

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    /// <seealso cref="T:Bubba.Extractor" />
    public class Extractor : IDisposable
    {
        /// <summary>
        /// The busy
        /// </summary>
        private protected bool _busy;

        /// <summary>
        /// The entry
        /// </summary>
        private protected object _entry = new object( );

        /// <summary>
        /// The timer
        /// </summary>
        private protected Timer _timer;

        /// <summary>
        /// The timer callback
        /// </summary>
        private protected TimerCallback _timerCallback;

        /// <summary>
        /// Initializes a new instance of the <see cref="Extractor"/> class.
        /// </summary>
        public Extractor( )
        {
        }

        /// <summary>
        /// Extracts the text.
        /// </summary>
        /// <param name="directoryPath">The directory path.</param>
        /// <returns></returns>
        public List<string> ExtractText( string directoryPath )
        {
            var _chunks = new List<string>();
            var _files = Directory.GetFiles( directoryPath, "*.pdf");
            foreach( var _file in _files )
            {
                using var _document = PdfReader.Open( _file, PdfDocumentOpenMode.ReadOnly );
                var _text = new StringBuilder( );
                foreach( var _page in _document.Pages )
                {
                    var _content = _page.Contents.Elements;
                    foreach( var _element in _content )
                    {
                        _text.Append( _element );
                    }
                }

                _chunks.Add( _text.ToString( ) );
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
