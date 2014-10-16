// $ANTLR 3.2 Sep 23, 2009 12:02:23 D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmScript.g 2010-11-22 13:28:43

import org.antlr.runtime.*;
import java.util.Stack;
import java.util.List;
import java.util.ArrayList;

public class MvmScriptLexer extends Lexer {
    public static final int Syn_If=27;
    public static final int FloatTypeSuffix=59;
    public static final int Syn_DoWhile=33;
    public static final int OctalLiteral=54;
    public static final int Ast_Element=7;
    public static final int EOF=-1;
    public static final int Syn_IfCondition=28;
    public static final int T__93=93;
    public static final int T__94=94;
    public static final int T__91=91;
    public static final int Ast_Primary=4;
    public static final int T__92=92;
    public static final int T__148=148;
    public static final int T__90=90;
    public static final int T__147=147;
    public static final int T__149=149;
    public static final int Ast_Value=14;
    public static final int Syn_Lvalue=22;
    public static final int COMMENT=61;
    public static final int T__99=99;
    public static final int T__150=150;
    public static final int T__98=98;
    public static final int T__97=97;
    public static final int T__151=151;
    public static final int Ast_Secondary=5;
    public static final int Ast_Parameters=10;
    public static final int T__96=96;
    public static final int T__152=152;
    public static final int T__95=95;
    public static final int T__153=153;
    public static final int T__139=139;
    public static final int Syn_DoWhileCondition=34;
    public static final int T__138=138;
    public static final int Syn_PostIncrement=46;
    public static final int T__137=137;
    public static final int T__136=136;
    public static final int T__80=80;
    public static final int T__81=81;
    public static final int Syn_ForeachList=42;
    public static final int T__82=82;
    public static final int T__83=83;
    public static final int LINE_COMMENT=62;
    public static final int IntegerTypeSuffix=57;
    public static final int Syn_LiteralInt=15;
    public static final int Syn_Label=43;
    public static final int Syn_IfElse=30;
    public static final int Syn_DataType=21;
    public static final int T__85=85;
    public static final int T__141=141;
    public static final int Syn_Foreach=40;
    public static final int T__84=84;
    public static final int T__142=142;
    public static final int T__87=87;
    public static final int T__86=86;
    public static final int T__140=140;
    public static final int Syn_LiteralNull=19;
    public static final int T__89=89;
    public static final int T__145=145;
    public static final int T__88=88;
    public static final int T__146=146;
    public static final int Syn_LiteralFloat=16;
    public static final int T__143=143;
    public static final int T__144=144;
    public static final int T__126=126;
    public static final int T__125=125;
    public static final int Syn_While=31;
    public static final int T__128=128;
    public static final int T__127=127;
    public static final int WS=60;
    public static final int T__71=71;
    public static final int T__72=72;
    public static final int T__129=129;
    public static final int Syn_Initializer=23;
    public static final int T__70=70;
    public static final int Ast_Brace=12;
    public static final int T__76=76;
    public static final int T__75=75;
    public static final int T__74=74;
    public static final int T__130=130;
    public static final int T__73=73;
    public static final int T__131=131;
    public static final int T__132=132;
    public static final int T__79=79;
    public static final int T__133=133;
    public static final int T__78=78;
    public static final int T__134=134;
    public static final int T__77=77;
    public static final int T__135=135;
    public static final int T__68=68;
    public static final int T__69=69;
    public static final int Ast_ElementName=8;
    public static final int T__66=66;
    public static final int T__67=67;
    public static final int T__64=64;
    public static final int T__65=65;
    public static final int Syn_ForCondition=38;
    public static final int T__63=63;
    public static final int T__118=118;
    public static final int T__119=119;
    public static final int T__116=116;
    public static final int Syn_IfThen=29;
    public static final int T__117=117;
    public static final int T__114=114;
    public static final int T__115=115;
    public static final int T__124=124;
    public static final int T__123=123;
    public static final int T__122=122;
    public static final int Exponent=58;
    public static final int T__121=121;
    public static final int T__120=120;
    public static final int Syn_ForInitialize=37;
    public static final int Syn_PreDecrement=45;
    public static final int HexDigit=56;
    public static final int Syn_WhileCondition=32;
    public static final int Syn_IsArray=24;
    public static final int Syn_ForeachItem=41;
    public static final int Syn_TypeArgs=25;
    public static final int T__107=107;
    public static final int T__108=108;
    public static final int Syn_Try=48;
    public static final int T__109=109;
    public static final int T__103=103;
    public static final int T__104=104;
    public static final int T__105=105;
    public static final int T__106=106;
    public static final int Ast_Bracket=13;
    public static final int Syn_StaticType=49;
    public static final int T__111=111;
    public static final int Syn_Block=35;
    public static final int Syn_PostDecrement=47;
    public static final int T__110=110;
    public static final int T__113=113;
    public static final int T__112=112;
    public static final int Syn_literalString=17;
    public static final int Syn_LiteralBool=18;
    public static final int Id=50;
    public static final int Ast_Dot=9;
    public static final int Ast_Children=11;
    public static final int Syn_ForStep=39;
    public static final int HexLiteral=53;
    public static final int Syn_PreIncrement=44;
    public static final int T__102=102;
    public static final int T__101=101;
    public static final int T__100=100;
    public static final int DecimalLiteral=51;
    public static final int Syn_Args=26;
    public static final int StringLiteral=52;
    public static final int Syn_For=36;
    public static final int IntegerLiteral=55;
    public static final int Ast_NodeNamer=6;
    public static final int Syn_NewClassInst=20;

    // delegates
    // delegators

    public MvmScriptLexer() {;} 
    public MvmScriptLexer(CharStream input) {
        this(input, new RecognizerSharedState());
    }
    public MvmScriptLexer(CharStream input, RecognizerSharedState state) {
        super(input,state);

    }
    public String getGrammarFileName() { return "D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmScript.g"; }

    // $ANTLR start "T__63"
    public final void mT__63() throws RecognitionException {
        try {
            int _type = T__63;
            int _channel = DEFAULT_TOKEN_CHANNEL;
            // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmScript.g:3:7: ( '=>' )
            // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmScript.g:3:9: '=>'
            {
            match("=>"); 


            }

            state.type = _type;
            state.channel = _channel;
        }
        finally {
        }
    }
    // $ANTLR end "T__63"

    // $ANTLR start "T__64"
    public final void mT__64() throws RecognitionException {
        try {
            int _type = T__64;
            int _channel = DEFAULT_TOKEN_CHANNEL;
            // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmScript.g:4:7: ( '{' )
            // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmScript.g:4:9: '{'
            {
            match('{'); 

            }

            state.type = _type;
            state.channel = _channel;
        }
        finally {
        }
    }
    // $ANTLR end "T__64"

    // $ANTLR start "T__65"
    public final void mT__65() throws RecognitionException {
        try {
            int _type = T__65;
            int _channel = DEFAULT_TOKEN_CHANNEL;
            // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmScript.g:5:7: ( '}' )
            // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmScript.g:5:9: '}'
            {
            match('}'); 

            }

            state.type = _type;
            state.channel = _channel;
        }
        finally {
        }
    }
    // $ANTLR end "T__65"

    // $ANTLR start "T__66"
    public final void mT__66() throws RecognitionException {
        try {
            int _type = T__66;
            int _channel = DEFAULT_TOKEN_CHANNEL;
            // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmScript.g:6:7: ( '=' )
            // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmScript.g:6:9: '='
            {
            match('='); 

            }

            state.type = _type;
            state.channel = _channel;
        }
        finally {
        }
    }
    // $ANTLR end "T__66"

    // $ANTLR start "T__67"
    public final void mT__67() throws RecognitionException {
        try {
            int _type = T__67;
            int _channel = DEFAULT_TOKEN_CHANNEL;
            // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmScript.g:7:7: ( '+=' )
            // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmScript.g:7:9: '+='
            {
            match("+="); 


            }

            state.type = _type;
            state.channel = _channel;
        }
        finally {
        }
    }
    // $ANTLR end "T__67"

    // $ANTLR start "T__68"
    public final void mT__68() throws RecognitionException {
        try {
            int _type = T__68;
            int _channel = DEFAULT_TOKEN_CHANNEL;
            // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmScript.g:8:7: ( '-=' )
            // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmScript.g:8:9: '-='
            {
            match("-="); 


            }

            state.type = _type;
            state.channel = _channel;
        }
        finally {
        }
    }
    // $ANTLR end "T__68"

    // $ANTLR start "T__69"
    public final void mT__69() throws RecognitionException {
        try {
            int _type = T__69;
            int _channel = DEFAULT_TOKEN_CHANNEL;
            // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmScript.g:9:7: ( '*=' )
            // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmScript.g:9:9: '*='
            {
            match("*="); 


            }

            state.type = _type;
            state.channel = _channel;
        }
        finally {
        }
    }
    // $ANTLR end "T__69"

    // $ANTLR start "T__70"
    public final void mT__70() throws RecognitionException {
        try {
            int _type = T__70;
            int _channel = DEFAULT_TOKEN_CHANNEL;
            // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmScript.g:10:7: ( '/=' )
            // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmScript.g:10:9: '/='
            {
            match("/="); 


            }

            state.type = _type;
            state.channel = _channel;
        }
        finally {
        }
    }
    // $ANTLR end "T__70"

    // $ANTLR start "T__71"
    public final void mT__71() throws RecognitionException {
        try {
            int _type = T__71;
            int _channel = DEFAULT_TOKEN_CHANNEL;
            // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmScript.g:11:7: ( '&=' )
            // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmScript.g:11:9: '&='
            {
            match("&="); 


            }

            state.type = _type;
            state.channel = _channel;
        }
        finally {
        }
    }
    // $ANTLR end "T__71"

    // $ANTLR start "T__72"
    public final void mT__72() throws RecognitionException {
        try {
            int _type = T__72;
            int _channel = DEFAULT_TOKEN_CHANNEL;
            // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmScript.g:12:7: ( '|=' )
            // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmScript.g:12:9: '|='
            {
            match("|="); 


            }

            state.type = _type;
            state.channel = _channel;
        }
        finally {
        }
    }
    // $ANTLR end "T__72"

    // $ANTLR start "T__73"
    public final void mT__73() throws RecognitionException {
        try {
            int _type = T__73;
            int _channel = DEFAULT_TOKEN_CHANNEL;
            // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmScript.g:13:7: ( '^=' )
            // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmScript.g:13:9: '^='
            {
            match("^="); 


            }

            state.type = _type;
            state.channel = _channel;
        }
        finally {
        }
    }
    // $ANTLR end "T__73"

    // $ANTLR start "T__74"
    public final void mT__74() throws RecognitionException {
        try {
            int _type = T__74;
            int _channel = DEFAULT_TOKEN_CHANNEL;
            // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmScript.g:14:7: ( '%=' )
            // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmScript.g:14:9: '%='
            {
            match("%="); 


            }

            state.type = _type;
            state.channel = _channel;
        }
        finally {
        }
    }
    // $ANTLR end "T__74"

    // $ANTLR start "T__75"
    public final void mT__75() throws RecognitionException {
        try {
            int _type = T__75;
            int _channel = DEFAULT_TOKEN_CHANNEL;
            // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmScript.g:15:7: ( '~=' )
            // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmScript.g:15:9: '~='
            {
            match("~="); 


            }

            state.type = _type;
            state.channel = _channel;
        }
        finally {
        }
    }
    // $ANTLR end "T__75"

    // $ANTLR start "T__76"
    public final void mT__76() throws RecognitionException {
        try {
            int _type = T__76;
            int _channel = DEFAULT_TOKEN_CHANNEL;
            // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmScript.g:16:7: ( '<<=' )
            // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmScript.g:16:9: '<<='
            {
            match("<<="); 


            }

            state.type = _type;
            state.channel = _channel;
        }
        finally {
        }
    }
    // $ANTLR end "T__76"

    // $ANTLR start "T__77"
    public final void mT__77() throws RecognitionException {
        try {
            int _type = T__77;
            int _channel = DEFAULT_TOKEN_CHANNEL;
            // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmScript.g:17:7: ( '>>=' )
            // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmScript.g:17:9: '>>='
            {
            match(">>="); 


            }

            state.type = _type;
            state.channel = _channel;
        }
        finally {
        }
    }
    // $ANTLR end "T__77"

    // $ANTLR start "T__78"
    public final void mT__78() throws RecognitionException {
        try {
            int _type = T__78;
            int _channel = DEFAULT_TOKEN_CHANNEL;
            // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmScript.g:18:7: ( '?' )
            // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmScript.g:18:9: '?'
            {
            match('?'); 

            }

            state.type = _type;
            state.channel = _channel;
        }
        finally {
        }
    }
    // $ANTLR end "T__78"

    // $ANTLR start "T__79"
    public final void mT__79() throws RecognitionException {
        try {
            int _type = T__79;
            int _channel = DEFAULT_TOKEN_CHANNEL;
            // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmScript.g:19:7: ( ':' )
            // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmScript.g:19:9: ':'
            {
            match(':'); 

            }

            state.type = _type;
            state.channel = _channel;
        }
        finally {
        }
    }
    // $ANTLR end "T__79"

    // $ANTLR start "T__80"
    public final void mT__80() throws RecognitionException {
        try {
            int _type = T__80;
            int _channel = DEFAULT_TOKEN_CHANNEL;
            // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmScript.g:20:7: ( '||' )
            // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmScript.g:20:9: '||'
            {
            match("||"); 


            }

            state.type = _type;
            state.channel = _channel;
        }
        finally {
        }
    }
    // $ANTLR end "T__80"

    // $ANTLR start "T__81"
    public final void mT__81() throws RecognitionException {
        try {
            int _type = T__81;
            int _channel = DEFAULT_TOKEN_CHANNEL;
            // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmScript.g:21:7: ( 'or' )
            // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmScript.g:21:9: 'or'
            {
            match("or"); 


            }

            state.type = _type;
            state.channel = _channel;
        }
        finally {
        }
    }
    // $ANTLR end "T__81"

    // $ANTLR start "T__82"
    public final void mT__82() throws RecognitionException {
        try {
            int _type = T__82;
            int _channel = DEFAULT_TOKEN_CHANNEL;
            // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmScript.g:22:7: ( 'OR' )
            // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmScript.g:22:9: 'OR'
            {
            match("OR"); 


            }

            state.type = _type;
            state.channel = _channel;
        }
        finally {
        }
    }
    // $ANTLR end "T__82"

    // $ANTLR start "T__83"
    public final void mT__83() throws RecognitionException {
        try {
            int _type = T__83;
            int _channel = DEFAULT_TOKEN_CHANNEL;
            // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmScript.g:23:7: ( '&&' )
            // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmScript.g:23:9: '&&'
            {
            match("&&"); 


            }

            state.type = _type;
            state.channel = _channel;
        }
        finally {
        }
    }
    // $ANTLR end "T__83"

    // $ANTLR start "T__84"
    public final void mT__84() throws RecognitionException {
        try {
            int _type = T__84;
            int _channel = DEFAULT_TOKEN_CHANNEL;
            // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmScript.g:24:7: ( 'and' )
            // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmScript.g:24:9: 'and'
            {
            match("and"); 


            }

            state.type = _type;
            state.channel = _channel;
        }
        finally {
        }
    }
    // $ANTLR end "T__84"

    // $ANTLR start "T__85"
    public final void mT__85() throws RecognitionException {
        try {
            int _type = T__85;
            int _channel = DEFAULT_TOKEN_CHANNEL;
            // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmScript.g:25:7: ( 'AND' )
            // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmScript.g:25:9: 'AND'
            {
            match("AND"); 


            }

            state.type = _type;
            state.channel = _channel;
        }
        finally {
        }
    }
    // $ANTLR end "T__85"

    // $ANTLR start "T__86"
    public final void mT__86() throws RecognitionException {
        try {
            int _type = T__86;
            int _channel = DEFAULT_TOKEN_CHANNEL;
            // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmScript.g:26:7: ( '|' )
            // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmScript.g:26:9: '|'
            {
            match('|'); 

            }

            state.type = _type;
            state.channel = _channel;
        }
        finally {
        }
    }
    // $ANTLR end "T__86"

    // $ANTLR start "T__87"
    public final void mT__87() throws RecognitionException {
        try {
            int _type = T__87;
            int _channel = DEFAULT_TOKEN_CHANNEL;
            // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmScript.g:27:7: ( '^' )
            // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmScript.g:27:9: '^'
            {
            match('^'); 

            }

            state.type = _type;
            state.channel = _channel;
        }
        finally {
        }
    }
    // $ANTLR end "T__87"

    // $ANTLR start "T__88"
    public final void mT__88() throws RecognitionException {
        try {
            int _type = T__88;
            int _channel = DEFAULT_TOKEN_CHANNEL;
            // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmScript.g:28:7: ( '&' )
            // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmScript.g:28:9: '&'
            {
            match('&'); 

            }

            state.type = _type;
            state.channel = _channel;
        }
        finally {
        }
    }
    // $ANTLR end "T__88"

    // $ANTLR start "T__89"
    public final void mT__89() throws RecognitionException {
        try {
            int _type = T__89;
            int _channel = DEFAULT_TOKEN_CHANNEL;
            // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmScript.g:29:7: ( '==' )
            // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmScript.g:29:9: '=='
            {
            match("=="); 


            }

            state.type = _type;
            state.channel = _channel;
        }
        finally {
        }
    }
    // $ANTLR end "T__89"

    // $ANTLR start "T__90"
    public final void mT__90() throws RecognitionException {
        try {
            int _type = T__90;
            int _channel = DEFAULT_TOKEN_CHANNEL;
            // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmScript.g:30:7: ( '!=' )
            // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmScript.g:30:9: '!='
            {
            match("!="); 


            }

            state.type = _type;
            state.channel = _channel;
        }
        finally {
        }
    }
    // $ANTLR end "T__90"

    // $ANTLR start "T__91"
    public final void mT__91() throws RecognitionException {
        try {
            int _type = T__91;
            int _channel = DEFAULT_TOKEN_CHANNEL;
            // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmScript.g:31:7: ( 'eq' )
            // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmScript.g:31:9: 'eq'
            {
            match("eq"); 


            }

            state.type = _type;
            state.channel = _channel;
        }
        finally {
        }
    }
    // $ANTLR end "T__91"

    // $ANTLR start "T__92"
    public final void mT__92() throws RecognitionException {
        try {
            int _type = T__92;
            int _channel = DEFAULT_TOKEN_CHANNEL;
            // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmScript.g:32:7: ( 'ne' )
            // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmScript.g:32:9: 'ne'
            {
            match("ne"); 


            }

            state.type = _type;
            state.channel = _channel;
        }
        finally {
        }
    }
    // $ANTLR end "T__92"

    // $ANTLR start "T__93"
    public final void mT__93() throws RecognitionException {
        try {
            int _type = T__93;
            int _channel = DEFAULT_TOKEN_CHANNEL;
            // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmScript.g:33:7: ( 'Eq' )
            // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmScript.g:33:9: 'Eq'
            {
            match("Eq"); 


            }

            state.type = _type;
            state.channel = _channel;
        }
        finally {
        }
    }
    // $ANTLR end "T__93"

    // $ANTLR start "T__94"
    public final void mT__94() throws RecognitionException {
        try {
            int _type = T__94;
            int _channel = DEFAULT_TOKEN_CHANNEL;
            // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmScript.g:34:7: ( 'Ne' )
            // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmScript.g:34:9: 'Ne'
            {
            match("Ne"); 


            }

            state.type = _type;
            state.channel = _channel;
        }
        finally {
        }
    }
    // $ANTLR end "T__94"

    // $ANTLR start "T__95"
    public final void mT__95() throws RecognitionException {
        try {
            int _type = T__95;
            int _channel = DEFAULT_TOKEN_CHANNEL;
            // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmScript.g:35:7: ( 'EQ' )
            // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmScript.g:35:9: 'EQ'
            {
            match("EQ"); 


            }

            state.type = _type;
            state.channel = _channel;
        }
        finally {
        }
    }
    // $ANTLR end "T__95"

    // $ANTLR start "T__96"
    public final void mT__96() throws RecognitionException {
        try {
            int _type = T__96;
            int _channel = DEFAULT_TOKEN_CHANNEL;
            // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmScript.g:36:7: ( 'NE' )
            // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmScript.g:36:9: 'NE'
            {
            match("NE"); 


            }

            state.type = _type;
            state.channel = _channel;
        }
        finally {
        }
    }
    // $ANTLR end "T__96"

    // $ANTLR start "T__97"
    public final void mT__97() throws RecognitionException {
        try {
            int _type = T__97;
            int _channel = DEFAULT_TOKEN_CHANNEL;
            // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmScript.g:37:7: ( 'eqEQ' )
            // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmScript.g:37:9: 'eqEQ'
            {
            match("eqEQ"); 


            }

            state.type = _type;
            state.channel = _channel;
        }
        finally {
        }
    }
    // $ANTLR end "T__97"

    // $ANTLR start "T__98"
    public final void mT__98() throws RecognitionException {
        try {
            int _type = T__98;
            int _channel = DEFAULT_TOKEN_CHANNEL;
            // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmScript.g:38:7: ( 'EqEQ' )
            // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmScript.g:38:9: 'EqEQ'
            {
            match("EqEQ"); 


            }

            state.type = _type;
            state.channel = _channel;
        }
        finally {
        }
    }
    // $ANTLR end "T__98"

    // $ANTLR start "T__99"
    public final void mT__99() throws RecognitionException {
        try {
            int _type = T__99;
            int _channel = DEFAULT_TOKEN_CHANNEL;
            // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmScript.g:39:7: ( 'neNE' )
            // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmScript.g:39:9: 'neNE'
            {
            match("neNE"); 


            }

            state.type = _type;
            state.channel = _channel;
        }
        finally {
        }
    }
    // $ANTLR end "T__99"

    // $ANTLR start "T__100"
    public final void mT__100() throws RecognitionException {
        try {
            int _type = T__100;
            int _channel = DEFAULT_TOKEN_CHANNEL;
            // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmScript.g:40:8: ( 'NeNE' )
            // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmScript.g:40:10: 'NeNE'
            {
            match("NeNE"); 


            }

            state.type = _type;
            state.channel = _channel;
        }
        finally {
        }
    }
    // $ANTLR end "T__100"

    // $ANTLR start "T__101"
    public final void mT__101() throws RecognitionException {
        try {
            int _type = T__101;
            int _channel = DEFAULT_TOKEN_CHANNEL;
            // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmScript.g:41:8: ( '<=' )
            // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmScript.g:41:10: '<='
            {
            match("<="); 


            }

            state.type = _type;
            state.channel = _channel;
        }
        finally {
        }
    }
    // $ANTLR end "T__101"

    // $ANTLR start "T__102"
    public final void mT__102() throws RecognitionException {
        try {
            int _type = T__102;
            int _channel = DEFAULT_TOKEN_CHANNEL;
            // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmScript.g:42:8: ( '>=' )
            // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmScript.g:42:10: '>='
            {
            match(">="); 


            }

            state.type = _type;
            state.channel = _channel;
        }
        finally {
        }
    }
    // $ANTLR end "T__102"

    // $ANTLR start "T__103"
    public final void mT__103() throws RecognitionException {
        try {
            int _type = T__103;
            int _channel = DEFAULT_TOKEN_CHANNEL;
            // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmScript.g:43:8: ( '<' )
            // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmScript.g:43:10: '<'
            {
            match('<'); 

            }

            state.type = _type;
            state.channel = _channel;
        }
        finally {
        }
    }
    // $ANTLR end "T__103"

    // $ANTLR start "T__104"
    public final void mT__104() throws RecognitionException {
        try {
            int _type = T__104;
            int _channel = DEFAULT_TOKEN_CHANNEL;
            // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmScript.g:44:8: ( '>' )
            // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmScript.g:44:10: '>'
            {
            match('>'); 

            }

            state.type = _type;
            state.channel = _channel;
        }
        finally {
        }
    }
    // $ANTLR end "T__104"

    // $ANTLR start "T__105"
    public final void mT__105() throws RecognitionException {
        try {
            int _type = T__105;
            int _channel = DEFAULT_TOKEN_CHANNEL;
            // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmScript.g:45:8: ( 'gt' )
            // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmScript.g:45:10: 'gt'
            {
            match("gt"); 


            }

            state.type = _type;
            state.channel = _channel;
        }
        finally {
        }
    }
    // $ANTLR end "T__105"

    // $ANTLR start "T__106"
    public final void mT__106() throws RecognitionException {
        try {
            int _type = T__106;
            int _channel = DEFAULT_TOKEN_CHANNEL;
            // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmScript.g:46:8: ( 'lt' )
            // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmScript.g:46:10: 'lt'
            {
            match("lt"); 


            }

            state.type = _type;
            state.channel = _channel;
        }
        finally {
        }
    }
    // $ANTLR end "T__106"

    // $ANTLR start "T__107"
    public final void mT__107() throws RecognitionException {
        try {
            int _type = T__107;
            int _channel = DEFAULT_TOKEN_CHANNEL;
            // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmScript.g:47:8: ( 'gte' )
            // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmScript.g:47:10: 'gte'
            {
            match("gte"); 


            }

            state.type = _type;
            state.channel = _channel;
        }
        finally {
        }
    }
    // $ANTLR end "T__107"

    // $ANTLR start "T__108"
    public final void mT__108() throws RecognitionException {
        try {
            int _type = T__108;
            int _channel = DEFAULT_TOKEN_CHANNEL;
            // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmScript.g:48:8: ( 'lte' )
            // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmScript.g:48:10: 'lte'
            {
            match("lte"); 


            }

            state.type = _type;
            state.channel = _channel;
        }
        finally {
        }
    }
    // $ANTLR end "T__108"

    // $ANTLR start "T__109"
    public final void mT__109() throws RecognitionException {
        try {
            int _type = T__109;
            int _channel = DEFAULT_TOKEN_CHANNEL;
            // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmScript.g:49:8: ( 'Gt' )
            // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmScript.g:49:10: 'Gt'
            {
            match("Gt"); 


            }

            state.type = _type;
            state.channel = _channel;
        }
        finally {
        }
    }
    // $ANTLR end "T__109"

    // $ANTLR start "T__110"
    public final void mT__110() throws RecognitionException {
        try {
            int _type = T__110;
            int _channel = DEFAULT_TOKEN_CHANNEL;
            // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmScript.g:50:8: ( 'Lt' )
            // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmScript.g:50:10: 'Lt'
            {
            match("Lt"); 


            }

            state.type = _type;
            state.channel = _channel;
        }
        finally {
        }
    }
    // $ANTLR end "T__110"

    // $ANTLR start "T__111"
    public final void mT__111() throws RecognitionException {
        try {
            int _type = T__111;
            int _channel = DEFAULT_TOKEN_CHANNEL;
            // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmScript.g:51:8: ( 'Gte' )
            // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmScript.g:51:10: 'Gte'
            {
            match("Gte"); 


            }

            state.type = _type;
            state.channel = _channel;
        }
        finally {
        }
    }
    // $ANTLR end "T__111"

    // $ANTLR start "T__112"
    public final void mT__112() throws RecognitionException {
        try {
            int _type = T__112;
            int _channel = DEFAULT_TOKEN_CHANNEL;
            // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmScript.g:52:8: ( 'Lte' )
            // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmScript.g:52:10: 'Lte'
            {
            match("Lte"); 


            }

            state.type = _type;
            state.channel = _channel;
        }
        finally {
        }
    }
    // $ANTLR end "T__112"

    // $ANTLR start "T__113"
    public final void mT__113() throws RecognitionException {
        try {
            int _type = T__113;
            int _channel = DEFAULT_TOKEN_CHANNEL;
            // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmScript.g:53:8: ( 'GT' )
            // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmScript.g:53:10: 'GT'
            {
            match("GT"); 


            }

            state.type = _type;
            state.channel = _channel;
        }
        finally {
        }
    }
    // $ANTLR end "T__113"

    // $ANTLR start "T__114"
    public final void mT__114() throws RecognitionException {
        try {
            int _type = T__114;
            int _channel = DEFAULT_TOKEN_CHANNEL;
            // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmScript.g:54:8: ( 'LT' )
            // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmScript.g:54:10: 'LT'
            {
            match("LT"); 


            }

            state.type = _type;
            state.channel = _channel;
        }
        finally {
        }
    }
    // $ANTLR end "T__114"

    // $ANTLR start "T__115"
    public final void mT__115() throws RecognitionException {
        try {
            int _type = T__115;
            int _channel = DEFAULT_TOKEN_CHANNEL;
            // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmScript.g:55:8: ( 'GTE' )
            // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmScript.g:55:10: 'GTE'
            {
            match("GTE"); 


            }

            state.type = _type;
            state.channel = _channel;
        }
        finally {
        }
    }
    // $ANTLR end "T__115"

    // $ANTLR start "T__116"
    public final void mT__116() throws RecognitionException {
        try {
            int _type = T__116;
            int _channel = DEFAULT_TOKEN_CHANNEL;
            // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmScript.g:56:8: ( 'LTE' )
            // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmScript.g:56:10: 'LTE'
            {
            match("LTE"); 


            }

            state.type = _type;
            state.channel = _channel;
        }
        finally {
        }
    }
    // $ANTLR end "T__116"

    // $ANTLR start "T__117"
    public final void mT__117() throws RecognitionException {
        try {
            int _type = T__117;
            int _channel = DEFAULT_TOKEN_CHANNEL;
            // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmScript.g:57:8: ( '<<' )
            // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmScript.g:57:10: '<<'
            {
            match("<<"); 


            }

            state.type = _type;
            state.channel = _channel;
        }
        finally {
        }
    }
    // $ANTLR end "T__117"

    // $ANTLR start "T__118"
    public final void mT__118() throws RecognitionException {
        try {
            int _type = T__118;
            int _channel = DEFAULT_TOKEN_CHANNEL;
            // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmScript.g:58:8: ( '>>' )
            // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmScript.g:58:10: '>>'
            {
            match(">>"); 


            }

            state.type = _type;
            state.channel = _channel;
        }
        finally {
        }
    }
    // $ANTLR end "T__118"

    // $ANTLR start "T__119"
    public final void mT__119() throws RecognitionException {
        try {
            int _type = T__119;
            int _channel = DEFAULT_TOKEN_CHANNEL;
            // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmScript.g:59:8: ( '+' )
            // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmScript.g:59:10: '+'
            {
            match('+'); 

            }

            state.type = _type;
            state.channel = _channel;
        }
        finally {
        }
    }
    // $ANTLR end "T__119"

    // $ANTLR start "T__120"
    public final void mT__120() throws RecognitionException {
        try {
            int _type = T__120;
            int _channel = DEFAULT_TOKEN_CHANNEL;
            // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmScript.g:60:8: ( '-' )
            // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmScript.g:60:10: '-'
            {
            match('-'); 

            }

            state.type = _type;
            state.channel = _channel;
        }
        finally {
        }
    }
    // $ANTLR end "T__120"

    // $ANTLR start "T__121"
    public final void mT__121() throws RecognitionException {
        try {
            int _type = T__121;
            int _channel = DEFAULT_TOKEN_CHANNEL;
            // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmScript.g:61:8: ( '*' )
            // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmScript.g:61:10: '*'
            {
            match('*'); 

            }

            state.type = _type;
            state.channel = _channel;
        }
        finally {
        }
    }
    // $ANTLR end "T__121"

    // $ANTLR start "T__122"
    public final void mT__122() throws RecognitionException {
        try {
            int _type = T__122;
            int _channel = DEFAULT_TOKEN_CHANNEL;
            // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmScript.g:62:8: ( '/' )
            // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmScript.g:62:10: '/'
            {
            match('/'); 

            }

            state.type = _type;
            state.channel = _channel;
        }
        finally {
        }
    }
    // $ANTLR end "T__122"

    // $ANTLR start "T__123"
    public final void mT__123() throws RecognitionException {
        try {
            int _type = T__123;
            int _channel = DEFAULT_TOKEN_CHANNEL;
            // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmScript.g:63:8: ( '%' )
            // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmScript.g:63:10: '%'
            {
            match('%'); 

            }

            state.type = _type;
            state.channel = _channel;
        }
        finally {
        }
    }
    // $ANTLR end "T__123"

    // $ANTLR start "T__124"
    public final void mT__124() throws RecognitionException {
        try {
            int _type = T__124;
            int _channel = DEFAULT_TOKEN_CHANNEL;
            // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmScript.g:64:8: ( '->' )
            // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmScript.g:64:10: '->'
            {
            match("->"); 


            }

            state.type = _type;
            state.channel = _channel;
        }
        finally {
        }
    }
    // $ANTLR end "T__124"

    // $ANTLR start "T__125"
    public final void mT__125() throws RecognitionException {
        try {
            int _type = T__125;
            int _channel = DEFAULT_TOKEN_CHANNEL;
            // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmScript.g:65:8: ( '++' )
            // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmScript.g:65:10: '++'
            {
            match("++"); 


            }

            state.type = _type;
            state.channel = _channel;
        }
        finally {
        }
    }
    // $ANTLR end "T__125"

    // $ANTLR start "T__126"
    public final void mT__126() throws RecognitionException {
        try {
            int _type = T__126;
            int _channel = DEFAULT_TOKEN_CHANNEL;
            // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmScript.g:66:8: ( '--' )
            // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmScript.g:66:10: '--'
            {
            match("--"); 


            }

            state.type = _type;
            state.channel = _channel;
        }
        finally {
        }
    }
    // $ANTLR end "T__126"

    // $ANTLR start "T__127"
    public final void mT__127() throws RecognitionException {
        try {
            int _type = T__127;
            int _channel = DEFAULT_TOKEN_CHANNEL;
            // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmScript.g:67:8: ( '~' )
            // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmScript.g:67:10: '~'
            {
            match('~'); 

            }

            state.type = _type;
            state.channel = _channel;
        }
        finally {
        }
    }
    // $ANTLR end "T__127"

    // $ANTLR start "T__128"
    public final void mT__128() throws RecognitionException {
        try {
            int _type = T__128;
            int _channel = DEFAULT_TOKEN_CHANNEL;
            // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmScript.g:68:8: ( '!' )
            // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmScript.g:68:10: '!'
            {
            match('!'); 

            }

            state.type = _type;
            state.channel = _channel;
        }
        finally {
        }
    }
    // $ANTLR end "T__128"

    // $ANTLR start "T__129"
    public final void mT__129() throws RecognitionException {
        try {
            int _type = T__129;
            int _channel = DEFAULT_TOKEN_CHANNEL;
            // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmScript.g:69:8: ( '(' )
            // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmScript.g:69:10: '('
            {
            match('('); 

            }

            state.type = _type;
            state.channel = _channel;
        }
        finally {
        }
    }
    // $ANTLR end "T__129"

    // $ANTLR start "T__130"
    public final void mT__130() throws RecognitionException {
        try {
            int _type = T__130;
            int _channel = DEFAULT_TOKEN_CHANNEL;
            // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmScript.g:70:8: ( ')' )
            // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmScript.g:70:10: ')'
            {
            match(')'); 

            }

            state.type = _type;
            state.channel = _channel;
        }
        finally {
        }
    }
    // $ANTLR end "T__130"

    // $ANTLR start "T__131"
    public final void mT__131() throws RecognitionException {
        try {
            int _type = T__131;
            int _channel = DEFAULT_TOKEN_CHANNEL;
            // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmScript.g:71:8: ( '.' )
            // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmScript.g:71:10: '.'
            {
            match('.'); 

            }

            state.type = _type;
            state.channel = _channel;
        }
        finally {
        }
    }
    // $ANTLR end "T__131"

    // $ANTLR start "T__132"
    public final void mT__132() throws RecognitionException {
        try {
            int _type = T__132;
            int _channel = DEFAULT_TOKEN_CHANNEL;
            // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmScript.g:72:8: ( '[' )
            // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmScript.g:72:10: '['
            {
            match('['); 

            }

            state.type = _type;
            state.channel = _channel;
        }
        finally {
        }
    }
    // $ANTLR end "T__132"

    // $ANTLR start "T__133"
    public final void mT__133() throws RecognitionException {
        try {
            int _type = T__133;
            int _channel = DEFAULT_TOKEN_CHANNEL;
            // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmScript.g:73:8: ( ']' )
            // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmScript.g:73:10: ']'
            {
            match(']'); 

            }

            state.type = _type;
            state.channel = _channel;
        }
        finally {
        }
    }
    // $ANTLR end "T__133"

    // $ANTLR start "T__134"
    public final void mT__134() throws RecognitionException {
        try {
            int _type = T__134;
            int _channel = DEFAULT_TOKEN_CHANNEL;
            // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmScript.g:74:8: ( ',' )
            // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmScript.g:74:10: ','
            {
            match(','); 

            }

            state.type = _type;
            state.channel = _channel;
        }
        finally {
        }
    }
    // $ANTLR end "T__134"

    // $ANTLR start "T__135"
    public final void mT__135() throws RecognitionException {
        try {
            int _type = T__135;
            int _channel = DEFAULT_TOKEN_CHANNEL;
            // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmScript.g:75:8: ( 'new' )
            // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmScript.g:75:10: 'new'
            {
            match("new"); 


            }

            state.type = _type;
            state.channel = _channel;
        }
        finally {
        }
    }
    // $ANTLR end "T__135"

    // $ANTLR start "T__136"
    public final void mT__136() throws RecognitionException {
        try {
            int _type = T__136;
            int _channel = DEFAULT_TOKEN_CHANNEL;
            // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmScript.g:76:8: ( ';' )
            // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmScript.g:76:10: ';'
            {
            match(';'); 

            }

            state.type = _type;
            state.channel = _channel;
        }
        finally {
        }
    }
    // $ANTLR end "T__136"

    // $ANTLR start "T__137"
    public final void mT__137() throws RecognitionException {
        try {
            int _type = T__137;
            int _channel = DEFAULT_TOKEN_CHANNEL;
            // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmScript.g:77:8: ( 'if' )
            // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmScript.g:77:10: 'if'
            {
            match("if"); 


            }

            state.type = _type;
            state.channel = _channel;
        }
        finally {
        }
    }
    // $ANTLR end "T__137"

    // $ANTLR start "T__138"
    public final void mT__138() throws RecognitionException {
        try {
            int _type = T__138;
            int _channel = DEFAULT_TOKEN_CHANNEL;
            // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmScript.g:78:8: ( 'else' )
            // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmScript.g:78:10: 'else'
            {
            match("else"); 


            }

            state.type = _type;
            state.channel = _channel;
        }
        finally {
        }
    }
    // $ANTLR end "T__138"

    // $ANTLR start "T__139"
    public final void mT__139() throws RecognitionException {
        try {
            int _type = T__139;
            int _channel = DEFAULT_TOKEN_CHANNEL;
            // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmScript.g:79:8: ( 'while' )
            // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmScript.g:79:10: 'while'
            {
            match("while"); 


            }

            state.type = _type;
            state.channel = _channel;
        }
        finally {
        }
    }
    // $ANTLR end "T__139"

    // $ANTLR start "T__140"
    public final void mT__140() throws RecognitionException {
        try {
            int _type = T__140;
            int _channel = DEFAULT_TOKEN_CHANNEL;
            // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmScript.g:80:8: ( 'do' )
            // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmScript.g:80:10: 'do'
            {
            match("do"); 


            }

            state.type = _type;
            state.channel = _channel;
        }
        finally {
        }
    }
    // $ANTLR end "T__140"

    // $ANTLR start "T__141"
    public final void mT__141() throws RecognitionException {
        try {
            int _type = T__141;
            int _channel = DEFAULT_TOKEN_CHANNEL;
            // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmScript.g:81:8: ( 'for' )
            // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmScript.g:81:10: 'for'
            {
            match("for"); 


            }

            state.type = _type;
            state.channel = _channel;
        }
        finally {
        }
    }
    // $ANTLR end "T__141"

    // $ANTLR start "T__142"
    public final void mT__142() throws RecognitionException {
        try {
            int _type = T__142;
            int _channel = DEFAULT_TOKEN_CHANNEL;
            // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmScript.g:82:8: ( 'foreach' )
            // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmScript.g:82:10: 'foreach'
            {
            match("foreach"); 


            }

            state.type = _type;
            state.channel = _channel;
        }
        finally {
        }
    }
    // $ANTLR end "T__142"

    // $ANTLR start "T__143"
    public final void mT__143() throws RecognitionException {
        try {
            int _type = T__143;
            int _channel = DEFAULT_TOKEN_CHANNEL;
            // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmScript.g:83:8: ( 'in' )
            // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmScript.g:83:10: 'in'
            {
            match("in"); 


            }

            state.type = _type;
            state.channel = _channel;
        }
        finally {
        }
    }
    // $ANTLR end "T__143"

    // $ANTLR start "T__144"
    public final void mT__144() throws RecognitionException {
        try {
            int _type = T__144;
            int _channel = DEFAULT_TOKEN_CHANNEL;
            // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmScript.g:84:8: ( 'continue' )
            // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmScript.g:84:10: 'continue'
            {
            match("continue"); 


            }

            state.type = _type;
            state.channel = _channel;
        }
        finally {
        }
    }
    // $ANTLR end "T__144"

    // $ANTLR start "T__145"
    public final void mT__145() throws RecognitionException {
        try {
            int _type = T__145;
            int _channel = DEFAULT_TOKEN_CHANNEL;
            // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmScript.g:85:8: ( 'break' )
            // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmScript.g:85:10: 'break'
            {
            match("break"); 


            }

            state.type = _type;
            state.channel = _channel;
        }
        finally {
        }
    }
    // $ANTLR end "T__145"

    // $ANTLR start "T__146"
    public final void mT__146() throws RecognitionException {
        try {
            int _type = T__146;
            int _channel = DEFAULT_TOKEN_CHANNEL;
            // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmScript.g:86:8: ( 'return' )
            // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmScript.g:86:10: 'return'
            {
            match("return"); 


            }

            state.type = _type;
            state.channel = _channel;
        }
        finally {
        }
    }
    // $ANTLR end "T__146"

    // $ANTLR start "T__147"
    public final void mT__147() throws RecognitionException {
        try {
            int _type = T__147;
            int _channel = DEFAULT_TOKEN_CHANNEL;
            // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmScript.g:87:8: ( 'try' )
            // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmScript.g:87:10: 'try'
            {
            match("try"); 


            }

            state.type = _type;
            state.channel = _channel;
        }
        finally {
        }
    }
    // $ANTLR end "T__147"

    // $ANTLR start "T__148"
    public final void mT__148() throws RecognitionException {
        try {
            int _type = T__148;
            int _channel = DEFAULT_TOKEN_CHANNEL;
            // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmScript.g:88:8: ( 'catch' )
            // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmScript.g:88:10: 'catch'
            {
            match("catch"); 


            }

            state.type = _type;
            state.channel = _channel;
        }
        finally {
        }
    }
    // $ANTLR end "T__148"

    // $ANTLR start "T__149"
    public final void mT__149() throws RecognitionException {
        try {
            int _type = T__149;
            int _channel = DEFAULT_TOKEN_CHANNEL;
            // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmScript.g:89:8: ( 'finally' )
            // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmScript.g:89:10: 'finally'
            {
            match("finally"); 


            }

            state.type = _type;
            state.channel = _channel;
        }
        finally {
        }
    }
    // $ANTLR end "T__149"

    // $ANTLR start "T__150"
    public final void mT__150() throws RecognitionException {
        try {
            int _type = T__150;
            int _channel = DEFAULT_TOKEN_CHANNEL;
            // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmScript.g:90:8: ( 'true' )
            // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmScript.g:90:10: 'true'
            {
            match("true"); 


            }

            state.type = _type;
            state.channel = _channel;
        }
        finally {
        }
    }
    // $ANTLR end "T__150"

    // $ANTLR start "T__151"
    public final void mT__151() throws RecognitionException {
        try {
            int _type = T__151;
            int _channel = DEFAULT_TOKEN_CHANNEL;
            // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmScript.g:91:8: ( 'false' )
            // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmScript.g:91:10: 'false'
            {
            match("false"); 


            }

            state.type = _type;
            state.channel = _channel;
        }
        finally {
        }
    }
    // $ANTLR end "T__151"

    // $ANTLR start "T__152"
    public final void mT__152() throws RecognitionException {
        try {
            int _type = T__152;
            int _channel = DEFAULT_TOKEN_CHANNEL;
            // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmScript.g:92:8: ( 'null' )
            // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmScript.g:92:10: 'null'
            {
            match("null"); 


            }

            state.type = _type;
            state.channel = _channel;
        }
        finally {
        }
    }
    // $ANTLR end "T__152"

    // $ANTLR start "T__153"
    public final void mT__153() throws RecognitionException {
        try {
            int _type = T__153;
            int _channel = DEFAULT_TOKEN_CHANNEL;
            // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmScript.g:93:8: ( 'NULL' )
            // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmScript.g:93:10: 'NULL'
            {
            match("NULL"); 


            }

            state.type = _type;
            state.channel = _channel;
        }
        finally {
        }
    }
    // $ANTLR end "T__153"

    // $ANTLR start "Id"
    public final void mId() throws RecognitionException {
        try {
            int _type = Id;
            int _channel = DEFAULT_TOKEN_CHANNEL;
            // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmScript.g:755:2: ( ( 'a' .. 'z' | 'A' .. 'Z' | '_' ) ( 'a' .. 'z' | 'A' .. 'Z' | '0' .. '9' | '_' )* )
            // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmScript.g:755:4: ( 'a' .. 'z' | 'A' .. 'Z' | '_' ) ( 'a' .. 'z' | 'A' .. 'Z' | '0' .. '9' | '_' )*
            {
            if ( (input.LA(1)>='A' && input.LA(1)<='Z')||input.LA(1)=='_'||(input.LA(1)>='a' && input.LA(1)<='z') ) {
                input.consume();

            }
            else {
                MismatchedSetException mse = new MismatchedSetException(null,input);
                recover(mse);
                throw mse;}

            // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmScript.g:755:28: ( 'a' .. 'z' | 'A' .. 'Z' | '0' .. '9' | '_' )*
            loop1:
            do {
                int alt1=2;
                int LA1_0 = input.LA(1);

                if ( ((LA1_0>='0' && LA1_0<='9')||(LA1_0>='A' && LA1_0<='Z')||LA1_0=='_'||(LA1_0>='a' && LA1_0<='z')) ) {
                    alt1=1;
                }


                switch (alt1) {
            	case 1 :
            	    // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmScript.g:
            	    {
            	    if ( (input.LA(1)>='0' && input.LA(1)<='9')||(input.LA(1)>='A' && input.LA(1)<='Z')||input.LA(1)=='_'||(input.LA(1)>='a' && input.LA(1)<='z') ) {
            	        input.consume();

            	    }
            	    else {
            	        MismatchedSetException mse = new MismatchedSetException(null,input);
            	        recover(mse);
            	        throw mse;}


            	    }
            	    break;

            	default :
            	    break loop1;
                }
            } while (true);


            }

            state.type = _type;
            state.channel = _channel;
        }
        finally {
        }
    }
    // $ANTLR end "Id"

    // $ANTLR start "HexLiteral"
    public final void mHexLiteral() throws RecognitionException {
        try {
            int _type = HexLiteral;
            int _channel = DEFAULT_TOKEN_CHANNEL;
            // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmScript.g:758:2: ( '0' ( 'x' | 'X' ) ( HexDigit )+ ( IntegerTypeSuffix )? )
            // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmScript.g:758:4: '0' ( 'x' | 'X' ) ( HexDigit )+ ( IntegerTypeSuffix )?
            {
            match('0'); 
            if ( input.LA(1)=='X'||input.LA(1)=='x' ) {
                input.consume();

            }
            else {
                MismatchedSetException mse = new MismatchedSetException(null,input);
                recover(mse);
                throw mse;}

            // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmScript.g:758:18: ( HexDigit )+
            int cnt2=0;
            loop2:
            do {
                int alt2=2;
                int LA2_0 = input.LA(1);

                if ( ((LA2_0>='0' && LA2_0<='9')||(LA2_0>='A' && LA2_0<='F')||(LA2_0>='a' && LA2_0<='f')) ) {
                    alt2=1;
                }


                switch (alt2) {
            	case 1 :
            	    // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmScript.g:758:18: HexDigit
            	    {
            	    mHexDigit(); 

            	    }
            	    break;

            	default :
            	    if ( cnt2 >= 1 ) break loop2;
                        EarlyExitException eee =
                            new EarlyExitException(2, input);
                        throw eee;
                }
                cnt2++;
            } while (true);

            // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmScript.g:758:28: ( IntegerTypeSuffix )?
            int alt3=2;
            int LA3_0 = input.LA(1);

            if ( (LA3_0=='L'||LA3_0=='l') ) {
                alt3=1;
            }
            switch (alt3) {
                case 1 :
                    // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmScript.g:758:28: IntegerTypeSuffix
                    {
                    mIntegerTypeSuffix(); 

                    }
                    break;

            }


            }

            state.type = _type;
            state.channel = _channel;
        }
        finally {
        }
    }
    // $ANTLR end "HexLiteral"

    // $ANTLR start "IntegerLiteral"
    public final void mIntegerLiteral() throws RecognitionException {
        try {
            int _type = IntegerLiteral;
            int _channel = DEFAULT_TOKEN_CHANNEL;
            // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmScript.g:761:2: ( ( '0' | '1' .. '9' ( '0' .. '9' )* ) ( IntegerTypeSuffix )? )
            // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmScript.g:762:2: ( '0' | '1' .. '9' ( '0' .. '9' )* ) ( IntegerTypeSuffix )?
            {
            // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmScript.g:762:2: ( '0' | '1' .. '9' ( '0' .. '9' )* )
            int alt5=2;
            int LA5_0 = input.LA(1);

            if ( (LA5_0=='0') ) {
                alt5=1;
            }
            else if ( ((LA5_0>='1' && LA5_0<='9')) ) {
                alt5=2;
            }
            else {
                NoViableAltException nvae =
                    new NoViableAltException("", 5, 0, input);

                throw nvae;
            }
            switch (alt5) {
                case 1 :
                    // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmScript.g:762:3: '0'
                    {
                    match('0'); 

                    }
                    break;
                case 2 :
                    // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmScript.g:762:9: '1' .. '9' ( '0' .. '9' )*
                    {
                    matchRange('1','9'); 
                    // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmScript.g:762:18: ( '0' .. '9' )*
                    loop4:
                    do {
                        int alt4=2;
                        int LA4_0 = input.LA(1);

                        if ( ((LA4_0>='0' && LA4_0<='9')) ) {
                            alt4=1;
                        }


                        switch (alt4) {
                    	case 1 :
                    	    // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmScript.g:762:18: '0' .. '9'
                    	    {
                    	    matchRange('0','9'); 

                    	    }
                    	    break;

                    	default :
                    	    break loop4;
                        }
                    } while (true);


                    }
                    break;

            }

            // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmScript.g:762:29: ( IntegerTypeSuffix )?
            int alt6=2;
            int LA6_0 = input.LA(1);

            if ( (LA6_0=='L'||LA6_0=='l') ) {
                alt6=1;
            }
            switch (alt6) {
                case 1 :
                    // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmScript.g:762:29: IntegerTypeSuffix
                    {
                    mIntegerTypeSuffix(); 

                    }
                    break;

            }


            }

            state.type = _type;
            state.channel = _channel;
        }
        finally {
        }
    }
    // $ANTLR end "IntegerLiteral"

    // $ANTLR start "OctalLiteral"
    public final void mOctalLiteral() throws RecognitionException {
        try {
            int _type = OctalLiteral;
            int _channel = DEFAULT_TOKEN_CHANNEL;
            // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmScript.g:765:2: ( '0' ( '0' .. '7' )+ ( IntegerTypeSuffix )? )
            // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmScript.g:765:4: '0' ( '0' .. '7' )+ ( IntegerTypeSuffix )?
            {
            match('0'); 
            // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmScript.g:765:8: ( '0' .. '7' )+
            int cnt7=0;
            loop7:
            do {
                int alt7=2;
                int LA7_0 = input.LA(1);

                if ( ((LA7_0>='0' && LA7_0<='7')) ) {
                    alt7=1;
                }


                switch (alt7) {
            	case 1 :
            	    // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmScript.g:765:9: '0' .. '7'
            	    {
            	    matchRange('0','7'); 

            	    }
            	    break;

            	default :
            	    if ( cnt7 >= 1 ) break loop7;
                        EarlyExitException eee =
                            new EarlyExitException(7, input);
                        throw eee;
                }
                cnt7++;
            } while (true);

            // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmScript.g:765:20: ( IntegerTypeSuffix )?
            int alt8=2;
            int LA8_0 = input.LA(1);

            if ( (LA8_0=='L'||LA8_0=='l') ) {
                alt8=1;
            }
            switch (alt8) {
                case 1 :
                    // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmScript.g:765:20: IntegerTypeSuffix
                    {
                    mIntegerTypeSuffix(); 

                    }
                    break;

            }


            }

            state.type = _type;
            state.channel = _channel;
        }
        finally {
        }
    }
    // $ANTLR end "OctalLiteral"

    // $ANTLR start "HexDigit"
    public final void mHexDigit() throws RecognitionException {
        try {
            // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmScript.g:769:3: ( ( '0' .. '9' | 'a' .. 'f' | 'A' .. 'F' ) )
            // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmScript.g:769:5: ( '0' .. '9' | 'a' .. 'f' | 'A' .. 'F' )
            {
            if ( (input.LA(1)>='0' && input.LA(1)<='9')||(input.LA(1)>='A' && input.LA(1)<='F')||(input.LA(1)>='a' && input.LA(1)<='f') ) {
                input.consume();

            }
            else {
                MismatchedSetException mse = new MismatchedSetException(null,input);
                recover(mse);
                throw mse;}


            }

        }
        finally {
        }
    }
    // $ANTLR end "HexDigit"

    // $ANTLR start "IntegerTypeSuffix"
    public final void mIntegerTypeSuffix() throws RecognitionException {
        try {
            // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmScript.g:773:2: ( ( 'l' | 'L' ) )
            // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmScript.g:773:4: ( 'l' | 'L' )
            {
            if ( input.LA(1)=='L'||input.LA(1)=='l' ) {
                input.consume();

            }
            else {
                MismatchedSetException mse = new MismatchedSetException(null,input);
                recover(mse);
                throw mse;}


            }

        }
        finally {
        }
    }
    // $ANTLR end "IntegerTypeSuffix"

    // $ANTLR start "DecimalLiteral"
    public final void mDecimalLiteral() throws RecognitionException {
        try {
            int _type = DecimalLiteral;
            int _channel = DEFAULT_TOKEN_CHANNEL;
            // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmScript.g:776:2: ( ( '0' .. '9' )+ '.' ( '0' .. '9' )* ( Exponent )? ( FloatTypeSuffix )? | '.' ( '0' .. '9' )+ ( Exponent )? ( FloatTypeSuffix )? | ( '0' .. '9' )+ Exponent ( FloatTypeSuffix )? | ( '0' .. '9' )+ FloatTypeSuffix )
            int alt19=4;
            alt19 = dfa19.predict(input);
            switch (alt19) {
                case 1 :
                    // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmScript.g:776:4: ( '0' .. '9' )+ '.' ( '0' .. '9' )* ( Exponent )? ( FloatTypeSuffix )?
                    {
                    // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmScript.g:776:4: ( '0' .. '9' )+
                    int cnt9=0;
                    loop9:
                    do {
                        int alt9=2;
                        int LA9_0 = input.LA(1);

                        if ( ((LA9_0>='0' && LA9_0<='9')) ) {
                            alt9=1;
                        }


                        switch (alt9) {
                    	case 1 :
                    	    // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmScript.g:776:5: '0' .. '9'
                    	    {
                    	    matchRange('0','9'); 

                    	    }
                    	    break;

                    	default :
                    	    if ( cnt9 >= 1 ) break loop9;
                                EarlyExitException eee =
                                    new EarlyExitException(9, input);
                                throw eee;
                        }
                        cnt9++;
                    } while (true);

                    match('.'); 
                    // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmScript.g:776:20: ( '0' .. '9' )*
                    loop10:
                    do {
                        int alt10=2;
                        int LA10_0 = input.LA(1);

                        if ( ((LA10_0>='0' && LA10_0<='9')) ) {
                            alt10=1;
                        }


                        switch (alt10) {
                    	case 1 :
                    	    // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmScript.g:776:21: '0' .. '9'
                    	    {
                    	    matchRange('0','9'); 

                    	    }
                    	    break;

                    	default :
                    	    break loop10;
                        }
                    } while (true);

                    // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmScript.g:776:32: ( Exponent )?
                    int alt11=2;
                    int LA11_0 = input.LA(1);

                    if ( (LA11_0=='E'||LA11_0=='e') ) {
                        alt11=1;
                    }
                    switch (alt11) {
                        case 1 :
                            // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmScript.g:776:32: Exponent
                            {
                            mExponent(); 

                            }
                            break;

                    }

                    // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmScript.g:776:42: ( FloatTypeSuffix )?
                    int alt12=2;
                    int LA12_0 = input.LA(1);

                    if ( (LA12_0=='D'||LA12_0=='F'||LA12_0=='d'||LA12_0=='f') ) {
                        alt12=1;
                    }
                    switch (alt12) {
                        case 1 :
                            // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmScript.g:776:42: FloatTypeSuffix
                            {
                            mFloatTypeSuffix(); 

                            }
                            break;

                    }


                    }
                    break;
                case 2 :
                    // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmScript.g:777:4: '.' ( '0' .. '9' )+ ( Exponent )? ( FloatTypeSuffix )?
                    {
                    match('.'); 
                    // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmScript.g:777:8: ( '0' .. '9' )+
                    int cnt13=0;
                    loop13:
                    do {
                        int alt13=2;
                        int LA13_0 = input.LA(1);

                        if ( ((LA13_0>='0' && LA13_0<='9')) ) {
                            alt13=1;
                        }


                        switch (alt13) {
                    	case 1 :
                    	    // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmScript.g:777:9: '0' .. '9'
                    	    {
                    	    matchRange('0','9'); 

                    	    }
                    	    break;

                    	default :
                    	    if ( cnt13 >= 1 ) break loop13;
                                EarlyExitException eee =
                                    new EarlyExitException(13, input);
                                throw eee;
                        }
                        cnt13++;
                    } while (true);

                    // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmScript.g:777:20: ( Exponent )?
                    int alt14=2;
                    int LA14_0 = input.LA(1);

                    if ( (LA14_0=='E'||LA14_0=='e') ) {
                        alt14=1;
                    }
                    switch (alt14) {
                        case 1 :
                            // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmScript.g:777:20: Exponent
                            {
                            mExponent(); 

                            }
                            break;

                    }

                    // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmScript.g:777:30: ( FloatTypeSuffix )?
                    int alt15=2;
                    int LA15_0 = input.LA(1);

                    if ( (LA15_0=='D'||LA15_0=='F'||LA15_0=='d'||LA15_0=='f') ) {
                        alt15=1;
                    }
                    switch (alt15) {
                        case 1 :
                            // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmScript.g:777:30: FloatTypeSuffix
                            {
                            mFloatTypeSuffix(); 

                            }
                            break;

                    }


                    }
                    break;
                case 3 :
                    // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmScript.g:778:4: ( '0' .. '9' )+ Exponent ( FloatTypeSuffix )?
                    {
                    // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmScript.g:778:4: ( '0' .. '9' )+
                    int cnt16=0;
                    loop16:
                    do {
                        int alt16=2;
                        int LA16_0 = input.LA(1);

                        if ( ((LA16_0>='0' && LA16_0<='9')) ) {
                            alt16=1;
                        }


                        switch (alt16) {
                    	case 1 :
                    	    // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmScript.g:778:5: '0' .. '9'
                    	    {
                    	    matchRange('0','9'); 

                    	    }
                    	    break;

                    	default :
                    	    if ( cnt16 >= 1 ) break loop16;
                                EarlyExitException eee =
                                    new EarlyExitException(16, input);
                                throw eee;
                        }
                        cnt16++;
                    } while (true);

                    mExponent(); 
                    // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmScript.g:778:25: ( FloatTypeSuffix )?
                    int alt17=2;
                    int LA17_0 = input.LA(1);

                    if ( (LA17_0=='D'||LA17_0=='F'||LA17_0=='d'||LA17_0=='f') ) {
                        alt17=1;
                    }
                    switch (alt17) {
                        case 1 :
                            // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmScript.g:778:25: FloatTypeSuffix
                            {
                            mFloatTypeSuffix(); 

                            }
                            break;

                    }


                    }
                    break;
                case 4 :
                    // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmScript.g:779:4: ( '0' .. '9' )+ FloatTypeSuffix
                    {
                    // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmScript.g:779:4: ( '0' .. '9' )+
                    int cnt18=0;
                    loop18:
                    do {
                        int alt18=2;
                        int LA18_0 = input.LA(1);

                        if ( ((LA18_0>='0' && LA18_0<='9')) ) {
                            alt18=1;
                        }


                        switch (alt18) {
                    	case 1 :
                    	    // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmScript.g:779:5: '0' .. '9'
                    	    {
                    	    matchRange('0','9'); 

                    	    }
                    	    break;

                    	default :
                    	    if ( cnt18 >= 1 ) break loop18;
                                EarlyExitException eee =
                                    new EarlyExitException(18, input);
                                throw eee;
                        }
                        cnt18++;
                    } while (true);

                    mFloatTypeSuffix(); 

                    }
                    break;

            }
            state.type = _type;
            state.channel = _channel;
        }
        finally {
        }
    }
    // $ANTLR end "DecimalLiteral"

    // $ANTLR start "Exponent"
    public final void mExponent() throws RecognitionException {
        try {
            // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmScript.g:783:2: ( ( 'e' | 'E' ) ( '+' | '-' )? ( '0' .. '9' )+ )
            // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmScript.g:783:4: ( 'e' | 'E' ) ( '+' | '-' )? ( '0' .. '9' )+
            {
            if ( input.LA(1)=='E'||input.LA(1)=='e' ) {
                input.consume();

            }
            else {
                MismatchedSetException mse = new MismatchedSetException(null,input);
                recover(mse);
                throw mse;}

            // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmScript.g:783:14: ( '+' | '-' )?
            int alt20=2;
            int LA20_0 = input.LA(1);

            if ( (LA20_0=='+'||LA20_0=='-') ) {
                alt20=1;
            }
            switch (alt20) {
                case 1 :
                    // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmScript.g:
                    {
                    if ( input.LA(1)=='+'||input.LA(1)=='-' ) {
                        input.consume();

                    }
                    else {
                        MismatchedSetException mse = new MismatchedSetException(null,input);
                        recover(mse);
                        throw mse;}


                    }
                    break;

            }

            // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmScript.g:783:25: ( '0' .. '9' )+
            int cnt21=0;
            loop21:
            do {
                int alt21=2;
                int LA21_0 = input.LA(1);

                if ( ((LA21_0>='0' && LA21_0<='9')) ) {
                    alt21=1;
                }


                switch (alt21) {
            	case 1 :
            	    // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmScript.g:783:26: '0' .. '9'
            	    {
            	    matchRange('0','9'); 

            	    }
            	    break;

            	default :
            	    if ( cnt21 >= 1 ) break loop21;
                        EarlyExitException eee =
                            new EarlyExitException(21, input);
                        throw eee;
                }
                cnt21++;
            } while (true);


            }

        }
        finally {
        }
    }
    // $ANTLR end "Exponent"

    // $ANTLR start "FloatTypeSuffix"
    public final void mFloatTypeSuffix() throws RecognitionException {
        try {
            // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmScript.g:787:2: ( ( 'f' | 'F' | 'd' | 'D' ) )
            // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmScript.g:787:4: ( 'f' | 'F' | 'd' | 'D' )
            {
            if ( input.LA(1)=='D'||input.LA(1)=='F'||input.LA(1)=='d'||input.LA(1)=='f' ) {
                input.consume();

            }
            else {
                MismatchedSetException mse = new MismatchedSetException(null,input);
                recover(mse);
                throw mse;}


            }

        }
        finally {
        }
    }
    // $ANTLR end "FloatTypeSuffix"

    // $ANTLR start "StringLiteral"
    public final void mStringLiteral() throws RecognitionException {
        try {
            int _type = StringLiteral;
            int _channel = DEFAULT_TOKEN_CHANNEL;
            // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmScript.g:790:1: ( '\"' (~ ( '\"' ) )* '\"' | '\\'' (~ ( '\\'' ) )* '\\'' )
            int alt24=2;
            int LA24_0 = input.LA(1);

            if ( (LA24_0=='\"') ) {
                alt24=1;
            }
            else if ( (LA24_0=='\'') ) {
                alt24=2;
            }
            else {
                NoViableAltException nvae =
                    new NoViableAltException("", 24, 0, input);

                throw nvae;
            }
            switch (alt24) {
                case 1 :
                    // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmScript.g:790:3: '\"' (~ ( '\"' ) )* '\"'
                    {
                    match('\"'); 
                    // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmScript.g:790:7: (~ ( '\"' ) )*
                    loop22:
                    do {
                        int alt22=2;
                        int LA22_0 = input.LA(1);

                        if ( ((LA22_0>='\u0000' && LA22_0<='!')||(LA22_0>='#' && LA22_0<='\uFFFF')) ) {
                            alt22=1;
                        }


                        switch (alt22) {
                    	case 1 :
                    	    // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmScript.g:790:9: ~ ( '\"' )
                    	    {
                    	    if ( (input.LA(1)>='\u0000' && input.LA(1)<='!')||(input.LA(1)>='#' && input.LA(1)<='\uFFFF') ) {
                    	        input.consume();

                    	    }
                    	    else {
                    	        MismatchedSetException mse = new MismatchedSetException(null,input);
                    	        recover(mse);
                    	        throw mse;}


                    	    }
                    	    break;

                    	default :
                    	    break loop22;
                        }
                    } while (true);

                    match('\"'); 

                    }
                    break;
                case 2 :
                    // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmScript.g:791:3: '\\'' (~ ( '\\'' ) )* '\\''
                    {
                    match('\''); 
                    // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmScript.g:791:8: (~ ( '\\'' ) )*
                    loop23:
                    do {
                        int alt23=2;
                        int LA23_0 = input.LA(1);

                        if ( ((LA23_0>='\u0000' && LA23_0<='&')||(LA23_0>='(' && LA23_0<='\uFFFF')) ) {
                            alt23=1;
                        }


                        switch (alt23) {
                    	case 1 :
                    	    // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmScript.g:791:10: ~ ( '\\'' )
                    	    {
                    	    if ( (input.LA(1)>='\u0000' && input.LA(1)<='&')||(input.LA(1)>='(' && input.LA(1)<='\uFFFF') ) {
                    	        input.consume();

                    	    }
                    	    else {
                    	        MismatchedSetException mse = new MismatchedSetException(null,input);
                    	        recover(mse);
                    	        throw mse;}


                    	    }
                    	    break;

                    	default :
                    	    break loop23;
                        }
                    } while (true);

                    match('\''); 

                    }
                    break;

            }
            state.type = _type;
            state.channel = _channel;
        }
        finally {
        }
    }
    // $ANTLR end "StringLiteral"

    // $ANTLR start "WS"
    public final void mWS() throws RecognitionException {
        try {
            int _type = WS;
            int _channel = DEFAULT_TOKEN_CHANNEL;
            // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmScript.g:794:2: ( ( ' ' | '\\r' | '\\t' | '\\u000C' | '\\n' ) )
            // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmScript.g:794:5: ( ' ' | '\\r' | '\\t' | '\\u000C' | '\\n' )
            {
            if ( (input.LA(1)>='\t' && input.LA(1)<='\n')||(input.LA(1)>='\f' && input.LA(1)<='\r')||input.LA(1)==' ' ) {
                input.consume();

            }
            else {
                MismatchedSetException mse = new MismatchedSetException(null,input);
                recover(mse);
                throw mse;}

            _channel=HIDDEN;

            }

            state.type = _type;
            state.channel = _channel;
        }
        finally {
        }
    }
    // $ANTLR end "WS"

    // $ANTLR start "COMMENT"
    public final void mCOMMENT() throws RecognitionException {
        try {
            int _type = COMMENT;
            int _channel = DEFAULT_TOKEN_CHANNEL;
            // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmScript.g:797:2: ( '/*' ( options {greedy=false; } : . )* '*/' )
            // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmScript.g:797:4: '/*' ( options {greedy=false; } : . )* '*/'
            {
            match("/*"); 

            // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmScript.g:797:9: ( options {greedy=false; } : . )*
            loop25:
            do {
                int alt25=2;
                int LA25_0 = input.LA(1);

                if ( (LA25_0=='*') ) {
                    int LA25_1 = input.LA(2);

                    if ( (LA25_1=='/') ) {
                        alt25=2;
                    }
                    else if ( ((LA25_1>='\u0000' && LA25_1<='.')||(LA25_1>='0' && LA25_1<='\uFFFF')) ) {
                        alt25=1;
                    }


                }
                else if ( ((LA25_0>='\u0000' && LA25_0<=')')||(LA25_0>='+' && LA25_0<='\uFFFF')) ) {
                    alt25=1;
                }


                switch (alt25) {
            	case 1 :
            	    // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmScript.g:797:37: .
            	    {
            	    matchAny(); 

            	    }
            	    break;

            	default :
            	    break loop25;
                }
            } while (true);

            match("*/"); 

            _channel=HIDDEN;

            }

            state.type = _type;
            state.channel = _channel;
        }
        finally {
        }
    }
    // $ANTLR end "COMMENT"

    // $ANTLR start "LINE_COMMENT"
    public final void mLINE_COMMENT() throws RecognitionException {
        try {
            int _type = LINE_COMMENT;
            int _channel = DEFAULT_TOKEN_CHANNEL;
            // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmScript.g:800:2: ( '//' (~ ( '\\n' | '\\r' ) )* ( '\\r' )? '\\n' )
            // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmScript.g:800:4: '//' (~ ( '\\n' | '\\r' ) )* ( '\\r' )? '\\n'
            {
            match("//"); 

            // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmScript.g:800:9: (~ ( '\\n' | '\\r' ) )*
            loop26:
            do {
                int alt26=2;
                int LA26_0 = input.LA(1);

                if ( ((LA26_0>='\u0000' && LA26_0<='\t')||(LA26_0>='\u000B' && LA26_0<='\f')||(LA26_0>='\u000E' && LA26_0<='\uFFFF')) ) {
                    alt26=1;
                }


                switch (alt26) {
            	case 1 :
            	    // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmScript.g:800:9: ~ ( '\\n' | '\\r' )
            	    {
            	    if ( (input.LA(1)>='\u0000' && input.LA(1)<='\t')||(input.LA(1)>='\u000B' && input.LA(1)<='\f')||(input.LA(1)>='\u000E' && input.LA(1)<='\uFFFF') ) {
            	        input.consume();

            	    }
            	    else {
            	        MismatchedSetException mse = new MismatchedSetException(null,input);
            	        recover(mse);
            	        throw mse;}


            	    }
            	    break;

            	default :
            	    break loop26;
                }
            } while (true);

            // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmScript.g:800:23: ( '\\r' )?
            int alt27=2;
            int LA27_0 = input.LA(1);

            if ( (LA27_0=='\r') ) {
                alt27=1;
            }
            switch (alt27) {
                case 1 :
                    // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmScript.g:800:23: '\\r'
                    {
                    match('\r'); 

                    }
                    break;

            }

            match('\n'); 
            _channel=HIDDEN;

            }

            state.type = _type;
            state.channel = _channel;
        }
        finally {
        }
    }
    // $ANTLR end "LINE_COMMENT"

    public void mTokens() throws RecognitionException {
        // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmScript.g:1:8: ( T__63 | T__64 | T__65 | T__66 | T__67 | T__68 | T__69 | T__70 | T__71 | T__72 | T__73 | T__74 | T__75 | T__76 | T__77 | T__78 | T__79 | T__80 | T__81 | T__82 | T__83 | T__84 | T__85 | T__86 | T__87 | T__88 | T__89 | T__90 | T__91 | T__92 | T__93 | T__94 | T__95 | T__96 | T__97 | T__98 | T__99 | T__100 | T__101 | T__102 | T__103 | T__104 | T__105 | T__106 | T__107 | T__108 | T__109 | T__110 | T__111 | T__112 | T__113 | T__114 | T__115 | T__116 | T__117 | T__118 | T__119 | T__120 | T__121 | T__122 | T__123 | T__124 | T__125 | T__126 | T__127 | T__128 | T__129 | T__130 | T__131 | T__132 | T__133 | T__134 | T__135 | T__136 | T__137 | T__138 | T__139 | T__140 | T__141 | T__142 | T__143 | T__144 | T__145 | T__146 | T__147 | T__148 | T__149 | T__150 | T__151 | T__152 | T__153 | Id | HexLiteral | IntegerLiteral | OctalLiteral | DecimalLiteral | StringLiteral | WS | COMMENT | LINE_COMMENT )
        int alt28=100;
        alt28 = dfa28.predict(input);
        switch (alt28) {
            case 1 :
                // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmScript.g:1:10: T__63
                {
                mT__63(); 

                }
                break;
            case 2 :
                // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmScript.g:1:16: T__64
                {
                mT__64(); 

                }
                break;
            case 3 :
                // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmScript.g:1:22: T__65
                {
                mT__65(); 

                }
                break;
            case 4 :
                // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmScript.g:1:28: T__66
                {
                mT__66(); 

                }
                break;
            case 5 :
                // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmScript.g:1:34: T__67
                {
                mT__67(); 

                }
                break;
            case 6 :
                // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmScript.g:1:40: T__68
                {
                mT__68(); 

                }
                break;
            case 7 :
                // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmScript.g:1:46: T__69
                {
                mT__69(); 

                }
                break;
            case 8 :
                // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmScript.g:1:52: T__70
                {
                mT__70(); 

                }
                break;
            case 9 :
                // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmScript.g:1:58: T__71
                {
                mT__71(); 

                }
                break;
            case 10 :
                // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmScript.g:1:64: T__72
                {
                mT__72(); 

                }
                break;
            case 11 :
                // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmScript.g:1:70: T__73
                {
                mT__73(); 

                }
                break;
            case 12 :
                // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmScript.g:1:76: T__74
                {
                mT__74(); 

                }
                break;
            case 13 :
                // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmScript.g:1:82: T__75
                {
                mT__75(); 

                }
                break;
            case 14 :
                // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmScript.g:1:88: T__76
                {
                mT__76(); 

                }
                break;
            case 15 :
                // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmScript.g:1:94: T__77
                {
                mT__77(); 

                }
                break;
            case 16 :
                // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmScript.g:1:100: T__78
                {
                mT__78(); 

                }
                break;
            case 17 :
                // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmScript.g:1:106: T__79
                {
                mT__79(); 

                }
                break;
            case 18 :
                // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmScript.g:1:112: T__80
                {
                mT__80(); 

                }
                break;
            case 19 :
                // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmScript.g:1:118: T__81
                {
                mT__81(); 

                }
                break;
            case 20 :
                // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmScript.g:1:124: T__82
                {
                mT__82(); 

                }
                break;
            case 21 :
                // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmScript.g:1:130: T__83
                {
                mT__83(); 

                }
                break;
            case 22 :
                // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmScript.g:1:136: T__84
                {
                mT__84(); 

                }
                break;
            case 23 :
                // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmScript.g:1:142: T__85
                {
                mT__85(); 

                }
                break;
            case 24 :
                // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmScript.g:1:148: T__86
                {
                mT__86(); 

                }
                break;
            case 25 :
                // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmScript.g:1:154: T__87
                {
                mT__87(); 

                }
                break;
            case 26 :
                // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmScript.g:1:160: T__88
                {
                mT__88(); 

                }
                break;
            case 27 :
                // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmScript.g:1:166: T__89
                {
                mT__89(); 

                }
                break;
            case 28 :
                // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmScript.g:1:172: T__90
                {
                mT__90(); 

                }
                break;
            case 29 :
                // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmScript.g:1:178: T__91
                {
                mT__91(); 

                }
                break;
            case 30 :
                // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmScript.g:1:184: T__92
                {
                mT__92(); 

                }
                break;
            case 31 :
                // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmScript.g:1:190: T__93
                {
                mT__93(); 

                }
                break;
            case 32 :
                // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmScript.g:1:196: T__94
                {
                mT__94(); 

                }
                break;
            case 33 :
                // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmScript.g:1:202: T__95
                {
                mT__95(); 

                }
                break;
            case 34 :
                // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmScript.g:1:208: T__96
                {
                mT__96(); 

                }
                break;
            case 35 :
                // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmScript.g:1:214: T__97
                {
                mT__97(); 

                }
                break;
            case 36 :
                // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmScript.g:1:220: T__98
                {
                mT__98(); 

                }
                break;
            case 37 :
                // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmScript.g:1:226: T__99
                {
                mT__99(); 

                }
                break;
            case 38 :
                // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmScript.g:1:232: T__100
                {
                mT__100(); 

                }
                break;
            case 39 :
                // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmScript.g:1:239: T__101
                {
                mT__101(); 

                }
                break;
            case 40 :
                // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmScript.g:1:246: T__102
                {
                mT__102(); 

                }
                break;
            case 41 :
                // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmScript.g:1:253: T__103
                {
                mT__103(); 

                }
                break;
            case 42 :
                // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmScript.g:1:260: T__104
                {
                mT__104(); 

                }
                break;
            case 43 :
                // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmScript.g:1:267: T__105
                {
                mT__105(); 

                }
                break;
            case 44 :
                // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmScript.g:1:274: T__106
                {
                mT__106(); 

                }
                break;
            case 45 :
                // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmScript.g:1:281: T__107
                {
                mT__107(); 

                }
                break;
            case 46 :
                // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmScript.g:1:288: T__108
                {
                mT__108(); 

                }
                break;
            case 47 :
                // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmScript.g:1:295: T__109
                {
                mT__109(); 

                }
                break;
            case 48 :
                // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmScript.g:1:302: T__110
                {
                mT__110(); 

                }
                break;
            case 49 :
                // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmScript.g:1:309: T__111
                {
                mT__111(); 

                }
                break;
            case 50 :
                // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmScript.g:1:316: T__112
                {
                mT__112(); 

                }
                break;
            case 51 :
                // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmScript.g:1:323: T__113
                {
                mT__113(); 

                }
                break;
            case 52 :
                // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmScript.g:1:330: T__114
                {
                mT__114(); 

                }
                break;
            case 53 :
                // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmScript.g:1:337: T__115
                {
                mT__115(); 

                }
                break;
            case 54 :
                // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmScript.g:1:344: T__116
                {
                mT__116(); 

                }
                break;
            case 55 :
                // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmScript.g:1:351: T__117
                {
                mT__117(); 

                }
                break;
            case 56 :
                // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmScript.g:1:358: T__118
                {
                mT__118(); 

                }
                break;
            case 57 :
                // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmScript.g:1:365: T__119
                {
                mT__119(); 

                }
                break;
            case 58 :
                // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmScript.g:1:372: T__120
                {
                mT__120(); 

                }
                break;
            case 59 :
                // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmScript.g:1:379: T__121
                {
                mT__121(); 

                }
                break;
            case 60 :
                // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmScript.g:1:386: T__122
                {
                mT__122(); 

                }
                break;
            case 61 :
                // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmScript.g:1:393: T__123
                {
                mT__123(); 

                }
                break;
            case 62 :
                // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmScript.g:1:400: T__124
                {
                mT__124(); 

                }
                break;
            case 63 :
                // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmScript.g:1:407: T__125
                {
                mT__125(); 

                }
                break;
            case 64 :
                // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmScript.g:1:414: T__126
                {
                mT__126(); 

                }
                break;
            case 65 :
                // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmScript.g:1:421: T__127
                {
                mT__127(); 

                }
                break;
            case 66 :
                // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmScript.g:1:428: T__128
                {
                mT__128(); 

                }
                break;
            case 67 :
                // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmScript.g:1:435: T__129
                {
                mT__129(); 

                }
                break;
            case 68 :
                // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmScript.g:1:442: T__130
                {
                mT__130(); 

                }
                break;
            case 69 :
                // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmScript.g:1:449: T__131
                {
                mT__131(); 

                }
                break;
            case 70 :
                // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmScript.g:1:456: T__132
                {
                mT__132(); 

                }
                break;
            case 71 :
                // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmScript.g:1:463: T__133
                {
                mT__133(); 

                }
                break;
            case 72 :
                // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmScript.g:1:470: T__134
                {
                mT__134(); 

                }
                break;
            case 73 :
                // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmScript.g:1:477: T__135
                {
                mT__135(); 

                }
                break;
            case 74 :
                // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmScript.g:1:484: T__136
                {
                mT__136(); 

                }
                break;
            case 75 :
                // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmScript.g:1:491: T__137
                {
                mT__137(); 

                }
                break;
            case 76 :
                // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmScript.g:1:498: T__138
                {
                mT__138(); 

                }
                break;
            case 77 :
                // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmScript.g:1:505: T__139
                {
                mT__139(); 

                }
                break;
            case 78 :
                // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmScript.g:1:512: T__140
                {
                mT__140(); 

                }
                break;
            case 79 :
                // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmScript.g:1:519: T__141
                {
                mT__141(); 

                }
                break;
            case 80 :
                // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmScript.g:1:526: T__142
                {
                mT__142(); 

                }
                break;
            case 81 :
                // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmScript.g:1:533: T__143
                {
                mT__143(); 

                }
                break;
            case 82 :
                // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmScript.g:1:540: T__144
                {
                mT__144(); 

                }
                break;
            case 83 :
                // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmScript.g:1:547: T__145
                {
                mT__145(); 

                }
                break;
            case 84 :
                // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmScript.g:1:554: T__146
                {
                mT__146(); 

                }
                break;
            case 85 :
                // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmScript.g:1:561: T__147
                {
                mT__147(); 

                }
                break;
            case 86 :
                // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmScript.g:1:568: T__148
                {
                mT__148(); 

                }
                break;
            case 87 :
                // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmScript.g:1:575: T__149
                {
                mT__149(); 

                }
                break;
            case 88 :
                // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmScript.g:1:582: T__150
                {
                mT__150(); 

                }
                break;
            case 89 :
                // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmScript.g:1:589: T__151
                {
                mT__151(); 

                }
                break;
            case 90 :
                // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmScript.g:1:596: T__152
                {
                mT__152(); 

                }
                break;
            case 91 :
                // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmScript.g:1:603: T__153
                {
                mT__153(); 

                }
                break;
            case 92 :
                // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmScript.g:1:610: Id
                {
                mId(); 

                }
                break;
            case 93 :
                // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmScript.g:1:613: HexLiteral
                {
                mHexLiteral(); 

                }
                break;
            case 94 :
                // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmScript.g:1:624: IntegerLiteral
                {
                mIntegerLiteral(); 

                }
                break;
            case 95 :
                // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmScript.g:1:639: OctalLiteral
                {
                mOctalLiteral(); 

                }
                break;
            case 96 :
                // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmScript.g:1:652: DecimalLiteral
                {
                mDecimalLiteral(); 

                }
                break;
            case 97 :
                // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmScript.g:1:667: StringLiteral
                {
                mStringLiteral(); 

                }
                break;
            case 98 :
                // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmScript.g:1:681: WS
                {
                mWS(); 

                }
                break;
            case 99 :
                // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmScript.g:1:684: COMMENT
                {
                mCOMMENT(); 

                }
                break;
            case 100 :
                // D:\\MetraTech\\SourceCode\\MvmSourceCode\\MVM_DLL\\mvm_script\\MvmScript.g:1:692: LINE_COMMENT
                {
                mLINE_COMMENT(); 

                }
                break;

        }

    }


    protected DFA19 dfa19 = new DFA19(this);
    protected DFA28 dfa28 = new DFA28(this);
    static final String DFA19_eotS =
        "\6\uffff";
    static final String DFA19_eofS =
        "\6\uffff";
    static final String DFA19_minS =
        "\2\56\4\uffff";
    static final String DFA19_maxS =
        "\1\71\1\146\4\uffff";
    static final String DFA19_acceptS =
        "\2\uffff\1\2\1\4\1\3\1\1";
    static final String DFA19_specialS =
        "\6\uffff}>";
    static final String[] DFA19_transitionS = {
            "\1\2\1\uffff\12\1",
            "\1\5\1\uffff\12\1\12\uffff\1\3\1\4\1\3\35\uffff\1\3\1\4\1"+
            "\3",
            "",
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
            return "775:1: DecimalLiteral : ( ( '0' .. '9' )+ '.' ( '0' .. '9' )* ( Exponent )? ( FloatTypeSuffix )? | '.' ( '0' .. '9' )+ ( Exponent )? ( FloatTypeSuffix )? | ( '0' .. '9' )+ Exponent ( FloatTypeSuffix )? | ( '0' .. '9' )+ FloatTypeSuffix );";
        }
    }
    static final String DFA28_eotS =
        "\1\uffff\1\64\2\uffff\1\67\1\73\1\75\1\101\1\104\1\107\1\111\1"+
        "\113\1\115\1\120\1\123\2\uffff\4\55\1\131\10\55\2\uffff\1\151\4"+
        "\uffff\10\55\1\uffff\2\171\36\uffff\1\174\2\uffff\1\176\2\uffff"+
        "\1\177\1\u0080\2\55\2\uffff\1\u0084\1\55\1\u0088\1\55\1\u008b\1"+
        "\u008c\1\u008e\1\u008f\1\55\1\u0092\1\u0094\1\u0096\1\u0098\1\u009a"+
        "\1\u009c\2\uffff\1\u009d\1\u009e\1\55\1\u00a0\10\55\1\uffff\1\u00aa"+
        "\1\uffff\1\171\6\uffff\1\u00ab\1\u00ac\1\55\1\uffff\2\55\1\u00b0"+
        "\1\uffff\2\55\2\uffff\1\55\2\uffff\1\55\1\u00b5\1\uffff\1\u00b6"+
        "\1\uffff\1\u00b7\1\uffff\1\u00b8\1\uffff\1\u00b9\1\uffff\1\u00ba"+
        "\3\uffff\1\55\1\uffff\1\u00bd\6\55\1\u00c4\1\55\3\uffff\1\u00c6"+
        "\1\u00c7\1\u00c8\1\uffff\1\u00c9\1\u00ca\1\u00cb\1\u00cc\6\uffff"+
        "\2\55\1\uffff\6\55\1\uffff\1\u00d5\7\uffff\1\u00d6\2\55\1\u00d9"+
        "\1\55\1\u00db\1\u00dc\1\55\2\uffff\2\55\1\uffff\1\55\2\uffff\1\u00e1"+
        "\1\u00e2\1\u00e3\1\55\3\uffff\1\u00e5\1\uffff";
    static final String DFA28_eofS =
        "\u00e6\uffff";
    static final String DFA28_minS =
        "\1\11\1\75\2\uffff\1\53\1\55\1\75\1\52\1\46\4\75\1\74\1\75\2\uffff"+
        "\1\162\1\122\1\156\1\116\1\75\1\154\1\145\1\121\1\105\2\164\2\124"+
        "\2\uffff\1\60\4\uffff\1\146\1\150\1\157\2\141\1\162\1\145\1\162"+
        "\1\uffff\2\56\36\uffff\1\75\2\uffff\1\75\2\uffff\2\60\1\144\1\104"+
        "\2\uffff\1\60\1\163\1\60\1\154\4\60\1\114\6\60\2\uffff\2\60\1\151"+
        "\1\60\1\162\1\156\1\154\1\156\1\164\1\145\1\164\1\165\1\uffff\1"+
        "\56\1\uffff\1\56\6\uffff\2\60\1\121\1\uffff\1\145\1\105\1\60\1\uffff"+
        "\1\154\1\121\2\uffff\1\105\2\uffff\1\114\1\60\1\uffff\1\60\1\uffff"+
        "\1\60\1\uffff\1\60\1\uffff\1\60\1\uffff\1\60\3\uffff\1\154\1\uffff"+
        "\1\60\1\141\1\163\1\164\1\143\1\141\1\165\1\60\1\145\3\uffff\3\60"+
        "\1\uffff\4\60\6\uffff\1\145\1\141\1\uffff\1\154\1\145\1\151\1\150"+
        "\1\153\1\162\1\uffff\1\60\7\uffff\1\60\1\143\1\154\1\60\1\156\2"+
        "\60\1\156\2\uffff\1\150\1\171\1\uffff\1\165\2\uffff\3\60\1\145\3"+
        "\uffff\1\60\1\uffff";
    static final String DFA28_maxS =
        "\1\176\1\76\2\uffff\1\75\1\76\3\75\1\174\4\75\1\76\2\uffff\1\162"+
        "\1\122\1\156\1\116\1\75\1\161\1\165\1\161\1\145\4\164\2\uffff\1"+
        "\71\4\uffff\1\156\1\150\3\157\1\162\1\145\1\162\1\uffff\1\170\1"+
        "\146\36\uffff\1\75\2\uffff\1\75\2\uffff\2\172\1\144\1\104\2\uffff"+
        "\1\172\1\163\1\172\1\154\4\172\1\114\6\172\2\uffff\2\172\1\151\1"+
        "\172\1\162\1\156\1\154\1\156\1\164\1\145\1\164\1\171\1\uffff\1\146"+
        "\1\uffff\1\146\6\uffff\2\172\1\121\1\uffff\1\145\1\105\1\172\1\uffff"+
        "\1\154\1\121\2\uffff\1\105\2\uffff\1\114\1\172\1\uffff\1\172\1\uffff"+
        "\1\172\1\uffff\1\172\1\uffff\1\172\1\uffff\1\172\3\uffff\1\154\1"+
        "\uffff\1\172\1\141\1\163\1\164\1\143\1\141\1\165\1\172\1\145\3\uffff"+
        "\3\172\1\uffff\4\172\6\uffff\1\145\1\141\1\uffff\1\154\1\145\1\151"+
        "\1\150\1\153\1\162\1\uffff\1\172\7\uffff\1\172\1\143\1\154\1\172"+
        "\1\156\2\172\1\156\2\uffff\1\150\1\171\1\uffff\1\165\2\uffff\3\172"+
        "\1\145\3\uffff\1\172\1\uffff";
    static final String DFA28_acceptS =
        "\2\uffff\1\2\1\3\13\uffff\1\20\1\21\15\uffff\1\103\1\104\1\uffff"+
        "\1\106\1\107\1\110\1\112\10\uffff\1\134\2\uffff\1\141\1\142\1\1"+
        "\1\33\1\4\1\5\1\77\1\71\1\6\1\76\1\100\1\72\1\7\1\73\1\10\1\143"+
        "\1\144\1\74\1\11\1\25\1\32\1\12\1\22\1\30\1\13\1\31\1\14\1\75\1"+
        "\15\1\101\1\uffff\1\47\1\51\1\uffff\1\50\1\52\4\uffff\1\34\1\102"+
        "\17\uffff\1\105\1\140\14\uffff\1\135\1\uffff\1\136\1\uffff\1\16"+
        "\1\67\1\17\1\70\1\23\1\24\3\uffff\1\35\3\uffff\1\36\2\uffff\1\37"+
        "\1\41\1\uffff\1\40\1\42\2\uffff\1\53\1\uffff\1\54\1\uffff\1\57\1"+
        "\uffff\1\63\1\uffff\1\60\1\uffff\1\64\1\113\1\121\1\uffff\1\116"+
        "\11\uffff\1\137\1\26\1\27\3\uffff\1\111\4\uffff\1\55\1\56\1\61\1"+
        "\65\1\62\1\66\2\uffff\1\117\6\uffff\1\125\1\uffff\1\43\1\114\1\45"+
        "\1\132\1\44\1\46\1\133\10\uffff\1\130\1\115\2\uffff\1\131\1\uffff"+
        "\1\126\1\123\4\uffff\1\124\1\120\1\127\1\uffff\1\122";
    static final String DFA28_specialS =
        "\u00e6\uffff}>";
    static final String[] DFA28_transitionS = {
            "\2\61\1\uffff\2\61\22\uffff\1\61\1\25\1\60\2\uffff\1\13\1\10"+
            "\1\60\1\36\1\37\1\6\1\4\1\43\1\5\1\40\1\7\1\56\11\57\1\20\1"+
            "\44\1\15\1\1\1\16\1\17\1\uffff\1\24\3\55\1\30\1\55\1\34\4\55"+
            "\1\35\1\55\1\31\1\22\13\55\1\41\1\uffff\1\42\1\12\1\55\1\uffff"+
            "\1\23\1\52\1\51\1\47\1\26\1\50\1\32\1\55\1\45\2\55\1\33\1\55"+
            "\1\27\1\21\2\55\1\53\1\55\1\54\2\55\1\46\3\55\1\2\1\11\1\3\1"+
            "\14",
            "\1\63\1\62",
            "",
            "",
            "\1\66\21\uffff\1\65",
            "\1\72\17\uffff\1\70\1\71",
            "\1\74",
            "\1\77\4\uffff\1\100\15\uffff\1\76",
            "\1\103\26\uffff\1\102",
            "\1\105\76\uffff\1\106",
            "\1\110",
            "\1\112",
            "\1\114",
            "\1\116\1\117",
            "\1\122\1\121",
            "",
            "",
            "\1\124",
            "\1\125",
            "\1\126",
            "\1\127",
            "\1\130",
            "\1\133\4\uffff\1\132",
            "\1\134\17\uffff\1\135",
            "\1\137\37\uffff\1\136",
            "\1\141\17\uffff\1\142\17\uffff\1\140",
            "\1\143",
            "\1\144",
            "\1\146\37\uffff\1\145",
            "\1\150\37\uffff\1\147",
            "",
            "",
            "\12\152",
            "",
            "",
            "",
            "",
            "\1\153\7\uffff\1\154",
            "\1\155",
            "\1\156",
            "\1\161\7\uffff\1\160\5\uffff\1\157",
            "\1\163\15\uffff\1\162",
            "\1\164",
            "\1\165",
            "\1\166",
            "",
            "\1\152\1\uffff\10\170\2\152\12\uffff\3\152\21\uffff\1\167"+
            "\13\uffff\3\152\21\uffff\1\167",
            "\1\152\1\uffff\12\172\12\uffff\3\152\35\uffff\3\152",
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
            "",
            "",
            "",
            "\1\173",
            "",
            "",
            "\1\175",
            "",
            "",
            "\12\55\7\uffff\32\55\4\uffff\1\55\1\uffff\32\55",
            "\12\55\7\uffff\32\55\4\uffff\1\55\1\uffff\32\55",
            "\1\u0081",
            "\1\u0082",
            "",
            "",
            "\12\55\7\uffff\4\55\1\u0083\25\55\4\uffff\1\55\1\uffff\32"+
            "\55",
            "\1\u0085",
            "\12\55\7\uffff\15\55\1\u0086\14\55\4\uffff\1\55\1\uffff\26"+
            "\55\1\u0087\3\55",
            "\1\u0089",
            "\12\55\7\uffff\4\55\1\u008a\25\55\4\uffff\1\55\1\uffff\32"+
            "\55",
            "\12\55\7\uffff\32\55\4\uffff\1\55\1\uffff\32\55",
            "\12\55\7\uffff\15\55\1\u008d\14\55\4\uffff\1\55\1\uffff\32"+
            "\55",
            "\12\55\7\uffff\32\55\4\uffff\1\55\1\uffff\32\55",
            "\1\u0090",
            "\12\55\7\uffff\32\55\4\uffff\1\55\1\uffff\4\55\1\u0091\25"+
            "\55",
            "\12\55\7\uffff\32\55\4\uffff\1\55\1\uffff\4\55\1\u0093\25"+
            "\55",
            "\12\55\7\uffff\32\55\4\uffff\1\55\1\uffff\4\55\1\u0095\25"+
            "\55",
            "\12\55\7\uffff\4\55\1\u0097\25\55\4\uffff\1\55\1\uffff\32"+
            "\55",
            "\12\55\7\uffff\32\55\4\uffff\1\55\1\uffff\4\55\1\u0099\25"+
            "\55",
            "\12\55\7\uffff\4\55\1\u009b\25\55\4\uffff\1\55\1\uffff\32"+
            "\55",
            "",
            "",
            "\12\55\7\uffff\32\55\4\uffff\1\55\1\uffff\32\55",
            "\12\55\7\uffff\32\55\4\uffff\1\55\1\uffff\32\55",
            "\1\u009f",
            "\12\55\7\uffff\32\55\4\uffff\1\55\1\uffff\32\55",
            "\1\u00a1",
            "\1\u00a2",
            "\1\u00a3",
            "\1\u00a4",
            "\1\u00a5",
            "\1\u00a6",
            "\1\u00a7",
            "\1\u00a9\3\uffff\1\u00a8",
            "",
            "\1\152\1\uffff\10\170\2\152\12\uffff\3\152\35\uffff\3\152",
            "",
            "\1\152\1\uffff\12\172\12\uffff\3\152\35\uffff\3\152",
            "",
            "",
            "",
            "",
            "",
            "",
            "\12\55\7\uffff\32\55\4\uffff\1\55\1\uffff\32\55",
            "\12\55\7\uffff\32\55\4\uffff\1\55\1\uffff\32\55",
            "\1\u00ad",
            "",
            "\1\u00ae",
            "\1\u00af",
            "\12\55\7\uffff\32\55\4\uffff\1\55\1\uffff\32\55",
            "",
            "\1\u00b1",
            "\1\u00b2",
            "",
            "",
            "\1\u00b3",
            "",
            "",
            "\1\u00b4",
            "\12\55\7\uffff\32\55\4\uffff\1\55\1\uffff\32\55",
            "",
            "\12\55\7\uffff\32\55\4\uffff\1\55\1\uffff\32\55",
            "",
            "\12\55\7\uffff\32\55\4\uffff\1\55\1\uffff\32\55",
            "",
            "\12\55\7\uffff\32\55\4\uffff\1\55\1\uffff\32\55",
            "",
            "\12\55\7\uffff\32\55\4\uffff\1\55\1\uffff\32\55",
            "",
            "\12\55\7\uffff\32\55\4\uffff\1\55\1\uffff\32\55",
            "",
            "",
            "",
            "\1\u00bb",
            "",
            "\12\55\7\uffff\32\55\4\uffff\1\55\1\uffff\4\55\1\u00bc\25"+
            "\55",
            "\1\u00be",
            "\1\u00bf",
            "\1\u00c0",
            "\1\u00c1",
            "\1\u00c2",
            "\1\u00c3",
            "\12\55\7\uffff\32\55\4\uffff\1\55\1\uffff\32\55",
            "\1\u00c5",
            "",
            "",
            "",
            "\12\55\7\uffff\32\55\4\uffff\1\55\1\uffff\32\55",
            "\12\55\7\uffff\32\55\4\uffff\1\55\1\uffff\32\55",
            "\12\55\7\uffff\32\55\4\uffff\1\55\1\uffff\32\55",
            "",
            "\12\55\7\uffff\32\55\4\uffff\1\55\1\uffff\32\55",
            "\12\55\7\uffff\32\55\4\uffff\1\55\1\uffff\32\55",
            "\12\55\7\uffff\32\55\4\uffff\1\55\1\uffff\32\55",
            "\12\55\7\uffff\32\55\4\uffff\1\55\1\uffff\32\55",
            "",
            "",
            "",
            "",
            "",
            "",
            "\1\u00cd",
            "\1\u00ce",
            "",
            "\1\u00cf",
            "\1\u00d0",
            "\1\u00d1",
            "\1\u00d2",
            "\1\u00d3",
            "\1\u00d4",
            "",
            "\12\55\7\uffff\32\55\4\uffff\1\55\1\uffff\32\55",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "\12\55\7\uffff\32\55\4\uffff\1\55\1\uffff\32\55",
            "\1\u00d7",
            "\1\u00d8",
            "\12\55\7\uffff\32\55\4\uffff\1\55\1\uffff\32\55",
            "\1\u00da",
            "\12\55\7\uffff\32\55\4\uffff\1\55\1\uffff\32\55",
            "\12\55\7\uffff\32\55\4\uffff\1\55\1\uffff\32\55",
            "\1\u00dd",
            "",
            "",
            "\1\u00de",
            "\1\u00df",
            "",
            "\1\u00e0",
            "",
            "",
            "\12\55\7\uffff\32\55\4\uffff\1\55\1\uffff\32\55",
            "\12\55\7\uffff\32\55\4\uffff\1\55\1\uffff\32\55",
            "\12\55\7\uffff\32\55\4\uffff\1\55\1\uffff\32\55",
            "\1\u00e4",
            "",
            "",
            "",
            "\12\55\7\uffff\32\55\4\uffff\1\55\1\uffff\32\55",
            ""
    };

    static final short[] DFA28_eot = DFA.unpackEncodedString(DFA28_eotS);
    static final short[] DFA28_eof = DFA.unpackEncodedString(DFA28_eofS);
    static final char[] DFA28_min = DFA.unpackEncodedStringToUnsignedChars(DFA28_minS);
    static final char[] DFA28_max = DFA.unpackEncodedStringToUnsignedChars(DFA28_maxS);
    static final short[] DFA28_accept = DFA.unpackEncodedString(DFA28_acceptS);
    static final short[] DFA28_special = DFA.unpackEncodedString(DFA28_specialS);
    static final short[][] DFA28_transition;

    static {
        int numStates = DFA28_transitionS.length;
        DFA28_transition = new short[numStates][];
        for (int i=0; i<numStates; i++) {
            DFA28_transition[i] = DFA.unpackEncodedString(DFA28_transitionS[i]);
        }
    }

    class DFA28 extends DFA {

        public DFA28(BaseRecognizer recognizer) {
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
        public String getDescription() {
            return "1:1: Tokens : ( T__63 | T__64 | T__65 | T__66 | T__67 | T__68 | T__69 | T__70 | T__71 | T__72 | T__73 | T__74 | T__75 | T__76 | T__77 | T__78 | T__79 | T__80 | T__81 | T__82 | T__83 | T__84 | T__85 | T__86 | T__87 | T__88 | T__89 | T__90 | T__91 | T__92 | T__93 | T__94 | T__95 | T__96 | T__97 | T__98 | T__99 | T__100 | T__101 | T__102 | T__103 | T__104 | T__105 | T__106 | T__107 | T__108 | T__109 | T__110 | T__111 | T__112 | T__113 | T__114 | T__115 | T__116 | T__117 | T__118 | T__119 | T__120 | T__121 | T__122 | T__123 | T__124 | T__125 | T__126 | T__127 | T__128 | T__129 | T__130 | T__131 | T__132 | T__133 | T__134 | T__135 | T__136 | T__137 | T__138 | T__139 | T__140 | T__141 | T__142 | T__143 | T__144 | T__145 | T__146 | T__147 | T__148 | T__149 | T__150 | T__151 | T__152 | T__153 | Id | HexLiteral | IntegerLiteral | OctalLiteral | DecimalLiteral | StringLiteral | WS | COMMENT | LINE_COMMENT );";
        }
    }
 

}