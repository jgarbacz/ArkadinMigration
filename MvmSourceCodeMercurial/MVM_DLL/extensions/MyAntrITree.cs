using System;
using System.Collections.Generic;
using System.Text;
using Antlr.Runtime;
using Antlr.Runtime.Tree;

namespace MVM
{
    public static class MyAntlrITree
    {
        /// <summary>
        /// Climbs to the top and returns the parent ITree (which should be nil).
        /// </summary>
        /// <param name="iTree"></param>
        /// <returns></returns>
        public static ITree GetRoot(this ITree iTree)
        {
            // if pass tree is root
            if (iTree.Parent==null) return iTree;
            return iTree.Parent.GetRoot();
        }
        /// <summary>
        /// Walks tree depth first.
        /// </summary>
        /// <param name="itree"></param>
        /// <returns></returns>
        public static IEnumerable<ITree> WalkTree(this ITree itree)
        {
            foreach(ITree output in itree.WalkChildren())
                 yield return output;
            yield return itree;
        }
        /// <summary>
        /// Walks children depth first.
        /// </summary>
        /// <param name="itree"></param>
        /// <returns></returns>
        public static IEnumerable<ITree> WalkChildren(this ITree itree)
        {
            for (int i = 0; i < itree.ChildCount; i++)
            {
                ITree antlrChild = itree.GetChild(i);
                foreach (ITree output in antlrChild.WalkTree())
                    yield return output;
            }
        }
        /// <summary>
        /// Yields the immediate children.
        /// </summary>
        /// <param name="itree"></param>
        /// <returns></returns>
        public static IEnumerable<ITree> Children(this ITree itree)
        {
            for (int i = 0; i < itree.ChildCount; i++)
            {
                ITree antlrChild = itree.GetChild(i);
                yield return antlrChild;
            }
        }
        /// <summary>
        /// Recursively pretty prints the tree
        /// </summary>
        /// <param name="iTree"></param>
        public static void PrettyPrint(this ITree iTree)
        {
            iTree.PrettyPrint(0);
        }
        /// <summary>
        /// Recursively pretty prints the treat starting with a tab depth.
        /// </summary>
        /// <param name="iTree"></param>
        /// <param name="depth"></param>
        public static void PrettyPrint(this ITree iTree, int depth)
        {
            string prefix = "  ".repeat(depth++);
            if (iTree.ChildCount == 0)
            {
                Console.WriteLine(prefix + iTree.Text);
            }
            else
            {
                Console.WriteLine(prefix + iTree.Text + "(");
                for (int i = 0; i < iTree.ChildCount; i++) 
                    iTree.GetChild(i).PrettyPrint(depth);
                Console.WriteLine(prefix + ")");
            }
        }

        /// <summary>
        /// Recursively detail prints the tree
        /// </summary>
        /// <param name="iTree"></param>
        public static void PrintDetail(this ITree iTree)
        {
            iTree.PrintDetail(0);
        }
        /// <summary>
        /// Recursively detail prints the treat starting with a tab depth.
        /// </summary>
        /// <param name="iTree"></param>
        /// <param name="depth"></param>
        public static void PrintDetail(this ITree iTree, int depth)
        {
            string prefix = "  ".repeat(depth++);
            string detail = iTree.Text + "[line=" + iTree.Line + ",pos=" + iTree.CharPositionInLine + ",type=" + iTree.Type + "]";
            if (iTree.ChildCount == 0)
            {
                Console.WriteLine(prefix + detail);
            }
            else
            {
                Console.WriteLine(prefix + detail + "(");
                for (int i = 0; i < iTree.ChildCount; i++)
                    iTree.GetChild(i).PrintDetail(depth);
                Console.WriteLine(prefix + ")");
            }
        }

        /// <summary>
        /// Replace self with the replacement and return the replacement
        /// </summary>
        /// <param name="iTree"></param>
        /// <param name="replacement"></param>
        /// <returns></returns>
        public static ITree ReplaceSelfWith(this ITree iTree, ITree replacement)
        {
            int myIdx = iTree.ChildIndex;
            ITree myParent = iTree.Parent;
            myParent.ReplaceChildren(myIdx, myIdx, replacement);
            return replacement;
        }

      /// <summary>
      /// Returns the first child or null if none
      /// </summary>
      /// <param name="iTree"></param>
      /// <returns></returns>
        public static ITree GetFirstChild(this ITree iTree)
        {
            if (iTree.ChildCount <= 0) return null;
            return iTree.GetChild(0);
        }

        /// <summary>
        /// Returns the next sibling node or null
        /// </summary>
        /// <param name="iTree"></param>
        /// <returns></returns>
        public static ITree NextSibling(this ITree iTree)
        {
            if (iTree.Parent == null) return null;
            ITree myParent=iTree.Parent;
            int myChildIndex = iTree.ChildIndex;
            int mySiblingIndex = myChildIndex + 1;
            if (mySiblingIndex >= myParent.ChildCount) return null;
            return myParent.GetChild(mySiblingIndex);
        }
        /// <summary>
        /// Returns the previous sibling or null
        /// </summary>
        /// <param name="iTree"></param>
        /// <returns></returns>
        public static ITree PreviousSibling(this ITree iTree)
        {
            if (iTree.Parent == null) return null;
            ITree myParent = iTree.Parent;
            int myChildIndex = iTree.ChildIndex;
            int mySiblingIndex = myChildIndex - 1;
            if (mySiblingIndex < 0) return null;
            return myParent.GetChild(mySiblingIndex);
        }
        /// <summary>
        /// Deletes self from parent.
        /// </summary>
        /// <param name="iTree"></param>
        /// <returns></returns>
        public static ITree DeleteSelf(this ITree iTree)
        {
            if (iTree.Parent == null) return null;
            ITree myParent = iTree.Parent;
            return (ITree)myParent.DeleteChild(iTree.ChildIndex);
        }
        /// <summary>
        /// Return token position in line starting from 1
        /// </summary>
        /// <param name="iTree"></param>
        /// <returns></returns>
        public static int LinePosition(this ITree iTree)
        {
            
            return iTree.CharPositionInLine+1;
        }

        
    }
}
