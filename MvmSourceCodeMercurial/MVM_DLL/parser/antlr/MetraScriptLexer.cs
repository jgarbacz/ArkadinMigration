// $ANTLR 3.2 Sep 23, 2009 12:02:23 C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g 2012-03-14 17:58:07

// The variable 'variable' is assigned but its value is never used.
#pragma warning disable 168, 219
// Unreachable code detected.
#pragma warning disable 162


using System;
using Antlr.Runtime;
using IList 		= System.Collections.IList;
using ArrayList 	= System.Collections.ArrayList;
using Stack 		= Antlr.Runtime.Collections.StackList;


public partial class MetraScriptLexer : Lexer {
    public const int FUNCTION = 15;
    public const int FloatTypeSuffix = 27;
    public const int OctalLiteral = 22;
    public const int EOF = -1;
    public const int T__93 = 93;
    public const int T__94 = 94;
    public const int T__91 = 91;
    public const int T__92 = 92;
    public const int T__90 = 90;
    public const int COMMENT = 29;
    public const int T__99 = 99;
    public const int T__98 = 98;
    public const int T__97 = 97;
    public const int T__96 = 96;
    public const int T__95 = 95;
    public const int T__80 = 80;
    public const int T__81 = 81;
    public const int T__82 = 82;
    public const int T__83 = 83;
    public const int LINE_COMMENT = 30;
    public const int IntegerTypeSuffix = 25;
    public const int TEMP = 11;
    public const int NULL = 8;
    public const int BOOL = 7;
    public const int INT = 4;
    public const int T__85 = 85;
    public const int T__84 = 84;
    public const int T__87 = 87;
    public const int T__86 = 86;
    public const int T__89 = 89;
    public const int T__88 = 88;
    public const int T__71 = 71;
    public const int WS = 28;
    public const int T__72 = 72;
    public const int T__70 = 70;
    public const int FloatingPointLiteral = 19;
    public const int NAMED_ARGUMENT = 17;
    public const int T__76 = 76;
    public const int T__75 = 75;
    public const int T__74 = 74;
    public const int T__73 = 73;
    public const int T__79 = 79;
    public const int T__78 = 78;
    public const int T__77 = 77;
    public const int T__68 = 68;
    public const int T__69 = 69;
    public const int T__66 = 66;
    public const int T__67 = 67;
    public const int T__64 = 64;
    public const int T__65 = 65;
    public const int T__62 = 62;
    public const int T__63 = 63;
    public const int Exponent = 26;
    public const int FLOAT = 5;
    public const int T__61 = 61;
    public const int T__60 = 60;
    public const int HexDigit = 24;
    public const int T__55 = 55;
    public const int T__56 = 56;
    public const int T__57 = 57;
    public const int T__58 = 58;
    public const int T__51 = 51;
    public const int T__52 = 52;
    public const int T__53 = 53;
    public const int T__54 = 54;
    public const int OBJECT = 10;
    public const int T__59 = 59;
    public const int ARGUMENT = 16;
    public const int PROC = 13;
    public const int CURRENT_OBJECT = 9;
    public const int T__50 = 50;
    public const int Id = 18;
    public const int T__42 = 42;
    public const int T__43 = 43;
    public const int HexLiteral = 21;
    public const int T__40 = 40;
    public const int THREAD = 14;
    public const int T__41 = 41;
    public const int T__46 = 46;
    public const int T__47 = 47;
    public const int T__44 = 44;
    public const int T__45 = 45;
    public const int T__48 = 48;
    public const int T__49 = 49;
    public const int T__100 = 100;
    public const int DecimalLiteral = 23;
    public const int StringLiteral = 20;
    public const int T__31 = 31;
    public const int T__32 = 32;
    public const int T__33 = 33;
    public const int T__34 = 34;
    public const int T__35 = 35;
    public const int T__36 = 36;
    public const int T__37 = 37;
    public const int T__38 = 38;
    public const int T__39 = 39;
    public const int GLOBAL = 12;
    public const int STRING = 6;

        override public void Recover(RecognitionException re)
        {
            throw re;
        }
        override public void ReportError(RecognitionException e)
        {
            var antlrMsg = base.GetErrorMessage(e, TokenNames);
            var stack = GetRuleInvocationStack(e, this.GetType().FullName);
            throw new MvmScript.MvmScriptRecognitionException(e, stack, antlrMsg);
        }
        public void MissingClosingError(string msg,int startLine,int startPosition,string startText,string endText){
        	throw new MvmScript.MvmScriptMissingClosingException(msg,startLine,startPosition,startText,endText);
        }


    // delegates
    // delegators

    public MetraScriptLexer() 
    {
		InitializeCyclicDFAs();
    }
    public MetraScriptLexer(ICharStream input)
		: this(input, null) {
    }
    public MetraScriptLexer(ICharStream input, RecognizerSharedState state)
		: base(input, state) {
		InitializeCyclicDFAs(); 

    }
    
    override public string GrammarFileName
    {
    	get { return "C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g";} 
    }

    // $ANTLR start "T__31"
    public void mT__31() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = T__31;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:23:7: ( 'OBJECT.' )
            // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:23:9: 'OBJECT.'
            {
            	Match("OBJECT."); 


            }

            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "T__31"

    // $ANTLR start "T__32"
    public void mT__32() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = T__32;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:24:7: ( 'OBJECT(' )
            // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:24:9: 'OBJECT('
            {
            	Match("OBJECT("); 


            }

            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "T__32"

    // $ANTLR start "T__33"
    public void mT__33() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = T__33;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:25:7: ( ').' )
            // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:25:9: ').'
            {
            	Match(")."); 


            }

            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "T__33"

    // $ANTLR start "T__34"
    public void mT__34() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = T__34;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:26:7: ( 'TEMP.' )
            // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:26:9: 'TEMP.'
            {
            	Match("TEMP."); 


            }

            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "T__34"

    // $ANTLR start "T__35"
    public void mT__35() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = T__35;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:27:7: ( 'GLOBAL.' )
            // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:27:9: 'GLOBAL.'
            {
            	Match("GLOBAL."); 


            }

            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "T__35"

    // $ANTLR start "T__36"
    public void mT__36() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = T__36;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:28:7: ( 'THREAD.' )
            // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:28:9: 'THREAD.'
            {
            	Match("THREAD."); 


            }

            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "T__36"

    // $ANTLR start "T__37"
    public void mT__37() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = T__37;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:29:7: ( 'PROC.' )
            // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:29:9: 'PROC.'
            {
            	Match("PROC."); 


            }

            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "T__37"

    // $ANTLR start "T__38"
    public void mT__38() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = T__38;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:30:7: ( 'null' )
            // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:30:9: 'null'
            {
            	Match("null"); 


            }

            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "T__38"

    // $ANTLR start "T__39"
    public void mT__39() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = T__39;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:31:7: ( 'true' )
            // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:31:9: 'true'
            {
            	Match("true"); 


            }

            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "T__39"

    // $ANTLR start "T__40"
    public void mT__40() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = T__40;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:32:7: ( 'false' )
            // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:32:9: 'false'
            {
            	Match("false"); 


            }

            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "T__40"

    // $ANTLR start "T__41"
    public void mT__41() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = T__41;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:33:7: ( '=' )
            // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:33:9: '='
            {
            	Match('='); 

            }

            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "T__41"

    // $ANTLR start "T__42"
    public void mT__42() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = T__42;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:34:7: ( '~' )
            // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:34:9: '~'
            {
            	Match('~'); 

            }

            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "T__42"

    // $ANTLR start "T__43"
    public void mT__43() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = T__43;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:35:7: ( '+=' )
            // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:35:9: '+='
            {
            	Match("+="); 


            }

            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "T__43"

    // $ANTLR start "T__44"
    public void mT__44() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = T__44;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:36:7: ( '-=' )
            // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:36:9: '-='
            {
            	Match("-="); 


            }

            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "T__44"

    // $ANTLR start "T__45"
    public void mT__45() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = T__45;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:37:7: ( '*=' )
            // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:37:9: '*='
            {
            	Match("*="); 


            }

            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "T__45"

    // $ANTLR start "T__46"
    public void mT__46() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = T__46;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:38:7: ( '/=' )
            // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:38:9: '/='
            {
            	Match("/="); 


            }

            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "T__46"

    // $ANTLR start "T__47"
    public void mT__47() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = T__47;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:39:7: ( '&=' )
            // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:39:9: '&='
            {
            	Match("&="); 


            }

            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "T__47"

    // $ANTLR start "T__48"
    public void mT__48() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = T__48;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:40:7: ( '|=' )
            // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:40:9: '|='
            {
            	Match("|="); 


            }

            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "T__48"

    // $ANTLR start "T__49"
    public void mT__49() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = T__49;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:41:7: ( '^=' )
            // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:41:9: '^='
            {
            	Match("^="); 


            }

            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "T__49"

    // $ANTLR start "T__50"
    public void mT__50() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = T__50;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:42:7: ( '%=' )
            // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:42:9: '%='
            {
            	Match("%="); 


            }

            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "T__50"

    // $ANTLR start "T__51"
    public void mT__51() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = T__51;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:43:7: ( '~=' )
            // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:43:9: '~='
            {
            	Match("~="); 


            }

            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "T__51"

    // $ANTLR start "T__52"
    public void mT__52() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = T__52;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:44:7: ( '<' )
            // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:44:9: '<'
            {
            	Match('<'); 

            }

            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "T__52"

    // $ANTLR start "T__53"
    public void mT__53() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = T__53;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:45:7: ( '>' )
            // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:45:9: '>'
            {
            	Match('>'); 

            }

            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "T__53"

    // $ANTLR start "T__54"
    public void mT__54() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = T__54;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:46:7: ( '?' )
            // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:46:9: '?'
            {
            	Match('?'); 

            }

            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "T__54"

    // $ANTLR start "T__55"
    public void mT__55() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = T__55;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:47:7: ( ':' )
            // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:47:9: ':'
            {
            	Match(':'); 

            }

            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "T__55"

    // $ANTLR start "T__56"
    public void mT__56() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = T__56;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:48:7: ( '||' )
            // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:48:9: '||'
            {
            	Match("||"); 


            }

            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "T__56"

    // $ANTLR start "T__57"
    public void mT__57() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = T__57;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:49:7: ( 'or' )
            // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:49:9: 'or'
            {
            	Match("or"); 


            }

            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "T__57"

    // $ANTLR start "T__58"
    public void mT__58() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = T__58;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:50:7: ( 'OR' )
            // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:50:9: 'OR'
            {
            	Match("OR"); 


            }

            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "T__58"

    // $ANTLR start "T__59"
    public void mT__59() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = T__59;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:51:7: ( '&&' )
            // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:51:9: '&&'
            {
            	Match("&&"); 


            }

            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "T__59"

    // $ANTLR start "T__60"
    public void mT__60() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = T__60;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:52:7: ( 'and' )
            // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:52:9: 'and'
            {
            	Match("and"); 


            }

            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "T__60"

    // $ANTLR start "T__61"
    public void mT__61() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = T__61;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:53:7: ( 'AND' )
            // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:53:9: 'AND'
            {
            	Match("AND"); 


            }

            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "T__61"

    // $ANTLR start "T__62"
    public void mT__62() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = T__62;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:54:7: ( '|' )
            // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:54:9: '|'
            {
            	Match('|'); 

            }

            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "T__62"

    // $ANTLR start "T__63"
    public void mT__63() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = T__63;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:55:7: ( '^' )
            // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:55:9: '^'
            {
            	Match('^'); 

            }

            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "T__63"

    // $ANTLR start "T__64"
    public void mT__64() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = T__64;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:56:7: ( '&' )
            // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:56:9: '&'
            {
            	Match('&'); 

            }

            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "T__64"

    // $ANTLR start "T__65"
    public void mT__65() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = T__65;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:57:7: ( '==' )
            // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:57:9: '=='
            {
            	Match("=="); 


            }

            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "T__65"

    // $ANTLR start "T__66"
    public void mT__66() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = T__66;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:58:7: ( '!=' )
            // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:58:9: '!='
            {
            	Match("!="); 


            }

            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "T__66"

    // $ANTLR start "T__67"
    public void mT__67() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = T__67;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:59:7: ( 'eq' )
            // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:59:9: 'eq'
            {
            	Match("eq"); 


            }

            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "T__67"

    // $ANTLR start "T__68"
    public void mT__68() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = T__68;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:60:7: ( 'ne' )
            // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:60:9: 'ne'
            {
            	Match("ne"); 


            }

            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "T__68"

    // $ANTLR start "T__69"
    public void mT__69() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = T__69;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:61:7: ( 'Eq' )
            // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:61:9: 'Eq'
            {
            	Match("Eq"); 


            }

            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "T__69"

    // $ANTLR start "T__70"
    public void mT__70() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = T__70;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:62:7: ( 'Ne' )
            // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:62:9: 'Ne'
            {
            	Match("Ne"); 


            }

            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "T__70"

    // $ANTLR start "T__71"
    public void mT__71() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = T__71;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:63:7: ( 'EQ' )
            // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:63:9: 'EQ'
            {
            	Match("EQ"); 


            }

            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "T__71"

    // $ANTLR start "T__72"
    public void mT__72() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = T__72;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:64:7: ( 'NE' )
            // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:64:9: 'NE'
            {
            	Match("NE"); 


            }

            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "T__72"

    // $ANTLR start "T__73"
    public void mT__73() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = T__73;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:65:7: ( 'eqEQ' )
            // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:65:9: 'eqEQ'
            {
            	Match("eqEQ"); 


            }

            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "T__73"

    // $ANTLR start "T__74"
    public void mT__74() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = T__74;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:66:7: ( 'EqEQ' )
            // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:66:9: 'EqEQ'
            {
            	Match("EqEQ"); 


            }

            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "T__74"

    // $ANTLR start "T__75"
    public void mT__75() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = T__75;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:67:7: ( 'neNE' )
            // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:67:9: 'neNE'
            {
            	Match("neNE"); 


            }

            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "T__75"

    // $ANTLR start "T__76"
    public void mT__76() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = T__76;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:68:7: ( 'NeNE' )
            // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:68:9: 'NeNE'
            {
            	Match("NeNE"); 


            }

            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "T__76"

    // $ANTLR start "T__77"
    public void mT__77() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = T__77;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:69:7: ( 'gt' )
            // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:69:9: 'gt'
            {
            	Match("gt"); 


            }

            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "T__77"

    // $ANTLR start "T__78"
    public void mT__78() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = T__78;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:70:7: ( 'lt' )
            // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:70:9: 'lt'
            {
            	Match("lt"); 


            }

            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "T__78"

    // $ANTLR start "T__79"
    public void mT__79() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = T__79;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:71:7: ( 'gte' )
            // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:71:9: 'gte'
            {
            	Match("gte"); 


            }

            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "T__79"

    // $ANTLR start "T__80"
    public void mT__80() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = T__80;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:72:7: ( 'lte' )
            // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:72:9: 'lte'
            {
            	Match("lte"); 


            }

            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "T__80"

    // $ANTLR start "T__81"
    public void mT__81() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = T__81;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:73:7: ( 'Gt' )
            // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:73:9: 'Gt'
            {
            	Match("Gt"); 


            }

            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "T__81"

    // $ANTLR start "T__82"
    public void mT__82() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = T__82;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:74:7: ( 'Lt' )
            // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:74:9: 'Lt'
            {
            	Match("Lt"); 


            }

            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "T__82"

    // $ANTLR start "T__83"
    public void mT__83() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = T__83;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:75:7: ( 'Gte' )
            // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:75:9: 'Gte'
            {
            	Match("Gte"); 


            }

            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "T__83"

    // $ANTLR start "T__84"
    public void mT__84() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = T__84;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:76:7: ( 'Lte' )
            // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:76:9: 'Lte'
            {
            	Match("Lte"); 


            }

            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "T__84"

    // $ANTLR start "T__85"
    public void mT__85() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = T__85;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:77:7: ( 'GT' )
            // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:77:9: 'GT'
            {
            	Match("GT"); 


            }

            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "T__85"

    // $ANTLR start "T__86"
    public void mT__86() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = T__86;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:78:7: ( 'LT' )
            // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:78:9: 'LT'
            {
            	Match("LT"); 


            }

            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "T__86"

    // $ANTLR start "T__87"
    public void mT__87() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = T__87;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:79:7: ( 'GTE' )
            // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:79:9: 'GTE'
            {
            	Match("GTE"); 


            }

            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "T__87"

    // $ANTLR start "T__88"
    public void mT__88() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = T__88;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:80:7: ( 'LTE' )
            // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:80:9: 'LTE'
            {
            	Match("LTE"); 


            }

            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "T__88"

    // $ANTLR start "T__89"
    public void mT__89() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = T__89;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:81:7: ( '+' )
            // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:81:9: '+'
            {
            	Match('+'); 

            }

            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "T__89"

    // $ANTLR start "T__90"
    public void mT__90() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = T__90;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:82:7: ( '-' )
            // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:82:9: '-'
            {
            	Match('-'); 

            }

            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "T__90"

    // $ANTLR start "T__91"
    public void mT__91() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = T__91;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:83:7: ( '*' )
            // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:83:9: '*'
            {
            	Match('*'); 

            }

            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "T__91"

    // $ANTLR start "T__92"
    public void mT__92() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = T__92;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:84:7: ( '/' )
            // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:84:9: '/'
            {
            	Match('/'); 

            }

            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "T__92"

    // $ANTLR start "T__93"
    public void mT__93() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = T__93;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:85:7: ( '%' )
            // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:85:9: '%'
            {
            	Match('%'); 

            }

            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "T__93"

    // $ANTLR start "T__94"
    public void mT__94() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = T__94;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:86:7: ( '++' )
            // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:86:9: '++'
            {
            	Match("++"); 


            }

            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "T__94"

    // $ANTLR start "T__95"
    public void mT__95() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = T__95;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:87:7: ( '--' )
            // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:87:9: '--'
            {
            	Match("--"); 


            }

            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "T__95"

    // $ANTLR start "T__96"
    public void mT__96() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = T__96;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:88:7: ( '!' )
            // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:88:9: '!'
            {
            	Match('!'); 

            }

            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "T__96"

    // $ANTLR start "T__97"
    public void mT__97() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = T__97;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:89:7: ( '(' )
            // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:89:9: '('
            {
            	Match('('); 

            }

            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "T__97"

    // $ANTLR start "T__98"
    public void mT__98() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = T__98;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:90:7: ( ')' )
            // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:90:9: ')'
            {
            	Match(')'); 

            }

            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "T__98"

    // $ANTLR start "T__99"
    public void mT__99() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = T__99;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:91:7: ( '=>' )
            // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:91:9: '=>'
            {
            	Match("=>"); 


            }

            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "T__99"

    // $ANTLR start "T__100"
    public void mT__100() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = T__100;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:92:8: ( ',' )
            // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:92:10: ','
            {
            	Match(','); 

            }

            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "T__100"

    // $ANTLR start "Id"
    public void mId() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = Id;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:347:2: ( ( 'a' .. 'z' | 'A' .. 'Z' | '_' ) ( 'a' .. 'z' | 'A' .. 'Z' | '0' .. '9' | '_' )* )
            // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:347:4: ( 'a' .. 'z' | 'A' .. 'Z' | '_' ) ( 'a' .. 'z' | 'A' .. 'Z' | '0' .. '9' | '_' )*
            {
            	if ( (input.LA(1) >= 'A' && input.LA(1) <= 'Z') || input.LA(1) == '_' || (input.LA(1) >= 'a' && input.LA(1) <= 'z') ) 
            	{
            	    input.Consume();

            	}
            	else 
            	{
            	    MismatchedSetException mse = new MismatchedSetException(null,input);
            	    Recover(mse);
            	    throw mse;}

            	// C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:347:28: ( 'a' .. 'z' | 'A' .. 'Z' | '0' .. '9' | '_' )*
            	do 
            	{
            	    int alt1 = 2;
            	    int LA1_0 = input.LA(1);

            	    if ( ((LA1_0 >= '0' && LA1_0 <= '9') || (LA1_0 >= 'A' && LA1_0 <= 'Z') || LA1_0 == '_' || (LA1_0 >= 'a' && LA1_0 <= 'z')) )
            	    {
            	        alt1 = 1;
            	    }


            	    switch (alt1) 
            		{
            			case 1 :
            			    // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:
            			    {
            			    	if ( (input.LA(1) >= '0' && input.LA(1) <= '9') || (input.LA(1) >= 'A' && input.LA(1) <= 'Z') || input.LA(1) == '_' || (input.LA(1) >= 'a' && input.LA(1) <= 'z') ) 
            			    	{
            			    	    input.Consume();

            			    	}
            			    	else 
            			    	{
            			    	    MismatchedSetException mse = new MismatchedSetException(null,input);
            			    	    Recover(mse);
            			    	    throw mse;}


            			    }
            			    break;

            			default:
            			    goto loop1;
            	    }
            	} while (true);

            	loop1:
            		;	// Stops C# compiler whining that label 'loop1' has no statements


            }

            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "Id"

    // $ANTLR start "HexLiteral"
    public void mHexLiteral() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = HexLiteral;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:350:2: ( '0' ( 'x' | 'X' ) ( HexDigit )+ ( IntegerTypeSuffix )? )
            // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:350:4: '0' ( 'x' | 'X' ) ( HexDigit )+ ( IntegerTypeSuffix )?
            {
            	Match('0'); 
            	if ( input.LA(1) == 'X' || input.LA(1) == 'x' ) 
            	{
            	    input.Consume();

            	}
            	else 
            	{
            	    MismatchedSetException mse = new MismatchedSetException(null,input);
            	    Recover(mse);
            	    throw mse;}

            	// C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:350:18: ( HexDigit )+
            	int cnt2 = 0;
            	do 
            	{
            	    int alt2 = 2;
            	    int LA2_0 = input.LA(1);

            	    if ( ((LA2_0 >= '0' && LA2_0 <= '9') || (LA2_0 >= 'A' && LA2_0 <= 'F') || (LA2_0 >= 'a' && LA2_0 <= 'f')) )
            	    {
            	        alt2 = 1;
            	    }


            	    switch (alt2) 
            		{
            			case 1 :
            			    // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:350:18: HexDigit
            			    {
            			    	mHexDigit(); 

            			    }
            			    break;

            			default:
            			    if ( cnt2 >= 1 ) goto loop2;
            		            EarlyExitException eee2 =
            		                new EarlyExitException(2, input);
            		            throw eee2;
            	    }
            	    cnt2++;
            	} while (true);

            	loop2:
            		;	// Stops C# compiler whining that label 'loop2' has no statements

            	// C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:350:28: ( IntegerTypeSuffix )?
            	int alt3 = 2;
            	int LA3_0 = input.LA(1);

            	if ( (LA3_0 == 'L' || LA3_0 == 'l') )
            	{
            	    alt3 = 1;
            	}
            	switch (alt3) 
            	{
            	    case 1 :
            	        // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:350:28: IntegerTypeSuffix
            	        {
            	        	mIntegerTypeSuffix(); 

            	        }
            	        break;

            	}


            }

            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "HexLiteral"

    // $ANTLR start "DecimalLiteral"
    public void mDecimalLiteral() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = DecimalLiteral;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:353:2: ( ( '0' | '1' .. '9' ( '0' .. '9' )* ) ( IntegerTypeSuffix )? )
            // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:354:2: ( '0' | '1' .. '9' ( '0' .. '9' )* ) ( IntegerTypeSuffix )?
            {
            	// C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:354:2: ( '0' | '1' .. '9' ( '0' .. '9' )* )
            	int alt5 = 2;
            	int LA5_0 = input.LA(1);

            	if ( (LA5_0 == '0') )
            	{
            	    alt5 = 1;
            	}
            	else if ( ((LA5_0 >= '1' && LA5_0 <= '9')) )
            	{
            	    alt5 = 2;
            	}
            	else 
            	{
            	    NoViableAltException nvae_d5s0 =
            	        new NoViableAltException("", 5, 0, input);

            	    throw nvae_d5s0;
            	}
            	switch (alt5) 
            	{
            	    case 1 :
            	        // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:354:3: '0'
            	        {
            	        	Match('0'); 

            	        }
            	        break;
            	    case 2 :
            	        // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:354:9: '1' .. '9' ( '0' .. '9' )*
            	        {
            	        	MatchRange('1','9'); 
            	        	// C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:354:18: ( '0' .. '9' )*
            	        	do 
            	        	{
            	        	    int alt4 = 2;
            	        	    int LA4_0 = input.LA(1);

            	        	    if ( ((LA4_0 >= '0' && LA4_0 <= '9')) )
            	        	    {
            	        	        alt4 = 1;
            	        	    }


            	        	    switch (alt4) 
            	        		{
            	        			case 1 :
            	        			    // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:354:18: '0' .. '9'
            	        			    {
            	        			    	MatchRange('0','9'); 

            	        			    }
            	        			    break;

            	        			default:
            	        			    goto loop4;
            	        	    }
            	        	} while (true);

            	        	loop4:
            	        		;	// Stops C# compiler whining that label 'loop4' has no statements


            	        }
            	        break;

            	}

            	// C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:354:29: ( IntegerTypeSuffix )?
            	int alt6 = 2;
            	int LA6_0 = input.LA(1);

            	if ( (LA6_0 == 'L' || LA6_0 == 'l') )
            	{
            	    alt6 = 1;
            	}
            	switch (alt6) 
            	{
            	    case 1 :
            	        // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:354:29: IntegerTypeSuffix
            	        {
            	        	mIntegerTypeSuffix(); 

            	        }
            	        break;

            	}


            }

            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "DecimalLiteral"

    // $ANTLR start "OctalLiteral"
    public void mOctalLiteral() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = OctalLiteral;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:357:2: ( '0' ( '0' .. '7' )+ ( IntegerTypeSuffix )? )
            // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:357:4: '0' ( '0' .. '7' )+ ( IntegerTypeSuffix )?
            {
            	Match('0'); 
            	// C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:357:8: ( '0' .. '7' )+
            	int cnt7 = 0;
            	do 
            	{
            	    int alt7 = 2;
            	    int LA7_0 = input.LA(1);

            	    if ( ((LA7_0 >= '0' && LA7_0 <= '7')) )
            	    {
            	        alt7 = 1;
            	    }


            	    switch (alt7) 
            		{
            			case 1 :
            			    // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:357:9: '0' .. '7'
            			    {
            			    	MatchRange('0','7'); 

            			    }
            			    break;

            			default:
            			    if ( cnt7 >= 1 ) goto loop7;
            		            EarlyExitException eee7 =
            		                new EarlyExitException(7, input);
            		            throw eee7;
            	    }
            	    cnt7++;
            	} while (true);

            	loop7:
            		;	// Stops C# compiler whining that label 'loop7' has no statements

            	// C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:357:20: ( IntegerTypeSuffix )?
            	int alt8 = 2;
            	int LA8_0 = input.LA(1);

            	if ( (LA8_0 == 'L' || LA8_0 == 'l') )
            	{
            	    alt8 = 1;
            	}
            	switch (alt8) 
            	{
            	    case 1 :
            	        // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:357:20: IntegerTypeSuffix
            	        {
            	        	mIntegerTypeSuffix(); 

            	        }
            	        break;

            	}


            }

            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "OctalLiteral"

    // $ANTLR start "HexDigit"
    public void mHexDigit() // throws RecognitionException [2]
    {
    		try
    		{
            // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:361:3: ( ( '0' .. '9' | 'a' .. 'f' | 'A' .. 'F' ) )
            // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:361:5: ( '0' .. '9' | 'a' .. 'f' | 'A' .. 'F' )
            {
            	if ( (input.LA(1) >= '0' && input.LA(1) <= '9') || (input.LA(1) >= 'A' && input.LA(1) <= 'F') || (input.LA(1) >= 'a' && input.LA(1) <= 'f') ) 
            	{
            	    input.Consume();

            	}
            	else 
            	{
            	    MismatchedSetException mse = new MismatchedSetException(null,input);
            	    Recover(mse);
            	    throw mse;}


            }

        }
        finally 
    	{
        }
    }
    // $ANTLR end "HexDigit"

    // $ANTLR start "IntegerTypeSuffix"
    public void mIntegerTypeSuffix() // throws RecognitionException [2]
    {
    		try
    		{
            // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:365:2: ( ( 'l' | 'L' ) )
            // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:365:4: ( 'l' | 'L' )
            {
            	if ( input.LA(1) == 'L' || input.LA(1) == 'l' ) 
            	{
            	    input.Consume();

            	}
            	else 
            	{
            	    MismatchedSetException mse = new MismatchedSetException(null,input);
            	    Recover(mse);
            	    throw mse;}


            }

        }
        finally 
    	{
        }
    }
    // $ANTLR end "IntegerTypeSuffix"

    // $ANTLR start "FloatingPointLiteral"
    public void mFloatingPointLiteral() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = FloatingPointLiteral;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:368:2: ( ( '0' .. '9' )+ '.' ( '0' .. '9' )* ( Exponent )? ( FloatTypeSuffix )? | '.' ( '0' .. '9' )+ ( Exponent )? ( FloatTypeSuffix )? | ( '0' .. '9' )+ Exponent ( FloatTypeSuffix )? | ( '0' .. '9' )+ FloatTypeSuffix )
            int alt19 = 4;
            alt19 = dfa19.Predict(input);
            switch (alt19) 
            {
                case 1 :
                    // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:368:4: ( '0' .. '9' )+ '.' ( '0' .. '9' )* ( Exponent )? ( FloatTypeSuffix )?
                    {
                    	// C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:368:4: ( '0' .. '9' )+
                    	int cnt9 = 0;
                    	do 
                    	{
                    	    int alt9 = 2;
                    	    int LA9_0 = input.LA(1);

                    	    if ( ((LA9_0 >= '0' && LA9_0 <= '9')) )
                    	    {
                    	        alt9 = 1;
                    	    }


                    	    switch (alt9) 
                    		{
                    			case 1 :
                    			    // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:368:5: '0' .. '9'
                    			    {
                    			    	MatchRange('0','9'); 

                    			    }
                    			    break;

                    			default:
                    			    if ( cnt9 >= 1 ) goto loop9;
                    		            EarlyExitException eee9 =
                    		                new EarlyExitException(9, input);
                    		            throw eee9;
                    	    }
                    	    cnt9++;
                    	} while (true);

                    	loop9:
                    		;	// Stops C# compiler whining that label 'loop9' has no statements

                    	Match('.'); 
                    	// C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:368:20: ( '0' .. '9' )*
                    	do 
                    	{
                    	    int alt10 = 2;
                    	    int LA10_0 = input.LA(1);

                    	    if ( ((LA10_0 >= '0' && LA10_0 <= '9')) )
                    	    {
                    	        alt10 = 1;
                    	    }


                    	    switch (alt10) 
                    		{
                    			case 1 :
                    			    // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:368:21: '0' .. '9'
                    			    {
                    			    	MatchRange('0','9'); 

                    			    }
                    			    break;

                    			default:
                    			    goto loop10;
                    	    }
                    	} while (true);

                    	loop10:
                    		;	// Stops C# compiler whining that label 'loop10' has no statements

                    	// C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:368:32: ( Exponent )?
                    	int alt11 = 2;
                    	int LA11_0 = input.LA(1);

                    	if ( (LA11_0 == 'E' || LA11_0 == 'e') )
                    	{
                    	    alt11 = 1;
                    	}
                    	switch (alt11) 
                    	{
                    	    case 1 :
                    	        // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:368:32: Exponent
                    	        {
                    	        	mExponent(); 

                    	        }
                    	        break;

                    	}

                    	// C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:368:42: ( FloatTypeSuffix )?
                    	int alt12 = 2;
                    	int LA12_0 = input.LA(1);

                    	if ( (LA12_0 == 'D' || LA12_0 == 'F' || LA12_0 == 'd' || LA12_0 == 'f') )
                    	{
                    	    alt12 = 1;
                    	}
                    	switch (alt12) 
                    	{
                    	    case 1 :
                    	        // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:368:42: FloatTypeSuffix
                    	        {
                    	        	mFloatTypeSuffix(); 

                    	        }
                    	        break;

                    	}


                    }
                    break;
                case 2 :
                    // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:369:4: '.' ( '0' .. '9' )+ ( Exponent )? ( FloatTypeSuffix )?
                    {
                    	Match('.'); 
                    	// C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:369:8: ( '0' .. '9' )+
                    	int cnt13 = 0;
                    	do 
                    	{
                    	    int alt13 = 2;
                    	    int LA13_0 = input.LA(1);

                    	    if ( ((LA13_0 >= '0' && LA13_0 <= '9')) )
                    	    {
                    	        alt13 = 1;
                    	    }


                    	    switch (alt13) 
                    		{
                    			case 1 :
                    			    // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:369:9: '0' .. '9'
                    			    {
                    			    	MatchRange('0','9'); 

                    			    }
                    			    break;

                    			default:
                    			    if ( cnt13 >= 1 ) goto loop13;
                    		            EarlyExitException eee13 =
                    		                new EarlyExitException(13, input);
                    		            throw eee13;
                    	    }
                    	    cnt13++;
                    	} while (true);

                    	loop13:
                    		;	// Stops C# compiler whining that label 'loop13' has no statements

                    	// C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:369:20: ( Exponent )?
                    	int alt14 = 2;
                    	int LA14_0 = input.LA(1);

                    	if ( (LA14_0 == 'E' || LA14_0 == 'e') )
                    	{
                    	    alt14 = 1;
                    	}
                    	switch (alt14) 
                    	{
                    	    case 1 :
                    	        // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:369:20: Exponent
                    	        {
                    	        	mExponent(); 

                    	        }
                    	        break;

                    	}

                    	// C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:369:30: ( FloatTypeSuffix )?
                    	int alt15 = 2;
                    	int LA15_0 = input.LA(1);

                    	if ( (LA15_0 == 'D' || LA15_0 == 'F' || LA15_0 == 'd' || LA15_0 == 'f') )
                    	{
                    	    alt15 = 1;
                    	}
                    	switch (alt15) 
                    	{
                    	    case 1 :
                    	        // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:369:30: FloatTypeSuffix
                    	        {
                    	        	mFloatTypeSuffix(); 

                    	        }
                    	        break;

                    	}


                    }
                    break;
                case 3 :
                    // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:370:4: ( '0' .. '9' )+ Exponent ( FloatTypeSuffix )?
                    {
                    	// C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:370:4: ( '0' .. '9' )+
                    	int cnt16 = 0;
                    	do 
                    	{
                    	    int alt16 = 2;
                    	    int LA16_0 = input.LA(1);

                    	    if ( ((LA16_0 >= '0' && LA16_0 <= '9')) )
                    	    {
                    	        alt16 = 1;
                    	    }


                    	    switch (alt16) 
                    		{
                    			case 1 :
                    			    // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:370:5: '0' .. '9'
                    			    {
                    			    	MatchRange('0','9'); 

                    			    }
                    			    break;

                    			default:
                    			    if ( cnt16 >= 1 ) goto loop16;
                    		            EarlyExitException eee16 =
                    		                new EarlyExitException(16, input);
                    		            throw eee16;
                    	    }
                    	    cnt16++;
                    	} while (true);

                    	loop16:
                    		;	// Stops C# compiler whining that label 'loop16' has no statements

                    	mExponent(); 
                    	// C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:370:25: ( FloatTypeSuffix )?
                    	int alt17 = 2;
                    	int LA17_0 = input.LA(1);

                    	if ( (LA17_0 == 'D' || LA17_0 == 'F' || LA17_0 == 'd' || LA17_0 == 'f') )
                    	{
                    	    alt17 = 1;
                    	}
                    	switch (alt17) 
                    	{
                    	    case 1 :
                    	        // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:370:25: FloatTypeSuffix
                    	        {
                    	        	mFloatTypeSuffix(); 

                    	        }
                    	        break;

                    	}


                    }
                    break;
                case 4 :
                    // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:371:4: ( '0' .. '9' )+ FloatTypeSuffix
                    {
                    	// C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:371:4: ( '0' .. '9' )+
                    	int cnt18 = 0;
                    	do 
                    	{
                    	    int alt18 = 2;
                    	    int LA18_0 = input.LA(1);

                    	    if ( ((LA18_0 >= '0' && LA18_0 <= '9')) )
                    	    {
                    	        alt18 = 1;
                    	    }


                    	    switch (alt18) 
                    		{
                    			case 1 :
                    			    // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:371:5: '0' .. '9'
                    			    {
                    			    	MatchRange('0','9'); 

                    			    }
                    			    break;

                    			default:
                    			    if ( cnt18 >= 1 ) goto loop18;
                    		            EarlyExitException eee18 =
                    		                new EarlyExitException(18, input);
                    		            throw eee18;
                    	    }
                    	    cnt18++;
                    	} while (true);

                    	loop18:
                    		;	// Stops C# compiler whining that label 'loop18' has no statements

                    	mFloatTypeSuffix(); 

                    }
                    break;

            }
            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "FloatingPointLiteral"

    // $ANTLR start "Exponent"
    public void mExponent() // throws RecognitionException [2]
    {
    		try
    		{
            // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:375:2: ( ( 'e' | 'E' ) ( '+' | '-' )? ( '0' .. '9' )+ )
            // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:375:4: ( 'e' | 'E' ) ( '+' | '-' )? ( '0' .. '9' )+
            {
            	if ( input.LA(1) == 'E' || input.LA(1) == 'e' ) 
            	{
            	    input.Consume();

            	}
            	else 
            	{
            	    MismatchedSetException mse = new MismatchedSetException(null,input);
            	    Recover(mse);
            	    throw mse;}

            	// C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:375:14: ( '+' | '-' )?
            	int alt20 = 2;
            	int LA20_0 = input.LA(1);

            	if ( (LA20_0 == '+' || LA20_0 == '-') )
            	{
            	    alt20 = 1;
            	}
            	switch (alt20) 
            	{
            	    case 1 :
            	        // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:
            	        {
            	        	if ( input.LA(1) == '+' || input.LA(1) == '-' ) 
            	        	{
            	        	    input.Consume();

            	        	}
            	        	else 
            	        	{
            	        	    MismatchedSetException mse = new MismatchedSetException(null,input);
            	        	    Recover(mse);
            	        	    throw mse;}


            	        }
            	        break;

            	}

            	// C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:375:25: ( '0' .. '9' )+
            	int cnt21 = 0;
            	do 
            	{
            	    int alt21 = 2;
            	    int LA21_0 = input.LA(1);

            	    if ( ((LA21_0 >= '0' && LA21_0 <= '9')) )
            	    {
            	        alt21 = 1;
            	    }


            	    switch (alt21) 
            		{
            			case 1 :
            			    // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:375:26: '0' .. '9'
            			    {
            			    	MatchRange('0','9'); 

            			    }
            			    break;

            			default:
            			    if ( cnt21 >= 1 ) goto loop21;
            		            EarlyExitException eee21 =
            		                new EarlyExitException(21, input);
            		            throw eee21;
            	    }
            	    cnt21++;
            	} while (true);

            	loop21:
            		;	// Stops C# compiler whining that label 'loop21' has no statements


            }

        }
        finally 
    	{
        }
    }
    // $ANTLR end "Exponent"

    // $ANTLR start "FloatTypeSuffix"
    public void mFloatTypeSuffix() // throws RecognitionException [2]
    {
    		try
    		{
            // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:379:2: ( ( 'f' | 'F' | 'd' | 'D' ) )
            // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:379:4: ( 'f' | 'F' | 'd' | 'D' )
            {
            	if ( input.LA(1) == 'D' || input.LA(1) == 'F' || input.LA(1) == 'd' || input.LA(1) == 'f' ) 
            	{
            	    input.Consume();

            	}
            	else 
            	{
            	    MismatchedSetException mse = new MismatchedSetException(null,input);
            	    Recover(mse);
            	    throw mse;}


            }

        }
        finally 
    	{
        }
    }
    // $ANTLR end "FloatTypeSuffix"

    // $ANTLR start "StringLiteral"
    public void mStringLiteral() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = StringLiteral;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:383:1: ( '\"' (~ ( '\"' ) )* '\"' | '\\'' (~ ( '\\'' ) )* '\\'' )
            int alt24 = 2;
            int LA24_0 = input.LA(1);

            if ( (LA24_0 == '\"') )
            {
                alt24 = 1;
            }
            else if ( (LA24_0 == '\'') )
            {
                alt24 = 2;
            }
            else 
            {
                NoViableAltException nvae_d24s0 =
                    new NoViableAltException("", 24, 0, input);

                throw nvae_d24s0;
            }
            switch (alt24) 
            {
                case 1 :
                    // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:383:3: '\"' (~ ( '\"' ) )* '\"'
                    {
                    	Match('\"'); 
                    	// C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:383:7: (~ ( '\"' ) )*
                    	do 
                    	{
                    	    int alt22 = 2;
                    	    int LA22_0 = input.LA(1);

                    	    if ( ((LA22_0 >= '\u0000' && LA22_0 <= '!') || (LA22_0 >= '#' && LA22_0 <= '\uFFFF')) )
                    	    {
                    	        alt22 = 1;
                    	    }


                    	    switch (alt22) 
                    		{
                    			case 1 :
                    			    // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:383:9: ~ ( '\"' )
                    			    {
                    			    	if ( (input.LA(1) >= '\u0000' && input.LA(1) <= '!') || (input.LA(1) >= '#' && input.LA(1) <= '\uFFFF') ) 
                    			    	{
                    			    	    input.Consume();

                    			    	}
                    			    	else 
                    			    	{
                    			    	    MismatchedSetException mse = new MismatchedSetException(null,input);
                    			    	    Recover(mse);
                    			    	    throw mse;}


                    			    }
                    			    break;

                    			default:
                    			    goto loop22;
                    	    }
                    	} while (true);

                    	loop22:
                    		;	// Stops C# compiler whining that label 'loop22' has no statements

                    	Match('\"'); 

                    }
                    break;
                case 2 :
                    // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:384:3: '\\'' (~ ( '\\'' ) )* '\\''
                    {
                    	Match('\''); 
                    	// C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:384:8: (~ ( '\\'' ) )*
                    	do 
                    	{
                    	    int alt23 = 2;
                    	    int LA23_0 = input.LA(1);

                    	    if ( ((LA23_0 >= '\u0000' && LA23_0 <= '&') || (LA23_0 >= '(' && LA23_0 <= '\uFFFF')) )
                    	    {
                    	        alt23 = 1;
                    	    }


                    	    switch (alt23) 
                    		{
                    			case 1 :
                    			    // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:384:10: ~ ( '\\'' )
                    			    {
                    			    	if ( (input.LA(1) >= '\u0000' && input.LA(1) <= '&') || (input.LA(1) >= '(' && input.LA(1) <= '\uFFFF') ) 
                    			    	{
                    			    	    input.Consume();

                    			    	}
                    			    	else 
                    			    	{
                    			    	    MismatchedSetException mse = new MismatchedSetException(null,input);
                    			    	    Recover(mse);
                    			    	    throw mse;}


                    			    }
                    			    break;

                    			default:
                    			    goto loop23;
                    	    }
                    	} while (true);

                    	loop23:
                    		;	// Stops C# compiler whining that label 'loop23' has no statements

                    	Match('\''); 

                    }
                    break;

            }
            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "StringLiteral"

    // $ANTLR start "WS"
    public void mWS() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = WS;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:413:2: ( ( ' ' | '\\r' | '\\t' | '\\u000C' | '\\n' ) )
            // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:413:5: ( ' ' | '\\r' | '\\t' | '\\u000C' | '\\n' )
            {
            	if ( (input.LA(1) >= '\t' && input.LA(1) <= '\n') || (input.LA(1) >= '\f' && input.LA(1) <= '\r') || input.LA(1) == ' ' ) 
            	{
            	    input.Consume();

            	}
            	else 
            	{
            	    MismatchedSetException mse = new MismatchedSetException(null,input);
            	    Recover(mse);
            	    throw mse;}

            	_channel=HIDDEN;

            }

            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "WS"

    // $ANTLR start "COMMENT"
    public void mCOMMENT() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = COMMENT;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:416:2: ( '/*' ( options {greedy=false; } : . )* '*/' )
            // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:416:4: '/*' ( options {greedy=false; } : . )* '*/'
            {
            	Match("/*"); 

            	// C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:416:9: ( options {greedy=false; } : . )*
            	do 
            	{
            	    int alt25 = 2;
            	    int LA25_0 = input.LA(1);

            	    if ( (LA25_0 == '*') )
            	    {
            	        int LA25_1 = input.LA(2);

            	        if ( (LA25_1 == '/') )
            	        {
            	            alt25 = 2;
            	        }
            	        else if ( ((LA25_1 >= '\u0000' && LA25_1 <= '.') || (LA25_1 >= '0' && LA25_1 <= '\uFFFF')) )
            	        {
            	            alt25 = 1;
            	        }


            	    }
            	    else if ( ((LA25_0 >= '\u0000' && LA25_0 <= ')') || (LA25_0 >= '+' && LA25_0 <= '\uFFFF')) )
            	    {
            	        alt25 = 1;
            	    }


            	    switch (alt25) 
            		{
            			case 1 :
            			    // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:416:37: .
            			    {
            			    	MatchAny(); 

            			    }
            			    break;

            			default:
            			    goto loop25;
            	    }
            	} while (true);

            	loop25:
            		;	// Stops C# compiler whining that label 'loop25' has no statements

            	Match("*/"); 

            	_channel=HIDDEN;

            }

            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "COMMENT"

    // $ANTLR start "LINE_COMMENT"
    public void mLINE_COMMENT() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = LINE_COMMENT;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:419:2: ( '//' (~ ( '\\n' | '\\r' ) )* ( '\\r' )? '\\n' )
            // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:419:4: '//' (~ ( '\\n' | '\\r' ) )* ( '\\r' )? '\\n'
            {
            	Match("//"); 

            	// C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:419:9: (~ ( '\\n' | '\\r' ) )*
            	do 
            	{
            	    int alt26 = 2;
            	    int LA26_0 = input.LA(1);

            	    if ( ((LA26_0 >= '\u0000' && LA26_0 <= '\t') || (LA26_0 >= '\u000B' && LA26_0 <= '\f') || (LA26_0 >= '\u000E' && LA26_0 <= '\uFFFF')) )
            	    {
            	        alt26 = 1;
            	    }


            	    switch (alt26) 
            		{
            			case 1 :
            			    // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:419:9: ~ ( '\\n' | '\\r' )
            			    {
            			    	if ( (input.LA(1) >= '\u0000' && input.LA(1) <= '\t') || (input.LA(1) >= '\u000B' && input.LA(1) <= '\f') || (input.LA(1) >= '\u000E' && input.LA(1) <= '\uFFFF') ) 
            			    	{
            			    	    input.Consume();

            			    	}
            			    	else 
            			    	{
            			    	    MismatchedSetException mse = new MismatchedSetException(null,input);
            			    	    Recover(mse);
            			    	    throw mse;}


            			    }
            			    break;

            			default:
            			    goto loop26;
            	    }
            	} while (true);

            	loop26:
            		;	// Stops C# compiler whining that label 'loop26' has no statements

            	// C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:419:23: ( '\\r' )?
            	int alt27 = 2;
            	int LA27_0 = input.LA(1);

            	if ( (LA27_0 == '\r') )
            	{
            	    alt27 = 1;
            	}
            	switch (alt27) 
            	{
            	    case 1 :
            	        // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:419:23: '\\r'
            	        {
            	        	Match('\r'); 

            	        }
            	        break;

            	}

            	Match('\n'); 
            	_channel=HIDDEN;

            }

            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "LINE_COMMENT"

    override public void mTokens() // throws RecognitionException 
    {
        // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:1:8: ( T__31 | T__32 | T__33 | T__34 | T__35 | T__36 | T__37 | T__38 | T__39 | T__40 | T__41 | T__42 | T__43 | T__44 | T__45 | T__46 | T__47 | T__48 | T__49 | T__50 | T__51 | T__52 | T__53 | T__54 | T__55 | T__56 | T__57 | T__58 | T__59 | T__60 | T__61 | T__62 | T__63 | T__64 | T__65 | T__66 | T__67 | T__68 | T__69 | T__70 | T__71 | T__72 | T__73 | T__74 | T__75 | T__76 | T__77 | T__78 | T__79 | T__80 | T__81 | T__82 | T__83 | T__84 | T__85 | T__86 | T__87 | T__88 | T__89 | T__90 | T__91 | T__92 | T__93 | T__94 | T__95 | T__96 | T__97 | T__98 | T__99 | T__100 | Id | HexLiteral | DecimalLiteral | OctalLiteral | FloatingPointLiteral | StringLiteral | WS | COMMENT | LINE_COMMENT )
        int alt28 = 79;
        alt28 = dfa28.Predict(input);
        switch (alt28) 
        {
            case 1 :
                // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:1:10: T__31
                {
                	mT__31(); 

                }
                break;
            case 2 :
                // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:1:16: T__32
                {
                	mT__32(); 

                }
                break;
            case 3 :
                // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:1:22: T__33
                {
                	mT__33(); 

                }
                break;
            case 4 :
                // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:1:28: T__34
                {
                	mT__34(); 

                }
                break;
            case 5 :
                // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:1:34: T__35
                {
                	mT__35(); 

                }
                break;
            case 6 :
                // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:1:40: T__36
                {
                	mT__36(); 

                }
                break;
            case 7 :
                // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:1:46: T__37
                {
                	mT__37(); 

                }
                break;
            case 8 :
                // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:1:52: T__38
                {
                	mT__38(); 

                }
                break;
            case 9 :
                // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:1:58: T__39
                {
                	mT__39(); 

                }
                break;
            case 10 :
                // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:1:64: T__40
                {
                	mT__40(); 

                }
                break;
            case 11 :
                // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:1:70: T__41
                {
                	mT__41(); 

                }
                break;
            case 12 :
                // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:1:76: T__42
                {
                	mT__42(); 

                }
                break;
            case 13 :
                // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:1:82: T__43
                {
                	mT__43(); 

                }
                break;
            case 14 :
                // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:1:88: T__44
                {
                	mT__44(); 

                }
                break;
            case 15 :
                // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:1:94: T__45
                {
                	mT__45(); 

                }
                break;
            case 16 :
                // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:1:100: T__46
                {
                	mT__46(); 

                }
                break;
            case 17 :
                // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:1:106: T__47
                {
                	mT__47(); 

                }
                break;
            case 18 :
                // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:1:112: T__48
                {
                	mT__48(); 

                }
                break;
            case 19 :
                // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:1:118: T__49
                {
                	mT__49(); 

                }
                break;
            case 20 :
                // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:1:124: T__50
                {
                	mT__50(); 

                }
                break;
            case 21 :
                // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:1:130: T__51
                {
                	mT__51(); 

                }
                break;
            case 22 :
                // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:1:136: T__52
                {
                	mT__52(); 

                }
                break;
            case 23 :
                // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:1:142: T__53
                {
                	mT__53(); 

                }
                break;
            case 24 :
                // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:1:148: T__54
                {
                	mT__54(); 

                }
                break;
            case 25 :
                // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:1:154: T__55
                {
                	mT__55(); 

                }
                break;
            case 26 :
                // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:1:160: T__56
                {
                	mT__56(); 

                }
                break;
            case 27 :
                // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:1:166: T__57
                {
                	mT__57(); 

                }
                break;
            case 28 :
                // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:1:172: T__58
                {
                	mT__58(); 

                }
                break;
            case 29 :
                // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:1:178: T__59
                {
                	mT__59(); 

                }
                break;
            case 30 :
                // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:1:184: T__60
                {
                	mT__60(); 

                }
                break;
            case 31 :
                // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:1:190: T__61
                {
                	mT__61(); 

                }
                break;
            case 32 :
                // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:1:196: T__62
                {
                	mT__62(); 

                }
                break;
            case 33 :
                // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:1:202: T__63
                {
                	mT__63(); 

                }
                break;
            case 34 :
                // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:1:208: T__64
                {
                	mT__64(); 

                }
                break;
            case 35 :
                // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:1:214: T__65
                {
                	mT__65(); 

                }
                break;
            case 36 :
                // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:1:220: T__66
                {
                	mT__66(); 

                }
                break;
            case 37 :
                // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:1:226: T__67
                {
                	mT__67(); 

                }
                break;
            case 38 :
                // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:1:232: T__68
                {
                	mT__68(); 

                }
                break;
            case 39 :
                // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:1:238: T__69
                {
                	mT__69(); 

                }
                break;
            case 40 :
                // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:1:244: T__70
                {
                	mT__70(); 

                }
                break;
            case 41 :
                // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:1:250: T__71
                {
                	mT__71(); 

                }
                break;
            case 42 :
                // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:1:256: T__72
                {
                	mT__72(); 

                }
                break;
            case 43 :
                // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:1:262: T__73
                {
                	mT__73(); 

                }
                break;
            case 44 :
                // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:1:268: T__74
                {
                	mT__74(); 

                }
                break;
            case 45 :
                // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:1:274: T__75
                {
                	mT__75(); 

                }
                break;
            case 46 :
                // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:1:280: T__76
                {
                	mT__76(); 

                }
                break;
            case 47 :
                // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:1:286: T__77
                {
                	mT__77(); 

                }
                break;
            case 48 :
                // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:1:292: T__78
                {
                	mT__78(); 

                }
                break;
            case 49 :
                // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:1:298: T__79
                {
                	mT__79(); 

                }
                break;
            case 50 :
                // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:1:304: T__80
                {
                	mT__80(); 

                }
                break;
            case 51 :
                // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:1:310: T__81
                {
                	mT__81(); 

                }
                break;
            case 52 :
                // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:1:316: T__82
                {
                	mT__82(); 

                }
                break;
            case 53 :
                // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:1:322: T__83
                {
                	mT__83(); 

                }
                break;
            case 54 :
                // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:1:328: T__84
                {
                	mT__84(); 

                }
                break;
            case 55 :
                // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:1:334: T__85
                {
                	mT__85(); 

                }
                break;
            case 56 :
                // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:1:340: T__86
                {
                	mT__86(); 

                }
                break;
            case 57 :
                // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:1:346: T__87
                {
                	mT__87(); 

                }
                break;
            case 58 :
                // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:1:352: T__88
                {
                	mT__88(); 

                }
                break;
            case 59 :
                // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:1:358: T__89
                {
                	mT__89(); 

                }
                break;
            case 60 :
                // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:1:364: T__90
                {
                	mT__90(); 

                }
                break;
            case 61 :
                // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:1:370: T__91
                {
                	mT__91(); 

                }
                break;
            case 62 :
                // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:1:376: T__92
                {
                	mT__92(); 

                }
                break;
            case 63 :
                // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:1:382: T__93
                {
                	mT__93(); 

                }
                break;
            case 64 :
                // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:1:388: T__94
                {
                	mT__94(); 

                }
                break;
            case 65 :
                // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:1:394: T__95
                {
                	mT__95(); 

                }
                break;
            case 66 :
                // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:1:400: T__96
                {
                	mT__96(); 

                }
                break;
            case 67 :
                // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:1:406: T__97
                {
                	mT__97(); 

                }
                break;
            case 68 :
                // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:1:412: T__98
                {
                	mT__98(); 

                }
                break;
            case 69 :
                // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:1:418: T__99
                {
                	mT__99(); 

                }
                break;
            case 70 :
                // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:1:424: T__100
                {
                	mT__100(); 

                }
                break;
            case 71 :
                // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:1:431: Id
                {
                	mId(); 

                }
                break;
            case 72 :
                // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:1:434: HexLiteral
                {
                	mHexLiteral(); 

                }
                break;
            case 73 :
                // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:1:445: DecimalLiteral
                {
                	mDecimalLiteral(); 

                }
                break;
            case 74 :
                // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:1:460: OctalLiteral
                {
                	mOctalLiteral(); 

                }
                break;
            case 75 :
                // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:1:473: FloatingPointLiteral
                {
                	mFloatingPointLiteral(); 

                }
                break;
            case 76 :
                // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:1:494: StringLiteral
                {
                	mStringLiteral(); 

                }
                break;
            case 77 :
                // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:1:508: WS
                {
                	mWS(); 

                }
                break;
            case 78 :
                // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:1:511: COMMENT
                {
                	mCOMMENT(); 

                }
                break;
            case 79 :
                // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:1:519: LINE_COMMENT
                {
                	mLINE_COMMENT(); 

                }
                break;

        }

    }


    protected DFA19 dfa19;
    protected DFA28 dfa28;
	private void InitializeCyclicDFAs()
	{
	    this.dfa19 = new DFA19(this);
	    this.dfa28 = new DFA28(this);
	}

    const string DFA19_eotS =
        "\x06\uffff";
    const string DFA19_eofS =
        "\x06\uffff";
    const string DFA19_minS =
        "\x02\x2e\x04\uffff";
    const string DFA19_maxS =
        "\x01\x39\x01\x66\x04\uffff";
    const string DFA19_acceptS =
        "\x02\uffff\x01\x02\x01\x04\x01\x03\x01\x01";
    const string DFA19_specialS =
        "\x06\uffff}>";
    static readonly string[] DFA19_transitionS = {
            "\x01\x02\x01\uffff\x0a\x01",
            "\x01\x05\x01\uffff\x0a\x01\x0a\uffff\x01\x03\x01\x04\x01\x03"+
            "\x1d\uffff\x01\x03\x01\x04\x01\x03",
            "",
            "",
            "",
            ""
    };

    static readonly short[] DFA19_eot = DFA.UnpackEncodedString(DFA19_eotS);
    static readonly short[] DFA19_eof = DFA.UnpackEncodedString(DFA19_eofS);
    static readonly char[] DFA19_min = DFA.UnpackEncodedStringToUnsignedChars(DFA19_minS);
    static readonly char[] DFA19_max = DFA.UnpackEncodedStringToUnsignedChars(DFA19_maxS);
    static readonly short[] DFA19_accept = DFA.UnpackEncodedString(DFA19_acceptS);
    static readonly short[] DFA19_special = DFA.UnpackEncodedString(DFA19_specialS);
    static readonly short[][] DFA19_transition = DFA.UnpackEncodedStringArray(DFA19_transitionS);

    protected class DFA19 : DFA
    {
        public DFA19(BaseRecognizer recognizer)
        {
            this.recognizer = recognizer;
            this.decisionNumber = 19;
            this.eot = DFA19_eot;
            this.eof = DFA19_eof;
            this.min = DFA19_min;
            this.max = DFA19_max;
            this.accept = DFA19_accept;
            this.special = DFA19_special;
            this.transition = DFA19_transition;

        }

        override public string Description
        {
            get { return "367:1: FloatingPointLiteral : ( ( '0' .. '9' )+ '.' ( '0' .. '9' )* ( Exponent )? ( FloatTypeSuffix )? | '.' ( '0' .. '9' )+ ( Exponent )? ( FloatTypeSuffix )? | ( '0' .. '9' )+ Exponent ( FloatTypeSuffix )? | ( '0' .. '9' )+ FloatTypeSuffix );"; }
        }

    }

    const string DFA28_eotS =
        "\x01\uffff\x01\x23\x01\x2c\x06\x23\x01\x39\x01\x3b\x01\x3e\x01"+
        "\x41\x01\x43\x01\x47\x01\x4a\x01\x4d\x01\x4f\x01\x51\x04\uffff\x03"+
        "\x23\x01\x56\x06\x23\x03\uffff\x02\x62\x03\uffff\x01\x23\x01\x65"+
        "\x02\uffff\x03\x23\x01\x6a\x01\x6c\x02\x23\x01\x70\x02\x23\x1b\uffff"+
        "\x01\x73\x02\x23\x02\uffff\x01\x77\x01\x79\x01\x7a\x01\x7c\x01\x7d"+
        "\x01\x7f\x01\u0081\x01\u0083\x01\u0085\x01\uffff\x01\u0086\x01\uffff"+
        "\x01\x62\x01\x23\x01\uffff\x03\x23\x01\u008b\x01\uffff\x01\u008c"+
        "\x01\uffff\x03\x23\x01\uffff\x02\x23\x01\uffff\x01\u0092\x01\u0093"+
        "\x01\x23\x01\uffff\x01\x23\x02\uffff\x01\x23\x02\uffff\x01\u0097"+
        "\x01\uffff\x01\u0098\x01\uffff\x01\u0099\x01\uffff\x01\u009a\x02"+
        "\uffff\x04\x23\x02\uffff\x01\x23\x01\u00a0\x01\u00a1\x01\u00a2\x01"+
        "\x23\x02\uffff\x01\u00a4\x01\u00a5\x01\u00a6\x04\uffff\x01\x23\x01"+
        "\uffff\x02\x23\x04\uffff\x01\u00aa\x03\uffff\x03\x23\x05\uffff";
    const string DFA28_eofS =
        "\u00af\uffff";
    const string DFA28_minS =
        "\x01\x09\x01\x42\x01\x2e\x01\x45\x01\x4c\x01\x52\x01\x65\x01\x72"+
        "\x01\x61\x02\x3d\x01\x2b\x01\x2d\x01\x3d\x01\x2a\x01\x26\x03\x3d"+
        "\x04\uffff\x01\x72\x01\x6e\x01\x4e\x01\x3d\x01\x71\x01\x51\x01\x45"+
        "\x02\x74\x01\x54\x03\uffff\x02\x2e\x03\uffff\x01\x4a\x01\x30\x02"+
        "\uffff\x01\x4d\x01\x52\x01\x4f\x02\x30\x01\x4f\x01\x6c\x01\x30\x01"+
        "\x75\x01\x6c\x1b\uffff\x01\x30\x01\x64\x01\x44\x02\uffff\x09\x30"+
        "\x01\uffff\x01\x2e\x01\uffff\x01\x2e\x01\x45\x01\uffff\x01\x50\x01"+
        "\x45\x01\x42\x01\x30\x01\uffff\x01\x30\x01\uffff\x01\x43\x01\x6c"+
        "\x01\x45\x01\uffff\x01\x65\x01\x73\x01\uffff\x02\x30\x01\x51\x01"+
        "\uffff\x01\x51\x02\uffff\x01\x45\x02\uffff\x01\x30\x01\uffff\x01"+
        "\x30\x01\uffff\x01\x30\x01\uffff\x01\x30\x02\uffff\x01\x43\x01\x2e"+
        "\x02\x41\x02\uffff\x01\x2e\x03\x30\x01\x65\x02\uffff\x03\x30\x04"+
        "\uffff\x01\x54\x01\uffff\x01\x44\x01\x4c\x04\uffff\x01\x30\x03\uffff"+
        "\x01\x28\x02\x2e\x05\uffff";
    const string DFA28_maxS =
        "\x01\x7e\x01\x52\x01\x2e\x01\x48\x01\x74\x01\x52\x01\x75\x01\x72"+
        "\x01\x61\x01\x3e\x06\x3d\x01\x7c\x02\x3d\x04\uffff\x01\x72\x01\x6e"+
        "\x01\x4e\x01\x3d\x02\x71\x01\x65\x03\x74\x03\uffff\x01\x78\x01\x66"+
        "\x03\uffff\x01\x4a\x01\x7a\x02\uffff\x01\x4d\x01\x52\x01\x4f\x02"+
        "\x7a\x01\x4f\x01\x6c\x01\x7a\x01\x75\x01\x6c\x1b\uffff\x01\x7a\x01"+
        "\x64\x01\x44\x02\uffff\x09\x7a\x01\uffff\x01\x66\x01\uffff\x01\x66"+
        "\x01\x45\x01\uffff\x01\x50\x01\x45\x01\x42\x01\x7a\x01\uffff\x01"+
        "\x7a\x01\uffff\x01\x43\x01\x6c\x01\x45\x01\uffff\x01\x65\x01\x73"+
        "\x01\uffff\x02\x7a\x01\x51\x01\uffff\x01\x51\x02\uffff\x01\x45\x02"+
        "\uffff\x01\x7a\x01\uffff\x01\x7a\x01\uffff\x01\x7a\x01\uffff\x01"+
        "\x7a\x02\uffff\x01\x43\x01\x2e\x02\x41\x02\uffff\x01\x2e\x03\x7a"+
        "\x01\x65\x02\uffff\x03\x7a\x04\uffff\x01\x54\x01\uffff\x01\x44\x01"+
        "\x4c\x04\uffff\x01\x7a\x03\uffff\x03\x2e\x05\uffff";
    const string DFA28_acceptS =
        "\x13\uffff\x01\x16\x01\x17\x01\x18\x01\x19\x0a\uffff\x01\x43\x01"+
        "\x46\x01\x47\x02\uffff\x01\x4b\x01\x4c\x01\x4d\x02\uffff\x01\x03"+
        "\x01\x44\x0a\uffff\x01\x23\x01\x45\x01\x0b\x01\x15\x01\x0c\x01\x0d"+
        "\x01\x40\x01\x3b\x01\x0e\x01\x41\x01\x3c\x01\x0f\x01\x3d\x01\x10"+
        "\x01\x4e\x01\x4f\x01\x3e\x01\x11\x01\x1d\x01\x22\x01\x12\x01\x1a"+
        "\x01\x20\x01\x13\x01\x21\x01\x14\x01\x3f\x03\uffff\x01\x24\x01\x42"+
        "\x09\uffff\x01\x48\x01\uffff\x01\x49\x02\uffff\x01\x1c\x04\uffff"+
        "\x01\x33\x01\uffff\x01\x37\x03\uffff\x01\x26\x02\uffff\x01\x1b\x03"+
        "\uffff\x01\x25\x01\uffff\x01\x27\x01\x29\x01\uffff\x01\x28\x01\x2a"+
        "\x01\uffff\x01\x2f\x01\uffff\x01\x30\x01\uffff\x01\x34\x01\uffff"+
        "\x01\x38\x01\x4a\x04\uffff\x01\x35\x01\x39\x05\uffff\x01\x1e\x01"+
        "\x1f\x03\uffff\x01\x31\x01\x32\x01\x36\x01\x3a\x01\uffff\x01\x04"+
        "\x02\uffff\x01\x07\x01\x08\x01\x2d\x01\x09\x01\uffff\x01\x2b\x01"+
        "\x2c\x01\x2e\x03\uffff\x01\x0a\x01\x01\x01\x02\x01\x06\x01\x05";
    const string DFA28_specialS =
        "\u00af\uffff}>";
    static readonly string[] DFA28_transitionS = {
            "\x02\x28\x01\uffff\x02\x28\x12\uffff\x01\x28\x01\x1a\x01\x27"+
            "\x02\uffff\x01\x12\x01\x0f\x01\x27\x01\x21\x01\x02\x01\x0d\x01"+
            "\x0b\x01\x22\x01\x0c\x01\x26\x01\x0e\x01\x24\x09\x25\x01\x16"+
            "\x01\uffff\x01\x13\x01\x09\x01\x14\x01\x15\x01\uffff\x01\x19"+
            "\x03\x23\x01\x1c\x01\x23\x01\x04\x04\x23\x01\x20\x01\x23\x01"+
            "\x1d\x01\x01\x01\x05\x03\x23\x01\x03\x06\x23\x03\uffff\x01\x11"+
            "\x01\x23\x01\uffff\x01\x18\x03\x23\x01\x1b\x01\x08\x01\x1e\x04"+
            "\x23\x01\x1f\x01\x23\x01\x06\x01\x17\x04\x23\x01\x07\x06\x23"+
            "\x01\uffff\x01\x10\x01\uffff\x01\x0a",
            "\x01\x29\x0f\uffff\x01\x2a",
            "\x01\x2b",
            "\x01\x2d\x02\uffff\x01\x2e",
            "\x01\x2f\x07\uffff\x01\x31\x1f\uffff\x01\x30",
            "\x01\x32",
            "\x01\x34\x0f\uffff\x01\x33",
            "\x01\x35",
            "\x01\x36",
            "\x01\x37\x01\x38",
            "\x01\x3a",
            "\x01\x3d\x11\uffff\x01\x3c",
            "\x01\x40\x0f\uffff\x01\x3f",
            "\x01\x42",
            "\x01\x45\x04\uffff\x01\x46\x0d\uffff\x01\x44",
            "\x01\x49\x16\uffff\x01\x48",
            "\x01\x4b\x3e\uffff\x01\x4c",
            "\x01\x4e",
            "\x01\x50",
            "",
            "",
            "",
            "",
            "\x01\x52",
            "\x01\x53",
            "\x01\x54",
            "\x01\x55",
            "\x01\x57",
            "\x01\x59\x1f\uffff\x01\x58",
            "\x01\x5b\x1f\uffff\x01\x5a",
            "\x01\x5c",
            "\x01\x5d",
            "\x01\x5f\x1f\uffff\x01\x5e",
            "",
            "",
            "",
            "\x01\x26\x01\uffff\x08\x61\x02\x26\x0a\uffff\x03\x26\x11\uffff"+
            "\x01\x60\x0b\uffff\x03\x26\x11\uffff\x01\x60",
            "\x01\x26\x01\uffff\x0a\x63\x0a\uffff\x03\x26\x1d\uffff\x03"+
            "\x26",
            "",
            "",
            "",
            "\x01\x64",
            "\x0a\x23\x07\uffff\x1a\x23\x04\uffff\x01\x23\x01\uffff\x1a"+
            "\x23",
            "",
            "",
            "\x01\x66",
            "\x01\x67",
            "\x01\x68",
            "\x0a\x23\x07\uffff\x1a\x23\x04\uffff\x01\x23\x01\uffff\x04"+
            "\x23\x01\x69\x15\x23",
            "\x0a\x23\x07\uffff\x04\x23\x01\x6b\x15\x23\x04\uffff\x01\x23"+
            "\x01\uffff\x1a\x23",
            "\x01\x6d",
            "\x01\x6e",
            "\x0a\x23\x07\uffff\x0d\x23\x01\x6f\x0c\x23\x04\uffff\x01\x23"+
            "\x01\uffff\x1a\x23",
            "\x01\x71",
            "\x01\x72",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "\x0a\x23\x07\uffff\x1a\x23\x04\uffff\x01\x23\x01\uffff\x1a"+
            "\x23",
            "\x01\x74",
            "\x01\x75",
            "",
            "",
            "\x0a\x23\x07\uffff\x04\x23\x01\x76\x15\x23\x04\uffff\x01\x23"+
            "\x01\uffff\x1a\x23",
            "\x0a\x23\x07\uffff\x04\x23\x01\x78\x15\x23\x04\uffff\x01\x23"+
            "\x01\uffff\x1a\x23",
            "\x0a\x23\x07\uffff\x1a\x23\x04\uffff\x01\x23\x01\uffff\x1a"+
            "\x23",
            "\x0a\x23\x07\uffff\x0d\x23\x01\x7b\x0c\x23\x04\uffff\x01\x23"+
            "\x01\uffff\x1a\x23",
            "\x0a\x23\x07\uffff\x1a\x23\x04\uffff\x01\x23\x01\uffff\x1a"+
            "\x23",
            "\x0a\x23\x07\uffff\x1a\x23\x04\uffff\x01\x23\x01\uffff\x04"+
            "\x23\x01\x7e\x15\x23",
            "\x0a\x23\x07\uffff\x1a\x23\x04\uffff\x01\x23\x01\uffff\x04"+
            "\x23\x01\u0080\x15\x23",
            "\x0a\x23\x07\uffff\x1a\x23\x04\uffff\x01\x23\x01\uffff\x04"+
            "\x23\x01\u0082\x15\x23",
            "\x0a\x23\x07\uffff\x04\x23\x01\u0084\x15\x23\x04\uffff\x01"+
            "\x23\x01\uffff\x1a\x23",
            "",
            "\x01\x26\x01\uffff\x08\x61\x02\x26\x0a\uffff\x03\x26\x1d\uffff"+
            "\x03\x26",
            "",
            "\x01\x26\x01\uffff\x0a\x63\x0a\uffff\x03\x26\x1d\uffff\x03"+
            "\x26",
            "\x01\u0087",
            "",
            "\x01\u0088",
            "\x01\u0089",
            "\x01\u008a",
            "\x0a\x23\x07\uffff\x1a\x23\x04\uffff\x01\x23\x01\uffff\x1a"+
            "\x23",
            "",
            "\x0a\x23\x07\uffff\x1a\x23\x04\uffff\x01\x23\x01\uffff\x1a"+
            "\x23",
            "",
            "\x01\u008d",
            "\x01\u008e",
            "\x01\u008f",
            "",
            "\x01\u0090",
            "\x01\u0091",
            "",
            "\x0a\x23\x07\uffff\x1a\x23\x04\uffff\x01\x23\x01\uffff\x1a"+
            "\x23",
            "\x0a\x23\x07\uffff\x1a\x23\x04\uffff\x01\x23\x01\uffff\x1a"+
            "\x23",
            "\x01\u0094",
            "",
            "\x01\u0095",
            "",
            "",
            "\x01\u0096",
            "",
            "",
            "\x0a\x23\x07\uffff\x1a\x23\x04\uffff\x01\x23\x01\uffff\x1a"+
            "\x23",
            "",
            "\x0a\x23\x07\uffff\x1a\x23\x04\uffff\x01\x23\x01\uffff\x1a"+
            "\x23",
            "",
            "\x0a\x23\x07\uffff\x1a\x23\x04\uffff\x01\x23\x01\uffff\x1a"+
            "\x23",
            "",
            "\x0a\x23\x07\uffff\x1a\x23\x04\uffff\x01\x23\x01\uffff\x1a"+
            "\x23",
            "",
            "",
            "\x01\u009b",
            "\x01\u009c",
            "\x01\u009d",
            "\x01\u009e",
            "",
            "",
            "\x01\u009f",
            "\x0a\x23\x07\uffff\x1a\x23\x04\uffff\x01\x23\x01\uffff\x1a"+
            "\x23",
            "\x0a\x23\x07\uffff\x1a\x23\x04\uffff\x01\x23\x01\uffff\x1a"+
            "\x23",
            "\x0a\x23\x07\uffff\x1a\x23\x04\uffff\x01\x23\x01\uffff\x1a"+
            "\x23",
            "\x01\u00a3",
            "",
            "",
            "\x0a\x23\x07\uffff\x1a\x23\x04\uffff\x01\x23\x01\uffff\x1a"+
            "\x23",
            "\x0a\x23\x07\uffff\x1a\x23\x04\uffff\x01\x23\x01\uffff\x1a"+
            "\x23",
            "\x0a\x23\x07\uffff\x1a\x23\x04\uffff\x01\x23\x01\uffff\x1a"+
            "\x23",
            "",
            "",
            "",
            "",
            "\x01\u00a7",
            "",
            "\x01\u00a8",
            "\x01\u00a9",
            "",
            "",
            "",
            "",
            "\x0a\x23\x07\uffff\x1a\x23\x04\uffff\x01\x23\x01\uffff\x1a"+
            "\x23",
            "",
            "",
            "",
            "\x01\u00ac\x05\uffff\x01\u00ab",
            "\x01\u00ad",
            "\x01\u00ae",
            "",
            "",
            "",
            "",
            ""
    };

    static readonly short[] DFA28_eot = DFA.UnpackEncodedString(DFA28_eotS);
    static readonly short[] DFA28_eof = DFA.UnpackEncodedString(DFA28_eofS);
    static readonly char[] DFA28_min = DFA.UnpackEncodedStringToUnsignedChars(DFA28_minS);
    static readonly char[] DFA28_max = DFA.UnpackEncodedStringToUnsignedChars(DFA28_maxS);
    static readonly short[] DFA28_accept = DFA.UnpackEncodedString(DFA28_acceptS);
    static readonly short[] DFA28_special = DFA.UnpackEncodedString(DFA28_specialS);
    static readonly short[][] DFA28_transition = DFA.UnpackEncodedStringArray(DFA28_transitionS);

    protected class DFA28 : DFA
    {
        public DFA28(BaseRecognizer recognizer)
        {
            this.recognizer = recognizer;
            this.decisionNumber = 28;
            this.eot = DFA28_eot;
            this.eof = DFA28_eof;
            this.min = DFA28_min;
            this.max = DFA28_max;
            this.accept = DFA28_accept;
            this.special = DFA28_special;
            this.transition = DFA28_transition;

        }

        override public string Description
        {
            get { return "1:1: Tokens : ( T__31 | T__32 | T__33 | T__34 | T__35 | T__36 | T__37 | T__38 | T__39 | T__40 | T__41 | T__42 | T__43 | T__44 | T__45 | T__46 | T__47 | T__48 | T__49 | T__50 | T__51 | T__52 | T__53 | T__54 | T__55 | T__56 | T__57 | T__58 | T__59 | T__60 | T__61 | T__62 | T__63 | T__64 | T__65 | T__66 | T__67 | T__68 | T__69 | T__70 | T__71 | T__72 | T__73 | T__74 | T__75 | T__76 | T__77 | T__78 | T__79 | T__80 | T__81 | T__82 | T__83 | T__84 | T__85 | T__86 | T__87 | T__88 | T__89 | T__90 | T__91 | T__92 | T__93 | T__94 | T__95 | T__96 | T__97 | T__98 | T__99 | T__100 | Id | HexLiteral | DecimalLiteral | OctalLiteral | FloatingPointLiteral | StringLiteral | WS | COMMENT | LINE_COMMENT );"; }
        }

    }

 
    
}
