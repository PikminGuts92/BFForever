using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

// Custom inherited class created with big endian / little endian in mind
// Added in custom features as required
//
// Last Updated: Mar. 15, 2016

namespace BFForever
{
    public class AwesomeWriter : BinaryWriter
    {
        private bool _big;

        /// <summary>
        /// Creates AwesomeWriter stream
        /// </summary>
        /// <param name="input">Stream</param>
        public AwesomeWriter(Stream input)
            : base(input)
        {
            _big = false;
        }

        /// <summary>
        /// Creates AwesomeWriter stream and sets byte order
        /// </summary>
        /// <param name="input"></param>
        /// <param name="bigEndian"></param>
        public AwesomeWriter(Stream input, bool bigEndian)
            : base(input)
        {
            _big = bigEndian;
        }

        /// <summary>
        /// Writes 32-bit float
        /// </summary>
        /// <param name="value">Float</param>
        public override void Write(float value)
        {
            byte[] buffer = BitConverter.GetBytes(value);
            if (this._big) Array.Reverse(buffer);
            this.Write(buffer);
        }

        /// <summary>
        /// Writes 16-bit integer
        /// </summary>
        /// <param name="value">Integer</param>
        public override void Write(short value)
        {
            byte[] buffer = BitConverter.GetBytes(value);
            if (this._big) Array.Reverse(buffer);
            this.Write(buffer);
        }

        /// <summary>
        /// Writes 16-bit unsigned integer
        /// </summary>
        /// <param name="value">Integer</param>
        public override void Write(ushort value)
        {
            byte[] buffer = BitConverter.GetBytes(value);
            if (this._big) Array.Reverse(buffer);
            this.Write(buffer);
        }
        
        /// <summary>
        /// Writes 32-bit integer
        /// </summary>
        /// <param name="value">Integer</param>
        public override void Write(int value)
        {
            byte[] buffer = BitConverter.GetBytes(value);
            if (this._big) Array.Reverse(buffer);
            this.Write(buffer);
        }

        /// <summary>
        /// Writes 32-bit unsigned integer
        /// </summary>
        /// <param name="value">Integer</param>
        public override void Write(uint value)
        {
            byte[] buffer = BitConverter.GetBytes(value);
            if (this._big) Array.Reverse(buffer);
            this.Write(buffer);
        }

        /// <summary>
        /// Writes 64-bit integer
        /// </summary>
        /// <param name="value">Integer</param>
        public override void Write(long value)
        {
            byte[] buffer = BitConverter.GetBytes(value);
            if (this._big) Array.Reverse(buffer);
            this.Write(buffer);
        }

        /// <summary>
        /// Writes 64-bit unsigned integer
        /// </summary>
        /// <param name="value">Integer</param>
        public override void Write(ulong value)
        {
            byte[] buffer = BitConverter.GetBytes(value);
            if (this._big) Array.Reverse(buffer);
            this.Write(buffer);
        }

        /// <summary>
        /// Writes string with 32-bit length preceding
        /// </summary>
        /// <param name="value">String</param>
        public override void Write(string value)
        {
            byte[] str = Encoding.UTF8.GetBytes(value);
            byte[] length = BitConverter.GetBytes(str.Length);
            if (this._big) Array.Reverse(length);

            this.Write(length);
            this.Write(str);
        }

        /// <summary>
        /// Writes null-terminated string
        /// </summary>
        /// <param name="value">String</param>
        /// <param maxSize="value">Max Size</param>
        public void WriteNullString(string value, int maxSize = 0)
        {
            if (maxSize <= 0)
            {
                byte[] stringData = Encoding.UTF8.GetBytes(value);
                this.Write(stringData);
                this.Write((byte)0x00);
                return;
            }

            byte[] buffer = new byte[maxSize];
            byte[] data = Encoding.UTF8.GetBytes(value);

            Array.Copy(data, buffer, Math.Min(data.Length, maxSize));
            buffer[buffer.Length - 1] = 0x00;
            this.Write(buffer);
        }

        /// <summary>
        /// Gets or sets write byte order
        /// </summary>
        public bool BigEndian { get { return _big; } set { _big = value; } }
    }
}
