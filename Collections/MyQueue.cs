﻿using System.Collections;

namespace Collections;

public class MyQueue<T> : IEnumerable<T>, ICollection
{
    private MyQueueNode<T>? _first;
    private MyQueueNode<T>? _last;

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