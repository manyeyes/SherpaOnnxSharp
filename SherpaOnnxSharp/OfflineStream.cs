using SherpaOnnxSharp.Structs;

namespace SherpaOnnxSharp
{
    public class OfflineStream : OfflineBase
    {
        internal OfflineStream(SherpaOnnxOfflineStream offlineStream)
        {
            this._offlineStream = offlineStream;
        }

        protected override void Dispose(bool disposing)
        {
            if (!disposing)
            {
                DLL.SherpaOnnx.DestroyOfflineStream(_offlineStream);
                _offlineStream.impl = IntPtr.Zero;
                this._disposed = true;
                base.Dispose();
            }            
        }
    }
}
