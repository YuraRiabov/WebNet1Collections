using System.Collections;

namespace Collections;

public class MyQueue<T> : IEnumerable<T>
{
    private MyQueueNode<T>? _first;
    private MyQueueNode<T>? _last;
    public IEnumerator<T> GetEnumerator()
    {
        var currentNode = _first;
        while (currentNode is not null)
        {
            yield return currentNode.Value;
            currentNode = currentNode.Next;
        }
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    private class MyQueueNode<TValue>
    {
        public TValue Value { get; set; }
        public MyQueueNode<TValue>? Next { get; set; }
        public bool HasNext => Next is null;

        public MyQueueNode(TValue value)
        {
            Value = value;
        }
    }
}