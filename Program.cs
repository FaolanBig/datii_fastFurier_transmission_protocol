using System.IO.Enumeration;
using CommandLine;

namespace datii_fastFurier_transmission_protocol
{
    internal class Program
    {
        static void Main(string[] args)
        {
            if (args[0] == "encode")
            {
                Encoder encodeObj = new Encoder();
                encodeObj.filename = args[1];
            }
            else if (args[0] == "decode")
            {
                Decoder decodeObj = new Decoder();
                decodeObj.filename = args[1];
            }
        }
    }
}
