using System;
using System.Text;

namespace Exercise_09
{
    public class AVLTree
    {
        private Node Root { get; set; }

        private bool wasTreeRotationInitialized { get; set; } = false;

        private class Node
        {
            public Node? Left { get; set; }

            public Node? Right { get; set; }

            public int Data { get; }

            public int BalanceFactor { get; set; }

            public int Height { get; set; }

            public Node(int data)
            {
                Data = data;
                BalanceFactor = 0;
                Height = 0;
            }
        }

        public void InsertIntoTree(int data)
        {
            if (ReferenceEquals(Root, null)) Root = new Node(data);
            else
            {
                if (data > Root.Data) 
                {
                    if (ReferenceEquals(Root.Right, null)) Root.Right = new Node(data);
                    else InsertIntoSubtree(Root.Right, Root, data);
                }
                else 
                {
                    if (ReferenceEquals(Root.Left, null)) Root.Left = new Node(data);
                    else InsertIntoSubtree(Root.Left, Root, data);
                }

                if (wasTreeRotationInitialized == false) 
                {
                    UpdateTreeNodeBalanceFactor(Root);

                    if (Root.BalanceFactor == 2 || Root.BalanceFactor == -2) DoTreeRotation(Root, null);
                }

                wasTreeRotationInitialized = false;
            }
        }

        public string GetTreeContentPreorder()
        {
            if (ReferenceEquals(Root, null)) return "";

            StringBuilder treeContent = new StringBuilder();

            treeContent.Append(Root.Data.ToString()).Append(",");

            if (ReferenceEquals(Root.Left, null) == false) ExtractSubtreeContentPreorder(Root.Left, treeContent);

            if (ReferenceEquals(Root.Right, null) == false) ExtractSubtreeContentPreorder(Root.Right, treeContent);

            treeContent.AppendLine();

            return treeContent.ToString();
        }

        public string GetTreeContentInorder()
        {
            if (ReferenceEquals(Root, null)) return "";

            StringBuilder treeContent = new StringBuilder();

            if (ReferenceEquals(Root.Left, null) == false) ExtractSubtreeContentInorder(Root.Left, treeContent);

            treeContent.Append(Root.Data.ToString()).Append(",");

            if (ReferenceEquals(Root.Right, null) == false) ExtractSubtreeContentInorder(Root.Right, treeContent);

            treeContent.AppendLine();

            return treeContent.ToString();
        }

        public string GetTreeContentPostorder()
        {
            if (ReferenceEquals(Root, null)) return "";

            StringBuilder treeContent = new StringBuilder();

            if (ReferenceEquals(Root.Left, null) == false) ExtractSubtreeContentPostorder(Root.Left, treeContent);

            if (ReferenceEquals(Root.Right, null) == false) ExtractSubtreeContentPostorder(Root.Right, treeContent);

            treeContent.Append(Root.Data.ToString()).Append(",");

            treeContent.AppendLine();

            return treeContent.ToString();
        }
        private void InsertIntoSubtree(Node subtreeNode, Node subtreeNodeParent, int data)
        {
            if (data > subtreeNode.Data)
            {
                if (ReferenceEquals(subtreeNode.Right, null)) subtreeNode.Right = new Node(data);
                else InsertIntoSubtree(subtreeNode.Right, subtreeNode, data);
            }
            else
            {
                if (ReferenceEquals(subtreeNode.Left, null)) subtreeNode.Left = new Node(data);
                else InsertIntoSubtree(subtreeNode.Left, subtreeNode, data);
            }

            if (wasTreeRotationInitialized == false) 
            {
                UpdateTreeNodeBalanceFactor(subtreeNode);

                if (subtreeNode.BalanceFactor == 2 || subtreeNode.BalanceFactor == -2) DoTreeRotation(subtreeNode, subtreeNodeParent);
            }
        }

        private void UpdateTreeNodeBalanceFactor(Node treeNode) 
        {
            int leftSubtreeHeight = ReferenceEquals(treeNode.Left, null) ? -1 : treeNode.Left.Height;
            int rightSubtreeHeight = ReferenceEquals(treeNode.Right, null) ? -1 : treeNode.Right.Height;

            treeNode.BalanceFactor = leftSubtreeHeight - rightSubtreeHeight;
            treeNode.Height = leftSubtreeHeight > rightSubtreeHeight ? leftSubtreeHeight + 1 : rightSubtreeHeight + 1;
        }

        private void DoTreeRotation(Node treeNode, Node treeNodeParent) 
        {
            switch (treeNode.BalanceFactor) 
            {
                case 2:
                    DoLeftSubtreeRotation(treeNode, treeNodeParent);
                    break;
                case -2:
                    DoRightSubtreeRotation(treeNode, treeNodeParent);
                    break;
            }

            wasTreeRotationInitialized = true;
        }

        private void DoLeftSubtreeRotation(Node subtreeRoot, Node treeNodeParent) 
        {
            switch (subtreeRoot.Left.BalanceFactor) 
            {
                case 1:

                    DoRightRotation(subtreeRoot, treeNodeParent);
                    break;
                case -1:

                    DoLeftRightRotation(subtreeRoot, treeNodeParent);
                    break;
            }
        }

        private void DoRightSubtreeRotation(Node subtreeRoot, Node treeNodeParent)
        {
            switch (subtreeRoot.Right.BalanceFactor)
            {
                case 1:

                    DoRightLeftRotation(subtreeRoot, treeNodeParent);
                    break;
                case -1:

                    DoLeftRotation(subtreeRoot, treeNodeParent);
                    break;
            }
        }

        private void DoLeftRotation(Node subtreeRoot, Node treeNodeParent) 
        {
            Node subtreeRootRightNode = subtreeRoot.Right;
            Node subtreeRootRightNodeLeftNode = subtreeRootRightNode.Left;

            subtreeRoot.Right = subtreeRootRightNodeLeftNode;
            subtreeRootRightNode.Left = subtreeRoot;

            if (ReferenceEquals(subtreeRoot, Root)) Root = subtreeRootRightNode;
            else 
            {
                if (ReferenceEquals(treeNodeParent.Left, subtreeRoot)) treeNodeParent.Left = subtreeRootRightNode;
                else treeNodeParent.Right = subtreeRootRightNode;
            }

            UpdateTreeNodeBalanceFactor(subtreeRoot);
            UpdateTreeNodeBalanceFactor(subtreeRootRightNode);
        }

        private void DoRightRotation(Node subtreeRoot, Node treeNodeParent)
        {
            Node subtreeRootLeftNode = subtreeRoot.Left;
            Node subtreeRootLeftNodeRightNode = subtreeRootLeftNode.Right;

            subtreeRoot.Left = subtreeRootLeftNodeRightNode;
            subtreeRootLeftNode.Right = subtreeRoot;

            if (ReferenceEquals(subtreeRoot, Root)) Root = subtreeRootLeftNode;
            else
            {
                if (ReferenceEquals(treeNodeParent.Left, subtreeRoot)) treeNodeParent.Left = subtreeRootLeftNode;
                else treeNodeParent.Right = subtreeRootLeftNode;
            }

            UpdateTreeNodeBalanceFactor(subtreeRoot);
            UpdateTreeNodeBalanceFactor(subtreeRootLeftNode);
        }

        private void DoLeftRightRotation(Node subtreeRoot, Node treeNodeParent) 
        {
            Node subtreeRootLeftNode = subtreeRoot.Left;
            Node nodeLR = subtreeRootLeftNode.Right;

            Node NodeLRLeftNode = nodeLR.Left;
            Node NodeLRRightNode = nodeLR.Right;

            subtreeRootLeftNode.Right = NodeLRLeftNode;
            subtreeRoot.Left = NodeLRRightNode;

            nodeLR.Left = subtreeRootLeftNode;
            nodeLR.Right = subtreeRoot;

            if (ReferenceEquals(subtreeRoot, Root)) Root = nodeLR;
            else
            {
                if (ReferenceEquals(treeNodeParent.Left, subtreeRoot)) treeNodeParent.Left = nodeLR;
                else treeNodeParent.Right = nodeLR;
            }

            UpdateTreeNodeBalanceFactor(subtreeRoot);
            UpdateTreeNodeBalanceFactor(subtreeRootLeftNode);
            UpdateTreeNodeBalanceFactor(nodeLR);
        }

        private void DoRightLeftRotation(Node subtreeRoot, Node treeNodeParent) 
        {
            Node subtreeRootRightNode = subtreeRoot.Right;
            Node nodeLR = subtreeRootRightNode.Left;

            Node NodeLRLeftNode = nodeLR.Left;
            Node NodeLRRightNode = nodeLR.Right;

            subtreeRootRightNode.Left = NodeLRRightNode;
            subtreeRoot.Right = NodeLRLeftNode;

            nodeLR.Left = subtreeRoot;
            nodeLR.Right = subtreeRootRightNode;

            if (ReferenceEquals(subtreeRoot, Root)) Root = nodeLR;
            else
            {
                if (ReferenceEquals(treeNodeParent.Left, subtreeRoot)) treeNodeParent.Left = nodeLR;
                else treeNodeParent.Right = nodeLR;
            }

            UpdateTreeNodeBalanceFactor(subtreeRoot);
            UpdateTreeNodeBalanceFactor(subtreeRootRightNode);
            UpdateTreeNodeBalanceFactor(nodeLR);
        }

        private void ExtractSubtreeContentPreorder(Node subtreeNode, StringBuilder treeContent)
        {
            treeContent.Append(subtreeNode.Data.ToString()).Append(",");

            if (ReferenceEquals(subtreeNode.Left, null) == false) ExtractSubtreeContentPreorder(subtreeNode.Left, treeContent);

            if (ReferenceEquals(subtreeNode.Right, null) == false) ExtractSubtreeContentPreorder(subtreeNode.Right, treeContent);
        }

        private void ExtractSubtreeContentInorder(Node subtreeNode, StringBuilder treeContent)
        {
            if (ReferenceEquals(subtreeNode.Left, null) == false) ExtractSubtreeContentInorder(subtreeNode.Left, treeContent);

            treeContent.Append(subtreeNode.Data.ToString()).Append(",");

            if (ReferenceEquals(subtreeNode.Right, null) == false) ExtractSubtreeContentInorder(subtreeNode.Right, treeContent);
        }

        private void ExtractSubtreeContentPostorder(Node subtreeNode, StringBuilder treeContent)
        {
            if (ReferenceEquals(subtreeNode.Left, null) == false) ExtractSubtreeContentPostorder(subtreeNode.Left, treeContent);

            if (ReferenceEquals(subtreeNode.Right, null) == false) ExtractSubtreeContentPostorder(subtreeNode.Right, treeContent);

            treeContent.Append(subtreeNode.Data.ToString()).Append(",");
        }
    }
}
