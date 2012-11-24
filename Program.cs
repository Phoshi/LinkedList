using System;
using System.Diagnostics;
using System.Linq;
using LinkedList;

namespace LinkedListTest{
    internal class Program{
        private static void Main(){
            var intList = new LinkedList<int>{0};
            var skipList = new LinkedList<int>(useSkipList: true){0};

            for (int i = 1; i < 10000; i++){
                intList.Insert(i, intList.Length/3);
                skipList.Insert(i, skipList.Length/3);
                intList.Insert(i, i-1);
                skipList.Insert(i, i-1);
                //for (int j = 0; j < intList.Length; j++) {
                //    if (intList[j] != skipList[j]) {
                //        Console.WriteLine(string.Format("{0}\t{1}", intList[i], skipList[i]));
                //        Debugger.Break();
                //    }
                //}
            }

            for (int i = 9999; i > 0; i-=3){
                intList.Remove(i);
                skipList.Remove(i);
            }

            

            Console.WriteLine(intList.Length);
            for (int i = 0; i < intList.Length; i++){
                Console.WriteLine(string.Format("{0}\t{1}", intList[i], skipList[i]));
                if (intList[i] != skipList[i]){
                    Debugger.Break();
                }
            }

            Console.WriteLine(intList.Equals(skipList));

            int total = 0;
            var list = skipList;
            for (int i = 0; i < 20000000; i++){
                total += list[i%list.Length];
            }

        //}
    }
}