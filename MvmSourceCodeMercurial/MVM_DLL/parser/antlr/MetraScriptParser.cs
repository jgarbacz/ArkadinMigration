// $ANTLR 3.2 Sep 23, 2009 12:02:23 C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g 2012-03-14 17:58:06

// The variable 'variable' is assigned but its value is never used.
#pragma warning disable 168, 219
// Unreachable code detected.
#pragma warning disable 162


using ParserExensionsNameSpace;


using System;
using Antlr.Runtime;
using IList 		= System.Collections.IList;
using ArrayList 	= System.Collections.ArrayList;
using Stack 		= Antlr.Runtime.Collections.StackList;

using IDictionary	= System.Collections.IDictionary;
using Hashtable 	= System.Collections.Hashtable;

using Antlr.Runtime.Tree;

public partial class MetraScriptParser : Parser
{
    public static readonly string[] tokenNames = new string[] 
	{
        "<invalid>", 
		"<EOR>", 
		"<DOWN>", 
		"<UP>", 
		"INT", 
		"FLOAT", 
		"STRING", 
		"BOOL", 
		"NULL", 
		"CURRENT_OBJECT", 
		"OBJECT", 
		"TEMP", 
		"GLOBAL", 
		"PROC", 
		"THREAD", 
		"FUNCTION", 
		"ARGUMENT", 
		"NAMED_ARGUMENT", 
		"Id", 
		"FloatingPointLiteral", 
		"StringLiteral", 
		"HexLiteral", 
		"OctalLiteral", 
		"DecimalLiteral", 
		"HexDigit", 
		"IntegerTypeSuffix", 
		"Exponent", 
		"FloatTypeSuffix", 
		"WS", 
		"COMMENT", 
		"LINE_COMMENT", 
		"'OBJECT.'", 
		"'OBJECT('", 
		"').'", 
		"'TEMP.'", 
		"'GLOBAL.'", 
		"'THREAD.'", 
		"'PROC.'", 
		"'null'", 
		"'true'", 
		"'false'", 
		"'='", 
		"'~'", 
		"'+='", 
		"'-='", 
		"'*='", 
		"'/='", 
		"'&='", 
		"'|='", 
		"'^='", 
		"'%='", 
		"'~='", 
		"'<'", 
		"'>'", 
		"'?'", 
		"':'", 
		"'||'", 
		"'or'", 
		"'OR'", 
		"'&&'", 
		"'and'", 
		"'AND'", 
		"'|'", 
		"'^'", 
		"'&'", 
		"'=='", 
		"'!='", 
		"'eq'", 
		"'ne'", 
		"'Eq'", 
		"'Ne'", 
		"'EQ'", 
		"'NE'", 
		"'eqEQ'", 
		"'EqEQ'", 
		"'neNE'", 
		"'NeNE'", 
		"'gt'", 
		"'lt'", 
		"'gte'", 
		"'lte'", 
		"'Gt'", 
		"'Lt'", 
		"'Gte'", 
		"'Lte'", 
		"'GT'", 
		"'LT'", 
		"'GTE'", 
		"'LTE'", 
		"'+'", 
		"'-'", 
		"'*'", 
		"'/'", 
		"'%'", 
		"'++'", 
		"'--'", 
		"'!'", 
		"'('", 
		"')'", 
		"'=>'", 
		"','"
    };

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
    public const int WS = 28;
    public const int T__71 = 71;
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

    // delegates
    // delegators



        public MetraScriptParser(ITokenStream input)
    		: this(input, new RecognizerSharedState()) {
        }

        public MetraScriptParser(ITokenStream input, RecognizerSharedState state)
    		: base(input, state) {
            InitializeCyclicDFAs();
            this.state.ruleMemo = new Hashtable[120+1];
             
             
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
		get { return MetraScriptParser.tokenNames; }
    }

    override public string GrammarFileName {
		get { return "C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g"; }
    }


    	Stack paraphrases = new Stack();
    	public void PushPassphrase(String phrase){
    		paraphrases.Push(phrase);
    	}
    	public void PopPassphrase(){
    		paraphrases.Pop();
    	}
       override public String GetErrorMessage(RecognitionException e, String[] tokenNames)
            {
                var stack = GetRuleInvocationStack(e, this.GetType().FullName);
                //foreach (var x in stack) Console.WriteLine(x);
                var antlrMsg = base.GetErrorMessage(e, tokenNames);
                string paraphrase=" ";
                if (paraphrases.Count > 0)
                {
                    paraphrase += (string)paraphrases.Peek();
                }
                if (e is MismatchedTokenException)
                {
                    var mte = e as MismatchedTokenException;
                    if (e.Line > 0)
                    {
                        antlrMsg = "Expecting " + tokenNames[mte.Expecting] + paraphrase +" but found '" + mte.Token.Text + "' at line="+e.Line+", position="+(e.CharPositionInLine+1);
                    }
                    else
                    {
                        antlrMsg = "Expecting " + tokenNames[mte.Expecting] + paraphrase + " but reached end-of-file";
                    }
                }
                else if (e is NoViableAltException)
                {
                    var nvae = e as NoViableAltException;
                    if (e.Line > 0)
                    {
                        antlrMsg = "Not expecting '" + nvae.Token.Text + "' " + paraphrase + " at line=" + e.Line + ", position=" + (e.CharPositionInLine + 1);
                    }
                    else
                    {
                        if (stack.Count == 1 && stack[0].ToString().Equals("terminator"))
                        {
                            antlrMsg = "Reached end-of-file, are you missing a final semicolon?";
                        }
                    }
                }
                else
                {
                    antlrMsg += paraphrase + " at line=" + e.Line + ", position=" + (e.CharPositionInLine + 1);
                }
                throw new MvmScript.MvmScriptRecognitionException(e, stack, antlrMsg);
            }

        override public String GetTokenErrorDisplay(IToken t)
        {
            return t.ToString();
        }
        
        override protected void Mismatch(IIntStream input, int ttype, BitSet follow)
        {
            throw new MismatchedTokenException(ttype, input);
        }
       
        override public object RecoverFromMismatchedSet(IIntStream input, RecognitionException e, BitSet follow)
        {
            throw e;
        }
        public bool TruePrint(string msg){
        	Console.WriteLine(msg);
        	return true;
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
    // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:158:1: start : expression ;
    public MetraScriptParser.start_return start() // throws RecognitionException [1]
    {   
        MetraScriptParser.start_return retval = new MetraScriptParser.start_return();
        retval.Start = input.LT(1);
        int start_StartIndex = input.Index();
        object root_0 = null;

        MetraScriptParser.expression_return expression1 = default(MetraScriptParser.expression_return);



        try 
    	{
    	    if ( (state.backtracking > 0) && AlreadyParsedRule(input, 1) ) 
    	    {
    	    	return retval; 
    	    }
            // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:158:7: ( expression )
            // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:159:2: expression
            {
            	root_0 = (object)adaptor.GetNilNode();

            	PushFollow(FOLLOW_expression_in_start147);
            	expression1 = expression();
            	state.followingStackPointer--;
            	if (state.failed) return retval;
            	if ( state.backtracking == 0 ) adaptor.AddChild(root_0, expression1.Tree);

            }

            retval.Stop = input.LT(-1);

            if ( (state.backtracking==0) )
            {	retval.Tree = (object)adaptor.RulePostProcessing(root_0);
            	adaptor.SetTokenBoundaries(retval.Tree, (IToken) retval.Start, (IToken) retval.Stop);}
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
            if ( state.backtracking > 0 ) 
            {
            	Memoize(input, 1, start_StartIndex); 
            }
        }
        return retval;
    }
    // $ANTLR end "start"

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
    // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:162:1: variable : ( 'OBJECT.' Id -> ^( OBJECT CURRENT_OBJECT Id ) | 'OBJECT(' expression ').' Id -> ^( OBJECT expression Id ) | 'TEMP.' Id -> ^( TEMP Id ) | 'GLOBAL.' Id -> ^( GLOBAL Id ) | 'THREAD.' Id -> ^( THREAD Id ) | 'PROC.' Id -> ^( PROC Id ) );
    public MetraScriptParser.variable_return variable() // throws RecognitionException [1]
    {   
        MetraScriptParser.variable_return retval = new MetraScriptParser.variable_return();
        retval.Start = input.LT(1);
        int variable_StartIndex = input.Index();
        object root_0 = null;

        IToken string_literal2 = null;
        IToken Id3 = null;
        IToken string_literal4 = null;
        IToken string_literal6 = null;
        IToken Id7 = null;
        IToken string_literal8 = null;
        IToken Id9 = null;
        IToken string_literal10 = null;
        IToken Id11 = null;
        IToken string_literal12 = null;
        IToken Id13 = null;
        IToken string_literal14 = null;
        IToken Id15 = null;
        MetraScriptParser.expression_return expression5 = default(MetraScriptParser.expression_return);


        object string_literal2_tree=null;
        object Id3_tree=null;
        object string_literal4_tree=null;
        object string_literal6_tree=null;
        object Id7_tree=null;
        object string_literal8_tree=null;
        object Id9_tree=null;
        object string_literal10_tree=null;
        object Id11_tree=null;
        object string_literal12_tree=null;
        object Id13_tree=null;
        object string_literal14_tree=null;
        object Id15_tree=null;
        RewriteRuleTokenStream stream_32 = new RewriteRuleTokenStream(adaptor,"token 32");
        RewriteRuleTokenStream stream_31 = new RewriteRuleTokenStream(adaptor,"token 31");
        RewriteRuleTokenStream stream_35 = new RewriteRuleTokenStream(adaptor,"token 35");
        RewriteRuleTokenStream stream_36 = new RewriteRuleTokenStream(adaptor,"token 36");
        RewriteRuleTokenStream stream_33 = new RewriteRuleTokenStream(adaptor,"token 33");
        RewriteRuleTokenStream stream_Id = new RewriteRuleTokenStream(adaptor,"token Id");
        RewriteRuleTokenStream stream_34 = new RewriteRuleTokenStream(adaptor,"token 34");
        RewriteRuleTokenStream stream_37 = new RewriteRuleTokenStream(adaptor,"token 37");
        RewriteRuleSubtreeStream stream_expression = new RewriteRuleSubtreeStream(adaptor,"rule expression");
        try 
    	{
    	    if ( (state.backtracking > 0) && AlreadyParsedRule(input, 2) ) 
    	    {
    	    	return retval; 
    	    }
            // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:163:2: ( 'OBJECT.' Id -> ^( OBJECT CURRENT_OBJECT Id ) | 'OBJECT(' expression ').' Id -> ^( OBJECT expression Id ) | 'TEMP.' Id -> ^( TEMP Id ) | 'GLOBAL.' Id -> ^( GLOBAL Id ) | 'THREAD.' Id -> ^( THREAD Id ) | 'PROC.' Id -> ^( PROC Id ) )
            int alt1 = 6;
            switch ( input.LA(1) ) 
            {
            case 31:
            	{
                alt1 = 1;
                }
                break;
            case 32:
            	{
                alt1 = 2;
                }
                break;
            case 34:
            	{
                alt1 = 3;
                }
                break;
            case 35:
            	{
                alt1 = 4;
                }
                break;
            case 36:
            	{
                alt1 = 5;
                }
                break;
            case 37:
            	{
                alt1 = 6;
                }
                break;
            	default:
            	    if ( state.backtracking > 0 ) {state.failed = true; return retval;}
            	    NoViableAltException nvae_d1s0 =
            	        new NoViableAltException("", 1, 0, input);

            	    throw nvae_d1s0;
            }

            switch (alt1) 
            {
                case 1 :
                    // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:163:4: 'OBJECT.' Id
                    {
                    	string_literal2=(IToken)Match(input,31,FOLLOW_31_in_variable160); if (state.failed) return retval; 
                    	if ( (state.backtracking==0) ) stream_31.Add(string_literal2);

                    	Id3=(IToken)Match(input,Id,FOLLOW_Id_in_variable162); if (state.failed) return retval; 
                    	if ( (state.backtracking==0) ) stream_Id.Add(Id3);



                    	// AST REWRITE
                    	// elements:          Id
                    	// token labels:      
                    	// rule labels:       retval
                    	// token list labels: 
                    	// rule list labels:  
                    	// wildcard labels: 
                    	if ( (state.backtracking==0) ) {
                    	retval.Tree = root_0;
                    	RewriteRuleSubtreeStream stream_retval = new RewriteRuleSubtreeStream(adaptor, "rule retval", retval!=null ? retval.Tree : null);

                    	root_0 = (object)adaptor.GetNilNode();
                    	// 163:18: -> ^( OBJECT CURRENT_OBJECT Id )
                    	{
                    	    // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:163:21: ^( OBJECT CURRENT_OBJECT Id )
                    	    {
                    	    object root_1 = (object)adaptor.GetNilNode();
                    	    root_1 = (object)adaptor.BecomeRoot((object)adaptor.Create(OBJECT, "OBJECT"), root_1);

                    	    adaptor.AddChild(root_1, (object)adaptor.Create(CURRENT_OBJECT, "CURRENT_OBJECT"));
                    	    adaptor.AddChild(root_1, stream_Id.NextNode());

                    	    adaptor.AddChild(root_0, root_1);
                    	    }

                    	}

                    	retval.Tree = root_0;retval.Tree = root_0;}
                    }
                    break;
                case 2 :
                    // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:164:4: 'OBJECT(' expression ').' Id
                    {
                    	string_literal4=(IToken)Match(input,32,FOLLOW_32_in_variable178); if (state.failed) return retval; 
                    	if ( (state.backtracking==0) ) stream_32.Add(string_literal4);

                    	PushFollow(FOLLOW_expression_in_variable180);
                    	expression5 = expression();
                    	state.followingStackPointer--;
                    	if (state.failed) return retval;
                    	if ( (state.backtracking==0) ) stream_expression.Add(expression5.Tree);
                    	string_literal6=(IToken)Match(input,33,FOLLOW_33_in_variable182); if (state.failed) return retval; 
                    	if ( (state.backtracking==0) ) stream_33.Add(string_literal6);

                    	Id7=(IToken)Match(input,Id,FOLLOW_Id_in_variable184); if (state.failed) return retval; 
                    	if ( (state.backtracking==0) ) stream_Id.Add(Id7);



                    	// AST REWRITE
                    	// elements:          Id, expression
                    	// token labels:      
                    	// rule labels:       retval
                    	// token list labels: 
                    	// rule list labels:  
                    	// wildcard labels: 
                    	if ( (state.backtracking==0) ) {
                    	retval.Tree = root_0;
                    	RewriteRuleSubtreeStream stream_retval = new RewriteRuleSubtreeStream(adaptor, "rule retval", retval!=null ? retval.Tree : null);

                    	root_0 = (object)adaptor.GetNilNode();
                    	// 164:34: -> ^( OBJECT expression Id )
                    	{
                    	    // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:164:37: ^( OBJECT expression Id )
                    	    {
                    	    object root_1 = (object)adaptor.GetNilNode();
                    	    root_1 = (object)adaptor.BecomeRoot((object)adaptor.Create(OBJECT, "OBJECT"), root_1);

                    	    adaptor.AddChild(root_1, stream_expression.NextTree());
                    	    adaptor.AddChild(root_1, stream_Id.NextNode());

                    	    adaptor.AddChild(root_0, root_1);
                    	    }

                    	}

                    	retval.Tree = root_0;retval.Tree = root_0;}
                    }
                    break;
                case 3 :
                    // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:165:4: 'TEMP.' Id
                    {
                    	string_literal8=(IToken)Match(input,34,FOLLOW_34_in_variable200); if (state.failed) return retval; 
                    	if ( (state.backtracking==0) ) stream_34.Add(string_literal8);

                    	Id9=(IToken)Match(input,Id,FOLLOW_Id_in_variable202); if (state.failed) return retval; 
                    	if ( (state.backtracking==0) ) stream_Id.Add(Id9);



                    	// AST REWRITE
                    	// elements:          Id
                    	// token labels:      
                    	// rule labels:       retval
                    	// token list labels: 
                    	// rule list labels:  
                    	// wildcard labels: 
                    	if ( (state.backtracking==0) ) {
                    	retval.Tree = root_0;
                    	RewriteRuleSubtreeStream stream_retval = new RewriteRuleSubtreeStream(adaptor, "rule retval", retval!=null ? retval.Tree : null);

                    	root_0 = (object)adaptor.GetNilNode();
                    	// 165:16: -> ^( TEMP Id )
                    	{
                    	    // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:165:19: ^( TEMP Id )
                    	    {
                    	    object root_1 = (object)adaptor.GetNilNode();
                    	    root_1 = (object)adaptor.BecomeRoot((object)adaptor.Create(TEMP, "TEMP"), root_1);

                    	    adaptor.AddChild(root_1, stream_Id.NextNode());

                    	    adaptor.AddChild(root_0, root_1);
                    	    }

                    	}

                    	retval.Tree = root_0;retval.Tree = root_0;}
                    }
                    break;
                case 4 :
                    // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:166:4: 'GLOBAL.' Id
                    {
                    	string_literal10=(IToken)Match(input,35,FOLLOW_35_in_variable216); if (state.failed) return retval; 
                    	if ( (state.backtracking==0) ) stream_35.Add(string_literal10);

                    	Id11=(IToken)Match(input,Id,FOLLOW_Id_in_variable218); if (state.failed) return retval; 
                    	if ( (state.backtracking==0) ) stream_Id.Add(Id11);



                    	// AST REWRITE
                    	// elements:          Id
                    	// token labels:      
                    	// rule labels:       retval
                    	// token list labels: 
                    	// rule list labels:  
                    	// wildcard labels: 
                    	if ( (state.backtracking==0) ) {
                    	retval.Tree = root_0;
                    	RewriteRuleSubtreeStream stream_retval = new RewriteRuleSubtreeStream(adaptor, "rule retval", retval!=null ? retval.Tree : null);

                    	root_0 = (object)adaptor.GetNilNode();
                    	// 166:18: -> ^( GLOBAL Id )
                    	{
                    	    // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:166:21: ^( GLOBAL Id )
                    	    {
                    	    object root_1 = (object)adaptor.GetNilNode();
                    	    root_1 = (object)adaptor.BecomeRoot((object)adaptor.Create(GLOBAL, "GLOBAL"), root_1);

                    	    adaptor.AddChild(root_1, stream_Id.NextNode());

                    	    adaptor.AddChild(root_0, root_1);
                    	    }

                    	}

                    	retval.Tree = root_0;retval.Tree = root_0;}
                    }
                    break;
                case 5 :
                    // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:167:4: 'THREAD.' Id
                    {
                    	string_literal12=(IToken)Match(input,36,FOLLOW_36_in_variable232); if (state.failed) return retval; 
                    	if ( (state.backtracking==0) ) stream_36.Add(string_literal12);

                    	Id13=(IToken)Match(input,Id,FOLLOW_Id_in_variable234); if (state.failed) return retval; 
                    	if ( (state.backtracking==0) ) stream_Id.Add(Id13);



                    	// AST REWRITE
                    	// elements:          Id
                    	// token labels:      
                    	// rule labels:       retval
                    	// token list labels: 
                    	// rule list labels:  
                    	// wildcard labels: 
                    	if ( (state.backtracking==0) ) {
                    	retval.Tree = root_0;
                    	RewriteRuleSubtreeStream stream_retval = new RewriteRuleSubtreeStream(adaptor, "rule retval", retval!=null ? retval.Tree : null);

                    	root_0 = (object)adaptor.GetNilNode();
                    	// 167:18: -> ^( THREAD Id )
                    	{
                    	    // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:167:21: ^( THREAD Id )
                    	    {
                    	    object root_1 = (object)adaptor.GetNilNode();
                    	    root_1 = (object)adaptor.BecomeRoot((object)adaptor.Create(THREAD, "THREAD"), root_1);

                    	    adaptor.AddChild(root_1, stream_Id.NextNode());

                    	    adaptor.AddChild(root_0, root_1);
                    	    }

                    	}

                    	retval.Tree = root_0;retval.Tree = root_0;}
                    }
                    break;
                case 6 :
                    // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:168:4: 'PROC.' Id
                    {
                    	string_literal14=(IToken)Match(input,37,FOLLOW_37_in_variable248); if (state.failed) return retval; 
                    	if ( (state.backtracking==0) ) stream_37.Add(string_literal14);

                    	Id15=(IToken)Match(input,Id,FOLLOW_Id_in_variable250); if (state.failed) return retval; 
                    	if ( (state.backtracking==0) ) stream_Id.Add(Id15);



                    	// AST REWRITE
                    	// elements:          Id
                    	// token labels:      
                    	// rule labels:       retval
                    	// token list labels: 
                    	// rule list labels:  
                    	// wildcard labels: 
                    	if ( (state.backtracking==0) ) {
                    	retval.Tree = root_0;
                    	RewriteRuleSubtreeStream stream_retval = new RewriteRuleSubtreeStream(adaptor, "rule retval", retval!=null ? retval.Tree : null);

                    	root_0 = (object)adaptor.GetNilNode();
                    	// 168:16: -> ^( PROC Id )
                    	{
                    	    // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:168:19: ^( PROC Id )
                    	    {
                    	    object root_1 = (object)adaptor.GetNilNode();
                    	    root_1 = (object)adaptor.BecomeRoot((object)adaptor.Create(PROC, "PROC"), root_1);

                    	    adaptor.AddChild(root_1, stream_Id.NextNode());

                    	    adaptor.AddChild(root_0, root_1);
                    	    }

                    	}

                    	retval.Tree = root_0;retval.Tree = root_0;}
                    }
                    break;

            }
            retval.Stop = input.LT(-1);

            if ( (state.backtracking==0) )
            {	retval.Tree = (object)adaptor.RulePostProcessing(root_0);
            	adaptor.SetTokenBoundaries(retval.Tree, (IToken) retval.Start, (IToken) retval.Stop);}
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
            if ( state.backtracking > 0 ) 
            {
            	Memoize(input, 2, variable_StartIndex); 
            }
        }
        return retval;
    }
    // $ANTLR end "variable"

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
    // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:171:1: literal : ( integerLiteral -> ^( INT integerLiteral ) | FloatingPointLiteral -> ^( FLOAT FloatingPointLiteral ) | StringLiteral -> ^( STRING StringLiteral ) | booleanLiteral -> ^( BOOL booleanLiteral ) | 'null' -> ^( NULL ) );
    public MetraScriptParser.literal_return literal() // throws RecognitionException [1]
    {   
        MetraScriptParser.literal_return retval = new MetraScriptParser.literal_return();
        retval.Start = input.LT(1);
        int literal_StartIndex = input.Index();
        object root_0 = null;

        IToken FloatingPointLiteral17 = null;
        IToken StringLiteral18 = null;
        IToken string_literal20 = null;
        MetraScriptParser.integerLiteral_return integerLiteral16 = default(MetraScriptParser.integerLiteral_return);

        MetraScriptParser.booleanLiteral_return booleanLiteral19 = default(MetraScriptParser.booleanLiteral_return);


        object FloatingPointLiteral17_tree=null;
        object StringLiteral18_tree=null;
        object string_literal20_tree=null;
        RewriteRuleTokenStream stream_StringLiteral = new RewriteRuleTokenStream(adaptor,"token StringLiteral");
        RewriteRuleTokenStream stream_FloatingPointLiteral = new RewriteRuleTokenStream(adaptor,"token FloatingPointLiteral");
        RewriteRuleTokenStream stream_38 = new RewriteRuleTokenStream(adaptor,"token 38");
        RewriteRuleSubtreeStream stream_booleanLiteral = new RewriteRuleSubtreeStream(adaptor,"rule booleanLiteral");
        RewriteRuleSubtreeStream stream_integerLiteral = new RewriteRuleSubtreeStream(adaptor,"rule integerLiteral");
        try 
    	{
    	    if ( (state.backtracking > 0) && AlreadyParsedRule(input, 3) ) 
    	    {
    	    	return retval; 
    	    }
            // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:172:2: ( integerLiteral -> ^( INT integerLiteral ) | FloatingPointLiteral -> ^( FLOAT FloatingPointLiteral ) | StringLiteral -> ^( STRING StringLiteral ) | booleanLiteral -> ^( BOOL booleanLiteral ) | 'null' -> ^( NULL ) )
            int alt2 = 5;
            switch ( input.LA(1) ) 
            {
            case HexLiteral:
            case OctalLiteral:
            case DecimalLiteral:
            	{
                alt2 = 1;
                }
                break;
            case FloatingPointLiteral:
            	{
                alt2 = 2;
                }
                break;
            case StringLiteral:
            	{
                alt2 = 3;
                }
                break;
            case 39:
            case 40:
            	{
                alt2 = 4;
                }
                break;
            case 38:
            	{
                alt2 = 5;
                }
                break;
            	default:
            	    if ( state.backtracking > 0 ) {state.failed = true; return retval;}
            	    NoViableAltException nvae_d2s0 =
            	        new NoViableAltException("", 2, 0, input);

            	    throw nvae_d2s0;
            }

            switch (alt2) 
            {
                case 1 :
                    // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:172:4: integerLiteral
                    {
                    	PushFollow(FOLLOW_integerLiteral_in_literal271);
                    	integerLiteral16 = integerLiteral();
                    	state.followingStackPointer--;
                    	if (state.failed) return retval;
                    	if ( (state.backtracking==0) ) stream_integerLiteral.Add(integerLiteral16.Tree);


                    	// AST REWRITE
                    	// elements:          integerLiteral
                    	// token labels:      
                    	// rule labels:       retval
                    	// token list labels: 
                    	// rule list labels:  
                    	// wildcard labels: 
                    	if ( (state.backtracking==0) ) {
                    	retval.Tree = root_0;
                    	RewriteRuleSubtreeStream stream_retval = new RewriteRuleSubtreeStream(adaptor, "rule retval", retval!=null ? retval.Tree : null);

                    	root_0 = (object)adaptor.GetNilNode();
                    	// 172:19: -> ^( INT integerLiteral )
                    	{
                    	    // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:172:22: ^( INT integerLiteral )
                    	    {
                    	    object root_1 = (object)adaptor.GetNilNode();
                    	    root_1 = (object)adaptor.BecomeRoot((object)adaptor.Create(INT, "INT"), root_1);

                    	    adaptor.AddChild(root_1, stream_integerLiteral.NextTree());

                    	    adaptor.AddChild(root_0, root_1);
                    	    }

                    	}

                    	retval.Tree = root_0;retval.Tree = root_0;}
                    }
                    break;
                case 2 :
                    // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:173:4: FloatingPointLiteral
                    {
                    	FloatingPointLiteral17=(IToken)Match(input,FloatingPointLiteral,FOLLOW_FloatingPointLiteral_in_literal284); if (state.failed) return retval; 
                    	if ( (state.backtracking==0) ) stream_FloatingPointLiteral.Add(FloatingPointLiteral17);



                    	// AST REWRITE
                    	// elements:          FloatingPointLiteral
                    	// token labels:      
                    	// rule labels:       retval
                    	// token list labels: 
                    	// rule list labels:  
                    	// wildcard labels: 
                    	if ( (state.backtracking==0) ) {
                    	retval.Tree = root_0;
                    	RewriteRuleSubtreeStream stream_retval = new RewriteRuleSubtreeStream(adaptor, "rule retval", retval!=null ? retval.Tree : null);

                    	root_0 = (object)adaptor.GetNilNode();
                    	// 173:24: -> ^( FLOAT FloatingPointLiteral )
                    	{
                    	    // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:173:27: ^( FLOAT FloatingPointLiteral )
                    	    {
                    	    object root_1 = (object)adaptor.GetNilNode();
                    	    root_1 = (object)adaptor.BecomeRoot((object)adaptor.Create(FLOAT, "FLOAT"), root_1);

                    	    adaptor.AddChild(root_1, stream_FloatingPointLiteral.NextNode());

                    	    adaptor.AddChild(root_0, root_1);
                    	    }

                    	}

                    	retval.Tree = root_0;retval.Tree = root_0;}
                    }
                    break;
                case 3 :
                    // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:174:4: StringLiteral
                    {
                    	StringLiteral18=(IToken)Match(input,StringLiteral,FOLLOW_StringLiteral_in_literal296); if (state.failed) return retval; 
                    	if ( (state.backtracking==0) ) stream_StringLiteral.Add(StringLiteral18);



                    	// AST REWRITE
                    	// elements:          StringLiteral
                    	// token labels:      
                    	// rule labels:       retval
                    	// token list labels: 
                    	// rule list labels:  
                    	// wildcard labels: 
                    	if ( (state.backtracking==0) ) {
                    	retval.Tree = root_0;
                    	RewriteRuleSubtreeStream stream_retval = new RewriteRuleSubtreeStream(adaptor, "rule retval", retval!=null ? retval.Tree : null);

                    	root_0 = (object)adaptor.GetNilNode();
                    	// 174:17: -> ^( STRING StringLiteral )
                    	{
                    	    // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:174:20: ^( STRING StringLiteral )
                    	    {
                    	    object root_1 = (object)adaptor.GetNilNode();
                    	    root_1 = (object)adaptor.BecomeRoot((object)adaptor.Create(STRING, "STRING"), root_1);

                    	    adaptor.AddChild(root_1, stream_StringLiteral.NextNode());

                    	    adaptor.AddChild(root_0, root_1);
                    	    }

                    	}

                    	retval.Tree = root_0;retval.Tree = root_0;}
                    }
                    break;
                case 4 :
                    // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:175:4: booleanLiteral
                    {
                    	PushFollow(FOLLOW_booleanLiteral_in_literal308);
                    	booleanLiteral19 = booleanLiteral();
                    	state.followingStackPointer--;
                    	if (state.failed) return retval;
                    	if ( (state.backtracking==0) ) stream_booleanLiteral.Add(booleanLiteral19.Tree);


                    	// AST REWRITE
                    	// elements:          booleanLiteral
                    	// token labels:      
                    	// rule labels:       retval
                    	// token list labels: 
                    	// rule list labels:  
                    	// wildcard labels: 
                    	if ( (state.backtracking==0) ) {
                    	retval.Tree = root_0;
                    	RewriteRuleSubtreeStream stream_retval = new RewriteRuleSubtreeStream(adaptor, "rule retval", retval!=null ? retval.Tree : null);

                    	root_0 = (object)adaptor.GetNilNode();
                    	// 175:19: -> ^( BOOL booleanLiteral )
                    	{
                    	    // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:175:22: ^( BOOL booleanLiteral )
                    	    {
                    	    object root_1 = (object)adaptor.GetNilNode();
                    	    root_1 = (object)adaptor.BecomeRoot((object)adaptor.Create(BOOL, "BOOL"), root_1);

                    	    adaptor.AddChild(root_1, stream_booleanLiteral.NextTree());

                    	    adaptor.AddChild(root_0, root_1);
                    	    }

                    	}

                    	retval.Tree = root_0;retval.Tree = root_0;}
                    }
                    break;
                case 5 :
                    // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:176:4: 'null'
                    {
                    	string_literal20=(IToken)Match(input,38,FOLLOW_38_in_literal321); if (state.failed) return retval; 
                    	if ( (state.backtracking==0) ) stream_38.Add(string_literal20);



                    	// AST REWRITE
                    	// elements:          
                    	// token labels:      
                    	// rule labels:       retval
                    	// token list labels: 
                    	// rule list labels:  
                    	// wildcard labels: 
                    	if ( (state.backtracking==0) ) {
                    	retval.Tree = root_0;
                    	RewriteRuleSubtreeStream stream_retval = new RewriteRuleSubtreeStream(adaptor, "rule retval", retval!=null ? retval.Tree : null);

                    	root_0 = (object)adaptor.GetNilNode();
                    	// 176:11: -> ^( NULL )
                    	{
                    	    // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:176:14: ^( NULL )
                    	    {
                    	    object root_1 = (object)adaptor.GetNilNode();
                    	    root_1 = (object)adaptor.BecomeRoot((object)adaptor.Create(NULL, "NULL"), root_1);

                    	    adaptor.AddChild(root_0, root_1);
                    	    }

                    	}

                    	retval.Tree = root_0;retval.Tree = root_0;}
                    }
                    break;

            }
            retval.Stop = input.LT(-1);

            if ( (state.backtracking==0) )
            {	retval.Tree = (object)adaptor.RulePostProcessing(root_0);
            	adaptor.SetTokenBoundaries(retval.Tree, (IToken) retval.Start, (IToken) retval.Stop);}
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
            if ( state.backtracking > 0 ) 
            {
            	Memoize(input, 3, literal_StartIndex); 
            }
        }
        return retval;
    }
    // $ANTLR end "literal"

    public class integerLiteral_return : ParserRuleReturnScope
    {
        private object tree;
        override public object Tree
        {
        	get { return tree; }
        	set { tree = (object) value; }
        }
    };

    // $ANTLR start "integerLiteral"
    // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:178:1: integerLiteral : ( HexLiteral | OctalLiteral | DecimalLiteral );
    public MetraScriptParser.integerLiteral_return integerLiteral() // throws RecognitionException [1]
    {   
        MetraScriptParser.integerLiteral_return retval = new MetraScriptParser.integerLiteral_return();
        retval.Start = input.LT(1);
        int integerLiteral_StartIndex = input.Index();
        object root_0 = null;

        IToken set21 = null;

        object set21_tree=null;

        try 
    	{
    	    if ( (state.backtracking > 0) && AlreadyParsedRule(input, 4) ) 
    	    {
    	    	return retval; 
    	    }
            // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:179:2: ( HexLiteral | OctalLiteral | DecimalLiteral )
            // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:
            {
            	root_0 = (object)adaptor.GetNilNode();

            	set21 = (IToken)input.LT(1);
            	if ( (input.LA(1) >= HexLiteral && input.LA(1) <= DecimalLiteral) ) 
            	{
            	    input.Consume();
            	    if ( state.backtracking == 0 ) adaptor.AddChild(root_0, (object)adaptor.Create(set21));
            	    state.errorRecovery = false;state.failed = false;
            	}
            	else 
            	{
            	    if ( state.backtracking > 0 ) {state.failed = true; return retval;}
            	    MismatchedSetException mse = new MismatchedSetException(null,input);
            	    throw mse;
            	}


            }

            retval.Stop = input.LT(-1);

            if ( (state.backtracking==0) )
            {	retval.Tree = (object)adaptor.RulePostProcessing(root_0);
            	adaptor.SetTokenBoundaries(retval.Tree, (IToken) retval.Start, (IToken) retval.Stop);}
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
            if ( state.backtracking > 0 ) 
            {
            	Memoize(input, 4, integerLiteral_StartIndex); 
            }
        }
        return retval;
    }
    // $ANTLR end "integerLiteral"

    public class booleanLiteral_return : ParserRuleReturnScope
    {
        private object tree;
        override public object Tree
        {
        	get { return tree; }
        	set { tree = (object) value; }
        }
    };

    // $ANTLR start "booleanLiteral"
    // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:183:1: booleanLiteral : ( 'true' | 'false' );
    public MetraScriptParser.booleanLiteral_return booleanLiteral() // throws RecognitionException [1]
    {   
        MetraScriptParser.booleanLiteral_return retval = new MetraScriptParser.booleanLiteral_return();
        retval.Start = input.LT(1);
        int booleanLiteral_StartIndex = input.Index();
        object root_0 = null;

        IToken set22 = null;

        object set22_tree=null;

        try 
    	{
    	    if ( (state.backtracking > 0) && AlreadyParsedRule(input, 5) ) 
    	    {
    	    	return retval; 
    	    }
            // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:184:2: ( 'true' | 'false' )
            // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:
            {
            	root_0 = (object)adaptor.GetNilNode();

            	set22 = (IToken)input.LT(1);
            	if ( (input.LA(1) >= 39 && input.LA(1) <= 40) ) 
            	{
            	    input.Consume();
            	    if ( state.backtracking == 0 ) adaptor.AddChild(root_0, (object)adaptor.Create(set22));
            	    state.errorRecovery = false;state.failed = false;
            	}
            	else 
            	{
            	    if ( state.backtracking > 0 ) {state.failed = true; return retval;}
            	    MismatchedSetException mse = new MismatchedSetException(null,input);
            	    throw mse;
            	}


            }

            retval.Stop = input.LT(-1);

            if ( (state.backtracking==0) )
            {	retval.Tree = (object)adaptor.RulePostProcessing(root_0);
            	adaptor.SetTokenBoundaries(retval.Tree, (IToken) retval.Start, (IToken) retval.Stop);}
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
            if ( state.backtracking > 0 ) 
            {
            	Memoize(input, 5, booleanLiteral_StartIndex); 
            }
        }
        return retval;
    }
    // $ANTLR end "booleanLiteral"

    public class expression_return : ParserRuleReturnScope
    {
        private object tree;
        override public object Tree
        {
        	get { return tree; }
        	set { tree = (object) value; }
        }
    };

    // $ANTLR start "expression"
    // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:188:1: expression : conditionalExpression ( assignmentOperator expression )? ;
    public MetraScriptParser.expression_return expression() // throws RecognitionException [1]
    {   
        MetraScriptParser.expression_return retval = new MetraScriptParser.expression_return();
        retval.Start = input.LT(1);
        int expression_StartIndex = input.Index();
        object root_0 = null;

        MetraScriptParser.conditionalExpression_return conditionalExpression23 = default(MetraScriptParser.conditionalExpression_return);

        MetraScriptParser.assignmentOperator_return assignmentOperator24 = default(MetraScriptParser.assignmentOperator_return);

        MetraScriptParser.expression_return expression25 = default(MetraScriptParser.expression_return);



        try 
    	{
    	    if ( (state.backtracking > 0) && AlreadyParsedRule(input, 6) ) 
    	    {
    	    	return retval; 
    	    }
            // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:189:2: ( conditionalExpression ( assignmentOperator expression )? )
            // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:189:4: conditionalExpression ( assignmentOperator expression )?
            {
            	root_0 = (object)adaptor.GetNilNode();

            	PushFollow(FOLLOW_conditionalExpression_in_expression373);
            	conditionalExpression23 = conditionalExpression();
            	state.followingStackPointer--;
            	if (state.failed) return retval;
            	if ( state.backtracking == 0 ) adaptor.AddChild(root_0, conditionalExpression23.Tree);
            	// C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:189:26: ( assignmentOperator expression )?
            	int alt3 = 2;
            	alt3 = dfa3.Predict(input);
            	switch (alt3) 
            	{
            	    case 1 :
            	        // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:189:27: assignmentOperator expression
            	        {
            	        	PushFollow(FOLLOW_assignmentOperator_in_expression376);
            	        	assignmentOperator24 = assignmentOperator();
            	        	state.followingStackPointer--;
            	        	if (state.failed) return retval;
            	        	if ( state.backtracking == 0 ) root_0 = (object)adaptor.BecomeRoot(assignmentOperator24.Tree, root_0);
            	        	PushFollow(FOLLOW_expression_in_expression379);
            	        	expression25 = expression();
            	        	state.followingStackPointer--;
            	        	if (state.failed) return retval;
            	        	if ( state.backtracking == 0 ) adaptor.AddChild(root_0, expression25.Tree);

            	        }
            	        break;

            	}


            }

            retval.Stop = input.LT(-1);

            if ( (state.backtracking==0) )
            {	retval.Tree = (object)adaptor.RulePostProcessing(root_0);
            	adaptor.SetTokenBoundaries(retval.Tree, (IToken) retval.Start, (IToken) retval.Stop);}
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
            if ( state.backtracking > 0 ) 
            {
            	Memoize(input, 6, expression_StartIndex); 
            }
        }
        return retval;
    }
    // $ANTLR end "expression"

    public class assignmentOperator_return : ParserRuleReturnScope
    {
        private object tree;
        override public object Tree
        {
        	get { return tree; }
        	set { tree = (object) value; }
        }
    };

    // $ANTLR start "assignmentOperator"
    // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:191:1: assignmentOperator : ( '=' | '~' | '+=' | '-=' | '*=' | '/=' | '&=' | '|=' | '^=' | '%=' | '~=' | ( '<' '<' '=' )=>t1= '<' t2= '<' t3= '=' {...}? | ( '>' '>' '>' '=' )=>t1= '>' t2= '>' t3= '>' t4= '=' {...}? | ( '>' '>' '=' )=>t1= '>' t2= '>' t3= '=' {...}?);
    public MetraScriptParser.assignmentOperator_return assignmentOperator() // throws RecognitionException [1]
    {   
        MetraScriptParser.assignmentOperator_return retval = new MetraScriptParser.assignmentOperator_return();
        retval.Start = input.LT(1);
        int assignmentOperator_StartIndex = input.Index();
        object root_0 = null;

        IToken t1 = null;
        IToken t2 = null;
        IToken t3 = null;
        IToken t4 = null;
        IToken char_literal26 = null;
        IToken char_literal27 = null;
        IToken string_literal28 = null;
        IToken string_literal29 = null;
        IToken string_literal30 = null;
        IToken string_literal31 = null;
        IToken string_literal32 = null;
        IToken string_literal33 = null;
        IToken string_literal34 = null;
        IToken string_literal35 = null;
        IToken string_literal36 = null;

        object t1_tree=null;
        object t2_tree=null;
        object t3_tree=null;
        object t4_tree=null;
        object char_literal26_tree=null;
        object char_literal27_tree=null;
        object string_literal28_tree=null;
        object string_literal29_tree=null;
        object string_literal30_tree=null;
        object string_literal31_tree=null;
        object string_literal32_tree=null;
        object string_literal33_tree=null;
        object string_literal34_tree=null;
        object string_literal35_tree=null;
        object string_literal36_tree=null;

        try 
    	{
    	    if ( (state.backtracking > 0) && AlreadyParsedRule(input, 7) ) 
    	    {
    	    	return retval; 
    	    }
            // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:192:2: ( '=' | '~' | '+=' | '-=' | '*=' | '/=' | '&=' | '|=' | '^=' | '%=' | '~=' | ( '<' '<' '=' )=>t1= '<' t2= '<' t3= '=' {...}? | ( '>' '>' '>' '=' )=>t1= '>' t2= '>' t3= '>' t4= '=' {...}? | ( '>' '>' '=' )=>t1= '>' t2= '>' t3= '=' {...}?)
            int alt4 = 14;
            alt4 = dfa4.Predict(input);
            switch (alt4) 
            {
                case 1 :
                    // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:192:4: '='
                    {
                    	root_0 = (object)adaptor.GetNilNode();

                    	char_literal26=(IToken)Match(input,41,FOLLOW_41_in_assignmentOperator391); if (state.failed) return retval;
                    	if ( state.backtracking == 0 )
                    	{char_literal26_tree = (object)adaptor.Create(char_literal26);
                    		adaptor.AddChild(root_0, char_literal26_tree);
                    	}

                    }
                    break;
                case 2 :
                    // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:193:4: '~'
                    {
                    	root_0 = (object)adaptor.GetNilNode();

                    	char_literal27=(IToken)Match(input,42,FOLLOW_42_in_assignmentOperator396); if (state.failed) return retval;
                    	if ( state.backtracking == 0 )
                    	{char_literal27_tree = (object)adaptor.Create(char_literal27);
                    		adaptor.AddChild(root_0, char_literal27_tree);
                    	}

                    }
                    break;
                case 3 :
                    // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:194:4: '+='
                    {
                    	root_0 = (object)adaptor.GetNilNode();

                    	string_literal28=(IToken)Match(input,43,FOLLOW_43_in_assignmentOperator401); if (state.failed) return retval;
                    	if ( state.backtracking == 0 )
                    	{string_literal28_tree = (object)adaptor.Create(string_literal28);
                    		adaptor.AddChild(root_0, string_literal28_tree);
                    	}

                    }
                    break;
                case 4 :
                    // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:195:4: '-='
                    {
                    	root_0 = (object)adaptor.GetNilNode();

                    	string_literal29=(IToken)Match(input,44,FOLLOW_44_in_assignmentOperator406); if (state.failed) return retval;
                    	if ( state.backtracking == 0 )
                    	{string_literal29_tree = (object)adaptor.Create(string_literal29);
                    		adaptor.AddChild(root_0, string_literal29_tree);
                    	}

                    }
                    break;
                case 5 :
                    // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:196:4: '*='
                    {
                    	root_0 = (object)adaptor.GetNilNode();

                    	string_literal30=(IToken)Match(input,45,FOLLOW_45_in_assignmentOperator411); if (state.failed) return retval;
                    	if ( state.backtracking == 0 )
                    	{string_literal30_tree = (object)adaptor.Create(string_literal30);
                    		adaptor.AddChild(root_0, string_literal30_tree);
                    	}

                    }
                    break;
                case 6 :
                    // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:197:4: '/='
                    {
                    	root_0 = (object)adaptor.GetNilNode();

                    	string_literal31=(IToken)Match(input,46,FOLLOW_46_in_assignmentOperator416); if (state.failed) return retval;
                    	if ( state.backtracking == 0 )
                    	{string_literal31_tree = (object)adaptor.Create(string_literal31);
                    		adaptor.AddChild(root_0, string_literal31_tree);
                    	}

                    }
                    break;
                case 7 :
                    // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:198:4: '&='
                    {
                    	root_0 = (object)adaptor.GetNilNode();

                    	string_literal32=(IToken)Match(input,47,FOLLOW_47_in_assignmentOperator421); if (state.failed) return retval;
                    	if ( state.backtracking == 0 )
                    	{string_literal32_tree = (object)adaptor.Create(string_literal32);
                    		adaptor.AddChild(root_0, string_literal32_tree);
                    	}

                    }
                    break;
                case 8 :
                    // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:199:4: '|='
                    {
                    	root_0 = (object)adaptor.GetNilNode();

                    	string_literal33=(IToken)Match(input,48,FOLLOW_48_in_assignmentOperator426); if (state.failed) return retval;
                    	if ( state.backtracking == 0 )
                    	{string_literal33_tree = (object)adaptor.Create(string_literal33);
                    		adaptor.AddChild(root_0, string_literal33_tree);
                    	}

                    }
                    break;
                case 9 :
                    // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:200:4: '^='
                    {
                    	root_0 = (object)adaptor.GetNilNode();

                    	string_literal34=(IToken)Match(input,49,FOLLOW_49_in_assignmentOperator431); if (state.failed) return retval;
                    	if ( state.backtracking == 0 )
                    	{string_literal34_tree = (object)adaptor.Create(string_literal34);
                    		adaptor.AddChild(root_0, string_literal34_tree);
                    	}

                    }
                    break;
                case 10 :
                    // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:201:4: '%='
                    {
                    	root_0 = (object)adaptor.GetNilNode();

                    	string_literal35=(IToken)Match(input,50,FOLLOW_50_in_assignmentOperator436); if (state.failed) return retval;
                    	if ( state.backtracking == 0 )
                    	{string_literal35_tree = (object)adaptor.Create(string_literal35);
                    		adaptor.AddChild(root_0, string_literal35_tree);
                    	}

                    }
                    break;
                case 11 :
                    // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:202:4: '~='
                    {
                    	root_0 = (object)adaptor.GetNilNode();

                    	string_literal36=(IToken)Match(input,51,FOLLOW_51_in_assignmentOperator441); if (state.failed) return retval;
                    	if ( state.backtracking == 0 )
                    	{string_literal36_tree = (object)adaptor.Create(string_literal36);
                    		adaptor.AddChild(root_0, string_literal36_tree);
                    	}

                    }
                    break;
                case 12 :
                    // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:203:4: ( '<' '<' '=' )=>t1= '<' t2= '<' t3= '=' {...}?
                    {
                    	root_0 = (object)adaptor.GetNilNode();

                    	t1=(IToken)Match(input,52,FOLLOW_52_in_assignmentOperator457); if (state.failed) return retval;
                    	if ( state.backtracking == 0 )
                    	{t1_tree = (object)adaptor.Create(t1);
                    		adaptor.AddChild(root_0, t1_tree);
                    	}
                    	t2=(IToken)Match(input,52,FOLLOW_52_in_assignmentOperator461); if (state.failed) return retval;
                    	if ( state.backtracking == 0 )
                    	{t2_tree = (object)adaptor.Create(t2);
                    		adaptor.AddChild(root_0, t2_tree);
                    	}
                    	t3=(IToken)Match(input,41,FOLLOW_41_in_assignmentOperator465); if (state.failed) return retval;
                    	if ( state.backtracking == 0 )
                    	{t3_tree = (object)adaptor.Create(t3);
                    		adaptor.AddChild(root_0, t3_tree);
                    	}
                    	if ( !(( t1.getLine() == t2.getLine() &&
                    		  t1.getCharPositionInLine() + 1 == t2.getCharPositionInLine() && 
                    		  t2.getLine() == t3.getLine() && 
                    		  t2.getCharPositionInLine() + 1 == t3.getCharPositionInLine() )) ) 
                    	{
                    	    if ( state.backtracking > 0 ) {state.failed = true; return retval;}
                    	    throw new FailedPredicateException(input, "assignmentOperator", " $t1.getLine() == $t2.getLine() &&\r\n\t  $t1.getCharPositionInLine() + 1 == $t2.getCharPositionInLine() && \r\n\t  $t2.getLine() == $t3.getLine() && \r\n\t  $t2.getCharPositionInLine() + 1 == $t3.getCharPositionInLine() ");
                    	}

                    }
                    break;
                case 13 :
                    // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:208:4: ( '>' '>' '>' '=' )=>t1= '>' t2= '>' t3= '>' t4= '=' {...}?
                    {
                    	root_0 = (object)adaptor.GetNilNode();

                    	t1=(IToken)Match(input,53,FOLLOW_53_in_assignmentOperator489); if (state.failed) return retval;
                    	if ( state.backtracking == 0 )
                    	{t1_tree = (object)adaptor.Create(t1);
                    		adaptor.AddChild(root_0, t1_tree);
                    	}
                    	t2=(IToken)Match(input,53,FOLLOW_53_in_assignmentOperator493); if (state.failed) return retval;
                    	if ( state.backtracking == 0 )
                    	{t2_tree = (object)adaptor.Create(t2);
                    		adaptor.AddChild(root_0, t2_tree);
                    	}
                    	t3=(IToken)Match(input,53,FOLLOW_53_in_assignmentOperator497); if (state.failed) return retval;
                    	if ( state.backtracking == 0 )
                    	{t3_tree = (object)adaptor.Create(t3);
                    		adaptor.AddChild(root_0, t3_tree);
                    	}
                    	t4=(IToken)Match(input,41,FOLLOW_41_in_assignmentOperator501); if (state.failed) return retval;
                    	if ( state.backtracking == 0 )
                    	{t4_tree = (object)adaptor.Create(t4);
                    		adaptor.AddChild(root_0, t4_tree);
                    	}
                    	if ( !(( t1.getLine() == t2.getLine() && 
                    		  t1.getCharPositionInLine() + 1 == t2.getCharPositionInLine() &&
                    		  t2.getLine() == t3.getLine() && 
                    		  t2.getCharPositionInLine() + 1 == t3.getCharPositionInLine() &&
                    		  t3.getLine() == t4.getLine() && 
                    		  t3.getCharPositionInLine() + 1 == t4.getCharPositionInLine() )) ) 
                    	{
                    	    if ( state.backtracking > 0 ) {state.failed = true; return retval;}
                    	    throw new FailedPredicateException(input, "assignmentOperator", " $t1.getLine() == $t2.getLine() && \r\n\t  $t1.getCharPositionInLine() + 1 == $t2.getCharPositionInLine() &&\r\n\t  $t2.getLine() == $t3.getLine() && \r\n\t  $t2.getCharPositionInLine() + 1 == $t3.getCharPositionInLine() &&\r\n\t  $t3.getLine() == $t4.getLine() && \r\n\t  $t3.getCharPositionInLine() + 1 == $t4.getCharPositionInLine() ");
                    	}

                    }
                    break;
                case 14 :
                    // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:215:4: ( '>' '>' '=' )=>t1= '>' t2= '>' t3= '=' {...}?
                    {
                    	root_0 = (object)adaptor.GetNilNode();

                    	t1=(IToken)Match(input,53,FOLLOW_53_in_assignmentOperator522); if (state.failed) return retval;
                    	if ( state.backtracking == 0 )
                    	{t1_tree = (object)adaptor.Create(t1);
                    		adaptor.AddChild(root_0, t1_tree);
                    	}
                    	t2=(IToken)Match(input,53,FOLLOW_53_in_assignmentOperator526); if (state.failed) return retval;
                    	if ( state.backtracking == 0 )
                    	{t2_tree = (object)adaptor.Create(t2);
                    		adaptor.AddChild(root_0, t2_tree);
                    	}
                    	t3=(IToken)Match(input,41,FOLLOW_41_in_assignmentOperator530); if (state.failed) return retval;
                    	if ( state.backtracking == 0 )
                    	{t3_tree = (object)adaptor.Create(t3);
                    		adaptor.AddChild(root_0, t3_tree);
                    	}
                    	if ( !(( t1.getLine() == t2.getLine() && 
                    		  t1.getCharPositionInLine() + 1 == t2.getCharPositionInLine() && 
                    		  t2.getLine() == t3.getLine() && 
                    		  t2.getCharPositionInLine() + 1 == t3.getCharPositionInLine() )) ) 
                    	{
                    	    if ( state.backtracking > 0 ) {state.failed = true; return retval;}
                    	    throw new FailedPredicateException(input, "assignmentOperator", " $t1.getLine() == $t2.getLine() && \r\n\t  $t1.getCharPositionInLine() + 1 == $t2.getCharPositionInLine() && \r\n\t  $t2.getLine() == $t3.getLine() && \r\n\t  $t2.getCharPositionInLine() + 1 == $t3.getCharPositionInLine() ");
                    	}

                    }
                    break;

            }
            retval.Stop = input.LT(-1);

            if ( (state.backtracking==0) )
            {	retval.Tree = (object)adaptor.RulePostProcessing(root_0);
            	adaptor.SetTokenBoundaries(retval.Tree, (IToken) retval.Start, (IToken) retval.Stop);}
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
            if ( state.backtracking > 0 ) 
            {
            	Memoize(input, 7, assignmentOperator_StartIndex); 
            }
        }
        return retval;
    }
    // $ANTLR end "assignmentOperator"

    public class conditionalExpression_return : ParserRuleReturnScope
    {
        private object tree;
        override public object Tree
        {
        	get { return tree; }
        	set { tree = (object) value; }
        }
    };

    // $ANTLR start "conditionalExpression"
    // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:221:1: conditionalExpression : conditionalOrExpression ( '?' expression ':' expression )? ;
    public MetraScriptParser.conditionalExpression_return conditionalExpression() // throws RecognitionException [1]
    {   
        MetraScriptParser.conditionalExpression_return retval = new MetraScriptParser.conditionalExpression_return();
        retval.Start = input.LT(1);
        int conditionalExpression_StartIndex = input.Index();
        object root_0 = null;

        IToken char_literal38 = null;
        IToken char_literal40 = null;
        MetraScriptParser.conditionalOrExpression_return conditionalOrExpression37 = default(MetraScriptParser.conditionalOrExpression_return);

        MetraScriptParser.expression_return expression39 = default(MetraScriptParser.expression_return);

        MetraScriptParser.expression_return expression41 = default(MetraScriptParser.expression_return);


        object char_literal38_tree=null;
        object char_literal40_tree=null;

        try 
    	{
    	    if ( (state.backtracking > 0) && AlreadyParsedRule(input, 8) ) 
    	    {
    	    	return retval; 
    	    }
            // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:222:2: ( conditionalOrExpression ( '?' expression ':' expression )? )
            // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:222:4: conditionalOrExpression ( '?' expression ':' expression )?
            {
            	root_0 = (object)adaptor.GetNilNode();

            	PushFollow(FOLLOW_conditionalOrExpression_in_conditionalExpression545);
            	conditionalOrExpression37 = conditionalOrExpression();
            	state.followingStackPointer--;
            	if (state.failed) return retval;
            	if ( state.backtracking == 0 ) adaptor.AddChild(root_0, conditionalOrExpression37.Tree);
            	// C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:222:28: ( '?' expression ':' expression )?
            	int alt5 = 2;
            	int LA5_0 = input.LA(1);

            	if ( (LA5_0 == 54) )
            	{
            	    alt5 = 1;
            	}
            	switch (alt5) 
            	{
            	    case 1 :
            	        // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:222:30: '?' expression ':' expression
            	        {
            	        	char_literal38=(IToken)Match(input,54,FOLLOW_54_in_conditionalExpression549); if (state.failed) return retval;
            	        	if ( state.backtracking == 0 )
            	        	{char_literal38_tree = (object)adaptor.Create(char_literal38);
            	        		root_0 = (object)adaptor.BecomeRoot(char_literal38_tree, root_0);
            	        	}
            	        	PushFollow(FOLLOW_expression_in_conditionalExpression552);
            	        	expression39 = expression();
            	        	state.followingStackPointer--;
            	        	if (state.failed) return retval;
            	        	if ( state.backtracking == 0 ) adaptor.AddChild(root_0, expression39.Tree);
            	        	char_literal40=(IToken)Match(input,55,FOLLOW_55_in_conditionalExpression554); if (state.failed) return retval;
            	        	PushFollow(FOLLOW_expression_in_conditionalExpression557);
            	        	expression41 = expression();
            	        	state.followingStackPointer--;
            	        	if (state.failed) return retval;
            	        	if ( state.backtracking == 0 ) adaptor.AddChild(root_0, expression41.Tree);

            	        }
            	        break;

            	}


            }

            retval.Stop = input.LT(-1);

            if ( (state.backtracking==0) )
            {	retval.Tree = (object)adaptor.RulePostProcessing(root_0);
            	adaptor.SetTokenBoundaries(retval.Tree, (IToken) retval.Start, (IToken) retval.Stop);}
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
            if ( state.backtracking > 0 ) 
            {
            	Memoize(input, 8, conditionalExpression_StartIndex); 
            }
        }
        return retval;
    }
    // $ANTLR end "conditionalExpression"

    public class conditionalOrExpression_return : ParserRuleReturnScope
    {
        private object tree;
        override public object Tree
        {
        	get { return tree; }
        	set { tree = (object) value; }
        }
    };

    // $ANTLR start "conditionalOrExpression"
    // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:224:1: conditionalOrExpression : conditionalAndExpression ( ( '||' | 'or' | 'OR' ) conditionalAndExpression )* ;
    public MetraScriptParser.conditionalOrExpression_return conditionalOrExpression() // throws RecognitionException [1]
    {   
        MetraScriptParser.conditionalOrExpression_return retval = new MetraScriptParser.conditionalOrExpression_return();
        retval.Start = input.LT(1);
        int conditionalOrExpression_StartIndex = input.Index();
        object root_0 = null;

        IToken set43 = null;
        MetraScriptParser.conditionalAndExpression_return conditionalAndExpression42 = default(MetraScriptParser.conditionalAndExpression_return);

        MetraScriptParser.conditionalAndExpression_return conditionalAndExpression44 = default(MetraScriptParser.conditionalAndExpression_return);


        object set43_tree=null;

        try 
    	{
    	    if ( (state.backtracking > 0) && AlreadyParsedRule(input, 9) ) 
    	    {
    	    	return retval; 
    	    }
            // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:225:2: ( conditionalAndExpression ( ( '||' | 'or' | 'OR' ) conditionalAndExpression )* )
            // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:225:4: conditionalAndExpression ( ( '||' | 'or' | 'OR' ) conditionalAndExpression )*
            {
            	root_0 = (object)adaptor.GetNilNode();

            	PushFollow(FOLLOW_conditionalAndExpression_in_conditionalOrExpression570);
            	conditionalAndExpression42 = conditionalAndExpression();
            	state.followingStackPointer--;
            	if (state.failed) return retval;
            	if ( state.backtracking == 0 ) adaptor.AddChild(root_0, conditionalAndExpression42.Tree);
            	// C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:225:29: ( ( '||' | 'or' | 'OR' ) conditionalAndExpression )*
            	do 
            	{
            	    int alt6 = 2;
            	    int LA6_0 = input.LA(1);

            	    if ( ((LA6_0 >= 56 && LA6_0 <= 58)) )
            	    {
            	        alt6 = 1;
            	    }


            	    switch (alt6) 
            		{
            			case 1 :
            			    // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:225:31: ( '||' | 'or' | 'OR' ) conditionalAndExpression
            			    {
            			    	set43=(IToken)input.LT(1);
            			    	set43 = (IToken)input.LT(1);
            			    	if ( (input.LA(1) >= 56 && input.LA(1) <= 58) ) 
            			    	{
            			    	    input.Consume();
            			    	    if ( state.backtracking == 0 ) root_0 = (object)adaptor.BecomeRoot((object)adaptor.Create(set43), root_0);
            			    	    state.errorRecovery = false;state.failed = false;
            			    	}
            			    	else 
            			    	{
            			    	    if ( state.backtracking > 0 ) {state.failed = true; return retval;}
            			    	    MismatchedSetException mse = new MismatchedSetException(null,input);
            			    	    throw mse;
            			    	}

            			    	PushFollow(FOLLOW_conditionalAndExpression_in_conditionalOrExpression583);
            			    	conditionalAndExpression44 = conditionalAndExpression();
            			    	state.followingStackPointer--;
            			    	if (state.failed) return retval;
            			    	if ( state.backtracking == 0 ) adaptor.AddChild(root_0, conditionalAndExpression44.Tree);

            			    }
            			    break;

            			default:
            			    goto loop6;
            	    }
            	} while (true);

            	loop6:
            		;	// Stops C# compiler whining that label 'loop6' has no statements


            }

            retval.Stop = input.LT(-1);

            if ( (state.backtracking==0) )
            {	retval.Tree = (object)adaptor.RulePostProcessing(root_0);
            	adaptor.SetTokenBoundaries(retval.Tree, (IToken) retval.Start, (IToken) retval.Stop);}
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
            if ( state.backtracking > 0 ) 
            {
            	Memoize(input, 9, conditionalOrExpression_StartIndex); 
            }
        }
        return retval;
    }
    // $ANTLR end "conditionalOrExpression"

    public class conditionalAndExpression_return : ParserRuleReturnScope
    {
        private object tree;
        override public object Tree
        {
        	get { return tree; }
        	set { tree = (object) value; }
        }
    };

    // $ANTLR start "conditionalAndExpression"
    // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:228:1: conditionalAndExpression : inclusiveOrExpression ( ( '&&' | 'and' | 'AND' ) inclusiveOrExpression )* ;
    public MetraScriptParser.conditionalAndExpression_return conditionalAndExpression() // throws RecognitionException [1]
    {   
        MetraScriptParser.conditionalAndExpression_return retval = new MetraScriptParser.conditionalAndExpression_return();
        retval.Start = input.LT(1);
        int conditionalAndExpression_StartIndex = input.Index();
        object root_0 = null;

        IToken set46 = null;
        MetraScriptParser.inclusiveOrExpression_return inclusiveOrExpression45 = default(MetraScriptParser.inclusiveOrExpression_return);

        MetraScriptParser.inclusiveOrExpression_return inclusiveOrExpression47 = default(MetraScriptParser.inclusiveOrExpression_return);


        object set46_tree=null;

        try 
    	{
    	    if ( (state.backtracking > 0) && AlreadyParsedRule(input, 10) ) 
    	    {
    	    	return retval; 
    	    }
            // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:229:2: ( inclusiveOrExpression ( ( '&&' | 'and' | 'AND' ) inclusiveOrExpression )* )
            // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:229:4: inclusiveOrExpression ( ( '&&' | 'and' | 'AND' ) inclusiveOrExpression )*
            {
            	root_0 = (object)adaptor.GetNilNode();

            	PushFollow(FOLLOW_inclusiveOrExpression_in_conditionalAndExpression598);
            	inclusiveOrExpression45 = inclusiveOrExpression();
            	state.followingStackPointer--;
            	if (state.failed) return retval;
            	if ( state.backtracking == 0 ) adaptor.AddChild(root_0, inclusiveOrExpression45.Tree);
            	// C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:229:26: ( ( '&&' | 'and' | 'AND' ) inclusiveOrExpression )*
            	do 
            	{
            	    int alt7 = 2;
            	    int LA7_0 = input.LA(1);

            	    if ( ((LA7_0 >= 59 && LA7_0 <= 61)) )
            	    {
            	        alt7 = 1;
            	    }


            	    switch (alt7) 
            		{
            			case 1 :
            			    // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:229:28: ( '&&' | 'and' | 'AND' ) inclusiveOrExpression
            			    {
            			    	set46=(IToken)input.LT(1);
            			    	set46 = (IToken)input.LT(1);
            			    	if ( (input.LA(1) >= 59 && input.LA(1) <= 61) ) 
            			    	{
            			    	    input.Consume();
            			    	    if ( state.backtracking == 0 ) root_0 = (object)adaptor.BecomeRoot((object)adaptor.Create(set46), root_0);
            			    	    state.errorRecovery = false;state.failed = false;
            			    	}
            			    	else 
            			    	{
            			    	    if ( state.backtracking > 0 ) {state.failed = true; return retval;}
            			    	    MismatchedSetException mse = new MismatchedSetException(null,input);
            			    	    throw mse;
            			    	}

            			    	PushFollow(FOLLOW_inclusiveOrExpression_in_conditionalAndExpression611);
            			    	inclusiveOrExpression47 = inclusiveOrExpression();
            			    	state.followingStackPointer--;
            			    	if (state.failed) return retval;
            			    	if ( state.backtracking == 0 ) adaptor.AddChild(root_0, inclusiveOrExpression47.Tree);

            			    }
            			    break;

            			default:
            			    goto loop7;
            	    }
            	} while (true);

            	loop7:
            		;	// Stops C# compiler whining that label 'loop7' has no statements


            }

            retval.Stop = input.LT(-1);

            if ( (state.backtracking==0) )
            {	retval.Tree = (object)adaptor.RulePostProcessing(root_0);
            	adaptor.SetTokenBoundaries(retval.Tree, (IToken) retval.Start, (IToken) retval.Stop);}
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
            if ( state.backtracking > 0 ) 
            {
            	Memoize(input, 10, conditionalAndExpression_StartIndex); 
            }
        }
        return retval;
    }
    // $ANTLR end "conditionalAndExpression"

    public class inclusiveOrExpression_return : ParserRuleReturnScope
    {
        private object tree;
        override public object Tree
        {
        	get { return tree; }
        	set { tree = (object) value; }
        }
    };

    // $ANTLR start "inclusiveOrExpression"
    // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:232:1: inclusiveOrExpression : exclusiveOrExpression ( '|' exclusiveOrExpression )* ;
    public MetraScriptParser.inclusiveOrExpression_return inclusiveOrExpression() // throws RecognitionException [1]
    {   
        MetraScriptParser.inclusiveOrExpression_return retval = new MetraScriptParser.inclusiveOrExpression_return();
        retval.Start = input.LT(1);
        int inclusiveOrExpression_StartIndex = input.Index();
        object root_0 = null;

        IToken char_literal49 = null;
        MetraScriptParser.exclusiveOrExpression_return exclusiveOrExpression48 = default(MetraScriptParser.exclusiveOrExpression_return);

        MetraScriptParser.exclusiveOrExpression_return exclusiveOrExpression50 = default(MetraScriptParser.exclusiveOrExpression_return);


        object char_literal49_tree=null;

        try 
    	{
    	    if ( (state.backtracking > 0) && AlreadyParsedRule(input, 11) ) 
    	    {
    	    	return retval; 
    	    }
            // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:233:2: ( exclusiveOrExpression ( '|' exclusiveOrExpression )* )
            // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:233:4: exclusiveOrExpression ( '|' exclusiveOrExpression )*
            {
            	root_0 = (object)adaptor.GetNilNode();

            	PushFollow(FOLLOW_exclusiveOrExpression_in_inclusiveOrExpression626);
            	exclusiveOrExpression48 = exclusiveOrExpression();
            	state.followingStackPointer--;
            	if (state.failed) return retval;
            	if ( state.backtracking == 0 ) adaptor.AddChild(root_0, exclusiveOrExpression48.Tree);
            	// C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:233:26: ( '|' exclusiveOrExpression )*
            	do 
            	{
            	    int alt8 = 2;
            	    int LA8_0 = input.LA(1);

            	    if ( (LA8_0 == 62) )
            	    {
            	        alt8 = 1;
            	    }


            	    switch (alt8) 
            		{
            			case 1 :
            			    // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:233:28: '|' exclusiveOrExpression
            			    {
            			    	char_literal49=(IToken)Match(input,62,FOLLOW_62_in_inclusiveOrExpression630); if (state.failed) return retval;
            			    	if ( state.backtracking == 0 )
            			    	{char_literal49_tree = (object)adaptor.Create(char_literal49);
            			    		root_0 = (object)adaptor.BecomeRoot(char_literal49_tree, root_0);
            			    	}
            			    	PushFollow(FOLLOW_exclusiveOrExpression_in_inclusiveOrExpression633);
            			    	exclusiveOrExpression50 = exclusiveOrExpression();
            			    	state.followingStackPointer--;
            			    	if (state.failed) return retval;
            			    	if ( state.backtracking == 0 ) adaptor.AddChild(root_0, exclusiveOrExpression50.Tree);

            			    }
            			    break;

            			default:
            			    goto loop8;
            	    }
            	} while (true);

            	loop8:
            		;	// Stops C# compiler whining that label 'loop8' has no statements


            }

            retval.Stop = input.LT(-1);

            if ( (state.backtracking==0) )
            {	retval.Tree = (object)adaptor.RulePostProcessing(root_0);
            	adaptor.SetTokenBoundaries(retval.Tree, (IToken) retval.Start, (IToken) retval.Stop);}
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
            if ( state.backtracking > 0 ) 
            {
            	Memoize(input, 11, inclusiveOrExpression_StartIndex); 
            }
        }
        return retval;
    }
    // $ANTLR end "inclusiveOrExpression"

    public class exclusiveOrExpression_return : ParserRuleReturnScope
    {
        private object tree;
        override public object Tree
        {
        	get { return tree; }
        	set { tree = (object) value; }
        }
    };

    // $ANTLR start "exclusiveOrExpression"
    // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:235:1: exclusiveOrExpression : andExpression ( '^' andExpression )* ;
    public MetraScriptParser.exclusiveOrExpression_return exclusiveOrExpression() // throws RecognitionException [1]
    {   
        MetraScriptParser.exclusiveOrExpression_return retval = new MetraScriptParser.exclusiveOrExpression_return();
        retval.Start = input.LT(1);
        int exclusiveOrExpression_StartIndex = input.Index();
        object root_0 = null;

        IToken char_literal52 = null;
        MetraScriptParser.andExpression_return andExpression51 = default(MetraScriptParser.andExpression_return);

        MetraScriptParser.andExpression_return andExpression53 = default(MetraScriptParser.andExpression_return);


        object char_literal52_tree=null;

        try 
    	{
    	    if ( (state.backtracking > 0) && AlreadyParsedRule(input, 12) ) 
    	    {
    	    	return retval; 
    	    }
            // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:236:2: ( andExpression ( '^' andExpression )* )
            // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:236:4: andExpression ( '^' andExpression )*
            {
            	root_0 = (object)adaptor.GetNilNode();

            	PushFollow(FOLLOW_andExpression_in_exclusiveOrExpression646);
            	andExpression51 = andExpression();
            	state.followingStackPointer--;
            	if (state.failed) return retval;
            	if ( state.backtracking == 0 ) adaptor.AddChild(root_0, andExpression51.Tree);
            	// C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:236:18: ( '^' andExpression )*
            	do 
            	{
            	    int alt9 = 2;
            	    int LA9_0 = input.LA(1);

            	    if ( (LA9_0 == 63) )
            	    {
            	        alt9 = 1;
            	    }


            	    switch (alt9) 
            		{
            			case 1 :
            			    // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:236:20: '^' andExpression
            			    {
            			    	char_literal52=(IToken)Match(input,63,FOLLOW_63_in_exclusiveOrExpression650); if (state.failed) return retval;
            			    	if ( state.backtracking == 0 )
            			    	{char_literal52_tree = (object)adaptor.Create(char_literal52);
            			    		root_0 = (object)adaptor.BecomeRoot(char_literal52_tree, root_0);
            			    	}
            			    	PushFollow(FOLLOW_andExpression_in_exclusiveOrExpression653);
            			    	andExpression53 = andExpression();
            			    	state.followingStackPointer--;
            			    	if (state.failed) return retval;
            			    	if ( state.backtracking == 0 ) adaptor.AddChild(root_0, andExpression53.Tree);

            			    }
            			    break;

            			default:
            			    goto loop9;
            	    }
            	} while (true);

            	loop9:
            		;	// Stops C# compiler whining that label 'loop9' has no statements


            }

            retval.Stop = input.LT(-1);

            if ( (state.backtracking==0) )
            {	retval.Tree = (object)adaptor.RulePostProcessing(root_0);
            	adaptor.SetTokenBoundaries(retval.Tree, (IToken) retval.Start, (IToken) retval.Stop);}
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
            if ( state.backtracking > 0 ) 
            {
            	Memoize(input, 12, exclusiveOrExpression_StartIndex); 
            }
        }
        return retval;
    }
    // $ANTLR end "exclusiveOrExpression"

    public class andExpression_return : ParserRuleReturnScope
    {
        private object tree;
        override public object Tree
        {
        	get { return tree; }
        	set { tree = (object) value; }
        }
    };

    // $ANTLR start "andExpression"
    // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:238:1: andExpression : equalityExpression ( '&' equalityExpression )* ;
    public MetraScriptParser.andExpression_return andExpression() // throws RecognitionException [1]
    {   
        MetraScriptParser.andExpression_return retval = new MetraScriptParser.andExpression_return();
        retval.Start = input.LT(1);
        int andExpression_StartIndex = input.Index();
        object root_0 = null;

        IToken char_literal55 = null;
        MetraScriptParser.equalityExpression_return equalityExpression54 = default(MetraScriptParser.equalityExpression_return);

        MetraScriptParser.equalityExpression_return equalityExpression56 = default(MetraScriptParser.equalityExpression_return);


        object char_literal55_tree=null;

        try 
    	{
    	    if ( (state.backtracking > 0) && AlreadyParsedRule(input, 13) ) 
    	    {
    	    	return retval; 
    	    }
            // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:239:2: ( equalityExpression ( '&' equalityExpression )* )
            // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:239:4: equalityExpression ( '&' equalityExpression )*
            {
            	root_0 = (object)adaptor.GetNilNode();

            	PushFollow(FOLLOW_equalityExpression_in_andExpression666);
            	equalityExpression54 = equalityExpression();
            	state.followingStackPointer--;
            	if (state.failed) return retval;
            	if ( state.backtracking == 0 ) adaptor.AddChild(root_0, equalityExpression54.Tree);
            	// C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:239:23: ( '&' equalityExpression )*
            	do 
            	{
            	    int alt10 = 2;
            	    int LA10_0 = input.LA(1);

            	    if ( (LA10_0 == 64) )
            	    {
            	        alt10 = 1;
            	    }


            	    switch (alt10) 
            		{
            			case 1 :
            			    // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:239:25: '&' equalityExpression
            			    {
            			    	char_literal55=(IToken)Match(input,64,FOLLOW_64_in_andExpression670); if (state.failed) return retval;
            			    	if ( state.backtracking == 0 )
            			    	{char_literal55_tree = (object)adaptor.Create(char_literal55);
            			    		root_0 = (object)adaptor.BecomeRoot(char_literal55_tree, root_0);
            			    	}
            			    	PushFollow(FOLLOW_equalityExpression_in_andExpression673);
            			    	equalityExpression56 = equalityExpression();
            			    	state.followingStackPointer--;
            			    	if (state.failed) return retval;
            			    	if ( state.backtracking == 0 ) adaptor.AddChild(root_0, equalityExpression56.Tree);

            			    }
            			    break;

            			default:
            			    goto loop10;
            	    }
            	} while (true);

            	loop10:
            		;	// Stops C# compiler whining that label 'loop10' has no statements


            }

            retval.Stop = input.LT(-1);

            if ( (state.backtracking==0) )
            {	retval.Tree = (object)adaptor.RulePostProcessing(root_0);
            	adaptor.SetTokenBoundaries(retval.Tree, (IToken) retval.Start, (IToken) retval.Stop);}
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
            if ( state.backtracking > 0 ) 
            {
            	Memoize(input, 13, andExpression_StartIndex); 
            }
        }
        return retval;
    }
    // $ANTLR end "andExpression"

    public class equalityExpression_return : ParserRuleReturnScope
    {
        private object tree;
        override public object Tree
        {
        	get { return tree; }
        	set { tree = (object) value; }
        }
    };

    // $ANTLR start "equalityExpression"
    // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:241:1: equalityExpression : instanceOfExpression ( ( '==' | '!=' | 'eq' | 'ne' | 'Eq' | 'Ne' | 'EQ' | 'NE' | 'eqEQ' | 'EqEQ' | 'neNE' | 'NeNE' ) instanceOfExpression )* ;
    public MetraScriptParser.equalityExpression_return equalityExpression() // throws RecognitionException [1]
    {   
        MetraScriptParser.equalityExpression_return retval = new MetraScriptParser.equalityExpression_return();
        retval.Start = input.LT(1);
        int equalityExpression_StartIndex = input.Index();
        object root_0 = null;

        IToken set58 = null;
        MetraScriptParser.instanceOfExpression_return instanceOfExpression57 = default(MetraScriptParser.instanceOfExpression_return);

        MetraScriptParser.instanceOfExpression_return instanceOfExpression59 = default(MetraScriptParser.instanceOfExpression_return);


        object set58_tree=null;

        try 
    	{
    	    if ( (state.backtracking > 0) && AlreadyParsedRule(input, 14) ) 
    	    {
    	    	return retval; 
    	    }
            // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:242:2: ( instanceOfExpression ( ( '==' | '!=' | 'eq' | 'ne' | 'Eq' | 'Ne' | 'EQ' | 'NE' | 'eqEQ' | 'EqEQ' | 'neNE' | 'NeNE' ) instanceOfExpression )* )
            // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:242:4: instanceOfExpression ( ( '==' | '!=' | 'eq' | 'ne' | 'Eq' | 'Ne' | 'EQ' | 'NE' | 'eqEQ' | 'EqEQ' | 'neNE' | 'NeNE' ) instanceOfExpression )*
            {
            	root_0 = (object)adaptor.GetNilNode();

            	PushFollow(FOLLOW_instanceOfExpression_in_equalityExpression686);
            	instanceOfExpression57 = instanceOfExpression();
            	state.followingStackPointer--;
            	if (state.failed) return retval;
            	if ( state.backtracking == 0 ) adaptor.AddChild(root_0, instanceOfExpression57.Tree);
            	// C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:242:25: ( ( '==' | '!=' | 'eq' | 'ne' | 'Eq' | 'Ne' | 'EQ' | 'NE' | 'eqEQ' | 'EqEQ' | 'neNE' | 'NeNE' ) instanceOfExpression )*
            	do 
            	{
            	    int alt11 = 2;
            	    int LA11_0 = input.LA(1);

            	    if ( ((LA11_0 >= 65 && LA11_0 <= 76)) )
            	    {
            	        alt11 = 1;
            	    }


            	    switch (alt11) 
            		{
            			case 1 :
            			    // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:242:27: ( '==' | '!=' | 'eq' | 'ne' | 'Eq' | 'Ne' | 'EQ' | 'NE' | 'eqEQ' | 'EqEQ' | 'neNE' | 'NeNE' ) instanceOfExpression
            			    {
            			    	set58=(IToken)input.LT(1);
            			    	set58 = (IToken)input.LT(1);
            			    	if ( (input.LA(1) >= 65 && input.LA(1) <= 76) ) 
            			    	{
            			    	    input.Consume();
            			    	    if ( state.backtracking == 0 ) root_0 = (object)adaptor.BecomeRoot((object)adaptor.Create(set58), root_0);
            			    	    state.errorRecovery = false;state.failed = false;
            			    	}
            			    	else 
            			    	{
            			    	    if ( state.backtracking > 0 ) {state.failed = true; return retval;}
            			    	    MismatchedSetException mse = new MismatchedSetException(null,input);
            			    	    throw mse;
            			    	}

            			    	PushFollow(FOLLOW_instanceOfExpression_in_equalityExpression723);
            			    	instanceOfExpression59 = instanceOfExpression();
            			    	state.followingStackPointer--;
            			    	if (state.failed) return retval;
            			    	if ( state.backtracking == 0 ) adaptor.AddChild(root_0, instanceOfExpression59.Tree);

            			    }
            			    break;

            			default:
            			    goto loop11;
            	    }
            	} while (true);

            	loop11:
            		;	// Stops C# compiler whining that label 'loop11' has no statements


            }

            retval.Stop = input.LT(-1);

            if ( (state.backtracking==0) )
            {	retval.Tree = (object)adaptor.RulePostProcessing(root_0);
            	adaptor.SetTokenBoundaries(retval.Tree, (IToken) retval.Start, (IToken) retval.Stop);}
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
            if ( state.backtracking > 0 ) 
            {
            	Memoize(input, 14, equalityExpression_StartIndex); 
            }
        }
        return retval;
    }
    // $ANTLR end "equalityExpression"

    public class instanceOfExpression_return : ParserRuleReturnScope
    {
        private object tree;
        override public object Tree
        {
        	get { return tree; }
        	set { tree = (object) value; }
        }
    };

    // $ANTLR start "instanceOfExpression"
    // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:244:1: instanceOfExpression : relationalExpression ;
    public MetraScriptParser.instanceOfExpression_return instanceOfExpression() // throws RecognitionException [1]
    {   
        MetraScriptParser.instanceOfExpression_return retval = new MetraScriptParser.instanceOfExpression_return();
        retval.Start = input.LT(1);
        int instanceOfExpression_StartIndex = input.Index();
        object root_0 = null;

        MetraScriptParser.relationalExpression_return relationalExpression60 = default(MetraScriptParser.relationalExpression_return);



        try 
    	{
    	    if ( (state.backtracking > 0) && AlreadyParsedRule(input, 15) ) 
    	    {
    	    	return retval; 
    	    }
            // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:245:2: ( relationalExpression )
            // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:245:4: relationalExpression
            {
            	root_0 = (object)adaptor.GetNilNode();

            	PushFollow(FOLLOW_relationalExpression_in_instanceOfExpression736);
            	relationalExpression60 = relationalExpression();
            	state.followingStackPointer--;
            	if (state.failed) return retval;
            	if ( state.backtracking == 0 ) adaptor.AddChild(root_0, relationalExpression60.Tree);

            }

            retval.Stop = input.LT(-1);

            if ( (state.backtracking==0) )
            {	retval.Tree = (object)adaptor.RulePostProcessing(root_0);
            	adaptor.SetTokenBoundaries(retval.Tree, (IToken) retval.Start, (IToken) retval.Stop);}
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
            if ( state.backtracking > 0 ) 
            {
            	Memoize(input, 15, instanceOfExpression_StartIndex); 
            }
        }
        return retval;
    }
    // $ANTLR end "instanceOfExpression"

    public class relationalExpression_return : ParserRuleReturnScope
    {
        private object tree;
        override public object Tree
        {
        	get { return tree; }
        	set { tree = (object) value; }
        }
    };

    // $ANTLR start "relationalExpression"
    // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:247:1: relationalExpression : shiftExpression ( relationalOp shiftExpression )* ;
    public MetraScriptParser.relationalExpression_return relationalExpression() // throws RecognitionException [1]
    {   
        MetraScriptParser.relationalExpression_return retval = new MetraScriptParser.relationalExpression_return();
        retval.Start = input.LT(1);
        int relationalExpression_StartIndex = input.Index();
        object root_0 = null;

        MetraScriptParser.shiftExpression_return shiftExpression61 = default(MetraScriptParser.shiftExpression_return);

        MetraScriptParser.relationalOp_return relationalOp62 = default(MetraScriptParser.relationalOp_return);

        MetraScriptParser.shiftExpression_return shiftExpression63 = default(MetraScriptParser.shiftExpression_return);



        try 
    	{
    	    if ( (state.backtracking > 0) && AlreadyParsedRule(input, 16) ) 
    	    {
    	    	return retval; 
    	    }
            // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:248:2: ( shiftExpression ( relationalOp shiftExpression )* )
            // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:248:4: shiftExpression ( relationalOp shiftExpression )*
            {
            	root_0 = (object)adaptor.GetNilNode();

            	PushFollow(FOLLOW_shiftExpression_in_relationalExpression747);
            	shiftExpression61 = shiftExpression();
            	state.followingStackPointer--;
            	if (state.failed) return retval;
            	if ( state.backtracking == 0 ) adaptor.AddChild(root_0, shiftExpression61.Tree);
            	// C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:248:20: ( relationalOp shiftExpression )*
            	do 
            	{
            	    int alt12 = 2;
            	    switch ( input.LA(1) ) 
            	    {
            	    case 52:
            	    	{
            	        int LA12_2 = input.LA(2);

            	        if ( ((LA12_2 >= Id && LA12_2 <= DecimalLiteral) || (LA12_2 >= 31 && LA12_2 <= 32) || (LA12_2 >= 34 && LA12_2 <= 42) || (LA12_2 >= 89 && LA12_2 <= 90) || (LA12_2 >= 94 && LA12_2 <= 97)) )
            	        {
            	            alt12 = 1;
            	        }


            	        }
            	        break;
            	    case 53:
            	    	{
            	        int LA12_3 = input.LA(2);

            	        if ( ((LA12_3 >= Id && LA12_3 <= DecimalLiteral) || (LA12_3 >= 31 && LA12_3 <= 32) || (LA12_3 >= 34 && LA12_3 <= 42) || (LA12_3 >= 89 && LA12_3 <= 90) || (LA12_3 >= 94 && LA12_3 <= 97)) )
            	        {
            	            alt12 = 1;
            	        }


            	        }
            	        break;
            	    case 77:
            	    case 78:
            	    case 79:
            	    case 80:
            	    case 81:
            	    case 82:
            	    case 83:
            	    case 84:
            	    case 85:
            	    case 86:
            	    case 87:
            	    case 88:
            	    	{
            	        alt12 = 1;
            	        }
            	        break;

            	    }

            	    switch (alt12) 
            		{
            			case 1 :
            			    // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:248:22: relationalOp shiftExpression
            			    {
            			    	PushFollow(FOLLOW_relationalOp_in_relationalExpression751);
            			    	relationalOp62 = relationalOp();
            			    	state.followingStackPointer--;
            			    	if (state.failed) return retval;
            			    	if ( state.backtracking == 0 ) root_0 = (object)adaptor.BecomeRoot(relationalOp62.Tree, root_0);
            			    	PushFollow(FOLLOW_shiftExpression_in_relationalExpression754);
            			    	shiftExpression63 = shiftExpression();
            			    	state.followingStackPointer--;
            			    	if (state.failed) return retval;
            			    	if ( state.backtracking == 0 ) adaptor.AddChild(root_0, shiftExpression63.Tree);

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

            if ( (state.backtracking==0) )
            {	retval.Tree = (object)adaptor.RulePostProcessing(root_0);
            	adaptor.SetTokenBoundaries(retval.Tree, (IToken) retval.Start, (IToken) retval.Stop);}
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
            if ( state.backtracking > 0 ) 
            {
            	Memoize(input, 16, relationalExpression_StartIndex); 
            }
        }
        return retval;
    }
    // $ANTLR end "relationalExpression"

    public class relationalOp_return : ParserRuleReturnScope
    {
        private object tree;
        override public object Tree
        {
        	get { return tree; }
        	set { tree = (object) value; }
        }
    };

    // $ANTLR start "relationalOp"
    // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:250:1: relationalOp : ( ( '<' '=' )=>t1= '<' t2= '=' {...}? | ( '>' '=' )=>t1= '>' t2= '=' {...}? | '<' | '>' | 'gt' | 'lt' | 'gte' | 'lte' | 'Gt' | 'Lt' | 'Gte' | 'Lte' | 'GT' | 'LT' | 'GTE' | 'LTE' );
    public MetraScriptParser.relationalOp_return relationalOp() // throws RecognitionException [1]
    {   
        MetraScriptParser.relationalOp_return retval = new MetraScriptParser.relationalOp_return();
        retval.Start = input.LT(1);
        int relationalOp_StartIndex = input.Index();
        object root_0 = null;

        IToken t1 = null;
        IToken t2 = null;
        IToken char_literal64 = null;
        IToken char_literal65 = null;
        IToken string_literal66 = null;
        IToken string_literal67 = null;
        IToken string_literal68 = null;
        IToken string_literal69 = null;
        IToken string_literal70 = null;
        IToken string_literal71 = null;
        IToken string_literal72 = null;
        IToken string_literal73 = null;
        IToken string_literal74 = null;
        IToken string_literal75 = null;
        IToken string_literal76 = null;
        IToken string_literal77 = null;

        object t1_tree=null;
        object t2_tree=null;
        object char_literal64_tree=null;
        object char_literal65_tree=null;
        object string_literal66_tree=null;
        object string_literal67_tree=null;
        object string_literal68_tree=null;
        object string_literal69_tree=null;
        object string_literal70_tree=null;
        object string_literal71_tree=null;
        object string_literal72_tree=null;
        object string_literal73_tree=null;
        object string_literal74_tree=null;
        object string_literal75_tree=null;
        object string_literal76_tree=null;
        object string_literal77_tree=null;

        try 
    	{
    	    if ( (state.backtracking > 0) && AlreadyParsedRule(input, 17) ) 
    	    {
    	    	return retval; 
    	    }
            // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:251:2: ( ( '<' '=' )=>t1= '<' t2= '=' {...}? | ( '>' '=' )=>t1= '>' t2= '=' {...}? | '<' | '>' | 'gt' | 'lt' | 'gte' | 'lte' | 'Gt' | 'Lt' | 'Gte' | 'Lte' | 'GT' | 'LT' | 'GTE' | 'LTE' )
            int alt13 = 16;
            alt13 = dfa13.Predict(input);
            switch (alt13) 
            {
                case 1 :
                    // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:251:4: ( '<' '=' )=>t1= '<' t2= '=' {...}?
                    {
                    	root_0 = (object)adaptor.GetNilNode();

                    	t1=(IToken)Match(input,52,FOLLOW_52_in_relationalOp776); if (state.failed) return retval;
                    	if ( state.backtracking == 0 )
                    	{t1_tree = (object)adaptor.Create(t1);
                    		adaptor.AddChild(root_0, t1_tree);
                    	}
                    	t2=(IToken)Match(input,41,FOLLOW_41_in_relationalOp780); if (state.failed) return retval;
                    	if ( state.backtracking == 0 )
                    	{t2_tree = (object)adaptor.Create(t2);
                    		adaptor.AddChild(root_0, t2_tree);
                    	}
                    	if ( !(( t1.getLine() == t2.getLine() && 
                    		  t1.getCharPositionInLine() + 1 == t2.getCharPositionInLine() )) ) 
                    	{
                    	    if ( state.backtracking > 0 ) {state.failed = true; return retval;}
                    	    throw new FailedPredicateException(input, "relationalOp", " $t1.getLine() == $t2.getLine() && \r\n\t  $t1.getCharPositionInLine() + 1 == $t2.getCharPositionInLine() ");
                    	}

                    }
                    break;
                case 2 :
                    // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:254:4: ( '>' '=' )=>t1= '>' t2= '=' {...}?
                    {
                    	root_0 = (object)adaptor.GetNilNode();

                    	t1=(IToken)Match(input,53,FOLLOW_53_in_relationalOp800); if (state.failed) return retval;
                    	if ( state.backtracking == 0 )
                    	{t1_tree = (object)adaptor.Create(t1);
                    		adaptor.AddChild(root_0, t1_tree);
                    	}
                    	t2=(IToken)Match(input,41,FOLLOW_41_in_relationalOp804); if (state.failed) return retval;
                    	if ( state.backtracking == 0 )
                    	{t2_tree = (object)adaptor.Create(t2);
                    		adaptor.AddChild(root_0, t2_tree);
                    	}
                    	if ( !(( t1.getLine() == t2.getLine() && 
                    		  t1.getCharPositionInLine() + 1 == t2.getCharPositionInLine() )) ) 
                    	{
                    	    if ( state.backtracking > 0 ) {state.failed = true; return retval;}
                    	    throw new FailedPredicateException(input, "relationalOp", " $t1.getLine() == $t2.getLine() && \r\n\t  $t1.getCharPositionInLine() + 1 == $t2.getCharPositionInLine() ");
                    	}

                    }
                    break;
                case 3 :
                    // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:257:4: '<'
                    {
                    	root_0 = (object)adaptor.GetNilNode();

                    	char_literal64=(IToken)Match(input,52,FOLLOW_52_in_relationalOp815); if (state.failed) return retval;
                    	if ( state.backtracking == 0 )
                    	{char_literal64_tree = (object)adaptor.Create(char_literal64);
                    		adaptor.AddChild(root_0, char_literal64_tree);
                    	}

                    }
                    break;
                case 4 :
                    // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:258:4: '>'
                    {
                    	root_0 = (object)adaptor.GetNilNode();

                    	char_literal65=(IToken)Match(input,53,FOLLOW_53_in_relationalOp821); if (state.failed) return retval;
                    	if ( state.backtracking == 0 )
                    	{char_literal65_tree = (object)adaptor.Create(char_literal65);
                    		adaptor.AddChild(root_0, char_literal65_tree);
                    	}

                    }
                    break;
                case 5 :
                    // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:259:4: 'gt'
                    {
                    	root_0 = (object)adaptor.GetNilNode();

                    	string_literal66=(IToken)Match(input,77,FOLLOW_77_in_relationalOp827); if (state.failed) return retval;
                    	if ( state.backtracking == 0 )
                    	{string_literal66_tree = (object)adaptor.Create(string_literal66);
                    		adaptor.AddChild(root_0, string_literal66_tree);
                    	}

                    }
                    break;
                case 6 :
                    // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:260:4: 'lt'
                    {
                    	root_0 = (object)adaptor.GetNilNode();

                    	string_literal67=(IToken)Match(input,78,FOLLOW_78_in_relationalOp832); if (state.failed) return retval;
                    	if ( state.backtracking == 0 )
                    	{string_literal67_tree = (object)adaptor.Create(string_literal67);
                    		adaptor.AddChild(root_0, string_literal67_tree);
                    	}

                    }
                    break;
                case 7 :
                    // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:262:4: 'gte'
                    {
                    	root_0 = (object)adaptor.GetNilNode();

                    	string_literal68=(IToken)Match(input,79,FOLLOW_79_in_relationalOp839); if (state.failed) return retval;
                    	if ( state.backtracking == 0 )
                    	{string_literal68_tree = (object)adaptor.Create(string_literal68);
                    		adaptor.AddChild(root_0, string_literal68_tree);
                    	}

                    }
                    break;
                case 8 :
                    // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:263:4: 'lte'
                    {
                    	root_0 = (object)adaptor.GetNilNode();

                    	string_literal69=(IToken)Match(input,80,FOLLOW_80_in_relationalOp844); if (state.failed) return retval;
                    	if ( state.backtracking == 0 )
                    	{string_literal69_tree = (object)adaptor.Create(string_literal69);
                    		adaptor.AddChild(root_0, string_literal69_tree);
                    	}

                    }
                    break;
                case 9 :
                    // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:264:4: 'Gt'
                    {
                    	root_0 = (object)adaptor.GetNilNode();

                    	string_literal70=(IToken)Match(input,81,FOLLOW_81_in_relationalOp849); if (state.failed) return retval;
                    	if ( state.backtracking == 0 )
                    	{string_literal70_tree = (object)adaptor.Create(string_literal70);
                    		adaptor.AddChild(root_0, string_literal70_tree);
                    	}

                    }
                    break;
                case 10 :
                    // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:265:4: 'Lt'
                    {
                    	root_0 = (object)adaptor.GetNilNode();

                    	string_literal71=(IToken)Match(input,82,FOLLOW_82_in_relationalOp854); if (state.failed) return retval;
                    	if ( state.backtracking == 0 )
                    	{string_literal71_tree = (object)adaptor.Create(string_literal71);
                    		adaptor.AddChild(root_0, string_literal71_tree);
                    	}

                    }
                    break;
                case 11 :
                    // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:267:4: 'Gte'
                    {
                    	root_0 = (object)adaptor.GetNilNode();

                    	string_literal72=(IToken)Match(input,83,FOLLOW_83_in_relationalOp861); if (state.failed) return retval;
                    	if ( state.backtracking == 0 )
                    	{string_literal72_tree = (object)adaptor.Create(string_literal72);
                    		adaptor.AddChild(root_0, string_literal72_tree);
                    	}

                    }
                    break;
                case 12 :
                    // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:268:4: 'Lte'
                    {
                    	root_0 = (object)adaptor.GetNilNode();

                    	string_literal73=(IToken)Match(input,84,FOLLOW_84_in_relationalOp866); if (state.failed) return retval;
                    	if ( state.backtracking == 0 )
                    	{string_literal73_tree = (object)adaptor.Create(string_literal73);
                    		adaptor.AddChild(root_0, string_literal73_tree);
                    	}

                    }
                    break;
                case 13 :
                    // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:269:4: 'GT'
                    {
                    	root_0 = (object)adaptor.GetNilNode();

                    	string_literal74=(IToken)Match(input,85,FOLLOW_85_in_relationalOp871); if (state.failed) return retval;
                    	if ( state.backtracking == 0 )
                    	{string_literal74_tree = (object)adaptor.Create(string_literal74);
                    		adaptor.AddChild(root_0, string_literal74_tree);
                    	}

                    }
                    break;
                case 14 :
                    // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:270:4: 'LT'
                    {
                    	root_0 = (object)adaptor.GetNilNode();

                    	string_literal75=(IToken)Match(input,86,FOLLOW_86_in_relationalOp876); if (state.failed) return retval;
                    	if ( state.backtracking == 0 )
                    	{string_literal75_tree = (object)adaptor.Create(string_literal75);
                    		adaptor.AddChild(root_0, string_literal75_tree);
                    	}

                    }
                    break;
                case 15 :
                    // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:272:4: 'GTE'
                    {
                    	root_0 = (object)adaptor.GetNilNode();

                    	string_literal76=(IToken)Match(input,87,FOLLOW_87_in_relationalOp883); if (state.failed) return retval;
                    	if ( state.backtracking == 0 )
                    	{string_literal76_tree = (object)adaptor.Create(string_literal76);
                    		adaptor.AddChild(root_0, string_literal76_tree);
                    	}

                    }
                    break;
                case 16 :
                    // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:273:4: 'LTE'
                    {
                    	root_0 = (object)adaptor.GetNilNode();

                    	string_literal77=(IToken)Match(input,88,FOLLOW_88_in_relationalOp888); if (state.failed) return retval;
                    	if ( state.backtracking == 0 )
                    	{string_literal77_tree = (object)adaptor.Create(string_literal77);
                    		adaptor.AddChild(root_0, string_literal77_tree);
                    	}

                    }
                    break;

            }
            retval.Stop = input.LT(-1);

            if ( (state.backtracking==0) )
            {	retval.Tree = (object)adaptor.RulePostProcessing(root_0);
            	adaptor.SetTokenBoundaries(retval.Tree, (IToken) retval.Start, (IToken) retval.Stop);}
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
            if ( state.backtracking > 0 ) 
            {
            	Memoize(input, 17, relationalOp_StartIndex); 
            }
        }
        return retval;
    }
    // $ANTLR end "relationalOp"

    public class shiftExpression_return : ParserRuleReturnScope
    {
        private object tree;
        override public object Tree
        {
        	get { return tree; }
        	set { tree = (object) value; }
        }
    };

    // $ANTLR start "shiftExpression"
    // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:275:1: shiftExpression : additiveExpression ( shiftOp additiveExpression )* ;
    public MetraScriptParser.shiftExpression_return shiftExpression() // throws RecognitionException [1]
    {   
        MetraScriptParser.shiftExpression_return retval = new MetraScriptParser.shiftExpression_return();
        retval.Start = input.LT(1);
        int shiftExpression_StartIndex = input.Index();
        object root_0 = null;

        MetraScriptParser.additiveExpression_return additiveExpression78 = default(MetraScriptParser.additiveExpression_return);

        MetraScriptParser.shiftOp_return shiftOp79 = default(MetraScriptParser.shiftOp_return);

        MetraScriptParser.additiveExpression_return additiveExpression80 = default(MetraScriptParser.additiveExpression_return);



        try 
    	{
    	    if ( (state.backtracking > 0) && AlreadyParsedRule(input, 18) ) 
    	    {
    	    	return retval; 
    	    }
            // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:276:2: ( additiveExpression ( shiftOp additiveExpression )* )
            // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:276:4: additiveExpression ( shiftOp additiveExpression )*
            {
            	root_0 = (object)adaptor.GetNilNode();

            	PushFollow(FOLLOW_additiveExpression_in_shiftExpression898);
            	additiveExpression78 = additiveExpression();
            	state.followingStackPointer--;
            	if (state.failed) return retval;
            	if ( state.backtracking == 0 ) adaptor.AddChild(root_0, additiveExpression78.Tree);
            	// C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:276:23: ( shiftOp additiveExpression )*
            	do 
            	{
            	    int alt14 = 2;
            	    int LA14_0 = input.LA(1);

            	    if ( (LA14_0 == 52) )
            	    {
            	        int LA14_1 = input.LA(2);

            	        if ( (LA14_1 == 52) )
            	        {
            	            int LA14_4 = input.LA(3);

            	            if ( ((LA14_4 >= Id && LA14_4 <= DecimalLiteral) || (LA14_4 >= 31 && LA14_4 <= 32) || (LA14_4 >= 34 && LA14_4 <= 40) || LA14_4 == 42 || (LA14_4 >= 89 && LA14_4 <= 90) || (LA14_4 >= 94 && LA14_4 <= 97)) )
            	            {
            	                alt14 = 1;
            	            }


            	        }


            	    }
            	    else if ( (LA14_0 == 53) )
            	    {
            	        int LA14_2 = input.LA(2);

            	        if ( (LA14_2 == 53) )
            	        {
            	            int LA14_5 = input.LA(3);

            	            if ( (LA14_5 == 53) )
            	            {
            	                int LA14_7 = input.LA(4);

            	                if ( ((LA14_7 >= Id && LA14_7 <= DecimalLiteral) || (LA14_7 >= 31 && LA14_7 <= 32) || (LA14_7 >= 34 && LA14_7 <= 40) || LA14_7 == 42 || (LA14_7 >= 89 && LA14_7 <= 90) || (LA14_7 >= 94 && LA14_7 <= 97)) )
            	                {
            	                    alt14 = 1;
            	                }


            	            }
            	            else if ( ((LA14_5 >= Id && LA14_5 <= DecimalLiteral) || (LA14_5 >= 31 && LA14_5 <= 32) || (LA14_5 >= 34 && LA14_5 <= 40) || LA14_5 == 42 || (LA14_5 >= 89 && LA14_5 <= 90) || (LA14_5 >= 94 && LA14_5 <= 97)) )
            	            {
            	                alt14 = 1;
            	            }


            	        }


            	    }


            	    switch (alt14) 
            		{
            			case 1 :
            			    // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:276:25: shiftOp additiveExpression
            			    {
            			    	PushFollow(FOLLOW_shiftOp_in_shiftExpression902);
            			    	shiftOp79 = shiftOp();
            			    	state.followingStackPointer--;
            			    	if (state.failed) return retval;
            			    	if ( state.backtracking == 0 ) root_0 = (object)adaptor.BecomeRoot(shiftOp79.Tree, root_0);
            			    	PushFollow(FOLLOW_additiveExpression_in_shiftExpression905);
            			    	additiveExpression80 = additiveExpression();
            			    	state.followingStackPointer--;
            			    	if (state.failed) return retval;
            			    	if ( state.backtracking == 0 ) adaptor.AddChild(root_0, additiveExpression80.Tree);

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

            if ( (state.backtracking==0) )
            {	retval.Tree = (object)adaptor.RulePostProcessing(root_0);
            	adaptor.SetTokenBoundaries(retval.Tree, (IToken) retval.Start, (IToken) retval.Stop);}
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
            if ( state.backtracking > 0 ) 
            {
            	Memoize(input, 18, shiftExpression_StartIndex); 
            }
        }
        return retval;
    }
    // $ANTLR end "shiftExpression"

    public class shiftOp_return : ParserRuleReturnScope
    {
        private object tree;
        override public object Tree
        {
        	get { return tree; }
        	set { tree = (object) value; }
        }
    };

    // $ANTLR start "shiftOp"
    // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:278:1: shiftOp : ( ( '<' '<' )=>t1= '<' t2= '<' {...}? | ( '>' '>' '>' )=>t1= '>' t2= '>' t3= '>' {...}? | ( '>' '>' )=>t1= '>' t2= '>' {...}?);
    public MetraScriptParser.shiftOp_return shiftOp() // throws RecognitionException [1]
    {   
        MetraScriptParser.shiftOp_return retval = new MetraScriptParser.shiftOp_return();
        retval.Start = input.LT(1);
        int shiftOp_StartIndex = input.Index();
        object root_0 = null;

        IToken t1 = null;
        IToken t2 = null;
        IToken t3 = null;

        object t1_tree=null;
        object t2_tree=null;
        object t3_tree=null;

        try 
    	{
    	    if ( (state.backtracking > 0) && AlreadyParsedRule(input, 19) ) 
    	    {
    	    	return retval; 
    	    }
            // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:279:2: ( ( '<' '<' )=>t1= '<' t2= '<' {...}? | ( '>' '>' '>' )=>t1= '>' t2= '>' t3= '>' {...}? | ( '>' '>' )=>t1= '>' t2= '>' {...}?)
            int alt15 = 3;
            alt15 = dfa15.Predict(input);
            switch (alt15) 
            {
                case 1 :
                    // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:279:4: ( '<' '<' )=>t1= '<' t2= '<' {...}?
                    {
                    	root_0 = (object)adaptor.GetNilNode();

                    	t1=(IToken)Match(input,52,FOLLOW_52_in_shiftOp927); if (state.failed) return retval;
                    	if ( state.backtracking == 0 )
                    	{t1_tree = (object)adaptor.Create(t1);
                    		adaptor.AddChild(root_0, t1_tree);
                    	}
                    	t2=(IToken)Match(input,52,FOLLOW_52_in_shiftOp931); if (state.failed) return retval;
                    	if ( state.backtracking == 0 )
                    	{t2_tree = (object)adaptor.Create(t2);
                    		adaptor.AddChild(root_0, t2_tree);
                    	}
                    	if ( !(( t1.getLine() == t2.getLine() && 
                    		  t1.getCharPositionInLine() + 1 == t2.getCharPositionInLine() )) ) 
                    	{
                    	    if ( state.backtracking > 0 ) {state.failed = true; return retval;}
                    	    throw new FailedPredicateException(input, "shiftOp", " $t1.getLine() == $t2.getLine() && \r\n\t  $t1.getCharPositionInLine() + 1 == $t2.getCharPositionInLine() ");
                    	}

                    }
                    break;
                case 2 :
                    // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:282:4: ( '>' '>' '>' )=>t1= '>' t2= '>' t3= '>' {...}?
                    {
                    	root_0 = (object)adaptor.GetNilNode();

                    	t1=(IToken)Match(input,53,FOLLOW_53_in_shiftOp953); if (state.failed) return retval;
                    	if ( state.backtracking == 0 )
                    	{t1_tree = (object)adaptor.Create(t1);
                    		adaptor.AddChild(root_0, t1_tree);
                    	}
                    	t2=(IToken)Match(input,53,FOLLOW_53_in_shiftOp957); if (state.failed) return retval;
                    	if ( state.backtracking == 0 )
                    	{t2_tree = (object)adaptor.Create(t2);
                    		adaptor.AddChild(root_0, t2_tree);
                    	}
                    	t3=(IToken)Match(input,53,FOLLOW_53_in_shiftOp961); if (state.failed) return retval;
                    	if ( state.backtracking == 0 )
                    	{t3_tree = (object)adaptor.Create(t3);
                    		adaptor.AddChild(root_0, t3_tree);
                    	}
                    	if ( !(( t1.getLine() == t2.getLine() && 
                    		  t1.getCharPositionInLine() + 1 == t2.getCharPositionInLine() &&
                    		  t2.getLine() == t3.getLine() && 
                    		  t2.getCharPositionInLine() + 1 == t3.getCharPositionInLine() )) ) 
                    	{
                    	    if ( state.backtracking > 0 ) {state.failed = true; return retval;}
                    	    throw new FailedPredicateException(input, "shiftOp", " $t1.getLine() == $t2.getLine() && \r\n\t  $t1.getCharPositionInLine() + 1 == $t2.getCharPositionInLine() &&\r\n\t  $t2.getLine() == $t3.getLine() && \r\n\t  $t2.getCharPositionInLine() + 1 == $t3.getCharPositionInLine() ");
                    	}

                    }
                    break;
                case 3 :
                    // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:287:4: ( '>' '>' )=>t1= '>' t2= '>' {...}?
                    {
                    	root_0 = (object)adaptor.GetNilNode();

                    	t1=(IToken)Match(input,53,FOLLOW_53_in_shiftOp981); if (state.failed) return retval;
                    	if ( state.backtracking == 0 )
                    	{t1_tree = (object)adaptor.Create(t1);
                    		adaptor.AddChild(root_0, t1_tree);
                    	}
                    	t2=(IToken)Match(input,53,FOLLOW_53_in_shiftOp985); if (state.failed) return retval;
                    	if ( state.backtracking == 0 )
                    	{t2_tree = (object)adaptor.Create(t2);
                    		adaptor.AddChild(root_0, t2_tree);
                    	}
                    	if ( !(( t1.getLine() == t2.getLine() && 
                    		  t1.getCharPositionInLine() + 1 == t2.getCharPositionInLine() )) ) 
                    	{
                    	    if ( state.backtracking > 0 ) {state.failed = true; return retval;}
                    	    throw new FailedPredicateException(input, "shiftOp", " $t1.getLine() == $t2.getLine() && \r\n\t  $t1.getCharPositionInLine() + 1 == $t2.getCharPositionInLine() ");
                    	}

                    }
                    break;

            }
            retval.Stop = input.LT(-1);

            if ( (state.backtracking==0) )
            {	retval.Tree = (object)adaptor.RulePostProcessing(root_0);
            	adaptor.SetTokenBoundaries(retval.Tree, (IToken) retval.Start, (IToken) retval.Stop);}
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
            if ( state.backtracking > 0 ) 
            {
            	Memoize(input, 19, shiftOp_StartIndex); 
            }
        }
        return retval;
    }
    // $ANTLR end "shiftOp"

    public class additiveExpression_return : ParserRuleReturnScope
    {
        private object tree;
        override public object Tree
        {
        	get { return tree; }
        	set { tree = (object) value; }
        }
    };

    // $ANTLR start "additiveExpression"
    // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:291:1: additiveExpression : multiplicativeExpression ( ( '+' | '-' ) multiplicativeExpression )* ;
    public MetraScriptParser.additiveExpression_return additiveExpression() // throws RecognitionException [1]
    {   
        MetraScriptParser.additiveExpression_return retval = new MetraScriptParser.additiveExpression_return();
        retval.Start = input.LT(1);
        int additiveExpression_StartIndex = input.Index();
        object root_0 = null;

        IToken set82 = null;
        MetraScriptParser.multiplicativeExpression_return multiplicativeExpression81 = default(MetraScriptParser.multiplicativeExpression_return);

        MetraScriptParser.multiplicativeExpression_return multiplicativeExpression83 = default(MetraScriptParser.multiplicativeExpression_return);


        object set82_tree=null;

        try 
    	{
    	    if ( (state.backtracking > 0) && AlreadyParsedRule(input, 20) ) 
    	    {
    	    	return retval; 
    	    }
            // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:292:2: ( multiplicativeExpression ( ( '+' | '-' ) multiplicativeExpression )* )
            // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:292:4: multiplicativeExpression ( ( '+' | '-' ) multiplicativeExpression )*
            {
            	root_0 = (object)adaptor.GetNilNode();

            	PushFollow(FOLLOW_multiplicativeExpression_in_additiveExpression1000);
            	multiplicativeExpression81 = multiplicativeExpression();
            	state.followingStackPointer--;
            	if (state.failed) return retval;
            	if ( state.backtracking == 0 ) adaptor.AddChild(root_0, multiplicativeExpression81.Tree);
            	// C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:292:29: ( ( '+' | '-' ) multiplicativeExpression )*
            	do 
            	{
            	    int alt16 = 2;
            	    int LA16_0 = input.LA(1);

            	    if ( ((LA16_0 >= 89 && LA16_0 <= 90)) )
            	    {
            	        alt16 = 1;
            	    }


            	    switch (alt16) 
            		{
            			case 1 :
            			    // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:292:31: ( '+' | '-' ) multiplicativeExpression
            			    {
            			    	set82=(IToken)input.LT(1);
            			    	set82 = (IToken)input.LT(1);
            			    	if ( (input.LA(1) >= 89 && input.LA(1) <= 90) ) 
            			    	{
            			    	    input.Consume();
            			    	    if ( state.backtracking == 0 ) root_0 = (object)adaptor.BecomeRoot((object)adaptor.Create(set82), root_0);
            			    	    state.errorRecovery = false;state.failed = false;
            			    	}
            			    	else 
            			    	{
            			    	    if ( state.backtracking > 0 ) {state.failed = true; return retval;}
            			    	    MismatchedSetException mse = new MismatchedSetException(null,input);
            			    	    throw mse;
            			    	}

            			    	PushFollow(FOLLOW_multiplicativeExpression_in_additiveExpression1013);
            			    	multiplicativeExpression83 = multiplicativeExpression();
            			    	state.followingStackPointer--;
            			    	if (state.failed) return retval;
            			    	if ( state.backtracking == 0 ) adaptor.AddChild(root_0, multiplicativeExpression83.Tree);

            			    }
            			    break;

            			default:
            			    goto loop16;
            	    }
            	} while (true);

            	loop16:
            		;	// Stops C# compiler whining that label 'loop16' has no statements


            }

            retval.Stop = input.LT(-1);

            if ( (state.backtracking==0) )
            {	retval.Tree = (object)adaptor.RulePostProcessing(root_0);
            	adaptor.SetTokenBoundaries(retval.Tree, (IToken) retval.Start, (IToken) retval.Stop);}
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
            if ( state.backtracking > 0 ) 
            {
            	Memoize(input, 20, additiveExpression_StartIndex); 
            }
        }
        return retval;
    }
    // $ANTLR end "additiveExpression"

    public class multiplicativeExpression_return : ParserRuleReturnScope
    {
        private object tree;
        override public object Tree
        {
        	get { return tree; }
        	set { tree = (object) value; }
        }
    };

    // $ANTLR start "multiplicativeExpression"
    // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:294:1: multiplicativeExpression : unaryExpression ( ( '*' | '/' | '%' ) unaryExpression )* ;
    public MetraScriptParser.multiplicativeExpression_return multiplicativeExpression() // throws RecognitionException [1]
    {   
        MetraScriptParser.multiplicativeExpression_return retval = new MetraScriptParser.multiplicativeExpression_return();
        retval.Start = input.LT(1);
        int multiplicativeExpression_StartIndex = input.Index();
        object root_0 = null;

        IToken set85 = null;
        MetraScriptParser.unaryExpression_return unaryExpression84 = default(MetraScriptParser.unaryExpression_return);

        MetraScriptParser.unaryExpression_return unaryExpression86 = default(MetraScriptParser.unaryExpression_return);


        object set85_tree=null;

        try 
    	{
    	    if ( (state.backtracking > 0) && AlreadyParsedRule(input, 21) ) 
    	    {
    	    	return retval; 
    	    }
            // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:295:2: ( unaryExpression ( ( '*' | '/' | '%' ) unaryExpression )* )
            // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:295:4: unaryExpression ( ( '*' | '/' | '%' ) unaryExpression )*
            {
            	root_0 = (object)adaptor.GetNilNode();

            	PushFollow(FOLLOW_unaryExpression_in_multiplicativeExpression1026);
            	unaryExpression84 = unaryExpression();
            	state.followingStackPointer--;
            	if (state.failed) return retval;
            	if ( state.backtracking == 0 ) adaptor.AddChild(root_0, unaryExpression84.Tree);
            	// C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:295:20: ( ( '*' | '/' | '%' ) unaryExpression )*
            	do 
            	{
            	    int alt17 = 2;
            	    int LA17_0 = input.LA(1);

            	    if ( ((LA17_0 >= 91 && LA17_0 <= 93)) )
            	    {
            	        alt17 = 1;
            	    }


            	    switch (alt17) 
            		{
            			case 1 :
            			    // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:295:22: ( '*' | '/' | '%' ) unaryExpression
            			    {
            			    	set85=(IToken)input.LT(1);
            			    	set85 = (IToken)input.LT(1);
            			    	if ( (input.LA(1) >= 91 && input.LA(1) <= 93) ) 
            			    	{
            			    	    input.Consume();
            			    	    if ( state.backtracking == 0 ) root_0 = (object)adaptor.BecomeRoot((object)adaptor.Create(set85), root_0);
            			    	    state.errorRecovery = false;state.failed = false;
            			    	}
            			    	else 
            			    	{
            			    	    if ( state.backtracking > 0 ) {state.failed = true; return retval;}
            			    	    MismatchedSetException mse = new MismatchedSetException(null,input);
            			    	    throw mse;
            			    	}

            			    	PushFollow(FOLLOW_unaryExpression_in_multiplicativeExpression1045);
            			    	unaryExpression86 = unaryExpression();
            			    	state.followingStackPointer--;
            			    	if (state.failed) return retval;
            			    	if ( state.backtracking == 0 ) adaptor.AddChild(root_0, unaryExpression86.Tree);

            			    }
            			    break;

            			default:
            			    goto loop17;
            	    }
            	} while (true);

            	loop17:
            		;	// Stops C# compiler whining that label 'loop17' has no statements


            }

            retval.Stop = input.LT(-1);

            if ( (state.backtracking==0) )
            {	retval.Tree = (object)adaptor.RulePostProcessing(root_0);
            	adaptor.SetTokenBoundaries(retval.Tree, (IToken) retval.Start, (IToken) retval.Stop);}
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
            if ( state.backtracking > 0 ) 
            {
            	Memoize(input, 21, multiplicativeExpression_StartIndex); 
            }
        }
        return retval;
    }
    // $ANTLR end "multiplicativeExpression"

    public class unaryExpression_return : ParserRuleReturnScope
    {
        private object tree;
        override public object Tree
        {
        	get { return tree; }
        	set { tree = (object) value; }
        }
    };

    // $ANTLR start "unaryExpression"
    // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:297:1: unaryExpression : ( '+' unaryExpression | '-' unaryExpression | '++' unaryExpression | '--' unaryExpression | unaryExpressionNotPlusMinus );
    public MetraScriptParser.unaryExpression_return unaryExpression() // throws RecognitionException [1]
    {   
        MetraScriptParser.unaryExpression_return retval = new MetraScriptParser.unaryExpression_return();
        retval.Start = input.LT(1);
        int unaryExpression_StartIndex = input.Index();
        object root_0 = null;

        IToken char_literal87 = null;
        IToken char_literal89 = null;
        IToken string_literal91 = null;
        IToken string_literal93 = null;
        MetraScriptParser.unaryExpression_return unaryExpression88 = default(MetraScriptParser.unaryExpression_return);

        MetraScriptParser.unaryExpression_return unaryExpression90 = default(MetraScriptParser.unaryExpression_return);

        MetraScriptParser.unaryExpression_return unaryExpression92 = default(MetraScriptParser.unaryExpression_return);

        MetraScriptParser.unaryExpression_return unaryExpression94 = default(MetraScriptParser.unaryExpression_return);

        MetraScriptParser.unaryExpressionNotPlusMinus_return unaryExpressionNotPlusMinus95 = default(MetraScriptParser.unaryExpressionNotPlusMinus_return);


        object char_literal87_tree=null;
        object char_literal89_tree=null;
        object string_literal91_tree=null;
        object string_literal93_tree=null;

        try 
    	{
    	    if ( (state.backtracking > 0) && AlreadyParsedRule(input, 22) ) 
    	    {
    	    	return retval; 
    	    }
            // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:298:2: ( '+' unaryExpression | '-' unaryExpression | '++' unaryExpression | '--' unaryExpression | unaryExpressionNotPlusMinus )
            int alt18 = 5;
            switch ( input.LA(1) ) 
            {
            case 89:
            	{
                alt18 = 1;
                }
                break;
            case 90:
            	{
                alt18 = 2;
                }
                break;
            case 94:
            	{
                alt18 = 3;
                }
                break;
            case 95:
            	{
                alt18 = 4;
                }
                break;
            case Id:
            case FloatingPointLiteral:
            case StringLiteral:
            case HexLiteral:
            case OctalLiteral:
            case DecimalLiteral:
            case 31:
            case 32:
            case 34:
            case 35:
            case 36:
            case 37:
            case 38:
            case 39:
            case 40:
            case 42:
            case 96:
            case 97:
            	{
                alt18 = 5;
                }
                break;
            	default:
            	    if ( state.backtracking > 0 ) {state.failed = true; return retval;}
            	    NoViableAltException nvae_d18s0 =
            	        new NoViableAltException("", 18, 0, input);

            	    throw nvae_d18s0;
            }

            switch (alt18) 
            {
                case 1 :
                    // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:298:4: '+' unaryExpression
                    {
                    	root_0 = (object)adaptor.GetNilNode();

                    	char_literal87=(IToken)Match(input,89,FOLLOW_89_in_unaryExpression1058); if (state.failed) return retval;
                    	if ( state.backtracking == 0 )
                    	{char_literal87_tree = (object)adaptor.Create(char_literal87);
                    		root_0 = (object)adaptor.BecomeRoot(char_literal87_tree, root_0);
                    	}
                    	PushFollow(FOLLOW_unaryExpression_in_unaryExpression1061);
                    	unaryExpression88 = unaryExpression();
                    	state.followingStackPointer--;
                    	if (state.failed) return retval;
                    	if ( state.backtracking == 0 ) adaptor.AddChild(root_0, unaryExpression88.Tree);

                    }
                    break;
                case 2 :
                    // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:299:4: '-' unaryExpression
                    {
                    	root_0 = (object)adaptor.GetNilNode();

                    	char_literal89=(IToken)Match(input,90,FOLLOW_90_in_unaryExpression1066); if (state.failed) return retval;
                    	if ( state.backtracking == 0 )
                    	{char_literal89_tree = (object)adaptor.Create(char_literal89);
                    		root_0 = (object)adaptor.BecomeRoot(char_literal89_tree, root_0);
                    	}
                    	PushFollow(FOLLOW_unaryExpression_in_unaryExpression1069);
                    	unaryExpression90 = unaryExpression();
                    	state.followingStackPointer--;
                    	if (state.failed) return retval;
                    	if ( state.backtracking == 0 ) adaptor.AddChild(root_0, unaryExpression90.Tree);

                    }
                    break;
                case 3 :
                    // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:300:4: '++' unaryExpression
                    {
                    	root_0 = (object)adaptor.GetNilNode();

                    	string_literal91=(IToken)Match(input,94,FOLLOW_94_in_unaryExpression1074); if (state.failed) return retval;
                    	if ( state.backtracking == 0 )
                    	{string_literal91_tree = (object)adaptor.Create(string_literal91);
                    		root_0 = (object)adaptor.BecomeRoot(string_literal91_tree, root_0);
                    	}
                    	PushFollow(FOLLOW_unaryExpression_in_unaryExpression1077);
                    	unaryExpression92 = unaryExpression();
                    	state.followingStackPointer--;
                    	if (state.failed) return retval;
                    	if ( state.backtracking == 0 ) adaptor.AddChild(root_0, unaryExpression92.Tree);

                    }
                    break;
                case 4 :
                    // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:301:4: '--' unaryExpression
                    {
                    	root_0 = (object)adaptor.GetNilNode();

                    	string_literal93=(IToken)Match(input,95,FOLLOW_95_in_unaryExpression1082); if (state.failed) return retval;
                    	if ( state.backtracking == 0 )
                    	{string_literal93_tree = (object)adaptor.Create(string_literal93);
                    		root_0 = (object)adaptor.BecomeRoot(string_literal93_tree, root_0);
                    	}
                    	PushFollow(FOLLOW_unaryExpression_in_unaryExpression1085);
                    	unaryExpression94 = unaryExpression();
                    	state.followingStackPointer--;
                    	if (state.failed) return retval;
                    	if ( state.backtracking == 0 ) adaptor.AddChild(root_0, unaryExpression94.Tree);

                    }
                    break;
                case 5 :
                    // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:302:4: unaryExpressionNotPlusMinus
                    {
                    	root_0 = (object)adaptor.GetNilNode();

                    	PushFollow(FOLLOW_unaryExpressionNotPlusMinus_in_unaryExpression1090);
                    	unaryExpressionNotPlusMinus95 = unaryExpressionNotPlusMinus();
                    	state.followingStackPointer--;
                    	if (state.failed) return retval;
                    	if ( state.backtracking == 0 ) adaptor.AddChild(root_0, unaryExpressionNotPlusMinus95.Tree);

                    }
                    break;

            }
            retval.Stop = input.LT(-1);

            if ( (state.backtracking==0) )
            {	retval.Tree = (object)adaptor.RulePostProcessing(root_0);
            	adaptor.SetTokenBoundaries(retval.Tree, (IToken) retval.Start, (IToken) retval.Stop);}
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
            if ( state.backtracking > 0 ) 
            {
            	Memoize(input, 22, unaryExpression_StartIndex); 
            }
        }
        return retval;
    }
    // $ANTLR end "unaryExpression"

    public class unaryExpressionNotPlusMinus_return : ParserRuleReturnScope
    {
        private object tree;
        override public object Tree
        {
        	get { return tree; }
        	set { tree = (object) value; }
        }
    };

    // $ANTLR start "unaryExpressionNotPlusMinus"
    // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:304:1: unaryExpressionNotPlusMinus : ( '~' unaryExpression | '!' unaryExpression | castExpression | primary ( '++' | '--' ) | primary );
    public MetraScriptParser.unaryExpressionNotPlusMinus_return unaryExpressionNotPlusMinus() // throws RecognitionException [1]
    {   
        MetraScriptParser.unaryExpressionNotPlusMinus_return retval = new MetraScriptParser.unaryExpressionNotPlusMinus_return();
        retval.Start = input.LT(1);
        int unaryExpressionNotPlusMinus_StartIndex = input.Index();
        object root_0 = null;

        IToken char_literal96 = null;
        IToken char_literal98 = null;
        IToken set102 = null;
        MetraScriptParser.unaryExpression_return unaryExpression97 = default(MetraScriptParser.unaryExpression_return);

        MetraScriptParser.unaryExpression_return unaryExpression99 = default(MetraScriptParser.unaryExpression_return);

        MetraScriptParser.castExpression_return castExpression100 = default(MetraScriptParser.castExpression_return);

        MetraScriptParser.primary_return primary101 = default(MetraScriptParser.primary_return);

        MetraScriptParser.primary_return primary103 = default(MetraScriptParser.primary_return);


        object char_literal96_tree=null;
        object char_literal98_tree=null;
        object set102_tree=null;

        try 
    	{
    	    if ( (state.backtracking > 0) && AlreadyParsedRule(input, 23) ) 
    	    {
    	    	return retval; 
    	    }
            // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:305:2: ( '~' unaryExpression | '!' unaryExpression | castExpression | primary ( '++' | '--' ) | primary )
            int alt19 = 5;
            alt19 = dfa19.Predict(input);
            switch (alt19) 
            {
                case 1 :
                    // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:305:4: '~' unaryExpression
                    {
                    	root_0 = (object)adaptor.GetNilNode();

                    	char_literal96=(IToken)Match(input,42,FOLLOW_42_in_unaryExpressionNotPlusMinus1100); if (state.failed) return retval;
                    	if ( state.backtracking == 0 )
                    	{char_literal96_tree = (object)adaptor.Create(char_literal96);
                    		root_0 = (object)adaptor.BecomeRoot(char_literal96_tree, root_0);
                    	}
                    	PushFollow(FOLLOW_unaryExpression_in_unaryExpressionNotPlusMinus1103);
                    	unaryExpression97 = unaryExpression();
                    	state.followingStackPointer--;
                    	if (state.failed) return retval;
                    	if ( state.backtracking == 0 ) adaptor.AddChild(root_0, unaryExpression97.Tree);

                    }
                    break;
                case 2 :
                    // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:306:4: '!' unaryExpression
                    {
                    	root_0 = (object)adaptor.GetNilNode();

                    	char_literal98=(IToken)Match(input,96,FOLLOW_96_in_unaryExpressionNotPlusMinus1108); if (state.failed) return retval;
                    	if ( state.backtracking == 0 )
                    	{char_literal98_tree = (object)adaptor.Create(char_literal98);
                    		root_0 = (object)adaptor.BecomeRoot(char_literal98_tree, root_0);
                    	}
                    	PushFollow(FOLLOW_unaryExpression_in_unaryExpressionNotPlusMinus1111);
                    	unaryExpression99 = unaryExpression();
                    	state.followingStackPointer--;
                    	if (state.failed) return retval;
                    	if ( state.backtracking == 0 ) adaptor.AddChild(root_0, unaryExpression99.Tree);

                    }
                    break;
                case 3 :
                    // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:307:4: castExpression
                    {
                    	root_0 = (object)adaptor.GetNilNode();

                    	PushFollow(FOLLOW_castExpression_in_unaryExpressionNotPlusMinus1116);
                    	castExpression100 = castExpression();
                    	state.followingStackPointer--;
                    	if (state.failed) return retval;
                    	if ( state.backtracking == 0 ) adaptor.AddChild(root_0, castExpression100.Tree);

                    }
                    break;
                case 4 :
                    // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:308:4: primary ( '++' | '--' )
                    {
                    	root_0 = (object)adaptor.GetNilNode();

                    	PushFollow(FOLLOW_primary_in_unaryExpressionNotPlusMinus1121);
                    	primary101 = primary();
                    	state.followingStackPointer--;
                    	if (state.failed) return retval;
                    	if ( state.backtracking == 0 ) adaptor.AddChild(root_0, primary101.Tree);
                    	set102=(IToken)input.LT(1);
                    	set102 = (IToken)input.LT(1);
                    	if ( (input.LA(1) >= 94 && input.LA(1) <= 95) ) 
                    	{
                    	    input.Consume();
                    	    if ( state.backtracking == 0 ) root_0 = (object)adaptor.BecomeRoot((object)adaptor.Create(set102), root_0);
                    	    state.errorRecovery = false;state.failed = false;
                    	}
                    	else 
                    	{
                    	    if ( state.backtracking > 0 ) {state.failed = true; return retval;}
                    	    MismatchedSetException mse = new MismatchedSetException(null,input);
                    	    throw mse;
                    	}


                    }
                    break;
                case 5 :
                    // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:309:4: primary
                    {
                    	root_0 = (object)adaptor.GetNilNode();

                    	PushFollow(FOLLOW_primary_in_unaryExpressionNotPlusMinus1135);
                    	primary103 = primary();
                    	state.followingStackPointer--;
                    	if (state.failed) return retval;
                    	if ( state.backtracking == 0 ) adaptor.AddChild(root_0, primary103.Tree);

                    }
                    break;

            }
            retval.Stop = input.LT(-1);

            if ( (state.backtracking==0) )
            {	retval.Tree = (object)adaptor.RulePostProcessing(root_0);
            	adaptor.SetTokenBoundaries(retval.Tree, (IToken) retval.Start, (IToken) retval.Stop);}
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
            if ( state.backtracking > 0 ) 
            {
            	Memoize(input, 23, unaryExpressionNotPlusMinus_StartIndex); 
            }
        }
        return retval;
    }
    // $ANTLR end "unaryExpressionNotPlusMinus"

    public class castExpression_return : ParserRuleReturnScope
    {
        private object tree;
        override public object Tree
        {
        	get { return tree; }
        	set { tree = (object) value; }
        }
    };

    // $ANTLR start "castExpression"
    // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:311:1: castExpression : ( '(' Id ')' unaryExpression | '(' Id ')' unaryExpressionNotPlusMinus );
    public MetraScriptParser.castExpression_return castExpression() // throws RecognitionException [1]
    {   
        MetraScriptParser.castExpression_return retval = new MetraScriptParser.castExpression_return();
        retval.Start = input.LT(1);
        int castExpression_StartIndex = input.Index();
        object root_0 = null;

        IToken char_literal104 = null;
        IToken Id105 = null;
        IToken char_literal106 = null;
        IToken char_literal108 = null;
        IToken Id109 = null;
        IToken char_literal110 = null;
        MetraScriptParser.unaryExpression_return unaryExpression107 = default(MetraScriptParser.unaryExpression_return);

        MetraScriptParser.unaryExpressionNotPlusMinus_return unaryExpressionNotPlusMinus111 = default(MetraScriptParser.unaryExpressionNotPlusMinus_return);


        object char_literal104_tree=null;
        object Id105_tree=null;
        object char_literal106_tree=null;
        object char_literal108_tree=null;
        object Id109_tree=null;
        object char_literal110_tree=null;

        try 
    	{
    	    if ( (state.backtracking > 0) && AlreadyParsedRule(input, 24) ) 
    	    {
    	    	return retval; 
    	    }
            // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:312:2: ( '(' Id ')' unaryExpression | '(' Id ')' unaryExpressionNotPlusMinus )
            int alt20 = 2;
            int LA20_0 = input.LA(1);

            if ( (LA20_0 == 97) )
            {
                int LA20_1 = input.LA(2);

                if ( (synpred84_MetraScript()) )
                {
                    alt20 = 1;
                }
                else if ( (true) )
                {
                    alt20 = 2;
                }
                else 
                {
                    if ( state.backtracking > 0 ) {state.failed = true; return retval;}
                    NoViableAltException nvae_d20s1 =
                        new NoViableAltException("", 20, 1, input);

                    throw nvae_d20s1;
                }
            }
            else 
            {
                if ( state.backtracking > 0 ) {state.failed = true; return retval;}
                NoViableAltException nvae_d20s0 =
                    new NoViableAltException("", 20, 0, input);

                throw nvae_d20s0;
            }
            switch (alt20) 
            {
                case 1 :
                    // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:312:5: '(' Id ')' unaryExpression
                    {
                    	root_0 = (object)adaptor.GetNilNode();

                    	char_literal104=(IToken)Match(input,97,FOLLOW_97_in_castExpression1146); if (state.failed) return retval;
                    	if ( state.backtracking == 0 )
                    	{char_literal104_tree = (object)adaptor.Create(char_literal104);
                    		adaptor.AddChild(root_0, char_literal104_tree);
                    	}
                    	Id105=(IToken)Match(input,Id,FOLLOW_Id_in_castExpression1148); if (state.failed) return retval;
                    	if ( state.backtracking == 0 )
                    	{Id105_tree = (object)adaptor.Create(Id105);
                    		adaptor.AddChild(root_0, Id105_tree);
                    	}
                    	char_literal106=(IToken)Match(input,98,FOLLOW_98_in_castExpression1150); if (state.failed) return retval;
                    	if ( state.backtracking == 0 )
                    	{char_literal106_tree = (object)adaptor.Create(char_literal106);
                    		adaptor.AddChild(root_0, char_literal106_tree);
                    	}
                    	PushFollow(FOLLOW_unaryExpression_in_castExpression1152);
                    	unaryExpression107 = unaryExpression();
                    	state.followingStackPointer--;
                    	if (state.failed) return retval;
                    	if ( state.backtracking == 0 ) adaptor.AddChild(root_0, unaryExpression107.Tree);

                    }
                    break;
                case 2 :
                    // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:313:5: '(' Id ')' unaryExpressionNotPlusMinus
                    {
                    	root_0 = (object)adaptor.GetNilNode();

                    	char_literal108=(IToken)Match(input,97,FOLLOW_97_in_castExpression1158); if (state.failed) return retval;
                    	if ( state.backtracking == 0 )
                    	{char_literal108_tree = (object)adaptor.Create(char_literal108);
                    		adaptor.AddChild(root_0, char_literal108_tree);
                    	}
                    	Id109=(IToken)Match(input,Id,FOLLOW_Id_in_castExpression1160); if (state.failed) return retval;
                    	if ( state.backtracking == 0 )
                    	{Id109_tree = (object)adaptor.Create(Id109);
                    		adaptor.AddChild(root_0, Id109_tree);
                    	}
                    	char_literal110=(IToken)Match(input,98,FOLLOW_98_in_castExpression1162); if (state.failed) return retval;
                    	if ( state.backtracking == 0 )
                    	{char_literal110_tree = (object)adaptor.Create(char_literal110);
                    		adaptor.AddChild(root_0, char_literal110_tree);
                    	}
                    	PushFollow(FOLLOW_unaryExpressionNotPlusMinus_in_castExpression1164);
                    	unaryExpressionNotPlusMinus111 = unaryExpressionNotPlusMinus();
                    	state.followingStackPointer--;
                    	if (state.failed) return retval;
                    	if ( state.backtracking == 0 ) adaptor.AddChild(root_0, unaryExpressionNotPlusMinus111.Tree);

                    }
                    break;

            }
            retval.Stop = input.LT(-1);

            if ( (state.backtracking==0) )
            {	retval.Tree = (object)adaptor.RulePostProcessing(root_0);
            	adaptor.SetTokenBoundaries(retval.Tree, (IToken) retval.Start, (IToken) retval.Stop);}
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
            if ( state.backtracking > 0 ) 
            {
            	Memoize(input, 24, castExpression_StartIndex); 
            }
        }
        return retval;
    }
    // $ANTLR end "castExpression"

    public class parExpression_return : ParserRuleReturnScope
    {
        private object tree;
        override public object Tree
        {
        	get { return tree; }
        	set { tree = (object) value; }
        }
    };

    // $ANTLR start "parExpression"
    // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:316:1: parExpression : '(' expression ')' -> expression ;
    public MetraScriptParser.parExpression_return parExpression() // throws RecognitionException [1]
    {   
        MetraScriptParser.parExpression_return retval = new MetraScriptParser.parExpression_return();
        retval.Start = input.LT(1);
        int parExpression_StartIndex = input.Index();
        object root_0 = null;

        IToken char_literal112 = null;
        IToken char_literal114 = null;
        MetraScriptParser.expression_return expression113 = default(MetraScriptParser.expression_return);


        object char_literal112_tree=null;
        object char_literal114_tree=null;
        RewriteRuleTokenStream stream_98 = new RewriteRuleTokenStream(adaptor,"token 98");
        RewriteRuleTokenStream stream_97 = new RewriteRuleTokenStream(adaptor,"token 97");
        RewriteRuleSubtreeStream stream_expression = new RewriteRuleSubtreeStream(adaptor,"rule expression");
        try 
    	{
    	    if ( (state.backtracking > 0) && AlreadyParsedRule(input, 25) ) 
    	    {
    	    	return retval; 
    	    }
            // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:317:2: ( '(' expression ')' -> expression )
            // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:317:4: '(' expression ')'
            {
            	char_literal112=(IToken)Match(input,97,FOLLOW_97_in_parExpression1175); if (state.failed) return retval; 
            	if ( (state.backtracking==0) ) stream_97.Add(char_literal112);

            	PushFollow(FOLLOW_expression_in_parExpression1177);
            	expression113 = expression();
            	state.followingStackPointer--;
            	if (state.failed) return retval;
            	if ( (state.backtracking==0) ) stream_expression.Add(expression113.Tree);
            	char_literal114=(IToken)Match(input,98,FOLLOW_98_in_parExpression1179); if (state.failed) return retval; 
            	if ( (state.backtracking==0) ) stream_98.Add(char_literal114);



            	// AST REWRITE
            	// elements:          expression
            	// token labels:      
            	// rule labels:       retval
            	// token list labels: 
            	// rule list labels:  
            	// wildcard labels: 
            	if ( (state.backtracking==0) ) {
            	retval.Tree = root_0;
            	RewriteRuleSubtreeStream stream_retval = new RewriteRuleSubtreeStream(adaptor, "rule retval", retval!=null ? retval.Tree : null);

            	root_0 = (object)adaptor.GetNilNode();
            	// 317:23: -> expression
            	{
            	    adaptor.AddChild(root_0, stream_expression.NextTree());

            	}

            	retval.Tree = root_0;retval.Tree = root_0;}
            }

            retval.Stop = input.LT(-1);

            if ( (state.backtracking==0) )
            {	retval.Tree = (object)adaptor.RulePostProcessing(root_0);
            	adaptor.SetTokenBoundaries(retval.Tree, (IToken) retval.Start, (IToken) retval.Stop);}
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
            if ( state.backtracking > 0 ) 
            {
            	Memoize(input, 25, parExpression_StartIndex); 
            }
        }
        return retval;
    }
    // $ANTLR end "parExpression"

    public class unit_return : ParserRuleReturnScope
    {
        private object tree;
        override public object Tree
        {
        	get { return tree; }
        	set { tree = (object) value; }
        }
    };

    // $ANTLR start "unit"
    // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:319:1: unit : ( literal | variable | function );
    public MetraScriptParser.unit_return unit() // throws RecognitionException [1]
    {   
        MetraScriptParser.unit_return retval = new MetraScriptParser.unit_return();
        retval.Start = input.LT(1);
        int unit_StartIndex = input.Index();
        object root_0 = null;

        MetraScriptParser.literal_return literal115 = default(MetraScriptParser.literal_return);

        MetraScriptParser.variable_return variable116 = default(MetraScriptParser.variable_return);

        MetraScriptParser.function_return function117 = default(MetraScriptParser.function_return);



        try 
    	{
    	    if ( (state.backtracking > 0) && AlreadyParsedRule(input, 26) ) 
    	    {
    	    	return retval; 
    	    }
            // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:320:2: ( literal | variable | function )
            int alt21 = 3;
            switch ( input.LA(1) ) 
            {
            case FloatingPointLiteral:
            case StringLiteral:
            case HexLiteral:
            case OctalLiteral:
            case DecimalLiteral:
            case 38:
            case 39:
            case 40:
            	{
                alt21 = 1;
                }
                break;
            case 31:
            case 32:
            case 34:
            case 35:
            case 36:
            case 37:
            	{
                alt21 = 2;
                }
                break;
            case Id:
            	{
                alt21 = 3;
                }
                break;
            	default:
            	    if ( state.backtracking > 0 ) {state.failed = true; return retval;}
            	    NoViableAltException nvae_d21s0 =
            	        new NoViableAltException("", 21, 0, input);

            	    throw nvae_d21s0;
            }

            switch (alt21) 
            {
                case 1 :
                    // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:320:4: literal
                    {
                    	root_0 = (object)adaptor.GetNilNode();

                    	PushFollow(FOLLOW_literal_in_unit1193);
                    	literal115 = literal();
                    	state.followingStackPointer--;
                    	if (state.failed) return retval;
                    	if ( state.backtracking == 0 ) adaptor.AddChild(root_0, literal115.Tree);

                    }
                    break;
                case 2 :
                    // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:321:4: variable
                    {
                    	root_0 = (object)adaptor.GetNilNode();

                    	PushFollow(FOLLOW_variable_in_unit1198);
                    	variable116 = variable();
                    	state.followingStackPointer--;
                    	if (state.failed) return retval;
                    	if ( state.backtracking == 0 ) adaptor.AddChild(root_0, variable116.Tree);

                    }
                    break;
                case 3 :
                    // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:322:4: function
                    {
                    	root_0 = (object)adaptor.GetNilNode();

                    	PushFollow(FOLLOW_function_in_unit1203);
                    	function117 = function();
                    	state.followingStackPointer--;
                    	if (state.failed) return retval;
                    	if ( state.backtracking == 0 ) adaptor.AddChild(root_0, function117.Tree);

                    }
                    break;

            }
            retval.Stop = input.LT(-1);

            if ( (state.backtracking==0) )
            {	retval.Tree = (object)adaptor.RulePostProcessing(root_0);
            	adaptor.SetTokenBoundaries(retval.Tree, (IToken) retval.Start, (IToken) retval.Stop);}
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
            if ( state.backtracking > 0 ) 
            {
            	Memoize(input, 26, unit_StartIndex); 
            }
        }
        return retval;
    }
    // $ANTLR end "unit"

    public class primary_return : ParserRuleReturnScope
    {
        private object tree;
        override public object Tree
        {
        	get { return tree; }
        	set { tree = (object) value; }
        }
    };

    // $ANTLR start "primary"
    // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:324:1: primary : ( parExpression | unit );
    public MetraScriptParser.primary_return primary() // throws RecognitionException [1]
    {   
        MetraScriptParser.primary_return retval = new MetraScriptParser.primary_return();
        retval.Start = input.LT(1);
        int primary_StartIndex = input.Index();
        object root_0 = null;

        MetraScriptParser.parExpression_return parExpression118 = default(MetraScriptParser.parExpression_return);

        MetraScriptParser.unit_return unit119 = default(MetraScriptParser.unit_return);



        try 
    	{
    	    if ( (state.backtracking > 0) && AlreadyParsedRule(input, 27) ) 
    	    {
    	    	return retval; 
    	    }
            // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:325:2: ( parExpression | unit )
            int alt22 = 2;
            int LA22_0 = input.LA(1);

            if ( (LA22_0 == 97) )
            {
                alt22 = 1;
            }
            else if ( ((LA22_0 >= Id && LA22_0 <= DecimalLiteral) || (LA22_0 >= 31 && LA22_0 <= 32) || (LA22_0 >= 34 && LA22_0 <= 40)) )
            {
                alt22 = 2;
            }
            else 
            {
                if ( state.backtracking > 0 ) {state.failed = true; return retval;}
                NoViableAltException nvae_d22s0 =
                    new NoViableAltException("", 22, 0, input);

                throw nvae_d22s0;
            }
            switch (alt22) 
            {
                case 1 :
                    // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:325:4: parExpression
                    {
                    	root_0 = (object)adaptor.GetNilNode();

                    	PushFollow(FOLLOW_parExpression_in_primary1213);
                    	parExpression118 = parExpression();
                    	state.followingStackPointer--;
                    	if (state.failed) return retval;
                    	if ( state.backtracking == 0 ) adaptor.AddChild(root_0, parExpression118.Tree);

                    }
                    break;
                case 2 :
                    // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:326:4: unit
                    {
                    	root_0 = (object)adaptor.GetNilNode();

                    	PushFollow(FOLLOW_unit_in_primary1218);
                    	unit119 = unit();
                    	state.followingStackPointer--;
                    	if (state.failed) return retval;
                    	if ( state.backtracking == 0 ) adaptor.AddChild(root_0, unit119.Tree);

                    }
                    break;

            }
            retval.Stop = input.LT(-1);

            if ( (state.backtracking==0) )
            {	retval.Tree = (object)adaptor.RulePostProcessing(root_0);
            	adaptor.SetTokenBoundaries(retval.Tree, (IToken) retval.Start, (IToken) retval.Stop);}
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
            if ( state.backtracking > 0 ) 
            {
            	Memoize(input, 27, primary_StartIndex); 
            }
        }
        return retval;
    }
    // $ANTLR end "primary"

    public class argument_return : ParserRuleReturnScope
    {
        private object tree;
        override public object Tree
        {
        	get { return tree; }
        	set { tree = (object) value; }
        }
    };

    // $ANTLR start "argument"
    // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:329:1: argument : ( Id '=>' expression -> ^( NAMED_ARGUMENT Id expression ) | expression -> ^( ARGUMENT expression ) );
    public MetraScriptParser.argument_return argument() // throws RecognitionException [1]
    {   
        MetraScriptParser.argument_return retval = new MetraScriptParser.argument_return();
        retval.Start = input.LT(1);
        int argument_StartIndex = input.Index();
        object root_0 = null;

        IToken Id120 = null;
        IToken string_literal121 = null;
        MetraScriptParser.expression_return expression122 = default(MetraScriptParser.expression_return);

        MetraScriptParser.expression_return expression123 = default(MetraScriptParser.expression_return);


        object Id120_tree=null;
        object string_literal121_tree=null;
        RewriteRuleTokenStream stream_Id = new RewriteRuleTokenStream(adaptor,"token Id");
        RewriteRuleTokenStream stream_99 = new RewriteRuleTokenStream(adaptor,"token 99");
        RewriteRuleSubtreeStream stream_expression = new RewriteRuleSubtreeStream(adaptor,"rule expression");
        try 
    	{
    	    if ( (state.backtracking > 0) && AlreadyParsedRule(input, 28) ) 
    	    {
    	    	return retval; 
    	    }
            // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:330:2: ( Id '=>' expression -> ^( NAMED_ARGUMENT Id expression ) | expression -> ^( ARGUMENT expression ) )
            int alt23 = 2;
            int LA23_0 = input.LA(1);

            if ( (LA23_0 == Id) )
            {
                int LA23_1 = input.LA(2);

                if ( (LA23_1 == 99) )
                {
                    alt23 = 1;
                }
                else if ( (LA23_1 == 97) )
                {
                    alt23 = 2;
                }
                else 
                {
                    if ( state.backtracking > 0 ) {state.failed = true; return retval;}
                    NoViableAltException nvae_d23s1 =
                        new NoViableAltException("", 23, 1, input);

                    throw nvae_d23s1;
                }
            }
            else if ( ((LA23_0 >= FloatingPointLiteral && LA23_0 <= DecimalLiteral) || (LA23_0 >= 31 && LA23_0 <= 32) || (LA23_0 >= 34 && LA23_0 <= 40) || LA23_0 == 42 || (LA23_0 >= 89 && LA23_0 <= 90) || (LA23_0 >= 94 && LA23_0 <= 97)) )
            {
                alt23 = 2;
            }
            else 
            {
                if ( state.backtracking > 0 ) {state.failed = true; return retval;}
                NoViableAltException nvae_d23s0 =
                    new NoViableAltException("", 23, 0, input);

                throw nvae_d23s0;
            }
            switch (alt23) 
            {
                case 1 :
                    // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:330:4: Id '=>' expression
                    {
                    	Id120=(IToken)Match(input,Id,FOLLOW_Id_in_argument1229); if (state.failed) return retval; 
                    	if ( (state.backtracking==0) ) stream_Id.Add(Id120);

                    	string_literal121=(IToken)Match(input,99,FOLLOW_99_in_argument1231); if (state.failed) return retval; 
                    	if ( (state.backtracking==0) ) stream_99.Add(string_literal121);

                    	PushFollow(FOLLOW_expression_in_argument1233);
                    	expression122 = expression();
                    	state.followingStackPointer--;
                    	if (state.failed) return retval;
                    	if ( (state.backtracking==0) ) stream_expression.Add(expression122.Tree);


                    	// AST REWRITE
                    	// elements:          expression, Id
                    	// token labels:      
                    	// rule labels:       retval
                    	// token list labels: 
                    	// rule list labels:  
                    	// wildcard labels: 
                    	if ( (state.backtracking==0) ) {
                    	retval.Tree = root_0;
                    	RewriteRuleSubtreeStream stream_retval = new RewriteRuleSubtreeStream(adaptor, "rule retval", retval!=null ? retval.Tree : null);

                    	root_0 = (object)adaptor.GetNilNode();
                    	// 330:23: -> ^( NAMED_ARGUMENT Id expression )
                    	{
                    	    // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:330:26: ^( NAMED_ARGUMENT Id expression )
                    	    {
                    	    object root_1 = (object)adaptor.GetNilNode();
                    	    root_1 = (object)adaptor.BecomeRoot((object)adaptor.Create(NAMED_ARGUMENT, "NAMED_ARGUMENT"), root_1);

                    	    adaptor.AddChild(root_1, stream_Id.NextNode());
                    	    adaptor.AddChild(root_1, stream_expression.NextTree());

                    	    adaptor.AddChild(root_0, root_1);
                    	    }

                    	}

                    	retval.Tree = root_0;retval.Tree = root_0;}
                    }
                    break;
                case 2 :
                    // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:331:4: expression
                    {
                    	PushFollow(FOLLOW_expression_in_argument1248);
                    	expression123 = expression();
                    	state.followingStackPointer--;
                    	if (state.failed) return retval;
                    	if ( (state.backtracking==0) ) stream_expression.Add(expression123.Tree);


                    	// AST REWRITE
                    	// elements:          expression
                    	// token labels:      
                    	// rule labels:       retval
                    	// token list labels: 
                    	// rule list labels:  
                    	// wildcard labels: 
                    	if ( (state.backtracking==0) ) {
                    	retval.Tree = root_0;
                    	RewriteRuleSubtreeStream stream_retval = new RewriteRuleSubtreeStream(adaptor, "rule retval", retval!=null ? retval.Tree : null);

                    	root_0 = (object)adaptor.GetNilNode();
                    	// 331:15: -> ^( ARGUMENT expression )
                    	{
                    	    // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:331:18: ^( ARGUMENT expression )
                    	    {
                    	    object root_1 = (object)adaptor.GetNilNode();
                    	    root_1 = (object)adaptor.BecomeRoot((object)adaptor.Create(ARGUMENT, "ARGUMENT"), root_1);

                    	    adaptor.AddChild(root_1, stream_expression.NextTree());

                    	    adaptor.AddChild(root_0, root_1);
                    	    }

                    	}

                    	retval.Tree = root_0;retval.Tree = root_0;}
                    }
                    break;

            }
            retval.Stop = input.LT(-1);

            if ( (state.backtracking==0) )
            {	retval.Tree = (object)adaptor.RulePostProcessing(root_0);
            	adaptor.SetTokenBoundaries(retval.Tree, (IToken) retval.Start, (IToken) retval.Stop);}
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
            if ( state.backtracking > 0 ) 
            {
            	Memoize(input, 28, argument_StartIndex); 
            }
        }
        return retval;
    }
    // $ANTLR end "argument"

    public class argument_list_return : ParserRuleReturnScope
    {
        private object tree;
        override public object Tree
        {
        	get { return tree; }
        	set { tree = (object) value; }
        }
    };

    // $ANTLR start "argument_list"
    // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:334:1: argument_list : argument ( ',' argument )* -> ( argument )+ ;
    public MetraScriptParser.argument_list_return argument_list() // throws RecognitionException [1]
    {   
        MetraScriptParser.argument_list_return retval = new MetraScriptParser.argument_list_return();
        retval.Start = input.LT(1);
        int argument_list_StartIndex = input.Index();
        object root_0 = null;

        IToken char_literal125 = null;
        MetraScriptParser.argument_return argument124 = default(MetraScriptParser.argument_return);

        MetraScriptParser.argument_return argument126 = default(MetraScriptParser.argument_return);


        object char_literal125_tree=null;
        RewriteRuleTokenStream stream_100 = new RewriteRuleTokenStream(adaptor,"token 100");
        RewriteRuleSubtreeStream stream_argument = new RewriteRuleSubtreeStream(adaptor,"rule argument");
        try 
    	{
    	    if ( (state.backtracking > 0) && AlreadyParsedRule(input, 29) ) 
    	    {
    	    	return retval; 
    	    }
            // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:335:2: ( argument ( ',' argument )* -> ( argument )+ )
            // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:335:4: argument ( ',' argument )*
            {
            	PushFollow(FOLLOW_argument_in_argument_list1267);
            	argument124 = argument();
            	state.followingStackPointer--;
            	if (state.failed) return retval;
            	if ( (state.backtracking==0) ) stream_argument.Add(argument124.Tree);
            	// C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:335:13: ( ',' argument )*
            	do 
            	{
            	    int alt24 = 2;
            	    int LA24_0 = input.LA(1);

            	    if ( (LA24_0 == 100) )
            	    {
            	        alt24 = 1;
            	    }


            	    switch (alt24) 
            		{
            			case 1 :
            			    // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:335:14: ',' argument
            			    {
            			    	char_literal125=(IToken)Match(input,100,FOLLOW_100_in_argument_list1270); if (state.failed) return retval; 
            			    	if ( (state.backtracking==0) ) stream_100.Add(char_literal125);

            			    	PushFollow(FOLLOW_argument_in_argument_list1272);
            			    	argument126 = argument();
            			    	state.followingStackPointer--;
            			    	if (state.failed) return retval;
            			    	if ( (state.backtracking==0) ) stream_argument.Add(argument126.Tree);

            			    }
            			    break;

            			default:
            			    goto loop24;
            	    }
            	} while (true);

            	loop24:
            		;	// Stops C# compiler whining that label 'loop24' has no statements



            	// AST REWRITE
            	// elements:          argument
            	// token labels:      
            	// rule labels:       retval
            	// token list labels: 
            	// rule list labels:  
            	// wildcard labels: 
            	if ( (state.backtracking==0) ) {
            	retval.Tree = root_0;
            	RewriteRuleSubtreeStream stream_retval = new RewriteRuleSubtreeStream(adaptor, "rule retval", retval!=null ? retval.Tree : null);

            	root_0 = (object)adaptor.GetNilNode();
            	// 335:29: -> ( argument )+
            	{
            	    if ( !(stream_argument.HasNext()) ) {
            	        throw new RewriteEarlyExitException();
            	    }
            	    while ( stream_argument.HasNext() )
            	    {
            	        adaptor.AddChild(root_0, stream_argument.NextTree());

            	    }
            	    stream_argument.Reset();

            	}

            	retval.Tree = root_0;retval.Tree = root_0;}
            }

            retval.Stop = input.LT(-1);

            if ( (state.backtracking==0) )
            {	retval.Tree = (object)adaptor.RulePostProcessing(root_0);
            	adaptor.SetTokenBoundaries(retval.Tree, (IToken) retval.Start, (IToken) retval.Stop);}
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
            if ( state.backtracking > 0 ) 
            {
            	Memoize(input, 29, argument_list_StartIndex); 
            }
        }
        return retval;
    }
    // $ANTLR end "argument_list"

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
    // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:338:1: function : ( Id '(' ')' -> ^( FUNCTION Id ) | Id '(' argument_list ')' -> ^( FUNCTION Id argument_list ) );
    public MetraScriptParser.function_return function() // throws RecognitionException [1]
    {   
        MetraScriptParser.function_return retval = new MetraScriptParser.function_return();
        retval.Start = input.LT(1);
        int function_StartIndex = input.Index();
        object root_0 = null;

        IToken Id127 = null;
        IToken char_literal128 = null;
        IToken char_literal129 = null;
        IToken Id130 = null;
        IToken char_literal131 = null;
        IToken char_literal133 = null;
        MetraScriptParser.argument_list_return argument_list132 = default(MetraScriptParser.argument_list_return);


        object Id127_tree=null;
        object char_literal128_tree=null;
        object char_literal129_tree=null;
        object Id130_tree=null;
        object char_literal131_tree=null;
        object char_literal133_tree=null;
        RewriteRuleTokenStream stream_98 = new RewriteRuleTokenStream(adaptor,"token 98");
        RewriteRuleTokenStream stream_97 = new RewriteRuleTokenStream(adaptor,"token 97");
        RewriteRuleTokenStream stream_Id = new RewriteRuleTokenStream(adaptor,"token Id");
        RewriteRuleSubtreeStream stream_argument_list = new RewriteRuleSubtreeStream(adaptor,"rule argument_list");
        try 
    	{
    	    if ( (state.backtracking > 0) && AlreadyParsedRule(input, 30) ) 
    	    {
    	    	return retval; 
    	    }
            // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:339:2: ( Id '(' ')' -> ^( FUNCTION Id ) | Id '(' argument_list ')' -> ^( FUNCTION Id argument_list ) )
            int alt25 = 2;
            int LA25_0 = input.LA(1);

            if ( (LA25_0 == Id) )
            {
                int LA25_1 = input.LA(2);

                if ( (LA25_1 == 97) )
                {
                    int LA25_2 = input.LA(3);

                    if ( (LA25_2 == 98) )
                    {
                        alt25 = 1;
                    }
                    else if ( ((LA25_2 >= Id && LA25_2 <= DecimalLiteral) || (LA25_2 >= 31 && LA25_2 <= 32) || (LA25_2 >= 34 && LA25_2 <= 40) || LA25_2 == 42 || (LA25_2 >= 89 && LA25_2 <= 90) || (LA25_2 >= 94 && LA25_2 <= 97)) )
                    {
                        alt25 = 2;
                    }
                    else 
                    {
                        if ( state.backtracking > 0 ) {state.failed = true; return retval;}
                        NoViableAltException nvae_d25s2 =
                            new NoViableAltException("", 25, 2, input);

                        throw nvae_d25s2;
                    }
                }
                else 
                {
                    if ( state.backtracking > 0 ) {state.failed = true; return retval;}
                    NoViableAltException nvae_d25s1 =
                        new NoViableAltException("", 25, 1, input);

                    throw nvae_d25s1;
                }
            }
            else 
            {
                if ( state.backtracking > 0 ) {state.failed = true; return retval;}
                NoViableAltException nvae_d25s0 =
                    new NoViableAltException("", 25, 0, input);

                throw nvae_d25s0;
            }
            switch (alt25) 
            {
                case 1 :
                    // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:339:4: Id '(' ')'
                    {
                    	Id127=(IToken)Match(input,Id,FOLLOW_Id_in_function1290); if (state.failed) return retval; 
                    	if ( (state.backtracking==0) ) stream_Id.Add(Id127);

                    	char_literal128=(IToken)Match(input,97,FOLLOW_97_in_function1292); if (state.failed) return retval; 
                    	if ( (state.backtracking==0) ) stream_97.Add(char_literal128);

                    	char_literal129=(IToken)Match(input,98,FOLLOW_98_in_function1294); if (state.failed) return retval; 
                    	if ( (state.backtracking==0) ) stream_98.Add(char_literal129);



                    	// AST REWRITE
                    	// elements:          Id
                    	// token labels:      
                    	// rule labels:       retval
                    	// token list labels: 
                    	// rule list labels:  
                    	// wildcard labels: 
                    	if ( (state.backtracking==0) ) {
                    	retval.Tree = root_0;
                    	RewriteRuleSubtreeStream stream_retval = new RewriteRuleSubtreeStream(adaptor, "rule retval", retval!=null ? retval.Tree : null);

                    	root_0 = (object)adaptor.GetNilNode();
                    	// 339:15: -> ^( FUNCTION Id )
                    	{
                    	    // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:339:18: ^( FUNCTION Id )
                    	    {
                    	    object root_1 = (object)adaptor.GetNilNode();
                    	    root_1 = (object)adaptor.BecomeRoot((object)adaptor.Create(FUNCTION, "FUNCTION"), root_1);

                    	    adaptor.AddChild(root_1, stream_Id.NextNode());

                    	    adaptor.AddChild(root_0, root_1);
                    	    }

                    	}

                    	retval.Tree = root_0;retval.Tree = root_0;}
                    }
                    break;
                case 2 :
                    // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:340:4: Id '(' argument_list ')'
                    {
                    	Id130=(IToken)Match(input,Id,FOLLOW_Id_in_function1307); if (state.failed) return retval; 
                    	if ( (state.backtracking==0) ) stream_Id.Add(Id130);

                    	char_literal131=(IToken)Match(input,97,FOLLOW_97_in_function1309); if (state.failed) return retval; 
                    	if ( (state.backtracking==0) ) stream_97.Add(char_literal131);

                    	PushFollow(FOLLOW_argument_list_in_function1311);
                    	argument_list132 = argument_list();
                    	state.followingStackPointer--;
                    	if (state.failed) return retval;
                    	if ( (state.backtracking==0) ) stream_argument_list.Add(argument_list132.Tree);
                    	char_literal133=(IToken)Match(input,98,FOLLOW_98_in_function1313); if (state.failed) return retval; 
                    	if ( (state.backtracking==0) ) stream_98.Add(char_literal133);



                    	// AST REWRITE
                    	// elements:          argument_list, Id
                    	// token labels:      
                    	// rule labels:       retval
                    	// token list labels: 
                    	// rule list labels:  
                    	// wildcard labels: 
                    	if ( (state.backtracking==0) ) {
                    	retval.Tree = root_0;
                    	RewriteRuleSubtreeStream stream_retval = new RewriteRuleSubtreeStream(adaptor, "rule retval", retval!=null ? retval.Tree : null);

                    	root_0 = (object)adaptor.GetNilNode();
                    	// 340:29: -> ^( FUNCTION Id argument_list )
                    	{
                    	    // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:340:32: ^( FUNCTION Id argument_list )
                    	    {
                    	    object root_1 = (object)adaptor.GetNilNode();
                    	    root_1 = (object)adaptor.BecomeRoot((object)adaptor.Create(FUNCTION, "FUNCTION"), root_1);

                    	    adaptor.AddChild(root_1, stream_Id.NextNode());
                    	    adaptor.AddChild(root_1, stream_argument_list.NextTree());

                    	    adaptor.AddChild(root_0, root_1);
                    	    }

                    	}

                    	retval.Tree = root_0;retval.Tree = root_0;}
                    }
                    break;

            }
            retval.Stop = input.LT(-1);

            if ( (state.backtracking==0) )
            {	retval.Tree = (object)adaptor.RulePostProcessing(root_0);
            	adaptor.SetTokenBoundaries(retval.Tree, (IToken) retval.Start, (IToken) retval.Stop);}
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
            if ( state.backtracking > 0 ) 
            {
            	Memoize(input, 30, function_StartIndex); 
            }
        }
        return retval;
    }
    // $ANTLR end "function"

    // $ANTLR start "synpred13_MetraScript"
    public void synpred13_MetraScript_fragment() {
        // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:189:27: ( assignmentOperator expression )
        // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:189:27: assignmentOperator expression
        {
        	PushFollow(FOLLOW_assignmentOperator_in_synpred13_MetraScript376);
        	assignmentOperator();
        	state.followingStackPointer--;
        	if (state.failed) return ;
        	PushFollow(FOLLOW_expression_in_synpred13_MetraScript379);
        	expression();
        	state.followingStackPointer--;
        	if (state.failed) return ;

        }
    }
    // $ANTLR end "synpred13_MetraScript"

    // $ANTLR start "synpred25_MetraScript"
    public void synpred25_MetraScript_fragment() {
        // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:203:4: ( '<' '<' '=' )
        // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:203:5: '<' '<' '='
        {
        	Match(input,52,FOLLOW_52_in_synpred25_MetraScript447); if (state.failed) return ;
        	Match(input,52,FOLLOW_52_in_synpred25_MetraScript449); if (state.failed) return ;
        	Match(input,41,FOLLOW_41_in_synpred25_MetraScript451); if (state.failed) return ;

        }
    }
    // $ANTLR end "synpred25_MetraScript"

    // $ANTLR start "synpred26_MetraScript"
    public void synpred26_MetraScript_fragment() {
        // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:208:4: ( '>' '>' '>' '=' )
        // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:208:5: '>' '>' '>' '='
        {
        	Match(input,53,FOLLOW_53_in_synpred26_MetraScript477); if (state.failed) return ;
        	Match(input,53,FOLLOW_53_in_synpred26_MetraScript479); if (state.failed) return ;
        	Match(input,53,FOLLOW_53_in_synpred26_MetraScript481); if (state.failed) return ;
        	Match(input,41,FOLLOW_41_in_synpred26_MetraScript483); if (state.failed) return ;

        }
    }
    // $ANTLR end "synpred26_MetraScript"

    // $ANTLR start "synpred27_MetraScript"
    public void synpred27_MetraScript_fragment() {
        // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:215:4: ( '>' '>' '=' )
        // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:215:5: '>' '>' '='
        {
        	Match(input,53,FOLLOW_53_in_synpred27_MetraScript512); if (state.failed) return ;
        	Match(input,53,FOLLOW_53_in_synpred27_MetraScript514); if (state.failed) return ;
        	Match(input,41,FOLLOW_41_in_synpred27_MetraScript516); if (state.failed) return ;

        }
    }
    // $ANTLR end "synpred27_MetraScript"

    // $ANTLR start "synpred51_MetraScript"
    public void synpred51_MetraScript_fragment() {
        // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:251:4: ( '<' '=' )
        // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:251:5: '<' '='
        {
        	Match(input,52,FOLLOW_52_in_synpred51_MetraScript768); if (state.failed) return ;
        	Match(input,41,FOLLOW_41_in_synpred51_MetraScript770); if (state.failed) return ;

        }
    }
    // $ANTLR end "synpred51_MetraScript"

    // $ANTLR start "synpred52_MetraScript"
    public void synpred52_MetraScript_fragment() {
        // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:254:4: ( '>' '=' )
        // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:254:5: '>' '='
        {
        	Match(input,53,FOLLOW_53_in_synpred52_MetraScript792); if (state.failed) return ;
        	Match(input,41,FOLLOW_41_in_synpred52_MetraScript794); if (state.failed) return ;

        }
    }
    // $ANTLR end "synpred52_MetraScript"

    // $ANTLR start "synpred67_MetraScript"
    public void synpred67_MetraScript_fragment() {
        // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:279:4: ( '<' '<' )
        // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:279:5: '<' '<'
        {
        	Match(input,52,FOLLOW_52_in_synpred67_MetraScript919); if (state.failed) return ;
        	Match(input,52,FOLLOW_52_in_synpred67_MetraScript921); if (state.failed) return ;

        }
    }
    // $ANTLR end "synpred67_MetraScript"

    // $ANTLR start "synpred68_MetraScript"
    public void synpred68_MetraScript_fragment() {
        // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:282:4: ( '>' '>' '>' )
        // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:282:5: '>' '>' '>'
        {
        	Match(input,53,FOLLOW_53_in_synpred68_MetraScript943); if (state.failed) return ;
        	Match(input,53,FOLLOW_53_in_synpred68_MetraScript945); if (state.failed) return ;
        	Match(input,53,FOLLOW_53_in_synpred68_MetraScript947); if (state.failed) return ;

        }
    }
    // $ANTLR end "synpred68_MetraScript"

    // $ANTLR start "synpred69_MetraScript"
    public void synpred69_MetraScript_fragment() {
        // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:287:4: ( '>' '>' )
        // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:287:5: '>' '>'
        {
        	Match(input,53,FOLLOW_53_in_synpred69_MetraScript973); if (state.failed) return ;
        	Match(input,53,FOLLOW_53_in_synpred69_MetraScript975); if (state.failed) return ;

        }
    }
    // $ANTLR end "synpred69_MetraScript"

    // $ANTLR start "synpred81_MetraScript"
    public void synpred81_MetraScript_fragment() {
        // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:307:4: ( castExpression )
        // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:307:4: castExpression
        {
        	PushFollow(FOLLOW_castExpression_in_synpred81_MetraScript1116);
        	castExpression();
        	state.followingStackPointer--;
        	if (state.failed) return ;

        }
    }
    // $ANTLR end "synpred81_MetraScript"

    // $ANTLR start "synpred83_MetraScript"
    public void synpred83_MetraScript_fragment() {
        // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:308:4: ( primary ( '++' | '--' ) )
        // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:308:4: primary ( '++' | '--' )
        {
        	PushFollow(FOLLOW_primary_in_synpred83_MetraScript1121);
        	primary();
        	state.followingStackPointer--;
        	if (state.failed) return ;
        	if ( (input.LA(1) >= 94 && input.LA(1) <= 95) ) 
        	{
        	    input.Consume();
        	    state.errorRecovery = false;state.failed = false;
        	}
        	else 
        	{
        	    if ( state.backtracking > 0 ) {state.failed = true; return ;}
        	    MismatchedSetException mse = new MismatchedSetException(null,input);
        	    throw mse;
        	}


        }
    }
    // $ANTLR end "synpred83_MetraScript"

    // $ANTLR start "synpred84_MetraScript"
    public void synpred84_MetraScript_fragment() {
        // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:312:5: ( '(' Id ')' unaryExpression )
        // C:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\parser\\antlr\\MetraScript.g:312:5: '(' Id ')' unaryExpression
        {
        	Match(input,97,FOLLOW_97_in_synpred84_MetraScript1146); if (state.failed) return ;
        	Match(input,Id,FOLLOW_Id_in_synpred84_MetraScript1148); if (state.failed) return ;
        	Match(input,98,FOLLOW_98_in_synpred84_MetraScript1150); if (state.failed) return ;
        	PushFollow(FOLLOW_unaryExpression_in_synpred84_MetraScript1152);
        	unaryExpression();
        	state.followingStackPointer--;
        	if (state.failed) return ;

        }
    }
    // $ANTLR end "synpred84_MetraScript"

    // Delegated rules

   	public bool synpred67_MetraScript() 
   	{
   	    state.backtracking++;
   	    int start = input.Mark();
   	    try 
   	    {
   	        synpred67_MetraScript_fragment(); // can never throw exception
   	    }
   	    catch (RecognitionException re) 
   	    {
   	        Console.Error.WriteLine("impossible: "+re);
   	    }
   	    bool success = !state.failed;
   	    input.Rewind(start);
   	    state.backtracking--;
   	    state.failed = false;
   	    return success;
   	}
   	public bool synpred69_MetraScript() 
   	{
   	    state.backtracking++;
   	    int start = input.Mark();
   	    try 
   	    {
   	        synpred69_MetraScript_fragment(); // can never throw exception
   	    }
   	    catch (RecognitionException re) 
   	    {
   	        Console.Error.WriteLine("impossible: "+re);
   	    }
   	    bool success = !state.failed;
   	    input.Rewind(start);
   	    state.backtracking--;
   	    state.failed = false;
   	    return success;
   	}
   	public bool synpred68_MetraScript() 
   	{
   	    state.backtracking++;
   	    int start = input.Mark();
   	    try 
   	    {
   	        synpred68_MetraScript_fragment(); // can never throw exception
   	    }
   	    catch (RecognitionException re) 
   	    {
   	        Console.Error.WriteLine("impossible: "+re);
   	    }
   	    bool success = !state.failed;
   	    input.Rewind(start);
   	    state.backtracking--;
   	    state.failed = false;
   	    return success;
   	}
   	public bool synpred81_MetraScript() 
   	{
   	    state.backtracking++;
   	    int start = input.Mark();
   	    try 
   	    {
   	        synpred81_MetraScript_fragment(); // can never throw exception
   	    }
   	    catch (RecognitionException re) 
   	    {
   	        Console.Error.WriteLine("impossible: "+re);
   	    }
   	    bool success = !state.failed;
   	    input.Rewind(start);
   	    state.backtracking--;
   	    state.failed = false;
   	    return success;
   	}
   	public bool synpred51_MetraScript() 
   	{
   	    state.backtracking++;
   	    int start = input.Mark();
   	    try 
   	    {
   	        synpred51_MetraScript_fragment(); // can never throw exception
   	    }
   	    catch (RecognitionException re) 
   	    {
   	        Console.Error.WriteLine("impossible: "+re);
   	    }
   	    bool success = !state.failed;
   	    input.Rewind(start);
   	    state.backtracking--;
   	    state.failed = false;
   	    return success;
   	}
   	public bool synpred26_MetraScript() 
   	{
   	    state.backtracking++;
   	    int start = input.Mark();
   	    try 
   	    {
   	        synpred26_MetraScript_fragment(); // can never throw exception
   	    }
   	    catch (RecognitionException re) 
   	    {
   	        Console.Error.WriteLine("impossible: "+re);
   	    }
   	    bool success = !state.failed;
   	    input.Rewind(start);
   	    state.backtracking--;
   	    state.failed = false;
   	    return success;
   	}
   	public bool synpred84_MetraScript() 
   	{
   	    state.backtracking++;
   	    int start = input.Mark();
   	    try 
   	    {
   	        synpred84_MetraScript_fragment(); // can never throw exception
   	    }
   	    catch (RecognitionException re) 
   	    {
   	        Console.Error.WriteLine("impossible: "+re);
   	    }
   	    bool success = !state.failed;
   	    input.Rewind(start);
   	    state.backtracking--;
   	    state.failed = false;
   	    return success;
   	}
   	public bool synpred27_MetraScript() 
   	{
   	    state.backtracking++;
   	    int start = input.Mark();
   	    try 
   	    {
   	        synpred27_MetraScript_fragment(); // can never throw exception
   	    }
   	    catch (RecognitionException re) 
   	    {
   	        Console.Error.WriteLine("impossible: "+re);
   	    }
   	    bool success = !state.failed;
   	    input.Rewind(start);
   	    state.backtracking--;
   	    state.failed = false;
   	    return success;
   	}
   	public bool synpred25_MetraScript() 
   	{
   	    state.backtracking++;
   	    int start = input.Mark();
   	    try 
   	    {
   	        synpred25_MetraScript_fragment(); // can never throw exception
   	    }
   	    catch (RecognitionException re) 
   	    {
   	        Console.Error.WriteLine("impossible: "+re);
   	    }
   	    bool success = !state.failed;
   	    input.Rewind(start);
   	    state.backtracking--;
   	    state.failed = false;
   	    return success;
   	}
   	public bool synpred83_MetraScript() 
   	{
   	    state.backtracking++;
   	    int start = input.Mark();
   	    try 
   	    {
   	        synpred83_MetraScript_fragment(); // can never throw exception
   	    }
   	    catch (RecognitionException re) 
   	    {
   	        Console.Error.WriteLine("impossible: "+re);
   	    }
   	    bool success = !state.failed;
   	    input.Rewind(start);
   	    state.backtracking--;
   	    state.failed = false;
   	    return success;
   	}
   	public bool synpred13_MetraScript() 
   	{
   	    state.backtracking++;
   	    int start = input.Mark();
   	    try 
   	    {
   	        synpred13_MetraScript_fragment(); // can never throw exception
   	    }
   	    catch (RecognitionException re) 
   	    {
   	        Console.Error.WriteLine("impossible: "+re);
   	    }
   	    bool success = !state.failed;
   	    input.Rewind(start);
   	    state.backtracking--;
   	    state.failed = false;
   	    return success;
   	}
   	public bool synpred52_MetraScript() 
   	{
   	    state.backtracking++;
   	    int start = input.Mark();
   	    try 
   	    {
   	        synpred52_MetraScript_fragment(); // can never throw exception
   	    }
   	    catch (RecognitionException re) 
   	    {
   	        Console.Error.WriteLine("impossible: "+re);
   	    }
   	    bool success = !state.failed;
   	    input.Rewind(start);
   	    state.backtracking--;
   	    state.failed = false;
   	    return success;
   	}


   	protected DFA3 dfa3;
   	protected DFA4 dfa4;
   	protected DFA13 dfa13;
   	protected DFA15 dfa15;
   	protected DFA19 dfa19;
	private void InitializeCyclicDFAs()
	{
    	this.dfa3 = new DFA3(this);
    	this.dfa4 = new DFA4(this);
    	this.dfa13 = new DFA13(this);
    	this.dfa15 = new DFA15(this);
    	this.dfa19 = new DFA19(this);
	    this.dfa3.specialStateTransitionHandler = new DFA.SpecialStateTransitionHandler(DFA3_SpecialStateTransition);
	    this.dfa4.specialStateTransitionHandler = new DFA.SpecialStateTransitionHandler(DFA4_SpecialStateTransition);
	    this.dfa13.specialStateTransitionHandler = new DFA.SpecialStateTransitionHandler(DFA13_SpecialStateTransition);
	    this.dfa15.specialStateTransitionHandler = new DFA.SpecialStateTransitionHandler(DFA15_SpecialStateTransition);
	    this.dfa19.specialStateTransitionHandler = new DFA.SpecialStateTransitionHandler(DFA19_SpecialStateTransition);
	}

    const string DFA3_eotS =
        "\x10\uffff";
    const string DFA3_eofS =
        "\x01\x0e\x0f\uffff";
    const string DFA3_minS =
        "\x01\x21\x0d\x00\x02\uffff";
    const string DFA3_maxS =
        "\x01\x64\x0d\x00\x02\uffff";
    const string DFA3_acceptS =
        "\x0e\uffff\x01\x02\x01\x01";
    const string DFA3_specialS =
        "\x01\uffff\x01\x06\x01\x04\x01\x01\x01\x0b\x01\x08\x01\x05\x01"+
        "\x02\x01\x0c\x01\x09\x01\x00\x01\x03\x01\x07\x01\x0a\x02\uffff}>";
    static readonly string[] DFA3_transitionS = {
            "\x01\x0e\x07\uffff\x01\x01\x01\x02\x01\x03\x01\x04\x01\x05"+
            "\x01\x06\x01\x07\x01\x08\x01\x09\x01\x0a\x01\x0b\x01\x0c\x01"+
            "\x0d\x01\uffff\x01\x0e\x2a\uffff\x01\x0e\x01\uffff\x01\x0e",
            "\x01\uffff",
            "\x01\uffff",
            "\x01\uffff",
            "\x01\uffff",
            "\x01\uffff",
            "\x01\uffff",
            "\x01\uffff",
            "\x01\uffff",
            "\x01\uffff",
            "\x01\uffff",
            "\x01\uffff",
            "\x01\uffff",
            "\x01\uffff",
            "",
            ""
    };

    static readonly short[] DFA3_eot = DFA.UnpackEncodedString(DFA3_eotS);
    static readonly short[] DFA3_eof = DFA.UnpackEncodedString(DFA3_eofS);
    static readonly char[] DFA3_min = DFA.UnpackEncodedStringToUnsignedChars(DFA3_minS);
    static readonly char[] DFA3_max = DFA.UnpackEncodedStringToUnsignedChars(DFA3_maxS);
    static readonly short[] DFA3_accept = DFA.UnpackEncodedString(DFA3_acceptS);
    static readonly short[] DFA3_special = DFA.UnpackEncodedString(DFA3_specialS);
    static readonly short[][] DFA3_transition = DFA.UnpackEncodedStringArray(DFA3_transitionS);

    protected class DFA3 : DFA
    {
        public DFA3(BaseRecognizer recognizer)
        {
            this.recognizer = recognizer;
            this.decisionNumber = 3;
            this.eot = DFA3_eot;
            this.eof = DFA3_eof;
            this.min = DFA3_min;
            this.max = DFA3_max;
            this.accept = DFA3_accept;
            this.special = DFA3_special;
            this.transition = DFA3_transition;

        }

        override public string Description
        {
            get { return "189:26: ( assignmentOperator expression )?"; }
        }

    }


    protected internal int DFA3_SpecialStateTransition(DFA dfa, int s, IIntStream _input) //throws NoViableAltException
    {
            ITokenStream input = (ITokenStream)_input;
    	int _s = s;
        switch ( s )
        {
               	case 0 : 
                   	int LA3_10 = input.LA(1);

                   	 
                   	int index3_10 = input.Index();
                   	input.Rewind();
                   	s = -1;
                   	if ( (synpred13_MetraScript()) ) { s = 15; }

                   	else if ( (true) ) { s = 14; }

                   	 
                   	input.Seek(index3_10);
                   	if ( s >= 0 ) return s;
                   	break;
               	case 1 : 
                   	int LA3_3 = input.LA(1);

                   	 
                   	int index3_3 = input.Index();
                   	input.Rewind();
                   	s = -1;
                   	if ( (synpred13_MetraScript()) ) { s = 15; }

                   	else if ( (true) ) { s = 14; }

                   	 
                   	input.Seek(index3_3);
                   	if ( s >= 0 ) return s;
                   	break;
               	case 2 : 
                   	int LA3_7 = input.LA(1);

                   	 
                   	int index3_7 = input.Index();
                   	input.Rewind();
                   	s = -1;
                   	if ( (synpred13_MetraScript()) ) { s = 15; }

                   	else if ( (true) ) { s = 14; }

                   	 
                   	input.Seek(index3_7);
                   	if ( s >= 0 ) return s;
                   	break;
               	case 3 : 
                   	int LA3_11 = input.LA(1);

                   	 
                   	int index3_11 = input.Index();
                   	input.Rewind();
                   	s = -1;
                   	if ( (synpred13_MetraScript()) ) { s = 15; }

                   	else if ( (true) ) { s = 14; }

                   	 
                   	input.Seek(index3_11);
                   	if ( s >= 0 ) return s;
                   	break;
               	case 4 : 
                   	int LA3_2 = input.LA(1);

                   	 
                   	int index3_2 = input.Index();
                   	input.Rewind();
                   	s = -1;
                   	if ( (synpred13_MetraScript()) ) { s = 15; }

                   	else if ( (true) ) { s = 14; }

                   	 
                   	input.Seek(index3_2);
                   	if ( s >= 0 ) return s;
                   	break;
               	case 5 : 
                   	int LA3_6 = input.LA(1);

                   	 
                   	int index3_6 = input.Index();
                   	input.Rewind();
                   	s = -1;
                   	if ( (synpred13_MetraScript()) ) { s = 15; }

                   	else if ( (true) ) { s = 14; }

                   	 
                   	input.Seek(index3_6);
                   	if ( s >= 0 ) return s;
                   	break;
               	case 6 : 
                   	int LA3_1 = input.LA(1);

                   	 
                   	int index3_1 = input.Index();
                   	input.Rewind();
                   	s = -1;
                   	if ( (synpred13_MetraScript()) ) { s = 15; }

                   	else if ( (true) ) { s = 14; }

                   	 
                   	input.Seek(index3_1);
                   	if ( s >= 0 ) return s;
                   	break;
               	case 7 : 
                   	int LA3_12 = input.LA(1);

                   	 
                   	int index3_12 = input.Index();
                   	input.Rewind();
                   	s = -1;
                   	if ( (synpred13_MetraScript()) ) { s = 15; }

                   	else if ( (true) ) { s = 14; }

                   	 
                   	input.Seek(index3_12);
                   	if ( s >= 0 ) return s;
                   	break;
               	case 8 : 
                   	int LA3_5 = input.LA(1);

                   	 
                   	int index3_5 = input.Index();
                   	input.Rewind();
                   	s = -1;
                   	if ( (synpred13_MetraScript()) ) { s = 15; }

                   	else if ( (true) ) { s = 14; }

                   	 
                   	input.Seek(index3_5);
                   	if ( s >= 0 ) return s;
                   	break;
               	case 9 : 
                   	int LA3_9 = input.LA(1);

                   	 
                   	int index3_9 = input.Index();
                   	input.Rewind();
                   	s = -1;
                   	if ( (synpred13_MetraScript()) ) { s = 15; }

                   	else if ( (true) ) { s = 14; }

                   	 
                   	input.Seek(index3_9);
                   	if ( s >= 0 ) return s;
                   	break;
               	case 10 : 
                   	int LA3_13 = input.LA(1);

                   	 
                   	int index3_13 = input.Index();
                   	input.Rewind();
                   	s = -1;
                   	if ( (synpred13_MetraScript()) ) { s = 15; }

                   	else if ( (true) ) { s = 14; }

                   	 
                   	input.Seek(index3_13);
                   	if ( s >= 0 ) return s;
                   	break;
               	case 11 : 
                   	int LA3_4 = input.LA(1);

                   	 
                   	int index3_4 = input.Index();
                   	input.Rewind();
                   	s = -1;
                   	if ( (synpred13_MetraScript()) ) { s = 15; }

                   	else if ( (true) ) { s = 14; }

                   	 
                   	input.Seek(index3_4);
                   	if ( s >= 0 ) return s;
                   	break;
               	case 12 : 
                   	int LA3_8 = input.LA(1);

                   	 
                   	int index3_8 = input.Index();
                   	input.Rewind();
                   	s = -1;
                   	if ( (synpred13_MetraScript()) ) { s = 15; }

                   	else if ( (true) ) { s = 14; }

                   	 
                   	input.Seek(index3_8);
                   	if ( s >= 0 ) return s;
                   	break;
        }
        if (state.backtracking > 0) {state.failed = true; return -1;}
        NoViableAltException nvae3 =
            new NoViableAltException(dfa.Description, 3, _s, input);
        dfa.Error(nvae3);
        throw nvae3;
    }
    const string DFA4_eotS =
        "\x11\uffff";
    const string DFA4_eofS =
        "\x11\uffff";
    const string DFA4_minS =
        "\x01\x29\x0c\uffff\x01\x35\x01\x29\x02\uffff";
    const string DFA4_maxS =
        "\x01\x35\x0c\uffff\x02\x35\x02\uffff";
    const string DFA4_acceptS =
        "\x01\uffff\x01\x01\x01\x02\x01\x03\x01\x04\x01\x05\x01\x06\x01"+
        "\x07\x01\x08\x01\x09\x01\x0a\x01\x0b\x01\x0c\x02\uffff\x01\x0d\x01"+
        "\x0e";
    const string DFA4_specialS =
        "\x01\x01\x0d\uffff\x01\x00\x02\uffff}>";
    static readonly string[] DFA4_transitionS = {
            "\x01\x01\x01\x02\x01\x03\x01\x04\x01\x05\x01\x06\x01\x07\x01"+
            "\x08\x01\x09\x01\x0a\x01\x0b\x01\x0c\x01\x0d",
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
            "\x01\x0e",
            "\x01\x10\x0b\uffff\x01\x0f",
            "",
            ""
    };

    static readonly short[] DFA4_eot = DFA.UnpackEncodedString(DFA4_eotS);
    static readonly short[] DFA4_eof = DFA.UnpackEncodedString(DFA4_eofS);
    static readonly char[] DFA4_min = DFA.UnpackEncodedStringToUnsignedChars(DFA4_minS);
    static readonly char[] DFA4_max = DFA.UnpackEncodedStringToUnsignedChars(DFA4_maxS);
    static readonly short[] DFA4_accept = DFA.UnpackEncodedString(DFA4_acceptS);
    static readonly short[] DFA4_special = DFA.UnpackEncodedString(DFA4_specialS);
    static readonly short[][] DFA4_transition = DFA.UnpackEncodedStringArray(DFA4_transitionS);

    protected class DFA4 : DFA
    {
        public DFA4(BaseRecognizer recognizer)
        {
            this.recognizer = recognizer;
            this.decisionNumber = 4;
            this.eot = DFA4_eot;
            this.eof = DFA4_eof;
            this.min = DFA4_min;
            this.max = DFA4_max;
            this.accept = DFA4_accept;
            this.special = DFA4_special;
            this.transition = DFA4_transition;

        }

        override public string Description
        {
            get { return "191:1: assignmentOperator : ( '=' | '~' | '+=' | '-=' | '*=' | '/=' | '&=' | '|=' | '^=' | '%=' | '~=' | ( '<' '<' '=' )=>t1= '<' t2= '<' t3= '=' {...}? | ( '>' '>' '>' '=' )=>t1= '>' t2= '>' t3= '>' t4= '=' {...}? | ( '>' '>' '=' )=>t1= '>' t2= '>' t3= '=' {...}?);"; }
        }

    }


    protected internal int DFA4_SpecialStateTransition(DFA dfa, int s, IIntStream _input) //throws NoViableAltException
    {
            ITokenStream input = (ITokenStream)_input;
    	int _s = s;
        switch ( s )
        {
               	case 0 : 
                   	int LA4_14 = input.LA(1);

                   	 
                   	int index4_14 = input.Index();
                   	input.Rewind();
                   	s = -1;
                   	if ( (LA4_14 == 53) && (synpred26_MetraScript()) ) { s = 15; }

                   	else if ( (LA4_14 == 41) && (synpred27_MetraScript()) ) { s = 16; }

                   	 
                   	input.Seek(index4_14);
                   	if ( s >= 0 ) return s;
                   	break;
               	case 1 : 
                   	int LA4_0 = input.LA(1);

                   	 
                   	int index4_0 = input.Index();
                   	input.Rewind();
                   	s = -1;
                   	if ( (LA4_0 == 41) ) { s = 1; }

                   	else if ( (LA4_0 == 42) ) { s = 2; }

                   	else if ( (LA4_0 == 43) ) { s = 3; }

                   	else if ( (LA4_0 == 44) ) { s = 4; }

                   	else if ( (LA4_0 == 45) ) { s = 5; }

                   	else if ( (LA4_0 == 46) ) { s = 6; }

                   	else if ( (LA4_0 == 47) ) { s = 7; }

                   	else if ( (LA4_0 == 48) ) { s = 8; }

                   	else if ( (LA4_0 == 49) ) { s = 9; }

                   	else if ( (LA4_0 == 50) ) { s = 10; }

                   	else if ( (LA4_0 == 51) ) { s = 11; }

                   	else if ( (LA4_0 == 52) && (synpred25_MetraScript()) ) { s = 12; }

                   	else if ( (LA4_0 == 53) ) { s = 13; }

                   	 
                   	input.Seek(index4_0);
                   	if ( s >= 0 ) return s;
                   	break;
        }
        if (state.backtracking > 0) {state.failed = true; return -1;}
        NoViableAltException nvae4 =
            new NoViableAltException(dfa.Description, 4, _s, input);
        dfa.Error(nvae4);
        throw nvae4;
    }
    const string DFA13_eotS =
        "\x13\uffff";
    const string DFA13_eofS =
        "\x13\uffff";
    const string DFA13_minS =
        "\x01\x34\x02\x12\x10\uffff";
    const string DFA13_maxS =
        "\x01\x58\x02\x61\x10\uffff";
    const string DFA13_acceptS =
        "\x03\uffff\x01\x05\x01\x06\x01\x07\x01\x08\x01\x09\x01\x0a\x01"+
        "\x0b\x01\x0c\x01\x0d\x01\x0e\x01\x0f\x01\x10\x01\x01\x01\x03\x01"+
        "\x02\x01\x04";
    const string DFA13_specialS =
        "\x01\uffff\x01\x00\x01\x01\x10\uffff}>";
    static readonly string[] DFA13_transitionS = {
            "\x01\x01\x01\x02\x17\uffff\x01\x03\x01\x04\x01\x05\x01\x06"+
            "\x01\x07\x01\x08\x01\x09\x01\x0a\x01\x0b\x01\x0c\x01\x0d\x01"+
            "\x0e",
            "\x06\x10\x07\uffff\x02\x10\x01\uffff\x07\x10\x01\x0f\x01\x10"+
            "\x2e\uffff\x02\x10\x03\uffff\x04\x10",
            "\x06\x12\x07\uffff\x02\x12\x01\uffff\x07\x12\x01\x11\x01\x12"+
            "\x2e\uffff\x02\x12\x03\uffff\x04\x12",
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
            ""
    };

    static readonly short[] DFA13_eot = DFA.UnpackEncodedString(DFA13_eotS);
    static readonly short[] DFA13_eof = DFA.UnpackEncodedString(DFA13_eofS);
    static readonly char[] DFA13_min = DFA.UnpackEncodedStringToUnsignedChars(DFA13_minS);
    static readonly char[] DFA13_max = DFA.UnpackEncodedStringToUnsignedChars(DFA13_maxS);
    static readonly short[] DFA13_accept = DFA.UnpackEncodedString(DFA13_acceptS);
    static readonly short[] DFA13_special = DFA.UnpackEncodedString(DFA13_specialS);
    static readonly short[][] DFA13_transition = DFA.UnpackEncodedStringArray(DFA13_transitionS);

    protected class DFA13 : DFA
    {
        public DFA13(BaseRecognizer recognizer)
        {
            this.recognizer = recognizer;
            this.decisionNumber = 13;
            this.eot = DFA13_eot;
            this.eof = DFA13_eof;
            this.min = DFA13_min;
            this.max = DFA13_max;
            this.accept = DFA13_accept;
            this.special = DFA13_special;
            this.transition = DFA13_transition;

        }

        override public string Description
        {
            get { return "250:1: relationalOp : ( ( '<' '=' )=>t1= '<' t2= '=' {...}? | ( '>' '=' )=>t1= '>' t2= '=' {...}? | '<' | '>' | 'gt' | 'lt' | 'gte' | 'lte' | 'Gt' | 'Lt' | 'Gte' | 'Lte' | 'GT' | 'LT' | 'GTE' | 'LTE' );"; }
        }

    }


    protected internal int DFA13_SpecialStateTransition(DFA dfa, int s, IIntStream _input) //throws NoViableAltException
    {
            ITokenStream input = (ITokenStream)_input;
    	int _s = s;
        switch ( s )
        {
               	case 0 : 
                   	int LA13_1 = input.LA(1);

                   	 
                   	int index13_1 = input.Index();
                   	input.Rewind();
                   	s = -1;
                   	if ( (LA13_1 == 41) && (synpred51_MetraScript()) ) { s = 15; }

                   	else if ( ((LA13_1 >= Id && LA13_1 <= DecimalLiteral) || (LA13_1 >= 31 && LA13_1 <= 32) || (LA13_1 >= 34 && LA13_1 <= 40) || LA13_1 == 42 || (LA13_1 >= 89 && LA13_1 <= 90) || (LA13_1 >= 94 && LA13_1 <= 97)) ) { s = 16; }

                   	 
                   	input.Seek(index13_1);
                   	if ( s >= 0 ) return s;
                   	break;
               	case 1 : 
                   	int LA13_2 = input.LA(1);

                   	 
                   	int index13_2 = input.Index();
                   	input.Rewind();
                   	s = -1;
                   	if ( (LA13_2 == 41) && (synpred52_MetraScript()) ) { s = 17; }

                   	else if ( ((LA13_2 >= Id && LA13_2 <= DecimalLiteral) || (LA13_2 >= 31 && LA13_2 <= 32) || (LA13_2 >= 34 && LA13_2 <= 40) || LA13_2 == 42 || (LA13_2 >= 89 && LA13_2 <= 90) || (LA13_2 >= 94 && LA13_2 <= 97)) ) { s = 18; }

                   	 
                   	input.Seek(index13_2);
                   	if ( s >= 0 ) return s;
                   	break;
        }
        if (state.backtracking > 0) {state.failed = true; return -1;}
        NoViableAltException nvae13 =
            new NoViableAltException(dfa.Description, 13, _s, input);
        dfa.Error(nvae13);
        throw nvae13;
    }
    const string DFA15_eotS =
        "\x18\uffff";
    const string DFA15_eofS =
        "\x18\uffff";
    const string DFA15_minS =
        "\x01\x34\x01\uffff\x01\x35\x01\x12\x14\uffff";
    const string DFA15_maxS =
        "\x01\x35\x01\uffff\x01\x35\x01\x61\x14\uffff";
    const string DFA15_acceptS =
        "\x01\uffff\x01\x01\x02\uffff\x01\x02\x13\x03";
    const string DFA15_specialS =
        "\x01\x01\x02\uffff\x01\x00\x14\uffff}>";
    static readonly string[] DFA15_transitionS = {
            "\x01\x01\x01\x02",
            "",
            "\x01\x03",
            "\x01\x17\x01\x0d\x01\x0e\x03\x0c\x07\uffff\x01\x11\x01\x12"+
            "\x01\uffff\x01\x13\x01\x14\x01\x15\x01\x16\x01\x10\x02\x0f\x01"+
            "\uffff\x01\x09\x0a\uffff\x01\x04\x23\uffff\x01\x05\x01\x06\x03"+
            "\uffff\x01\x07\x01\x08\x01\x0a\x01\x0b",
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
            ""
    };

    static readonly short[] DFA15_eot = DFA.UnpackEncodedString(DFA15_eotS);
    static readonly short[] DFA15_eof = DFA.UnpackEncodedString(DFA15_eofS);
    static readonly char[] DFA15_min = DFA.UnpackEncodedStringToUnsignedChars(DFA15_minS);
    static readonly char[] DFA15_max = DFA.UnpackEncodedStringToUnsignedChars(DFA15_maxS);
    static readonly short[] DFA15_accept = DFA.UnpackEncodedString(DFA15_acceptS);
    static readonly short[] DFA15_special = DFA.UnpackEncodedString(DFA15_specialS);
    static readonly short[][] DFA15_transition = DFA.UnpackEncodedStringArray(DFA15_transitionS);

    protected class DFA15 : DFA
    {
        public DFA15(BaseRecognizer recognizer)
        {
            this.recognizer = recognizer;
            this.decisionNumber = 15;
            this.eot = DFA15_eot;
            this.eof = DFA15_eof;
            this.min = DFA15_min;
            this.max = DFA15_max;
            this.accept = DFA15_accept;
            this.special = DFA15_special;
            this.transition = DFA15_transition;

        }

        override public string Description
        {
            get { return "278:1: shiftOp : ( ( '<' '<' )=>t1= '<' t2= '<' {...}? | ( '>' '>' '>' )=>t1= '>' t2= '>' t3= '>' {...}? | ( '>' '>' )=>t1= '>' t2= '>' {...}?);"; }
        }

    }


    protected internal int DFA15_SpecialStateTransition(DFA dfa, int s, IIntStream _input) //throws NoViableAltException
    {
            ITokenStream input = (ITokenStream)_input;
    	int _s = s;
        switch ( s )
        {
               	case 0 : 
                   	int LA15_3 = input.LA(1);

                   	 
                   	int index15_3 = input.Index();
                   	input.Rewind();
                   	s = -1;
                   	if ( (LA15_3 == 53) && (synpred68_MetraScript()) ) { s = 4; }

                   	else if ( (LA15_3 == 89) && (synpred69_MetraScript()) ) { s = 5; }

                   	else if ( (LA15_3 == 90) && (synpred69_MetraScript()) ) { s = 6; }

                   	else if ( (LA15_3 == 94) && (synpred69_MetraScript()) ) { s = 7; }

                   	else if ( (LA15_3 == 95) && (synpred69_MetraScript()) ) { s = 8; }

                   	else if ( (LA15_3 == 42) && (synpred69_MetraScript()) ) { s = 9; }

                   	else if ( (LA15_3 == 96) && (synpred69_MetraScript()) ) { s = 10; }

                   	else if ( (LA15_3 == 97) && (synpred69_MetraScript()) ) { s = 11; }

                   	else if ( ((LA15_3 >= HexLiteral && LA15_3 <= DecimalLiteral)) && (synpred69_MetraScript()) ) { s = 12; }

                   	else if ( (LA15_3 == FloatingPointLiteral) && (synpred69_MetraScript()) ) { s = 13; }

                   	else if ( (LA15_3 == StringLiteral) && (synpred69_MetraScript()) ) { s = 14; }

                   	else if ( ((LA15_3 >= 39 && LA15_3 <= 40)) && (synpred69_MetraScript()) ) { s = 15; }

                   	else if ( (LA15_3 == 38) && (synpred69_MetraScript()) ) { s = 16; }

                   	else if ( (LA15_3 == 31) && (synpred69_MetraScript()) ) { s = 17; }

                   	else if ( (LA15_3 == 32) && (synpred69_MetraScript()) ) { s = 18; }

                   	else if ( (LA15_3 == 34) && (synpred69_MetraScript()) ) { s = 19; }

                   	else if ( (LA15_3 == 35) && (synpred69_MetraScript()) ) { s = 20; }

                   	else if ( (LA15_3 == 36) && (synpred69_MetraScript()) ) { s = 21; }

                   	else if ( (LA15_3 == 37) && (synpred69_MetraScript()) ) { s = 22; }

                   	else if ( (LA15_3 == Id) && (synpred69_MetraScript()) ) { s = 23; }

                   	 
                   	input.Seek(index15_3);
                   	if ( s >= 0 ) return s;
                   	break;
               	case 1 : 
                   	int LA15_0 = input.LA(1);

                   	 
                   	int index15_0 = input.Index();
                   	input.Rewind();
                   	s = -1;
                   	if ( (LA15_0 == 52) && (synpred67_MetraScript()) ) { s = 1; }

                   	else if ( (LA15_0 == 53) ) { s = 2; }

                   	 
                   	input.Seek(index15_0);
                   	if ( s >= 0 ) return s;
                   	break;
        }
        if (state.backtracking > 0) {state.failed = true; return -1;}
        NoViableAltException nvae15 =
            new NoViableAltException(dfa.Description, 15, _s, input);
        dfa.Error(nvae15);
        throw nvae15;
    }
    const string DFA19_eotS =
        "\x13\uffff";
    const string DFA19_eofS =
        "\x13\uffff";
    const string DFA19_minS =
        "\x01\x12\x02\uffff\x0d\x00\x03\uffff";
    const string DFA19_maxS =
        "\x01\x61\x02\uffff\x0d\x00\x03\uffff";
    const string DFA19_acceptS =
        "\x01\uffff\x01\x01\x01\x02\x0d\uffff\x01\x03\x01\x04\x01\x05";
    const string DFA19_specialS =
        "\x03\uffff\x01\x00\x01\x01\x01\x02\x01\x03\x01\x04\x01\x05\x01"+
        "\x06\x01\x07\x01\x08\x01\x09\x01\x0a\x01\x0b\x01\x0c\x03\uffff}>";
    static readonly string[] DFA19_transitionS = {
            "\x01\x0f\x01\x05\x01\x06\x03\x04\x07\uffff\x01\x09\x01\x0a"+
            "\x01\uffff\x01\x0b\x01\x0c\x01\x0d\x01\x0e\x01\x08\x02\x07\x01"+
            "\uffff\x01\x01\x35\uffff\x01\x02\x01\x03",
            "",
            "",
            "\x01\uffff",
            "\x01\uffff",
            "\x01\uffff",
            "\x01\uffff",
            "\x01\uffff",
            "\x01\uffff",
            "\x01\uffff",
            "\x01\uffff",
            "\x01\uffff",
            "\x01\uffff",
            "\x01\uffff",
            "\x01\uffff",
            "\x01\uffff",
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
            get { return "304:1: unaryExpressionNotPlusMinus : ( '~' unaryExpression | '!' unaryExpression | castExpression | primary ( '++' | '--' ) | primary );"; }
        }

    }


    protected internal int DFA19_SpecialStateTransition(DFA dfa, int s, IIntStream _input) //throws NoViableAltException
    {
            ITokenStream input = (ITokenStream)_input;
    	int _s = s;
        switch ( s )
        {
               	case 0 : 
                   	int LA19_3 = input.LA(1);

                   	 
                   	int index19_3 = input.Index();
                   	input.Rewind();
                   	s = -1;
                   	if ( (synpred81_MetraScript()) ) { s = 16; }

                   	else if ( (synpred83_MetraScript()) ) { s = 17; }

                   	else if ( (true) ) { s = 18; }

                   	 
                   	input.Seek(index19_3);
                   	if ( s >= 0 ) return s;
                   	break;
               	case 1 : 
                   	int LA19_4 = input.LA(1);

                   	 
                   	int index19_4 = input.Index();
                   	input.Rewind();
                   	s = -1;
                   	if ( (synpred83_MetraScript()) ) { s = 17; }

                   	else if ( (true) ) { s = 18; }

                   	 
                   	input.Seek(index19_4);
                   	if ( s >= 0 ) return s;
                   	break;
               	case 2 : 
                   	int LA19_5 = input.LA(1);

                   	 
                   	int index19_5 = input.Index();
                   	input.Rewind();
                   	s = -1;
                   	if ( (synpred83_MetraScript()) ) { s = 17; }

                   	else if ( (true) ) { s = 18; }

                   	 
                   	input.Seek(index19_5);
                   	if ( s >= 0 ) return s;
                   	break;
               	case 3 : 
                   	int LA19_6 = input.LA(1);

                   	 
                   	int index19_6 = input.Index();
                   	input.Rewind();
                   	s = -1;
                   	if ( (synpred83_MetraScript()) ) { s = 17; }

                   	else if ( (true) ) { s = 18; }

                   	 
                   	input.Seek(index19_6);
                   	if ( s >= 0 ) return s;
                   	break;
               	case 4 : 
                   	int LA19_7 = input.LA(1);

                   	 
                   	int index19_7 = input.Index();
                   	input.Rewind();
                   	s = -1;
                   	if ( (synpred83_MetraScript()) ) { s = 17; }

                   	else if ( (true) ) { s = 18; }

                   	 
                   	input.Seek(index19_7);
                   	if ( s >= 0 ) return s;
                   	break;
               	case 5 : 
                   	int LA19_8 = input.LA(1);

                   	 
                   	int index19_8 = input.Index();
                   	input.Rewind();
                   	s = -1;
                   	if ( (synpred83_MetraScript()) ) { s = 17; }

                   	else if ( (true) ) { s = 18; }

                   	 
                   	input.Seek(index19_8);
                   	if ( s >= 0 ) return s;
                   	break;
               	case 6 : 
                   	int LA19_9 = input.LA(1);

                   	 
                   	int index19_9 = input.Index();
                   	input.Rewind();
                   	s = -1;
                   	if ( (synpred83_MetraScript()) ) { s = 17; }

                   	else if ( (true) ) { s = 18; }

                   	 
                   	input.Seek(index19_9);
                   	if ( s >= 0 ) return s;
                   	break;
               	case 7 : 
                   	int LA19_10 = input.LA(1);

                   	 
                   	int index19_10 = input.Index();
                   	input.Rewind();
                   	s = -1;
                   	if ( (synpred83_MetraScript()) ) { s = 17; }

                   	else if ( (true) ) { s = 18; }

                   	 
                   	input.Seek(index19_10);
                   	if ( s >= 0 ) return s;
                   	break;
               	case 8 : 
                   	int LA19_11 = input.LA(1);

                   	 
                   	int index19_11 = input.Index();
                   	input.Rewind();
                   	s = -1;
                   	if ( (synpred83_MetraScript()) ) { s = 17; }

                   	else if ( (true) ) { s = 18; }

                   	 
                   	input.Seek(index19_11);
                   	if ( s >= 0 ) return s;
                   	break;
               	case 9 : 
                   	int LA19_12 = input.LA(1);

                   	 
                   	int index19_12 = input.Index();
                   	input.Rewind();
                   	s = -1;
                   	if ( (synpred83_MetraScript()) ) { s = 17; }

                   	else if ( (true) ) { s = 18; }

                   	 
                   	input.Seek(index19_12);
                   	if ( s >= 0 ) return s;
                   	break;
               	case 10 : 
                   	int LA19_13 = input.LA(1);

                   	 
                   	int index19_13 = input.Index();
                   	input.Rewind();
                   	s = -1;
                   	if ( (synpred83_MetraScript()) ) { s = 17; }

                   	else if ( (true) ) { s = 18; }

                   	 
                   	input.Seek(index19_13);
                   	if ( s >= 0 ) return s;
                   	break;
               	case 11 : 
                   	int LA19_14 = input.LA(1);

                   	 
                   	int index19_14 = input.Index();
                   	input.Rewind();
                   	s = -1;
                   	if ( (synpred83_MetraScript()) ) { s = 17; }

                   	else if ( (true) ) { s = 18; }

                   	 
                   	input.Seek(index19_14);
                   	if ( s >= 0 ) return s;
                   	break;
               	case 12 : 
                   	int LA19_15 = input.LA(1);

                   	 
                   	int index19_15 = input.Index();
                   	input.Rewind();
                   	s = -1;
                   	if ( (synpred83_MetraScript()) ) { s = 17; }

                   	else if ( (true) ) { s = 18; }

                   	 
                   	input.Seek(index19_15);
                   	if ( s >= 0 ) return s;
                   	break;
        }
        if (state.backtracking > 0) {state.failed = true; return -1;}
        NoViableAltException nvae19 =
            new NoViableAltException(dfa.Description, 19, _s, input);
        dfa.Error(nvae19);
        throw nvae19;
    }
 

    public static readonly BitSet FOLLOW_expression_in_start147 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_31_in_variable160 = new BitSet(new ulong[]{0x0000000000040000UL});
    public static readonly BitSet FOLLOW_Id_in_variable162 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_32_in_variable178 = new BitSet(new ulong[]{0x000005FD80FC0000UL,0x00000003C6000000UL});
    public static readonly BitSet FOLLOW_expression_in_variable180 = new BitSet(new ulong[]{0x0000000200000000UL});
    public static readonly BitSet FOLLOW_33_in_variable182 = new BitSet(new ulong[]{0x0000000000040000UL});
    public static readonly BitSet FOLLOW_Id_in_variable184 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_34_in_variable200 = new BitSet(new ulong[]{0x0000000000040000UL});
    public static readonly BitSet FOLLOW_Id_in_variable202 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_35_in_variable216 = new BitSet(new ulong[]{0x0000000000040000UL});
    public static readonly BitSet FOLLOW_Id_in_variable218 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_36_in_variable232 = new BitSet(new ulong[]{0x0000000000040000UL});
    public static readonly BitSet FOLLOW_Id_in_variable234 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_37_in_variable248 = new BitSet(new ulong[]{0x0000000000040000UL});
    public static readonly BitSet FOLLOW_Id_in_variable250 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_integerLiteral_in_literal271 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_FloatingPointLiteral_in_literal284 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_StringLiteral_in_literal296 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_booleanLiteral_in_literal308 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_38_in_literal321 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_set_in_integerLiteral0 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_set_in_booleanLiteral0 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_conditionalExpression_in_expression373 = new BitSet(new ulong[]{0x003FFE0000000002UL});
    public static readonly BitSet FOLLOW_assignmentOperator_in_expression376 = new BitSet(new ulong[]{0x000005FD80FC0000UL,0x00000003C6000000UL});
    public static readonly BitSet FOLLOW_expression_in_expression379 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_41_in_assignmentOperator391 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_42_in_assignmentOperator396 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_43_in_assignmentOperator401 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_44_in_assignmentOperator406 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_45_in_assignmentOperator411 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_46_in_assignmentOperator416 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_47_in_assignmentOperator421 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_48_in_assignmentOperator426 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_49_in_assignmentOperator431 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_50_in_assignmentOperator436 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_51_in_assignmentOperator441 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_52_in_assignmentOperator457 = new BitSet(new ulong[]{0x0010000000000000UL});
    public static readonly BitSet FOLLOW_52_in_assignmentOperator461 = new BitSet(new ulong[]{0x0000020000000000UL});
    public static readonly BitSet FOLLOW_41_in_assignmentOperator465 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_53_in_assignmentOperator489 = new BitSet(new ulong[]{0x0020000000000000UL});
    public static readonly BitSet FOLLOW_53_in_assignmentOperator493 = new BitSet(new ulong[]{0x0020000000000000UL});
    public static readonly BitSet FOLLOW_53_in_assignmentOperator497 = new BitSet(new ulong[]{0x0000020000000000UL});
    public static readonly BitSet FOLLOW_41_in_assignmentOperator501 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_53_in_assignmentOperator522 = new BitSet(new ulong[]{0x0020000000000000UL});
    public static readonly BitSet FOLLOW_53_in_assignmentOperator526 = new BitSet(new ulong[]{0x0000020000000000UL});
    public static readonly BitSet FOLLOW_41_in_assignmentOperator530 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_conditionalOrExpression_in_conditionalExpression545 = new BitSet(new ulong[]{0x0040000000000002UL});
    public static readonly BitSet FOLLOW_54_in_conditionalExpression549 = new BitSet(new ulong[]{0x000005FD80FC0000UL,0x00000003C6000000UL});
    public static readonly BitSet FOLLOW_expression_in_conditionalExpression552 = new BitSet(new ulong[]{0x0080000000000000UL});
    public static readonly BitSet FOLLOW_55_in_conditionalExpression554 = new BitSet(new ulong[]{0x000005FD80FC0000UL,0x00000003C6000000UL});
    public static readonly BitSet FOLLOW_expression_in_conditionalExpression557 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_conditionalAndExpression_in_conditionalOrExpression570 = new BitSet(new ulong[]{0x0700000000000002UL});
    public static readonly BitSet FOLLOW_set_in_conditionalOrExpression574 = new BitSet(new ulong[]{0x000005FD80FC0000UL,0x00000003C6000000UL});
    public static readonly BitSet FOLLOW_conditionalAndExpression_in_conditionalOrExpression583 = new BitSet(new ulong[]{0x0700000000000002UL});
    public static readonly BitSet FOLLOW_inclusiveOrExpression_in_conditionalAndExpression598 = new BitSet(new ulong[]{0x3800000000000002UL});
    public static readonly BitSet FOLLOW_set_in_conditionalAndExpression602 = new BitSet(new ulong[]{0x000005FD80FC0000UL,0x00000003C6000000UL});
    public static readonly BitSet FOLLOW_inclusiveOrExpression_in_conditionalAndExpression611 = new BitSet(new ulong[]{0x3800000000000002UL});
    public static readonly BitSet FOLLOW_exclusiveOrExpression_in_inclusiveOrExpression626 = new BitSet(new ulong[]{0x4000000000000002UL});
    public static readonly BitSet FOLLOW_62_in_inclusiveOrExpression630 = new BitSet(new ulong[]{0x000005FD80FC0000UL,0x00000003C6000000UL});
    public static readonly BitSet FOLLOW_exclusiveOrExpression_in_inclusiveOrExpression633 = new BitSet(new ulong[]{0x4000000000000002UL});
    public static readonly BitSet FOLLOW_andExpression_in_exclusiveOrExpression646 = new BitSet(new ulong[]{0x8000000000000002UL});
    public static readonly BitSet FOLLOW_63_in_exclusiveOrExpression650 = new BitSet(new ulong[]{0x000005FD80FC0000UL,0x00000003C6000000UL});
    public static readonly BitSet FOLLOW_andExpression_in_exclusiveOrExpression653 = new BitSet(new ulong[]{0x8000000000000002UL});
    public static readonly BitSet FOLLOW_equalityExpression_in_andExpression666 = new BitSet(new ulong[]{0x0000000000000002UL,0x0000000000000001UL});
    public static readonly BitSet FOLLOW_64_in_andExpression670 = new BitSet(new ulong[]{0x000005FD80FC0000UL,0x00000003C6000000UL});
    public static readonly BitSet FOLLOW_equalityExpression_in_andExpression673 = new BitSet(new ulong[]{0x0000000000000002UL,0x0000000000000001UL});
    public static readonly BitSet FOLLOW_instanceOfExpression_in_equalityExpression686 = new BitSet(new ulong[]{0x0000000000000002UL,0x0000000000001FFEUL});
    public static readonly BitSet FOLLOW_set_in_equalityExpression690 = new BitSet(new ulong[]{0x000005FD80FC0000UL,0x00000003C6000000UL});
    public static readonly BitSet FOLLOW_instanceOfExpression_in_equalityExpression723 = new BitSet(new ulong[]{0x0000000000000002UL,0x0000000000001FFEUL});
    public static readonly BitSet FOLLOW_relationalExpression_in_instanceOfExpression736 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_shiftExpression_in_relationalExpression747 = new BitSet(new ulong[]{0x0030000000000002UL,0x0000000001FFE000UL});
    public static readonly BitSet FOLLOW_relationalOp_in_relationalExpression751 = new BitSet(new ulong[]{0x000005FD80FC0000UL,0x00000003C6000000UL});
    public static readonly BitSet FOLLOW_shiftExpression_in_relationalExpression754 = new BitSet(new ulong[]{0x0030000000000002UL,0x0000000001FFE000UL});
    public static readonly BitSet FOLLOW_52_in_relationalOp776 = new BitSet(new ulong[]{0x0000020000000000UL});
    public static readonly BitSet FOLLOW_41_in_relationalOp780 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_53_in_relationalOp800 = new BitSet(new ulong[]{0x0000020000000000UL});
    public static readonly BitSet FOLLOW_41_in_relationalOp804 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_52_in_relationalOp815 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_53_in_relationalOp821 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_77_in_relationalOp827 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_78_in_relationalOp832 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_79_in_relationalOp839 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_80_in_relationalOp844 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_81_in_relationalOp849 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_82_in_relationalOp854 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_83_in_relationalOp861 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_84_in_relationalOp866 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_85_in_relationalOp871 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_86_in_relationalOp876 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_87_in_relationalOp883 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_88_in_relationalOp888 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_additiveExpression_in_shiftExpression898 = new BitSet(new ulong[]{0x0030000000000002UL});
    public static readonly BitSet FOLLOW_shiftOp_in_shiftExpression902 = new BitSet(new ulong[]{0x000005FD80FC0000UL,0x00000003C6000000UL});
    public static readonly BitSet FOLLOW_additiveExpression_in_shiftExpression905 = new BitSet(new ulong[]{0x0030000000000002UL});
    public static readonly BitSet FOLLOW_52_in_shiftOp927 = new BitSet(new ulong[]{0x0010000000000000UL});
    public static readonly BitSet FOLLOW_52_in_shiftOp931 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_53_in_shiftOp953 = new BitSet(new ulong[]{0x0020000000000000UL});
    public static readonly BitSet FOLLOW_53_in_shiftOp957 = new BitSet(new ulong[]{0x0020000000000000UL});
    public static readonly BitSet FOLLOW_53_in_shiftOp961 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_53_in_shiftOp981 = new BitSet(new ulong[]{0x0020000000000000UL});
    public static readonly BitSet FOLLOW_53_in_shiftOp985 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_multiplicativeExpression_in_additiveExpression1000 = new BitSet(new ulong[]{0x0000000000000002UL,0x0000000006000000UL});
    public static readonly BitSet FOLLOW_set_in_additiveExpression1004 = new BitSet(new ulong[]{0x000005FD80FC0000UL,0x00000003C6000000UL});
    public static readonly BitSet FOLLOW_multiplicativeExpression_in_additiveExpression1013 = new BitSet(new ulong[]{0x0000000000000002UL,0x0000000006000000UL});
    public static readonly BitSet FOLLOW_unaryExpression_in_multiplicativeExpression1026 = new BitSet(new ulong[]{0x0000000000000002UL,0x0000000038000000UL});
    public static readonly BitSet FOLLOW_set_in_multiplicativeExpression1030 = new BitSet(new ulong[]{0x000005FD80FC0000UL,0x00000003C6000000UL});
    public static readonly BitSet FOLLOW_unaryExpression_in_multiplicativeExpression1045 = new BitSet(new ulong[]{0x0000000000000002UL,0x0000000038000000UL});
    public static readonly BitSet FOLLOW_89_in_unaryExpression1058 = new BitSet(new ulong[]{0x000005FD80FC0000UL,0x00000003C6000000UL});
    public static readonly BitSet FOLLOW_unaryExpression_in_unaryExpression1061 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_90_in_unaryExpression1066 = new BitSet(new ulong[]{0x000005FD80FC0000UL,0x00000003C6000000UL});
    public static readonly BitSet FOLLOW_unaryExpression_in_unaryExpression1069 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_94_in_unaryExpression1074 = new BitSet(new ulong[]{0x000005FD80FC0000UL,0x00000003C6000000UL});
    public static readonly BitSet FOLLOW_unaryExpression_in_unaryExpression1077 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_95_in_unaryExpression1082 = new BitSet(new ulong[]{0x000005FD80FC0000UL,0x00000003C6000000UL});
    public static readonly BitSet FOLLOW_unaryExpression_in_unaryExpression1085 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_unaryExpressionNotPlusMinus_in_unaryExpression1090 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_42_in_unaryExpressionNotPlusMinus1100 = new BitSet(new ulong[]{0x000005FD80FC0000UL,0x00000003C6000000UL});
    public static readonly BitSet FOLLOW_unaryExpression_in_unaryExpressionNotPlusMinus1103 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_96_in_unaryExpressionNotPlusMinus1108 = new BitSet(new ulong[]{0x000005FD80FC0000UL,0x00000003C6000000UL});
    public static readonly BitSet FOLLOW_unaryExpression_in_unaryExpressionNotPlusMinus1111 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_castExpression_in_unaryExpressionNotPlusMinus1116 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_primary_in_unaryExpressionNotPlusMinus1121 = new BitSet(new ulong[]{0x0000000000000000UL,0x00000000C0000000UL});
    public static readonly BitSet FOLLOW_set_in_unaryExpressionNotPlusMinus1123 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_primary_in_unaryExpressionNotPlusMinus1135 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_97_in_castExpression1146 = new BitSet(new ulong[]{0x0000000000040000UL});
    public static readonly BitSet FOLLOW_Id_in_castExpression1148 = new BitSet(new ulong[]{0x0000000000000000UL,0x0000000400000000UL});
    public static readonly BitSet FOLLOW_98_in_castExpression1150 = new BitSet(new ulong[]{0x000005FD80FC0000UL,0x00000003C6000000UL});
    public static readonly BitSet FOLLOW_unaryExpression_in_castExpression1152 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_97_in_castExpression1158 = new BitSet(new ulong[]{0x0000000000040000UL});
    public static readonly BitSet FOLLOW_Id_in_castExpression1160 = new BitSet(new ulong[]{0x0000000000000000UL,0x0000000400000000UL});
    public static readonly BitSet FOLLOW_98_in_castExpression1162 = new BitSet(new ulong[]{0x000005FD80FC0000UL,0x00000003C6000000UL});
    public static readonly BitSet FOLLOW_unaryExpressionNotPlusMinus_in_castExpression1164 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_97_in_parExpression1175 = new BitSet(new ulong[]{0x000005FD80FC0000UL,0x00000003C6000000UL});
    public static readonly BitSet FOLLOW_expression_in_parExpression1177 = new BitSet(new ulong[]{0x0000000000000000UL,0x0000000400000000UL});
    public static readonly BitSet FOLLOW_98_in_parExpression1179 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_literal_in_unit1193 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_variable_in_unit1198 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_function_in_unit1203 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_parExpression_in_primary1213 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_unit_in_primary1218 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_Id_in_argument1229 = new BitSet(new ulong[]{0x0000000000000000UL,0x0000000800000000UL});
    public static readonly BitSet FOLLOW_99_in_argument1231 = new BitSet(new ulong[]{0x000005FD80FC0000UL,0x00000003C6000000UL});
    public static readonly BitSet FOLLOW_expression_in_argument1233 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_expression_in_argument1248 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_argument_in_argument_list1267 = new BitSet(new ulong[]{0x0000000000000002UL,0x0000001000000000UL});
    public static readonly BitSet FOLLOW_100_in_argument_list1270 = new BitSet(new ulong[]{0x000005FD80FC0000UL,0x00000003C6000000UL});
    public static readonly BitSet FOLLOW_argument_in_argument_list1272 = new BitSet(new ulong[]{0x0000000000000002UL,0x0000001000000000UL});
    public static readonly BitSet FOLLOW_Id_in_function1290 = new BitSet(new ulong[]{0x0000000000000000UL,0x0000000200000000UL});
    public static readonly BitSet FOLLOW_97_in_function1292 = new BitSet(new ulong[]{0x0000000000000000UL,0x0000000400000000UL});
    public static readonly BitSet FOLLOW_98_in_function1294 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_Id_in_function1307 = new BitSet(new ulong[]{0x0000000000000000UL,0x0000000200000000UL});
    public static readonly BitSet FOLLOW_97_in_function1309 = new BitSet(new ulong[]{0x000005FD80FC0000UL,0x00000003C6000000UL});
    public static readonly BitSet FOLLOW_argument_list_in_function1311 = new BitSet(new ulong[]{0x0000000000000000UL,0x0000000400000000UL});
    public static readonly BitSet FOLLOW_98_in_function1313 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_assignmentOperator_in_synpred13_MetraScript376 = new BitSet(new ulong[]{0x000005FD80FC0000UL,0x00000003C6000000UL});
    public static readonly BitSet FOLLOW_expression_in_synpred13_MetraScript379 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_52_in_synpred25_MetraScript447 = new BitSet(new ulong[]{0x0010000000000000UL});
    public static readonly BitSet FOLLOW_52_in_synpred25_MetraScript449 = new BitSet(new ulong[]{0x0000020000000000UL});
    public static readonly BitSet FOLLOW_41_in_synpred25_MetraScript451 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_53_in_synpred26_MetraScript477 = new BitSet(new ulong[]{0x0020000000000000UL});
    public static readonly BitSet FOLLOW_53_in_synpred26_MetraScript479 = new BitSet(new ulong[]{0x0020000000000000UL});
    public static readonly BitSet FOLLOW_53_in_synpred26_MetraScript481 = new BitSet(new ulong[]{0x0000020000000000UL});
    public static readonly BitSet FOLLOW_41_in_synpred26_MetraScript483 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_53_in_synpred27_MetraScript512 = new BitSet(new ulong[]{0x0020000000000000UL});
    public static readonly BitSet FOLLOW_53_in_synpred27_MetraScript514 = new BitSet(new ulong[]{0x0000020000000000UL});
    public static readonly BitSet FOLLOW_41_in_synpred27_MetraScript516 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_52_in_synpred51_MetraScript768 = new BitSet(new ulong[]{0x0000020000000000UL});
    public static readonly BitSet FOLLOW_41_in_synpred51_MetraScript770 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_53_in_synpred52_MetraScript792 = new BitSet(new ulong[]{0x0000020000000000UL});
    public static readonly BitSet FOLLOW_41_in_synpred52_MetraScript794 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_52_in_synpred67_MetraScript919 = new BitSet(new ulong[]{0x0010000000000000UL});
    public static readonly BitSet FOLLOW_52_in_synpred67_MetraScript921 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_53_in_synpred68_MetraScript943 = new BitSet(new ulong[]{0x0020000000000000UL});
    public static readonly BitSet FOLLOW_53_in_synpred68_MetraScript945 = new BitSet(new ulong[]{0x0020000000000000UL});
    public static readonly BitSet FOLLOW_53_in_synpred68_MetraScript947 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_53_in_synpred69_MetraScript973 = new BitSet(new ulong[]{0x0020000000000000UL});
    public static readonly BitSet FOLLOW_53_in_synpred69_MetraScript975 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_castExpression_in_synpred81_MetraScript1116 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_primary_in_synpred83_MetraScript1121 = new BitSet(new ulong[]{0x0000000000000000UL,0x00000000C0000000UL});
    public static readonly BitSet FOLLOW_set_in_synpred83_MetraScript1123 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_97_in_synpred84_MetraScript1146 = new BitSet(new ulong[]{0x0000000000040000UL});
    public static readonly BitSet FOLLOW_Id_in_synpred84_MetraScript1148 = new BitSet(new ulong[]{0x0000000000000000UL,0x0000000400000000UL});
    public static readonly BitSet FOLLOW_98_in_synpred84_MetraScript1150 = new BitSet(new ulong[]{0x000005FD80FC0000UL,0x00000003C6000000UL});
    public static readonly BitSet FOLLOW_unaryExpression_in_synpred84_MetraScript1152 = new BitSet(new ulong[]{0x0000000000000002UL});

}
