using System;
using VREPRemoteAPI.Enums;

namespace VREPRemoteAPI
{
    public class VREPSetBlock : IDisposable
    {
        private readonly int clientID;

        public VREPSetBlock(int cID)
        {
            // Console.WriteLine("--- Opening Set Block -----");
            clientID = cID;
            if (VREPWrapper.simxPauseCommunication(cID, 1) != 0) throw new VREPException(SimXError.RemoteErrorFlag, "Pausing communication failed");
        }

        public void Dispose()
        {
            if (VREPWrapper.simxPauseCommunication(clientID, 0) != 0) throw new VREPException(SimXError.RemoteErrorFlag, "Restarting communication failed");
            // Console.WriteLine("--- Set Block closed ------");
        }
    }
}