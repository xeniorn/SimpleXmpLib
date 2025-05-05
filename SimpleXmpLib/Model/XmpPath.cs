namespace SimpleXmpLib.Model;

public record XmpPath
{
    public string NamespaceUri { get; }

    protected XmpPath(string namespaceUri)
    {
        NamespaceUri = namespaceUri;
    }

    public XmpPath(string namespaceUri, string pathString) : this(namespaceUri)
    {
        PathComponents = [new PropertyNameComponent(pathString)];
    }

    public XmpPath(string namespaceUri, string basePathString, XmpArrayIndex index) : this(namespaceUri)
    {
        PathComponents = [new PropertyNameComponent(basePathString), new ArrayIndexComponent(index)];
    }

    public XmpPath(string namespaceUri, string basePathString, XmpQualifierWithValue altArrayIndex) : this(namespaceUri)
    {
        PathComponents = [new PropertyNameComponent(basePathString), new QualifierWithValueComponent(altArrayIndex)];
    }

    internal XmpPath(string namespaceUri, IReadOnlyList<PathComponent> pathComponents) : this(namespaceUri)
    {
        PathComponents = pathComponents;
    }

    internal enum PathComponentType
    {
        PropertyNameOrAssembledPath,
        StructField,
        ArrayIndex,
        QualifierWithValue
    }

    internal IReadOnlyList<PathComponent> PathComponents { get; }

    internal record PropertyNameComponent(string Name) : PathComponent(Name, PathComponentType.PropertyNameOrAssembledPath);
    internal record ArrayIndexComponent(XmpArrayIndex Index) : PathComponent(Index, PathComponentType.ArrayIndex);
    internal record QualifierWithValueComponent(XmpQualifierWithValue QualifierWithValue) : PathComponent(QualifierWithValue, PathComponentType.QualifierWithValue);
    internal record StructFieldComponent(XmpNameAndNamespace FieldNameAndNamespace) : PathComponent(FieldNameAndNamespace, PathComponentType.StructField);

    internal record PathComponent(object Item, PathComponentType PathComponentType)
    {
        internal PropertyNameComponent AsPropertyNameComponent() => (PropertyNameComponent)this;
        internal ArrayIndexComponent AsArrayIndexComponent() => (ArrayIndexComponent)this;
        internal QualifierWithValueComponent AsQualifierWithValueComponent() => (QualifierWithValueComponent)this;
        internal StructFieldComponent AsStructFieldComponent() => (StructFieldComponent)this;
    };
    

    public static XmpPath Combine(XmpPath path, XmpArrayIndex index)
    {
        return new XmpPath(path.NamespaceUri, [..path.PathComponents, new ArrayIndexComponent(index)]);
    }
}
