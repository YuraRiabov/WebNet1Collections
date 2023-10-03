using Collections.Test.Unit.Abstract;
using Xunit;

namespace Collections.Test.Unit;

public class MyQueueCoreTests : MyQueueTestsBase
{
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
}