using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Antlr.Runtime;
using Antlr.Runtime.Tree;
using System.IO;
using MVM;
using System.Xml;

namespace MvmScript
{
    public abstract class AstNode : CommonTree
    {
        public static readonly int[] SubNodeTypes = new int[] { 
            MvmScriptParser.Ast_Children,
            MvmScriptParser.Ast_Dot, 
            MvmScriptParser.Ast_Bracket,
            MvmScriptParser.Ast_Parameters,
            MvmScriptParser.Ast_TypeParameters,
            MvmScriptParser.Ast_Brace, 
            MvmScriptParser.Ast_Children 
        };
        public static readonly int[] LiteralNodeTypes = new int[] { 
            MvmScriptParser.Syn_literalString,
            MvmScriptParser.Syn_LiteralNull, 
            MvmScriptParser.Syn_LiteralInt,
            MvmScriptParser.Syn_LiteralFloat, 
            MvmScriptParser.Syn_LiteralBool
        };

        public static readonly int[] AstNodeTypes = new int[]{
            MvmScriptLexer.Ast_NodeNamer,
            MvmScriptLexer.Ast_Element,
            MvmScriptLexer.Ast_Value
        };

        public string LocalName
        {
            get
            {
                return this.Name;
            }
        }

        /// <summary>
        /// Parses the passed MvmScript file and return the root AstNode
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public static AstNode ParseFile(string file)
        {
            AstNode astNode = ParseFileNoBuildAst(file);
            return BuildAst(astNode);
        }

        public static AstNode ParseFileNoBuildAst(string file)
        {
            string fullpath;
            if (Path.IsPathRooted(file))
                fullpath = file;
            else
                fullpath = Path.Combine(Environment.CurrentDirectory, file);
            try
            {
                ICharStream input = new ANTLRFileStream(fullpath);
                MvmScriptLexer lex = new MvmScriptLexer(input);
                CommonTokenStream tokens = new CommonTokenStream(lex);
                MvmScriptParser parser = new MvmScriptParser(tokens);
                AstNodeAdaptor adaptor = new AstNodeAdaptor(new FileLocation(file));
                parser.TreeAdaptor = adaptor;
                MvmScriptParser.start_return r = parser.start();
                return (AstNode)r.Tree;
            }
            catch (MvmScriptParserException e)
            {
                e.FromLocation = new FileLocation(file);
                throw e;
            }
            catch (Exception e)
            {
                StringBuilder msg = new StringBuilder();
                msg.AppendLine("Error, cannot parse file [" + file + "]");
                throw new Exception(msg.ToString(), e);
            }
        }

        /// <summary>
        /// Parses the passed MvmScript syntax and returns the root AstNode.
        /// </summary>
        /// <param name="syntax"></param>
        /// <returns></returns>
        public static AstNode ParseSyntax(string syntax)
        {
            AstNode astNode = ParseAstSyntax(syntax);
            return BuildAst(astNode);
        }

        private static AstNode ParseAstSyntax(string syntax)
        {
            try
            {
                //Console.WriteLine("ParseMvmScript=" + syntax); 
                ICharStream input = new ANTLRStringStream(syntax);
                MvmScriptLexer lex = new MvmScriptLexer(input);
                CommonTokenStream tokens = new CommonTokenStream(lex);
                MvmScriptParser parser = new MvmScriptParser(tokens);
                AstNodeAdaptor adaptor = new AstNodeAdaptor(new FileLocation("csharp_code"));
                parser.TreeAdaptor = adaptor;
                MvmScriptParser.start_return r = parser.start();
                if (r.Tree is CommonErrorNode)
                {
                    var err = r.Tree as CommonErrorNode;
                    throw new Exception("Antlr error:" + err.Text, err.trappedException);
                }
                return (AstNode)r.Tree;
            }
            catch (MvmScriptParserException e)
            {
                throw e;
            }
            catch (Exception e)
            {
                StringBuilder msg = new StringBuilder();
                msg.AppendLine("Error, cannot parse MvmScript syntax [" + syntax + "]");
                throw new Exception(msg.ToString(), e);
            }
        }

        public static AstNode ParseStatement(string syntax)
        {
            AstNode astNode = ParseAstStatement(syntax);
            return BuildAst(astNode);
        }
        private static AstNode ParseAstStatement(string syntax)
        {
            // add the requisite semi colon.
            syntax = syntax + ";";
            try
            {
                //Console.WriteLine("ParseMvmScript=" + syntax); 
                ICharStream input = new ANTLRStringStream(syntax);
                MvmScriptLexer lex = new MvmScriptLexer(input);
                CommonTokenStream tokens = new CommonTokenStream(lex);
                MvmScriptParser parser = new MvmScriptParser(tokens);
                AstNodeAdaptor adaptor = new AstNodeAdaptor(new FileLocation("csharp_code"));
                parser.TreeAdaptor = adaptor;
                MvmScriptParser.statement_return r = parser.statement();
                if (r.Tree is CommonErrorNode)
                {
                    var err = r.Tree as CommonErrorNode;
                    throw new Exception("Antlr error:" + err.Text, err.trappedException);
                }
                return (AstNode)r.Tree;
            }
            catch (MvmScriptParserException e)
            {
                throw e;
            }
            catch (Exception e)
            {
                StringBuilder msg = new StringBuilder();
                msg.AppendLine("Error, cannot parse MvmScript syntax [" + syntax + "]");
                throw new Exception(msg.ToString(), e);
            }
        }

        public static AstNode BuildAst(AstNode astNode)
        {
            // make sure the ast node is nil, otherwise stuff it under dummy nil
            AstNode root;
            if (!astNode.IsNil)
            {
                root = new AstData(null, astNode.location);
                root.AddChild(astNode);
            }
            else
            {
                root = astNode;
            }

            //Console.WriteLine("----ROOT-----");
            //root.PrettyPrint();

            // fold secondarys into primarys
            foreach (ITree secondary in (root as ITree).SelectNodes("//Ast_Secondary"))
            {
                ITree primary = secondary.PreviousSibling();
                if (primary == null) throw new Exception("Expecting primary before secondary");
                foreach (var c in secondary.Children())
                {
                    primary.AddChild(c);
                }
                secondary.DeleteSelf();
                //Console.WriteLine("---primary(after secondary)---");
                //primary.PrettyPrint();
            }

            // Process the primaries
            foreach (ITree primary in (root as ITree).SelectNodes("//Ast_Primary"))
            {

                //Console.WriteLine("---primary---");
                //primary.PrettyPrint();

                if (primary.ChildCount == 0) throw new Exception("unexpected, primary should have children");
                // if only 1 kid, let kid replace me.
                if (primary.ChildCount == 1)
                {
                    primary.ReplaceSelfWith(primary.GetFirstChild());
                    continue;
                }
                // if more then one kid we need to chain them up together.
                else
                {
                    // e can be a child of e,[,(
                    // { can be a child of e, and ( 'but only if grandparent is elem
                    // ( can be a child of e, and ( 'but only if granparent is e'
                    // [ can be a child of e,[, and ( 'but only if grandparent!= ('
                    AstElement elemPtr = null;
                    foreach (AstNode n in primary.Children())
                    {
                        if (elemPtr == null) elemPtr = (AstElement)n;
                        else if (n.Type == MvmScriptParser.Ast_Dot)
                        {
                            elemPtr.AddChild(n);
                            elemPtr = (AstElement)n.GetFirstChild();
                        }
                        else if (n.Type == MvmScriptParser.Ast_Parameters)
                        {
                            if (elemPtr.CountBraceNodes >= 1)
                                throw new Exception("Unexpected paren '(' at " + n.Location + ". Paren cannot follow '}'.");
                            if (elemPtr.CountParameterNodes + elemPtr.CountChildrenNodes >= 2)
                                throw new Exception("Unexpected paren '(' at " + n.Location + ". Paren cannot follow ')'.");
                            if (elemPtr.CountBracketNodes >= 1)
                                throw new Exception("Unexpected paren '(' at " + n.Location + ". Paren cannot follow ']'.");
                            if (elemPtr.CountParameterNodes >= 2)
                                throw new Exception("Unexpected paren '(' at " + n.Location + ". Paren cannot follow ')'.");
                            if (elemPtr.CountParameterNodes >= 1)
                            {
                                var astChildren = (n as AstNode).ChangeToken(new CommonToken(MvmScriptParser.Ast_Children, "Ast_Children"));
                                elemPtr.AddChild(astChildren);
                            }
                            else
                            {
                                elemPtr.AddChild(n);
                            }
                        }
                        else if (n.Type == MvmScriptParser.Ast_TypeParameters)
                        {
                            if (elemPtr.CountBraceNodes >= 1)
                                throw new Exception("Unexpected left angle '<' at " + n.Location + ". Left angle cannot follow '}'.");
                            if (elemPtr.CountBracketNodes >= 1)
                                throw new Exception("Unexpected left angle '<' at " + n.Location + ". Left angle cannot follow ']'.");
                            if (elemPtr.CountParameterNodes >= 2)
                                throw new Exception("Unexpected left angle '(' at " + n.Location + ". Left angle cannot follow ')'.");
                            elemPtr.AddChild(n);
                        }
                        else if (n.Type == MvmScriptParser.Ast_Brace)
                        {
                            if (elemPtr.CountBraceNodes >= 1)
                                throw new Exception("Unexpected brace '{' at " + n.Location + ". Brace cannot follow '}'.");
                            if (elemPtr.CountParameterNodes + elemPtr.CountChildrenNodes >= 2)
                                throw new Exception("Unexpected brace '{' at " + n.Location + ". Brace cannot follow ')'.");
                            if (elemPtr.CountBracketNodes >= 1)
                                throw new Exception("Unexpected paren '{' at " + n.Location + ". Brace cannot follow ']'.");
                            elemPtr.AddChild(n);
                        }
                        else if (n.Type == MvmScriptParser.Ast_Bracket)
                        {
                            if (elemPtr.CountBraceNodes >= 1)
                                throw new Exception("Unexpected bracket '[' at " + n.Location + ". Bracket cannot follow '}'.");
                            if (elemPtr.CountParameterNodes + elemPtr.CountChildrenNodes >= 2)
                                throw new Exception("Unexpected bracket '[' at " + n.Location + ". Bracket cannot follow ')'.");
                            elemPtr.AddChild(n);
                            elemPtr = (AstElement)n.GetFirstChild();
                        }
                        else
                        {
                            throw new Exception("not supported: " + n);
                        }
                    }
                    primary.ReplaceSelfWith(primary.GetFirstChild());
                }
                //Console.WriteLine("----ROOT again-----");
                //root.PrettyPrint();
            }
            //Console.WriteLine("----OUTER AST-----");
            //Console.WriteLine(root.OuterAst);
            return root;
        }

        /// <summary>
        /// Changes the token of the current node in place.
        /// </summary>
        /// <param name="iTree"></param>
        /// <returns></returns>
        public AstNode ChangeToken(IToken token)
        {
            AstNode newNode = this.CreateNode(token);
            if (this.Children != null)
            {
                foreach (ITree childNode in this.Children)
                    newNode.AddChild(childNode);
            }
            var replacement = (AstNode)this.ReplaceSelfWith(newNode);
            return replacement;
        }



        public AstNodeAdaptor astNodeAdapter = new AstNodeAdaptor();
        public AstNode CreateNode(IToken token)
        {
            return (AstNode)astNodeAdapter.Create(token);
        }

        public ILocation location;
        /// <summary>
        /// Override the default constructor
        /// </summary>
        /// <param name="t"></param>
        public AstNode(IToken t, ILocation location)
            : base(t)
        {
        }

        /// <summary>
        /// Links the current node to the node that created it. This keeps audit trail when one node generates another.
        /// </summary>
        public readonly AstNode FromNode;

        /// <summary>
        /// Constructor for generated nodes.
        /// </summary>
        /// <param name="t"></param>
        public AstNode(IToken t, AstNode fromNode)
            : base(t)
        {
            this.FromNode = fromNode;
        }

        /// <summary>
        /// Creates an attribute from the current node.
        /// </summary>
        /// <param name="attributeName"></param>
        /// <returns></returns>
        public AstNode CreateAttribute(string attributeName)
        {
            //var attributeNode = new AstNode(new CommonToken(MvmScriptLexer.Ast_NodeNamer), this);
            //AstNode attributeNode = new CommonTree(new CommonToken(MvmScriptLexer.Ast_NodeNamer));
            //AstNode attributeName = new CommonTree(new CommonToken(MvmScriptLexer.Ast_AttributeName));
            //AstNode attributeValue = new CommonTree(new CommonToken(MvmScriptLexer.Ast_AttributeValue));
            //return attributeNode;
            return null;
        }

        public string NodeTypeName
        {
            get
            {
                return MvmScriptParser.tokenNames[this.Type];
            }
        }

        /// <summary>
        /// Returns true if node is an AST attribute, element, or value node
        /// </summary>
        /// <param name="AstNode"></param>
        /// <returns></returns>
        public bool IsAstNode
        {
            get
            {
                return this.Type.In(AstNodeTypes);
            }
        }
        /// <summary>
        /// Tests if node is an ast attribute node.
        /// </summary>
        /// <param name="AstNode"></param>
        /// <returns></returns>
        public bool IsNodeNamer
        {
            get
            {
                return this.Type == MvmScriptLexer.Ast_NodeNamer;
            }
        }
        /// <summary>
        /// Tests if node is an ast element node.
        /// </summary>
        /// <param name="AstNode"></param>
        /// <returns></returns>
        public bool IsElement
        {
            get
            {
                return this.Type == MvmScriptLexer.Ast_Element;
            }
        }
        /// <summary>
        /// Tests if node is an ast value node.
        /// </summary>
        /// <param name="AstNode"></param>
        /// <returns></returns>
        public bool IsValue
        {
            get
            {
                return this.Type == MvmScriptLexer.Ast_Value;
            }
        }



        #region Auditing
        /// <summary>
        /// Token position in line starting from 1.
        /// </summary>
        public int LinePosition
        {
            get
            {
                return this.CharPositionInLine + 1;
            }
        }
        public string Location
        {
            get
            {
                return "line=" + this.Line + " position=" + this.LinePosition + " in file=xxxx";
            }
        }
        #endregion

        # region Transforming to text

        // Gets the ast text representing this node and all it children 
        public string OuterAst
        {
            get
            {
                //this.PrettyPrint();
                // do not want final newline
                string aststring = this.GetOuterAst(0);
                if (aststring == null) return null;
                if (aststring.Length > 2) aststring = aststring.Substring(0,aststring.Length - 2);
                return aststring;

            }
        }

        // to read AST as if it was xml
        public string InnerText
        {
            get
            {
                return this.GetSubNodes().Select(n => n.OuterAst).JoinLines();
            }
        }

       






        /// <summary>
        /// Returns the ast as a formatted tree
        /// </summary>
        /// <param name="depth"></param>
        /// <returns></returns>
        public string GetOuterAst(int depth)
        {
            StringBuilder sb = new StringBuilder();
            if (this.IsNodeNamer)
            {
                return (this as AstNodeNamer).Value.OuterAst;
            }
            else if (this.IsNil)
            {
                sb.AppendLine("  ".repeat(depth++) + "(");
                foreach (AstNode n in this.Children)
                {
                    sb.Append(n.GetOuterAst(depth));
                }
                sb.AppendLine("  ".repeat(--depth) + ")");
            }
            else if (this.IsElement)
            {
                var astElement = this as AstElement;
                var nodeName = astElement.NodeName;
                string literalValue = null;
                if (astElement.IsLiteral)
                {
                    literalValue = astElement.LiteralValue;
                    if (nodeName != null)
                    {
                        sb.AppendLine("  ".repeat(depth++) + nodeName + " => " + literalValue);
                    }
                    else
                    {
                        sb.AppendLine("  ".repeat(depth++) + literalValue);
                    }
                    if (astElement.HasDot)
                    {
                        sb.AppendLine("  ".repeat(depth++) + ".(");
                        foreach (AstNode n in astElement.DotSubNodes())
                        {
                            sb.Append(n.GetOuterAst(depth));
                        }
                        sb.AppendLine("  ".repeat(--depth) + ")");
                    }
                }
                else
                {
                    if (nodeName != null)
                    {
                        sb.AppendLine("  ".repeat(depth++) + nodeName + " => " + astElement.Name);
                        sb.Append(GetElementSubNodesAst(astElement, depth));
                    }
                    else
                    {
                        sb.AppendLine("  ".repeat(depth++) + astElement.Name);
                        sb.Append(GetElementSubNodesAst(astElement, depth));
                    }
                }
            }
            else if (this.IsValue)
            {
                var astValue = this as AstValue;
                sb.AppendLine("  ".repeat(depth) + astValue.Name);
            }
            else
            {
                throw new Exception("Unexpected node (type=" + this.Type + ",type_name=" + this.NodeTypeName + ") passed to OuterAst:" + this.ToStringTree());
            }
            return sb.ToString();
        }
        private string GetElementSubNodesAst(AstElement astElement, int depth)
        {
            StringBuilder sb = new StringBuilder();
            if (astElement.CountTypeParameterNodes > 0)
            {
                sb.AppendLine("  ".repeat(depth++) + "<");
                foreach (AstNode n in astElement.TypeParameterSubNodes())
                {
                    sb.Append(n.GetOuterAst(depth));
                }
                sb.AppendLine("  ".repeat(--depth) + ">");
            }
            if (astElement.CountParameterNodes > 0)
            {
                sb.AppendLine("  ".repeat(depth++) + "(");
                foreach (AstNode n in astElement.ParameterSubNodes())
                {
                    sb.Append(n.GetOuterAst(depth));
                }
                sb.AppendLine("  ".repeat(--depth) + ")");
            }
            if (astElement.CountChildrenNodes > 0)
            {
                sb.AppendLine("  ".repeat(depth++) + "(");
                foreach (AstNode n in astElement.ChildrenSubNodes())
                {
                    sb.Append(n.GetOuterAst(depth));
                }
                sb.AppendLine("  ".repeat(--depth) + ")");
            }
            if (astElement.CountBraceNodes > 0)
            {
                sb.AppendLine("  ".repeat(depth++) + "{");
                foreach (AstNode n in astElement.BraceSubNodes())
                {
                    sb.Append(n.GetOuterAst(depth));
                }
                sb.AppendLine("  ".repeat(--depth) + "}");
            }
            if (astElement.CountBracketNodes > 0)
            {
                sb.AppendLine("  ".repeat(depth++) + "[");
                // print bracket params (first sub node will be an element) in parens
                AstElement bracketElem = (AstElement)astElement.BracketSubNodes().First();
                sb.Append(GetElementSubNodesAst(bracketElem, depth));
                foreach (AstNode n in astElement.BracketSubNodes().Skip(1))
                {
                    sb.Append(n.GetOuterAst(depth));
                }
                sb.AppendLine("  ".repeat(--depth) + "]");
            }
            if (astElement.CountDotNodes > 0)
            {
                sb.AppendLine("  ".repeat(depth++) + ".(");
                foreach (AstNode n in astElement.DotSubNodes())
                {
                    sb.Append(n.GetOuterAst(depth));
                }
                sb.AppendLine("  ".repeat(--depth) + ")");
            }
            return sb.ToString();
        }

        /// <summary>
        /// Returns the name which differs based on node type.
        /// </summary>
        public abstract string Name { get; }


        /// <summary>
        /// Returns the parent astNode which could be an AstNamer or AstElement. Should not
        /// be confused with xml parentNode. For that functionality, use ParentElement.
        /// </summary>
        public AstNode ParentAstNode
        {
            get
            {
                if (this.Parent == null) return null;
                if (this.Parent.Parent == null) return null;
                return this.Parent.Parent as AstNode;
            }
        }

        /// <summary>
        ///  Returns the enclosing parent AstElement or null
        /// </summary>
        public AstElement ParentElement
        {
            get
            {

                if (this.Parent == null) return null;
                if (this.Parent.Parent == null) return null;
                // otherwise we're 2 levels removed
                // Parent will be: 
                //  1)attributeNodes/childNodes of an ast element
                //  or
                //  2)a value of a AstNodeNamer
                // 
                // Since we abstract out the NodeNamers, go one level above if on namer
                if (this.Parent.Type.In(SubNodeTypes))
                {
                    return this.Parent.Parent as AstElement;
                }
                else if (this.Parent.Parent.Type == MvmScriptParser.Ast_NodeNamer)
                {
                    return (this.Parent.Parent as AstNodeNamer).ParentElement;
                }
                else
                {
                    return null; // maybe throw exception?
                }
            }
        }




        public AstNode Value
        {
            get
            {
                return null;
            }
        }

        #endregion

        # region sub-nodes

        virtual public IEnumerable<AstNode> GetSubNodes()
        {
            if (this.IsNil)
            {
                foreach (var node in this.Children())
                {
                    if (node is AstNodeNamer)
                        yield return (node as AstNodeNamer).Value;
                    else
                        yield return (AstNode)node;
                }
            }
        }
        virtual public IEnumerable<AstNode> WalkSubNodes()
        {
            foreach (AstNode subNode in this.GetSubNodes())
            {
                foreach (AstNode output in subNode.WalkAllNodes())
                    yield return output;
            }
        }
        public IEnumerable<AstNode> WalkAllNodes()
        {
            yield return this;
            foreach (var node in this.WalkSubNodes())
                yield return (AstNode)node;
        }
        public IEnumerable<AstElement> WalkSubElements()
        {
            foreach (var node in this.WalkSubNodes().Where(n => n.IsElement))
                yield return (AstElement)node;
        }
        public IEnumerable<AstElement> WalkAllElements()
        {
            if (this.IsElement) yield return (AstElement)this;
            foreach (var node in this.WalkSubNodes().Where(n => n.IsElement))
                yield return (AstElement)node;
        }
        /// <summary>
        /// Yields the child or attribute AstNodes of the current AstElement.
        /// </summary>
        /// <param name="astNode"></param>
        /// <returns></returns>
        public IEnumerable<AstElement> GetSubElements()
        {
            foreach (var node in this.GetSubNodes().Where(n => n.IsElement))
                yield return (AstElement)node;
        }

        # endregion

        # region XPath

        /// <summary>
        /// Depth first, Lazy evaluates the xpath expression on the MvmScript tree.
        /// Supports:
        /// - '/absolute/paths'
        /// - './relative/paths'
        /// - '//recursive/paths
        /// - '.'
        /// - '..'
        /// - '|' union
        /// - '*' wildcard
        /// - '@nodeName to select namedNodes (like xml attributes)
        /// </summary>
        /// <param name="AstNode"></param>
        /// <param name="xpath"></param>
        /// <returns></returns>
        public IEnumerable<AstNode> SelectNodes(string xpath)
        {
            ITree xpathTree = AstNode.ParseXPath(xpath);
            //xpathTree.PrintDetail();
            xpathTree.PrettyPrint();
            foreach (ITree output in AstNode.XPathOperator(xpathTree, this))
                yield return (AstNode)output;
        }
        public IEnumerable<AstElement> SelectElements(string xpath)
        {
            foreach (ITree output in SelectNodes(xpath))
                yield return (AstElement)output;
        }

        public AstNode SelectFirstNode(string xpath)
        {
            foreach (var astNode in this.SelectNodes(xpath))
                return astNode;
            return null;
        }
        

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
                MvmXPathLexer lex = new MvmXPathLexer(input);
                CommonTokenStream tokens = new CommonTokenStream(lex);
                MvmXPathParser parser = new MvmXPathParser(tokens);
                MvmXPathParser.main_return r = parser.main();
                ITree iTree = (ITree)r.Tree;
                //Console.WriteLine("tree=" + iTree.ToStringTree());
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
        public static IEnumerable<ITree> XPathOperator(ITree xpathTree, AstNode astNode)
        {
            foreach (ITree output in XPathOperator(xpathTree, 0, astNode))
                yield return output;
        }
        private static IEnumerable<ITree> XPathOperator(ITree xpathTree, int childNo, AstNode astNode)
        {
            ITree xpathOperator = xpathTree.GetChild(childNo);
            switch (xpathOperator.Type)
            {
                case MvmXPathParser.RELATIVE_PATH:
                    foreach (ITree output in XPathRelativePath(xpathOperator, astNode))
                        yield return output;
                    break;
                case MvmXPathParser.ABSOLUTE_PATH:
                    foreach (ITree output in XPathAbsolutePath(xpathOperator, astNode))
                        yield return output;
                    break;
                case MvmXPathParser.PIPE:
                    foreach (ITree output in XPathUnion(xpathOperator, astNode))
                        yield return output;
                    break;
                case MvmXPathParser.MATCH:
                    foreach (ITree output in XPathMatch(xpathTree, childNo, astNode))
                        yield return output;
                    break;
                case MvmXPathParser.RECURSIVE_MATCH:
                    foreach (ITree output in XPathRecursiveMatch(xpathTree, childNo, astNode))
                        yield return output;
                    break;
                case MvmXPathParser.CURRENT_NODE:
                    foreach (ITree output in XPathCurrentNode(xpathTree, childNo, astNode))
                        yield return output;
                    break;
                case MvmXPathParser.TRAVERSE_UP:
                    foreach (ITree output in XPathTraverseUp(xpathTree, childNo, astNode))
                        yield return output;
                    break;
                case MvmXPathParser.NAME_MATCH:
                    foreach (ITree output in XPathNameMatch(xpathTree, childNo, astNode))
                        yield return output;
                    break;
                case MvmXPathParser.RECURSIVE_NAME_MATCH:
                    foreach (ITree output in XPathRecursiveNameMatch(xpathTree, childNo, astNode))
                        yield return output;
                    break;
                default:
                    throw new Exception("Unexpected xpath operator [" + xpathOperator.Text + "]");
            }
        }
        public static IEnumerable<ITree> XPathCurrentNode(ITree xpathTree, int childNo, AstNode astNode)
        {
            ITree xpathOperator = xpathTree.GetChild(childNo);
            bool isFinalOperator = xpathTree.ChildCount == childNo + 1;
            if (isFinalOperator)
                yield return astNode;
            else
                foreach (AstNode output in XPathOperator(xpathTree, childNo + 1, astNode))
                    yield return output;
        }
        public static IEnumerable<ITree> XPathTraverseUp(ITree xpathTree, int childNo, AstNode astNode)
        {
            ITree xpathOperator = xpathTree.GetChild(childNo);
            bool isFinalOperator = xpathTree.ChildCount == childNo + 1;
            if (isFinalOperator)
                yield return astNode.ParentElement;
            else
                foreach (AstNode output in XPathOperator(xpathTree, childNo + 1, astNode.ParentElement))
                    yield return output;
        }
        private static IEnumerable<ITree> XPathMatch(ITree xpathTree, int childNo, AstNode astNode)
        {
            ITree xpathOperator = xpathTree.GetChild(childNo);
            bool isFinalOperator = xpathTree.ChildCount == childNo + 1;
            ITree matchTree = xpathOperator.GetChild(0);
            foreach (AstElement childElement in astNode.GetSubElements())
            {
                if (matchTree.Type == MvmXPathParser.MUL || childElement.Name.Equals(matchTree.Text))
                {
                    if (isFinalOperator)
                        yield return childElement;
                    else
                        foreach (AstNode output in XPathOperator(xpathTree, childNo + 1, childElement))
                            yield return output;
                }
            }
        }
        private static IEnumerable<ITree> XPathNameMatch(ITree xpathTree, int childNo, AstNode astNode)
        {
            ITree xpathOperator = xpathTree.GetChild(childNo);
            bool isFinalOperator = xpathTree.ChildCount == childNo + 1;
            ITree matchTree = xpathOperator.GetChild(0);
            foreach (AstElement childElement in astNode.GetSubElements())
            {
                string nodeName = childElement.NodeName;
                if (nodeName == null) continue;
                if (matchTree.Type == MvmXPathParser.MUL || nodeName.Equals(matchTree.Text))
                {
                    if (isFinalOperator)
                        yield return childElement;
                    else
                        foreach (AstNode output in XPathOperator(xpathTree, childNo + 1, childElement))
                            yield return output;
                }
            }
        }

        private static IEnumerable<ITree> XPathRecursiveMatch(ITree xpathTree, int childNo, AstNode astNode)
        {
            ITree xpathOperator = xpathTree.GetChild(childNo);
            bool isFinalOperator = xpathTree.ChildCount == childNo + 1;
            ITree matchTree = xpathOperator.GetChild(0);
            foreach (AstElement subElement in astNode.WalkSubElements())
            {
                if (matchTree.Type == MvmXPathParser.MUL || subElement.Name.Equals(matchTree.Text))
                {
                    if (isFinalOperator)
                        yield return subElement;
                    else
                        foreach (AstNode output in XPathOperator(xpathTree, childNo + 1, subElement))
                            yield return output;
                }
            }
        }
        private static IEnumerable<ITree> XPathRecursiveNameMatch(ITree xpathTree, int childNo, AstNode astNode)
        {
            ITree xpathOperator = xpathTree.GetChild(childNo);
            bool isFinalOperator = xpathTree.ChildCount == childNo + 1;
            ITree matchTree = xpathOperator.GetChild(0);
            foreach (AstElement subElement in astNode.WalkSubElements())
            {
                string nodeName = subElement.NodeName;
                if (nodeName == null) continue;
                if (matchTree.Type == MvmXPathParser.MUL || nodeName.Equals(matchTree.Text))
                {
                    if (isFinalOperator)
                        yield return subElement;
                    else
                        foreach (AstNode output in XPathOperator(xpathTree, childNo + 1, subElement))
                            yield return output;
                }
            }
        }
        private static IEnumerable<ITree> XPathUnion(ITree xpathTree, AstNode astNode)
        {
            foreach (ITree childXpathTree in xpathTree.Children())
            {
                foreach (AstNode output in XPathOperator(childXpathTree, 0, astNode))
                    yield return output;
            }
        }
        private static IEnumerable<ITree> XPathRelativePath(ITree xpathTree, AstNode astNode)
        {
            foreach (AstNode output in XPathOperator(xpathTree, 0, astNode))
                yield return output;
        }
        private static IEnumerable<ITree> XPathAbsolutePath(ITree xpathTree, AstNode astNode)
        {
            AstNode rootTree = astNode.GetRoot() as AstNode;
            foreach (AstNode output in XPathOperator(xpathTree, 0, rootTree))
                yield return output;
        }
        # endregion


        #region xml
        /// <summary>
        /// Parses a basic xml data (not code) file into AST. Treats all text as literal strings. This is useful
        /// for manipulating xml using ast xpath. 
        /// 
        /// Does not support:
        ///     mixed content
        ///     namespace
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public static string ConvertXmlFileToMvmScript(string file)
        {
            var doc = MyXml.ParseXmlFile(file);
            return ConvertXmlElementToMvmScript(doc.DocumentElement);
        }
        private static string ConvertXmlElementToMvmScript(XmlElement xmlElement)
        {
            return ConvertXmlElementToMvmScript(xmlElement, 0);
        }
        private static string ConvertXmlElementToMvmScript(XmlElement xmlElement, int depth)
        {
            if (xmlElement.HasMixedContent())
                throw new Exception("Not expecting xml with mixed content:" + xmlElement.OuterXml);
            StringBuilder sb = new StringBuilder();
            sb.Append("  ".repeat(depth) + xmlElement.Name);
            if (!xmlElement.HasChildNodes && !xmlElement.HasAttributes)
                return sb.ToString();

            if (xmlElement.HasAttributes)
            {
                List<string> alist = new List<string>();
                foreach (XmlAttribute a in xmlElement.Attributes)
                {
                    alist.Add(a.Name + "=>" + a.Value.SingleOrDoubleQuoteMe());
                }
                sb.Append("(" + alist.Join(", ") + ")");
            }

            if (xmlElement.HasChildElements())
            {
                sb.AppendLine("(");
                depth++;
                List<string> aList = new List<string>();
                foreach (var e in xmlElement.GetChildElems())
                    aList.Add(ConvertXmlElementToMvmScript(e, depth));
                sb.Append(aList.Join(",".AppendLine()).AppendLine());
                sb.Append("  ".repeat(--depth) + ")");
            }
            else if (xmlElement.HasChildXmlText())
            {
                StringBuilder text = new StringBuilder();
                foreach (XmlNode n in xmlElement.ChildNodes)
                    text.Append(n.InnerText);
                sb.Append("(" + text.ToString().SingleOrDoubleQuoteMe() + ")");
            }
            else if (xmlElement.HasAttributes)
            {
                sb.Append("()");
            }
            return sb.ToString();
        }

        # endregion

        public static void Test()
        {
            string syntax =
@"proc('hi'){
    test(x=>'lit');	
    define_format(name=>'acct_records')(
        field(name=>'id_acc',type=>'int'),
        field(name=>'fname',type=>'string')
    );
    while(x==x)y=y;
    while(1==2){
        yo(a=>b);
	    print('works');	
        y=f(x);
        z=g(x);
    }
}";
            AstElement mytree = (AstElement)AstNode.ParseSyntax(syntax);
            Console.WriteLine("INPUT DATA...............");
            Console.WriteLine(syntax);
            Console.WriteLine("GetSubElements...............");
            mytree.GetSubElements().ForEach(e => Console.WriteLine("-->" + e.Name));
            Console.WriteLine("WalkSubElements...............");
            mytree.WalkSubElements().ForEach(e => Console.WriteLine("-->" + e.Name));
            Console.WriteLine("AST.....................................");
            Console.WriteLine(mytree.OuterAst);
            Console.WriteLine("ANTLR...............");
            mytree.PrettyPrint();
            Console.WriteLine("INPUT DATA...............");
            Console.WriteLine(syntax);
            Console.WriteLine("TESTS................");
            string[] xpaths = new string[] {
                "//while/@condition",
                "//test/@x",
                "/test//@x",
                "//@*",
                "//Syn_literalString",
                "//print",
                "//y",
                "//f",
                "//f/x",
                "//field/@name"
            };
            foreach (string xpath in xpaths)
            {
                Console.WriteLine("xpath=" + xpath + ":");
                foreach (var t in mytree.SelectNodes(xpath))
                {
                    Console.WriteLine("selected:");
                    Console.WriteLine(t.OuterAst);
                    //t.PrettyPrint();
                    //Console.WriteLine("parent:");
                    //t.ParentNode.PrettyPrint();
                    //Console.WriteLine("parent.children:");
                    //t.ParentNode.ChildNodes().ForEach(x => x.PrettyPrint());
                    //Console.WriteLine("parent.parent:");
                    //t.ParentNode.ParentNode.PrettyPrint();
                }
            }
            //Console.WriteLine("WALK ELEMENTS.....................................");
            //mytree.WalkElements().ForEach(e => e.PrettyPrint());
            //mytree.WalkElements().ForEach(e => Console.WriteLine(e.Name));
        }
    }
}