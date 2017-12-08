using VREPRemoteAPI.Enums;
using System;
using System.Runtime.InteropServices;
using System.Threading;

namespace VREPRemoteAPI
{
    public static class VREPWrapper
    {
        public const string DLL_NAME = "remoteApi.dll";

        #region Remote API helper functions

        [DllImport(DLL_NAME)]
        public static extern int simxStart(string ip, int port, bool waitForConnection, bool reconnectOnDisconnect, int timeoutMS, int cycleTimeMS);

        [DllImport(DLL_NAME, CallingConvention = CallingConvention.StdCall)]
        public static extern void simxFinish(int clientID);

        [DllImport(DLL_NAME, CallingConvention = CallingConvention.StdCall)]
        public static extern int simxGetConnectionId(int clientID);

        [DllImport(DLL_NAME)]
        public static extern SimXError simxPauseCommunication(int cliendID, int pause);

        #endregion

        #region Simulation functionality

        [DllImport(DLL_NAME)]
        public static extern SimXError simxStartSimulation(int clientID, SimXOpMode opmode);

        [DllImport(DLL_NAME)]
        public static extern SimXError simxStopSimulation(int clientID, SimXOpMode opmode);

        #endregion

        #region Signals

        [DllImport(DLL_NAME)]
        public static extern SimXError simxGetFloatSignal(int clientID, string signalName, ref float value, SimXOpMode opmode);

        [DllImport(DLL_NAME)]
        public static extern SimXError simxGetIntegerSignal(int clientID, string signalName, ref int value, SimXOpMode opmode);

        public static int simwGetIntegerSignal(int clientID, string signalName)
        {
            int v = -1;
            simxGetIntegerSignal(clientID, signalName, ref v, SimXOpMode.Streaming);
            Thread.Sleep(150);
            simxGetIntegerSignal(clientID, signalName, ref v, SimXOpMode.Buffer);
            return v;
        }

        [DllImport(DLL_NAME)]
        public static extern SimXError simxSetStringSignal(int clientID, string signalName, string value, int length, SimXOpMode opmode);

        [DllImport(DLL_NAME)]
        public static extern SimXError simxGetStringSignal(int clientID, string signalName, ref IntPtr pointerToValue, ref int signalLength, SimXOpMode opmode);

        public static string simwGetStringSignal(int clientID, string signalName)
        {
            IntPtr p = IntPtr.Zero;
            var l1 = 0;
            var l2 = 0;
            var e = simxGetStringSignal(clientID, signalName, ref p, ref l1, SimXOpMode.Streaming);
            Thread.Sleep(150);
            e = simxGetStringSignal(clientID, signalName, ref p, ref l2, SimXOpMode.Buffer);
            Console.WriteLine("Signal {0} -> {1}/{2}", signalName, l1, l2);
            if (e == SimXError.NoError && p != IntPtr.Zero)
            {
                var s = Marshal.PtrToStringAnsi(p, l2);
                Marshal.Release(p);
                return s;
            }
            return "";
        }

        [DllImport(DLL_NAME)]
        public static extern SimXError simxGetAndClearStringSignal(int clientID, string signalName, ref IntPtr pointerToValue, ref int signalLength, SimXOpMode opmode);

        #endregion
        
        #region Joint object functionality

        [DllImport(DLL_NAME)]
        public static extern SimXError simxGetJointPosition(int clientID, int jointHandle, ref float targetPosition, SimXOpMode opmode);

        [DllImport(DLL_NAME)]
        public static extern SimXError simxGetObjectIntParameter(int clientID, int objectHandle, int parameterID, ref int parameterValue, SimXOpMode opmode);

        [DllImport(DLL_NAME)]
        public static extern SimXError simxGetObjectFloatParameter(int clientID, int objectHandle, int parameterID, ref float parameterValue, SimXOpMode opmode);

        [DllImport(DLL_NAME)]
        public static extern SimXError simxGetObjectOrientation(int clientID, int jointHandle, int relativeToHandle, float[] orientations, SimXOpMode opmode);

        [DllImport(DLL_NAME)]
        public static extern SimXError simxGetObjectPosition(int clientID, int jointHandle, int relativeToHandle, float[] positions, SimXOpMode opmode);
        
        [DllImport(DLL_NAME)]
        public static extern SimXError simxSetJointTargetPosition(int clientID, int jointHandle, float targetPosition, SimXOpMode opmode);

        [DllImport(DLL_NAME)]
        public static extern SimXError simxSetJointTargetVelocity(int clientID, int jointHandle, float velocity, SimXOpMode opmode);
        
        #endregion

        #region Proximity sensor functionality

        [DllImport(DLL_NAME)]
        public extern static SimXError simxReadProximitySensor(int clientID, int sensorHandle,
                                                         ref char detectionState, float[] detectionPoint, ref int objectHandle, float[] normalVector, SimXOpMode opmode);

        [DllImport(DLL_NAME)]
        public static extern SimXError simxSetObjectFloatParameter(int clientID, int objectHandle, int parameterID, float parameterValue, SimXOpMode opmode);

        [DllImport(DLL_NAME)]
        public static extern SimXError simxSetObjectIntParameter(int clientID, int objectHandle, int parameterID, int parameterValue, SimXOpMode opmode);

        #endregion

        [DllImport(DLL_NAME)]
        public static extern SimXError simxGetUIEventButton(int clientID, int uiHandle, ref int uiEventButtonID, IntPtr aux, SimXOpMode opmode);

        [DllImport(DLL_NAME)]
        public static extern SimXError simxGetUIHandle(int clientID, string uiName, IntPtr p, SimXOpMode opmode);

        [DllImport(DLL_NAME)]
        public static extern SimXError simxGetObjectHandle(int clientID, string objectName, out int handle, SimXOpMode opmode);

    }
}