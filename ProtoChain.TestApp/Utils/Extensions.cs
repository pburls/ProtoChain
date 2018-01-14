using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ProtoChain.TestApp.Utils
{

    public static class StringArrayExtension
    {
        // This is the extension method.
        // The first parameter takes the "this" modifier
        // and specifies the type for which the method is defined.
        public static byte[] ToByteArray(this string[] stringArray)
        {
            using (var stream = new MemoryStream())
            using (var writer = new BinaryWriter(stream, Encoding.UTF8))
            {
                var rows = stringArray.GetLength(0);
                writer.Write(rows);
                for (int i = 0; i < rows; i++)
                {
                    writer.Write(stringArray[i]);
                }
                return stream.ToArray();
            }
        }

        public static string[] FromByteArray(this byte[] bytes)
        {
            using (var stream = new MemoryStream(bytes))
            using (var reader = new BinaryReader(stream, Encoding.UTF8))
            {
                var rows = reader.ReadInt32();
                var result = new string[rows];
                for (int i = 0; i < rows; i++)
                {
                    result[i] = reader.ReadString();
                }
                return result;
            }
        }
    }
}
