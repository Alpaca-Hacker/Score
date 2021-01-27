
using AutoMapper;

namespace Score.Api.Mappers
{
    public class ClientMapper :Profile
    {
        public ClientMapper()
        {
            CreateMap<Clients.Models.ScoreRecord, Domain.Models.ScoreRecord>();
        }
    }
}
