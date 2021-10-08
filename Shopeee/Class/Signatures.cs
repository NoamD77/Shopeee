using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shopeee.Class
{
    public class Signatures
    {
        public static readonly Dictionary<string, List<byte[]>> _fileSignature = new Dictionary<string, List<byte[]>>
        {
            { ".jpeg", new List<byte[]>
                {
                    new byte[] { 0xFF, 0xD8, 0xFF, 0xE0 },
                    new byte[] { 0xFF, 0xD8, 0xFF, 0xE2 },
                    new byte[] { 0xFF, 0xD8, 0xFF, 0xE3 },
                }
            },

            { ".jpg", new List<byte[]>
                {
                    new byte[] { 0xFF, 0xD8, 0xFF, 0xE0 },
                    new byte[] { 0xFF, 0xD8, 0xFF, 0xE1 },
                    new byte[] { 0xFF, 0xD8, 0xFF, 0xE8 },
                }
            },

            { ".bmp", new List<byte[]>
                {
                    new byte[] { 0x42, 0x4D },
                }
            },

            { ".png", new List<byte[]>
                {
                    new byte[] { 0x89, 0x50, 0x4E, 0x47, 0x0D, 0x0A, 0x1A, 0x0A },
                }
            },

            { ".webp", new List<byte[]>
                {
                    new byte[] { 0x52, 0x49, 0x46, 0x46 },
                    new byte[] { 0x57, 0x45, 0x42, 0x50 },
                }
            },
        };
    }
}
