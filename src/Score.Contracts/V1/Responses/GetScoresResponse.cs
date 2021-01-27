using System.Collections.Generic;
using Score.Contracts.Models;

namespace Score.Contracts.Responses
{
    public class GetScoresResponse
    {
        public IEnumerable<ScoreRecord> Scores { get; set; }
    }
}
