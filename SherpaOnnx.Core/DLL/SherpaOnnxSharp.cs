using System.Runtime.InteropServices;
using SherpaOnnx.Core.Structs;

namespace SherpaOnnx.Core.DLL
{
    /// <summary>
    /// csharp api for sherpa-onnx 
    /// Copyright (c)  2023 by manyeyes
    /// </summary>
    public static class SherpaOnnxSharp
    {
        private const string dllName = @"\lib\SherpaOnnxSharp.dll";

        [DllImport(dllName, EntryPoint = "CreateOfflineRecognizer", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public static extern SherpaOnnxOfflineRecognizer CreateOfflineRecognizer(SherpaOnnxOfflineRecognizerConfig config);

        [DllImport(dllName, EntryPoint = "CreateOfflineStream", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public static extern SherpaOnnxOfflineStream CreateOfflineStream(SherpaOnnxOfflineRecognizer offlineRecognizer);

        [DllImport(dllName, EntryPoint = "AcceptWaveform", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public static extern void AcceptWaveform(SherpaOnnxOfflineStream stream, int sample_rate,float[] samples, int samples_size);

        [DllImport(dllName, EntryPoint = "DecodeOfflineStream", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public static extern void DecodeOfflineStream(SherpaOnnxOfflineRecognizer recognizer, SherpaOnnxOfflineStream stream);

        [DllImport(dllName, EntryPoint = "DecodeMultipleOfflineStreams", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public static extern void DecodeMultipleOfflineStreams(SherpaOnnxOfflineRecognizer recognizer,IntPtr
         streams, int n);

        [DllImport(dllName, EntryPoint = "GetOfflineStreamResult", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr GetOfflineStreamResult(SherpaOnnxOfflineStream stream);

        [DllImport(dllName, EntryPoint = "DestroyOfflineRecognizerResult", CallingConvention = CallingConvention.Cdecl)]
        public static extern void DestroyOfflineRecognizerResult(IntPtr result);

        [DllImport(dllName, EntryPoint = "DestroyOfflineStream", CallingConvention = CallingConvention.Cdecl)]
        public static extern void DestroyOfflineStream(SherpaOnnxOfflineStream stream);

        [DllImport(dllName, EntryPoint = "DestroyOfflineRecognizer", CallingConvention = CallingConvention.Cdecl)]
        public static extern void DestroyOfflineRecognizer(SherpaOnnxOfflineRecognizer offlineRecognizer);
        
    }
}
