namespace ConsoleApp1;

public class Program
{
    static async Task Main(string[] args)
    {
        await SampleWithAwaitTimeSpan.SampleAwaitTimeSpan();
        
        await SampleWithAwaiter.SampleForeachAwaiter();
        
        SampleEnumeratorAsStruct.SampleListWithoutAllocationEnumerator();
        
        Talk talk = new("C# Patterns", "NDC Oslo 2025",
            new DateTimeOffset(2025, 05, 22, 0, 0, 0, TimeSpan.FromHours(0)));
        TalkRecord talkRecord = new("C# Patterns", "NDC Oslo 2025",
            new DateTimeOffset(2025, 05, 22, 0, 0, 0, TimeSpan.FromHours(0)));

        (string title, string conference, DateTimeOffset start) = talk;

        Talk talk2 = new(true, "C# Patterns" );
    
        (bool active, string titleNew) = talk2;

        var timespan = TimeSpan.FromHours(1);
        if (talk is Talk
            {
                Title: "C# Patterns",
                Conference: "NDC Oslo 2025",
                Start: { Year: 2025, Month: 5, Day: 22, Hour: 0, Minute: 0, Offset: { Hours: 0 } }
            } instance)
        {
            Console.WriteLine(
                $"Welcome to {instance.Title} at {instance.Conference} starting at {instance.Start:HH:mm}!");
        }
    }
}