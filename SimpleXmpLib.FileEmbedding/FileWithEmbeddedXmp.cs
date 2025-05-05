using SE.Halligang.CsXmpToolkit;

namespace SimpleXmpLib.FileEmbedding;

public record FileWithEmbeddedXmp(string FilePath, SupportedFileType FileType)
{
    /// <summary>
    /// If no 
    /// </summary>
    /// <returns></returns>
    public (bool HasExistingContainer, IXmpContainer Container) GetContainer()
    {
        var existingContainer = XmpToolkitBasedXmpContainerHelper.CreateFromFile(FilePath, FileType);
        var hasExistingContainer = existingContainer is not null;

        return (hasExistingContainer, existingContainer ?? XmpToolkitBasedXmpContainer.CreateNew());
    }

    /// <summary>
    /// Null if no XMP data found
    /// </summary>
    /// <returns></returns>
    public string? GetRawXmp()
    {
        var format = SupportedFileTypes.GetInnerFileFormat(FileType);
        using var dirtyXmpFileThingy = new XmpFiles(FilePath, format, OpenFlags.OpenForRead);
        
        using XmpCore xmpObject = new XmpCore();
        PacketInfo pInfo = new PacketInfo();

        var success = dirtyXmpFileThingy.GetXmp(xmpObject, pInfo, out var packetString);
        dirtyXmpFileThingy.CloseFile(CloseFlags.None);
        return success ? packetString : null;
    }
}