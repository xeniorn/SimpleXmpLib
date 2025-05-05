using SimpleXmpLib.Exceptions;
using SimpleXmpLib.Model;

namespace SimpleXmpLib.XmpProperty.Base;

public abstract record OrderedArrayOfSimpleXmpProperty(XmpPath Path)
    : XmpProperty(Path, XmpPropertyType.Array)
{
    public IReadOnlyList<string> GetRawPropertyValues(IXmpContainer xmpContainer)
        => xmpContainer.GetPropertyValueArray(Path) ?? throw new XmpPropertyDoesNotExistException(Path);
}

public record OrderedArrayOfSimpleXmpProperty<T>(XmpPath Path, Func<string, T> Parser)
    : OrderedArrayOfSimpleXmpProperty(Path)
{
    public IReadOnlyList<T> GetPropertyValues(IXmpContainer xmpContainer) 
        => GetRawPropertyValues(xmpContainer).Select(Parser.Invoke).ToList();
}