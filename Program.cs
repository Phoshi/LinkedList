using System;
using System.Linq;
using LinkedList;

namespace LinkedListTest{
    internal class Program{
        private static void Main(){
            var intList = new LinkedList<int>{22, 33, 44};

            //LinkedList<string> stringList = new List<string>{"hi!", "boop!"}.ToLinkedList();
            //Console.WriteLine(stringList);


            intList.Append(1);
            intList.Append(2);
            intList.Append(3);

            Console.WriteLine(intList.Contains(99));
            Console.WriteLine(intList.Contains(22));

            intList.Prepend(0);
            intList.Prepend(-1);

            intList.Insert(17, 3);

            intList.Remove(2);
            Console.WriteLine(intList);

            intList.RemoveAt(1);
            Console.WriteLine(intList);

            Console.WriteLine(intList[0]);
            Console.WriteLine(intList[1]);
            Console.WriteLine(intList[2]);

            foreach (var i in intList){
                Console.WriteLine(i);
            }

            Console.WriteLine(string.Join(", ", from number in intList where number > 20 select number));

            int j = 0;
            while (j < 5000){
                j++;
                intList.Append(j);
            }
            Console.WriteLine(intList);


        }
    }
}