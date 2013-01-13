﻿using System;

class Program
{
    // Base10ToBase2Integer(5) -> 101
    static string Base10ToBase2Integer(int d)
    {
        string b = String.Empty;

        for (; d != 0; d /= 2) b = d % 2 + b;

        return b;
    }

    // Base10ToBase2Integer(5, 8) -> 00000101
    static string Base10ToBase2Integer(int d, int size)
    {
        string b = String.Empty;

        for (int i = 0; i < size; i++) b = (d >> i & 1) + b;

        return b;
    }

    // Base10ToBase2Fraction(.125) -> 001; 0.125 = 1 / 8 = 1 / (2 ^ 3) = 0.001 binary
    static string Base10ToBase2Fraction(float f)
    {
        string b = String.Empty;

        for (f *= 2; f != 0; f *= 2)
        {
            if (f >= 1)
            {
                b += '1';
                f--;
            }
            else b += '0';
        }

        return b;
    }

    // Sign is 1 bit long
    static int GetSign(float f)
    {
        return f < 0 ? 1 : 0;
    }

    // Exponent is the next 8 bits
    static string GetExponent(string integer, string fraction)
    {
        // 1 -> 0; 2 -> 1; 3 -> 1; 4 -> 2; 5 -> 2; 6 -> 2; 7 -> 2; 8 -> 3; 9 -> 3; ... 15 -> 3; 16 -> 4 ...
        // 0.8 -> -1; 0.4 -> -2; 0.2 -> -3; 0.1 -> -4
        int power;

        if (integer.Length != 0) power = integer.Length - 1; // 8 is 2 ^ 3 in binary is 1000
        else power = -fraction.IndexOf('1') - 1; // No integer part - get first non-zero in fraction e.g. 0.0001

        return Base10ToBase2Integer(127 + power, 8); // Convert power to binary
    }

    // Mantissa is the last 23 bits
    static string GetMantissa(string integer, string fraction)
    {
        string b;

        if (integer.Length != 0) b = integer.Substring(1) + fraction; // First can't be 0 so it must be 1 (no leading zeros)
        else b = fraction.Substring(fraction.IndexOf('1') + 1); // No integer part - get first non-zero in fraction

        return b.PadRight(23, '0');
    }

    static void Main(string[] args)
    {
        float f = -27.25F; // 32 bits = 1 + 8 + 23 with 24 bits of precision in mantissa

        if (f == 0) return; // TODO: Print 0

        Console.WriteLine(GetSign(f));

        f = Math.Abs(f); // If the number is negative make it positive for easier calculations
        string integer = Base10ToBase2Integer((int)f); // 123.456 -> 123
        string fraction = Base10ToBase2Fraction(f - (int)f); // 123.456 -> .456

        Console.WriteLine(GetExponent(integer, fraction));
        Console.WriteLine(GetMantissa(integer, fraction));
    }
}