
namespace VREPRemoteAPI.Enums
{
    public enum SimXOpMode
    {
        Oneshot = 0,
        OneshotWait = 65536,
        Streaming = 131072,
        Continuous = 131072,
        OneshotSplit = 196608,
        ContinuousSplit = 262144,
        StreamingSplit = 262144,
        Discontinue = 327680,
        Buffer = 393216,
        Remove = 458752
    }
}
