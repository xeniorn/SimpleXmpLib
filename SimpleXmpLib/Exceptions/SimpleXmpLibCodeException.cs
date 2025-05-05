namespace SimpleXmpLib.Exceptions;

public class SimpleXmpLibCodeException : Exception, ISimpleXmpLibException
{
    public SimpleXmpLibCodeException() : base()
    {
    }

    public SimpleXmpLibCodeException(string message) : base(message)
    {
    }

    public SimpleXmpLibCodeException(string message, Exception innerException) : base(message, innerException)
    {
    }
}