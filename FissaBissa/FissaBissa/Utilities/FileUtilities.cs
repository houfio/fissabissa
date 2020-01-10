using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace FissaBissa.Utilities
{
    public static class FileUtilities
    {
        private static readonly Dictionary<string, List<byte[]>> _signatures = new Dictionary<string, List<byte[]>>
        {
            {".gif", new List<byte[]> {new byte[] {0x47, 0x49, 0x46, 0x38}}},
            {".png", new List<byte[]> {new byte[] {0x89, 0x50, 0x4E, 0x47, 0x0D, 0x0A, 0x1A, 0x0A}}},
            {
                ".jpeg", new List<byte[]>
                {
                    new byte[] {0xFF, 0xD8, 0xFF, 0xE0},
                    new byte[] {0xFF, 0xD8, 0xFF, 0xE2},
                    new byte[] {0xFF, 0xD8, 0xFF, 0xE3},
                }
            },
            {
                ".jpg", new List<byte[]>
                {
                    new byte[] {0xFF, 0xD8, 0xFF, 0xE0},
                    new byte[] {0xFF, 0xD8, 0xFF, 0xE1},
                    new byte[] {0xFF, 0xD8, 0xFF, 0xE8},
                }
            }
        };

        public static async Task<string> StoreImage(string name, IFormFile file, ModelStateDictionary state)
        {
            await using var memoryStream = new MemoryStream();
            await file.CopyToAsync(memoryStream);

            var extension = GetExtension(file.FileName, memoryStream);

            if (extension == null)
            {
                state.AddModelError(name,
                    $"The {name} field only accepts files with the following extensions: {string.Join(", ", _signatures.Keys)}.");
            }

            var relative = new[] {"uploads", $"{Guid.NewGuid()}{extension}"};
            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", Path.Combine(relative));

            if (state.IsValid)
            {
                await using var fileStream = File.Create(path);
                await fileStream.WriteAsync(memoryStream.ToArray());
            }

            return $"/{string.Join("/", relative)}";
        }

        private static string GetExtension(string fileName, Stream data)
        {
            if (string.IsNullOrEmpty(fileName) || data == null || data.Length == 0)
            {
                return null;
            }

            var extension = Path.GetExtension(fileName).ToLowerInvariant();

            if (string.IsNullOrEmpty(extension) || !_signatures.Keys.Contains(extension))
            {
                return null;
            }

            data.Position = 0;

            using var reader = new BinaryReader(data);

            var signature = _signatures[extension];
            var header = reader.ReadBytes(signature.Max(m => m.Length));

            return signature.Any(s => header.Take(s.Length).SequenceEqual(s)) ? extension : null;
        }
    }
}
