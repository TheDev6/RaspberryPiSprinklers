namespace SprinklerAgent.App
{
    using System;
    using Models;

    public interface ISprinklerManager
    {
        void RunZone(SprinklerZone zone, TimeSpan runSpan);
        SprinklerZone GetActiveZone();
    }
}
