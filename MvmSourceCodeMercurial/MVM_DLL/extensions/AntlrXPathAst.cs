using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Antlr.Runtime;
using Antlr.Runtime.Tree;
using MVM;

namespace MvmScript
{
    public static class AntlrXPathAst
    {
        ///// <summary>
        ///// Parses the xpath expression and returns the xpath ITree.
        ///// </summary>
        ///// <param name="syntax"></param>
        ///// <returns></returns>
        //public static ITree ParseXPath(string syntax)
        //{
        //    try
        //    {
        //        //Console.WriteLine("ParseXPath=" + syntax); 
        //        ICharStream input = new ANTLRStringStream(syntax);
        //        XPathLexer lex = new XPathLexer(input);
        //        CommonTokenStream tokens = new CommonTokenStream(lex);
        //        XPathParser parser = new XPathParser(tokens);
        //        XPathParser.main_return r = parser.main();
        //        ITree iTree = (ITree)r.Tree;
        //        //Console.WriteLine("tree=" + iTree.ToStringTree());
        //        return iTree;
        //    }
        //    catch (Antlr.Runtime.NoViableAltException e)
        //    {
        //        StringBuilder msg = new StringBuilder();
        //        msg.AppendLine("Error, cannot parse xpath syntax [" + syntax + "]");
        //        msg.AppendLine("Msg:" + e.Message);
        //        msg.AppendLine("Line:" + e.Line);
        //        msg.AppendLine("Pos:" + e.CharPositionInLine);
        //        throw new Exception(msg.ToString(), e);
        //    }
        //    catch (Exception e)
        //    {
        //        StringBuilder msg = new StringBuilder();
        //        msg.AppendLine("Error, cannot parse xpath syntax [" + syntax + "]");
        //        throw new Exception(msg.ToString(), e);
        //    }
        //}
        //public static IEnumerable<ITree> XPathOperator(ITree xpathTree, AstNode astNode)
        //{
        //    foreach (ITree output in XPathOperator(xpathTree, 0, astNode))
        //        yield return output;
        //}
        //private static IEnumerable<ITree> XPathOperator(ITree xpathTree, int childNo, AstNode astNode)
        //{
        //    ITree xpathOperator = xpathTree.GetChild(childNo);
        //    switch (xpathOperator.Type)
        //    {
        //        case XPathParser.RELATIVE_PATH:
        //            foreach (ITree output in XPathRelativePath(xpathOperator, astNode))
        //                yield return output;
        //            break;
        //        case XPathParser.ABSOLUTE_PATH:
        //            foreach (ITree output in XPathAbsolutePath(xpathOperator, astNode))
        //                yield return output;
        //            break;
        //        case XPathParser.PIPE:
        //            foreach (ITree output in XPathUnion(xpathOperator, astNode))
        //                yield return output;
        //            break;
        //        case XPathParser.MATCH:
        //            foreach (ITree output in XPathMatch(xpathTree, childNo, astNode))
        //                yield return output;
        //            break;
        //        case XPathParser.RECURSIVE_MATCH:
        //            foreach (ITree output in XPathRecursiveMatch(xpathTree, childNo, astNode))
        //                yield return output;
        //            break;
        //        case XPathParser.CURRENT_NODE:
        //            foreach (ITree output in XPathCurrentNode(xpathTree, childNo, astNode))
        //                yield return output;
        //            break;
        //        case XPathParser.TRAVERSE_UP:
        //            foreach (ITree output in XPathTraverseUp(xpathTree, childNo, astNode))
        //                yield return output;
        //            break;
        //        default:
        //            throw new Exception("Unexpected xpath operator [" + xpathOperator.Text + "]");
        //    }
        //}
        //public static IEnumerable<ITree> XPathCurrentNode(ITree xpathTree, int childNo, AstNode astNode)
        //{
        //    ITree xpathOperator = xpathTree.GetChild(childNo);
        //    bool isFinalOperator = xpathTree.ChildCount == childNo + 1;
        //    if (isFinalOperator)
        //        yield return astNode;
        //    else
        //        foreach (AstNode output in XPathOperator(xpathTree, childNo + 1, astNode))
        //            yield return output;
        //}
        //public static IEnumerable<ITree> XPathTraverseUp(ITree xpathTree, int childNo, AstNode astNode)
        //{
        //    ITree xpathOperator = xpathTree.GetChild(childNo);
        //    bool isFinalOperator = xpathTree.ChildCount == childNo + 1;
        //    if (isFinalOperator)
        //        yield return astNode.Parent;
        //    else
        //        foreach (AstNode output in XPathOperator(xpathTree, childNo + 1, astNode.ParentNode()))
        //            yield return output;
        //}
        //private static IEnumerable<ITree> XPathMatch(ITree xpathTree, int childNo, AstNode astNode)
        //{
        //    ITree xpathOperator = xpathTree.GetChild(childNo);
        //    bool isFinalOperator = xpathTree.ChildCount == childNo + 1;
        //    string match = xpathOperator.GetChild(0).Text;
        //    foreach (AstNode antlrChild in astNode.ChildNodes())
        //    {
        //        if (antlrChild.Text.Equals(match)||match.Equals("*"))
        //        {
        //            if (isFinalOperator)
        //                yield return antlrChild;
        //            else
        //                foreach (AstNode output in XPathOperator(xpathTree, childNo + 1, antlrChild))
        //                    yield return output;
        //        }
        //    }
        //}
        //private static IEnumerable<ITree> XPathRecursiveMatch(ITree xpathTree, int childNo, AstNode astNode)
        //{
        //    ITree xpathOperator = xpathTree.GetChild(childNo);
        //    bool isFinalOperator = xpathTree.ChildCount == childNo + 1;
        //    ITree matchTree = xpathOperator.GetChild(0);
        //    foreach (AstNode antlrChild in astNode.WalkChildElements())
        //    {
        //        if (matchTree.Type == XPathParser.MUL || antlrChild.Text.Equals(matchTree.Text))
        //        {
        //            if (isFinalOperator)
        //                yield return antlrChild;
        //            else
        //                foreach (AstNode output in XPathOperator(xpathTree, childNo + 1, antlrChild))
        //                    yield return output;
        //        }
        //    }
        //}
        //private static IEnumerable<ITree> XPathUnion(ITree xpathTree, AstNode astNode)
        //{
        //    foreach (ITree childXpathTree in xpathTree.Children())
        //    {
        //        foreach (AstNode output in XPathOperator(childXpathTree, 0, astNode))
        //            yield return output;
        //    }
        //}
        //private static IEnumerable<ITree> XPathRelativePath(ITree xpathTree, AstNode astNode)
        //{
        //    foreach (AstNode output in XPathOperator(xpathTree, 0, astNode))
        //        yield return output;
        //}
        //private static IEnumerable<ITree> XPathAbsolutePath(ITree xpathTree, AstNode astNode)
        //{
        //    AstNode rootTree = astNode.GetRoot() as AstNode;
        //    foreach (AstNode output in XPathOperator(xpathTree, 0, rootTree))
        //        yield return output;
        //}
    }
}
