using SimpleXmpLib.Model;
using SimpleXmpLib.XmpProperty.Base;

namespace SimpleXmpLib.XmpProperty.Typed;

public record AbsoluteDateTimeXmpProperty(XmpPath Path) : SimpleXmpProperty<DateTime>(Path, XmpHelper.AbsoluteTimeFromDateTimeXmpString, XmpHelper.GetUtcDateTimeXmpString)
{
}