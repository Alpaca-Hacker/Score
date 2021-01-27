using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Score.Clients.Clients;
using Score.Clients.Models;
using Score.Domain.Results;
using ScoreRecord = Score.Domain.Models.ScoreRecord;

namespace Score.Domain.Services
{
    public class ScoreService : IScoreService
    {
        private readonly IScoreClient _scoreClient;
        private readonly IMapper _mapper;

        public ScoreService(IScoreClient scoreClient, IMapper mapper)
        {
            _scoreClient = scoreClient;
            _mapper = mapper;
        }

        public async Task<GetScoresResult> GetScores(int score)
        {
            var result = await _scoreClient.GetScores(score);

            var scores = _mapper.Map<List<ScoreRecord>>(result.data);

            return new GetScoresResult
            {
                Success = result.success,
                Scores = scores
            };
        }

        public async Task<UpdateScoreResult> UpdateScore(string player, int score)
        {
            var result = await _scoreClient.UpdateScore(player, score);

            if (!result.success)
            {
                var responseCode = result.responseCode == ResponseCode.NotFound ? 404 : 500;
                
                return new UpdateScoreResult
                {
                    Success = false,
                    ResponseCode = responseCode
                };
            }

            return new UpdateScoreResult
            {
                Success = true
            };
        }
    }
}