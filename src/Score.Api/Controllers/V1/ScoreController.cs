using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Swashbuckle.Swagger.Annotations;
using Microsoft.AspNetCore.Mvc;
using Score.Contracts.Requests;
using Score.Contracts.Responses;
using Score.Contracts.V1.Requests;
using Score.Domain.Commands;
using Score.Domain.Queries;
using Serilog;

namespace Score.Api.Controllers.V1
{
    [ApiController]
 //   [ApiVersion("1.0")]
    [Route("/score")]
    public class ScoreController : ControllerBase
    {
        private readonly ILogger _logger;
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public ScoreController(ILogger logger, IMediator mediator, IMapper mapper)
        {
            _logger = logger;
            _mediator = mediator;
            _mapper = mapper;
        }


        /// <summary>
        /// Returns all the scores above the score 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet("{Score}")]
        [Produces("application/json")]
        [SwaggerOperation(OperationId = "score")]
        [SwaggerResponse(400, "Bad request")]
        [SwaggerResponse(500, "Error")]
        [SwaggerResponse(200, "Data returned", typeof(GetScoresResponse))]
        public async Task<ActionResult<GetScoresResponse>> GetScores([FromRoute] GetScoresRequest request)
        {
            if (request.Score < 0 || request.Score > 5)
            {
                return BadRequest("Score is out of range");
            }

            var query = new GetScoresQuery
            {
                Score = request.Score
            };

            var result = await _mediator.Send(query);

            if (!result.Success)
            {
                return StatusCode(500, "Unable to get score data at this time");
            }

            var response = _mapper.Map<GetScoresResponse>(result);

            return response;
        }

        /// <summary>
        /// Updates a player score 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("Update")]
        [Produces("application/json")]
        [SwaggerOperation(OperationId = "Update score")]
        [SwaggerResponse(400, "Bad request")]
        [SwaggerResponse(500, "Error")]
        [SwaggerResponse(204, "Score Updated")]
        [SwaggerResponse(404, "Player not found")]
        public async Task<ActionResult> UpdateScore([FromBody] UpdateScoreRequest request)
        {
            if (request.Score < 0 || request.Score > 5)
            {
                return BadRequest("Score is out of range");
            }

            if (string.IsNullOrWhiteSpace(request.Player))
            {
                return BadRequest("Player must be provided");
            }

            var command = new UpdateScoreCommand
            {
                Score = request.Score,
                Player = request.Player
            };

            var result = await _mediator.Send(command);

            if (!result.Success)
            {
                if (result.ResponseCode == 404)
                {
                    return NotFound("Player not found");
                }

                return StatusCode(500, "Unable to fulfill request at this time");
            }

            return StatusCode(204);
        }
    }
}


