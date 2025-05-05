using SimpleXmpLib.Model;

namespace SimpleXmpLib.Exceptions;

public class XmpPropertyDoesNotExistException : Exception, ISimpleXmpLibException
{
    public XmpPropertyDoesNotExistException() : base("XMP property missing")
    {
    }

    public XmpPropertyDoesNotExistException(string message) : base(message)
    {
    }

    public XmpPropertyDoesNotExistException(XmpPath xmpPath) : base($"XMP property missing: {xmpPath}")
    {
    }
    
    public XmpPropertyDoesNotExistException(XmpPath xmpPath, string message) : base($"XMP property missing: {xmpPath}. {message}")
    {
    }
}