using System;
using System.Numerics;

namespace Exercise_06
{
    public class GenericQueue<V>
    {
        private Node Start { get; set; }

        public BigInteger Count { get; private set; } = 0;

        private class Node
        {
            public Node Next { get; set; }

            public V Value { get; private set; }

            public Node(V value)
            {
                Value = value;
            }
        }

        public void Push(V value)
        {
            Node newNode = new Node(value);

            if (Start == null)
            {
                Start = newNode;
            }
            else
            {
                Node lastNode = Start;

                while (lastNode.Next != null)
                {
                    lastNode = lastNode.Next;
                }

                lastNode.Next = newNode;
            }

            Count++;
        }

        public V Pop()
        {
            if (Start == null)
            {
                throw new Exception("Queue is empty");
            }

            V removedNodeValue = Start.Value;

            Start = Start.Next;

            Count--;

            return removedNodeValue;
        }
    }
}
