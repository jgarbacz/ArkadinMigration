// $ANTLR 3.2 Sep 23, 2009 12:02:23 D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\entity_parser\\EntityGrammar.g 2010-08-27 12:52:28

// The variable 'variable' is assigned but its value is never used.
#pragma warning disable 168, 219
// Unreachable code detected.
#pragma warning disable 162


using System;
using Antlr.Runtime;
using IList 		= System.Collections.IList;
using ArrayList 	= System.Collections.ArrayList;
using Stack 		= Antlr.Runtime.Collections.StackList;


public partial class EntityGrammarLexer : Lexer {
    public const int CHILD = 8;
    public const int FUNCTION = 5;
    public const int EXPONENT = 17;
    public const int T__29 = 29;
    public const int T__28 = 28;
    public const int T__27 = 27;
    public const int T__26 = 26;
    public const int T__25 = 25;
    public const int T__24 = 24;
    public const int UNICODE_ESC = 22;
    public const int OCTAL_ESC = 23;
    public const int HEX_DIGIT = 21;
    public const int RIGHT = 15;
    public const int LITERAL = 10;
    public const int INT = 11;
    public const int FLOAT = 12;
    public const int ID = 16;
    public const int EOF = -1;
    public const int ENTITY = 7;
    public const int T__30 = 30;
    public const int PARENT = 9;
    public const int WS = 19;
    public const int ESC_SEQ = 20;
    public const int LEFT = 14;
    public const int FUNCTION_NAME = 4;
    public const int COMMENT = 18;
    public const int FUNCTION_PARAMS = 6;
    public const int STRING = 13;

    // delegates
    // delegators

    public EntityGrammarLexer() 
    {
		InitializeCyclicDFAs();
    }
    public EntityGrammarLexer(ICharStream input)
		: this(input, null) {
    }
    public EntityGrammarLexer(ICharStream input, RecognizerSharedState state)
		: base(input, state) {
		InitializeCyclicDFAs(); 

    }
    
    override public string GrammarFileName
    {
    	get { return "D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\entity_parser\\EntityGrammar.g";} 
    }

    // $ANTLR start "T__24"
    public void mT__24() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = T__24;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\entity_parser\\EntityGrammar.g:7:7: ( 'ENTITY.' )
            // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\entity_parser\\EntityGrammar.g:7:9: 'ENTITY.'
            {
            	Match("ENTITY."); 


            }

            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "T__24"

    // $ANTLR start "T__25"
    public void mT__25() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = T__25;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\entity_parser\\EntityGrammar.g:8:7: ( 'CHILD.' )
            // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\entity_parser\\EntityGrammar.g:8:9: 'CHILD.'
            {
            	Match("CHILD."); 


            }

            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "T__25"

    // $ANTLR start "T__26"
    public void mT__26() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = T__26;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\entity_parser\\EntityGrammar.g:9:7: ( 'PARENT.' )
            // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\entity_parser\\EntityGrammar.g:9:9: 'PARENT.'
            {
            	Match("PARENT."); 


            }

            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "T__26"

    // $ANTLR start "T__27"
    public void mT__27() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = T__27;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\entity_parser\\EntityGrammar.g:10:7: ( '(' )
            // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\entity_parser\\EntityGrammar.g:10:9: '('
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
    // $ANTLR end "T__27"

    // $ANTLR start "T__28"
    public void mT__28() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = T__28;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\entity_parser\\EntityGrammar.g:11:7: ( ')' )
            // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\entity_parser\\EntityGrammar.g:11:9: ')'
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
    // $ANTLR end "T__28"

    // $ANTLR start "T__29"
    public void mT__29() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = T__29;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\entity_parser\\EntityGrammar.g:12:7: ( ',' )
            // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\entity_parser\\EntityGrammar.g:12:9: ','
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
    // $ANTLR end "T__29"

    // $ANTLR start "T__30"
    public void mT__30() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = T__30;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\entity_parser\\EntityGrammar.g:13:7: ( '=' )
            // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\entity_parser\\EntityGrammar.g:13:9: '='
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
    // $ANTLR end "T__30"

    // $ANTLR start "ID"
    public void mID() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = ID;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\entity_parser\\EntityGrammar.g:76:5: ( ( 'a' .. 'z' | 'A' .. 'Z' | '_' ) ( 'a' .. 'z' | 'A' .. 'Z' | '0' .. '9' | '_' )* )
            // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\entity_parser\\EntityGrammar.g:76:7: ( 'a' .. 'z' | 'A' .. 'Z' | '_' ) ( 'a' .. 'z' | 'A' .. 'Z' | '0' .. '9' | '_' )*
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

            	// D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\entity_parser\\EntityGrammar.g:76:31: ( 'a' .. 'z' | 'A' .. 'Z' | '0' .. '9' | '_' )*
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
            			    // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\entity_parser\\EntityGrammar.g:
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
    // $ANTLR end "ID"

    // $ANTLR start "INT"
    public void mINT() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = INT;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\entity_parser\\EntityGrammar.g:79:5: ( ( '0' .. '9' )+ )
            // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\entity_parser\\EntityGrammar.g:79:7: ( '0' .. '9' )+
            {
            	// D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\entity_parser\\EntityGrammar.g:79:7: ( '0' .. '9' )+
            	int cnt2 = 0;
            	do 
            	{
            	    int alt2 = 2;
            	    int LA2_0 = input.LA(1);

            	    if ( ((LA2_0 >= '0' && LA2_0 <= '9')) )
            	    {
            	        alt2 = 1;
            	    }


            	    switch (alt2) 
            		{
            			case 1 :
            			    // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\entity_parser\\EntityGrammar.g:79:7: '0' .. '9'
            			    {
            			    	MatchRange('0','9'); 

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


            }

            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "INT"

    // $ANTLR start "FLOAT"
    public void mFLOAT() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = FLOAT;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\entity_parser\\EntityGrammar.g:83:5: ( ( '0' .. '9' )+ '.' ( '0' .. '9' )* ( EXPONENT )? | '.' ( '0' .. '9' )+ ( EXPONENT )? | ( '0' .. '9' )+ EXPONENT )
            int alt9 = 3;
            alt9 = dfa9.Predict(input);
            switch (alt9) 
            {
                case 1 :
                    // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\entity_parser\\EntityGrammar.g:83:9: ( '0' .. '9' )+ '.' ( '0' .. '9' )* ( EXPONENT )?
                    {
                    	// D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\entity_parser\\EntityGrammar.g:83:9: ( '0' .. '9' )+
                    	int cnt3 = 0;
                    	do 
                    	{
                    	    int alt3 = 2;
                    	    int LA3_0 = input.LA(1);

                    	    if ( ((LA3_0 >= '0' && LA3_0 <= '9')) )
                    	    {
                    	        alt3 = 1;
                    	    }


                    	    switch (alt3) 
                    		{
                    			case 1 :
                    			    // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\entity_parser\\EntityGrammar.g:83:10: '0' .. '9'
                    			    {
                    			    	MatchRange('0','9'); 

                    			    }
                    			    break;

                    			default:
                    			    if ( cnt3 >= 1 ) goto loop3;
                    		            EarlyExitException eee3 =
                    		                new EarlyExitException(3, input);
                    		            throw eee3;
                    	    }
                    	    cnt3++;
                    	} while (true);

                    	loop3:
                    		;	// Stops C# compiler whining that label 'loop3' has no statements

                    	Match('.'); 
                    	// D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\entity_parser\\EntityGrammar.g:83:25: ( '0' .. '9' )*
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
                    			    // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\entity_parser\\EntityGrammar.g:83:26: '0' .. '9'
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

                    	// D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\entity_parser\\EntityGrammar.g:83:37: ( EXPONENT )?
                    	int alt5 = 2;
                    	int LA5_0 = input.LA(1);

                    	if ( (LA5_0 == 'E' || LA5_0 == 'e') )
                    	{
                    	    alt5 = 1;
                    	}
                    	switch (alt5) 
                    	{
                    	    case 1 :
                    	        // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\entity_parser\\EntityGrammar.g:83:37: EXPONENT
                    	        {
                    	        	mEXPONENT(); 

                    	        }
                    	        break;

                    	}


                    }
                    break;
                case 2 :
                    // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\entity_parser\\EntityGrammar.g:84:9: '.' ( '0' .. '9' )+ ( EXPONENT )?
                    {
                    	Match('.'); 
                    	// D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\entity_parser\\EntityGrammar.g:84:13: ( '0' .. '9' )+
                    	int cnt6 = 0;
                    	do 
                    	{
                    	    int alt6 = 2;
                    	    int LA6_0 = input.LA(1);

                    	    if ( ((LA6_0 >= '0' && LA6_0 <= '9')) )
                    	    {
                    	        alt6 = 1;
                    	    }


                    	    switch (alt6) 
                    		{
                    			case 1 :
                    			    // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\entity_parser\\EntityGrammar.g:84:14: '0' .. '9'
                    			    {
                    			    	MatchRange('0','9'); 

                    			    }
                    			    break;

                    			default:
                    			    if ( cnt6 >= 1 ) goto loop6;
                    		            EarlyExitException eee6 =
                    		                new EarlyExitException(6, input);
                    		            throw eee6;
                    	    }
                    	    cnt6++;
                    	} while (true);

                    	loop6:
                    		;	// Stops C# compiler whining that label 'loop6' has no statements

                    	// D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\entity_parser\\EntityGrammar.g:84:25: ( EXPONENT )?
                    	int alt7 = 2;
                    	int LA7_0 = input.LA(1);

                    	if ( (LA7_0 == 'E' || LA7_0 == 'e') )
                    	{
                    	    alt7 = 1;
                    	}
                    	switch (alt7) 
                    	{
                    	    case 1 :
                    	        // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\entity_parser\\EntityGrammar.g:84:25: EXPONENT
                    	        {
                    	        	mEXPONENT(); 

                    	        }
                    	        break;

                    	}


                    }
                    break;
                case 3 :
                    // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\entity_parser\\EntityGrammar.g:85:9: ( '0' .. '9' )+ EXPONENT
                    {
                    	// D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\entity_parser\\EntityGrammar.g:85:9: ( '0' .. '9' )+
                    	int cnt8 = 0;
                    	do 
                    	{
                    	    int alt8 = 2;
                    	    int LA8_0 = input.LA(1);

                    	    if ( ((LA8_0 >= '0' && LA8_0 <= '9')) )
                    	    {
                    	        alt8 = 1;
                    	    }


                    	    switch (alt8) 
                    		{
                    			case 1 :
                    			    // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\entity_parser\\EntityGrammar.g:85:10: '0' .. '9'
                    			    {
                    			    	MatchRange('0','9'); 

                    			    }
                    			    break;

                    			default:
                    			    if ( cnt8 >= 1 ) goto loop8;
                    		            EarlyExitException eee8 =
                    		                new EarlyExitException(8, input);
                    		            throw eee8;
                    	    }
                    	    cnt8++;
                    	} while (true);

                    	loop8:
                    		;	// Stops C# compiler whining that label 'loop8' has no statements

                    	mEXPONENT(); 

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
    // $ANTLR end "FLOAT"

    // $ANTLR start "COMMENT"
    public void mCOMMENT() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = COMMENT;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\entity_parser\\EntityGrammar.g:89:5: ( '//' (~ ( '\\n' | '\\r' ) )* ( '\\r' )? '\\n' | '/*' ( options {greedy=false; } : . )* '*/' )
            int alt13 = 2;
            int LA13_0 = input.LA(1);

            if ( (LA13_0 == '/') )
            {
                int LA13_1 = input.LA(2);

                if ( (LA13_1 == '/') )
                {
                    alt13 = 1;
                }
                else if ( (LA13_1 == '*') )
                {
                    alt13 = 2;
                }
                else 
                {
                    NoViableAltException nvae_d13s1 =
                        new NoViableAltException("", 13, 1, input);

                    throw nvae_d13s1;
                }
            }
            else 
            {
                NoViableAltException nvae_d13s0 =
                    new NoViableAltException("", 13, 0, input);

                throw nvae_d13s0;
            }
            switch (alt13) 
            {
                case 1 :
                    // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\entity_parser\\EntityGrammar.g:89:9: '//' (~ ( '\\n' | '\\r' ) )* ( '\\r' )? '\\n'
                    {
                    	Match("//"); 

                    	// D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\entity_parser\\EntityGrammar.g:89:14: (~ ( '\\n' | '\\r' ) )*
                    	do 
                    	{
                    	    int alt10 = 2;
                    	    int LA10_0 = input.LA(1);

                    	    if ( ((LA10_0 >= '\u0000' && LA10_0 <= '\t') || (LA10_0 >= '\u000B' && LA10_0 <= '\f') || (LA10_0 >= '\u000E' && LA10_0 <= '\uFFFF')) )
                    	    {
                    	        alt10 = 1;
                    	    }


                    	    switch (alt10) 
                    		{
                    			case 1 :
                    			    // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\entity_parser\\EntityGrammar.g:89:14: ~ ( '\\n' | '\\r' )
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
                    			    goto loop10;
                    	    }
                    	} while (true);

                    	loop10:
                    		;	// Stops C# compiler whining that label 'loop10' has no statements

                    	// D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\entity_parser\\EntityGrammar.g:89:28: ( '\\r' )?
                    	int alt11 = 2;
                    	int LA11_0 = input.LA(1);

                    	if ( (LA11_0 == '\r') )
                    	{
                    	    alt11 = 1;
                    	}
                    	switch (alt11) 
                    	{
                    	    case 1 :
                    	        // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\entity_parser\\EntityGrammar.g:89:28: '\\r'
                    	        {
                    	        	Match('\r'); 

                    	        }
                    	        break;

                    	}

                    	Match('\n'); 
                    	_channel=HIDDEN;

                    }
                    break;
                case 2 :
                    // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\entity_parser\\EntityGrammar.g:90:9: '/*' ( options {greedy=false; } : . )* '*/'
                    {
                    	Match("/*"); 

                    	// D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\entity_parser\\EntityGrammar.g:90:14: ( options {greedy=false; } : . )*
                    	do 
                    	{
                    	    int alt12 = 2;
                    	    int LA12_0 = input.LA(1);

                    	    if ( (LA12_0 == '*') )
                    	    {
                    	        int LA12_1 = input.LA(2);

                    	        if ( (LA12_1 == '/') )
                    	        {
                    	            alt12 = 2;
                    	        }
                    	        else if ( ((LA12_1 >= '\u0000' && LA12_1 <= '.') || (LA12_1 >= '0' && LA12_1 <= '\uFFFF')) )
                    	        {
                    	            alt12 = 1;
                    	        }


                    	    }
                    	    else if ( ((LA12_0 >= '\u0000' && LA12_0 <= ')') || (LA12_0 >= '+' && LA12_0 <= '\uFFFF')) )
                    	    {
                    	        alt12 = 1;
                    	    }


                    	    switch (alt12) 
                    		{
                    			case 1 :
                    			    // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\entity_parser\\EntityGrammar.g:90:42: .
                    			    {
                    			    	MatchAny(); 

                    			    }
                    			    break;

                    			default:
                    			    goto loop12;
                    	    }
                    	} while (true);

                    	loop12:
                    		;	// Stops C# compiler whining that label 'loop12' has no statements

                    	Match("*/"); 

                    	_channel=HIDDEN;

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
    // $ANTLR end "COMMENT"

    // $ANTLR start "WS"
    public void mWS() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = WS;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\entity_parser\\EntityGrammar.g:93:5: ( ( ' ' | '\\t' | '\\r' | '\\n' ) )
            // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\entity_parser\\EntityGrammar.g:93:9: ( ' ' | '\\t' | '\\r' | '\\n' )
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

    // $ANTLR start "STRING"
    public void mSTRING() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = STRING;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\entity_parser\\EntityGrammar.g:101:5: ( '\"' ( ESC_SEQ | ~ ( '\\\\' | '\"' ) )* '\"' | '\\'' ( ESC_SEQ | ~ ( '\\\\' | '\\'' ) )* '\\'' )
            int alt16 = 2;
            int LA16_0 = input.LA(1);

            if ( (LA16_0 == '\"') )
            {
                alt16 = 1;
            }
            else if ( (LA16_0 == '\'') )
            {
                alt16 = 2;
            }
            else 
            {
                NoViableAltException nvae_d16s0 =
                    new NoViableAltException("", 16, 0, input);

                throw nvae_d16s0;
            }
            switch (alt16) 
            {
                case 1 :
                    // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\entity_parser\\EntityGrammar.g:101:8: '\"' ( ESC_SEQ | ~ ( '\\\\' | '\"' ) )* '\"'
                    {
                    	Match('\"'); 
                    	// D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\entity_parser\\EntityGrammar.g:101:12: ( ESC_SEQ | ~ ( '\\\\' | '\"' ) )*
                    	do 
                    	{
                    	    int alt14 = 3;
                    	    int LA14_0 = input.LA(1);

                    	    if ( (LA14_0 == '\\') )
                    	    {
                    	        alt14 = 1;
                    	    }
                    	    else if ( ((LA14_0 >= '\u0000' && LA14_0 <= '!') || (LA14_0 >= '#' && LA14_0 <= '[') || (LA14_0 >= ']' && LA14_0 <= '\uFFFF')) )
                    	    {
                    	        alt14 = 2;
                    	    }


                    	    switch (alt14) 
                    		{
                    			case 1 :
                    			    // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\entity_parser\\EntityGrammar.g:101:14: ESC_SEQ
                    			    {
                    			    	mESC_SEQ(); 

                    			    }
                    			    break;
                    			case 2 :
                    			    // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\entity_parser\\EntityGrammar.g:101:24: ~ ( '\\\\' | '\"' )
                    			    {
                    			    	if ( (input.LA(1) >= '\u0000' && input.LA(1) <= '!') || (input.LA(1) >= '#' && input.LA(1) <= '[') || (input.LA(1) >= ']' && input.LA(1) <= '\uFFFF') ) 
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
                    			    goto loop14;
                    	    }
                    	} while (true);

                    	loop14:
                    		;	// Stops C# compiler whining that label 'loop14' has no statements

                    	Match('\"'); 

                    }
                    break;
                case 2 :
                    // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\entity_parser\\EntityGrammar.g:102:8: '\\'' ( ESC_SEQ | ~ ( '\\\\' | '\\'' ) )* '\\''
                    {
                    	Match('\''); 
                    	// D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\entity_parser\\EntityGrammar.g:102:13: ( ESC_SEQ | ~ ( '\\\\' | '\\'' ) )*
                    	do 
                    	{
                    	    int alt15 = 3;
                    	    int LA15_0 = input.LA(1);

                    	    if ( (LA15_0 == '\\') )
                    	    {
                    	        alt15 = 1;
                    	    }
                    	    else if ( ((LA15_0 >= '\u0000' && LA15_0 <= '&') || (LA15_0 >= '(' && LA15_0 <= '[') || (LA15_0 >= ']' && LA15_0 <= '\uFFFF')) )
                    	    {
                    	        alt15 = 2;
                    	    }


                    	    switch (alt15) 
                    		{
                    			case 1 :
                    			    // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\entity_parser\\EntityGrammar.g:102:15: ESC_SEQ
                    			    {
                    			    	mESC_SEQ(); 

                    			    }
                    			    break;
                    			case 2 :
                    			    // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\entity_parser\\EntityGrammar.g:102:25: ~ ( '\\\\' | '\\'' )
                    			    {
                    			    	if ( (input.LA(1) >= '\u0000' && input.LA(1) <= '&') || (input.LA(1) >= '(' && input.LA(1) <= '[') || (input.LA(1) >= ']' && input.LA(1) <= '\uFFFF') ) 
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
                    			    goto loop15;
                    	    }
                    	} while (true);

                    	loop15:
                    		;	// Stops C# compiler whining that label 'loop15' has no statements

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
    // $ANTLR end "STRING"

    // $ANTLR start "EXPONENT"
    public void mEXPONENT() // throws RecognitionException [2]
    {
    		try
    		{
            // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\entity_parser\\EntityGrammar.g:106:10: ( ( 'e' | 'E' ) ( '+' | '-' )? ( '0' .. '9' )+ )
            // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\entity_parser\\EntityGrammar.g:106:12: ( 'e' | 'E' ) ( '+' | '-' )? ( '0' .. '9' )+
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

            	// D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\entity_parser\\EntityGrammar.g:106:22: ( '+' | '-' )?
            	int alt17 = 2;
            	int LA17_0 = input.LA(1);

            	if ( (LA17_0 == '+' || LA17_0 == '-') )
            	{
            	    alt17 = 1;
            	}
            	switch (alt17) 
            	{
            	    case 1 :
            	        // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\entity_parser\\EntityGrammar.g:
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

            	// D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\entity_parser\\EntityGrammar.g:106:33: ( '0' .. '9' )+
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
            			    // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\entity_parser\\EntityGrammar.g:106:34: '0' .. '9'
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


            }

        }
        finally 
    	{
        }
    }
    // $ANTLR end "EXPONENT"

    // $ANTLR start "HEX_DIGIT"
    public void mHEX_DIGIT() // throws RecognitionException [2]
    {
    		try
    		{
            // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\entity_parser\\EntityGrammar.g:109:11: ( ( '0' .. '9' | 'a' .. 'f' | 'A' .. 'F' ) )
            // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\entity_parser\\EntityGrammar.g:109:13: ( '0' .. '9' | 'a' .. 'f' | 'A' .. 'F' )
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
    // $ANTLR end "HEX_DIGIT"

    // $ANTLR start "ESC_SEQ"
    public void mESC_SEQ() // throws RecognitionException [2]
    {
    		try
    		{
            // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\entity_parser\\EntityGrammar.g:113:5: ( '\\\\' ( 'b' | 't' | 'n' | 'f' | 'r' | '\\\"' | '\\'' | '\\\\' ) | UNICODE_ESC | OCTAL_ESC )
            int alt19 = 3;
            int LA19_0 = input.LA(1);

            if ( (LA19_0 == '\\') )
            {
                switch ( input.LA(2) ) 
                {
                case '\"':
                case '\'':
                case '\\':
                case 'b':
                case 'f':
                case 'n':
                case 'r':
                case 't':
                	{
                    alt19 = 1;
                    }
                    break;
                case 'u':
                	{
                    alt19 = 2;
                    }
                    break;
                case '0':
                case '1':
                case '2':
                case '3':
                case '4':
                case '5':
                case '6':
                case '7':
                	{
                    alt19 = 3;
                    }
                    break;
                	default:
                	    NoViableAltException nvae_d19s1 =
                	        new NoViableAltException("", 19, 1, input);

                	    throw nvae_d19s1;
                }

            }
            else 
            {
                NoViableAltException nvae_d19s0 =
                    new NoViableAltException("", 19, 0, input);

                throw nvae_d19s0;
            }
            switch (alt19) 
            {
                case 1 :
                    // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\entity_parser\\EntityGrammar.g:113:9: '\\\\' ( 'b' | 't' | 'n' | 'f' | 'r' | '\\\"' | '\\'' | '\\\\' )
                    {
                    	Match('\\'); 
                    	if ( input.LA(1) == '\"' || input.LA(1) == '\'' || input.LA(1) == '\\' || input.LA(1) == 'b' || input.LA(1) == 'f' || input.LA(1) == 'n' || input.LA(1) == 'r' || input.LA(1) == 't' ) 
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
                case 2 :
                    // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\entity_parser\\EntityGrammar.g:114:9: UNICODE_ESC
                    {
                    	mUNICODE_ESC(); 

                    }
                    break;
                case 3 :
                    // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\entity_parser\\EntityGrammar.g:115:9: OCTAL_ESC
                    {
                    	mOCTAL_ESC(); 

                    }
                    break;

            }
        }
        finally 
    	{
        }
    }
    // $ANTLR end "ESC_SEQ"

    // $ANTLR start "OCTAL_ESC"
    public void mOCTAL_ESC() // throws RecognitionException [2]
    {
    		try
    		{
            // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\entity_parser\\EntityGrammar.g:120:5: ( '\\\\' ( '0' .. '3' ) ( '0' .. '7' ) ( '0' .. '7' ) | '\\\\' ( '0' .. '7' ) ( '0' .. '7' ) | '\\\\' ( '0' .. '7' ) )
            int alt20 = 3;
            int LA20_0 = input.LA(1);

            if ( (LA20_0 == '\\') )
            {
                int LA20_1 = input.LA(2);

                if ( ((LA20_1 >= '0' && LA20_1 <= '3')) )
                {
                    int LA20_2 = input.LA(3);

                    if ( ((LA20_2 >= '0' && LA20_2 <= '7')) )
                    {
                        int LA20_4 = input.LA(4);

                        if ( ((LA20_4 >= '0' && LA20_4 <= '7')) )
                        {
                            alt20 = 1;
                        }
                        else 
                        {
                            alt20 = 2;}
                    }
                    else 
                    {
                        alt20 = 3;}
                }
                else if ( ((LA20_1 >= '4' && LA20_1 <= '7')) )
                {
                    int LA20_3 = input.LA(3);

                    if ( ((LA20_3 >= '0' && LA20_3 <= '7')) )
                    {
                        alt20 = 2;
                    }
                    else 
                    {
                        alt20 = 3;}
                }
                else 
                {
                    NoViableAltException nvae_d20s1 =
                        new NoViableAltException("", 20, 1, input);

                    throw nvae_d20s1;
                }
            }
            else 
            {
                NoViableAltException nvae_d20s0 =
                    new NoViableAltException("", 20, 0, input);

                throw nvae_d20s0;
            }
            switch (alt20) 
            {
                case 1 :
                    // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\entity_parser\\EntityGrammar.g:120:9: '\\\\' ( '0' .. '3' ) ( '0' .. '7' ) ( '0' .. '7' )
                    {
                    	Match('\\'); 
                    	// D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\entity_parser\\EntityGrammar.g:120:14: ( '0' .. '3' )
                    	// D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\entity_parser\\EntityGrammar.g:120:15: '0' .. '3'
                    	{
                    		MatchRange('0','3'); 

                    	}

                    	// D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\entity_parser\\EntityGrammar.g:120:25: ( '0' .. '7' )
                    	// D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\entity_parser\\EntityGrammar.g:120:26: '0' .. '7'
                    	{
                    		MatchRange('0','7'); 

                    	}

                    	// D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\entity_parser\\EntityGrammar.g:120:36: ( '0' .. '7' )
                    	// D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\entity_parser\\EntityGrammar.g:120:37: '0' .. '7'
                    	{
                    		MatchRange('0','7'); 

                    	}


                    }
                    break;
                case 2 :
                    // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\entity_parser\\EntityGrammar.g:121:9: '\\\\' ( '0' .. '7' ) ( '0' .. '7' )
                    {
                    	Match('\\'); 
                    	// D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\entity_parser\\EntityGrammar.g:121:14: ( '0' .. '7' )
                    	// D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\entity_parser\\EntityGrammar.g:121:15: '0' .. '7'
                    	{
                    		MatchRange('0','7'); 

                    	}

                    	// D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\entity_parser\\EntityGrammar.g:121:25: ( '0' .. '7' )
                    	// D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\entity_parser\\EntityGrammar.g:121:26: '0' .. '7'
                    	{
                    		MatchRange('0','7'); 

                    	}


                    }
                    break;
                case 3 :
                    // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\entity_parser\\EntityGrammar.g:122:9: '\\\\' ( '0' .. '7' )
                    {
                    	Match('\\'); 
                    	// D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\entity_parser\\EntityGrammar.g:122:14: ( '0' .. '7' )
                    	// D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\entity_parser\\EntityGrammar.g:122:15: '0' .. '7'
                    	{
                    		MatchRange('0','7'); 

                    	}


                    }
                    break;

            }
        }
        finally 
    	{
        }
    }
    // $ANTLR end "OCTAL_ESC"

    // $ANTLR start "UNICODE_ESC"
    public void mUNICODE_ESC() // throws RecognitionException [2]
    {
    		try
    		{
            // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\entity_parser\\EntityGrammar.g:127:5: ( '\\\\' 'u' HEX_DIGIT HEX_DIGIT HEX_DIGIT HEX_DIGIT )
            // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\entity_parser\\EntityGrammar.g:127:9: '\\\\' 'u' HEX_DIGIT HEX_DIGIT HEX_DIGIT HEX_DIGIT
            {
            	Match('\\'); 
            	Match('u'); 
            	mHEX_DIGIT(); 
            	mHEX_DIGIT(); 
            	mHEX_DIGIT(); 
            	mHEX_DIGIT(); 

            }

        }
        finally 
    	{
        }
    }
    // $ANTLR end "UNICODE_ESC"

    override public void mTokens() // throws RecognitionException 
    {
        // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\entity_parser\\EntityGrammar.g:1:8: ( T__24 | T__25 | T__26 | T__27 | T__28 | T__29 | T__30 | ID | INT | FLOAT | COMMENT | WS | STRING )
        int alt21 = 13;
        alt21 = dfa21.Predict(input);
        switch (alt21) 
        {
            case 1 :
                // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\entity_parser\\EntityGrammar.g:1:10: T__24
                {
                	mT__24(); 

                }
                break;
            case 2 :
                // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\entity_parser\\EntityGrammar.g:1:16: T__25
                {
                	mT__25(); 

                }
                break;
            case 3 :
                // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\entity_parser\\EntityGrammar.g:1:22: T__26
                {
                	mT__26(); 

                }
                break;
            case 4 :
                // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\entity_parser\\EntityGrammar.g:1:28: T__27
                {
                	mT__27(); 

                }
                break;
            case 5 :
                // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\entity_parser\\EntityGrammar.g:1:34: T__28
                {
                	mT__28(); 

                }
                break;
            case 6 :
                // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\entity_parser\\EntityGrammar.g:1:40: T__29
                {
                	mT__29(); 

                }
                break;
            case 7 :
                // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\entity_parser\\EntityGrammar.g:1:46: T__30
                {
                	mT__30(); 

                }
                break;
            case 8 :
                // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\entity_parser\\EntityGrammar.g:1:52: ID
                {
                	mID(); 

                }
                break;
            case 9 :
                // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\entity_parser\\EntityGrammar.g:1:55: INT
                {
                	mINT(); 

                }
                break;
            case 10 :
                // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\entity_parser\\EntityGrammar.g:1:59: FLOAT
                {
                	mFLOAT(); 

                }
                break;
            case 11 :
                // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\entity_parser\\EntityGrammar.g:1:65: COMMENT
                {
                	mCOMMENT(); 

                }
                break;
            case 12 :
                // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\entity_parser\\EntityGrammar.g:1:73: WS
                {
                	mWS(); 

                }
                break;
            case 13 :
                // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\entity_parser\\EntityGrammar.g:1:76: STRING
                {
                	mSTRING(); 

                }
                break;

        }

    }


    protected DFA9 dfa9;
    protected DFA21 dfa21;
	private void InitializeCyclicDFAs()
	{
	    this.dfa9 = new DFA9(this);
	    this.dfa21 = new DFA21(this);
	}

    const string DFA9_eotS =
        "\x05\uffff";
    const string DFA9_eofS =
        "\x05\uffff";
    const string DFA9_minS =
        "\x02\x2e\x03\uffff";
    const string DFA9_maxS =
        "\x01\x39\x01\x65\x03\uffff";
    const string DFA9_acceptS =
        "\x02\uffff\x01\x02\x01\x01\x01\x03";
    const string DFA9_specialS =
        "\x05\uffff}>";
    static readonly string[] DFA9_transitionS = {
            "\x01\x02\x01\uffff\x0a\x01",
            "\x01\x03\x01\uffff\x0a\x01\x0b\uffff\x01\x04\x1f\uffff\x01"+
            "\x04",
            "",
            "",
            ""
    };

    static readonly short[] DFA9_eot = DFA.UnpackEncodedString(DFA9_eotS);
    static readonly short[] DFA9_eof = DFA.UnpackEncodedString(DFA9_eofS);
    static readonly char[] DFA9_min = DFA.UnpackEncodedStringToUnsignedChars(DFA9_minS);
    static readonly char[] DFA9_max = DFA.UnpackEncodedStringToUnsignedChars(DFA9_maxS);
    static readonly short[] DFA9_accept = DFA.UnpackEncodedString(DFA9_acceptS);
    static readonly short[] DFA9_special = DFA.UnpackEncodedString(DFA9_specialS);
    static readonly short[][] DFA9_transition = DFA.UnpackEncodedStringArray(DFA9_transitionS);

    protected class DFA9 : DFA
    {
        public DFA9(BaseRecognizer recognizer)
        {
            this.recognizer = recognizer;
            this.decisionNumber = 9;
            this.eot = DFA9_eot;
            this.eof = DFA9_eof;
            this.min = DFA9_min;
            this.max = DFA9_max;
            this.accept = DFA9_accept;
            this.special = DFA9_special;
            this.transition = DFA9_transition;

        }

        override public string Description
        {
            get { return "82:1: FLOAT : ( ( '0' .. '9' )+ '.' ( '0' .. '9' )* ( EXPONENT )? | '.' ( '0' .. '9' )+ ( EXPONENT )? | ( '0' .. '9' )+ EXPONENT );"; }
        }

    }

    const string DFA21_eotS =
        "\x01\uffff\x03\x08\x05\uffff\x01\x11\x04\uffff\x03\x08\x01\uffff"+
        "\x0a\x08\x01\uffff\x01\x08\x02\uffff";
    const string DFA21_eofS =
        "\x20\uffff";
    const string DFA21_minS =
        "\x01\x09\x01\x4e\x01\x48\x01\x41\x05\uffff\x01\x2e\x04\uffff\x01"+
        "\x54\x01\x49\x01\x52\x01\uffff\x01\x49\x01\x4c\x01\x45\x01\x54\x01"+
        "\x44\x01\x4e\x01\x59\x01\x2e\x01\x54\x01\x2e\x01\uffff\x01\x2e\x02"+
        "\uffff";
    const string DFA21_maxS =
        "\x01\x7a\x01\x4e\x01\x48\x01\x41\x05\uffff\x01\x65\x04\uffff\x01"+
        "\x54\x01\x49\x01\x52\x01\uffff\x01\x49\x01\x4c\x01\x45\x01\x54\x01"+
        "\x44\x01\x4e\x01\x59\x01\x2e\x01\x54\x01\x2e\x01\uffff\x01\x2e\x02"+
        "\uffff";
    const string DFA21_acceptS =
        "\x04\uffff\x01\x04\x01\x05\x01\x06\x01\x07\x01\x08\x01\uffff\x01"+
        "\x0a\x01\x0b\x01\x0c\x01\x0d\x03\uffff\x01\x09\x0a\uffff\x01\x02"+
        "\x01\uffff\x01\x01\x01\x03";
    const string DFA21_specialS =
        "\x20\uffff}>";
    static readonly string[] DFA21_transitionS = {
            "\x02\x0c\x02\uffff\x01\x0c\x12\uffff\x01\x0c\x01\uffff\x01"+
            "\x0d\x04\uffff\x01\x0d\x01\x04\x01\x05\x02\uffff\x01\x06\x01"+
            "\uffff\x01\x0a\x01\x0b\x0a\x09\x03\uffff\x01\x07\x03\uffff\x02"+
            "\x08\x01\x02\x01\x08\x01\x01\x0a\x08\x01\x03\x0a\x08\x04\uffff"+
            "\x01\x08\x01\uffff\x1a\x08",
            "\x01\x0e",
            "\x01\x0f",
            "\x01\x10",
            "",
            "",
            "",
            "",
            "",
            "\x01\x0a\x01\uffff\x0a\x09\x0b\uffff\x01\x0a\x1f\uffff\x01"+
            "\x0a",
            "",
            "",
            "",
            "",
            "\x01\x12",
            "\x01\x13",
            "\x01\x14",
            "",
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
            "",
            "\x01\x1f",
            "",
            ""
    };

    static readonly short[] DFA21_eot = DFA.UnpackEncodedString(DFA21_eotS);
    static readonly short[] DFA21_eof = DFA.UnpackEncodedString(DFA21_eofS);
    static readonly char[] DFA21_min = DFA.UnpackEncodedStringToUnsignedChars(DFA21_minS);
    static readonly char[] DFA21_max = DFA.UnpackEncodedStringToUnsignedChars(DFA21_maxS);
    static readonly short[] DFA21_accept = DFA.UnpackEncodedString(DFA21_acceptS);
    static readonly short[] DFA21_special = DFA.UnpackEncodedString(DFA21_specialS);
    static readonly short[][] DFA21_transition = DFA.UnpackEncodedStringArray(DFA21_transitionS);

    protected class DFA21 : DFA
    {
        public DFA21(BaseRecognizer recognizer)
        {
            this.recognizer = recognizer;
            this.decisionNumber = 21;
            this.eot = DFA21_eot;
            this.eof = DFA21_eof;
            this.min = DFA21_min;
            this.max = DFA21_max;
            this.accept = DFA21_accept;
            this.special = DFA21_special;
            this.transition = DFA21_transition;

        }

        override public string Description
        {
            get { return "1:1: Tokens : ( T__24 | T__25 | T__26 | T__27 | T__28 | T__29 | T__30 | ID | INT | FLOAT | COMMENT | WS | STRING );"; }
        }

    }

 
    
}
