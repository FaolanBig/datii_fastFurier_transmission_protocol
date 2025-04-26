// This code is part of the datii_fastFurier_transmission_protocol project.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;


namespace datii_fastFurier_transmission_protocol
{
    internal class FrequencyAnalyzer
    {
        public static List<(double frequency, double magnitude)> GetTopFrequencies(Complex[] fftResult, double sampleRate, int count)
        {
            int n = fftResult.Length;
            var magnitudes = fftResult.Take(n / 2)
                .Select((c, i) => new { Index = i, Magnitude = c.Magnitude })
                .OrderByDescending(x => x.Magnitude)
                .Take(count)
                .Select(x => (x.Index * sampleRate / n, x.Magnitude))
                .ToList();

            return magnitudes;
        }

    }
}
