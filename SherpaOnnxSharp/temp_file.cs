using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SherpaOnnxSharp
{
    internal class temp_file
    {
        private double[] compute_fbank_bytes(double[] wavsignal, int fs = 16000, int num_bins = 80)
        {
            var window = new FftSharp.Windows.Hamming();
            NDarray x = np.linspace(0, 400 - 1, num_bins * 2, true, np.longlong);
            NDarray w = 0.54 - 0.46 * np.cos(2 * np.pi * (x) / (400 - 1));  // 汉明窗
            // wav波形 加时间窗以及时移10ms
            int time_window = 10;  // 单位ms
            NDarray wav_arr = np.array(wavsignal);

            int range0_end = (int)(wavsignal.Length / (fs / 1000) - time_window) / 10 + 1; // 计算循环终止的位置，也就是最终生成的窗数
            NDarray data_input = np.zeros((range0_end, num_bins), np.longlong);  // 用于存放最终的频率特征数据
            NDarray data_line = np.zeros((1, num_bins * 2), np.longlong);
            for (int i = 0; i < range0_end; i++)
            {
                int p_start = i * 160;
                int p_end = p_start + num_bins * 2;
                NDarray new_data_line = wav_arr[p_start.ToString() + ":" + p_end.ToString()];
                //if (new_data_line.size < num_bins*2)
                //{
                //    new_data_line=np.pad(new_data_line, np.array(new int[] { 0, num_bins*2 - new_data_line.size }), "stat_length", new int[] { 0, 0 });
                //}                
                new_data_line = new_data_line * w;  // 加窗
                Complex[] fftRaw = FftSharp.Transform.MakeComplex(new_data_line.GetData<double>());
                double[] fft = FftSharp.Transform.Absolute(fftRaw);
                new_data_line = np.abs(fft);
                data_input[i] = new_data_line["0:" + num_bins.ToString() + ""];
            }
            data_input = np.log(data_input + 1);
            NDarray out_input = data_input.flatten();
            //data_input = data_input[::]
            double[] fbanks = out_input.GetData<double>();
            return fbanks;
        }
    }
}
