using CsXmpToolkitExtensions;
using SE.Halligang.CsXmpToolkit;
using SimpleXmpLib.Constants;
using SimpleXmpLib.Model;

namespace SimpleXmpLib;

/// <summary>
/// Implementation of <see cref="IXmpContainer"/> that uses the CsXmpToolkit library.
/// </summary>
/// <param name="InnerContainer"></param>
internal record XmpToolkitBasedXmpContainer : IXmpContainer, IDisposable
{
    internal XmpCore InnerContainer { get; }

    internal XmpToolkitBasedXmpContainer(XmpCore innerContainer)
    {
        InnerContainer = innerContainer;
    }

    public void Dispose()
    {
        InnerContainer.Dispose();
    }

    public bool PropertyExists(XmpPath path) => InnerContainer.PropertyExists(path.NamespaceUri, path.PathString());

    public bool? IsArray(XmpPath path) => InnerContainer.IsArray(path.NamespaceUri, path.PathString());
    public int? GetArrayCount(XmpPath path) => InnerContainer.GetArrayCount(path.NamespaceUri, path.PathString());

    public string? GetPropertyValue(XmpPath path) => InnerContainer.GetPropertyValue(path.NamespaceUri, path.PathString());
    public string[]? GetPropertyValueArray(XmpPath path) => InnerContainer.GetPropertyValueArray(path.NamespaceUri, path.PathString());

    public IReadOnlyDictionary<string, string>? GetPropertyValueAltArray(XmpPath path, XmpQualifier qualifier)
        => InnerContainer.GetPropertyValueAltArray(path.NamespaceUri, path.PathString(), qualifier.Namespace, qualifier.QualifierName);

    public string? GetRawXmp() => InnerContainer.GetRawXmp();
    
    public bool SetPropertyValue(XmpPath path, string value)
    {
        EnsureNameSpaceRegistered(path.NamespaceUri);
        return InnerContainer.SetPropertyValue(path.NamespaceUri, path.PathString(), value);
    }

    public bool SetPropertyValueArray(XmpPath path, IReadOnlyList<string> value, bool ordered = false)
    {
        EnsureNameSpaceRegistered(path.NamespaceUri);
        return InnerContainer.SetPropertyValueArray(path.NamespaceUri, path.PathString(), value.ToArray(), ordered);
    }
    
    public bool SetPropertyValueAltArray(XmpPath path, XmpQualifier qualifier,
        IReadOnlyDictionary<string, string> qualifiedValues)
    {
        throw new NotImplementedException();
        EnsureNameSpaceRegistered(path.NamespaceUri);
        return InnerContainer.SetPropertyValueAltArray(path.NamespaceUri, path.PathString(), qualifier.Namespace, qualifier.QualifierName, qualifiedValues);
    }
        

    public bool DeleteProperty(XmpPath path)
    {
        try
        {
            InnerContainer.DeleteProperty(path.NamespaceUri, path.PathString());
            return true;
        }
        catch (Exception ex)
        {
            return false;
        }
    }

    public bool EnsureLanguageAlternativeArrayExists(XmpPath path)
    {
        EnsureNameSpaceRegistered(path.NamespaceUri);
        return InnerContainer.EnsureLanguageAlternativeArrayExists(path.NamespaceUri, path.PathString());
    }

    public bool SetLanguageAlternativeArrayValue(XmpPath arrayPath, XmpLanguage language, string value)
        => InnerContainer.SetLanguageAlternativeArrayValue(arrayPath.NamespaceUri, arrayPath.PathString(), language.GenericLanguageGroup, language.Language, value);

    public bool SetLanguageAlternativeArrayDefaultValue(XmpPath arrayPath, string valueForDefaultLanguage)
        => SetLanguageAlternativeArrayValue(arrayPath, XmpLanguage.DefaultLanguage, valueForDefaultLanguage);

    private string EnsureNameSpaceRegistered(string nameSpace)
    {
        XmpCore.GetNamespacePrefix(nameSpace, out var existingPrefix);
        if (existingPrefix != null) return existingPrefix;

        XmpCore.RegisterNamespace(nameSpace, (CommonXmpNamespaces.TryGetPrefix(nameSpace) ?? "CustomNamespace") + ":", out var newPrefix);
        if (newPrefix is null) throw new Exception("Failed to register namespace");
        return newPrefix;
    }
    

    public static XmpToolkitBasedXmpContainer CreateNew()
    {
        return new XmpToolkitBasedXmpContainer(new XmpCore());
    }
}