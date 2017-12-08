using System;
using System.Collections.Generic;
using System.Linq;
using VREPRemoteAPI.Enums;

namespace VREPRemoteAPI
{
    public static class VREPHelper
    {
        private static bool positionControlEnabled = false;

        public static float D { get; set; }

        public static float I { get; set; }

        public static float P { get; set; }

        public static void BeginSetBlock(int clientID)
        {
            VREPWrapper.simxPauseCommunication(clientID, 1);
        }

        public static void EndSetBlock(int clientID)
        {
            VREPWrapper.simxPauseCommunication(clientID, 0);
        }

        public static SimXError SetJointPositionControl(int clientID, int jointID, bool onOff)
        {
            if (positionControlEnabled == onOff) return SimXError.NoError;
            var code = new List<SimXError>
                           {
                               VREPWrapper.simxSetObjectIntParameter(clientID, jointID, 2001, (onOff) ? 1 : 0, SimXOpMode.OneshotWait),
                               VREPWrapper.simxSetObjectFloatParameter(clientID, jointID, 2002, P, SimXOpMode.Oneshot),
                               VREPWrapper.simxSetObjectFloatParameter(clientID, jointID, 2003, I, SimXOpMode.Oneshot),
                               VREPWrapper.simxSetObjectFloatParameter(clientID, jointID, 2004, D, SimXOpMode.Oneshot)
                           };

            foreach (var c in code.Where(p => p > SimXError.NoError).Select((p, i) => new { Code = p, Index = i }))
            {
                Console.WriteLine("Errors: {0}: {1} -> {2}", jointID, c.Index, c.Code);
            }

            positionControlEnabled = onOff;

            return code.Max();
        }
    }

    public class VREPException : Exception
    {
        public VREPException(SimXError Error, string Message = "")
            : base(String.Format("{0}: {1}", Enum.GetName(typeof(SimXError), Error), Message))
        {
        }
    }
}