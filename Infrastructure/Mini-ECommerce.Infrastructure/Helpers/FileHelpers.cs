using Microsoft.AspNetCore.Hosting;
using Mini_ECommerce.Application.Abstractions.Services.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mini_ECommerce.Infrastructure.Helpers
{
    internal static class FileHelpers
    {
        public delegate Task<bool> HasFileAsync (string path, string fileName);
        private static readonly Dictionary<string, string> CharacterReplacements = new()
        {
            {"\"", ""}, {"!", ""}, {"'", ""}, {"^", ""}, {"+", ""}, {"%", ""},
            {"&", ""}, {"/", ""}, {"(", ""}, {")", ""}, {"=", ""}, {"?", ""},
            {"_", ""}, {" ", "-"}, {"@", ""}, {"€", ""}, {"¨", ""}, {"~", ""},
            {",", ""}, {";", ""}, {":", ""}, {".", "-"}, {"Ö", "o"}, {"ö", "o"},
            {"Ü", "u"}, {"ü", "u"}, {"ı", "i"}, {"İ", "i"}, {"ğ", "g"}, {"Ğ", "g"},
            {"æ", ""}, {"ß", ""}, {"â", "a"}, {"î", "i"}, {"ş", "s"}, {"Ş", "s"},
            {"Ç", "c"}, {"ç", "c"}, {"<", ""}, {">", ""}, {"|", ""}
        };

        public static string CharacterRegulatory(string name)
        {
            foreach (var replacement in CharacterReplacements)
            {
                name = name.Replace(replacement.Key, replacement.Value);
            }
            return name;
        }

        public static async Task<string> RenameFileAsync(string path, string fileName, HasFileAsync hasFileAsync)
        {
            string oldName = Path.GetFileNameWithoutExtension(fileName);
            string extension = Path.GetExtension(fileName);
            string newFileName = $"{CharacterRegulatory(oldName)}{extension}";

            int counter = 1;
            while (await hasFileAsync(path, newFileName))
            {
                newFileName = $"{CharacterRegulatory(oldName)}-{counter++}{extension}";
            }

            return newFileName;
        }

        public static async Task EnsureDirectoryExists(string path)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
                await Task.CompletedTask; // Simulating async for consistency

            }
        }

        public static async Task CleanupFailedUploads(List<(string fileName, string path)> uploadedFiles)
        {
            foreach (var (_, path) in uploadedFiles)
            {
                if (File.Exists(path))
                {
                    File.Delete(path);

                    await Task.CompletedTask; // Simulating async for consistency
                }
            }
        }

        public static Task<string> GenerateUniqueFileNameAsync(string fileName)
        {
            Path.GetRandomFileName();
            string uniqueFileName = $"{Path.GetFileNameWithoutExtension(fileName)}_{Guid.NewGuid()}{Path.GetExtension(fileName)}";
            return Task.FromResult(uniqueFileName);
        }
    }
}
