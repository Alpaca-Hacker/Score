
using MediatR;
using Score.Domain.Results;

namespace Score.Domain.Commands
{
    public class UpdateScoreCommand :  IRequest<UpdateScoreResult>
    {
        public string Player { get; set; }
        public int Score { get; set; }
    }
}
