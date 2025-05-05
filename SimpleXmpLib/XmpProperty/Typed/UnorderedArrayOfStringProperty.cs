using SimpleXmpLib.Model;
using SimpleXmpLib.XmpProperty.Base;

namespace SimpleXmpLib.XmpProperty.Typed;

public record UnorderedArrayOfStringProperty(XmpPath Path) : UnorderedArrayOfSimpleXmpProperty<string>(Path, s => s, s=>s);