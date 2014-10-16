namespace Antlr.Rob
{
    using System;
    using Path = System.IO.Path;
    using Antlr.Runtime;
    using Antlr.Runtime.Tree;
    using MyExtensions;

    public class MetraScriptTester
    {
        public static void TestMain(string[] args)
        {
            if (args.Length != 1) Console.Error.WriteLine("Usage: MetraScript <input_file>");
            
            string fullpath;
            if (Path.IsPathRooted(args[0]))
                fullpath = args[0];
            else
                fullpath = Path.Combine(Environment.CurrentDirectory, args[0]);

            Console.Out.WriteLine("Processing file: {0}", fullpath);
            ICharStream input = new ANTLRFileStream(fullpath);
            MetraScriptLexer lex = new MetraScriptLexer(input);
            CommonTokenStream tokens = new CommonTokenStream(lex);
            MetraScriptParser parser = new MetraScriptParser(tokens);
            MetraScriptParser.start_return r = parser.start();
            
            ITree iTree = (ITree)r.Tree;
            Console.Out.WriteLine("tree=" + iTree.ToStringTree());
            

            //printDetail(iTree);
            prettyPrint(iTree);


            Antlr.Runtime.Tree.CommonTree t = new Antlr.Runtime.Tree.CommonTree();

            // all done
            Console.WriteLine("Press any key to exit");
            Console.ReadKey();
        }


        public static void prettyPrint(ITree iTree)
        {
            prettyPrint(iTree, 0);
        }

        public static void prettyPrint(ITree iTree, int depth)
        {
            string prefix = "  ".repeat(depth++);
            if (iTree.ChildCount == 0)
            {
                Console.WriteLine(prefix + iTree.Text);
            }
            else
            {
                Console.WriteLine(prefix + iTree.Text + "(");
                for (int i = 0; i < iTree.ChildCount; i++) prettyPrint(iTree.GetChild(i), depth);
                Console.WriteLine(prefix + ")");
            }
        }

        public static void printDetail(ITree iTree)
        {
            if (iTree.ChildCount == 0)
            {
                Console.WriteLine("[Text=" + iTree.Text + ",line=" + iTree.Line + ",pos=" + iTree.CharPositionInLine + "]");
            }
            else
            {
                Console.WriteLine("Text=" + iTree.Text + ",line=" + iTree.Line + ",pos=" + iTree.CharPositionInLine);
                Console.WriteLine("(");

                for (int i = 0; i < iTree.ChildCount; i++)
                {
                    printDetail(iTree.GetChild(i));
                }
                Console.WriteLine(")");
            }
        }
    }


}
