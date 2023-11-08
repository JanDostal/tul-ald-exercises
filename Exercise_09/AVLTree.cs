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

            if (ReferenceEquals(Root.Left, null) == false) SearchSubtreePreorder(Root.Left, treeContent);

            if (ReferenceEquals(Root.Right, null) == false) SearchSubtreePreorder(Root.Right, treeContent);

            return treeContent.ToString();
        }

        public string GetTreeContentInorder()
        {
            if (ReferenceEquals(Root, null)) return "";

            StringBuilder treeContent = new StringBuilder();

            if (ReferenceEquals(Root.Left, null) == false) SearchSubtreeInorder(Root.Left, treeContent);

            treeContent.AppendLine(Root.Data.ToString());

            if (ReferenceEquals(Root.Right, null) == false) SearchSubtreeInorder(Root.Right, treeContent);

            return treeContent.ToString();
        }

        public string GetTreeContentPostorder()
        {
            if (ReferenceEquals(Root, null)) return "";

            StringBuilder treeContent = new StringBuilder();

            if (ReferenceEquals(Root.Left, null) == false) SearchSubtreePostorder(Root.Left, treeContent);

            if (ReferenceEquals(Root.Right, null) == false) SearchSubtreePostorder(Root.Right, treeContent);

            treeContent.AppendLine(Root.Data.ToString());

            return treeContent.ToString();
        }

        private void SearchSubtreePreorder(Node subtreeNode, StringBuilder treeContent)
        {
            treeContent.AppendLine(subtreeNode.Data.ToString());

            if (ReferenceEquals(subtreeNode.Left, null) == false) SearchSubtreePreorder(subtreeNode.Left, treeContent);

            if (ReferenceEquals(subtreeNode.Right, null) == false) SearchSubtreePreorder(subtreeNode.Right, treeContent);
        }

        private void SearchSubtreeInorder(Node subtreeNode, StringBuilder treeContent)
        {
            if (ReferenceEquals(subtreeNode.Left, null) == false) SearchSubtreeInorder(subtreeNode.Left, treeContent);

            treeContent.AppendLine(subtreeNode.Data.ToString());

            if (ReferenceEquals(subtreeNode.Right, null) == false) SearchSubtreeInorder(subtreeNode.Right, treeContent);
        }

        private void SearchSubtreePostorder(Node subtreeNode, StringBuilder treeContent)
        {
            if (ReferenceEquals(subtreeNode.Left, null) == false) SearchSubtreePostorder(subtreeNode.Left, treeContent);

            if (ReferenceEquals(subtreeNode.Right, null) == false) SearchSubtreePostorder(subtreeNode.Right, treeContent);

            treeContent.AppendLine(subtreeNode.Data.ToString());
        }
    }
}
