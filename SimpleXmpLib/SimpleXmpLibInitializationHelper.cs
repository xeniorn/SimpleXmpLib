using System.Runtime.CompilerServices;
using SE.Halligang.CsXmpToolkit;

namespace SimpleXmpLib;

/// <summary>
/// Ensures all the stuff is initialized
/// </summary>
internal static class SimpleXmpLibInitializationHelper
{
    [ModuleInitializer]
    internal static void Initialize()
    {
        _xmpCore = new();
    }
    
    private static XmpCoreContextHelper _xmpCore = null!;

    private class XmpCoreContextHelper
    {
        public XmpCoreContextHelper()
        {
            XmpCore.Initialize();
            AppDomain.CurrentDomain.ProcessExit += (sender, args) => XmpCore.Terminate();
        }
    }
}