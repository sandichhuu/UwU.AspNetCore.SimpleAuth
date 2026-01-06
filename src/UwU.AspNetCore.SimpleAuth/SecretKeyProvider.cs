using System;

namespace UwU.AspNetCore.SimpleAuth
{
    public class SecretKeyProvider
    {
        public readonly byte[] data;

        private readonly CompareSafeDelegate _comparerator;
        
        public SecretKeyProvider(string expected, CompareSafeDelegate comparerator)
        {
            this.data = System.Text.Encoding.UTF8.GetBytes(expected);
            this._comparerator = comparerator;
        }

        public bool Compare(ReadOnlySpan<byte> input)
        {
            return this._comparerator(input, this.data);
        }

        public delegate bool CompareSafeDelegate(ReadOnlySpan<byte> a, ReadOnlySpan<byte> b);
    }
}