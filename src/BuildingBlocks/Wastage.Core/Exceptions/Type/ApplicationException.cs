namespace Wastage.Core.Exceptions.Type;

public abstract class ApplicationException : Exception
{
    public string Title { get; }

    public ApplicationException(string title, string message) : base(message)
    {
        Title = title;
    }

    protected ApplicationException(string title, string message, Exception? innerException) : base(message, innerException)
    {
        Title = title;
    }
}
