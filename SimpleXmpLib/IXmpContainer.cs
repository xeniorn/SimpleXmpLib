using SimpleXmpLib.Model;

namespace SimpleXmpLib;

public interface IXmpContainer
{
    bool PropertyExists(XmpPath path);

    /// <summary>
    /// Null if doesn't exist, false if not array, true if array
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    bool? IsArray(XmpPath path);

    /// <summary>
    /// Null if not an array / prop doesn't exist
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    int? GetArrayCount(XmpPath path);

    string? GetPropertyValue(XmpPath path);
    string[]? GetPropertyValueArray(XmpPath path);
    IReadOnlyDictionary<string, string>? GetPropertyValueAltArray(XmpPath path, XmpQualifier qualifier);

    string? GetRawXmp();
    
    public bool SetPropertyValue(XmpPath path, string value);
    public bool SetPropertyValueArray(XmpPath path, IReadOnlyList<string> value, bool ordered = false);
    public bool SetPropertyValueAltArray(XmpPath path, XmpQualifier qualifier, IReadOnlyDictionary<string, string> qualifiedValues);
    bool DeleteProperty(XmpPath path);
    
    bool EnsureLanguageAlternativeArrayExists(XmpPath path);
    bool SetLanguageAlternativeArrayValue(XmpPath path, XmpLanguage language, string value);
}