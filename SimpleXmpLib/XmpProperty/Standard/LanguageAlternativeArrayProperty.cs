using SimpleXmpLib.Model;

namespace SimpleXmpLib.XmpProperty.Standard;

public record LanguageAlternativeArrayProperty(XmpPath Path) : Base.XmpProperty(Path, XmpPropertyType.Array)
{
    public bool EnsureArrayExists(IXmpContainer xmpContainer)
    {
        return xmpContainer.EnsureLanguageAlternativeArrayExists(Path);
    }
    
    //public string? GetFromDefaultLanguage(IXmpContainer xmpContainer)
    //{
    //    var itemsMap = GetPropertyValuesWithQualifier(xmpContainer);
    //    var item = itemsMap.FirstOrDefault().Value;
    //    return item;
    //}
    
}