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
    internal class Encoder
    {
        private readonly double[] Frequencies = { 1000, 2000, 3000, 4000, 5000, 6000, 7000, 8000 }; // 8 Frequenzen
        private readonly AudioPlayer player = new AudioPlayer();

        public void SendFile(string filePath)
        {
            byte[] data = File.ReadAllBytes(filePath);
            foreach (byte b in data)
            {
                for (int i = 0; i < 8; i++)
                {
                    if ((b & (1 << i)) != 0)
                    {
                        player.PlayTone(Frequencies[i], 100); // 100 ms pro Ton
                    }
                    else
                    {
                        System.Threading.Thread.Sleep(100); // Pause für 0-Bit
                    }
                }
            }
        }
    }
}
