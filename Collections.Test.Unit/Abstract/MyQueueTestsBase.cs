using Xunit;

namespace Collections.Test.Unit.Abstract;

public abstract class MyQueueTestsBase
{
    private static readonly int[] NumberArray = { 1, 2, 3, 4 };
    private static readonly TestStructure[] StructureArray = { new(1), new(2), new(3), new(4) };
    private static readonly TestClass[] ObjectArray = { new(1), new(2), null, new(3), new(4) };

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
    
    protected static void AssertEqualCollections<T>(IEnumerable<T> expected, IEnumerable<T> actual)
    {
        using var enumerator = actual.GetEnumerator();
        foreach (T value in expected)
        {
            enumerator.MoveNext();
            var currentValue = enumerator.Current;

            Assert.Equal(value, currentValue);
        }
    }
    
    protected class TestClass
    {
        public int Value { get; set; }

        public TestClass(int value)
        {
            Value = value;
        }
    }
    
    protected struct TestStructure
    {
        public int Value { get; set; }

        public TestStructure(int value)
        {
            Value = value;
        }
    }
}