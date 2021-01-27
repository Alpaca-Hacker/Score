using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using AutoFixture;
using FluentAssertions;
using FunctionalTests.Mocks;
using FunctionalTests.Orchestration;
using Microsoft.Extensions.DependencyInjection;
using Score.Clients.Clients;
using Score.Contracts.Responses;
using TechTalk.SpecFlow;

namespace FunctionalTests.Steps
{
    [Binding]
    public sealed class GetScoresStepDefinitions
    {
        private readonly ScenarioContext _scenarioContext; 
        private readonly HttpOrchestrator _httpOrchestrator;
        private readonly MockScoreClient _scoreClient;
        private readonly Fixture _fixture;

        public GetScoresStepDefinitions(ScenarioContext scenarioContext, HttpOrchestrator httpOrchestrator)
        {
            _scenarioContext = scenarioContext;
            _httpOrchestrator = httpOrchestrator;
            _scoreClient = _httpOrchestrator.TestServer.Host.Services.GetRequiredService<IScoreClient>() as MockScoreClient;
            _fixture = new Fixture();
        }

        [Given(@"I have a score of '(.*)'")]
        public void GivenIHaveAScoreOf(int score)
        {
            _scenarioContext["score"] = score;
        }

        [When(@"I call the get scores endpoint")]
        public Task WhenICallTheGetScoresEndpoint()
        {
            _httpOrchestrator.AbsolutePath = $"Score/{_scenarioContext["score"]}";
            return _httpOrchestrator.SendAsync(HttpMethod.Get);
        }

        [Then(@"I will get '(.*)' results back")]
        public async void ThenIWillGetOfResultsBack(int expected)
        {
            var response = await _httpOrchestrator.GetResponseContentModel<GetScoresResponse>();
            response.Scores.Should().HaveCount(expected);
        }

        [Then(@"all the records are correct")]
        public async void ThenAllTheRecordsAreCorrect()
        {
            var response = await _httpOrchestrator.GetResponseContentModel<GetScoresResponse>();
            response.Scores.Where(s => s.Score < 0 || s.Score > 5 || string.IsNullOrEmpty(s.Player)).Should().BeEmpty();
        }

        [Then(@"all the scores are above '(.*)'")]
        public async void ThenAllTheScoresAreAbove(int score)
        {
            var response = await _httpOrchestrator.GetResponseContentModel<GetScoresResponse>();
            response.Scores.Where(s => s.Score <= score).Should().BeEmpty();
        }

        [Then(@"the status code is '(.*)'")]
        public Task ThenStatusCodeIs(HttpStatusCode statusCode)
        {
            return _httpOrchestrator.AssertStatusCode(statusCode);
        }

        [Given(@"the client has an issue")]
        public void GivenTheClientHasAnIssue()
        {
            _scoreClient.ThrowError = true;
        }


    }
}
