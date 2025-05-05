using System.Diagnostics;
using SE.Halligang.CsXmpToolkit;

namespace SimpleXmpLib.FileEmbedding;

public static class XmpToolkitBasedXmpContainerHelper
{
    internal enum FileWithEmbeddedXmpOpenMode
    {
        Null,
        Read,
        Write
    }

    /// <summary>
    /// Null if does not contain XMP
    /// </summary>
    /// <param name="filePath"></param>
    /// <param name="fileType"></param>
    /// <returns></returns>
    /// <exception cref="FileNotFoundException"></exception>
    internal static XmpToolkitBasedXmpContainer? CreateFromFile(string filePath, SupportedFileType fileType)
    {
        if (!System.IO.File.Exists(filePath)) throw new FileNotFoundException("File not found", filePath);

        var format = SupportedFileTypes.GetInnerFileFormat(fileType);

        // try to open the file using the smart reader first, if that fails, try the packet scanning reader
        // for some files it simply fails with smart (e.g. Pixel 9 or S24 Ultra jpgs unless scanning is used
        using var dirtyXmpFileThingy = TryVeryHardToGetXmpFilesObject(filePath, format, FileWithEmbeddedXmpOpenMode.Read);

        XmpCore xmpObject = new XmpCore();
        PacketInfo packetInfo = new PacketInfo();
        var reportedSuccess = dirtyXmpFileThingy.GetXmp(xmpObject, packetInfo);
        dirtyXmpFileThingy.CloseFile(CloseFlags.None);

        // gotta do a packet length check as well as some handlers (e.g. jpg, tiff... return true even when the "packet" is completely empty!
        if (reportedSuccess && packetInfo.Length > 0)
        {
            return new XmpToolkitBasedXmpContainer(xmpObject);
        }

        return null;
    }

    internal static XmpFiles TryVeryHardToGetXmpFilesObject(string filePath, FileFormat format, FileWithEmbeddedXmpOpenMode openMode)
    {
        var readOrWriteFlag = openMode switch
        {
            FileWithEmbeddedXmpOpenMode.Read => OpenFlags.OpenForRead | OpenFlags.OpenOnlyXmp,
            FileWithEmbeddedXmpOpenMode.Write => OpenFlags.OpenForUpdate,
            _ => throw new ArgumentOutOfRangeException(nameof(openMode), openMode, null)
        };

        try
        {
            var dirtyXmpFileThingy_smart = new XmpFiles(filePath, format,
                readOrWriteFlag | OpenFlags.OpenStrictly | OpenFlags.OpenUseSmartHandler);
           return dirtyXmpFileThingy_smart;
        }
        catch (Exception ex)
        {
            var message1 = $"Failed to open file {filePath} as {format} using smart reader.";
            Debug.Print(message1);
            var failException1 = new Exception(message1, ex);

            try
            {
                var dirtyXmpFileThingy_smart_flexible = new XmpFiles(filePath, format,
                    readOrWriteFlag | OpenFlags.OpenUseSmartHandler);
                return dirtyXmpFileThingy_smart_flexible;
            }
            catch (Exception ex1)
            {
                var message11 = $"Failed to open file {filePath} using smart reader and guessing the format.";
                Debug.Print(message11);
                var failException11 = new Exception(message11, ex1);

                try
                {
                    var dirtyXmpFileThingy_scan = new XmpFiles(filePath, format,
                        readOrWriteFlag | OpenFlags.OpenUsePacketScanning);
                    return dirtyXmpFileThingy_scan;
                }
                catch (Exception ex2)
                {
                    var message2 = $"Failed to open file {filePath} as {format} using packet scanning";
                    Debug.Print(message2);

                    var ex3 = new Exception(
                        "Failed to create an XmpFiles object/file handler by any technique, something is weird with the file probably.",
                        new AggregateException(failException1, failException11, ex2));

                    throw ex3;
                }
            }
        }
    }
}