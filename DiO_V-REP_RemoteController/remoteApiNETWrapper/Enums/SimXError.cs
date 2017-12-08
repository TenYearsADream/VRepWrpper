
namespace VREPRemoteAPI.Enums
{
    public enum SimXError : int
    {
        NoError = 0,
        NoValueFlag = 1,
        TimeoutFlag = 2,
        IllegalOpModeFlag = 4,
        RemoteErrorFlag = 8,
        Split_progressFlag = 16,
        LocalErrorFlag = 32,
        InitializeErrorFlag = 64
    }
}
