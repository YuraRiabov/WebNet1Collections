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

        using var enumerator = queue.GetEnumerator();
        foreach (T value in values)
        {
            enumerator.MoveNext();
            var currentValue = enumerator.Current;
            
            Assert.Equal(value, currentValue);
        }
    }
    
    [Theory]
    [MemberData(nameof(GetTestDataForQueueFill))]
    public void EnumerableConstructor_ShouldAddElements<T>(T[] values)
    {
        var queue = new MyQueue<T>(values);

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