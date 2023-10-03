using Collections.Test.Unit.Abstract;
using Xunit;

namespace Collections.Test.Unit;

public class MyQueueCollectionTests : MyQueueTestsBase
{
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
}