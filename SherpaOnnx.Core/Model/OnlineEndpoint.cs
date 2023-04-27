using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SherpaOnnx.Core.Model
{
    public class OnlineEndpoint
    {
        /// 0 to disable endpoint detection.
        /// A non-zero value to enable endpoint detection.
        private int enableEndpoint;

        /// An endpoint is detected if trailing silence in seconds is larger than
        /// this value even if nothing has been decoded.
        /// Used only when enable_endpoint is not 0.
        private float rule1MinTrailingSilence;

        /// An endpoint is detected if trailing silence in seconds is larger than
        /// this value after something that is not blank has been decoded.
        /// Used only when enable_endpoint is not 0.
        private float rule2MinTrailingSilence;

        /// An endpoint is detected if the utterance in seconds is larger than
        /// this value.
        /// Used only when enable_endpoint is not 0.
        private float rule3MinUtteranceLength;

        public int EnableEndpoint { get => enableEndpoint; set => enableEndpoint = value; }
        public float Rule1MinTrailingSilence { get => rule1MinTrailingSilence; set => rule1MinTrailingSilence = value; }
        public float Rule2MinTrailingSilence { get => rule2MinTrailingSilence; set => rule2MinTrailingSilence = value; }
        public float Rule3MinUtteranceLength { get => rule3MinUtteranceLength; set => rule3MinUtteranceLength = value; }
    }
}
