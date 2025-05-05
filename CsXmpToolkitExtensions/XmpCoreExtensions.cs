using SE.Halligang.CsXmpToolkit;

namespace CsXmpToolkitExtensions;

public static class XmpCoreExtensions
{

    public static bool PropertyExists(this XmpCore xmpObject, string schema, string propertyPath)
        => xmpObject.DoesPropertyExist(schema, propertyPath);

    /// <summary>
    /// Null if doesn't exist, false if not array, true if array
    /// </summary>
    /// <param name="xmpObject"></param>
    /// <param name="schema"></param>
    /// <param name="propertyPath"></param>
    /// <returns></returns>
    public static bool? IsArray(this XmpCore xmpObject, string schema, string propertyPath)
    {
        var namespaceSuccess = XmpCore.GetNamespacePrefix(schema, out var prefix);
        if (!namespaceSuccess) return null;

        var exists = xmpObject.DoesPropertyExist(schema, propertyPath);
        if (!exists) return null;

        var prop = xmpObject.GetProperty(schema, propertyPath, out string value, out var options);

        return options.HasFlag(PropertyFlags.ValueIsArray);
    }

    public static bool? IsSimple(this XmpCore xmpObject, string schema, string propertyPath)
    {
        var namespaceSuccess = XmpCore.GetNamespacePrefix(schema, out var prefix);
        if (!namespaceSuccess) return null;

        var exists = xmpObject.DoesPropertyExist(schema, propertyPath);
        if (!exists) return null;

        var prop = xmpObject.GetProperty(schema, propertyPath, out string value, out var options);

        return !options.HasFlag(PropertyFlags.ValueIsArray) && !options.HasFlag(PropertyFlags.ValueIsStruct);
    }

    public static int? GetArrayCount(this XmpCore xmpObject, string schema, string arrayPropertyPath)
    {
        var namespaceSuccess = XmpCore.GetNamespacePrefix(schema, out var prefix);
        if (!namespaceSuccess) return null;

        var exists = xmpObject.DoesPropertyExist(schema, arrayPropertyPath);
        if (!exists) return null;

        var initIndex = 1;
        var index = initIndex;
        while (xmpObject.DoesArrayItemExist(schema, arrayPropertyPath, index)) index++;
        return index - 1;
    }

    public static string? GetPropertyValue(this XmpCore xmpObject, string schema, string propertyPath)
    {
        var namespaceSuccess = XmpCore.GetNamespacePrefix(schema, out var prefix);
        if (!namespaceSuccess) return null;

        if (!xmpObject.IsSimple(schema, propertyPath) == true) return null;

        var success = xmpObject.GetProperty(schema, propertyPath, out string value, out var flags);
        if (!success) return null;

        return value;
    }
    
    public static string[]? GetPropertyValueArray(this XmpCore xmpObject, string schema, string arrayPropertyPath)
    {
        var namespaceSuccess = XmpCore.GetNamespacePrefix(schema, out var prefix);
        if (!namespaceSuccess) return null;

        if (!xmpObject.IsArray(schema, arrayPropertyPath) == true) return null;

        var count = xmpObject.GetArrayCount(schema, arrayPropertyPath);
        if (count is not { } itemCount) return null;

        var res = new string[itemCount];

        for (var i = 1; i <= itemCount; i++)
        {
            XmpUtils.ComposeArrayItemPath(schema, arrayPropertyPath, i, out var arrayItemPropertyPath);
            var success = xmpObject.GetProperty(schema, arrayItemPropertyPath, out string value, out var flags);
            if (!success) throw new Exception($"Failed to get item from path {arrayItemPropertyPath} from xmp object");
            res[i - 1] = value;
        }

        return res;
    }

    public static IReadOnlyDictionary<string, string>? GetPropertyValueAltArray(this XmpCore xmpObject, string schema, string arrayPropertyPath, string qualifierSchema, string qualifierName)
    {
        var namespaceSuccess = XmpCore.GetNamespacePrefix(schema, out var prefix);
        if (!namespaceSuccess) return null;

        if (!xmpObject.IsArray(schema, arrayPropertyPath) == true) return null;
        //TODO: check if alt array

        var count = xmpObject.GetArrayCount(schema, arrayPropertyPath);
        if (count is not { } itemCount) return null;

        var res = new Dictionary<string, string>();

        for (var i = 1; i <= itemCount; i++)
        {
            XmpUtils.ComposeArrayItemPath(schema, arrayPropertyPath, i, out var arrayItemPropertyPath);
            var success = xmpObject.GetProperty(schema, arrayItemPropertyPath, out string value, out var flags);
            var qualSuccess = xmpObject.GetQualifier(schema, arrayItemPropertyPath, qualifierSchema, qualifierName, out var qualValue, out var qualFlags);

            if (!success) throw new Exception($"Failed to get item from path {arrayItemPropertyPath} from xmp object");
            if (!qualSuccess || qualValue is null) throw new Exception($"Failed to get qualifier for item from path {arrayItemPropertyPath} from xmp object");
            res[qualValue] = value;
        }

        return res;
    }

    public static string? GetRawXmp(this XmpCore xmpObject)
    {
        xmpObject.SerializeToBuffer(out var rdfString, SerializeFlags.None, 0, "\n", "\t", 0);
        return rdfString;
    }

    public static bool SetPropertyValue(this XmpCore xmpObject, string schema, string propertyPath, string value)
    {
        var namespaceSuccess = XmpCore.GetNamespacePrefix(schema, out var prefix);
        if (!namespaceSuccess) return false;
        xmpObject.SetProperty(schema, propertyPath, value, PropertyFlags.None);
        return true;
    }

    public static bool CreateEmptyArray(this XmpCore xmpObject, string schema, string arrayPropertyPath, bool ordered = false)
    {
        var orderednessFlag = ordered ? PropertyFlags.ArrayIsOrdered : PropertyFlags.ArrayIsUnordered;
        var propertyFlag = PropertyFlags.ValueIsArray | orderednessFlag;
        xmpObject.SetProperty(schema, arrayPropertyPath, null, propertyFlag);
        return true;
    }

    public static bool SetPropertyValueArray(this XmpCore xmpObject, string schema, string arrayPropertyPath, string[] values, bool ordered = false)
    {
        var namespaceSuccess = XmpCore.GetNamespacePrefix(schema, out var prefix);
        if (!namespaceSuccess) return false;

        var arrayItselfExists = xmpObject.PropertyExists(schema, arrayPropertyPath);
        if (arrayItselfExists)
        {
            // remove existing
            xmpObject.DeleteProperty(schema, arrayPropertyPath);
        }

        // make sure the new array is created
        if (!xmpObject.CreateEmptyArray(schema, arrayPropertyPath, ordered))
        {
            throw new Exception($"Failed to create array at path {arrayPropertyPath} in xmp object");
            return false;
        }
        
        for (var i = 1; i <= values.Length; i++)
        {
            var value = values[i - 1];
            try
            {
                xmpObject.SetArrayItem(schema, arrayPropertyPath, i, value, PropertyFlags.None);
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to set item at path {arrayPropertyPath} index {i} from xmp object", ex);
            }
        }

        return true;
    }

    public static bool SetLanguageAlternativeArrayValue(this XmpCore xmpObject, string schema, string arrayPropertyPath, string genericLanguageGroup, string languageName, string valueForDefaultLanguage)
    {
        if (!xmpObject.PropertyExists(schema, arrayPropertyPath))
        {
            var ensuredExistence = xmpObject.EnsureLanguageAlternativeArrayExists(schema, arrayPropertyPath);
            if (!ensuredExistence) return false;
        }

        xmpObject.SetLocalizedText(schema, arrayPropertyPath, genericLanguageGroup, languageName, valueForDefaultLanguage, 0);
        return true;
    }

    public static bool EnsureLanguageAlternativeArrayExists(this XmpCore xmpObject, string schema, string arrayPropertyPath)
    {
        var exists = xmpObject.PropertyExists(schema, arrayPropertyPath);
        if (exists) return true;
        
        var propertyFlag = PropertyFlags.ValueIsArray | PropertyFlags.ArrayIsAlternate | PropertyFlags.ArrayIsAltText;
        xmpObject.SetProperty(schema, arrayPropertyPath, null, propertyFlag);
        return true;
    }
    


    public static bool SetPropertyValueAltArray(this XmpCore xmpObject, string schema, string arrayPropertyPath, string qualifierNamespace, string qualifierName, IReadOnlyDictionary<string, string> qualifiedValues)
    {
        throw new NotImplementedException();

        var namespaceSuccess = XmpCore.GetNamespacePrefix(schema, out var prefix);
        if (!namespaceSuccess) return false;

        var arrayItselfExists = xmpObject.GetPropertyValue(schema, arrayPropertyPath) is not null;


        var propertyFlag = PropertyFlags.ValueIsArray | PropertyFlags.ArrayIsAlternate;
        if (!arrayItselfExists) 
        {
            xmpObject.SetProperty(schema, arrayPropertyPath, null, propertyFlag);
        }

        var existingItems = xmpObject.GetPropertyValueAltArray(schema, arrayPropertyPath, qualifierNamespace, qualifierName);

        var itemCount = xmpObject.GetArrayCount(schema, arrayPropertyPath) ?? 0;
        
        if (existingItems is not null && existingItems.Count > 0)
        {
            var extraQualifiers = existingItems.Keys.Except(qualifiedValues.Keys).ToList();

            foreach (var extraQualifier in extraQualifiers)
            {
                try
                {
                    //var path = X

                    //xmpObject.Prope

                    xmpObject.DeleteQualifier(schema, arrayPropertyPath, qualifierNamespace, extraQualifier);
                }
                catch (Exception ex)
                {
                    throw new Exception($"Failed to delete item from path {arrayPropertyPath} index {extraQualifier} from xmp object", ex);
                }
            }
        }

        foreach (var (qualifierValue, propertyValue) in qualifiedValues)
        {
            try
            {
                xmpObject.AppendArrayItem(schema, arrayPropertyPath, propertyFlag, propertyValue, PropertyFlags.None);
                //xmpObject.SetQualifier(schema, arrayPropertyPath, qualifierNamespace, qualifierValue, PropertyFlags.None);
                throw new NotImplementedException();

                //xmpObject.SetArrayItem(schema, arrayPropertyPath, i, value, PropertyFlags.None);
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to set item at path {arrayPropertyPath} index {qualifierValue} from xmp object", ex);
            }
        }

        return true;
    }

    
}