using Collections;

void OutputCollection(IEnumerable<int> collection)
{
    foreach (var item in collection)
    {
        Console.Write($"{item} ");
    }

    Console.WriteLine();
}

void OnLastItemLeft() => Console.WriteLine("Hello world from last element left");
void OnEmptyQueue() => Console.WriteLine("Goodbye world from empty queue");

var queue = new MyQueue<int>();

queue.Enqueue(1);
queue.Enqueue(2);
queue.Enqueue(3);
queue.Enqueue(4);

Console.WriteLine($"Contains check: {queue.Contains(3)}");


Console.WriteLine("Iterator check, queue:");
OutputCollection(queue);


Console.WriteLine($"Peek check, next item: {queue.Peek()}");
var dequeued = queue.Dequeue();
Console.WriteLine($"Dequeue check, next item: {dequeued}");
Console.WriteLine($"Contains after dequeue: {queue.Contains(dequeued)}");
Console.WriteLine($"Count check, after dequeue: {queue.Count}");

var array = new int[3];

queue.CopyTo(array, 0);

Console.WriteLine("Copy to check, array:");
OutputCollection(array);

var longArray = new int[6];
queue.CopyTo(longArray, 2);
Console.WriteLine("Copy to long check, array:");
OutputCollection(longArray);

var shortArray = new int[2];
try
{
    queue.CopyTo(shortArray, 0);
}
catch(ArgumentOutOfRangeException)
{
    Console.WriteLine("Hello world from catch after invalid array copy attempt");
}

var copiedQueue = new MyQueue<int>(longArray);
Console.WriteLine($"Constructor with source check, copied queue:");
OutputCollection(copiedQueue);

queue.LastElementLeft += OnLastItemLeft;
queue.LastElementRemoved += OnEmptyQueue;
for (int i = 0; i < 3; i++)
{
    queue.TryDequeue(out dequeued);
    Console.WriteLine($"Events check, dequeued with try dequeue: {dequeued}");
    Console.WriteLine($"Try peek check, try peek result after dequeue: {queue.TryPeek(out _)}");
}

foreach (var item in longArray)
{
    queue.Enqueue(item);
}
Console.WriteLine("Refill empty queue check, queue after refill from long array:");
OutputCollection(queue);

var queueAsArray = queue.ToArray();
Console.WriteLine("To array check, copy as array:");
OutputCollection(queueAsArray);

Console.WriteLine("Clear check");
queue.Clear();
Console.WriteLine($"Queue count after clear: {queue.Count}");