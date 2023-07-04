using SherpaOnnxSharp.Structs;
using SherpaOnnxSharp.DLL;

namespace SherpaOnnxSharp
{
    public class OfflineBase : IDisposable
    {
        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposing)
            {
                if(_offlineRecognizerResult != IntPtr.Zero)
                {
                    DLL.SherpaOnnx.DestroyOfflineRecognizerResult(_offlineRecognizerResult);
                    _offlineRecognizerResult = IntPtr.Zero;
                }
                if (_offlineStream.impl != IntPtr.Zero)
                {
                    DLL.SherpaOnnx.DestroyOfflineStream(_offlineStream);
                    _offlineStream.impl = IntPtr.Zero;
                }
                if (_offlineRecognizer.impl != IntPtr.Zero)
                {
                    DLL.SherpaOnnx.DestroyOfflineRecognizer(_offlineRecognizer);
                    _offlineRecognizer.impl = IntPtr.Zero;
                }                
                this._disposed = true;
            }
        }

        ~OfflineBase()
        {
            Dispose(this._disposed);
        }

        internal SherpaOnnxOfflineStream _offlineStream;
        internal IntPtr _offlineRecognizerResult;
        internal SherpaOnnxOfflineRecognizer _offlineRecognizer;
        internal bool _disposed = false;
    }
}
