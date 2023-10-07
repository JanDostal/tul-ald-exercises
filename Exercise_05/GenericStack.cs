using System;
using System.Numerics;

namespace Exercise_05
{
    public class GenericStack<V>
    {
        private Node Top { get; set; }

        public BigInteger Count { get; private set; } = 0;

        private class Node
        {
            public Node Previous { get; private set; }

            public V Value { get; private set; }

            public Node(Node previous, V value)
            {
                Previous = previous;
                Value = value;
            }
        }

        public void Push(V value) 
        {
            Node newNode = new Node(Top, value);

            Top = newNode;

            Count++;
        }

        public V Pop() 
        {
            if (Top == null) 
            {
                throw new Exception("Stack is empty");
            }

            V removedNodeValue = Top.Value;

            Top = Top.Previous;

            Count--;

            return removedNodeValue;
        }
    }
}
