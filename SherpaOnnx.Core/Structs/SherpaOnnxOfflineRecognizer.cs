using System.Runtime.InteropServices;

namespace SherpaOnnx.Core.Structs
{
    /// Please refer to
    /// https://k2-fsa.github.io/sherpa/onnx/pretrained_models/index.html
    /// to download pre-trained models. That is, you can find encoder-xxx.onnx
    /// decoder-xxx.onnx, joiner-xxx.onnx, and tokens.txt for this struct
    /// from there.
    /// Copyright (c)  2023 by manyeyes
    public struct SherpaOnnxOfflineTransducer
    {
        public string encoder_filename;
        public string decoder_filename;
        public string joiner_filename;
    };

    public struct SherpaOnnxOfflineParaformer
    {
        public string model;
    };

    public struct SherpaOnnxOfflineNemoEncDecCtc
    {
        public string model;
    };


    public struct SherpaOnnxOfflineModelConfig
    {
        public SherpaOnnxOfflineTransducer transducer;
        public SherpaOnnxOfflineParaformer paraformer;
        public SherpaOnnxOfflineNemoEncDecCtc nemo_ctc;
        public string tokens;
        public int num_threads;
        public bool debug;
    };

    /// It expects 16 kHz 16-bit single channel wave format.
    public struct SherpaOnnxFeatureConfig
    {
        /// Sample rate of the input data. MUST match the one expected
        /// by the model. For instance, it should be 16000 for models provided
        /// by us.
        public int sample_rate;

        /// Feature dimension of the model.
        /// For instance, it should be 80 for models provided by us.
        public int feature_dim;
    };

    public struct SherpaOnnxOfflineRecognizerConfig
    {
        public SherpaOnnxFeatureConfig feat_config;
        public SherpaOnnxOfflineModelConfig model_config;

        /// Possible values are: greedy_search, modified_beam_search
        public string decoding_method;

    };

    public struct SherpaOnnxOfflineRecognizer
    {
        public IntPtr impl;
    };

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
    public struct SherpaOnnxOfflineStream
    {
        public IntPtr impl;
    };

    public struct SherpaOnnxOfflineRecognizerResult
    {
        public IntPtr text;
        public int text_len;
    }
}
