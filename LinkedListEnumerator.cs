using System.Collections;
using System.Collections.Generic;

namespace LinkedList {
    internal class LinkedListEnumerator<T> : IEnumerator<T>{
        private readonly LinkedListNode<T> _headNode;
        private LinkedListNode<T> _currentNode;
 
        public LinkedListEnumerator(LinkedListNode<T> head){
            _headNode = head;

        } 
        public void Dispose(){}

        public bool MoveNext(){
            _currentNode = _currentNode == null ? _headNode : _currentNode.Next;

            return !_currentNode.IsEndNode;
        }

        public void Reset(){
            _currentNode = null;
        }

        public T Current{
            get { return _currentNode.Item; }
        }

        object IEnumerator.Current{
            get { return Current; }
        }
    }
}
