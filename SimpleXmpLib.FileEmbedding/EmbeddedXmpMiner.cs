using SE.Halligang.CsXmpToolkit;
using SimpleXmpLib.Exceptions;

namespace SimpleXmpLib.FileEmbedding;

public static class EmbeddedXmpMiner
{
    public class UnsupportedFileTypeException : ArgumentException, ISimpleXmpLibException
    {
        public UnsupportedFileTypeException(string fileName) : base($"Provided file type is not supported {fileName}")
        {
        }

        public UnsupportedFileTypeException(string fileName, string message) : base($"Provided file type is not supported {fileName}. {message}")
        {
        }
    }

    public static FileWithEmbeddedXmp GetXmpSource(string filePath)
    {
        var fileType = GetSupportedFileType(filePath);
        return new(filePath, fileType);
    }

    private static SupportedFileType GetSupportedFileType(string filePath)
    {
        if (!System.IO.File.Exists(filePath)) throw new FileNotFoundException("File does not exist", filePath);

        var matches = SupportedFileTypes.All.Where(x => x.MatchesPath(filePath)).ToList();

        if (!matches.Any()) throw new UnsupportedFileTypeException(filePath);
        if (matches.Count > 1) throw new SimpleXmpLibUnreachableCodeException("Multiple matches for file type");
        
        return matches.Single();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="fileWithXmp"></param>
    /// <param name="container"></param>
    /// <exception cref="NotSupportedException"></exception>
    /// <exception cref="Exception"></exception>
    private static void WriteXmpToFile(FileWithEmbeddedXmp fileWithXmp, IXmpContainer container)
    {
        if (container is not XmpToolkitBasedXmpContainer toolkitBasedContainer)
        {
            throw new NotSupportedException(
                $"Unsupported implementation of {nameof(IXmpContainer)} ({container.GetType().Name}), cannot save to file. Only {nameof(XmpToolkitBasedXmpContainer)} is supported.");
        }

        var filePath = fileWithXmp.FilePath;
        var format = SupportedFileTypes.GetInnerFileFormat(fileWithXmp.FileType);

        using var dirtyXmpFileThingy = XmpToolkitBasedXmpContainerHelper
            .TryVeryHardToGetXmpFilesObject(
                filePath, 
                format, 
                XmpToolkitBasedXmpContainerHelper.FileWithEmbeddedXmpOpenMode.Write);
        
        if (!dirtyXmpFileThingy.CanPutXmp(toolkitBasedContainer.InnerContainer))
        {
            dirtyXmpFileThingy.CloseFile(CloseFlags.None);
            throw new Exception("CsXmpToolkit is unable to save provided xmp object inside the provided file");
        }
        else
        {
            dirtyXmpFileThingy.PutXmp(toolkitBasedContainer.InnerContainer);
            dirtyXmpFileThingy.CloseFile(CloseFlags.None);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="filePath"></param>
    /// <param name="container"></param>
    public static void PersistXmpInFile(string filePath, IXmpContainer container)
    {
        var xmpSource = GetXmpSource(filePath);
        WriteXmpToFile(xmpSource, container);
    }
}