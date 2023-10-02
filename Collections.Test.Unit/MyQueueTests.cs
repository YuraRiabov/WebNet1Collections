using System.Collections;
using Xunit;

namespace Collections.Test.Unit;

public class MyQueueTests
{
    private static readonly int[] NumberArray = { 1, 2, 3, 4 };
    private static readonly TestStructure[] StructureArray = { new(1), new(2), new(3), new(4) };
    private static readonly TestClass[] ObjectArray = { new(1), new(2), null, new(3), new(4) };
    
    [Theory]
    [MemberData(nameof(GetEmptyQueuesTestData))]
    public void GetEnumerator_WhenEmptyQueue_ShouldReturnMoveNextFalse<T>(MyQueue<T> queue)
    {
        using var enumerator = queue.GetEnumerator();
        
        var moveNextResult = enumerator.MoveNext();
        var current = enumerator.Current;
        
        Assert.False(moveNextResult);
        Assert.Equal(default, current);
    }

    [Theory]
    [MemberData(nameof(GetTestDataForQueueFill))]
    public void Enqueue_WhenEmptyQueue_ShouldAddElements<T>(T[] values)
    {
        var queue = new MyQueue<T>();
        foreach (var value in values)
        {
            queue.Enqueue(value);
        }

        AssertEqualCollections(values, queue);
    }
    
    [Theory]
    [MemberData(nameof(GetTestDataForQueueFill))]
    public void Enqueue_WhenQueueCleared_ShouldAddElements<T>(T[] values)
    {
        var queue = new MyQueue<T>(values);
        
        queue.Clear();
        foreach (var value in values)
        {
            queue.Enqueue(value);
        }

        AssertEqualCollections(values, queue);
    }
    
    [Theory]
    [MemberData(nameof(GetTestDataForQueueFill))]
    public void Enqueue_WhenNotEmptyQueue_ShouldAddElements<T>(T[] values)
    {
        var queue = new MyQueue<T>(values);
        foreach (var value in values)
        {
            queue.Enqueue(value);
        }

        Assert.Equal(values.Length * 2, queue.Count);
    }

    [Theory]
    [MemberData(nameof(GetTestDataForQueueFill))]
    public void EnumerableConstructor_ShouldAddElements<T>(T[] values)
    {
        var queue = new MyQueue<T>(values);

        AssertEqualCollections(values, queue);
    }


    [Theory]
    [MemberData(nameof(GetTestDataForQueueFill))]
    public void Dequeue_WhenNotEmptyQueue_ShouldReturnAndRemove<T>(T[] values)
    {
        var queue = new MyQueue<T>(values);
        
        foreach (T value in values)
        {
            var currentValue = queue.Dequeue();

            Assert.Equal(value, currentValue);
        }
        
        Assert.Empty(queue);
    }
    
    [Theory]
    [MemberData(nameof(GetEmptyQueuesTestData))]
    public void Dequeue_WhenEmptyQueue_ShouldThrow<T>(MyQueue<T> queue)
    {
        var call = () =>
        {
            queue.Dequeue();
        };

        Assert.Throws<InvalidOperationException>(call);
    }
    
    [Theory]
    [MemberData(nameof(GetTestDataForQueueFill))]
    public void TryDequeue_WhenNotEmptyQueue_ShouldReturnTrueAndRemove<T>(T[] values)
    {
        var queue = new MyQueue<T>(values);

        foreach (T value in values)
        {
            bool tryDequeueResult = queue.TryDequeue(out var currentValue);

            Assert.Equal(value, currentValue);
            Assert.True(tryDequeueResult);
        }
        
        Assert.Empty(queue);
    }
    
    [Theory]
    [MemberData(nameof(GetEmptyQueuesTestData))]
    public void TryDequeue_WhenEmptyQueue_ShouldReturnFalse<T>(MyQueue<T> queue)
    {
        var tryDequeueResult = queue.TryDequeue(out var currentValue);
        
        Assert.Empty(queue);
        Assert.False(tryDequeueResult);
        Assert.Equal(default, currentValue);
    }
    
    [Theory]
    [MemberData(nameof(GetTestDataForQueueFill))]
    public void Peek_WhenNotEmptyQueue_ShouldReturnAndNotRemove<T>(T[] values)
    {
        var queue = new MyQueue<T>(values);
        
        foreach (T value in values)
        {
            var currentValue = queue.Peek();

            Assert.Equal(value, currentValue);

            queue.Dequeue();
        }
        
        Assert.Empty(queue);
    }
    
    [Theory]
    [MemberData(nameof(GetEmptyQueuesTestData))]
    public void Peek_WhenEmptyQueue_ShouldThrow<T>(MyQueue<T> queue)
    {
        var call = () =>
        {
            queue.Peek();
        };

        Assert.Throws<InvalidOperationException>(call);
    }
    
    [Theory]
    [MemberData(nameof(GetTestDataForQueueFill))]
    public void TryPeek_WhenNotEmptyQueue_ShouldReturnTrueAndNotRemove<T>(T[] values)
    {
        var queue = new MyQueue<T>(values);

        foreach (T value in values)
        {
            bool tryDequeueResult = queue.TryPeek(out var currentValue);

            Assert.Equal(value, currentValue);
            Assert.True(tryDequeueResult);

            queue.Dequeue();
        }
        
        Assert.Empty(queue);
    }
    
    [Theory]
    [MemberData(nameof(GetEmptyQueuesTestData))]
    public void TryPeek_WhenEmptyQueue_ShouldReturnFalse<T>(MyQueue<T> queue)
    {
        var tryDequeueResult = queue.TryPeek(out var currentValue);
        
        Assert.Empty(queue);
        Assert.False(tryDequeueResult);
        Assert.Equal(default, currentValue);
    }

    [Theory]
    [MemberData(nameof(GetTestDataForQueueFill))]
    public void Clear_ShouldMakeQueueEmpty<T>(T[] values)
    {
        var queue = new MyQueue<T>(values);
        
        queue.Clear();
        
        Assert.Empty(queue);
    }
    
    [Theory]
    [MemberData(nameof(GetTestDataForQueueFill))]
    public void Contains_WhenHasElement_ShouldBeTrue<T>(T[] values)
    {
        var queue = new MyQueue<T>(values);

        foreach (var value in values)
        {
            var containsResult = queue.Contains(value);
        
            Assert.True(containsResult);
        }
    }
    
    [Theory]
    [MemberData(nameof(GetTestDataForQueueFill))]
    public void Contains_WhenHasNoElement_ShouldBeFalse<T>(T[] values)
    {
        var queue = new MyQueue<T>(values);

        queue.Dequeue();
        var containsResult = queue.Contains(values[0]);
    
        Assert.False(containsResult);
    }
    
    [Theory]
    [MemberData(nameof(GetTestDataForQueueFill))]
    public void Count_WhenElementsChange_ShouldBeCorrect<T>(T[] values)
    {
        var queue = new MyQueue<T>();
        for (int i = 0; i < values.Length; i++)
        {
            queue.Enqueue(values[i]);
            
            Assert.Equal(i + 1, queue.Count);
        }
        
        queue.Clear();
        Assert.Equal(0, queue.Count);
    }

    [Theory]
    [MemberData(nameof(GetEmptyQueuesTestData))]
    public void ToArray_WhenEmptyQueue_ShouldReturnEmptyArray<T>(MyQueue<T> queue)
    {
        var array = queue.ToArray();
        
        Assert.Empty(array);
    }
    
    [Theory]
    [MemberData(nameof(GetTestDataForQueueFill))]
    public void ToArray_WhenNotEmptyQueue_ShouldReturnArray<T>(T[] values)
    {
        var queue = new MyQueue<T>(values);

        var array = queue.ToArray();
        
        AssertEqualCollections(array, values);
    }
    
    [Theory]
    [MemberData(nameof(GetTestDataForQueueFill))]
    public void CopyTo_WhenValidParams_ShouldCopy<T>(T[] values)
    {
        var queue = new MyQueue<T>(values);

        var array = new T[values.Length];
        
        queue.CopyTo(array, 0);
        
        AssertEqualCollections(array, values);
    }
    
    [Theory]
    [MemberData(nameof(GetTestDataForQueueFill))]
    public void CopyTo_WhenLongerArray_ShouldCopy<T>(T[] values)
    {
        var queue = new MyQueue<T>(values);

        var array = new T[values.Length];

        queue.Dequeue();
        
        queue.CopyTo(array, 1);
        
        Assert.DoesNotContain(values[0], array);
        Assert.Equal(default, array[0]);
        for (var i = 1; i < values.Length; i++)
        {
            Assert.Equal(values[i], array[i]);
        }
    }
    
    [Theory]
    [MemberData(nameof(GetTestDataForQueueFill))]
    public void CopyTo_WhenShortArray_ShouldThrow<T>(T[] values)
    {
        var queue = new MyQueue<T>(values);

        var array = new T[values.Length - 1];
        
        var code = () => queue.CopyTo(array, 0);

        Assert.Throws<ArgumentOutOfRangeException>(code);
    }
    
    [Theory]
    [MemberData(nameof(GetTestDataForQueueFill))]
    public void CopyTo_WhenNullArray_ShouldThrow<T>(T[] values)
    {
        var queue = new MyQueue<T>(values);

        T[] array = null;
        
        var code = () => queue.CopyTo(array, 0);

        Assert.Throws<ArgumentOutOfRangeException>(code);
    }
    
    [Theory]
    [MemberData(nameof(GetTestDataForQueueFill))]
    public void CopyTo_WhenInvalidIndex_ShouldThrow<T>(T[] values)
    {
        var queue = new MyQueue<T>(values);

        var array = new T[values.Length];
        
        var code = () => queue.CopyTo(array,values.Length + 1);

        Assert.Throws<ArgumentOutOfRangeException>(code);
    }
    
    [Theory]
    [MemberData(nameof(GetTestDataForQueueFill))]
    public void CopyToNonGeneric_WhenValidParams_ShouldCopy<T>(T[] values)
    {
        ICollection queue = new MyQueue<T>(values);

        Array array = new T[values.Length];
        
        queue.CopyTo(array, 0);
        
        Assert.Equal(array.Length, values.Length);
        Assert.Equal(array.GetValue(0), values[0]);
    }
    
    [Theory]
    [MemberData(nameof(GetTestDataForQueueFill))]
    public void CopyToNonGeneric_WhenLongerArray_ShouldCopy<T>(T[] values)
    {
        ICollection queue = new MyQueue<T>(values);

        Array array = new T[values.Length + 1];
        
        queue.CopyTo(array, 1);
        
        Assert.Equal(default(T), array.GetValue(0));
        for (var i = 0; i < values.Length; i++)
        {
            Assert.Equal(values[i], array.GetValue(i + 1));
        }
    }
    
    [Theory]
    [MemberData(nameof(GetTestDataForQueueFill))]
    public void CopyToNotGeneric_WhenShortArray_ShouldThrow<T>(T[] values)
    {
        ICollection queue = new MyQueue<T>(values);

        Array array = new T[values.Length - 1];
        
        var code = () => queue.CopyTo(array, 0);

        Assert.Throws<ArgumentOutOfRangeException>(code);
    }
    
    [Theory]
    [MemberData(nameof(GetTestDataForQueueFill))]
    public void CopyToNotGeneric_WhenNullArray_ShouldThrow<T>(T[] values)
    {
        ICollection queue = new MyQueue<T>(values);

        Array array = null;
        
        var code = () => queue.CopyTo(array, 0);

        Assert.Throws<ArgumentOutOfRangeException>(code);
    }
    
    [Theory]
    [MemberData(nameof(GetTestDataForQueueFill))]
    public void CopyToNotGeneric_WhenInvalidIndex_ShouldThrow<T>(T[] values)
    {
        ICollection queue = new MyQueue<T>(values);

        Array array = new T[values.Length];
        
        var code = () => queue.CopyTo(array,values.Length + 1);

        Assert.Throws<ArgumentOutOfRangeException>(code);
    }
    
    [Theory]
    [MemberData(nameof(GetTestDataForQueueFill))]
    public void CopyToNotGeneric_WhenInvalidRank_ShouldThrow<T>(T[] values)
    {
        ICollection queue = new MyQueue<T>(values);

        Array array = Array.CreateInstance(typeof(T), new[] { values.Length, values.Length }, new[] { 0, 0 });
        
        var code = () => queue.CopyTo(array,0);

        Assert.Throws<ArgumentException>(code);
    }
    
    [Theory]
    [MemberData(nameof(GetTestDataForQueueFill))]
    public void CopyToNotGeneric_WhenInvalidLowerBound_ShouldThrow<T>(T[] values)
    {
        ICollection queue = new MyQueue<T>(values);

        Array array = Array.CreateInstance(typeof(T), new[] { values.Length }, new[] { 1 });
        
        var code = () => queue.CopyTo(array,0);

        Assert.Throws<ArgumentException>(code);
    }

    private static void AssertEqualCollections<T>(IEnumerable<T> expected, IEnumerable<T> actual)
    {
        using var enumerator = actual.GetEnumerator();
        foreach (T value in expected)
        {
            enumerator.MoveNext();
            var currentValue = enumerator.Current;

            Assert.Equal(value, currentValue);
        }
    }

    public static IEnumerable<object[]> GetEmptyQueuesTestData()
    {
        yield return new object[] { new MyQueue<int>() };
        yield return new object[] { new MyQueue<TestStructure>() };
        yield return new object[] { new MyQueue<TestClass>() };
    }
    
    public static IEnumerable<object[]> GetTestDataForQueueFill()
    {
        yield return new object[] { NumberArray };
        yield return new object[] { StructureArray };
        yield return new object[] { ObjectArray };
    }
    
    private class TestClass
    {
        public int Value { get; set; }

        public TestClass(int value)
        {
            Value = value;
        }
    }
    
    private struct TestStructure
    {
        public int Value { get; set; }

        public TestStructure(int value)
        {
            Value = value;
        }
    }
}