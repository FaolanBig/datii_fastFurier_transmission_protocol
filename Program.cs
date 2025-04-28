using System.IO.Enumeration;

namespace datii_fastFurier_transmission_protocol
{
    internal class Program
    {
        static void Main(string[] args)
        {
            if (args.Length != 2)
            {
                Console.WriteLine("error: wrong input");
                Console.WriteLine("usage: datii send file_to_send");
                Console.WriteLine("       datii receive folder_to_receive_to");
                Environment.Exit(1);
            }
            if (args[0] == "send")
            {
                string fileToSend = args[1];
                if (!File.Exists(fileToSend))
                {
                    Console.WriteLine("error: file does not exist");
                    Environment.Exit(1);
                }
            }
            else if (args[0] == "receive")
            {
                string folderToReceiveTo = args[1];
                if (!Directory.Exists(folderToReceiveTo))
                {
                    Console.WriteLine("error: folder does not exist");
                    Console.WriteLine($"create directory {folderToReceiveTo}? (y/n)");
                    if (Console.ReadKey().Key == ConsoleKey.Y)
                    {
                        Directory.CreateDirectory(folderToReceiveTo);
                    }
                    else
                    {
                        Console.WriteLine("abort");
                        Environment.Exit(1);
                    }
                }
            }
            else
            {
                Console.WriteLine("error: wrong input");
                Console.WriteLine("usage: datii send file_to_send");
                Console.WriteLine("       datii receive folder_to_receive_to");
                Environment.Exit(1);
            }
        }
    }
}
