using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Antlr.Runtime;
using Antlr.Runtime.Tree;
using MVM;

namespace MvmScript
{
    public static class MyAntlrITreeAst
    {
        //// Users are going to want to generate ast piece meal... Need an api for doing the
        //// simple stuff like creating an attribute node and setting it to a value.
        //// Simple stuff should abstract out complications with literals.
        ////
        //// We can auto inherit the location from the passed iTree (better than nothing).
        //// if we want to we can using something besides CommonTree so we can set additional
        //// attributes.
        //public static ITree AstCreateAttributeNode(this ITree iTree,string attributeName)
        //{
        //    ITree attributeNode = new CommonTree(new CommonToken(MvmScriptLexer.Ast_NodeNamer));
        //    //ITree attributeName = new CommonTree(new CommonToken(MvmScriptLexer.Ast_AttributeName));
        //    //ITree attributeValue = new CommonTree(new CommonToken(MvmScriptLexer.Ast_AttributeValue));
        //    return attributeNode;
        //}
        //public static ITree AstCreateValueNode(this ITree iTree, string attributeName)
        //{
        //    ITree attributeValue = new CommonTree(new CommonToken(MvmScriptLexer.Ast_AttributeValue));
        //    return attributeValue;
        //}
        //public static string AstNodeTypeName(this ITree iTree)
        //{
        //    return MvmScriptParser.tokenNames[iTree.Type];
        //}

        ///// <summary>
        ///// Appends a child AstNode to the iTree element.
        ///// </summary>
        ///// <param name="iTree"></param>
        ///// <param name="attributeName"></param>
        ///// <returns></returns>
        //public static ITree AstAppendChild(this ITree astElementNode, ITree astNode)
        //{
        //    if (!astElementNode.AstIsElementNode())
        //        throw new ArgumentException("AstAppendChild not valid for nodeType:" + astElementNode.AstNodeTypeName(),"astElementNode");
        //    if(!astNode.AstIsNode())
        //        throw new ArgumentException("AstAppendChild not valid for nodeType:" + astNode.AstNodeTypeName(), "astNode");
        //    if (!astElementNode.AstElementNodeHasChildNodesITree()) 
        //        astElementNode.AstElementNodeAddChildNodesITree();
        //    return astNode;
        //}
        //public static bool AstElementNodeHasChildNodesITree(this ITree astElementNode)
        //{
        //    if (!astElementNode.AstIsElementNode())
        //        throw new ArgumentException("AstAppendChild not valid for nodeType:" + astElementNode.AstNodeTypeName(),"astElementNode");
        //    return astElementNode.Children().Where(n => n.Type == MvmScriptParser.Ast_ChildNodes).Count() > 0;
        //}
        //public static void AstElementNodeAddChildNodesITree(this ITree itree)
        //{
        //}


        ///// <summary>
        ///// Returns true if the antlr tree is an AST attribute, element, or value node
        ///// </summary>
        ///// <param name="itree"></param>
        ///// <returns></returns>
        //public static bool AstIsNode(this ITree itree)
        //{
        //    return itree.Type.In(
        //        MvmScriptLexer.Ast_NodeNamer, 
        //        MvmScriptLexer.Ast_Element, 
        //        MvmScriptLexer.Ast_Value
        //        );
        //}
        ///// <summary>
        ///// Tests if passed antlr tree is an ast attribute node.
        ///// </summary>
        ///// <param name="itree"></param>
        ///// <returns></returns>
        //public static bool AstIsAttributeNode(this ITree itree)
        //{
        //    return itree.Type == MvmScriptLexer.Ast_NodeNamer;
        //}
        ///// <summary>
        ///// Tests if passed antlr tree is an ast value node.
        ///// </summary>
        ///// <param name="itree"></param>
        ///// <returns></returns>
        //public static bool AstIsValueNode(this ITree itree)
        //{
        //    return itree.Type == MvmScriptLexer.Ast_Value;
        //}
        ///// <summary>
        ///// Tests if passed antlr tree is an ast Element node.
        ///// </summary>
        ///// <param name="itree"></param>
        ///// <returns></returns>
        //public static bool AstIsElementNode(this ITree itree)
        //{
        //    return itree.Type == MvmScriptLexer.Ast_Element;
        //}

        ///// <summary>
        ///// Yields the current node and all recursive children.
        ///// </summary>
        ///// <param name="itree"></param>
        ///// <returns></returns>
        //public static IEnumerable<ITree> AstWalkElements(this ITree astElementNode)
        //{
        //    if (!astElementNode.AstIsElementNode())
        //        throw new ArgumentException("AstWalkElements expect Ast_ElementNode not [" + astElementNode.AstNodeTypeName()+"]", " astElementNode");
        //    yield return astElementNode;
        //    foreach (ITree childElement in astElementNode.AstWalkChildElements())
        //        yield return childElement;
        //}

        ///// <summary>
        ///// Yields all the recursive children.
        ///// </summary>
        ///// <param name="itree"></param>
        ///// <returns></returns>
        //public static IEnumerable<ITree> AstWalkChildElements(this ITree itree)
        //{
        //    foreach (ITree childElement in itree.AstChildElements())
        //    {
        //        foreach (ITree output in childElement.AstWalkElements())
        //            yield return output;
        //    }
        //}


        //// Returns the parent AstNode of the current AstNode
        //public static ITree AstParentNode(this ITree astNode)
        //{
        //    if (!astNode.AstIsNode())
        //        throw new ArgumentException("AstParentNode not valid for nodeType:" + astNode.AstNodeTypeName(), "astNode");
        //    if (astNode.Parent.Parent.Type == MvmScriptParser.Ast_ElementNode) 
        //        return astNode.Parent.Parent;
        //    return null;
        //}

        ///// <summary>
        ///// Returns the child AstNodes of the current AstNode.
        ///// </summary>
        ///// <param name="astNode"></param>
        ///// <returns></returns>
        //public static IEnumerable<ITree> AstChildNodes(this ITree astNode)
        //{
        //    if (!astNode.AstIsNode())
        //        throw new ArgumentException("AstChildNodes not valid for nodeType:" + astNode.AstNodeTypeName(), "astNode");
        //    if (astNode.AstIsElementNode())
        //    {
        //        foreach (var childNodes in astNode.Children().Where(c => c.Type == MvmScriptParser.Ast_ChildNodes))
        //        {
        //            foreach (var child in childNodes.Children())
        //            {
        //                yield return child;
        //            }
        //        }
        //    }
        //}
        ///// <summary>
        ///// Returns immediate child elements of the passed element
        ///// </summary>
        ///// <param name="astNode"></param>
        ///// <returns></returns>
        //public static IEnumerable<ITree> AstChildElements(this ITree astNode)
        //{
        //    if (!astNode.AstIsNode())
        //        throw new ArgumentException("AstChildNodes not valid for nodeType:" + astNode.AstNodeTypeName(), "astNode");
        //    if (astNode.AstIsElementNode())
        //    {
        //        foreach (var childNodes in astNode.Children().Where(c => c.Type == MvmScriptParser.Ast_ChildNodes))
        //        {
        //            foreach (var child in childNodes.Children().Where(c=>c.AstIsElementNode()))
        //            {
        //                yield return child;
        //            }
        //        }
        //    }
        //}

        ///*
        // *
        // * OK, I KNOW I CAN PARSE, LETS TRY TO PUT THE TREE BACK TO AST BUT WITH EXPRESSION.
        // * can't tell the difference between code and args. intensionally... since we 
        // * leave it to the module to decide.
        // * 
        // */



        ////public string ToXml()
        ////{
        ////    StringBuilder sb = new StringBuilder();
        ////    return sb.ToString();
        ////}



    }
}
