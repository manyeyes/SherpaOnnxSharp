# SherpaOnnxSharp
## 简介
新一代Kaldi包含Lhotse、k2、Icefall、sherpa，是有着良好的生态且专于ASR的开源项目。其中sherpa-onnx是新一代Kaldi中用于部署onnx模型的语音识别服务，作者使用c、c++开发，性能优异，且支持众多优秀的开源免费模型。本项目用c#语言将sherpa-onnx的c-api进行封装，使得在c#中应用语音识别模型更加方便。
## 如何使用
### 1.在项目中引用
```
using SherpaOnnxSharp;
using SherpaOnnxSharp.Model;
```
### 2.调用
实例化：
```
OfflineRecognizer<OfflineTransducer> offlineRecognizer = new OfflineRecognizer<OfflineTransducer>(
                offlineTransducer,
                tokens,
                num_threads: numThreads,
                debug: isDebug,
                decoding_method: decodingMethod);
```
准备数据：
```
List<float[]> samplesList = new List<float[]>();
foreach (string wavFile in wavFiles)
{
     float[] samples = Utils.AudioHelper.GetFileSamples(wavFile, ref duration);
     samplesList.Add(samples);
     total_duration += duration;
}
OfflineStream[] streams = offlineRecognizer.CreateOfflineStream(samplesList);
```
解码：
```
offlineRecognizer.DecodeMultipleOfflineStreams(streams);
```
获取解码结果：
 ```
results = offlineRecognizer.GetResults(streams);
```
## 运行示例
### 1.流式识别:
运行：SherpaOnnxSharp.Examples.OnlineDecodeFile
出现cmd窗口后，根据提示输入命令。
transducer Usage:
```
  --tokens=./all_models/sherpa-onnx-streaming-zipformer-bilingual-zh-en-2023-02-20/tokens.txt `
  --encoder=./all_models/sherpa-onnx-streaming-zipformer-bilingual-zh-en-2023-02-20/encoder-epoch-99-avg-1.onnx `
  --decoder=./all_models/sherpa-onnx-streaming-zipformer-bilingual-zh-en-2023-02-20/decoder-epoch-99-avg-1.onnx `
  --joiner=./all_models/sherpa-onnx-streaming-zipformer-bilingual-zh-en-2023-02-20/joiner-epoch-99-avg-1.onnx `
  --num-threads=2 `
  --decoding-method=modified_beam_search `
  --debug=false `
  ./all_models/sherpa-onnx-streaming-zipformer-bilingual-zh-en-2023-02-20/test_wavs/0.wav
```
运行结果：

```
昨
昨天
昨天
昨天
昨天是
昨天是▁MON
昨天是▁MONDAY
昨天是▁MONDAY
昨天是▁MONDAY
昨天是▁MONDAY
昨天是▁MONDAY
昨天是▁MONDAY
昨天是▁MONDAY
昨天是▁MONDAY
昨天是▁MONDAY▁TODAY
昨天是▁MONDAY▁TODAY
昨天是▁MONDAY▁TODAY▁IS
昨天是▁MONDAY▁TODAY▁IS
昨天是▁MONDAY▁TODAY▁IS
昨天是▁MONDAY▁TODAY▁IS▁LI
昨天是▁MONDAY▁TODAY▁IS▁LIB
昨天是▁MONDAY▁TODAY▁IS▁LIBR
昨天是▁MONDAY▁TODAY▁IS▁LIBR
昨天是▁MONDAY▁TODAY▁IS▁LIBR
昨天是▁MONDAY▁TODAY▁IS礼拜二▁THE
昨天是▁MONDAY▁TODAY▁IS礼拜二▁THE▁DAY
昨天是▁MONDAY▁TODAY▁IS礼拜二▁THE▁DAY
昨天是▁MONDAY▁TODAY▁IS礼拜二▁THE▁DAY▁AFTER
昨天是▁MONDAY▁TODAY▁IS礼拜二▁THE▁DAY▁AFTER
昨天是▁MONDAY▁TODAY▁IS礼拜二▁THE▁DAY▁AFTER▁TOMORROW
昨天是▁MONDAY▁TODAY▁IS礼拜二▁THE▁DAY▁AFTER▁TOMORROW
昨天是▁MONDAY▁TODAY▁IS礼拜二▁THE▁DAY▁AFTER▁TOMORROW
昨天是▁MONDAY▁TODAY▁IS礼拜二▁THE▁DAY▁AFTER▁TOMORROW
昨天是▁MONDAY▁TODAY▁IS礼拜二▁THE▁DAY▁AFTER▁TOMORROW是
昨天是▁MONDAY▁TODAY▁IS礼拜二▁THE▁DAY▁AFTER▁TOMORROW是星
昨天是▁MONDAY▁TODAY▁IS礼拜二▁THE▁DAY▁AFTER▁TOMORROW是星
昨天是▁MONDAY▁TODAY▁IS礼拜二▁THE▁DAY▁AFTER▁TOMORROW是星期三
num_threads:2
decoding_method:modified_beam_search
elapsed_milliseconds:1996.171875
wave total_duration_milliseconds:412178.125
Real time factor (RTF):0.0048429835401866605
End!
```
