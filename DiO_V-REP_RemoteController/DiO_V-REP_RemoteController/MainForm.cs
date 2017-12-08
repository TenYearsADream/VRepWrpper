using System;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;

using VREPRemoteAPI;
using VREPRemoteAPI.Enums;

namespace DiO_V_REP_RemoteController
{
    public partial class MainForm : Form
    {

        private Thread processThread;

        private bool reqToStop = false;

        private string[] jNames = new string[] { "", "", "", "", "", "" };

        private int clientID = 0;

        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        public MainForm()
        {
            InitializeComponent();
        }

        #endregion

        #region Form

        private void MainForm_Load(object sender, EventArgs e)
        {
            this.DoubleBuffered = true;
            this.SetupDataGridView();
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.StopProcessingRobotData();
        }

        #endregion

        #region Visualization

        private void SetSimState(SimXError state)
        {
            // Refresh the image.
            if (this.lblResponseStat.InvokeRequired)
            {
                this.lblResponseStat.BeginInvoke((MethodInvoker)delegate()
                {
                    this.lblResponseStat.Text = String.Format("State: {0}", state);
                });
            }
            else
            {
                this.lblResponseStat.Refresh();
            }
        }

        private void SetupDataGridView()
        {
            //this.Controls.Add(dgvSelectWorkpiece);

            dgvJointParameters.ColumnCount = 4;

            dgvJointParameters.ColumnHeadersDefaultCellStyle.BackColor = Color.Navy;
            dgvJointParameters.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvJointParameters.ColumnHeadersDefaultCellStyle.Font =
                new Font(dgvJointParameters.Font, FontStyle.Bold);

            dgvJointParameters.Name = "dgvJointParameters";
            //dgvJointParameters.Location = new System.Drawing.Point(8, 8);
            //dgvJointParameters.Size = new Size(500, 250);
            dgvJointParameters.AutoSizeRowsMode =
                DataGridViewAutoSizeRowsMode.DisplayedCellsExceptHeaders;
            dgvJointParameters.ColumnHeadersBorderStyle =
                DataGridViewHeaderBorderStyle.Single;
            dgvJointParameters.CellBorderStyle = DataGridViewCellBorderStyle.Single;
            dgvJointParameters.GridColor = Color.Black;
            dgvJointParameters.RowHeadersVisible = false;

            dgvJointParameters.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            dgvJointParameters.Columns[0].Name = "Index";
            dgvJointParameters.Columns[0].Width = 150;
            dgvJointParameters.Columns[1].Name = "Handle";
            dgvJointParameters.Columns[2].Name = "Name";
            dgvJointParameters.Columns[3].Name = "Position";

            //dgvSelectWorkpiece.Columns[4].DefaultCellStyle.Font =
            //    new Font(dgvSelectWorkpiece.DefaultCellStyle.Font, FontStyle.Italic);

            dgvJointParameters.SelectionMode =
                DataGridViewSelectionMode.FullRowSelect;
            dgvJointParameters.MultiSelect = false;
            //dgvJointParameters.Dock = DockStyle.Fill;
        }

        private void PositionalDataFiller(int[] jHandles, double[] jPositions, string[] jNames)
        {
            if (this.dgvJointParameters.ColumnCount <= 0)
            {
                return;
            }

            this.dgvJointParameters.Rows.Clear();
            
            for (int jIndex = 0; jIndex < 6; jIndex++)
            {
                string[] row = new string[]
                {
                    jIndex.ToString(),
                    jHandles[jIndex].ToString(),
                    jNames[jIndex].ToString(),
                    jPositions[jIndex].ToString("F3")
                };

                this.dgvJointParameters.Rows.Add(row);
            }
        }

        private void FillPositionalData(int[] jHandles, double[] jPositions, string[] jNames)
        {
            // Refresh the image.
            if (this.lblResponseStat.InvokeRequired)
            {
                this.lblResponseStat.BeginInvoke((MethodInvoker)delegate()
                {
                    this.PositionalDataFiller(jHandles, jPositions, jNames);
                });
            }
            else
            {
                this.PositionalDataFiller(jHandles, jPositions, jNames);
            }
        }

        #endregion

        #region Robot simulation control

        private void ProcessRobotData()
        {
            clientID = -1;

            // Close all connections no meter what.
            VREPWrapper.simxFinish(clientID);
            
            // Create new connection.
            clientID = VREPWrapper.simxStart("127.0.0.1", 19997, true, true, 2000, 5);

            //VREPWrapper.simxStartSimulation(clientID, SimXOpMode.Continuous);

            // If client ID is valid then begin read data.
            if (clientID == -1)
            {
                return;
            }

            while (VREPWrapper.simxGetConnectionId(clientID) != -1 && !this.reqToStop)
            {
                int jCount = 6;
                string robotJPrefix = "UR5_joint";
                int[] jHandles = new int[jCount];
                double[] jPositions = new double[jCount];
                float tmpPosition = 0.0f;

                for (int jIndex = 0; jIndex < jCount; jIndex++)
                {
                    // Create joint handles names.
                    string jHandle = String.Format("{0}{1}", robotJPrefix, jIndex + 1);
                    this.jNames[jIndex] = jHandle;
                    
                    // Get handles.
                    SimXError simState = VREPWrapper.simxGetObjectHandle(clientID, jHandle, out jHandles[jIndex], SimXOpMode.Continuous);
                    
                    // If no errors then get data.
                    if (simState == SimXError.NoError)
                    {
                        // Clear temporal position.
                        tmpPosition = 0.0f;

                        // Get joint position.
                        VREPWrapper.simxGetJointPosition(clientID, jHandles[jIndex], ref tmpPosition, SimXOpMode.Continuous);
                        // 
                        jPositions[jIndex] = RadianToDegree(tmpPosition);
                        
                        //VREPWrapper.simxSetJointTargetPosition(clientID, jHandles[0], 0.0f, SimXOpMode.Continuous);
                        //VREPWrapper.simxSetJointTargetVelocity(clientID, leftMotorHandle, motorSpeeds[0], SimXOpMode.Oneshot);
                        //VREPWrapper.simxSetJointTargetVelocity(clientID, rightMotorHandle, motorSpeeds[1], SimXOpMode.Oneshot);
                    }

                    this.SetSimState(simState);

                    // Wait for a while
                    Thread.Sleep(1);
                }

                this.FillPositionalData(jHandles, jPositions, this.jNames);
            }

            // Close connection.
            //VREPWrapper.simxFinish(clientID);

            // Clear the thread.
            this.processThread = null;
        }

        private void StartProcessingRobotData()
        {
            if (processThread != null)
            {
                return;
            }

            this.processThread = new Thread(new ThreadStart(this.ProcessRobotData));
            this.processThread.Start();

        }

        private void StopProcessingRobotData()
        {
            if (processThread != null && processThread.ThreadState == ThreadState.Running)
            {
                this.reqToStop = true;
                VREPWrapper.simxStopSimulation(clientID, SimXOpMode.Continuous);
                processThread.Join();
                processThread = null;
            }
        }
        
        #endregion

        #region Math

        /// <summary>
        /// Convert degree to radians.
        /// </summary>
        /// <param name="angle">Angle in [deg]./</param>
        /// <returns>Angle in [rad]</returns>
        public static double DegreeToRadian(double angle)
        {
            return Math.PI * angle / 180.0;
        }

        /// <summary>
        /// Convert radians to degree.
        /// </summary>
        /// <param name="angle">Angle in [rad]./</param>
        /// <returns>Angle in [deg]</returns>
        public static double RadianToDegree(double angle)
        {
            return angle * (180.0 / Math.PI);
        }

        #endregion

        #region Buttons

        private void btnStart_Click(object sender, EventArgs e)
        {
            this.StartProcessingRobotData();
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            this.StopProcessingRobotData();
        }

        #endregion


    }
}
