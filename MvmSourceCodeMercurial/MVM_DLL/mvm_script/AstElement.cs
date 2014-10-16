using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Antlr.Runtime;
using Antlr.Runtime.Tree;
using System.IO;
using MVM;

namespace MvmScript
{
    public class AstElement : AstNode
    {

        /// <summary>
        /// Override the default constructor
        /// </summary>
        /// <param name="t"></param>
        public AstElement(IToken t, ILocation location)
            : base(t, location)
        {
        }

        /// <summary>
        /// Returns the element as MvmScript (TBD:need to unroll parameters)...
        /// </summary>
        /// <returns></returns>
        public string ToCode()
        {
            StringBuilder sb = new StringBuilder();
            var astElement = this as AstElement;
            var nodeName = astElement.NodeName;
            if (nodeName != null)
                sb.AppendLine(nodeName + "=>");
            if (astElement.IsLiteral)
            {
                sb.Append(astElement.LiteralValue);
                if (this.HasDot)
                {
                    sb.Append(this.GetDotSubElements().Select(t => ".".Append(t.ToCode())).JoinStrings());
                }
            }
            else
            {
                sb.Append(astElement.Name);
                if (this.HasDot)
                {
                    sb.Append(this.GetDotSubElements().Select(t => ".".Append(t.ToCode())).JoinStrings());
                }
                if (this.HasTypeParameters)
                {
                    sb.Append("<");
                    sb.AppendLine(this.GetTypeParameterSubElements().Select(t => t.ToCode()).JoinStrings(","));
                    sb.Append(">");
                }
                if (this.HasParameters)
                {
                    sb.Append("(");
                    sb.Append(this.GetParameterSubElements().Select(t => t.ToCode()).JoinStrings(","));
                    sb.Append(")");
                }
                if (this.HasChildren)
                {
                    sb.Append("(");
                    sb.Append(this.GetChildrenSubElements().Select(t => t.ToCode()).JoinStrings(","));
                    sb.Append(")");
                }
                if (this.HasBrackets)
                {
                    sb.Append("[");
                    sb.Append(this.GetBracketSubElements().Select(t => t.ToCode().AppendLine()).JoinStrings(","));
                    sb.Append("]");
                }
                if (this.HasBraces)
                {
                    sb.AppendLine();
                    sb.AppendLine("{");
                    sb.Append(this.GetBraceSubElements().Select(t => t.ToCode().AppendLine(";")).JoinStrings());
                    sb.AppendLine("}");
                }
            }
            return sb.ToString();
        }

        // access sub elements.
        public IEnumerable<AstElement> GetDotSubElements()
        {
            foreach (var astElement in this.DotSubNodes().Select(n => n as AstElement)) 
                yield return astElement;
        }
        public IEnumerable<AstElement> GetParameterSubElements()
        {
            foreach (var astElement in this.ParameterSubNodes().Select(n => n as AstElement)) 
                yield return astElement;
        }
        public IEnumerable<AstElement> GetTypeParameterSubElements()
        {
            foreach (var astElement in this.TypeParameterSubNodes().Select(n => n as AstElement)) 
                yield return astElement;
        }
        public IEnumerable<AstElement> GetChildrenSubElements()
        {
            foreach (var astElement in this.ChildrenSubNodes().Select(n => n as AstElement)) 
                yield return astElement;
        }
        public IEnumerable<AstElement> GetBracketSubElements()
        {
            foreach (var astElement in this.BracketSubNodes().Select(n => n as AstElement)) 
                yield return astElement;
        }
        public IEnumerable<AstElement> GetBraceSubElements()
        {
            foreach (var astElement in this.BraceSubNodes().Select(n => n as AstElement)) 
                yield return astElement;
        }

        # region Access sub nodes
        /// <summary>
        /// Yields the dot sub nodes.
        /// </summary>
        /// <param name="astNode"></param>
        /// <returns></returns>
        public IEnumerable<AstNode> DotSubNodes()
        {
            foreach (var nodes in this.Children().Where(c => c.Type.In(MvmScriptParser.Ast_Dot)))
            {
                foreach (var node in nodes.Children())
                {
                    if (node is AstNodeNamer)
                        yield return (node as AstNodeNamer).Value;
                    else
                        yield return (AstNode)node;
                }
            }
        }
        /// <summary>
        /// Yields the parameter sub nodes.
        /// </summary>
        /// <param name="astNode"></param>
        /// <returns></returns>
        public IEnumerable<AstNode> ParameterSubNodes()
        {
            foreach (var nodes in this.Children().Where(c => c.Type.In(MvmScriptParser.Ast_Parameters)))
            {
                foreach (var node in nodes.Children())
                {
                    if (node is AstNodeNamer)
                        yield return (node as AstNodeNamer).Value;
                    else
                        yield return (AstNode)node;
                }
            }
        }
        /// <summary>
        /// Yields the type parameter sub nodes.
        /// </summary>
        /// <param name="astNode"></param>
        /// <returns></returns>
        public IEnumerable<AstNode> TypeParameterSubNodes()
        {
            foreach (var nodes in this.Children().Where(c => c.Type.In(MvmScriptParser.Ast_TypeParameters)))
            {
                foreach (var node in nodes.Children())
                {
                    if (node is AstNodeNamer)
                        yield return (node as AstNodeNamer).Value;
                    else
                        yield return (AstNode)node;
                }
            }
        }
        /// <summary>
        /// Yields the children sub nodes.
        /// </summary>
        /// <param name="astNode"></param>
        /// <returns></returns>
        public IEnumerable<AstNode> ChildrenSubNodes()
        {
            foreach (var nodes in this.Children().Where(c => c.Type.In(MvmScriptParser.Ast_Children)))
            {
                foreach (var node in nodes.Children())
                {
                    if (node is AstNodeNamer)
                        yield return (node as AstNodeNamer).Value;
                    else
                        yield return (AstNode)node;
                }
            }
        }
        /// <summary>
        /// Yields the bracket sub nodes.
        /// </summary>
        /// <param name="astNode"></param>
        /// <returns></returns>
        public IEnumerable<AstNode> BracketSubNodes()
        {
            foreach (var nodes in this.Children().Where(c => c.Type.In(MvmScriptParser.Ast_Bracket)))
            {
                foreach (var node in nodes.Children())
                {
                    if (node is AstNodeNamer)
                        yield return (node as AstNodeNamer).Value;
                    else
                        yield return (AstNode)node;
                }
            }
        }
        /// <summary>
        /// Yields the brace sub nodes.
        /// </summary>
        /// <param name="astNode"></param>
        /// <returns></returns>
        public IEnumerable<AstNode> BraceSubNodes()
        {
            foreach (var nodes in this.Children().Where(c => c.Type.In(MvmScriptParser.Ast_Brace)))
            {
                foreach (var node in nodes.Children())
                {
                    if (node is AstNodeNamer)
                        yield return (node as AstNodeNamer).Value;
                    else
                        yield return (AstNode)node;
                }
            }
        }
        # endregion

        /// <summary>
        /// Yields the child or attribute AstNodes of the current AstElement.
        /// </summary>
        /// <param name="astNode"></param>
        /// <returns></returns>
        override public IEnumerable<AstNode> GetSubNodes()
        {
            foreach (var nodes in this.Children().Where(c => c.Type.In(SubNodeTypes)))
            {
                foreach (var node in nodes.Children())
                {
                    if (node is AstNodeNamer)
                        yield return (node as AstNodeNamer).Value;
                    else
                        yield return (AstNode)node;
                }
            }
        }
        public AstElement NextSibling()
        {
            AstNode nodePtr = this;
            // if this node is named, gotta go up then over
            if (this.NodeName != null)
            {
                nodePtr = this.NodeNamer;
            }
            AstNode siblingNode = (AstNode)(nodePtr as ITree).NextSibling();
            if (siblingNode == null)
                return null;
            if (siblingNode is AstNodeNamer)
                return (AstElement)(siblingNode as AstNodeNamer).Value;
            else
                return (AstElement)siblingNode;
        }

        /// <summary>
        /// Returns the element name
        /// </summary>
        public override string Name
        {
            get
            {
                return this.ElementNameNode.Text;
            }
        }

        /// <summary>
        /// Returns the element name
        /// </summary>
        public AstNode ElementNameNode
        {
            get
            {
                return (AstNode)this.GetChild(0).GetChild(0);
            }
        }

        /// <summary>
        /// Return
        /// </summary>
        public bool IsLiteral
        {
            get
            {
                return this.ElementNameNode.Type.In(LiteralNodeTypes);
            }
        }

        public string LiteralValue
        {
            get
            {
                if (this.IsLiteral)
                {
                    return this.ParameterSubNodes().First().GetChild(0).Text;
                }
                return null;
            }
        }

        public string SelectNodeInnerText(string xpath)
        {
            foreach (AstElement elem in this.SelectNodes(xpath))
                return elem.InnerText;
            return null;
        }

        /// <summary>
        /// If this element was define like nodeName=>element, this returns AstNodeNamer,else null.
        /// </summary>
        /// <returns></returns>
        public AstNodeNamer NodeNamer
        {
            get
            {
                if (this.Parent == null) return null;
                if (this.Parent.Parent == null) return null;
                if (this.Parent.Parent.Type == MvmScriptParser.Ast_NodeNamer)
                {
                    return this.Parent.Parent as AstNodeNamer;
                }
                return null;
            }
        }

        /// <summary>
        /// If this element was define like nodeName=>element, this returns 'nodeName',else null.
        /// </summary>
        /// <returns></returns>
        public string NodeName
        {
            get
            {
                var nodeNamer = this.NodeNamer;
                if (nodeNamer == null) return null;
                return nodeNamer.Name;
            }
        }
        public int CountDotNodes
        {
            get
            {
                return this.Children().Where(x => x.Type.In(MvmScriptParser.Ast_Dot)).Count();
            }
        }
        public int CountTypeParameterNodes
        {
            get
            {
                return this.Children().Where(x => x.Type.In(MvmScriptParser.Ast_TypeParameters)).Count();
            }
        }
        public int CountParameterNodes
        {
            get
            {
                return this.Children().Where(x => x.Type.In(MvmScriptParser.Ast_Parameters)).Count();
            }
        }
        public int CountBraceNodes
        {
            get
            {
                return this.Children().Where(x => x.Type.In(MvmScriptParser.Ast_Brace)).Count();
            }
        }
        public int CountBracketNodes
        {
            get
            {
                return this.Children().Where(x => x.Type.In(MvmScriptParser.Ast_Bracket)).Count();
            }
        }
        public int CountChildrenNodes
        {
            get
            {
                return this.Children().Where(x => x.Type.In(MvmScriptParser.Ast_Children)).Count();
            }
        }


        public bool HasDot
        {
            get
            {
                return this.CountDotNodes>=1;
            }
        }
        
        public AstElement DotChild
        {
            get
            {
                return this.GetDotSubElements().FirstOrDefault();
            }
        }
        public bool HasDotFunction
        {
            get
            {
                return this.DotFunction != null;

            }
        }
        public AstElement DotFunction
        {
            get
            {
                var child = this.DotChild;
                if (child == null) return null;
                if (child.HasParameters) return child;
                return null;
            }
        }
        public bool HasDotProperty
        {
            get
            {
                return this.DotProperty != null;

            }
        }
        public AstElement DotProperty
        {
            get
            {
                var child = this.DotChild;
                if (child == null) return null;
                if (!child.HasParameters) return child;
                return null;
            }
        }
        
        public bool HasTypeParameters
        {
            get
            {
                return this.CountTypeParameterNodes >= 1;
            }
        }
        public bool HasParameters
        {
            get
            {
                return this.CountParameterNodes >= 1;
            }
        }
        public bool HasBraces
        {
            get
            {
                return this.CountBraceNodes>=1;
            }
        }
        public bool HasBrackets
        {
            get
            {
                return this.CountBracketNodes >= 1;
            }
        }
        public bool HasChildren
        {
            get
            {
                return this.CountChildrenNodes>=1;
            }
        }
        public override string ToString()
        {
            return this.OuterAst;
        }

    }
}
