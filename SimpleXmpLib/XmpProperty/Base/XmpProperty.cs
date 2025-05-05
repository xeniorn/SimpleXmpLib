using SimpleXmpLib.Model;

namespace SimpleXmpLib.XmpProperty.Base;

public abstract record XmpProperty(XmpPath Path, XmpPropertyType PropertyType)
{
    public virtual bool DeleteProperty(IXmpContainer xmpContainer)
        => xmpContainer.DeleteProperty(Path);
}