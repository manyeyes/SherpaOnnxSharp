using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace SherpaOnnx.Core.DLL
{
    internal static partial class SherpaOnnxSharp
    {
        //if WINDOWS
        private const string dllName = @"\lib\SherpaOnnxSharp.dll";
        //if LINUX
        //private const string dllName = @"\lib\SherpaOnnxSharp.so";
    }
}
