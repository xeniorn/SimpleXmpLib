using SimpleXmpLib.Model;
using SimpleXmpLib.XmpProperty.Base;

namespace SimpleXmpLib.XmpProperty.Typed;

public record AlternativeArrayOfStringProperty(XmpPath Path, XmpQualifier Qualifier) : AlternativeArrayOfSimpleXmpProperty<string>(Path, Qualifier, s => s);