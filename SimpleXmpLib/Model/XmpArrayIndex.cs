namespace SimpleXmpLib.Model;

/// <summary>
/// Index is 1-based
/// </summary>
/// <param name="Value"></param>
public readonly record struct XmpArrayIndex
{
    public int Value { get; } = 1;

    public XmpArrayIndex(int value)
    {
        if (value < 1) 
            throw new ArgumentOutOfRangeException(nameof(value), "Index must be 1 or greater");
        Value = value;
    }

    public static XmpArrayIndex First = new(1);
}