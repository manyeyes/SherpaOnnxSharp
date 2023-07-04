using System.Runtime.InteropServices;

namespace SherpaOnnxSharp.Structs
{
    /// Please refer to
    /// https://k2-fsa.github.io/sherpa/onnx/pretrained_models/index.html
    /// to download pre-trained models. That is, you can find encoder-xxx.onnx
    /// decoder-xxx.onnx, joiner-xxx.onnx, and tokens.txt for this struct
    /// from there.
    /// Copyright (c)  2023 by manyeyes
    internal struct SherpaOnnxOfflineTransducer
    {
        public string encoder_filename;
        public string decoder_filename;
        public string joiner_filename;
        public SherpaOnnxOfflineTransducer()
        {
            encoder_filename = "";
            decoder_filename = "";
            joiner_filename = "";
        }
    };

    internal struct SherpaOnnxOfflineParaformer
    {
        public string model;
        public SherpaOnnxOfflineParaformer()
        {
            model = "";
        }
    };

    internal struct SherpaOnnxOfflineNemoEncDecCtc
    {
        public string model;
        public SherpaOnnxOfflineNemoEncDecCtc()
        {
            model = "";
        }
    };


    internal struct SherpaOnnxOfflineModelConfig
    {
        public SherpaOnnxOfflineTransducer transducer;
        public SherpaOnnxOfflineParaformer paraformer;
        public SherpaOnnxOfflineNemoEncDecCtc nemo_ctc;
        public string tokens;
        public int num_threads;
        public bool debug;
    };

    /// It expects 16 kHz 16-bit single channel wave format.
    internal struct SherpaOnnxFeatureConfig
    {
        /// Sample rate of the input data. MUST match the one expected
        /// by the model. For instance, it should be 16000 for models provided
        /// by us.
        public int sample_rate;

        /// Feature dimension of the model.
        /// For instance, it should be 80 for models provided by us.
        public int feature_dim;
    };

    internal struct SherpaOnnxOfflineRecognizerConfig
    {
        public SherpaOnnxFeatureConfig feat_config;
        public SherpaOnnxOfflineModelConfig model_config;

        /// Possible values are: greedy_search, modified_beam_search
        public string decoding_method;

    };

    internal struct SherpaOnnxOfflineRecognizer
    {
        public IntPtr impl;
    };

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
    internal struct SherpaOnnxOfflineStream
    {
        public IntPtr impl;
    };

    internal struct SherpaOnnxOfflineRecognizerResult
    {
        public IntPtr text;
        public int text_len;
    }
}
