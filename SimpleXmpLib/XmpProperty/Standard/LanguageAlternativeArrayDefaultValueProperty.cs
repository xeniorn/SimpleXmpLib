using SimpleXmpLib.Model;
using SimpleXmpLib.XmpProperty.Base;

namespace SimpleXmpLib.XmpProperty.Standard;

public record LanguageAlternativeArrayDefaultValueProperty
    : SimpleXmpProperty<string>
{
    private LanguageAlternativeArrayProperty ParentProperty { get; }

    public LanguageAlternativeArrayDefaultValueProperty(XmpPath ParentAltArrayPropertyPath) : base(
        XmpPath.Combine(ParentAltArrayPropertyPath, XmpArrayIndex.First), x => x, x => x)
    {
        ParentProperty = new LanguageAlternativeArrayProperty(ParentAltArrayPropertyPath);
    }
    
    public override bool SetRawPropertyValue(IXmpContainer xmpContainer, string rawValue)
    {
        return xmpContainer.SetLanguageAlternativeArrayValue(ParentProperty.Path, Constants.Constants.XmlLangQualifierDefault, rawValue);

        //var exists = ParentProperty.EnsureArrayExists(xmpContainer);
        
        //return exists
        //    ? base.SetRawPropertyValue(xmpContainer, rawValue)
        //    : false;
    }
}