using SimpleXmpLib.Exceptions;
using SimpleXmpLib.Model;

namespace SimpleXmpLib.XmpProperty.Base;

public abstract record UnorderedArrayOfSimpleXmpProperty(XmpPath Path)
    : XmpProperty(Path, XmpPropertyType.Array)
{
    public virtual IReadOnlyCollection<string> GetRawPropertyValues(IXmpContainer xmpContainer)
        => xmpContainer.GetPropertyValueArray(Path) ?? throw new XmpPropertyDoesNotExistException(Path);

    public virtual bool SetRawPropertyValues(IXmpContainer xmpContainer, IReadOnlyCollection<string> rawValues)
        => xmpContainer.SetPropertyValueArray(Path, rawValues.ToArray(), ordered: false);
}

public record UnorderedArrayOfSimpleXmpProperty<T>(XmpPath Path, Func<string, T> ReadParser, Func<T, string> WriteParser)
    : UnorderedArrayOfSimpleXmpProperty(Path)
{
    public virtual IReadOnlyCollection<T> GetPropertyValues(IXmpContainer xmpContainer)
        => GetRawPropertyValues(xmpContainer).Select(ReadParser.Invoke).ToList();

    public virtual bool SetPropertyValues(IXmpContainer xmpContainer, IReadOnlyCollection<T> values)
        => SetRawPropertyValues(xmpContainer, values.Select(WriteParser.Invoke).ToArray());
}