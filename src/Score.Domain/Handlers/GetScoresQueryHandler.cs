using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Score.Domain.Models;
using Score.Domain.Queries;
using Score.Domain.Results;
using Score.Domain.Services;

namespace Score.Domain.Handlers
{
    public class GetScoresQueryHandler : IRequestHandler<GetScoresQuery, GetScoresResult>
    {
        private readonly IScoreService _scoreService;

        public GetScoresQueryHandler(IScoreService scoreService)
        {
            _scoreService = scoreService;
        }
        public async Task<GetScoresResult> Handle(GetScoresQuery request, CancellationToken cancellationToken)
        {
            var result = await _scoreService.GetScores(request.Score);

            return result;
        }
    }
}
