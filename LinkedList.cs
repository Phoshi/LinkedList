using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace LinkedList{
    public class LinkedList<T> : IEnumerable<T>{
        private LinkedListNode<T> _head = new LinkedListNode<T>();
        private readonly int _maxLength;

        /// <summary>
        /// The first element node of the list
        /// </summary>
        private LinkedListNode<T> HeadNode{
            get { return _head; }
            set { _head = value; }
        }

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

        /// <summary>
        /// The current length of the list
        /// </summary>
        public int Length{
            get{
                var length = 0;
                var searchNode = HeadNode;
                while (!searchNode.IsEndNode){
                    searchNode = searchNode.Next;
                    length++;
                }
                return length;
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
        public LinkedList(IEnumerable<T> values = null, int length = -1){
            _maxLength = length;
            if (values != null){
                foreach (var value in values){
                    Append(value);
                }
            }
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
            if (position == 0){
                newNode = new LinkedListNode<T>(value, HeadNode);
                HeadNode = newNode;
                return;
            }

            var searchNode = GetNodeAt(position);

            var previousNode = searchNode.Previous;
            newNode = new LinkedListNode<T>(value, searchNode);
            previousNode.Next = newNode;
            newNode.Previous = previousNode;
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
            node.Previous.Next = node.Next;
            node.Next.Previous = node.Previous;
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

            while (!searchNode.Item.Equals(value)){
                searchNode = searchNode.Next;
                searchIndex++;

                if (searchNode.IsEndNode){
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
        }

        /// <summary>
        /// Accesses an indexed element on the list
        /// </summary>
        /// <param name="index">The index to retrieve</param>
        /// <returns>The item at that index</returns>
        public T this[int index]{
            get { return GetNodeAt(index).Item; }
        }

        /// <summary>
        /// Returns the node at the specified index
        /// </summary>
        /// <param name="index">The index to retrieve</param>
        /// <returns>The node</returns>
        private LinkedListNode<T> GetNodeAt(int index){
            var searchIndex = 0;
            var searchNode = HeadNode;

            while (searchIndex < index){
                searchNode = searchNode.Next;
                searchIndex++;
            }
            return searchNode;
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
    internal class ValueNotFoundException : Exception{
        public ValueNotFoundException(string message) : base(message){}

        public ValueNotFoundException(string message, Exception innerException) : base(message, innerException){}

        public ValueNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context){}
    }
}