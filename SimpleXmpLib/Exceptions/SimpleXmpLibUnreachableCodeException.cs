namespace SimpleXmpLib.Exceptions;

public class SimpleXmpLibUnreachableCodeException : Exception, ISimpleXmpLibException
{
    public const string ErrorMessageBase = @"Something occurred that should not be possible by design, indicating an internal error in the library.";

    public SimpleXmpLibUnreachableCodeException() : base(ErrorMessageBase)
    {
    }

    public SimpleXmpLibUnreachableCodeException(string message) : base(string.Join(" ", ErrorMessageBase, message))
    {
    }

    public SimpleXmpLibUnreachableCodeException(string message, Exception innerException) : base(string.Join(" ", ErrorMessageBase, message), innerException)
    {
    }
}