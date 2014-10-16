using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Antlr.Runtime;
using Antlr.Runtime.Tree;
using MyExtensions;

namespace MVM
{
    public class SyntaxMaster
    {
        static Dictionary<string, ISetupSyntax> syntaxBuilders = new Dictionary<string, ISetupSyntax>();
        static Dictionary<string, ISetupWritable> writableBuilders = new Dictionary<string, ISetupWritable>();

        static SyntaxMaster()
        {
            // calling functions
            syntaxBuilders["FUNCTION"] = new CallFunction();

            // data access
            syntaxBuilders["THREAD"] = new AccessThread();
            syntaxBuilders["GLOBAL"] = new AccessGlobal();
            syntaxBuilders["TEMP"] = new AccessTemp();
            syntaxBuilders["OBJECT"] = new AccessObject();
            syntaxBuilders["STRING"] = new AccessString();
            syntaxBuilders["INT"] = new AccessNumber();
            syntaxBuilders["FLOAT"] = new AccessNumber();
            syntaxBuilders["BOOL"] = new AccessBool();
            syntaxBuilders["NULL"] = new AccessNull();
            syntaxBuilders["?"] = new OpIff();
            // assignment
            syntaxBuilders["="] = new OpAssign();
            syntaxBuilders["+="] = new OpThisEquals("+");
            syntaxBuilders["-="] = new OpThisEquals("-");
            syntaxBuilders["*="] = new OpThisEquals("*");
            syntaxBuilders["/="] = new OpThisEquals("/");
            syntaxBuilders["~="] = new OpThisEquals("~");
            syntaxBuilders["^="] = new OpThisEquals("^");
            // string
            syntaxBuilders["~"] = new OpConcat();
            syntaxBuilders["eq"] = new OpCaseSensitiveEq();
            syntaxBuilders["ne"] = new OpCaseSensitiveNe();
            syntaxBuilders["gt"] = new OpCaseSensitiveGt();
            syntaxBuilders["gte"] = new OpCaseSensitiveGte();
            syntaxBuilders["lt"] = new OpCaseSensitiveLt();
            syntaxBuilders["lte"] = new OpCaseSensitiveLte();
            // string case insensitive
            syntaxBuilders["Eq"] = new OpCaseInsensitiveEq();
            syntaxBuilders["Ne"] = new OpCaseInsensitiveNe();
            syntaxBuilders["Gt"] = new OpCaseInsensitiveGt();
            syntaxBuilders["Gte"] = new OpCaseInsensitiveGte();
            syntaxBuilders["Lt"] = new OpCaseInsensitiveLt();
            syntaxBuilders["Lte"] = new OpCaseInsensitiveLte();
            // numeric
            syntaxBuilders["+"] = new OpAdd();
            syntaxBuilders["-"] = new OpSubtract();
            syntaxBuilders["*"] = new OpMultiply();
            syntaxBuilders["/"] = new OpDivide();
            syntaxBuilders["EQ"] = new OpEq();
            syntaxBuilders["NE"] = new OpNe();
            syntaxBuilders["GT"] = new OpGt();
            syntaxBuilders["GTE"] = new OpGte();
            syntaxBuilders["LT"] = new OpLt();
            syntaxBuilders["LTE"] = new OpLte();
            // NOT SURE THESE ARE GOOD CHOICES
            syntaxBuilders[">"] = new OpGt();
            syntaxBuilders[">="] = new OpGte();
            syntaxBuilders["<"] = new OpLt();
            syntaxBuilders["<="] = new OpLte();
            syntaxBuilders["=="] = new OpCaseSensitiveOrNumericEq();  // this is gonna hurt when it burns
            // string or numeric
            syntaxBuilders["eqEQ"] = new OpCaseSensitiveOrNumericEq();
            syntaxBuilders["EqEQ"] = new OpCaseInsensitiveOrNumericEq();
            syntaxBuilders["neNE"] = new OpCaseSensitiveOrNumericNe();
            syntaxBuilders["NeNE"] = new OpCaseInsensitiveOrNumericNe();
            // logical
            syntaxBuilders["!"] = new OpNot();
            syntaxBuilders["&&"] = new OpAnd();
            syntaxBuilders["and"] = new OpAnd();
            syntaxBuilders["||"] = new OpOr();
            syntaxBuilders["or"] = new OpOr();
            // bitwise
            syntaxBuilders["|"] = new OpTBD();
            syntaxBuilders["&"] = new OpTBD();
            syntaxBuilders["^"] = new OpTBD();

            // setup writables
            writableBuilders["OBJECT"] = new AccessObject();
            writableBuilders["TEMP"] = new AccessTemp();
            writableBuilders["GLOBAL"] = new AccessGlobal();
            writableBuilders["THREAD"] = new AccessThread();
        }

        public static bool IsBinaryOperator(string op)
        {
            return syntaxBuilders.ContainsKey(op) && syntaxBuilders[op] is BaseBinaryOpSetup;
        }

        public static bool IsAssignmentOperator(string op)
        {
            return syntaxBuilders.ContainsKey(op) && (syntaxBuilders[op] is OpThisEquals || syntaxBuilders[op] is OpAssign);
        }

        public static bool IsOpReturningDecimal(string op)
        {
            return op.In("+", "-", "*", "/");
        }
        public static bool IsOpReturningBoolean(string op)
        {
            return op.In("==", "<", "<=", ">", ">=", "!=", "eq", "Eq", "EQ", "ne", "Ne", "NE", "lt", "Lt", "LT", "lte", "Lte", "LTE", "gt", "Gt", "GT", "gte", "Gte", "GTE");
        }
        public static bool IsOpReturningString(string op)
        {
            return op.In("~");
        }
        public static Type GetOpReturnType(string op)
        {
            if (IsOpReturningBoolean(op)) return typeof(bool);
            if (IsOpReturningDecimal(op)) return typeof(decimal);
            if (IsOpReturningString(op)) return typeof(string);
            return null;
        }

        // Returns object to execute syntax at runtime
        public IReadString SetupRead(string syntax)
        {
            ITree tree = ParseSyntax(syntax);
            return SetupRead(tree);
        }

        // Returns object to execute syntax at runtime
        public IReadString SetupRead(ITree readTree)
        {
            if (syntaxBuilders.ContainsKey(readTree.Text))
            {
                ISetupSyntax syntaxBuilder = syntaxBuilders[readTree.Text];
                return syntaxBuilder.SetupRead(this, readTree);
            }
            throw new Exception("unhandled parser node:" + readTree.Text);
        }

        // Returns object to execute syntax at runtime
        public IReadString SetupWrite(ITree leftTree, ITree rightTree)
        {
            if (syntaxBuilders.ContainsKey(leftTree.Text))
            {
                ISetupSyntax syntaxBuilder = syntaxBuilders[leftTree.Text];
                return syntaxBuilder.SetupWrite(this, leftTree, rightTree);
            }
            throw new Exception("unhandled parser node:" + leftTree.Text);
        }

        // Parses the syntax into antlr tree
        public ITree ParseSyntax(string syntax)
        {
            try
            {
                ICharStream input = new ANTLRStringStream(syntax);
                MetraScriptLexer lex = new MetraScriptLexer(input);
                CommonTokenStream tokens = new CommonTokenStream(lex);
                MetraScriptParser parser = new MetraScriptParser(tokens);
                MetraScriptParser.start_return r = parser.start();
                ITree iTree = (ITree)r.Tree;
                //Console.WriteLine("tree=" + iTree.ToStringTree());

                return iTree;
            }
            catch (Exception e)
            {
                string details = e.Message;
                Antlr.Runtime.NoViableAltException nvae = e as Antlr.Runtime.NoViableAltException;
                if (nvae != null)
                {
                    details += "; line=" + nvae.Line + "; pos=" + nvae.CharPositionInLine;
                }
                throw new Exception("Cannot parse syntax [" + syntax + "]: " + details);
            }
        }

        public IWriteString SetupWritable(string syntax)
        {
            ITree tree = ParseSyntax(syntax);
            return SetupWritable(tree);
        }

        public IWriteString SetupWritable(ITree leftTree)
        {
            if (writableBuilders.ContainsKey(leftTree.Text))
            {
                ISetupWritable setupWritable = writableBuilders[leftTree.Text];
                return setupWritable.SetupWritable(this, leftTree);
            }
            throw new Exception("Error:" + leftTree.Text + " is not writable!");
        }
    }
}
