using SimpleXmpLib.Constants;
using SimpleXmpLib.Model;
using SimpleXmpLib.XmpProperty.Typed;

namespace SimpleXmpLib.XmpProperty.Standard;



public record StandardAltArrayProperty(XmpPath Path)
    : AlternativeArrayOfStringProperty(Path, CommonXmpQualifiers.XmlLang)
{
    public string? GetFromDefaultLanguage(IXmpContainer xmpContainer)
    {
        var itemsMap = GetPropertyValuesWithQualifier(xmpContainer);
        var item = itemsMap.FirstOrDefault().Value;
        return item;
    }
}