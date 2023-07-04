using SherpaOnnxSharp.Structs;

namespace SherpaOnnxSharp
{
    public class OnlineStream : OnlineBase
    {
        internal OnlineStream(SherpaOnnxOnlineStream onlineStream)
        {
            this._onlineStream = onlineStream;
        }

        protected override void Dispose(bool disposing)
        {
            if (!disposing)
            {
                DLL.SherpaOnnx.DestroyOnlineStream(_onlineStream);
                _onlineStream.impl = IntPtr.Zero;
                this._disposed = true;
                base.Dispose();
            }            
        }
    }
}
