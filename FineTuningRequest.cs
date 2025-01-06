

namespace Bubba
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Net.Http;

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    /// <seealso cref="T:Bubba.GptRequest" />
    [ SuppressMessage( "ReSharper", "UnusedType.Global" ) ]
    public class FineTuningRequest : GptRequest
    {
        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="FineTuningRequest"/> class.
        /// </summary>
        /// <inheritdoc />
        public FineTuningRequest( ) 
            : base( )
        {
            _entry = new object();
            _httpClient = new HttpClient();
            _presence = 0.00;
            _frequency = 0.00;
            _topPercent = 0.11;
            _temperature = 0.18;
            _maximumTokens = 2048;
            _model = "gpt-4o-mini";
            _endPoint = new GptEndPoint( ).FineTuning;
            _number = 1;
        }
    }
}
