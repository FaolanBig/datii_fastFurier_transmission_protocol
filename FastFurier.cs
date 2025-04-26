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
using System.Numerics;


namespace datii_fastFurier_transmission_protocol
{
    internal class FastFurier
    {
        public static Complex[] Transform(float[] samples)
        {
            int n = NextPowerOfTwo(samples.Length);
            Array.Resize(ref samples, n);

            var buffer = samples.Select(x => new Complex(x, 0)).ToArray();
            ComputeFFT(buffer);
            return buffer;
        }

        private static void ComputeFFT(Complex[] buffer)
        {
            int n = buffer.Length;
            if (n <= 1)
                return;

            var even = new Complex[n / 2];
            var odd = new Complex[n / 2];

            for (int i = 0; i < n / 2; i++)
            {
                even[i] = buffer[i * 2];
                odd[i] = buffer[i * 2 + 1];
            }

            ComputeFFT(even);
            ComputeFFT(odd);

            for (int k = 0; k < n / 2; k++)
            {
                var t = Complex.FromPolarCoordinates(1.0, -2 * Math.PI * k / n) * odd[k];
                buffer[k] = even[k] + t;
                buffer[k + n / 2] = even[k] - t;
            }
        }

        private static int NextPowerOfTwo(int x)
        {
            int power = 1;
            while (power < x) power *= 2;
            return power;
        }
    }
}
