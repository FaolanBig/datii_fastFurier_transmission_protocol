// This code is part of the datii_fastFurier_transmission_protocol project.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace datii_fastFurier_transmission_protocol
{
    internal class Decoder
    {
        private readonly int sampleRate = 44100;
        private readonly int durationPerCharMs = 500;
        private readonly int silenceDurationMs = 100;
        private readonly double minFrequency = 1000;
        private readonly double maxFrequency = 4000;
        private readonly int frequenciesCount = 10;
        private readonly double detectionThreshold = 0.7;

        public string Receive()
        {
            var frequencyMap = GenerateFrequencyMap();
            var recorder = new AudioRecorder();
            var analyzer = new FrequencyAnalyzer(sampleRate);

            Console.WriteLine("Listening for transmission...");
            recorder.StartRecording();

            StringBuilder binaryData = new StringBuilder();
            bool receiving = false;
            DateTime lastSignalTime = DateTime.MinValue;

            while (true)
            {
                // Get samples for the duration of one bit
                float[] samples = recorder.GetAudioSamples(durationPerCharMs);

                // Analyze frequencies
                var dominantFreqs = analyzer.GetDominantFrequencies(samples);

                // Check if we're detecting our frequency pattern
                bool isSignal = IsSignalPresent(dominantFreqs, frequencyMap);

                if (isSignal)
                {
                    if (!receiving)
                    {
                        receiving = true;
                        Console.WriteLine("Signal detected, receiving data...");
                    }

                    binaryData.Append('1');
                    lastSignalTime = DateTime.Now;
                }
                else if (receiving)
                {
                    // Only record '0' if we're in receiving mode
                    binaryData.Append('0');
                }

                // Check for end of transmission (2 seconds of silence)
                if (receiving && (DateTime.Now - lastSignalTime).TotalSeconds > 2)
                {
                    Console.WriteLine("End of transmission detected");
                    break;
                }
            }

            recorder.StopRecording();

            // Convert binary to string
            string text = BinaryToString(binaryData.ToString());
            Console.WriteLine($"Received {text.Length} characters");
            return text;
        }

        private double[] GenerateFrequencyMap()
        {
            double[] map = new double[frequenciesCount];
            for (int i = 0; i < frequenciesCount; i++)
            {
                map[i] = minFrequency + i * (maxFrequency - minFrequency) / (frequenciesCount - 1);
            }
            return map;
        }

        private bool IsSignalPresent(List<(double Frequency, double Magnitude)> dominantFreqs, double[] frequencyMap)
        {
            // Check if at least 80% of our frequencies are present with sufficient magnitude
            int detectedCount = 0;

            foreach (var targetFreq in frequencyMap)
            {
                foreach (var (freq, mag) in dominantFreqs)
                {
                    if (Math.Abs(freq - targetFreq) < 20 && mag > detectionThreshold) // 20Hz tolerance
                    {
                        detectedCount++;
                        break;
                    }
                }
            }

            return detectedCount >= frequencyMap.Length * 0.8;
        }

        private string BinaryToString(string binary)
        {
            StringBuilder text = new StringBuilder();

            for (int i = 0; i < binary.Length; i += 8)
            {
                if (i + 8 > binary.Length) break;

                string byteString = binary.Substring(i, 8);
                char c = (char)Convert.ToByte(byteString, 2);
                text.Append(c);
            }

            return text.ToString();
        }
    }
}
