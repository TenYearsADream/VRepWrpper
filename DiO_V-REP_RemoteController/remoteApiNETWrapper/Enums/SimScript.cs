
namespace VREPRemoteAPI.Enums
{
    public enum SimScript
    {
        NoError = 0,
        MainScriptNonexistent = 1,
        MainScriptNotCalled = 2,
        ReentranceError = 4,
        LuaError = 8,
        CallError = 16
    }
}
