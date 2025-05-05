using System.Collections.Concurrent;

namespace SharedTestData;

public abstract class FileBasedTestBase : IDisposable
{
    private readonly ConcurrentBag<string> _tempFiles = [];

    /// <summary>
    /// Gets a temp file with the same contents & extension etc as the original file.
    /// </summary>
    /// <param name="filePath"></param>
    /// <returns></returns>
    protected string CreateTempCopy(string filePath)
    {
        var tempFile = Path.GetTempFileName() + "." + Path.GetFileName(filePath);
        File.Copy(filePath, tempFile, true);
        _tempFiles.Add(tempFile);
        return tempFile;
    }

    public static string DefaultFileWithXmp => TestData.WithEmbeddedXmp.First();
    public static string DefaultFileWithoutXmp => TestData.WithoutEmbeddedXmp.First();

    public static IEnumerable<object[]> TestFilesWithoutEmbeddedXmp => TestData.WithoutEmbeddedXmp
        .Select(x => new[] { x })
        .ToArray();

    public static IEnumerable<object[]> TestFilesWithEmbeddedXmp => TestData.WithEmbeddedXmp
        .Select(x => new[] { x })
        .ToArray();

    public static IEnumerable<object[]> TestFilesWithUnsupportedFileType => TestData.UnsupportedFileType
        .Select(x => new[] { x })
        .ToArray();

    public void Dispose()
    {
        _tempFiles.Distinct().ToList().ForEach(file =>
        {
            try
            {
                File.Delete(file);
            }
            catch (Exception ex)
            {
                //don't care lol
            }
        });
    }
}