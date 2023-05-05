using SherpaOnnx.Core.Structs;
using SherpaOnnx.Core.DLL;
using SherpaOnnx.Core.Model;
using System.Runtime.InteropServices;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace SherpaOnnx.Core
{
    /// <summary>
    /// online recognizer package
    /// Copyright (c)  2023 by manyeyes
    /// </summary>
    public class OnlineRecognizer<T> : OnlineBase
        where T : class, new()
    {
        private readonly ILogger<OnlineRecognizer<T>> _logger;

        public OnlineRecognizer(T t,
            string tokensFilePath, string decoding_method = "greedy_search",
            int sample_rate = 16000, int feature_dim = 80,
            int num_threads = 2, bool debug = false, 
            int max_active_paths = 4, int enable_endpoint=0,
            int rule1_min_trailing_silence=0,int rule2_min_trailing_silence=0,
            int rule3_min_utterance_length=0)
        {
            SherpaOnnxOnlineTransducer transducer = new SherpaOnnxOnlineTransducer();
            SherpaOnnxOnlineModelConfig model_config = new SherpaOnnxOnlineModelConfig();
            if (t is not null && t.GetType() == typeof(OnlineTransducer))
            {
                OnlineTransducer? onlineTransducer = t as OnlineTransducer;
#pragma warning disable CS8602 // 解引用可能出现空引用。
                Trace.Assert(File.Exists(onlineTransducer.DecoderFilename)
                && File.Exists(onlineTransducer.EncoderFilename)
                && File.Exists(onlineTransducer.JoinerFilename), "Please provide a model");
#pragma warning restore CS8602 // 解引用可能出现空引用。
                Trace.Assert(File.Exists(tokensFilePath), "Please provide a tokens");
                Trace.Assert(num_threads > 0, "num_threads must be greater than 0");
                transducer.encoder_filename = onlineTransducer.EncoderFilename;
                transducer.decoder_filename = onlineTransducer.DecoderFilename;
                transducer.joiner_filename = onlineTransducer.JoinerFilename;
            }

            model_config.transducer = transducer;
            model_config.num_threads = num_threads;
            model_config.debug = debug;
            model_config.tokens = tokensFilePath;

            SherpaOnnxFeatureConfig feat_config = new SherpaOnnxFeatureConfig();
            feat_config.sample_rate = sample_rate;
            feat_config.feature_dim = feature_dim;

            SherpaOnnxOnlineRecognizerConfig sherpaOnnxOnlineRecognizerConfig;
            sherpaOnnxOnlineRecognizerConfig.decoding_method = decoding_method;
            sherpaOnnxOnlineRecognizerConfig.feat_config = feat_config;
            sherpaOnnxOnlineRecognizerConfig.model_config = model_config;
            sherpaOnnxOnlineRecognizerConfig.max_active_paths = max_active_paths;
            //endpoint
            sherpaOnnxOnlineRecognizerConfig.enable_endpoint = enable_endpoint;
            sherpaOnnxOnlineRecognizerConfig.rule1_min_trailing_silence = rule1_min_trailing_silence;
            sherpaOnnxOnlineRecognizerConfig.rule2_min_trailing_silence = rule2_min_trailing_silence;
            sherpaOnnxOnlineRecognizerConfig.rule3_min_utterance_length = rule3_min_utterance_length;

            _onlineRecognizer =
                DLL.SherpaOnnxSharp.CreateOnlineRecognizer(sherpaOnnxOnlineRecognizerConfig);
            ILoggerFactory loggerFactory = new LoggerFactory();
            _logger = new Logger<OnlineRecognizer<T>>(loggerFactory);
        }

        internal OnlineStream CreateOnlineStream()
        {
            SherpaOnnxOnlineStream stream = DLL.SherpaOnnxSharp.CreateOnlineStream(_onlineRecognizer);
            return new OnlineStream(stream);
        }
        public void InputFinished(OnlineStream stream)
        {
            DLL.SherpaOnnxSharp.InputFinished(stream._onlineStream);
        }

        public List<OnlineStream> CreateStreams(List<float[]> samplesList)
        {
            int batch_size = samplesList.Count;
            List<OnlineStream> streams = new List<OnlineStream>();
            for (int i = 0; i < batch_size; i++)
            {
                OnlineStream stream = CreateOnlineStream();
                AcceptWaveform(stream._onlineStream, 16000, samplesList[i]);
                InputFinished(stream);
                streams.Add(stream);
            }
            return streams;
        }

        public OnlineStream CreateStream()
        {
            OnlineStream stream = CreateOnlineStream();
            return stream;
        }

        internal void AcceptWaveform(SherpaOnnxOnlineStream stream, int sample_rate, float[] samples)
        {
            SherpaOnnxSharp.AcceptOnlineWaveform(stream, sample_rate, samples, samples.Length);
        }
        public void AcceptWaveForm(OnlineStream stream, int sample_rate, float[] samples)
        {
            AcceptWaveform(stream._onlineStream, sample_rate, samples);
        }

        internal IntPtr GetStreamsIntPtr(OnlineStream[] streams)
        {
            int streams_len = streams.Length;
            int size = Marshal.SizeOf(typeof(SherpaOnnxOnlineStream));
            IntPtr streamsIntPtr = Marshal.AllocHGlobal(size * streams_len);
            unsafe
            {
                byte* ptrbds = (byte*)(streamsIntPtr.ToPointer());
                for (int i = 0; i < streams_len; i++, ptrbds += (size))
                {
                    IntPtr streamIntptr = new IntPtr(ptrbds);
                    Marshal.StructureToPtr(streams[i]._onlineStream, streamIntptr, false);
                }

            }
            return streamsIntPtr;
        }
        internal bool IsReady(OnlineStream stream)
        {
            return DLL.SherpaOnnxSharp.IsOnlineStreamReady(_onlineRecognizer, stream._onlineStream) != 0;
        }
        public void DecodeMultipleStreams(List<OnlineStream> streams)
        {
            while (true)
            {
                List<OnlineStream> streamList = new List<OnlineStream>();
                foreach (OnlineStream stream in streams)
                {
                    if (IsReady(stream))
                    {
                        streamList.Add(stream);
                    }
                }
                if (streamList.Count == 0)
                {
                    break;
                }
                OnlineStream[] streamsBatch = new OnlineStream[streamList.Count];
                for (int i = 0; i < streamsBatch.Length; i++)
                {
                    streamsBatch[i] = streamList[i];
                }
                streamList.Clear();
                IntPtr streamsIntPtr = GetStreamsIntPtr(streamsBatch);
                SherpaOnnxSharp.DecodeMultipleOnlineStreams(_onlineRecognizer, streamsIntPtr, streamsBatch.Length);
                Marshal.FreeHGlobal(streamsIntPtr);
            }
        }

        public void DecodeStream(OnlineStream stream)
        {
            while (IsReady(stream))
            {
                SherpaOnnxSharp.DecodeOnlineStream(_onlineRecognizer, stream._onlineStream);
            }
        }

        internal OnlineRecognizerResultEntity GetResult(SherpaOnnxOnlineStream stream)
        {
            IntPtr result_ip = SherpaOnnxSharp.GetOnlineStreamResult(_onlineRecognizer, stream);
            OnlineRecognizerResult onlineRecognizerResult = new OnlineRecognizerResult(result_ip);
#pragma warning disable CS8605 // 取消装箱可能为 null 的值。
            SherpaOnnxOnlineRecognizerResult result =
                (SherpaOnnxOnlineRecognizerResult)Marshal.PtrToStructure(
                    onlineRecognizerResult._onlineRecognizerResult, typeof(SherpaOnnxOnlineRecognizerResult));
#pragma warning restore CS8605 // 取消装箱可能为 null 的值。

#pragma warning disable CS8600 // 将 null 字面量或可能为 null 的值转换为非 null 类型。
            string text = Marshal.PtrToStringAnsi(result.text);
#pragma warning restore CS8600 // 将 null 字面量或可能为 null 的值转换为非 null 类型。
            this._logger.LogInformation("GetResult result.text:{0}", text);
            this._logger.LogInformation("GetResult result.text_len:{0}", result.text_len.ToString());
            OnlineRecognizerResultEntity onlineRecognizerResultEntity =
                new OnlineRecognizerResultEntity();
            onlineRecognizerResultEntity.text = text;
            onlineRecognizerResultEntity.text_len = result.text_len;

            return onlineRecognizerResultEntity;
        }

        public OnlineRecognizerResultEntity GetResult(OnlineStream stream)
        {
            OnlineRecognizerResultEntity result = GetResult(stream._onlineStream);
            return result;
        }

        public List<OnlineRecognizerResultEntity> GetResults(List<OnlineStream> streams)
        {
            List<OnlineRecognizerResultEntity> results = new List<OnlineRecognizerResultEntity>();
            foreach (OnlineStream stream in streams)
            {
                OnlineRecognizerResultEntity onlineRecognizerResultEntity = GetResult(stream._onlineStream);
                results.Add(onlineRecognizerResultEntity);
            }
            return results;
        }

        protected override void Dispose(bool disposing)
        {
            if (!disposing)
            {
                DLL.SherpaOnnxSharp.DestroyOnlineRecognizer(_onlineRecognizer);
                _onlineRecognizer.impl = IntPtr.Zero;
                this._disposed = true;
                base.Dispose();
            }
        }
    }
}