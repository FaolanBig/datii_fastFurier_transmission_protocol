// This code is part of the datii_fastFurier_transmission_protocol project.

using NAudio.Wave.SampleProviders;
using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace datii_fastFurier_transmission_protocol
{
    internal class Encoder
    {
        private readonly int sampleRate = 44100;
        private readonly int durationPerCharMs = 500;
        private readonly int silenceDurationMs = 100;
        private readonly double minFrequency = 1000;
        private readonly double maxFrequency = 4000;
        private readonly int frequenciesCount = 10;

        public void Transmit(string text)
        {
            if (string.IsNullOrEmpty(text))
                throw new ArgumentException("Text cannot be empty");

            // Generate frequency map (10 frequencies between min and max)
            double[] frequencyMap = new double[frequenciesCount];
            for (int i = 0; i < frequenciesCount; i++)
            {
                frequencyMap[i] = minFrequency + i * (maxFrequency - minFrequency) / (frequenciesCount - 1);
            }

            // Convert text to binary
            string binary = StringToBinary(text);

            // Encode binary as tones
            using (var waveOut = new WaveOutEvent())
            {
                foreach (char bit in binary)
                {
                    if (bit == '1')
                    {
                        // Play all frequencies simultaneously for '1'
                        var signals = new List<SignalGenerator>();
                        foreach (var freq in frequencyMap)
                        {
                            signals.Add(new SignalGenerator()
                            {
                                Gain = 0.2,
                                Frequency = freq,
                                Type = SignalGeneratorType.Sin,
                                SampleRate = sampleRate
                            });
                        }

                        using (var mixer = new MixingSampleProvider(signals.Select(s => s.ToSampleProvider())))
                        {
                            waveOut.Init(mixer);
                            waveOut.Play();
                            Thread.Sleep(durationPerCharMs);
                            waveOut.Stop();
                        }
                    }
                    else
                    {
                        // Silence for '0'
                        Thread.Sleep(durationPerCharMs);
                    }

                    // Short silence between bits
                    Thread.Sleep(silenceDurationMs);
                }
            }

            Console.WriteLine($"Transmitted {text.Length} characters as sound");
        }

        private string StringToBinary(string text)
        {
            // Convert each character to 8-bit binary
            string binary = "";
            foreach (char c in text)
            {
                binary += Convert.ToString(c, 2).PadLeft(8, '0');
            }
            return binary;
        }
    }
}
