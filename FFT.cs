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
using MathNet.Numerics.IntegralTransforms;



namespace datii_fastFurier_transmission_protocol
{
    internal class FFT
    {
        public Complex[] PerformFFT(float[] audioData)
        {
            Complex[] buffer = new Complex[audioData.Length];
            for (int i = 0; i < audioData.Length; i++)
                buffer[i] = new Complex(audioData[i], 0);

            Fourier.Forward(buffer, FourierOptions.Matlab);
            return buffer;
        }
    }
}
