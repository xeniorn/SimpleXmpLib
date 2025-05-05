namespace SimpleXmpLib.Constants;

public static class CommonXmpNamespaces
{
    public record CommonXmpNamespaceDefinition(string NamespaceUri, string PreferredPrefix);

    public static CommonXmpNamespaceDefinition Rdf = new(@"http://www.w3.org/1999/02/22-rdf-syntax-ns#", "rdf");

    public static CommonXmpNamespaceDefinition Xml = new(@"http://www.w3.org/XML/1998/namespace", "xml");

    public static CommonXmpNamespaceDefinition DublinCore = new(@"http://purl.org/dc/elements/1.1/", @"dc");
    public static CommonXmpNamespaceDefinition Xmp = new(@"http://ns.adobe.com/xap/1.0/", @"xmp");
    public static CommonXmpNamespaceDefinition Photoshop = new(@"http://ns.adobe.com/photoshop/1.0/", @"photoshop");
    public static CommonXmpNamespaceDefinition IptcXmpExtension = new(@"http://iptc.org/std/Iptc4xmpExt/2008-02-29/", @"Iptc4xmpExt");

    public static IReadOnlyCollection<CommonXmpNamespaceDefinition> All
        => typeof(CommonXmpNamespaces)
            .GetFields()
            .Where(x => x.IsStatic && x.FieldType == typeof(CommonXmpNamespaceDefinition))
            .Select(x => x.GetValue(null) as CommonXmpNamespaceDefinition)
            .Where(x => x is not null)
            .Cast<CommonXmpNamespaceDefinition>()
            .ToHashSet();


    public static string? TryGetPrefix(string nameSpace)
        => All.FirstOrDefault(x => x.NamespaceUri == nameSpace)?.PreferredPrefix;
    
}