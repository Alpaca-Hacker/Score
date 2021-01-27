using System.Threading.Tasks;
using Score.Domain.Results;

namespace Score.Domain.Services
{
    public interface IScoreService
    {
        Task<GetScoresResult> GetScores(int score);
        Task<UpdateScoreResult> UpdateScore(string player, int score);
    }
}
