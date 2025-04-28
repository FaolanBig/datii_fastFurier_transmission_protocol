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
                Console.WriteLine("Usage: program.exe [send|receive] [file_or_folder]");
                return;
            }

            string mode = args[0];
            string path = args[1];

            if (mode == "send")
            {
                var encoder = new Encoder();
                encoder.SendFile(path);
            }
            else if (mode == "receive")
            {
                var decoder = new Decoder();
                decoder.ReceiveFile(path);
            }
            else
            {
                Console.WriteLine("Invalid mode. Use 'send' or 'receive'.");
            }
        }
    }
}
