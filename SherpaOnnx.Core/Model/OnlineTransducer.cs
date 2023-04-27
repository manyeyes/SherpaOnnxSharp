using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SherpaOnnx.Core.Model
{
    public class OnlineTransducer
    {
        private string encoderFilename = string.Empty;
        private string decoderFilename = string.Empty;
        private string joinerFilename = string.Empty;
        public string EncoderFilename { get => encoderFilename; set => encoderFilename = value; }
        public string DecoderFilename { get => decoderFilename; set => decoderFilename = value; }
        public string JoinerFilename { get => joinerFilename; set => joinerFilename = value; }
    }
}
