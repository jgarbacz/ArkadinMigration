import java.io.*;
import org.antlr.runtime.*;
import org.antlr.runtime.debug.DebugEventSocketProxy;


public class __Test__ {

    public static void main(String args[]) throws Exception {
        MetraScriptLexer lex = new MetraScriptLexer(new ANTLRFileStream("C:\\Documents and Settings\\RParks\\My Documents\\Visual Studio 2008\\Projects\\MVM\\MVM\\parser\\antlr\\__Test___input.txt"));
        CommonTokenStream tokens = new CommonTokenStream(lex);

        MetraScriptParser g = new MetraScriptParser(tokens, 49100, null);
        try {
            g.start();
        } catch (RecognitionException e) {
            e.printStackTrace();
        }
    }
}