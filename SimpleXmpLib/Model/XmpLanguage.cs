namespace SimpleXmpLib.Model;

public record XmpLanguage(string Language, string GenericLanguageGroup)
{
    public static readonly XmpLanguage DefaultLanguage = new(CsXmpToolkitExtensions.Constants.XmlLangQualifierDefault,  CsXmpToolkitExtensions.Constants.XmlLangEmptyGenericLanguage);
};