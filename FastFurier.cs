// This code is part of the datii_fastFurier_transmission_protocol project.

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
        public static Complex[] Transform(Complex[] input)
        {
            int n = input.Length;
            if (n == 1)
                return new Complex[] { input[0] };

            // Check if power of 2
            if ((n & (n - 1)) != 0)
                throw new ArgumentException("Input length must be a power of 2");

            // Even/odd separation
            Complex[] even = new Complex[n / 2];
            Complex[] odd = new Complex[n / 2];

            for (int i = 0; i < n / 2; i++)
            {
                even[i] = input[2 * i];
                odd[i] = input[2 * i + 1];
            }

            // Recursion
            Complex[] q = Transform(even);
            Complex[] r = Transform(odd);

            // Combine results
            Complex[] output = new Complex[n];
            for (int k = 0; k < n / 2; k++)
            {
                double kth = -2 * k * Math.PI / n;
                Complex wk = new Complex(Math.Cos(kth), Math.Sin(kth));
                output[k] = q[k] + wk * r[k];
                output[k + n / 2] = q[k] - wk * r[k];
            }

            return output;
        }

        // Compute power spectrum (magnitude squared)
        public static double[] PowerSpectrum(Complex[] fftOutput)
        {
            double[] power = new double[fftOutput.Length / 2];
            for (int i = 0; i < power.Length; i++)
            {
                power[i] = fftOutput[i].Magnitude * fftOutput[i].Magnitude;
            }
            return power;
        }

        // Find the closest power of 2 for zero padding
        public static int NextPowerOfTwo(int n)
        {
            int power = 1;
            while (power < n)
            {
                power <<= 1;
            }
            return power;
        }
    }
}
