

namespace Bubba
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using AngleSharp;
    using AngleSharp.Dom;

    public class HtmlDocument
    {
        private readonly IDocument _document;

        public HtmlDocument( )
        {
        }

        public HtmlDocument(IDocument document)
        {
            _document = document;
        }

        public HtmlElement Root
        {
            get
            {
                return new HtmlElement( _document.DocumentElement );
            }
        }

        public HtmlElement QuerySelector(string selector)
        {
            return _document.QuerySelector( selector ) is IElement element
                ? new HtmlElement( element )
                : null;
        }

        public IEnumerable<HtmlElement> QuerySelectorAll(string selector)
        {
            return _document.QuerySelectorAll( selector ).Select( e => new HtmlElement( e ) );
        }

        public override string ToString()
        {
            return _document.DocumentElement.OuterHtml;
        }
    }
}
