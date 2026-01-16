namespace FitJournal.Core.Results;

public record Error(string Code, string Message)
{
    public static Error Create(Type type, string name, string message) =>
        new($"{string.Join(".", GetNames(type))}.{name}", message);

    private static Stack<string> GetNames(Type type)
    {
        var stack = new Stack<string>();
        for (var t = type; t != null; t = t.DeclaringType)
            stack.Push(t.Name);
        return stack;
    }
}
