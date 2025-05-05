using SE.Halligang.CsXmpToolkit;
using SimpleXmpLib.Model;

namespace SimpleXmpLib;

/// <summary>
/// Implementation of Xmp path string generation using xmpToolkit
/// </summary>
internal static class XmpPathExtensions
{
    public static string PathString(this XmpPath path)
    {
        if (path.PathComponents.Count == 0) throw new ArgumentException();
        
        if (path.PathComponents.First().PathComponentType != XmpPath.PathComponentType.PropertyNameOrAssembledPath) throw new ArgumentException();

        var rootPath = path.PathComponents.First().AsPropertyNameComponent().Name;
        var currentPath = rootPath;

        foreach (var pathComponent in path.PathComponents.Skip(1))
        {
            switch (pathComponent.PathComponentType)
            {
                case XmpPath.PathComponentType.PropertyNameOrAssembledPath:
                    var fieldDef = pathComponent.AsStructFieldComponent().FieldNameAndNamespace;
                    XmpUtils.ComposeStructFieldPath(path.NamespaceUri, currentPath, fieldDef.NamespaceUri, fieldDef.Name, out var pathWithStructThing);
                    currentPath = pathWithStructThing;
                    continue;

                case XmpPath.PathComponentType.ArrayIndex:
                    var index = pathComponent.AsArrayIndexComponent().Index.Value;
                    XmpUtils.ComposeArrayItemPath(path.NamespaceUri, currentPath, index, out var pathWithIndex);
                    currentPath = pathWithIndex;
                    continue;

                case XmpPath.PathComponentType.QualifierWithValue:
                    var (qualifier, value) = pathComponent.AsQualifierWithValueComponent().QualifierWithValue;
                    XmpUtils.ComposeFieldSelector(path.NamespaceUri, currentPath, qualifier.Namespace, qualifier.QualifierName, value, out var pathWithQualifier);
                    currentPath = pathWithQualifier;
                    continue;

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        return currentPath;
    }
}