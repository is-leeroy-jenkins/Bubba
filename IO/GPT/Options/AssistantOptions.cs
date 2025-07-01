

namespace Bubba
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using Properties;

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    /// <seealso cref="T:Bubba.GptOptions" />
    [ SuppressMessage( "ReSharper", "ClassCanBeSealed.Global" ) ]
    [ SuppressMessage( "ReSharper", "PreferConcreteValueOverDefault" ) ]
    [ SuppressMessage( "ReSharper", "PossibleUnintendedReferenceComparison" ) ]
    [ SuppressMessage( "ReSharper", "MemberCanBePrivate.Global" ) ]
    [ SuppressMessage( "ReSharper", "MemberCanBePrivate.Global" ) ]
    public class AssistantOptions : GptOptions
    {
        /// <summary>
        /// The reasoning effort
        /// </summary>
        private protected string _reasoningEffort;

        /// <summary>
        /// The tool choice
        /// </summary>
        private protected string _toolChoice;

        /// <summary>
        /// The tools
        /// </summary>
        private protected IList<string> _tools;

        /// <summary>
        /// The image URL
        /// </summary>
        private protected string _imageUrl;

        /// <summary>
        /// The file
        /// </summary>
        private protected object _file;

        /// <summary>
        /// The file path
        /// </summary>
        private protected string _filePath;

        /// <summary>
        /// The vector store ids
        /// </summary>
        private protected IList<string> _vectorStoreIds;

        /// <summary>
        /// The input
        /// </summary>
        private protected IList<string> _input;

        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="AssistantOptions"/> class.
        /// </summary>
        /// <inheritdoc />
        public AssistantOptions( ) 
            : base( )
        {
            _model = "gpt-4o-mini";
            _endPoint = GptEndPoint.Assistants;
            _instructions = OpenAI.BubbaPrompt;
            _store = true;
            _stream = true;
            _number = 1;
            _temperature = 0.80;
            _topPercent = 0.90;
            _frequencyPenalty = 0.00;
            _presencePenalty = 0.00;
            _maxCompletionTokens = 2048;
            _stop = "['#', ';']";
            _modalities = "['text', 'audio']";
            _responseFormat = "auto";
        }

        /// <summary>
        /// Gets or sets the reasoning effort.
        /// </summary>
        /// <value>
        /// The reasoning effort.
        /// </value>
        public string ReasoningEffort
        {
            get
            {
                return _reasoningEffort;
            }
            set
            {
                if( _reasoningEffort != value )
                {
                    _reasoningEffort = value;
                    OnPropertyChanged( ReasoningEffort );
                }
            }
        }

        /// <summary>
        /// Gets or sets the tool choice.
        /// </summary>
        /// <value>
        /// The tool choice.
        /// </value>
        public string ToolChoice
        {
            get
            {
                return _toolChoice;
            }
            set
            {
                if( _toolChoice != value )
                {
                    _toolChoice = value;
                    OnPropertyChanged( ToolChoice );
                }
            }
        }

        /// <summary>
        /// Gets or sets the tools.
        /// </summary>
        /// <value>
        /// The tools.
        /// </value>
        public IList<string> Tools
        {
            get
            {
                return _tools;
            }
            set
            {
                if( _tools != value )
                {
                    _tools = value;
                    OnPropertyChanged( nameof( Tools ) );
                }
            }
        }

        /// <summary>
        /// Gets or sets the image URL.
        /// </summary>
        /// <value>
        /// The image URL.
        /// </value>
        public string ImageUrl
        {
            get
            {
                return _imageUrl;
            }
            set
            {
                if( _imageUrl != value )
                {
                    _imageUrl = value;
                    OnPropertyChanged( nameof( ImageUrl ) );
                }
            }
        }

        /// <summary>
        /// The instructions parameter gives the model high-level instructions on
        /// how it should behave while generating a response, including tone, goals,
        /// and examples of correct responses. Any instructions provided this way
        /// will take priority over a prompt in the input parameter.
        /// </summary>
        /// <value>
        /// The instructions.
        /// </value>
        public override string Instructions
        {
            get
            {
                return _instructions;
            }
            set
            {
                if( _instructions != value )
                {
                    _instructions = value;
                    OnPropertyChanged( nameof( Instructions ) );
                }
            }
        }

        /// <summary>
        /// Gets or sets the file path.
        /// </summary>
        /// <value>
        /// The file path.
        /// </value>
        public string FilePath
        {
            get
            {
                return _filePath;
            }
            set
            {
                if( _filePath != value )
                {
                    _filePath = value;
                    OnPropertyChanged( nameof( FilePath ) );
                }
            }
        }

        /// <summary>
        /// Gets or sets the file.
        /// </summary>
        /// <value>
        /// The file.
        /// </value>
        public object File
        {
            get
            {
                return _file;
            }
            set
            {
                if( _file != value )
                {
                    _file = value;
                    OnPropertyChanged( nameof( File ) );
                }
            }
        }

        /// <summary>
        /// Gets or sets the vector store ids.
        /// </summary>
        /// <value>
        /// The vector store ids.
        /// </value>
        public IList<string> VectorStoreIds
        {
            get
            {
                return _vectorStoreIds;
            }
            set
            {
                if( _vectorStoreIds != value )
                {
                    _vectorStoreIds = value;
                    OnPropertyChanged( nameof( VectorStoreIds ) );
                }
            }
        }

        /// <summary>
        /// Gets or sets the input.
        /// </summary>
        /// <value>
        /// The input.
        /// </value>
        public IList<string> Input
        {
            get
            {
                return _input;
            }
            set
            {
                if( _input != value )
                {
                    _input = value;
                    OnPropertyChanged( nameof( Input ) );
                }
            }
        }

        /// <inheritdoc />
        /// <summary>
        /// Gets the data.
        /// </summary>
        /// <returns></returns>
        public override IDictionary<string, object> GetData( )
        {
            try
            {
                _data.Add( "Number", _number );
                _data.Add( "MaxCompletionTokens", _maxCompletionTokens );
                _data.Add( "Store", _store );
                _data.Add( "Stream", _stream );
                _data.Add( "Temperature", _temperature );
                _data.Add( "TopPercent", _topPercent );
                _data.Add( "FrequencyPenalty", _frequencyPenalty );
                _data.Add( "PresencePenalty", _presencePenalty );
                _data.Add( "ResponseFormat", _responseFormat );
                _data.Add( "ReasoningEffort", _reasoningEffort );
                _data.Add( "Modalities", _modalities );
                _data.Add( "VectorStoreIds", _vectorStoreIds );
                _data.Add( "Input", _input );
                _data.Add( "ImageUrl", _imageUrl );
                _data.Add( "Tools", _tools );
                _data.Add( "ToolChoice", _toolChoice );
                return _data?.Any( ) == true
                    ? _data
                    : default( IDictionary<string, object> );
            }
            catch( Exception ex )
            {
                Fail( ex );
                return default( IDictionary<string, object> );
            }
        }
    }
}
