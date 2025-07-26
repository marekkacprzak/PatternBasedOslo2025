using System.Runtime.CompilerServices;

namespace ConsoleApp1;

public static class SampleWithAwaitTimeSpan
{
    public static async ValueTask SampleAwaitTimeSpan()
    {
        Console.WriteLine($"Before: {DateTimeOffset.Now}");
        var delay = await TimeSpan.FromSeconds(2);
        Console.WriteLine($"After: {DateTimeOffset.Now}");
        Console.WriteLine(delay);
    }
}

    
public static class Extension
{
    // public static TaskAwaiter GetAwaiter(this TimeSpan timeSpan)
    // {
    //     return Task.Delay(timeSpan).GetAwaiter();
    // }
    public static OurAwaiter GetAwaiter(this TimeSpan timeSpan)
    {
        return new OurAwaiter(timeSpan);
    }
}

public sealed class OurAwaiter:ICriticalNotifyCompletion
{
    private readonly TaskAwaiter _awaiter;
    private readonly TimeSpan _value;
    public bool IsCompleted => _awaiter.IsCompleted;
    public OurAwaiter(TimeSpan timeSpan)
    {
        _value = timeSpan;
        _awaiter = Task.Delay(timeSpan).GetAwaiter();
    }

    public void OnCompleted(Action continuation)
    {
        _awaiter.OnCompleted(continuation);
    }

    public void UnsafeOnCompleted(Action continuation)
    {
        _awaiter.UnsafeOnCompleted(continuation);
    }

    public TimeSpan GetResult()
    {
        _awaiter.GetResult();
        return _value;
    }
}