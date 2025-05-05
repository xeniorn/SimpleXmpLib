using System.Collections.Frozen;
using SE.Halligang.CsXmpToolkit;

namespace SimpleXmpLib.FileEmbedding;

public static class SupportedFileTypes
{
    public static SupportedFileType Tiff { get; }  = new(["tiff", "tif"]);
    public static SupportedFileType Jpg { get; } = new(["jpeg", "jpg"]);
    public static SupportedFileType Png { get; } = new(["png"]);

    public static SupportedFileType Mp4 { get; } = new(["mp4"]);
    public static SupportedFileType Mov { get; } = new(["mov"]);
    public static SupportedFileType Avi { get; } = new(["avi"]);
    public static SupportedFileType Wmv { get; } = new(["wmv"]);

    public static SupportedFileType Wav { get; } = new(["wav"]);
    public static SupportedFileType Mp3 { get; } = new(["mp3"]);
    public static SupportedFileType Wma { get; } = new(["wma"]);
    public static SupportedFileType M4a { get; } = new(["m4a"]);

    public static SupportedFileType Pdf { get; } = new(["pdf"]);
    
    internal static IReadOnlyDictionary<SupportedFileType, FileFormat> FileTypeToXmpToolkitFileFormatMap { get; } =
        new Dictionary<SupportedFileType, FileFormat>()
        {
            [Tiff] = FileFormat.Tiff,
            [Jpg] = FileFormat.Jpeg,
            [Png] = FileFormat.Png,

            [Mp4] = FileFormat.Mpeg4,
            [Mov] = FileFormat.Mov,
            [Avi] = FileFormat.Avi,
            [Wmv] = FileFormat.Wmav,

            [Wav] = FileFormat.Wav,
            [Mp3] = FileFormat.Mp3,
            [Wma] = FileFormat.Wmav,
            [M4a] = FileFormat.Mpeg4,

            [Pdf] = FileFormat.Pdf,
        }.ToFrozenDictionary();

    public static IReadOnlyCollection<SupportedFileType> All { get; } = FileTypeToXmpToolkitFileFormatMap.Keys.ToHashSet();

    internal static FileFormat GetInnerFileFormat(SupportedFileType supportedFileType) => FileTypeToXmpToolkitFileFormatMap[supportedFileType];

}