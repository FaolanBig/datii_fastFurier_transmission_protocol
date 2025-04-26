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
        public static void PlayFrequencies(List<double> frequencies)
        {
            var sampleRate = 44100;
            var waveOut = new WaveOutEvent();
            var mixer = new MixingSampleProvider(WaveFormat.CreateIeeeFloatWaveFormat(sampleRate, 1));

            foreach (var freq in frequencies)
            {
                var sineWave = new SignalGenerator(sampleRate, 1)
                {
                    Gain = 0.2,
                    Frequency = freq,
                    Type = SignalGeneratorType.Sin
                }.Take(TimeSpan.FromMilliseconds(100)); // Play each tone for 100ms

                mixer.AddMixerInput(sineWave);
            }

            waveOut.Init(mixer);
            waveOut.Play();

            while (waveOut.PlaybackState == PlaybackState.Playing)
            {
                System.Threading.Thread.Sleep(100);
            }
        }
    }
}
