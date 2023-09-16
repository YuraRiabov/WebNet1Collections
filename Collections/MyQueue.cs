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

    public MyQueue()
    {
        
    }

    public MyQueue(IEnumerable<T> source)
    {
        foreach (var value in source)
        {
            Enqueue(value);
        }
    }

    public void Enqueue(T value)
    {
        var newNode = new MyQueueNode<T>(value);
        
        if (_first is null)
        {
            _first = newNode;
            return;
        }

        if (_last is null)
        {
            _first.Next = newNode;
            _last = newNode;
            return;
        }

        _last.Next = newNode;
        _last = newNode;
    }

    public T Dequeue()
    {
        if (_first is null)
        {
            throw new InvalidOperationException();
        }
        
        var first = _first;
        _first = _first.Next;

        if (_first is not null && !first.HasNext)
        {
            _last = null;
        }
        
        return first.Value;
    }

    public T Peek()
    {
        if (_first is null)
        {
            throw new InvalidOperationException();
        }

        return _first.Value;
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