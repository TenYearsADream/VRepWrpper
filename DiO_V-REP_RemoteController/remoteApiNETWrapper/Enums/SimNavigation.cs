
namespace VREPRemoteAPI.Enums
{
    public enum SimNavigation
    {
        passive = 0,
        CameraShift = 1,
        CameraRotate = 2,
        CameraZoom = 3,
        CameraTilt = 4,
        CameraAngle = 5,
        CameraFly = 6,
        ObjectShift = 7,
        ObjectRotate = 8,
        Reserved2 = 9,
        Reserved3 = 10,
        JointPathTest = 11,
        IKManip = 12,
        ObjectMultipleSelection = 13,
        Reserved4 = 256,
        ClickSelection = 512,
        CtrlSelection = 1024,
        ShiftSelection = 2048,
        CameraZoomWheel = 4096,
        CameraRotateRightButton = 8192
    }
}
