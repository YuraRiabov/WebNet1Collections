using System.Collections;
using Collections.Test.Unit.Abstract;
using Xunit;

namespace Collections.Test.Unit;

public class MyQueueCopyToTests : MyQueueTestsBase
{
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
    [MemberData(nameof(GetCopyToGenericTestData))]
    public void CopyTo_WhenInvalidParams_ShouldThrow<T>(T[] values, T[] array, int index, Type exceptionType)
    {
        var queue = new MyQueue<T>(values);
        
        var code = () => queue.CopyTo(array, index);

        Assert.Throws(exceptionType, code);
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
    [MemberData(nameof(GetCopyToNotGenericTestData))]
    public void CopyToNotGeneric_WhenInvalidData_ShouldThrow<T>(T[] values, Array array, int index, Type exceptionType)
    {
        ICollection queue = new MyQueue<T>(values);
        
        var code = () => queue.CopyTo(array, index);

        Assert.Throws(exceptionType, code);
    }
    
    public static IEnumerable<object[]> GetCopyToGenericTestData() => GetCopyToTestData(true);

    public static IEnumerable<object[]> GetCopyToNotGenericTestData() => GetCopyToTestData(false);
    
    private static IEnumerable<object[]> GetCopyToTestData(bool isGeneric)
    {
        var types = new[] { typeof(int), typeof(TestStructure), typeof(TestClass) };
        var currentTypeIndex = 0;
        foreach (var values in GetTestDataForQueueFill())
        {
            var array = values[0] as Array;
            var type = types[currentTypeIndex];
            yield return new object[] { array, null, 0, typeof(ArgumentOutOfRangeException) };
            yield return new object[] { array, Array.CreateInstance(type, array.Length - 1), 0, typeof(ArgumentOutOfRangeException) };
            yield return new object[] { array, Array.CreateInstance(type, array.Length ), 1, typeof(ArgumentOutOfRangeException) };
            yield return new object[] { array, Array.CreateInstance(type, array.Length ), -1, typeof(ArgumentOutOfRangeException) };
            yield return new object[] { array, Array.CreateInstance(type, array.Length ), array.Length + 1, typeof(ArgumentOutOfRangeException) };
            if (!isGeneric)
            {
                yield return new object[] { array, Array.CreateInstance(type, new []{ array.Length, array.Length }, new []{ 0, 0 } ), 0, typeof(ArgumentException) };
                yield return new object[] { array, Array.CreateInstance(type, new []{ array.Length }, new []{ 1 } ), 0, typeof(ArgumentException) };
            }
            currentTypeIndex++;
        }
    }
}