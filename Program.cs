using System;
using System.Linq;
using LinkedList;

namespace LinkedListTest{
    internal class Program{
        private static void Main(){
            var intList = new LinkedList<int>{22, 33, 44};

            for (int i = 0; i < 100000; i++){
                intList.Insert(i, intList.Length/((i%3) + 1));
            }
            Console.WriteLine(intList.Length);

        }
    }
}