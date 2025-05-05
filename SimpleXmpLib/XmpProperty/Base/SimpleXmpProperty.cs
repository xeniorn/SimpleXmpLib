using SimpleXmpLib.Exceptions;
using SimpleXmpLib.Model;

namespace SimpleXmpLib.XmpProperty.Base;

public abstract record SimpleXmpProperty(XmpPath Path) : XmpProperty(Path, XmpPropertyType.Simple)
{
    public virtual string GetRawPropertyValue(IXmpContainer xmpContainer)
        => xmpContainer.GetPropertyValue(Path) ?? throw new XmpPropertyDoesNotExistException(Path);

    public virtual bool SetRawPropertyValue(IXmpContainer xmpContainer, string rawValue)
        => xmpContainer.SetPropertyValue(Path, rawValue);
}

public record SimpleXmpProperty<T>(XmpPath Path, Func<string, T> ReadParser, Func<T, string> WriteParser) : SimpleXmpProperty(Path)
{
    public virtual T GetPropertyValue(IXmpContainer xmpContainer) => ReadParser.Invoke(GetRawPropertyValue(xmpContainer));
    
    public virtual bool SetPropertyValue(IXmpContainer xmpContainer, T value)
        => SetRawPropertyValue(xmpContainer, WriteParser.Invoke(value));
}