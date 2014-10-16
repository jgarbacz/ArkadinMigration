// $ANTLR 3.1.2 C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g 2009-04-29 17:00:07

//using ParserExensionsNameSpace;


import org.antlr.runtime.*;
import java.util.Stack;
import java.util.List;
import java.util.ArrayList;
import java.util.Map;
import java.util.HashMap;
import org.antlr.runtime.debug.*;
import java.io.IOException;

import org.antlr.runtime.tree.*;

public class MetraScriptParser extends DebugParser {
    public static final String[] tokenNames = new String[] {
        "<invalid>", "<EOR>", "<DOWN>", "<UP>", "INT", "FLOAT", "STRING", "BOOL", "NULL", "CURRENT_OBJECT", "OBJECT", "TEMP", "GLOBAL", "PROC", "THREAD", "Id", "FloatingPointLiteral", "StringLiteral", "HexLiteral", "OctalLiteral", "DecimalLiteral", "HexDigit", "IntegerTypeSuffix", "Exponent", "FloatTypeSuffix", "EscapeSequence", "UnicodeEscape", "OctalEscape", "WS", "COMMENT", "LINE_COMMENT", "'OBJECT.'", "'OBJECT('", "').'", "'TEMP.'", "'GLOBAL.'", "'THREAD.'", "'PROC.'", "'null'", "'true'", "'false'", "'='", "'~'", "'+='", "'-='", "'*='", "'/='", "'&='", "'|='", "'^='", "'%='", "'~='", "'<'", "'>'", "'?'", "':'", "'||'", "'or'", "'&&'", "'and'", "'|'", "'^'", "'&'", "'=='", "'!='", "'eq'", "'ne'", "'gt'", "'lt'", "'gte'", "'lte'", "'GT'", "'LT'", "'EQ'", "'GTE'", "'LTE'", "'+'", "'-'", "'*'", "'/'", "'%'", "'++'", "'--'", "'!'", "'('", "')'"
    };
    public static final int T__68=68;
    public static final int T__69=69;
    public static final int T__66=66;
    public static final int T__67=67;
    public static final int T__64=64;
    public static final int T__65=65;
    public static final int T__62=62;
    public static final int T__63=63;
    public static final int FloatTypeSuffix=24;
    public static final int OctalLiteral=19;
    public static final int Exponent=23;
    public static final int FLOAT=5;
    public static final int T__61=61;
    public static final int EOF=-1;
    public static final int T__60=60;
    public static final int HexDigit=21;
    public static final int T__55=55;
    public static final int T__56=56;
    public static final int T__57=57;
    public static final int T__58=58;
    public static final int T__51=51;
    public static final int T__52=52;
    public static final int T__53=53;
    public static final int T__54=54;
    public static final int OBJECT=10;
    public static final int T__59=59;
    public static final int PROC=13;
    public static final int COMMENT=29;
    public static final int CURRENT_OBJECT=9;
    public static final int Id=15;
    public static final int T__50=50;
    public static final int T__42=42;
    public static final int HexLiteral=18;
    public static final int T__43=43;
    public static final int THREAD=14;
    public static final int T__40=40;
    public static final int T__41=41;
    public static final int T__80=80;
    public static final int T__46=46;
    public static final int T__81=81;
    public static final int T__47=47;
    public static final int T__82=82;
    public static final int T__44=44;
    public static final int T__83=83;
    public static final int T__45=45;
    public static final int LINE_COMMENT=30;
    public static final int IntegerTypeSuffix=22;
    public static final int T__48=48;
    public static final int T__49=49;
    public static final int TEMP=11;
    public static final int NULL=8;
    public static final int BOOL=7;
    public static final int INT=4;
    public static final int T__85=85;
    public static final int DecimalLiteral=20;
    public static final int T__84=84;
    public static final int StringLiteral=17;
    public static final int T__31=31;
    public static final int T__32=32;
    public static final int WS=28;
    public static final int T__71=71;
    public static final int T__33=33;
    public static final int T__72=72;
    public static final int T__34=34;
    public static final int T__35=35;
    public static final int T__36=36;
    public static final int T__70=70;
    public static final int T__37=37;
    public static final int T__38=38;
    public static final int T__39=39;
    public static final int UnicodeEscape=26;
    public static final int FloatingPointLiteral=16;
    public static final int GLOBAL=12;
    public static final int T__76=76;
    public static final int T__75=75;
    public static final int T__74=74;
    public static final int OctalEscape=27;
    public static final int EscapeSequence=25;
    public static final int T__73=73;
    public static final int T__79=79;
    public static final int STRING=6;
    public static final int T__78=78;
    public static final int T__77=77;

    // delegates
    // delegators

    public static final String[] ruleNames = new String[] {
        "invalidRule", "primary", "synpred54_MetraScript", "synpred11_MetraScript", 
        "synpred28_MetraScript", "synpred46_MetraScript", "synpred35_MetraScript", 
        "synpred48_MetraScript", "synpred66_MetraScript", "integerLiteral", 
        "synpred50_MetraScript", "synpred67_MetraScript", "synpred16_MetraScript", 
        "synpred6_MetraScript", "synpred36_MetraScript", "synpred42_MetraScript", 
        "conditionalExpression", "synpred32_MetraScript", "synpred15_MetraScript", 
        "multiplicativeExpression", "relationalOp", "synpred26_MetraScript", 
        "synpred69_MetraScript", "literal", "synpred47_MetraScript", "synpred22_MetraScript", 
        "synpred44_MetraScript", "synpred13_MetraScript", "synpred18_MetraScript", 
        "synpred55_MetraScript", "exclusiveOrExpression", "synpred40_MetraScript", 
        "synpred61_MetraScript", "synpred37_MetraScript", "synpred8_MetraScript", 
        "unit", "synpred2_MetraScript", "assignmentOperator", "synpred60_MetraScript", 
        "parExpression", "booleanLiteral", "synpred30_MetraScript", "synpred51_MetraScript", 
        "castExpression", "synpred63_MetraScript", "synpred27_MetraScript", 
        "synpred20_MetraScript", "synpred23_MetraScript", "synpred10_MetraScript", 
        "synpred41_MetraScript", "synpred39_MetraScript", "shiftExpression", 
        "expression", "synpred57_MetraScript", "synpred59_MetraScript", 
        "synpred45_MetraScript", "variable", "synpred33_MetraScript", "additiveExpression", 
        "synpred31_MetraScript", "equalityExpression", "shiftOp", "synpred9_MetraScript", 
        "synpred34_MetraScript", "synpred56_MetraScript", "andExpression", 
        "synpred29_MetraScript", "synpred21_MetraScript", "synpred3_MetraScript", 
        "synpred43_MetraScript", "synpred62_MetraScript", "synpred49_MetraScript", 
        "inclusiveOrExpression", "synpred17_MetraScript", "synpred70_MetraScript", 
        "synpred64_MetraScript", "synpred65_MetraScript", "synpred1_MetraScript", 
        "synpred24_MetraScript", "synpred38_MetraScript", "synpred71_MetraScript", 
        "synpred58_MetraScript", "unaryExpression", "conditionalAndExpression", 
        "relationalExpression", "synpred73_MetraScript", "synpred68_MetraScript", 
        "synpred19_MetraScript", "synpred74_MetraScript", "synpred12_MetraScript", 
        "start", "conditionalOrExpression", "synpred25_MetraScript", "synpred7_MetraScript", 
        "synpred53_MetraScript", "synpred4_MetraScript", "instanceOfExpression", 
        "synpred14_MetraScript", "synpred5_MetraScript", "synpred52_MetraScript", 
        "synpred72_MetraScript", "unaryExpressionNotPlusMinus"
    };
     
        public int ruleLevel = 0;
        public int getRuleLevel() { return ruleLevel; }
        public void incRuleLevel() { ruleLevel++; }
        public void decRuleLevel() { ruleLevel--; }
        public MetraScriptParser(TokenStream input) {
            this(input, DebugEventSocketProxy.DEFAULT_DEBUGGER_PORT, new RecognizerSharedState());
        }
        public MetraScriptParser(TokenStream input, int port, RecognizerSharedState state) {
            super(input, state);
            this.state.ruleMemo = new HashMap[101+1];
             
            DebugEventSocketProxy proxy =
                new DebugEventSocketProxy(this,port,adaptor);
            setDebugListener(proxy);
            setTokenStream(new DebugTokenStream(input,proxy));
            try {
                proxy.handshake();
            }
            catch (IOException ioe) {
                reportError(ioe);
            }
            TreeAdaptor adap = new CommonTreeAdaptor();
            setTreeAdaptor(adap);
            proxy.setTreeAdaptor(adap);
        }
    public MetraScriptParser(TokenStream input, DebugEventListener dbg) {
        super(input, dbg);
        this.state.ruleMemo = new HashMap[101+1];
         
         
        TreeAdaptor adap = new CommonTreeAdaptor();
        setTreeAdaptor(adap);

    }
    protected boolean evalPredicate(boolean result, String predicate) {
        dbg.semanticPredicate(result, predicate);
        return result;
    }

    protected DebugTreeAdaptor adaptor;
    public void setTreeAdaptor(TreeAdaptor adaptor) {
        this.adaptor = new DebugTreeAdaptor(dbg,adaptor);

    }
    public TreeAdaptor getTreeAdaptor() {
        return adaptor;
    }


    public String[] getTokenNames() { return MetraScriptParser.tokenNames; }
    public String getGrammarFileName() { return "C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g"; }


    public static class start_return extends ParserRuleReturnScope {
        Object tree;
        public Object getTree() { return tree; }
    };

    // $ANTLR start "start"
    // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:34:1: start : expression ;
    public final MetraScriptParser.start_return start() throws RecognitionException {
        MetraScriptParser.start_return retval = new MetraScriptParser.start_return();
        retval.start = input.LT(1);
        int start_StartIndex = input.index();
        Object root_0 = null;

        MetraScriptParser.expression_return expression1 = null;



        try { dbg.enterRule(getGrammarFileName(), "start");
        if ( getRuleLevel()==0 ) {dbg.commence();}
        incRuleLevel();
        dbg.location(34, 1);

        try {
            if ( state.backtracking>0 && alreadyParsedRule(input, 1) ) { return retval; }
            // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:34:7: ( expression )
            dbg.enterAlt(1);

            // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:35:2: expression
            {
            root_0 = (Object)adaptor.nil();

            dbg.location(35,2);
            pushFollow(FOLLOW_expression_in_start97);
            expression1=expression();

            state._fsp--;
            if (state.failed) return retval;
            if ( state.backtracking==0 ) adaptor.addChild(root_0, expression1.getTree());

            }

            retval.stop = input.LT(-1);

            if ( state.backtracking==0 ) {

            retval.tree = (Object)adaptor.rulePostProcessing(root_0);
            adaptor.setTokenBoundaries(retval.tree, retval.start, retval.stop);
            }
        }
        catch (RecognitionException re) {
            reportError(re);
            recover(input,re);
    	retval.tree = (Object)adaptor.errorNode(input, retval.start, input.LT(-1), re);

        }
        finally {
            if ( state.backtracking>0 ) { memoize(input, 1, start_StartIndex); }
        }
        dbg.location(36, 2);

        }
        finally {
            dbg.exitRule(getGrammarFileName(), "start");
            decRuleLevel();
            if ( getRuleLevel()==0 ) {dbg.terminate();}
        }

        return retval;
    }
    // $ANTLR end "start"

    public static class variable_return extends ParserRuleReturnScope {
        Object tree;
        public Object getTree() { return tree; }
    };

    // $ANTLR start "variable"
    // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:39:1: variable : ( 'OBJECT.' Id -> ^( OBJECT CURRENT_OBJECT Id ) | 'OBJECT(' expression ').' Id -> ^( OBJECT expression Id ) | 'TEMP.' Id -> ^( TEMP Id ) | 'GLOBAL.' Id -> ^( GLOBAL Id ) | 'THREAD.' Id -> ^( THREAD Id ) | 'PROC.' Id -> ^( PROC Id ) );
    public final MetraScriptParser.variable_return variable() throws RecognitionException {
        MetraScriptParser.variable_return retval = new MetraScriptParser.variable_return();
        retval.start = input.LT(1);
        int variable_StartIndex = input.index();
        Object root_0 = null;

        Token string_literal2=null;
        Token Id3=null;
        Token string_literal4=null;
        Token string_literal6=null;
        Token Id7=null;
        Token string_literal8=null;
        Token Id9=null;
        Token string_literal10=null;
        Token Id11=null;
        Token string_literal12=null;
        Token Id13=null;
        Token string_literal14=null;
        Token Id15=null;
        MetraScriptParser.expression_return expression5 = null;


        Object string_literal2_tree=null;
        Object Id3_tree=null;
        Object string_literal4_tree=null;
        Object string_literal6_tree=null;
        Object Id7_tree=null;
        Object string_literal8_tree=null;
        Object Id9_tree=null;
        Object string_literal10_tree=null;
        Object Id11_tree=null;
        Object string_literal12_tree=null;
        Object Id13_tree=null;
        Object string_literal14_tree=null;
        Object Id15_tree=null;
        RewriteRuleTokenStream stream_32=new RewriteRuleTokenStream(adaptor,"token 32");
        RewriteRuleTokenStream stream_31=new RewriteRuleTokenStream(adaptor,"token 31");
        RewriteRuleTokenStream stream_35=new RewriteRuleTokenStream(adaptor,"token 35");
        RewriteRuleTokenStream stream_36=new RewriteRuleTokenStream(adaptor,"token 36");
        RewriteRuleTokenStream stream_33=new RewriteRuleTokenStream(adaptor,"token 33");
        RewriteRuleTokenStream stream_Id=new RewriteRuleTokenStream(adaptor,"token Id");
        RewriteRuleTokenStream stream_34=new RewriteRuleTokenStream(adaptor,"token 34");
        RewriteRuleTokenStream stream_37=new RewriteRuleTokenStream(adaptor,"token 37");
        RewriteRuleSubtreeStream stream_expression=new RewriteRuleSubtreeStream(adaptor,"rule expression");
        try { dbg.enterRule(getGrammarFileName(), "variable");
        if ( getRuleLevel()==0 ) {dbg.commence();}
        incRuleLevel();
        dbg.location(39, 1);

        try {
            if ( state.backtracking>0 && alreadyParsedRule(input, 2) ) { return retval; }
            // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:40:2: ( 'OBJECT.' Id -> ^( OBJECT CURRENT_OBJECT Id ) | 'OBJECT(' expression ').' Id -> ^( OBJECT expression Id ) | 'TEMP.' Id -> ^( TEMP Id ) | 'GLOBAL.' Id -> ^( GLOBAL Id ) | 'THREAD.' Id -> ^( THREAD Id ) | 'PROC.' Id -> ^( PROC Id ) )
            int alt1=6;
            try { dbg.enterDecision(1);

            switch ( input.LA(1) ) {
            case 31:
                {
                alt1=1;
                }
                break;
            case 32:
                {
                alt1=2;
                }
                break;
            case 34:
                {
                alt1=3;
                }
                break;
            case 35:
                {
                alt1=4;
                }
                break;
            case 36:
                {
                alt1=5;
                }
                break;
            case 37:
                {
                alt1=6;
                }
                break;
            default:
                if (state.backtracking>0) {state.failed=true; return retval;}
                NoViableAltException nvae =
                    new NoViableAltException("", 1, 0, input);

                dbg.recognitionException(nvae);
                throw nvae;
            }

            } finally {dbg.exitDecision(1);}

            switch (alt1) {
                case 1 :
                    dbg.enterAlt(1);

                    // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:40:4: 'OBJECT.' Id
                    {
                    dbg.location(40,4);
                    string_literal2=(Token)match(input,31,FOLLOW_31_in_variable112); if (state.failed) return retval; 
                    if ( state.backtracking==0 ) stream_31.add(string_literal2);

                    dbg.location(40,14);
                    Id3=(Token)match(input,Id,FOLLOW_Id_in_variable114); if (state.failed) return retval; 
                    if ( state.backtracking==0 ) stream_Id.add(Id3);



                    // AST REWRITE
                    // elements: Id
                    // token labels: 
                    // rule labels: retval
                    // token list labels: 
                    // rule list labels: 
                    // wildcard labels: 
                    if ( state.backtracking==0 ) {
                    retval.tree = root_0;
                    RewriteRuleSubtreeStream stream_retval=new RewriteRuleSubtreeStream(adaptor,"rule retval",retval!=null?retval.tree:null);

                    root_0 = (Object)adaptor.nil();
                    // 40:18: -> ^( OBJECT CURRENT_OBJECT Id )
                    {
                        dbg.location(40,21);
                        // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:40:21: ^( OBJECT CURRENT_OBJECT Id )
                        {
                        Object root_1 = (Object)adaptor.nil();
                        dbg.location(40,23);
                        root_1 = (Object)adaptor.becomeRoot((Object)adaptor.create(OBJECT, "OBJECT"), root_1);

                        dbg.location(40,30);
                        adaptor.addChild(root_1, (Object)adaptor.create(CURRENT_OBJECT, "CURRENT_OBJECT"));
                        dbg.location(40,45);
                        adaptor.addChild(root_1, stream_Id.nextNode());

                        adaptor.addChild(root_0, root_1);
                        }

                    }

                    retval.tree = root_0;}
                    }
                    break;
                case 2 :
                    dbg.enterAlt(2);

                    // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:41:4: 'OBJECT(' expression ').' Id
                    {
                    dbg.location(41,4);
                    string_literal4=(Token)match(input,32,FOLLOW_32_in_variable130); if (state.failed) return retval; 
                    if ( state.backtracking==0 ) stream_32.add(string_literal4);

                    dbg.location(41,14);
                    pushFollow(FOLLOW_expression_in_variable132);
                    expression5=expression();

                    state._fsp--;
                    if (state.failed) return retval;
                    if ( state.backtracking==0 ) stream_expression.add(expression5.getTree());
                    dbg.location(41,25);
                    string_literal6=(Token)match(input,33,FOLLOW_33_in_variable134); if (state.failed) return retval; 
                    if ( state.backtracking==0 ) stream_33.add(string_literal6);

                    dbg.location(41,30);
                    Id7=(Token)match(input,Id,FOLLOW_Id_in_variable136); if (state.failed) return retval; 
                    if ( state.backtracking==0 ) stream_Id.add(Id7);



                    // AST REWRITE
                    // elements: expression, Id
                    // token labels: 
                    // rule labels: retval
                    // token list labels: 
                    // rule list labels: 
                    // wildcard labels: 
                    if ( state.backtracking==0 ) {
                    retval.tree = root_0;
                    RewriteRuleSubtreeStream stream_retval=new RewriteRuleSubtreeStream(adaptor,"rule retval",retval!=null?retval.tree:null);

                    root_0 = (Object)adaptor.nil();
                    // 41:34: -> ^( OBJECT expression Id )
                    {
                        dbg.location(41,37);
                        // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:41:37: ^( OBJECT expression Id )
                        {
                        Object root_1 = (Object)adaptor.nil();
                        dbg.location(41,39);
                        root_1 = (Object)adaptor.becomeRoot((Object)adaptor.create(OBJECT, "OBJECT"), root_1);

                        dbg.location(41,46);
                        adaptor.addChild(root_1, stream_expression.nextTree());
                        dbg.location(41,57);
                        adaptor.addChild(root_1, stream_Id.nextNode());

                        adaptor.addChild(root_0, root_1);
                        }

                    }

                    retval.tree = root_0;}
                    }
                    break;
                case 3 :
                    dbg.enterAlt(3);

                    // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:42:4: 'TEMP.' Id
                    {
                    dbg.location(42,4);
                    string_literal8=(Token)match(input,34,FOLLOW_34_in_variable152); if (state.failed) return retval; 
                    if ( state.backtracking==0 ) stream_34.add(string_literal8);

                    dbg.location(42,12);
                    Id9=(Token)match(input,Id,FOLLOW_Id_in_variable154); if (state.failed) return retval; 
                    if ( state.backtracking==0 ) stream_Id.add(Id9);



                    // AST REWRITE
                    // elements: Id
                    // token labels: 
                    // rule labels: retval
                    // token list labels: 
                    // rule list labels: 
                    // wildcard labels: 
                    if ( state.backtracking==0 ) {
                    retval.tree = root_0;
                    RewriteRuleSubtreeStream stream_retval=new RewriteRuleSubtreeStream(adaptor,"rule retval",retval!=null?retval.tree:null);

                    root_0 = (Object)adaptor.nil();
                    // 42:16: -> ^( TEMP Id )
                    {
                        dbg.location(42,19);
                        // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:42:19: ^( TEMP Id )
                        {
                        Object root_1 = (Object)adaptor.nil();
                        dbg.location(42,21);
                        root_1 = (Object)adaptor.becomeRoot((Object)adaptor.create(TEMP, "TEMP"), root_1);

                        dbg.location(42,26);
                        adaptor.addChild(root_1, stream_Id.nextNode());

                        adaptor.addChild(root_0, root_1);
                        }

                    }

                    retval.tree = root_0;}
                    }
                    break;
                case 4 :
                    dbg.enterAlt(4);

                    // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:43:4: 'GLOBAL.' Id
                    {
                    dbg.location(43,4);
                    string_literal10=(Token)match(input,35,FOLLOW_35_in_variable168); if (state.failed) return retval; 
                    if ( state.backtracking==0 ) stream_35.add(string_literal10);

                    dbg.location(43,14);
                    Id11=(Token)match(input,Id,FOLLOW_Id_in_variable170); if (state.failed) return retval; 
                    if ( state.backtracking==0 ) stream_Id.add(Id11);



                    // AST REWRITE
                    // elements: Id
                    // token labels: 
                    // rule labels: retval
                    // token list labels: 
                    // rule list labels: 
                    // wildcard labels: 
                    if ( state.backtracking==0 ) {
                    retval.tree = root_0;
                    RewriteRuleSubtreeStream stream_retval=new RewriteRuleSubtreeStream(adaptor,"rule retval",retval!=null?retval.tree:null);

                    root_0 = (Object)adaptor.nil();
                    // 43:18: -> ^( GLOBAL Id )
                    {
                        dbg.location(43,21);
                        // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:43:21: ^( GLOBAL Id )
                        {
                        Object root_1 = (Object)adaptor.nil();
                        dbg.location(43,23);
                        root_1 = (Object)adaptor.becomeRoot((Object)adaptor.create(GLOBAL, "GLOBAL"), root_1);

                        dbg.location(43,30);
                        adaptor.addChild(root_1, stream_Id.nextNode());

                        adaptor.addChild(root_0, root_1);
                        }

                    }

                    retval.tree = root_0;}
                    }
                    break;
                case 5 :
                    dbg.enterAlt(5);

                    // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:44:4: 'THREAD.' Id
                    {
                    dbg.location(44,4);
                    string_literal12=(Token)match(input,36,FOLLOW_36_in_variable184); if (state.failed) return retval; 
                    if ( state.backtracking==0 ) stream_36.add(string_literal12);

                    dbg.location(44,14);
                    Id13=(Token)match(input,Id,FOLLOW_Id_in_variable186); if (state.failed) return retval; 
                    if ( state.backtracking==0 ) stream_Id.add(Id13);



                    // AST REWRITE
                    // elements: Id
                    // token labels: 
                    // rule labels: retval
                    // token list labels: 
                    // rule list labels: 
                    // wildcard labels: 
                    if ( state.backtracking==0 ) {
                    retval.tree = root_0;
                    RewriteRuleSubtreeStream stream_retval=new RewriteRuleSubtreeStream(adaptor,"rule retval",retval!=null?retval.tree:null);

                    root_0 = (Object)adaptor.nil();
                    // 44:18: -> ^( THREAD Id )
                    {
                        dbg.location(44,21);
                        // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:44:21: ^( THREAD Id )
                        {
                        Object root_1 = (Object)adaptor.nil();
                        dbg.location(44,23);
                        root_1 = (Object)adaptor.becomeRoot((Object)adaptor.create(THREAD, "THREAD"), root_1);

                        dbg.location(44,30);
                        adaptor.addChild(root_1, stream_Id.nextNode());

                        adaptor.addChild(root_0, root_1);
                        }

                    }

                    retval.tree = root_0;}
                    }
                    break;
                case 6 :
                    dbg.enterAlt(6);

                    // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:45:4: 'PROC.' Id
                    {
                    dbg.location(45,4);
                    string_literal14=(Token)match(input,37,FOLLOW_37_in_variable200); if (state.failed) return retval; 
                    if ( state.backtracking==0 ) stream_37.add(string_literal14);

                    dbg.location(45,12);
                    Id15=(Token)match(input,Id,FOLLOW_Id_in_variable202); if (state.failed) return retval; 
                    if ( state.backtracking==0 ) stream_Id.add(Id15);



                    // AST REWRITE
                    // elements: Id
                    // token labels: 
                    // rule labels: retval
                    // token list labels: 
                    // rule list labels: 
                    // wildcard labels: 
                    if ( state.backtracking==0 ) {
                    retval.tree = root_0;
                    RewriteRuleSubtreeStream stream_retval=new RewriteRuleSubtreeStream(adaptor,"rule retval",retval!=null?retval.tree:null);

                    root_0 = (Object)adaptor.nil();
                    // 45:16: -> ^( PROC Id )
                    {
                        dbg.location(45,19);
                        // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:45:19: ^( PROC Id )
                        {
                        Object root_1 = (Object)adaptor.nil();
                        dbg.location(45,21);
                        root_1 = (Object)adaptor.becomeRoot((Object)adaptor.create(PROC, "PROC"), root_1);

                        dbg.location(45,26);
                        adaptor.addChild(root_1, stream_Id.nextNode());

                        adaptor.addChild(root_0, root_1);
                        }

                    }

                    retval.tree = root_0;}
                    }
                    break;

            }
            retval.stop = input.LT(-1);

            if ( state.backtracking==0 ) {

            retval.tree = (Object)adaptor.rulePostProcessing(root_0);
            adaptor.setTokenBoundaries(retval.tree, retval.start, retval.stop);
            }
        }
        catch (RecognitionException re) {
            reportError(re);
            recover(input,re);
    	retval.tree = (Object)adaptor.errorNode(input, retval.start, input.LT(-1), re);

        }
        finally {
            if ( state.backtracking>0 ) { memoize(input, 2, variable_StartIndex); }
        }
        dbg.location(46, 2);

        }
        finally {
            dbg.exitRule(getGrammarFileName(), "variable");
            decRuleLevel();
            if ( getRuleLevel()==0 ) {dbg.terminate();}
        }

        return retval;
    }
    // $ANTLR end "variable"

    public static class literal_return extends ParserRuleReturnScope {
        Object tree;
        public Object getTree() { return tree; }
    };

    // $ANTLR start "literal"
    // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:48:1: literal : ( integerLiteral -> ^( INT integerLiteral ) | FloatingPointLiteral -> ^( FLOAT FloatingPointLiteral ) | StringLiteral -> ^( STRING StringLiteral ) | booleanLiteral -> ^( BOOL booleanLiteral ) | 'null' -> ^( NULL ) );
    public final MetraScriptParser.literal_return literal() throws RecognitionException {
        MetraScriptParser.literal_return retval = new MetraScriptParser.literal_return();
        retval.start = input.LT(1);
        int literal_StartIndex = input.index();
        Object root_0 = null;

        Token FloatingPointLiteral17=null;
        Token StringLiteral18=null;
        Token string_literal20=null;
        MetraScriptParser.integerLiteral_return integerLiteral16 = null;

        MetraScriptParser.booleanLiteral_return booleanLiteral19 = null;


        Object FloatingPointLiteral17_tree=null;
        Object StringLiteral18_tree=null;
        Object string_literal20_tree=null;
        RewriteRuleTokenStream stream_StringLiteral=new RewriteRuleTokenStream(adaptor,"token StringLiteral");
        RewriteRuleTokenStream stream_FloatingPointLiteral=new RewriteRuleTokenStream(adaptor,"token FloatingPointLiteral");
        RewriteRuleTokenStream stream_38=new RewriteRuleTokenStream(adaptor,"token 38");
        RewriteRuleSubtreeStream stream_booleanLiteral=new RewriteRuleSubtreeStream(adaptor,"rule booleanLiteral");
        RewriteRuleSubtreeStream stream_integerLiteral=new RewriteRuleSubtreeStream(adaptor,"rule integerLiteral");
        try { dbg.enterRule(getGrammarFileName(), "literal");
        if ( getRuleLevel()==0 ) {dbg.commence();}
        incRuleLevel();
        dbg.location(48, 1);

        try {
            if ( state.backtracking>0 && alreadyParsedRule(input, 3) ) { return retval; }
            // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:49:2: ( integerLiteral -> ^( INT integerLiteral ) | FloatingPointLiteral -> ^( FLOAT FloatingPointLiteral ) | StringLiteral -> ^( STRING StringLiteral ) | booleanLiteral -> ^( BOOL booleanLiteral ) | 'null' -> ^( NULL ) )
            int alt2=5;
            try { dbg.enterDecision(2);

            switch ( input.LA(1) ) {
            case HexLiteral:
            case OctalLiteral:
            case DecimalLiteral:
                {
                alt2=1;
                }
                break;
            case FloatingPointLiteral:
                {
                alt2=2;
                }
                break;
            case StringLiteral:
                {
                alt2=3;
                }
                break;
            case 39:
            case 40:
                {
                alt2=4;
                }
                break;
            case 38:
                {
                alt2=5;
                }
                break;
            default:
                if (state.backtracking>0) {state.failed=true; return retval;}
                NoViableAltException nvae =
                    new NoViableAltException("", 2, 0, input);

                dbg.recognitionException(nvae);
                throw nvae;
            }

            } finally {dbg.exitDecision(2);}

            switch (alt2) {
                case 1 :
                    dbg.enterAlt(1);

                    // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:49:4: integerLiteral
                    {
                    dbg.location(49,4);
                    pushFollow(FOLLOW_integerLiteral_in_literal223);
                    integerLiteral16=integerLiteral();

                    state._fsp--;
                    if (state.failed) return retval;
                    if ( state.backtracking==0 ) stream_integerLiteral.add(integerLiteral16.getTree());


                    // AST REWRITE
                    // elements: integerLiteral
                    // token labels: 
                    // rule labels: retval
                    // token list labels: 
                    // rule list labels: 
                    // wildcard labels: 
                    if ( state.backtracking==0 ) {
                    retval.tree = root_0;
                    RewriteRuleSubtreeStream stream_retval=new RewriteRuleSubtreeStream(adaptor,"rule retval",retval!=null?retval.tree:null);

                    root_0 = (Object)adaptor.nil();
                    // 49:19: -> ^( INT integerLiteral )
                    {
                        dbg.location(49,22);
                        // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:49:22: ^( INT integerLiteral )
                        {
                        Object root_1 = (Object)adaptor.nil();
                        dbg.location(49,24);
                        root_1 = (Object)adaptor.becomeRoot((Object)adaptor.create(INT, "INT"), root_1);

                        dbg.location(49,28);
                        adaptor.addChild(root_1, stream_integerLiteral.nextTree());

                        adaptor.addChild(root_0, root_1);
                        }

                    }

                    retval.tree = root_0;}
                    }
                    break;
                case 2 :
                    dbg.enterAlt(2);

                    // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:50:4: FloatingPointLiteral
                    {
                    dbg.location(50,4);
                    FloatingPointLiteral17=(Token)match(input,FloatingPointLiteral,FOLLOW_FloatingPointLiteral_in_literal236); if (state.failed) return retval; 
                    if ( state.backtracking==0 ) stream_FloatingPointLiteral.add(FloatingPointLiteral17);



                    // AST REWRITE
                    // elements: FloatingPointLiteral
                    // token labels: 
                    // rule labels: retval
                    // token list labels: 
                    // rule list labels: 
                    // wildcard labels: 
                    if ( state.backtracking==0 ) {
                    retval.tree = root_0;
                    RewriteRuleSubtreeStream stream_retval=new RewriteRuleSubtreeStream(adaptor,"rule retval",retval!=null?retval.tree:null);

                    root_0 = (Object)adaptor.nil();
                    // 50:24: -> ^( FLOAT FloatingPointLiteral )
                    {
                        dbg.location(50,27);
                        // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:50:27: ^( FLOAT FloatingPointLiteral )
                        {
                        Object root_1 = (Object)adaptor.nil();
                        dbg.location(50,29);
                        root_1 = (Object)adaptor.becomeRoot((Object)adaptor.create(FLOAT, "FLOAT"), root_1);

                        dbg.location(50,35);
                        adaptor.addChild(root_1, stream_FloatingPointLiteral.nextNode());

                        adaptor.addChild(root_0, root_1);
                        }

                    }

                    retval.tree = root_0;}
                    }
                    break;
                case 3 :
                    dbg.enterAlt(3);

                    // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:51:4: StringLiteral
                    {
                    dbg.location(51,4);
                    StringLiteral18=(Token)match(input,StringLiteral,FOLLOW_StringLiteral_in_literal248); if (state.failed) return retval; 
                    if ( state.backtracking==0 ) stream_StringLiteral.add(StringLiteral18);



                    // AST REWRITE
                    // elements: StringLiteral
                    // token labels: 
                    // rule labels: retval
                    // token list labels: 
                    // rule list labels: 
                    // wildcard labels: 
                    if ( state.backtracking==0 ) {
                    retval.tree = root_0;
                    RewriteRuleSubtreeStream stream_retval=new RewriteRuleSubtreeStream(adaptor,"rule retval",retval!=null?retval.tree:null);

                    root_0 = (Object)adaptor.nil();
                    // 51:17: -> ^( STRING StringLiteral )
                    {
                        dbg.location(51,20);
                        // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:51:20: ^( STRING StringLiteral )
                        {
                        Object root_1 = (Object)adaptor.nil();
                        dbg.location(51,22);
                        root_1 = (Object)adaptor.becomeRoot((Object)adaptor.create(STRING, "STRING"), root_1);

                        dbg.location(51,29);
                        adaptor.addChild(root_1, stream_StringLiteral.nextNode());

                        adaptor.addChild(root_0, root_1);
                        }

                    }

                    retval.tree = root_0;}
                    }
                    break;
                case 4 :
                    dbg.enterAlt(4);

                    // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:52:4: booleanLiteral
                    {
                    dbg.location(52,4);
                    pushFollow(FOLLOW_booleanLiteral_in_literal260);
                    booleanLiteral19=booleanLiteral();

                    state._fsp--;
                    if (state.failed) return retval;
                    if ( state.backtracking==0 ) stream_booleanLiteral.add(booleanLiteral19.getTree());


                    // AST REWRITE
                    // elements: booleanLiteral
                    // token labels: 
                    // rule labels: retval
                    // token list labels: 
                    // rule list labels: 
                    // wildcard labels: 
                    if ( state.backtracking==0 ) {
                    retval.tree = root_0;
                    RewriteRuleSubtreeStream stream_retval=new RewriteRuleSubtreeStream(adaptor,"rule retval",retval!=null?retval.tree:null);

                    root_0 = (Object)adaptor.nil();
                    // 52:19: -> ^( BOOL booleanLiteral )
                    {
                        dbg.location(52,22);
                        // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:52:22: ^( BOOL booleanLiteral )
                        {
                        Object root_1 = (Object)adaptor.nil();
                        dbg.location(52,24);
                        root_1 = (Object)adaptor.becomeRoot((Object)adaptor.create(BOOL, "BOOL"), root_1);

                        dbg.location(52,29);
                        adaptor.addChild(root_1, stream_booleanLiteral.nextTree());

                        adaptor.addChild(root_0, root_1);
                        }

                    }

                    retval.tree = root_0;}
                    }
                    break;
                case 5 :
                    dbg.enterAlt(5);

                    // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:53:4: 'null'
                    {
                    dbg.location(53,4);
                    string_literal20=(Token)match(input,38,FOLLOW_38_in_literal273); if (state.failed) return retval; 
                    if ( state.backtracking==0 ) stream_38.add(string_literal20);



                    // AST REWRITE
                    // elements: 
                    // token labels: 
                    // rule labels: retval
                    // token list labels: 
                    // rule list labels: 
                    // wildcard labels: 
                    if ( state.backtracking==0 ) {
                    retval.tree = root_0;
                    RewriteRuleSubtreeStream stream_retval=new RewriteRuleSubtreeStream(adaptor,"rule retval",retval!=null?retval.tree:null);

                    root_0 = (Object)adaptor.nil();
                    // 53:11: -> ^( NULL )
                    {
                        dbg.location(53,14);
                        // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:53:14: ^( NULL )
                        {
                        Object root_1 = (Object)adaptor.nil();
                        dbg.location(53,16);
                        root_1 = (Object)adaptor.becomeRoot((Object)adaptor.create(NULL, "NULL"), root_1);

                        adaptor.addChild(root_0, root_1);
                        }

                    }

                    retval.tree = root_0;}
                    }
                    break;

            }
            retval.stop = input.LT(-1);

            if ( state.backtracking==0 ) {

            retval.tree = (Object)adaptor.rulePostProcessing(root_0);
            adaptor.setTokenBoundaries(retval.tree, retval.start, retval.stop);
            }
        }
        catch (RecognitionException re) {
            reportError(re);
            recover(input,re);
    	retval.tree = (Object)adaptor.errorNode(input, retval.start, input.LT(-1), re);

        }
        finally {
            if ( state.backtracking>0 ) { memoize(input, 3, literal_StartIndex); }
        }
        dbg.location(54, 2);

        }
        finally {
            dbg.exitRule(getGrammarFileName(), "literal");
            decRuleLevel();
            if ( getRuleLevel()==0 ) {dbg.terminate();}
        }

        return retval;
    }
    // $ANTLR end "literal"

    public static class integerLiteral_return extends ParserRuleReturnScope {
        Object tree;
        public Object getTree() { return tree; }
    };

    // $ANTLR start "integerLiteral"
    // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:55:1: integerLiteral : ( HexLiteral | OctalLiteral | DecimalLiteral );
    public final MetraScriptParser.integerLiteral_return integerLiteral() throws RecognitionException {
        MetraScriptParser.integerLiteral_return retval = new MetraScriptParser.integerLiteral_return();
        retval.start = input.LT(1);
        int integerLiteral_StartIndex = input.index();
        Object root_0 = null;

        Token set21=null;

        Object set21_tree=null;

        try { dbg.enterRule(getGrammarFileName(), "integerLiteral");
        if ( getRuleLevel()==0 ) {dbg.commence();}
        incRuleLevel();
        dbg.location(55, 1);

        try {
            if ( state.backtracking>0 && alreadyParsedRule(input, 4) ) { return retval; }
            // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:56:2: ( HexLiteral | OctalLiteral | DecimalLiteral )
            dbg.enterAlt(1);

            // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:
            {
            root_0 = (Object)adaptor.nil();

            dbg.location(56,2);
            set21=(Token)input.LT(1);
            if ( (input.LA(1)>=HexLiteral && input.LA(1)<=DecimalLiteral) ) {
                input.consume();
                if ( state.backtracking==0 ) adaptor.addChild(root_0, (Object)adaptor.create(set21));
                state.errorRecovery=false;state.failed=false;
            }
            else {
                if (state.backtracking>0) {state.failed=true; return retval;}
                MismatchedSetException mse = new MismatchedSetException(null,input);
                dbg.recognitionException(mse);
                throw mse;
            }


            }

            retval.stop = input.LT(-1);

            if ( state.backtracking==0 ) {

            retval.tree = (Object)adaptor.rulePostProcessing(root_0);
            adaptor.setTokenBoundaries(retval.tree, retval.start, retval.stop);
            }
        }
        catch (RecognitionException re) {
            reportError(re);
            recover(input,re);
    	retval.tree = (Object)adaptor.errorNode(input, retval.start, input.LT(-1), re);

        }
        finally {
            if ( state.backtracking>0 ) { memoize(input, 4, integerLiteral_StartIndex); }
        }
        dbg.location(59, 2);

        }
        finally {
            dbg.exitRule(getGrammarFileName(), "integerLiteral");
            decRuleLevel();
            if ( getRuleLevel()==0 ) {dbg.terminate();}
        }

        return retval;
    }
    // $ANTLR end "integerLiteral"

    public static class booleanLiteral_return extends ParserRuleReturnScope {
        Object tree;
        public Object getTree() { return tree; }
    };

    // $ANTLR start "booleanLiteral"
    // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:60:1: booleanLiteral : ( 'true' | 'false' );
    public final MetraScriptParser.booleanLiteral_return booleanLiteral() throws RecognitionException {
        MetraScriptParser.booleanLiteral_return retval = new MetraScriptParser.booleanLiteral_return();
        retval.start = input.LT(1);
        int booleanLiteral_StartIndex = input.index();
        Object root_0 = null;

        Token set22=null;

        Object set22_tree=null;

        try { dbg.enterRule(getGrammarFileName(), "booleanLiteral");
        if ( getRuleLevel()==0 ) {dbg.commence();}
        incRuleLevel();
        dbg.location(60, 1);

        try {
            if ( state.backtracking>0 && alreadyParsedRule(input, 5) ) { return retval; }
            // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:61:2: ( 'true' | 'false' )
            dbg.enterAlt(1);

            // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:
            {
            root_0 = (Object)adaptor.nil();

            dbg.location(61,2);
            set22=(Token)input.LT(1);
            if ( (input.LA(1)>=39 && input.LA(1)<=40) ) {
                input.consume();
                if ( state.backtracking==0 ) adaptor.addChild(root_0, (Object)adaptor.create(set22));
                state.errorRecovery=false;state.failed=false;
            }
            else {
                if (state.backtracking>0) {state.failed=true; return retval;}
                MismatchedSetException mse = new MismatchedSetException(null,input);
                dbg.recognitionException(mse);
                throw mse;
            }


            }

            retval.stop = input.LT(-1);

            if ( state.backtracking==0 ) {

            retval.tree = (Object)adaptor.rulePostProcessing(root_0);
            adaptor.setTokenBoundaries(retval.tree, retval.start, retval.stop);
            }
        }
        catch (RecognitionException re) {
            reportError(re);
            recover(input,re);
    	retval.tree = (Object)adaptor.errorNode(input, retval.start, input.LT(-1), re);

        }
        finally {
            if ( state.backtracking>0 ) { memoize(input, 5, booleanLiteral_StartIndex); }
        }
        dbg.location(63, 2);

        }
        finally {
            dbg.exitRule(getGrammarFileName(), "booleanLiteral");
            decRuleLevel();
            if ( getRuleLevel()==0 ) {dbg.terminate();}
        }

        return retval;
    }
    // $ANTLR end "booleanLiteral"

    public static class expression_return extends ParserRuleReturnScope {
        Object tree;
        public Object getTree() { return tree; }
    };

    // $ANTLR start "expression"
    // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:65:1: expression : conditionalExpression ( assignmentOperator expression )? ;
    public final MetraScriptParser.expression_return expression() throws RecognitionException {
        MetraScriptParser.expression_return retval = new MetraScriptParser.expression_return();
        retval.start = input.LT(1);
        int expression_StartIndex = input.index();
        Object root_0 = null;

        MetraScriptParser.conditionalExpression_return conditionalExpression23 = null;

        MetraScriptParser.assignmentOperator_return assignmentOperator24 = null;

        MetraScriptParser.expression_return expression25 = null;



        try { dbg.enterRule(getGrammarFileName(), "expression");
        if ( getRuleLevel()==0 ) {dbg.commence();}
        incRuleLevel();
        dbg.location(65, 1);

        try {
            if ( state.backtracking>0 && alreadyParsedRule(input, 6) ) { return retval; }
            // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:66:2: ( conditionalExpression ( assignmentOperator expression )? )
            dbg.enterAlt(1);

            // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:66:4: conditionalExpression ( assignmentOperator expression )?
            {
            root_0 = (Object)adaptor.nil();

            dbg.location(66,4);
            pushFollow(FOLLOW_conditionalExpression_in_expression325);
            conditionalExpression23=conditionalExpression();

            state._fsp--;
            if (state.failed) return retval;
            if ( state.backtracking==0 ) adaptor.addChild(root_0, conditionalExpression23.getTree());
            dbg.location(66,26);
            // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:66:26: ( assignmentOperator expression )?
            int alt3=2;
            try { dbg.enterSubRule(3);
            try { dbg.enterDecision(3);

            try {
                isCyclicDecision = true;
                alt3 = dfa3.predict(input);
            }
            catch (NoViableAltException nvae) {
                dbg.recognitionException(nvae);
                throw nvae;
            }
            } finally {dbg.exitDecision(3);}

            switch (alt3) {
                case 1 :
                    dbg.enterAlt(1);

                    // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:66:27: assignmentOperator expression
                    {
                    dbg.location(66,45);
                    pushFollow(FOLLOW_assignmentOperator_in_expression328);
                    assignmentOperator24=assignmentOperator();

                    state._fsp--;
                    if (state.failed) return retval;
                    if ( state.backtracking==0 ) root_0 = (Object)adaptor.becomeRoot(assignmentOperator24.getTree(), root_0);
                    dbg.location(66,47);
                    pushFollow(FOLLOW_expression_in_expression331);
                    expression25=expression();

                    state._fsp--;
                    if (state.failed) return retval;
                    if ( state.backtracking==0 ) adaptor.addChild(root_0, expression25.getTree());

                    }
                    break;

            }
            } finally {dbg.exitSubRule(3);}


            }

            retval.stop = input.LT(-1);

            if ( state.backtracking==0 ) {

            retval.tree = (Object)adaptor.rulePostProcessing(root_0);
            adaptor.setTokenBoundaries(retval.tree, retval.start, retval.stop);
            }
        }
        catch (RecognitionException re) {
            reportError(re);
            recover(input,re);
    	retval.tree = (Object)adaptor.errorNode(input, retval.start, input.LT(-1), re);

        }
        finally {
            if ( state.backtracking>0 ) { memoize(input, 6, expression_StartIndex); }
        }
        dbg.location(67, 2);

        }
        finally {
            dbg.exitRule(getGrammarFileName(), "expression");
            decRuleLevel();
            if ( getRuleLevel()==0 ) {dbg.terminate();}
        }

        return retval;
    }
    // $ANTLR end "expression"

    public static class assignmentOperator_return extends ParserRuleReturnScope {
        Object tree;
        public Object getTree() { return tree; }
    };

    // $ANTLR start "assignmentOperator"
    // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:68:1: assignmentOperator : ( '=' | '~' | '+=' | '-=' | '*=' | '/=' | '&=' | '|=' | '^=' | '%=' | '~=' | ( '<' '<' '=' )=>t1= '<' t2= '<' t3= '=' {...}? | ( '>' '>' '>' '=' )=>t1= '>' t2= '>' t3= '>' t4= '=' {...}? | ( '>' '>' '=' )=>t1= '>' t2= '>' t3= '=' {...}?);
    public final MetraScriptParser.assignmentOperator_return assignmentOperator() throws RecognitionException {
        MetraScriptParser.assignmentOperator_return retval = new MetraScriptParser.assignmentOperator_return();
        retval.start = input.LT(1);
        int assignmentOperator_StartIndex = input.index();
        Object root_0 = null;

        Token t1=null;
        Token t2=null;
        Token t3=null;
        Token t4=null;
        Token char_literal26=null;
        Token char_literal27=null;
        Token string_literal28=null;
        Token string_literal29=null;
        Token string_literal30=null;
        Token string_literal31=null;
        Token string_literal32=null;
        Token string_literal33=null;
        Token string_literal34=null;
        Token string_literal35=null;
        Token string_literal36=null;

        Object t1_tree=null;
        Object t2_tree=null;
        Object t3_tree=null;
        Object t4_tree=null;
        Object char_literal26_tree=null;
        Object char_literal27_tree=null;
        Object string_literal28_tree=null;
        Object string_literal29_tree=null;
        Object string_literal30_tree=null;
        Object string_literal31_tree=null;
        Object string_literal32_tree=null;
        Object string_literal33_tree=null;
        Object string_literal34_tree=null;
        Object string_literal35_tree=null;
        Object string_literal36_tree=null;

        try { dbg.enterRule(getGrammarFileName(), "assignmentOperator");
        if ( getRuleLevel()==0 ) {dbg.commence();}
        incRuleLevel();
        dbg.location(68, 1);

        try {
            if ( state.backtracking>0 && alreadyParsedRule(input, 7) ) { return retval; }
            // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:69:2: ( '=' | '~' | '+=' | '-=' | '*=' | '/=' | '&=' | '|=' | '^=' | '%=' | '~=' | ( '<' '<' '=' )=>t1= '<' t2= '<' t3= '=' {...}? | ( '>' '>' '>' '=' )=>t1= '>' t2= '>' t3= '>' t4= '=' {...}? | ( '>' '>' '=' )=>t1= '>' t2= '>' t3= '=' {...}?)
            int alt4=14;
            try { dbg.enterDecision(4);

            try {
                isCyclicDecision = true;
                alt4 = dfa4.predict(input);
            }
            catch (NoViableAltException nvae) {
                dbg.recognitionException(nvae);
                throw nvae;
            }
            } finally {dbg.exitDecision(4);}

            switch (alt4) {
                case 1 :
                    dbg.enterAlt(1);

                    // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:69:4: '='
                    {
                    root_0 = (Object)adaptor.nil();

                    dbg.location(69,4);
                    char_literal26=(Token)match(input,41,FOLLOW_41_in_assignmentOperator343); if (state.failed) return retval;
                    if ( state.backtracking==0 ) {
                    char_literal26_tree = (Object)adaptor.create(char_literal26);
                    adaptor.addChild(root_0, char_literal26_tree);
                    }

                    }
                    break;
                case 2 :
                    dbg.enterAlt(2);

                    // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:70:4: '~'
                    {
                    root_0 = (Object)adaptor.nil();

                    dbg.location(70,4);
                    char_literal27=(Token)match(input,42,FOLLOW_42_in_assignmentOperator348); if (state.failed) return retval;
                    if ( state.backtracking==0 ) {
                    char_literal27_tree = (Object)adaptor.create(char_literal27);
                    adaptor.addChild(root_0, char_literal27_tree);
                    }

                    }
                    break;
                case 3 :
                    dbg.enterAlt(3);

                    // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:71:4: '+='
                    {
                    root_0 = (Object)adaptor.nil();

                    dbg.location(71,4);
                    string_literal28=(Token)match(input,43,FOLLOW_43_in_assignmentOperator353); if (state.failed) return retval;
                    if ( state.backtracking==0 ) {
                    string_literal28_tree = (Object)adaptor.create(string_literal28);
                    adaptor.addChild(root_0, string_literal28_tree);
                    }

                    }
                    break;
                case 4 :
                    dbg.enterAlt(4);

                    // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:72:4: '-='
                    {
                    root_0 = (Object)adaptor.nil();

                    dbg.location(72,4);
                    string_literal29=(Token)match(input,44,FOLLOW_44_in_assignmentOperator358); if (state.failed) return retval;
                    if ( state.backtracking==0 ) {
                    string_literal29_tree = (Object)adaptor.create(string_literal29);
                    adaptor.addChild(root_0, string_literal29_tree);
                    }

                    }
                    break;
                case 5 :
                    dbg.enterAlt(5);

                    // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:73:4: '*='
                    {
                    root_0 = (Object)adaptor.nil();

                    dbg.location(73,4);
                    string_literal30=(Token)match(input,45,FOLLOW_45_in_assignmentOperator363); if (state.failed) return retval;
                    if ( state.backtracking==0 ) {
                    string_literal30_tree = (Object)adaptor.create(string_literal30);
                    adaptor.addChild(root_0, string_literal30_tree);
                    }

                    }
                    break;
                case 6 :
                    dbg.enterAlt(6);

                    // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:74:4: '/='
                    {
                    root_0 = (Object)adaptor.nil();

                    dbg.location(74,4);
                    string_literal31=(Token)match(input,46,FOLLOW_46_in_assignmentOperator368); if (state.failed) return retval;
                    if ( state.backtracking==0 ) {
                    string_literal31_tree = (Object)adaptor.create(string_literal31);
                    adaptor.addChild(root_0, string_literal31_tree);
                    }

                    }
                    break;
                case 7 :
                    dbg.enterAlt(7);

                    // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:75:4: '&='
                    {
                    root_0 = (Object)adaptor.nil();

                    dbg.location(75,4);
                    string_literal32=(Token)match(input,47,FOLLOW_47_in_assignmentOperator373); if (state.failed) return retval;
                    if ( state.backtracking==0 ) {
                    string_literal32_tree = (Object)adaptor.create(string_literal32);
                    adaptor.addChild(root_0, string_literal32_tree);
                    }

                    }
                    break;
                case 8 :
                    dbg.enterAlt(8);

                    // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:76:4: '|='
                    {
                    root_0 = (Object)adaptor.nil();

                    dbg.location(76,4);
                    string_literal33=(Token)match(input,48,FOLLOW_48_in_assignmentOperator378); if (state.failed) return retval;
                    if ( state.backtracking==0 ) {
                    string_literal33_tree = (Object)adaptor.create(string_literal33);
                    adaptor.addChild(root_0, string_literal33_tree);
                    }

                    }
                    break;
                case 9 :
                    dbg.enterAlt(9);

                    // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:77:4: '^='
                    {
                    root_0 = (Object)adaptor.nil();

                    dbg.location(77,4);
                    string_literal34=(Token)match(input,49,FOLLOW_49_in_assignmentOperator383); if (state.failed) return retval;
                    if ( state.backtracking==0 ) {
                    string_literal34_tree = (Object)adaptor.create(string_literal34);
                    adaptor.addChild(root_0, string_literal34_tree);
                    }

                    }
                    break;
                case 10 :
                    dbg.enterAlt(10);

                    // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:78:4: '%='
                    {
                    root_0 = (Object)adaptor.nil();

                    dbg.location(78,4);
                    string_literal35=(Token)match(input,50,FOLLOW_50_in_assignmentOperator388); if (state.failed) return retval;
                    if ( state.backtracking==0 ) {
                    string_literal35_tree = (Object)adaptor.create(string_literal35);
                    adaptor.addChild(root_0, string_literal35_tree);
                    }

                    }
                    break;
                case 11 :
                    dbg.enterAlt(11);

                    // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:79:4: '~='
                    {
                    root_0 = (Object)adaptor.nil();

                    dbg.location(79,4);
                    string_literal36=(Token)match(input,51,FOLLOW_51_in_assignmentOperator393); if (state.failed) return retval;
                    if ( state.backtracking==0 ) {
                    string_literal36_tree = (Object)adaptor.create(string_literal36);
                    adaptor.addChild(root_0, string_literal36_tree);
                    }

                    }
                    break;
                case 12 :
                    dbg.enterAlt(12);

                    // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:80:4: ( '<' '<' '=' )=>t1= '<' t2= '<' t3= '=' {...}?
                    {
                    root_0 = (Object)adaptor.nil();

                    dbg.location(80,22);
                    t1=(Token)match(input,52,FOLLOW_52_in_assignmentOperator409); if (state.failed) return retval;
                    if ( state.backtracking==0 ) {
                    t1_tree = (Object)adaptor.create(t1);
                    adaptor.addChild(root_0, t1_tree);
                    }
                    dbg.location(80,29);
                    t2=(Token)match(input,52,FOLLOW_52_in_assignmentOperator413); if (state.failed) return retval;
                    if ( state.backtracking==0 ) {
                    t2_tree = (Object)adaptor.create(t2);
                    adaptor.addChild(root_0, t2_tree);
                    }
                    dbg.location(80,36);
                    t3=(Token)match(input,41,FOLLOW_41_in_assignmentOperator417); if (state.failed) return retval;
                    if ( state.backtracking==0 ) {
                    t3_tree = (Object)adaptor.create(t3);
                    adaptor.addChild(root_0, t3_tree);
                    }
                    dbg.location(81,4);
                    if ( !(evalPredicate( t1.getLine() == t2.getLine() &&
                    	  t1.getCharPositionInLine() + 1 == t2.getCharPositionInLine() && 
                    	  t2.getLine() == t3.getLine() && 
                    	  t2.getCharPositionInLine() + 1 == t3.getCharPositionInLine() ," $t1.getLine() == $t2.getLine() &&\r\n\t  $t1.getCharPositionInLine() + 1 == $t2.getCharPositionInLine() && \r\n\t  $t2.getLine() == $t3.getLine() && \r\n\t  $t2.getCharPositionInLine() + 1 == $t3.getCharPositionInLine() ")) ) {
                        if (state.backtracking>0) {state.failed=true; return retval;}
                        throw new FailedPredicateException(input, "assignmentOperator", " $t1.getLine() == $t2.getLine() &&\r\n\t  $t1.getCharPositionInLine() + 1 == $t2.getCharPositionInLine() && \r\n\t  $t2.getLine() == $t3.getLine() && \r\n\t  $t2.getCharPositionInLine() + 1 == $t3.getCharPositionInLine() ");
                    }

                    }
                    break;
                case 13 :
                    dbg.enterAlt(13);

                    // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:85:4: ( '>' '>' '>' '=' )=>t1= '>' t2= '>' t3= '>' t4= '=' {...}?
                    {
                    root_0 = (Object)adaptor.nil();

                    dbg.location(85,26);
                    t1=(Token)match(input,53,FOLLOW_53_in_assignmentOperator441); if (state.failed) return retval;
                    if ( state.backtracking==0 ) {
                    t1_tree = (Object)adaptor.create(t1);
                    adaptor.addChild(root_0, t1_tree);
                    }
                    dbg.location(85,33);
                    t2=(Token)match(input,53,FOLLOW_53_in_assignmentOperator445); if (state.failed) return retval;
                    if ( state.backtracking==0 ) {
                    t2_tree = (Object)adaptor.create(t2);
                    adaptor.addChild(root_0, t2_tree);
                    }
                    dbg.location(85,40);
                    t3=(Token)match(input,53,FOLLOW_53_in_assignmentOperator449); if (state.failed) return retval;
                    if ( state.backtracking==0 ) {
                    t3_tree = (Object)adaptor.create(t3);
                    adaptor.addChild(root_0, t3_tree);
                    }
                    dbg.location(85,47);
                    t4=(Token)match(input,41,FOLLOW_41_in_assignmentOperator453); if (state.failed) return retval;
                    if ( state.backtracking==0 ) {
                    t4_tree = (Object)adaptor.create(t4);
                    adaptor.addChild(root_0, t4_tree);
                    }
                    dbg.location(86,4);
                    if ( !(evalPredicate( t1.getLine() == t2.getLine() && 
                    	  t1.getCharPositionInLine() + 1 == t2.getCharPositionInLine() &&
                    	  t2.getLine() == t3.getLine() && 
                    	  t2.getCharPositionInLine() + 1 == t3.getCharPositionInLine() &&
                    	  t3.getLine() == t4.getLine() && 
                    	  t3.getCharPositionInLine() + 1 == t4.getCharPositionInLine() ," $t1.getLine() == $t2.getLine() && \r\n\t  $t1.getCharPositionInLine() + 1 == $t2.getCharPositionInLine() &&\r\n\t  $t2.getLine() == $t3.getLine() && \r\n\t  $t2.getCharPositionInLine() + 1 == $t3.getCharPositionInLine() &&\r\n\t  $t3.getLine() == $t4.getLine() && \r\n\t  $t3.getCharPositionInLine() + 1 == $t4.getCharPositionInLine() ")) ) {
                        if (state.backtracking>0) {state.failed=true; return retval;}
                        throw new FailedPredicateException(input, "assignmentOperator", " $t1.getLine() == $t2.getLine() && \r\n\t  $t1.getCharPositionInLine() + 1 == $t2.getCharPositionInLine() &&\r\n\t  $t2.getLine() == $t3.getLine() && \r\n\t  $t2.getCharPositionInLine() + 1 == $t3.getCharPositionInLine() &&\r\n\t  $t3.getLine() == $t4.getLine() && \r\n\t  $t3.getCharPositionInLine() + 1 == $t4.getCharPositionInLine() ");
                    }

                    }
                    break;
                case 14 :
                    dbg.enterAlt(14);

                    // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:92:4: ( '>' '>' '=' )=>t1= '>' t2= '>' t3= '=' {...}?
                    {
                    root_0 = (Object)adaptor.nil();

                    dbg.location(92,22);
                    t1=(Token)match(input,53,FOLLOW_53_in_assignmentOperator474); if (state.failed) return retval;
                    if ( state.backtracking==0 ) {
                    t1_tree = (Object)adaptor.create(t1);
                    adaptor.addChild(root_0, t1_tree);
                    }
                    dbg.location(92,29);
                    t2=(Token)match(input,53,FOLLOW_53_in_assignmentOperator478); if (state.failed) return retval;
                    if ( state.backtracking==0 ) {
                    t2_tree = (Object)adaptor.create(t2);
                    adaptor.addChild(root_0, t2_tree);
                    }
                    dbg.location(92,36);
                    t3=(Token)match(input,41,FOLLOW_41_in_assignmentOperator482); if (state.failed) return retval;
                    if ( state.backtracking==0 ) {
                    t3_tree = (Object)adaptor.create(t3);
                    adaptor.addChild(root_0, t3_tree);
                    }
                    dbg.location(93,4);
                    if ( !(evalPredicate( t1.getLine() == t2.getLine() && 
                    	  t1.getCharPositionInLine() + 1 == t2.getCharPositionInLine() && 
                    	  t2.getLine() == t3.getLine() && 
                    	  t2.getCharPositionInLine() + 1 == t3.getCharPositionInLine() ," $t1.getLine() == $t2.getLine() && \r\n\t  $t1.getCharPositionInLine() + 1 == $t2.getCharPositionInLine() && \r\n\t  $t2.getLine() == $t3.getLine() && \r\n\t  $t2.getCharPositionInLine() + 1 == $t3.getCharPositionInLine() ")) ) {
                        if (state.backtracking>0) {state.failed=true; return retval;}
                        throw new FailedPredicateException(input, "assignmentOperator", " $t1.getLine() == $t2.getLine() && \r\n\t  $t1.getCharPositionInLine() + 1 == $t2.getCharPositionInLine() && \r\n\t  $t2.getLine() == $t3.getLine() && \r\n\t  $t2.getCharPositionInLine() + 1 == $t3.getCharPositionInLine() ");
                    }

                    }
                    break;

            }
            retval.stop = input.LT(-1);

            if ( state.backtracking==0 ) {

            retval.tree = (Object)adaptor.rulePostProcessing(root_0);
            adaptor.setTokenBoundaries(retval.tree, retval.start, retval.stop);
            }
        }
        catch (RecognitionException re) {
            reportError(re);
            recover(input,re);
    	retval.tree = (Object)adaptor.errorNode(input, retval.start, input.LT(-1), re);

        }
        finally {
            if ( state.backtracking>0 ) { memoize(input, 7, assignmentOperator_StartIndex); }
        }
        dbg.location(97, 2);

        }
        finally {
            dbg.exitRule(getGrammarFileName(), "assignmentOperator");
            decRuleLevel();
            if ( getRuleLevel()==0 ) {dbg.terminate();}
        }

        return retval;
    }
    // $ANTLR end "assignmentOperator"

    public static class conditionalExpression_return extends ParserRuleReturnScope {
        Object tree;
        public Object getTree() { return tree; }
    };

    // $ANTLR start "conditionalExpression"
    // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:98:1: conditionalExpression : conditionalOrExpression ( '?' expression ':' expression )? ;
    public final MetraScriptParser.conditionalExpression_return conditionalExpression() throws RecognitionException {
        MetraScriptParser.conditionalExpression_return retval = new MetraScriptParser.conditionalExpression_return();
        retval.start = input.LT(1);
        int conditionalExpression_StartIndex = input.index();
        Object root_0 = null;

        Token char_literal38=null;
        Token char_literal40=null;
        MetraScriptParser.conditionalOrExpression_return conditionalOrExpression37 = null;

        MetraScriptParser.expression_return expression39 = null;

        MetraScriptParser.expression_return expression41 = null;


        Object char_literal38_tree=null;
        Object char_literal40_tree=null;

        try { dbg.enterRule(getGrammarFileName(), "conditionalExpression");
        if ( getRuleLevel()==0 ) {dbg.commence();}
        incRuleLevel();
        dbg.location(98, 1);

        try {
            if ( state.backtracking>0 && alreadyParsedRule(input, 8) ) { return retval; }
            // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:99:2: ( conditionalOrExpression ( '?' expression ':' expression )? )
            dbg.enterAlt(1);

            // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:99:4: conditionalOrExpression ( '?' expression ':' expression )?
            {
            root_0 = (Object)adaptor.nil();

            dbg.location(99,4);
            pushFollow(FOLLOW_conditionalOrExpression_in_conditionalExpression497);
            conditionalOrExpression37=conditionalOrExpression();

            state._fsp--;
            if (state.failed) return retval;
            if ( state.backtracking==0 ) adaptor.addChild(root_0, conditionalOrExpression37.getTree());
            dbg.location(99,28);
            // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:99:28: ( '?' expression ':' expression )?
            int alt5=2;
            try { dbg.enterSubRule(5);
            try { dbg.enterDecision(5);

            int LA5_0 = input.LA(1);

            if ( (LA5_0==54) ) {
                alt5=1;
            }
            } finally {dbg.exitDecision(5);}

            switch (alt5) {
                case 1 :
                    dbg.enterAlt(1);

                    // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:99:30: '?' expression ':' expression
                    {
                    dbg.location(99,33);
                    char_literal38=(Token)match(input,54,FOLLOW_54_in_conditionalExpression501); if (state.failed) return retval;
                    if ( state.backtracking==0 ) {
                    char_literal38_tree = (Object)adaptor.create(char_literal38);
                    root_0 = (Object)adaptor.becomeRoot(char_literal38_tree, root_0);
                    }
                    dbg.location(99,35);
                    pushFollow(FOLLOW_expression_in_conditionalExpression504);
                    expression39=expression();

                    state._fsp--;
                    if (state.failed) return retval;
                    if ( state.backtracking==0 ) adaptor.addChild(root_0, expression39.getTree());
                    dbg.location(99,49);
                    char_literal40=(Token)match(input,55,FOLLOW_55_in_conditionalExpression506); if (state.failed) return retval;
                    dbg.location(99,51);
                    pushFollow(FOLLOW_expression_in_conditionalExpression509);
                    expression41=expression();

                    state._fsp--;
                    if (state.failed) return retval;
                    if ( state.backtracking==0 ) adaptor.addChild(root_0, expression41.getTree());

                    }
                    break;

            }
            } finally {dbg.exitSubRule(5);}


            }

            retval.stop = input.LT(-1);

            if ( state.backtracking==0 ) {

            retval.tree = (Object)adaptor.rulePostProcessing(root_0);
            adaptor.setTokenBoundaries(retval.tree, retval.start, retval.stop);
            }
        }
        catch (RecognitionException re) {
            reportError(re);
            recover(input,re);
    	retval.tree = (Object)adaptor.errorNode(input, retval.start, input.LT(-1), re);

        }
        finally {
            if ( state.backtracking>0 ) { memoize(input, 8, conditionalExpression_StartIndex); }
        }
        dbg.location(100, 2);

        }
        finally {
            dbg.exitRule(getGrammarFileName(), "conditionalExpression");
            decRuleLevel();
            if ( getRuleLevel()==0 ) {dbg.terminate();}
        }

        return retval;
    }
    // $ANTLR end "conditionalExpression"

    public static class conditionalOrExpression_return extends ParserRuleReturnScope {
        Object tree;
        public Object getTree() { return tree; }
    };

    // $ANTLR start "conditionalOrExpression"
    // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:101:1: conditionalOrExpression : conditionalAndExpression ( ( '||' | 'or' ) conditionalAndExpression )* ;
    public final MetraScriptParser.conditionalOrExpression_return conditionalOrExpression() throws RecognitionException {
        MetraScriptParser.conditionalOrExpression_return retval = new MetraScriptParser.conditionalOrExpression_return();
        retval.start = input.LT(1);
        int conditionalOrExpression_StartIndex = input.index();
        Object root_0 = null;

        Token set43=null;
        MetraScriptParser.conditionalAndExpression_return conditionalAndExpression42 = null;

        MetraScriptParser.conditionalAndExpression_return conditionalAndExpression44 = null;


        Object set43_tree=null;

        try { dbg.enterRule(getGrammarFileName(), "conditionalOrExpression");
        if ( getRuleLevel()==0 ) {dbg.commence();}
        incRuleLevel();
        dbg.location(101, 1);

        try {
            if ( state.backtracking>0 && alreadyParsedRule(input, 9) ) { return retval; }
            // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:102:2: ( conditionalAndExpression ( ( '||' | 'or' ) conditionalAndExpression )* )
            dbg.enterAlt(1);

            // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:102:4: conditionalAndExpression ( ( '||' | 'or' ) conditionalAndExpression )*
            {
            root_0 = (Object)adaptor.nil();

            dbg.location(102,4);
            pushFollow(FOLLOW_conditionalAndExpression_in_conditionalOrExpression522);
            conditionalAndExpression42=conditionalAndExpression();

            state._fsp--;
            if (state.failed) return retval;
            if ( state.backtracking==0 ) adaptor.addChild(root_0, conditionalAndExpression42.getTree());
            dbg.location(102,29);
            // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:102:29: ( ( '||' | 'or' ) conditionalAndExpression )*
            try { dbg.enterSubRule(6);

            loop6:
            do {
                int alt6=2;
                try { dbg.enterDecision(6);

                int LA6_0 = input.LA(1);

                if ( ((LA6_0>=56 && LA6_0<=57)) ) {
                    alt6=1;
                }


                } finally {dbg.exitDecision(6);}

                switch (alt6) {
            	case 1 :
            	    dbg.enterAlt(1);

            	    // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:102:31: ( '||' | 'or' ) conditionalAndExpression
            	    {
            	    dbg.location(102,31);
            	    set43=(Token)input.LT(1);
            	    set43=(Token)input.LT(1);
            	    if ( (input.LA(1)>=56 && input.LA(1)<=57) ) {
            	        input.consume();
            	        if ( state.backtracking==0 ) root_0 = (Object)adaptor.becomeRoot((Object)adaptor.create(set43), root_0);
            	        state.errorRecovery=false;state.failed=false;
            	    }
            	    else {
            	        if (state.backtracking>0) {state.failed=true; return retval;}
            	        MismatchedSetException mse = new MismatchedSetException(null,input);
            	        dbg.recognitionException(mse);
            	        throw mse;
            	    }

            	    dbg.location(102,44);
            	    pushFollow(FOLLOW_conditionalAndExpression_in_conditionalOrExpression533);
            	    conditionalAndExpression44=conditionalAndExpression();

            	    state._fsp--;
            	    if (state.failed) return retval;
            	    if ( state.backtracking==0 ) adaptor.addChild(root_0, conditionalAndExpression44.getTree());

            	    }
            	    break;

            	default :
            	    break loop6;
                }
            } while (true);
            } finally {dbg.exitSubRule(6);}


            }

            retval.stop = input.LT(-1);

            if ( state.backtracking==0 ) {

            retval.tree = (Object)adaptor.rulePostProcessing(root_0);
            adaptor.setTokenBoundaries(retval.tree, retval.start, retval.stop);
            }
        }
        catch (RecognitionException re) {
            reportError(re);
            recover(input,re);
    	retval.tree = (Object)adaptor.errorNode(input, retval.start, input.LT(-1), re);

        }
        finally {
            if ( state.backtracking>0 ) { memoize(input, 9, conditionalOrExpression_StartIndex); }
        }
        dbg.location(104, 2);

        }
        finally {
            dbg.exitRule(getGrammarFileName(), "conditionalOrExpression");
            decRuleLevel();
            if ( getRuleLevel()==0 ) {dbg.terminate();}
        }

        return retval;
    }
    // $ANTLR end "conditionalOrExpression"

    public static class conditionalAndExpression_return extends ParserRuleReturnScope {
        Object tree;
        public Object getTree() { return tree; }
    };

    // $ANTLR start "conditionalAndExpression"
    // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:105:1: conditionalAndExpression : inclusiveOrExpression ( ( '&&' | 'and' ) inclusiveOrExpression )* ;
    public final MetraScriptParser.conditionalAndExpression_return conditionalAndExpression() throws RecognitionException {
        MetraScriptParser.conditionalAndExpression_return retval = new MetraScriptParser.conditionalAndExpression_return();
        retval.start = input.LT(1);
        int conditionalAndExpression_StartIndex = input.index();
        Object root_0 = null;

        Token set46=null;
        MetraScriptParser.inclusiveOrExpression_return inclusiveOrExpression45 = null;

        MetraScriptParser.inclusiveOrExpression_return inclusiveOrExpression47 = null;


        Object set46_tree=null;

        try { dbg.enterRule(getGrammarFileName(), "conditionalAndExpression");
        if ( getRuleLevel()==0 ) {dbg.commence();}
        incRuleLevel();
        dbg.location(105, 1);

        try {
            if ( state.backtracking>0 && alreadyParsedRule(input, 10) ) { return retval; }
            // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:106:2: ( inclusiveOrExpression ( ( '&&' | 'and' ) inclusiveOrExpression )* )
            dbg.enterAlt(1);

            // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:106:4: inclusiveOrExpression ( ( '&&' | 'and' ) inclusiveOrExpression )*
            {
            root_0 = (Object)adaptor.nil();

            dbg.location(106,4);
            pushFollow(FOLLOW_inclusiveOrExpression_in_conditionalAndExpression548);
            inclusiveOrExpression45=inclusiveOrExpression();

            state._fsp--;
            if (state.failed) return retval;
            if ( state.backtracking==0 ) adaptor.addChild(root_0, inclusiveOrExpression45.getTree());
            dbg.location(106,26);
            // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:106:26: ( ( '&&' | 'and' ) inclusiveOrExpression )*
            try { dbg.enterSubRule(7);

            loop7:
            do {
                int alt7=2;
                try { dbg.enterDecision(7);

                int LA7_0 = input.LA(1);

                if ( ((LA7_0>=58 && LA7_0<=59)) ) {
                    alt7=1;
                }


                } finally {dbg.exitDecision(7);}

                switch (alt7) {
            	case 1 :
            	    dbg.enterAlt(1);

            	    // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:106:28: ( '&&' | 'and' ) inclusiveOrExpression
            	    {
            	    dbg.location(106,28);
            	    set46=(Token)input.LT(1);
            	    set46=(Token)input.LT(1);
            	    if ( (input.LA(1)>=58 && input.LA(1)<=59) ) {
            	        input.consume();
            	        if ( state.backtracking==0 ) root_0 = (Object)adaptor.becomeRoot((Object)adaptor.create(set46), root_0);
            	        state.errorRecovery=false;state.failed=false;
            	    }
            	    else {
            	        if (state.backtracking>0) {state.failed=true; return retval;}
            	        MismatchedSetException mse = new MismatchedSetException(null,input);
            	        dbg.recognitionException(mse);
            	        throw mse;
            	    }

            	    dbg.location(106,42);
            	    pushFollow(FOLLOW_inclusiveOrExpression_in_conditionalAndExpression559);
            	    inclusiveOrExpression47=inclusiveOrExpression();

            	    state._fsp--;
            	    if (state.failed) return retval;
            	    if ( state.backtracking==0 ) adaptor.addChild(root_0, inclusiveOrExpression47.getTree());

            	    }
            	    break;

            	default :
            	    break loop7;
                }
            } while (true);
            } finally {dbg.exitSubRule(7);}


            }

            retval.stop = input.LT(-1);

            if ( state.backtracking==0 ) {

            retval.tree = (Object)adaptor.rulePostProcessing(root_0);
            adaptor.setTokenBoundaries(retval.tree, retval.start, retval.stop);
            }
        }
        catch (RecognitionException re) {
            reportError(re);
            recover(input,re);
    	retval.tree = (Object)adaptor.errorNode(input, retval.start, input.LT(-1), re);

        }
        finally {
            if ( state.backtracking>0 ) { memoize(input, 10, conditionalAndExpression_StartIndex); }
        }
        dbg.location(108, 2);

        }
        finally {
            dbg.exitRule(getGrammarFileName(), "conditionalAndExpression");
            decRuleLevel();
            if ( getRuleLevel()==0 ) {dbg.terminate();}
        }

        return retval;
    }
    // $ANTLR end "conditionalAndExpression"

    public static class inclusiveOrExpression_return extends ParserRuleReturnScope {
        Object tree;
        public Object getTree() { return tree; }
    };

    // $ANTLR start "inclusiveOrExpression"
    // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:109:1: inclusiveOrExpression : exclusiveOrExpression ( '|' exclusiveOrExpression )* ;
    public final MetraScriptParser.inclusiveOrExpression_return inclusiveOrExpression() throws RecognitionException {
        MetraScriptParser.inclusiveOrExpression_return retval = new MetraScriptParser.inclusiveOrExpression_return();
        retval.start = input.LT(1);
        int inclusiveOrExpression_StartIndex = input.index();
        Object root_0 = null;

        Token char_literal49=null;
        MetraScriptParser.exclusiveOrExpression_return exclusiveOrExpression48 = null;

        MetraScriptParser.exclusiveOrExpression_return exclusiveOrExpression50 = null;


        Object char_literal49_tree=null;

        try { dbg.enterRule(getGrammarFileName(), "inclusiveOrExpression");
        if ( getRuleLevel()==0 ) {dbg.commence();}
        incRuleLevel();
        dbg.location(109, 1);

        try {
            if ( state.backtracking>0 && alreadyParsedRule(input, 11) ) { return retval; }
            // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:110:2: ( exclusiveOrExpression ( '|' exclusiveOrExpression )* )
            dbg.enterAlt(1);

            // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:110:4: exclusiveOrExpression ( '|' exclusiveOrExpression )*
            {
            root_0 = (Object)adaptor.nil();

            dbg.location(110,4);
            pushFollow(FOLLOW_exclusiveOrExpression_in_inclusiveOrExpression574);
            exclusiveOrExpression48=exclusiveOrExpression();

            state._fsp--;
            if (state.failed) return retval;
            if ( state.backtracking==0 ) adaptor.addChild(root_0, exclusiveOrExpression48.getTree());
            dbg.location(110,26);
            // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:110:26: ( '|' exclusiveOrExpression )*
            try { dbg.enterSubRule(8);

            loop8:
            do {
                int alt8=2;
                try { dbg.enterDecision(8);

                int LA8_0 = input.LA(1);

                if ( (LA8_0==60) ) {
                    alt8=1;
                }


                } finally {dbg.exitDecision(8);}

                switch (alt8) {
            	case 1 :
            	    dbg.enterAlt(1);

            	    // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:110:28: '|' exclusiveOrExpression
            	    {
            	    dbg.location(110,31);
            	    char_literal49=(Token)match(input,60,FOLLOW_60_in_inclusiveOrExpression578); if (state.failed) return retval;
            	    if ( state.backtracking==0 ) {
            	    char_literal49_tree = (Object)adaptor.create(char_literal49);
            	    root_0 = (Object)adaptor.becomeRoot(char_literal49_tree, root_0);
            	    }
            	    dbg.location(110,33);
            	    pushFollow(FOLLOW_exclusiveOrExpression_in_inclusiveOrExpression581);
            	    exclusiveOrExpression50=exclusiveOrExpression();

            	    state._fsp--;
            	    if (state.failed) return retval;
            	    if ( state.backtracking==0 ) adaptor.addChild(root_0, exclusiveOrExpression50.getTree());

            	    }
            	    break;

            	default :
            	    break loop8;
                }
            } while (true);
            } finally {dbg.exitSubRule(8);}


            }

            retval.stop = input.LT(-1);

            if ( state.backtracking==0 ) {

            retval.tree = (Object)adaptor.rulePostProcessing(root_0);
            adaptor.setTokenBoundaries(retval.tree, retval.start, retval.stop);
            }
        }
        catch (RecognitionException re) {
            reportError(re);
            recover(input,re);
    	retval.tree = (Object)adaptor.errorNode(input, retval.start, input.LT(-1), re);

        }
        finally {
            if ( state.backtracking>0 ) { memoize(input, 11, inclusiveOrExpression_StartIndex); }
        }
        dbg.location(111, 2);

        }
        finally {
            dbg.exitRule(getGrammarFileName(), "inclusiveOrExpression");
            decRuleLevel();
            if ( getRuleLevel()==0 ) {dbg.terminate();}
        }

        return retval;
    }
    // $ANTLR end "inclusiveOrExpression"

    public static class exclusiveOrExpression_return extends ParserRuleReturnScope {
        Object tree;
        public Object getTree() { return tree; }
    };

    // $ANTLR start "exclusiveOrExpression"
    // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:112:1: exclusiveOrExpression : andExpression ( '^' andExpression )* ;
    public final MetraScriptParser.exclusiveOrExpression_return exclusiveOrExpression() throws RecognitionException {
        MetraScriptParser.exclusiveOrExpression_return retval = new MetraScriptParser.exclusiveOrExpression_return();
        retval.start = input.LT(1);
        int exclusiveOrExpression_StartIndex = input.index();
        Object root_0 = null;

        Token char_literal52=null;
        MetraScriptParser.andExpression_return andExpression51 = null;

        MetraScriptParser.andExpression_return andExpression53 = null;


        Object char_literal52_tree=null;

        try { dbg.enterRule(getGrammarFileName(), "exclusiveOrExpression");
        if ( getRuleLevel()==0 ) {dbg.commence();}
        incRuleLevel();
        dbg.location(112, 1);

        try {
            if ( state.backtracking>0 && alreadyParsedRule(input, 12) ) { return retval; }
            // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:113:2: ( andExpression ( '^' andExpression )* )
            dbg.enterAlt(1);

            // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:113:4: andExpression ( '^' andExpression )*
            {
            root_0 = (Object)adaptor.nil();

            dbg.location(113,4);
            pushFollow(FOLLOW_andExpression_in_exclusiveOrExpression594);
            andExpression51=andExpression();

            state._fsp--;
            if (state.failed) return retval;
            if ( state.backtracking==0 ) adaptor.addChild(root_0, andExpression51.getTree());
            dbg.location(113,18);
            // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:113:18: ( '^' andExpression )*
            try { dbg.enterSubRule(9);

            loop9:
            do {
                int alt9=2;
                try { dbg.enterDecision(9);

                int LA9_0 = input.LA(1);

                if ( (LA9_0==61) ) {
                    alt9=1;
                }


                } finally {dbg.exitDecision(9);}

                switch (alt9) {
            	case 1 :
            	    dbg.enterAlt(1);

            	    // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:113:20: '^' andExpression
            	    {
            	    dbg.location(113,23);
            	    char_literal52=(Token)match(input,61,FOLLOW_61_in_exclusiveOrExpression598); if (state.failed) return retval;
            	    if ( state.backtracking==0 ) {
            	    char_literal52_tree = (Object)adaptor.create(char_literal52);
            	    root_0 = (Object)adaptor.becomeRoot(char_literal52_tree, root_0);
            	    }
            	    dbg.location(113,25);
            	    pushFollow(FOLLOW_andExpression_in_exclusiveOrExpression601);
            	    andExpression53=andExpression();

            	    state._fsp--;
            	    if (state.failed) return retval;
            	    if ( state.backtracking==0 ) adaptor.addChild(root_0, andExpression53.getTree());

            	    }
            	    break;

            	default :
            	    break loop9;
                }
            } while (true);
            } finally {dbg.exitSubRule(9);}


            }

            retval.stop = input.LT(-1);

            if ( state.backtracking==0 ) {

            retval.tree = (Object)adaptor.rulePostProcessing(root_0);
            adaptor.setTokenBoundaries(retval.tree, retval.start, retval.stop);
            }
        }
        catch (RecognitionException re) {
            reportError(re);
            recover(input,re);
    	retval.tree = (Object)adaptor.errorNode(input, retval.start, input.LT(-1), re);

        }
        finally {
            if ( state.backtracking>0 ) { memoize(input, 12, exclusiveOrExpression_StartIndex); }
        }
        dbg.location(114, 2);

        }
        finally {
            dbg.exitRule(getGrammarFileName(), "exclusiveOrExpression");
            decRuleLevel();
            if ( getRuleLevel()==0 ) {dbg.terminate();}
        }

        return retval;
    }
    // $ANTLR end "exclusiveOrExpression"

    public static class andExpression_return extends ParserRuleReturnScope {
        Object tree;
        public Object getTree() { return tree; }
    };

    // $ANTLR start "andExpression"
    // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:115:1: andExpression : equalityExpression ( '&' equalityExpression )* ;
    public final MetraScriptParser.andExpression_return andExpression() throws RecognitionException {
        MetraScriptParser.andExpression_return retval = new MetraScriptParser.andExpression_return();
        retval.start = input.LT(1);
        int andExpression_StartIndex = input.index();
        Object root_0 = null;

        Token char_literal55=null;
        MetraScriptParser.equalityExpression_return equalityExpression54 = null;

        MetraScriptParser.equalityExpression_return equalityExpression56 = null;


        Object char_literal55_tree=null;

        try { dbg.enterRule(getGrammarFileName(), "andExpression");
        if ( getRuleLevel()==0 ) {dbg.commence();}
        incRuleLevel();
        dbg.location(115, 1);

        try {
            if ( state.backtracking>0 && alreadyParsedRule(input, 13) ) { return retval; }
            // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:116:2: ( equalityExpression ( '&' equalityExpression )* )
            dbg.enterAlt(1);

            // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:116:4: equalityExpression ( '&' equalityExpression )*
            {
            root_0 = (Object)adaptor.nil();

            dbg.location(116,4);
            pushFollow(FOLLOW_equalityExpression_in_andExpression614);
            equalityExpression54=equalityExpression();

            state._fsp--;
            if (state.failed) return retval;
            if ( state.backtracking==0 ) adaptor.addChild(root_0, equalityExpression54.getTree());
            dbg.location(116,23);
            // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:116:23: ( '&' equalityExpression )*
            try { dbg.enterSubRule(10);

            loop10:
            do {
                int alt10=2;
                try { dbg.enterDecision(10);

                int LA10_0 = input.LA(1);

                if ( (LA10_0==62) ) {
                    alt10=1;
                }


                } finally {dbg.exitDecision(10);}

                switch (alt10) {
            	case 1 :
            	    dbg.enterAlt(1);

            	    // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:116:25: '&' equalityExpression
            	    {
            	    dbg.location(116,28);
            	    char_literal55=(Token)match(input,62,FOLLOW_62_in_andExpression618); if (state.failed) return retval;
            	    if ( state.backtracking==0 ) {
            	    char_literal55_tree = (Object)adaptor.create(char_literal55);
            	    root_0 = (Object)adaptor.becomeRoot(char_literal55_tree, root_0);
            	    }
            	    dbg.location(116,30);
            	    pushFollow(FOLLOW_equalityExpression_in_andExpression621);
            	    equalityExpression56=equalityExpression();

            	    state._fsp--;
            	    if (state.failed) return retval;
            	    if ( state.backtracking==0 ) adaptor.addChild(root_0, equalityExpression56.getTree());

            	    }
            	    break;

            	default :
            	    break loop10;
                }
            } while (true);
            } finally {dbg.exitSubRule(10);}


            }

            retval.stop = input.LT(-1);

            if ( state.backtracking==0 ) {

            retval.tree = (Object)adaptor.rulePostProcessing(root_0);
            adaptor.setTokenBoundaries(retval.tree, retval.start, retval.stop);
            }
        }
        catch (RecognitionException re) {
            reportError(re);
            recover(input,re);
    	retval.tree = (Object)adaptor.errorNode(input, retval.start, input.LT(-1), re);

        }
        finally {
            if ( state.backtracking>0 ) { memoize(input, 13, andExpression_StartIndex); }
        }
        dbg.location(117, 2);

        }
        finally {
            dbg.exitRule(getGrammarFileName(), "andExpression");
            decRuleLevel();
            if ( getRuleLevel()==0 ) {dbg.terminate();}
        }

        return retval;
    }
    // $ANTLR end "andExpression"

    public static class equalityExpression_return extends ParserRuleReturnScope {
        Object tree;
        public Object getTree() { return tree; }
    };

    // $ANTLR start "equalityExpression"
    // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:118:1: equalityExpression : instanceOfExpression ( ( '==' | '!=' | 'eq' | 'ne' ) instanceOfExpression )* ;
    public final MetraScriptParser.equalityExpression_return equalityExpression() throws RecognitionException {
        MetraScriptParser.equalityExpression_return retval = new MetraScriptParser.equalityExpression_return();
        retval.start = input.LT(1);
        int equalityExpression_StartIndex = input.index();
        Object root_0 = null;

        Token set58=null;
        MetraScriptParser.instanceOfExpression_return instanceOfExpression57 = null;

        MetraScriptParser.instanceOfExpression_return instanceOfExpression59 = null;


        Object set58_tree=null;

        try { dbg.enterRule(getGrammarFileName(), "equalityExpression");
        if ( getRuleLevel()==0 ) {dbg.commence();}
        incRuleLevel();
        dbg.location(118, 1);

        try {
            if ( state.backtracking>0 && alreadyParsedRule(input, 14) ) { return retval; }
            // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:119:2: ( instanceOfExpression ( ( '==' | '!=' | 'eq' | 'ne' ) instanceOfExpression )* )
            dbg.enterAlt(1);

            // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:119:4: instanceOfExpression ( ( '==' | '!=' | 'eq' | 'ne' ) instanceOfExpression )*
            {
            root_0 = (Object)adaptor.nil();

            dbg.location(119,4);
            pushFollow(FOLLOW_instanceOfExpression_in_equalityExpression634);
            instanceOfExpression57=instanceOfExpression();

            state._fsp--;
            if (state.failed) return retval;
            if ( state.backtracking==0 ) adaptor.addChild(root_0, instanceOfExpression57.getTree());
            dbg.location(119,25);
            // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:119:25: ( ( '==' | '!=' | 'eq' | 'ne' ) instanceOfExpression )*
            try { dbg.enterSubRule(11);

            loop11:
            do {
                int alt11=2;
                try { dbg.enterDecision(11);

                int LA11_0 = input.LA(1);

                if ( ((LA11_0>=63 && LA11_0<=66)) ) {
                    alt11=1;
                }


                } finally {dbg.exitDecision(11);}

                switch (alt11) {
            	case 1 :
            	    dbg.enterAlt(1);

            	    // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:119:27: ( '==' | '!=' | 'eq' | 'ne' ) instanceOfExpression
            	    {
            	    dbg.location(119,27);
            	    set58=(Token)input.LT(1);
            	    set58=(Token)input.LT(1);
            	    if ( (input.LA(1)>=63 && input.LA(1)<=66) ) {
            	        input.consume();
            	        if ( state.backtracking==0 ) root_0 = (Object)adaptor.becomeRoot((Object)adaptor.create(set58), root_0);
            	        state.errorRecovery=false;state.failed=false;
            	    }
            	    else {
            	        if (state.backtracking>0) {state.failed=true; return retval;}
            	        MismatchedSetException mse = new MismatchedSetException(null,input);
            	        dbg.recognitionException(mse);
            	        throw mse;
            	    }

            	    dbg.location(119,56);
            	    pushFollow(FOLLOW_instanceOfExpression_in_equalityExpression655);
            	    instanceOfExpression59=instanceOfExpression();

            	    state._fsp--;
            	    if (state.failed) return retval;
            	    if ( state.backtracking==0 ) adaptor.addChild(root_0, instanceOfExpression59.getTree());

            	    }
            	    break;

            	default :
            	    break loop11;
                }
            } while (true);
            } finally {dbg.exitSubRule(11);}


            }

            retval.stop = input.LT(-1);

            if ( state.backtracking==0 ) {

            retval.tree = (Object)adaptor.rulePostProcessing(root_0);
            adaptor.setTokenBoundaries(retval.tree, retval.start, retval.stop);
            }
        }
        catch (RecognitionException re) {
            reportError(re);
            recover(input,re);
    	retval.tree = (Object)adaptor.errorNode(input, retval.start, input.LT(-1), re);

        }
        finally {
            if ( state.backtracking>0 ) { memoize(input, 14, equalityExpression_StartIndex); }
        }
        dbg.location(120, 2);

        }
        finally {
            dbg.exitRule(getGrammarFileName(), "equalityExpression");
            decRuleLevel();
            if ( getRuleLevel()==0 ) {dbg.terminate();}
        }

        return retval;
    }
    // $ANTLR end "equalityExpression"

    public static class instanceOfExpression_return extends ParserRuleReturnScope {
        Object tree;
        public Object getTree() { return tree; }
    };

    // $ANTLR start "instanceOfExpression"
    // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:121:1: instanceOfExpression : relationalExpression ;
    public final MetraScriptParser.instanceOfExpression_return instanceOfExpression() throws RecognitionException {
        MetraScriptParser.instanceOfExpression_return retval = new MetraScriptParser.instanceOfExpression_return();
        retval.start = input.LT(1);
        int instanceOfExpression_StartIndex = input.index();
        Object root_0 = null;

        MetraScriptParser.relationalExpression_return relationalExpression60 = null;



        try { dbg.enterRule(getGrammarFileName(), "instanceOfExpression");
        if ( getRuleLevel()==0 ) {dbg.commence();}
        incRuleLevel();
        dbg.location(121, 1);

        try {
            if ( state.backtracking>0 && alreadyParsedRule(input, 15) ) { return retval; }
            // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:122:2: ( relationalExpression )
            dbg.enterAlt(1);

            // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:122:4: relationalExpression
            {
            root_0 = (Object)adaptor.nil();

            dbg.location(122,4);
            pushFollow(FOLLOW_relationalExpression_in_instanceOfExpression668);
            relationalExpression60=relationalExpression();

            state._fsp--;
            if (state.failed) return retval;
            if ( state.backtracking==0 ) adaptor.addChild(root_0, relationalExpression60.getTree());

            }

            retval.stop = input.LT(-1);

            if ( state.backtracking==0 ) {

            retval.tree = (Object)adaptor.rulePostProcessing(root_0);
            adaptor.setTokenBoundaries(retval.tree, retval.start, retval.stop);
            }
        }
        catch (RecognitionException re) {
            reportError(re);
            recover(input,re);
    	retval.tree = (Object)adaptor.errorNode(input, retval.start, input.LT(-1), re);

        }
        finally {
            if ( state.backtracking>0 ) { memoize(input, 15, instanceOfExpression_StartIndex); }
        }
        dbg.location(123, 2);

        }
        finally {
            dbg.exitRule(getGrammarFileName(), "instanceOfExpression");
            decRuleLevel();
            if ( getRuleLevel()==0 ) {dbg.terminate();}
        }

        return retval;
    }
    // $ANTLR end "instanceOfExpression"

    public static class relationalExpression_return extends ParserRuleReturnScope {
        Object tree;
        public Object getTree() { return tree; }
    };

    // $ANTLR start "relationalExpression"
    // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:124:1: relationalExpression : shiftExpression ( relationalOp shiftExpression )* ;
    public final MetraScriptParser.relationalExpression_return relationalExpression() throws RecognitionException {
        MetraScriptParser.relationalExpression_return retval = new MetraScriptParser.relationalExpression_return();
        retval.start = input.LT(1);
        int relationalExpression_StartIndex = input.index();
        Object root_0 = null;

        MetraScriptParser.shiftExpression_return shiftExpression61 = null;

        MetraScriptParser.relationalOp_return relationalOp62 = null;

        MetraScriptParser.shiftExpression_return shiftExpression63 = null;



        try { dbg.enterRule(getGrammarFileName(), "relationalExpression");
        if ( getRuleLevel()==0 ) {dbg.commence();}
        incRuleLevel();
        dbg.location(124, 1);

        try {
            if ( state.backtracking>0 && alreadyParsedRule(input, 16) ) { return retval; }
            // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:125:2: ( shiftExpression ( relationalOp shiftExpression )* )
            dbg.enterAlt(1);

            // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:125:4: shiftExpression ( relationalOp shiftExpression )*
            {
            root_0 = (Object)adaptor.nil();

            dbg.location(125,4);
            pushFollow(FOLLOW_shiftExpression_in_relationalExpression679);
            shiftExpression61=shiftExpression();

            state._fsp--;
            if (state.failed) return retval;
            if ( state.backtracking==0 ) adaptor.addChild(root_0, shiftExpression61.getTree());
            dbg.location(125,20);
            // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:125:20: ( relationalOp shiftExpression )*
            try { dbg.enterSubRule(12);

            loop12:
            do {
                int alt12=2;
                try { dbg.enterDecision(12);

                try {
                    isCyclicDecision = true;
                    alt12 = dfa12.predict(input);
                }
                catch (NoViableAltException nvae) {
                    dbg.recognitionException(nvae);
                    throw nvae;
                }
                } finally {dbg.exitDecision(12);}

                switch (alt12) {
            	case 1 :
            	    dbg.enterAlt(1);

            	    // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:125:22: relationalOp shiftExpression
            	    {
            	    dbg.location(125,34);
            	    pushFollow(FOLLOW_relationalOp_in_relationalExpression683);
            	    relationalOp62=relationalOp();

            	    state._fsp--;
            	    if (state.failed) return retval;
            	    if ( state.backtracking==0 ) root_0 = (Object)adaptor.becomeRoot(relationalOp62.getTree(), root_0);
            	    dbg.location(125,36);
            	    pushFollow(FOLLOW_shiftExpression_in_relationalExpression686);
            	    shiftExpression63=shiftExpression();

            	    state._fsp--;
            	    if (state.failed) return retval;
            	    if ( state.backtracking==0 ) adaptor.addChild(root_0, shiftExpression63.getTree());

            	    }
            	    break;

            	default :
            	    break loop12;
                }
            } while (true);
            } finally {dbg.exitSubRule(12);}


            }

            retval.stop = input.LT(-1);

            if ( state.backtracking==0 ) {

            retval.tree = (Object)adaptor.rulePostProcessing(root_0);
            adaptor.setTokenBoundaries(retval.tree, retval.start, retval.stop);
            }
        }
        catch (RecognitionException re) {
            reportError(re);
            recover(input,re);
    	retval.tree = (Object)adaptor.errorNode(input, retval.start, input.LT(-1), re);

        }
        finally {
            if ( state.backtracking>0 ) { memoize(input, 16, relationalExpression_StartIndex); }
        }
        dbg.location(126, 2);

        }
        finally {
            dbg.exitRule(getGrammarFileName(), "relationalExpression");
            decRuleLevel();
            if ( getRuleLevel()==0 ) {dbg.terminate();}
        }

        return retval;
    }
    // $ANTLR end "relationalExpression"

    public static class relationalOp_return extends ParserRuleReturnScope {
        Object tree;
        public Object getTree() { return tree; }
    };

    // $ANTLR start "relationalOp"
    // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:127:1: relationalOp : ( ( '<' '=' )=>t1= '<' t2= '=' {...}? | ( '>' '=' )=>t1= '>' t2= '=' {...}? | '<' | '>' | 'gt' | 'lt' | 'eq' | 'gte' | 'lte' | 'GT' | 'LT' | 'EQ' | 'GTE' | 'LTE' );
    public final MetraScriptParser.relationalOp_return relationalOp() throws RecognitionException {
        MetraScriptParser.relationalOp_return retval = new MetraScriptParser.relationalOp_return();
        retval.start = input.LT(1);
        int relationalOp_StartIndex = input.index();
        Object root_0 = null;

        Token t1=null;
        Token t2=null;
        Token char_literal64=null;
        Token char_literal65=null;
        Token string_literal66=null;
        Token string_literal67=null;
        Token string_literal68=null;
        Token string_literal69=null;
        Token string_literal70=null;
        Token string_literal71=null;
        Token string_literal72=null;
        Token string_literal73=null;
        Token string_literal74=null;
        Token string_literal75=null;

        Object t1_tree=null;
        Object t2_tree=null;
        Object char_literal64_tree=null;
        Object char_literal65_tree=null;
        Object string_literal66_tree=null;
        Object string_literal67_tree=null;
        Object string_literal68_tree=null;
        Object string_literal69_tree=null;
        Object string_literal70_tree=null;
        Object string_literal71_tree=null;
        Object string_literal72_tree=null;
        Object string_literal73_tree=null;
        Object string_literal74_tree=null;
        Object string_literal75_tree=null;

        try { dbg.enterRule(getGrammarFileName(), "relationalOp");
        if ( getRuleLevel()==0 ) {dbg.commence();}
        incRuleLevel();
        dbg.location(127, 1);

        try {
            if ( state.backtracking>0 && alreadyParsedRule(input, 17) ) { return retval; }
            // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:128:2: ( ( '<' '=' )=>t1= '<' t2= '=' {...}? | ( '>' '=' )=>t1= '>' t2= '=' {...}? | '<' | '>' | 'gt' | 'lt' | 'eq' | 'gte' | 'lte' | 'GT' | 'LT' | 'EQ' | 'GTE' | 'LTE' )
            int alt13=14;
            try { dbg.enterDecision(13);

            try {
                isCyclicDecision = true;
                alt13 = dfa13.predict(input);
            }
            catch (NoViableAltException nvae) {
                dbg.recognitionException(nvae);
                throw nvae;
            }
            } finally {dbg.exitDecision(13);}

            switch (alt13) {
                case 1 :
                    dbg.enterAlt(1);

                    // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:128:4: ( '<' '=' )=>t1= '<' t2= '=' {...}?
                    {
                    root_0 = (Object)adaptor.nil();

                    dbg.location(128,18);
                    t1=(Token)match(input,52,FOLLOW_52_in_relationalOp708); if (state.failed) return retval;
                    if ( state.backtracking==0 ) {
                    t1_tree = (Object)adaptor.create(t1);
                    adaptor.addChild(root_0, t1_tree);
                    }
                    dbg.location(128,25);
                    t2=(Token)match(input,41,FOLLOW_41_in_relationalOp712); if (state.failed) return retval;
                    if ( state.backtracking==0 ) {
                    t2_tree = (Object)adaptor.create(t2);
                    adaptor.addChild(root_0, t2_tree);
                    }
                    dbg.location(129,4);
                    if ( !(evalPredicate( t1.getLine() == t2.getLine() && 
                    	  t1.getCharPositionInLine() + 1 == t2.getCharPositionInLine() ," $t1.getLine() == $t2.getLine() && \r\n\t  $t1.getCharPositionInLine() + 1 == $t2.getCharPositionInLine() ")) ) {
                        if (state.backtracking>0) {state.failed=true; return retval;}
                        throw new FailedPredicateException(input, "relationalOp", " $t1.getLine() == $t2.getLine() && \r\n\t  $t1.getCharPositionInLine() + 1 == $t2.getCharPositionInLine() ");
                    }

                    }
                    break;
                case 2 :
                    dbg.enterAlt(2);

                    // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:131:4: ( '>' '=' )=>t1= '>' t2= '=' {...}?
                    {
                    root_0 = (Object)adaptor.nil();

                    dbg.location(131,18);
                    t1=(Token)match(input,53,FOLLOW_53_in_relationalOp732); if (state.failed) return retval;
                    if ( state.backtracking==0 ) {
                    t1_tree = (Object)adaptor.create(t1);
                    adaptor.addChild(root_0, t1_tree);
                    }
                    dbg.location(131,25);
                    t2=(Token)match(input,41,FOLLOW_41_in_relationalOp736); if (state.failed) return retval;
                    if ( state.backtracking==0 ) {
                    t2_tree = (Object)adaptor.create(t2);
                    adaptor.addChild(root_0, t2_tree);
                    }
                    dbg.location(132,4);
                    if ( !(evalPredicate( t1.getLine() == t2.getLine() && 
                    	  t1.getCharPositionInLine() + 1 == t2.getCharPositionInLine() ," $t1.getLine() == $t2.getLine() && \r\n\t  $t1.getCharPositionInLine() + 1 == $t2.getCharPositionInLine() ")) ) {
                        if (state.backtracking>0) {state.failed=true; return retval;}
                        throw new FailedPredicateException(input, "relationalOp", " $t1.getLine() == $t2.getLine() && \r\n\t  $t1.getCharPositionInLine() + 1 == $t2.getCharPositionInLine() ");
                    }

                    }
                    break;
                case 3 :
                    dbg.enterAlt(3);

                    // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:134:4: '<'
                    {
                    root_0 = (Object)adaptor.nil();

                    dbg.location(134,4);
                    char_literal64=(Token)match(input,52,FOLLOW_52_in_relationalOp747); if (state.failed) return retval;
                    if ( state.backtracking==0 ) {
                    char_literal64_tree = (Object)adaptor.create(char_literal64);
                    adaptor.addChild(root_0, char_literal64_tree);
                    }

                    }
                    break;
                case 4 :
                    dbg.enterAlt(4);

                    // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:135:4: '>'
                    {
                    root_0 = (Object)adaptor.nil();

                    dbg.location(135,4);
                    char_literal65=(Token)match(input,53,FOLLOW_53_in_relationalOp753); if (state.failed) return retval;
                    if ( state.backtracking==0 ) {
                    char_literal65_tree = (Object)adaptor.create(char_literal65);
                    adaptor.addChild(root_0, char_literal65_tree);
                    }

                    }
                    break;
                case 5 :
                    dbg.enterAlt(5);

                    // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:136:4: 'gt'
                    {
                    root_0 = (Object)adaptor.nil();

                    dbg.location(136,4);
                    string_literal66=(Token)match(input,67,FOLLOW_67_in_relationalOp759); if (state.failed) return retval;
                    if ( state.backtracking==0 ) {
                    string_literal66_tree = (Object)adaptor.create(string_literal66);
                    adaptor.addChild(root_0, string_literal66_tree);
                    }

                    }
                    break;
                case 6 :
                    dbg.enterAlt(6);

                    // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:137:4: 'lt'
                    {
                    root_0 = (Object)adaptor.nil();

                    dbg.location(137,4);
                    string_literal67=(Token)match(input,68,FOLLOW_68_in_relationalOp764); if (state.failed) return retval;
                    if ( state.backtracking==0 ) {
                    string_literal67_tree = (Object)adaptor.create(string_literal67);
                    adaptor.addChild(root_0, string_literal67_tree);
                    }

                    }
                    break;
                case 7 :
                    dbg.enterAlt(7);

                    // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:138:4: 'eq'
                    {
                    root_0 = (Object)adaptor.nil();

                    dbg.location(138,4);
                    string_literal68=(Token)match(input,65,FOLLOW_65_in_relationalOp769); if (state.failed) return retval;
                    if ( state.backtracking==0 ) {
                    string_literal68_tree = (Object)adaptor.create(string_literal68);
                    adaptor.addChild(root_0, string_literal68_tree);
                    }

                    }
                    break;
                case 8 :
                    dbg.enterAlt(8);

                    // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:139:4: 'gte'
                    {
                    root_0 = (Object)adaptor.nil();

                    dbg.location(139,4);
                    string_literal69=(Token)match(input,69,FOLLOW_69_in_relationalOp774); if (state.failed) return retval;
                    if ( state.backtracking==0 ) {
                    string_literal69_tree = (Object)adaptor.create(string_literal69);
                    adaptor.addChild(root_0, string_literal69_tree);
                    }

                    }
                    break;
                case 9 :
                    dbg.enterAlt(9);

                    // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:140:4: 'lte'
                    {
                    root_0 = (Object)adaptor.nil();

                    dbg.location(140,4);
                    string_literal70=(Token)match(input,70,FOLLOW_70_in_relationalOp779); if (state.failed) return retval;
                    if ( state.backtracking==0 ) {
                    string_literal70_tree = (Object)adaptor.create(string_literal70);
                    adaptor.addChild(root_0, string_literal70_tree);
                    }

                    }
                    break;
                case 10 :
                    dbg.enterAlt(10);

                    // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:141:4: 'GT'
                    {
                    root_0 = (Object)adaptor.nil();

                    dbg.location(141,4);
                    string_literal71=(Token)match(input,71,FOLLOW_71_in_relationalOp784); if (state.failed) return retval;
                    if ( state.backtracking==0 ) {
                    string_literal71_tree = (Object)adaptor.create(string_literal71);
                    adaptor.addChild(root_0, string_literal71_tree);
                    }

                    }
                    break;
                case 11 :
                    dbg.enterAlt(11);

                    // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:142:4: 'LT'
                    {
                    root_0 = (Object)adaptor.nil();

                    dbg.location(142,4);
                    string_literal72=(Token)match(input,72,FOLLOW_72_in_relationalOp789); if (state.failed) return retval;
                    if ( state.backtracking==0 ) {
                    string_literal72_tree = (Object)adaptor.create(string_literal72);
                    adaptor.addChild(root_0, string_literal72_tree);
                    }

                    }
                    break;
                case 12 :
                    dbg.enterAlt(12);

                    // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:143:4: 'EQ'
                    {
                    root_0 = (Object)adaptor.nil();

                    dbg.location(143,4);
                    string_literal73=(Token)match(input,73,FOLLOW_73_in_relationalOp794); if (state.failed) return retval;
                    if ( state.backtracking==0 ) {
                    string_literal73_tree = (Object)adaptor.create(string_literal73);
                    adaptor.addChild(root_0, string_literal73_tree);
                    }

                    }
                    break;
                case 13 :
                    dbg.enterAlt(13);

                    // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:144:4: 'GTE'
                    {
                    root_0 = (Object)adaptor.nil();

                    dbg.location(144,4);
                    string_literal74=(Token)match(input,74,FOLLOW_74_in_relationalOp799); if (state.failed) return retval;
                    if ( state.backtracking==0 ) {
                    string_literal74_tree = (Object)adaptor.create(string_literal74);
                    adaptor.addChild(root_0, string_literal74_tree);
                    }

                    }
                    break;
                case 14 :
                    dbg.enterAlt(14);

                    // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:145:4: 'LTE'
                    {
                    root_0 = (Object)adaptor.nil();

                    dbg.location(145,4);
                    string_literal75=(Token)match(input,75,FOLLOW_75_in_relationalOp804); if (state.failed) return retval;
                    if ( state.backtracking==0 ) {
                    string_literal75_tree = (Object)adaptor.create(string_literal75);
                    adaptor.addChild(root_0, string_literal75_tree);
                    }

                    }
                    break;

            }
            retval.stop = input.LT(-1);

            if ( state.backtracking==0 ) {

            retval.tree = (Object)adaptor.rulePostProcessing(root_0);
            adaptor.setTokenBoundaries(retval.tree, retval.start, retval.stop);
            }
        }
        catch (RecognitionException re) {
            reportError(re);
            recover(input,re);
    	retval.tree = (Object)adaptor.errorNode(input, retval.start, input.LT(-1), re);

        }
        finally {
            if ( state.backtracking>0 ) { memoize(input, 17, relationalOp_StartIndex); }
        }
        dbg.location(146, 2);

        }
        finally {
            dbg.exitRule(getGrammarFileName(), "relationalOp");
            decRuleLevel();
            if ( getRuleLevel()==0 ) {dbg.terminate();}
        }

        return retval;
    }
    // $ANTLR end "relationalOp"

    public static class shiftExpression_return extends ParserRuleReturnScope {
        Object tree;
        public Object getTree() { return tree; }
    };

    // $ANTLR start "shiftExpression"
    // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:147:1: shiftExpression : additiveExpression ( shiftOp additiveExpression )* ;
    public final MetraScriptParser.shiftExpression_return shiftExpression() throws RecognitionException {
        MetraScriptParser.shiftExpression_return retval = new MetraScriptParser.shiftExpression_return();
        retval.start = input.LT(1);
        int shiftExpression_StartIndex = input.index();
        Object root_0 = null;

        MetraScriptParser.additiveExpression_return additiveExpression76 = null;

        MetraScriptParser.shiftOp_return shiftOp77 = null;

        MetraScriptParser.additiveExpression_return additiveExpression78 = null;



        try { dbg.enterRule(getGrammarFileName(), "shiftExpression");
        if ( getRuleLevel()==0 ) {dbg.commence();}
        incRuleLevel();
        dbg.location(147, 1);

        try {
            if ( state.backtracking>0 && alreadyParsedRule(input, 18) ) { return retval; }
            // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:148:2: ( additiveExpression ( shiftOp additiveExpression )* )
            dbg.enterAlt(1);

            // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:148:4: additiveExpression ( shiftOp additiveExpression )*
            {
            root_0 = (Object)adaptor.nil();

            dbg.location(148,4);
            pushFollow(FOLLOW_additiveExpression_in_shiftExpression814);
            additiveExpression76=additiveExpression();

            state._fsp--;
            if (state.failed) return retval;
            if ( state.backtracking==0 ) adaptor.addChild(root_0, additiveExpression76.getTree());
            dbg.location(148,23);
            // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:148:23: ( shiftOp additiveExpression )*
            try { dbg.enterSubRule(14);

            loop14:
            do {
                int alt14=2;
                try { dbg.enterDecision(14);

                int LA14_0 = input.LA(1);

                if ( (LA14_0==52) ) {
                    int LA14_1 = input.LA(2);

                    if ( (LA14_1==52) ) {
                        int LA14_4 = input.LA(3);

                        if ( ((LA14_4>=FloatingPointLiteral && LA14_4<=DecimalLiteral)||(LA14_4>=31 && LA14_4<=32)||(LA14_4>=34 && LA14_4<=40)||LA14_4==42||(LA14_4>=76 && LA14_4<=77)||(LA14_4>=81 && LA14_4<=84)) ) {
                            alt14=1;
                        }


                    }


                }
                else if ( (LA14_0==53) ) {
                    int LA14_2 = input.LA(2);

                    if ( (LA14_2==53) ) {
                        int LA14_5 = input.LA(3);

                        if ( (LA14_5==53) ) {
                            int LA14_7 = input.LA(4);

                            if ( ((LA14_7>=FloatingPointLiteral && LA14_7<=DecimalLiteral)||(LA14_7>=31 && LA14_7<=32)||(LA14_7>=34 && LA14_7<=40)||LA14_7==42||(LA14_7>=76 && LA14_7<=77)||(LA14_7>=81 && LA14_7<=84)) ) {
                                alt14=1;
                            }


                        }
                        else if ( ((LA14_5>=FloatingPointLiteral && LA14_5<=DecimalLiteral)||(LA14_5>=31 && LA14_5<=32)||(LA14_5>=34 && LA14_5<=40)||LA14_5==42||(LA14_5>=76 && LA14_5<=77)||(LA14_5>=81 && LA14_5<=84)) ) {
                            alt14=1;
                        }


                    }


                }


                } finally {dbg.exitDecision(14);}

                switch (alt14) {
            	case 1 :
            	    dbg.enterAlt(1);

            	    // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:148:25: shiftOp additiveExpression
            	    {
            	    dbg.location(148,32);
            	    pushFollow(FOLLOW_shiftOp_in_shiftExpression818);
            	    shiftOp77=shiftOp();

            	    state._fsp--;
            	    if (state.failed) return retval;
            	    if ( state.backtracking==0 ) root_0 = (Object)adaptor.becomeRoot(shiftOp77.getTree(), root_0);
            	    dbg.location(148,34);
            	    pushFollow(FOLLOW_additiveExpression_in_shiftExpression821);
            	    additiveExpression78=additiveExpression();

            	    state._fsp--;
            	    if (state.failed) return retval;
            	    if ( state.backtracking==0 ) adaptor.addChild(root_0, additiveExpression78.getTree());

            	    }
            	    break;

            	default :
            	    break loop14;
                }
            } while (true);
            } finally {dbg.exitSubRule(14);}


            }

            retval.stop = input.LT(-1);

            if ( state.backtracking==0 ) {

            retval.tree = (Object)adaptor.rulePostProcessing(root_0);
            adaptor.setTokenBoundaries(retval.tree, retval.start, retval.stop);
            }
        }
        catch (RecognitionException re) {
            reportError(re);
            recover(input,re);
    	retval.tree = (Object)adaptor.errorNode(input, retval.start, input.LT(-1), re);

        }
        finally {
            if ( state.backtracking>0 ) { memoize(input, 18, shiftExpression_StartIndex); }
        }
        dbg.location(149, 2);

        }
        finally {
            dbg.exitRule(getGrammarFileName(), "shiftExpression");
            decRuleLevel();
            if ( getRuleLevel()==0 ) {dbg.terminate();}
        }

        return retval;
    }
    // $ANTLR end "shiftExpression"

    public static class shiftOp_return extends ParserRuleReturnScope {
        Object tree;
        public Object getTree() { return tree; }
    };

    // $ANTLR start "shiftOp"
    // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:150:1: shiftOp : ( ( '<' '<' )=>t1= '<' t2= '<' {...}? | ( '>' '>' '>' )=>t1= '>' t2= '>' t3= '>' {...}? | ( '>' '>' )=>t1= '>' t2= '>' {...}?);
    public final MetraScriptParser.shiftOp_return shiftOp() throws RecognitionException {
        MetraScriptParser.shiftOp_return retval = new MetraScriptParser.shiftOp_return();
        retval.start = input.LT(1);
        int shiftOp_StartIndex = input.index();
        Object root_0 = null;

        Token t1=null;
        Token t2=null;
        Token t3=null;

        Object t1_tree=null;
        Object t2_tree=null;
        Object t3_tree=null;

        try { dbg.enterRule(getGrammarFileName(), "shiftOp");
        if ( getRuleLevel()==0 ) {dbg.commence();}
        incRuleLevel();
        dbg.location(150, 1);

        try {
            if ( state.backtracking>0 && alreadyParsedRule(input, 19) ) { return retval; }
            // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:151:2: ( ( '<' '<' )=>t1= '<' t2= '<' {...}? | ( '>' '>' '>' )=>t1= '>' t2= '>' t3= '>' {...}? | ( '>' '>' )=>t1= '>' t2= '>' {...}?)
            int alt15=3;
            try { dbg.enterDecision(15);

            try {
                isCyclicDecision = true;
                alt15 = dfa15.predict(input);
            }
            catch (NoViableAltException nvae) {
                dbg.recognitionException(nvae);
                throw nvae;
            }
            } finally {dbg.exitDecision(15);}

            switch (alt15) {
                case 1 :
                    dbg.enterAlt(1);

                    // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:151:4: ( '<' '<' )=>t1= '<' t2= '<' {...}?
                    {
                    root_0 = (Object)adaptor.nil();

                    dbg.location(151,18);
                    t1=(Token)match(input,52,FOLLOW_52_in_shiftOp843); if (state.failed) return retval;
                    if ( state.backtracking==0 ) {
                    t1_tree = (Object)adaptor.create(t1);
                    adaptor.addChild(root_0, t1_tree);
                    }
                    dbg.location(151,25);
                    t2=(Token)match(input,52,FOLLOW_52_in_shiftOp847); if (state.failed) return retval;
                    if ( state.backtracking==0 ) {
                    t2_tree = (Object)adaptor.create(t2);
                    adaptor.addChild(root_0, t2_tree);
                    }
                    dbg.location(152,4);
                    if ( !(evalPredicate( t1.getLine() == t2.getLine() && 
                    	  t1.getCharPositionInLine() + 1 == t2.getCharPositionInLine() ," $t1.getLine() == $t2.getLine() && \r\n\t  $t1.getCharPositionInLine() + 1 == $t2.getCharPositionInLine() ")) ) {
                        if (state.backtracking>0) {state.failed=true; return retval;}
                        throw new FailedPredicateException(input, "shiftOp", " $t1.getLine() == $t2.getLine() && \r\n\t  $t1.getCharPositionInLine() + 1 == $t2.getCharPositionInLine() ");
                    }

                    }
                    break;
                case 2 :
                    dbg.enterAlt(2);

                    // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:154:4: ( '>' '>' '>' )=>t1= '>' t2= '>' t3= '>' {...}?
                    {
                    root_0 = (Object)adaptor.nil();

                    dbg.location(154,22);
                    t1=(Token)match(input,53,FOLLOW_53_in_shiftOp869); if (state.failed) return retval;
                    if ( state.backtracking==0 ) {
                    t1_tree = (Object)adaptor.create(t1);
                    adaptor.addChild(root_0, t1_tree);
                    }
                    dbg.location(154,29);
                    t2=(Token)match(input,53,FOLLOW_53_in_shiftOp873); if (state.failed) return retval;
                    if ( state.backtracking==0 ) {
                    t2_tree = (Object)adaptor.create(t2);
                    adaptor.addChild(root_0, t2_tree);
                    }
                    dbg.location(154,36);
                    t3=(Token)match(input,53,FOLLOW_53_in_shiftOp877); if (state.failed) return retval;
                    if ( state.backtracking==0 ) {
                    t3_tree = (Object)adaptor.create(t3);
                    adaptor.addChild(root_0, t3_tree);
                    }
                    dbg.location(155,4);
                    if ( !(evalPredicate( t1.getLine() == t2.getLine() && 
                    	  t1.getCharPositionInLine() + 1 == t2.getCharPositionInLine() &&
                    	  t2.getLine() == t3.getLine() && 
                    	  t2.getCharPositionInLine() + 1 == t3.getCharPositionInLine() ," $t1.getLine() == $t2.getLine() && \r\n\t  $t1.getCharPositionInLine() + 1 == $t2.getCharPositionInLine() &&\r\n\t  $t2.getLine() == $t3.getLine() && \r\n\t  $t2.getCharPositionInLine() + 1 == $t3.getCharPositionInLine() ")) ) {
                        if (state.backtracking>0) {state.failed=true; return retval;}
                        throw new FailedPredicateException(input, "shiftOp", " $t1.getLine() == $t2.getLine() && \r\n\t  $t1.getCharPositionInLine() + 1 == $t2.getCharPositionInLine() &&\r\n\t  $t2.getLine() == $t3.getLine() && \r\n\t  $t2.getCharPositionInLine() + 1 == $t3.getCharPositionInLine() ");
                    }

                    }
                    break;
                case 3 :
                    dbg.enterAlt(3);

                    // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:159:4: ( '>' '>' )=>t1= '>' t2= '>' {...}?
                    {
                    root_0 = (Object)adaptor.nil();

                    dbg.location(159,18);
                    t1=(Token)match(input,53,FOLLOW_53_in_shiftOp897); if (state.failed) return retval;
                    if ( state.backtracking==0 ) {
                    t1_tree = (Object)adaptor.create(t1);
                    adaptor.addChild(root_0, t1_tree);
                    }
                    dbg.location(159,25);
                    t2=(Token)match(input,53,FOLLOW_53_in_shiftOp901); if (state.failed) return retval;
                    if ( state.backtracking==0 ) {
                    t2_tree = (Object)adaptor.create(t2);
                    adaptor.addChild(root_0, t2_tree);
                    }
                    dbg.location(160,4);
                    if ( !(evalPredicate( t1.getLine() == t2.getLine() && 
                    	  t1.getCharPositionInLine() + 1 == t2.getCharPositionInLine() ," $t1.getLine() == $t2.getLine() && \r\n\t  $t1.getCharPositionInLine() + 1 == $t2.getCharPositionInLine() ")) ) {
                        if (state.backtracking>0) {state.failed=true; return retval;}
                        throw new FailedPredicateException(input, "shiftOp", " $t1.getLine() == $t2.getLine() && \r\n\t  $t1.getCharPositionInLine() + 1 == $t2.getCharPositionInLine() ");
                    }

                    }
                    break;

            }
            retval.stop = input.LT(-1);

            if ( state.backtracking==0 ) {

            retval.tree = (Object)adaptor.rulePostProcessing(root_0);
            adaptor.setTokenBoundaries(retval.tree, retval.start, retval.stop);
            }
        }
        catch (RecognitionException re) {
            reportError(re);
            recover(input,re);
    	retval.tree = (Object)adaptor.errorNode(input, retval.start, input.LT(-1), re);

        }
        finally {
            if ( state.backtracking>0 ) { memoize(input, 19, shiftOp_StartIndex); }
        }
        dbg.location(162, 2);

        }
        finally {
            dbg.exitRule(getGrammarFileName(), "shiftOp");
            decRuleLevel();
            if ( getRuleLevel()==0 ) {dbg.terminate();}
        }

        return retval;
    }
    // $ANTLR end "shiftOp"

    public static class additiveExpression_return extends ParserRuleReturnScope {
        Object tree;
        public Object getTree() { return tree; }
    };

    // $ANTLR start "additiveExpression"
    // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:163:1: additiveExpression : multiplicativeExpression ( ( '+' | '-' ) multiplicativeExpression )* ;
    public final MetraScriptParser.additiveExpression_return additiveExpression() throws RecognitionException {
        MetraScriptParser.additiveExpression_return retval = new MetraScriptParser.additiveExpression_return();
        retval.start = input.LT(1);
        int additiveExpression_StartIndex = input.index();
        Object root_0 = null;

        Token set80=null;
        MetraScriptParser.multiplicativeExpression_return multiplicativeExpression79 = null;

        MetraScriptParser.multiplicativeExpression_return multiplicativeExpression81 = null;


        Object set80_tree=null;

        try { dbg.enterRule(getGrammarFileName(), "additiveExpression");
        if ( getRuleLevel()==0 ) {dbg.commence();}
        incRuleLevel();
        dbg.location(163, 1);

        try {
            if ( state.backtracking>0 && alreadyParsedRule(input, 20) ) { return retval; }
            // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:164:2: ( multiplicativeExpression ( ( '+' | '-' ) multiplicativeExpression )* )
            dbg.enterAlt(1);

            // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:164:4: multiplicativeExpression ( ( '+' | '-' ) multiplicativeExpression )*
            {
            root_0 = (Object)adaptor.nil();

            dbg.location(164,4);
            pushFollow(FOLLOW_multiplicativeExpression_in_additiveExpression916);
            multiplicativeExpression79=multiplicativeExpression();

            state._fsp--;
            if (state.failed) return retval;
            if ( state.backtracking==0 ) adaptor.addChild(root_0, multiplicativeExpression79.getTree());
            dbg.location(164,29);
            // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:164:29: ( ( '+' | '-' ) multiplicativeExpression )*
            try { dbg.enterSubRule(16);

            loop16:
            do {
                int alt16=2;
                try { dbg.enterDecision(16);

                int LA16_0 = input.LA(1);

                if ( ((LA16_0>=76 && LA16_0<=77)) ) {
                    alt16=1;
                }


                } finally {dbg.exitDecision(16);}

                switch (alt16) {
            	case 1 :
            	    dbg.enterAlt(1);

            	    // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:164:31: ( '+' | '-' ) multiplicativeExpression
            	    {
            	    dbg.location(164,31);
            	    set80=(Token)input.LT(1);
            	    set80=(Token)input.LT(1);
            	    if ( (input.LA(1)>=76 && input.LA(1)<=77) ) {
            	        input.consume();
            	        if ( state.backtracking==0 ) root_0 = (Object)adaptor.becomeRoot((Object)adaptor.create(set80), root_0);
            	        state.errorRecovery=false;state.failed=false;
            	    }
            	    else {
            	        if (state.backtracking>0) {state.failed=true; return retval;}
            	        MismatchedSetException mse = new MismatchedSetException(null,input);
            	        dbg.recognitionException(mse);
            	        throw mse;
            	    }

            	    dbg.location(164,44);
            	    pushFollow(FOLLOW_multiplicativeExpression_in_additiveExpression929);
            	    multiplicativeExpression81=multiplicativeExpression();

            	    state._fsp--;
            	    if (state.failed) return retval;
            	    if ( state.backtracking==0 ) adaptor.addChild(root_0, multiplicativeExpression81.getTree());

            	    }
            	    break;

            	default :
            	    break loop16;
                }
            } while (true);
            } finally {dbg.exitSubRule(16);}


            }

            retval.stop = input.LT(-1);

            if ( state.backtracking==0 ) {

            retval.tree = (Object)adaptor.rulePostProcessing(root_0);
            adaptor.setTokenBoundaries(retval.tree, retval.start, retval.stop);
            }
        }
        catch (RecognitionException re) {
            reportError(re);
            recover(input,re);
    	retval.tree = (Object)adaptor.errorNode(input, retval.start, input.LT(-1), re);

        }
        finally {
            if ( state.backtracking>0 ) { memoize(input, 20, additiveExpression_StartIndex); }
        }
        dbg.location(165, 2);

        }
        finally {
            dbg.exitRule(getGrammarFileName(), "additiveExpression");
            decRuleLevel();
            if ( getRuleLevel()==0 ) {dbg.terminate();}
        }

        return retval;
    }
    // $ANTLR end "additiveExpression"

    public static class multiplicativeExpression_return extends ParserRuleReturnScope {
        Object tree;
        public Object getTree() { return tree; }
    };

    // $ANTLR start "multiplicativeExpression"
    // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:166:1: multiplicativeExpression : unaryExpression ( ( '*' | '/' | '%' ) unaryExpression )* ;
    public final MetraScriptParser.multiplicativeExpression_return multiplicativeExpression() throws RecognitionException {
        MetraScriptParser.multiplicativeExpression_return retval = new MetraScriptParser.multiplicativeExpression_return();
        retval.start = input.LT(1);
        int multiplicativeExpression_StartIndex = input.index();
        Object root_0 = null;

        Token set83=null;
        MetraScriptParser.unaryExpression_return unaryExpression82 = null;

        MetraScriptParser.unaryExpression_return unaryExpression84 = null;


        Object set83_tree=null;

        try { dbg.enterRule(getGrammarFileName(), "multiplicativeExpression");
        if ( getRuleLevel()==0 ) {dbg.commence();}
        incRuleLevel();
        dbg.location(166, 1);

        try {
            if ( state.backtracking>0 && alreadyParsedRule(input, 21) ) { return retval; }
            // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:167:2: ( unaryExpression ( ( '*' | '/' | '%' ) unaryExpression )* )
            dbg.enterAlt(1);

            // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:167:4: unaryExpression ( ( '*' | '/' | '%' ) unaryExpression )*
            {
            root_0 = (Object)adaptor.nil();

            dbg.location(167,4);
            pushFollow(FOLLOW_unaryExpression_in_multiplicativeExpression942);
            unaryExpression82=unaryExpression();

            state._fsp--;
            if (state.failed) return retval;
            if ( state.backtracking==0 ) adaptor.addChild(root_0, unaryExpression82.getTree());
            dbg.location(167,20);
            // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:167:20: ( ( '*' | '/' | '%' ) unaryExpression )*
            try { dbg.enterSubRule(17);

            loop17:
            do {
                int alt17=2;
                try { dbg.enterDecision(17);

                int LA17_0 = input.LA(1);

                if ( ((LA17_0>=78 && LA17_0<=80)) ) {
                    alt17=1;
                }


                } finally {dbg.exitDecision(17);}

                switch (alt17) {
            	case 1 :
            	    dbg.enterAlt(1);

            	    // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:167:22: ( '*' | '/' | '%' ) unaryExpression
            	    {
            	    dbg.location(167,22);
            	    set83=(Token)input.LT(1);
            	    set83=(Token)input.LT(1);
            	    if ( (input.LA(1)>=78 && input.LA(1)<=80) ) {
            	        input.consume();
            	        if ( state.backtracking==0 ) root_0 = (Object)adaptor.becomeRoot((Object)adaptor.create(set83), root_0);
            	        state.errorRecovery=false;state.failed=false;
            	    }
            	    else {
            	        if (state.backtracking>0) {state.failed=true; return retval;}
            	        MismatchedSetException mse = new MismatchedSetException(null,input);
            	        dbg.recognitionException(mse);
            	        throw mse;
            	    }

            	    dbg.location(167,43);
            	    pushFollow(FOLLOW_unaryExpression_in_multiplicativeExpression961);
            	    unaryExpression84=unaryExpression();

            	    state._fsp--;
            	    if (state.failed) return retval;
            	    if ( state.backtracking==0 ) adaptor.addChild(root_0, unaryExpression84.getTree());

            	    }
            	    break;

            	default :
            	    break loop17;
                }
            } while (true);
            } finally {dbg.exitSubRule(17);}


            }

            retval.stop = input.LT(-1);

            if ( state.backtracking==0 ) {

            retval.tree = (Object)adaptor.rulePostProcessing(root_0);
            adaptor.setTokenBoundaries(retval.tree, retval.start, retval.stop);
            }
        }
        catch (RecognitionException re) {
            reportError(re);
            recover(input,re);
    	retval.tree = (Object)adaptor.errorNode(input, retval.start, input.LT(-1), re);

        }
        finally {
            if ( state.backtracking>0 ) { memoize(input, 21, multiplicativeExpression_StartIndex); }
        }
        dbg.location(168, 2);

        }
        finally {
            dbg.exitRule(getGrammarFileName(), "multiplicativeExpression");
            decRuleLevel();
            if ( getRuleLevel()==0 ) {dbg.terminate();}
        }

        return retval;
    }
    // $ANTLR end "multiplicativeExpression"

    public static class unaryExpression_return extends ParserRuleReturnScope {
        Object tree;
        public Object getTree() { return tree; }
    };

    // $ANTLR start "unaryExpression"
    // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:169:1: unaryExpression : ( '+' unaryExpression | '-' unaryExpression | '++' unaryExpression | '--' unaryExpression | unaryExpressionNotPlusMinus );
    public final MetraScriptParser.unaryExpression_return unaryExpression() throws RecognitionException {
        MetraScriptParser.unaryExpression_return retval = new MetraScriptParser.unaryExpression_return();
        retval.start = input.LT(1);
        int unaryExpression_StartIndex = input.index();
        Object root_0 = null;

        Token char_literal85=null;
        Token char_literal87=null;
        Token string_literal89=null;
        Token string_literal91=null;
        MetraScriptParser.unaryExpression_return unaryExpression86 = null;

        MetraScriptParser.unaryExpression_return unaryExpression88 = null;

        MetraScriptParser.unaryExpression_return unaryExpression90 = null;

        MetraScriptParser.unaryExpression_return unaryExpression92 = null;

        MetraScriptParser.unaryExpressionNotPlusMinus_return unaryExpressionNotPlusMinus93 = null;


        Object char_literal85_tree=null;
        Object char_literal87_tree=null;
        Object string_literal89_tree=null;
        Object string_literal91_tree=null;

        try { dbg.enterRule(getGrammarFileName(), "unaryExpression");
        if ( getRuleLevel()==0 ) {dbg.commence();}
        incRuleLevel();
        dbg.location(169, 1);

        try {
            if ( state.backtracking>0 && alreadyParsedRule(input, 22) ) { return retval; }
            // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:170:2: ( '+' unaryExpression | '-' unaryExpression | '++' unaryExpression | '--' unaryExpression | unaryExpressionNotPlusMinus )
            int alt18=5;
            try { dbg.enterDecision(18);

            switch ( input.LA(1) ) {
            case 76:
                {
                alt18=1;
                }
                break;
            case 77:
                {
                alt18=2;
                }
                break;
            case 81:
                {
                alt18=3;
                }
                break;
            case 82:
                {
                alt18=4;
                }
                break;
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
            case 83:
            case 84:
                {
                alt18=5;
                }
                break;
            default:
                if (state.backtracking>0) {state.failed=true; return retval;}
                NoViableAltException nvae =
                    new NoViableAltException("", 18, 0, input);

                dbg.recognitionException(nvae);
                throw nvae;
            }

            } finally {dbg.exitDecision(18);}

            switch (alt18) {
                case 1 :
                    dbg.enterAlt(1);

                    // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:170:4: '+' unaryExpression
                    {
                    root_0 = (Object)adaptor.nil();

                    dbg.location(170,7);
                    char_literal85=(Token)match(input,76,FOLLOW_76_in_unaryExpression974); if (state.failed) return retval;
                    if ( state.backtracking==0 ) {
                    char_literal85_tree = (Object)adaptor.create(char_literal85);
                    root_0 = (Object)adaptor.becomeRoot(char_literal85_tree, root_0);
                    }
                    dbg.location(170,9);
                    pushFollow(FOLLOW_unaryExpression_in_unaryExpression977);
                    unaryExpression86=unaryExpression();

                    state._fsp--;
                    if (state.failed) return retval;
                    if ( state.backtracking==0 ) adaptor.addChild(root_0, unaryExpression86.getTree());

                    }
                    break;
                case 2 :
                    dbg.enterAlt(2);

                    // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:171:4: '-' unaryExpression
                    {
                    root_0 = (Object)adaptor.nil();

                    dbg.location(171,7);
                    char_literal87=(Token)match(input,77,FOLLOW_77_in_unaryExpression982); if (state.failed) return retval;
                    if ( state.backtracking==0 ) {
                    char_literal87_tree = (Object)adaptor.create(char_literal87);
                    root_0 = (Object)adaptor.becomeRoot(char_literal87_tree, root_0);
                    }
                    dbg.location(171,9);
                    pushFollow(FOLLOW_unaryExpression_in_unaryExpression985);
                    unaryExpression88=unaryExpression();

                    state._fsp--;
                    if (state.failed) return retval;
                    if ( state.backtracking==0 ) adaptor.addChild(root_0, unaryExpression88.getTree());

                    }
                    break;
                case 3 :
                    dbg.enterAlt(3);

                    // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:172:4: '++' unaryExpression
                    {
                    root_0 = (Object)adaptor.nil();

                    dbg.location(172,8);
                    string_literal89=(Token)match(input,81,FOLLOW_81_in_unaryExpression990); if (state.failed) return retval;
                    if ( state.backtracking==0 ) {
                    string_literal89_tree = (Object)adaptor.create(string_literal89);
                    root_0 = (Object)adaptor.becomeRoot(string_literal89_tree, root_0);
                    }
                    dbg.location(172,10);
                    pushFollow(FOLLOW_unaryExpression_in_unaryExpression993);
                    unaryExpression90=unaryExpression();

                    state._fsp--;
                    if (state.failed) return retval;
                    if ( state.backtracking==0 ) adaptor.addChild(root_0, unaryExpression90.getTree());

                    }
                    break;
                case 4 :
                    dbg.enterAlt(4);

                    // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:173:4: '--' unaryExpression
                    {
                    root_0 = (Object)adaptor.nil();

                    dbg.location(173,8);
                    string_literal91=(Token)match(input,82,FOLLOW_82_in_unaryExpression998); if (state.failed) return retval;
                    if ( state.backtracking==0 ) {
                    string_literal91_tree = (Object)adaptor.create(string_literal91);
                    root_0 = (Object)adaptor.becomeRoot(string_literal91_tree, root_0);
                    }
                    dbg.location(173,10);
                    pushFollow(FOLLOW_unaryExpression_in_unaryExpression1001);
                    unaryExpression92=unaryExpression();

                    state._fsp--;
                    if (state.failed) return retval;
                    if ( state.backtracking==0 ) adaptor.addChild(root_0, unaryExpression92.getTree());

                    }
                    break;
                case 5 :
                    dbg.enterAlt(5);

                    // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:174:4: unaryExpressionNotPlusMinus
                    {
                    root_0 = (Object)adaptor.nil();

                    dbg.location(174,4);
                    pushFollow(FOLLOW_unaryExpressionNotPlusMinus_in_unaryExpression1006);
                    unaryExpressionNotPlusMinus93=unaryExpressionNotPlusMinus();

                    state._fsp--;
                    if (state.failed) return retval;
                    if ( state.backtracking==0 ) adaptor.addChild(root_0, unaryExpressionNotPlusMinus93.getTree());

                    }
                    break;

            }
            retval.stop = input.LT(-1);

            if ( state.backtracking==0 ) {

            retval.tree = (Object)adaptor.rulePostProcessing(root_0);
            adaptor.setTokenBoundaries(retval.tree, retval.start, retval.stop);
            }
        }
        catch (RecognitionException re) {
            reportError(re);
            recover(input,re);
    	retval.tree = (Object)adaptor.errorNode(input, retval.start, input.LT(-1), re);

        }
        finally {
            if ( state.backtracking>0 ) { memoize(input, 22, unaryExpression_StartIndex); }
        }
        dbg.location(175, 2);

        }
        finally {
            dbg.exitRule(getGrammarFileName(), "unaryExpression");
            decRuleLevel();
            if ( getRuleLevel()==0 ) {dbg.terminate();}
        }

        return retval;
    }
    // $ANTLR end "unaryExpression"

    public static class unaryExpressionNotPlusMinus_return extends ParserRuleReturnScope {
        Object tree;
        public Object getTree() { return tree; }
    };

    // $ANTLR start "unaryExpressionNotPlusMinus"
    // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:176:1: unaryExpressionNotPlusMinus : ( '~' unaryExpression | '!' unaryExpression | castExpression | primary ( '++' | '--' ) | primary );
    public final MetraScriptParser.unaryExpressionNotPlusMinus_return unaryExpressionNotPlusMinus() throws RecognitionException {
        MetraScriptParser.unaryExpressionNotPlusMinus_return retval = new MetraScriptParser.unaryExpressionNotPlusMinus_return();
        retval.start = input.LT(1);
        int unaryExpressionNotPlusMinus_StartIndex = input.index();
        Object root_0 = null;

        Token char_literal94=null;
        Token char_literal96=null;
        Token set100=null;
        MetraScriptParser.unaryExpression_return unaryExpression95 = null;

        MetraScriptParser.unaryExpression_return unaryExpression97 = null;

        MetraScriptParser.castExpression_return castExpression98 = null;

        MetraScriptParser.primary_return primary99 = null;

        MetraScriptParser.primary_return primary101 = null;


        Object char_literal94_tree=null;
        Object char_literal96_tree=null;
        Object set100_tree=null;

        try { dbg.enterRule(getGrammarFileName(), "unaryExpressionNotPlusMinus");
        if ( getRuleLevel()==0 ) {dbg.commence();}
        incRuleLevel();
        dbg.location(176, 1);

        try {
            if ( state.backtracking>0 && alreadyParsedRule(input, 23) ) { return retval; }
            // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:177:2: ( '~' unaryExpression | '!' unaryExpression | castExpression | primary ( '++' | '--' ) | primary )
            int alt19=5;
            try { dbg.enterDecision(19);

            try {
                isCyclicDecision = true;
                alt19 = dfa19.predict(input);
            }
            catch (NoViableAltException nvae) {
                dbg.recognitionException(nvae);
                throw nvae;
            }
            } finally {dbg.exitDecision(19);}

            switch (alt19) {
                case 1 :
                    dbg.enterAlt(1);

                    // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:177:4: '~' unaryExpression
                    {
                    root_0 = (Object)adaptor.nil();

                    dbg.location(177,7);
                    char_literal94=(Token)match(input,42,FOLLOW_42_in_unaryExpressionNotPlusMinus1016); if (state.failed) return retval;
                    if ( state.backtracking==0 ) {
                    char_literal94_tree = (Object)adaptor.create(char_literal94);
                    root_0 = (Object)adaptor.becomeRoot(char_literal94_tree, root_0);
                    }
                    dbg.location(177,9);
                    pushFollow(FOLLOW_unaryExpression_in_unaryExpressionNotPlusMinus1019);
                    unaryExpression95=unaryExpression();

                    state._fsp--;
                    if (state.failed) return retval;
                    if ( state.backtracking==0 ) adaptor.addChild(root_0, unaryExpression95.getTree());

                    }
                    break;
                case 2 :
                    dbg.enterAlt(2);

                    // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:178:4: '!' unaryExpression
                    {
                    root_0 = (Object)adaptor.nil();

                    dbg.location(178,7);
                    char_literal96=(Token)match(input,83,FOLLOW_83_in_unaryExpressionNotPlusMinus1024); if (state.failed) return retval;
                    if ( state.backtracking==0 ) {
                    char_literal96_tree = (Object)adaptor.create(char_literal96);
                    root_0 = (Object)adaptor.becomeRoot(char_literal96_tree, root_0);
                    }
                    dbg.location(178,9);
                    pushFollow(FOLLOW_unaryExpression_in_unaryExpressionNotPlusMinus1027);
                    unaryExpression97=unaryExpression();

                    state._fsp--;
                    if (state.failed) return retval;
                    if ( state.backtracking==0 ) adaptor.addChild(root_0, unaryExpression97.getTree());

                    }
                    break;
                case 3 :
                    dbg.enterAlt(3);

                    // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:179:4: castExpression
                    {
                    root_0 = (Object)adaptor.nil();

                    dbg.location(179,4);
                    pushFollow(FOLLOW_castExpression_in_unaryExpressionNotPlusMinus1032);
                    castExpression98=castExpression();

                    state._fsp--;
                    if (state.failed) return retval;
                    if ( state.backtracking==0 ) adaptor.addChild(root_0, castExpression98.getTree());

                    }
                    break;
                case 4 :
                    dbg.enterAlt(4);

                    // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:180:4: primary ( '++' | '--' )
                    {
                    root_0 = (Object)adaptor.nil();

                    dbg.location(180,4);
                    pushFollow(FOLLOW_primary_in_unaryExpressionNotPlusMinus1037);
                    primary99=primary();

                    state._fsp--;
                    if (state.failed) return retval;
                    if ( state.backtracking==0 ) adaptor.addChild(root_0, primary99.getTree());
                    dbg.location(180,12);
                    set100=(Token)input.LT(1);
                    set100=(Token)input.LT(1);
                    if ( (input.LA(1)>=81 && input.LA(1)<=82) ) {
                        input.consume();
                        if ( state.backtracking==0 ) root_0 = (Object)adaptor.becomeRoot((Object)adaptor.create(set100), root_0);
                        state.errorRecovery=false;state.failed=false;
                    }
                    else {
                        if (state.backtracking>0) {state.failed=true; return retval;}
                        MismatchedSetException mse = new MismatchedSetException(null,input);
                        dbg.recognitionException(mse);
                        throw mse;
                    }


                    }
                    break;
                case 5 :
                    dbg.enterAlt(5);

                    // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:181:4: primary
                    {
                    root_0 = (Object)adaptor.nil();

                    dbg.location(181,4);
                    pushFollow(FOLLOW_primary_in_unaryExpressionNotPlusMinus1051);
                    primary101=primary();

                    state._fsp--;
                    if (state.failed) return retval;
                    if ( state.backtracking==0 ) adaptor.addChild(root_0, primary101.getTree());

                    }
                    break;

            }
            retval.stop = input.LT(-1);

            if ( state.backtracking==0 ) {

            retval.tree = (Object)adaptor.rulePostProcessing(root_0);
            adaptor.setTokenBoundaries(retval.tree, retval.start, retval.stop);
            }
        }
        catch (RecognitionException re) {
            reportError(re);
            recover(input,re);
    	retval.tree = (Object)adaptor.errorNode(input, retval.start, input.LT(-1), re);

        }
        finally {
            if ( state.backtracking>0 ) { memoize(input, 23, unaryExpressionNotPlusMinus_StartIndex); }
        }
        dbg.location(182, 2);

        }
        finally {
            dbg.exitRule(getGrammarFileName(), "unaryExpressionNotPlusMinus");
            decRuleLevel();
            if ( getRuleLevel()==0 ) {dbg.terminate();}
        }

        return retval;
    }
    // $ANTLR end "unaryExpressionNotPlusMinus"

    public static class castExpression_return extends ParserRuleReturnScope {
        Object tree;
        public Object getTree() { return tree; }
    };

    // $ANTLR start "castExpression"
    // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:183:1: castExpression : ( '(' Id ')' unaryExpression | '(' Id ')' unaryExpressionNotPlusMinus );
    public final MetraScriptParser.castExpression_return castExpression() throws RecognitionException {
        MetraScriptParser.castExpression_return retval = new MetraScriptParser.castExpression_return();
        retval.start = input.LT(1);
        int castExpression_StartIndex = input.index();
        Object root_0 = null;

        Token char_literal102=null;
        Token Id103=null;
        Token char_literal104=null;
        Token char_literal106=null;
        Token Id107=null;
        Token char_literal108=null;
        MetraScriptParser.unaryExpression_return unaryExpression105 = null;

        MetraScriptParser.unaryExpressionNotPlusMinus_return unaryExpressionNotPlusMinus109 = null;


        Object char_literal102_tree=null;
        Object Id103_tree=null;
        Object char_literal104_tree=null;
        Object char_literal106_tree=null;
        Object Id107_tree=null;
        Object char_literal108_tree=null;

        try { dbg.enterRule(getGrammarFileName(), "castExpression");
        if ( getRuleLevel()==0 ) {dbg.commence();}
        incRuleLevel();
        dbg.location(183, 1);

        try {
            if ( state.backtracking>0 && alreadyParsedRule(input, 24) ) { return retval; }
            // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:184:2: ( '(' Id ')' unaryExpression | '(' Id ')' unaryExpressionNotPlusMinus )
            int alt20=2;
            try { dbg.enterDecision(20);

            int LA20_0 = input.LA(1);

            if ( (LA20_0==84) ) {
                int LA20_1 = input.LA(2);

                if ( (synpred72_MetraScript()) ) {
                    alt20=1;
                }
                else if ( (true) ) {
                    alt20=2;
                }
                else {
                    if (state.backtracking>0) {state.failed=true; return retval;}
                    NoViableAltException nvae =
                        new NoViableAltException("", 20, 1, input);

                    dbg.recognitionException(nvae);
                    throw nvae;
                }
            }
            else {
                if (state.backtracking>0) {state.failed=true; return retval;}
                NoViableAltException nvae =
                    new NoViableAltException("", 20, 0, input);

                dbg.recognitionException(nvae);
                throw nvae;
            }
            } finally {dbg.exitDecision(20);}

            switch (alt20) {
                case 1 :
                    dbg.enterAlt(1);

                    // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:184:5: '(' Id ')' unaryExpression
                    {
                    root_0 = (Object)adaptor.nil();

                    dbg.location(184,5);
                    char_literal102=(Token)match(input,84,FOLLOW_84_in_castExpression1062); if (state.failed) return retval;
                    if ( state.backtracking==0 ) {
                    char_literal102_tree = (Object)adaptor.create(char_literal102);
                    adaptor.addChild(root_0, char_literal102_tree);
                    }
                    dbg.location(184,9);
                    Id103=(Token)match(input,Id,FOLLOW_Id_in_castExpression1064); if (state.failed) return retval;
                    if ( state.backtracking==0 ) {
                    Id103_tree = (Object)adaptor.create(Id103);
                    adaptor.addChild(root_0, Id103_tree);
                    }
                    dbg.location(184,12);
                    char_literal104=(Token)match(input,85,FOLLOW_85_in_castExpression1066); if (state.failed) return retval;
                    if ( state.backtracking==0 ) {
                    char_literal104_tree = (Object)adaptor.create(char_literal104);
                    adaptor.addChild(root_0, char_literal104_tree);
                    }
                    dbg.location(184,16);
                    pushFollow(FOLLOW_unaryExpression_in_castExpression1068);
                    unaryExpression105=unaryExpression();

                    state._fsp--;
                    if (state.failed) return retval;
                    if ( state.backtracking==0 ) adaptor.addChild(root_0, unaryExpression105.getTree());

                    }
                    break;
                case 2 :
                    dbg.enterAlt(2);

                    // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:185:5: '(' Id ')' unaryExpressionNotPlusMinus
                    {
                    root_0 = (Object)adaptor.nil();

                    dbg.location(185,5);
                    char_literal106=(Token)match(input,84,FOLLOW_84_in_castExpression1074); if (state.failed) return retval;
                    if ( state.backtracking==0 ) {
                    char_literal106_tree = (Object)adaptor.create(char_literal106);
                    adaptor.addChild(root_0, char_literal106_tree);
                    }
                    dbg.location(185,9);
                    Id107=(Token)match(input,Id,FOLLOW_Id_in_castExpression1076); if (state.failed) return retval;
                    if ( state.backtracking==0 ) {
                    Id107_tree = (Object)adaptor.create(Id107);
                    adaptor.addChild(root_0, Id107_tree);
                    }
                    dbg.location(185,12);
                    char_literal108=(Token)match(input,85,FOLLOW_85_in_castExpression1078); if (state.failed) return retval;
                    if ( state.backtracking==0 ) {
                    char_literal108_tree = (Object)adaptor.create(char_literal108);
                    adaptor.addChild(root_0, char_literal108_tree);
                    }
                    dbg.location(185,16);
                    pushFollow(FOLLOW_unaryExpressionNotPlusMinus_in_castExpression1080);
                    unaryExpressionNotPlusMinus109=unaryExpressionNotPlusMinus();

                    state._fsp--;
                    if (state.failed) return retval;
                    if ( state.backtracking==0 ) adaptor.addChild(root_0, unaryExpressionNotPlusMinus109.getTree());

                    }
                    break;

            }
            retval.stop = input.LT(-1);

            if ( state.backtracking==0 ) {

            retval.tree = (Object)adaptor.rulePostProcessing(root_0);
            adaptor.setTokenBoundaries(retval.tree, retval.start, retval.stop);
            }
        }
        catch (RecognitionException re) {
            reportError(re);
            recover(input,re);
    	retval.tree = (Object)adaptor.errorNode(input, retval.start, input.LT(-1), re);

        }
        finally {
            if ( state.backtracking>0 ) { memoize(input, 24, castExpression_StartIndex); }
        }
        dbg.location(186, 2);

        }
        finally {
            dbg.exitRule(getGrammarFileName(), "castExpression");
            decRuleLevel();
            if ( getRuleLevel()==0 ) {dbg.terminate();}
        }

        return retval;
    }
    // $ANTLR end "castExpression"

    public static class parExpression_return extends ParserRuleReturnScope {
        Object tree;
        public Object getTree() { return tree; }
    };

    // $ANTLR start "parExpression"
    // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:188:1: parExpression : '(' expression ')' -> expression ;
    public final MetraScriptParser.parExpression_return parExpression() throws RecognitionException {
        MetraScriptParser.parExpression_return retval = new MetraScriptParser.parExpression_return();
        retval.start = input.LT(1);
        int parExpression_StartIndex = input.index();
        Object root_0 = null;

        Token char_literal110=null;
        Token char_literal112=null;
        MetraScriptParser.expression_return expression111 = null;


        Object char_literal110_tree=null;
        Object char_literal112_tree=null;
        RewriteRuleTokenStream stream_84=new RewriteRuleTokenStream(adaptor,"token 84");
        RewriteRuleTokenStream stream_85=new RewriteRuleTokenStream(adaptor,"token 85");
        RewriteRuleSubtreeStream stream_expression=new RewriteRuleSubtreeStream(adaptor,"rule expression");
        try { dbg.enterRule(getGrammarFileName(), "parExpression");
        if ( getRuleLevel()==0 ) {dbg.commence();}
        incRuleLevel();
        dbg.location(188, 1);

        try {
            if ( state.backtracking>0 && alreadyParsedRule(input, 25) ) { return retval; }
            // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:189:2: ( '(' expression ')' -> expression )
            dbg.enterAlt(1);

            // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:189:4: '(' expression ')'
            {
            dbg.location(189,4);
            char_literal110=(Token)match(input,84,FOLLOW_84_in_parExpression1092); if (state.failed) return retval; 
            if ( state.backtracking==0 ) stream_84.add(char_literal110);

            dbg.location(189,8);
            pushFollow(FOLLOW_expression_in_parExpression1094);
            expression111=expression();

            state._fsp--;
            if (state.failed) return retval;
            if ( state.backtracking==0 ) stream_expression.add(expression111.getTree());
            dbg.location(189,19);
            char_literal112=(Token)match(input,85,FOLLOW_85_in_parExpression1096); if (state.failed) return retval; 
            if ( state.backtracking==0 ) stream_85.add(char_literal112);



            // AST REWRITE
            // elements: expression
            // token labels: 
            // rule labels: retval
            // token list labels: 
            // rule list labels: 
            // wildcard labels: 
            if ( state.backtracking==0 ) {
            retval.tree = root_0;
            RewriteRuleSubtreeStream stream_retval=new RewriteRuleSubtreeStream(adaptor,"rule retval",retval!=null?retval.tree:null);

            root_0 = (Object)adaptor.nil();
            // 189:23: -> expression
            {
                dbg.location(189,26);
                adaptor.addChild(root_0, stream_expression.nextTree());

            }

            retval.tree = root_0;}
            }

            retval.stop = input.LT(-1);

            if ( state.backtracking==0 ) {

            retval.tree = (Object)adaptor.rulePostProcessing(root_0);
            adaptor.setTokenBoundaries(retval.tree, retval.start, retval.stop);
            }
        }
        catch (RecognitionException re) {
            reportError(re);
            recover(input,re);
    	retval.tree = (Object)adaptor.errorNode(input, retval.start, input.LT(-1), re);

        }
        finally {
            if ( state.backtracking>0 ) { memoize(input, 25, parExpression_StartIndex); }
        }
        dbg.location(190, 2);

        }
        finally {
            dbg.exitRule(getGrammarFileName(), "parExpression");
            decRuleLevel();
            if ( getRuleLevel()==0 ) {dbg.terminate();}
        }

        return retval;
    }
    // $ANTLR end "parExpression"

    public static class unit_return extends ParserRuleReturnScope {
        Object tree;
        public Object getTree() { return tree; }
    };

    // $ANTLR start "unit"
    // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:191:1: unit : ( literal | variable );
    public final MetraScriptParser.unit_return unit() throws RecognitionException {
        MetraScriptParser.unit_return retval = new MetraScriptParser.unit_return();
        retval.start = input.LT(1);
        int unit_StartIndex = input.index();
        Object root_0 = null;

        MetraScriptParser.literal_return literal113 = null;

        MetraScriptParser.variable_return variable114 = null;



        try { dbg.enterRule(getGrammarFileName(), "unit");
        if ( getRuleLevel()==0 ) {dbg.commence();}
        incRuleLevel();
        dbg.location(191, 1);

        try {
            if ( state.backtracking>0 && alreadyParsedRule(input, 26) ) { return retval; }
            // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:192:2: ( literal | variable )
            int alt21=2;
            try { dbg.enterDecision(21);

            int LA21_0 = input.LA(1);

            if ( ((LA21_0>=FloatingPointLiteral && LA21_0<=DecimalLiteral)||(LA21_0>=38 && LA21_0<=40)) ) {
                alt21=1;
            }
            else if ( ((LA21_0>=31 && LA21_0<=32)||(LA21_0>=34 && LA21_0<=37)) ) {
                alt21=2;
            }
            else {
                if (state.backtracking>0) {state.failed=true; return retval;}
                NoViableAltException nvae =
                    new NoViableAltException("", 21, 0, input);

                dbg.recognitionException(nvae);
                throw nvae;
            }
            } finally {dbg.exitDecision(21);}

            switch (alt21) {
                case 1 :
                    dbg.enterAlt(1);

                    // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:192:4: literal
                    {
                    root_0 = (Object)adaptor.nil();

                    dbg.location(192,4);
                    pushFollow(FOLLOW_literal_in_unit1111);
                    literal113=literal();

                    state._fsp--;
                    if (state.failed) return retval;
                    if ( state.backtracking==0 ) adaptor.addChild(root_0, literal113.getTree());

                    }
                    break;
                case 2 :
                    dbg.enterAlt(2);

                    // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:193:4: variable
                    {
                    root_0 = (Object)adaptor.nil();

                    dbg.location(193,4);
                    pushFollow(FOLLOW_variable_in_unit1116);
                    variable114=variable();

                    state._fsp--;
                    if (state.failed) return retval;
                    if ( state.backtracking==0 ) adaptor.addChild(root_0, variable114.getTree());

                    }
                    break;

            }
            retval.stop = input.LT(-1);

            if ( state.backtracking==0 ) {

            retval.tree = (Object)adaptor.rulePostProcessing(root_0);
            adaptor.setTokenBoundaries(retval.tree, retval.start, retval.stop);
            }
        }
        catch (RecognitionException re) {
            reportError(re);
            recover(input,re);
    	retval.tree = (Object)adaptor.errorNode(input, retval.start, input.LT(-1), re);

        }
        finally {
            if ( state.backtracking>0 ) { memoize(input, 26, unit_StartIndex); }
        }
        dbg.location(194, 2);

        }
        finally {
            dbg.exitRule(getGrammarFileName(), "unit");
            decRuleLevel();
            if ( getRuleLevel()==0 ) {dbg.terminate();}
        }

        return retval;
    }
    // $ANTLR end "unit"

    public static class primary_return extends ParserRuleReturnScope {
        Object tree;
        public Object getTree() { return tree; }
    };

    // $ANTLR start "primary"
    // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:195:1: primary : ( parExpression | unit );
    public final MetraScriptParser.primary_return primary() throws RecognitionException {
        MetraScriptParser.primary_return retval = new MetraScriptParser.primary_return();
        retval.start = input.LT(1);
        int primary_StartIndex = input.index();
        Object root_0 = null;

        MetraScriptParser.parExpression_return parExpression115 = null;

        MetraScriptParser.unit_return unit116 = null;



        try { dbg.enterRule(getGrammarFileName(), "primary");
        if ( getRuleLevel()==0 ) {dbg.commence();}
        incRuleLevel();
        dbg.location(195, 1);

        try {
            if ( state.backtracking>0 && alreadyParsedRule(input, 27) ) { return retval; }
            // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:196:2: ( parExpression | unit )
            int alt22=2;
            try { dbg.enterDecision(22);

            int LA22_0 = input.LA(1);

            if ( (LA22_0==84) ) {
                alt22=1;
            }
            else if ( ((LA22_0>=FloatingPointLiteral && LA22_0<=DecimalLiteral)||(LA22_0>=31 && LA22_0<=32)||(LA22_0>=34 && LA22_0<=40)) ) {
                alt22=2;
            }
            else {
                if (state.backtracking>0) {state.failed=true; return retval;}
                NoViableAltException nvae =
                    new NoViableAltException("", 22, 0, input);

                dbg.recognitionException(nvae);
                throw nvae;
            }
            } finally {dbg.exitDecision(22);}

            switch (alt22) {
                case 1 :
                    dbg.enterAlt(1);

                    // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:196:4: parExpression
                    {
                    root_0 = (Object)adaptor.nil();

                    dbg.location(196,4);
                    pushFollow(FOLLOW_parExpression_in_primary1127);
                    parExpression115=parExpression();

                    state._fsp--;
                    if (state.failed) return retval;
                    if ( state.backtracking==0 ) adaptor.addChild(root_0, parExpression115.getTree());

                    }
                    break;
                case 2 :
                    dbg.enterAlt(2);

                    // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:197:4: unit
                    {
                    root_0 = (Object)adaptor.nil();

                    dbg.location(197,4);
                    pushFollow(FOLLOW_unit_in_primary1132);
                    unit116=unit();

                    state._fsp--;
                    if (state.failed) return retval;
                    if ( state.backtracking==0 ) adaptor.addChild(root_0, unit116.getTree());

                    }
                    break;

            }
            retval.stop = input.LT(-1);

            if ( state.backtracking==0 ) {

            retval.tree = (Object)adaptor.rulePostProcessing(root_0);
            adaptor.setTokenBoundaries(retval.tree, retval.start, retval.stop);
            }
        }
        catch (RecognitionException re) {
            reportError(re);
            recover(input,re);
    	retval.tree = (Object)adaptor.errorNode(input, retval.start, input.LT(-1), re);

        }
        finally {
            if ( state.backtracking>0 ) { memoize(input, 27, primary_StartIndex); }
        }
        dbg.location(198, 2);

        }
        finally {
            dbg.exitRule(getGrammarFileName(), "primary");
            decRuleLevel();
            if ( getRuleLevel()==0 ) {dbg.terminate();}
        }

        return retval;
    }
    // $ANTLR end "primary"

    // $ANTLR start synpred13_MetraScript
    public final void synpred13_MetraScript_fragment() throws RecognitionException {   
        // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:66:27: ( assignmentOperator expression )
        dbg.enterAlt(1);

        // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:66:27: assignmentOperator expression
        {
        dbg.location(66,27);
        pushFollow(FOLLOW_assignmentOperator_in_synpred13_MetraScript328);
        assignmentOperator();

        state._fsp--;
        if (state.failed) return ;
        dbg.location(66,47);
        pushFollow(FOLLOW_expression_in_synpred13_MetraScript331);
        expression();

        state._fsp--;
        if (state.failed) return ;

        }
    }
    // $ANTLR end synpred13_MetraScript

    // $ANTLR start synpred25_MetraScript
    public final void synpred25_MetraScript_fragment() throws RecognitionException {   
        // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:80:4: ( '<' '<' '=' )
        dbg.enterAlt(1);

        // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:80:5: '<' '<' '='
        {
        dbg.location(80,5);
        match(input,52,FOLLOW_52_in_synpred25_MetraScript399); if (state.failed) return ;
        dbg.location(80,9);
        match(input,52,FOLLOW_52_in_synpred25_MetraScript401); if (state.failed) return ;
        dbg.location(80,13);
        match(input,41,FOLLOW_41_in_synpred25_MetraScript403); if (state.failed) return ;

        }
    }
    // $ANTLR end synpred25_MetraScript

    // $ANTLR start synpred26_MetraScript
    public final void synpred26_MetraScript_fragment() throws RecognitionException {   
        // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:85:4: ( '>' '>' '>' '=' )
        dbg.enterAlt(1);

        // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:85:5: '>' '>' '>' '='
        {
        dbg.location(85,5);
        match(input,53,FOLLOW_53_in_synpred26_MetraScript429); if (state.failed) return ;
        dbg.location(85,9);
        match(input,53,FOLLOW_53_in_synpred26_MetraScript431); if (state.failed) return ;
        dbg.location(85,13);
        match(input,53,FOLLOW_53_in_synpred26_MetraScript433); if (state.failed) return ;
        dbg.location(85,17);
        match(input,41,FOLLOW_41_in_synpred26_MetraScript435); if (state.failed) return ;

        }
    }
    // $ANTLR end synpred26_MetraScript

    // $ANTLR start synpred27_MetraScript
    public final void synpred27_MetraScript_fragment() throws RecognitionException {   
        // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:92:4: ( '>' '>' '=' )
        dbg.enterAlt(1);

        // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:92:5: '>' '>' '='
        {
        dbg.location(92,5);
        match(input,53,FOLLOW_53_in_synpred27_MetraScript464); if (state.failed) return ;
        dbg.location(92,9);
        match(input,53,FOLLOW_53_in_synpred27_MetraScript466); if (state.failed) return ;
        dbg.location(92,13);
        match(input,41,FOLLOW_41_in_synpred27_MetraScript468); if (state.failed) return ;

        }
    }
    // $ANTLR end synpred27_MetraScript

    // $ANTLR start synpred40_MetraScript
    public final void synpred40_MetraScript_fragment() throws RecognitionException {   
        // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:125:22: ( relationalOp shiftExpression )
        dbg.enterAlt(1);

        // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:125:22: relationalOp shiftExpression
        {
        dbg.location(125,22);
        pushFollow(FOLLOW_relationalOp_in_synpred40_MetraScript683);
        relationalOp();

        state._fsp--;
        if (state.failed) return ;
        dbg.location(125,36);
        pushFollow(FOLLOW_shiftExpression_in_synpred40_MetraScript686);
        shiftExpression();

        state._fsp--;
        if (state.failed) return ;

        }
    }
    // $ANTLR end synpred40_MetraScript

    // $ANTLR start synpred41_MetraScript
    public final void synpred41_MetraScript_fragment() throws RecognitionException {   
        // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:128:4: ( '<' '=' )
        dbg.enterAlt(1);

        // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:128:5: '<' '='
        {
        dbg.location(128,5);
        match(input,52,FOLLOW_52_in_synpred41_MetraScript700); if (state.failed) return ;
        dbg.location(128,9);
        match(input,41,FOLLOW_41_in_synpred41_MetraScript702); if (state.failed) return ;

        }
    }
    // $ANTLR end synpred41_MetraScript

    // $ANTLR start synpred42_MetraScript
    public final void synpred42_MetraScript_fragment() throws RecognitionException {   
        // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:131:4: ( '>' '=' )
        dbg.enterAlt(1);

        // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:131:5: '>' '='
        {
        dbg.location(131,5);
        match(input,53,FOLLOW_53_in_synpred42_MetraScript724); if (state.failed) return ;
        dbg.location(131,9);
        match(input,41,FOLLOW_41_in_synpred42_MetraScript726); if (state.failed) return ;

        }
    }
    // $ANTLR end synpred42_MetraScript

    // $ANTLR start synpred55_MetraScript
    public final void synpred55_MetraScript_fragment() throws RecognitionException {   
        // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:151:4: ( '<' '<' )
        dbg.enterAlt(1);

        // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:151:5: '<' '<'
        {
        dbg.location(151,5);
        match(input,52,FOLLOW_52_in_synpred55_MetraScript835); if (state.failed) return ;
        dbg.location(151,9);
        match(input,52,FOLLOW_52_in_synpred55_MetraScript837); if (state.failed) return ;

        }
    }
    // $ANTLR end synpred55_MetraScript

    // $ANTLR start synpred56_MetraScript
    public final void synpred56_MetraScript_fragment() throws RecognitionException {   
        // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:154:4: ( '>' '>' '>' )
        dbg.enterAlt(1);

        // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:154:5: '>' '>' '>'
        {
        dbg.location(154,5);
        match(input,53,FOLLOW_53_in_synpred56_MetraScript859); if (state.failed) return ;
        dbg.location(154,9);
        match(input,53,FOLLOW_53_in_synpred56_MetraScript861); if (state.failed) return ;
        dbg.location(154,13);
        match(input,53,FOLLOW_53_in_synpred56_MetraScript863); if (state.failed) return ;

        }
    }
    // $ANTLR end synpred56_MetraScript

    // $ANTLR start synpred57_MetraScript
    public final void synpred57_MetraScript_fragment() throws RecognitionException {   
        // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:159:4: ( '>' '>' )
        dbg.enterAlt(1);

        // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:159:5: '>' '>'
        {
        dbg.location(159,5);
        match(input,53,FOLLOW_53_in_synpred57_MetraScript889); if (state.failed) return ;
        dbg.location(159,9);
        match(input,53,FOLLOW_53_in_synpred57_MetraScript891); if (state.failed) return ;

        }
    }
    // $ANTLR end synpred57_MetraScript

    // $ANTLR start synpred69_MetraScript
    public final void synpred69_MetraScript_fragment() throws RecognitionException {   
        // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:179:4: ( castExpression )
        dbg.enterAlt(1);

        // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:179:4: castExpression
        {
        dbg.location(179,4);
        pushFollow(FOLLOW_castExpression_in_synpred69_MetraScript1032);
        castExpression();

        state._fsp--;
        if (state.failed) return ;

        }
    }
    // $ANTLR end synpred69_MetraScript

    // $ANTLR start synpred71_MetraScript
    public final void synpred71_MetraScript_fragment() throws RecognitionException {   
        // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:180:4: ( primary ( '++' | '--' ) )
        dbg.enterAlt(1);

        // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:180:4: primary ( '++' | '--' )
        {
        dbg.location(180,4);
        pushFollow(FOLLOW_primary_in_synpred71_MetraScript1037);
        primary();

        state._fsp--;
        if (state.failed) return ;
        dbg.location(180,12);
        if ( (input.LA(1)>=81 && input.LA(1)<=82) ) {
            input.consume();
            state.errorRecovery=false;state.failed=false;
        }
        else {
            if (state.backtracking>0) {state.failed=true; return ;}
            MismatchedSetException mse = new MismatchedSetException(null,input);
            dbg.recognitionException(mse);
            throw mse;
        }


        }
    }
    // $ANTLR end synpred71_MetraScript

    // $ANTLR start synpred72_MetraScript
    public final void synpred72_MetraScript_fragment() throws RecognitionException {   
        // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:184:5: ( '(' Id ')' unaryExpression )
        dbg.enterAlt(1);

        // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:184:5: '(' Id ')' unaryExpression
        {
        dbg.location(184,5);
        match(input,84,FOLLOW_84_in_synpred72_MetraScript1062); if (state.failed) return ;
        dbg.location(184,9);
        match(input,Id,FOLLOW_Id_in_synpred72_MetraScript1064); if (state.failed) return ;
        dbg.location(184,12);
        match(input,85,FOLLOW_85_in_synpred72_MetraScript1066); if (state.failed) return ;
        dbg.location(184,16);
        pushFollow(FOLLOW_unaryExpression_in_synpred72_MetraScript1068);
        unaryExpression();

        state._fsp--;
        if (state.failed) return ;

        }
    }
    // $ANTLR end synpred72_MetraScript

    // Delegated rules

    public final boolean synpred57_MetraScript() {
        state.backtracking++;
        dbg.beginBacktrack(state.backtracking);
        int start = input.mark();
        try {
            synpred57_MetraScript_fragment(); // can never throw exception
        } catch (RecognitionException re) {
            System.err.println("impossible: "+re);
        }
        boolean success = !state.failed;
        input.rewind(start);
        dbg.endBacktrack(state.backtracking, success);
        state.backtracking--;
        state.failed=false;
        return success;
    }
    public final boolean synpred56_MetraScript() {
        state.backtracking++;
        dbg.beginBacktrack(state.backtracking);
        int start = input.mark();
        try {
            synpred56_MetraScript_fragment(); // can never throw exception
        } catch (RecognitionException re) {
            System.err.println("impossible: "+re);
        }
        boolean success = !state.failed;
        input.rewind(start);
        dbg.endBacktrack(state.backtracking, success);
        state.backtracking--;
        state.failed=false;
        return success;
    }
    public final boolean synpred26_MetraScript() {
        state.backtracking++;
        dbg.beginBacktrack(state.backtracking);
        int start = input.mark();
        try {
            synpred26_MetraScript_fragment(); // can never throw exception
        } catch (RecognitionException re) {
            System.err.println("impossible: "+re);
        }
        boolean success = !state.failed;
        input.rewind(start);
        dbg.endBacktrack(state.backtracking, success);
        state.backtracking--;
        state.failed=false;
        return success;
    }
    public final boolean synpred27_MetraScript() {
        state.backtracking++;
        dbg.beginBacktrack(state.backtracking);
        int start = input.mark();
        try {
            synpred27_MetraScript_fragment(); // can never throw exception
        } catch (RecognitionException re) {
            System.err.println("impossible: "+re);
        }
        boolean success = !state.failed;
        input.rewind(start);
        dbg.endBacktrack(state.backtracking, success);
        state.backtracking--;
        state.failed=false;
        return success;
    }
    public final boolean synpred40_MetraScript() {
        state.backtracking++;
        dbg.beginBacktrack(state.backtracking);
        int start = input.mark();
        try {
            synpred40_MetraScript_fragment(); // can never throw exception
        } catch (RecognitionException re) {
            System.err.println("impossible: "+re);
        }
        boolean success = !state.failed;
        input.rewind(start);
        dbg.endBacktrack(state.backtracking, success);
        state.backtracking--;
        state.failed=false;
        return success;
    }
    public final boolean synpred13_MetraScript() {
        state.backtracking++;
        dbg.beginBacktrack(state.backtracking);
        int start = input.mark();
        try {
            synpred13_MetraScript_fragment(); // can never throw exception
        } catch (RecognitionException re) {
            System.err.println("impossible: "+re);
        }
        boolean success = !state.failed;
        input.rewind(start);
        dbg.endBacktrack(state.backtracking, success);
        state.backtracking--;
        state.failed=false;
        return success;
    }
    public final boolean synpred42_MetraScript() {
        state.backtracking++;
        dbg.beginBacktrack(state.backtracking);
        int start = input.mark();
        try {
            synpred42_MetraScript_fragment(); // can never throw exception
        } catch (RecognitionException re) {
            System.err.println("impossible: "+re);
        }
        boolean success = !state.failed;
        input.rewind(start);
        dbg.endBacktrack(state.backtracking, success);
        state.backtracking--;
        state.failed=false;
        return success;
    }
    public final boolean synpred69_MetraScript() {
        state.backtracking++;
        dbg.beginBacktrack(state.backtracking);
        int start = input.mark();
        try {
            synpred69_MetraScript_fragment(); // can never throw exception
        } catch (RecognitionException re) {
            System.err.println("impossible: "+re);
        }
        boolean success = !state.failed;
        input.rewind(start);
        dbg.endBacktrack(state.backtracking, success);
        state.backtracking--;
        state.failed=false;
        return success;
    }
    public final boolean synpred41_MetraScript() {
        state.backtracking++;
        dbg.beginBacktrack(state.backtracking);
        int start = input.mark();
        try {
            synpred41_MetraScript_fragment(); // can never throw exception
        } catch (RecognitionException re) {
            System.err.println("impossible: "+re);
        }
        boolean success = !state.failed;
        input.rewind(start);
        dbg.endBacktrack(state.backtracking, success);
        state.backtracking--;
        state.failed=false;
        return success;
    }
    public final boolean synpred55_MetraScript() {
        state.backtracking++;
        dbg.beginBacktrack(state.backtracking);
        int start = input.mark();
        try {
            synpred55_MetraScript_fragment(); // can never throw exception
        } catch (RecognitionException re) {
            System.err.println("impossible: "+re);
        }
        boolean success = !state.failed;
        input.rewind(start);
        dbg.endBacktrack(state.backtracking, success);
        state.backtracking--;
        state.failed=false;
        return success;
    }
    public final boolean synpred25_MetraScript() {
        state.backtracking++;
        dbg.beginBacktrack(state.backtracking);
        int start = input.mark();
        try {
            synpred25_MetraScript_fragment(); // can never throw exception
        } catch (RecognitionException re) {
            System.err.println("impossible: "+re);
        }
        boolean success = !state.failed;
        input.rewind(start);
        dbg.endBacktrack(state.backtracking, success);
        state.backtracking--;
        state.failed=false;
        return success;
    }
    public final boolean synpred71_MetraScript() {
        state.backtracking++;
        dbg.beginBacktrack(state.backtracking);
        int start = input.mark();
        try {
            synpred71_MetraScript_fragment(); // can never throw exception
        } catch (RecognitionException re) {
            System.err.println("impossible: "+re);
        }
        boolean success = !state.failed;
        input.rewind(start);
        dbg.endBacktrack(state.backtracking, success);
        state.backtracking--;
        state.failed=false;
        return success;
    }
    public final boolean synpred72_MetraScript() {
        state.backtracking++;
        dbg.beginBacktrack(state.backtracking);
        int start = input.mark();
        try {
            synpred72_MetraScript_fragment(); // can never throw exception
        } catch (RecognitionException re) {
            System.err.println("impossible: "+re);
        }
        boolean success = !state.failed;
        input.rewind(start);
        dbg.endBacktrack(state.backtracking, success);
        state.backtracking--;
        state.failed=false;
        return success;
    }


    protected DFA3 dfa3 = new DFA3(this);
    protected DFA4 dfa4 = new DFA4(this);
    protected DFA12 dfa12 = new DFA12(this);
    protected DFA13 dfa13 = new DFA13(this);
    protected DFA15 dfa15 = new DFA15(this);
    protected DFA19 dfa19 = new DFA19(this);
    static final String DFA3_eotS =
        "\20\uffff";
    static final String DFA3_eofS =
        "\1\16\17\uffff";
    static final String DFA3_minS =
        "\1\41\15\0\2\uffff";
    static final String DFA3_maxS =
        "\1\125\15\0\2\uffff";
    static final String DFA3_acceptS =
        "\16\uffff\1\2\1\1";
    static final String DFA3_specialS =
        "\1\uffff\1\7\1\12\1\0\1\5\1\10\1\13\1\2\1\6\1\11\1\14\1\3\1\1\1"+
        "\4\2\uffff}>";
    static final String[] DFA3_transitionS = {
            "\1\16\7\uffff\1\1\1\2\1\3\1\4\1\5\1\6\1\7\1\10\1\11\1\12\1"+
            "\13\1\14\1\15\1\uffff\1\16\35\uffff\1\16",
            "\1\uffff",
            "\1\uffff",
            "\1\uffff",
            "\1\uffff",
            "\1\uffff",
            "\1\uffff",
            "\1\uffff",
            "\1\uffff",
            "\1\uffff",
            "\1\uffff",
            "\1\uffff",
            "\1\uffff",
            "\1\uffff",
            "",
            ""
    };

    static final short[] DFA3_eot = DFA.unpackEncodedString(DFA3_eotS);
    static final short[] DFA3_eof = DFA.unpackEncodedString(DFA3_eofS);
    static final char[] DFA3_min = DFA.unpackEncodedStringToUnsignedChars(DFA3_minS);
    static final char[] DFA3_max = DFA.unpackEncodedStringToUnsignedChars(DFA3_maxS);
    static final short[] DFA3_accept = DFA.unpackEncodedString(DFA3_acceptS);
    static final short[] DFA3_special = DFA.unpackEncodedString(DFA3_specialS);
    static final short[][] DFA3_transition;

    static {
        int numStates = DFA3_transitionS.length;
        DFA3_transition = new short[numStates][];
        for (int i=0; i<numStates; i++) {
            DFA3_transition[i] = DFA.unpackEncodedString(DFA3_transitionS[i]);
        }
    }

    class DFA3 extends DFA {

        public DFA3(BaseRecognizer recognizer) {
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
        public String getDescription() {
            return "66:26: ( assignmentOperator expression )?";
        }
        public void error(NoViableAltException nvae) {
            dbg.recognitionException(nvae);
        }
        public int specialStateTransition(int s, IntStream _input) throws NoViableAltException {
            TokenStream input = (TokenStream)_input;
        	int _s = s;
            switch ( s ) {
                    case 0 : 
                        int LA3_3 = input.LA(1);

                         
                        int index3_3 = input.index();
                        input.rewind();
                        s = -1;
                        if ( (synpred13_MetraScript()) ) {s = 15;}

                        else if ( (true) ) {s = 14;}

                         
                        input.seek(index3_3);
                        if ( s>=0 ) return s;
                        break;
                    case 1 : 
                        int LA3_12 = input.LA(1);

                         
                        int index3_12 = input.index();
                        input.rewind();
                        s = -1;
                        if ( (synpred13_MetraScript()) ) {s = 15;}

                        else if ( (true) ) {s = 14;}

                         
                        input.seek(index3_12);
                        if ( s>=0 ) return s;
                        break;
                    case 2 : 
                        int LA3_7 = input.LA(1);

                         
                        int index3_7 = input.index();
                        input.rewind();
                        s = -1;
                        if ( (synpred13_MetraScript()) ) {s = 15;}

                        else if ( (true) ) {s = 14;}

                         
                        input.seek(index3_7);
                        if ( s>=0 ) return s;
                        break;
                    case 3 : 
                        int LA3_11 = input.LA(1);

                         
                        int index3_11 = input.index();
                        input.rewind();
                        s = -1;
                        if ( (synpred13_MetraScript()) ) {s = 15;}

                        else if ( (true) ) {s = 14;}

                         
                        input.seek(index3_11);
                        if ( s>=0 ) return s;
                        break;
                    case 4 : 
                        int LA3_13 = input.LA(1);

                         
                        int index3_13 = input.index();
                        input.rewind();
                        s = -1;
                        if ( (synpred13_MetraScript()) ) {s = 15;}

                        else if ( (true) ) {s = 14;}

                         
                        input.seek(index3_13);
                        if ( s>=0 ) return s;
                        break;
                    case 5 : 
                        int LA3_4 = input.LA(1);

                         
                        int index3_4 = input.index();
                        input.rewind();
                        s = -1;
                        if ( (synpred13_MetraScript()) ) {s = 15;}

                        else if ( (true) ) {s = 14;}

                         
                        input.seek(index3_4);
                        if ( s>=0 ) return s;
                        break;
                    case 6 : 
                        int LA3_8 = input.LA(1);

                         
                        int index3_8 = input.index();
                        input.rewind();
                        s = -1;
                        if ( (synpred13_MetraScript()) ) {s = 15;}

                        else if ( (true) ) {s = 14;}

                         
                        input.seek(index3_8);
                        if ( s>=0 ) return s;
                        break;
                    case 7 : 
                        int LA3_1 = input.LA(1);

                         
                        int index3_1 = input.index();
                        input.rewind();
                        s = -1;
                        if ( (synpred13_MetraScript()) ) {s = 15;}

                        else if ( (true) ) {s = 14;}

                         
                        input.seek(index3_1);
                        if ( s>=0 ) return s;
                        break;
                    case 8 : 
                        int LA3_5 = input.LA(1);

                         
                        int index3_5 = input.index();
                        input.rewind();
                        s = -1;
                        if ( (synpred13_MetraScript()) ) {s = 15;}

                        else if ( (true) ) {s = 14;}

                         
                        input.seek(index3_5);
                        if ( s>=0 ) return s;
                        break;
                    case 9 : 
                        int LA3_9 = input.LA(1);

                         
                        int index3_9 = input.index();
                        input.rewind();
                        s = -1;
                        if ( (synpred13_MetraScript()) ) {s = 15;}

                        else if ( (true) ) {s = 14;}

                         
                        input.seek(index3_9);
                        if ( s>=0 ) return s;
                        break;
                    case 10 : 
                        int LA3_2 = input.LA(1);

                         
                        int index3_2 = input.index();
                        input.rewind();
                        s = -1;
                        if ( (synpred13_MetraScript()) ) {s = 15;}

                        else if ( (true) ) {s = 14;}

                         
                        input.seek(index3_2);
                        if ( s>=0 ) return s;
                        break;
                    case 11 : 
                        int LA3_6 = input.LA(1);

                         
                        int index3_6 = input.index();
                        input.rewind();
                        s = -1;
                        if ( (synpred13_MetraScript()) ) {s = 15;}

                        else if ( (true) ) {s = 14;}

                         
                        input.seek(index3_6);
                        if ( s>=0 ) return s;
                        break;
                    case 12 : 
                        int LA3_10 = input.LA(1);

                         
                        int index3_10 = input.index();
                        input.rewind();
                        s = -1;
                        if ( (synpred13_MetraScript()) ) {s = 15;}

                        else if ( (true) ) {s = 14;}

                         
                        input.seek(index3_10);
                        if ( s>=0 ) return s;
                        break;
            }
            if (state.backtracking>0) {state.failed=true; return -1;}
            NoViableAltException nvae =
                new NoViableAltException(getDescription(), 3, _s, input);
            error(nvae);
            throw nvae;
        }
    }
    static final String DFA4_eotS =
        "\21\uffff";
    static final String DFA4_eofS =
        "\21\uffff";
    static final String DFA4_minS =
        "\1\51\14\uffff\1\65\1\51\2\uffff";
    static final String DFA4_maxS =
        "\1\65\14\uffff\2\65\2\uffff";
    static final String DFA4_acceptS =
        "\1\uffff\1\1\1\2\1\3\1\4\1\5\1\6\1\7\1\10\1\11\1\12\1\13\1\14\2"+
        "\uffff\1\15\1\16";
    static final String DFA4_specialS =
        "\1\1\15\uffff\1\0\2\uffff}>";
    static final String[] DFA4_transitionS = {
            "\1\1\1\2\1\3\1\4\1\5\1\6\1\7\1\10\1\11\1\12\1\13\1\14\1\15",
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
            "\1\16",
            "\1\20\13\uffff\1\17",
            "",
            ""
    };

    static final short[] DFA4_eot = DFA.unpackEncodedString(DFA4_eotS);
    static final short[] DFA4_eof = DFA.unpackEncodedString(DFA4_eofS);
    static final char[] DFA4_min = DFA.unpackEncodedStringToUnsignedChars(DFA4_minS);
    static final char[] DFA4_max = DFA.unpackEncodedStringToUnsignedChars(DFA4_maxS);
    static final short[] DFA4_accept = DFA.unpackEncodedString(DFA4_acceptS);
    static final short[] DFA4_special = DFA.unpackEncodedString(DFA4_specialS);
    static final short[][] DFA4_transition;

    static {
        int numStates = DFA4_transitionS.length;
        DFA4_transition = new short[numStates][];
        for (int i=0; i<numStates; i++) {
            DFA4_transition[i] = DFA.unpackEncodedString(DFA4_transitionS[i]);
        }
    }

    class DFA4 extends DFA {

        public DFA4(BaseRecognizer recognizer) {
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
        public String getDescription() {
            return "68:1: assignmentOperator : ( '=' | '~' | '+=' | '-=' | '*=' | '/=' | '&=' | '|=' | '^=' | '%=' | '~=' | ( '<' '<' '=' )=>t1= '<' t2= '<' t3= '=' {...}? | ( '>' '>' '>' '=' )=>t1= '>' t2= '>' t3= '>' t4= '=' {...}? | ( '>' '>' '=' )=>t1= '>' t2= '>' t3= '=' {...}?);";
        }
        public void error(NoViableAltException nvae) {
            dbg.recognitionException(nvae);
        }
        public int specialStateTransition(int s, IntStream _input) throws NoViableAltException {
            TokenStream input = (TokenStream)_input;
        	int _s = s;
            switch ( s ) {
                    case 0 : 
                        int LA4_14 = input.LA(1);

                         
                        int index4_14 = input.index();
                        input.rewind();
                        s = -1;
                        if ( (LA4_14==53) && (synpred26_MetraScript())) {s = 15;}

                        else if ( (LA4_14==41) && (synpred27_MetraScript())) {s = 16;}

                         
                        input.seek(index4_14);
                        if ( s>=0 ) return s;
                        break;
                    case 1 : 
                        int LA4_0 = input.LA(1);

                         
                        int index4_0 = input.index();
                        input.rewind();
                        s = -1;
                        if ( (LA4_0==41) ) {s = 1;}

                        else if ( (LA4_0==42) ) {s = 2;}

                        else if ( (LA4_0==43) ) {s = 3;}

                        else if ( (LA4_0==44) ) {s = 4;}

                        else if ( (LA4_0==45) ) {s = 5;}

                        else if ( (LA4_0==46) ) {s = 6;}

                        else if ( (LA4_0==47) ) {s = 7;}

                        else if ( (LA4_0==48) ) {s = 8;}

                        else if ( (LA4_0==49) ) {s = 9;}

                        else if ( (LA4_0==50) ) {s = 10;}

                        else if ( (LA4_0==51) ) {s = 11;}

                        else if ( (LA4_0==52) && (synpred25_MetraScript())) {s = 12;}

                        else if ( (LA4_0==53) ) {s = 13;}

                         
                        input.seek(index4_0);
                        if ( s>=0 ) return s;
                        break;
            }
            if (state.backtracking>0) {state.failed=true; return -1;}
            NoViableAltException nvae =
                new NoViableAltException(getDescription(), 4, _s, input);
            error(nvae);
            throw nvae;
        }
    }
    static final String DFA12_eotS =
        "\43\uffff";
    static final String DFA12_eofS =
        "\1\2\42\uffff";
    static final String DFA12_minS =
        "\1\41\1\0\21\uffff\2\0\16\uffff";
    static final String DFA12_maxS =
        "\1\125\1\0\21\uffff\2\0\16\uffff";
    static final String DFA12_acceptS =
        "\2\uffff\1\2\26\uffff\1\1\11\uffff";
    static final String DFA12_specialS =
        "\1\uffff\1\0\21\uffff\1\1\1\2\16\uffff}>";
    static final String[] DFA12_transitionS = {
            "\1\2\7\uffff\13\2\1\23\1\24\13\2\1\1\1\2\11\31\11\uffff\1\2",
            "\1\uffff",
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
            "\1\uffff",
            "\1\uffff",
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

    static final short[] DFA12_eot = DFA.unpackEncodedString(DFA12_eotS);
    static final short[] DFA12_eof = DFA.unpackEncodedString(DFA12_eofS);
    static final char[] DFA12_min = DFA.unpackEncodedStringToUnsignedChars(DFA12_minS);
    static final char[] DFA12_max = DFA.unpackEncodedStringToUnsignedChars(DFA12_maxS);
    static final short[] DFA12_accept = DFA.unpackEncodedString(DFA12_acceptS);
    static final short[] DFA12_special = DFA.unpackEncodedString(DFA12_specialS);
    static final short[][] DFA12_transition;

    static {
        int numStates = DFA12_transitionS.length;
        DFA12_transition = new short[numStates][];
        for (int i=0; i<numStates; i++) {
            DFA12_transition[i] = DFA.unpackEncodedString(DFA12_transitionS[i]);
        }
    }

    class DFA12 extends DFA {

        public DFA12(BaseRecognizer recognizer) {
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
        public String getDescription() {
            return "()* loopback of 125:20: ( relationalOp shiftExpression )*";
        }
        public void error(NoViableAltException nvae) {
            dbg.recognitionException(nvae);
        }
        public int specialStateTransition(int s, IntStream _input) throws NoViableAltException {
            TokenStream input = (TokenStream)_input;
        	int _s = s;
            switch ( s ) {
                    case 0 : 
                        int LA12_1 = input.LA(1);

                         
                        int index12_1 = input.index();
                        input.rewind();
                        s = -1;
                        if ( (synpred40_MetraScript()) ) {s = 25;}

                        else if ( (true) ) {s = 2;}

                         
                        input.seek(index12_1);
                        if ( s>=0 ) return s;
                        break;
                    case 1 : 
                        int LA12_19 = input.LA(1);

                         
                        int index12_19 = input.index();
                        input.rewind();
                        s = -1;
                        if ( (synpred40_MetraScript()) ) {s = 25;}

                        else if ( (true) ) {s = 2;}

                         
                        input.seek(index12_19);
                        if ( s>=0 ) return s;
                        break;
                    case 2 : 
                        int LA12_20 = input.LA(1);

                         
                        int index12_20 = input.index();
                        input.rewind();
                        s = -1;
                        if ( (synpred40_MetraScript()) ) {s = 25;}

                        else if ( (true) ) {s = 2;}

                         
                        input.seek(index12_20);
                        if ( s>=0 ) return s;
                        break;
            }
            if (state.backtracking>0) {state.failed=true; return -1;}
            NoViableAltException nvae =
                new NoViableAltException(getDescription(), 12, _s, input);
            error(nvae);
            throw nvae;
        }
    }
    static final String DFA13_eotS =
        "\21\uffff";
    static final String DFA13_eofS =
        "\21\uffff";
    static final String DFA13_minS =
        "\1\64\2\20\16\uffff";
    static final String DFA13_maxS =
        "\1\113\2\124\16\uffff";
    static final String DFA13_acceptS =
        "\3\uffff\1\5\1\6\1\7\1\10\1\11\1\12\1\13\1\14\1\15\1\16\1\1\1\3"+
        "\1\2\1\4";
    static final String DFA13_specialS =
        "\1\uffff\1\1\1\0\16\uffff}>";
    static final String[] DFA13_transitionS = {
            "\1\1\1\2\13\uffff\1\5\1\uffff\1\3\1\4\1\6\1\7\1\10\1\11\1\12"+
            "\1\13\1\14",
            "\5\16\12\uffff\2\16\1\uffff\7\16\1\15\1\16\41\uffff\2\16\3"+
            "\uffff\4\16",
            "\5\20\12\uffff\2\20\1\uffff\7\20\1\17\1\20\41\uffff\2\20\3"+
            "\uffff\4\20",
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

    static final short[] DFA13_eot = DFA.unpackEncodedString(DFA13_eotS);
    static final short[] DFA13_eof = DFA.unpackEncodedString(DFA13_eofS);
    static final char[] DFA13_min = DFA.unpackEncodedStringToUnsignedChars(DFA13_minS);
    static final char[] DFA13_max = DFA.unpackEncodedStringToUnsignedChars(DFA13_maxS);
    static final short[] DFA13_accept = DFA.unpackEncodedString(DFA13_acceptS);
    static final short[] DFA13_special = DFA.unpackEncodedString(DFA13_specialS);
    static final short[][] DFA13_transition;

    static {
        int numStates = DFA13_transitionS.length;
        DFA13_transition = new short[numStates][];
        for (int i=0; i<numStates; i++) {
            DFA13_transition[i] = DFA.unpackEncodedString(DFA13_transitionS[i]);
        }
    }

    class DFA13 extends DFA {

        public DFA13(BaseRecognizer recognizer) {
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
        public String getDescription() {
            return "127:1: relationalOp : ( ( '<' '=' )=>t1= '<' t2= '=' {...}? | ( '>' '=' )=>t1= '>' t2= '=' {...}? | '<' | '>' | 'gt' | 'lt' | 'eq' | 'gte' | 'lte' | 'GT' | 'LT' | 'EQ' | 'GTE' | 'LTE' );";
        }
        public void error(NoViableAltException nvae) {
            dbg.recognitionException(nvae);
        }
        public int specialStateTransition(int s, IntStream _input) throws NoViableAltException {
            TokenStream input = (TokenStream)_input;
        	int _s = s;
            switch ( s ) {
                    case 0 : 
                        int LA13_2 = input.LA(1);

                         
                        int index13_2 = input.index();
                        input.rewind();
                        s = -1;
                        if ( (LA13_2==41) && (synpred42_MetraScript())) {s = 15;}

                        else if ( ((LA13_2>=FloatingPointLiteral && LA13_2<=DecimalLiteral)||(LA13_2>=31 && LA13_2<=32)||(LA13_2>=34 && LA13_2<=40)||LA13_2==42||(LA13_2>=76 && LA13_2<=77)||(LA13_2>=81 && LA13_2<=84)) ) {s = 16;}

                         
                        input.seek(index13_2);
                        if ( s>=0 ) return s;
                        break;
                    case 1 : 
                        int LA13_1 = input.LA(1);

                         
                        int index13_1 = input.index();
                        input.rewind();
                        s = -1;
                        if ( (LA13_1==41) && (synpred41_MetraScript())) {s = 13;}

                        else if ( ((LA13_1>=FloatingPointLiteral && LA13_1<=DecimalLiteral)||(LA13_1>=31 && LA13_1<=32)||(LA13_1>=34 && LA13_1<=40)||LA13_1==42||(LA13_1>=76 && LA13_1<=77)||(LA13_1>=81 && LA13_1<=84)) ) {s = 14;}

                         
                        input.seek(index13_1);
                        if ( s>=0 ) return s;
                        break;
            }
            if (state.backtracking>0) {state.failed=true; return -1;}
            NoViableAltException nvae =
                new NoViableAltException(getDescription(), 13, _s, input);
            error(nvae);
            throw nvae;
        }
    }
    static final String DFA15_eotS =
        "\27\uffff";
    static final String DFA15_eofS =
        "\27\uffff";
    static final String DFA15_minS =
        "\1\64\1\uffff\1\65\1\20\23\uffff";
    static final String DFA15_maxS =
        "\1\65\1\uffff\1\65\1\124\23\uffff";
    static final String DFA15_acceptS =
        "\1\uffff\1\1\2\uffff\1\2\22\3";
    static final String DFA15_specialS =
        "\1\0\2\uffff\1\1\23\uffff}>";
    static final String[] DFA15_transitionS = {
            "\1\1\1\2",
            "",
            "\1\3",
            "\1\15\1\16\3\14\12\uffff\1\21\1\22\1\uffff\1\23\1\24\1\25"+
            "\1\26\1\20\2\17\1\uffff\1\11\12\uffff\1\4\26\uffff\1\5\1\6\3"+
            "\uffff\1\7\1\10\1\12\1\13",
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

    static final short[] DFA15_eot = DFA.unpackEncodedString(DFA15_eotS);
    static final short[] DFA15_eof = DFA.unpackEncodedString(DFA15_eofS);
    static final char[] DFA15_min = DFA.unpackEncodedStringToUnsignedChars(DFA15_minS);
    static final char[] DFA15_max = DFA.unpackEncodedStringToUnsignedChars(DFA15_maxS);
    static final short[] DFA15_accept = DFA.unpackEncodedString(DFA15_acceptS);
    static final short[] DFA15_special = DFA.unpackEncodedString(DFA15_specialS);
    static final short[][] DFA15_transition;

    static {
        int numStates = DFA15_transitionS.length;
        DFA15_transition = new short[numStates][];
        for (int i=0; i<numStates; i++) {
            DFA15_transition[i] = DFA.unpackEncodedString(DFA15_transitionS[i]);
        }
    }

    class DFA15 extends DFA {

        public DFA15(BaseRecognizer recognizer) {
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
        public String getDescription() {
            return "150:1: shiftOp : ( ( '<' '<' )=>t1= '<' t2= '<' {...}? | ( '>' '>' '>' )=>t1= '>' t2= '>' t3= '>' {...}? | ( '>' '>' )=>t1= '>' t2= '>' {...}?);";
        }
        public void error(NoViableAltException nvae) {
            dbg.recognitionException(nvae);
        }
        public int specialStateTransition(int s, IntStream _input) throws NoViableAltException {
            TokenStream input = (TokenStream)_input;
        	int _s = s;
            switch ( s ) {
                    case 0 : 
                        int LA15_0 = input.LA(1);

                         
                        int index15_0 = input.index();
                        input.rewind();
                        s = -1;
                        if ( (LA15_0==52) && (synpred55_MetraScript())) {s = 1;}

                        else if ( (LA15_0==53) ) {s = 2;}

                         
                        input.seek(index15_0);
                        if ( s>=0 ) return s;
                        break;
                    case 1 : 
                        int LA15_3 = input.LA(1);

                         
                        int index15_3 = input.index();
                        input.rewind();
                        s = -1;
                        if ( (LA15_3==53) && (synpred56_MetraScript())) {s = 4;}

                        else if ( (LA15_3==76) && (synpred57_MetraScript())) {s = 5;}

                        else if ( (LA15_3==77) && (synpred57_MetraScript())) {s = 6;}

                        else if ( (LA15_3==81) && (synpred57_MetraScript())) {s = 7;}

                        else if ( (LA15_3==82) && (synpred57_MetraScript())) {s = 8;}

                        else if ( (LA15_3==42) && (synpred57_MetraScript())) {s = 9;}

                        else if ( (LA15_3==83) && (synpred57_MetraScript())) {s = 10;}

                        else if ( (LA15_3==84) && (synpred57_MetraScript())) {s = 11;}

                        else if ( ((LA15_3>=HexLiteral && LA15_3<=DecimalLiteral)) && (synpred57_MetraScript())) {s = 12;}

                        else if ( (LA15_3==FloatingPointLiteral) && (synpred57_MetraScript())) {s = 13;}

                        else if ( (LA15_3==StringLiteral) && (synpred57_MetraScript())) {s = 14;}

                        else if ( ((LA15_3>=39 && LA15_3<=40)) && (synpred57_MetraScript())) {s = 15;}

                        else if ( (LA15_3==38) && (synpred57_MetraScript())) {s = 16;}

                        else if ( (LA15_3==31) && (synpred57_MetraScript())) {s = 17;}

                        else if ( (LA15_3==32) && (synpred57_MetraScript())) {s = 18;}

                        else if ( (LA15_3==34) && (synpred57_MetraScript())) {s = 19;}

                        else if ( (LA15_3==35) && (synpred57_MetraScript())) {s = 20;}

                        else if ( (LA15_3==36) && (synpred57_MetraScript())) {s = 21;}

                        else if ( (LA15_3==37) && (synpred57_MetraScript())) {s = 22;}

                         
                        input.seek(index15_3);
                        if ( s>=0 ) return s;
                        break;
            }
            if (state.backtracking>0) {state.failed=true; return -1;}
            NoViableAltException nvae =
                new NoViableAltException(getDescription(), 15, _s, input);
            error(nvae);
            throw nvae;
        }
    }
    static final String DFA19_eotS =
        "\22\uffff";
    static final String DFA19_eofS =
        "\22\uffff";
    static final String DFA19_minS =
        "\1\20\2\uffff\14\0\3\uffff";
    static final String DFA19_maxS =
        "\1\124\2\uffff\14\0\3\uffff";
    static final String DFA19_acceptS =
        "\1\uffff\1\1\1\2\14\uffff\1\3\1\4\1\5";
    static final String DFA19_specialS =
        "\3\uffff\1\0\1\1\1\2\1\3\1\4\1\5\1\6\1\7\1\10\1\11\1\12\1\13\3"+
        "\uffff}>";
    static final String[] DFA19_transitionS = {
            "\1\5\1\6\3\4\12\uffff\1\11\1\12\1\uffff\1\13\1\14\1\15\1\16"+
            "\1\10\2\7\1\uffff\1\1\50\uffff\1\2\1\3",
            "",
            "",
            "\1\uffff",
            "\1\uffff",
            "\1\uffff",
            "\1\uffff",
            "\1\uffff",
            "\1\uffff",
            "\1\uffff",
            "\1\uffff",
            "\1\uffff",
            "\1\uffff",
            "\1\uffff",
            "\1\uffff",
            "",
            "",
            ""
    };

    static final short[] DFA19_eot = DFA.unpackEncodedString(DFA19_eotS);
    static final short[] DFA19_eof = DFA.unpackEncodedString(DFA19_eofS);
    static final char[] DFA19_min = DFA.unpackEncodedStringToUnsignedChars(DFA19_minS);
    static final char[] DFA19_max = DFA.unpackEncodedStringToUnsignedChars(DFA19_maxS);
    static final short[] DFA19_accept = DFA.unpackEncodedString(DFA19_acceptS);
    static final short[] DFA19_special = DFA.unpackEncodedString(DFA19_specialS);
    static final short[][] DFA19_transition;

    static {
        int numStates = DFA19_transitionS.length;
        DFA19_transition = new short[numStates][];
        for (int i=0; i<numStates; i++) {
            DFA19_transition[i] = DFA.unpackEncodedString(DFA19_transitionS[i]);
        }
    }

    class DFA19 extends DFA {

        public DFA19(BaseRecognizer recognizer) {
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
        public String getDescription() {
            return "176:1: unaryExpressionNotPlusMinus : ( '~' unaryExpression | '!' unaryExpression | castExpression | primary ( '++' | '--' ) | primary );";
        }
        public void error(NoViableAltException nvae) {
            dbg.recognitionException(nvae);
        }
        public int specialStateTransition(int s, IntStream _input) throws NoViableAltException {
            TokenStream input = (TokenStream)_input;
        	int _s = s;
            switch ( s ) {
                    case 0 : 
                        int LA19_3 = input.LA(1);

                         
                        int index19_3 = input.index();
                        input.rewind();
                        s = -1;
                        if ( (synpred69_MetraScript()) ) {s = 15;}

                        else if ( (synpred71_MetraScript()) ) {s = 16;}

                        else if ( (true) ) {s = 17;}

                         
                        input.seek(index19_3);
                        if ( s>=0 ) return s;
                        break;
                    case 1 : 
                        int LA19_4 = input.LA(1);

                         
                        int index19_4 = input.index();
                        input.rewind();
                        s = -1;
                        if ( (synpred71_MetraScript()) ) {s = 16;}

                        else if ( (true) ) {s = 17;}

                         
                        input.seek(index19_4);
                        if ( s>=0 ) return s;
                        break;
                    case 2 : 
                        int LA19_5 = input.LA(1);

                         
                        int index19_5 = input.index();
                        input.rewind();
                        s = -1;
                        if ( (synpred71_MetraScript()) ) {s = 16;}

                        else if ( (true) ) {s = 17;}

                         
                        input.seek(index19_5);
                        if ( s>=0 ) return s;
                        break;
                    case 3 : 
                        int LA19_6 = input.LA(1);

                         
                        int index19_6 = input.index();
                        input.rewind();
                        s = -1;
                        if ( (synpred71_MetraScript()) ) {s = 16;}

                        else if ( (true) ) {s = 17;}

                         
                        input.seek(index19_6);
                        if ( s>=0 ) return s;
                        break;
                    case 4 : 
                        int LA19_7 = input.LA(1);

                         
                        int index19_7 = input.index();
                        input.rewind();
                        s = -1;
                        if ( (synpred71_MetraScript()) ) {s = 16;}

                        else if ( (true) ) {s = 17;}

                         
                        input.seek(index19_7);
                        if ( s>=0 ) return s;
                        break;
                    case 5 : 
                        int LA19_8 = input.LA(1);

                         
                        int index19_8 = input.index();
                        input.rewind();
                        s = -1;
                        if ( (synpred71_MetraScript()) ) {s = 16;}

                        else if ( (true) ) {s = 17;}

                         
                        input.seek(index19_8);
                        if ( s>=0 ) return s;
                        break;
                    case 6 : 
                        int LA19_9 = input.LA(1);

                         
                        int index19_9 = input.index();
                        input.rewind();
                        s = -1;
                        if ( (synpred71_MetraScript()) ) {s = 16;}

                        else if ( (true) ) {s = 17;}

                         
                        input.seek(index19_9);
                        if ( s>=0 ) return s;
                        break;
                    case 7 : 
                        int LA19_10 = input.LA(1);

                         
                        int index19_10 = input.index();
                        input.rewind();
                        s = -1;
                        if ( (synpred71_MetraScript()) ) {s = 16;}

                        else if ( (true) ) {s = 17;}

                         
                        input.seek(index19_10);
                        if ( s>=0 ) return s;
                        break;
                    case 8 : 
                        int LA19_11 = input.LA(1);

                         
                        int index19_11 = input.index();
                        input.rewind();
                        s = -1;
                        if ( (synpred71_MetraScript()) ) {s = 16;}

                        else if ( (true) ) {s = 17;}

                         
                        input.seek(index19_11);
                        if ( s>=0 ) return s;
                        break;
                    case 9 : 
                        int LA19_12 = input.LA(1);

                         
                        int index19_12 = input.index();
                        input.rewind();
                        s = -1;
                        if ( (synpred71_MetraScript()) ) {s = 16;}

                        else if ( (true) ) {s = 17;}

                         
                        input.seek(index19_12);
                        if ( s>=0 ) return s;
                        break;
                    case 10 : 
                        int LA19_13 = input.LA(1);

                         
                        int index19_13 = input.index();
                        input.rewind();
                        s = -1;
                        if ( (synpred71_MetraScript()) ) {s = 16;}

                        else if ( (true) ) {s = 17;}

                         
                        input.seek(index19_13);
                        if ( s>=0 ) return s;
                        break;
                    case 11 : 
                        int LA19_14 = input.LA(1);

                         
                        int index19_14 = input.index();
                        input.rewind();
                        s = -1;
                        if ( (synpred71_MetraScript()) ) {s = 16;}

                        else if ( (true) ) {s = 17;}

                         
                        input.seek(index19_14);
                        if ( s>=0 ) return s;
                        break;
            }
            if (state.backtracking>0) {state.failed=true; return -1;}
            NoViableAltException nvae =
                new NoViableAltException(getDescription(), 19, _s, input);
            error(nvae);
            throw nvae;
        }
    }
 

    public static final BitSet FOLLOW_expression_in_start97 = new BitSet(new long[]{0x0000000000000002L});
    public static final BitSet FOLLOW_31_in_variable112 = new BitSet(new long[]{0x0000000000008000L});
    public static final BitSet FOLLOW_Id_in_variable114 = new BitSet(new long[]{0x0000000000000002L});
    public static final BitSet FOLLOW_32_in_variable130 = new BitSet(new long[]{0x000005FD801F0000L,0x00000000001E3000L});
    public static final BitSet FOLLOW_expression_in_variable132 = new BitSet(new long[]{0x0000000200000000L});
    public static final BitSet FOLLOW_33_in_variable134 = new BitSet(new long[]{0x0000000000008000L});
    public static final BitSet FOLLOW_Id_in_variable136 = new BitSet(new long[]{0x0000000000000002L});
    public static final BitSet FOLLOW_34_in_variable152 = new BitSet(new long[]{0x0000000000008000L});
    public static final BitSet FOLLOW_Id_in_variable154 = new BitSet(new long[]{0x0000000000000002L});
    public static final BitSet FOLLOW_35_in_variable168 = new BitSet(new long[]{0x0000000000008000L});
    public static final BitSet FOLLOW_Id_in_variable170 = new BitSet(new long[]{0x0000000000000002L});
    public static final BitSet FOLLOW_36_in_variable184 = new BitSet(new long[]{0x0000000000008000L});
    public static final BitSet FOLLOW_Id_in_variable186 = new BitSet(new long[]{0x0000000000000002L});
    public static final BitSet FOLLOW_37_in_variable200 = new BitSet(new long[]{0x0000000000008000L});
    public static final BitSet FOLLOW_Id_in_variable202 = new BitSet(new long[]{0x0000000000000002L});
    public static final BitSet FOLLOW_integerLiteral_in_literal223 = new BitSet(new long[]{0x0000000000000002L});
    public static final BitSet FOLLOW_FloatingPointLiteral_in_literal236 = new BitSet(new long[]{0x0000000000000002L});
    public static final BitSet FOLLOW_StringLiteral_in_literal248 = new BitSet(new long[]{0x0000000000000002L});
    public static final BitSet FOLLOW_booleanLiteral_in_literal260 = new BitSet(new long[]{0x0000000000000002L});
    public static final BitSet FOLLOW_38_in_literal273 = new BitSet(new long[]{0x0000000000000002L});
    public static final BitSet FOLLOW_set_in_integerLiteral0 = new BitSet(new long[]{0x0000000000000002L});
    public static final BitSet FOLLOW_set_in_booleanLiteral0 = new BitSet(new long[]{0x0000000000000002L});
    public static final BitSet FOLLOW_conditionalExpression_in_expression325 = new BitSet(new long[]{0x003FFE0000000002L});
    public static final BitSet FOLLOW_assignmentOperator_in_expression328 = new BitSet(new long[]{0x000005FD801F0000L,0x00000000001E3000L});
    public static final BitSet FOLLOW_expression_in_expression331 = new BitSet(new long[]{0x0000000000000002L});
    public static final BitSet FOLLOW_41_in_assignmentOperator343 = new BitSet(new long[]{0x0000000000000002L});
    public static final BitSet FOLLOW_42_in_assignmentOperator348 = new BitSet(new long[]{0x0000000000000002L});
    public static final BitSet FOLLOW_43_in_assignmentOperator353 = new BitSet(new long[]{0x0000000000000002L});
    public static final BitSet FOLLOW_44_in_assignmentOperator358 = new BitSet(new long[]{0x0000000000000002L});
    public static final BitSet FOLLOW_45_in_assignmentOperator363 = new BitSet(new long[]{0x0000000000000002L});
    public static final BitSet FOLLOW_46_in_assignmentOperator368 = new BitSet(new long[]{0x0000000000000002L});
    public static final BitSet FOLLOW_47_in_assignmentOperator373 = new BitSet(new long[]{0x0000000000000002L});
    public static final BitSet FOLLOW_48_in_assignmentOperator378 = new BitSet(new long[]{0x0000000000000002L});
    public static final BitSet FOLLOW_49_in_assignmentOperator383 = new BitSet(new long[]{0x0000000000000002L});
    public static final BitSet FOLLOW_50_in_assignmentOperator388 = new BitSet(new long[]{0x0000000000000002L});
    public static final BitSet FOLLOW_51_in_assignmentOperator393 = new BitSet(new long[]{0x0000000000000002L});
    public static final BitSet FOLLOW_52_in_assignmentOperator409 = new BitSet(new long[]{0x0010000000000000L});
    public static final BitSet FOLLOW_52_in_assignmentOperator413 = new BitSet(new long[]{0x0000020000000000L});
    public static final BitSet FOLLOW_41_in_assignmentOperator417 = new BitSet(new long[]{0x0000000000000002L});
    public static final BitSet FOLLOW_53_in_assignmentOperator441 = new BitSet(new long[]{0x0020000000000000L});
    public static final BitSet FOLLOW_53_in_assignmentOperator445 = new BitSet(new long[]{0x0020000000000000L});
    public static final BitSet FOLLOW_53_in_assignmentOperator449 = new BitSet(new long[]{0x0000020000000000L});
    public static final BitSet FOLLOW_41_in_assignmentOperator453 = new BitSet(new long[]{0x0000000000000002L});
    public static final BitSet FOLLOW_53_in_assignmentOperator474 = new BitSet(new long[]{0x0020000000000000L});
    public static final BitSet FOLLOW_53_in_assignmentOperator478 = new BitSet(new long[]{0x0000020000000000L});
    public static final BitSet FOLLOW_41_in_assignmentOperator482 = new BitSet(new long[]{0x0000000000000002L});
    public static final BitSet FOLLOW_conditionalOrExpression_in_conditionalExpression497 = new BitSet(new long[]{0x0040000000000002L});
    public static final BitSet FOLLOW_54_in_conditionalExpression501 = new BitSet(new long[]{0x000005FD801F0000L,0x00000000001E3000L});
    public static final BitSet FOLLOW_expression_in_conditionalExpression504 = new BitSet(new long[]{0x0080000000000000L});
    public static final BitSet FOLLOW_55_in_conditionalExpression506 = new BitSet(new long[]{0x000005FD801F0000L,0x00000000001E3000L});
    public static final BitSet FOLLOW_expression_in_conditionalExpression509 = new BitSet(new long[]{0x0000000000000002L});
    public static final BitSet FOLLOW_conditionalAndExpression_in_conditionalOrExpression522 = new BitSet(new long[]{0x0300000000000002L});
    public static final BitSet FOLLOW_set_in_conditionalOrExpression526 = new BitSet(new long[]{0x000005FD801F0000L,0x00000000001E3000L});
    public static final BitSet FOLLOW_conditionalAndExpression_in_conditionalOrExpression533 = new BitSet(new long[]{0x0300000000000002L});
    public static final BitSet FOLLOW_inclusiveOrExpression_in_conditionalAndExpression548 = new BitSet(new long[]{0x0C00000000000002L});
    public static final BitSet FOLLOW_set_in_conditionalAndExpression552 = new BitSet(new long[]{0x000005FD801F0000L,0x00000000001E3000L});
    public static final BitSet FOLLOW_inclusiveOrExpression_in_conditionalAndExpression559 = new BitSet(new long[]{0x0C00000000000002L});
    public static final BitSet FOLLOW_exclusiveOrExpression_in_inclusiveOrExpression574 = new BitSet(new long[]{0x1000000000000002L});
    public static final BitSet FOLLOW_60_in_inclusiveOrExpression578 = new BitSet(new long[]{0x000005FD801F0000L,0x00000000001E3000L});
    public static final BitSet FOLLOW_exclusiveOrExpression_in_inclusiveOrExpression581 = new BitSet(new long[]{0x1000000000000002L});
    public static final BitSet FOLLOW_andExpression_in_exclusiveOrExpression594 = new BitSet(new long[]{0x2000000000000002L});
    public static final BitSet FOLLOW_61_in_exclusiveOrExpression598 = new BitSet(new long[]{0x000005FD801F0000L,0x00000000001E3000L});
    public static final BitSet FOLLOW_andExpression_in_exclusiveOrExpression601 = new BitSet(new long[]{0x2000000000000002L});
    public static final BitSet FOLLOW_equalityExpression_in_andExpression614 = new BitSet(new long[]{0x4000000000000002L});
    public static final BitSet FOLLOW_62_in_andExpression618 = new BitSet(new long[]{0x000005FD801F0000L,0x00000000001E3000L});
    public static final BitSet FOLLOW_equalityExpression_in_andExpression621 = new BitSet(new long[]{0x4000000000000002L});
    public static final BitSet FOLLOW_instanceOfExpression_in_equalityExpression634 = new BitSet(new long[]{0x8000000000000002L,0x0000000000000007L});
    public static final BitSet FOLLOW_set_in_equalityExpression638 = new BitSet(new long[]{0x000005FD801F0000L,0x00000000001E3000L});
    public static final BitSet FOLLOW_instanceOfExpression_in_equalityExpression655 = new BitSet(new long[]{0x8000000000000002L,0x0000000000000007L});
    public static final BitSet FOLLOW_relationalExpression_in_instanceOfExpression668 = new BitSet(new long[]{0x0000000000000002L});
    public static final BitSet FOLLOW_shiftExpression_in_relationalExpression679 = new BitSet(new long[]{0x0030000000000002L,0x0000000000000FFAL});
    public static final BitSet FOLLOW_relationalOp_in_relationalExpression683 = new BitSet(new long[]{0x000005FD801F0000L,0x00000000001E3000L});
    public static final BitSet FOLLOW_shiftExpression_in_relationalExpression686 = new BitSet(new long[]{0x0030000000000002L,0x0000000000000FFAL});
    public static final BitSet FOLLOW_52_in_relationalOp708 = new BitSet(new long[]{0x0000020000000000L});
    public static final BitSet FOLLOW_41_in_relationalOp712 = new BitSet(new long[]{0x0000000000000002L});
    public static final BitSet FOLLOW_53_in_relationalOp732 = new BitSet(new long[]{0x0000020000000000L});
    public static final BitSet FOLLOW_41_in_relationalOp736 = new BitSet(new long[]{0x0000000000000002L});
    public static final BitSet FOLLOW_52_in_relationalOp747 = new BitSet(new long[]{0x0000000000000002L});
    public static final BitSet FOLLOW_53_in_relationalOp753 = new BitSet(new long[]{0x0000000000000002L});
    public static final BitSet FOLLOW_67_in_relationalOp759 = new BitSet(new long[]{0x0000000000000002L});
    public static final BitSet FOLLOW_68_in_relationalOp764 = new BitSet(new long[]{0x0000000000000002L});
    public static final BitSet FOLLOW_65_in_relationalOp769 = new BitSet(new long[]{0x0000000000000002L});
    public static final BitSet FOLLOW_69_in_relationalOp774 = new BitSet(new long[]{0x0000000000000002L});
    public static final BitSet FOLLOW_70_in_relationalOp779 = new BitSet(new long[]{0x0000000000000002L});
    public static final BitSet FOLLOW_71_in_relationalOp784 = new BitSet(new long[]{0x0000000000000002L});
    public static final BitSet FOLLOW_72_in_relationalOp789 = new BitSet(new long[]{0x0000000000000002L});
    public static final BitSet FOLLOW_73_in_relationalOp794 = new BitSet(new long[]{0x0000000000000002L});
    public static final BitSet FOLLOW_74_in_relationalOp799 = new BitSet(new long[]{0x0000000000000002L});
    public static final BitSet FOLLOW_75_in_relationalOp804 = new BitSet(new long[]{0x0000000000000002L});
    public static final BitSet FOLLOW_additiveExpression_in_shiftExpression814 = new BitSet(new long[]{0x0030000000000002L});
    public static final BitSet FOLLOW_shiftOp_in_shiftExpression818 = new BitSet(new long[]{0x000005FD801F0000L,0x00000000001E3000L});
    public static final BitSet FOLLOW_additiveExpression_in_shiftExpression821 = new BitSet(new long[]{0x0030000000000002L});
    public static final BitSet FOLLOW_52_in_shiftOp843 = new BitSet(new long[]{0x0010000000000000L});
    public static final BitSet FOLLOW_52_in_shiftOp847 = new BitSet(new long[]{0x0000000000000002L});
    public static final BitSet FOLLOW_53_in_shiftOp869 = new BitSet(new long[]{0x0020000000000000L});
    public static final BitSet FOLLOW_53_in_shiftOp873 = new BitSet(new long[]{0x0020000000000000L});
    public static final BitSet FOLLOW_53_in_shiftOp877 = new BitSet(new long[]{0x0000000000000002L});
    public static final BitSet FOLLOW_53_in_shiftOp897 = new BitSet(new long[]{0x0020000000000000L});
    public static final BitSet FOLLOW_53_in_shiftOp901 = new BitSet(new long[]{0x0000000000000002L});
    public static final BitSet FOLLOW_multiplicativeExpression_in_additiveExpression916 = new BitSet(new long[]{0x0000000000000002L,0x0000000000003000L});
    public static final BitSet FOLLOW_set_in_additiveExpression920 = new BitSet(new long[]{0x000005FD801F0000L,0x00000000001E3000L});
    public static final BitSet FOLLOW_multiplicativeExpression_in_additiveExpression929 = new BitSet(new long[]{0x0000000000000002L,0x0000000000003000L});
    public static final BitSet FOLLOW_unaryExpression_in_multiplicativeExpression942 = new BitSet(new long[]{0x0000000000000002L,0x000000000001C000L});
    public static final BitSet FOLLOW_set_in_multiplicativeExpression946 = new BitSet(new long[]{0x000005FD801F0000L,0x00000000001E3000L});
    public static final BitSet FOLLOW_unaryExpression_in_multiplicativeExpression961 = new BitSet(new long[]{0x0000000000000002L,0x000000000001C000L});
    public static final BitSet FOLLOW_76_in_unaryExpression974 = new BitSet(new long[]{0x000005FD801F0000L,0x00000000001E3000L});
    public static final BitSet FOLLOW_unaryExpression_in_unaryExpression977 = new BitSet(new long[]{0x0000000000000002L});
    public static final BitSet FOLLOW_77_in_unaryExpression982 = new BitSet(new long[]{0x000005FD801F0000L,0x00000000001E3000L});
    public static final BitSet FOLLOW_unaryExpression_in_unaryExpression985 = new BitSet(new long[]{0x0000000000000002L});
    public static final BitSet FOLLOW_81_in_unaryExpression990 = new BitSet(new long[]{0x000005FD801F0000L,0x00000000001E3000L});
    public static final BitSet FOLLOW_unaryExpression_in_unaryExpression993 = new BitSet(new long[]{0x0000000000000002L});
    public static final BitSet FOLLOW_82_in_unaryExpression998 = new BitSet(new long[]{0x000005FD801F0000L,0x00000000001E3000L});
    public static final BitSet FOLLOW_unaryExpression_in_unaryExpression1001 = new BitSet(new long[]{0x0000000000000002L});
    public static final BitSet FOLLOW_unaryExpressionNotPlusMinus_in_unaryExpression1006 = new BitSet(new long[]{0x0000000000000002L});
    public static final BitSet FOLLOW_42_in_unaryExpressionNotPlusMinus1016 = new BitSet(new long[]{0x000005FD801F0000L,0x00000000001E3000L});
    public static final BitSet FOLLOW_unaryExpression_in_unaryExpressionNotPlusMinus1019 = new BitSet(new long[]{0x0000000000000002L});
    public static final BitSet FOLLOW_83_in_unaryExpressionNotPlusMinus1024 = new BitSet(new long[]{0x000005FD801F0000L,0x00000000001E3000L});
    public static final BitSet FOLLOW_unaryExpression_in_unaryExpressionNotPlusMinus1027 = new BitSet(new long[]{0x0000000000000002L});
    public static final BitSet FOLLOW_castExpression_in_unaryExpressionNotPlusMinus1032 = new BitSet(new long[]{0x0000000000000002L});
    public static final BitSet FOLLOW_primary_in_unaryExpressionNotPlusMinus1037 = new BitSet(new long[]{0x0000000000000000L,0x0000000000060000L});
    public static final BitSet FOLLOW_set_in_unaryExpressionNotPlusMinus1039 = new BitSet(new long[]{0x0000000000000002L});
    public static final BitSet FOLLOW_primary_in_unaryExpressionNotPlusMinus1051 = new BitSet(new long[]{0x0000000000000002L});
    public static final BitSet FOLLOW_84_in_castExpression1062 = new BitSet(new long[]{0x0000000000008000L});
    public static final BitSet FOLLOW_Id_in_castExpression1064 = new BitSet(new long[]{0x0000000000000000L,0x0000000000200000L});
    public static final BitSet FOLLOW_85_in_castExpression1066 = new BitSet(new long[]{0x000005FD801F0000L,0x00000000001E3000L});
    public static final BitSet FOLLOW_unaryExpression_in_castExpression1068 = new BitSet(new long[]{0x0000000000000002L});
    public static final BitSet FOLLOW_84_in_castExpression1074 = new BitSet(new long[]{0x0000000000008000L});
    public static final BitSet FOLLOW_Id_in_castExpression1076 = new BitSet(new long[]{0x0000000000000000L,0x0000000000200000L});
    public static final BitSet FOLLOW_85_in_castExpression1078 = new BitSet(new long[]{0x000005FD801F0000L,0x00000000001E3000L});
    public static final BitSet FOLLOW_unaryExpressionNotPlusMinus_in_castExpression1080 = new BitSet(new long[]{0x0000000000000002L});
    public static final BitSet FOLLOW_84_in_parExpression1092 = new BitSet(new long[]{0x000005FD801F0000L,0x00000000001E3000L});
    public static final BitSet FOLLOW_expression_in_parExpression1094 = new BitSet(new long[]{0x0000000000000000L,0x0000000000200000L});
    public static final BitSet FOLLOW_85_in_parExpression1096 = new BitSet(new long[]{0x0000000000000002L});
    public static final BitSet FOLLOW_literal_in_unit1111 = new BitSet(new long[]{0x0000000000000002L});
    public static final BitSet FOLLOW_variable_in_unit1116 = new BitSet(new long[]{0x0000000000000002L});
    public static final BitSet FOLLOW_parExpression_in_primary1127 = new BitSet(new long[]{0x0000000000000002L});
    public static final BitSet FOLLOW_unit_in_primary1132 = new BitSet(new long[]{0x0000000000000002L});
    public static final BitSet FOLLOW_assignmentOperator_in_synpred13_MetraScript328 = new BitSet(new long[]{0x000005FD801F0000L,0x00000000001E3000L});
    public static final BitSet FOLLOW_expression_in_synpred13_MetraScript331 = new BitSet(new long[]{0x0000000000000002L});
    public static final BitSet FOLLOW_52_in_synpred25_MetraScript399 = new BitSet(new long[]{0x0010000000000000L});
    public static final BitSet FOLLOW_52_in_synpred25_MetraScript401 = new BitSet(new long[]{0x0000020000000000L});
    public static final BitSet FOLLOW_41_in_synpred25_MetraScript403 = new BitSet(new long[]{0x0000000000000002L});
    public static final BitSet FOLLOW_53_in_synpred26_MetraScript429 = new BitSet(new long[]{0x0020000000000000L});
    public static final BitSet FOLLOW_53_in_synpred26_MetraScript431 = new BitSet(new long[]{0x0020000000000000L});
    public static final BitSet FOLLOW_53_in_synpred26_MetraScript433 = new BitSet(new long[]{0x0000020000000000L});
    public static final BitSet FOLLOW_41_in_synpred26_MetraScript435 = new BitSet(new long[]{0x0000000000000002L});
    public static final BitSet FOLLOW_53_in_synpred27_MetraScript464 = new BitSet(new long[]{0x0020000000000000L});
    public static final BitSet FOLLOW_53_in_synpred27_MetraScript466 = new BitSet(new long[]{0x0000020000000000L});
    public static final BitSet FOLLOW_41_in_synpred27_MetraScript468 = new BitSet(new long[]{0x0000000000000002L});
    public static final BitSet FOLLOW_relationalOp_in_synpred40_MetraScript683 = new BitSet(new long[]{0x000005FD801F0000L,0x00000000001E3000L});
    public static final BitSet FOLLOW_shiftExpression_in_synpred40_MetraScript686 = new BitSet(new long[]{0x0000000000000002L});
    public static final BitSet FOLLOW_52_in_synpred41_MetraScript700 = new BitSet(new long[]{0x0000020000000000L});
    public static final BitSet FOLLOW_41_in_synpred41_MetraScript702 = new BitSet(new long[]{0x0000000000000002L});
    public static final BitSet FOLLOW_53_in_synpred42_MetraScript724 = new BitSet(new long[]{0x0000020000000000L});
    public static final BitSet FOLLOW_41_in_synpred42_MetraScript726 = new BitSet(new long[]{0x0000000000000002L});
    public static final BitSet FOLLOW_52_in_synpred55_MetraScript835 = new BitSet(new long[]{0x0010000000000000L});
    public static final BitSet FOLLOW_52_in_synpred55_MetraScript837 = new BitSet(new long[]{0x0000000000000002L});
    public static final BitSet FOLLOW_53_in_synpred56_MetraScript859 = new BitSet(new long[]{0x0020000000000000L});
    public static final BitSet FOLLOW_53_in_synpred56_MetraScript861 = new BitSet(new long[]{0x0020000000000000L});
    public static final BitSet FOLLOW_53_in_synpred56_MetraScript863 = new BitSet(new long[]{0x0000000000000002L});
    public static final BitSet FOLLOW_53_in_synpred57_MetraScript889 = new BitSet(new long[]{0x0020000000000000L});
    public static final BitSet FOLLOW_53_in_synpred57_MetraScript891 = new BitSet(new long[]{0x0000000000000002L});
    public static final BitSet FOLLOW_castExpression_in_synpred69_MetraScript1032 = new BitSet(new long[]{0x0000000000000002L});
    public static final BitSet FOLLOW_primary_in_synpred71_MetraScript1037 = new BitSet(new long[]{0x0000000000000000L,0x0000000000060000L});
    public static final BitSet FOLLOW_set_in_synpred71_MetraScript1039 = new BitSet(new long[]{0x0000000000000002L});
    public static final BitSet FOLLOW_84_in_synpred72_MetraScript1062 = new BitSet(new long[]{0x0000000000008000L});
    public static final BitSet FOLLOW_Id_in_synpred72_MetraScript1064 = new BitSet(new long[]{0x0000000000000000L,0x0000000000200000L});
    public static final BitSet FOLLOW_85_in_synpred72_MetraScript1066 = new BitSet(new long[]{0x000005FD801F0000L,0x00000000001E3000L});
    public static final BitSet FOLLOW_unaryExpression_in_synpred72_MetraScript1068 = new BitSet(new long[]{0x0000000000000002L});

}