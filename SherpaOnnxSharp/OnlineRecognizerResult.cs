namespace SherpaOnnxSharp
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
                DLL.SherpaOnnx.DestroyOnlineRecognizerResult(_onlineRecognizerResult);
                _onlineRecognizerResult = IntPtr.Zero;
                this._disposed = true;
                base.Dispose(disposing);
            }            
        }
    }
}
