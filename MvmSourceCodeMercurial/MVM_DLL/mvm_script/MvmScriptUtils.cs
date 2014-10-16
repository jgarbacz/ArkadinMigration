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
    public class MvmScriptUtils
    {
        //public static ITree ParseFile(string file)
        //{
        //    string fullpath;
        //    if (Path.IsPathRooted(file))
        //        fullpath = file;
        //    else
        //        fullpath = Path.Combine(Environment.CurrentDirectory, file);
        //    try
        //    {
        //        ICharStream input = new ANTLRFileStream(fullpath);
        //        MvmScriptLexer lex = new MvmScriptLexer(input);
        //        CommonTokenStream tokens = new CommonTokenStream(lex);
        //        MvmScriptParser parser = new MvmScriptParser(tokens);
        //        AstNodeAdaptor adaptor = new AstNodeAdaptor();
        //        parser.TreeAdaptor = adaptor;
        //        MvmScriptParser.start_return r = parser.start();
        //        return (ITree)r.Tree;
        //    }
        //    catch (Antlr.Runtime.NoViableAltException e)
        //    {
        //        StringBuilder msg = new StringBuilder();
        //        msg.AppendLine("Error, cannot parse MvmScript file [" + file + "]");
        //        msg.AppendLine("Msg:" + e.Message);
        //        msg.AppendLine("Line:" + e.Line);
        //        msg.AppendLine("Pos:" + e.CharPositionInLine);
        //        throw new Exception(msg.ToString(), e);
        //    }
        //}

        //public static ITree ParseSyntax(string syntax)
        //{
        //    try
        //    {
        //        //Console.WriteLine("ParseMvmScript=" + syntax); 
        //        ICharStream input = new ANTLRStringStream(syntax);
        //        MvmScriptLexer lex = new MvmScriptLexer(input);
        //        CommonTokenStream tokens = new CommonTokenStream(lex);
        //        MvmScriptParser parser = new MvmScriptParser(tokens);
        //        AstNodeAdaptor adaptor = new AstNodeAdaptor();
        //        parser.TreeAdaptor = adaptor;
        //        MvmScriptParser.start_return r = parser.start();
        //        ITree iTree = (ITree)r.Tree;
        //        //Console.WriteLine("tree=" + iTree.ToStringTree());
        //        return iTree;
        //    }
        //    catch (Antlr.Runtime.NoViableAltException e)
        //    {
        //        StringBuilder msg = new StringBuilder();
        //        msg.AppendLine("Error, cannot parse MvmScript syntax [" + syntax + "]");
        //        msg.AppendLine("Msg:" + e.Message);
        //        msg.AppendLine("Line:" + e.Line);
        //        msg.AppendLine("Pos:" + e.CharPositionInLine);
        //        throw new Exception(msg.ToString(), e);
        //    }
        //    catch (Exception e)
        //    {
        //        StringBuilder msg = new StringBuilder();
        //        msg.AppendLine("Error, cannot parse MvmScript syntax [" + syntax + "]");
        //        throw new Exception(msg.ToString(), e);
        //    }
        //}
    }
}
