// This code is part of the datii_fastFurier_transmission_protocol project.

using System;
using System.IO;
using System.Linq;



namespace datii_fastFurier_transmission_protocol
{
    internal class Program
    {
        static int Main(string[] args)
        {
            if (args.Length < 2)
            {
                Console.WriteLine("Usage:");
                Console.WriteLine("  To send:    program.exe send file_to_send");
                Console.WriteLine("  To receive: program.exe receive folder_to_receive_to");
                return 1;
            }

            string mode = args[0].ToLower();

            try
            {
                if (mode == "send")
                {
                    if (args.Length < 2)
                    {
                        Console.WriteLine("Please specify file to send");
                        return 1;
                    }

                    string filePath = args[1];
                    if (!File.Exists(filePath))
                    {
                        Console.WriteLine($"File not found: {filePath}");
                        return 1;
                    }

                    string text = File.ReadAllText(filePath);
                    Encoder encoder = new Encoder();
                    encoder.Transmit(text);
                }
                else if (mode == "receive")
                {
                    string folderPath = args.Length > 1 ? args[1] : ".";
                    if (!Directory.Exists(folderPath))
                    {
                        Directory.CreateDirectory(folderPath);
                    }

                    Decoder decoder = new Decoder();
                    string receivedText = decoder.Receive();

                    string filePath = Path.Combine(folderPath, $"received_{DateTime.Now:yyyyMMdd_HHmmss}.txt");
                    File.WriteAllText(filePath, receivedText);
                    Console.WriteLine($"File saved to: {filePath}");
                }
                else
                {
                    Console.WriteLine("Invalid mode. Use 'send' or 'receive'");
                    return 1;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return 1;
            }

            return 0;
        }

    }
}
