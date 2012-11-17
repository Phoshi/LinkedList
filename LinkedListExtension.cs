using System.Collections.Generic;

namespace LinkedList {
    static internal class LinkedListExtension {
        public static LinkedList<T> ToLinkedList<T>(this IEnumerable<T> enumerable){
            return new LinkedList<T>(enumerable);
        } 
    }
}
