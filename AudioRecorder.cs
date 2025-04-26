// This code is part of the datii_fastFurier_transmission_protocol project.

using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace datii_fastFurier_transmission_protocol
{
    internal class AudioRecorder : IDisposable
    {
        private WaveInEvent waveIn;
        private List<float> audioBuffer = new List<float>();
        private bool isRecording = false;

        public int SampleRate { get; private set; } = 44100;
        public int BufferMilliseconds { get; private set; } = 100;
        public int Bits { get; private set; } = 16;
        public int Channels { get; private set; } = 1;

        public event Action<byte[]> DataAvailable;

        public AudioRecorder()
        {
            waveIn = new WaveInEvent();
            waveIn.WaveFormat = new WaveFormat(SampleRate, Bits, Channels);
            waveIn.BufferMilliseconds = BufferMilliseconds;
            waveIn.DataAvailable += OnDataAvailable;
        }

        public void StartRecording()
        {
            if (!isRecording)
            {
                audioBuffer.Clear();
                waveIn.StartRecording();
                isRecording = true;
            }
        }

        public void StopRecording()
        {
            if (isRecording)
            {
                waveIn.StopRecording();
                isRecording = false;
            }
        }

        public float[] GetAudioSamples(int durationMilliseconds)
        {
            int samplesNeeded = (int)(SampleRate * durationMilliseconds / 1000);
            while (audioBuffer.Count < samplesNeeded)
            {
                System.Threading.Thread.Sleep(10);
            }

            lock (audioBuffer)
            {
                var samples = audioBuffer.Take(samplesNeeded).ToArray();
                audioBuffer.RemoveRange(0, samplesNeeded);
                return samples;
            }
        }

        private void OnDataAvailable(object sender, WaveInEventArgs e)
        {
            DataAvailable?.Invoke(e.Buffer);

            // Convert byte[] to float[]
            lock (audioBuffer)
            {
                for (int i = 0; i < e.BytesRecorded; i += 2)
                {
                    short sample = BitConverter.ToInt16(e.Buffer, i);
                    audioBuffer.Add(sample / 32768f);
                }
            }
        }

        public void Dispose()
        {
            waveIn.Dispose();
        }
    }
}
