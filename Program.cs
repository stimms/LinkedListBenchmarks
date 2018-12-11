using System;
using System.Collections.Generic;
using System.Linq;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;

namespace linkedlist
{
    class Program
    {
        static void Main(string[] args)
        {
            var summary = BenchmarkRunner.Run<LinkedListAddTests>();
            var summary2 = BenchmarkRunner.Run<LinkedListAddWithRetainedTailTests>();
            var summary3 = BenchmarkRunner.Run<LinkedListRemoveTests>();
            var summary4 = BenchmarkRunner.Run<LinkedListAddMiddleTests>();
            var summary6 = BenchmarkRunner.Run<LinkedListRandomAccessTests>();


        }
    }

    public class LinkedListAddTests
    {
        int[] data;
        public LinkedListAddTests()
        {
            var random = new Random();
            data = new int[10_000];
            for (int i = 0; i < 10_000; i++)
            {
                data[i] = random.Next();
            }
        }
        [Benchmark]
        public LinkedList<int> LinkedList()
        {
            var list = new LinkedList<int>();
            foreach (var item in data)
            {
                list.AddLast(item);
            }
            return list;
        }

        [Benchmark]
        public List<int> List()
        {
            var list = new List<int>();
            foreach (var item in data)
            {
                list.Add(item);
            }
            return list;
        }
    }

    public class LinkedListAddWithRetainedTailTests
    {
        int[] data;
        public LinkedListAddWithRetainedTailTests()
        {
            var random = new Random();
            data = new int[10_000];
            for (int i = 0; i < 10_000; i++)
            {
                data[i] = random.Next();
            }
        }
        [Benchmark]
        public LinkedList<int> LinkedList()
        {
            var list = new LinkedList<int>();
            LinkedListNode<int> currentNode = new LinkedListNode<int>(data[0]);
            list.AddFirst(currentNode);
            foreach (var item in data.Skip(1))
            {
                currentNode = list.AddAfter(currentNode, item);
            }
            return list;
        }

        [Benchmark]
        public List<int> List()
        {
            var list = new List<int>();
            foreach (var item in data)
            {
                list.Add(item);
            }
            return list;
        }
    }

    public class LinkedListRandomAccessTests
    {
        Random rand = new Random();
        int[] array_data;
        LinkedList<int> linkedlist_data;
        public LinkedListRandomAccessTests()
        {

            array_data = new int[10_000];
            linkedlist_data = new LinkedList<int>();
            for (int i = 0; i < 10_000; i++)
            {
                array_data[i] = rand.Next();
                linkedlist_data.AddLast(rand.Next());
            }

        }
        [Benchmark]
        public int LinkedList()
        {
            var index = rand.Next(10_000);
            var currentNode = linkedlist_data.First;
            for (int i = 0; i < index; i++)
            {
                currentNode = currentNode.Next;
            }
            var result = currentNode.Value;
            return result;
        }

        [Benchmark]
        public int Array()
        {
            var index = rand.Next(10_000);
            var result = array_data[index];
            return result;
        }
    }

    public class LinkedListRemoveTests
    {
        Random rand = new Random();
        List<int> list_data;
        LinkedList<int> linkedlist_data;
        public LinkedListRemoveTests()
        {
            list_data = new List<int>();
            linkedlist_data = new LinkedList<int>();
            for (int i = 0; i < 10_000; i++)
            {
                list_data.Add(rand.Next());
                linkedlist_data.AddLast(rand.Next());
            }

        }
        [Benchmark]
        public LinkedList<int> LinkedList()
        {
            var currentNode = linkedlist_data.First;
            for (int i = 0; i < 10_000 && currentNode != null && currentNode.Next != null; i++)
            {
                if (i % 2 == 0)
                {
                    linkedlist_data.Remove(currentNode.Next);
                }
                currentNode = currentNode.Next;
            }
            return linkedlist_data;
        }

        [Benchmark]
        public List<int> List()
        {
            var result = list_data.Where((item, index) => index % 2 == 0);
            return result;
        }
    }

    public class LinkedListAddMiddleTests
    {
        Random rand = new Random();
        List<int> list_data;
        LinkedList<int> linkedlist_data;
        LinkedListNode<int> middle;
        const int listLength = 1000;
        public LinkedListAddMiddleTests()
        {
            list_data = new List<int>();
            linkedlist_data = new LinkedList<int>();
            
            for (int i = 0; i < listLength; i++)
            {
                list_data.Add(rand.Next());
                linkedlist_data.AddLast(rand.Next());
            }
            middle = linkedlist_data.First;
            for (int i = 0; i < listLength / 2; i++)
            {
                middle = middle.Next;
            }
        }
        [Benchmark]
        public LinkedList<int> LinkedList()
        {
            linkedlist_data.AddAfter(middle, 77);
            return linkedlist_data;
        }

        [Benchmark]
        public List<int> List()
        {
            list_data.Insert(listLength/2, 77);
            return list_data;
        }
    }
}
