using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SherpaOnnx.Core.Utils
{
    /// <summary>
    /// audio processing
    /// Copyright (c)  2023 by manyeyes
    /// </summary>
    public class AudioHelper
    {
        public static float[] GetTestSamples(string wavFilePath,ref TimeSpan duration)
        {
            if (!File.Exists(wavFilePath))
            {
                return new float[1];
            }
            AudioFileReader _audioFileReader = new AudioFileReader(wavFilePath);
            byte[] datas = new byte[_audioFileReader.Length];
            _audioFileReader.Read(datas, 0, datas.Length);
            duration = _audioFileReader.TotalTime;
            float[] wavdata = new float[datas.Length / sizeof(float)];
            Buffer.BlockCopy(datas, 0, wavdata, 0, datas.Length);
            return wavdata;
        }
    }
}
