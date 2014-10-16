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



using Antlr.Runtime.Tree;

public partial class EntityGrammarParser : Parser
{
    public static readonly string[] tokenNames = new string[] 
	{
        "<invalid>", 
		"<EOR>", 
		"<DOWN>", 
		"<UP>", 
		"FUNCTION_NAME", 
		"FUNCTION", 
		"FUNCTION_PARAMS", 
		"ENTITY", 
		"CHILD", 
		"PARENT", 
		"LITERAL", 
		"INT", 
		"FLOAT", 
		"STRING", 
		"LEFT", 
		"RIGHT", 
		"ID", 
		"EXPONENT", 
		"COMMENT", 
		"WS", 
		"ESC_SEQ", 
		"HEX_DIGIT", 
		"UNICODE_ESC", 
		"OCTAL_ESC", 
		"'ENTITY.'", 
		"'CHILD.'", 
		"'PARENT.'", 
		"'('", 
		"')'", 
		"','", 
		"'='"
    };

    public const int FUNCTION = 5;
    public const int CHILD = 8;
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
    public const int ESC_SEQ = 20;
    public const int WS = 19;
    public const int LEFT = 14;
    public const int FUNCTION_NAME = 4;
    public const int COMMENT = 18;
    public const int FUNCTION_PARAMS = 6;
    public const int STRING = 13;

    // delegates
    // delegators



        public EntityGrammarParser(ITokenStream input)
    		: this(input, new RecognizerSharedState()) {
        }

        public EntityGrammarParser(ITokenStream input, RecognizerSharedState state)
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
		get { return EntityGrammarParser.tokenNames; }
    }

    override public string GrammarFileName {
		get { return "D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\entity_parser\\EntityGrammar.g"; }
    }


    public class start_return : ParserRuleReturnScope
    {
        private object tree;
        override public object Tree
        {
        	get { return tree; }
        	set { tree = (object) value; }
        }
    };

    // $ANTLR start "start"
    // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\entity_parser\\EntityGrammar.g:44:1: start : assignment ;
    public EntityGrammarParser.start_return start() // throws RecognitionException [1]
    {   
        EntityGrammarParser.start_return retval = new EntityGrammarParser.start_return();
        retval.Start = input.LT(1);

        object root_0 = null;

        EntityGrammarParser.assignment_return assignment1 = default(EntityGrammarParser.assignment_return);



        try 
    	{
            // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\entity_parser\\EntityGrammar.g:45:2: ( assignment )
            // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\entity_parser\\EntityGrammar.g:46:2: assignment
            {
            	root_0 = (object)adaptor.GetNilNode();

            	PushFollow(FOLLOW_assignment_in_start106);
            	assignment1 = assignment();
            	state.followingStackPointer--;

            	adaptor.AddChild(root_0, assignment1.Tree);

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
    // $ANTLR end "start"

    public class literal_return : ParserRuleReturnScope
    {
        private object tree;
        override public object Tree
        {
        	get { return tree; }
        	set { tree = (object) value; }
        }
    };

    // $ANTLR start "literal"
    // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\entity_parser\\EntityGrammar.g:48:1: literal : ( INT -> ^( LITERAL INT ^( INT ) ) | FLOAT -> ^( LITERAL FLOAT ^( FLOAT ) ) | STRING -> ^( LITERAL STRING ^( STRING ) ) );
    public EntityGrammarParser.literal_return literal() // throws RecognitionException [1]
    {   
        EntityGrammarParser.literal_return retval = new EntityGrammarParser.literal_return();
        retval.Start = input.LT(1);

        object root_0 = null;

        IToken INT2 = null;
        IToken FLOAT3 = null;
        IToken STRING4 = null;

        object INT2_tree=null;
        object FLOAT3_tree=null;
        object STRING4_tree=null;
        RewriteRuleTokenStream stream_FLOAT = new RewriteRuleTokenStream(adaptor,"token FLOAT");
        RewriteRuleTokenStream stream_INT = new RewriteRuleTokenStream(adaptor,"token INT");
        RewriteRuleTokenStream stream_STRING = new RewriteRuleTokenStream(adaptor,"token STRING");

        try 
    	{
            // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\entity_parser\\EntityGrammar.g:49:2: ( INT -> ^( LITERAL INT ^( INT ) ) | FLOAT -> ^( LITERAL FLOAT ^( FLOAT ) ) | STRING -> ^( LITERAL STRING ^( STRING ) ) )
            int alt1 = 3;
            switch ( input.LA(1) ) 
            {
            case INT:
            	{
                alt1 = 1;
                }
                break;
            case FLOAT:
            	{
                alt1 = 2;
                }
                break;
            case STRING:
            	{
                alt1 = 3;
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
                    // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\entity_parser\\EntityGrammar.g:49:4: INT
                    {
                    	INT2=(IToken)Match(input,INT,FOLLOW_INT_in_literal119);  
                    	stream_INT.Add(INT2);



                    	// AST REWRITE
                    	// elements:          INT, INT
                    	// token labels:      
                    	// rule labels:       retval
                    	// token list labels: 
                    	// rule list labels:  
                    	// wildcard labels: 
                    	retval.Tree = root_0;
                    	RewriteRuleSubtreeStream stream_retval = new RewriteRuleSubtreeStream(adaptor, "rule retval", retval!=null ? retval.Tree : null);

                    	root_0 = (object)adaptor.GetNilNode();
                    	// 49:8: -> ^( LITERAL INT ^( INT ) )
                    	{
                    	    // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\entity_parser\\EntityGrammar.g:49:11: ^( LITERAL INT ^( INT ) )
                    	    {
                    	    object root_1 = (object)adaptor.GetNilNode();
                    	    root_1 = (object)adaptor.BecomeRoot((object)adaptor.Create(LITERAL, "LITERAL"), root_1);

                    	    adaptor.AddChild(root_1, stream_INT.NextNode());
                    	    // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\entity_parser\\EntityGrammar.g:49:25: ^( INT )
                    	    {
                    	    object root_2 = (object)adaptor.GetNilNode();
                    	    root_2 = (object)adaptor.BecomeRoot(stream_INT.NextNode(), root_2);

                    	    adaptor.AddChild(root_1, root_2);
                    	    }

                    	    adaptor.AddChild(root_0, root_1);
                    	    }

                    	}

                    	retval.Tree = root_0;retval.Tree = root_0;
                    }
                    break;
                case 2 :
                    // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\entity_parser\\EntityGrammar.g:50:4: FLOAT
                    {
                    	FLOAT3=(IToken)Match(input,FLOAT,FOLLOW_FLOAT_in_literal136);  
                    	stream_FLOAT.Add(FLOAT3);



                    	// AST REWRITE
                    	// elements:          FLOAT, FLOAT
                    	// token labels:      
                    	// rule labels:       retval
                    	// token list labels: 
                    	// rule list labels:  
                    	// wildcard labels: 
                    	retval.Tree = root_0;
                    	RewriteRuleSubtreeStream stream_retval = new RewriteRuleSubtreeStream(adaptor, "rule retval", retval!=null ? retval.Tree : null);

                    	root_0 = (object)adaptor.GetNilNode();
                    	// 50:10: -> ^( LITERAL FLOAT ^( FLOAT ) )
                    	{
                    	    // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\entity_parser\\EntityGrammar.g:50:13: ^( LITERAL FLOAT ^( FLOAT ) )
                    	    {
                    	    object root_1 = (object)adaptor.GetNilNode();
                    	    root_1 = (object)adaptor.BecomeRoot((object)adaptor.Create(LITERAL, "LITERAL"), root_1);

                    	    adaptor.AddChild(root_1, stream_FLOAT.NextNode());
                    	    // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\entity_parser\\EntityGrammar.g:50:29: ^( FLOAT )
                    	    {
                    	    object root_2 = (object)adaptor.GetNilNode();
                    	    root_2 = (object)adaptor.BecomeRoot(stream_FLOAT.NextNode(), root_2);

                    	    adaptor.AddChild(root_1, root_2);
                    	    }

                    	    adaptor.AddChild(root_0, root_1);
                    	    }

                    	}

                    	retval.Tree = root_0;retval.Tree = root_0;
                    }
                    break;
                case 3 :
                    // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\entity_parser\\EntityGrammar.g:51:4: STRING
                    {
                    	STRING4=(IToken)Match(input,STRING,FOLLOW_STRING_in_literal153);  
                    	stream_STRING.Add(STRING4);



                    	// AST REWRITE
                    	// elements:          STRING, STRING
                    	// token labels:      
                    	// rule labels:       retval
                    	// token list labels: 
                    	// rule list labels:  
                    	// wildcard labels: 
                    	retval.Tree = root_0;
                    	RewriteRuleSubtreeStream stream_retval = new RewriteRuleSubtreeStream(adaptor, "rule retval", retval!=null ? retval.Tree : null);

                    	root_0 = (object)adaptor.GetNilNode();
                    	// 51:11: -> ^( LITERAL STRING ^( STRING ) )
                    	{
                    	    // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\entity_parser\\EntityGrammar.g:51:14: ^( LITERAL STRING ^( STRING ) )
                    	    {
                    	    object root_1 = (object)adaptor.GetNilNode();
                    	    root_1 = (object)adaptor.BecomeRoot((object)adaptor.Create(LITERAL, "LITERAL"), root_1);

                    	    adaptor.AddChild(root_1, stream_STRING.NextNode());
                    	    // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\entity_parser\\EntityGrammar.g:51:31: ^( STRING )
                    	    {
                    	    object root_2 = (object)adaptor.GetNilNode();
                    	    root_2 = (object)adaptor.BecomeRoot(stream_STRING.NextNode(), root_2);

                    	    adaptor.AddChild(root_1, root_2);
                    	    }

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
    // $ANTLR end "literal"

    public class variable_return : ParserRuleReturnScope
    {
        private object tree;
        override public object Tree
        {
        	get { return tree; }
        	set { tree = (object) value; }
        }
    };

    // $ANTLR start "variable"
    // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\entity_parser\\EntityGrammar.g:53:1: variable : ( 'ENTITY.' ID -> ^( ENTITY ID ) | 'CHILD.' ID -> ^( CHILD ID ) | 'PARENT.' ID -> ^( PARENT ID ) );
    public EntityGrammarParser.variable_return variable() // throws RecognitionException [1]
    {   
        EntityGrammarParser.variable_return retval = new EntityGrammarParser.variable_return();
        retval.Start = input.LT(1);

        object root_0 = null;

        IToken string_literal5 = null;
        IToken ID6 = null;
        IToken string_literal7 = null;
        IToken ID8 = null;
        IToken string_literal9 = null;
        IToken ID10 = null;

        object string_literal5_tree=null;
        object ID6_tree=null;
        object string_literal7_tree=null;
        object ID8_tree=null;
        object string_literal9_tree=null;
        object ID10_tree=null;
        RewriteRuleTokenStream stream_ID = new RewriteRuleTokenStream(adaptor,"token ID");
        RewriteRuleTokenStream stream_24 = new RewriteRuleTokenStream(adaptor,"token 24");
        RewriteRuleTokenStream stream_25 = new RewriteRuleTokenStream(adaptor,"token 25");
        RewriteRuleTokenStream stream_26 = new RewriteRuleTokenStream(adaptor,"token 26");

        try 
    	{
            // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\entity_parser\\EntityGrammar.g:54:2: ( 'ENTITY.' ID -> ^( ENTITY ID ) | 'CHILD.' ID -> ^( CHILD ID ) | 'PARENT.' ID -> ^( PARENT ID ) )
            int alt2 = 3;
            switch ( input.LA(1) ) 
            {
            case 24:
            	{
                alt2 = 1;
                }
                break;
            case 25:
            	{
                alt2 = 2;
                }
                break;
            case 26:
            	{
                alt2 = 3;
                }
                break;
            	default:
            	    NoViableAltException nvae_d2s0 =
            	        new NoViableAltException("", 2, 0, input);

            	    throw nvae_d2s0;
            }

            switch (alt2) 
            {
                case 1 :
                    // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\entity_parser\\EntityGrammar.g:54:4: 'ENTITY.' ID
                    {
                    	string_literal5=(IToken)Match(input,24,FOLLOW_24_in_variable175);  
                    	stream_24.Add(string_literal5);

                    	ID6=(IToken)Match(input,ID,FOLLOW_ID_in_variable177);  
                    	stream_ID.Add(ID6);



                    	// AST REWRITE
                    	// elements:          ID
                    	// token labels:      
                    	// rule labels:       retval
                    	// token list labels: 
                    	// rule list labels:  
                    	// wildcard labels: 
                    	retval.Tree = root_0;
                    	RewriteRuleSubtreeStream stream_retval = new RewriteRuleSubtreeStream(adaptor, "rule retval", retval!=null ? retval.Tree : null);

                    	root_0 = (object)adaptor.GetNilNode();
                    	// 54:18: -> ^( ENTITY ID )
                    	{
                    	    // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\entity_parser\\EntityGrammar.g:54:21: ^( ENTITY ID )
                    	    {
                    	    object root_1 = (object)adaptor.GetNilNode();
                    	    root_1 = (object)adaptor.BecomeRoot((object)adaptor.Create(ENTITY, "ENTITY"), root_1);

                    	    adaptor.AddChild(root_1, stream_ID.NextNode());

                    	    adaptor.AddChild(root_0, root_1);
                    	    }

                    	}

                    	retval.Tree = root_0;retval.Tree = root_0;
                    }
                    break;
                case 2 :
                    // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\entity_parser\\EntityGrammar.g:55:4: 'CHILD.' ID
                    {
                    	string_literal7=(IToken)Match(input,25,FOLLOW_25_in_variable191);  
                    	stream_25.Add(string_literal7);

                    	ID8=(IToken)Match(input,ID,FOLLOW_ID_in_variable193);  
                    	stream_ID.Add(ID8);



                    	// AST REWRITE
                    	// elements:          ID
                    	// token labels:      
                    	// rule labels:       retval
                    	// token list labels: 
                    	// rule list labels:  
                    	// wildcard labels: 
                    	retval.Tree = root_0;
                    	RewriteRuleSubtreeStream stream_retval = new RewriteRuleSubtreeStream(adaptor, "rule retval", retval!=null ? retval.Tree : null);

                    	root_0 = (object)adaptor.GetNilNode();
                    	// 55:16: -> ^( CHILD ID )
                    	{
                    	    // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\entity_parser\\EntityGrammar.g:55:19: ^( CHILD ID )
                    	    {
                    	    object root_1 = (object)adaptor.GetNilNode();
                    	    root_1 = (object)adaptor.BecomeRoot((object)adaptor.Create(CHILD, "CHILD"), root_1);

                    	    adaptor.AddChild(root_1, stream_ID.NextNode());

                    	    adaptor.AddChild(root_0, root_1);
                    	    }

                    	}

                    	retval.Tree = root_0;retval.Tree = root_0;
                    }
                    break;
                case 3 :
                    // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\entity_parser\\EntityGrammar.g:56:4: 'PARENT.' ID
                    {
                    	string_literal9=(IToken)Match(input,26,FOLLOW_26_in_variable206);  
                    	stream_26.Add(string_literal9);

                    	ID10=(IToken)Match(input,ID,FOLLOW_ID_in_variable208);  
                    	stream_ID.Add(ID10);



                    	// AST REWRITE
                    	// elements:          ID
                    	// token labels:      
                    	// rule labels:       retval
                    	// token list labels: 
                    	// rule list labels:  
                    	// wildcard labels: 
                    	retval.Tree = root_0;
                    	RewriteRuleSubtreeStream stream_retval = new RewriteRuleSubtreeStream(adaptor, "rule retval", retval!=null ? retval.Tree : null);

                    	root_0 = (object)adaptor.GetNilNode();
                    	// 56:18: -> ^( PARENT ID )
                    	{
                    	    // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\entity_parser\\EntityGrammar.g:56:21: ^( PARENT ID )
                    	    {
                    	    object root_1 = (object)adaptor.GetNilNode();
                    	    root_1 = (object)adaptor.BecomeRoot((object)adaptor.Create(PARENT, "PARENT"), root_1);

                    	    adaptor.AddChild(root_1, stream_ID.NextNode());

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
    // $ANTLR end "variable"

    public class function_return : ParserRuleReturnScope
    {
        private object tree;
        override public object Tree
        {
        	get { return tree; }
        	set { tree = (object) value; }
        }
    };

    // $ANTLR start "function"
    // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\entity_parser\\EntityGrammar.g:58:1: function : ID '(' atom_list ')' -> ^( FUNCTION ^( FUNCTION_NAME ID ) ^( FUNCTION_PARAMS atom_list ) ) ;
    public EntityGrammarParser.function_return function() // throws RecognitionException [1]
    {   
        EntityGrammarParser.function_return retval = new EntityGrammarParser.function_return();
        retval.Start = input.LT(1);

        object root_0 = null;

        IToken ID11 = null;
        IToken char_literal12 = null;
        IToken char_literal14 = null;
        EntityGrammarParser.atom_list_return atom_list13 = default(EntityGrammarParser.atom_list_return);


        object ID11_tree=null;
        object char_literal12_tree=null;
        object char_literal14_tree=null;
        RewriteRuleTokenStream stream_ID = new RewriteRuleTokenStream(adaptor,"token ID");
        RewriteRuleTokenStream stream_27 = new RewriteRuleTokenStream(adaptor,"token 27");
        RewriteRuleTokenStream stream_28 = new RewriteRuleTokenStream(adaptor,"token 28");
        RewriteRuleSubtreeStream stream_atom_list = new RewriteRuleSubtreeStream(adaptor,"rule atom_list");
        try 
    	{
            // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\entity_parser\\EntityGrammar.g:59:2: ( ID '(' atom_list ')' -> ^( FUNCTION ^( FUNCTION_NAME ID ) ^( FUNCTION_PARAMS atom_list ) ) )
            // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\entity_parser\\EntityGrammar.g:59:4: ID '(' atom_list ')'
            {
            	ID11=(IToken)Match(input,ID,FOLLOW_ID_in_function227);  
            	stream_ID.Add(ID11);

            	char_literal12=(IToken)Match(input,27,FOLLOW_27_in_function228);  
            	stream_27.Add(char_literal12);

            	PushFollow(FOLLOW_atom_list_in_function229);
            	atom_list13 = atom_list();
            	state.followingStackPointer--;

            	stream_atom_list.Add(atom_list13.Tree);
            	char_literal14=(IToken)Match(input,28,FOLLOW_28_in_function230);  
            	stream_28.Add(char_literal14);



            	// AST REWRITE
            	// elements:          ID, atom_list
            	// token labels:      
            	// rule labels:       retval
            	// token list labels: 
            	// rule list labels:  
            	// wildcard labels: 
            	retval.Tree = root_0;
            	RewriteRuleSubtreeStream stream_retval = new RewriteRuleSubtreeStream(adaptor, "rule retval", retval!=null ? retval.Tree : null);

            	root_0 = (object)adaptor.GetNilNode();
            	// 59:22: -> ^( FUNCTION ^( FUNCTION_NAME ID ) ^( FUNCTION_PARAMS atom_list ) )
            	{
            	    // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\entity_parser\\EntityGrammar.g:59:25: ^( FUNCTION ^( FUNCTION_NAME ID ) ^( FUNCTION_PARAMS atom_list ) )
            	    {
            	    object root_1 = (object)adaptor.GetNilNode();
            	    root_1 = (object)adaptor.BecomeRoot((object)adaptor.Create(FUNCTION, "FUNCTION"), root_1);

            	    // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\entity_parser\\EntityGrammar.g:59:36: ^( FUNCTION_NAME ID )
            	    {
            	    object root_2 = (object)adaptor.GetNilNode();
            	    root_2 = (object)adaptor.BecomeRoot((object)adaptor.Create(FUNCTION_NAME, "FUNCTION_NAME"), root_2);

            	    adaptor.AddChild(root_2, stream_ID.NextNode());

            	    adaptor.AddChild(root_1, root_2);
            	    }
            	    // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\entity_parser\\EntityGrammar.g:59:56: ^( FUNCTION_PARAMS atom_list )
            	    {
            	    object root_2 = (object)adaptor.GetNilNode();
            	    root_2 = (object)adaptor.BecomeRoot((object)adaptor.Create(FUNCTION_PARAMS, "FUNCTION_PARAMS"), root_2);

            	    adaptor.AddChild(root_2, stream_atom_list.NextTree());

            	    adaptor.AddChild(root_1, root_2);
            	    }

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
    // $ANTLR end "function"

    public class atom_return : ParserRuleReturnScope
    {
        private object tree;
        override public object Tree
        {
        	get { return tree; }
        	set { tree = (object) value; }
        }
    };

    // $ANTLR start "atom"
    // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\entity_parser\\EntityGrammar.g:61:1: atom : ( literal | variable | function );
    public EntityGrammarParser.atom_return atom() // throws RecognitionException [1]
    {   
        EntityGrammarParser.atom_return retval = new EntityGrammarParser.atom_return();
        retval.Start = input.LT(1);

        object root_0 = null;

        EntityGrammarParser.literal_return literal15 = default(EntityGrammarParser.literal_return);

        EntityGrammarParser.variable_return variable16 = default(EntityGrammarParser.variable_return);

        EntityGrammarParser.function_return function17 = default(EntityGrammarParser.function_return);



        try 
    	{
            // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\entity_parser\\EntityGrammar.g:62:2: ( literal | variable | function )
            int alt3 = 3;
            switch ( input.LA(1) ) 
            {
            case INT:
            case FLOAT:
            case STRING:
            	{
                alt3 = 1;
                }
                break;
            case 24:
            case 25:
            case 26:
            	{
                alt3 = 2;
                }
                break;
            case ID:
            	{
                alt3 = 3;
                }
                break;
            	default:
            	    NoViableAltException nvae_d3s0 =
            	        new NoViableAltException("", 3, 0, input);

            	    throw nvae_d3s0;
            }

            switch (alt3) 
            {
                case 1 :
                    // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\entity_parser\\EntityGrammar.g:62:4: literal
                    {
                    	root_0 = (object)adaptor.GetNilNode();

                    	PushFollow(FOLLOW_literal_in_atom259);
                    	literal15 = literal();
                    	state.followingStackPointer--;

                    	adaptor.AddChild(root_0, literal15.Tree);

                    }
                    break;
                case 2 :
                    // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\entity_parser\\EntityGrammar.g:63:4: variable
                    {
                    	root_0 = (object)adaptor.GetNilNode();

                    	PushFollow(FOLLOW_variable_in_atom264);
                    	variable16 = variable();
                    	state.followingStackPointer--;

                    	adaptor.AddChild(root_0, variable16.Tree);

                    }
                    break;
                case 3 :
                    // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\entity_parser\\EntityGrammar.g:64:4: function
                    {
                    	root_0 = (object)adaptor.GetNilNode();

                    	PushFollow(FOLLOW_function_in_atom269);
                    	function17 = function();
                    	state.followingStackPointer--;

                    	adaptor.AddChild(root_0, function17.Tree);

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
    // $ANTLR end "atom"

    public class atom_list_return : ParserRuleReturnScope
    {
        private object tree;
        override public object Tree
        {
        	get { return tree; }
        	set { tree = (object) value; }
        }
    };

    // $ANTLR start "atom_list"
    // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\entity_parser\\EntityGrammar.g:66:1: atom_list : atom ( ',' atom )* ;
    public EntityGrammarParser.atom_list_return atom_list() // throws RecognitionException [1]
    {   
        EntityGrammarParser.atom_list_return retval = new EntityGrammarParser.atom_list_return();
        retval.Start = input.LT(1);

        object root_0 = null;

        IToken char_literal19 = null;
        EntityGrammarParser.atom_return atom18 = default(EntityGrammarParser.atom_return);

        EntityGrammarParser.atom_return atom20 = default(EntityGrammarParser.atom_return);


        object char_literal19_tree=null;

        try 
    	{
            // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\entity_parser\\EntityGrammar.g:67:2: ( atom ( ',' atom )* )
            // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\entity_parser\\EntityGrammar.g:67:4: atom ( ',' atom )*
            {
            	root_0 = (object)adaptor.GetNilNode();

            	PushFollow(FOLLOW_atom_in_atom_list280);
            	atom18 = atom();
            	state.followingStackPointer--;

            	adaptor.AddChild(root_0, atom18.Tree);
            	// D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\entity_parser\\EntityGrammar.g:67:9: ( ',' atom )*
            	do 
            	{
            	    int alt4 = 2;
            	    int LA4_0 = input.LA(1);

            	    if ( (LA4_0 == 29) )
            	    {
            	        alt4 = 1;
            	    }


            	    switch (alt4) 
            		{
            			case 1 :
            			    // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\entity_parser\\EntityGrammar.g:67:10: ',' atom
            			    {
            			    	char_literal19=(IToken)Match(input,29,FOLLOW_29_in_atom_list283); 
            			    	PushFollow(FOLLOW_atom_in_atom_list286);
            			    	atom20 = atom();
            			    	state.followingStackPointer--;

            			    	adaptor.AddChild(root_0, atom20.Tree);

            			    }
            			    break;

            			default:
            			    goto loop4;
            	    }
            	} while (true);

            	loop4:
            		;	// Stops C# compiler whining that label 'loop4' has no statements


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
    // $ANTLR end "atom_list"

    public class assignment_return : ParserRuleReturnScope
    {
        private object tree;
        override public object Tree
        {
        	get { return tree; }
        	set { tree = (object) value; }
        }
    };

    // $ANTLR start "assignment"
    // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\entity_parser\\EntityGrammar.g:70:1: assignment : atom_list '=' atom -> ^( LEFT atom_list ) ^( RIGHT atom ) ;
    public EntityGrammarParser.assignment_return assignment() // throws RecognitionException [1]
    {   
        EntityGrammarParser.assignment_return retval = new EntityGrammarParser.assignment_return();
        retval.Start = input.LT(1);

        object root_0 = null;

        IToken char_literal22 = null;
        EntityGrammarParser.atom_list_return atom_list21 = default(EntityGrammarParser.atom_list_return);

        EntityGrammarParser.atom_return atom23 = default(EntityGrammarParser.atom_return);


        object char_literal22_tree=null;
        RewriteRuleTokenStream stream_30 = new RewriteRuleTokenStream(adaptor,"token 30");
        RewriteRuleSubtreeStream stream_atom = new RewriteRuleSubtreeStream(adaptor,"rule atom");
        RewriteRuleSubtreeStream stream_atom_list = new RewriteRuleSubtreeStream(adaptor,"rule atom_list");
        try 
    	{
            // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\entity_parser\\EntityGrammar.g:71:2: ( atom_list '=' atom -> ^( LEFT atom_list ) ^( RIGHT atom ) )
            // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\entity_parser\\EntityGrammar.g:71:4: atom_list '=' atom
            {
            	PushFollow(FOLLOW_atom_list_in_assignment302);
            	atom_list21 = atom_list();
            	state.followingStackPointer--;

            	stream_atom_list.Add(atom_list21.Tree);
            	char_literal22=(IToken)Match(input,30,FOLLOW_30_in_assignment304);  
            	stream_30.Add(char_literal22);

            	PushFollow(FOLLOW_atom_in_assignment306);
            	atom23 = atom();
            	state.followingStackPointer--;

            	stream_atom.Add(atom23.Tree);


            	// AST REWRITE
            	// elements:          atom, atom_list
            	// token labels:      
            	// rule labels:       retval
            	// token list labels: 
            	// rule list labels:  
            	// wildcard labels: 
            	retval.Tree = root_0;
            	RewriteRuleSubtreeStream stream_retval = new RewriteRuleSubtreeStream(adaptor, "rule retval", retval!=null ? retval.Tree : null);

            	root_0 = (object)adaptor.GetNilNode();
            	// 71:23: -> ^( LEFT atom_list ) ^( RIGHT atom )
            	{
            	    // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\entity_parser\\EntityGrammar.g:71:26: ^( LEFT atom_list )
            	    {
            	    object root_1 = (object)adaptor.GetNilNode();
            	    root_1 = (object)adaptor.BecomeRoot((object)adaptor.Create(LEFT, "LEFT"), root_1);

            	    adaptor.AddChild(root_1, stream_atom_list.NextTree());

            	    adaptor.AddChild(root_0, root_1);
            	    }
            	    // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\entity_parser\\EntityGrammar.g:71:44: ^( RIGHT atom )
            	    {
            	    object root_1 = (object)adaptor.GetNilNode();
            	    root_1 = (object)adaptor.BecomeRoot((object)adaptor.Create(RIGHT, "RIGHT"), root_1);

            	    adaptor.AddChild(root_1, stream_atom.NextTree());

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
    // $ANTLR end "assignment"

    // Delegated rules


	private void InitializeCyclicDFAs()
	{
	}

 

    public static readonly BitSet FOLLOW_assignment_in_start106 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_INT_in_literal119 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_FLOAT_in_literal136 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_STRING_in_literal153 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_24_in_variable175 = new BitSet(new ulong[]{0x0000000000010000UL});
    public static readonly BitSet FOLLOW_ID_in_variable177 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_25_in_variable191 = new BitSet(new ulong[]{0x0000000000010000UL});
    public static readonly BitSet FOLLOW_ID_in_variable193 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_26_in_variable206 = new BitSet(new ulong[]{0x0000000000010000UL});
    public static readonly BitSet FOLLOW_ID_in_variable208 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_ID_in_function227 = new BitSet(new ulong[]{0x0000000008000000UL});
    public static readonly BitSet FOLLOW_27_in_function228 = new BitSet(new ulong[]{0x0000000007013800UL});
    public static readonly BitSet FOLLOW_atom_list_in_function229 = new BitSet(new ulong[]{0x0000000010000000UL});
    public static readonly BitSet FOLLOW_28_in_function230 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_literal_in_atom259 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_variable_in_atom264 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_function_in_atom269 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_atom_in_atom_list280 = new BitSet(new ulong[]{0x0000000020000002UL});
    public static readonly BitSet FOLLOW_29_in_atom_list283 = new BitSet(new ulong[]{0x0000000007013800UL});
    public static readonly BitSet FOLLOW_atom_in_atom_list286 = new BitSet(new ulong[]{0x0000000020000002UL});
    public static readonly BitSet FOLLOW_atom_list_in_assignment302 = new BitSet(new ulong[]{0x0000000040000000UL});
    public static readonly BitSet FOLLOW_30_in_assignment304 = new BitSet(new ulong[]{0x0000000007013800UL});
    public static readonly BitSet FOLLOW_atom_in_assignment306 = new BitSet(new ulong[]{0x0000000000000002UL});

}
