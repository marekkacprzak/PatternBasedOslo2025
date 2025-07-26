using System.Runtime.CompilerServices;

namespace ConsoleApp1;

public static class SampleWithAwaiter
{
    public static async Task SampleForeachAwaiter()
    {
        await foreach(var item in GetItemAsync())
        {
            Console.WriteLine(item);
        }
        OurListWithoutEnumeratorButWithAsync<int> ourListWithoutEnumeratorButWithAsync
            = [5,6,7,8,9];
        
        Console.WriteLine("Test async enumerator");
        
        await foreach(var item in ourListWithoutEnumeratorButWithAsync[1..^2])
        {
            Console.WriteLine(item);
        }
    }
    static async IAsyncEnumerable<int> GetItemAsync()
    {
        for (int i = 0; i < 10; i++)
        {
            await Task.Delay(100);
            yield return i;
        }
    }

}