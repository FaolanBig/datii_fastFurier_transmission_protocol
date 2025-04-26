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
        private readonly int sampleRate;
        private readonly int fftSize;

        public FrequencyAnalyzer(int sampleRate = 44100, int fftSize = 4096)
        {
            this.sampleRate = sampleRate;
            this.fftSize = fftSize;
        }

        public List<(double Frequency, double Magnitude)> GetDominantFrequencies(float[] samples)
        {
            // Zero padding if needed
            Complex[] input = new Complex[fftSize];
            for (int i = 0; i < samples.Length && i < fftSize; i++)
            {
                input[i] = new Complex(samples[i], 0);
            }

            // Apply Hanning window
            for (int i = 0; i < fftSize; i++)
            {
                double window = 0.5 * (1 - Math.Cos(2 * Math.PI * i / (fftSize - 1)));
                input[i] *= window;
            }

            // Perform FFT
            Complex[] fftResult = FastFurier.Transform(input);

            // Calculate power spectrum
            double[] powerSpectrum = FastFurier.PowerSpectrum(fftResult);

            // Find top 10 frequencies
            var frequencies = new List<(double Frequency, double Magnitude)>();
            for (int i = 1; i < powerSpectrum.Length / 2; i++) // Skip DC and Nyquist
            {
                double freq = i * (double)sampleRate / fftSize;
                frequencies.Add((freq, powerSpectrum[i]));
            }

            // Get top 10 by magnitude
            var top10 = frequencies.OrderByDescending(f => f.Magnitude)
                                  .Take(10)
                                  .OrderBy(f => f.Frequency)
                                  .ToList();

            // Normalize magnitudes (0-1)
            double maxMagnitude = top10.Max(f => f.Magnitude);
            if (maxMagnitude > 0)
            {
                for (int i = 0; i < top10.Count; i++)
                {
                    top10[i] = (top10[i].Frequency, top10[i].Magnitude / maxMagnitude);
                }
            }

            return top10;
        }
    }
}
