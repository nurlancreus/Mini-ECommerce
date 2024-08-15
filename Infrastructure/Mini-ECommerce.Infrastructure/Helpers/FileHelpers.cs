﻿using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mini_ECommerce.Infrastructure.Helpers
{
    internal static class FileHelpers
    {
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

        public static async Task<string> RenameFileAsync(string path, string fileName, string webRootPath)
        {
            string oldName = Path.GetFileNameWithoutExtension(fileName);
            string extension = Path.GetExtension(fileName);
            string newFileName = $"{CharacterRegulatory(oldName)}{extension}";

            string newFilePath = Path.Combine(path, newFileName);

            int counter = 1;

            while (File.Exists(Path.Combine(webRootPath, newFilePath)))
            {
                newFileName = $"{CharacterRegulatory(oldName)}-{++counter}{extension}";
                newFilePath = Path.Combine(path, newFileName);
            }

            return await Task.FromResult(newFileName);
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
