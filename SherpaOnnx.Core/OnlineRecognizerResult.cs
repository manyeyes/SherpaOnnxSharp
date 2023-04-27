namespace SherpaOnnx.Core
{
    public class OnlineRecognizerResult : OnlineBase
    {
        internal OnlineRecognizerResult(IntPtr onlineRecognizerResult)
        {
            this._onlineRecognizerResult = onlineRecognizerResult;
        }

        protected override void Dispose(bool disposing)
        {
            if (!disposing)
            {
                DLL.SherpaOnnxSharp.DestroyOnlineRecognizerResult(_onlineRecognizerResult);
                _onlineRecognizerResult = IntPtr.Zero;
                this._disposed = true;
                base.Dispose(disposing);
            }            
        }
    }
}
