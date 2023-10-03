using Collections.Test.Unit.Abstract;
using FakeItEasy;
using Xunit;

namespace Collections.Test.Unit;

public class MyQueueEventTests : MyQueueTestsBase
{
    [Theory]
    [MemberData(nameof(GetTestDataForQueueFill))]
    public void LastElementRemoved_WhenLastItemRemoved_ShouldBeCalled<T>(T[] values)
    {
        var queue = new MyQueue<T>(values);
        var eventHandler = A.Fake<ITestEventsHandler>();
        queue.LastElementRemoved += eventHandler.Callback;
        queue.LastElementRemoved += eventHandler.Callback;

        foreach (var _ in values)
        {
            queue.Dequeue();
        }

        A.CallTo(() => eventHandler.Callback).MustHaveHappened(2, Times.Exactly);
    }
    
    [Theory]
    [MemberData(nameof(GetTestDataForQueueFill))]
    public void LastElementRemoved_WhenQueueCleared_ShouldBeCalled<T>(T[] values)
    {
        var queue = new MyQueue<T>(values);
        var eventHandler = A.Fake<ITestEventsHandler>();
        queue.LastElementRemoved += eventHandler.Callback;

        queue.Clear();

        A.CallTo(() => eventHandler.Callback).MustHaveHappened(1, Times.Exactly);
    }
    
    [Theory]
    [MemberData(nameof(GetTestDataForQueueFill))]
    public void LastElementLeft_WhenLastItemLeft_ShouldBeCalled<T>(T[] values)
    {
        var queue = new MyQueue<T>(values);
        var eventHandler = A.Fake<ITestEventsHandler>();
        queue.LastElementLeft += eventHandler.Callback;

        for (var i = 0; i < values.Length - 1; i++)
        {
            queue.Dequeue();
        }
            
        A.CallTo(() => eventHandler.Callback).MustHaveHappened(1, Times.Exactly);
    }

    public interface ITestEventsHandler
    {
        public Action Callback { get; set; }
    }
}