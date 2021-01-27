using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Score.Domain.Commands;
using Score.Domain.Models;
using Score.Domain.Queries;
using Score.Domain.Results;
using Score.Domain.Services;

namespace Score.Domain.Handlers
{
    public class UpdateScoreHandler : IRequestHandler<UpdateScoreCommand, UpdateScoreResult>
    {
        private readonly IScoreService _scoreService;

        public UpdateScoreHandler(IScoreService scoreService)
        {
            _scoreService = scoreService;
        }
        public async Task<UpdateScoreResult> Handle(UpdateScoreCommand command, CancellationToken cancellationToken)
        {
            var result = await _scoreService.UpdateScore(command.Player, command.Score);

            return result;
        }
    }
}
