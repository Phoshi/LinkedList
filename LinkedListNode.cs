namespace LinkedList {
    /// <summary>
    /// Represents a single node inside a linked list
    /// </summary>
    /// <typeparam name="T">The type to store</typeparam>
    internal class LinkedListNode<T> {
        /// <summary>
        /// The value stored inside this node
        /// </summary>
        public T Item { get; internal set; }
        /// <summary>
        /// The node after this in the list, or null if the end node.
        /// </summary>
        public LinkedListNode<T> Next { get; set; }
        /// <summary>
        /// The node before this in the list, or null if the start node.
        /// </summary>
        public LinkedListNode<T> Previous { get; set; } 

        /// <summary>
        /// Whether this node represents the end of the list
        /// </summary>
        public bool IsEndNode{
            get { return Next==null; }
        }

        /// <summary>
        /// Instantiates a new LinkedListNode structure
        /// </summary>
        /// <param name="item">The element to store</param>
        /// <param name="next">The item after this in the list</param>
        public LinkedListNode(T item, LinkedListNode<T> next) {
            Item = item;
            Next = next;
            next.Previous = this;
        }

        /// <summary>
        /// Creates a new blank LinkedListNode structure
        /// </summary>
        public LinkedListNode(){}

        /// <summary>
        /// Returns a string representation of the node and those after it.
        /// </summary>
        /// <returns></returns>
        public override string ToString(){
            return Next == null ? "End" : string.Format("{0} -> {1}", Item, Next);
        }
    }

}
