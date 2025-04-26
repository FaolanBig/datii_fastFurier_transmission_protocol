// This code is part of the datii_fastFurier_transmission_protocol project.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace datii_fastFurier_transmission_protocol
{
    internal class Encoder
    {
        private static readonly int BaseFrequency = 1000; // Hz
        private static readonly int StepFrequency = 200;  // Hz between bits

        public static List<double> EncodeText(string text)
        {
            var frequencies = new List<double>();
            foreach (char c in text)
            {
                byte b = (byte)c;
                for (int i = 0; i < 8; i++)
                {
                    if ((b & (1 << i)) != 0)
                    {
                        frequencies.Add(BaseFrequency + i * StepFrequency);
                    }
                }
                frequencies.Add(BaseFrequency + 8 * StepFrequency); // Delimiter after each character
            }
            return frequencies;
        }
    }
}
