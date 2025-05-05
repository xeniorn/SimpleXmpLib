using System.Collections.Frozen;

namespace SimpleXmpLib.FileEmbedding;

public record SupportedFileType
{
    internal SupportedFileType(IEnumerable<string> extensions)
    {
        CommonFilenameExtensions = extensions.Distinct().ToFrozenSet();
    }

    public IReadOnlyCollection<string> CommonFilenameExtensions { get; }

    public bool MatchesPath(string filePath)
    {
        return CommonFilenameExtensions.Any(ext => filePath.EndsWith(ext, StringComparison.OrdinalIgnoreCase));
    }
}