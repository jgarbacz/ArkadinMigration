using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Antlr.Runtime;
using Antlr.Runtime.Tree;
namespace MVM
{
    public static class AntlrXPath
    {
        /// <summary>
        /// Parses the xpath expression and returns the xpath ITree.
        /// </summary>
        /// <param name="syntax"></param>
        /// <returns></returns>
        public static ITree ParseXPath(string syntax)
        {
            try
            {
                //Console.WriteLine("ParseXPath=" + syntax); 
                ICharStream input = new ANTLRStringStream(syntax);
                XPathLexer lex = new XPathLexer(input);
                CommonTokenStream tokens = new CommonTokenStream(lex);
                XPathParser parser = new XPathParser(tokens);
                XPathParser.main_return r = parser.main();
                ITree iTree = (ITree)r.Tree;
                return iTree;
            }
            catch (Antlr.Runtime.NoViableAltException e)
            {
                StringBuilder msg = new StringBuilder();
                msg.AppendLine("Error, cannot parse xpath syntax [" + syntax + "]");
                msg.AppendLine("Msg:" + e.Message);
                msg.AppendLine("Line:" + e.Line);
                msg.AppendLine("Pos:" + e.CharPositionInLine);
                throw new Exception(msg.ToString(), e);
            }
            catch (Exception e)
            {
                StringBuilder msg = new StringBuilder();
                msg.AppendLine("Error, cannot parse xpath syntax [" + syntax + "]");
                throw new Exception(msg.ToString(), e);
            }
        }
        public static IEnumerable<ITree> XPathOperator(ITree xpathTree, ITree antlrTree)
        {
            foreach (ITree output in XPathOperator(xpathTree, 0, antlrTree))
                yield return output;
        }
        public static IEnumerable<ITree> XPathOperator(ITree xpathTree, int childNo, ITree antlrTree)
        {
            ITree xpathOperator = xpathTree.GetChild(childNo);
            switch (xpathOperator.Type)
            {
                case XPathParser.RELATIVE_PATH:
                    foreach (ITree output in XPathRelativePath(xpathOperator, antlrTree))
                        yield return output;
                    break;
                case XPathParser.ABSOLUTE_PATH:
                    foreach (ITree output in XPathAbsolutePath(xpathOperator, antlrTree))
                        yield return output;
                    break;
                case XPathParser.PIPE:
                    foreach (ITree output in XPathUnion(xpathOperator, antlrTree))
                        yield return output;
                    break;
                case XPathParser.MATCH:
                    foreach (ITree output in XPathMatch(xpathTree, childNo, antlrTree))
                        yield return output;
                    break;
                case XPathParser.RECURSIVE_MATCH:
                    foreach (ITree output in XPathRecursiveMatch(xpathTree, childNo, antlrTree))
                        yield return output;
                    break;
                case XPathParser.CURRENT_NODE:
                    foreach (ITree output in XPathCurrentNode(xpathTree, childNo, antlrTree))
                        yield return output;
                    break;
                case XPathParser.TRAVERSE_UP:
                    foreach (ITree output in XPathTraverseUp(xpathTree, childNo, antlrTree))
                        yield return output;
                    break;
                default:
                    throw new Exception("Unexpected xpath operator [" + xpathOperator.Text + "]");
            }
        }
        private static IEnumerable<ITree> XPathCurrentNode(ITree xpathTree, int childNo, ITree antlrTree)
        {
            ITree xpathOperator = xpathTree.GetChild(childNo);
            bool isFinalOperator = xpathTree.ChildCount == childNo + 1;
            if (isFinalOperator)
                yield return antlrTree;
            else
                foreach (ITree output in XPathOperator(xpathTree, childNo + 1, antlrTree))
                    yield return output;
        }
        private static IEnumerable<ITree> XPathTraverseUp(ITree xpathTree, int childNo, ITree antlrTree)
        {
            ITree xpathOperator = xpathTree.GetChild(childNo);
            bool isFinalOperator = xpathTree.ChildCount == childNo + 1;
            if (isFinalOperator)
                yield return antlrTree.Parent;
            else
                foreach (ITree output in XPathOperator(xpathTree, childNo + 1, antlrTree.Parent))
                    yield return output;
        }
        private static IEnumerable<ITree> XPathMatch(ITree xpathTree, int childNo, ITree antlrTree)
        {
            ITree xpathOperator = xpathTree.GetChild(childNo);
            bool isFinalOperator = xpathTree.ChildCount == childNo + 1;
            string match = xpathOperator.GetChild(0).Text;
            for (int i = 0; i < antlrTree.ChildCount; i++)
            {
                ITree antlrChild = antlrTree.GetChild(i);
                if (antlrChild.Text.Equals(match)||match.Equals("*"))
                {
                    if (isFinalOperator)
                        yield return antlrChild;
                    else
                        foreach (ITree output in XPathOperator(xpathTree, childNo + 1, antlrChild))
                            yield return output;
                }
            }
        }
        private static IEnumerable<ITree> XPathRecursiveMatch(ITree xpathTree, int childNo, ITree antlrTree)
        {
            ITree xpathOperator = xpathTree.GetChild(childNo);
            bool isFinalOperator = xpathTree.ChildCount == childNo + 1;
            ITree matchTree = xpathOperator.GetChild(0);
            foreach (ITree antlrChild in antlrTree.WalkChildren())
            {
                if (matchTree.Type == XPathParser.MUL || antlrChild.Text.Equals(matchTree.Text))
                {
                    if (isFinalOperator)
                        yield return antlrChild;
                    else
                        foreach (ITree output in XPathOperator(xpathTree, childNo + 1, antlrChild))
                            yield return output;
                }
            }
        }
        private static IEnumerable<ITree> XPathUnion(ITree xpathTree, ITree antlrTree)
        {
            foreach (ITree childXpathTree in xpathTree.Children())
            {
                foreach (ITree output in XPathOperator(childXpathTree, 0, antlrTree))
                    yield return output;
            }
        }
        private static IEnumerable<ITree> XPathRelativePath(ITree xpathTree, ITree antlrTree)
        {
            foreach (ITree output in XPathOperator(xpathTree, 0, antlrTree))
                yield return output;
        }
        private static IEnumerable<ITree> XPathAbsolutePath(ITree xpathTree, ITree antlrTree)
        {
            ITree rootTree = antlrTree.GetRoot();
            foreach (ITree output in XPathOperator(xpathTree, 0, rootTree))
                yield return output;
        }
        /// <summary>
        /// Depth first, Lazy evaluates the xpath expression on the antlrTree. No concept of attributes.
        /// Supports:
        /// - '/absolute/paths'
        /// - './relative/paths'
        /// - '.'
        /// - '..'
        /// - '|' union
        /// - '*' wildcard
        /// </summary>
        /// <param name="antlrTree"></param>
        /// <param name="xpath"></param>
        /// <returns></returns>
        public static IEnumerable<ITree> SelectNodes(this ITree antlrTree, string xpath)
        {
            ITree xpathTree = ParseXPath(xpath);
            //xpathTree.PrettyPrint();
            foreach (ITree output in XPathOperator(xpathTree, antlrTree))
                yield return (ITree)output;
        }

        /// <summary>
        /// Evaluates the xpath using <code>AntlrXPath.SelectNodes()</code> and 
        /// materializes the output as a List.
        /// </summary>
        /// <param name="antlrTree"></param>
        /// <param name="xpath"></param>
        /// <returns></returns>
        public static List<ITree> SelectNodeList(this ITree antlrTree, string xpath)
        {
            return antlrTree.SelectNodes(xpath).ToList();
        }

        public static void Test()
        {
            string syntax = @"f(x);";
            Console.WriteLine("SYNTAX.....".AppendLine(syntax));
            ITree mytree = MvmScript.AstNode.ParseSyntax(syntax);
            Console.WriteLine("PRINT TREE.....");
            mytree.PrettyPrint();
            Console.WriteLine("TEST XPATHS....");
            string[] xpaths = new string[] {
                "/Ast_Primary",
                "Ast_Primary",
                "//Ast_Primary",
                "./Ast_Primary",
                "//Ast_Element/Ast_ElementName"
            };

            //string syntax = "ENTITY.x,ENTITY.y,99.1=funcy(22,'abc',PARENT.f,9)";
            //ITree mytree = MGenerateEntities.ParseEntitySyntax(syntax);
            //Console.WriteLine("Full tree:");
            //Console.WriteLine(mytree.ToStringTree());
            //Console.WriteLine("parent="+mytree.Parent.Nvl("IS_NULL"));

            //Console.WriteLine("walk children:");
            //foreach (var x in mytree.WalkChildren())
            //{
            //    Console.WriteLine(x.Text);
            //}
            
            /*
            string[] xpaths = new string[] {
                "./LEFT",
                "./nohits",
                "//FUNCTION/*",
                "LEFT|RIGHT",
                "LEFT/ENTITY", 
                "RIGHT/FUNCTION", 
                "//FUNCTION", 
                "//LITERAL_INT",
                "//FUNCTION_NAME/.." 
            };
             */
            foreach (string xpath in xpaths)
            {
                Console.WriteLine("RUNNING xpath=" + xpath + ":");

                foreach (var t in mytree.SelectNodes(xpath))
                {
                    Console.WriteLine("SELECTED: "+ t.ToStringTree());
                }
            }
        }
    }
}
