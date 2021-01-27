using System.Collections.Generic;
using System.Threading.Tasks;
using Score.Clients.Models;

namespace Score.Clients.Clients
{
    public interface IScoreClient
    {
        Task<(IEnumerable<ScoreRecord> data, bool success)> GetScores(int score);
        Task<(ResponseCode responseCode, bool success)> UpdateScore(string player, int score);
    }
}
