using System.Runtime.InteropServices;
using SherpaOnnxSharp.Structs;

namespace SherpaOnnxSharp.DLL
{
    /// <summary>
    /// csharp api for sherpa-onnx 
    /// Copyright (c)  2023 by manyeyes
    /// </summary>
    internal static partial class SherpaOnnx
    {
        [DllImport(dllName, EntryPoint = "CreateOnlineRecognizer", CallingConvention = CallingConvention.Cdecl)]
        internal static extern SherpaOnnxOnlineRecognizer CreateOnlineRecognizer(SherpaOnnxOnlineRecognizerConfig config);

        /// Free a pointer returned by CreateOnlineRecognizer()
        ///
        /// @param p A pointer returned by CreateOnlineRecognizer()
        [DllImport(dllName, EntryPoint = "DestroyOnlineRecognizer", CallingConvention = CallingConvention.Cdecl)]
        internal static extern void DestroyOnlineRecognizer(SherpaOnnxOnlineRecognizer recognizer);

        /// Create an online stream for accepting wave samples.
        ///
        /// @param recognizer  A pointer returned by CreateOnlineRecognizer()
        /// @return Return a pointer to an OnlineStream. The user has to invoke
        ///         DestroyOnlineStream() to free it to avoid memory leak.
        [DllImport(dllName, EntryPoint = "CreateOnlineStream", CallingConvention = CallingConvention.Cdecl)]
        internal static extern SherpaOnnxOnlineStream CreateOnlineStream(
            SherpaOnnxOnlineRecognizer recognizer);

        /// Destroy an online stream.
        ///
        /// @param stream A pointer returned by CreateOnlineStream()
        [DllImport(dllName, EntryPoint = "DestroyOnlineStream", CallingConvention = CallingConvention.Cdecl)]
        internal static extern void DestroyOnlineStream(SherpaOnnxOnlineStream stream);

        /// Accept input audio samples and compute the features.
        /// The user has to invoke DecodeOnlineStream() to run the neural network and
        /// decoding.
        ///
        /// @param stream  A pointer returned by CreateOnlineStream().
        /// @param sample_rate  Sample rate of the input samples. If it is different
        ///                     from config.feat_config.sample_rate, we will do
        ///                     resampling inside sherpa-onnx.
        /// @param samples A pointer to a 1-D array containing audio samples.
        ///                The range of samples has to be normalized to [-1, 1].
        /// @param n  Number of elements in the samples array.
        [DllImport(dllName, EntryPoint = "AcceptOnlineWaveform", CallingConvention = CallingConvention.Cdecl)]
        internal static extern void AcceptOnlineWaveform(SherpaOnnxOnlineStream stream, int sample_rate,
            float[] samples, int n);

        /// Return 1 if there are enough number of feature frames for decoding.
        /// Return 0 otherwise.
        ///
        /// @param recognizer  A pointer returned by CreateOnlineRecognizer
        /// @param stream  A pointer returned by CreateOnlineStream
        [DllImport(dllName, EntryPoint = "IsOnlineStreamReady", CallingConvention = CallingConvention.Cdecl)]
        internal static extern int IsOnlineStreamReady(SherpaOnnxOnlineRecognizer recognizer,
                SherpaOnnxOnlineStream stream);

        /// Call this function to run the neural network model and decoding.
        //
        /// Precondition for this function: IsOnlineStreamReady() MUST return 1.
        ///
        /// Usage example:
        ///
        ///  while (IsOnlineStreamReady(recognizer, stream)) {
        ///     DecodeOnlineStream(recognizer, stream);
        ///  }
        ///
        [DllImport(dllName, EntryPoint = "DecodeOnlineStream", CallingConvention = CallingConvention.Cdecl)]
        internal static extern void DecodeOnlineStream(SherpaOnnxOnlineRecognizer recognizer,
            SherpaOnnxOnlineStream stream);

        /// This function is similar to DecodeOnlineStream(). It decodes multiple
        /// OnlineStream in parallel.
        ///
        /// Caution: The caller has to ensure each OnlineStream is ready, i.e.,
        /// IsOnlineStreamReady() for that stream should return 1.
        ///
        /// @param recognizer  A pointer returned by CreateOnlineRecognizer()
        /// @param streams  A pointer array containing pointers returned by
        ///                 CreateOnlineRecognizer()
        /// @param n  Number of elements in the given streams array.
        [DllImport(dllName, EntryPoint = "DecodeMultipleOnlineStreams", CallingConvention = CallingConvention.Cdecl)]
        internal static extern void DecodeMultipleOnlineStreams(SherpaOnnxOnlineRecognizer recognizer,
            IntPtr streams, int n);

        /// Get the decoding results so far for an OnlineStream.
        ///
        /// @param recognizer A pointer returned by CreateOnlineRecognizer().
        /// @param stream A pointer returned by CreateOnlineStream().
        /// @return A pointer containing the result. The user has to invoke
        ///         DestroyOnlineRecognizerResult() to free the returned pointer to
        ///         avoid memory leak.
        [DllImport(dllName, EntryPoint = "GetOnlineStreamResult", CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr GetOnlineStreamResult(
            SherpaOnnxOnlineRecognizer recognizer, SherpaOnnxOnlineStream stream);

        /// Destroy the pointer returned by GetOnlineStreamResult().
        ///
        /// @param r A pointer returned by GetOnlineStreamResult()
        [DllImport(dllName, EntryPoint = "DestroyOnlineRecognizerResult", CallingConvention = CallingConvention.Cdecl)]
        internal static extern void DestroyOnlineRecognizerResult(IntPtr result);

        /// Reset an OnlineStream , which clears the neural network model state
        /// and the state for decoding.
        ///
        /// @param recognizer A pointer returned by CreateOnlineRecognizer().
        /// @param stream A pointer returned by CreateOnlineStream
        [DllImport(dllName, EntryPoint = "Reset", CallingConvention = CallingConvention.Cdecl)]
        internal static extern void Reset(SherpaOnnxOnlineRecognizer recognizer,
            SherpaOnnxOnlineStream stream);

        /// Signal that no more audio samples would be available.
        /// After this call, you cannot call AcceptWaveform() any more.
        ///
        /// @param stream A pointer returned by CreateOnlineStream()
        [DllImport(dllName, EntryPoint = "InputFinished", CallingConvention = CallingConvention.Cdecl)]
        internal static extern void InputFinished(SherpaOnnxOnlineStream stream);

        /// Return 1 if an endpoint has been detected.
        ///
        /// @param recognizer A pointer returned by CreateOnlineRecognizer()
        /// @param stream A pointer returned by CreateOnlineStream()
        /// @return Return 1 if an endpoint is detected. Return 0 otherwise.
        [DllImport(dllName, EntryPoint = "IsEndpoint", CallingConvention = CallingConvention.Cdecl)]
        internal static extern int IsEndpoint(SherpaOnnxOnlineRecognizer recognizer,
            SherpaOnnxOnlineStream stream);

        /// Create a display object. Must be freed using DestroyDisplay to avoid
        /// memory leak.
        //[DllImport(dllName, EntryPoint = "CreateDisplay", CallingConvention = CallingConvention.Cdecl)]
        //internal static extern SherpaOnnxDisplay CreateDisplay(int max_word_per_line);

        //[DllImport(dllName, EntryPoint = "DestroyDisplay", CallingConvention = CallingConvention.Cdecl)]
        //internal static extern void DestroyDisplay(SherpaOnnxDisplay display);

        /// Print the result.
        //[DllImport(dllName, EntryPoint = "SherpaOnnxPrint", CallingConvention = CallingConvention.Cdecl)]
        //internal static extern void SherpaOnnxPrint(SherpaOnnxDisplay display, int idx, string s);

    }
}
