// $ANTLR 3.2 Sep 23, 2009 12:02:23 D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmXPath.g 2010-11-12 11:09:25

// The variable 'variable' is assigned but its value is never used.
#pragma warning disable 168, 219
// Unreachable code detected.
#pragma warning disable 162


using System;
using Antlr.Runtime;
using IList 		= System.Collections.IList;
using ArrayList 	= System.Collections.ArrayList;
using Stack 		= Antlr.Runtime.Collections.StackList;



using Antlr.Runtime.Tree;

public partial class MvmXPathParser : Parser
{
    public static readonly string[] tokenNames = new string[] 
	{
        "<invalid>", 
		"<EOR>", 
		"<DOWN>", 
		"<UP>", 
		"XPATH", 
		"ABSOLUTE_PATH", 
		"RELATIVE_PATH", 
		"ROOT_PATH", 
		"PREDICATE", 
		"RECURSIVE_MATCH", 
		"TRAVERSE_UP", 
		"CURRENT_NODE", 
		"MATCH", 
		"NAME_MATCH", 
		"RECURSIVE_NAME_MATCH", 
		"PATHSEP", 
		"ABRPATH", 
		"LPAR", 
		"RPAR", 
		"LBRAC", 
		"RBRAC", 
		"MINUS", 
		"PLUS", 
		"DOT", 
		"MUL", 
		"DOTDOT", 
		"AT", 
		"COMMA", 
		"PIPE", 
		"LESS", 
		"MORE", 
		"LE", 
		"GE", 
		"COLON", 
		"CC", 
		"APOS", 
		"QUOT", 
		"AxisName", 
		"NodeType", 
		"Literal", 
		"Number", 
		"NCName", 
		"Digits", 
		"Whitespace", 
		"NCNameStartChar", 
		"NCNameChar", 
		"'/..'", 
		"'/.'", 
		"'/@'", 
		"'//@'", 
		"'processing-instruction'", 
		"'or'", 
		"'and'", 
		"'='", 
		"'!='", 
		"'div'", 
		"'mod'", 
		"'$'"
    };

    public const int NCName = 41;
    public const int APOS = 35;
    public const int ABSOLUTE_PATH = 5;
    public const int PATHSEP = 15;
    public const int DOTDOT = 25;
    public const int EOF = -1;
    public const int PREDICATE = 8;
    public const int TRAVERSE_UP = 10;
    public const int T__55 = 55;
    public const int AT = 26;
    public const int T__56 = 56;
    public const int T__57 = 57;
    public const int Literal = 39;
    public const int T__51 = 51;
    public const int T__52 = 52;
    public const int Number = 40;
    public const int T__53 = 53;
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
    public const int COLON = 33;
    public const int NAME_MATCH = 13;
    public const int XPATH = 4;
    public const int MORE = 30;
    public const int CURRENT_NODE = 11;
    public const int ROOT_PATH = 7;
    public const int RELATIVE_PATH = 6;
    public const int RPAR = 18;
    public const int CC = 34;
    public const int LE = 31;

    // delegates
    // delegators



        public MvmXPathParser(ITokenStream input)
    		: this(input, new RecognizerSharedState()) {
        }

        public MvmXPathParser(ITokenStream input, RecognizerSharedState state)
    		: base(input, state) {
            InitializeCyclicDFAs();

             
        }
        
    protected ITreeAdaptor adaptor = new CommonTreeAdaptor();

    public ITreeAdaptor TreeAdaptor
    {
        get { return this.adaptor; }
        set {
    	this.adaptor = value;
    	}
    }

    override public string[] TokenNames {
		get { return MvmXPathParser.tokenNames; }
    }

    override public string GrammarFileName {
		get { return "D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmXPath.g"; }
    }


    public class main_return : ParserRuleReturnScope
    {
        private object tree;
        override public object Tree
        {
        	get { return tree; }
        	set { tree = (object) value; }
        }
    };

    // $ANTLR start "main"
    // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmXPath.g:80:1: main : expr -> ^( XPATH expr ) ;
    public MvmXPathParser.main_return main() // throws RecognitionException [1]
    {   
        MvmXPathParser.main_return retval = new MvmXPathParser.main_return();
        retval.Start = input.LT(1);

        object root_0 = null;

        MvmXPathParser.expr_return expr1 = default(MvmXPathParser.expr_return);


        RewriteRuleSubtreeStream stream_expr = new RewriteRuleSubtreeStream(adaptor,"rule expr");
        try 
    	{
            // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmXPath.g:80:7: ( expr -> ^( XPATH expr ) )
            // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmXPath.g:80:10: expr
            {
            	PushFollow(FOLLOW_expr_in_main350);
            	expr1 = expr();
            	state.followingStackPointer--;

            	stream_expr.Add(expr1.Tree);


            	// AST REWRITE
            	// elements:          expr
            	// token labels:      
            	// rule labels:       retval
            	// token list labels: 
            	// rule list labels:  
            	// wildcard labels: 
            	retval.Tree = root_0;
            	RewriteRuleSubtreeStream stream_retval = new RewriteRuleSubtreeStream(adaptor, "rule retval", retval!=null ? retval.Tree : null);

            	root_0 = (object)adaptor.GetNilNode();
            	// 80:15: -> ^( XPATH expr )
            	{
            	    // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmXPath.g:80:18: ^( XPATH expr )
            	    {
            	    object root_1 = (object)adaptor.GetNilNode();
            	    root_1 = (object)adaptor.BecomeRoot((object)adaptor.Create(XPATH, "XPATH"), root_1);

            	    adaptor.AddChild(root_1, stream_expr.NextTree());

            	    adaptor.AddChild(root_0, root_1);
            	    }

            	}

            	retval.Tree = root_0;retval.Tree = root_0;
            }

            retval.Stop = input.LT(-1);

            	retval.Tree = (object)adaptor.RulePostProcessing(root_0);
            	adaptor.SetTokenBoundaries(retval.Tree, (IToken) retval.Start, (IToken) retval.Stop);
        }
        catch (RecognitionException re) 
    	{
            ReportError(re);
            Recover(input,re);
    	// Conversion of the second argument necessary, but harmless
    	retval.Tree = (object)adaptor.ErrorNode(input, (IToken) retval.Start, input.LT(-1), re);

        }
        finally 
    	{
        }
        return retval;
    }
    // $ANTLR end "main"

    public class locationPath_return : ParserRuleReturnScope
    {
        private object tree;
        override public object Tree
        {
        	get { return tree; }
        	set { tree = (object) value; }
        }
    };

    // $ANTLR start "locationPath"
    // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmXPath.g:83:1: locationPath : ( relativeLocationPath | absoluteLocationPathNoroot );
    public MvmXPathParser.locationPath_return locationPath() // throws RecognitionException [1]
    {   
        MvmXPathParser.locationPath_return retval = new MvmXPathParser.locationPath_return();
        retval.Start = input.LT(1);

        object root_0 = null;

        MvmXPathParser.relativeLocationPath_return relativeLocationPath2 = default(MvmXPathParser.relativeLocationPath_return);

        MvmXPathParser.absoluteLocationPathNoroot_return absoluteLocationPathNoroot3 = default(MvmXPathParser.absoluteLocationPathNoroot_return);



        try 
    	{
            // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmXPath.g:84:3: ( relativeLocationPath | absoluteLocationPathNoroot )
            int alt1 = 2;
            int LA1_0 = input.LA(1);

            if ( ((LA1_0 >= DOT && LA1_0 <= AT) || (LA1_0 >= AxisName && LA1_0 <= NodeType) || LA1_0 == NCName || LA1_0 == 50) )
            {
                alt1 = 1;
            }
            else if ( ((LA1_0 >= PATHSEP && LA1_0 <= ABRPATH) || (LA1_0 >= 46 && LA1_0 <= 49)) )
            {
                alt1 = 2;
            }
            else 
            {
                NoViableAltException nvae_d1s0 =
                    new NoViableAltException("", 1, 0, input);

                throw nvae_d1s0;
            }
            switch (alt1) 
            {
                case 1 :
                    // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmXPath.g:84:6: relativeLocationPath
                    {
                    	root_0 = (object)adaptor.GetNilNode();

                    	PushFollow(FOLLOW_relativeLocationPath_in_locationPath372);
                    	relativeLocationPath2 = relativeLocationPath();
                    	state.followingStackPointer--;

                    	adaptor.AddChild(root_0, relativeLocationPath2.Tree);

                    }
                    break;
                case 2 :
                    // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmXPath.g:85:6: absoluteLocationPathNoroot
                    {
                    	root_0 = (object)adaptor.GetNilNode();

                    	PushFollow(FOLLOW_absoluteLocationPathNoroot_in_locationPath379);
                    	absoluteLocationPathNoroot3 = absoluteLocationPathNoroot();
                    	state.followingStackPointer--;

                    	adaptor.AddChild(root_0, absoluteLocationPathNoroot3.Tree);

                    }
                    break;

            }
            retval.Stop = input.LT(-1);

            	retval.Tree = (object)adaptor.RulePostProcessing(root_0);
            	adaptor.SetTokenBoundaries(retval.Tree, (IToken) retval.Start, (IToken) retval.Stop);
        }
        catch (RecognitionException re) 
    	{
            ReportError(re);
            Recover(input,re);
    	// Conversion of the second argument necessary, but harmless
    	retval.Tree = (object)adaptor.ErrorNode(input, (IToken) retval.Start, input.LT(-1), re);

        }
        finally 
    	{
        }
        return retval;
    }
    // $ANTLR end "locationPath"

    public class absoluteLocationPathNoroot_return : ParserRuleReturnScope
    {
        private object tree;
        override public object Tree
        {
        	get { return tree; }
        	set { tree = (object) value; }
        }
    };

    // $ANTLR start "absoluteLocationPathNoroot"
    // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmXPath.g:88:1: absoluteLocationPathNoroot : ( '/..' ( traverse )* -> ^( ABSOLUTE_PATH ^( TRAVERSE_UP ) ( traverse )* ) | '/.' ( traverse )* -> ^( ABSOLUTE_PATH ^( CURRENT_NODE ) ( traverse )* ) | '/@' nodeTest ( traverse )* -> ^( ABSOLUTE_PATH ^( NAME_MATCH nodeTest ) ( traverse )* ) | '/' step ( traverse )* -> ^( ABSOLUTE_PATH ^( MATCH step ) ( traverse )* ) | '//@' nodeTest ( traverse )* -> ^( ABSOLUTE_PATH ^( RECURSIVE_NAME_MATCH nodeTest ) ( traverse )* ) | '//' step ( traverse )* -> ^( ABSOLUTE_PATH ^( RECURSIVE_MATCH step ) ( traverse )* ) );
    public MvmXPathParser.absoluteLocationPathNoroot_return absoluteLocationPathNoroot() // throws RecognitionException [1]
    {   
        MvmXPathParser.absoluteLocationPathNoroot_return retval = new MvmXPathParser.absoluteLocationPathNoroot_return();
        retval.Start = input.LT(1);

        object root_0 = null;

        IToken string_literal4 = null;
        IToken string_literal6 = null;
        IToken string_literal8 = null;
        IToken char_literal11 = null;
        IToken string_literal14 = null;
        IToken string_literal17 = null;
        MvmXPathParser.traverse_return traverse5 = default(MvmXPathParser.traverse_return);

        MvmXPathParser.traverse_return traverse7 = default(MvmXPathParser.traverse_return);

        MvmXPathParser.nodeTest_return nodeTest9 = default(MvmXPathParser.nodeTest_return);

        MvmXPathParser.traverse_return traverse10 = default(MvmXPathParser.traverse_return);

        MvmXPathParser.step_return step12 = default(MvmXPathParser.step_return);

        MvmXPathParser.traverse_return traverse13 = default(MvmXPathParser.traverse_return);

        MvmXPathParser.nodeTest_return nodeTest15 = default(MvmXPathParser.nodeTest_return);

        MvmXPathParser.traverse_return traverse16 = default(MvmXPathParser.traverse_return);

        MvmXPathParser.step_return step18 = default(MvmXPathParser.step_return);

        MvmXPathParser.traverse_return traverse19 = default(MvmXPathParser.traverse_return);


        object string_literal4_tree=null;
        object string_literal6_tree=null;
        object string_literal8_tree=null;
        object char_literal11_tree=null;
        object string_literal14_tree=null;
        object string_literal17_tree=null;
        RewriteRuleTokenStream stream_49 = new RewriteRuleTokenStream(adaptor,"token 49");
        RewriteRuleTokenStream stream_48 = new RewriteRuleTokenStream(adaptor,"token 48");
        RewriteRuleTokenStream stream_ABRPATH = new RewriteRuleTokenStream(adaptor,"token ABRPATH");
        RewriteRuleTokenStream stream_47 = new RewriteRuleTokenStream(adaptor,"token 47");
        RewriteRuleTokenStream stream_46 = new RewriteRuleTokenStream(adaptor,"token 46");
        RewriteRuleTokenStream stream_PATHSEP = new RewriteRuleTokenStream(adaptor,"token PATHSEP");
        RewriteRuleSubtreeStream stream_nodeTest = new RewriteRuleSubtreeStream(adaptor,"rule nodeTest");
        RewriteRuleSubtreeStream stream_traverse = new RewriteRuleSubtreeStream(adaptor,"rule traverse");
        RewriteRuleSubtreeStream stream_step = new RewriteRuleSubtreeStream(adaptor,"rule step");
        try 
    	{
            // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmXPath.g:89:3: ( '/..' ( traverse )* -> ^( ABSOLUTE_PATH ^( TRAVERSE_UP ) ( traverse )* ) | '/.' ( traverse )* -> ^( ABSOLUTE_PATH ^( CURRENT_NODE ) ( traverse )* ) | '/@' nodeTest ( traverse )* -> ^( ABSOLUTE_PATH ^( NAME_MATCH nodeTest ) ( traverse )* ) | '/' step ( traverse )* -> ^( ABSOLUTE_PATH ^( MATCH step ) ( traverse )* ) | '//@' nodeTest ( traverse )* -> ^( ABSOLUTE_PATH ^( RECURSIVE_NAME_MATCH nodeTest ) ( traverse )* ) | '//' step ( traverse )* -> ^( ABSOLUTE_PATH ^( RECURSIVE_MATCH step ) ( traverse )* ) )
            int alt8 = 6;
            switch ( input.LA(1) ) 
            {
            case 46:
            	{
                alt8 = 1;
                }
                break;
            case 47:
            	{
                alt8 = 2;
                }
                break;
            case 48:
            	{
                alt8 = 3;
                }
                break;
            case PATHSEP:
            	{
                alt8 = 4;
                }
                break;
            case 49:
            	{
                alt8 = 5;
                }
                break;
            case ABRPATH:
            	{
                alt8 = 6;
                }
                break;
            	default:
            	    NoViableAltException nvae_d8s0 =
            	        new NoViableAltException("", 8, 0, input);

            	    throw nvae_d8s0;
            }

            switch (alt8) 
            {
                case 1 :
                    // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmXPath.g:89:5: '/..' ( traverse )*
                    {
                    	string_literal4=(IToken)Match(input,46,FOLLOW_46_in_absoluteLocationPathNoroot392);  
                    	stream_46.Add(string_literal4);

                    	// D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmXPath.g:89:11: ( traverse )*
                    	do 
                    	{
                    	    int alt2 = 2;
                    	    int LA2_0 = input.LA(1);

                    	    if ( ((LA2_0 >= PATHSEP && LA2_0 <= ABRPATH) || (LA2_0 >= 46 && LA2_0 <= 49)) )
                    	    {
                    	        alt2 = 1;
                    	    }


                    	    switch (alt2) 
                    		{
                    			case 1 :
                    			    // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmXPath.g:89:12: traverse
                    			    {
                    			    	PushFollow(FOLLOW_traverse_in_absoluteLocationPathNoroot395);
                    			    	traverse5 = traverse();
                    			    	state.followingStackPointer--;

                    			    	stream_traverse.Add(traverse5.Tree);

                    			    }
                    			    break;

                    			default:
                    			    goto loop2;
                    	    }
                    	} while (true);

                    	loop2:
                    		;	// Stops C# compiler whining that label 'loop2' has no statements



                    	// AST REWRITE
                    	// elements:          traverse
                    	// token labels:      
                    	// rule labels:       retval
                    	// token list labels: 
                    	// rule list labels:  
                    	// wildcard labels: 
                    	retval.Tree = root_0;
                    	RewriteRuleSubtreeStream stream_retval = new RewriteRuleSubtreeStream(adaptor, "rule retval", retval!=null ? retval.Tree : null);

                    	root_0 = (object)adaptor.GetNilNode();
                    	// 89:23: -> ^( ABSOLUTE_PATH ^( TRAVERSE_UP ) ( traverse )* )
                    	{
                    	    // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmXPath.g:89:25: ^( ABSOLUTE_PATH ^( TRAVERSE_UP ) ( traverse )* )
                    	    {
                    	    object root_1 = (object)adaptor.GetNilNode();
                    	    root_1 = (object)adaptor.BecomeRoot((object)adaptor.Create(ABSOLUTE_PATH, "ABSOLUTE_PATH"), root_1);

                    	    // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmXPath.g:89:41: ^( TRAVERSE_UP )
                    	    {
                    	    object root_2 = (object)adaptor.GetNilNode();
                    	    root_2 = (object)adaptor.BecomeRoot((object)adaptor.Create(TRAVERSE_UP, "TRAVERSE_UP"), root_2);

                    	    adaptor.AddChild(root_1, root_2);
                    	    }
                    	    // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmXPath.g:89:56: ( traverse )*
                    	    while ( stream_traverse.HasNext() )
                    	    {
                    	        adaptor.AddChild(root_1, stream_traverse.NextTree());

                    	    }
                    	    stream_traverse.Reset();

                    	    adaptor.AddChild(root_0, root_1);
                    	    }

                    	}

                    	retval.Tree = root_0;retval.Tree = root_0;
                    }
                    break;
                case 2 :
                    // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmXPath.g:90:5: '/.' ( traverse )*
                    {
                    	string_literal6=(IToken)Match(input,47,FOLLOW_47_in_absoluteLocationPathNoroot417);  
                    	stream_47.Add(string_literal6);

                    	// D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmXPath.g:90:10: ( traverse )*
                    	do 
                    	{
                    	    int alt3 = 2;
                    	    int LA3_0 = input.LA(1);

                    	    if ( ((LA3_0 >= PATHSEP && LA3_0 <= ABRPATH) || (LA3_0 >= 46 && LA3_0 <= 49)) )
                    	    {
                    	        alt3 = 1;
                    	    }


                    	    switch (alt3) 
                    		{
                    			case 1 :
                    			    // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmXPath.g:90:11: traverse
                    			    {
                    			    	PushFollow(FOLLOW_traverse_in_absoluteLocationPathNoroot420);
                    			    	traverse7 = traverse();
                    			    	state.followingStackPointer--;

                    			    	stream_traverse.Add(traverse7.Tree);

                    			    }
                    			    break;

                    			default:
                    			    goto loop3;
                    	    }
                    	} while (true);

                    	loop3:
                    		;	// Stops C# compiler whining that label 'loop3' has no statements



                    	// AST REWRITE
                    	// elements:          traverse
                    	// token labels:      
                    	// rule labels:       retval
                    	// token list labels: 
                    	// rule list labels:  
                    	// wildcard labels: 
                    	retval.Tree = root_0;
                    	RewriteRuleSubtreeStream stream_retval = new RewriteRuleSubtreeStream(adaptor, "rule retval", retval!=null ? retval.Tree : null);

                    	root_0 = (object)adaptor.GetNilNode();
                    	// 90:22: -> ^( ABSOLUTE_PATH ^( CURRENT_NODE ) ( traverse )* )
                    	{
                    	    // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmXPath.g:90:24: ^( ABSOLUTE_PATH ^( CURRENT_NODE ) ( traverse )* )
                    	    {
                    	    object root_1 = (object)adaptor.GetNilNode();
                    	    root_1 = (object)adaptor.BecomeRoot((object)adaptor.Create(ABSOLUTE_PATH, "ABSOLUTE_PATH"), root_1);

                    	    // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmXPath.g:90:40: ^( CURRENT_NODE )
                    	    {
                    	    object root_2 = (object)adaptor.GetNilNode();
                    	    root_2 = (object)adaptor.BecomeRoot((object)adaptor.Create(CURRENT_NODE, "CURRENT_NODE"), root_2);

                    	    adaptor.AddChild(root_1, root_2);
                    	    }
                    	    // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmXPath.g:90:56: ( traverse )*
                    	    while ( stream_traverse.HasNext() )
                    	    {
                    	        adaptor.AddChild(root_1, stream_traverse.NextTree());

                    	    }
                    	    stream_traverse.Reset();

                    	    adaptor.AddChild(root_0, root_1);
                    	    }

                    	}

                    	retval.Tree = root_0;retval.Tree = root_0;
                    }
                    break;
                case 3 :
                    // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmXPath.g:91:5: '/@' nodeTest ( traverse )*
                    {
                    	string_literal8=(IToken)Match(input,48,FOLLOW_48_in_absoluteLocationPathNoroot442);  
                    	stream_48.Add(string_literal8);

                    	PushFollow(FOLLOW_nodeTest_in_absoluteLocationPathNoroot444);
                    	nodeTest9 = nodeTest();
                    	state.followingStackPointer--;

                    	stream_nodeTest.Add(nodeTest9.Tree);
                    	// D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmXPath.g:91:19: ( traverse )*
                    	do 
                    	{
                    	    int alt4 = 2;
                    	    int LA4_0 = input.LA(1);

                    	    if ( ((LA4_0 >= PATHSEP && LA4_0 <= ABRPATH) || (LA4_0 >= 46 && LA4_0 <= 49)) )
                    	    {
                    	        alt4 = 1;
                    	    }


                    	    switch (alt4) 
                    		{
                    			case 1 :
                    			    // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmXPath.g:91:20: traverse
                    			    {
                    			    	PushFollow(FOLLOW_traverse_in_absoluteLocationPathNoroot447);
                    			    	traverse10 = traverse();
                    			    	state.followingStackPointer--;

                    			    	stream_traverse.Add(traverse10.Tree);

                    			    }
                    			    break;

                    			default:
                    			    goto loop4;
                    	    }
                    	} while (true);

                    	loop4:
                    		;	// Stops C# compiler whining that label 'loop4' has no statements



                    	// AST REWRITE
                    	// elements:          traverse, nodeTest
                    	// token labels:      
                    	// rule labels:       retval
                    	// token list labels: 
                    	// rule list labels:  
                    	// wildcard labels: 
                    	retval.Tree = root_0;
                    	RewriteRuleSubtreeStream stream_retval = new RewriteRuleSubtreeStream(adaptor, "rule retval", retval!=null ? retval.Tree : null);

                    	root_0 = (object)adaptor.GetNilNode();
                    	// 91:30: -> ^( ABSOLUTE_PATH ^( NAME_MATCH nodeTest ) ( traverse )* )
                    	{
                    	    // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmXPath.g:91:33: ^( ABSOLUTE_PATH ^( NAME_MATCH nodeTest ) ( traverse )* )
                    	    {
                    	    object root_1 = (object)adaptor.GetNilNode();
                    	    root_1 = (object)adaptor.BecomeRoot((object)adaptor.Create(ABSOLUTE_PATH, "ABSOLUTE_PATH"), root_1);

                    	    // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmXPath.g:91:49: ^( NAME_MATCH nodeTest )
                    	    {
                    	    object root_2 = (object)adaptor.GetNilNode();
                    	    root_2 = (object)adaptor.BecomeRoot((object)adaptor.Create(NAME_MATCH, "NAME_MATCH"), root_2);

                    	    adaptor.AddChild(root_2, stream_nodeTest.NextTree());

                    	    adaptor.AddChild(root_1, root_2);
                    	    }
                    	    // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmXPath.g:91:72: ( traverse )*
                    	    while ( stream_traverse.HasNext() )
                    	    {
                    	        adaptor.AddChild(root_1, stream_traverse.NextTree());

                    	    }
                    	    stream_traverse.Reset();

                    	    adaptor.AddChild(root_0, root_1);
                    	    }

                    	}

                    	retval.Tree = root_0;retval.Tree = root_0;
                    }
                    break;
                case 4 :
                    // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmXPath.g:92:5: '/' step ( traverse )*
                    {
                    	char_literal11=(IToken)Match(input,PATHSEP,FOLLOW_PATHSEP_in_absoluteLocationPathNoroot471);  
                    	stream_PATHSEP.Add(char_literal11);

                    	PushFollow(FOLLOW_step_in_absoluteLocationPathNoroot474);
                    	step12 = step();
                    	state.followingStackPointer--;

                    	stream_step.Add(step12.Tree);
                    	// D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmXPath.g:92:15: ( traverse )*
                    	do 
                    	{
                    	    int alt5 = 2;
                    	    int LA5_0 = input.LA(1);

                    	    if ( ((LA5_0 >= PATHSEP && LA5_0 <= ABRPATH) || (LA5_0 >= 46 && LA5_0 <= 49)) )
                    	    {
                    	        alt5 = 1;
                    	    }


                    	    switch (alt5) 
                    		{
                    			case 1 :
                    			    // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmXPath.g:92:16: traverse
                    			    {
                    			    	PushFollow(FOLLOW_traverse_in_absoluteLocationPathNoroot477);
                    			    	traverse13 = traverse();
                    			    	state.followingStackPointer--;

                    			    	stream_traverse.Add(traverse13.Tree);

                    			    }
                    			    break;

                    			default:
                    			    goto loop5;
                    	    }
                    	} while (true);

                    	loop5:
                    		;	// Stops C# compiler whining that label 'loop5' has no statements



                    	// AST REWRITE
                    	// elements:          step, traverse
                    	// token labels:      
                    	// rule labels:       retval
                    	// token list labels: 
                    	// rule list labels:  
                    	// wildcard labels: 
                    	retval.Tree = root_0;
                    	RewriteRuleSubtreeStream stream_retval = new RewriteRuleSubtreeStream(adaptor, "rule retval", retval!=null ? retval.Tree : null);

                    	root_0 = (object)adaptor.GetNilNode();
                    	// 92:27: -> ^( ABSOLUTE_PATH ^( MATCH step ) ( traverse )* )
                    	{
                    	    // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmXPath.g:92:29: ^( ABSOLUTE_PATH ^( MATCH step ) ( traverse )* )
                    	    {
                    	    object root_1 = (object)adaptor.GetNilNode();
                    	    root_1 = (object)adaptor.BecomeRoot((object)adaptor.Create(ABSOLUTE_PATH, "ABSOLUTE_PATH"), root_1);

                    	    // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmXPath.g:92:45: ^( MATCH step )
                    	    {
                    	    object root_2 = (object)adaptor.GetNilNode();
                    	    root_2 = (object)adaptor.BecomeRoot((object)adaptor.Create(MATCH, "MATCH"), root_2);

                    	    adaptor.AddChild(root_2, stream_step.NextTree());

                    	    adaptor.AddChild(root_1, root_2);
                    	    }
                    	    // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmXPath.g:92:59: ( traverse )*
                    	    while ( stream_traverse.HasNext() )
                    	    {
                    	        adaptor.AddChild(root_1, stream_traverse.NextTree());

                    	    }
                    	    stream_traverse.Reset();

                    	    adaptor.AddChild(root_0, root_1);
                    	    }

                    	}

                    	retval.Tree = root_0;retval.Tree = root_0;
                    }
                    break;
                case 5 :
                    // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmXPath.g:93:5: '//@' nodeTest ( traverse )*
                    {
                    	string_literal14=(IToken)Match(input,49,FOLLOW_49_in_absoluteLocationPathNoroot501);  
                    	stream_49.Add(string_literal14);

                    	PushFollow(FOLLOW_nodeTest_in_absoluteLocationPathNoroot503);
                    	nodeTest15 = nodeTest();
                    	state.followingStackPointer--;

                    	stream_nodeTest.Add(nodeTest15.Tree);
                    	// D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmXPath.g:93:20: ( traverse )*
                    	do 
                    	{
                    	    int alt6 = 2;
                    	    int LA6_0 = input.LA(1);

                    	    if ( ((LA6_0 >= PATHSEP && LA6_0 <= ABRPATH) || (LA6_0 >= 46 && LA6_0 <= 49)) )
                    	    {
                    	        alt6 = 1;
                    	    }


                    	    switch (alt6) 
                    		{
                    			case 1 :
                    			    // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmXPath.g:93:21: traverse
                    			    {
                    			    	PushFollow(FOLLOW_traverse_in_absoluteLocationPathNoroot506);
                    			    	traverse16 = traverse();
                    			    	state.followingStackPointer--;

                    			    	stream_traverse.Add(traverse16.Tree);

                    			    }
                    			    break;

                    			default:
                    			    goto loop6;
                    	    }
                    	} while (true);

                    	loop6:
                    		;	// Stops C# compiler whining that label 'loop6' has no statements



                    	// AST REWRITE
                    	// elements:          traverse, nodeTest
                    	// token labels:      
                    	// rule labels:       retval
                    	// token list labels: 
                    	// rule list labels:  
                    	// wildcard labels: 
                    	retval.Tree = root_0;
                    	RewriteRuleSubtreeStream stream_retval = new RewriteRuleSubtreeStream(adaptor, "rule retval", retval!=null ? retval.Tree : null);

                    	root_0 = (object)adaptor.GetNilNode();
                    	// 93:31: -> ^( ABSOLUTE_PATH ^( RECURSIVE_NAME_MATCH nodeTest ) ( traverse )* )
                    	{
                    	    // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmXPath.g:93:34: ^( ABSOLUTE_PATH ^( RECURSIVE_NAME_MATCH nodeTest ) ( traverse )* )
                    	    {
                    	    object root_1 = (object)adaptor.GetNilNode();
                    	    root_1 = (object)adaptor.BecomeRoot((object)adaptor.Create(ABSOLUTE_PATH, "ABSOLUTE_PATH"), root_1);

                    	    // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmXPath.g:93:50: ^( RECURSIVE_NAME_MATCH nodeTest )
                    	    {
                    	    object root_2 = (object)adaptor.GetNilNode();
                    	    root_2 = (object)adaptor.BecomeRoot((object)adaptor.Create(RECURSIVE_NAME_MATCH, "RECURSIVE_NAME_MATCH"), root_2);

                    	    adaptor.AddChild(root_2, stream_nodeTest.NextTree());

                    	    adaptor.AddChild(root_1, root_2);
                    	    }
                    	    // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmXPath.g:93:83: ( traverse )*
                    	    while ( stream_traverse.HasNext() )
                    	    {
                    	        adaptor.AddChild(root_1, stream_traverse.NextTree());

                    	    }
                    	    stream_traverse.Reset();

                    	    adaptor.AddChild(root_0, root_1);
                    	    }

                    	}

                    	retval.Tree = root_0;retval.Tree = root_0;
                    }
                    break;
                case 6 :
                    // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmXPath.g:94:5: '//' step ( traverse )*
                    {
                    	string_literal17=(IToken)Match(input,ABRPATH,FOLLOW_ABRPATH_in_absoluteLocationPathNoroot530);  
                    	stream_ABRPATH.Add(string_literal17);

                    	PushFollow(FOLLOW_step_in_absoluteLocationPathNoroot533);
                    	step18 = step();
                    	state.followingStackPointer--;

                    	stream_step.Add(step18.Tree);
                    	// D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmXPath.g:94:16: ( traverse )*
                    	do 
                    	{
                    	    int alt7 = 2;
                    	    int LA7_0 = input.LA(1);

                    	    if ( ((LA7_0 >= PATHSEP && LA7_0 <= ABRPATH) || (LA7_0 >= 46 && LA7_0 <= 49)) )
                    	    {
                    	        alt7 = 1;
                    	    }


                    	    switch (alt7) 
                    		{
                    			case 1 :
                    			    // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmXPath.g:94:17: traverse
                    			    {
                    			    	PushFollow(FOLLOW_traverse_in_absoluteLocationPathNoroot536);
                    			    	traverse19 = traverse();
                    			    	state.followingStackPointer--;

                    			    	stream_traverse.Add(traverse19.Tree);

                    			    }
                    			    break;

                    			default:
                    			    goto loop7;
                    	    }
                    	} while (true);

                    	loop7:
                    		;	// Stops C# compiler whining that label 'loop7' has no statements



                    	// AST REWRITE
                    	// elements:          traverse, step
                    	// token labels:      
                    	// rule labels:       retval
                    	// token list labels: 
                    	// rule list labels:  
                    	// wildcard labels: 
                    	retval.Tree = root_0;
                    	RewriteRuleSubtreeStream stream_retval = new RewriteRuleSubtreeStream(adaptor, "rule retval", retval!=null ? retval.Tree : null);

                    	root_0 = (object)adaptor.GetNilNode();
                    	// 94:28: -> ^( ABSOLUTE_PATH ^( RECURSIVE_MATCH step ) ( traverse )* )
                    	{
                    	    // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmXPath.g:94:30: ^( ABSOLUTE_PATH ^( RECURSIVE_MATCH step ) ( traverse )* )
                    	    {
                    	    object root_1 = (object)adaptor.GetNilNode();
                    	    root_1 = (object)adaptor.BecomeRoot((object)adaptor.Create(ABSOLUTE_PATH, "ABSOLUTE_PATH"), root_1);

                    	    // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmXPath.g:94:46: ^( RECURSIVE_MATCH step )
                    	    {
                    	    object root_2 = (object)adaptor.GetNilNode();
                    	    root_2 = (object)adaptor.BecomeRoot((object)adaptor.Create(RECURSIVE_MATCH, "RECURSIVE_MATCH"), root_2);

                    	    adaptor.AddChild(root_2, stream_step.NextTree());

                    	    adaptor.AddChild(root_1, root_2);
                    	    }
                    	    // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmXPath.g:94:70: ( traverse )*
                    	    while ( stream_traverse.HasNext() )
                    	    {
                    	        adaptor.AddChild(root_1, stream_traverse.NextTree());

                    	    }
                    	    stream_traverse.Reset();

                    	    adaptor.AddChild(root_0, root_1);
                    	    }

                    	}

                    	retval.Tree = root_0;retval.Tree = root_0;
                    }
                    break;

            }
            retval.Stop = input.LT(-1);

            	retval.Tree = (object)adaptor.RulePostProcessing(root_0);
            	adaptor.SetTokenBoundaries(retval.Tree, (IToken) retval.Start, (IToken) retval.Stop);
        }
        catch (RecognitionException re) 
    	{
            ReportError(re);
            Recover(input,re);
    	// Conversion of the second argument necessary, but harmless
    	retval.Tree = (object)adaptor.ErrorNode(input, (IToken) retval.Start, input.LT(-1), re);

        }
        finally 
    	{
        }
        return retval;
    }
    // $ANTLR end "absoluteLocationPathNoroot"

    public class relativeLocationPath_return : ParserRuleReturnScope
    {
        private object tree;
        override public object Tree
        {
        	get { return tree; }
        	set { tree = (object) value; }
        }
    };

    // $ANTLR start "relativeLocationPath"
    // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmXPath.g:97:1: relativeLocationPath : ( '..' ( traverse )* -> ^( RELATIVE_PATH ^( TRAVERSE_UP ) ( traverse )* ) | '.' ( traverse )* -> ^( RELATIVE_PATH ^( CURRENT_NODE ) ( traverse )* ) | step ( traverse )* -> ^( RELATIVE_PATH ^( MATCH step ) ( traverse )* ) );
    public MvmXPathParser.relativeLocationPath_return relativeLocationPath() // throws RecognitionException [1]
    {   
        MvmXPathParser.relativeLocationPath_return retval = new MvmXPathParser.relativeLocationPath_return();
        retval.Start = input.LT(1);

        object root_0 = null;

        IToken string_literal20 = null;
        IToken char_literal22 = null;
        MvmXPathParser.traverse_return traverse21 = default(MvmXPathParser.traverse_return);

        MvmXPathParser.traverse_return traverse23 = default(MvmXPathParser.traverse_return);

        MvmXPathParser.step_return step24 = default(MvmXPathParser.step_return);

        MvmXPathParser.traverse_return traverse25 = default(MvmXPathParser.traverse_return);


        object string_literal20_tree=null;
        object char_literal22_tree=null;
        RewriteRuleTokenStream stream_DOTDOT = new RewriteRuleTokenStream(adaptor,"token DOTDOT");
        RewriteRuleTokenStream stream_DOT = new RewriteRuleTokenStream(adaptor,"token DOT");
        RewriteRuleSubtreeStream stream_traverse = new RewriteRuleSubtreeStream(adaptor,"rule traverse");
        RewriteRuleSubtreeStream stream_step = new RewriteRuleSubtreeStream(adaptor,"rule step");
        try 
    	{
            // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmXPath.g:98:3: ( '..' ( traverse )* -> ^( RELATIVE_PATH ^( TRAVERSE_UP ) ( traverse )* ) | '.' ( traverse )* -> ^( RELATIVE_PATH ^( CURRENT_NODE ) ( traverse )* ) | step ( traverse )* -> ^( RELATIVE_PATH ^( MATCH step ) ( traverse )* ) )
            int alt12 = 3;
            switch ( input.LA(1) ) 
            {
            case DOTDOT:
            	{
                alt12 = 1;
                }
                break;
            case DOT:
            	{
                alt12 = 2;
                }
                break;
            case MUL:
            case AT:
            case AxisName:
            case NodeType:
            case NCName:
            case 50:
            	{
                alt12 = 3;
                }
                break;
            	default:
            	    NoViableAltException nvae_d12s0 =
            	        new NoViableAltException("", 12, 0, input);

            	    throw nvae_d12s0;
            }

            switch (alt12) 
            {
                case 1 :
                    // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmXPath.g:98:5: '..' ( traverse )*
                    {
                    	string_literal20=(IToken)Match(input,DOTDOT,FOLLOW_DOTDOT_in_relativeLocationPath567);  
                    	stream_DOTDOT.Add(string_literal20);

                    	// D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmXPath.g:98:10: ( traverse )*
                    	do 
                    	{
                    	    int alt9 = 2;
                    	    int LA9_0 = input.LA(1);

                    	    if ( ((LA9_0 >= PATHSEP && LA9_0 <= ABRPATH) || (LA9_0 >= 46 && LA9_0 <= 49)) )
                    	    {
                    	        alt9 = 1;
                    	    }


                    	    switch (alt9) 
                    		{
                    			case 1 :
                    			    // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmXPath.g:98:11: traverse
                    			    {
                    			    	PushFollow(FOLLOW_traverse_in_relativeLocationPath570);
                    			    	traverse21 = traverse();
                    			    	state.followingStackPointer--;

                    			    	stream_traverse.Add(traverse21.Tree);

                    			    }
                    			    break;

                    			default:
                    			    goto loop9;
                    	    }
                    	} while (true);

                    	loop9:
                    		;	// Stops C# compiler whining that label 'loop9' has no statements



                    	// AST REWRITE
                    	// elements:          traverse
                    	// token labels:      
                    	// rule labels:       retval
                    	// token list labels: 
                    	// rule list labels:  
                    	// wildcard labels: 
                    	retval.Tree = root_0;
                    	RewriteRuleSubtreeStream stream_retval = new RewriteRuleSubtreeStream(adaptor, "rule retval", retval!=null ? retval.Tree : null);

                    	root_0 = (object)adaptor.GetNilNode();
                    	// 98:22: -> ^( RELATIVE_PATH ^( TRAVERSE_UP ) ( traverse )* )
                    	{
                    	    // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmXPath.g:98:24: ^( RELATIVE_PATH ^( TRAVERSE_UP ) ( traverse )* )
                    	    {
                    	    object root_1 = (object)adaptor.GetNilNode();
                    	    root_1 = (object)adaptor.BecomeRoot((object)adaptor.Create(RELATIVE_PATH, "RELATIVE_PATH"), root_1);

                    	    // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmXPath.g:98:40: ^( TRAVERSE_UP )
                    	    {
                    	    object root_2 = (object)adaptor.GetNilNode();
                    	    root_2 = (object)adaptor.BecomeRoot((object)adaptor.Create(TRAVERSE_UP, "TRAVERSE_UP"), root_2);

                    	    adaptor.AddChild(root_1, root_2);
                    	    }
                    	    // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmXPath.g:98:55: ( traverse )*
                    	    while ( stream_traverse.HasNext() )
                    	    {
                    	        adaptor.AddChild(root_1, stream_traverse.NextTree());

                    	    }
                    	    stream_traverse.Reset();

                    	    adaptor.AddChild(root_0, root_1);
                    	    }

                    	}

                    	retval.Tree = root_0;retval.Tree = root_0;
                    }
                    break;
                case 2 :
                    // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmXPath.g:99:5: '.' ( traverse )*
                    {
                    	char_literal22=(IToken)Match(input,DOT,FOLLOW_DOT_in_relativeLocationPath592);  
                    	stream_DOT.Add(char_literal22);

                    	// D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmXPath.g:99:9: ( traverse )*
                    	do 
                    	{
                    	    int alt10 = 2;
                    	    int LA10_0 = input.LA(1);

                    	    if ( ((LA10_0 >= PATHSEP && LA10_0 <= ABRPATH) || (LA10_0 >= 46 && LA10_0 <= 49)) )
                    	    {
                    	        alt10 = 1;
                    	    }


                    	    switch (alt10) 
                    		{
                    			case 1 :
                    			    // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmXPath.g:99:10: traverse
                    			    {
                    			    	PushFollow(FOLLOW_traverse_in_relativeLocationPath595);
                    			    	traverse23 = traverse();
                    			    	state.followingStackPointer--;

                    			    	stream_traverse.Add(traverse23.Tree);

                    			    }
                    			    break;

                    			default:
                    			    goto loop10;
                    	    }
                    	} while (true);

                    	loop10:
                    		;	// Stops C# compiler whining that label 'loop10' has no statements



                    	// AST REWRITE
                    	// elements:          traverse
                    	// token labels:      
                    	// rule labels:       retval
                    	// token list labels: 
                    	// rule list labels:  
                    	// wildcard labels: 
                    	retval.Tree = root_0;
                    	RewriteRuleSubtreeStream stream_retval = new RewriteRuleSubtreeStream(adaptor, "rule retval", retval!=null ? retval.Tree : null);

                    	root_0 = (object)adaptor.GetNilNode();
                    	// 99:21: -> ^( RELATIVE_PATH ^( CURRENT_NODE ) ( traverse )* )
                    	{
                    	    // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmXPath.g:99:23: ^( RELATIVE_PATH ^( CURRENT_NODE ) ( traverse )* )
                    	    {
                    	    object root_1 = (object)adaptor.GetNilNode();
                    	    root_1 = (object)adaptor.BecomeRoot((object)adaptor.Create(RELATIVE_PATH, "RELATIVE_PATH"), root_1);

                    	    // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmXPath.g:99:39: ^( CURRENT_NODE )
                    	    {
                    	    object root_2 = (object)adaptor.GetNilNode();
                    	    root_2 = (object)adaptor.BecomeRoot((object)adaptor.Create(CURRENT_NODE, "CURRENT_NODE"), root_2);

                    	    adaptor.AddChild(root_1, root_2);
                    	    }
                    	    // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmXPath.g:99:55: ( traverse )*
                    	    while ( stream_traverse.HasNext() )
                    	    {
                    	        adaptor.AddChild(root_1, stream_traverse.NextTree());

                    	    }
                    	    stream_traverse.Reset();

                    	    adaptor.AddChild(root_0, root_1);
                    	    }

                    	}

                    	retval.Tree = root_0;retval.Tree = root_0;
                    }
                    break;
                case 3 :
                    // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmXPath.g:100:6: step ( traverse )*
                    {
                    	PushFollow(FOLLOW_step_in_relativeLocationPath618);
                    	step24 = step();
                    	state.followingStackPointer--;

                    	stream_step.Add(step24.Tree);
                    	// D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmXPath.g:100:11: ( traverse )*
                    	do 
                    	{
                    	    int alt11 = 2;
                    	    int LA11_0 = input.LA(1);

                    	    if ( ((LA11_0 >= PATHSEP && LA11_0 <= ABRPATH) || (LA11_0 >= 46 && LA11_0 <= 49)) )
                    	    {
                    	        alt11 = 1;
                    	    }


                    	    switch (alt11) 
                    		{
                    			case 1 :
                    			    // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmXPath.g:100:12: traverse
                    			    {
                    			    	PushFollow(FOLLOW_traverse_in_relativeLocationPath621);
                    			    	traverse25 = traverse();
                    			    	state.followingStackPointer--;

                    			    	stream_traverse.Add(traverse25.Tree);

                    			    }
                    			    break;

                    			default:
                    			    goto loop11;
                    	    }
                    	} while (true);

                    	loop11:
                    		;	// Stops C# compiler whining that label 'loop11' has no statements



                    	// AST REWRITE
                    	// elements:          traverse, step
                    	// token labels:      
                    	// rule labels:       retval
                    	// token list labels: 
                    	// rule list labels:  
                    	// wildcard labels: 
                    	retval.Tree = root_0;
                    	RewriteRuleSubtreeStream stream_retval = new RewriteRuleSubtreeStream(adaptor, "rule retval", retval!=null ? retval.Tree : null);

                    	root_0 = (object)adaptor.GetNilNode();
                    	// 100:23: -> ^( RELATIVE_PATH ^( MATCH step ) ( traverse )* )
                    	{
                    	    // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmXPath.g:100:25: ^( RELATIVE_PATH ^( MATCH step ) ( traverse )* )
                    	    {
                    	    object root_1 = (object)adaptor.GetNilNode();
                    	    root_1 = (object)adaptor.BecomeRoot((object)adaptor.Create(RELATIVE_PATH, "RELATIVE_PATH"), root_1);

                    	    // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmXPath.g:100:41: ^( MATCH step )
                    	    {
                    	    object root_2 = (object)adaptor.GetNilNode();
                    	    root_2 = (object)adaptor.BecomeRoot((object)adaptor.Create(MATCH, "MATCH"), root_2);

                    	    adaptor.AddChild(root_2, stream_step.NextTree());

                    	    adaptor.AddChild(root_1, root_2);
                    	    }
                    	    // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmXPath.g:100:55: ( traverse )*
                    	    while ( stream_traverse.HasNext() )
                    	    {
                    	        adaptor.AddChild(root_1, stream_traverse.NextTree());

                    	    }
                    	    stream_traverse.Reset();

                    	    adaptor.AddChild(root_0, root_1);
                    	    }

                    	}

                    	retval.Tree = root_0;retval.Tree = root_0;
                    }
                    break;

            }
            retval.Stop = input.LT(-1);

            	retval.Tree = (object)adaptor.RulePostProcessing(root_0);
            	adaptor.SetTokenBoundaries(retval.Tree, (IToken) retval.Start, (IToken) retval.Stop);
        }
        catch (RecognitionException re) 
    	{
            ReportError(re);
            Recover(input,re);
    	// Conversion of the second argument necessary, but harmless
    	retval.Tree = (object)adaptor.ErrorNode(input, (IToken) retval.Start, input.LT(-1), re);

        }
        finally 
    	{
        }
        return retval;
    }
    // $ANTLR end "relativeLocationPath"

    public class traverse_return : ParserRuleReturnScope
    {
        private object tree;
        override public object Tree
        {
        	get { return tree; }
        	set { tree = (object) value; }
        }
    };

    // $ANTLR start "traverse"
    // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmXPath.g:103:1: traverse : ( '/..' -> ^( TRAVERSE_UP ) | '/.' -> ^( CURRENT_NODE ) | '/@' nodeTest -> ^( NAME_MATCH nodeTest ) | '/' step -> ^( MATCH step ) | '//@' nodeTest -> ^( RECURSIVE_NAME_MATCH nodeTest ) | '//' step -> ^( RECURSIVE_MATCH step ) );
    public MvmXPathParser.traverse_return traverse() // throws RecognitionException [1]
    {   
        MvmXPathParser.traverse_return retval = new MvmXPathParser.traverse_return();
        retval.Start = input.LT(1);

        object root_0 = null;

        IToken string_literal26 = null;
        IToken string_literal27 = null;
        IToken string_literal28 = null;
        IToken char_literal30 = null;
        IToken string_literal32 = null;
        IToken string_literal34 = null;
        MvmXPathParser.nodeTest_return nodeTest29 = default(MvmXPathParser.nodeTest_return);

        MvmXPathParser.step_return step31 = default(MvmXPathParser.step_return);

        MvmXPathParser.nodeTest_return nodeTest33 = default(MvmXPathParser.nodeTest_return);

        MvmXPathParser.step_return step35 = default(MvmXPathParser.step_return);


        object string_literal26_tree=null;
        object string_literal27_tree=null;
        object string_literal28_tree=null;
        object char_literal30_tree=null;
        object string_literal32_tree=null;
        object string_literal34_tree=null;
        RewriteRuleTokenStream stream_49 = new RewriteRuleTokenStream(adaptor,"token 49");
        RewriteRuleTokenStream stream_48 = new RewriteRuleTokenStream(adaptor,"token 48");
        RewriteRuleTokenStream stream_ABRPATH = new RewriteRuleTokenStream(adaptor,"token ABRPATH");
        RewriteRuleTokenStream stream_47 = new RewriteRuleTokenStream(adaptor,"token 47");
        RewriteRuleTokenStream stream_46 = new RewriteRuleTokenStream(adaptor,"token 46");
        RewriteRuleTokenStream stream_PATHSEP = new RewriteRuleTokenStream(adaptor,"token PATHSEP");
        RewriteRuleSubtreeStream stream_nodeTest = new RewriteRuleSubtreeStream(adaptor,"rule nodeTest");
        RewriteRuleSubtreeStream stream_step = new RewriteRuleSubtreeStream(adaptor,"rule step");
        try 
    	{
            // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmXPath.g:104:2: ( '/..' -> ^( TRAVERSE_UP ) | '/.' -> ^( CURRENT_NODE ) | '/@' nodeTest -> ^( NAME_MATCH nodeTest ) | '/' step -> ^( MATCH step ) | '//@' nodeTest -> ^( RECURSIVE_NAME_MATCH nodeTest ) | '//' step -> ^( RECURSIVE_MATCH step ) )
            int alt13 = 6;
            switch ( input.LA(1) ) 
            {
            case 46:
            	{
                alt13 = 1;
                }
                break;
            case 47:
            	{
                alt13 = 2;
                }
                break;
            case 48:
            	{
                alt13 = 3;
                }
                break;
            case PATHSEP:
            	{
                alt13 = 4;
                }
                break;
            case 49:
            	{
                alt13 = 5;
                }
                break;
            case ABRPATH:
            	{
                alt13 = 6;
                }
                break;
            	default:
            	    NoViableAltException nvae_d13s0 =
            	        new NoViableAltException("", 13, 0, input);

            	    throw nvae_d13s0;
            }

            switch (alt13) 
            {
                case 1 :
                    // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmXPath.g:104:4: '/..'
                    {
                    	string_literal26=(IToken)Match(input,46,FOLLOW_46_in_traverse653);  
                    	stream_46.Add(string_literal26);



                    	// AST REWRITE
                    	// elements:          
                    	// token labels:      
                    	// rule labels:       retval
                    	// token list labels: 
                    	// rule list labels:  
                    	// wildcard labels: 
                    	retval.Tree = root_0;
                    	RewriteRuleSubtreeStream stream_retval = new RewriteRuleSubtreeStream(adaptor, "rule retval", retval!=null ? retval.Tree : null);

                    	root_0 = (object)adaptor.GetNilNode();
                    	// 104:10: -> ^( TRAVERSE_UP )
                    	{
                    	    // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmXPath.g:104:13: ^( TRAVERSE_UP )
                    	    {
                    	    object root_1 = (object)adaptor.GetNilNode();
                    	    root_1 = (object)adaptor.BecomeRoot((object)adaptor.Create(TRAVERSE_UP, "TRAVERSE_UP"), root_1);

                    	    adaptor.AddChild(root_0, root_1);
                    	    }

                    	}

                    	retval.Tree = root_0;retval.Tree = root_0;
                    }
                    break;
                case 2 :
                    // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmXPath.g:105:4: '/.'
                    {
                    	string_literal27=(IToken)Match(input,47,FOLLOW_47_in_traverse664);  
                    	stream_47.Add(string_literal27);



                    	// AST REWRITE
                    	// elements:          
                    	// token labels:      
                    	// rule labels:       retval
                    	// token list labels: 
                    	// rule list labels:  
                    	// wildcard labels: 
                    	retval.Tree = root_0;
                    	RewriteRuleSubtreeStream stream_retval = new RewriteRuleSubtreeStream(adaptor, "rule retval", retval!=null ? retval.Tree : null);

                    	root_0 = (object)adaptor.GetNilNode();
                    	// 105:9: -> ^( CURRENT_NODE )
                    	{
                    	    // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmXPath.g:105:12: ^( CURRENT_NODE )
                    	    {
                    	    object root_1 = (object)adaptor.GetNilNode();
                    	    root_1 = (object)adaptor.BecomeRoot((object)adaptor.Create(CURRENT_NODE, "CURRENT_NODE"), root_1);

                    	    adaptor.AddChild(root_0, root_1);
                    	    }

                    	}

                    	retval.Tree = root_0;retval.Tree = root_0;
                    }
                    break;
                case 3 :
                    // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmXPath.g:106:4: '/@' nodeTest
                    {
                    	string_literal28=(IToken)Match(input,48,FOLLOW_48_in_traverse675);  
                    	stream_48.Add(string_literal28);

                    	PushFollow(FOLLOW_nodeTest_in_traverse677);
                    	nodeTest29 = nodeTest();
                    	state.followingStackPointer--;

                    	stream_nodeTest.Add(nodeTest29.Tree);


                    	// AST REWRITE
                    	// elements:          nodeTest
                    	// token labels:      
                    	// rule labels:       retval
                    	// token list labels: 
                    	// rule list labels:  
                    	// wildcard labels: 
                    	retval.Tree = root_0;
                    	RewriteRuleSubtreeStream stream_retval = new RewriteRuleSubtreeStream(adaptor, "rule retval", retval!=null ? retval.Tree : null);

                    	root_0 = (object)adaptor.GetNilNode();
                    	// 106:17: -> ^( NAME_MATCH nodeTest )
                    	{
                    	    // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmXPath.g:106:20: ^( NAME_MATCH nodeTest )
                    	    {
                    	    object root_1 = (object)adaptor.GetNilNode();
                    	    root_1 = (object)adaptor.BecomeRoot((object)adaptor.Create(NAME_MATCH, "NAME_MATCH"), root_1);

                    	    adaptor.AddChild(root_1, stream_nodeTest.NextTree());

                    	    adaptor.AddChild(root_0, root_1);
                    	    }

                    	}

                    	retval.Tree = root_0;retval.Tree = root_0;
                    }
                    break;
                case 4 :
                    // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmXPath.g:107:4: '/' step
                    {
                    	char_literal30=(IToken)Match(input,PATHSEP,FOLLOW_PATHSEP_in_traverse689);  
                    	stream_PATHSEP.Add(char_literal30);

                    	PushFollow(FOLLOW_step_in_traverse691);
                    	step31 = step();
                    	state.followingStackPointer--;

                    	stream_step.Add(step31.Tree);


                    	// AST REWRITE
                    	// elements:          step
                    	// token labels:      
                    	// rule labels:       retval
                    	// token list labels: 
                    	// rule list labels:  
                    	// wildcard labels: 
                    	retval.Tree = root_0;
                    	RewriteRuleSubtreeStream stream_retval = new RewriteRuleSubtreeStream(adaptor, "rule retval", retval!=null ? retval.Tree : null);

                    	root_0 = (object)adaptor.GetNilNode();
                    	// 107:13: -> ^( MATCH step )
                    	{
                    	    // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmXPath.g:107:16: ^( MATCH step )
                    	    {
                    	    object root_1 = (object)adaptor.GetNilNode();
                    	    root_1 = (object)adaptor.BecomeRoot((object)adaptor.Create(MATCH, "MATCH"), root_1);

                    	    adaptor.AddChild(root_1, stream_step.NextTree());

                    	    adaptor.AddChild(root_0, root_1);
                    	    }

                    	}

                    	retval.Tree = root_0;retval.Tree = root_0;
                    }
                    break;
                case 5 :
                    // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmXPath.g:108:4: '//@' nodeTest
                    {
                    	string_literal32=(IToken)Match(input,49,FOLLOW_49_in_traverse704);  
                    	stream_49.Add(string_literal32);

                    	PushFollow(FOLLOW_nodeTest_in_traverse706);
                    	nodeTest33 = nodeTest();
                    	state.followingStackPointer--;

                    	stream_nodeTest.Add(nodeTest33.Tree);


                    	// AST REWRITE
                    	// elements:          nodeTest
                    	// token labels:      
                    	// rule labels:       retval
                    	// token list labels: 
                    	// rule list labels:  
                    	// wildcard labels: 
                    	retval.Tree = root_0;
                    	RewriteRuleSubtreeStream stream_retval = new RewriteRuleSubtreeStream(adaptor, "rule retval", retval!=null ? retval.Tree : null);

                    	root_0 = (object)adaptor.GetNilNode();
                    	// 108:18: -> ^( RECURSIVE_NAME_MATCH nodeTest )
                    	{
                    	    // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmXPath.g:108:21: ^( RECURSIVE_NAME_MATCH nodeTest )
                    	    {
                    	    object root_1 = (object)adaptor.GetNilNode();
                    	    root_1 = (object)adaptor.BecomeRoot((object)adaptor.Create(RECURSIVE_NAME_MATCH, "RECURSIVE_NAME_MATCH"), root_1);

                    	    adaptor.AddChild(root_1, stream_nodeTest.NextTree());

                    	    adaptor.AddChild(root_0, root_1);
                    	    }

                    	}

                    	retval.Tree = root_0;retval.Tree = root_0;
                    }
                    break;
                case 6 :
                    // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmXPath.g:109:4: '//' step
                    {
                    	string_literal34=(IToken)Match(input,ABRPATH,FOLLOW_ABRPATH_in_traverse718);  
                    	stream_ABRPATH.Add(string_literal34);

                    	PushFollow(FOLLOW_step_in_traverse720);
                    	step35 = step();
                    	state.followingStackPointer--;

                    	stream_step.Add(step35.Tree);


                    	// AST REWRITE
                    	// elements:          step
                    	// token labels:      
                    	// rule labels:       retval
                    	// token list labels: 
                    	// rule list labels:  
                    	// wildcard labels: 
                    	retval.Tree = root_0;
                    	RewriteRuleSubtreeStream stream_retval = new RewriteRuleSubtreeStream(adaptor, "rule retval", retval!=null ? retval.Tree : null);

                    	root_0 = (object)adaptor.GetNilNode();
                    	// 109:13: -> ^( RECURSIVE_MATCH step )
                    	{
                    	    // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmXPath.g:109:16: ^( RECURSIVE_MATCH step )
                    	    {
                    	    object root_1 = (object)adaptor.GetNilNode();
                    	    root_1 = (object)adaptor.BecomeRoot((object)adaptor.Create(RECURSIVE_MATCH, "RECURSIVE_MATCH"), root_1);

                    	    adaptor.AddChild(root_1, stream_step.NextTree());

                    	    adaptor.AddChild(root_0, root_1);
                    	    }

                    	}

                    	retval.Tree = root_0;retval.Tree = root_0;
                    }
                    break;

            }
            retval.Stop = input.LT(-1);

            	retval.Tree = (object)adaptor.RulePostProcessing(root_0);
            	adaptor.SetTokenBoundaries(retval.Tree, (IToken) retval.Start, (IToken) retval.Stop);
        }
        catch (RecognitionException re) 
    	{
            ReportError(re);
            Recover(input,re);
    	// Conversion of the second argument necessary, but harmless
    	retval.Tree = (object)adaptor.ErrorNode(input, (IToken) retval.Start, input.LT(-1), re);

        }
        finally 
    	{
        }
        return retval;
    }
    // $ANTLR end "traverse"

    public class step_return : ParserRuleReturnScope
    {
        private object tree;
        override public object Tree
        {
        	get { return tree; }
        	set { tree = (object) value; }
        }
    };

    // $ANTLR start "step"
    // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmXPath.g:112:1: step : axisSpecifier nodeTest ( predicate )* ;
    public MvmXPathParser.step_return step() // throws RecognitionException [1]
    {   
        MvmXPathParser.step_return retval = new MvmXPathParser.step_return();
        retval.Start = input.LT(1);

        object root_0 = null;

        MvmXPathParser.axisSpecifier_return axisSpecifier36 = default(MvmXPathParser.axisSpecifier_return);

        MvmXPathParser.nodeTest_return nodeTest37 = default(MvmXPathParser.nodeTest_return);

        MvmXPathParser.predicate_return predicate38 = default(MvmXPathParser.predicate_return);



        try 
    	{
            // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmXPath.g:113:3: ( axisSpecifier nodeTest ( predicate )* )
            // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmXPath.g:113:6: axisSpecifier nodeTest ( predicate )*
            {
            	root_0 = (object)adaptor.GetNilNode();

            	PushFollow(FOLLOW_axisSpecifier_in_step742);
            	axisSpecifier36 = axisSpecifier();
            	state.followingStackPointer--;

            	adaptor.AddChild(root_0, axisSpecifier36.Tree);
            	PushFollow(FOLLOW_nodeTest_in_step744);
            	nodeTest37 = nodeTest();
            	state.followingStackPointer--;

            	adaptor.AddChild(root_0, nodeTest37.Tree);
            	// D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmXPath.g:113:29: ( predicate )*
            	do 
            	{
            	    int alt14 = 2;
            	    int LA14_0 = input.LA(1);

            	    if ( (LA14_0 == LBRAC) )
            	    {
            	        alt14 = 1;
            	    }


            	    switch (alt14) 
            		{
            			case 1 :
            			    // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmXPath.g:113:29: predicate
            			    {
            			    	PushFollow(FOLLOW_predicate_in_step746);
            			    	predicate38 = predicate();
            			    	state.followingStackPointer--;

            			    	adaptor.AddChild(root_0, predicate38.Tree);

            			    }
            			    break;

            			default:
            			    goto loop14;
            	    }
            	} while (true);

            	loop14:
            		;	// Stops C# compiler whining that label 'loop14' has no statements


            }

            retval.Stop = input.LT(-1);

            	retval.Tree = (object)adaptor.RulePostProcessing(root_0);
            	adaptor.SetTokenBoundaries(retval.Tree, (IToken) retval.Start, (IToken) retval.Stop);
        }
        catch (RecognitionException re) 
    	{
            ReportError(re);
            Recover(input,re);
    	// Conversion of the second argument necessary, but harmless
    	retval.Tree = (object)adaptor.ErrorNode(input, (IToken) retval.Start, input.LT(-1), re);

        }
        finally 
    	{
        }
        return retval;
    }
    // $ANTLR end "step"

    public class axisSpecifier_return : ParserRuleReturnScope
    {
        private object tree;
        override public object Tree
        {
        	get { return tree; }
        	set { tree = (object) value; }
        }
    };

    // $ANTLR start "axisSpecifier"
    // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmXPath.g:117:1: axisSpecifier : ( AxisName '::' | ( '@' )? );
    public MvmXPathParser.axisSpecifier_return axisSpecifier() // throws RecognitionException [1]
    {   
        MvmXPathParser.axisSpecifier_return retval = new MvmXPathParser.axisSpecifier_return();
        retval.Start = input.LT(1);

        object root_0 = null;

        IToken AxisName39 = null;
        IToken string_literal40 = null;
        IToken char_literal41 = null;

        object AxisName39_tree=null;
        object string_literal40_tree=null;
        object char_literal41_tree=null;

        try 
    	{
            // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmXPath.g:118:3: ( AxisName '::' | ( '@' )? )
            int alt16 = 2;
            int LA16_0 = input.LA(1);

            if ( (LA16_0 == AxisName) )
            {
                int LA16_1 = input.LA(2);

                if ( (LA16_1 == CC) )
                {
                    alt16 = 1;
                }
                else if ( (LA16_1 == EOF || (LA16_1 >= PATHSEP && LA16_1 <= ABRPATH) || (LA16_1 >= RPAR && LA16_1 <= PLUS) || LA16_1 == MUL || (LA16_1 >= COMMA && LA16_1 <= COLON) || (LA16_1 >= 46 && LA16_1 <= 49) || (LA16_1 >= 51 && LA16_1 <= 56)) )
                {
                    alt16 = 2;
                }
                else 
                {
                    NoViableAltException nvae_d16s1 =
                        new NoViableAltException("", 16, 1, input);

                    throw nvae_d16s1;
                }
            }
            else if ( (LA16_0 == MUL || LA16_0 == AT || LA16_0 == NodeType || LA16_0 == NCName || LA16_0 == 50) )
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
                    // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmXPath.g:118:6: AxisName '::'
                    {
                    	root_0 = (object)adaptor.GetNilNode();

                    	AxisName39=(IToken)Match(input,AxisName,FOLLOW_AxisName_in_axisSpecifier764); 
                    		AxisName39_tree = (object)adaptor.Create(AxisName39);
                    		adaptor.AddChild(root_0, AxisName39_tree);

                    	string_literal40=(IToken)Match(input,CC,FOLLOW_CC_in_axisSpecifier766); 
                    		string_literal40_tree = (object)adaptor.Create(string_literal40);
                    		adaptor.AddChild(root_0, string_literal40_tree);


                    }
                    break;
                case 2 :
                    // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmXPath.g:119:6: ( '@' )?
                    {
                    	root_0 = (object)adaptor.GetNilNode();

                    	// D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmXPath.g:119:6: ( '@' )?
                    	int alt15 = 2;
                    	int LA15_0 = input.LA(1);

                    	if ( (LA15_0 == AT) )
                    	{
                    	    alt15 = 1;
                    	}
                    	switch (alt15) 
                    	{
                    	    case 1 :
                    	        // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmXPath.g:119:6: '@'
                    	        {
                    	        	char_literal41=(IToken)Match(input,AT,FOLLOW_AT_in_axisSpecifier773); 
                    	        		char_literal41_tree = (object)adaptor.Create(char_literal41);
                    	        		adaptor.AddChild(root_0, char_literal41_tree);


                    	        }
                    	        break;

                    	}


                    }
                    break;

            }
            retval.Stop = input.LT(-1);

            	retval.Tree = (object)adaptor.RulePostProcessing(root_0);
            	adaptor.SetTokenBoundaries(retval.Tree, (IToken) retval.Start, (IToken) retval.Stop);
        }
        catch (RecognitionException re) 
    	{
            ReportError(re);
            Recover(input,re);
    	// Conversion of the second argument necessary, but harmless
    	retval.Tree = (object)adaptor.ErrorNode(input, (IToken) retval.Start, input.LT(-1), re);

        }
        finally 
    	{
        }
        return retval;
    }
    // $ANTLR end "axisSpecifier"

    public class nodeTest_return : ParserRuleReturnScope
    {
        private object tree;
        override public object Tree
        {
        	get { return tree; }
        	set { tree = (object) value; }
        }
    };

    // $ANTLR start "nodeTest"
    // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmXPath.g:122:1: nodeTest : ( nameTest | NodeType '(' ')' | 'processing-instruction' '(' Literal ')' );
    public MvmXPathParser.nodeTest_return nodeTest() // throws RecognitionException [1]
    {   
        MvmXPathParser.nodeTest_return retval = new MvmXPathParser.nodeTest_return();
        retval.Start = input.LT(1);

        object root_0 = null;

        IToken NodeType43 = null;
        IToken char_literal44 = null;
        IToken char_literal45 = null;
        IToken string_literal46 = null;
        IToken char_literal47 = null;
        IToken Literal48 = null;
        IToken char_literal49 = null;
        MvmXPathParser.nameTest_return nameTest42 = default(MvmXPathParser.nameTest_return);


        object NodeType43_tree=null;
        object char_literal44_tree=null;
        object char_literal45_tree=null;
        object string_literal46_tree=null;
        object char_literal47_tree=null;
        object Literal48_tree=null;
        object char_literal49_tree=null;

        try 
    	{
            // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmXPath.g:123:3: ( nameTest | NodeType '(' ')' | 'processing-instruction' '(' Literal ')' )
            int alt17 = 3;
            switch ( input.LA(1) ) 
            {
            case MUL:
            case AxisName:
            case NCName:
            	{
                alt17 = 1;
                }
                break;
            case NodeType:
            	{
                alt17 = 2;
                }
                break;
            case 50:
            	{
                alt17 = 3;
                }
                break;
            	default:
            	    NoViableAltException nvae_d17s0 =
            	        new NoViableAltException("", 17, 0, input);

            	    throw nvae_d17s0;
            }

            switch (alt17) 
            {
                case 1 :
                    // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmXPath.g:123:6: nameTest
                    {
                    	root_0 = (object)adaptor.GetNilNode();

                    	PushFollow(FOLLOW_nameTest_in_nodeTest788);
                    	nameTest42 = nameTest();
                    	state.followingStackPointer--;

                    	adaptor.AddChild(root_0, nameTest42.Tree);

                    }
                    break;
                case 2 :
                    // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmXPath.g:124:6: NodeType '(' ')'
                    {
                    	root_0 = (object)adaptor.GetNilNode();

                    	NodeType43=(IToken)Match(input,NodeType,FOLLOW_NodeType_in_nodeTest795); 
                    		NodeType43_tree = (object)adaptor.Create(NodeType43);
                    		adaptor.AddChild(root_0, NodeType43_tree);

                    	char_literal44=(IToken)Match(input,LPAR,FOLLOW_LPAR_in_nodeTest797); 
                    		char_literal44_tree = (object)adaptor.Create(char_literal44);
                    		adaptor.AddChild(root_0, char_literal44_tree);

                    	char_literal45=(IToken)Match(input,RPAR,FOLLOW_RPAR_in_nodeTest799); 
                    		char_literal45_tree = (object)adaptor.Create(char_literal45);
                    		adaptor.AddChild(root_0, char_literal45_tree);


                    }
                    break;
                case 3 :
                    // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmXPath.g:125:6: 'processing-instruction' '(' Literal ')'
                    {
                    	root_0 = (object)adaptor.GetNilNode();

                    	string_literal46=(IToken)Match(input,50,FOLLOW_50_in_nodeTest806); 
                    		string_literal46_tree = (object)adaptor.Create(string_literal46);
                    		adaptor.AddChild(root_0, string_literal46_tree);

                    	char_literal47=(IToken)Match(input,LPAR,FOLLOW_LPAR_in_nodeTest808); 
                    		char_literal47_tree = (object)adaptor.Create(char_literal47);
                    		adaptor.AddChild(root_0, char_literal47_tree);

                    	Literal48=(IToken)Match(input,Literal,FOLLOW_Literal_in_nodeTest810); 
                    		Literal48_tree = (object)adaptor.Create(Literal48);
                    		adaptor.AddChild(root_0, Literal48_tree);

                    	char_literal49=(IToken)Match(input,RPAR,FOLLOW_RPAR_in_nodeTest812); 
                    		char_literal49_tree = (object)adaptor.Create(char_literal49);
                    		adaptor.AddChild(root_0, char_literal49_tree);


                    }
                    break;

            }
            retval.Stop = input.LT(-1);

            	retval.Tree = (object)adaptor.RulePostProcessing(root_0);
            	adaptor.SetTokenBoundaries(retval.Tree, (IToken) retval.Start, (IToken) retval.Stop);
        }
        catch (RecognitionException re) 
    	{
            ReportError(re);
            Recover(input,re);
    	// Conversion of the second argument necessary, but harmless
    	retval.Tree = (object)adaptor.ErrorNode(input, (IToken) retval.Start, input.LT(-1), re);

        }
        finally 
    	{
        }
        return retval;
    }
    // $ANTLR end "nodeTest"

    public class predicate_return : ParserRuleReturnScope
    {
        private object tree;
        override public object Tree
        {
        	get { return tree; }
        	set { tree = (object) value; }
        }
    };

    // $ANTLR start "predicate"
    // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmXPath.g:128:1: predicate : '[' expr ']' -> ^( PREDICATE expr ) ;
    public MvmXPathParser.predicate_return predicate() // throws RecognitionException [1]
    {   
        MvmXPathParser.predicate_return retval = new MvmXPathParser.predicate_return();
        retval.Start = input.LT(1);

        object root_0 = null;

        IToken char_literal50 = null;
        IToken char_literal52 = null;
        MvmXPathParser.expr_return expr51 = default(MvmXPathParser.expr_return);


        object char_literal50_tree=null;
        object char_literal52_tree=null;
        RewriteRuleTokenStream stream_RBRAC = new RewriteRuleTokenStream(adaptor,"token RBRAC");
        RewriteRuleTokenStream stream_LBRAC = new RewriteRuleTokenStream(adaptor,"token LBRAC");
        RewriteRuleSubtreeStream stream_expr = new RewriteRuleSubtreeStream(adaptor,"rule expr");
        try 
    	{
            // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmXPath.g:129:3: ( '[' expr ']' -> ^( PREDICATE expr ) )
            // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmXPath.g:129:6: '[' expr ']'
            {
            	char_literal50=(IToken)Match(input,LBRAC,FOLLOW_LBRAC_in_predicate826);  
            	stream_LBRAC.Add(char_literal50);

            	PushFollow(FOLLOW_expr_in_predicate828);
            	expr51 = expr();
            	state.followingStackPointer--;

            	stream_expr.Add(expr51.Tree);
            	char_literal52=(IToken)Match(input,RBRAC,FOLLOW_RBRAC_in_predicate830);  
            	stream_RBRAC.Add(char_literal52);



            	// AST REWRITE
            	// elements:          expr
            	// token labels:      
            	// rule labels:       retval
            	// token list labels: 
            	// rule list labels:  
            	// wildcard labels: 
            	retval.Tree = root_0;
            	RewriteRuleSubtreeStream stream_retval = new RewriteRuleSubtreeStream(adaptor, "rule retval", retval!=null ? retval.Tree : null);

            	root_0 = (object)adaptor.GetNilNode();
            	// 129:19: -> ^( PREDICATE expr )
            	{
            	    // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmXPath.g:129:21: ^( PREDICATE expr )
            	    {
            	    object root_1 = (object)adaptor.GetNilNode();
            	    root_1 = (object)adaptor.BecomeRoot((object)adaptor.Create(PREDICATE, "PREDICATE"), root_1);

            	    adaptor.AddChild(root_1, stream_expr.NextTree());

            	    adaptor.AddChild(root_0, root_1);
            	    }

            	}

            	retval.Tree = root_0;retval.Tree = root_0;
            }

            retval.Stop = input.LT(-1);

            	retval.Tree = (object)adaptor.RulePostProcessing(root_0);
            	adaptor.SetTokenBoundaries(retval.Tree, (IToken) retval.Start, (IToken) retval.Stop);
        }
        catch (RecognitionException re) 
    	{
            ReportError(re);
            Recover(input,re);
    	// Conversion of the second argument necessary, but harmless
    	retval.Tree = (object)adaptor.ErrorNode(input, (IToken) retval.Start, input.LT(-1), re);

        }
        finally 
    	{
        }
        return retval;
    }
    // $ANTLR end "predicate"

    public class abbreviatedStep_return : ParserRuleReturnScope
    {
        private object tree;
        override public object Tree
        {
        	get { return tree; }
        	set { tree = (object) value; }
        }
    };

    // $ANTLR start "abbreviatedStep"
    // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmXPath.g:132:1: abbreviatedStep : ( '.' | '..' );
    public MvmXPathParser.abbreviatedStep_return abbreviatedStep() // throws RecognitionException [1]
    {   
        MvmXPathParser.abbreviatedStep_return retval = new MvmXPathParser.abbreviatedStep_return();
        retval.Start = input.LT(1);

        object root_0 = null;

        IToken set53 = null;

        object set53_tree=null;

        try 
    	{
            // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmXPath.g:133:3: ( '.' | '..' )
            // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmXPath.g:
            {
            	root_0 = (object)adaptor.GetNilNode();

            	set53 = (IToken)input.LT(1);
            	if ( input.LA(1) == DOT || input.LA(1) == DOTDOT ) 
            	{
            	    input.Consume();
            	    adaptor.AddChild(root_0, (object)adaptor.Create(set53));
            	    state.errorRecovery = false;
            	}
            	else 
            	{
            	    MismatchedSetException mse = new MismatchedSetException(null,input);
            	    throw mse;
            	}


            }

            retval.Stop = input.LT(-1);

            	retval.Tree = (object)adaptor.RulePostProcessing(root_0);
            	adaptor.SetTokenBoundaries(retval.Tree, (IToken) retval.Start, (IToken) retval.Stop);
        }
        catch (RecognitionException re) 
    	{
            ReportError(re);
            Recover(input,re);
    	// Conversion of the second argument necessary, but harmless
    	retval.Tree = (object)adaptor.ErrorNode(input, (IToken) retval.Start, input.LT(-1), re);

        }
        finally 
    	{
        }
        return retval;
    }
    // $ANTLR end "abbreviatedStep"

    public class expr_return : ParserRuleReturnScope
    {
        private object tree;
        override public object Tree
        {
        	get { return tree; }
        	set { tree = (object) value; }
        }
    };

    // $ANTLR start "expr"
    // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmXPath.g:137:1: expr : orExpr ;
    public MvmXPathParser.expr_return expr() // throws RecognitionException [1]
    {   
        MvmXPathParser.expr_return retval = new MvmXPathParser.expr_return();
        retval.Start = input.LT(1);

        object root_0 = null;

        MvmXPathParser.orExpr_return orExpr54 = default(MvmXPathParser.orExpr_return);



        try 
    	{
            // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmXPath.g:137:7: ( orExpr )
            // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmXPath.g:137:10: orExpr
            {
            	root_0 = (object)adaptor.GetNilNode();

            	PushFollow(FOLLOW_orExpr_in_expr871);
            	orExpr54 = orExpr();
            	state.followingStackPointer--;

            	adaptor.AddChild(root_0, orExpr54.Tree);

            }

            retval.Stop = input.LT(-1);

            	retval.Tree = (object)adaptor.RulePostProcessing(root_0);
            	adaptor.SetTokenBoundaries(retval.Tree, (IToken) retval.Start, (IToken) retval.Stop);
        }
        catch (RecognitionException re) 
    	{
            ReportError(re);
            Recover(input,re);
    	// Conversion of the second argument necessary, but harmless
    	retval.Tree = (object)adaptor.ErrorNode(input, (IToken) retval.Start, input.LT(-1), re);

        }
        finally 
    	{
        }
        return retval;
    }
    // $ANTLR end "expr"

    public class primaryExpr_return : ParserRuleReturnScope
    {
        private object tree;
        override public object Tree
        {
        	get { return tree; }
        	set { tree = (object) value; }
        }
    };

    // $ANTLR start "primaryExpr"
    // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmXPath.g:140:1: primaryExpr : ( variableReference | '(' expr ')' | Literal | Number | functionCall );
    public MvmXPathParser.primaryExpr_return primaryExpr() // throws RecognitionException [1]
    {   
        MvmXPathParser.primaryExpr_return retval = new MvmXPathParser.primaryExpr_return();
        retval.Start = input.LT(1);

        object root_0 = null;

        IToken char_literal56 = null;
        IToken char_literal58 = null;
        IToken Literal59 = null;
        IToken Number60 = null;
        MvmXPathParser.variableReference_return variableReference55 = default(MvmXPathParser.variableReference_return);

        MvmXPathParser.expr_return expr57 = default(MvmXPathParser.expr_return);

        MvmXPathParser.functionCall_return functionCall61 = default(MvmXPathParser.functionCall_return);


        object char_literal56_tree=null;
        object char_literal58_tree=null;
        object Literal59_tree=null;
        object Number60_tree=null;

        try 
    	{
            // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmXPath.g:141:3: ( variableReference | '(' expr ')' | Literal | Number | functionCall )
            int alt18 = 5;
            switch ( input.LA(1) ) 
            {
            case 57:
            	{
                alt18 = 1;
                }
                break;
            case LPAR:
            	{
                alt18 = 2;
                }
                break;
            case Literal:
            	{
                alt18 = 3;
                }
                break;
            case Number:
            	{
                alt18 = 4;
                }
                break;
            case AxisName:
            case NCName:
            	{
                alt18 = 5;
                }
                break;
            	default:
            	    NoViableAltException nvae_d18s0 =
            	        new NoViableAltException("", 18, 0, input);

            	    throw nvae_d18s0;
            }

            switch (alt18) 
            {
                case 1 :
                    // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmXPath.g:141:6: variableReference
                    {
                    	root_0 = (object)adaptor.GetNilNode();

                    	PushFollow(FOLLOW_variableReference_in_primaryExpr885);
                    	variableReference55 = variableReference();
                    	state.followingStackPointer--;

                    	adaptor.AddChild(root_0, variableReference55.Tree);

                    }
                    break;
                case 2 :
                    // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmXPath.g:142:6: '(' expr ')'
                    {
                    	root_0 = (object)adaptor.GetNilNode();

                    	char_literal56=(IToken)Match(input,LPAR,FOLLOW_LPAR_in_primaryExpr892); 
                    		char_literal56_tree = (object)adaptor.Create(char_literal56);
                    		adaptor.AddChild(root_0, char_literal56_tree);

                    	PushFollow(FOLLOW_expr_in_primaryExpr894);
                    	expr57 = expr();
                    	state.followingStackPointer--;

                    	adaptor.AddChild(root_0, expr57.Tree);
                    	char_literal58=(IToken)Match(input,RPAR,FOLLOW_RPAR_in_primaryExpr896); 
                    		char_literal58_tree = (object)adaptor.Create(char_literal58);
                    		adaptor.AddChild(root_0, char_literal58_tree);


                    }
                    break;
                case 3 :
                    // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmXPath.g:143:6: Literal
                    {
                    	root_0 = (object)adaptor.GetNilNode();

                    	Literal59=(IToken)Match(input,Literal,FOLLOW_Literal_in_primaryExpr903); 
                    		Literal59_tree = (object)adaptor.Create(Literal59);
                    		adaptor.AddChild(root_0, Literal59_tree);


                    }
                    break;
                case 4 :
                    // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmXPath.g:144:6: Number
                    {
                    	root_0 = (object)adaptor.GetNilNode();

                    	Number60=(IToken)Match(input,Number,FOLLOW_Number_in_primaryExpr910); 
                    		Number60_tree = (object)adaptor.Create(Number60);
                    		adaptor.AddChild(root_0, Number60_tree);


                    }
                    break;
                case 5 :
                    // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmXPath.g:145:6: functionCall
                    {
                    	root_0 = (object)adaptor.GetNilNode();

                    	PushFollow(FOLLOW_functionCall_in_primaryExpr919);
                    	functionCall61 = functionCall();
                    	state.followingStackPointer--;

                    	adaptor.AddChild(root_0, functionCall61.Tree);

                    }
                    break;

            }
            retval.Stop = input.LT(-1);

            	retval.Tree = (object)adaptor.RulePostProcessing(root_0);
            	adaptor.SetTokenBoundaries(retval.Tree, (IToken) retval.Start, (IToken) retval.Stop);
        }
        catch (RecognitionException re) 
    	{
            ReportError(re);
            Recover(input,re);
    	// Conversion of the second argument necessary, but harmless
    	retval.Tree = (object)adaptor.ErrorNode(input, (IToken) retval.Start, input.LT(-1), re);

        }
        finally 
    	{
        }
        return retval;
    }
    // $ANTLR end "primaryExpr"

    public class functionCall_return : ParserRuleReturnScope
    {
        private object tree;
        override public object Tree
        {
        	get { return tree; }
        	set { tree = (object) value; }
        }
    };

    // $ANTLR start "functionCall"
    // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmXPath.g:148:1: functionCall : functionName '(' ( expr ( ',' expr )* )? ')' ;
    public MvmXPathParser.functionCall_return functionCall() // throws RecognitionException [1]
    {   
        MvmXPathParser.functionCall_return retval = new MvmXPathParser.functionCall_return();
        retval.Start = input.LT(1);

        object root_0 = null;

        IToken char_literal63 = null;
        IToken char_literal65 = null;
        IToken char_literal67 = null;
        MvmXPathParser.functionName_return functionName62 = default(MvmXPathParser.functionName_return);

        MvmXPathParser.expr_return expr64 = default(MvmXPathParser.expr_return);

        MvmXPathParser.expr_return expr66 = default(MvmXPathParser.expr_return);


        object char_literal63_tree=null;
        object char_literal65_tree=null;
        object char_literal67_tree=null;

        try 
    	{
            // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmXPath.g:149:3: ( functionName '(' ( expr ( ',' expr )* )? ')' )
            // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmXPath.g:149:6: functionName '(' ( expr ( ',' expr )* )? ')'
            {
            	root_0 = (object)adaptor.GetNilNode();

            	PushFollow(FOLLOW_functionName_in_functionCall933);
            	functionName62 = functionName();
            	state.followingStackPointer--;

            	adaptor.AddChild(root_0, functionName62.Tree);
            	char_literal63=(IToken)Match(input,LPAR,FOLLOW_LPAR_in_functionCall935); 
            		char_literal63_tree = (object)adaptor.Create(char_literal63);
            		adaptor.AddChild(root_0, char_literal63_tree);

            	// D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmXPath.g:149:23: ( expr ( ',' expr )* )?
            	int alt20 = 2;
            	int LA20_0 = input.LA(1);

            	if ( ((LA20_0 >= PATHSEP && LA20_0 <= LPAR) || LA20_0 == MINUS || (LA20_0 >= DOT && LA20_0 <= AT) || (LA20_0 >= AxisName && LA20_0 <= NCName) || (LA20_0 >= 46 && LA20_0 <= 50) || LA20_0 == 57) )
            	{
            	    alt20 = 1;
            	}
            	switch (alt20) 
            	{
            	    case 1 :
            	        // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmXPath.g:149:25: expr ( ',' expr )*
            	        {
            	        	PushFollow(FOLLOW_expr_in_functionCall939);
            	        	expr64 = expr();
            	        	state.followingStackPointer--;

            	        	adaptor.AddChild(root_0, expr64.Tree);
            	        	// D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmXPath.g:149:30: ( ',' expr )*
            	        	do 
            	        	{
            	        	    int alt19 = 2;
            	        	    int LA19_0 = input.LA(1);

            	        	    if ( (LA19_0 == COMMA) )
            	        	    {
            	        	        alt19 = 1;
            	        	    }


            	        	    switch (alt19) 
            	        		{
            	        			case 1 :
            	        			    // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmXPath.g:149:32: ',' expr
            	        			    {
            	        			    	char_literal65=(IToken)Match(input,COMMA,FOLLOW_COMMA_in_functionCall943); 
            	        			    		char_literal65_tree = (object)adaptor.Create(char_literal65);
            	        			    		adaptor.AddChild(root_0, char_literal65_tree);

            	        			    	PushFollow(FOLLOW_expr_in_functionCall945);
            	        			    	expr66 = expr();
            	        			    	state.followingStackPointer--;

            	        			    	adaptor.AddChild(root_0, expr66.Tree);

            	        			    }
            	        			    break;

            	        			default:
            	        			    goto loop19;
            	        	    }
            	        	} while (true);

            	        	loop19:
            	        		;	// Stops C# compiler whining that label 'loop19' has no statements


            	        }
            	        break;

            	}

            	char_literal67=(IToken)Match(input,RPAR,FOLLOW_RPAR_in_functionCall953); 
            		char_literal67_tree = (object)adaptor.Create(char_literal67);
            		adaptor.AddChild(root_0, char_literal67_tree);


            }

            retval.Stop = input.LT(-1);

            	retval.Tree = (object)adaptor.RulePostProcessing(root_0);
            	adaptor.SetTokenBoundaries(retval.Tree, (IToken) retval.Start, (IToken) retval.Stop);
        }
        catch (RecognitionException re) 
    	{
            ReportError(re);
            Recover(input,re);
    	// Conversion of the second argument necessary, but harmless
    	retval.Tree = (object)adaptor.ErrorNode(input, (IToken) retval.Start, input.LT(-1), re);

        }
        finally 
    	{
        }
        return retval;
    }
    // $ANTLR end "functionCall"

    public class unionExprNoRoot_return : ParserRuleReturnScope
    {
        private object tree;
        override public object Tree
        {
        	get { return tree; }
        	set { tree = (object) value; }
        }
    };

    // $ANTLR start "unionExprNoRoot"
    // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmXPath.g:152:1: unionExprNoRoot : ( pathExprNoRoot ( '|' unionExprNoRoot )? | '/' '|' unionExprNoRoot );
    public MvmXPathParser.unionExprNoRoot_return unionExprNoRoot() // throws RecognitionException [1]
    {   
        MvmXPathParser.unionExprNoRoot_return retval = new MvmXPathParser.unionExprNoRoot_return();
        retval.Start = input.LT(1);

        object root_0 = null;

        IToken char_literal69 = null;
        IToken char_literal71 = null;
        IToken char_literal72 = null;
        MvmXPathParser.pathExprNoRoot_return pathExprNoRoot68 = default(MvmXPathParser.pathExprNoRoot_return);

        MvmXPathParser.unionExprNoRoot_return unionExprNoRoot70 = default(MvmXPathParser.unionExprNoRoot_return);

        MvmXPathParser.unionExprNoRoot_return unionExprNoRoot73 = default(MvmXPathParser.unionExprNoRoot_return);


        object char_literal69_tree=null;
        object char_literal71_tree=null;
        object char_literal72_tree=null;

        try 
    	{
            // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmXPath.g:153:3: ( pathExprNoRoot ( '|' unionExprNoRoot )? | '/' '|' unionExprNoRoot )
            int alt22 = 2;
            int LA22_0 = input.LA(1);

            if ( ((LA22_0 >= ABRPATH && LA22_0 <= LPAR) || (LA22_0 >= DOT && LA22_0 <= AT) || (LA22_0 >= AxisName && LA22_0 <= NCName) || (LA22_0 >= 46 && LA22_0 <= 50) || LA22_0 == 57) )
            {
                alt22 = 1;
            }
            else if ( (LA22_0 == PATHSEP) )
            {
                int LA22_2 = input.LA(2);

                if ( (LA22_2 == PIPE) )
                {
                    alt22 = 2;
                }
                else if ( (LA22_2 == MUL || LA22_2 == AT || (LA22_2 >= AxisName && LA22_2 <= NodeType) || LA22_2 == NCName || LA22_2 == 50) )
                {
                    alt22 = 1;
                }
                else 
                {
                    NoViableAltException nvae_d22s2 =
                        new NoViableAltException("", 22, 2, input);

                    throw nvae_d22s2;
                }
            }
            else 
            {
                NoViableAltException nvae_d22s0 =
                    new NoViableAltException("", 22, 0, input);

                throw nvae_d22s0;
            }
            switch (alt22) 
            {
                case 1 :
                    // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmXPath.g:153:6: pathExprNoRoot ( '|' unionExprNoRoot )?
                    {
                    	root_0 = (object)adaptor.GetNilNode();

                    	PushFollow(FOLLOW_pathExprNoRoot_in_unionExprNoRoot967);
                    	pathExprNoRoot68 = pathExprNoRoot();
                    	state.followingStackPointer--;

                    	adaptor.AddChild(root_0, pathExprNoRoot68.Tree);
                    	// D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmXPath.g:153:21: ( '|' unionExprNoRoot )?
                    	int alt21 = 2;
                    	int LA21_0 = input.LA(1);

                    	if ( (LA21_0 == PIPE) )
                    	{
                    	    alt21 = 1;
                    	}
                    	switch (alt21) 
                    	{
                    	    case 1 :
                    	        // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmXPath.g:153:22: '|' unionExprNoRoot
                    	        {
                    	        	char_literal69=(IToken)Match(input,PIPE,FOLLOW_PIPE_in_unionExprNoRoot970); 
                    	        		char_literal69_tree = (object)adaptor.Create(char_literal69);
                    	        		root_0 = (object)adaptor.BecomeRoot(char_literal69_tree, root_0);

                    	        	PushFollow(FOLLOW_unionExprNoRoot_in_unionExprNoRoot973);
                    	        	unionExprNoRoot70 = unionExprNoRoot();
                    	        	state.followingStackPointer--;

                    	        	adaptor.AddChild(root_0, unionExprNoRoot70.Tree);

                    	        }
                    	        break;

                    	}


                    }
                    break;
                case 2 :
                    // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmXPath.g:154:6: '/' '|' unionExprNoRoot
                    {
                    	root_0 = (object)adaptor.GetNilNode();

                    	char_literal71=(IToken)Match(input,PATHSEP,FOLLOW_PATHSEP_in_unionExprNoRoot982); 
                    		char_literal71_tree = (object)adaptor.Create(char_literal71);
                    		adaptor.AddChild(root_0, char_literal71_tree);

                    	char_literal72=(IToken)Match(input,PIPE,FOLLOW_PIPE_in_unionExprNoRoot984); 
                    		char_literal72_tree = (object)adaptor.Create(char_literal72);
                    		root_0 = (object)adaptor.BecomeRoot(char_literal72_tree, root_0);

                    	PushFollow(FOLLOW_unionExprNoRoot_in_unionExprNoRoot987);
                    	unionExprNoRoot73 = unionExprNoRoot();
                    	state.followingStackPointer--;

                    	adaptor.AddChild(root_0, unionExprNoRoot73.Tree);

                    }
                    break;

            }
            retval.Stop = input.LT(-1);

            	retval.Tree = (object)adaptor.RulePostProcessing(root_0);
            	adaptor.SetTokenBoundaries(retval.Tree, (IToken) retval.Start, (IToken) retval.Stop);
        }
        catch (RecognitionException re) 
    	{
            ReportError(re);
            Recover(input,re);
    	// Conversion of the second argument necessary, but harmless
    	retval.Tree = (object)adaptor.ErrorNode(input, (IToken) retval.Start, input.LT(-1), re);

        }
        finally 
    	{
        }
        return retval;
    }
    // $ANTLR end "unionExprNoRoot"

    public class pathExprNoRoot_return : ParserRuleReturnScope
    {
        private object tree;
        override public object Tree
        {
        	get { return tree; }
        	set { tree = (object) value; }
        }
    };

    // $ANTLR start "pathExprNoRoot"
    // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmXPath.g:157:1: pathExprNoRoot : ( locationPath | filterExpr ( ( '/' | '//' ) relativeLocationPath )? );
    public MvmXPathParser.pathExprNoRoot_return pathExprNoRoot() // throws RecognitionException [1]
    {   
        MvmXPathParser.pathExprNoRoot_return retval = new MvmXPathParser.pathExprNoRoot_return();
        retval.Start = input.LT(1);

        object root_0 = null;

        IToken set76 = null;
        MvmXPathParser.locationPath_return locationPath74 = default(MvmXPathParser.locationPath_return);

        MvmXPathParser.filterExpr_return filterExpr75 = default(MvmXPathParser.filterExpr_return);

        MvmXPathParser.relativeLocationPath_return relativeLocationPath77 = default(MvmXPathParser.relativeLocationPath_return);


        object set76_tree=null;

        try 
    	{
            // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmXPath.g:158:3: ( locationPath | filterExpr ( ( '/' | '//' ) relativeLocationPath )? )
            int alt24 = 2;
            switch ( input.LA(1) ) 
            {
            case PATHSEP:
            case ABRPATH:
            case DOT:
            case MUL:
            case DOTDOT:
            case AT:
            case NodeType:
            case 46:
            case 47:
            case 48:
            case 49:
            case 50:
            	{
                alt24 = 1;
                }
                break;
            case AxisName:
            	{
                switch ( input.LA(2) ) 
                {
                case EOF:
                case PATHSEP:
                case ABRPATH:
                case RPAR:
                case LBRAC:
                case RBRAC:
                case MINUS:
                case PLUS:
                case MUL:
                case COMMA:
                case PIPE:
                case LESS:
                case MORE:
                case LE:
                case GE:
                case CC:
                case 46:
                case 47:
                case 48:
                case 49:
                case 51:
                case 52:
                case 53:
                case 54:
                case 55:
                case 56:
                	{
                    alt24 = 1;
                    }
                    break;
                case COLON:
                	{
                    int LA24_5 = input.LA(3);

                    if ( (LA24_5 == MUL) )
                    {
                        alt24 = 1;
                    }
                    else if ( (LA24_5 == AxisName || LA24_5 == NCName) )
                    {
                        int LA24_6 = input.LA(4);

                        if ( (LA24_6 == EOF || (LA24_6 >= PATHSEP && LA24_6 <= ABRPATH) || (LA24_6 >= RPAR && LA24_6 <= PLUS) || LA24_6 == MUL || (LA24_6 >= COMMA && LA24_6 <= GE) || (LA24_6 >= 46 && LA24_6 <= 49) || (LA24_6 >= 51 && LA24_6 <= 56)) )
                        {
                            alt24 = 1;
                        }
                        else if ( (LA24_6 == LPAR) )
                        {
                            alt24 = 2;
                        }
                        else 
                        {
                            NoViableAltException nvae_d24s6 =
                                new NoViableAltException("", 24, 6, input);

                            throw nvae_d24s6;
                        }
                    }
                    else 
                    {
                        NoViableAltException nvae_d24s5 =
                            new NoViableAltException("", 24, 5, input);

                        throw nvae_d24s5;
                    }
                    }
                    break;
                case LPAR:
                	{
                    alt24 = 2;
                    }
                    break;
                	default:
                	    NoViableAltException nvae_d24s2 =
                	        new NoViableAltException("", 24, 2, input);

                	    throw nvae_d24s2;
                }

                }
                break;
            case NCName:
            	{
                switch ( input.LA(2) ) 
                {
                case COLON:
                	{
                    int LA24_5 = input.LA(3);

                    if ( (LA24_5 == MUL) )
                    {
                        alt24 = 1;
                    }
                    else if ( (LA24_5 == AxisName || LA24_5 == NCName) )
                    {
                        int LA24_6 = input.LA(4);

                        if ( (LA24_6 == EOF || (LA24_6 >= PATHSEP && LA24_6 <= ABRPATH) || (LA24_6 >= RPAR && LA24_6 <= PLUS) || LA24_6 == MUL || (LA24_6 >= COMMA && LA24_6 <= GE) || (LA24_6 >= 46 && LA24_6 <= 49) || (LA24_6 >= 51 && LA24_6 <= 56)) )
                        {
                            alt24 = 1;
                        }
                        else if ( (LA24_6 == LPAR) )
                        {
                            alt24 = 2;
                        }
                        else 
                        {
                            NoViableAltException nvae_d24s6 =
                                new NoViableAltException("", 24, 6, input);

                            throw nvae_d24s6;
                        }
                    }
                    else 
                    {
                        NoViableAltException nvae_d24s5 =
                            new NoViableAltException("", 24, 5, input);

                        throw nvae_d24s5;
                    }
                    }
                    break;
                case LPAR:
                	{
                    alt24 = 2;
                    }
                    break;
                case EOF:
                case PATHSEP:
                case ABRPATH:
                case RPAR:
                case LBRAC:
                case RBRAC:
                case MINUS:
                case PLUS:
                case MUL:
                case COMMA:
                case PIPE:
                case LESS:
                case MORE:
                case LE:
                case GE:
                case 46:
                case 47:
                case 48:
                case 49:
                case 51:
                case 52:
                case 53:
                case 54:
                case 55:
                case 56:
                	{
                    alt24 = 1;
                    }
                    break;
                	default:
                	    NoViableAltException nvae_d24s3 =
                	        new NoViableAltException("", 24, 3, input);

                	    throw nvae_d24s3;
                }

                }
                break;
            case LPAR:
            case Literal:
            case Number:
            case 57:
            	{
                alt24 = 2;
                }
                break;
            	default:
            	    NoViableAltException nvae_d24s0 =
            	        new NoViableAltException("", 24, 0, input);

            	    throw nvae_d24s0;
            }

            switch (alt24) 
            {
                case 1 :
                    // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmXPath.g:158:6: locationPath
                    {
                    	root_0 = (object)adaptor.GetNilNode();

                    	PushFollow(FOLLOW_locationPath_in_pathExprNoRoot1001);
                    	locationPath74 = locationPath();
                    	state.followingStackPointer--;

                    	adaptor.AddChild(root_0, locationPath74.Tree);

                    }
                    break;
                case 2 :
                    // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmXPath.g:159:6: filterExpr ( ( '/' | '//' ) relativeLocationPath )?
                    {
                    	root_0 = (object)adaptor.GetNilNode();

                    	PushFollow(FOLLOW_filterExpr_in_pathExprNoRoot1008);
                    	filterExpr75 = filterExpr();
                    	state.followingStackPointer--;

                    	adaptor.AddChild(root_0, filterExpr75.Tree);
                    	// D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmXPath.g:159:17: ( ( '/' | '//' ) relativeLocationPath )?
                    	int alt23 = 2;
                    	int LA23_0 = input.LA(1);

                    	if ( ((LA23_0 >= PATHSEP && LA23_0 <= ABRPATH)) )
                    	{
                    	    alt23 = 1;
                    	}
                    	switch (alt23) 
                    	{
                    	    case 1 :
                    	        // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmXPath.g:159:18: ( '/' | '//' ) relativeLocationPath
                    	        {
                    	        	set76 = (IToken)input.LT(1);
                    	        	if ( (input.LA(1) >= PATHSEP && input.LA(1) <= ABRPATH) ) 
                    	        	{
                    	        	    input.Consume();
                    	        	    adaptor.AddChild(root_0, (object)adaptor.Create(set76));
                    	        	    state.errorRecovery = false;
                    	        	}
                    	        	else 
                    	        	{
                    	        	    MismatchedSetException mse = new MismatchedSetException(null,input);
                    	        	    throw mse;
                    	        	}

                    	        	PushFollow(FOLLOW_relativeLocationPath_in_pathExprNoRoot1017);
                    	        	relativeLocationPath77 = relativeLocationPath();
                    	        	state.followingStackPointer--;

                    	        	adaptor.AddChild(root_0, relativeLocationPath77.Tree);

                    	        }
                    	        break;

                    	}


                    }
                    break;

            }
            retval.Stop = input.LT(-1);

            	retval.Tree = (object)adaptor.RulePostProcessing(root_0);
            	adaptor.SetTokenBoundaries(retval.Tree, (IToken) retval.Start, (IToken) retval.Stop);
        }
        catch (RecognitionException re) 
    	{
            ReportError(re);
            Recover(input,re);
    	// Conversion of the second argument necessary, but harmless
    	retval.Tree = (object)adaptor.ErrorNode(input, (IToken) retval.Start, input.LT(-1), re);

        }
        finally 
    	{
        }
        return retval;
    }
    // $ANTLR end "pathExprNoRoot"

    public class filterExpr_return : ParserRuleReturnScope
    {
        private object tree;
        override public object Tree
        {
        	get { return tree; }
        	set { tree = (object) value; }
        }
    };

    // $ANTLR start "filterExpr"
    // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmXPath.g:162:1: filterExpr : primaryExpr ( predicate )* ;
    public MvmXPathParser.filterExpr_return filterExpr() // throws RecognitionException [1]
    {   
        MvmXPathParser.filterExpr_return retval = new MvmXPathParser.filterExpr_return();
        retval.Start = input.LT(1);

        object root_0 = null;

        MvmXPathParser.primaryExpr_return primaryExpr78 = default(MvmXPathParser.primaryExpr_return);

        MvmXPathParser.predicate_return predicate79 = default(MvmXPathParser.predicate_return);



        try 
    	{
            // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmXPath.g:163:3: ( primaryExpr ( predicate )* )
            // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmXPath.g:163:6: primaryExpr ( predicate )*
            {
            	root_0 = (object)adaptor.GetNilNode();

            	PushFollow(FOLLOW_primaryExpr_in_filterExpr1033);
            	primaryExpr78 = primaryExpr();
            	state.followingStackPointer--;

            	adaptor.AddChild(root_0, primaryExpr78.Tree);
            	// D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmXPath.g:163:18: ( predicate )*
            	do 
            	{
            	    int alt25 = 2;
            	    int LA25_0 = input.LA(1);

            	    if ( (LA25_0 == LBRAC) )
            	    {
            	        alt25 = 1;
            	    }


            	    switch (alt25) 
            		{
            			case 1 :
            			    // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmXPath.g:163:18: predicate
            			    {
            			    	PushFollow(FOLLOW_predicate_in_filterExpr1035);
            			    	predicate79 = predicate();
            			    	state.followingStackPointer--;

            			    	adaptor.AddChild(root_0, predicate79.Tree);

            			    }
            			    break;

            			default:
            			    goto loop25;
            	    }
            	} while (true);

            	loop25:
            		;	// Stops C# compiler whining that label 'loop25' has no statements


            }

            retval.Stop = input.LT(-1);

            	retval.Tree = (object)adaptor.RulePostProcessing(root_0);
            	adaptor.SetTokenBoundaries(retval.Tree, (IToken) retval.Start, (IToken) retval.Stop);
        }
        catch (RecognitionException re) 
    	{
            ReportError(re);
            Recover(input,re);
    	// Conversion of the second argument necessary, but harmless
    	retval.Tree = (object)adaptor.ErrorNode(input, (IToken) retval.Start, input.LT(-1), re);

        }
        finally 
    	{
        }
        return retval;
    }
    // $ANTLR end "filterExpr"

    public class orExpr_return : ParserRuleReturnScope
    {
        private object tree;
        override public object Tree
        {
        	get { return tree; }
        	set { tree = (object) value; }
        }
    };

    // $ANTLR start "orExpr"
    // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmXPath.g:166:1: orExpr : andExpr ( 'or' andExpr )* ;
    public MvmXPathParser.orExpr_return orExpr() // throws RecognitionException [1]
    {   
        MvmXPathParser.orExpr_return retval = new MvmXPathParser.orExpr_return();
        retval.Start = input.LT(1);

        object root_0 = null;

        IToken string_literal81 = null;
        MvmXPathParser.andExpr_return andExpr80 = default(MvmXPathParser.andExpr_return);

        MvmXPathParser.andExpr_return andExpr82 = default(MvmXPathParser.andExpr_return);


        object string_literal81_tree=null;

        try 
    	{
            // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmXPath.g:166:9: ( andExpr ( 'or' andExpr )* )
            // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmXPath.g:166:12: andExpr ( 'or' andExpr )*
            {
            	root_0 = (object)adaptor.GetNilNode();

            	PushFollow(FOLLOW_andExpr_in_orExpr1049);
            	andExpr80 = andExpr();
            	state.followingStackPointer--;

            	adaptor.AddChild(root_0, andExpr80.Tree);
            	// D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmXPath.g:166:20: ( 'or' andExpr )*
            	do 
            	{
            	    int alt26 = 2;
            	    int LA26_0 = input.LA(1);

            	    if ( (LA26_0 == 51) )
            	    {
            	        alt26 = 1;
            	    }


            	    switch (alt26) 
            		{
            			case 1 :
            			    // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmXPath.g:166:21: 'or' andExpr
            			    {
            			    	string_literal81=(IToken)Match(input,51,FOLLOW_51_in_orExpr1052); 
            			    		string_literal81_tree = (object)adaptor.Create(string_literal81);
            			    		root_0 = (object)adaptor.BecomeRoot(string_literal81_tree, root_0);

            			    	PushFollow(FOLLOW_andExpr_in_orExpr1055);
            			    	andExpr82 = andExpr();
            			    	state.followingStackPointer--;

            			    	adaptor.AddChild(root_0, andExpr82.Tree);

            			    }
            			    break;

            			default:
            			    goto loop26;
            	    }
            	} while (true);

            	loop26:
            		;	// Stops C# compiler whining that label 'loop26' has no statements


            }

            retval.Stop = input.LT(-1);

            	retval.Tree = (object)adaptor.RulePostProcessing(root_0);
            	adaptor.SetTokenBoundaries(retval.Tree, (IToken) retval.Start, (IToken) retval.Stop);
        }
        catch (RecognitionException re) 
    	{
            ReportError(re);
            Recover(input,re);
    	// Conversion of the second argument necessary, but harmless
    	retval.Tree = (object)adaptor.ErrorNode(input, (IToken) retval.Start, input.LT(-1), re);

        }
        finally 
    	{
        }
        return retval;
    }
    // $ANTLR end "orExpr"

    public class andExpr_return : ParserRuleReturnScope
    {
        private object tree;
        override public object Tree
        {
        	get { return tree; }
        	set { tree = (object) value; }
        }
    };

    // $ANTLR start "andExpr"
    // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmXPath.g:169:1: andExpr : equalityExpr ( 'and' equalityExpr )* ;
    public MvmXPathParser.andExpr_return andExpr() // throws RecognitionException [1]
    {   
        MvmXPathParser.andExpr_return retval = new MvmXPathParser.andExpr_return();
        retval.Start = input.LT(1);

        object root_0 = null;

        IToken string_literal84 = null;
        MvmXPathParser.equalityExpr_return equalityExpr83 = default(MvmXPathParser.equalityExpr_return);

        MvmXPathParser.equalityExpr_return equalityExpr85 = default(MvmXPathParser.equalityExpr_return);


        object string_literal84_tree=null;

        try 
    	{
            // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmXPath.g:169:10: ( equalityExpr ( 'and' equalityExpr )* )
            // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmXPath.g:169:13: equalityExpr ( 'and' equalityExpr )*
            {
            	root_0 = (object)adaptor.GetNilNode();

            	PushFollow(FOLLOW_equalityExpr_in_andExpr1070);
            	equalityExpr83 = equalityExpr();
            	state.followingStackPointer--;

            	adaptor.AddChild(root_0, equalityExpr83.Tree);
            	// D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmXPath.g:169:26: ( 'and' equalityExpr )*
            	do 
            	{
            	    int alt27 = 2;
            	    int LA27_0 = input.LA(1);

            	    if ( (LA27_0 == 52) )
            	    {
            	        alt27 = 1;
            	    }


            	    switch (alt27) 
            		{
            			case 1 :
            			    // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmXPath.g:169:27: 'and' equalityExpr
            			    {
            			    	string_literal84=(IToken)Match(input,52,FOLLOW_52_in_andExpr1073); 
            			    		string_literal84_tree = (object)adaptor.Create(string_literal84);
            			    		root_0 = (object)adaptor.BecomeRoot(string_literal84_tree, root_0);

            			    	PushFollow(FOLLOW_equalityExpr_in_andExpr1076);
            			    	equalityExpr85 = equalityExpr();
            			    	state.followingStackPointer--;

            			    	adaptor.AddChild(root_0, equalityExpr85.Tree);

            			    }
            			    break;

            			default:
            			    goto loop27;
            	    }
            	} while (true);

            	loop27:
            		;	// Stops C# compiler whining that label 'loop27' has no statements


            }

            retval.Stop = input.LT(-1);

            	retval.Tree = (object)adaptor.RulePostProcessing(root_0);
            	adaptor.SetTokenBoundaries(retval.Tree, (IToken) retval.Start, (IToken) retval.Stop);
        }
        catch (RecognitionException re) 
    	{
            ReportError(re);
            Recover(input,re);
    	// Conversion of the second argument necessary, but harmless
    	retval.Tree = (object)adaptor.ErrorNode(input, (IToken) retval.Start, input.LT(-1), re);

        }
        finally 
    	{
        }
        return retval;
    }
    // $ANTLR end "andExpr"

    public class equalityExpr_return : ParserRuleReturnScope
    {
        private object tree;
        override public object Tree
        {
        	get { return tree; }
        	set { tree = (object) value; }
        }
    };

    // $ANTLR start "equalityExpr"
    // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmXPath.g:172:1: equalityExpr : relationalExpr ( ( '=' | '!=' ) relationalExpr )* ;
    public MvmXPathParser.equalityExpr_return equalityExpr() // throws RecognitionException [1]
    {   
        MvmXPathParser.equalityExpr_return retval = new MvmXPathParser.equalityExpr_return();
        retval.Start = input.LT(1);

        object root_0 = null;

        IToken set87 = null;
        MvmXPathParser.relationalExpr_return relationalExpr86 = default(MvmXPathParser.relationalExpr_return);

        MvmXPathParser.relationalExpr_return relationalExpr88 = default(MvmXPathParser.relationalExpr_return);


        object set87_tree=null;

        try 
    	{
            // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmXPath.g:173:3: ( relationalExpr ( ( '=' | '!=' ) relationalExpr )* )
            // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmXPath.g:173:6: relationalExpr ( ( '=' | '!=' ) relationalExpr )*
            {
            	root_0 = (object)adaptor.GetNilNode();

            	PushFollow(FOLLOW_relationalExpr_in_equalityExpr1092);
            	relationalExpr86 = relationalExpr();
            	state.followingStackPointer--;

            	adaptor.AddChild(root_0, relationalExpr86.Tree);
            	// D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmXPath.g:173:21: ( ( '=' | '!=' ) relationalExpr )*
            	do 
            	{
            	    int alt28 = 2;
            	    int LA28_0 = input.LA(1);

            	    if ( ((LA28_0 >= 53 && LA28_0 <= 54)) )
            	    {
            	        alt28 = 1;
            	    }


            	    switch (alt28) 
            		{
            			case 1 :
            			    // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmXPath.g:173:22: ( '=' | '!=' ) relationalExpr
            			    {
            			    	set87=(IToken)input.LT(1);
            			    	set87 = (IToken)input.LT(1);
            			    	if ( (input.LA(1) >= 53 && input.LA(1) <= 54) ) 
            			    	{
            			    	    input.Consume();
            			    	    root_0 = (object)adaptor.BecomeRoot((object)adaptor.Create(set87), root_0);
            			    	    state.errorRecovery = false;
            			    	}
            			    	else 
            			    	{
            			    	    MismatchedSetException mse = new MismatchedSetException(null,input);
            			    	    throw mse;
            			    	}

            			    	PushFollow(FOLLOW_relationalExpr_in_equalityExpr1102);
            			    	relationalExpr88 = relationalExpr();
            			    	state.followingStackPointer--;

            			    	adaptor.AddChild(root_0, relationalExpr88.Tree);

            			    }
            			    break;

            			default:
            			    goto loop28;
            	    }
            	} while (true);

            	loop28:
            		;	// Stops C# compiler whining that label 'loop28' has no statements


            }

            retval.Stop = input.LT(-1);

            	retval.Tree = (object)adaptor.RulePostProcessing(root_0);
            	adaptor.SetTokenBoundaries(retval.Tree, (IToken) retval.Start, (IToken) retval.Stop);
        }
        catch (RecognitionException re) 
    	{
            ReportError(re);
            Recover(input,re);
    	// Conversion of the second argument necessary, but harmless
    	retval.Tree = (object)adaptor.ErrorNode(input, (IToken) retval.Start, input.LT(-1), re);

        }
        finally 
    	{
        }
        return retval;
    }
    // $ANTLR end "equalityExpr"

    public class relationalExpr_return : ParserRuleReturnScope
    {
        private object tree;
        override public object Tree
        {
        	get { return tree; }
        	set { tree = (object) value; }
        }
    };

    // $ANTLR start "relationalExpr"
    // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmXPath.g:176:1: relationalExpr : additiveExpr ( ( '<' | '>' | '<=' | '>=' ) additiveExpr )* ;
    public MvmXPathParser.relationalExpr_return relationalExpr() // throws RecognitionException [1]
    {   
        MvmXPathParser.relationalExpr_return retval = new MvmXPathParser.relationalExpr_return();
        retval.Start = input.LT(1);

        object root_0 = null;

        IToken set90 = null;
        MvmXPathParser.additiveExpr_return additiveExpr89 = default(MvmXPathParser.additiveExpr_return);

        MvmXPathParser.additiveExpr_return additiveExpr91 = default(MvmXPathParser.additiveExpr_return);


        object set90_tree=null;

        try 
    	{
            // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmXPath.g:177:3: ( additiveExpr ( ( '<' | '>' | '<=' | '>=' ) additiveExpr )* )
            // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmXPath.g:177:6: additiveExpr ( ( '<' | '>' | '<=' | '>=' ) additiveExpr )*
            {
            	root_0 = (object)adaptor.GetNilNode();

            	PushFollow(FOLLOW_additiveExpr_in_relationalExpr1118);
            	additiveExpr89 = additiveExpr();
            	state.followingStackPointer--;

            	adaptor.AddChild(root_0, additiveExpr89.Tree);
            	// D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmXPath.g:177:19: ( ( '<' | '>' | '<=' | '>=' ) additiveExpr )*
            	do 
            	{
            	    int alt29 = 2;
            	    int LA29_0 = input.LA(1);

            	    if ( ((LA29_0 >= LESS && LA29_0 <= GE)) )
            	    {
            	        alt29 = 1;
            	    }


            	    switch (alt29) 
            		{
            			case 1 :
            			    // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmXPath.g:177:20: ( '<' | '>' | '<=' | '>=' ) additiveExpr
            			    {
            			    	set90=(IToken)input.LT(1);
            			    	set90 = (IToken)input.LT(1);
            			    	if ( (input.LA(1) >= LESS && input.LA(1) <= GE) ) 
            			    	{
            			    	    input.Consume();
            			    	    root_0 = (object)adaptor.BecomeRoot((object)adaptor.Create(set90), root_0);
            			    	    state.errorRecovery = false;
            			    	}
            			    	else 
            			    	{
            			    	    MismatchedSetException mse = new MismatchedSetException(null,input);
            			    	    throw mse;
            			    	}

            			    	PushFollow(FOLLOW_additiveExpr_in_relationalExpr1132);
            			    	additiveExpr91 = additiveExpr();
            			    	state.followingStackPointer--;

            			    	adaptor.AddChild(root_0, additiveExpr91.Tree);

            			    }
            			    break;

            			default:
            			    goto loop29;
            	    }
            	} while (true);

            	loop29:
            		;	// Stops C# compiler whining that label 'loop29' has no statements


            }

            retval.Stop = input.LT(-1);

            	retval.Tree = (object)adaptor.RulePostProcessing(root_0);
            	adaptor.SetTokenBoundaries(retval.Tree, (IToken) retval.Start, (IToken) retval.Stop);
        }
        catch (RecognitionException re) 
    	{
            ReportError(re);
            Recover(input,re);
    	// Conversion of the second argument necessary, but harmless
    	retval.Tree = (object)adaptor.ErrorNode(input, (IToken) retval.Start, input.LT(-1), re);

        }
        finally 
    	{
        }
        return retval;
    }
    // $ANTLR end "relationalExpr"

    public class additiveExpr_return : ParserRuleReturnScope
    {
        private object tree;
        override public object Tree
        {
        	get { return tree; }
        	set { tree = (object) value; }
        }
    };

    // $ANTLR start "additiveExpr"
    // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmXPath.g:180:1: additiveExpr : multiplicativeExpr ( ( '+' | '-' ) multiplicativeExpr )* ;
    public MvmXPathParser.additiveExpr_return additiveExpr() // throws RecognitionException [1]
    {   
        MvmXPathParser.additiveExpr_return retval = new MvmXPathParser.additiveExpr_return();
        retval.Start = input.LT(1);

        object root_0 = null;

        IToken set93 = null;
        MvmXPathParser.multiplicativeExpr_return multiplicativeExpr92 = default(MvmXPathParser.multiplicativeExpr_return);

        MvmXPathParser.multiplicativeExpr_return multiplicativeExpr94 = default(MvmXPathParser.multiplicativeExpr_return);


        object set93_tree=null;

        try 
    	{
            // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmXPath.g:181:3: ( multiplicativeExpr ( ( '+' | '-' ) multiplicativeExpr )* )
            // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmXPath.g:181:6: multiplicativeExpr ( ( '+' | '-' ) multiplicativeExpr )*
            {
            	root_0 = (object)adaptor.GetNilNode();

            	PushFollow(FOLLOW_multiplicativeExpr_in_additiveExpr1148);
            	multiplicativeExpr92 = multiplicativeExpr();
            	state.followingStackPointer--;

            	adaptor.AddChild(root_0, multiplicativeExpr92.Tree);
            	// D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmXPath.g:181:25: ( ( '+' | '-' ) multiplicativeExpr )*
            	do 
            	{
            	    int alt30 = 2;
            	    int LA30_0 = input.LA(1);

            	    if ( ((LA30_0 >= MINUS && LA30_0 <= PLUS)) )
            	    {
            	        alt30 = 1;
            	    }


            	    switch (alt30) 
            		{
            			case 1 :
            			    // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmXPath.g:181:26: ( '+' | '-' ) multiplicativeExpr
            			    {
            			    	set93=(IToken)input.LT(1);
            			    	set93 = (IToken)input.LT(1);
            			    	if ( (input.LA(1) >= MINUS && input.LA(1) <= PLUS) ) 
            			    	{
            			    	    input.Consume();
            			    	    root_0 = (object)adaptor.BecomeRoot((object)adaptor.Create(set93), root_0);
            			    	    state.errorRecovery = false;
            			    	}
            			    	else 
            			    	{
            			    	    MismatchedSetException mse = new MismatchedSetException(null,input);
            			    	    throw mse;
            			    	}

            			    	PushFollow(FOLLOW_multiplicativeExpr_in_additiveExpr1158);
            			    	multiplicativeExpr94 = multiplicativeExpr();
            			    	state.followingStackPointer--;

            			    	adaptor.AddChild(root_0, multiplicativeExpr94.Tree);

            			    }
            			    break;

            			default:
            			    goto loop30;
            	    }
            	} while (true);

            	loop30:
            		;	// Stops C# compiler whining that label 'loop30' has no statements


            }

            retval.Stop = input.LT(-1);

            	retval.Tree = (object)adaptor.RulePostProcessing(root_0);
            	adaptor.SetTokenBoundaries(retval.Tree, (IToken) retval.Start, (IToken) retval.Stop);
        }
        catch (RecognitionException re) 
    	{
            ReportError(re);
            Recover(input,re);
    	// Conversion of the second argument necessary, but harmless
    	retval.Tree = (object)adaptor.ErrorNode(input, (IToken) retval.Start, input.LT(-1), re);

        }
        finally 
    	{
        }
        return retval;
    }
    // $ANTLR end "additiveExpr"

    public class multiplicativeExpr_return : ParserRuleReturnScope
    {
        private object tree;
        override public object Tree
        {
        	get { return tree; }
        	set { tree = (object) value; }
        }
    };

    // $ANTLR start "multiplicativeExpr"
    // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmXPath.g:184:1: multiplicativeExpr : ( unaryExprNoRoot ( ( '*' | 'div' | 'numMods' ) multiplicativeExpr )? | '/' ( ( 'div' | 'numMods' ) multiplicativeExpr )? );
    public MvmXPathParser.multiplicativeExpr_return multiplicativeExpr() // throws RecognitionException [1]
    {   
        MvmXPathParser.multiplicativeExpr_return retval = new MvmXPathParser.multiplicativeExpr_return();
        retval.Start = input.LT(1);

        object root_0 = null;

        IToken set96 = null;
        IToken char_literal98 = null;
        IToken set99 = null;
        MvmXPathParser.unaryExprNoRoot_return unaryExprNoRoot95 = default(MvmXPathParser.unaryExprNoRoot_return);

        MvmXPathParser.multiplicativeExpr_return multiplicativeExpr97 = default(MvmXPathParser.multiplicativeExpr_return);

        MvmXPathParser.multiplicativeExpr_return multiplicativeExpr100 = default(MvmXPathParser.multiplicativeExpr_return);


        object set96_tree=null;
        object char_literal98_tree=null;
        object set99_tree=null;

        try 
    	{
            // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmXPath.g:185:3: ( unaryExprNoRoot ( ( '*' | 'div' | 'numMods' ) multiplicativeExpr )? | '/' ( ( 'div' | 'numMods' ) multiplicativeExpr )? )
            int alt33 = 2;
            int LA33_0 = input.LA(1);

            if ( ((LA33_0 >= ABRPATH && LA33_0 <= LPAR) || LA33_0 == MINUS || (LA33_0 >= DOT && LA33_0 <= AT) || (LA33_0 >= AxisName && LA33_0 <= NCName) || (LA33_0 >= 46 && LA33_0 <= 50) || LA33_0 == 57) )
            {
                alt33 = 1;
            }
            else if ( (LA33_0 == PATHSEP) )
            {
                int LA33_2 = input.LA(2);

                if ( (LA33_2 == MUL || LA33_2 == AT || LA33_2 == PIPE || (LA33_2 >= AxisName && LA33_2 <= NodeType) || LA33_2 == NCName || LA33_2 == 50) )
                {
                    alt33 = 1;
                }
                else if ( (LA33_2 == EOF || LA33_2 == RPAR || (LA33_2 >= RBRAC && LA33_2 <= PLUS) || LA33_2 == COMMA || (LA33_2 >= LESS && LA33_2 <= GE) || (LA33_2 >= 51 && LA33_2 <= 56)) )
                {
                    alt33 = 2;
                }
                else 
                {
                    NoViableAltException nvae_d33s2 =
                        new NoViableAltException("", 33, 2, input);

                    throw nvae_d33s2;
                }
            }
            else 
            {
                NoViableAltException nvae_d33s0 =
                    new NoViableAltException("", 33, 0, input);

                throw nvae_d33s0;
            }
            switch (alt33) 
            {
                case 1 :
                    // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmXPath.g:185:6: unaryExprNoRoot ( ( '*' | 'div' | 'numMods' ) multiplicativeExpr )?
                    {
                    	root_0 = (object)adaptor.GetNilNode();

                    	PushFollow(FOLLOW_unaryExprNoRoot_in_multiplicativeExpr1174);
                    	unaryExprNoRoot95 = unaryExprNoRoot();
                    	state.followingStackPointer--;

                    	adaptor.AddChild(root_0, unaryExprNoRoot95.Tree);
                    	// D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmXPath.g:185:22: ( ( '*' | 'div' | 'numMods' ) multiplicativeExpr )?
                    	int alt31 = 2;
                    	int LA31_0 = input.LA(1);

                    	if ( (LA31_0 == MUL || (LA31_0 >= 55 && LA31_0 <= 56)) )
                    	{
                    	    alt31 = 1;
                    	}
                    	switch (alt31) 
                    	{
                    	    case 1 :
                    	        // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmXPath.g:185:23: ( '*' | 'div' | 'numMods' ) multiplicativeExpr
                    	        {
                    	        	set96=(IToken)input.LT(1);
                    	        	set96 = (IToken)input.LT(1);
                    	        	if ( input.LA(1) == MUL || (input.LA(1) >= 55 && input.LA(1) <= 56) ) 
                    	        	{
                    	        	    input.Consume();
                    	        	    root_0 = (object)adaptor.BecomeRoot((object)adaptor.Create(set96), root_0);
                    	        	    state.errorRecovery = false;
                    	        	}
                    	        	else 
                    	        	{
                    	        	    MismatchedSetException mse = new MismatchedSetException(null,input);
                    	        	    throw mse;
                    	        	}

                    	        	PushFollow(FOLLOW_multiplicativeExpr_in_multiplicativeExpr1186);
                    	        	multiplicativeExpr97 = multiplicativeExpr();
                    	        	state.followingStackPointer--;

                    	        	adaptor.AddChild(root_0, multiplicativeExpr97.Tree);

                    	        }
                    	        break;

                    	}


                    }
                    break;
                case 2 :
                    // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmXPath.g:186:6: '/' ( ( 'div' | 'numMods' ) multiplicativeExpr )?
                    {
                    	root_0 = (object)adaptor.GetNilNode();

                    	char_literal98=(IToken)Match(input,PATHSEP,FOLLOW_PATHSEP_in_multiplicativeExpr1195); 
                    		char_literal98_tree = (object)adaptor.Create(char_literal98);
                    		adaptor.AddChild(root_0, char_literal98_tree);

                    	// D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmXPath.g:186:10: ( ( 'div' | 'numMods' ) multiplicativeExpr )?
                    	int alt32 = 2;
                    	int LA32_0 = input.LA(1);

                    	if ( ((LA32_0 >= 55 && LA32_0 <= 56)) )
                    	{
                    	    alt32 = 1;
                    	}
                    	switch (alt32) 
                    	{
                    	    case 1 :
                    	        // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmXPath.g:186:11: ( 'div' | 'numMods' ) multiplicativeExpr
                    	        {
                    	        	set99=(IToken)input.LT(1);
                    	        	set99 = (IToken)input.LT(1);
                    	        	if ( (input.LA(1) >= 55 && input.LA(1) <= 56) ) 
                    	        	{
                    	        	    input.Consume();
                    	        	    root_0 = (object)adaptor.BecomeRoot((object)adaptor.Create(set99), root_0);
                    	        	    state.errorRecovery = false;
                    	        	}
                    	        	else 
                    	        	{
                    	        	    MismatchedSetException mse = new MismatchedSetException(null,input);
                    	        	    throw mse;
                    	        	}

                    	        	PushFollow(FOLLOW_multiplicativeExpr_in_multiplicativeExpr1205);
                    	        	multiplicativeExpr100 = multiplicativeExpr();
                    	        	state.followingStackPointer--;

                    	        	adaptor.AddChild(root_0, multiplicativeExpr100.Tree);

                    	        }
                    	        break;

                    	}


                    }
                    break;

            }
            retval.Stop = input.LT(-1);

            	retval.Tree = (object)adaptor.RulePostProcessing(root_0);
            	adaptor.SetTokenBoundaries(retval.Tree, (IToken) retval.Start, (IToken) retval.Stop);
        }
        catch (RecognitionException re) 
    	{
            ReportError(re);
            Recover(input,re);
    	// Conversion of the second argument necessary, but harmless
    	retval.Tree = (object)adaptor.ErrorNode(input, (IToken) retval.Start, input.LT(-1), re);

        }
        finally 
    	{
        }
        return retval;
    }
    // $ANTLR end "multiplicativeExpr"

    public class unaryExprNoRoot_return : ParserRuleReturnScope
    {
        private object tree;
        override public object Tree
        {
        	get { return tree; }
        	set { tree = (object) value; }
        }
    };

    // $ANTLR start "unaryExprNoRoot"
    // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmXPath.g:189:1: unaryExprNoRoot : ( '-' )* unionExprNoRoot ;
    public MvmXPathParser.unaryExprNoRoot_return unaryExprNoRoot() // throws RecognitionException [1]
    {   
        MvmXPathParser.unaryExprNoRoot_return retval = new MvmXPathParser.unaryExprNoRoot_return();
        retval.Start = input.LT(1);

        object root_0 = null;

        IToken char_literal101 = null;
        MvmXPathParser.unionExprNoRoot_return unionExprNoRoot102 = default(MvmXPathParser.unionExprNoRoot_return);


        object char_literal101_tree=null;

        try 
    	{
            // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmXPath.g:190:3: ( ( '-' )* unionExprNoRoot )
            // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmXPath.g:190:6: ( '-' )* unionExprNoRoot
            {
            	root_0 = (object)adaptor.GetNilNode();

            	// D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmXPath.g:190:6: ( '-' )*
            	do 
            	{
            	    int alt34 = 2;
            	    int LA34_0 = input.LA(1);

            	    if ( (LA34_0 == MINUS) )
            	    {
            	        alt34 = 1;
            	    }


            	    switch (alt34) 
            		{
            			case 1 :
            			    // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmXPath.g:190:6: '-'
            			    {
            			    	char_literal101=(IToken)Match(input,MINUS,FOLLOW_MINUS_in_unaryExprNoRoot1221); 
            			    		char_literal101_tree = (object)adaptor.Create(char_literal101);
            			    		adaptor.AddChild(root_0, char_literal101_tree);


            			    }
            			    break;

            			default:
            			    goto loop34;
            	    }
            	} while (true);

            	loop34:
            		;	// Stops C# compiler whining that label 'loop34' has no statements

            	PushFollow(FOLLOW_unionExprNoRoot_in_unaryExprNoRoot1224);
            	unionExprNoRoot102 = unionExprNoRoot();
            	state.followingStackPointer--;

            	adaptor.AddChild(root_0, unionExprNoRoot102.Tree);

            }

            retval.Stop = input.LT(-1);

            	retval.Tree = (object)adaptor.RulePostProcessing(root_0);
            	adaptor.SetTokenBoundaries(retval.Tree, (IToken) retval.Start, (IToken) retval.Stop);
        }
        catch (RecognitionException re) 
    	{
            ReportError(re);
            Recover(input,re);
    	// Conversion of the second argument necessary, but harmless
    	retval.Tree = (object)adaptor.ErrorNode(input, (IToken) retval.Start, input.LT(-1), re);

        }
        finally 
    	{
        }
        return retval;
    }
    // $ANTLR end "unaryExprNoRoot"

    public class qName_return : ParserRuleReturnScope
    {
        private object tree;
        override public object Tree
        {
        	get { return tree; }
        	set { tree = (object) value; }
        }
    };

    // $ANTLR start "qName"
    // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmXPath.g:193:1: qName : nCName ( ':' nCName )? ;
    public MvmXPathParser.qName_return qName() // throws RecognitionException [1]
    {   
        MvmXPathParser.qName_return retval = new MvmXPathParser.qName_return();
        retval.Start = input.LT(1);

        object root_0 = null;

        IToken char_literal104 = null;
        MvmXPathParser.nCName_return nCName103 = default(MvmXPathParser.nCName_return);

        MvmXPathParser.nCName_return nCName105 = default(MvmXPathParser.nCName_return);


        object char_literal104_tree=null;

        try 
    	{
            // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmXPath.g:193:8: ( nCName ( ':' nCName )? )
            // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmXPath.g:193:11: nCName ( ':' nCName )?
            {
            	root_0 = (object)adaptor.GetNilNode();

            	PushFollow(FOLLOW_nCName_in_qName1237);
            	nCName103 = nCName();
            	state.followingStackPointer--;

            	adaptor.AddChild(root_0, nCName103.Tree);
            	// D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmXPath.g:193:18: ( ':' nCName )?
            	int alt35 = 2;
            	int LA35_0 = input.LA(1);

            	if ( (LA35_0 == COLON) )
            	{
            	    alt35 = 1;
            	}
            	switch (alt35) 
            	{
            	    case 1 :
            	        // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmXPath.g:193:19: ':' nCName
            	        {
            	        	char_literal104=(IToken)Match(input,COLON,FOLLOW_COLON_in_qName1240); 
            	        		char_literal104_tree = (object)adaptor.Create(char_literal104);
            	        		adaptor.AddChild(root_0, char_literal104_tree);

            	        	PushFollow(FOLLOW_nCName_in_qName1242);
            	        	nCName105 = nCName();
            	        	state.followingStackPointer--;

            	        	adaptor.AddChild(root_0, nCName105.Tree);

            	        }
            	        break;

            	}


            }

            retval.Stop = input.LT(-1);

            	retval.Tree = (object)adaptor.RulePostProcessing(root_0);
            	adaptor.SetTokenBoundaries(retval.Tree, (IToken) retval.Start, (IToken) retval.Stop);
        }
        catch (RecognitionException re) 
    	{
            ReportError(re);
            Recover(input,re);
    	// Conversion of the second argument necessary, but harmless
    	retval.Tree = (object)adaptor.ErrorNode(input, (IToken) retval.Start, input.LT(-1), re);

        }
        finally 
    	{
        }
        return retval;
    }
    // $ANTLR end "qName"

    public class functionName_return : ParserRuleReturnScope
    {
        private object tree;
        override public object Tree
        {
        	get { return tree; }
        	set { tree = (object) value; }
        }
    };

    // $ANTLR start "functionName"
    // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmXPath.g:196:1: functionName : qName ;
    public MvmXPathParser.functionName_return functionName() // throws RecognitionException [1]
    {   
        MvmXPathParser.functionName_return retval = new MvmXPathParser.functionName_return();
        retval.Start = input.LT(1);

        object root_0 = null;

        MvmXPathParser.qName_return qName106 = default(MvmXPathParser.qName_return);



        try 
    	{
            // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmXPath.g:197:3: ( qName )
            // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmXPath.g:197:6: qName
            {
            	root_0 = (object)adaptor.GetNilNode();

            	PushFollow(FOLLOW_qName_in_functionName1258);
            	qName106 = qName();
            	state.followingStackPointer--;

            	adaptor.AddChild(root_0, qName106.Tree);

            }

            retval.Stop = input.LT(-1);

            	retval.Tree = (object)adaptor.RulePostProcessing(root_0);
            	adaptor.SetTokenBoundaries(retval.Tree, (IToken) retval.Start, (IToken) retval.Stop);
        }
        catch (RecognitionException re) 
    	{
            ReportError(re);
            Recover(input,re);
    	// Conversion of the second argument necessary, but harmless
    	retval.Tree = (object)adaptor.ErrorNode(input, (IToken) retval.Start, input.LT(-1), re);

        }
        finally 
    	{
        }
        return retval;
    }
    // $ANTLR end "functionName"

    public class variableReference_return : ParserRuleReturnScope
    {
        private object tree;
        override public object Tree
        {
        	get { return tree; }
        	set { tree = (object) value; }
        }
    };

    // $ANTLR start "variableReference"
    // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmXPath.g:200:1: variableReference : '$' qName ;
    public MvmXPathParser.variableReference_return variableReference() // throws RecognitionException [1]
    {   
        MvmXPathParser.variableReference_return retval = new MvmXPathParser.variableReference_return();
        retval.Start = input.LT(1);

        object root_0 = null;

        IToken char_literal107 = null;
        MvmXPathParser.qName_return qName108 = default(MvmXPathParser.qName_return);


        object char_literal107_tree=null;

        try 
    	{
            // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmXPath.g:201:3: ( '$' qName )
            // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmXPath.g:201:6: '$' qName
            {
            	root_0 = (object)adaptor.GetNilNode();

            	char_literal107=(IToken)Match(input,57,FOLLOW_57_in_variableReference1275); 
            		char_literal107_tree = (object)adaptor.Create(char_literal107);
            		adaptor.AddChild(root_0, char_literal107_tree);

            	PushFollow(FOLLOW_qName_in_variableReference1277);
            	qName108 = qName();
            	state.followingStackPointer--;

            	adaptor.AddChild(root_0, qName108.Tree);

            }

            retval.Stop = input.LT(-1);

            	retval.Tree = (object)adaptor.RulePostProcessing(root_0);
            	adaptor.SetTokenBoundaries(retval.Tree, (IToken) retval.Start, (IToken) retval.Stop);
        }
        catch (RecognitionException re) 
    	{
            ReportError(re);
            Recover(input,re);
    	// Conversion of the second argument necessary, but harmless
    	retval.Tree = (object)adaptor.ErrorNode(input, (IToken) retval.Start, input.LT(-1), re);

        }
        finally 
    	{
        }
        return retval;
    }
    // $ANTLR end "variableReference"

    public class nameTest_return : ParserRuleReturnScope
    {
        private object tree;
        override public object Tree
        {
        	get { return tree; }
        	set { tree = (object) value; }
        }
    };

    // $ANTLR start "nameTest"
    // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmXPath.g:204:1: nameTest : ( '*' | nCName ':' '*' | qName );
    public MvmXPathParser.nameTest_return nameTest() // throws RecognitionException [1]
    {   
        MvmXPathParser.nameTest_return retval = new MvmXPathParser.nameTest_return();
        retval.Start = input.LT(1);

        object root_0 = null;

        IToken char_literal109 = null;
        IToken char_literal111 = null;
        IToken char_literal112 = null;
        MvmXPathParser.nCName_return nCName110 = default(MvmXPathParser.nCName_return);

        MvmXPathParser.qName_return qName113 = default(MvmXPathParser.qName_return);


        object char_literal109_tree=null;
        object char_literal111_tree=null;
        object char_literal112_tree=null;

        try 
    	{
            // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmXPath.g:205:3: ( '*' | nCName ':' '*' | qName )
            int alt36 = 3;
            int LA36_0 = input.LA(1);

            if ( (LA36_0 == MUL) )
            {
                alt36 = 1;
            }
            else if ( (LA36_0 == AxisName || LA36_0 == NCName) )
            {
                int LA36_2 = input.LA(2);

                if ( (LA36_2 == COLON) )
                {
                    int LA36_3 = input.LA(3);

                    if ( (LA36_3 == MUL) )
                    {
                        alt36 = 2;
                    }
                    else if ( (LA36_3 == AxisName || LA36_3 == NCName) )
                    {
                        alt36 = 3;
                    }
                    else 
                    {
                        NoViableAltException nvae_d36s3 =
                            new NoViableAltException("", 36, 3, input);

                        throw nvae_d36s3;
                    }
                }
                else if ( (LA36_2 == EOF || (LA36_2 >= PATHSEP && LA36_2 <= ABRPATH) || (LA36_2 >= RPAR && LA36_2 <= PLUS) || LA36_2 == MUL || (LA36_2 >= COMMA && LA36_2 <= GE) || (LA36_2 >= 46 && LA36_2 <= 49) || (LA36_2 >= 51 && LA36_2 <= 56)) )
                {
                    alt36 = 3;
                }
                else 
                {
                    NoViableAltException nvae_d36s2 =
                        new NoViableAltException("", 36, 2, input);

                    throw nvae_d36s2;
                }
            }
            else 
            {
                NoViableAltException nvae_d36s0 =
                    new NoViableAltException("", 36, 0, input);

                throw nvae_d36s0;
            }
            switch (alt36) 
            {
                case 1 :
                    // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmXPath.g:205:6: '*'
                    {
                    	root_0 = (object)adaptor.GetNilNode();

                    	char_literal109=(IToken)Match(input,MUL,FOLLOW_MUL_in_nameTest1291); 
                    		char_literal109_tree = (object)adaptor.Create(char_literal109);
                    		adaptor.AddChild(root_0, char_literal109_tree);


                    }
                    break;
                case 2 :
                    // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmXPath.g:206:6: nCName ':' '*'
                    {
                    	root_0 = (object)adaptor.GetNilNode();

                    	PushFollow(FOLLOW_nCName_in_nameTest1298);
                    	nCName110 = nCName();
                    	state.followingStackPointer--;

                    	adaptor.AddChild(root_0, nCName110.Tree);
                    	char_literal111=(IToken)Match(input,COLON,FOLLOW_COLON_in_nameTest1300); 
                    		char_literal111_tree = (object)adaptor.Create(char_literal111);
                    		adaptor.AddChild(root_0, char_literal111_tree);

                    	char_literal112=(IToken)Match(input,MUL,FOLLOW_MUL_in_nameTest1302); 
                    		char_literal112_tree = (object)adaptor.Create(char_literal112);
                    		adaptor.AddChild(root_0, char_literal112_tree);


                    }
                    break;
                case 3 :
                    // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmXPath.g:207:6: qName
                    {
                    	root_0 = (object)adaptor.GetNilNode();

                    	PushFollow(FOLLOW_qName_in_nameTest1309);
                    	qName113 = qName();
                    	state.followingStackPointer--;

                    	adaptor.AddChild(root_0, qName113.Tree);

                    }
                    break;

            }
            retval.Stop = input.LT(-1);

            	retval.Tree = (object)adaptor.RulePostProcessing(root_0);
            	adaptor.SetTokenBoundaries(retval.Tree, (IToken) retval.Start, (IToken) retval.Stop);
        }
        catch (RecognitionException re) 
    	{
            ReportError(re);
            Recover(input,re);
    	// Conversion of the second argument necessary, but harmless
    	retval.Tree = (object)adaptor.ErrorNode(input, (IToken) retval.Start, input.LT(-1), re);

        }
        finally 
    	{
        }
        return retval;
    }
    // $ANTLR end "nameTest"

    public class nCName_return : ParserRuleReturnScope
    {
        private object tree;
        override public object Tree
        {
        	get { return tree; }
        	set { tree = (object) value; }
        }
    };

    // $ANTLR start "nCName"
    // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmXPath.g:210:1: nCName : ( NCName | AxisName );
    public MvmXPathParser.nCName_return nCName() // throws RecognitionException [1]
    {   
        MvmXPathParser.nCName_return retval = new MvmXPathParser.nCName_return();
        retval.Start = input.LT(1);

        object root_0 = null;

        IToken set114 = null;

        object set114_tree=null;

        try 
    	{
            // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmXPath.g:210:9: ( NCName | AxisName )
            // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmXPath.g:
            {
            	root_0 = (object)adaptor.GetNilNode();

            	set114 = (IToken)input.LT(1);
            	if ( input.LA(1) == AxisName || input.LA(1) == NCName ) 
            	{
            	    input.Consume();
            	    adaptor.AddChild(root_0, (object)adaptor.Create(set114));
            	    state.errorRecovery = false;
            	}
            	else 
            	{
            	    MismatchedSetException mse = new MismatchedSetException(null,input);
            	    throw mse;
            	}


            }

            retval.Stop = input.LT(-1);

            	retval.Tree = (object)adaptor.RulePostProcessing(root_0);
            	adaptor.SetTokenBoundaries(retval.Tree, (IToken) retval.Start, (IToken) retval.Stop);
        }
        catch (RecognitionException re) 
    	{
            ReportError(re);
            Recover(input,re);
    	// Conversion of the second argument necessary, but harmless
    	retval.Tree = (object)adaptor.ErrorNode(input, (IToken) retval.Start, input.LT(-1), re);

        }
        finally 
    	{
        }
        return retval;
    }
    // $ANTLR end "nCName"

    // Delegated rules


	private void InitializeCyclicDFAs()
	{
	}

 

    public static readonly BitSet FOLLOW_expr_in_main350 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_relativeLocationPath_in_locationPath372 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_absoluteLocationPathNoroot_in_locationPath379 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_46_in_absoluteLocationPathNoroot392 = new BitSet(new ulong[]{0x0003C00000018002UL});
    public static readonly BitSet FOLLOW_traverse_in_absoluteLocationPathNoroot395 = new BitSet(new ulong[]{0x0003C00000018002UL});
    public static readonly BitSet FOLLOW_47_in_absoluteLocationPathNoroot417 = new BitSet(new ulong[]{0x0003C00000018002UL});
    public static readonly BitSet FOLLOW_traverse_in_absoluteLocationPathNoroot420 = new BitSet(new ulong[]{0x0003C00000018002UL});
    public static readonly BitSet FOLLOW_48_in_absoluteLocationPathNoroot442 = new BitSet(new ulong[]{0x0004026001000000UL});
    public static readonly BitSet FOLLOW_nodeTest_in_absoluteLocationPathNoroot444 = new BitSet(new ulong[]{0x0003C00000018002UL});
    public static readonly BitSet FOLLOW_traverse_in_absoluteLocationPathNoroot447 = new BitSet(new ulong[]{0x0003C00000018002UL});
    public static readonly BitSet FOLLOW_PATHSEP_in_absoluteLocationPathNoroot471 = new BitSet(new ulong[]{0x0004026005000000UL});
    public static readonly BitSet FOLLOW_step_in_absoluteLocationPathNoroot474 = new BitSet(new ulong[]{0x0003C00000018002UL});
    public static readonly BitSet FOLLOW_traverse_in_absoluteLocationPathNoroot477 = new BitSet(new ulong[]{0x0003C00000018002UL});
    public static readonly BitSet FOLLOW_49_in_absoluteLocationPathNoroot501 = new BitSet(new ulong[]{0x0004026001000000UL});
    public static readonly BitSet FOLLOW_nodeTest_in_absoluteLocationPathNoroot503 = new BitSet(new ulong[]{0x0003C00000018002UL});
    public static readonly BitSet FOLLOW_traverse_in_absoluteLocationPathNoroot506 = new BitSet(new ulong[]{0x0003C00000018002UL});
    public static readonly BitSet FOLLOW_ABRPATH_in_absoluteLocationPathNoroot530 = new BitSet(new ulong[]{0x0004026005000000UL});
    public static readonly BitSet FOLLOW_step_in_absoluteLocationPathNoroot533 = new BitSet(new ulong[]{0x0003C00000018002UL});
    public static readonly BitSet FOLLOW_traverse_in_absoluteLocationPathNoroot536 = new BitSet(new ulong[]{0x0003C00000018002UL});
    public static readonly BitSet FOLLOW_DOTDOT_in_relativeLocationPath567 = new BitSet(new ulong[]{0x0003C00000018002UL});
    public static readonly BitSet FOLLOW_traverse_in_relativeLocationPath570 = new BitSet(new ulong[]{0x0003C00000018002UL});
    public static readonly BitSet FOLLOW_DOT_in_relativeLocationPath592 = new BitSet(new ulong[]{0x0003C00000018002UL});
    public static readonly BitSet FOLLOW_traverse_in_relativeLocationPath595 = new BitSet(new ulong[]{0x0003C00000018002UL});
    public static readonly BitSet FOLLOW_step_in_relativeLocationPath618 = new BitSet(new ulong[]{0x0003C00000018002UL});
    public static readonly BitSet FOLLOW_traverse_in_relativeLocationPath621 = new BitSet(new ulong[]{0x0003C00000018002UL});
    public static readonly BitSet FOLLOW_46_in_traverse653 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_47_in_traverse664 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_48_in_traverse675 = new BitSet(new ulong[]{0x0004026001000000UL});
    public static readonly BitSet FOLLOW_nodeTest_in_traverse677 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_PATHSEP_in_traverse689 = new BitSet(new ulong[]{0x0004026005000000UL});
    public static readonly BitSet FOLLOW_step_in_traverse691 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_49_in_traverse704 = new BitSet(new ulong[]{0x0004026001000000UL});
    public static readonly BitSet FOLLOW_nodeTest_in_traverse706 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_ABRPATH_in_traverse718 = new BitSet(new ulong[]{0x0004026005000000UL});
    public static readonly BitSet FOLLOW_step_in_traverse720 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_axisSpecifier_in_step742 = new BitSet(new ulong[]{0x0004026001000000UL});
    public static readonly BitSet FOLLOW_nodeTest_in_step744 = new BitSet(new ulong[]{0x0000000000080002UL});
    public static readonly BitSet FOLLOW_predicate_in_step746 = new BitSet(new ulong[]{0x0000000000080002UL});
    public static readonly BitSet FOLLOW_AxisName_in_axisSpecifier764 = new BitSet(new ulong[]{0x0000000400000000UL});
    public static readonly BitSet FOLLOW_CC_in_axisSpecifier766 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_AT_in_axisSpecifier773 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_nameTest_in_nodeTest788 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_NodeType_in_nodeTest795 = new BitSet(new ulong[]{0x0000000000020000UL});
    public static readonly BitSet FOLLOW_LPAR_in_nodeTest797 = new BitSet(new ulong[]{0x0000000000040000UL});
    public static readonly BitSet FOLLOW_RPAR_in_nodeTest799 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_50_in_nodeTest806 = new BitSet(new ulong[]{0x0000000000020000UL});
    public static readonly BitSet FOLLOW_LPAR_in_nodeTest808 = new BitSet(new ulong[]{0x0000008000000000UL});
    public static readonly BitSet FOLLOW_Literal_in_nodeTest810 = new BitSet(new ulong[]{0x0000000000040000UL});
    public static readonly BitSet FOLLOW_RPAR_in_nodeTest812 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_LBRAC_in_predicate826 = new BitSet(new ulong[]{0x0207C3E007A38000UL});
    public static readonly BitSet FOLLOW_expr_in_predicate828 = new BitSet(new ulong[]{0x0000000000100000UL});
    public static readonly BitSet FOLLOW_RBRAC_in_predicate830 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_set_in_abbreviatedStep0 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_orExpr_in_expr871 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_variableReference_in_primaryExpr885 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_LPAR_in_primaryExpr892 = new BitSet(new ulong[]{0x0207C3E007A38000UL});
    public static readonly BitSet FOLLOW_expr_in_primaryExpr894 = new BitSet(new ulong[]{0x0000000000040000UL});
    public static readonly BitSet FOLLOW_RPAR_in_primaryExpr896 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_Literal_in_primaryExpr903 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_Number_in_primaryExpr910 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_functionCall_in_primaryExpr919 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_functionName_in_functionCall933 = new BitSet(new ulong[]{0x0000000000020000UL});
    public static readonly BitSet FOLLOW_LPAR_in_functionCall935 = new BitSet(new ulong[]{0x0207C3E007A78000UL});
    public static readonly BitSet FOLLOW_expr_in_functionCall939 = new BitSet(new ulong[]{0x0000000008040000UL});
    public static readonly BitSet FOLLOW_COMMA_in_functionCall943 = new BitSet(new ulong[]{0x0207C3E007A38000UL});
    public static readonly BitSet FOLLOW_expr_in_functionCall945 = new BitSet(new ulong[]{0x0000000008040000UL});
    public static readonly BitSet FOLLOW_RPAR_in_functionCall953 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_pathExprNoRoot_in_unionExprNoRoot967 = new BitSet(new ulong[]{0x0000000010000002UL});
    public static readonly BitSet FOLLOW_PIPE_in_unionExprNoRoot970 = new BitSet(new ulong[]{0x0207C3E007A38000UL});
    public static readonly BitSet FOLLOW_unionExprNoRoot_in_unionExprNoRoot973 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_PATHSEP_in_unionExprNoRoot982 = new BitSet(new ulong[]{0x0000000010000000UL});
    public static readonly BitSet FOLLOW_PIPE_in_unionExprNoRoot984 = new BitSet(new ulong[]{0x0207C3E007A38000UL});
    public static readonly BitSet FOLLOW_unionExprNoRoot_in_unionExprNoRoot987 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_locationPath_in_pathExprNoRoot1001 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_filterExpr_in_pathExprNoRoot1008 = new BitSet(new ulong[]{0x0000000000018002UL});
    public static readonly BitSet FOLLOW_set_in_pathExprNoRoot1011 = new BitSet(new ulong[]{0x0004026007800000UL});
    public static readonly BitSet FOLLOW_relativeLocationPath_in_pathExprNoRoot1017 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_primaryExpr_in_filterExpr1033 = new BitSet(new ulong[]{0x0000000000080002UL});
    public static readonly BitSet FOLLOW_predicate_in_filterExpr1035 = new BitSet(new ulong[]{0x0000000000080002UL});
    public static readonly BitSet FOLLOW_andExpr_in_orExpr1049 = new BitSet(new ulong[]{0x0008000000000002UL});
    public static readonly BitSet FOLLOW_51_in_orExpr1052 = new BitSet(new ulong[]{0x0207C3E007A38000UL});
    public static readonly BitSet FOLLOW_andExpr_in_orExpr1055 = new BitSet(new ulong[]{0x0008000000000002UL});
    public static readonly BitSet FOLLOW_equalityExpr_in_andExpr1070 = new BitSet(new ulong[]{0x0010000000000002UL});
    public static readonly BitSet FOLLOW_52_in_andExpr1073 = new BitSet(new ulong[]{0x0207C3E007A38000UL});
    public static readonly BitSet FOLLOW_equalityExpr_in_andExpr1076 = new BitSet(new ulong[]{0x0010000000000002UL});
    public static readonly BitSet FOLLOW_relationalExpr_in_equalityExpr1092 = new BitSet(new ulong[]{0x0060000000000002UL});
    public static readonly BitSet FOLLOW_set_in_equalityExpr1095 = new BitSet(new ulong[]{0x0207C3E007A38000UL});
    public static readonly BitSet FOLLOW_relationalExpr_in_equalityExpr1102 = new BitSet(new ulong[]{0x0060000000000002UL});
    public static readonly BitSet FOLLOW_additiveExpr_in_relationalExpr1118 = new BitSet(new ulong[]{0x00000001E0000002UL});
    public static readonly BitSet FOLLOW_set_in_relationalExpr1121 = new BitSet(new ulong[]{0x0207C3E007A38000UL});
    public static readonly BitSet FOLLOW_additiveExpr_in_relationalExpr1132 = new BitSet(new ulong[]{0x00000001E0000002UL});
    public static readonly BitSet FOLLOW_multiplicativeExpr_in_additiveExpr1148 = new BitSet(new ulong[]{0x0000000000600002UL});
    public static readonly BitSet FOLLOW_set_in_additiveExpr1151 = new BitSet(new ulong[]{0x0207C3E007A38000UL});
    public static readonly BitSet FOLLOW_multiplicativeExpr_in_additiveExpr1158 = new BitSet(new ulong[]{0x0000000000600002UL});
    public static readonly BitSet FOLLOW_unaryExprNoRoot_in_multiplicativeExpr1174 = new BitSet(new ulong[]{0x0180000001000002UL});
    public static readonly BitSet FOLLOW_set_in_multiplicativeExpr1177 = new BitSet(new ulong[]{0x0207C3E007A38000UL});
    public static readonly BitSet FOLLOW_multiplicativeExpr_in_multiplicativeExpr1186 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_PATHSEP_in_multiplicativeExpr1195 = new BitSet(new ulong[]{0x0180000000000002UL});
    public static readonly BitSet FOLLOW_set_in_multiplicativeExpr1198 = new BitSet(new ulong[]{0x0207C3E007A38000UL});
    public static readonly BitSet FOLLOW_multiplicativeExpr_in_multiplicativeExpr1205 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_MINUS_in_unaryExprNoRoot1221 = new BitSet(new ulong[]{0x0207C3E007A38000UL});
    public static readonly BitSet FOLLOW_unionExprNoRoot_in_unaryExprNoRoot1224 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_nCName_in_qName1237 = new BitSet(new ulong[]{0x0000000200000002UL});
    public static readonly BitSet FOLLOW_COLON_in_qName1240 = new BitSet(new ulong[]{0x0000022000000000UL});
    public static readonly BitSet FOLLOW_nCName_in_qName1242 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_qName_in_functionName1258 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_57_in_variableReference1275 = new BitSet(new ulong[]{0x0000022001000000UL});
    public static readonly BitSet FOLLOW_qName_in_variableReference1277 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_MUL_in_nameTest1291 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_nCName_in_nameTest1298 = new BitSet(new ulong[]{0x0000000200000000UL});
    public static readonly BitSet FOLLOW_COLON_in_nameTest1300 = new BitSet(new ulong[]{0x0000000001000000UL});
    public static readonly BitSet FOLLOW_MUL_in_nameTest1302 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_qName_in_nameTest1309 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_set_in_nCName0 = new BitSet(new ulong[]{0x0000000000000002UL});

}
