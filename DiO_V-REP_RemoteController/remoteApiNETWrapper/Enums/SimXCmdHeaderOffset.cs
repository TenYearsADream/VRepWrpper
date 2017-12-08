
namespace VREPRemoteAPI.Enums
{
    public enum SimXCmdHeaderOffset
    {
        MemSize = 0,
        FullMemSize = 4,
        PdataOffset0 = 8,
        PdataOffset1 = 10,
        Command = 14,
        DelayOrSplit = 18,
        SimTime = 20,
        Status = 24,
        Reserved = 25
    }
}
