using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Score.Clients.Models;
using Serilog;

namespace Score.Clients.Clients
{
    public class ScoreClient : IScoreClient
    {
        private readonly ILogger _logger;
        public const string FileName = "../../../../../data.json"; // Add to config

        public ScoreClient(ILogger logger)
        {
            _logger = logger;
        }
        public async Task<(IEnumerable<ScoreRecord> data, bool success)> GetScores(int score)
        {
            try
            {
                await using FileStream openStream = File.OpenRead(FileName);

                var data = await JsonSerializer.DeserializeAsync<List<ScoreRecord>>(openStream);

                var scores = data.Where(d => d.Score > score);

                return (scores, true);
            }

            catch (Exception e)
            {
                _logger.Error($"Issue with reading file {FileName}. {e.Message}");
                return (null, false);
            }

        }

        public async Task<(ResponseCode responseCode, bool success)> UpdateScore(string player, int score)
        {
            try
            {
                var options = new JsonSerializerOptions
                {
                    WriteIndented = true,
                };

                var jsonString = File.ReadAllText(FileName);
                var data = JsonSerializer.Deserialize<List<ScoreRecord>>(jsonString);
                
                var scores = (from s in data
                              where s.Player == player
                              select s).ToList();

                if (!scores.Any())
                {
                    return (ResponseCode.NotFound, false);
                }

                foreach (var scoreRecord in scores)
                {
                    scoreRecord.Score = score;
                }

                jsonString = JsonSerializer.Serialize(data, options);
                File.WriteAllText(FileName, jsonString);

                return (ResponseCode.Success, true);
            }

            catch (Exception e)
            {
                _logger.Error($"Issue with writing file {FileName}. {e.Message}");
                return (ResponseCode.Error, false);
            }
        }
    }
}