using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace DynamicCommandBinding.Models
{
    class Commands
    {
        [JsonPropertyName("Commands")]
        public List<CommandModel> CommandsList { get; set; }

        public static Commands GetModel(string filePath)
        {
            byte[] bytes = File.ReadAllBytes(filePath);
            return JsonSerializer.Deserialize<Commands>(GetReadOnlySpan(bytes));
        }

        private static ReadOnlySpan<T> GetReadOnlySpan<T>(T[] t) => new ReadOnlySpan<T>(t);
    }
}
