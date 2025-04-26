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
        private static readonly int BaseFrequency = 1000;
        private static readonly int StepFrequency = 200;
        private const double Tolerance = 50.0; // Hz

        public static string DecodeFrequencies(List<double> frequencies)
        {
            var chars = new List<char>();
            var currentCharFrequencies = new List<double>();

            foreach (var freq in frequencies)
            {
                if (Math.Abs(freq - (BaseFrequency + 8 * StepFrequency)) < Tolerance)
                {
                    byte value = 0;
                    foreach (var f in currentCharFrequencies)
                    {
                        for (int i = 0; i < 8; i++)
                        {
                            if (Math.Abs(f - (BaseFrequency + i * StepFrequency)) < Tolerance)
                            {
                                value |= (byte)(1 << i);
                            }
                        }
                    }
                    chars.Add((char)value);
                    currentCharFrequencies.Clear();
                }
                else
                {
                    currentCharFrequencies.Add(freq);
                }
            }

            return new string(chars.ToArray());
        }
    }
}
