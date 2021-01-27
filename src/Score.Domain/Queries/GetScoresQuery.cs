using MediatR;
using Score.Domain.Logging;
using Score.Domain.Results;

namespace Score.Domain.Queries
{
    public class GetScoresQuery: IRequest<GetScoresResult>, IExposeLoggingInfo
    {
        public int Score { get; set; }
        public object GetLoggingInfo()
        {
            return new
            {
                Score
            };
        }
    }
}
