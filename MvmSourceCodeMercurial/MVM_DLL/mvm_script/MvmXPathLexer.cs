// $ANTLR 3.2 Sep 23, 2009 12:02:23 D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmXPath.g 2010-11-12 11:09:26

// The variable 'variable' is assigned but its value is never used.
#pragma warning disable 168, 219
// Unreachable code detected.
#pragma warning disable 162


using System;
using Antlr.Runtime;
using IList 		= System.Collections.IList;
using ArrayList 	= System.Collections.ArrayList;
using Stack 		= Antlr.Runtime.Collections.StackList;


public partial class MvmXPathLexer : Lexer {
    public const int NCName = 41;
    public const int APOS = 35;
    public const int ABSOLUTE_PATH = 5;
    public const int PATHSEP = 15;
    public const int DOTDOT = 25;
    public const int EOF = -1;
    public const int PREDICATE = 8;
    public const int TRAVERSE_UP = 10;
    public const int AT = 26;
    public const int T__55 = 55;
    public const int T__56 = 56;
    public const int T__57 = 57;
    public const int T__51 = 51;
    public const int Literal = 39;
    public const int T__52 = 52;
    public const int T__53 = 53;
    public const int Number = 40;
    public const int LPAR = 17;
    public const int T__54 = 54;
    public const int RECURSIVE_MATCH = 9;
    public const int Digits = 42;
    public const int COMMA = 27;
    public const int RBRAC = 20;
    public const int LESS = 29;
    public const int AxisName = 37;
    public const int RECURSIVE_NAME_MATCH = 14;
    public const int PLUS = 22;
    public const int PIPE = 28;
    public const int NodeType = 38;
    public const int DOT = 23;
    public const int Whitespace = 43;
    public const int T__50 = 50;
    public const int NCNameStartChar = 44;
    public const int GE = 32;
    public const int T__46 = 46;
    public const int NCNameChar = 45;
    public const int T__47 = 47;
    public const int T__48 = 48;
    public const int T__49 = 49;
    public const int QUOT = 36;
    public const int MATCH = 12;
    public const int ABRPATH = 16;
    public const int MINUS = 21;
    public const int MUL = 24;
    public const int LBRAC = 19;
    public const int NAME_MATCH = 13;
    public const int COLON = 33;
    public const int XPATH = 4;
    public const int MORE = 30;
    public const int CURRENT_NODE = 11;
    public const int ROOT_PATH = 7;
    public const int RPAR = 18;
    public const int RELATIVE_PATH = 6;
    public const int CC = 34;
    public const int LE = 31;

    // delegates
    // delegators

    public MvmXPathLexer() 
    {
		InitializeCyclicDFAs();
    }
    public MvmXPathLexer(ICharStream input)
		: this(input, null) {
    }
    public MvmXPathLexer(ICharStream input, RecognizerSharedState state)
		: base(input, state) {
		InitializeCyclicDFAs(); 

    }
    
    override public string GrammarFileName
    {
    	get { return "D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmXPath.g";} 
    }

    // $ANTLR start "PATHSEP"
    public void mPATHSEP() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = PATHSEP;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmXPath.g:7:9: ( '/' )
            // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmXPath.g:7:11: '/'
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
    // $ANTLR end "PATHSEP"

    // $ANTLR start "ABRPATH"
    public void mABRPATH() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = ABRPATH;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmXPath.g:8:9: ( '//' )
            // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmXPath.g:8:11: '//'
            {
            	Match("//"); 


            }

            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "ABRPATH"

    // $ANTLR start "LPAR"
    public void mLPAR() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = LPAR;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmXPath.g:9:6: ( '(' )
            // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmXPath.g:9:8: '('
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
    // $ANTLR end "LPAR"

    // $ANTLR start "RPAR"
    public void mRPAR() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = RPAR;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmXPath.g:10:6: ( ')' )
            // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmXPath.g:10:8: ')'
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
    // $ANTLR end "RPAR"

    // $ANTLR start "LBRAC"
    public void mLBRAC() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = LBRAC;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmXPath.g:11:7: ( '[' )
            // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmXPath.g:11:9: '['
            {
            	Match('['); 

            }

            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "LBRAC"

    // $ANTLR start "RBRAC"
    public void mRBRAC() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = RBRAC;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmXPath.g:12:7: ( ']' )
            // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmXPath.g:12:9: ']'
            {
            	Match(']'); 

            }

            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "RBRAC"

    // $ANTLR start "MINUS"
    public void mMINUS() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = MINUS;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmXPath.g:13:7: ( '-' )
            // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmXPath.g:13:9: '-'
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
    // $ANTLR end "MINUS"

    // $ANTLR start "PLUS"
    public void mPLUS() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = PLUS;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmXPath.g:14:6: ( '+' )
            // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmXPath.g:14:8: '+'
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
    // $ANTLR end "PLUS"

    // $ANTLR start "DOT"
    public void mDOT() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = DOT;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmXPath.g:15:5: ( '.' )
            // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmXPath.g:15:7: '.'
            {
            	Match('.'); 

            }

            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "DOT"

    // $ANTLR start "MUL"
    public void mMUL() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = MUL;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmXPath.g:16:5: ( '*' )
            // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmXPath.g:16:7: '*'
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
    // $ANTLR end "MUL"

    // $ANTLR start "DOTDOT"
    public void mDOTDOT() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = DOTDOT;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmXPath.g:17:8: ( '..' )
            // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmXPath.g:17:10: '..'
            {
            	Match(".."); 


            }

            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "DOTDOT"

    // $ANTLR start "AT"
    public void mAT() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = AT;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmXPath.g:18:4: ( '@' )
            // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmXPath.g:18:6: '@'
            {
            	Match('@'); 

            }

            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "AT"

    // $ANTLR start "COMMA"
    public void mCOMMA() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = COMMA;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmXPath.g:19:7: ( ',' )
            // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmXPath.g:19:9: ','
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
    // $ANTLR end "COMMA"

    // $ANTLR start "PIPE"
    public void mPIPE() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = PIPE;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmXPath.g:20:6: ( '|' )
            // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmXPath.g:20:8: '|'
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
    // $ANTLR end "PIPE"

    // $ANTLR start "LESS"
    public void mLESS() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = LESS;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmXPath.g:21:6: ( '<' )
            // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmXPath.g:21:8: '<'
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
    // $ANTLR end "LESS"

    // $ANTLR start "MORE"
    public void mMORE() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = MORE;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmXPath.g:22:6: ( '>' )
            // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmXPath.g:22:8: '>'
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
    // $ANTLR end "MORE"

    // $ANTLR start "LE"
    public void mLE() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = LE;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmXPath.g:23:4: ( '<=' )
            // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmXPath.g:23:6: '<='
            {
            	Match("<="); 


            }

            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "LE"

    // $ANTLR start "GE"
    public void mGE() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = GE;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmXPath.g:24:4: ( '>=' )
            // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmXPath.g:24:6: '>='
            {
            	Match(">="); 


            }

            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "GE"

    // $ANTLR start "COLON"
    public void mCOLON() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = COLON;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmXPath.g:25:7: ( ':' )
            // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmXPath.g:25:9: ':'
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
    // $ANTLR end "COLON"

    // $ANTLR start "CC"
    public void mCC() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = CC;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmXPath.g:26:4: ( '::' )
            // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmXPath.g:26:6: '::'
            {
            	Match("::"); 


            }

            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "CC"

    // $ANTLR start "APOS"
    public void mAPOS() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = APOS;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmXPath.g:27:6: ( '\\'' )
            // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmXPath.g:27:8: '\\''
            {
            	Match('\''); 

            }

            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "APOS"

    // $ANTLR start "QUOT"
    public void mQUOT() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = QUOT;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmXPath.g:28:6: ( '\\\"' )
            // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmXPath.g:28:8: '\\\"'
            {
            	Match('\"'); 

            }

            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "QUOT"

    // $ANTLR start "T__46"
    public void mT__46() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = T__46;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmXPath.g:29:7: ( '/..' )
            // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmXPath.g:29:9: '/..'
            {
            	Match("/.."); 


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
            // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmXPath.g:30:7: ( '/.' )
            // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmXPath.g:30:9: '/.'
            {
            	Match("/."); 


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
            // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmXPath.g:31:7: ( '/@' )
            // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmXPath.g:31:9: '/@'
            {
            	Match("/@"); 


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
            // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmXPath.g:32:7: ( '//@' )
            // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmXPath.g:32:9: '//@'
            {
            	Match("//@"); 


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
            // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmXPath.g:33:7: ( 'processing-instruction' )
            // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmXPath.g:33:9: 'processing-instruction'
            {
            	Match("processing-instruction"); 


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
            // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmXPath.g:34:7: ( 'or' )
            // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmXPath.g:34:9: 'or'
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
    // $ANTLR end "T__51"

    // $ANTLR start "T__52"
    public void mT__52() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = T__52;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmXPath.g:35:7: ( 'and' )
            // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmXPath.g:35:9: 'and'
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
    // $ANTLR end "T__52"

    // $ANTLR start "T__53"
    public void mT__53() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = T__53;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmXPath.g:36:7: ( '=' )
            // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmXPath.g:36:9: '='
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
    // $ANTLR end "T__53"

    // $ANTLR start "T__54"
    public void mT__54() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = T__54;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmXPath.g:37:7: ( '!=' )
            // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmXPath.g:37:9: '!='
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
    // $ANTLR end "T__54"

    // $ANTLR start "T__55"
    public void mT__55() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = T__55;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmXPath.g:38:7: ( 'div' )
            // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmXPath.g:38:9: 'div'
            {
            	Match("div"); 


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
            // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmXPath.g:39:7: ( 'numMods' )
            // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmXPath.g:39:9: 'numMods'
            {
            	Match("mod"); 


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
            // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmXPath.g:40:7: ( '$' )
            // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmXPath.g:40:9: '$'
            {
            	Match('$'); 

            }

            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "T__57"

    // $ANTLR start "NodeType"
    public void mNodeType() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = NodeType;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmXPath.g:214:9: ( 'comment' | 'text' | 'processing-instruction' | 'node' )
            int alt1 = 4;
            switch ( input.LA(1) ) 
            {
            case 'c':
            	{
                alt1 = 1;
                }
                break;
            case 't':
            	{
                alt1 = 2;
                }
                break;
            case 'p':
            	{
                alt1 = 3;
                }
                break;
            case 'n':
            	{
                alt1 = 4;
                }
                break;
            	default:
            	    NoViableAltException nvae_d1s0 =
            	        new NoViableAltException("", 1, 0, input);

            	    throw nvae_d1s0;
            }

            switch (alt1) 
            {
                case 1 :
                    // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmXPath.g:214:12: 'comment'
                    {
                    	Match("comment"); 


                    }
                    break;
                case 2 :
                    // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmXPath.g:215:6: 'text'
                    {
                    	Match("text"); 


                    }
                    break;
                case 3 :
                    // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmXPath.g:216:6: 'processing-instruction'
                    {
                    	Match("processing-instruction"); 


                    }
                    break;
                case 4 :
                    // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmXPath.g:217:6: 'node'
                    {
                    	Match("node"); 


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
    // $ANTLR end "NodeType"

    // $ANTLR start "Number"
    public void mNumber() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = Number;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmXPath.g:220:9: ( Digits ( '.' ( Digits )? )? | '.' Digits )
            int alt4 = 2;
            int LA4_0 = input.LA(1);

            if ( ((LA4_0 >= '0' && LA4_0 <= '9')) )
            {
                alt4 = 1;
            }
            else if ( (LA4_0 == '.') )
            {
                alt4 = 2;
            }
            else 
            {
                NoViableAltException nvae_d4s0 =
                    new NoViableAltException("", 4, 0, input);

                throw nvae_d4s0;
            }
            switch (alt4) 
            {
                case 1 :
                    // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmXPath.g:220:12: Digits ( '.' ( Digits )? )?
                    {
                    	mDigits(); 
                    	// D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmXPath.g:220:19: ( '.' ( Digits )? )?
                    	int alt3 = 2;
                    	int LA3_0 = input.LA(1);

                    	if ( (LA3_0 == '.') )
                    	{
                    	    alt3 = 1;
                    	}
                    	switch (alt3) 
                    	{
                    	    case 1 :
                    	        // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmXPath.g:220:20: '.' ( Digits )?
                    	        {
                    	        	Match('.'); 
                    	        	// D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmXPath.g:220:24: ( Digits )?
                    	        	int alt2 = 2;
                    	        	int LA2_0 = input.LA(1);

                    	        	if ( ((LA2_0 >= '0' && LA2_0 <= '9')) )
                    	        	{
                    	        	    alt2 = 1;
                    	        	}
                    	        	switch (alt2) 
                    	        	{
                    	        	    case 1 :
                    	        	        // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmXPath.g:220:24: Digits
                    	        	        {
                    	        	        	mDigits(); 

                    	        	        }
                    	        	        break;

                    	        	}


                    	        }
                    	        break;

                    	}


                    }
                    break;
                case 2 :
                    // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmXPath.g:221:6: '.' Digits
                    {
                    	Match('.'); 
                    	mDigits(); 

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
    // $ANTLR end "Number"

    // $ANTLR start "Digits"
    public void mDigits() // throws RecognitionException [2]
    {
    		try
    		{
            // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmXPath.g:225:9: ( ( '0' .. '9' )+ )
            // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmXPath.g:225:12: ( '0' .. '9' )+
            {
            	// D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmXPath.g:225:12: ( '0' .. '9' )+
            	int cnt5 = 0;
            	do 
            	{
            	    int alt5 = 2;
            	    int LA5_0 = input.LA(1);

            	    if ( ((LA5_0 >= '0' && LA5_0 <= '9')) )
            	    {
            	        alt5 = 1;
            	    }


            	    switch (alt5) 
            		{
            			case 1 :
            			    // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmXPath.g:225:13: '0' .. '9'
            			    {
            			    	MatchRange('0','9'); 

            			    }
            			    break;

            			default:
            			    if ( cnt5 >= 1 ) goto loop5;
            		            EarlyExitException eee5 =
            		                new EarlyExitException(5, input);
            		            throw eee5;
            	    }
            	    cnt5++;
            	} while (true);

            	loop5:
            		;	// Stops C# compiler whining that label 'loop5' has no statements


            }

        }
        finally 
    	{
        }
    }
    // $ANTLR end "Digits"

    // $ANTLR start "AxisName"
    public void mAxisName() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = AxisName;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmXPath.g:228:9: ( 'ancestor' | 'ancestor-or-self' | 'attribute' | 'child' | 'descendant' | 'descendant-or-self' | 'following' | 'following-sibling' | 'namespace' | 'parent' | 'preceding' | 'preceding-sibling' | 'self' )
            int alt6 = 13;
            alt6 = dfa6.Predict(input);
            switch (alt6) 
            {
                case 1 :
                    // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmXPath.g:228:12: 'ancestor'
                    {
                    	Match("ancestor"); 


                    }
                    break;
                case 2 :
                    // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmXPath.g:229:6: 'ancestor-or-self'
                    {
                    	Match("ancestor-or-self"); 


                    }
                    break;
                case 3 :
                    // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmXPath.g:230:6: 'attribute'
                    {
                    	Match("attribute"); 


                    }
                    break;
                case 4 :
                    // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmXPath.g:231:6: 'child'
                    {
                    	Match("child"); 


                    }
                    break;
                case 5 :
                    // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmXPath.g:232:6: 'descendant'
                    {
                    	Match("descendant"); 


                    }
                    break;
                case 6 :
                    // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmXPath.g:233:6: 'descendant-or-self'
                    {
                    	Match("descendant-or-self"); 


                    }
                    break;
                case 7 :
                    // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmXPath.g:234:6: 'following'
                    {
                    	Match("following"); 


                    }
                    break;
                case 8 :
                    // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmXPath.g:235:6: 'following-sibling'
                    {
                    	Match("following-sibling"); 


                    }
                    break;
                case 9 :
                    // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmXPath.g:236:6: 'namespace'
                    {
                    	Match("namespace"); 


                    }
                    break;
                case 10 :
                    // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmXPath.g:237:6: 'parent'
                    {
                    	Match("parent"); 


                    }
                    break;
                case 11 :
                    // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmXPath.g:238:6: 'preceding'
                    {
                    	Match("preceding"); 


                    }
                    break;
                case 12 :
                    // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmXPath.g:239:6: 'preceding-sibling'
                    {
                    	Match("preceding-sibling"); 


                    }
                    break;
                case 13 :
                    // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmXPath.g:240:6: 'self'
                    {
                    	Match("self"); 


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
    // $ANTLR end "AxisName"

    // $ANTLR start "Literal"
    public void mLiteral() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = Literal;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmXPath.g:243:10: ( '\"' (~ '\"' )* '\"' | '\\'' (~ '\\'' )* '\\'' )
            int alt9 = 2;
            int LA9_0 = input.LA(1);

            if ( (LA9_0 == '\"') )
            {
                alt9 = 1;
            }
            else if ( (LA9_0 == '\'') )
            {
                alt9 = 2;
            }
            else 
            {
                NoViableAltException nvae_d9s0 =
                    new NoViableAltException("", 9, 0, input);

                throw nvae_d9s0;
            }
            switch (alt9) 
            {
                case 1 :
                    // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmXPath.g:243:13: '\"' (~ '\"' )* '\"'
                    {
                    	Match('\"'); 
                    	// D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmXPath.g:243:17: (~ '\"' )*
                    	do 
                    	{
                    	    int alt7 = 2;
                    	    int LA7_0 = input.LA(1);

                    	    if ( ((LA7_0 >= '\u0000' && LA7_0 <= '!') || (LA7_0 >= '#' && LA7_0 <= '\uFFFF')) )
                    	    {
                    	        alt7 = 1;
                    	    }


                    	    switch (alt7) 
                    		{
                    			case 1 :
                    			    // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmXPath.g:243:17: ~ '\"'
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
                    			    goto loop7;
                    	    }
                    	} while (true);

                    	loop7:
                    		;	// Stops C# compiler whining that label 'loop7' has no statements

                    	Match('\"'); 

                    }
                    break;
                case 2 :
                    // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmXPath.g:244:6: '\\'' (~ '\\'' )* '\\''
                    {
                    	Match('\''); 
                    	// D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmXPath.g:244:11: (~ '\\'' )*
                    	do 
                    	{
                    	    int alt8 = 2;
                    	    int LA8_0 = input.LA(1);

                    	    if ( ((LA8_0 >= '\u0000' && LA8_0 <= '&') || (LA8_0 >= '(' && LA8_0 <= '\uFFFF')) )
                    	    {
                    	        alt8 = 1;
                    	    }


                    	    switch (alt8) 
                    		{
                    			case 1 :
                    			    // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmXPath.g:244:11: ~ '\\''
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
                    			    goto loop8;
                    	    }
                    	} while (true);

                    	loop8:
                    		;	// Stops C# compiler whining that label 'loop8' has no statements

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
    // $ANTLR end "Literal"

    // $ANTLR start "Whitespace"
    public void mWhitespace() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = Whitespace;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmXPath.g:248:3: ( ( ' ' | '\\t' | '\\n' | '\\r' )+ )
            // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmXPath.g:248:6: ( ' ' | '\\t' | '\\n' | '\\r' )+
            {
            	// D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmXPath.g:248:6: ( ' ' | '\\t' | '\\n' | '\\r' )+
            	int cnt10 = 0;
            	do 
            	{
            	    int alt10 = 2;
            	    int LA10_0 = input.LA(1);

            	    if ( ((LA10_0 >= '\t' && LA10_0 <= '\n') || LA10_0 == '\r' || LA10_0 == ' ') )
            	    {
            	        alt10 = 1;
            	    }


            	    switch (alt10) 
            		{
            			case 1 :
            			    // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmXPath.g:
            			    {
            			    	if ( (input.LA(1) >= '\t' && input.LA(1) <= '\n') || input.LA(1) == '\r' || input.LA(1) == ' ' ) 
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
            			    if ( cnt10 >= 1 ) goto loop10;
            		            EarlyExitException eee10 =
            		                new EarlyExitException(10, input);
            		            throw eee10;
            	    }
            	    cnt10++;
            	} while (true);

            	loop10:
            		;	// Stops C# compiler whining that label 'loop10' has no statements

            	_channel = HIDDEN;

            }

            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "Whitespace"

    // $ANTLR start "NCName"
    public void mNCName() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = NCName;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmXPath.g:251:9: ( NCNameStartChar ( NCNameChar )* )
            // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmXPath.g:251:12: NCNameStartChar ( NCNameChar )*
            {
            	mNCNameStartChar(); 
            	// D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmXPath.g:251:28: ( NCNameChar )*
            	do 
            	{
            	    int alt11 = 2;
            	    int LA11_0 = input.LA(1);

            	    if ( ((LA11_0 >= '-' && LA11_0 <= '.') || (LA11_0 >= '0' && LA11_0 <= '9') || (LA11_0 >= 'A' && LA11_0 <= 'Z') || LA11_0 == '_' || (LA11_0 >= 'a' && LA11_0 <= 'z') || LA11_0 == '\u00B7' || (LA11_0 >= '\u00C0' && LA11_0 <= '\u00D6') || (LA11_0 >= '\u00D8' && LA11_0 <= '\u00F6') || (LA11_0 >= '\u00F8' && LA11_0 <= '\u037D') || (LA11_0 >= '\u037F' && LA11_0 <= '\u1FFF') || (LA11_0 >= '\u200C' && LA11_0 <= '\u200D') || (LA11_0 >= '\u203F' && LA11_0 <= '\u2040') || (LA11_0 >= '\u2070' && LA11_0 <= '\u218F') || (LA11_0 >= '\u2C00' && LA11_0 <= '\u2FEF') || (LA11_0 >= '\u3001' && LA11_0 <= '\uD7FF') || (LA11_0 >= '\uF900' && LA11_0 <= '\uFDCF') || (LA11_0 >= '\uFDF0' && LA11_0 <= '\uFFFD')) )
            	    {
            	        alt11 = 1;
            	    }


            	    switch (alt11) 
            		{
            			case 1 :
            			    // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmXPath.g:251:28: NCNameChar
            			    {
            			    	mNCNameChar(); 

            			    }
            			    break;

            			default:
            			    goto loop11;
            	    }
            	} while (true);

            	loop11:
            		;	// Stops C# compiler whining that label 'loop11' has no statements


            }

            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "NCName"

    // $ANTLR start "NCNameStartChar"
    public void mNCNameStartChar() // throws RecognitionException [2]
    {
    		try
    		{
            // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmXPath.g:256:3: ( 'A' .. 'Z' | '_' | 'a' .. 'z' | '\\u00C0' .. '\\u00D6' | '\\u00D8' .. '\\u00F6' | '\\u00F8' .. '\\u02FF' | '\\u0370' .. '\\u037D' | '\\u037F' .. '\\u1FFF' | '\\u200C' .. '\\u200D' | '\\u2070' .. '\\u218F' | '\\u2C00' .. '\\u2FEF' | '\\u3001' .. '\\uD7FF' | '\\uF900' .. '\\uFDCF' | '\\uFDF0' .. '\\uFFFD' )
            // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmXPath.g:
            {
            	if ( (input.LA(1) >= 'A' && input.LA(1) <= 'Z') || input.LA(1) == '_' || (input.LA(1) >= 'a' && input.LA(1) <= 'z') || (input.LA(1) >= '\u00C0' && input.LA(1) <= '\u00D6') || (input.LA(1) >= '\u00D8' && input.LA(1) <= '\u00F6') || (input.LA(1) >= '\u00F8' && input.LA(1) <= '\u02FF') || (input.LA(1) >= '\u0370' && input.LA(1) <= '\u037D') || (input.LA(1) >= '\u037F' && input.LA(1) <= '\u1FFF') || (input.LA(1) >= '\u200C' && input.LA(1) <= '\u200D') || (input.LA(1) >= '\u2070' && input.LA(1) <= '\u218F') || (input.LA(1) >= '\u2C00' && input.LA(1) <= '\u2FEF') || (input.LA(1) >= '\u3001' && input.LA(1) <= '\uD7FF') || (input.LA(1) >= '\uF900' && input.LA(1) <= '\uFDCF') || (input.LA(1) >= '\uFDF0' && input.LA(1) <= '\uFFFD') ) 
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
    // $ANTLR end "NCNameStartChar"

    // $ANTLR start "NCNameChar"
    public void mNCNameChar() // throws RecognitionException [2]
    {
    		try
    		{
            // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmXPath.g:277:3: ( NCNameStartChar | '-' | '.' | '0' .. '9' | '\\u00B7' | '\\u0300' .. '\\u036F' | '\\u203F' .. '\\u2040' )
            // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmXPath.g:
            {
            	if ( (input.LA(1) >= '-' && input.LA(1) <= '.') || (input.LA(1) >= '0' && input.LA(1) <= '9') || (input.LA(1) >= 'A' && input.LA(1) <= 'Z') || input.LA(1) == '_' || (input.LA(1) >= 'a' && input.LA(1) <= 'z') || input.LA(1) == '\u00B7' || (input.LA(1) >= '\u00C0' && input.LA(1) <= '\u00D6') || (input.LA(1) >= '\u00D8' && input.LA(1) <= '\u00F6') || (input.LA(1) >= '\u00F8' && input.LA(1) <= '\u037D') || (input.LA(1) >= '\u037F' && input.LA(1) <= '\u1FFF') || (input.LA(1) >= '\u200C' && input.LA(1) <= '\u200D') || (input.LA(1) >= '\u203F' && input.LA(1) <= '\u2040') || (input.LA(1) >= '\u2070' && input.LA(1) <= '\u218F') || (input.LA(1) >= '\u2C00' && input.LA(1) <= '\u2FEF') || (input.LA(1) >= '\u3001' && input.LA(1) <= '\uD7FF') || (input.LA(1) >= '\uF900' && input.LA(1) <= '\uFDCF') || (input.LA(1) >= '\uFDF0' && input.LA(1) <= '\uFFFD') ) 
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
    // $ANTLR end "NCNameChar"

    override public void mTokens() // throws RecognitionException 
    {
        // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmXPath.g:1:8: ( PATHSEP | ABRPATH | LPAR | RPAR | LBRAC | RBRAC | MINUS | PLUS | DOT | MUL | DOTDOT | AT | COMMA | PIPE | LESS | MORE | LE | GE | COLON | CC | APOS | QUOT | T__46 | T__47 | T__48 | T__49 | T__50 | T__51 | T__52 | T__53 | T__54 | T__55 | T__56 | T__57 | NodeType | Number | AxisName | Literal | Whitespace | NCName )
        int alt12 = 40;
        alt12 = dfa12.Predict(input);
        switch (alt12) 
        {
            case 1 :
                // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmXPath.g:1:10: PATHSEP
                {
                	mPATHSEP(); 

                }
                break;
            case 2 :
                // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmXPath.g:1:18: ABRPATH
                {
                	mABRPATH(); 

                }
                break;
            case 3 :
                // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmXPath.g:1:26: LPAR
                {
                	mLPAR(); 

                }
                break;
            case 4 :
                // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmXPath.g:1:31: RPAR
                {
                	mRPAR(); 

                }
                break;
            case 5 :
                // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmXPath.g:1:36: LBRAC
                {
                	mLBRAC(); 

                }
                break;
            case 6 :
                // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmXPath.g:1:42: RBRAC
                {
                	mRBRAC(); 

                }
                break;
            case 7 :
                // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmXPath.g:1:48: MINUS
                {
                	mMINUS(); 

                }
                break;
            case 8 :
                // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmXPath.g:1:54: PLUS
                {
                	mPLUS(); 

                }
                break;
            case 9 :
                // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmXPath.g:1:59: DOT
                {
                	mDOT(); 

                }
                break;
            case 10 :
                // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmXPath.g:1:63: MUL
                {
                	mMUL(); 

                }
                break;
            case 11 :
                // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmXPath.g:1:67: DOTDOT
                {
                	mDOTDOT(); 

                }
                break;
            case 12 :
                // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmXPath.g:1:74: AT
                {
                	mAT(); 

                }
                break;
            case 13 :
                // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmXPath.g:1:77: COMMA
                {
                	mCOMMA(); 

                }
                break;
            case 14 :
                // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmXPath.g:1:83: PIPE
                {
                	mPIPE(); 

                }
                break;
            case 15 :
                // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmXPath.g:1:88: LESS
                {
                	mLESS(); 

                }
                break;
            case 16 :
                // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmXPath.g:1:93: MORE
                {
                	mMORE(); 

                }
                break;
            case 17 :
                // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmXPath.g:1:98: LE
                {
                	mLE(); 

                }
                break;
            case 18 :
                // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmXPath.g:1:101: GE
                {
                	mGE(); 

                }
                break;
            case 19 :
                // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmXPath.g:1:104: COLON
                {
                	mCOLON(); 

                }
                break;
            case 20 :
                // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmXPath.g:1:110: CC
                {
                	mCC(); 

                }
                break;
            case 21 :
                // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmXPath.g:1:113: APOS
                {
                	mAPOS(); 

                }
                break;
            case 22 :
                // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmXPath.g:1:118: QUOT
                {
                	mQUOT(); 

                }
                break;
            case 23 :
                // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmXPath.g:1:123: T__46
                {
                	mT__46(); 

                }
                break;
            case 24 :
                // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmXPath.g:1:129: T__47
                {
                	mT__47(); 

                }
                break;
            case 25 :
                // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmXPath.g:1:135: T__48
                {
                	mT__48(); 

                }
                break;
            case 26 :
                // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmXPath.g:1:141: T__49
                {
                	mT__49(); 

                }
                break;
            case 27 :
                // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmXPath.g:1:147: T__50
                {
                	mT__50(); 

                }
                break;
            case 28 :
                // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmXPath.g:1:153: T__51
                {
                	mT__51(); 

                }
                break;
            case 29 :
                // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmXPath.g:1:159: T__52
                {
                	mT__52(); 

                }
                break;
            case 30 :
                // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmXPath.g:1:165: T__53
                {
                	mT__53(); 

                }
                break;
            case 31 :
                // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmXPath.g:1:171: T__54
                {
                	mT__54(); 

                }
                break;
            case 32 :
                // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmXPath.g:1:177: T__55
                {
                	mT__55(); 

                }
                break;
            case 33 :
                // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmXPath.g:1:183: T__56
                {
                	mT__56(); 

                }
                break;
            case 34 :
                // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmXPath.g:1:189: T__57
                {
                	mT__57(); 

                }
                break;
            case 35 :
                // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmXPath.g:1:195: NodeType
                {
                	mNodeType(); 

                }
                break;
            case 36 :
                // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmXPath.g:1:204: Number
                {
                	mNumber(); 

                }
                break;
            case 37 :
                // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmXPath.g:1:211: AxisName
                {
                	mAxisName(); 

                }
                break;
            case 38 :
                // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmXPath.g:1:220: Literal
                {
                	mLiteral(); 

                }
                break;
            case 39 :
                // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmXPath.g:1:228: Whitespace
                {
                	mWhitespace(); 

                }
                break;
            case 40 :
                // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmXPath.g:1:239: NCName
                {
                	mNCName(); 

                }
                break;

        }

    }


    protected DFA6 dfa6;
    protected DFA12 dfa12;
	private void InitializeCyclicDFAs()
	{
	    this.dfa6 = new DFA6(this);
	    this.dfa12 = new DFA12(this);
	    this.dfa12.specialStateTransitionHandler = new DFA.SpecialStateTransitionHandler(DFA12_SpecialStateTransition);
	}

    const string DFA6_eotS =
        "\x22\uffff\x01\x27\x06\uffff\x01\x2d\x01\x2f\x01\x31\x06\uffff";
    const string DFA6_eofS =
        "\x32\uffff";
    const string DFA6_minS =
        "\x01\x61\x01\x6e\x01\uffff\x01\x65\x01\x6f\x01\uffff\x01\x61\x01"+
        "\uffff\x01\x63\x01\uffff\x01\x73\x01\x6c\x01\uffff\x02\x65\x01\x63"+
        "\x01\x6c\x01\x63\x01\x73\x01\x65\x01\x6f\x01\x65\x01\x74\x01\x6e"+
        "\x01\x77\x01\x64\x01\x6f\x01\x64\x02\x69\x01\x72\x01\x61\x02\x6e"+
        "\x01\x2d\x01\x6e\x02\x67\x02\uffff\x01\x74\x03\x2d\x06\uffff";
    const string DFA6_maxS =
        "\x01\x73\x01\x74\x01\uffff\x01\x65\x01\x6f\x01\uffff\x01\x72\x01"+
        "\uffff\x01\x63\x01\uffff\x01\x73\x01\x6c\x01\uffff\x02\x65\x01\x63"+
        "\x01\x6c\x01\x63\x01\x73\x01\x65\x01\x6f\x01\x65\x01\x74\x01\x6e"+
        "\x01\x77\x01\x64\x01\x6f\x01\x64\x02\x69\x01\x72\x01\x61\x02\x6e"+
        "\x01\x2d\x01\x6e\x02\x67\x02\uffff\x01\x74\x03\x2d\x06\uffff";
    const string DFA6_acceptS =
        "\x02\uffff\x01\x04\x02\uffff\x01\x09\x01\uffff\x01\x0d\x01\uffff"+
        "\x01\x03\x02\uffff\x01\x0a\x19\uffff\x01\x02\x01\x01\x04\uffff\x01"+
        "\x08\x01\x07\x01\x0c\x01\x0b\x01\x06\x01\x05";
    const string DFA6_specialS =
        "\x32\uffff}>";
    static readonly string[] DFA6_transitionS = {
            "\x01\x01\x01\uffff\x01\x02\x01\x03\x01\uffff\x01\x04\x07\uffff"+
            "\x01\x05\x01\uffff\x01\x06\x02\uffff\x01\x07",
            "\x01\x08\x05\uffff\x01\x09",
            "",
            "\x01\x0a",
            "\x01\x0b",
            "",
            "\x01\x0c\x10\uffff\x01\x0d",
            "",
            "\x01\x0e",
            "",
            "\x01\x0f",
            "\x01\x10",
            "",
            "\x01\x11",
            "\x01\x12",
            "\x01\x13",
            "\x01\x14",
            "\x01\x15",
            "\x01\x16",
            "\x01\x17",
            "\x01\x18",
            "\x01\x19",
            "\x01\x1a",
            "\x01\x1b",
            "\x01\x1c",
            "\x01\x1d",
            "\x01\x1e",
            "\x01\x1f",
            "\x01\x20",
            "\x01\x21",
            "\x01\x22",
            "\x01\x23",
            "\x01\x24",
            "\x01\x25",
            "\x01\x26",
            "\x01\x28",
            "\x01\x29",
            "\x01\x2a",
            "",
            "",
            "\x01\x2b",
            "\x01\x2c",
            "\x01\x2e",
            "\x01\x30",
            "",
            "",
            "",
            "",
            "",
            ""
    };

    static readonly short[] DFA6_eot = DFA.UnpackEncodedString(DFA6_eotS);
    static readonly short[] DFA6_eof = DFA.UnpackEncodedString(DFA6_eofS);
    static readonly char[] DFA6_min = DFA.UnpackEncodedStringToUnsignedChars(DFA6_minS);
    static readonly char[] DFA6_max = DFA.UnpackEncodedStringToUnsignedChars(DFA6_maxS);
    static readonly short[] DFA6_accept = DFA.UnpackEncodedString(DFA6_acceptS);
    static readonly short[] DFA6_special = DFA.UnpackEncodedString(DFA6_specialS);
    static readonly short[][] DFA6_transition = DFA.UnpackEncodedStringArray(DFA6_transitionS);

    protected class DFA6 : DFA
    {
        public DFA6(BaseRecognizer recognizer)
        {
            this.recognizer = recognizer;
            this.decisionNumber = 6;
            this.eot = DFA6_eot;
            this.eof = DFA6_eof;
            this.min = DFA6_min;
            this.max = DFA6_max;
            this.accept = DFA6_accept;
            this.special = DFA6_special;
            this.transition = DFA6_transition;

        }

        override public string Description
        {
            get { return "228:1: AxisName : ( 'ancestor' | 'ancestor-or-self' | 'attribute' | 'child' | 'descendant' | 'descendant-or-self' | 'following' | 'following-sibling' | 'namespace' | 'parent' | 'preceding' | 'preceding-sibling' | 'self' );"; }
        }

    }

    const string DFA12_eotS =
        "\x01\uffff\x01\x25\x06\uffff\x01\x27\x04\uffff\x01\x29\x01\x2b"+
        "\x01\x2d\x01\x2f\x01\x30\x03\x21\x02\uffff\x02\x21\x01\uffff\x03"+
        "\x21\x01\uffff\x02\x21\x02\uffff\x01\x41\x01\x43\x0d\uffff\x02\x21"+
        "\x01\x47\x0c\x21\x04\uffff\x03\x21\x01\uffff\x01\x58\x02\x21\x01"+
        "\x5b\x01\x21\x01\x5d\x0a\x21\x01\uffff\x02\x21\x01\uffff\x01\x21"+
        "\x01\uffff\x02\x21\x02\x6d\x02\x21\x01\x70\x07\x21\x01\x70\x01\uffff"+
        "\x02\x21\x01\uffff\x02\x21\x01\x70\x0b\x21\x01\x6d\x04\x21\x01\x70"+
        "\x05\x21\x01\x70\x01\x21\x01\x70\x01\x21\x02\x70\x03\x21\x01\x70"+
        "\x1c\x21\x01\x70\x03\x21\x01\x70\x01\x21\x01\x70\x01\x21\x01\x70"+
        "\x03\x21\x01\u00bd\x01\uffff";
    const string DFA12_eofS =
        "\u00be\uffff";
    const string DFA12_minS =
        "\x01\x09\x01\x2e\x06\uffff\x01\x2e\x04\uffff\x02\x3d\x01\x3a\x02"+
        "\x00\x01\x61\x01\x72\x01\x6e\x02\uffff\x01\x65\x01\x6f\x01\uffff"+
        "\x01\x68\x01\x65\x01\x61\x01\uffff\x01\x6f\x01\x65\x02\uffff\x01"+
        "\x40\x01\x2e\x0d\uffff\x01\x65\x01\x72\x01\x2d\x01\x63\x01\x74\x01"+
        "\x76\x01\x73\x01\x64\x01\x6d\x01\x69\x01\x78\x01\x64\x01\x6d\x02"+
        "\x6c\x04\uffff\x02\x63\x01\x65\x01\uffff\x01\x2d\x01\x65\x01\x72"+
        "\x01\x2d\x01\x63\x01\x2d\x01\x6d\x01\x6c\x01\x74\x02\x65\x01\x6c"+
        "\x01\x66\x02\x65\x01\x6e\x01\uffff\x01\x73\x01\x69\x01\uffff\x01"+
        "\x65\x01\uffff\x01\x65\x01\x64\x02\x2d\x01\x73\x01\x6f\x01\x2d\x01"+
        "\x73\x01\x64\x02\x74\x01\x62\x02\x6e\x01\x2d\x01\uffff\x01\x70\x01"+
        "\x77\x01\uffff\x01\x73\x01\x69\x01\x2d\x01\x6f\x01\x75\x01\x64\x01"+
        "\x74\x01\x61\x02\x69\x01\x6e\x01\x72\x01\x74\x01\x61\x01\x2d\x01"+
        "\x63\x02\x6e\x01\x67\x01\x2d\x01\x65\x01\x6e\x01\x65\x02\x67\x01"+
        "\x2d\x01\x6f\x01\x2d\x01\x74\x03\x2d\x01\x73\x01\x72\x01\x2d\x01"+
        "\x73\x02\x69\x01\x2d\x01\x6f\x01\x69\x01\x6e\x01\x62\x01\x73\x01"+
        "\x72\x01\x62\x01\x73\x01\x6c\x01\x65\x01\x2d\x01\x6c\x01\x74\x01"+
        "\x69\x01\x6c\x01\x73\x01\x69\x01\x72\x01\x6e\x01\x66\x01\x65\x01"+
        "\x6e\x01\x75\x01\x67\x01\x2d\x01\x6c\x01\x67\x01\x63\x01\x2d\x01"+
        "\x66\x01\x2d\x01\x74\x01\x2d\x01\x69\x01\x6f\x01\x6e\x01\x2d\x01"+
        "\uffff";
    const string DFA12_maxS =
        "\x01\ufffd\x01\x40\x06\uffff\x01\x39\x04\uffff\x02\x3d\x01\x3a"+
        "\x02\uffff\x02\x72\x01\x74\x02\uffff\x01\x69\x01\x6f\x01\uffff\x01"+
        "\x6f\x01\x65\x01\x6f\x01\uffff\x01\x6f\x01\x65\x02\uffff\x01\x40"+
        "\x01\x2e\x0d\uffff\x01\x6f\x01\x72\x01\ufffd\x01\x64\x01\x74\x01"+
        "\x76\x01\x73\x01\x64\x01\x6d\x01\x69\x01\x78\x01\x64\x01\x6d\x02"+
        "\x6c\x04\uffff\x02\x63\x01\x65\x01\uffff\x01\ufffd\x01\x65\x01\x72"+
        "\x01\ufffd\x01\x63\x01\ufffd\x01\x6d\x01\x6c\x01\x74\x02\x65\x01"+
        "\x6c\x01\x66\x02\x65\x01\x6e\x01\uffff\x01\x73\x01\x69\x01\uffff"+
        "\x01\x65\x01\uffff\x01\x65\x01\x64\x02\ufffd\x01\x73\x01\x6f\x01"+
        "\ufffd\x01\x73\x01\x64\x02\x74\x01\x62\x02\x6e\x01\ufffd\x01\uffff"+
        "\x01\x70\x01\x77\x01\uffff\x01\x73\x01\x69\x01\ufffd\x01\x6f\x01"+
        "\x75\x01\x64\x01\x74\x01\x61\x02\x69\x01\x6e\x01\x72\x01\x74\x01"+
        "\x61\x01\ufffd\x01\x63\x02\x6e\x01\x67\x01\ufffd\x01\x65\x01\x6e"+
        "\x01\x65\x02\x67\x01\ufffd\x01\x6f\x01\ufffd\x01\x74\x02\ufffd\x01"+
        "\x2d\x01\x73\x01\x72\x01\ufffd\x01\x73\x02\x69\x01\x2d\x01\x6f\x01"+
        "\x69\x01\x6e\x01\x62\x01\x73\x01\x72\x01\x62\x01\x73\x01\x6c\x01"+
        "\x65\x01\x2d\x01\x6c\x01\x74\x01\x69\x01\x6c\x01\x73\x01\x69\x01"+
        "\x72\x01\x6e\x01\x66\x01\x65\x01\x6e\x01\x75\x01\x67\x01\ufffd\x01"+
        "\x6c\x01\x67\x01\x63\x01\ufffd\x01\x66\x01\ufffd\x01\x74\x01\ufffd"+
        "\x01\x69\x01\x6f\x01\x6e\x01\ufffd\x01\uffff";
    const string DFA12_acceptS =
        "\x02\uffff\x01\x03\x01\x04\x01\x05\x01\x06\x01\x07\x01\x08\x01"+
        "\uffff\x01\x0a\x01\x0c\x01\x0d\x01\x0e\x08\uffff\x01\x1e\x01\x1f"+
        "\x02\uffff\x01\x22\x03\uffff\x01\x24\x02\uffff\x01\x27\x01\x28\x02"+
        "\uffff\x01\x19\x01\x01\x01\x0b\x01\x09\x01\x11\x01\x0f\x01\x12\x01"+
        "\x10\x01\x14\x01\x13\x01\x26\x01\x15\x01\x16\x0f\uffff\x01\x1a\x01"+
        "\x02\x01\x17\x01\x18\x03\uffff\x01\x1c\x10\uffff\x01\x1d\x02\uffff"+
        "\x01\x20\x01\uffff\x01\x21\x0f\uffff\x01\x23\x02\uffff\x01\x25\x4c"+
        "\uffff\x01\x1b";
    const string DFA12_specialS =
        "\x10\uffff\x01\x00\x01\x01\u00ac\uffff}>";
    static readonly string[] DFA12_transitionS = {
            "\x02\x20\x02\uffff\x01\x20\x12\uffff\x01\x20\x01\x16\x01\x11"+
            "\x01\uffff\x01\x19\x02\uffff\x01\x10\x01\x02\x01\x03\x01\x09"+
            "\x01\x07\x01\x0b\x01\x06\x01\x08\x01\x01\x0a\x1d\x01\x0f\x01"+
            "\uffff\x01\x0d\x01\x15\x01\x0e\x01\uffff\x01\x0a\x1a\x21\x01"+
            "\x04\x01\uffff\x01\x05\x01\uffff\x01\x21\x01\uffff\x01\x14\x01"+
            "\x21\x01\x1a\x01\x17\x01\x21\x01\x1e\x06\x21\x01\x18\x01\x1c"+
            "\x01\x13\x01\x12\x02\x21\x01\x1f\x01\x1b\x06\x21\x01\uffff\x01"+
            "\x0c\x43\uffff\x17\x21\x01\uffff\x1f\x21\x01\uffff\u0208\x21"+
            "\x70\uffff\x0e\x21\x01\uffff\u1c81\x21\x0c\uffff\x02\x21\x62"+
            "\uffff\u0120\x21\u0a70\uffff\u03f0\x21\x11\uffff\ua7ff\x21\u2100"+
            "\uffff\u04d0\x21\x20\uffff\u020e\x21",
            "\x01\x23\x01\x22\x10\uffff\x01\x24",
            "",
            "",
            "",
            "",
            "",
            "",
            "\x01\x26\x01\uffff\x0a\x1d",
            "",
            "",
            "",
            "",
            "\x01\x28",
            "\x01\x2a",
            "\x01\x2c",
            "\x00\x2e",
            "\x00\x2e",
            "\x01\x32\x10\uffff\x01\x31",
            "\x01\x33",
            "\x01\x34\x05\uffff\x01\x35",
            "",
            "",
            "\x01\x37\x03\uffff\x01\x36",
            "\x01\x38",
            "",
            "\x01\x3a\x06\uffff\x01\x39",
            "\x01\x3b",
            "\x01\x3d\x0d\uffff\x01\x3c",
            "",
            "\x01\x3e",
            "\x01\x3f",
            "",
            "",
            "\x01\x40",
            "\x01\x42",
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
            "\x01\x45\x09\uffff\x01\x44",
            "\x01\x46",
            "\x02\x21\x01\uffff\x0a\x21\x07\uffff\x1a\x21\x04\uffff\x01"+
            "\x21\x01\uffff\x1a\x21\x3c\uffff\x01\x21\x08\uffff\x17\x21\x01"+
            "\uffff\x1f\x21\x01\uffff\u0286\x21\x01\uffff\u1c81\x21\x0c\uffff"+
            "\x02\x21\x31\uffff\x02\x21\x2f\uffff\u0120\x21\u0a70\uffff\u03f0"+
            "\x21\x11\uffff\ua7ff\x21\u2100\uffff\u04d0\x21\x20\uffff\u020e"+
            "\x21",
            "\x01\x49\x01\x48",
            "\x01\x4a",
            "\x01\x4b",
            "\x01\x4c",
            "\x01\x4d",
            "\x01\x4e",
            "\x01\x4f",
            "\x01\x50",
            "\x01\x51",
            "\x01\x52",
            "\x01\x53",
            "\x01\x54",
            "",
            "",
            "",
            "",
            "\x01\x55",
            "\x01\x56",
            "\x01\x57",
            "",
            "\x02\x21\x01\uffff\x0a\x21\x07\uffff\x1a\x21\x04\uffff\x01"+
            "\x21\x01\uffff\x1a\x21\x3c\uffff\x01\x21\x08\uffff\x17\x21\x01"+
            "\uffff\x1f\x21\x01\uffff\u0286\x21\x01\uffff\u1c81\x21\x0c\uffff"+
            "\x02\x21\x31\uffff\x02\x21\x2f\uffff\u0120\x21\u0a70\uffff\u03f0"+
            "\x21\x11\uffff\ua7ff\x21\u2100\uffff\u04d0\x21\x20\uffff\u020e"+
            "\x21",
            "\x01\x59",
            "\x01\x5a",
            "\x02\x21\x01\uffff\x0a\x21\x07\uffff\x1a\x21\x04\uffff\x01"+
            "\x21\x01\uffff\x1a\x21\x3c\uffff\x01\x21\x08\uffff\x17\x21\x01"+
            "\uffff\x1f\x21\x01\uffff\u0286\x21\x01\uffff\u1c81\x21\x0c\uffff"+
            "\x02\x21\x31\uffff\x02\x21\x2f\uffff\u0120\x21\u0a70\uffff\u03f0"+
            "\x21\x11\uffff\ua7ff\x21\u2100\uffff\u04d0\x21\x20\uffff\u020e"+
            "\x21",
            "\x01\x5c",
            "\x02\x21\x01\uffff\x0a\x21\x07\uffff\x1a\x21\x04\uffff\x01"+
            "\x21\x01\uffff\x1a\x21\x3c\uffff\x01\x21\x08\uffff\x17\x21\x01"+
            "\uffff\x1f\x21\x01\uffff\u0286\x21\x01\uffff\u1c81\x21\x0c\uffff"+
            "\x02\x21\x31\uffff\x02\x21\x2f\uffff\u0120\x21\u0a70\uffff\u03f0"+
            "\x21\x11\uffff\ua7ff\x21\u2100\uffff\u04d0\x21\x20\uffff\u020e"+
            "\x21",
            "\x01\x5e",
            "\x01\x5f",
            "\x01\x60",
            "\x01\x61",
            "\x01\x62",
            "\x01\x63",
            "\x01\x64",
            "\x01\x65",
            "\x01\x66",
            "\x01\x67",
            "",
            "\x01\x68",
            "\x01\x69",
            "",
            "\x01\x6a",
            "",
            "\x01\x6b",
            "\x01\x6c",
            "\x02\x21\x01\uffff\x0a\x21\x07\uffff\x1a\x21\x04\uffff\x01"+
            "\x21\x01\uffff\x1a\x21\x3c\uffff\x01\x21\x08\uffff\x17\x21\x01"+
            "\uffff\x1f\x21\x01\uffff\u0286\x21\x01\uffff\u1c81\x21\x0c\uffff"+
            "\x02\x21\x31\uffff\x02\x21\x2f\uffff\u0120\x21\u0a70\uffff\u03f0"+
            "\x21\x11\uffff\ua7ff\x21\u2100\uffff\u04d0\x21\x20\uffff\u020e"+
            "\x21",
            "\x02\x21\x01\uffff\x0a\x21\x07\uffff\x1a\x21\x04\uffff\x01"+
            "\x21\x01\uffff\x1a\x21\x3c\uffff\x01\x21\x08\uffff\x17\x21\x01"+
            "\uffff\x1f\x21\x01\uffff\u0286\x21\x01\uffff\u1c81\x21\x0c\uffff"+
            "\x02\x21\x31\uffff\x02\x21\x2f\uffff\u0120\x21\u0a70\uffff\u03f0"+
            "\x21\x11\uffff\ua7ff\x21\u2100\uffff\u04d0\x21\x20\uffff\u020e"+
            "\x21",
            "\x01\x6e",
            "\x01\x6f",
            "\x02\x21\x01\uffff\x0a\x21\x07\uffff\x1a\x21\x04\uffff\x01"+
            "\x21\x01\uffff\x1a\x21\x3c\uffff\x01\x21\x08\uffff\x17\x21\x01"+
            "\uffff\x1f\x21\x01\uffff\u0286\x21\x01\uffff\u1c81\x21\x0c\uffff"+
            "\x02\x21\x31\uffff\x02\x21\x2f\uffff\u0120\x21\u0a70\uffff\u03f0"+
            "\x21\x11\uffff\ua7ff\x21\u2100\uffff\u04d0\x21\x20\uffff\u020e"+
            "\x21",
            "\x01\x71",
            "\x01\x72",
            "\x01\x73",
            "\x01\x74",
            "\x01\x75",
            "\x01\x76",
            "\x01\x77",
            "\x02\x21\x01\uffff\x0a\x21\x07\uffff\x1a\x21\x04\uffff\x01"+
            "\x21\x01\uffff\x1a\x21\x3c\uffff\x01\x21\x08\uffff\x17\x21\x01"+
            "\uffff\x1f\x21\x01\uffff\u0286\x21\x01\uffff\u1c81\x21\x0c\uffff"+
            "\x02\x21\x31\uffff\x02\x21\x2f\uffff\u0120\x21\u0a70\uffff\u03f0"+
            "\x21\x11\uffff\ua7ff\x21\u2100\uffff\u04d0\x21\x20\uffff\u020e"+
            "\x21",
            "",
            "\x01\x78",
            "\x01\x79",
            "",
            "\x01\x7a",
            "\x01\x7b",
            "\x02\x21\x01\uffff\x0a\x21\x07\uffff\x1a\x21\x04\uffff\x01"+
            "\x21\x01\uffff\x1a\x21\x3c\uffff\x01\x21\x08\uffff\x17\x21\x01"+
            "\uffff\x1f\x21\x01\uffff\u0286\x21\x01\uffff\u1c81\x21\x0c\uffff"+
            "\x02\x21\x31\uffff\x02\x21\x2f\uffff\u0120\x21\u0a70\uffff\u03f0"+
            "\x21\x11\uffff\ua7ff\x21\u2100\uffff\u04d0\x21\x20\uffff\u020e"+
            "\x21",
            "\x01\x7c",
            "\x01\x7d",
            "\x01\x7e",
            "\x01\x7f",
            "\x01\u0080",
            "\x01\u0081",
            "\x01\u0082",
            "\x01\u0083",
            "\x01\u0084",
            "\x01\u0085",
            "\x01\u0086",
            "\x02\x21\x01\uffff\x0a\x21\x07\uffff\x1a\x21\x04\uffff\x01"+
            "\x21\x01\uffff\x1a\x21\x3c\uffff\x01\x21\x08\uffff\x17\x21\x01"+
            "\uffff\x1f\x21\x01\uffff\u0286\x21\x01\uffff\u1c81\x21\x0c\uffff"+
            "\x02\x21\x31\uffff\x02\x21\x2f\uffff\u0120\x21\u0a70\uffff\u03f0"+
            "\x21\x11\uffff\ua7ff\x21\u2100\uffff\u04d0\x21\x20\uffff\u020e"+
            "\x21",
            "\x01\u0087",
            "\x01\u0088",
            "\x01\u0089",
            "\x01\u008a",
            "\x01\u008b\x01\x21\x01\uffff\x0a\x21\x07\uffff\x1a\x21\x04"+
            "\uffff\x01\x21\x01\uffff\x1a\x21\x3c\uffff\x01\x21\x08\uffff"+
            "\x17\x21\x01\uffff\x1f\x21\x01\uffff\u0286\x21\x01\uffff\u1c81"+
            "\x21\x0c\uffff\x02\x21\x31\uffff\x02\x21\x2f\uffff\u0120\x21"+
            "\u0a70\uffff\u03f0\x21\x11\uffff\ua7ff\x21\u2100\uffff\u04d0"+
            "\x21\x20\uffff\u020e\x21",
            "\x01\u008c",
            "\x01\u008d",
            "\x01\u008e",
            "\x01\u008f",
            "\x01\u0090",
            "\x01\u0091\x01\x21\x01\uffff\x0a\x21\x07\uffff\x1a\x21\x04"+
            "\uffff\x01\x21\x01\uffff\x1a\x21\x3c\uffff\x01\x21\x08\uffff"+
            "\x17\x21\x01\uffff\x1f\x21\x01\uffff\u0286\x21\x01\uffff\u1c81"+
            "\x21\x0c\uffff\x02\x21\x31\uffff\x02\x21\x2f\uffff\u0120\x21"+
            "\u0a70\uffff\u03f0\x21\x11\uffff\ua7ff\x21\u2100\uffff\u04d0"+
            "\x21\x20\uffff\u020e\x21",
            "\x01\u0092",
            "\x02\x21\x01\uffff\x0a\x21\x07\uffff\x1a\x21\x04\uffff\x01"+
            "\x21\x01\uffff\x1a\x21\x3c\uffff\x01\x21\x08\uffff\x17\x21\x01"+
            "\uffff\x1f\x21\x01\uffff\u0286\x21\x01\uffff\u1c81\x21\x0c\uffff"+
            "\x02\x21\x31\uffff\x02\x21\x2f\uffff\u0120\x21\u0a70\uffff\u03f0"+
            "\x21\x11\uffff\ua7ff\x21\u2100\uffff\u04d0\x21\x20\uffff\u020e"+
            "\x21",
            "\x01\u0093",
            "\x02\x21\x01\uffff\x0a\x21\x07\uffff\x1a\x21\x04\uffff\x01"+
            "\x21\x01\uffff\x1a\x21\x3c\uffff\x01\x21\x08\uffff\x17\x21\x01"+
            "\uffff\x1f\x21\x01\uffff\u0286\x21\x01\uffff\u1c81\x21\x0c\uffff"+
            "\x02\x21\x31\uffff\x02\x21\x2f\uffff\u0120\x21\u0a70\uffff\u03f0"+
            "\x21\x11\uffff\ua7ff\x21\u2100\uffff\u04d0\x21\x20\uffff\u020e"+
            "\x21",
            "\x01\u0094\x01\x21\x01\uffff\x0a\x21\x07\uffff\x1a\x21\x04"+
            "\uffff\x01\x21\x01\uffff\x1a\x21\x3c\uffff\x01\x21\x08\uffff"+
            "\x17\x21\x01\uffff\x1f\x21\x01\uffff\u0286\x21\x01\uffff\u1c81"+
            "\x21\x0c\uffff\x02\x21\x31\uffff\x02\x21\x2f\uffff\u0120\x21"+
            "\u0a70\uffff\u03f0\x21\x11\uffff\ua7ff\x21\u2100\uffff\u04d0"+
            "\x21\x20\uffff\u020e\x21",
            "\x01\u0095",
            "\x01\u0096",
            "\x01\u0097",
            "\x01\u0098\x01\x21\x01\uffff\x0a\x21\x07\uffff\x1a\x21\x04"+
            "\uffff\x01\x21\x01\uffff\x1a\x21\x3c\uffff\x01\x21\x08\uffff"+
            "\x17\x21\x01\uffff\x1f\x21\x01\uffff\u0286\x21\x01\uffff\u1c81"+
            "\x21\x0c\uffff\x02\x21\x31\uffff\x02\x21\x2f\uffff\u0120\x21"+
            "\u0a70\uffff\u03f0\x21\x11\uffff\ua7ff\x21\u2100\uffff\u04d0"+
            "\x21\x20\uffff\u020e\x21",
            "\x01\u0099",
            "\x01\u009a",
            "\x01\u009b",
            "\x01\u009c",
            "\x01\u009d",
            "\x01\u009e",
            "\x01\u009f",
            "\x01\u00a0",
            "\x01\u00a1",
            "\x01\u00a2",
            "\x01\u00a3",
            "\x01\u00a4",
            "\x01\u00a5",
            "\x01\u00a6",
            "\x01\u00a7",
            "\x01\u00a8",
            "\x01\u00a9",
            "\x01\u00aa",
            "\x01\u00ab",
            "\x01\u00ac",
            "\x01\u00ad",
            "\x01\u00ae",
            "\x01\u00af",
            "\x01\u00b0",
            "\x01\u00b1",
            "\x01\u00b2",
            "\x01\u00b3",
            "\x01\u00b4",
            "\x02\x21\x01\uffff\x0a\x21\x07\uffff\x1a\x21\x04\uffff\x01"+
            "\x21\x01\uffff\x1a\x21\x3c\uffff\x01\x21\x08\uffff\x17\x21\x01"+
            "\uffff\x1f\x21\x01\uffff\u0286\x21\x01\uffff\u1c81\x21\x0c\uffff"+
            "\x02\x21\x31\uffff\x02\x21\x2f\uffff\u0120\x21\u0a70\uffff\u03f0"+
            "\x21\x11\uffff\ua7ff\x21\u2100\uffff\u04d0\x21\x20\uffff\u020e"+
            "\x21",
            "\x01\u00b5",
            "\x01\u00b6",
            "\x01\u00b7",
            "\x02\x21\x01\uffff\x0a\x21\x07\uffff\x1a\x21\x04\uffff\x01"+
            "\x21\x01\uffff\x1a\x21\x3c\uffff\x01\x21\x08\uffff\x17\x21\x01"+
            "\uffff\x1f\x21\x01\uffff\u0286\x21\x01\uffff\u1c81\x21\x0c\uffff"+
            "\x02\x21\x31\uffff\x02\x21\x2f\uffff\u0120\x21\u0a70\uffff\u03f0"+
            "\x21\x11\uffff\ua7ff\x21\u2100\uffff\u04d0\x21\x20\uffff\u020e"+
            "\x21",
            "\x01\u00b8",
            "\x02\x21\x01\uffff\x0a\x21\x07\uffff\x1a\x21\x04\uffff\x01"+
            "\x21\x01\uffff\x1a\x21\x3c\uffff\x01\x21\x08\uffff\x17\x21\x01"+
            "\uffff\x1f\x21\x01\uffff\u0286\x21\x01\uffff\u1c81\x21\x0c\uffff"+
            "\x02\x21\x31\uffff\x02\x21\x2f\uffff\u0120\x21\u0a70\uffff\u03f0"+
            "\x21\x11\uffff\ua7ff\x21\u2100\uffff\u04d0\x21\x20\uffff\u020e"+
            "\x21",
            "\x01\u00b9",
            "\x02\x21\x01\uffff\x0a\x21\x07\uffff\x1a\x21\x04\uffff\x01"+
            "\x21\x01\uffff\x1a\x21\x3c\uffff\x01\x21\x08\uffff\x17\x21\x01"+
            "\uffff\x1f\x21\x01\uffff\u0286\x21\x01\uffff\u1c81\x21\x0c\uffff"+
            "\x02\x21\x31\uffff\x02\x21\x2f\uffff\u0120\x21\u0a70\uffff\u03f0"+
            "\x21\x11\uffff\ua7ff\x21\u2100\uffff\u04d0\x21\x20\uffff\u020e"+
            "\x21",
            "\x01\u00ba",
            "\x01\u00bb",
            "\x01\u00bc",
            "\x02\x21\x01\uffff\x0a\x21\x07\uffff\x1a\x21\x04\uffff\x01"+
            "\x21\x01\uffff\x1a\x21\x3c\uffff\x01\x21\x08\uffff\x17\x21\x01"+
            "\uffff\x1f\x21\x01\uffff\u0286\x21\x01\uffff\u1c81\x21\x0c\uffff"+
            "\x02\x21\x31\uffff\x02\x21\x2f\uffff\u0120\x21\u0a70\uffff\u03f0"+
            "\x21\x11\uffff\ua7ff\x21\u2100\uffff\u04d0\x21\x20\uffff\u020e"+
            "\x21",
            ""
    };

    static readonly short[] DFA12_eot = DFA.UnpackEncodedString(DFA12_eotS);
    static readonly short[] DFA12_eof = DFA.UnpackEncodedString(DFA12_eofS);
    static readonly char[] DFA12_min = DFA.UnpackEncodedStringToUnsignedChars(DFA12_minS);
    static readonly char[] DFA12_max = DFA.UnpackEncodedStringToUnsignedChars(DFA12_maxS);
    static readonly short[] DFA12_accept = DFA.UnpackEncodedString(DFA12_acceptS);
    static readonly short[] DFA12_special = DFA.UnpackEncodedString(DFA12_specialS);
    static readonly short[][] DFA12_transition = DFA.UnpackEncodedStringArray(DFA12_transitionS);

    protected class DFA12 : DFA
    {
        public DFA12(BaseRecognizer recognizer)
        {
            this.recognizer = recognizer;
            this.decisionNumber = 12;
            this.eot = DFA12_eot;
            this.eof = DFA12_eof;
            this.min = DFA12_min;
            this.max = DFA12_max;
            this.accept = DFA12_accept;
            this.special = DFA12_special;
            this.transition = DFA12_transition;

        }

        override public string Description
        {
            get { return "1:1: Tokens : ( PATHSEP | ABRPATH | LPAR | RPAR | LBRAC | RBRAC | MINUS | PLUS | DOT | MUL | DOTDOT | AT | COMMA | PIPE | LESS | MORE | LE | GE | COLON | CC | APOS | QUOT | T__46 | T__47 | T__48 | T__49 | T__50 | T__51 | T__52 | T__53 | T__54 | T__55 | T__56 | T__57 | NodeType | Number | AxisName | Literal | Whitespace | NCName );"; }
        }

    }


    protected internal int DFA12_SpecialStateTransition(DFA dfa, int s, IIntStream _input) //throws NoViableAltException
    {
            IIntStream input = _input;
    	int _s = s;
        switch ( s )
        {
               	case 0 : 
                   	int LA12_16 = input.LA(1);

                   	s = -1;
                   	if ( ((LA12_16 >= '\u0000' && LA12_16 <= '\uFFFF')) ) { s = 46; }

                   	else s = 47;

                   	if ( s >= 0 ) return s;
                   	break;
               	case 1 : 
                   	int LA12_17 = input.LA(1);

                   	s = -1;
                   	if ( ((LA12_17 >= '\u0000' && LA12_17 <= '\uFFFF')) ) { s = 46; }

                   	else s = 48;

                   	if ( s >= 0 ) return s;
                   	break;
        }
        NoViableAltException nvae12 =
            new NoViableAltException(dfa.Description, 12, _s, input);
        dfa.Error(nvae12);
        throw nvae12;
    }
 
    
}
