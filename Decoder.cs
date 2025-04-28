// This code is part of the datii_fastFurier_transmission_protocol project.

/*  datii (datii_fastFurier_transmission_protocol) lets you transmit files via sound from one computer to another.

    This program is free software: you can redistribute it and/or modify
    it under the terms of the GNU Affero General Public License as published
    by the Free Software Foundation, either version 3 of the License, or
    any later version.

    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU Affero General Public License for more details.

    You should have received a copy of the GNU Affero General Public License
    along with this program.  If not, see <https://www.gnu.org/licenses/>.

    You can contact me via
        email: faolan.big@web.de
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace datii_fastFurier_transmission_protocol
{
    internal class Decoder
    {
        private readonly double[] Frequencies = { 1000, 2000, 3000, 4000, 5000, 6000, 7000, 8000 };
        private readonly AudioRecorder recorder = new AudioRecorder();
        private readonly FFT fft = new FFT();
        private readonly FrequencyAnalyzer analyzer = new FrequencyAnalyzer();

        public void ReceiveFile(string folderPath)
        {
            Console.WriteLine("Recording...");
            recorder.StartRecording();
            System.Threading.Thread.Sleep(10000); // Aufnahmezeit: 10 Sekunden
            byte[] rawData = recorder.StopRecording();
            Console.WriteLine("Recording finished.");

            // Umwandeln in Float-Daten
            float[] floatData = new float[rawData.Length / 2];
            for (int i = 0; i < floatData.Length; i++)
            {
                floatData[i] = BitConverter.ToInt16(rawData, i * 2) / 32768f;
            }

            List<byte> receivedBytes = new List<byte>();
            int chunkSize = 44100 / 10; // 100ms Chunks

            for (int i = 0; i < floatData.Length; i += chunkSize)
            {
                float[] chunk = floatData.Skip(i).Take(chunkSize).ToArray();
                if (chunk.Length < chunkSize)
                    break;

                var fftResult = fft.PerformFFT(chunk);
                double freq = analyzer.DetectDominantFrequency(fftResult, 44100);

                byte b = 0;
                for (int j = 0; j < Frequencies.Length; j++)
                {
                    if (Math.Abs(freq - Frequencies[j]) < 100) // Frequenz innerhalb 100Hz?
                    {
                        b |= (byte)(1 << j);
                    }
                }
                receivedBytes.Add(b);
            }

            Directory.CreateDirectory(folderPath);
            string filePath = Path.Combine(folderPath, $"received_{DateTime.Now:yyyyMMddHHmmss}.bin");
            File.WriteAllBytes(filePath, receivedBytes.ToArray());
            Console.WriteLine($"File saved to {filePath}");
        }
    }
}
