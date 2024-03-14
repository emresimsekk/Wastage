namespace Wastage.Core.Exceptions.Type;

public class BusinessException : ApplicationException
{
    public BusinessException(string message) : base("Business Error", message)
    {
    }
}
