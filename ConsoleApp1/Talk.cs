using System.Diagnostics.CodeAnalysis;
using GeneratorContract;


namespace ConsoleApp1;

[GeneratedDeconstruct]
public sealed partial class Talk
{
    [SetsRequiredMembers]
    public Talk(string title, string conference, DateTimeOffset start)
    {
        Title = title;
        Conference = conference;
        Start = start;
    }
    
    // [SetsRequiredMembers]
    // public Talk(string title123, string conference123)
    // {
    //     Title = title123;
    //     Conference = conference123;
    // }
    [SetsRequiredMembers]
    public Talk(bool active, string title)
    {
        Active123 = active;
        Title = title;
    }

    public bool Active123;

    public required DateTimeOffset Start { get; init; }

    public required string Conference { get; init; } = "Init";

    public required string Title { get; init; }
}