using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using VREPRemoteAPI;
using VREPRemoteAPI.Enums;

namespace DiO_V_REP_RemoteController
{
    public class UR5
    {

        private const int JOINT_COUNT = 6;

        private Thread processThread;

        private bool reqToStop = false;
        
        string robotJPrefix = "UR5_joint";

        private string[] jNames = new string[JOINT_COUNT];

        int[] jHandles = new int[JOINT_COUNT];
        
        public IPEndPoint endPoint { get; private set; }

        public int Timeout { get; set; }

        public bool WaitForConnection { get; set; }

        public bool Reconnect { get; set; }

        public int CycleTime { get; set; }

        public int ClientID { get; private set; }

        public UR5(IPEndPoint endPoint)
        {
            this.endPoint = endPoint;
            this.Timeout = 2000;
            this.WaitForConnection = true;
            this.Reconnect = true;
            this.CycleTime = 5;
            this.ClientID = -1;

            for (int jIndex = 0; jIndex < JOINT_COUNT; jIndex++)
            {
                // Create joint handles names.
                this.jNames[jIndex] = String.Format("{0}{1}", robotJPrefix, jIndex + 1);
            }
        }

        public void Connect()
        {
            if(processThread != null && processThread.ThreadState == ThreadState.Running)
            {
                return;
            }

            // Create new connection.
            this.ClientID = VREPWrapper.simxStart(this.endPoint.Address.ToString(), this.endPoint.Port, this.WaitForConnection, this.Reconnect, this.Timeout, this.CycleTime);
            // TODO: Add to wrapper We set the desired position and orientation
		    //                      simSetObjectPosition(targetSphereBase,irb140,pos)
		    //                      simSetObjectOrientation(targetSphereBase,irb140,orient)

            // If client ID is valid then begin read data.
            if (this.ClientID == -1)
            {
                return;
            }

            this.processThread = new Thread(new ThreadStart(this.ProcessRobotData));
            this.processThread.Start();
        }

        public void Disconnect()
        {
            if (processThread != null && processThread.ThreadState == ThreadState.Running)
            {
                this.reqToStop = true;
            }

            // Close connection.
            VREPWrapper.simxFinish(this.ClientID);
        }

        private void ProcessRobotData()
        {
            double[] jPositions = new double[JOINT_COUNT];
            float tmpPosition = 0.0f;
            SimXError simState;

            while (VREPWrapper.simxGetConnectionId(this.ClientID) != -1 && !this.reqToStop)
            {
                for (int jIndex = 0; jIndex < JOINT_COUNT; jIndex++)
                {

                    // Get handles.
                    simState = VREPWrapper.simxGetObjectHandle(this.ClientID, this.jNames[jIndex], out jHandles[jIndex], SimXOpMode.Continuous);

                    // If no errors then get data.
                    if (simState == SimXError.NoError)
                    {
                        // Clear temporal position.
                        tmpPosition = 0.0f;

                        // Get joint position.
                        VREPWrapper.simxGetJointPosition(this.ClientID, jHandles[jIndex], ref tmpPosition, SimXOpMode.Continuous);
                        
                        // 
                        jPositions[jIndex] = tmpPosition;

                        //VREPWrapper.simxSetJointTargetVelocity(clientID, leftMotorHandle, motorSpeeds[0], SimXOpMode.Oneshot);
                        //VREPWrapper.simxSetJointTargetVelocity(clientID, rightMotorHandle, motorSpeeds[1], SimXOpMode.Oneshot);
                    }

                    // Wait for a while
                    Thread.Sleep(1);
                }
            }
        }
    }
}
