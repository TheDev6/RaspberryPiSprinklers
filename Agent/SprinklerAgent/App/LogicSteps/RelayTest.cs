namespace SprinklerAgent.App.LogicSteps
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using LogicLadder;
    using Models;

    public class RelayTest : IStep<SprinklerAgentContext>
    {
        private readonly ISprinklerManager _manager;

        public RelayTest(ISprinklerManager manager)
        {
            _manager = manager;
        }

        public bool ContinueOnError => false;

        public Task<SprinklerAgentContext> RunStep(SprinklerAgentContext context)
        {
            _manager.RunZone(SprinklerZones.NorthGrass, TimeSpan.FromSeconds(1));
            Thread.Sleep(500);
            _manager.RunZone(SprinklerZones.SouthGrass, TimeSpan.FromSeconds(1));
            Thread.Sleep(500);
            _manager.RunZone(SprinklerZones.Trees, TimeSpan.FromSeconds(1));
            Thread.Sleep(500);
            _manager.RunZone(SprinklerZones.Bushes, TimeSpan.FromSeconds(1));

            return Task.FromResult(context);
        }
    }
}
