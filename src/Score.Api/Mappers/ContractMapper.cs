using AutoMapper;
using Score.Contracts.Responses;
using Score.Domain.Results;

namespace Score.Api.Mappers
{
    public class ContractMapper : Profile
    {
        public ContractMapper()
        {
            CreateMap<GetScoresResult, GetScoresResponse>();
            CreateMap<Score.Domain.Models.ScoreRecord, Score.Contracts.Models.ScoreRecord>();
        }
    }
}
