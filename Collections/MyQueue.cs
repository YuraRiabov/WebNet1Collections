using System.Collections;
using System.Diagnostics.CodeAnalysis;

namespace Collections;

public class MyQueue<T> : IEnumerable<T>, ICollection
{
    private MyQueueNode? _first;
    private MyQueueNode? _last;

    public event Action? LastElementRemoved;
    public event Action? LastElementLeft;

    public int Count
    {
        get
        {
            var count = 0;
            var currentItem = _first;
            while (currentItem is not null)
            {
                count++;
                currentItem = currentItem.Next;
            }

            return count;
        }
    }
    public bool IsSynchronized => false;
    public object SyncRoot => this;

    public MyQueue()
    {
        _first = null;
        _last = null;
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
        var newNode = new MyQueueNode(value);
        
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

        return DequeueInternal();
    }

    public bool TryDequeue([MaybeNullWhen(false)] out T value)
    {
        if (_first is null)
        {
            value = default;
            return false;
        }

        value = DequeueInternal();
        return true;
    }

    public T Peek()
    {
        if (_first is null)
        {
            throw new InvalidOperationException();
        }

        return _first.Value;
    }

    public bool TryPeek([MaybeNullWhen(false)] out T value)
    {
        if (_first is null)
        {
            value = default;
            return false;
        }

        value = _first.Value;
        return true;
    }

    public void Clear()
    {
        _first = null;
        _last = null;
        LastElementRemoved?.Invoke();
    }

    public bool Contains(T item)
    {
        var currentItem = _first;
        while (currentItem is not null)
        {
            if (currentItem.Value!.Equals(item))
            {
                return true;
            }

            currentItem = currentItem.Next;
        }

        return false;
    }

    public T[] ToArray()
    {
        var array = new T[Count];
        CopyToInternal(array, 0);
        return array;
    }
    
    public void CopyTo(T[] array, int index)
    {
        if (!IsValidArrayLengthIndexForCopy(array, index))
        {
            throw new ArgumentOutOfRangeException();
        }
        
        CopyToInternal(array, index);
    }
    
    void ICollection.CopyTo(Array array, int index)
    {
        if (!IsValidArrayLengthIndexForCopy(array, index))
        {
            throw new ArgumentOutOfRangeException();
        }

        if (!IsValidArrayTypeForCopy(array))
        {
            throw new ArgumentException(nameof(array));
        }
        
        CopyToInternal(array, index);
    }
    
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

    private T DequeueInternal()
    {
        var first = _first;
        _first = _first!.Next;

        if (_first is not null && !_first.HasNext)
        {
            _last = null;
            LastElementLeft?.Invoke();
        }

        if (_first is null)
        {
            LastElementRemoved?.Invoke();
        }

        return first!.Value;
    }

    private void CopyToInternal(Array array, int index)
    {
        var currentIndex = index;
        var currentItem = _first;
        while (currentItem is not null)
        {
            array.SetValue(currentItem.Value, currentIndex);
            currentItem = currentItem.Next;
            currentIndex++;
        }
    }

    private bool IsValidArrayLengthIndexForCopy(Array array, int index)
    {
        if (array is null)
        {
            return false;
        }

        if (index < 0 || index >= array.Length)
        {
            return false;
        }

        return array.Length - index >= Count;
    }

    private bool IsValidArrayTypeForCopy(Array array)
    {
        return array.Rank == 1 && array.GetLowerBound(0) == 0;
    }

    private class MyQueueNode
    {
        public T Value { get; }
        public MyQueueNode? Next { get; set; }
        public bool HasNext => Next is not null;

        public MyQueueNode(T value)
        {
            Value = value;
        }
    }
}