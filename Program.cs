// See https://aka.ms/new-console-template for more information
// Copyright (c)  2023 by manyeyes
using SherpaOnnx.Core;
using SherpaOnnx.Core.Structs;
using SherpaOnnx.Core.Model;

/// Please refer to
/// https://k2-fsa.github.io/sherpa/onnx/pretrained_models/index.html
/// to download pre-trained models. That is, you can find encoder-xxx.onnx
/// decoder-xxx.onnx, joiner-xxx.onnx, and tokens.txt for this struct
/// from there.

/// download model eg:
/// (The directory where the application runs)
/// [/path/to]=System.AppDomain.CurrentDomain.BaseDirectory
/// cd /path/to
/// git clone https://huggingface.co/csukuangfj/sherpa-onnx-zipformer-en-2023-04-01
/// git clone https://huggingface.co/csukuangfj/paraformer-onnxruntime-python-example
/// git clone https://huggingface.co/csukuangfj/sherpa-onnx-nemo-ctc-en-citrinet-512

/// NuGet for SherpaOnnx.Core
/// PM > Install-Package NAudio -version 2.1.0 -Project SherpaOnnx.Core
/// PM > Install-Package Microsoft.Extensions.Logging -version 7.0.0 -SherpaOnnx.Core

Console.WriteLine("Started!\n");

string? applicationBase = System.AppDomain.CurrentDomain.BaseDirectory;

// transducer model
string decoder = applicationBase + 
    @"all_models\sherpa-onnx-zipformer-en-2023-04-01\decoder-epoch-99-avg-1.int8.onnx";
string encoder = applicationBase + 
    @"all_models\sherpa-onnx-zipformer-en-2023-04-01\encoder-epoch-99-avg-1.int8.onnx";
string joiner = applicationBase + 
    @"all_models\sherpa-onnx-zipformer-en-2023-04-01\joiner-epoch-99-avg-1.int8.onnx";
string transducer_tokensFilePath = applicationBase + 
    @"all_models\sherpa-onnx-zipformer-en-2023-04-01\tokens.txt";

// paraformer model
string paraformer_modelFilePath = applicationBase +
    @"all_models\paraformer-onnxruntime-python-example\model.onnx";
string paraformer_tokensFilePath = applicationBase +
    @"all_models\paraformer-onnxruntime-python-example\tokens_id.txt";

// nemo_ctc model
string nemo_ctc_modelFilePath = applicationBase +
    @"all_models\sherpa-onnx-nemo-ctc-en-citrinet-512\model.int8.onnx";
string nemo_ctc_tokensFilePath = applicationBase +
    @"all_models\sherpa-onnx-nemo-ctc-en-citrinet-512\tokens.txt";

SherpaOnnxOfflineParaformer paraformer = new SherpaOnnxOfflineParaformer();
paraformer.model = paraformer_modelFilePath;

SherpaOnnxOfflineTransducer transducer = new SherpaOnnxOfflineTransducer();
transducer.encoder_filename = encoder;
transducer.decoder_filename = decoder;
transducer.joiner_filename = joiner;

SherpaOnnxOfflineNemoEncDecCtc nemo_ctc = new SherpaOnnxOfflineNemoEncDecCtc();
nemo_ctc.model = nemo_ctc_modelFilePath;

int num_threads = 2;
string decoding_method = "greedy_search";

if (string.IsNullOrEmpty(paraformer.model) 
    || string.IsNullOrEmpty(paraformer.model) 
    || string.IsNullOrEmpty(paraformer.model)) {
    Console.WriteLine("Please specify at least one model");
    return;
}

OfflineRecognizer offlineRecognizer = new OfflineRecognizer(
    transducer,
    transducer_tokensFilePath, 
    num_threads:num_threads, 
    decoding_method: decoding_method);

//batch decode
int batch_size = 2;
TimeSpan total_duration = new TimeSpan(0);
SherpaOnnxOfflineStream[] streams = new SherpaOnnxOfflineStream[batch_size];
List<string> wavFiles = new List<string>();
for (int i = 0; i < batch_size; i++)
{
    string wavFilePath = 
        string.Format(applicationBase + 
        @"all_models/sherpa-onnx-conformer-en-2023-03-18\test_wavs\{0}.wav", 
        i);
    wavFiles.Add(string.Format("{0}.wav",i));
    SherpaOnnxOfflineStream stream = offlineRecognizer.CreateOfflineStream();
    TimeSpan duration = new TimeSpan(0);
    float[] samples = SherpaOnnx.Core.Utils.AudioHelper.GetTestSamples(wavFilePath, ref duration);
    total_duration += duration;
    offlineRecognizer.AcceptWaveform(stream, 16000, samples);
    streams[i] = stream;
}
TimeSpan start_time = new TimeSpan(DateTime.Now.Ticks);
offlineRecognizer.DecodeMultipleOfflineStreams(streams, streams.Length);
List<OfflineRecognizerResultEntity> results = offlineRecognizer.GetResults(streams);
TimeSpan end_time = new TimeSpan(DateTime.Now.Ticks);

foreach (var item in results.Zip<OfflineRecognizerResultEntity, string>(wavFiles))
{
    Console.WriteLine("wavFile:{0}", item.Second);
    Console.WriteLine("text:{0}", item.First.text.ToLower());
    Console.WriteLine("text_len:{0}\n", item.First.text_len.ToString());
}

double elapsed_milliseconds = end_time.TotalMilliseconds - start_time.TotalMilliseconds;
double rtf = elapsed_milliseconds / total_duration.TotalMilliseconds;
Console.WriteLine("num_threads:{0}",num_threads);
Console.WriteLine("decoding_method:{0}",decoding_method);
Console.WriteLine("elapsed_milliseconds:{0}", elapsed_milliseconds.ToString());
Console.WriteLine("wave total_duration_milliseconds:{0}", total_duration.TotalMilliseconds.ToString());
Console.WriteLine("Real time factor (RTF):{0}", rtf.ToString());

Console.WriteLine("End!");
