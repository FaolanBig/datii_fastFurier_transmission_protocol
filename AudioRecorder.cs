// This code is part of the datii_fastFurier_transmission_protocol project.

using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace datii_fastFurier_transmission_protocol
{
    internal class AudioRecorder
        // This class is responsible for recording audio from the microphone.
        // It uses NAudio library to handle audio input.
        // The recorded audio is returned as an array of floats representing the samples.
    {
        public static float[] Record(double durationSeconds)
        {
            int sampleRate = 44100;
            int channels = 1;
            int bufferSize = (int)(sampleRate * durationSeconds);
            var buffer = new float[bufferSize];

            using (var waveIn = new WaveInEvent())
            {
                waveIn.WaveFormat = new WaveFormat(sampleRate, channels);
                int offset = 0;

                waveIn.DataAvailable += (s, e) =>
                {
                    for (int index = 0; index < e.BytesRecorded; index += 2)
                    {
                        if (offset >= buffer.Length) break;
                        short sample = (short)(e.Buffer[index] | (e.Buffer[index + 1] << 8));
                        buffer[offset++] = sample / 32768f;
                    }
                };

                waveIn.StartRecording();
                System.Threading.Thread.Sleep((int)(durationSeconds * 1000));
                waveIn.StopRecording();
            }

            return buffer;
        }
    }
}
