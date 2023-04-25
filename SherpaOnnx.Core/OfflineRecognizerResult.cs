namespace SherpaOnnx.Core
{
    public class OfflineRecognizerResult : OfflineBase
    {
        internal OfflineRecognizerResult(IntPtr offlineRecognizerResult)
        {
            this._offlineRecognizerResult = offlineRecognizerResult;
        }

        protected override void Dispose(bool disposing)
        {
            if (!disposing)
            {
                DLL.SherpaOnnxSharp.DestroyOfflineRecognizerResult(_offlineRecognizerResult);
                _offlineRecognizerResult = IntPtr.Zero;
                this._disposed = true;
                base.Dispose(disposing);
            }            
        }
    }
}
