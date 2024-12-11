

namespace Bubba
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    [ SuppressMessage( "ReSharper", "UnusedType.Global" ) ]
    public abstract class GptRequestBase : GptBase
    {
        /// <summary>
        /// The messages
        /// </summary>
        private protected IList<IGptMessage> _messages;

        /// <summary>
        /// The body
        /// </summary>
        private protected GptBody _body;

        /// <summary>
        /// The header
        /// </summary>
        private protected GptHeader _header;

        /// <summary>
        /// The user prompt
        /// </summary>
        private protected string _userPrompt;

        /// <summary>
        /// The assistant prompt
        /// </summary>
        private protected string _systemPrompt;

        /// <inheritdoc />
        /// <summary>
        /// </summary>
        protected GptRequestBase( ) 
            : base( )
        {
            _header = new GptHeader( );
            _apiKey = _header.ApiKey;
        }

        protected GptRequestBase( string user, string system, string model = "gpt-4o",
            string format = "text", bool store = false, bool stream = false,
            int number = 1, double frequency = 0.0, double presence = 0.0,
            double topPercent = 1.0, double temperature = 1.0, int completionTokens = 157 )
        {

        }

        /// <summary>
        /// Gets or sets the system prompt.
        /// </summary>
        /// <value>
        /// The system prompt.
        /// </value>
        public string SystemPrompt
        {
            get
            {
                return _systemPrompt;
            }
            set
            {
                if( _systemPrompt != value )
                {
                    _systemPrompt = value;
                    OnPropertyChanged( nameof( SystemPrompt ) );
                }
            }
        }

        /// <summary>
        /// Gets or sets the user prompt.
        /// </summary>
        /// <value>
        /// The user prompt.
        /// </value>
        public string UserPrompt
        {
            get
            {
                return _userPrompt;
            }
            set
            {
                if( _userPrompt != value )
                {
                    _userPrompt = value;
                    OnPropertyChanged( nameof( UserPrompt ) );
                }
            }
        }

        /// <inheritdoc />
        /// <summary>
        /// Gets the end point.
        /// </summary>
        /// <value>
        /// The end point.
        /// </value>
        public virtual string EndPoint
        {
            get
            {
                return _endPoint;
            }
            set
            {
                if( _endPoint != value )
                {
                    _endPoint = value;
                    OnPropertyChanged( nameof( EndPoint ) );
                }
            }
        }

        /// <inheritdoc />
        /// <summary>
        /// Gets the chat model.
        /// </summary>
        /// <value>
        /// The chat model.
        /// </value>
        public virtual string Model
        {
            get
            {
                return _model;
            }
            set
            {
                if( _model != value )
                {
                    _model = value;
                    OnPropertyChanged( nameof( Model ) );
                }
            }
        }

        /// <summary>
        /// An string specifying the format that the model must output.
        /// Important: when using JSON mode, you must also instruct the model
        /// to produce JSON yourself via a system or user message. Without this,
        /// the model may generate an unending stream of whitespace until the
        /// generation reaches the token limit, resulting in a
        /// long-running and seemingly "stuck" request.  
        /// </summary>
        /// <value>
        /// The response format.
        /// </value>
        public virtual string ResponseFormat
        {
            get
            {
                return _responseFormat;
            }
            set
            {
                if( _responseFormat != value )
                {
                    _responseFormat = value;
                    OnPropertyChanged( nameof( ResponseFormat ) );
                }
            }
        }

        /// <summary>
        /// THe number 'n' of responses returned by the API.
        /// </summary>
        /// <value>
        /// The user identifier.
        /// </value>
        public virtual int Number
        {
            get
            {
                return _number;
            }
            set
            {
                if( _number != value )
                {
                    _number = value;
                    OnPropertyChanged( nameof( Number ) );
                }
            }
        }

        /// <summary>
        /// Gets the maximum tokens.
        /// </summary>
        /// <value>
        /// The maximum tokens.
        /// </value>
        public virtual int MaximumCompletionTokens
        {
            get
            {
                return _maximumCompletionTokens;
            }
            set
            {
                if( _maximumCompletionTokens != value )
                {
                    _maximumCompletionTokens = value;
                    OnPropertyChanged( nameof( MaximumCompletionTokens ) );
                }
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this
        /// <see cref="GptRequest"/> is store.
        /// </summary>
        /// <value>
        ///   <c>true</c> if store; otherwise, <c>false</c>.
        /// </value>
        public virtual bool Store
        {
            get
            {
                return _store;
            }
            set
            {
                if( _store != value )
                {
                    _store = value;
                    OnPropertyChanged( nameof( Store ) );
                }
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this
        /// <see cref="Stream"/> is stream.
        /// </summary>
        /// <value>
        ///   <c>true</c> if stream; otherwise, <c>false</c>.
        /// </value>
        public virtual bool Stream
        {
            get
            {
                return _stream;
            }
            set
            {
                if( _stream != value )
                {
                    _stream = value;
                    OnPropertyChanged( nameof( Stream ) );
                }
            }
        }

        /// <summary>
        /// A number between -2.0 and 2.0 Positive value decrease the
        /// model's likelihood to repeat the same line verbatim.
        /// </summary>
        /// <value>
        /// The temperature.
        /// </value>
        public virtual double Temperature
        {
            get
            {
                return _temperature;
            }
            set
            {
                if( _temperature != value )
                {
                    _temperature = value;
                    OnPropertyChanged( nameof( Temperature ) );
                }
            }
        }

        /// <summary>
        /// An alternative to sampling with temperature,
        /// called nucleus sampling, where the model considers
        /// the results of the tokens with top_p probability mass.
        /// So 0.1 means only the tokens comprising the top 10% probability
        /// mass are considered. We generally recommend altering this
        /// or temperature but not both.
        /// </summary>
        /// <value>
        /// The top percent.
        /// </value>
        public virtual double TopPercent
        {
            get
            {
                return _topPercent;
            }
            set
            {
                if( _topPercent != value )
                {
                    _topPercent = value;
                    OnPropertyChanged( nameof( TopPercent ) );
                }
            }
        }

        /// <summary>
        /// Number between -2.0 and 2.0. Positive values penalize new tokens
        /// based on whether they appear in the text so far,
        /// ncreasing the model's likelihood to talk about new topics.
        /// </summary>
        /// <value>
        /// The frequency.
        /// </value>
        public virtual double Frequency
        {
            get
            {
                return _frequency;
            }
            set
            {
                if( _frequency != value )
                {
                    _frequency = value;
                    OnPropertyChanged( nameof( Frequency ) );
                }
            }
        }

        /// <summary>
        /// Number between -2.0 and 2.0. Positive values penalize new tokens
        /// based on whether they appear in the text so far,
        /// ncreasing the model's likelihood to talk about new topics.
        /// </summary>
        /// <value>
        /// The presence.
        /// </value>
        public virtual double Presence
        {
            get
            {
                return _presence;
            }
            set
            {
                if( _presence != value )
                {
                    _presence = value;
                    OnPropertyChanged( nameof( Presence ) );
                }
            }
        }
    }
}
