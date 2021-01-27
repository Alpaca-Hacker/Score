using System.Collections.Generic;
using Score.Domain.Models;

namespace Score.Domain.Results
{
    public class GetScoresResult
    {
        public bool Success { get; set; }
        public IEnumerable<ScoreRecord> Scores { get; set; }
    }
}
