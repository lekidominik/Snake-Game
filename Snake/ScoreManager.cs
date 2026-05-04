using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Snake
{
    public static class ScoreManager
    {
        private static readonly string FilePath = "scores.json";

        public static List<ScoreEntry> LoadScores()
        {
            if (!File.Exists(FilePath))
            {
                return new List<ScoreEntry>();
            }

            string json = File.ReadAllText(FilePath);

            return JsonSerializer.Deserialize<List<ScoreEntry>>(json)
                   ?? new List<ScoreEntry>();
        }
        public static void SaveAll(List<ScoreEntry> scores)
        {
            string json = JsonSerializer.Serialize(scores, new JsonSerializerOptions
            {
                WriteIndented = true
            });

            File.WriteAllText(FilePath, json);
        }

        public static void SaveScore(string name, int score)
        {
            List<ScoreEntry> scores = LoadScores();

            scores.Add(new ScoreEntry
            {
                Name = name,
                Score = score,
                Date = DateTime.Now
            });

            scores = scores
                .OrderByDescending(s => s.Score)
                .Take(10)
                .ToList();

            string json = JsonSerializer.Serialize(scores, new JsonSerializerOptions
            {
                WriteIndented = true
            });

            File.WriteAllText(FilePath, json);
        }
    }
}
