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
using System.IO;
using System.Linq;



namespace datii_fastFurier_transmission_protocol
{
    internal class Program
    {
        static void Main(string[] args)
        {
            if (args.Length != 2)
            {
                Console.WriteLine("Usage:");
                Console.WriteLine("  program.exe send <file_to_send>");
                Console.WriteLine("  program.exe receive <folder_to_receive_to>");
                return;
            }

            string mode = args[0];
            string path = args[1];

            if (mode == "send")
            {
                if (!File.Exists(path))
                {
                    Console.WriteLine($"File not found: {path}");
                    return;
                }

                string text = File.ReadAllText(path);
                var frequencies = Encoder.EncodeText(text);

                Console.WriteLine("Sending file...");
                AudioPlayer.PlayFrequencies(frequencies);
                Console.WriteLine("File sent.");
            }
            else if (mode == "receive")
            {
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                Console.WriteLine("Recording audio...");
                var samples = AudioRecorder.Record(5.0); // record for 5 seconds

                Console.WriteLine("Analyzing frequencies...");
                var fftResult = FastFurier.Transform(samples);
                var detectedFrequencies = FrequencyAnalyzer.GetTopFrequencies(fftResult, 44100, 100)
                                                            .Select(x => x.frequency)
                                                            .ToList();

                Console.WriteLine("Decoding text...");
                string decodedText = Decoder.DecodeFrequencies(detectedFrequencies);

                string outputFile = Path.Combine(path, $"received_{DateTime.Now:yyyyMMdd_HHmmss}.txt");
                File.WriteAllText(outputFile, decodedText);

                Console.WriteLine($"Received text saved to: {outputFile}");
            }
            else
            {
                Console.WriteLine($"Unknown mode: {mode}");
            }
        }
    }
}
