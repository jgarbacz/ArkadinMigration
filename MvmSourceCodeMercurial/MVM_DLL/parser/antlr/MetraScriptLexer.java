// $ANTLR 3.1.2 C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g 2009-04-29 17:00:07

import org.antlr.runtime.*;
import java.util.Stack;
import java.util.List;
import java.util.ArrayList;

public class MetraScriptLexer extends Lexer {
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
    public static final int T__60=60;
    public static final int EOF=-1;
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
    public static final int T__44=44;
    public static final int T__82=82;
    public static final int T__45=45;
    public static final int T__83=83;
    public static final int LINE_COMMENT=30;
    public static final int IntegerTypeSuffix=22;
    public static final int T__48=48;
    public static final int T__49=49;
    public static final int TEMP=11;
    public static final int NULL=8;
    public static final int BOOL=7;
    public static final int INT=4;
    public static final int T__85=85;
    public static final int T__84=84;
    public static final int DecimalLiteral=20;
    public static final int StringLiteral=17;
    public static final int T__31=31;
    public static final int T__32=32;
    public static final int T__33=33;
    public static final int T__71=71;
    public static final int WS=28;
    public static final int T__34=34;
    public static final int T__72=72;
    public static final int T__35=35;
    public static final int T__70=70;
    public static final int T__36=36;
    public static final int T__37=37;
    public static final int T__38=38;
    public static final int T__39=39;
    public static final int UnicodeEscape=26;
    public static final int FloatingPointLiteral=16;
    public static final int T__76=76;
    public static final int GLOBAL=12;
    public static final int T__75=75;
    public static final int T__74=74;
    public static final int T__73=73;
    public static final int EscapeSequence=25;
    public static final int OctalEscape=27;
    public static final int T__79=79;
    public static final int T__78=78;
    public static final int STRING=6;
    public static final int T__77=77;

    // delegates
    // delegators

    public MetraScriptLexer() {;} 
    public MetraScriptLexer(CharStream input) {
        this(input, new RecognizerSharedState());
    }
    public MetraScriptLexer(CharStream input, RecognizerSharedState state) {
        super(input,state);

    }
    public String getGrammarFileName() { return "C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g"; }

    // $ANTLR start "T__31"
    public final void mT__31() throws RecognitionException {
        try {
            int _type = T__31;
            int _channel = DEFAULT_TOKEN_CHANNEL;
            // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:3:7: ( 'OBJECT.' )
            // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:3:9: 'OBJECT.'
            {
            match("OBJECT."); 


            }

            state.type = _type;
            state.channel = _channel;
        }
        finally {
        }
    }
    // $ANTLR end "T__31"

    // $ANTLR start "T__32"
    public final void mT__32() throws RecognitionException {
        try {
            int _type = T__32;
            int _channel = DEFAULT_TOKEN_CHANNEL;
            // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:4:7: ( 'OBJECT(' )
            // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:4:9: 'OBJECT('
            {
            match("OBJECT("); 


            }

            state.type = _type;
            state.channel = _channel;
        }
        finally {
        }
    }
    // $ANTLR end "T__32"

    // $ANTLR start "T__33"
    public final void mT__33() throws RecognitionException {
        try {
            int _type = T__33;
            int _channel = DEFAULT_TOKEN_CHANNEL;
            // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:5:7: ( ').' )
            // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:5:9: ').'
            {
            match(")."); 


            }

            state.type = _type;
            state.channel = _channel;
        }
        finally {
        }
    }
    // $ANTLR end "T__33"

    // $ANTLR start "T__34"
    public final void mT__34() throws RecognitionException {
        try {
            int _type = T__34;
            int _channel = DEFAULT_TOKEN_CHANNEL;
            // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:6:7: ( 'TEMP.' )
            // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:6:9: 'TEMP.'
            {
            match("TEMP."); 


            }

            state.type = _type;
            state.channel = _channel;
        }
        finally {
        }
    }
    // $ANTLR end "T__34"

    // $ANTLR start "T__35"
    public final void mT__35() throws RecognitionException {
        try {
            int _type = T__35;
            int _channel = DEFAULT_TOKEN_CHANNEL;
            // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:7:7: ( 'GLOBAL.' )
            // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:7:9: 'GLOBAL.'
            {
            match("GLOBAL."); 


            }

            state.type = _type;
            state.channel = _channel;
        }
        finally {
        }
    }
    // $ANTLR end "T__35"

    // $ANTLR start "T__36"
    public final void mT__36() throws RecognitionException {
        try {
            int _type = T__36;
            int _channel = DEFAULT_TOKEN_CHANNEL;
            // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:8:7: ( 'THREAD.' )
            // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:8:9: 'THREAD.'
            {
            match("THREAD."); 


            }

            state.type = _type;
            state.channel = _channel;
        }
        finally {
        }
    }
    // $ANTLR end "T__36"

    // $ANTLR start "T__37"
    public final void mT__37() throws RecognitionException {
        try {
            int _type = T__37;
            int _channel = DEFAULT_TOKEN_CHANNEL;
            // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:9:7: ( 'PROC.' )
            // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:9:9: 'PROC.'
            {
            match("PROC."); 


            }

            state.type = _type;
            state.channel = _channel;
        }
        finally {
        }
    }
    // $ANTLR end "T__37"

    // $ANTLR start "T__38"
    public final void mT__38() throws RecognitionException {
        try {
            int _type = T__38;
            int _channel = DEFAULT_TOKEN_CHANNEL;
            // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:10:7: ( 'null' )
            // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:10:9: 'null'
            {
            match("null"); 


            }

            state.type = _type;
            state.channel = _channel;
        }
        finally {
        }
    }
    // $ANTLR end "T__38"

    // $ANTLR start "T__39"
    public final void mT__39() throws RecognitionException {
        try {
            int _type = T__39;
            int _channel = DEFAULT_TOKEN_CHANNEL;
            // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:11:7: ( 'true' )
            // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:11:9: 'true'
            {
            match("true"); 


            }

            state.type = _type;
            state.channel = _channel;
        }
        finally {
        }
    }
    // $ANTLR end "T__39"

    // $ANTLR start "T__40"
    public final void mT__40() throws RecognitionException {
        try {
            int _type = T__40;
            int _channel = DEFAULT_TOKEN_CHANNEL;
            // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:12:7: ( 'false' )
            // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:12:9: 'false'
            {
            match("false"); 


            }

            state.type = _type;
            state.channel = _channel;
        }
        finally {
        }
    }
    // $ANTLR end "T__40"

    // $ANTLR start "T__41"
    public final void mT__41() throws RecognitionException {
        try {
            int _type = T__41;
            int _channel = DEFAULT_TOKEN_CHANNEL;
            // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:13:7: ( '=' )
            // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:13:9: '='
            {
            match('='); 

            }

            state.type = _type;
            state.channel = _channel;
        }
        finally {
        }
    }
    // $ANTLR end "T__41"

    // $ANTLR start "T__42"
    public final void mT__42() throws RecognitionException {
        try {
            int _type = T__42;
            int _channel = DEFAULT_TOKEN_CHANNEL;
            // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:14:7: ( '~' )
            // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:14:9: '~'
            {
            match('~'); 

            }

            state.type = _type;
            state.channel = _channel;
        }
        finally {
        }
    }
    // $ANTLR end "T__42"

    // $ANTLR start "T__43"
    public final void mT__43() throws RecognitionException {
        try {
            int _type = T__43;
            int _channel = DEFAULT_TOKEN_CHANNEL;
            // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:15:7: ( '+=' )
            // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:15:9: '+='
            {
            match("+="); 


            }

            state.type = _type;
            state.channel = _channel;
        }
        finally {
        }
    }
    // $ANTLR end "T__43"

    // $ANTLR start "T__44"
    public final void mT__44() throws RecognitionException {
        try {
            int _type = T__44;
            int _channel = DEFAULT_TOKEN_CHANNEL;
            // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:16:7: ( '-=' )
            // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:16:9: '-='
            {
            match("-="); 


            }

            state.type = _type;
            state.channel = _channel;
        }
        finally {
        }
    }
    // $ANTLR end "T__44"

    // $ANTLR start "T__45"
    public final void mT__45() throws RecognitionException {
        try {
            int _type = T__45;
            int _channel = DEFAULT_TOKEN_CHANNEL;
            // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:17:7: ( '*=' )
            // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:17:9: '*='
            {
            match("*="); 


            }

            state.type = _type;
            state.channel = _channel;
        }
        finally {
        }
    }
    // $ANTLR end "T__45"

    // $ANTLR start "T__46"
    public final void mT__46() throws RecognitionException {
        try {
            int _type = T__46;
            int _channel = DEFAULT_TOKEN_CHANNEL;
            // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:18:7: ( '/=' )
            // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:18:9: '/='
            {
            match("/="); 


            }

            state.type = _type;
            state.channel = _channel;
        }
        finally {
        }
    }
    // $ANTLR end "T__46"

    // $ANTLR start "T__47"
    public final void mT__47() throws RecognitionException {
        try {
            int _type = T__47;
            int _channel = DEFAULT_TOKEN_CHANNEL;
            // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:19:7: ( '&=' )
            // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:19:9: '&='
            {
            match("&="); 


            }

            state.type = _type;
            state.channel = _channel;
        }
        finally {
        }
    }
    // $ANTLR end "T__47"

    // $ANTLR start "T__48"
    public final void mT__48() throws RecognitionException {
        try {
            int _type = T__48;
            int _channel = DEFAULT_TOKEN_CHANNEL;
            // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:20:7: ( '|=' )
            // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:20:9: '|='
            {
            match("|="); 


            }

            state.type = _type;
            state.channel = _channel;
        }
        finally {
        }
    }
    // $ANTLR end "T__48"

    // $ANTLR start "T__49"
    public final void mT__49() throws RecognitionException {
        try {
            int _type = T__49;
            int _channel = DEFAULT_TOKEN_CHANNEL;
            // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:21:7: ( '^=' )
            // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:21:9: '^='
            {
            match("^="); 


            }

            state.type = _type;
            state.channel = _channel;
        }
        finally {
        }
    }
    // $ANTLR end "T__49"

    // $ANTLR start "T__50"
    public final void mT__50() throws RecognitionException {
        try {
            int _type = T__50;
            int _channel = DEFAULT_TOKEN_CHANNEL;
            // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:22:7: ( '%=' )
            // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:22:9: '%='
            {
            match("%="); 


            }

            state.type = _type;
            state.channel = _channel;
        }
        finally {
        }
    }
    // $ANTLR end "T__50"

    // $ANTLR start "T__51"
    public final void mT__51() throws RecognitionException {
        try {
            int _type = T__51;
            int _channel = DEFAULT_TOKEN_CHANNEL;
            // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:23:7: ( '~=' )
            // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:23:9: '~='
            {
            match("~="); 


            }

            state.type = _type;
            state.channel = _channel;
        }
        finally {
        }
    }
    // $ANTLR end "T__51"

    // $ANTLR start "T__52"
    public final void mT__52() throws RecognitionException {
        try {
            int _type = T__52;
            int _channel = DEFAULT_TOKEN_CHANNEL;
            // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:24:7: ( '<' )
            // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:24:9: '<'
            {
            match('<'); 

            }

            state.type = _type;
            state.channel = _channel;
        }
        finally {
        }
    }
    // $ANTLR end "T__52"

    // $ANTLR start "T__53"
    public final void mT__53() throws RecognitionException {
        try {
            int _type = T__53;
            int _channel = DEFAULT_TOKEN_CHANNEL;
            // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:25:7: ( '>' )
            // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:25:9: '>'
            {
            match('>'); 

            }

            state.type = _type;
            state.channel = _channel;
        }
        finally {
        }
    }
    // $ANTLR end "T__53"

    // $ANTLR start "T__54"
    public final void mT__54() throws RecognitionException {
        try {
            int _type = T__54;
            int _channel = DEFAULT_TOKEN_CHANNEL;
            // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:26:7: ( '?' )
            // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:26:9: '?'
            {
            match('?'); 

            }

            state.type = _type;
            state.channel = _channel;
        }
        finally {
        }
    }
    // $ANTLR end "T__54"

    // $ANTLR start "T__55"
    public final void mT__55() throws RecognitionException {
        try {
            int _type = T__55;
            int _channel = DEFAULT_TOKEN_CHANNEL;
            // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:27:7: ( ':' )
            // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:27:9: ':'
            {
            match(':'); 

            }

            state.type = _type;
            state.channel = _channel;
        }
        finally {
        }
    }
    // $ANTLR end "T__55"

    // $ANTLR start "T__56"
    public final void mT__56() throws RecognitionException {
        try {
            int _type = T__56;
            int _channel = DEFAULT_TOKEN_CHANNEL;
            // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:28:7: ( '||' )
            // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:28:9: '||'
            {
            match("||"); 


            }

            state.type = _type;
            state.channel = _channel;
        }
        finally {
        }
    }
    // $ANTLR end "T__56"

    // $ANTLR start "T__57"
    public final void mT__57() throws RecognitionException {
        try {
            int _type = T__57;
            int _channel = DEFAULT_TOKEN_CHANNEL;
            // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:29:7: ( 'or' )
            // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:29:9: 'or'
            {
            match("or"); 


            }

            state.type = _type;
            state.channel = _channel;
        }
        finally {
        }
    }
    // $ANTLR end "T__57"

    // $ANTLR start "T__58"
    public final void mT__58() throws RecognitionException {
        try {
            int _type = T__58;
            int _channel = DEFAULT_TOKEN_CHANNEL;
            // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:30:7: ( '&&' )
            // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:30:9: '&&'
            {
            match("&&"); 


            }

            state.type = _type;
            state.channel = _channel;
        }
        finally {
        }
    }
    // $ANTLR end "T__58"

    // $ANTLR start "T__59"
    public final void mT__59() throws RecognitionException {
        try {
            int _type = T__59;
            int _channel = DEFAULT_TOKEN_CHANNEL;
            // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:31:7: ( 'and' )
            // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:31:9: 'and'
            {
            match("and"); 


            }

            state.type = _type;
            state.channel = _channel;
        }
        finally {
        }
    }
    // $ANTLR end "T__59"

    // $ANTLR start "T__60"
    public final void mT__60() throws RecognitionException {
        try {
            int _type = T__60;
            int _channel = DEFAULT_TOKEN_CHANNEL;
            // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:32:7: ( '|' )
            // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:32:9: '|'
            {
            match('|'); 

            }

            state.type = _type;
            state.channel = _channel;
        }
        finally {
        }
    }
    // $ANTLR end "T__60"

    // $ANTLR start "T__61"
    public final void mT__61() throws RecognitionException {
        try {
            int _type = T__61;
            int _channel = DEFAULT_TOKEN_CHANNEL;
            // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:33:7: ( '^' )
            // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:33:9: '^'
            {
            match('^'); 

            }

            state.type = _type;
            state.channel = _channel;
        }
        finally {
        }
    }
    // $ANTLR end "T__61"

    // $ANTLR start "T__62"
    public final void mT__62() throws RecognitionException {
        try {
            int _type = T__62;
            int _channel = DEFAULT_TOKEN_CHANNEL;
            // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:34:7: ( '&' )
            // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:34:9: '&'
            {
            match('&'); 

            }

            state.type = _type;
            state.channel = _channel;
        }
        finally {
        }
    }
    // $ANTLR end "T__62"

    // $ANTLR start "T__63"
    public final void mT__63() throws RecognitionException {
        try {
            int _type = T__63;
            int _channel = DEFAULT_TOKEN_CHANNEL;
            // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:35:7: ( '==' )
            // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:35:9: '=='
            {
            match("=="); 


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
            // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:36:7: ( '!=' )
            // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:36:9: '!='
            {
            match("!="); 


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
            // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:37:7: ( 'eq' )
            // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:37:9: 'eq'
            {
            match("eq"); 


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
            // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:38:7: ( 'ne' )
            // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:38:9: 'ne'
            {
            match("ne"); 


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
            // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:39:7: ( 'gt' )
            // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:39:9: 'gt'
            {
            match("gt"); 


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
            // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:40:7: ( 'lt' )
            // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:40:9: 'lt'
            {
            match("lt"); 


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
            // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:41:7: ( 'gte' )
            // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:41:9: 'gte'
            {
            match("gte"); 


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
            // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:42:7: ( 'lte' )
            // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:42:9: 'lte'
            {
            match("lte"); 


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
            // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:43:7: ( 'GT' )
            // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:43:9: 'GT'
            {
            match("GT"); 


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
            // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:44:7: ( 'LT' )
            // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:44:9: 'LT'
            {
            match("LT"); 


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
            // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:45:7: ( 'EQ' )
            // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:45:9: 'EQ'
            {
            match("EQ"); 


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
            // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:46:7: ( 'GTE' )
            // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:46:9: 'GTE'
            {
            match("GTE"); 


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
            // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:47:7: ( 'LTE' )
            // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:47:9: 'LTE'
            {
            match("LTE"); 


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
            // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:48:7: ( '+' )
            // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:48:9: '+'
            {
            match('+'); 

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
            // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:49:7: ( '-' )
            // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:49:9: '-'
            {
            match('-'); 

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
            // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:50:7: ( '*' )
            // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:50:9: '*'
            {
            match('*'); 

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
            // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:51:7: ( '/' )
            // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:51:9: '/'
            {
            match('/'); 

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
            // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:52:7: ( '%' )
            // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:52:9: '%'
            {
            match('%'); 

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
            // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:53:7: ( '++' )
            // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:53:9: '++'
            {
            match("++"); 


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
            // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:54:7: ( '--' )
            // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:54:9: '--'
            {
            match("--"); 


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
            // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:55:7: ( '!' )
            // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:55:9: '!'
            {
            match('!'); 

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
            // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:56:7: ( '(' )
            // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:56:9: '('
            {
            match('('); 

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
            // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:57:7: ( ')' )
            // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:57:9: ')'
            {
            match(')'); 

            }

            state.type = _type;
            state.channel = _channel;
        }
        finally {
        }
    }
    // $ANTLR end "T__85"

    // $ANTLR start "Id"
    public final void mId() throws RecognitionException {
        try {
            int _type = Id;
            int _channel = DEFAULT_TOKEN_CHANNEL;
            // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:204:2: ( ( 'a' .. 'z' | 'A' .. 'Z' | '_' ) ( 'a' .. 'z' | 'A' .. 'Z' | '0' .. '9' | '_' )* )
            // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:204:4: ( 'a' .. 'z' | 'A' .. 'Z' | '_' ) ( 'a' .. 'z' | 'A' .. 'Z' | '0' .. '9' | '_' )*
            {
            if ( (input.LA(1)>='A' && input.LA(1)<='Z')||input.LA(1)=='_'||(input.LA(1)>='a' && input.LA(1)<='z') ) {
                input.consume();

            }
            else {
                MismatchedSetException mse = new MismatchedSetException(null,input);
                recover(mse);
                throw mse;}

            // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:204:28: ( 'a' .. 'z' | 'A' .. 'Z' | '0' .. '9' | '_' )*
            loop1:
            do {
                int alt1=2;
                int LA1_0 = input.LA(1);

                if ( ((LA1_0>='0' && LA1_0<='9')||(LA1_0>='A' && LA1_0<='Z')||LA1_0=='_'||(LA1_0>='a' && LA1_0<='z')) ) {
                    alt1=1;
                }


                switch (alt1) {
            	case 1 :
            	    // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:
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
            // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:207:2: ( '0' ( 'x' | 'X' ) ( HexDigit )+ ( IntegerTypeSuffix )? )
            // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:207:4: '0' ( 'x' | 'X' ) ( HexDigit )+ ( IntegerTypeSuffix )?
            {
            match('0'); 
            if ( input.LA(1)=='X'||input.LA(1)=='x' ) {
                input.consume();

            }
            else {
                MismatchedSetException mse = new MismatchedSetException(null,input);
                recover(mse);
                throw mse;}

            // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:207:18: ( HexDigit )+
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
            	    // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:207:18: HexDigit
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

            // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:207:28: ( IntegerTypeSuffix )?
            int alt3=2;
            int LA3_0 = input.LA(1);

            if ( (LA3_0=='L'||LA3_0=='l') ) {
                alt3=1;
            }
            switch (alt3) {
                case 1 :
                    // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:207:28: IntegerTypeSuffix
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

    // $ANTLR start "DecimalLiteral"
    public final void mDecimalLiteral() throws RecognitionException {
        try {
            int _type = DecimalLiteral;
            int _channel = DEFAULT_TOKEN_CHANNEL;
            // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:210:2: ( ( '0' | '1' .. '9' ( '0' .. '9' )* ) ( IntegerTypeSuffix )? )
            // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:211:2: ( '0' | '1' .. '9' ( '0' .. '9' )* ) ( IntegerTypeSuffix )?
            {
            // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:211:2: ( '0' | '1' .. '9' ( '0' .. '9' )* )
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
                    // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:211:3: '0'
                    {
                    match('0'); 

                    }
                    break;
                case 2 :
                    // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:211:9: '1' .. '9' ( '0' .. '9' )*
                    {
                    matchRange('1','9'); 
                    // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:211:18: ( '0' .. '9' )*
                    loop4:
                    do {
                        int alt4=2;
                        int LA4_0 = input.LA(1);

                        if ( ((LA4_0>='0' && LA4_0<='9')) ) {
                            alt4=1;
                        }


                        switch (alt4) {
                    	case 1 :
                    	    // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:211:18: '0' .. '9'
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

            // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:211:29: ( IntegerTypeSuffix )?
            int alt6=2;
            int LA6_0 = input.LA(1);

            if ( (LA6_0=='L'||LA6_0=='l') ) {
                alt6=1;
            }
            switch (alt6) {
                case 1 :
                    // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:211:29: IntegerTypeSuffix
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
    // $ANTLR end "DecimalLiteral"

    // $ANTLR start "OctalLiteral"
    public final void mOctalLiteral() throws RecognitionException {
        try {
            int _type = OctalLiteral;
            int _channel = DEFAULT_TOKEN_CHANNEL;
            // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:214:2: ( '0' ( '0' .. '7' )+ ( IntegerTypeSuffix )? )
            // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:214:4: '0' ( '0' .. '7' )+ ( IntegerTypeSuffix )?
            {
            match('0'); 
            // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:214:8: ( '0' .. '7' )+
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
            	    // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:214:9: '0' .. '7'
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

            // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:214:20: ( IntegerTypeSuffix )?
            int alt8=2;
            int LA8_0 = input.LA(1);

            if ( (LA8_0=='L'||LA8_0=='l') ) {
                alt8=1;
            }
            switch (alt8) {
                case 1 :
                    // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:214:20: IntegerTypeSuffix
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
            // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:218:3: ( ( '0' .. '9' | 'a' .. 'f' | 'A' .. 'F' ) )
            // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:218:5: ( '0' .. '9' | 'a' .. 'f' | 'A' .. 'F' )
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
            // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:222:2: ( ( 'l' | 'L' ) )
            // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:222:4: ( 'l' | 'L' )
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

    // $ANTLR start "FloatingPointLiteral"
    public final void mFloatingPointLiteral() throws RecognitionException {
        try {
            int _type = FloatingPointLiteral;
            int _channel = DEFAULT_TOKEN_CHANNEL;
            // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:225:2: ( ( '0' .. '9' )+ '.' ( '0' .. '9' )* ( Exponent )? ( FloatTypeSuffix )? | '.' ( '0' .. '9' )+ ( Exponent )? ( FloatTypeSuffix )? | ( '0' .. '9' )+ Exponent ( FloatTypeSuffix )? | ( '0' .. '9' )+ FloatTypeSuffix )
            int alt19=4;
            alt19 = dfa19.predict(input);
            switch (alt19) {
                case 1 :
                    // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:225:4: ( '0' .. '9' )+ '.' ( '0' .. '9' )* ( Exponent )? ( FloatTypeSuffix )?
                    {
                    // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:225:4: ( '0' .. '9' )+
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
                    	    // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:225:5: '0' .. '9'
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
                    // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:225:20: ( '0' .. '9' )*
                    loop10:
                    do {
                        int alt10=2;
                        int LA10_0 = input.LA(1);

                        if ( ((LA10_0>='0' && LA10_0<='9')) ) {
                            alt10=1;
                        }


                        switch (alt10) {
                    	case 1 :
                    	    // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:225:21: '0' .. '9'
                    	    {
                    	    matchRange('0','9'); 

                    	    }
                    	    break;

                    	default :
                    	    break loop10;
                        }
                    } while (true);

                    // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:225:32: ( Exponent )?
                    int alt11=2;
                    int LA11_0 = input.LA(1);

                    if ( (LA11_0=='E'||LA11_0=='e') ) {
                        alt11=1;
                    }
                    switch (alt11) {
                        case 1 :
                            // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:225:32: Exponent
                            {
                            mExponent(); 

                            }
                            break;

                    }

                    // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:225:42: ( FloatTypeSuffix )?
                    int alt12=2;
                    int LA12_0 = input.LA(1);

                    if ( (LA12_0=='D'||LA12_0=='F'||LA12_0=='d'||LA12_0=='f') ) {
                        alt12=1;
                    }
                    switch (alt12) {
                        case 1 :
                            // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:225:42: FloatTypeSuffix
                            {
                            mFloatTypeSuffix(); 

                            }
                            break;

                    }


                    }
                    break;
                case 2 :
                    // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:226:4: '.' ( '0' .. '9' )+ ( Exponent )? ( FloatTypeSuffix )?
                    {
                    match('.'); 
                    // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:226:8: ( '0' .. '9' )+
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
                    	    // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:226:9: '0' .. '9'
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

                    // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:226:20: ( Exponent )?
                    int alt14=2;
                    int LA14_0 = input.LA(1);

                    if ( (LA14_0=='E'||LA14_0=='e') ) {
                        alt14=1;
                    }
                    switch (alt14) {
                        case 1 :
                            // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:226:20: Exponent
                            {
                            mExponent(); 

                            }
                            break;

                    }

                    // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:226:30: ( FloatTypeSuffix )?
                    int alt15=2;
                    int LA15_0 = input.LA(1);

                    if ( (LA15_0=='D'||LA15_0=='F'||LA15_0=='d'||LA15_0=='f') ) {
                        alt15=1;
                    }
                    switch (alt15) {
                        case 1 :
                            // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:226:30: FloatTypeSuffix
                            {
                            mFloatTypeSuffix(); 

                            }
                            break;

                    }


                    }
                    break;
                case 3 :
                    // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:227:4: ( '0' .. '9' )+ Exponent ( FloatTypeSuffix )?
                    {
                    // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:227:4: ( '0' .. '9' )+
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
                    	    // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:227:5: '0' .. '9'
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
                    // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:227:25: ( FloatTypeSuffix )?
                    int alt17=2;
                    int LA17_0 = input.LA(1);

                    if ( (LA17_0=='D'||LA17_0=='F'||LA17_0=='d'||LA17_0=='f') ) {
                        alt17=1;
                    }
                    switch (alt17) {
                        case 1 :
                            // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:227:25: FloatTypeSuffix
                            {
                            mFloatTypeSuffix(); 

                            }
                            break;

                    }


                    }
                    break;
                case 4 :
                    // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:228:4: ( '0' .. '9' )+ FloatTypeSuffix
                    {
                    // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:228:4: ( '0' .. '9' )+
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
                    	    // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:228:5: '0' .. '9'
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
    // $ANTLR end "FloatingPointLiteral"

    // $ANTLR start "Exponent"
    public final void mExponent() throws RecognitionException {
        try {
            // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:232:2: ( ( 'e' | 'E' ) ( '+' | '-' )? ( '0' .. '9' )+ )
            // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:232:4: ( 'e' | 'E' ) ( '+' | '-' )? ( '0' .. '9' )+
            {
            if ( input.LA(1)=='E'||input.LA(1)=='e' ) {
                input.consume();

            }
            else {
                MismatchedSetException mse = new MismatchedSetException(null,input);
                recover(mse);
                throw mse;}

            // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:232:14: ( '+' | '-' )?
            int alt20=2;
            int LA20_0 = input.LA(1);

            if ( (LA20_0=='+'||LA20_0=='-') ) {
                alt20=1;
            }
            switch (alt20) {
                case 1 :
                    // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:
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

            // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:232:25: ( '0' .. '9' )+
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
            	    // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:232:26: '0' .. '9'
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
            // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:236:2: ( ( 'f' | 'F' | 'd' | 'D' ) )
            // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:236:4: ( 'f' | 'F' | 'd' | 'D' )
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
            // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:239:2: ( '\"' ( EscapeSequence | ~ ( '\\\\' | '\"' ) )* '\"' | '\\'' ( EscapeSequence | ~ ( '\\\\' | '\\'' ) )* '\\'' )
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
                    // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:239:4: '\"' ( EscapeSequence | ~ ( '\\\\' | '\"' ) )* '\"'
                    {
                    match('\"'); 
                    // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:239:8: ( EscapeSequence | ~ ( '\\\\' | '\"' ) )*
                    loop22:
                    do {
                        int alt22=3;
                        int LA22_0 = input.LA(1);

                        if ( (LA22_0=='\\') ) {
                            alt22=1;
                        }
                        else if ( ((LA22_0>='\u0000' && LA22_0<='!')||(LA22_0>='#' && LA22_0<='[')||(LA22_0>=']' && LA22_0<='\uFFFF')) ) {
                            alt22=2;
                        }


                        switch (alt22) {
                    	case 1 :
                    	    // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:239:10: EscapeSequence
                    	    {
                    	    mEscapeSequence(); 

                    	    }
                    	    break;
                    	case 2 :
                    	    // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:239:27: ~ ( '\\\\' | '\"' )
                    	    {
                    	    if ( (input.LA(1)>='\u0000' && input.LA(1)<='!')||(input.LA(1)>='#' && input.LA(1)<='[')||(input.LA(1)>=']' && input.LA(1)<='\uFFFF') ) {
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
                    // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:240:4: '\\'' ( EscapeSequence | ~ ( '\\\\' | '\\'' ) )* '\\''
                    {
                    match('\''); 
                    // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:240:9: ( EscapeSequence | ~ ( '\\\\' | '\\'' ) )*
                    loop23:
                    do {
                        int alt23=3;
                        int LA23_0 = input.LA(1);

                        if ( (LA23_0=='\\') ) {
                            alt23=1;
                        }
                        else if ( ((LA23_0>='\u0000' && LA23_0<='&')||(LA23_0>='(' && LA23_0<='[')||(LA23_0>=']' && LA23_0<='\uFFFF')) ) {
                            alt23=2;
                        }


                        switch (alt23) {
                    	case 1 :
                    	    // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:240:11: EscapeSequence
                    	    {
                    	    mEscapeSequence(); 

                    	    }
                    	    break;
                    	case 2 :
                    	    // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:240:28: ~ ( '\\\\' | '\\'' )
                    	    {
                    	    if ( (input.LA(1)>='\u0000' && input.LA(1)<='&')||(input.LA(1)>='(' && input.LA(1)<='[')||(input.LA(1)>=']' && input.LA(1)<='\uFFFF') ) {
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

    // $ANTLR start "EscapeSequence"
    public final void mEscapeSequence() throws RecognitionException {
        try {
            // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:244:2: ( '\\\\' ( 'b' | 't' | 'n' | 'f' | 'r' | '\\\"' | '\\'' | '\\\\' ) | UnicodeEscape | OctalEscape )
            int alt25=3;
            int LA25_0 = input.LA(1);

            if ( (LA25_0=='\\') ) {
                switch ( input.LA(2) ) {
                case '\"':
                case '\'':
                case '\\':
                case 'b':
                case 'f':
                case 'n':
                case 'r':
                case 't':
                    {
                    alt25=1;
                    }
                    break;
                case 'u':
                    {
                    alt25=2;
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
                    alt25=3;
                    }
                    break;
                default:
                    NoViableAltException nvae =
                        new NoViableAltException("", 25, 1, input);

                    throw nvae;
                }

            }
            else {
                NoViableAltException nvae =
                    new NoViableAltException("", 25, 0, input);

                throw nvae;
            }
            switch (alt25) {
                case 1 :
                    // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:244:4: '\\\\' ( 'b' | 't' | 'n' | 'f' | 'r' | '\\\"' | '\\'' | '\\\\' )
                    {
                    match('\\'); 
                    if ( input.LA(1)=='\"'||input.LA(1)=='\''||input.LA(1)=='\\'||input.LA(1)=='b'||input.LA(1)=='f'||input.LA(1)=='n'||input.LA(1)=='r'||input.LA(1)=='t' ) {
                        input.consume();

                    }
                    else {
                        MismatchedSetException mse = new MismatchedSetException(null,input);
                        recover(mse);
                        throw mse;}


                    }
                    break;
                case 2 :
                    // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:245:4: UnicodeEscape
                    {
                    mUnicodeEscape(); 

                    }
                    break;
                case 3 :
                    // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:246:4: OctalEscape
                    {
                    mOctalEscape(); 

                    }
                    break;

            }
        }
        finally {
        }
    }
    // $ANTLR end "EscapeSequence"

    // $ANTLR start "OctalEscape"
    public final void mOctalEscape() throws RecognitionException {
        try {
            // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:250:2: ( '\\\\' ( '0' .. '3' ) ( '0' .. '7' ) ( '0' .. '7' ) | '\\\\' ( '0' .. '7' ) ( '0' .. '7' ) | '\\\\' ( '0' .. '7' ) )
            int alt26=3;
            int LA26_0 = input.LA(1);

            if ( (LA26_0=='\\') ) {
                int LA26_1 = input.LA(2);

                if ( ((LA26_1>='0' && LA26_1<='3')) ) {
                    int LA26_2 = input.LA(3);

                    if ( ((LA26_2>='0' && LA26_2<='7')) ) {
                        int LA26_4 = input.LA(4);

                        if ( ((LA26_4>='0' && LA26_4<='7')) ) {
                            alt26=1;
                        }
                        else {
                            alt26=2;}
                    }
                    else {
                        alt26=3;}
                }
                else if ( ((LA26_1>='4' && LA26_1<='7')) ) {
                    int LA26_3 = input.LA(3);

                    if ( ((LA26_3>='0' && LA26_3<='7')) ) {
                        alt26=2;
                    }
                    else {
                        alt26=3;}
                }
                else {
                    NoViableAltException nvae =
                        new NoViableAltException("", 26, 1, input);

                    throw nvae;
                }
            }
            else {
                NoViableAltException nvae =
                    new NoViableAltException("", 26, 0, input);

                throw nvae;
            }
            switch (alt26) {
                case 1 :
                    // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:250:4: '\\\\' ( '0' .. '3' ) ( '0' .. '7' ) ( '0' .. '7' )
                    {
                    match('\\'); 
                    // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:250:9: ( '0' .. '3' )
                    // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:250:10: '0' .. '3'
                    {
                    matchRange('0','3'); 

                    }

                    // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:250:20: ( '0' .. '7' )
                    // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:250:21: '0' .. '7'
                    {
                    matchRange('0','7'); 

                    }

                    // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:250:31: ( '0' .. '7' )
                    // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:250:32: '0' .. '7'
                    {
                    matchRange('0','7'); 

                    }


                    }
                    break;
                case 2 :
                    // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:251:4: '\\\\' ( '0' .. '7' ) ( '0' .. '7' )
                    {
                    match('\\'); 
                    // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:251:9: ( '0' .. '7' )
                    // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:251:10: '0' .. '7'
                    {
                    matchRange('0','7'); 

                    }

                    // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:251:20: ( '0' .. '7' )
                    // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:251:21: '0' .. '7'
                    {
                    matchRange('0','7'); 

                    }


                    }
                    break;
                case 3 :
                    // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:252:4: '\\\\' ( '0' .. '7' )
                    {
                    match('\\'); 
                    // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:252:9: ( '0' .. '7' )
                    // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:252:10: '0' .. '7'
                    {
                    matchRange('0','7'); 

                    }


                    }
                    break;

            }
        }
        finally {
        }
    }
    // $ANTLR end "OctalEscape"

    // $ANTLR start "UnicodeEscape"
    public final void mUnicodeEscape() throws RecognitionException {
        try {
            // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:256:2: ( '\\\\' 'u' HexDigit HexDigit HexDigit HexDigit )
            // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:256:4: '\\\\' 'u' HexDigit HexDigit HexDigit HexDigit
            {
            match('\\'); 
            match('u'); 
            mHexDigit(); 
            mHexDigit(); 
            mHexDigit(); 
            mHexDigit(); 

            }

        }
        finally {
        }
    }
    // $ANTLR end "UnicodeEscape"

    // $ANTLR start "WS"
    public final void mWS() throws RecognitionException {
        try {
            int _type = WS;
            int _channel = DEFAULT_TOKEN_CHANNEL;
            // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:262:2: ( ( ' ' | '\\r' | '\\t' | '\\u000C' | '\\n' ) )
            // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:262:5: ( ' ' | '\\r' | '\\t' | '\\u000C' | '\\n' )
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
            // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:265:2: ( '/*' ( options {greedy=false; } : . )* '*/' )
            // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:265:4: '/*' ( options {greedy=false; } : . )* '*/'
            {
            match("/*"); 

            // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:265:9: ( options {greedy=false; } : . )*
            loop27:
            do {
                int alt27=2;
                int LA27_0 = input.LA(1);

                if ( (LA27_0=='*') ) {
                    int LA27_1 = input.LA(2);

                    if ( (LA27_1=='/') ) {
                        alt27=2;
                    }
                    else if ( ((LA27_1>='\u0000' && LA27_1<='.')||(LA27_1>='0' && LA27_1<='\uFFFF')) ) {
                        alt27=1;
                    }


                }
                else if ( ((LA27_0>='\u0000' && LA27_0<=')')||(LA27_0>='+' && LA27_0<='\uFFFF')) ) {
                    alt27=1;
                }


                switch (alt27) {
            	case 1 :
            	    // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:265:37: .
            	    {
            	    matchAny(); 

            	    }
            	    break;

            	default :
            	    break loop27;
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
            // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:268:2: ( '//' (~ ( '\\n' | '\\r' ) )* ( '\\r' )? '\\n' )
            // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:268:4: '//' (~ ( '\\n' | '\\r' ) )* ( '\\r' )? '\\n'
            {
            match("//"); 

            // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:268:9: (~ ( '\\n' | '\\r' ) )*
            loop28:
            do {
                int alt28=2;
                int LA28_0 = input.LA(1);

                if ( ((LA28_0>='\u0000' && LA28_0<='\t')||(LA28_0>='\u000B' && LA28_0<='\f')||(LA28_0>='\u000E' && LA28_0<='\uFFFF')) ) {
                    alt28=1;
                }


                switch (alt28) {
            	case 1 :
            	    // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:268:9: ~ ( '\\n' | '\\r' )
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
            	    break loop28;
                }
            } while (true);

            // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:268:23: ( '\\r' )?
            int alt29=2;
            int LA29_0 = input.LA(1);

            if ( (LA29_0=='\r') ) {
                alt29=1;
            }
            switch (alt29) {
                case 1 :
                    // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:268:23: '\\r'
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
        // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:1:8: ( T__31 | T__32 | T__33 | T__34 | T__35 | T__36 | T__37 | T__38 | T__39 | T__40 | T__41 | T__42 | T__43 | T__44 | T__45 | T__46 | T__47 | T__48 | T__49 | T__50 | T__51 | T__52 | T__53 | T__54 | T__55 | T__56 | T__57 | T__58 | T__59 | T__60 | T__61 | T__62 | T__63 | T__64 | T__65 | T__66 | T__67 | T__68 | T__69 | T__70 | T__71 | T__72 | T__73 | T__74 | T__75 | T__76 | T__77 | T__78 | T__79 | T__80 | T__81 | T__82 | T__83 | T__84 | T__85 | Id | HexLiteral | DecimalLiteral | OctalLiteral | FloatingPointLiteral | StringLiteral | WS | COMMENT | LINE_COMMENT )
        int alt30=64;
        alt30 = dfa30.predict(input);
        switch (alt30) {
            case 1 :
                // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:1:10: T__31
                {
                mT__31(); 

                }
                break;
            case 2 :
                // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:1:16: T__32
                {
                mT__32(); 

                }
                break;
            case 3 :
                // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:1:22: T__33
                {
                mT__33(); 

                }
                break;
            case 4 :
                // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:1:28: T__34
                {
                mT__34(); 

                }
                break;
            case 5 :
                // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:1:34: T__35
                {
                mT__35(); 

                }
                break;
            case 6 :
                // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:1:40: T__36
                {
                mT__36(); 

                }
                break;
            case 7 :
                // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:1:46: T__37
                {
                mT__37(); 

                }
                break;
            case 8 :
                // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:1:52: T__38
                {
                mT__38(); 

                }
                break;
            case 9 :
                // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:1:58: T__39
                {
                mT__39(); 

                }
                break;
            case 10 :
                // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:1:64: T__40
                {
                mT__40(); 

                }
                break;
            case 11 :
                // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:1:70: T__41
                {
                mT__41(); 

                }
                break;
            case 12 :
                // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:1:76: T__42
                {
                mT__42(); 

                }
                break;
            case 13 :
                // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:1:82: T__43
                {
                mT__43(); 

                }
                break;
            case 14 :
                // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:1:88: T__44
                {
                mT__44(); 

                }
                break;
            case 15 :
                // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:1:94: T__45
                {
                mT__45(); 

                }
                break;
            case 16 :
                // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:1:100: T__46
                {
                mT__46(); 

                }
                break;
            case 17 :
                // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:1:106: T__47
                {
                mT__47(); 

                }
                break;
            case 18 :
                // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:1:112: T__48
                {
                mT__48(); 

                }
                break;
            case 19 :
                // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:1:118: T__49
                {
                mT__49(); 

                }
                break;
            case 20 :
                // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:1:124: T__50
                {
                mT__50(); 

                }
                break;
            case 21 :
                // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:1:130: T__51
                {
                mT__51(); 

                }
                break;
            case 22 :
                // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:1:136: T__52
                {
                mT__52(); 

                }
                break;
            case 23 :
                // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:1:142: T__53
                {
                mT__53(); 

                }
                break;
            case 24 :
                // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:1:148: T__54
                {
                mT__54(); 

                }
                break;
            case 25 :
                // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:1:154: T__55
                {
                mT__55(); 

                }
                break;
            case 26 :
                // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:1:160: T__56
                {
                mT__56(); 

                }
                break;
            case 27 :
                // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:1:166: T__57
                {
                mT__57(); 

                }
                break;
            case 28 :
                // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:1:172: T__58
                {
                mT__58(); 

                }
                break;
            case 29 :
                // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:1:178: T__59
                {
                mT__59(); 

                }
                break;
            case 30 :
                // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:1:184: T__60
                {
                mT__60(); 

                }
                break;
            case 31 :
                // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:1:190: T__61
                {
                mT__61(); 

                }
                break;
            case 32 :
                // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:1:196: T__62
                {
                mT__62(); 

                }
                break;
            case 33 :
                // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:1:202: T__63
                {
                mT__63(); 

                }
                break;
            case 34 :
                // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:1:208: T__64
                {
                mT__64(); 

                }
                break;
            case 35 :
                // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:1:214: T__65
                {
                mT__65(); 

                }
                break;
            case 36 :
                // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:1:220: T__66
                {
                mT__66(); 

                }
                break;
            case 37 :
                // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:1:226: T__67
                {
                mT__67(); 

                }
                break;
            case 38 :
                // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:1:232: T__68
                {
                mT__68(); 

                }
                break;
            case 39 :
                // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:1:238: T__69
                {
                mT__69(); 

                }
                break;
            case 40 :
                // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:1:244: T__70
                {
                mT__70(); 

                }
                break;
            case 41 :
                // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:1:250: T__71
                {
                mT__71(); 

                }
                break;
            case 42 :
                // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:1:256: T__72
                {
                mT__72(); 

                }
                break;
            case 43 :
                // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:1:262: T__73
                {
                mT__73(); 

                }
                break;
            case 44 :
                // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:1:268: T__74
                {
                mT__74(); 

                }
                break;
            case 45 :
                // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:1:274: T__75
                {
                mT__75(); 

                }
                break;
            case 46 :
                // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:1:280: T__76
                {
                mT__76(); 

                }
                break;
            case 47 :
                // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:1:286: T__77
                {
                mT__77(); 

                }
                break;
            case 48 :
                // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:1:292: T__78
                {
                mT__78(); 

                }
                break;
            case 49 :
                // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:1:298: T__79
                {
                mT__79(); 

                }
                break;
            case 50 :
                // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:1:304: T__80
                {
                mT__80(); 

                }
                break;
            case 51 :
                // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:1:310: T__81
                {
                mT__81(); 

                }
                break;
            case 52 :
                // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:1:316: T__82
                {
                mT__82(); 

                }
                break;
            case 53 :
                // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:1:322: T__83
                {
                mT__83(); 

                }
                break;
            case 54 :
                // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:1:328: T__84
                {
                mT__84(); 

                }
                break;
            case 55 :
                // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:1:334: T__85
                {
                mT__85(); 

                }
                break;
            case 56 :
                // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:1:340: Id
                {
                mId(); 

                }
                break;
            case 57 :
                // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:1:343: HexLiteral
                {
                mHexLiteral(); 

                }
                break;
            case 58 :
                // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:1:354: DecimalLiteral
                {
                mDecimalLiteral(); 

                }
                break;
            case 59 :
                // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:1:369: OctalLiteral
                {
                mOctalLiteral(); 

                }
                break;
            case 60 :
                // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:1:382: FloatingPointLiteral
                {
                mFloatingPointLiteral(); 

                }
                break;
            case 61 :
                // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:1:403: StringLiteral
                {
                mStringLiteral(); 

                }
                break;
            case 62 :
                // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:1:417: WS
                {
                mWS(); 

                }
                break;
            case 63 :
                // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:1:420: COMMENT
                {
                mCOMMENT(); 

                }
                break;
            case 64 :
                // C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\MetraScript.g:1:428: LINE_COMMENT
                {
                mLINE_COMMENT(); 

                }
                break;

        }

    }


    protected DFA19 dfa19 = new DFA19(this);
    protected DFA30 dfa30 = new DFA30(this);
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
            return "224:1: FloatingPointLiteral : ( ( '0' .. '9' )+ '.' ( '0' .. '9' )* ( Exponent )? ( FloatTypeSuffix )? | '.' ( '0' .. '9' )+ ( Exponent )? ( FloatTypeSuffix )? | ( '0' .. '9' )+ Exponent ( FloatTypeSuffix )? | ( '0' .. '9' )+ FloatTypeSuffix );";
        }
    }
    static final String DFA30_eotS =
        "\1\uffff\1\40\1\50\6\40\1\63\1\65\1\70\1\73\1\75\1\101\1\104\1"+
        "\107\1\111\1\113\4\uffff\2\40\1\117\5\40\2\uffff\2\127\3\uffff\1"+
        "\40\2\uffff\3\40\1\136\2\40\1\141\2\40\32\uffff\1\144\1\40\2\uffff"+
        "\1\146\1\150\1\152\1\154\1\155\1\uffff\1\156\1\uffff\1\127\4\40"+
        "\1\163\1\uffff\2\40\1\uffff\2\40\1\uffff\1\170\1\uffff\1\171\1\uffff"+
        "\1\172\1\uffff\1\173\3\uffff\4\40\1\uffff\1\40\1\u0081\1\u0082\1"+
        "\40\4\uffff\1\40\1\uffff\2\40\3\uffff\1\u0087\3\40\5\uffff";
    static final String DFA30_eofS =
        "\u008c\uffff";
    static final String DFA30_minS =
        "\1\11\1\102\1\56\1\105\1\114\1\122\1\145\1\162\1\141\2\75\1\53"+
        "\1\55\1\75\1\52\1\46\3\75\4\uffff\1\162\1\156\1\75\1\161\2\164\1"+
        "\124\1\121\2\uffff\2\56\3\uffff\1\112\2\uffff\1\115\1\122\1\117"+
        "\1\60\1\117\1\154\1\60\1\165\1\154\32\uffff\1\60\1\144\2\uffff\5"+
        "\60\1\uffff\1\56\1\uffff\1\56\1\105\1\120\1\105\1\102\1\60\1\uffff"+
        "\1\103\1\154\1\uffff\1\145\1\163\1\uffff\1\60\1\uffff\1\60\1\uffff"+
        "\1\60\1\uffff\1\60\3\uffff\1\103\1\56\2\101\1\uffff\1\56\2\60\1"+
        "\145\4\uffff\1\124\1\uffff\1\104\1\114\3\uffff\1\60\1\50\2\56\5"+
        "\uffff";
    static final String DFA30_maxS =
        "\1\176\1\102\1\56\1\110\1\124\1\122\1\165\1\162\1\141\7\75\1\174"+
        "\2\75\4\uffff\1\162\1\156\1\75\1\161\2\164\1\124\1\121\2\uffff\1"+
        "\170\1\146\3\uffff\1\112\2\uffff\1\115\1\122\1\117\1\172\1\117\1"+
        "\154\1\172\1\165\1\154\32\uffff\1\172\1\144\2\uffff\5\172\1\uffff"+
        "\1\146\1\uffff\1\146\1\105\1\120\1\105\1\102\1\172\1\uffff\1\103"+
        "\1\154\1\uffff\1\145\1\163\1\uffff\1\172\1\uffff\1\172\1\uffff\1"+
        "\172\1\uffff\1\172\3\uffff\1\103\1\56\2\101\1\uffff\1\56\2\172\1"+
        "\145\4\uffff\1\124\1\uffff\1\104\1\114\3\uffff\1\172\3\56\5\uffff";
    static final String DFA30_acceptS =
        "\23\uffff\1\26\1\27\1\30\1\31\10\uffff\1\66\1\70\2\uffff\1\74\1"+
        "\75\1\76\1\uffff\1\3\1\67\11\uffff\1\41\1\13\1\25\1\14\1\15\1\63"+
        "\1\56\1\16\1\64\1\57\1\17\1\60\1\20\1\77\1\100\1\61\1\21\1\34\1"+
        "\40\1\22\1\32\1\36\1\23\1\37\1\24\1\62\2\uffff\1\42\1\65\5\uffff"+
        "\1\71\1\uffff\1\72\6\uffff\1\51\2\uffff\1\44\2\uffff\1\33\1\uffff"+
        "\1\43\1\uffff\1\45\1\uffff\1\46\1\uffff\1\52\1\53\1\73\4\uffff\1"+
        "\54\4\uffff\1\35\1\47\1\50\1\55\1\uffff\1\4\2\uffff\1\7\1\10\1\11"+
        "\4\uffff\1\12\1\1\1\2\1\6\1\5";
    static final String DFA30_specialS =
        "\u008c\uffff}>";
    static final String[] DFA30_transitionS = {
            "\2\45\1\uffff\2\45\22\uffff\1\45\1\31\1\44\2\uffff\1\22\1\17"+
            "\1\44\1\37\1\2\1\15\1\13\1\uffff\1\14\1\43\1\16\1\41\11\42\1"+
            "\26\1\uffff\1\23\1\11\1\24\1\25\1\uffff\4\40\1\36\1\40\1\4\4"+
            "\40\1\35\2\40\1\1\1\5\3\40\1\3\6\40\3\uffff\1\21\1\40\1\uffff"+
            "\1\30\3\40\1\32\1\10\1\33\4\40\1\34\1\40\1\6\1\27\4\40\1\7\6"+
            "\40\1\uffff\1\20\1\uffff\1\12",
            "\1\46",
            "\1\47",
            "\1\51\2\uffff\1\52",
            "\1\53\7\uffff\1\54",
            "\1\55",
            "\1\57\17\uffff\1\56",
            "\1\60",
            "\1\61",
            "\1\62",
            "\1\64",
            "\1\67\21\uffff\1\66",
            "\1\72\17\uffff\1\71",
            "\1\74",
            "\1\77\4\uffff\1\100\15\uffff\1\76",
            "\1\103\26\uffff\1\102",
            "\1\105\76\uffff\1\106",
            "\1\110",
            "\1\112",
            "",
            "",
            "",
            "",
            "\1\114",
            "\1\115",
            "\1\116",
            "\1\120",
            "\1\121",
            "\1\122",
            "\1\123",
            "\1\124",
            "",
            "",
            "\1\43\1\uffff\10\126\2\43\12\uffff\3\43\21\uffff\1\125\13"+
            "\uffff\3\43\21\uffff\1\125",
            "\1\43\1\uffff\12\130\12\uffff\3\43\35\uffff\3\43",
            "",
            "",
            "",
            "\1\131",
            "",
            "",
            "\1\132",
            "\1\133",
            "\1\134",
            "\12\40\7\uffff\4\40\1\135\25\40\4\uffff\1\40\1\uffff\32\40",
            "\1\137",
            "\1\140",
            "\12\40\7\uffff\32\40\4\uffff\1\40\1\uffff\32\40",
            "\1\142",
            "\1\143",
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
            "\12\40\7\uffff\32\40\4\uffff\1\40\1\uffff\32\40",
            "\1\145",
            "",
            "",
            "\12\40\7\uffff\32\40\4\uffff\1\40\1\uffff\32\40",
            "\12\40\7\uffff\32\40\4\uffff\1\40\1\uffff\4\40\1\147\25\40",
            "\12\40\7\uffff\32\40\4\uffff\1\40\1\uffff\4\40\1\151\25\40",
            "\12\40\7\uffff\4\40\1\153\25\40\4\uffff\1\40\1\uffff\32\40",
            "\12\40\7\uffff\32\40\4\uffff\1\40\1\uffff\32\40",
            "",
            "\1\43\1\uffff\10\126\2\43\12\uffff\3\43\35\uffff\3\43",
            "",
            "\1\43\1\uffff\12\130\12\uffff\3\43\35\uffff\3\43",
            "\1\157",
            "\1\160",
            "\1\161",
            "\1\162",
            "\12\40\7\uffff\32\40\4\uffff\1\40\1\uffff\32\40",
            "",
            "\1\164",
            "\1\165",
            "",
            "\1\166",
            "\1\167",
            "",
            "\12\40\7\uffff\32\40\4\uffff\1\40\1\uffff\32\40",
            "",
            "\12\40\7\uffff\32\40\4\uffff\1\40\1\uffff\32\40",
            "",
            "\12\40\7\uffff\32\40\4\uffff\1\40\1\uffff\32\40",
            "",
            "\12\40\7\uffff\32\40\4\uffff\1\40\1\uffff\32\40",
            "",
            "",
            "",
            "\1\174",
            "\1\175",
            "\1\176",
            "\1\177",
            "",
            "\1\u0080",
            "\12\40\7\uffff\32\40\4\uffff\1\40\1\uffff\32\40",
            "\12\40\7\uffff\32\40\4\uffff\1\40\1\uffff\32\40",
            "\1\u0083",
            "",
            "",
            "",
            "",
            "\1\u0084",
            "",
            "\1\u0085",
            "\1\u0086",
            "",
            "",
            "",
            "\12\40\7\uffff\32\40\4\uffff\1\40\1\uffff\32\40",
            "\1\u0089\5\uffff\1\u0088",
            "\1\u008a",
            "\1\u008b",
            "",
            "",
            "",
            "",
            ""
    };

    static final short[] DFA30_eot = DFA.unpackEncodedString(DFA30_eotS);
    static final short[] DFA30_eof = DFA.unpackEncodedString(DFA30_eofS);
    static final char[] DFA30_min = DFA.unpackEncodedStringToUnsignedChars(DFA30_minS);
    static final char[] DFA30_max = DFA.unpackEncodedStringToUnsignedChars(DFA30_maxS);
    static final short[] DFA30_accept = DFA.unpackEncodedString(DFA30_acceptS);
    static final short[] DFA30_special = DFA.unpackEncodedString(DFA30_specialS);
    static final short[][] DFA30_transition;

    static {
        int numStates = DFA30_transitionS.length;
        DFA30_transition = new short[numStates][];
        for (int i=0; i<numStates; i++) {
            DFA30_transition[i] = DFA.unpackEncodedString(DFA30_transitionS[i]);
        }
    }

    class DFA30 extends DFA {

        public DFA30(BaseRecognizer recognizer) {
            this.recognizer = recognizer;
            this.decisionNumber = 30;
            this.eot = DFA30_eot;
            this.eof = DFA30_eof;
            this.min = DFA30_min;
            this.max = DFA30_max;
            this.accept = DFA30_accept;
            this.special = DFA30_special;
            this.transition = DFA30_transition;
        }
        public String getDescription() {
            return "1:1: Tokens : ( T__31 | T__32 | T__33 | T__34 | T__35 | T__36 | T__37 | T__38 | T__39 | T__40 | T__41 | T__42 | T__43 | T__44 | T__45 | T__46 | T__47 | T__48 | T__49 | T__50 | T__51 | T__52 | T__53 | T__54 | T__55 | T__56 | T__57 | T__58 | T__59 | T__60 | T__61 | T__62 | T__63 | T__64 | T__65 | T__66 | T__67 | T__68 | T__69 | T__70 | T__71 | T__72 | T__73 | T__74 | T__75 | T__76 | T__77 | T__78 | T__79 | T__80 | T__81 | T__82 | T__83 | T__84 | T__85 | Id | HexLiteral | DecimalLiteral | OctalLiteral | FloatingPointLiteral | StringLiteral | WS | COMMENT | LINE_COMMENT );";
        }
    }
 

}