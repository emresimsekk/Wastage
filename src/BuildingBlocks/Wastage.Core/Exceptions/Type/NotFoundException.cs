namespace Wastage.Core.Exceptions.Type;

public class NotFoundException : ApplicationException
{
    public NotFoundException(string message) : base("Not Found Error", message)
    {
    }
}
