using System.Runtime.CompilerServices;
using SE.Halligang.CsXmpToolkit;

namespace SimpleXmpLib.FileEmbedding;

/// <summary>
/// Ensures all the stuff is initialized
/// </summary>
internal static class SimpleXmpLibInitializationHelper
{
    [ModuleInitializer]
    internal static void Initialize()
    {
        _xmpFiles = new();
    }

    private static XmpFilesContextHelper? _xmpFiles = null!;

    private class XmpFilesContextHelper
    {
        public XmpFilesContextHelper()
        {
            XmpFiles.Initialize();
            AppDomain.CurrentDomain.ProcessExit += (sender, args) => XmpFiles.Terminate();
        }
    }
}