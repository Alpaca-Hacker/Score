using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Score.Clients.Clients;
using Score.Clients.Models;

namespace FunctionalTests.Mocks
{
    public class MockScoreClient : IScoreClient
    {
        public List<ScoreRecord> Data { get; set; }
        public bool ThrowError { get; set; }
        public MockScoreClient()
        {
            Data = new List<ScoreRecord>
            {
                new ScoreRecord
                {
                    Player = "Joe",
                    Score = 3
                },
                new ScoreRecord
                {
                    Player = "Jane",
                    Score = 1
                },
                new ScoreRecord
                {
                    Player = "Jonny",
                    Score = 5
                },
                new ScoreRecord
                {
                    Player = "Jeremy",
                    Score = 2
                },
            };
        }

        public async Task<(IEnumerable<ScoreRecord> data, bool success)> GetScores(int score)
        {
            return ThrowError ? (null, false) : (Data.Where(d => d.Score > score), true);
        }

        public async Task<(ResponseCode responseCode, bool success)> UpdateScore(string player, int score)
        {
            if (ThrowError)
            {
                return (ResponseCode.Error, false);
            }

            if (Data.All(d => d.Player != player))
            {
                return (ResponseCode.NotFound, false);
            }

            return (ResponseCode.Success, true);
        }
    }
}
