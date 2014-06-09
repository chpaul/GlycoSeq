using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using COL.GlycoLib;
namespace GlycanSeq_Form
{
    public class TreeStructure2TreeView
    {
        public static TreeNode Convert2TreeView( GlycanTreeNode argRoot)
        {                 
            Stack<GlycanTreeNode> NodeStack = new Stack<GlycanTreeNode>();
            NodeStack.Push(argRoot);
            TreeNode root = new TreeNode(argRoot.NodeID + "-" + argRoot.GlycanType.ToString());
            foreach (GlycanTreeNode GN in argRoot.TravelGlycanTreeBFS())
            {
                if (GN.Parent == null)
                {
                    continue;
                }
                string PID = GN.Parent.NodeID + "-" + GN.Parent.GlycanType.ToString();
                TreeNode ParentNode = FindNode(root, PID);
                ParentNode.Nodes.Add(GN.NodeID+"-"+GN.GlycanType.ToString());
            }
            return root;
        }
        private static TreeNode FindNode(TreeNode argNode, string argID)
        {
            Stack<TreeNode> treeStack = new Stack<TreeNode>();
            treeStack.Push(argNode);
            while (treeStack.Count != 0)
            {
                TreeNode treeNode = treeStack.Pop();
                if (treeNode.Text == argID)
                {
                    return treeNode;
                }
                for (int i = 0; i < treeNode.Nodes.Count; i++)
                {
                    treeStack.Push(treeNode.Nodes[i]);
                } 
            }
            return null;
        }
    }
}


