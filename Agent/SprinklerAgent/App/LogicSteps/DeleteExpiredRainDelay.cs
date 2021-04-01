namespace SprinklerAgent.App.LogicSteps
{
    using System.Threading.Tasks;
    using LogicLadder;
    using Models;
    using Sprinkler_Api;

    public class DeleteExpiredRainDelay : IStep<SprinklerAgentContext>
    {
        private readonly ISprinklerApiClient _sprinklerApi;
        public DeleteExpiredRainDelay(ISprinklerApiClient sprinklerApi)
        {
            _sprinklerApi = sprinklerApi;
        }
        
        public bool ContinueOnError => true;
        public async Task<SprinklerAgentContext> RunStep(SprinklerAgentContext context)
        {
            await _sprinklerApi.DeleteExpiredRainDelays();
            return context;
        }

        
    }
}
