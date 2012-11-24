using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace LinkedList {
    public class LinkedList<T> : IEnumerable<T>{
        private LinkedListNode<T> _head = new LinkedListNode<T>();
        private LinkedListNode<T> _tail; 
        private readonly int _maxLength;

        private const int SKIPLIST_DENSITY = 250;
        private LinkedList<LinkedListNode<T>> _skipList; 

        /// <summary>
        /// The first element node of the list
        /// </summary>
        private LinkedListNode<T> HeadNode{
            get { return _head; }
            set { _head = value; }
        }

        private LinkedListNode<T> TailNode{
            get { return _tail; }
            set { _tail = value; }
        }

        /// <summary>
        /// Whether the list uses a SkipList to significantly improve fetch times at the cost of insertion and removal times.
        /// </summary>
        public bool UseSkiplist { get; internal set; }

        /// <summary>
        /// Whether the list is empty
        /// </summary>
        public bool IsEmpty{
            get { return _head.IsEndNode; }
        }

        /// <summary>
        /// Whether the list is full
        /// </summary>
        public bool IsFull{
            get { return Length == MaxCapacity; }
        }

        private int _length;

        /// <summary>
        /// The current length of the list
        /// </summary>
        public int Length{
            get{
                return _length;
            }
        }

        /// <summary>
        /// The maximum length, or -1 for infinite
        /// </summary>
        public int MaxCapacity{
            get { return _maxLength; }
        }


        /// <summary>
        /// Creates a new LinkedList
        /// </summary>
        /// <param name="values">Optional values to populate the list with</param>
        /// <param name="length">Optional max length</param>
        /// <param name="useSkipList">Whether to use a skip list for this list.</param>
        public LinkedList(IEnumerable<T> values = null, int length = -1, bool useSkipList = false){
            _maxLength = length;
            if (values != null){
                foreach (var value in values){
                    Append(value);
                }
            }

            UseSkiplist = useSkipList;
        }

        /// <summary>
        /// Inserts a new value into the list at the specified position
        /// </summary>
        /// <param name="value">The value to insert</param>
        /// <param name="position">The position to insert it at</param>
        public void Insert(T value, int position){
            if (IsFull){
                throw new ArgumentOutOfRangeException("List is full!");
            }
            if (position < 0){
                throw new ArgumentOutOfRangeException("Cannot insert item outside of range.");
            }
            LinkedListNode<T> newNode;
            if (position == 0) {
                newNode = new LinkedListNode<T>(value, HeadNode);
                HeadNode = newNode;
            }
            else{
                var searchNode = GetNodeAt(position);

                var previousNode = searchNode.Previous;
                newNode = new LinkedListNode<T>(value, searchNode);
                previousNode.Next = newNode;
                newNode.Previous = previousNode;
            }

            if (position==Length){
                _tail = newNode;
            }
            _length++;
            if (UseSkiplist && _skipList != null) {
                if (position < (_skipList.Length * SKIPLIST_DENSITY)) {
                    int startIndex = (position) / SKIPLIST_DENSITY;
                    for (int i = startIndex; i < _skipList.Length; i++) {
                        _skipList.Insert(GetNodeAt(i * SKIPLIST_DENSITY), i);
                        _skipList.RemoveAt(i + 1);
                    }
                }
                if (position != 0 && ((Length-1) % SKIPLIST_DENSITY) == 0){
                    _skipList.Append(GetNodeAt(Length-1));
                }
            }
            else{
                if (position != 0 && ((Length-1) % SKIPLIST_DENSITY) == 0) {
                    _skipList = new LinkedList<LinkedListNode<T>> { HeadNode };
                    _skipList.Append(newNode);
                }
            }
        }

        /// <summary>
        /// Prepends a new value to the start of the list
        /// </summary>
        /// <param name="value">Value to insert</param>
        public void Prepend(T value){
            Insert(value, 0);
        }

        /// <summary>
        /// Appends a new value to the end of the list
        /// </summary>
        /// <param name="value">Value to insert</param>
        public void Append(T value){
            Insert(value, Length);
        }

        /// <summary>
        /// Adds a value to the end of the list
        /// </summary>
        /// <param name="value">The value to add</param>
        public void Add(T value){
            Append(value);
        }

        /// <summary>
        /// Removes the specified item
        /// </summary>
        /// <param name="value">The item to remove</param>
        public void Remove(T value){
            var index = GetIndexOf(value);
            RemoveAt(index);
        }

        /// <summary>
        /// Removes the item at the specified location
        /// </summary>
        /// <param name="index">The index</param>
        public void RemoveAt(int index){
            var node = GetNodeAt(index);
            if (index != 0){
                node.Previous.Next = node.Next;
            }
            else{
                HeadNode = node.Next;
            }
            node.Next.Previous = node.Previous;

            if (_skipList != null && index < (_skipList.Length * SKIPLIST_DENSITY)) {
                int startIndex = (index) / SKIPLIST_DENSITY;
                for (int i = startIndex + 1; i < _skipList.Length; i++) {
                    _skipList.RemoveAt(i);
                    if (i * SKIPLIST_DENSITY <= Length){
                        _skipList.Insert(GetNodeAt(i*SKIPLIST_DENSITY), i);
                    }
                }
            }
            _length--;
        }

        /// <summary>
        /// Returns the index of a specified value
        /// </summary>
        /// <param name="value">The value to find</param>
        /// <returns>The index of this, or -1 for not found</returns>
        public int GetIndexOf(T value){
            if (IsEmpty){
                return -1;
            }
            var searchIndex = 0;
            var searchNode = HeadNode;

            while (!searchNode.Item.Equals(value)) {
                searchNode = searchNode.Next;
                searchIndex++;

                if (searchNode.IsEndNode) {
                    return -1;
                }
            }
            return searchIndex;
        }

        /// <summary>
        /// Returns whether the list contains an item
        /// </summary>
        /// <param name="value">The item</param>
        /// <returns>Whether the list contains the item</returns>
        public bool Contains(T value){
            return GetIndexOf(value) != -1;
        }

        /// <summary>
        /// Empties the list
        /// </summary>
        public void Clear(){
            HeadNode = new LinkedListNode<T>();

            _length = 0;
        }

        /// <summary>
        /// Accesses an indexed element on the list
        /// </summary>
        /// <param name="index">The index to retrieve</param>
        /// <returns>The item at that index</returns>
        public T this[int index]{
            get { 
                return GetNodeAt(index).Item;
            }
        }

        /// <summary>
        /// Returns the node at the specified index
        /// </summary>
        /// <param name="index">The index to retrieve</param>
        /// <param name="skipList">Whether to use the skiplist to increase the speed, if possible. Usually desirable, but not if the skiplist is being updated.</param>
        /// <returns>The node</returns>
        private LinkedListNode<T> GetNodeAt(int index, bool skipList = true){
            if (index < 0 || index > Length) {
                throw new IndexOutOfRangeException();
            }

            if ((UseSkiplist&&skipList) || index <= Length / 2){
                var searchIndex = 0;
                var searchNode = HeadNode;

                if ((UseSkiplist&&skipList) && _skipList != null){
                    int skipListIndex = 0;
                    while (index > SKIPLIST_DENSITY){
                        index -= SKIPLIST_DENSITY;
                        skipListIndex++;
                    }
                    searchNode = _skipList[skipListIndex];
                }

                while (searchIndex < index){
                    searchNode = searchNode.Next;
                    searchIndex++;
                }
                return searchNode;
            }
            else{
                var searchIndex = Length;
                var searchNode = TailNode.Next;

                while (searchIndex > index) {
                    searchNode = searchNode.Previous;
                    searchIndex--;
                }
                return searchNode;
            }
        }

        /// <summary>
        /// Returns the class's enumerator
        /// </summary>
        /// <returns></returns>
        public IEnumerator<T> GetEnumerator(){
            return new LinkedListEnumerator<T>(HeadNode);
        }

        /// <summary>
        /// The string representation of the list.
        /// </summary>
        /// <returns>A string representing the list</returns>
        public override string ToString(){
            return String.Format("{0} elements: {1}", Length, HeadNode);
        }

        /// <summary>
        /// Returns the enumerator
        /// </summary>
        /// <returns></returns>
        IEnumerator IEnumerable.GetEnumerator(){
            return GetEnumerator();
        }
    }

    [Serializable]
    class ValueNotFoundException : Exception{

        public ValueNotFoundException(string message) : base(message){}

        public ValueNotFoundException(string message, Exception innerException) : base(message, innerException){}

        public ValueNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context){}

    }
}
