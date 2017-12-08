
namespace VREPRemoteAPI.Enums
{
    public enum SimSimulation
    {
        Stopped = 0,
        Paused = 8,
        Advancing = 16,
        AdvancingFirstafterstop = 16,
        AdvancingRunning = 17,
        AdvancingLastbeforepause = 19,
        AdvancingFirstafterpause = 20,
        AdvancingAbouttostop = 21,
        AdvancingLastbeforestop = 22
    }
}
