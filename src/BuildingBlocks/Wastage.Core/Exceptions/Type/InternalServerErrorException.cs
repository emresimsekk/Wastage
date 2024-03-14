namespace Wastage.Core.Exceptions.Type;

public class InternalServerErrorException : ApplicationException
{
    public InternalServerErrorException(string message) : base("Internal Server Error", message)
    {
    }
}
