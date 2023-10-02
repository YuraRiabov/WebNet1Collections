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
    public void Enqueue_ShouldAddElements<T>(T[] values)
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

    private static void AssertEqualCollections<T>(IEnumerable<T> values, IEnumerable<T> queue)
    {
        using var enumerator = queue.GetEnumerator();
        foreach (T value in values)
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