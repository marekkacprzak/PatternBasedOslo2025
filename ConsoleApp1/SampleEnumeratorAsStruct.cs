namespace ConsoleApp1;

public static class SampleEnumeratorAsStruct
{
    public static void SampleListWithoutAllocationEnumerator()
    {
        List<int> list = new() { 0, 1, 2, 3, 4 };
        OurList<int> our = new()
        {
            { 5 },
            { 6, 7 },
            { 8 },
            { 9 }
        };

        List<int> list1 = [0, 1, 2, 3, 4];
        OurListWithSpan<int> our1 = [5, 6, 7, 8, 9];

        Console.WriteLine($"First list: {list1[0]}");
        Console.WriteLine($" Last list: {our1[^1]}");

        foreach (var item in our1)
        {
            Console.WriteLine(item);
        }

        foreach (var item in our1[1..^2])
        {
            Console.WriteLine(item);
        }

        OurListWithoutEnumerator<int> ourListWithoutEnumerator
            = [5, 6, 7, 8, 9];

        foreach (ref var item in ourListWithoutEnumerator[1..^2])
        {
            Console.WriteLine(item);
        }
    }
}


