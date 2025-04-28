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

using NAudio.Wave.SampleProviders;
using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace datii_fastFurier_transmission_protocol
{
    internal class AudioPlayer
    {
        public void PlayTone(double frequency, int durationMs, int sampleRate = 44100)
        {
            int samples = (sampleRate * durationMs) / 1000;
            var buffer = new float[samples];

            for (int i = 0; i < samples; i++)
            {
                buffer[i] = (float)Math.Sin(2 * Math.PI * frequency * i / sampleRate);
            }

            var waveFormat = WaveFormat.CreateIeeeFloatWaveFormat(sampleRate, 1);
            var waveOut = new WaveOutEvent();
            var provider = new BufferedWaveProvider(waveFormat);

            byte[] byteBuffer = new byte[buffer.Length * sizeof(float)];
            Buffer.BlockCopy(buffer, 0, byteBuffer, 0, byteBuffer.Length);

            provider.AddSamples(byteBuffer, 0, byteBuffer.Length);
            waveOut.Init(provider);
            waveOut.Play();

            while (waveOut.PlaybackState == PlaybackState.Playing)
            {
                System.Threading.Thread.Sleep(10);
            }

            waveOut.Dispose();
        }
    }
}
