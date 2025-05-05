using SimpleXmpLib.Model;
using SimpleXmpLib.XmpProperty.Base;

namespace SimpleXmpLib.XmpProperty.Typed;

public record ZonedDateTimeXmpProperty(XmpPath Path) : SimpleXmpProperty<DateTimeOffset>(Path, XmpHelper.ZonedTimeFromDateTimeXmpString, XmpHelper.GetZonedDateTimeXmpString)
{
}