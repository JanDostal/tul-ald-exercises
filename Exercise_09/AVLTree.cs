using System;
using System.Text;

namespace Exercise_09
{
    public class AVLTree
    {
        private Node Root { get; set; }

        private class Node
        {
            public Node? Left { get; set; }

            public Node? Right { get; set; }

            public int Data { get; }

            public Node(int data)
            {
                Data = data;
            }
        }

        public void InsertIntoTree(int data)
        {
            if (ReferenceEquals(Root, null)) Root = new Node(data);
            else
            {
                bool wasNewNodeInserted = false;
                Node subtreeRoot = Root;

                while (wasNewNodeInserted == false)
                {
                    if (data > subtreeRoot.Data)
                    {
                        if (ReferenceEquals(subtreeRoot.Right, null) == false) subtreeRoot = subtreeRoot.Right;
                        else
                        {
                            subtreeRoot.Right = new Node(data);
                            wasNewNodeInserted = true;
                        }
                    }
                    else
                    {
                        if (ReferenceEquals(subtreeRoot.Left, null) == false) subtreeRoot = subtreeRoot.Left;
                        else
                        {
                            subtreeRoot.Left = new Node(data);
                            wasNewNodeInserted = true;
                        }
                    }
                }
            }
        }

        public string GetTreeContentPreorder()
        {
            if (ReferenceEquals(Root, null)) return "";

            StringBuilder treeContent = new StringBuilder();

            treeContent.AppendLine(Root.Data.ToString());

            if (ReferenceEquals(Root.Left, null) == false) ExtractSubtreeContentPreorder(Root.Left, treeContent);

            if (ReferenceEquals(Root.Right, null) == false) ExtractSubtreeContentPreorder(Root.Right, treeContent);

            return treeContent.ToString();
        }

        public string GetTreeContentInorder()
        {
            if (ReferenceEquals(Root, null)) return "";

            StringBuilder treeContent = new StringBuilder();

            if (ReferenceEquals(Root.Left, null) == false) ExtractSubtreeContentInorder(Root.Left, treeContent);

            treeContent.AppendLine(Root.Data.ToString());

            if (ReferenceEquals(Root.Right, null) == false) ExtractSubtreeContentInorder(Root.Right, treeContent);

            return treeContent.ToString();
        }

        public string GetTreeContentPostorder()
        {
            if (ReferenceEquals(Root, null)) return "";

            StringBuilder treeContent = new StringBuilder();

            if (ReferenceEquals(Root.Left, null) == false) ExtractSubtreeContentPostorder(Root.Left, treeContent);

            if (ReferenceEquals(Root.Right, null) == false) ExtractSubtreeContentPostorder(Root.Right, treeContent);

            treeContent.AppendLine(Root.Data.ToString());

            return treeContent.ToString();
        }

        private void ExtractSubtreeContentPreorder(Node subtreeNode, StringBuilder treeContent)
        {
            treeContent.AppendLine(subtreeNode.Data.ToString());

            if (ReferenceEquals(subtreeNode.Left, null) == false) ExtractSubtreeContentPreorder(subtreeNode.Left, treeContent);

            if (ReferenceEquals(subtreeNode.Right, null) == false) ExtractSubtreeContentPreorder(subtreeNode.Right, treeContent);
        }

        private void ExtractSubtreeContentInorder(Node subtreeNode, StringBuilder treeContent)
        {
            if (ReferenceEquals(subtreeNode.Left, null) == false) ExtractSubtreeContentInorder(subtreeNode.Left, treeContent);

            treeContent.AppendLine(subtreeNode.Data.ToString());

            if (ReferenceEquals(subtreeNode.Right, null) == false) ExtractSubtreeContentInorder(subtreeNode.Right, treeContent);
        }

        private void ExtractSubtreeContentPostorder(Node subtreeNode, StringBuilder treeContent)
        {
            if (ReferenceEquals(subtreeNode.Left, null) == false) ExtractSubtreeContentPostorder(subtreeNode.Left, treeContent);

            if (ReferenceEquals(subtreeNode.Right, null) == false) ExtractSubtreeContentPostorder(subtreeNode.Right, treeContent);

            treeContent.AppendLine(subtreeNode.Data.ToString());
        }
    }
}
