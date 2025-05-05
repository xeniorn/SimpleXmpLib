using SimpleXmpLib.Exceptions;
using SimpleXmpLib.Model;

namespace SimpleXmpLib.XmpProperty.Base;

public abstract record AlternativeArrayOfSimpleXmpProperty(XmpPath Path, XmpQualifier DiscriminatingQualifier)
    : XmpProperty(Path, XmpPropertyType.Array)
{
    public IReadOnlyDictionary<string, string> GetRawPropertyValuesWithQualifier(IXmpContainer xmpContainer)
        => xmpContainer.GetPropertyValueAltArray(Path, DiscriminatingQualifier) ?? throw new XmpPropertyDoesNotExistException(Path);

    public bool EnsureArrayExists(IXmpContainer xmpContainer)
    {
        if (xmpContainer.PropertyExists(Path))
        {
            if (xmpContainer.IsArray(Path) != true)
            {
                throw new Exception("Expected an array, found something else");
            }
            return true;
        }
            
        return xmpContainer.SetPropertyValueAltArray(Path, DiscriminatingQualifier, new Dictionary<string, string>());
    }

    public IReadOnlyCollection<string> GetRawPropertyValues(IXmpContainer xmpContainer) => GetRawPropertyValuesWithQualifier(xmpContainer).Values.ToList();
}

public record AlternativeArrayOfSimpleXmpProperty<T>(XmpPath Path, XmpQualifier DiscriminatingQualifier, Func<string, T> Parser)
    : AlternativeArrayOfSimpleXmpProperty(Path, DiscriminatingQualifier)
{
    public virtual IReadOnlyDictionary<string, T> GetPropertyValuesWithQualifier(IXmpContainer xmpContainer)
        => GetRawPropertyValuesWithQualifier(xmpContainer)
            .ToDictionary
            (
                x => x.Key,
                x => Parser.Invoke(x.Value)
            );

    public IReadOnlyCollection<T> GetPropertyValues(IXmpContainer xmpContainer) => GetPropertyValuesWithQualifier(xmpContainer).Values.ToList();
    
    public virtual bool SetPropertyValuesWithQualifier(IXmpContainer xmpContainer, T value)
        => throw new NotImplementedException($"SetPropertyValuesWithQualifier is not implemented for {GetType().Name}.");
}