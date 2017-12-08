
namespace VREPRemoteAPI.Enums
{
    public enum SimObjectSpecialProperty
    {
        Collidable = 1,
        Measurable = 2,
        DetectableUltrasonic = 16,
        DetectableInfrared = 32,
        DetectableLaser = 64,
        DetectableInductive = 128,
        DetectableCapacitive = 256,
        DetectableAll = 496,
        Renderable = 512,
        Cuttable = 1024,
        PathPlanningIgnored = 2048
    }
}
