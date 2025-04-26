// This code is part of the datii_fastFurier_transmission_protocol project.

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
