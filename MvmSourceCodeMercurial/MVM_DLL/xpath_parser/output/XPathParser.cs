// $ANTLR 3.2 Sep 23, 2009 12:02:23 D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\xpath_parser\\XPath.g 2010-08-27 10:29:54

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

public partial class XPathParser : Parser
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
		"'processing-instruction'", 
		"'or'", 
		"'and'", 
		"'='", 
		"'!='", 
		"'div'", 
		"'mod'", 
		"'$'"
    };

    public const int NCName = 39;
    public const int APOS = 33;
    public const int ABSOLUTE_PATH = 5;
    public const int PATHSEP = 13;
    public const int DOTDOT = 23;
    public const int EOF = -1;
    public const int PREDICATE = 8;
    public const int TRAVERSE_UP = 10;
    public const int AT = 24;
    public const int Literal = 37;
    public const int T__51 = 51;
    public const int T__52 = 52;
    public const int Number = 38;
    public const int T__53 = 53;
    public const int LPAR = 15;
    public const int RECURSIVE_MATCH = 9;
    public const int Digits = 40;
    public const int COMMA = 25;
    public const int RBRAC = 18;
    public const int LESS = 27;
    public const int AxisName = 35;
    public const int PLUS = 20;
    public const int PIPE = 26;
    public const int NodeType = 36;
    public const int DOT = 21;
    public const int Whitespace = 41;
    public const int T__50 = 50;
    public const int NCNameStartChar = 42;
    public const int GE = 30;
    public const int T__46 = 46;
    public const int NCNameChar = 43;
    public const int T__47 = 47;
    public const int T__44 = 44;
    public const int T__45 = 45;
    public const int T__48 = 48;
    public const int T__49 = 49;
    public const int QUOT = 34;
    public const int MATCH = 12;
    public const int ABRPATH = 14;
    public const int MINUS = 19;
    public const int MUL = 22;
    public const int LBRAC = 17;
    public const int COLON = 31;
    public const int XPATH = 4;
    public const int MORE = 28;
    public const int CURRENT_NODE = 11;
    public const int ROOT_PATH = 7;
    public const int RELATIVE_PATH = 6;
    public const int RPAR = 16;
    public const int CC = 32;
    public const int LE = 29;

    // delegates
    // delegators



        public XPathParser(ITokenStream input)
    		: this(input, new RecognizerSharedState()) {
        }

        public XPathParser(ITokenStream input, RecognizerSharedState state)
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
		get { return XPathParser.tokenNames; }
    }

    override public string GrammarFileName {
		get { return "D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\xpath_parser\\XPath.g"; }
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
    // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\xpath_parser\\XPath.g:75:1: main : expr -> ^( XPATH expr ) ;
    public XPathParser.main_return main() // throws RecognitionException [1]
    {   
        XPathParser.main_return retval = new XPathParser.main_return();
        retval.Start = input.LT(1);

        object root_0 = null;

        XPathParser.expr_return expr1 = default(XPathParser.expr_return);


        RewriteRuleSubtreeStream stream_expr = new RewriteRuleSubtreeStream(adaptor,"rule expr");
        try 
    	{
            // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\xpath_parser\\XPath.g:75:7: ( expr -> ^( XPATH expr ) )
            // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\xpath_parser\\XPath.g:75:10: expr
            {
            	PushFollow(FOLLOW_expr_in_main340);
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
            	// 75:15: -> ^( XPATH expr )
            	{
            	    // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\xpath_parser\\XPath.g:75:18: ^( XPATH expr )
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
    // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\xpath_parser\\XPath.g:78:1: locationPath : ( relativeLocationPath | absoluteLocationPathNoroot );
    public XPathParser.locationPath_return locationPath() // throws RecognitionException [1]
    {   
        XPathParser.locationPath_return retval = new XPathParser.locationPath_return();
        retval.Start = input.LT(1);

        object root_0 = null;

        XPathParser.relativeLocationPath_return relativeLocationPath2 = default(XPathParser.relativeLocationPath_return);

        XPathParser.absoluteLocationPathNoroot_return absoluteLocationPathNoroot3 = default(XPathParser.absoluteLocationPathNoroot_return);



        try 
    	{
            // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\xpath_parser\\XPath.g:79:3: ( relativeLocationPath | absoluteLocationPathNoroot )
            int alt1 = 2;
            int LA1_0 = input.LA(1);

            if ( ((LA1_0 >= DOT && LA1_0 <= AT) || (LA1_0 >= AxisName && LA1_0 <= NodeType) || LA1_0 == NCName || LA1_0 == 46) )
            {
                alt1 = 1;
            }
            else if ( ((LA1_0 >= PATHSEP && LA1_0 <= ABRPATH) || (LA1_0 >= 44 && LA1_0 <= 45)) )
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
                    // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\xpath_parser\\XPath.g:79:6: relativeLocationPath
                    {
                    	root_0 = (object)adaptor.GetNilNode();

                    	PushFollow(FOLLOW_relativeLocationPath_in_locationPath362);
                    	relativeLocationPath2 = relativeLocationPath();
                    	state.followingStackPointer--;

                    	adaptor.AddChild(root_0, relativeLocationPath2.Tree);

                    }
                    break;
                case 2 :
                    // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\xpath_parser\\XPath.g:80:6: absoluteLocationPathNoroot
                    {
                    	root_0 = (object)adaptor.GetNilNode();

                    	PushFollow(FOLLOW_absoluteLocationPathNoroot_in_locationPath369);
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
    // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\xpath_parser\\XPath.g:83:1: absoluteLocationPathNoroot : ( '/..' ( traverse )* -> ^( ABSOLUTE_PATH ^( TRAVERSE_UP ) ( traverse )* ) | '/.' ( traverse )* -> ^( ABSOLUTE_PATH ^( CURRENT_NODE ) ( traverse )* ) | '/' step ( traverse )* -> ^( ABSOLUTE_PATH ^( MATCH step ) ( traverse )* ) | '//' step ( traverse )* -> ^( ABSOLUTE_PATH ^( RECURSIVE_MATCH step ) ( traverse )* ) );
    public XPathParser.absoluteLocationPathNoroot_return absoluteLocationPathNoroot() // throws RecognitionException [1]
    {   
        XPathParser.absoluteLocationPathNoroot_return retval = new XPathParser.absoluteLocationPathNoroot_return();
        retval.Start = input.LT(1);

        object root_0 = null;

        IToken string_literal4 = null;
        IToken string_literal6 = null;
        IToken char_literal8 = null;
        IToken string_literal11 = null;
        XPathParser.traverse_return traverse5 = default(XPathParser.traverse_return);

        XPathParser.traverse_return traverse7 = default(XPathParser.traverse_return);

        XPathParser.step_return step9 = default(XPathParser.step_return);

        XPathParser.traverse_return traverse10 = default(XPathParser.traverse_return);

        XPathParser.step_return step12 = default(XPathParser.step_return);

        XPathParser.traverse_return traverse13 = default(XPathParser.traverse_return);


        object string_literal4_tree=null;
        object string_literal6_tree=null;
        object char_literal8_tree=null;
        object string_literal11_tree=null;
        RewriteRuleTokenStream stream_ABRPATH = new RewriteRuleTokenStream(adaptor,"token ABRPATH");
        RewriteRuleTokenStream stream_45 = new RewriteRuleTokenStream(adaptor,"token 45");
        RewriteRuleTokenStream stream_44 = new RewriteRuleTokenStream(adaptor,"token 44");
        RewriteRuleTokenStream stream_PATHSEP = new RewriteRuleTokenStream(adaptor,"token PATHSEP");
        RewriteRuleSubtreeStream stream_traverse = new RewriteRuleSubtreeStream(adaptor,"rule traverse");
        RewriteRuleSubtreeStream stream_step = new RewriteRuleSubtreeStream(adaptor,"rule step");
        try 
    	{
            // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\xpath_parser\\XPath.g:84:3: ( '/..' ( traverse )* -> ^( ABSOLUTE_PATH ^( TRAVERSE_UP ) ( traverse )* ) | '/.' ( traverse )* -> ^( ABSOLUTE_PATH ^( CURRENT_NODE ) ( traverse )* ) | '/' step ( traverse )* -> ^( ABSOLUTE_PATH ^( MATCH step ) ( traverse )* ) | '//' step ( traverse )* -> ^( ABSOLUTE_PATH ^( RECURSIVE_MATCH step ) ( traverse )* ) )
            int alt6 = 4;
            switch ( input.LA(1) ) 
            {
            case 44:
            	{
                alt6 = 1;
                }
                break;
            case 45:
            	{
                alt6 = 2;
                }
                break;
            case PATHSEP:
            	{
                alt6 = 3;
                }
                break;
            case ABRPATH:
            	{
                alt6 = 4;
                }
                break;
            	default:
            	    NoViableAltException nvae_d6s0 =
            	        new NoViableAltException("", 6, 0, input);

            	    throw nvae_d6s0;
            }

            switch (alt6) 
            {
                case 1 :
                    // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\xpath_parser\\XPath.g:84:5: '/..' ( traverse )*
                    {
                    	string_literal4=(IToken)Match(input,44,FOLLOW_44_in_absoluteLocationPathNoroot382);  
                    	stream_44.Add(string_literal4);

                    	// D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\xpath_parser\\XPath.g:84:11: ( traverse )*
                    	do 
                    	{
                    	    int alt2 = 2;
                    	    int LA2_0 = input.LA(1);

                    	    if ( ((LA2_0 >= PATHSEP && LA2_0 <= ABRPATH) || (LA2_0 >= 44 && LA2_0 <= 45)) )
                    	    {
                    	        alt2 = 1;
                    	    }


                    	    switch (alt2) 
                    		{
                    			case 1 :
                    			    // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\xpath_parser\\XPath.g:84:12: traverse
                    			    {
                    			    	PushFollow(FOLLOW_traverse_in_absoluteLocationPathNoroot385);
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
                    	// 84:23: -> ^( ABSOLUTE_PATH ^( TRAVERSE_UP ) ( traverse )* )
                    	{
                    	    // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\xpath_parser\\XPath.g:84:25: ^( ABSOLUTE_PATH ^( TRAVERSE_UP ) ( traverse )* )
                    	    {
                    	    object root_1 = (object)adaptor.GetNilNode();
                    	    root_1 = (object)adaptor.BecomeRoot((object)adaptor.Create(ABSOLUTE_PATH, "ABSOLUTE_PATH"), root_1);

                    	    // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\xpath_parser\\XPath.g:84:41: ^( TRAVERSE_UP )
                    	    {
                    	    object root_2 = (object)adaptor.GetNilNode();
                    	    root_2 = (object)adaptor.BecomeRoot((object)adaptor.Create(TRAVERSE_UP, "TRAVERSE_UP"), root_2);

                    	    adaptor.AddChild(root_1, root_2);
                    	    }
                    	    // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\xpath_parser\\XPath.g:84:56: ( traverse )*
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
                    // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\xpath_parser\\XPath.g:85:5: '/.' ( traverse )*
                    {
                    	string_literal6=(IToken)Match(input,45,FOLLOW_45_in_absoluteLocationPathNoroot407);  
                    	stream_45.Add(string_literal6);

                    	// D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\xpath_parser\\XPath.g:85:10: ( traverse )*
                    	do 
                    	{
                    	    int alt3 = 2;
                    	    int LA3_0 = input.LA(1);

                    	    if ( ((LA3_0 >= PATHSEP && LA3_0 <= ABRPATH) || (LA3_0 >= 44 && LA3_0 <= 45)) )
                    	    {
                    	        alt3 = 1;
                    	    }


                    	    switch (alt3) 
                    		{
                    			case 1 :
                    			    // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\xpath_parser\\XPath.g:85:11: traverse
                    			    {
                    			    	PushFollow(FOLLOW_traverse_in_absoluteLocationPathNoroot410);
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
                    	// 85:22: -> ^( ABSOLUTE_PATH ^( CURRENT_NODE ) ( traverse )* )
                    	{
                    	    // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\xpath_parser\\XPath.g:85:24: ^( ABSOLUTE_PATH ^( CURRENT_NODE ) ( traverse )* )
                    	    {
                    	    object root_1 = (object)adaptor.GetNilNode();
                    	    root_1 = (object)adaptor.BecomeRoot((object)adaptor.Create(ABSOLUTE_PATH, "ABSOLUTE_PATH"), root_1);

                    	    // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\xpath_parser\\XPath.g:85:40: ^( CURRENT_NODE )
                    	    {
                    	    object root_2 = (object)adaptor.GetNilNode();
                    	    root_2 = (object)adaptor.BecomeRoot((object)adaptor.Create(CURRENT_NODE, "CURRENT_NODE"), root_2);

                    	    adaptor.AddChild(root_1, root_2);
                    	    }
                    	    // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\xpath_parser\\XPath.g:85:56: ( traverse )*
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
                    // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\xpath_parser\\XPath.g:87:5: '/' step ( traverse )*
                    {
                    	char_literal8=(IToken)Match(input,PATHSEP,FOLLOW_PATHSEP_in_absoluteLocationPathNoroot435);  
                    	stream_PATHSEP.Add(char_literal8);

                    	PushFollow(FOLLOW_step_in_absoluteLocationPathNoroot438);
                    	step9 = step();
                    	state.followingStackPointer--;

                    	stream_step.Add(step9.Tree);
                    	// D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\xpath_parser\\XPath.g:87:15: ( traverse )*
                    	do 
                    	{
                    	    int alt4 = 2;
                    	    int LA4_0 = input.LA(1);

                    	    if ( ((LA4_0 >= PATHSEP && LA4_0 <= ABRPATH) || (LA4_0 >= 44 && LA4_0 <= 45)) )
                    	    {
                    	        alt4 = 1;
                    	    }


                    	    switch (alt4) 
                    		{
                    			case 1 :
                    			    // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\xpath_parser\\XPath.g:87:16: traverse
                    			    {
                    			    	PushFollow(FOLLOW_traverse_in_absoluteLocationPathNoroot441);
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
                    	// elements:          step, traverse
                    	// token labels:      
                    	// rule labels:       retval
                    	// token list labels: 
                    	// rule list labels:  
                    	// wildcard labels: 
                    	retval.Tree = root_0;
                    	RewriteRuleSubtreeStream stream_retval = new RewriteRuleSubtreeStream(adaptor, "rule retval", retval!=null ? retval.Tree : null);

                    	root_0 = (object)adaptor.GetNilNode();
                    	// 87:27: -> ^( ABSOLUTE_PATH ^( MATCH step ) ( traverse )* )
                    	{
                    	    // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\xpath_parser\\XPath.g:87:29: ^( ABSOLUTE_PATH ^( MATCH step ) ( traverse )* )
                    	    {
                    	    object root_1 = (object)adaptor.GetNilNode();
                    	    root_1 = (object)adaptor.BecomeRoot((object)adaptor.Create(ABSOLUTE_PATH, "ABSOLUTE_PATH"), root_1);

                    	    // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\xpath_parser\\XPath.g:87:45: ^( MATCH step )
                    	    {
                    	    object root_2 = (object)adaptor.GetNilNode();
                    	    root_2 = (object)adaptor.BecomeRoot((object)adaptor.Create(MATCH, "MATCH"), root_2);

                    	    adaptor.AddChild(root_2, stream_step.NextTree());

                    	    adaptor.AddChild(root_1, root_2);
                    	    }
                    	    // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\xpath_parser\\XPath.g:87:59: ( traverse )*
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
                    // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\xpath_parser\\XPath.g:89:5: '//' step ( traverse )*
                    {
                    	string_literal11=(IToken)Match(input,ABRPATH,FOLLOW_ABRPATH_in_absoluteLocationPathNoroot468);  
                    	stream_ABRPATH.Add(string_literal11);

                    	PushFollow(FOLLOW_step_in_absoluteLocationPathNoroot471);
                    	step12 = step();
                    	state.followingStackPointer--;

                    	stream_step.Add(step12.Tree);
                    	// D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\xpath_parser\\XPath.g:89:16: ( traverse )*
                    	do 
                    	{
                    	    int alt5 = 2;
                    	    int LA5_0 = input.LA(1);

                    	    if ( ((LA5_0 >= PATHSEP && LA5_0 <= ABRPATH) || (LA5_0 >= 44 && LA5_0 <= 45)) )
                    	    {
                    	        alt5 = 1;
                    	    }


                    	    switch (alt5) 
                    		{
                    			case 1 :
                    			    // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\xpath_parser\\XPath.g:89:17: traverse
                    			    {
                    			    	PushFollow(FOLLOW_traverse_in_absoluteLocationPathNoroot474);
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
                    	// elements:          traverse, step
                    	// token labels:      
                    	// rule labels:       retval
                    	// token list labels: 
                    	// rule list labels:  
                    	// wildcard labels: 
                    	retval.Tree = root_0;
                    	RewriteRuleSubtreeStream stream_retval = new RewriteRuleSubtreeStream(adaptor, "rule retval", retval!=null ? retval.Tree : null);

                    	root_0 = (object)adaptor.GetNilNode();
                    	// 89:28: -> ^( ABSOLUTE_PATH ^( RECURSIVE_MATCH step ) ( traverse )* )
                    	{
                    	    // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\xpath_parser\\XPath.g:89:30: ^( ABSOLUTE_PATH ^( RECURSIVE_MATCH step ) ( traverse )* )
                    	    {
                    	    object root_1 = (object)adaptor.GetNilNode();
                    	    root_1 = (object)adaptor.BecomeRoot((object)adaptor.Create(ABSOLUTE_PATH, "ABSOLUTE_PATH"), root_1);

                    	    // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\xpath_parser\\XPath.g:89:46: ^( RECURSIVE_MATCH step )
                    	    {
                    	    object root_2 = (object)adaptor.GetNilNode();
                    	    root_2 = (object)adaptor.BecomeRoot((object)adaptor.Create(RECURSIVE_MATCH, "RECURSIVE_MATCH"), root_2);

                    	    adaptor.AddChild(root_2, stream_step.NextTree());

                    	    adaptor.AddChild(root_1, root_2);
                    	    }
                    	    // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\xpath_parser\\XPath.g:89:70: ( traverse )*
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
    // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\xpath_parser\\XPath.g:92:1: relativeLocationPath : ( '..' ( traverse )* -> ^( RELATIVE_PATH ^( TRAVERSE_UP ) ( traverse )* ) | '.' ( traverse )* -> ^( RELATIVE_PATH ^( CURRENT_NODE ) ( traverse )* ) | step ( traverse )* -> ^( RELATIVE_PATH ^( MATCH step ) ( traverse )* ) );
    public XPathParser.relativeLocationPath_return relativeLocationPath() // throws RecognitionException [1]
    {   
        XPathParser.relativeLocationPath_return retval = new XPathParser.relativeLocationPath_return();
        retval.Start = input.LT(1);

        object root_0 = null;

        IToken string_literal14 = null;
        IToken char_literal16 = null;
        XPathParser.traverse_return traverse15 = default(XPathParser.traverse_return);

        XPathParser.traverse_return traverse17 = default(XPathParser.traverse_return);

        XPathParser.step_return step18 = default(XPathParser.step_return);

        XPathParser.traverse_return traverse19 = default(XPathParser.traverse_return);


        object string_literal14_tree=null;
        object char_literal16_tree=null;
        RewriteRuleTokenStream stream_DOTDOT = new RewriteRuleTokenStream(adaptor,"token DOTDOT");
        RewriteRuleTokenStream stream_DOT = new RewriteRuleTokenStream(adaptor,"token DOT");
        RewriteRuleSubtreeStream stream_traverse = new RewriteRuleSubtreeStream(adaptor,"rule traverse");
        RewriteRuleSubtreeStream stream_step = new RewriteRuleSubtreeStream(adaptor,"rule step");
        try 
    	{
            // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\xpath_parser\\XPath.g:93:3: ( '..' ( traverse )* -> ^( RELATIVE_PATH ^( TRAVERSE_UP ) ( traverse )* ) | '.' ( traverse )* -> ^( RELATIVE_PATH ^( CURRENT_NODE ) ( traverse )* ) | step ( traverse )* -> ^( RELATIVE_PATH ^( MATCH step ) ( traverse )* ) )
            int alt10 = 3;
            switch ( input.LA(1) ) 
            {
            case DOTDOT:
            	{
                alt10 = 1;
                }
                break;
            case DOT:
            	{
                alt10 = 2;
                }
                break;
            case MUL:
            case AT:
            case AxisName:
            case NodeType:
            case NCName:
            case 46:
            	{
                alt10 = 3;
                }
                break;
            	default:
            	    NoViableAltException nvae_d10s0 =
            	        new NoViableAltException("", 10, 0, input);

            	    throw nvae_d10s0;
            }

            switch (alt10) 
            {
                case 1 :
                    // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\xpath_parser\\XPath.g:93:5: '..' ( traverse )*
                    {
                    	string_literal14=(IToken)Match(input,DOTDOT,FOLLOW_DOTDOT_in_relativeLocationPath505);  
                    	stream_DOTDOT.Add(string_literal14);

                    	// D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\xpath_parser\\XPath.g:93:10: ( traverse )*
                    	do 
                    	{
                    	    int alt7 = 2;
                    	    int LA7_0 = input.LA(1);

                    	    if ( ((LA7_0 >= PATHSEP && LA7_0 <= ABRPATH) || (LA7_0 >= 44 && LA7_0 <= 45)) )
                    	    {
                    	        alt7 = 1;
                    	    }


                    	    switch (alt7) 
                    		{
                    			case 1 :
                    			    // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\xpath_parser\\XPath.g:93:11: traverse
                    			    {
                    			    	PushFollow(FOLLOW_traverse_in_relativeLocationPath508);
                    			    	traverse15 = traverse();
                    			    	state.followingStackPointer--;

                    			    	stream_traverse.Add(traverse15.Tree);

                    			    }
                    			    break;

                    			default:
                    			    goto loop7;
                    	    }
                    	} while (true);

                    	loop7:
                    		;	// Stops C# compiler whining that label 'loop7' has no statements



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
                    	// 93:22: -> ^( RELATIVE_PATH ^( TRAVERSE_UP ) ( traverse )* )
                    	{
                    	    // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\xpath_parser\\XPath.g:93:24: ^( RELATIVE_PATH ^( TRAVERSE_UP ) ( traverse )* )
                    	    {
                    	    object root_1 = (object)adaptor.GetNilNode();
                    	    root_1 = (object)adaptor.BecomeRoot((object)adaptor.Create(RELATIVE_PATH, "RELATIVE_PATH"), root_1);

                    	    // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\xpath_parser\\XPath.g:93:40: ^( TRAVERSE_UP )
                    	    {
                    	    object root_2 = (object)adaptor.GetNilNode();
                    	    root_2 = (object)adaptor.BecomeRoot((object)adaptor.Create(TRAVERSE_UP, "TRAVERSE_UP"), root_2);

                    	    adaptor.AddChild(root_1, root_2);
                    	    }
                    	    // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\xpath_parser\\XPath.g:93:55: ( traverse )*
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
                    // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\xpath_parser\\XPath.g:94:5: '.' ( traverse )*
                    {
                    	char_literal16=(IToken)Match(input,DOT,FOLLOW_DOT_in_relativeLocationPath530);  
                    	stream_DOT.Add(char_literal16);

                    	// D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\xpath_parser\\XPath.g:94:9: ( traverse )*
                    	do 
                    	{
                    	    int alt8 = 2;
                    	    int LA8_0 = input.LA(1);

                    	    if ( ((LA8_0 >= PATHSEP && LA8_0 <= ABRPATH) || (LA8_0 >= 44 && LA8_0 <= 45)) )
                    	    {
                    	        alt8 = 1;
                    	    }


                    	    switch (alt8) 
                    		{
                    			case 1 :
                    			    // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\xpath_parser\\XPath.g:94:10: traverse
                    			    {
                    			    	PushFollow(FOLLOW_traverse_in_relativeLocationPath533);
                    			    	traverse17 = traverse();
                    			    	state.followingStackPointer--;

                    			    	stream_traverse.Add(traverse17.Tree);

                    			    }
                    			    break;

                    			default:
                    			    goto loop8;
                    	    }
                    	} while (true);

                    	loop8:
                    		;	// Stops C# compiler whining that label 'loop8' has no statements



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
                    	// 94:21: -> ^( RELATIVE_PATH ^( CURRENT_NODE ) ( traverse )* )
                    	{
                    	    // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\xpath_parser\\XPath.g:94:23: ^( RELATIVE_PATH ^( CURRENT_NODE ) ( traverse )* )
                    	    {
                    	    object root_1 = (object)adaptor.GetNilNode();
                    	    root_1 = (object)adaptor.BecomeRoot((object)adaptor.Create(RELATIVE_PATH, "RELATIVE_PATH"), root_1);

                    	    // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\xpath_parser\\XPath.g:94:39: ^( CURRENT_NODE )
                    	    {
                    	    object root_2 = (object)adaptor.GetNilNode();
                    	    root_2 = (object)adaptor.BecomeRoot((object)adaptor.Create(CURRENT_NODE, "CURRENT_NODE"), root_2);

                    	    adaptor.AddChild(root_1, root_2);
                    	    }
                    	    // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\xpath_parser\\XPath.g:94:55: ( traverse )*
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
                    // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\xpath_parser\\XPath.g:97:8: step ( traverse )*
                    {
                    	PushFollow(FOLLOW_step_in_relativeLocationPath564);
                    	step18 = step();
                    	state.followingStackPointer--;

                    	stream_step.Add(step18.Tree);
                    	// D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\xpath_parser\\XPath.g:97:13: ( traverse )*
                    	do 
                    	{
                    	    int alt9 = 2;
                    	    int LA9_0 = input.LA(1);

                    	    if ( ((LA9_0 >= PATHSEP && LA9_0 <= ABRPATH) || (LA9_0 >= 44 && LA9_0 <= 45)) )
                    	    {
                    	        alt9 = 1;
                    	    }


                    	    switch (alt9) 
                    		{
                    			case 1 :
                    			    // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\xpath_parser\\XPath.g:97:14: traverse
                    			    {
                    			    	PushFollow(FOLLOW_traverse_in_relativeLocationPath567);
                    			    	traverse19 = traverse();
                    			    	state.followingStackPointer--;

                    			    	stream_traverse.Add(traverse19.Tree);

                    			    }
                    			    break;

                    			default:
                    			    goto loop9;
                    	    }
                    	} while (true);

                    	loop9:
                    		;	// Stops C# compiler whining that label 'loop9' has no statements



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
                    	// 97:25: -> ^( RELATIVE_PATH ^( MATCH step ) ( traverse )* )
                    	{
                    	    // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\xpath_parser\\XPath.g:97:27: ^( RELATIVE_PATH ^( MATCH step ) ( traverse )* )
                    	    {
                    	    object root_1 = (object)adaptor.GetNilNode();
                    	    root_1 = (object)adaptor.BecomeRoot((object)adaptor.Create(RELATIVE_PATH, "RELATIVE_PATH"), root_1);

                    	    // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\xpath_parser\\XPath.g:97:43: ^( MATCH step )
                    	    {
                    	    object root_2 = (object)adaptor.GetNilNode();
                    	    root_2 = (object)adaptor.BecomeRoot((object)adaptor.Create(MATCH, "MATCH"), root_2);

                    	    adaptor.AddChild(root_2, stream_step.NextTree());

                    	    adaptor.AddChild(root_1, root_2);
                    	    }
                    	    // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\xpath_parser\\XPath.g:97:57: ( traverse )*
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
    // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\xpath_parser\\XPath.g:100:1: traverse : ( '/..' -> ^( TRAVERSE_UP ) | '/.' -> ^( CURRENT_NODE ) | '/' step -> ^( MATCH step ) | '//' step -> ^( RECURSIVE_MATCH step ) );
    public XPathParser.traverse_return traverse() // throws RecognitionException [1]
    {   
        XPathParser.traverse_return retval = new XPathParser.traverse_return();
        retval.Start = input.LT(1);

        object root_0 = null;

        IToken string_literal20 = null;
        IToken string_literal21 = null;
        IToken char_literal22 = null;
        IToken string_literal24 = null;
        XPathParser.step_return step23 = default(XPathParser.step_return);

        XPathParser.step_return step25 = default(XPathParser.step_return);


        object string_literal20_tree=null;
        object string_literal21_tree=null;
        object char_literal22_tree=null;
        object string_literal24_tree=null;
        RewriteRuleTokenStream stream_ABRPATH = new RewriteRuleTokenStream(adaptor,"token ABRPATH");
        RewriteRuleTokenStream stream_45 = new RewriteRuleTokenStream(adaptor,"token 45");
        RewriteRuleTokenStream stream_44 = new RewriteRuleTokenStream(adaptor,"token 44");
        RewriteRuleTokenStream stream_PATHSEP = new RewriteRuleTokenStream(adaptor,"token PATHSEP");
        RewriteRuleSubtreeStream stream_step = new RewriteRuleSubtreeStream(adaptor,"rule step");
        try 
    	{
            // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\xpath_parser\\XPath.g:101:2: ( '/..' -> ^( TRAVERSE_UP ) | '/.' -> ^( CURRENT_NODE ) | '/' step -> ^( MATCH step ) | '//' step -> ^( RECURSIVE_MATCH step ) )
            int alt11 = 4;
            switch ( input.LA(1) ) 
            {
            case 44:
            	{
                alt11 = 1;
                }
                break;
            case 45:
            	{
                alt11 = 2;
                }
                break;
            case PATHSEP:
            	{
                alt11 = 3;
                }
                break;
            case ABRPATH:
            	{
                alt11 = 4;
                }
                break;
            	default:
            	    NoViableAltException nvae_d11s0 =
            	        new NoViableAltException("", 11, 0, input);

            	    throw nvae_d11s0;
            }

            switch (alt11) 
            {
                case 1 :
                    // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\xpath_parser\\XPath.g:101:4: '/..'
                    {
                    	string_literal20=(IToken)Match(input,44,FOLLOW_44_in_traverse599);  
                    	stream_44.Add(string_literal20);



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
                    	// 101:10: -> ^( TRAVERSE_UP )
                    	{
                    	    // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\xpath_parser\\XPath.g:101:13: ^( TRAVERSE_UP )
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
                    // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\xpath_parser\\XPath.g:102:4: '/.'
                    {
                    	string_literal21=(IToken)Match(input,45,FOLLOW_45_in_traverse610);  
                    	stream_45.Add(string_literal21);



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
                    	// 102:9: -> ^( CURRENT_NODE )
                    	{
                    	    // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\xpath_parser\\XPath.g:102:12: ^( CURRENT_NODE )
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
                    // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\xpath_parser\\XPath.g:103:4: '/' step
                    {
                    	char_literal22=(IToken)Match(input,PATHSEP,FOLLOW_PATHSEP_in_traverse621);  
                    	stream_PATHSEP.Add(char_literal22);

                    	PushFollow(FOLLOW_step_in_traverse623);
                    	step23 = step();
                    	state.followingStackPointer--;

                    	stream_step.Add(step23.Tree);


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
                    	// 103:13: -> ^( MATCH step )
                    	{
                    	    // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\xpath_parser\\XPath.g:103:16: ^( MATCH step )
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
                case 4 :
                    // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\xpath_parser\\XPath.g:104:4: '//' step
                    {
                    	string_literal24=(IToken)Match(input,ABRPATH,FOLLOW_ABRPATH_in_traverse636);  
                    	stream_ABRPATH.Add(string_literal24);

                    	PushFollow(FOLLOW_step_in_traverse638);
                    	step25 = step();
                    	state.followingStackPointer--;

                    	stream_step.Add(step25.Tree);


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
                    	// 104:13: -> ^( RECURSIVE_MATCH step )
                    	{
                    	    // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\xpath_parser\\XPath.g:104:16: ^( RECURSIVE_MATCH step )
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
    // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\xpath_parser\\XPath.g:107:1: step : axisSpecifier nodeTest ( predicate )* ;
    public XPathParser.step_return step() // throws RecognitionException [1]
    {   
        XPathParser.step_return retval = new XPathParser.step_return();
        retval.Start = input.LT(1);

        object root_0 = null;

        XPathParser.axisSpecifier_return axisSpecifier26 = default(XPathParser.axisSpecifier_return);

        XPathParser.nodeTest_return nodeTest27 = default(XPathParser.nodeTest_return);

        XPathParser.predicate_return predicate28 = default(XPathParser.predicate_return);



        try 
    	{
            // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\xpath_parser\\XPath.g:108:3: ( axisSpecifier nodeTest ( predicate )* )
            // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\xpath_parser\\XPath.g:108:6: axisSpecifier nodeTest ( predicate )*
            {
            	root_0 = (object)adaptor.GetNilNode();

            	PushFollow(FOLLOW_axisSpecifier_in_step660);
            	axisSpecifier26 = axisSpecifier();
            	state.followingStackPointer--;

            	adaptor.AddChild(root_0, axisSpecifier26.Tree);
            	PushFollow(FOLLOW_nodeTest_in_step662);
            	nodeTest27 = nodeTest();
            	state.followingStackPointer--;

            	adaptor.AddChild(root_0, nodeTest27.Tree);
            	// D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\xpath_parser\\XPath.g:108:29: ( predicate )*
            	do 
            	{
            	    int alt12 = 2;
            	    int LA12_0 = input.LA(1);

            	    if ( (LA12_0 == LBRAC) )
            	    {
            	        alt12 = 1;
            	    }


            	    switch (alt12) 
            		{
            			case 1 :
            			    // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\xpath_parser\\XPath.g:108:29: predicate
            			    {
            			    	PushFollow(FOLLOW_predicate_in_step664);
            			    	predicate28 = predicate();
            			    	state.followingStackPointer--;

            			    	adaptor.AddChild(root_0, predicate28.Tree);

            			    }
            			    break;

            			default:
            			    goto loop12;
            	    }
            	} while (true);

            	loop12:
            		;	// Stops C# compiler whining that label 'loop12' has no statements


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
    // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\xpath_parser\\XPath.g:112:1: axisSpecifier : ( AxisName '::' | ( '@' )? );
    public XPathParser.axisSpecifier_return axisSpecifier() // throws RecognitionException [1]
    {   
        XPathParser.axisSpecifier_return retval = new XPathParser.axisSpecifier_return();
        retval.Start = input.LT(1);

        object root_0 = null;

        IToken AxisName29 = null;
        IToken string_literal30 = null;
        IToken char_literal31 = null;

        object AxisName29_tree=null;
        object string_literal30_tree=null;
        object char_literal31_tree=null;

        try 
    	{
            // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\xpath_parser\\XPath.g:113:3: ( AxisName '::' | ( '@' )? )
            int alt14 = 2;
            int LA14_0 = input.LA(1);

            if ( (LA14_0 == AxisName) )
            {
                int LA14_1 = input.LA(2);

                if ( (LA14_1 == CC) )
                {
                    alt14 = 1;
                }
                else if ( (LA14_1 == EOF || (LA14_1 >= PATHSEP && LA14_1 <= ABRPATH) || (LA14_1 >= RPAR && LA14_1 <= PLUS) || LA14_1 == MUL || (LA14_1 >= COMMA && LA14_1 <= COLON) || (LA14_1 >= 44 && LA14_1 <= 45) || (LA14_1 >= 47 && LA14_1 <= 52)) )
                {
                    alt14 = 2;
                }
                else 
                {
                    NoViableAltException nvae_d14s1 =
                        new NoViableAltException("", 14, 1, input);

                    throw nvae_d14s1;
                }
            }
            else if ( (LA14_0 == MUL || LA14_0 == AT || LA14_0 == NodeType || LA14_0 == NCName || LA14_0 == 46) )
            {
                alt14 = 2;
            }
            else 
            {
                NoViableAltException nvae_d14s0 =
                    new NoViableAltException("", 14, 0, input);

                throw nvae_d14s0;
            }
            switch (alt14) 
            {
                case 1 :
                    // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\xpath_parser\\XPath.g:113:6: AxisName '::'
                    {
                    	root_0 = (object)adaptor.GetNilNode();

                    	AxisName29=(IToken)Match(input,AxisName,FOLLOW_AxisName_in_axisSpecifier682); 
                    		AxisName29_tree = (object)adaptor.Create(AxisName29);
                    		adaptor.AddChild(root_0, AxisName29_tree);

                    	string_literal30=(IToken)Match(input,CC,FOLLOW_CC_in_axisSpecifier684); 
                    		string_literal30_tree = (object)adaptor.Create(string_literal30);
                    		adaptor.AddChild(root_0, string_literal30_tree);


                    }
                    break;
                case 2 :
                    // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\xpath_parser\\XPath.g:114:6: ( '@' )?
                    {
                    	root_0 = (object)adaptor.GetNilNode();

                    	// D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\xpath_parser\\XPath.g:114:6: ( '@' )?
                    	int alt13 = 2;
                    	int LA13_0 = input.LA(1);

                    	if ( (LA13_0 == AT) )
                    	{
                    	    alt13 = 1;
                    	}
                    	switch (alt13) 
                    	{
                    	    case 1 :
                    	        // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\xpath_parser\\XPath.g:114:6: '@'
                    	        {
                    	        	char_literal31=(IToken)Match(input,AT,FOLLOW_AT_in_axisSpecifier691); 
                    	        		char_literal31_tree = (object)adaptor.Create(char_literal31);
                    	        		adaptor.AddChild(root_0, char_literal31_tree);


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
    // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\xpath_parser\\XPath.g:117:1: nodeTest : ( nameTest | NodeType '(' ')' | 'processing-instruction' '(' Literal ')' );
    public XPathParser.nodeTest_return nodeTest() // throws RecognitionException [1]
    {   
        XPathParser.nodeTest_return retval = new XPathParser.nodeTest_return();
        retval.Start = input.LT(1);

        object root_0 = null;

        IToken NodeType33 = null;
        IToken char_literal34 = null;
        IToken char_literal35 = null;
        IToken string_literal36 = null;
        IToken char_literal37 = null;
        IToken Literal38 = null;
        IToken char_literal39 = null;
        XPathParser.nameTest_return nameTest32 = default(XPathParser.nameTest_return);


        object NodeType33_tree=null;
        object char_literal34_tree=null;
        object char_literal35_tree=null;
        object string_literal36_tree=null;
        object char_literal37_tree=null;
        object Literal38_tree=null;
        object char_literal39_tree=null;

        try 
    	{
            // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\xpath_parser\\XPath.g:117:9: ( nameTest | NodeType '(' ')' | 'processing-instruction' '(' Literal ')' )
            int alt15 = 3;
            switch ( input.LA(1) ) 
            {
            case MUL:
            case AxisName:
            case NCName:
            	{
                alt15 = 1;
                }
                break;
            case NodeType:
            	{
                alt15 = 2;
                }
                break;
            case 46:
            	{
                alt15 = 3;
                }
                break;
            	default:
            	    NoViableAltException nvae_d15s0 =
            	        new NoViableAltException("", 15, 0, input);

            	    throw nvae_d15s0;
            }

            switch (alt15) 
            {
                case 1 :
                    // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\xpath_parser\\XPath.g:117:12: nameTest
                    {
                    	root_0 = (object)adaptor.GetNilNode();

                    	PushFollow(FOLLOW_nameTest_in_nodeTest703);
                    	nameTest32 = nameTest();
                    	state.followingStackPointer--;

                    	adaptor.AddChild(root_0, nameTest32.Tree);

                    }
                    break;
                case 2 :
                    // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\xpath_parser\\XPath.g:118:6: NodeType '(' ')'
                    {
                    	root_0 = (object)adaptor.GetNilNode();

                    	NodeType33=(IToken)Match(input,NodeType,FOLLOW_NodeType_in_nodeTest710); 
                    		NodeType33_tree = (object)adaptor.Create(NodeType33);
                    		adaptor.AddChild(root_0, NodeType33_tree);

                    	char_literal34=(IToken)Match(input,LPAR,FOLLOW_LPAR_in_nodeTest712); 
                    		char_literal34_tree = (object)adaptor.Create(char_literal34);
                    		adaptor.AddChild(root_0, char_literal34_tree);

                    	char_literal35=(IToken)Match(input,RPAR,FOLLOW_RPAR_in_nodeTest714); 
                    		char_literal35_tree = (object)adaptor.Create(char_literal35);
                    		adaptor.AddChild(root_0, char_literal35_tree);


                    }
                    break;
                case 3 :
                    // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\xpath_parser\\XPath.g:119:6: 'processing-instruction' '(' Literal ')'
                    {
                    	root_0 = (object)adaptor.GetNilNode();

                    	string_literal36=(IToken)Match(input,46,FOLLOW_46_in_nodeTest721); 
                    		string_literal36_tree = (object)adaptor.Create(string_literal36);
                    		adaptor.AddChild(root_0, string_literal36_tree);

                    	char_literal37=(IToken)Match(input,LPAR,FOLLOW_LPAR_in_nodeTest723); 
                    		char_literal37_tree = (object)adaptor.Create(char_literal37);
                    		adaptor.AddChild(root_0, char_literal37_tree);

                    	Literal38=(IToken)Match(input,Literal,FOLLOW_Literal_in_nodeTest725); 
                    		Literal38_tree = (object)adaptor.Create(Literal38);
                    		adaptor.AddChild(root_0, Literal38_tree);

                    	char_literal39=(IToken)Match(input,RPAR,FOLLOW_RPAR_in_nodeTest727); 
                    		char_literal39_tree = (object)adaptor.Create(char_literal39);
                    		adaptor.AddChild(root_0, char_literal39_tree);


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
    // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\xpath_parser\\XPath.g:122:1: predicate : '[' expr ']' -> ^( PREDICATE expr ) ;
    public XPathParser.predicate_return predicate() // throws RecognitionException [1]
    {   
        XPathParser.predicate_return retval = new XPathParser.predicate_return();
        retval.Start = input.LT(1);

        object root_0 = null;

        IToken char_literal40 = null;
        IToken char_literal42 = null;
        XPathParser.expr_return expr41 = default(XPathParser.expr_return);


        object char_literal40_tree=null;
        object char_literal42_tree=null;
        RewriteRuleTokenStream stream_RBRAC = new RewriteRuleTokenStream(adaptor,"token RBRAC");
        RewriteRuleTokenStream stream_LBRAC = new RewriteRuleTokenStream(adaptor,"token LBRAC");
        RewriteRuleSubtreeStream stream_expr = new RewriteRuleSubtreeStream(adaptor,"rule expr");
        try 
    	{
            // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\xpath_parser\\XPath.g:123:3: ( '[' expr ']' -> ^( PREDICATE expr ) )
            // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\xpath_parser\\XPath.g:123:6: '[' expr ']'
            {
            	char_literal40=(IToken)Match(input,LBRAC,FOLLOW_LBRAC_in_predicate741);  
            	stream_LBRAC.Add(char_literal40);

            	PushFollow(FOLLOW_expr_in_predicate743);
            	expr41 = expr();
            	state.followingStackPointer--;

            	stream_expr.Add(expr41.Tree);
            	char_literal42=(IToken)Match(input,RBRAC,FOLLOW_RBRAC_in_predicate745);  
            	stream_RBRAC.Add(char_literal42);



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
            	// 123:19: -> ^( PREDICATE expr )
            	{
            	    // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\xpath_parser\\XPath.g:123:21: ^( PREDICATE expr )
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
    // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\xpath_parser\\XPath.g:126:1: abbreviatedStep : ( '.' | '..' );
    public XPathParser.abbreviatedStep_return abbreviatedStep() // throws RecognitionException [1]
    {   
        XPathParser.abbreviatedStep_return retval = new XPathParser.abbreviatedStep_return();
        retval.Start = input.LT(1);

        object root_0 = null;

        IToken set43 = null;

        object set43_tree=null;

        try 
    	{
            // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\xpath_parser\\XPath.g:127:3: ( '.' | '..' )
            // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\xpath_parser\\XPath.g:
            {
            	root_0 = (object)adaptor.GetNilNode();

            	set43 = (IToken)input.LT(1);
            	if ( input.LA(1) == DOT || input.LA(1) == DOTDOT ) 
            	{
            	    input.Consume();
            	    adaptor.AddChild(root_0, (object)adaptor.Create(set43));
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
    // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\xpath_parser\\XPath.g:131:1: expr : orExpr ;
    public XPathParser.expr_return expr() // throws RecognitionException [1]
    {   
        XPathParser.expr_return retval = new XPathParser.expr_return();
        retval.Start = input.LT(1);

        object root_0 = null;

        XPathParser.orExpr_return orExpr44 = default(XPathParser.orExpr_return);



        try 
    	{
            // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\xpath_parser\\XPath.g:131:7: ( orExpr )
            // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\xpath_parser\\XPath.g:131:10: orExpr
            {
            	root_0 = (object)adaptor.GetNilNode();

            	PushFollow(FOLLOW_orExpr_in_expr786);
            	orExpr44 = orExpr();
            	state.followingStackPointer--;

            	adaptor.AddChild(root_0, orExpr44.Tree);

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
    // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\xpath_parser\\XPath.g:134:1: primaryExpr : ( variableReference | '(' expr ')' | Literal | Number | functionCall );
    public XPathParser.primaryExpr_return primaryExpr() // throws RecognitionException [1]
    {   
        XPathParser.primaryExpr_return retval = new XPathParser.primaryExpr_return();
        retval.Start = input.LT(1);

        object root_0 = null;

        IToken char_literal46 = null;
        IToken char_literal48 = null;
        IToken Literal49 = null;
        IToken Number50 = null;
        XPathParser.variableReference_return variableReference45 = default(XPathParser.variableReference_return);

        XPathParser.expr_return expr47 = default(XPathParser.expr_return);

        XPathParser.functionCall_return functionCall51 = default(XPathParser.functionCall_return);


        object char_literal46_tree=null;
        object char_literal48_tree=null;
        object Literal49_tree=null;
        object Number50_tree=null;

        try 
    	{
            // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\xpath_parser\\XPath.g:135:3: ( variableReference | '(' expr ')' | Literal | Number | functionCall )
            int alt16 = 5;
            switch ( input.LA(1) ) 
            {
            case 53:
            	{
                alt16 = 1;
                }
                break;
            case LPAR:
            	{
                alt16 = 2;
                }
                break;
            case Literal:
            	{
                alt16 = 3;
                }
                break;
            case Number:
            	{
                alt16 = 4;
                }
                break;
            case AxisName:
            case NCName:
            	{
                alt16 = 5;
                }
                break;
            	default:
            	    NoViableAltException nvae_d16s0 =
            	        new NoViableAltException("", 16, 0, input);

            	    throw nvae_d16s0;
            }

            switch (alt16) 
            {
                case 1 :
                    // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\xpath_parser\\XPath.g:135:6: variableReference
                    {
                    	root_0 = (object)adaptor.GetNilNode();

                    	PushFollow(FOLLOW_variableReference_in_primaryExpr800);
                    	variableReference45 = variableReference();
                    	state.followingStackPointer--;

                    	adaptor.AddChild(root_0, variableReference45.Tree);

                    }
                    break;
                case 2 :
                    // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\xpath_parser\\XPath.g:136:6: '(' expr ')'
                    {
                    	root_0 = (object)adaptor.GetNilNode();

                    	char_literal46=(IToken)Match(input,LPAR,FOLLOW_LPAR_in_primaryExpr807); 
                    		char_literal46_tree = (object)adaptor.Create(char_literal46);
                    		adaptor.AddChild(root_0, char_literal46_tree);

                    	PushFollow(FOLLOW_expr_in_primaryExpr809);
                    	expr47 = expr();
                    	state.followingStackPointer--;

                    	adaptor.AddChild(root_0, expr47.Tree);
                    	char_literal48=(IToken)Match(input,RPAR,FOLLOW_RPAR_in_primaryExpr811); 
                    		char_literal48_tree = (object)adaptor.Create(char_literal48);
                    		adaptor.AddChild(root_0, char_literal48_tree);


                    }
                    break;
                case 3 :
                    // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\xpath_parser\\XPath.g:137:6: Literal
                    {
                    	root_0 = (object)adaptor.GetNilNode();

                    	Literal49=(IToken)Match(input,Literal,FOLLOW_Literal_in_primaryExpr818); 
                    		Literal49_tree = (object)adaptor.Create(Literal49);
                    		adaptor.AddChild(root_0, Literal49_tree);


                    }
                    break;
                case 4 :
                    // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\xpath_parser\\XPath.g:138:6: Number
                    {
                    	root_0 = (object)adaptor.GetNilNode();

                    	Number50=(IToken)Match(input,Number,FOLLOW_Number_in_primaryExpr825); 
                    		Number50_tree = (object)adaptor.Create(Number50);
                    		adaptor.AddChild(root_0, Number50_tree);


                    }
                    break;
                case 5 :
                    // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\xpath_parser\\XPath.g:139:6: functionCall
                    {
                    	root_0 = (object)adaptor.GetNilNode();

                    	PushFollow(FOLLOW_functionCall_in_primaryExpr834);
                    	functionCall51 = functionCall();
                    	state.followingStackPointer--;

                    	adaptor.AddChild(root_0, functionCall51.Tree);

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
    // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\xpath_parser\\XPath.g:142:1: functionCall : functionName '(' ( expr ( ',' expr )* )? ')' ;
    public XPathParser.functionCall_return functionCall() // throws RecognitionException [1]
    {   
        XPathParser.functionCall_return retval = new XPathParser.functionCall_return();
        retval.Start = input.LT(1);

        object root_0 = null;

        IToken char_literal53 = null;
        IToken char_literal55 = null;
        IToken char_literal57 = null;
        XPathParser.functionName_return functionName52 = default(XPathParser.functionName_return);

        XPathParser.expr_return expr54 = default(XPathParser.expr_return);

        XPathParser.expr_return expr56 = default(XPathParser.expr_return);


        object char_literal53_tree=null;
        object char_literal55_tree=null;
        object char_literal57_tree=null;

        try 
    	{
            // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\xpath_parser\\XPath.g:143:3: ( functionName '(' ( expr ( ',' expr )* )? ')' )
            // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\xpath_parser\\XPath.g:143:6: functionName '(' ( expr ( ',' expr )* )? ')'
            {
            	root_0 = (object)adaptor.GetNilNode();

            	PushFollow(FOLLOW_functionName_in_functionCall848);
            	functionName52 = functionName();
            	state.followingStackPointer--;

            	adaptor.AddChild(root_0, functionName52.Tree);
            	char_literal53=(IToken)Match(input,LPAR,FOLLOW_LPAR_in_functionCall850); 
            		char_literal53_tree = (object)adaptor.Create(char_literal53);
            		adaptor.AddChild(root_0, char_literal53_tree);

            	// D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\xpath_parser\\XPath.g:143:23: ( expr ( ',' expr )* )?
            	int alt18 = 2;
            	int LA18_0 = input.LA(1);

            	if ( ((LA18_0 >= PATHSEP && LA18_0 <= LPAR) || LA18_0 == MINUS || (LA18_0 >= DOT && LA18_0 <= AT) || (LA18_0 >= AxisName && LA18_0 <= NCName) || (LA18_0 >= 44 && LA18_0 <= 46) || LA18_0 == 53) )
            	{
            	    alt18 = 1;
            	}
            	switch (alt18) 
            	{
            	    case 1 :
            	        // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\xpath_parser\\XPath.g:143:25: expr ( ',' expr )*
            	        {
            	        	PushFollow(FOLLOW_expr_in_functionCall854);
            	        	expr54 = expr();
            	        	state.followingStackPointer--;

            	        	adaptor.AddChild(root_0, expr54.Tree);
            	        	// D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\xpath_parser\\XPath.g:143:30: ( ',' expr )*
            	        	do 
            	        	{
            	        	    int alt17 = 2;
            	        	    int LA17_0 = input.LA(1);

            	        	    if ( (LA17_0 == COMMA) )
            	        	    {
            	        	        alt17 = 1;
            	        	    }


            	        	    switch (alt17) 
            	        		{
            	        			case 1 :
            	        			    // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\xpath_parser\\XPath.g:143:32: ',' expr
            	        			    {
            	        			    	char_literal55=(IToken)Match(input,COMMA,FOLLOW_COMMA_in_functionCall858); 
            	        			    		char_literal55_tree = (object)adaptor.Create(char_literal55);
            	        			    		adaptor.AddChild(root_0, char_literal55_tree);

            	        			    	PushFollow(FOLLOW_expr_in_functionCall860);
            	        			    	expr56 = expr();
            	        			    	state.followingStackPointer--;

            	        			    	adaptor.AddChild(root_0, expr56.Tree);

            	        			    }
            	        			    break;

            	        			default:
            	        			    goto loop17;
            	        	    }
            	        	} while (true);

            	        	loop17:
            	        		;	// Stops C# compiler whining that label 'loop17' has no statements


            	        }
            	        break;

            	}

            	char_literal57=(IToken)Match(input,RPAR,FOLLOW_RPAR_in_functionCall868); 
            		char_literal57_tree = (object)adaptor.Create(char_literal57);
            		adaptor.AddChild(root_0, char_literal57_tree);


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
    // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\xpath_parser\\XPath.g:146:1: unionExprNoRoot : ( pathExprNoRoot ( '|' unionExprNoRoot )? | '/' '|' unionExprNoRoot );
    public XPathParser.unionExprNoRoot_return unionExprNoRoot() // throws RecognitionException [1]
    {   
        XPathParser.unionExprNoRoot_return retval = new XPathParser.unionExprNoRoot_return();
        retval.Start = input.LT(1);

        object root_0 = null;

        IToken char_literal59 = null;
        IToken char_literal61 = null;
        IToken char_literal62 = null;
        XPathParser.pathExprNoRoot_return pathExprNoRoot58 = default(XPathParser.pathExprNoRoot_return);

        XPathParser.unionExprNoRoot_return unionExprNoRoot60 = default(XPathParser.unionExprNoRoot_return);

        XPathParser.unionExprNoRoot_return unionExprNoRoot63 = default(XPathParser.unionExprNoRoot_return);


        object char_literal59_tree=null;
        object char_literal61_tree=null;
        object char_literal62_tree=null;

        try 
    	{
            // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\xpath_parser\\XPath.g:147:3: ( pathExprNoRoot ( '|' unionExprNoRoot )? | '/' '|' unionExprNoRoot )
            int alt20 = 2;
            int LA20_0 = input.LA(1);

            if ( ((LA20_0 >= ABRPATH && LA20_0 <= LPAR) || (LA20_0 >= DOT && LA20_0 <= AT) || (LA20_0 >= AxisName && LA20_0 <= NCName) || (LA20_0 >= 44 && LA20_0 <= 46) || LA20_0 == 53) )
            {
                alt20 = 1;
            }
            else if ( (LA20_0 == PATHSEP) )
            {
                int LA20_2 = input.LA(2);

                if ( (LA20_2 == PIPE) )
                {
                    alt20 = 2;
                }
                else if ( (LA20_2 == MUL || LA20_2 == AT || (LA20_2 >= AxisName && LA20_2 <= NodeType) || LA20_2 == NCName || LA20_2 == 46) )
                {
                    alt20 = 1;
                }
                else 
                {
                    NoViableAltException nvae_d20s2 =
                        new NoViableAltException("", 20, 2, input);

                    throw nvae_d20s2;
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
                    // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\xpath_parser\\XPath.g:147:6: pathExprNoRoot ( '|' unionExprNoRoot )?
                    {
                    	root_0 = (object)adaptor.GetNilNode();

                    	PushFollow(FOLLOW_pathExprNoRoot_in_unionExprNoRoot882);
                    	pathExprNoRoot58 = pathExprNoRoot();
                    	state.followingStackPointer--;

                    	adaptor.AddChild(root_0, pathExprNoRoot58.Tree);
                    	// D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\xpath_parser\\XPath.g:147:21: ( '|' unionExprNoRoot )?
                    	int alt19 = 2;
                    	int LA19_0 = input.LA(1);

                    	if ( (LA19_0 == PIPE) )
                    	{
                    	    alt19 = 1;
                    	}
                    	switch (alt19) 
                    	{
                    	    case 1 :
                    	        // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\xpath_parser\\XPath.g:147:22: '|' unionExprNoRoot
                    	        {
                    	        	char_literal59=(IToken)Match(input,PIPE,FOLLOW_PIPE_in_unionExprNoRoot885); 
                    	        		char_literal59_tree = (object)adaptor.Create(char_literal59);
                    	        		root_0 = (object)adaptor.BecomeRoot(char_literal59_tree, root_0);

                    	        	PushFollow(FOLLOW_unionExprNoRoot_in_unionExprNoRoot888);
                    	        	unionExprNoRoot60 = unionExprNoRoot();
                    	        	state.followingStackPointer--;

                    	        	adaptor.AddChild(root_0, unionExprNoRoot60.Tree);

                    	        }
                    	        break;

                    	}


                    }
                    break;
                case 2 :
                    // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\xpath_parser\\XPath.g:148:6: '/' '|' unionExprNoRoot
                    {
                    	root_0 = (object)adaptor.GetNilNode();

                    	char_literal61=(IToken)Match(input,PATHSEP,FOLLOW_PATHSEP_in_unionExprNoRoot897); 
                    		char_literal61_tree = (object)adaptor.Create(char_literal61);
                    		adaptor.AddChild(root_0, char_literal61_tree);

                    	char_literal62=(IToken)Match(input,PIPE,FOLLOW_PIPE_in_unionExprNoRoot899); 
                    		char_literal62_tree = (object)adaptor.Create(char_literal62);
                    		root_0 = (object)adaptor.BecomeRoot(char_literal62_tree, root_0);

                    	PushFollow(FOLLOW_unionExprNoRoot_in_unionExprNoRoot902);
                    	unionExprNoRoot63 = unionExprNoRoot();
                    	state.followingStackPointer--;

                    	adaptor.AddChild(root_0, unionExprNoRoot63.Tree);

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
    // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\xpath_parser\\XPath.g:151:1: pathExprNoRoot : ( locationPath | filterExpr ( ( '/' | '//' ) relativeLocationPath )? );
    public XPathParser.pathExprNoRoot_return pathExprNoRoot() // throws RecognitionException [1]
    {   
        XPathParser.pathExprNoRoot_return retval = new XPathParser.pathExprNoRoot_return();
        retval.Start = input.LT(1);

        object root_0 = null;

        IToken set66 = null;
        XPathParser.locationPath_return locationPath64 = default(XPathParser.locationPath_return);

        XPathParser.filterExpr_return filterExpr65 = default(XPathParser.filterExpr_return);

        XPathParser.relativeLocationPath_return relativeLocationPath67 = default(XPathParser.relativeLocationPath_return);


        object set66_tree=null;

        try 
    	{
            // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\xpath_parser\\XPath.g:152:3: ( locationPath | filterExpr ( ( '/' | '//' ) relativeLocationPath )? )
            int alt22 = 2;
            switch ( input.LA(1) ) 
            {
            case PATHSEP:
            case ABRPATH:
            case DOT:
            case MUL:
            case DOTDOT:
            case AT:
            case NodeType:
            case 44:
            case 45:
            case 46:
            	{
                alt22 = 1;
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
                case 44:
                case 45:
                case 47:
                case 48:
                case 49:
                case 50:
                case 51:
                case 52:
                	{
                    alt22 = 1;
                    }
                    break;
                case COLON:
                	{
                    int LA22_5 = input.LA(3);

                    if ( (LA22_5 == MUL) )
                    {
                        alt22 = 1;
                    }
                    else if ( (LA22_5 == AxisName || LA22_5 == NCName) )
                    {
                        int LA22_6 = input.LA(4);

                        if ( (LA22_6 == LPAR) )
                        {
                            alt22 = 2;
                        }
                        else if ( (LA22_6 == EOF || (LA22_6 >= PATHSEP && LA22_6 <= ABRPATH) || (LA22_6 >= RPAR && LA22_6 <= PLUS) || LA22_6 == MUL || (LA22_6 >= COMMA && LA22_6 <= GE) || (LA22_6 >= 44 && LA22_6 <= 45) || (LA22_6 >= 47 && LA22_6 <= 52)) )
                        {
                            alt22 = 1;
                        }
                        else 
                        {
                            NoViableAltException nvae_d22s6 =
                                new NoViableAltException("", 22, 6, input);

                            throw nvae_d22s6;
                        }
                    }
                    else 
                    {
                        NoViableAltException nvae_d22s5 =
                            new NoViableAltException("", 22, 5, input);

                        throw nvae_d22s5;
                    }
                    }
                    break;
                case LPAR:
                	{
                    alt22 = 2;
                    }
                    break;
                	default:
                	    NoViableAltException nvae_d22s2 =
                	        new NoViableAltException("", 22, 2, input);

                	    throw nvae_d22s2;
                }

                }
                break;
            case NCName:
            	{
                switch ( input.LA(2) ) 
                {
                case COLON:
                	{
                    int LA22_5 = input.LA(3);

                    if ( (LA22_5 == MUL) )
                    {
                        alt22 = 1;
                    }
                    else if ( (LA22_5 == AxisName || LA22_5 == NCName) )
                    {
                        int LA22_6 = input.LA(4);

                        if ( (LA22_6 == LPAR) )
                        {
                            alt22 = 2;
                        }
                        else if ( (LA22_6 == EOF || (LA22_6 >= PATHSEP && LA22_6 <= ABRPATH) || (LA22_6 >= RPAR && LA22_6 <= PLUS) || LA22_6 == MUL || (LA22_6 >= COMMA && LA22_6 <= GE) || (LA22_6 >= 44 && LA22_6 <= 45) || (LA22_6 >= 47 && LA22_6 <= 52)) )
                        {
                            alt22 = 1;
                        }
                        else 
                        {
                            NoViableAltException nvae_d22s6 =
                                new NoViableAltException("", 22, 6, input);

                            throw nvae_d22s6;
                        }
                    }
                    else 
                    {
                        NoViableAltException nvae_d22s5 =
                            new NoViableAltException("", 22, 5, input);

                        throw nvae_d22s5;
                    }
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
                case 44:
                case 45:
                case 47:
                case 48:
                case 49:
                case 50:
                case 51:
                case 52:
                	{
                    alt22 = 1;
                    }
                    break;
                case LPAR:
                	{
                    alt22 = 2;
                    }
                    break;
                	default:
                	    NoViableAltException nvae_d22s3 =
                	        new NoViableAltException("", 22, 3, input);

                	    throw nvae_d22s3;
                }

                }
                break;
            case LPAR:
            case Literal:
            case Number:
            case 53:
            	{
                alt22 = 2;
                }
                break;
            	default:
            	    NoViableAltException nvae_d22s0 =
            	        new NoViableAltException("", 22, 0, input);

            	    throw nvae_d22s0;
            }

            switch (alt22) 
            {
                case 1 :
                    // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\xpath_parser\\XPath.g:152:6: locationPath
                    {
                    	root_0 = (object)adaptor.GetNilNode();

                    	PushFollow(FOLLOW_locationPath_in_pathExprNoRoot916);
                    	locationPath64 = locationPath();
                    	state.followingStackPointer--;

                    	adaptor.AddChild(root_0, locationPath64.Tree);

                    }
                    break;
                case 2 :
                    // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\xpath_parser\\XPath.g:153:6: filterExpr ( ( '/' | '//' ) relativeLocationPath )?
                    {
                    	root_0 = (object)adaptor.GetNilNode();

                    	PushFollow(FOLLOW_filterExpr_in_pathExprNoRoot923);
                    	filterExpr65 = filterExpr();
                    	state.followingStackPointer--;

                    	adaptor.AddChild(root_0, filterExpr65.Tree);
                    	// D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\xpath_parser\\XPath.g:153:17: ( ( '/' | '//' ) relativeLocationPath )?
                    	int alt21 = 2;
                    	int LA21_0 = input.LA(1);

                    	if ( ((LA21_0 >= PATHSEP && LA21_0 <= ABRPATH)) )
                    	{
                    	    alt21 = 1;
                    	}
                    	switch (alt21) 
                    	{
                    	    case 1 :
                    	        // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\xpath_parser\\XPath.g:153:18: ( '/' | '//' ) relativeLocationPath
                    	        {
                    	        	set66 = (IToken)input.LT(1);
                    	        	if ( (input.LA(1) >= PATHSEP && input.LA(1) <= ABRPATH) ) 
                    	        	{
                    	        	    input.Consume();
                    	        	    adaptor.AddChild(root_0, (object)adaptor.Create(set66));
                    	        	    state.errorRecovery = false;
                    	        	}
                    	        	else 
                    	        	{
                    	        	    MismatchedSetException mse = new MismatchedSetException(null,input);
                    	        	    throw mse;
                    	        	}

                    	        	PushFollow(FOLLOW_relativeLocationPath_in_pathExprNoRoot932);
                    	        	relativeLocationPath67 = relativeLocationPath();
                    	        	state.followingStackPointer--;

                    	        	adaptor.AddChild(root_0, relativeLocationPath67.Tree);

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
    // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\xpath_parser\\XPath.g:156:1: filterExpr : primaryExpr ( predicate )* ;
    public XPathParser.filterExpr_return filterExpr() // throws RecognitionException [1]
    {   
        XPathParser.filterExpr_return retval = new XPathParser.filterExpr_return();
        retval.Start = input.LT(1);

        object root_0 = null;

        XPathParser.primaryExpr_return primaryExpr68 = default(XPathParser.primaryExpr_return);

        XPathParser.predicate_return predicate69 = default(XPathParser.predicate_return);



        try 
    	{
            // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\xpath_parser\\XPath.g:157:3: ( primaryExpr ( predicate )* )
            // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\xpath_parser\\XPath.g:157:6: primaryExpr ( predicate )*
            {
            	root_0 = (object)adaptor.GetNilNode();

            	PushFollow(FOLLOW_primaryExpr_in_filterExpr948);
            	primaryExpr68 = primaryExpr();
            	state.followingStackPointer--;

            	adaptor.AddChild(root_0, primaryExpr68.Tree);
            	// D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\xpath_parser\\XPath.g:157:18: ( predicate )*
            	do 
            	{
            	    int alt23 = 2;
            	    int LA23_0 = input.LA(1);

            	    if ( (LA23_0 == LBRAC) )
            	    {
            	        alt23 = 1;
            	    }


            	    switch (alt23) 
            		{
            			case 1 :
            			    // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\xpath_parser\\XPath.g:157:18: predicate
            			    {
            			    	PushFollow(FOLLOW_predicate_in_filterExpr950);
            			    	predicate69 = predicate();
            			    	state.followingStackPointer--;

            			    	adaptor.AddChild(root_0, predicate69.Tree);

            			    }
            			    break;

            			default:
            			    goto loop23;
            	    }
            	} while (true);

            	loop23:
            		;	// Stops C# compiler whining that label 'loop23' has no statements


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
    // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\xpath_parser\\XPath.g:160:1: orExpr : andExpr ( 'or' andExpr )* ;
    public XPathParser.orExpr_return orExpr() // throws RecognitionException [1]
    {   
        XPathParser.orExpr_return retval = new XPathParser.orExpr_return();
        retval.Start = input.LT(1);

        object root_0 = null;

        IToken string_literal71 = null;
        XPathParser.andExpr_return andExpr70 = default(XPathParser.andExpr_return);

        XPathParser.andExpr_return andExpr72 = default(XPathParser.andExpr_return);


        object string_literal71_tree=null;

        try 
    	{
            // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\xpath_parser\\XPath.g:160:9: ( andExpr ( 'or' andExpr )* )
            // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\xpath_parser\\XPath.g:160:12: andExpr ( 'or' andExpr )*
            {
            	root_0 = (object)adaptor.GetNilNode();

            	PushFollow(FOLLOW_andExpr_in_orExpr964);
            	andExpr70 = andExpr();
            	state.followingStackPointer--;

            	adaptor.AddChild(root_0, andExpr70.Tree);
            	// D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\xpath_parser\\XPath.g:160:20: ( 'or' andExpr )*
            	do 
            	{
            	    int alt24 = 2;
            	    int LA24_0 = input.LA(1);

            	    if ( (LA24_0 == 47) )
            	    {
            	        alt24 = 1;
            	    }


            	    switch (alt24) 
            		{
            			case 1 :
            			    // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\xpath_parser\\XPath.g:160:21: 'or' andExpr
            			    {
            			    	string_literal71=(IToken)Match(input,47,FOLLOW_47_in_orExpr967); 
            			    		string_literal71_tree = (object)adaptor.Create(string_literal71);
            			    		root_0 = (object)adaptor.BecomeRoot(string_literal71_tree, root_0);

            			    	PushFollow(FOLLOW_andExpr_in_orExpr970);
            			    	andExpr72 = andExpr();
            			    	state.followingStackPointer--;

            			    	adaptor.AddChild(root_0, andExpr72.Tree);

            			    }
            			    break;

            			default:
            			    goto loop24;
            	    }
            	} while (true);

            	loop24:
            		;	// Stops C# compiler whining that label 'loop24' has no statements


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
    // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\xpath_parser\\XPath.g:163:1: andExpr : equalityExpr ( 'and' equalityExpr )* ;
    public XPathParser.andExpr_return andExpr() // throws RecognitionException [1]
    {   
        XPathParser.andExpr_return retval = new XPathParser.andExpr_return();
        retval.Start = input.LT(1);

        object root_0 = null;

        IToken string_literal74 = null;
        XPathParser.equalityExpr_return equalityExpr73 = default(XPathParser.equalityExpr_return);

        XPathParser.equalityExpr_return equalityExpr75 = default(XPathParser.equalityExpr_return);


        object string_literal74_tree=null;

        try 
    	{
            // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\xpath_parser\\XPath.g:163:10: ( equalityExpr ( 'and' equalityExpr )* )
            // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\xpath_parser\\XPath.g:163:13: equalityExpr ( 'and' equalityExpr )*
            {
            	root_0 = (object)adaptor.GetNilNode();

            	PushFollow(FOLLOW_equalityExpr_in_andExpr985);
            	equalityExpr73 = equalityExpr();
            	state.followingStackPointer--;

            	adaptor.AddChild(root_0, equalityExpr73.Tree);
            	// D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\xpath_parser\\XPath.g:163:26: ( 'and' equalityExpr )*
            	do 
            	{
            	    int alt25 = 2;
            	    int LA25_0 = input.LA(1);

            	    if ( (LA25_0 == 48) )
            	    {
            	        alt25 = 1;
            	    }


            	    switch (alt25) 
            		{
            			case 1 :
            			    // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\xpath_parser\\XPath.g:163:27: 'and' equalityExpr
            			    {
            			    	string_literal74=(IToken)Match(input,48,FOLLOW_48_in_andExpr988); 
            			    		string_literal74_tree = (object)adaptor.Create(string_literal74);
            			    		root_0 = (object)adaptor.BecomeRoot(string_literal74_tree, root_0);

            			    	PushFollow(FOLLOW_equalityExpr_in_andExpr991);
            			    	equalityExpr75 = equalityExpr();
            			    	state.followingStackPointer--;

            			    	adaptor.AddChild(root_0, equalityExpr75.Tree);

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
    // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\xpath_parser\\XPath.g:166:1: equalityExpr : relationalExpr ( ( '=' | '!=' ) relationalExpr )* ;
    public XPathParser.equalityExpr_return equalityExpr() // throws RecognitionException [1]
    {   
        XPathParser.equalityExpr_return retval = new XPathParser.equalityExpr_return();
        retval.Start = input.LT(1);

        object root_0 = null;

        IToken set77 = null;
        XPathParser.relationalExpr_return relationalExpr76 = default(XPathParser.relationalExpr_return);

        XPathParser.relationalExpr_return relationalExpr78 = default(XPathParser.relationalExpr_return);


        object set77_tree=null;

        try 
    	{
            // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\xpath_parser\\XPath.g:167:3: ( relationalExpr ( ( '=' | '!=' ) relationalExpr )* )
            // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\xpath_parser\\XPath.g:167:6: relationalExpr ( ( '=' | '!=' ) relationalExpr )*
            {
            	root_0 = (object)adaptor.GetNilNode();

            	PushFollow(FOLLOW_relationalExpr_in_equalityExpr1007);
            	relationalExpr76 = relationalExpr();
            	state.followingStackPointer--;

            	adaptor.AddChild(root_0, relationalExpr76.Tree);
            	// D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\xpath_parser\\XPath.g:167:21: ( ( '=' | '!=' ) relationalExpr )*
            	do 
            	{
            	    int alt26 = 2;
            	    int LA26_0 = input.LA(1);

            	    if ( ((LA26_0 >= 49 && LA26_0 <= 50)) )
            	    {
            	        alt26 = 1;
            	    }


            	    switch (alt26) 
            		{
            			case 1 :
            			    // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\xpath_parser\\XPath.g:167:22: ( '=' | '!=' ) relationalExpr
            			    {
            			    	set77=(IToken)input.LT(1);
            			    	set77 = (IToken)input.LT(1);
            			    	if ( (input.LA(1) >= 49 && input.LA(1) <= 50) ) 
            			    	{
            			    	    input.Consume();
            			    	    root_0 = (object)adaptor.BecomeRoot((object)adaptor.Create(set77), root_0);
            			    	    state.errorRecovery = false;
            			    	}
            			    	else 
            			    	{
            			    	    MismatchedSetException mse = new MismatchedSetException(null,input);
            			    	    throw mse;
            			    	}

            			    	PushFollow(FOLLOW_relationalExpr_in_equalityExpr1017);
            			    	relationalExpr78 = relationalExpr();
            			    	state.followingStackPointer--;

            			    	adaptor.AddChild(root_0, relationalExpr78.Tree);

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
    // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\xpath_parser\\XPath.g:170:1: relationalExpr : additiveExpr ( ( '<' | '>' | '<=' | '>=' ) additiveExpr )* ;
    public XPathParser.relationalExpr_return relationalExpr() // throws RecognitionException [1]
    {   
        XPathParser.relationalExpr_return retval = new XPathParser.relationalExpr_return();
        retval.Start = input.LT(1);

        object root_0 = null;

        IToken set80 = null;
        XPathParser.additiveExpr_return additiveExpr79 = default(XPathParser.additiveExpr_return);

        XPathParser.additiveExpr_return additiveExpr81 = default(XPathParser.additiveExpr_return);


        object set80_tree=null;

        try 
    	{
            // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\xpath_parser\\XPath.g:171:3: ( additiveExpr ( ( '<' | '>' | '<=' | '>=' ) additiveExpr )* )
            // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\xpath_parser\\XPath.g:171:6: additiveExpr ( ( '<' | '>' | '<=' | '>=' ) additiveExpr )*
            {
            	root_0 = (object)adaptor.GetNilNode();

            	PushFollow(FOLLOW_additiveExpr_in_relationalExpr1033);
            	additiveExpr79 = additiveExpr();
            	state.followingStackPointer--;

            	adaptor.AddChild(root_0, additiveExpr79.Tree);
            	// D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\xpath_parser\\XPath.g:171:19: ( ( '<' | '>' | '<=' | '>=' ) additiveExpr )*
            	do 
            	{
            	    int alt27 = 2;
            	    int LA27_0 = input.LA(1);

            	    if ( ((LA27_0 >= LESS && LA27_0 <= GE)) )
            	    {
            	        alt27 = 1;
            	    }


            	    switch (alt27) 
            		{
            			case 1 :
            			    // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\xpath_parser\\XPath.g:171:20: ( '<' | '>' | '<=' | '>=' ) additiveExpr
            			    {
            			    	set80=(IToken)input.LT(1);
            			    	set80 = (IToken)input.LT(1);
            			    	if ( (input.LA(1) >= LESS && input.LA(1) <= GE) ) 
            			    	{
            			    	    input.Consume();
            			    	    root_0 = (object)adaptor.BecomeRoot((object)adaptor.Create(set80), root_0);
            			    	    state.errorRecovery = false;
            			    	}
            			    	else 
            			    	{
            			    	    MismatchedSetException mse = new MismatchedSetException(null,input);
            			    	    throw mse;
            			    	}

            			    	PushFollow(FOLLOW_additiveExpr_in_relationalExpr1047);
            			    	additiveExpr81 = additiveExpr();
            			    	state.followingStackPointer--;

            			    	adaptor.AddChild(root_0, additiveExpr81.Tree);

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
    // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\xpath_parser\\XPath.g:174:1: additiveExpr : multiplicativeExpr ( ( '+' | '-' ) multiplicativeExpr )* ;
    public XPathParser.additiveExpr_return additiveExpr() // throws RecognitionException [1]
    {   
        XPathParser.additiveExpr_return retval = new XPathParser.additiveExpr_return();
        retval.Start = input.LT(1);

        object root_0 = null;

        IToken set83 = null;
        XPathParser.multiplicativeExpr_return multiplicativeExpr82 = default(XPathParser.multiplicativeExpr_return);

        XPathParser.multiplicativeExpr_return multiplicativeExpr84 = default(XPathParser.multiplicativeExpr_return);


        object set83_tree=null;

        try 
    	{
            // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\xpath_parser\\XPath.g:175:3: ( multiplicativeExpr ( ( '+' | '-' ) multiplicativeExpr )* )
            // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\xpath_parser\\XPath.g:175:6: multiplicativeExpr ( ( '+' | '-' ) multiplicativeExpr )*
            {
            	root_0 = (object)adaptor.GetNilNode();

            	PushFollow(FOLLOW_multiplicativeExpr_in_additiveExpr1063);
            	multiplicativeExpr82 = multiplicativeExpr();
            	state.followingStackPointer--;

            	adaptor.AddChild(root_0, multiplicativeExpr82.Tree);
            	// D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\xpath_parser\\XPath.g:175:25: ( ( '+' | '-' ) multiplicativeExpr )*
            	do 
            	{
            	    int alt28 = 2;
            	    int LA28_0 = input.LA(1);

            	    if ( ((LA28_0 >= MINUS && LA28_0 <= PLUS)) )
            	    {
            	        alt28 = 1;
            	    }


            	    switch (alt28) 
            		{
            			case 1 :
            			    // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\xpath_parser\\XPath.g:175:26: ( '+' | '-' ) multiplicativeExpr
            			    {
            			    	set83=(IToken)input.LT(1);
            			    	set83 = (IToken)input.LT(1);
            			    	if ( (input.LA(1) >= MINUS && input.LA(1) <= PLUS) ) 
            			    	{
            			    	    input.Consume();
            			    	    root_0 = (object)adaptor.BecomeRoot((object)adaptor.Create(set83), root_0);
            			    	    state.errorRecovery = false;
            			    	}
            			    	else 
            			    	{
            			    	    MismatchedSetException mse = new MismatchedSetException(null,input);
            			    	    throw mse;
            			    	}

            			    	PushFollow(FOLLOW_multiplicativeExpr_in_additiveExpr1073);
            			    	multiplicativeExpr84 = multiplicativeExpr();
            			    	state.followingStackPointer--;

            			    	adaptor.AddChild(root_0, multiplicativeExpr84.Tree);

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
    // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\xpath_parser\\XPath.g:178:1: multiplicativeExpr : ( unaryExprNoRoot ( ( '*' | 'div' | 'numMods' ) multiplicativeExpr )? | '/' ( ( 'div' | 'numMods' ) multiplicativeExpr )? );
    public XPathParser.multiplicativeExpr_return multiplicativeExpr() // throws RecognitionException [1]
    {   
        XPathParser.multiplicativeExpr_return retval = new XPathParser.multiplicativeExpr_return();
        retval.Start = input.LT(1);

        object root_0 = null;

        IToken set86 = null;
        IToken char_literal88 = null;
        IToken set89 = null;
        XPathParser.unaryExprNoRoot_return unaryExprNoRoot85 = default(XPathParser.unaryExprNoRoot_return);

        XPathParser.multiplicativeExpr_return multiplicativeExpr87 = default(XPathParser.multiplicativeExpr_return);

        XPathParser.multiplicativeExpr_return multiplicativeExpr90 = default(XPathParser.multiplicativeExpr_return);


        object set86_tree=null;
        object char_literal88_tree=null;
        object set89_tree=null;

        try 
    	{
            // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\xpath_parser\\XPath.g:179:3: ( unaryExprNoRoot ( ( '*' | 'div' | 'numMods' ) multiplicativeExpr )? | '/' ( ( 'div' | 'numMods' ) multiplicativeExpr )? )
            int alt31 = 2;
            int LA31_0 = input.LA(1);

            if ( ((LA31_0 >= ABRPATH && LA31_0 <= LPAR) || LA31_0 == MINUS || (LA31_0 >= DOT && LA31_0 <= AT) || (LA31_0 >= AxisName && LA31_0 <= NCName) || (LA31_0 >= 44 && LA31_0 <= 46) || LA31_0 == 53) )
            {
                alt31 = 1;
            }
            else if ( (LA31_0 == PATHSEP) )
            {
                int LA31_2 = input.LA(2);

                if ( (LA31_2 == MUL || LA31_2 == AT || LA31_2 == PIPE || (LA31_2 >= AxisName && LA31_2 <= NodeType) || LA31_2 == NCName || LA31_2 == 46) )
                {
                    alt31 = 1;
                }
                else if ( (LA31_2 == EOF || LA31_2 == RPAR || (LA31_2 >= RBRAC && LA31_2 <= PLUS) || LA31_2 == COMMA || (LA31_2 >= LESS && LA31_2 <= GE) || (LA31_2 >= 47 && LA31_2 <= 52)) )
                {
                    alt31 = 2;
                }
                else 
                {
                    NoViableAltException nvae_d31s2 =
                        new NoViableAltException("", 31, 2, input);

                    throw nvae_d31s2;
                }
            }
            else 
            {
                NoViableAltException nvae_d31s0 =
                    new NoViableAltException("", 31, 0, input);

                throw nvae_d31s0;
            }
            switch (alt31) 
            {
                case 1 :
                    // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\xpath_parser\\XPath.g:179:6: unaryExprNoRoot ( ( '*' | 'div' | 'numMods' ) multiplicativeExpr )?
                    {
                    	root_0 = (object)adaptor.GetNilNode();

                    	PushFollow(FOLLOW_unaryExprNoRoot_in_multiplicativeExpr1089);
                    	unaryExprNoRoot85 = unaryExprNoRoot();
                    	state.followingStackPointer--;

                    	adaptor.AddChild(root_0, unaryExprNoRoot85.Tree);
                    	// D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\xpath_parser\\XPath.g:179:22: ( ( '*' | 'div' | 'numMods' ) multiplicativeExpr )?
                    	int alt29 = 2;
                    	int LA29_0 = input.LA(1);

                    	if ( (LA29_0 == MUL || (LA29_0 >= 51 && LA29_0 <= 52)) )
                    	{
                    	    alt29 = 1;
                    	}
                    	switch (alt29) 
                    	{
                    	    case 1 :
                    	        // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\xpath_parser\\XPath.g:179:23: ( '*' | 'div' | 'numMods' ) multiplicativeExpr
                    	        {
                    	        	set86=(IToken)input.LT(1);
                    	        	set86 = (IToken)input.LT(1);
                    	        	if ( input.LA(1) == MUL || (input.LA(1) >= 51 && input.LA(1) <= 52) ) 
                    	        	{
                    	        	    input.Consume();
                    	        	    root_0 = (object)adaptor.BecomeRoot((object)adaptor.Create(set86), root_0);
                    	        	    state.errorRecovery = false;
                    	        	}
                    	        	else 
                    	        	{
                    	        	    MismatchedSetException mse = new MismatchedSetException(null,input);
                    	        	    throw mse;
                    	        	}

                    	        	PushFollow(FOLLOW_multiplicativeExpr_in_multiplicativeExpr1101);
                    	        	multiplicativeExpr87 = multiplicativeExpr();
                    	        	state.followingStackPointer--;

                    	        	adaptor.AddChild(root_0, multiplicativeExpr87.Tree);

                    	        }
                    	        break;

                    	}


                    }
                    break;
                case 2 :
                    // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\xpath_parser\\XPath.g:180:6: '/' ( ( 'div' | 'numMods' ) multiplicativeExpr )?
                    {
                    	root_0 = (object)adaptor.GetNilNode();

                    	char_literal88=(IToken)Match(input,PATHSEP,FOLLOW_PATHSEP_in_multiplicativeExpr1110); 
                    		char_literal88_tree = (object)adaptor.Create(char_literal88);
                    		adaptor.AddChild(root_0, char_literal88_tree);

                    	// D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\xpath_parser\\XPath.g:180:10: ( ( 'div' | 'numMods' ) multiplicativeExpr )?
                    	int alt30 = 2;
                    	int LA30_0 = input.LA(1);

                    	if ( ((LA30_0 >= 51 && LA30_0 <= 52)) )
                    	{
                    	    alt30 = 1;
                    	}
                    	switch (alt30) 
                    	{
                    	    case 1 :
                    	        // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\xpath_parser\\XPath.g:180:11: ( 'div' | 'numMods' ) multiplicativeExpr
                    	        {
                    	        	set89=(IToken)input.LT(1);
                    	        	set89 = (IToken)input.LT(1);
                    	        	if ( (input.LA(1) >= 51 && input.LA(1) <= 52) ) 
                    	        	{
                    	        	    input.Consume();
                    	        	    root_0 = (object)adaptor.BecomeRoot((object)adaptor.Create(set89), root_0);
                    	        	    state.errorRecovery = false;
                    	        	}
                    	        	else 
                    	        	{
                    	        	    MismatchedSetException mse = new MismatchedSetException(null,input);
                    	        	    throw mse;
                    	        	}

                    	        	PushFollow(FOLLOW_multiplicativeExpr_in_multiplicativeExpr1120);
                    	        	multiplicativeExpr90 = multiplicativeExpr();
                    	        	state.followingStackPointer--;

                    	        	adaptor.AddChild(root_0, multiplicativeExpr90.Tree);

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
    // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\xpath_parser\\XPath.g:183:1: unaryExprNoRoot : ( '-' )* unionExprNoRoot ;
    public XPathParser.unaryExprNoRoot_return unaryExprNoRoot() // throws RecognitionException [1]
    {   
        XPathParser.unaryExprNoRoot_return retval = new XPathParser.unaryExprNoRoot_return();
        retval.Start = input.LT(1);

        object root_0 = null;

        IToken char_literal91 = null;
        XPathParser.unionExprNoRoot_return unionExprNoRoot92 = default(XPathParser.unionExprNoRoot_return);


        object char_literal91_tree=null;

        try 
    	{
            // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\xpath_parser\\XPath.g:184:3: ( ( '-' )* unionExprNoRoot )
            // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\xpath_parser\\XPath.g:184:6: ( '-' )* unionExprNoRoot
            {
            	root_0 = (object)adaptor.GetNilNode();

            	// D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\xpath_parser\\XPath.g:184:6: ( '-' )*
            	do 
            	{
            	    int alt32 = 2;
            	    int LA32_0 = input.LA(1);

            	    if ( (LA32_0 == MINUS) )
            	    {
            	        alt32 = 1;
            	    }


            	    switch (alt32) 
            		{
            			case 1 :
            			    // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\xpath_parser\\XPath.g:184:6: '-'
            			    {
            			    	char_literal91=(IToken)Match(input,MINUS,FOLLOW_MINUS_in_unaryExprNoRoot1136); 
            			    		char_literal91_tree = (object)adaptor.Create(char_literal91);
            			    		adaptor.AddChild(root_0, char_literal91_tree);


            			    }
            			    break;

            			default:
            			    goto loop32;
            	    }
            	} while (true);

            	loop32:
            		;	// Stops C# compiler whining that label 'loop32' has no statements

            	PushFollow(FOLLOW_unionExprNoRoot_in_unaryExprNoRoot1139);
            	unionExprNoRoot92 = unionExprNoRoot();
            	state.followingStackPointer--;

            	adaptor.AddChild(root_0, unionExprNoRoot92.Tree);

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
    // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\xpath_parser\\XPath.g:187:1: qName : nCName ( ':' nCName )? ;
    public XPathParser.qName_return qName() // throws RecognitionException [1]
    {   
        XPathParser.qName_return retval = new XPathParser.qName_return();
        retval.Start = input.LT(1);

        object root_0 = null;

        IToken char_literal94 = null;
        XPathParser.nCName_return nCName93 = default(XPathParser.nCName_return);

        XPathParser.nCName_return nCName95 = default(XPathParser.nCName_return);


        object char_literal94_tree=null;

        try 
    	{
            // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\xpath_parser\\XPath.g:187:8: ( nCName ( ':' nCName )? )
            // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\xpath_parser\\XPath.g:187:11: nCName ( ':' nCName )?
            {
            	root_0 = (object)adaptor.GetNilNode();

            	PushFollow(FOLLOW_nCName_in_qName1152);
            	nCName93 = nCName();
            	state.followingStackPointer--;

            	adaptor.AddChild(root_0, nCName93.Tree);
            	// D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\xpath_parser\\XPath.g:187:18: ( ':' nCName )?
            	int alt33 = 2;
            	int LA33_0 = input.LA(1);

            	if ( (LA33_0 == COLON) )
            	{
            	    alt33 = 1;
            	}
            	switch (alt33) 
            	{
            	    case 1 :
            	        // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\xpath_parser\\XPath.g:187:19: ':' nCName
            	        {
            	        	char_literal94=(IToken)Match(input,COLON,FOLLOW_COLON_in_qName1155); 
            	        		char_literal94_tree = (object)adaptor.Create(char_literal94);
            	        		adaptor.AddChild(root_0, char_literal94_tree);

            	        	PushFollow(FOLLOW_nCName_in_qName1157);
            	        	nCName95 = nCName();
            	        	state.followingStackPointer--;

            	        	adaptor.AddChild(root_0, nCName95.Tree);

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
    // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\xpath_parser\\XPath.g:190:1: functionName : qName ;
    public XPathParser.functionName_return functionName() // throws RecognitionException [1]
    {   
        XPathParser.functionName_return retval = new XPathParser.functionName_return();
        retval.Start = input.LT(1);

        object root_0 = null;

        XPathParser.qName_return qName96 = default(XPathParser.qName_return);



        try 
    	{
            // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\xpath_parser\\XPath.g:191:3: ( qName )
            // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\xpath_parser\\XPath.g:191:6: qName
            {
            	root_0 = (object)adaptor.GetNilNode();

            	PushFollow(FOLLOW_qName_in_functionName1173);
            	qName96 = qName();
            	state.followingStackPointer--;

            	adaptor.AddChild(root_0, qName96.Tree);

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
    // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\xpath_parser\\XPath.g:194:1: variableReference : '$' qName ;
    public XPathParser.variableReference_return variableReference() // throws RecognitionException [1]
    {   
        XPathParser.variableReference_return retval = new XPathParser.variableReference_return();
        retval.Start = input.LT(1);

        object root_0 = null;

        IToken char_literal97 = null;
        XPathParser.qName_return qName98 = default(XPathParser.qName_return);


        object char_literal97_tree=null;

        try 
    	{
            // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\xpath_parser\\XPath.g:195:3: ( '$' qName )
            // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\xpath_parser\\XPath.g:195:6: '$' qName
            {
            	root_0 = (object)adaptor.GetNilNode();

            	char_literal97=(IToken)Match(input,53,FOLLOW_53_in_variableReference1190); 
            		char_literal97_tree = (object)adaptor.Create(char_literal97);
            		adaptor.AddChild(root_0, char_literal97_tree);

            	PushFollow(FOLLOW_qName_in_variableReference1192);
            	qName98 = qName();
            	state.followingStackPointer--;

            	adaptor.AddChild(root_0, qName98.Tree);

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
    // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\xpath_parser\\XPath.g:198:1: nameTest : ( '*' | nCName ':' '*' | qName );
    public XPathParser.nameTest_return nameTest() // throws RecognitionException [1]
    {   
        XPathParser.nameTest_return retval = new XPathParser.nameTest_return();
        retval.Start = input.LT(1);

        object root_0 = null;

        IToken char_literal99 = null;
        IToken char_literal101 = null;
        IToken char_literal102 = null;
        XPathParser.nCName_return nCName100 = default(XPathParser.nCName_return);

        XPathParser.qName_return qName103 = default(XPathParser.qName_return);


        object char_literal99_tree=null;
        object char_literal101_tree=null;
        object char_literal102_tree=null;

        try 
    	{
            // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\xpath_parser\\XPath.g:198:9: ( '*' | nCName ':' '*' | qName )
            int alt34 = 3;
            int LA34_0 = input.LA(1);

            if ( (LA34_0 == MUL) )
            {
                alt34 = 1;
            }
            else if ( (LA34_0 == AxisName || LA34_0 == NCName) )
            {
                int LA34_2 = input.LA(2);

                if ( (LA34_2 == COLON) )
                {
                    int LA34_3 = input.LA(3);

                    if ( (LA34_3 == MUL) )
                    {
                        alt34 = 2;
                    }
                    else if ( (LA34_3 == AxisName || LA34_3 == NCName) )
                    {
                        alt34 = 3;
                    }
                    else 
                    {
                        NoViableAltException nvae_d34s3 =
                            new NoViableAltException("", 34, 3, input);

                        throw nvae_d34s3;
                    }
                }
                else if ( (LA34_2 == EOF || (LA34_2 >= PATHSEP && LA34_2 <= ABRPATH) || (LA34_2 >= RPAR && LA34_2 <= PLUS) || LA34_2 == MUL || (LA34_2 >= COMMA && LA34_2 <= GE) || (LA34_2 >= 44 && LA34_2 <= 45) || (LA34_2 >= 47 && LA34_2 <= 52)) )
                {
                    alt34 = 3;
                }
                else 
                {
                    NoViableAltException nvae_d34s2 =
                        new NoViableAltException("", 34, 2, input);

                    throw nvae_d34s2;
                }
            }
            else 
            {
                NoViableAltException nvae_d34s0 =
                    new NoViableAltException("", 34, 0, input);

                throw nvae_d34s0;
            }
            switch (alt34) 
            {
                case 1 :
                    // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\xpath_parser\\XPath.g:198:12: '*'
                    {
                    	root_0 = (object)adaptor.GetNilNode();

                    	char_literal99=(IToken)Match(input,MUL,FOLLOW_MUL_in_nameTest1203); 
                    		char_literal99_tree = (object)adaptor.Create(char_literal99);
                    		adaptor.AddChild(root_0, char_literal99_tree);


                    }
                    break;
                case 2 :
                    // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\xpath_parser\\XPath.g:199:6: nCName ':' '*'
                    {
                    	root_0 = (object)adaptor.GetNilNode();

                    	PushFollow(FOLLOW_nCName_in_nameTest1210);
                    	nCName100 = nCName();
                    	state.followingStackPointer--;

                    	adaptor.AddChild(root_0, nCName100.Tree);
                    	char_literal101=(IToken)Match(input,COLON,FOLLOW_COLON_in_nameTest1212); 
                    		char_literal101_tree = (object)adaptor.Create(char_literal101);
                    		adaptor.AddChild(root_0, char_literal101_tree);

                    	char_literal102=(IToken)Match(input,MUL,FOLLOW_MUL_in_nameTest1214); 
                    		char_literal102_tree = (object)adaptor.Create(char_literal102);
                    		adaptor.AddChild(root_0, char_literal102_tree);


                    }
                    break;
                case 3 :
                    // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\xpath_parser\\XPath.g:200:6: qName
                    {
                    	root_0 = (object)adaptor.GetNilNode();

                    	PushFollow(FOLLOW_qName_in_nameTest1221);
                    	qName103 = qName();
                    	state.followingStackPointer--;

                    	adaptor.AddChild(root_0, qName103.Tree);

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
    // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\xpath_parser\\XPath.g:203:1: nCName : ( NCName | AxisName );
    public XPathParser.nCName_return nCName() // throws RecognitionException [1]
    {   
        XPathParser.nCName_return retval = new XPathParser.nCName_return();
        retval.Start = input.LT(1);

        object root_0 = null;

        IToken set104 = null;

        object set104_tree=null;

        try 
    	{
            // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\xpath_parser\\XPath.g:203:9: ( NCName | AxisName )
            // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\xpath_parser\\XPath.g:
            {
            	root_0 = (object)adaptor.GetNilNode();

            	set104 = (IToken)input.LT(1);
            	if ( input.LA(1) == AxisName || input.LA(1) == NCName ) 
            	{
            	    input.Consume();
            	    adaptor.AddChild(root_0, (object)adaptor.Create(set104));
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

 

    public static readonly BitSet FOLLOW_expr_in_main340 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_relativeLocationPath_in_locationPath362 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_absoluteLocationPathNoroot_in_locationPath369 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_44_in_absoluteLocationPathNoroot382 = new BitSet(new ulong[]{0x0000300000006002UL});
    public static readonly BitSet FOLLOW_traverse_in_absoluteLocationPathNoroot385 = new BitSet(new ulong[]{0x0000300000006002UL});
    public static readonly BitSet FOLLOW_45_in_absoluteLocationPathNoroot407 = new BitSet(new ulong[]{0x0000300000006002UL});
    public static readonly BitSet FOLLOW_traverse_in_absoluteLocationPathNoroot410 = new BitSet(new ulong[]{0x0000300000006002UL});
    public static readonly BitSet FOLLOW_PATHSEP_in_absoluteLocationPathNoroot435 = new BitSet(new ulong[]{0x0000409801400000UL});
    public static readonly BitSet FOLLOW_step_in_absoluteLocationPathNoroot438 = new BitSet(new ulong[]{0x0000300000006002UL});
    public static readonly BitSet FOLLOW_traverse_in_absoluteLocationPathNoroot441 = new BitSet(new ulong[]{0x0000300000006002UL});
    public static readonly BitSet FOLLOW_ABRPATH_in_absoluteLocationPathNoroot468 = new BitSet(new ulong[]{0x0000409801400000UL});
    public static readonly BitSet FOLLOW_step_in_absoluteLocationPathNoroot471 = new BitSet(new ulong[]{0x0000300000006002UL});
    public static readonly BitSet FOLLOW_traverse_in_absoluteLocationPathNoroot474 = new BitSet(new ulong[]{0x0000300000006002UL});
    public static readonly BitSet FOLLOW_DOTDOT_in_relativeLocationPath505 = new BitSet(new ulong[]{0x0000300000006002UL});
    public static readonly BitSet FOLLOW_traverse_in_relativeLocationPath508 = new BitSet(new ulong[]{0x0000300000006002UL});
    public static readonly BitSet FOLLOW_DOT_in_relativeLocationPath530 = new BitSet(new ulong[]{0x0000300000006002UL});
    public static readonly BitSet FOLLOW_traverse_in_relativeLocationPath533 = new BitSet(new ulong[]{0x0000300000006002UL});
    public static readonly BitSet FOLLOW_step_in_relativeLocationPath564 = new BitSet(new ulong[]{0x0000300000006002UL});
    public static readonly BitSet FOLLOW_traverse_in_relativeLocationPath567 = new BitSet(new ulong[]{0x0000300000006002UL});
    public static readonly BitSet FOLLOW_44_in_traverse599 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_45_in_traverse610 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_PATHSEP_in_traverse621 = new BitSet(new ulong[]{0x0000409801400000UL});
    public static readonly BitSet FOLLOW_step_in_traverse623 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_ABRPATH_in_traverse636 = new BitSet(new ulong[]{0x0000409801400000UL});
    public static readonly BitSet FOLLOW_step_in_traverse638 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_axisSpecifier_in_step660 = new BitSet(new ulong[]{0x0000409801400000UL});
    public static readonly BitSet FOLLOW_nodeTest_in_step662 = new BitSet(new ulong[]{0x0000000000020002UL});
    public static readonly BitSet FOLLOW_predicate_in_step664 = new BitSet(new ulong[]{0x0000000000020002UL});
    public static readonly BitSet FOLLOW_AxisName_in_axisSpecifier682 = new BitSet(new ulong[]{0x0000000100000000UL});
    public static readonly BitSet FOLLOW_CC_in_axisSpecifier684 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_AT_in_axisSpecifier691 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_nameTest_in_nodeTest703 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_NodeType_in_nodeTest710 = new BitSet(new ulong[]{0x0000000000008000UL});
    public static readonly BitSet FOLLOW_LPAR_in_nodeTest712 = new BitSet(new ulong[]{0x0000000000010000UL});
    public static readonly BitSet FOLLOW_RPAR_in_nodeTest714 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_46_in_nodeTest721 = new BitSet(new ulong[]{0x0000000000008000UL});
    public static readonly BitSet FOLLOW_LPAR_in_nodeTest723 = new BitSet(new ulong[]{0x0000002000000000UL});
    public static readonly BitSet FOLLOW_Literal_in_nodeTest725 = new BitSet(new ulong[]{0x0000000000010000UL});
    public static readonly BitSet FOLLOW_RPAR_in_nodeTest727 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_LBRAC_in_predicate741 = new BitSet(new ulong[]{0x002070F801E8E000UL});
    public static readonly BitSet FOLLOW_expr_in_predicate743 = new BitSet(new ulong[]{0x0000000000040000UL});
    public static readonly BitSet FOLLOW_RBRAC_in_predicate745 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_set_in_abbreviatedStep0 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_orExpr_in_expr786 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_variableReference_in_primaryExpr800 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_LPAR_in_primaryExpr807 = new BitSet(new ulong[]{0x002070F801E8E000UL});
    public static readonly BitSet FOLLOW_expr_in_primaryExpr809 = new BitSet(new ulong[]{0x0000000000010000UL});
    public static readonly BitSet FOLLOW_RPAR_in_primaryExpr811 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_Literal_in_primaryExpr818 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_Number_in_primaryExpr825 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_functionCall_in_primaryExpr834 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_functionName_in_functionCall848 = new BitSet(new ulong[]{0x0000000000008000UL});
    public static readonly BitSet FOLLOW_LPAR_in_functionCall850 = new BitSet(new ulong[]{0x002070F801E9E000UL});
    public static readonly BitSet FOLLOW_expr_in_functionCall854 = new BitSet(new ulong[]{0x0000000002010000UL});
    public static readonly BitSet FOLLOW_COMMA_in_functionCall858 = new BitSet(new ulong[]{0x002070F801E8E000UL});
    public static readonly BitSet FOLLOW_expr_in_functionCall860 = new BitSet(new ulong[]{0x0000000002010000UL});
    public static readonly BitSet FOLLOW_RPAR_in_functionCall868 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_pathExprNoRoot_in_unionExprNoRoot882 = new BitSet(new ulong[]{0x0000000004000002UL});
    public static readonly BitSet FOLLOW_PIPE_in_unionExprNoRoot885 = new BitSet(new ulong[]{0x002070F801E8E000UL});
    public static readonly BitSet FOLLOW_unionExprNoRoot_in_unionExprNoRoot888 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_PATHSEP_in_unionExprNoRoot897 = new BitSet(new ulong[]{0x0000000004000000UL});
    public static readonly BitSet FOLLOW_PIPE_in_unionExprNoRoot899 = new BitSet(new ulong[]{0x002070F801E8E000UL});
    public static readonly BitSet FOLLOW_unionExprNoRoot_in_unionExprNoRoot902 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_locationPath_in_pathExprNoRoot916 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_filterExpr_in_pathExprNoRoot923 = new BitSet(new ulong[]{0x0000000000006002UL});
    public static readonly BitSet FOLLOW_set_in_pathExprNoRoot926 = new BitSet(new ulong[]{0x0000409801E00000UL});
    public static readonly BitSet FOLLOW_relativeLocationPath_in_pathExprNoRoot932 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_primaryExpr_in_filterExpr948 = new BitSet(new ulong[]{0x0000000000020002UL});
    public static readonly BitSet FOLLOW_predicate_in_filterExpr950 = new BitSet(new ulong[]{0x0000000000020002UL});
    public static readonly BitSet FOLLOW_andExpr_in_orExpr964 = new BitSet(new ulong[]{0x0000800000000002UL});
    public static readonly BitSet FOLLOW_47_in_orExpr967 = new BitSet(new ulong[]{0x002070F801E8E000UL});
    public static readonly BitSet FOLLOW_andExpr_in_orExpr970 = new BitSet(new ulong[]{0x0000800000000002UL});
    public static readonly BitSet FOLLOW_equalityExpr_in_andExpr985 = new BitSet(new ulong[]{0x0001000000000002UL});
    public static readonly BitSet FOLLOW_48_in_andExpr988 = new BitSet(new ulong[]{0x002070F801E8E000UL});
    public static readonly BitSet FOLLOW_equalityExpr_in_andExpr991 = new BitSet(new ulong[]{0x0001000000000002UL});
    public static readonly BitSet FOLLOW_relationalExpr_in_equalityExpr1007 = new BitSet(new ulong[]{0x0006000000000002UL});
    public static readonly BitSet FOLLOW_set_in_equalityExpr1010 = new BitSet(new ulong[]{0x002070F801E8E000UL});
    public static readonly BitSet FOLLOW_relationalExpr_in_equalityExpr1017 = new BitSet(new ulong[]{0x0006000000000002UL});
    public static readonly BitSet FOLLOW_additiveExpr_in_relationalExpr1033 = new BitSet(new ulong[]{0x0000000078000002UL});
    public static readonly BitSet FOLLOW_set_in_relationalExpr1036 = new BitSet(new ulong[]{0x002070F801E8E000UL});
    public static readonly BitSet FOLLOW_additiveExpr_in_relationalExpr1047 = new BitSet(new ulong[]{0x0000000078000002UL});
    public static readonly BitSet FOLLOW_multiplicativeExpr_in_additiveExpr1063 = new BitSet(new ulong[]{0x0000000000180002UL});
    public static readonly BitSet FOLLOW_set_in_additiveExpr1066 = new BitSet(new ulong[]{0x002070F801E8E000UL});
    public static readonly BitSet FOLLOW_multiplicativeExpr_in_additiveExpr1073 = new BitSet(new ulong[]{0x0000000000180002UL});
    public static readonly BitSet FOLLOW_unaryExprNoRoot_in_multiplicativeExpr1089 = new BitSet(new ulong[]{0x0018000000400002UL});
    public static readonly BitSet FOLLOW_set_in_multiplicativeExpr1092 = new BitSet(new ulong[]{0x002070F801E8E000UL});
    public static readonly BitSet FOLLOW_multiplicativeExpr_in_multiplicativeExpr1101 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_PATHSEP_in_multiplicativeExpr1110 = new BitSet(new ulong[]{0x0018000000000002UL});
    public static readonly BitSet FOLLOW_set_in_multiplicativeExpr1113 = new BitSet(new ulong[]{0x002070F801E8E000UL});
    public static readonly BitSet FOLLOW_multiplicativeExpr_in_multiplicativeExpr1120 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_MINUS_in_unaryExprNoRoot1136 = new BitSet(new ulong[]{0x002070F801E8E000UL});
    public static readonly BitSet FOLLOW_unionExprNoRoot_in_unaryExprNoRoot1139 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_nCName_in_qName1152 = new BitSet(new ulong[]{0x0000000080000002UL});
    public static readonly BitSet FOLLOW_COLON_in_qName1155 = new BitSet(new ulong[]{0x0000008800000000UL});
    public static readonly BitSet FOLLOW_nCName_in_qName1157 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_qName_in_functionName1173 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_53_in_variableReference1190 = new BitSet(new ulong[]{0x0000008800400000UL});
    public static readonly BitSet FOLLOW_qName_in_variableReference1192 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_MUL_in_nameTest1203 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_nCName_in_nameTest1210 = new BitSet(new ulong[]{0x0000000080000000UL});
    public static readonly BitSet FOLLOW_COLON_in_nameTest1212 = new BitSet(new ulong[]{0x0000000000400000UL});
    public static readonly BitSet FOLLOW_MUL_in_nameTest1214 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_qName_in_nameTest1221 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_set_in_nCName0 = new BitSet(new ulong[]{0x0000000000000002UL});

}
