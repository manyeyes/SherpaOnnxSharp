using System.Runtime.InteropServices;

namespace SherpaOnnxSharp.Structs
{
    /// Please refer to
    /// https://k2-fsa.github.io/sherpa/onnx/pretrained_models/index.html
    /// to download pre-trained models. That is, you can find encoder-xxx.onnx
    /// decoder-xxx.onnx, joiner-xxx.onnx, and tokens.txt for this struct
    /// from there.
    internal struct SherpaOnnxOnlineTransducer
    {
        public string encoder_filename;
        public string decoder_filename;
        public string joiner_filename;
        public SherpaOnnxOnlineTransducer()
        {
            encoder_filename = string.Empty;
            decoder_filename = string.Empty;
            joiner_filename = string.Empty;
        }
    };

    internal struct SherpaOnnxOnlineModelConfig
    {
        public SherpaOnnxOnlineTransducer transducer;
        public string tokens;
        public int num_threads;
        public bool debug;  // true to print debug information of the model
    };

    ///// It expects 16 kHz 16-bit single channel wave format.
    //internal struct SherpaOnnxFeatureConfig
    //{
    //    /// Sample rate of the input data. MUST match the one expected
    //    /// by the model. For instance, it should be 16000 for models provided
    //    /// by us.
    //    public int sample_rate;

    //    /// Feature dimension of the model.
    //    /// For instance, it should be 80 for models provided by us.
    //    public int feature_dim;
    //};

    internal struct SherpaOnnxOnlineRecognizerConfig
    {
        public SherpaOnnxFeatureConfig feat_config;
        public SherpaOnnxOnlineModelConfig model_config;

        /// Possible values are: greedy_search, modified_beam_search
        public string decoding_method;

        /// Used only when decoding_method is modified_beam_search
        /// Example value: 4
        public int max_active_paths;

        /// 0 to disable endpoint detection.
        /// A non-zero value to enable endpoint detection.
        public int enable_endpoint;

        /// An endpoint is detected if trailing silence in seconds is larger than
        /// this value even if nothing has been decoded.
        /// Used only when enable_endpoint is not 0.
        public float rule1_min_trailing_silence;

        /// An endpoint is detected if trailing silence in seconds is larger than
        /// this value after something that is not blank has been decoded.
        /// Used only when enable_endpoint is not 0.
        public float rule2_min_trailing_silence;

        /// An endpoint is detected if the utterance in seconds is larger than
        /// this value.
        /// Used only when enable_endpoint is not 0.
        public float rule3_min_utterance_length;
    };

    internal struct SherpaOnnxOnlineRecognizerResult
    {
        public IntPtr text;
        public int text_len;
        // TODO: Add more fields
    }
    internal struct SherpaOnnxOnlineRecognizer
    {
        public IntPtr impl;
    };

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
    internal struct SherpaOnnxOnlineStream
    {
        public IntPtr impl;
    };
}

////internal struct SherpaOnnxOnlineParaformer
////{
////    public string model;
////    public SherpaOnnxOnlineParaformer()
////    {
////        model = "";
////    }
////};

////internal struct SherpaOnnxOnlineNemoEncDecCtc
////{
////    public string model;
////    public SherpaOnnxOnlineNemoEncDecCtc()
////    {
////        model = "";
////    }
////};




///// It expects 16 kHz 16-bit single channel wave format.
//internal struct SherpaOnnxFeatureConfig
//{
//    /// Sample rate of the input data. MUST match the one expected
//    /// by the model. For instance, it should be 16000 for models provided
//    /// by us.
//    public int sample_rate;

//    /// Feature dimension of the model.
//    /// For instance, it should be 80 for models provided by us.
//    public int feature_dim;
//};





