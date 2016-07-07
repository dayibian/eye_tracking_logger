using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Collections.ObjectModel;
using System.Windows.Forms;
using System.Data;
using TetComp;
using System.Text;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Timers;
using System.Net.Sockets;
using System.Net;

namespace Eye_Tracker_Component_C_Sharp_NET
{
	/// <summary>
	/// Tobii Eye Tracker Component API sample for Visual C# .NET
	/// 
	/// Ver 2005-05-02 PR
	/// 
	/// Copyright (C) Tobii Technology 2005, all rights reserved.
	/// </summary>
	public class MainForm : System.Windows.Forms.Form
	{
		ITetTrackStatus tetTrackStatus;
		ITetCalibPlot tetCalibPlot;
		TetClient tetClient;
		TetCalibProc tetCalibProc;
		GazeForm gazeForm;


        // Functions and variables used in tracking region
        //
        List<double> gaze_x_valid = new List<double>();
        List<double> gaze_y_valid = new List<double>();
        double fixation_duration;
        double fixation_point_x;
        double fixation_point_y;
        List<double> fixation_point_x_now = new List<double>();
        List<double> fixation_point_y_now = new List<double>();
        int now;
        int blink;
        int blink_now;
        float x, y;

        RichTextBox trackingTextBox;


        private System.Windows.Forms.TextBox serverAddressTextBox;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.GroupBox groupBox2;
		private System.Windows.Forms.GroupBox groupBox3;
		private System.Windows.Forms.Button trackStatusStartButton;
		private System.Windows.Forms.Button trackStatusStopButton;
		private System.Windows.Forms.Button calibrateButton;
		private System.Windows.Forms.Button recalibrateButton;
		private System.Windows.Forms.Button trackStopButton;
		private System.Windows.Forms.Button trackStartButton;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label label5;
		private AxTetComp.AxTetTrackStatus axTetTrackStatus;
		private AxTetComp.AxTetCalibPlot axTetCalibPlot;
		private System.Windows.Forms.RadioButton ninePtsRadioButton;
		private System.Windows.Forms.RadioButton fivePtsRadioButton;
		private System.Windows.Forms.RadioButton twoPtsRadioButton;
        private ComboBox eyetrackers;
        private CheckBox useManualIp;
        private Label label6;
        private TetComp.TetServiceBrowser serviceBrowser;
        private List<TetServiceEntryWrapper> services = null;
        private float posMinX, posMaxX, posMinY, posMaxY;
        private int pointNum, calibPintNum;
        private bool calibrationFlag;
        private Button button1;
        private Socket clienterCamera = null;
        private Socket clienterPha = null;
        private int port = 8080;
        private bool start = false;
        private bool end = false;
        SocketDLL.TrialData clientData = null;
        SocketDLL.ServerSocket socketser;
        Logger logger = null;
        DateTime _starttime = new DateTime();
        Stopwatch _stopwatch = null;
        public int ListNum;
        private TimeSpan trialStartTime;
        private TimeSpan trialCurrTime;
        private Button button2;
        private Label label1;
        private Label label7;
        /// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public MainForm()
		{
            //initial the parameters
            ListNum = 0;
            calibrationFlag = true;
            posMaxX = 0;
            posMaxY = 0;
            posMinX = 0;
            posMinY = 0;
            pointNum = 0;
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();
            services = new List<TetServiceEntryWrapper>();
            ////start the socket thread

            _starttime = DateTime.UtcNow;
            _stopwatch = Stopwatch.StartNew();

            // Start the application timer for time stamp
            _starttime = DateTime.UtcNow;
            _stopwatch = Stopwatch.StartNew();
            // Initialize the logger here
            DateTime highresDT = _starttime.AddTicks(_stopwatch.Elapsed.Ticks);
            DateTime cstTime = highresDT.ToLocalTime();

            string timeStamp = cstTime.ToString("_MM-dd-yyyy_HH-mm-ss-ffff");
            string raw = "RawData" + timeStamp + ".dat";
            logger = new Logger(raw);
        }

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{

			if( disposing )
			{
				if (components != null) 
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.serverAddressTextBox = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.trackStatusStopButton = new System.Windows.Forms.Button();
            this.trackStatusStartButton = new System.Windows.Forms.Button();
            this.axTetTrackStatus = new AxTetComp.AxTetTrackStatus();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.twoPtsRadioButton = new System.Windows.Forms.RadioButton();
            this.fivePtsRadioButton = new System.Windows.Forms.RadioButton();
            this.ninePtsRadioButton = new System.Windows.Forms.RadioButton();
            this.recalibrateButton = new System.Windows.Forms.Button();
            this.calibrateButton = new System.Windows.Forms.Button();
            this.axTetCalibPlot = new AxTetComp.AxTetCalibPlot();
            this.label2 = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.trackStopButton = new System.Windows.Forms.Button();
            this.trackStartButton = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.eyetrackers = new System.Windows.Forms.ComboBox();
            this.useManualIp = new System.Windows.Forms.CheckBox();
            this.label6 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.axTetTrackStatus)).BeginInit();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.axTetCalibPlot)).BeginInit();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // serverAddressTextBox
            // 
            this.serverAddressTextBox.Enabled = false;
            this.serverAddressTextBox.Location = new System.Drawing.Point(499, 44);
            this.serverAddressTextBox.Name = "serverAddressTextBox";
            this.serverAddressTextBox.Size = new System.Drawing.Size(100, 20);
            this.serverAddressTextBox.TabIndex = 0;
            this.serverAddressTextBox.Text = "localhost";
            this.serverAddressTextBox.TextChanged += new System.EventHandler(this.serverAddressTextBox_TextChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.trackStatusStopButton);
            this.groupBox1.Controls.Add(this.trackStatusStartButton);
            this.groupBox1.Controls.Add(this.axTetTrackStatus);
            this.groupBox1.Location = new System.Drawing.Point(188, 84);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(410, 219);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Track Status";
            // 
            // trackStatusStopButton
            // 
            this.trackStatusStopButton.Enabled = false;
            this.trackStatusStopButton.Location = new System.Drawing.Point(16, 72);
            this.trackStatusStopButton.Name = "trackStatusStopButton";
            this.trackStatusStopButton.Size = new System.Drawing.Size(75, 23);
            this.trackStatusStopButton.TabIndex = 2;
            this.trackStatusStopButton.Text = "S&top";
            this.trackStatusStopButton.Click += new System.EventHandler(this.trackStatusStopButton_Click);
            // 
            // trackStatusStartButton
            // 
            this.trackStatusStartButton.Enabled = false;
            this.trackStatusStartButton.Location = new System.Drawing.Point(16, 32);
            this.trackStatusStartButton.Name = "trackStatusStartButton";
            this.trackStatusStartButton.Size = new System.Drawing.Size(75, 23);
            this.trackStatusStartButton.TabIndex = 1;
            this.trackStatusStartButton.Text = "&Start";
            this.trackStatusStartButton.Click += new System.EventHandler(this.trackStatusStartButton_Click);
            // 
            // axTetTrackStatus
            // 
            this.axTetTrackStatus.Enabled = true;
            this.axTetTrackStatus.Location = new System.Drawing.Point(208, 16);
            this.axTetTrackStatus.Name = "axTetTrackStatus";
            this.axTetTrackStatus.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axTetTrackStatus.OcxState")));
            this.axTetTrackStatus.Size = new System.Drawing.Size(192, 192);
            this.axTetTrackStatus.TabIndex = 0;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.twoPtsRadioButton);
            this.groupBox2.Controls.Add(this.fivePtsRadioButton);
            this.groupBox2.Controls.Add(this.ninePtsRadioButton);
            this.groupBox2.Controls.Add(this.recalibrateButton);
            this.groupBox2.Controls.Add(this.calibrateButton);
            this.groupBox2.Controls.Add(this.axTetCalibPlot);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Location = new System.Drawing.Point(188, 309);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(410, 220);
            this.groupBox2.TabIndex = 3;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Calibrate";
            // 
            // twoPtsRadioButton
            // 
            this.twoPtsRadioButton.Location = new System.Drawing.Point(120, 64);
            this.twoPtsRadioButton.Name = "twoPtsRadioButton";
            this.twoPtsRadioButton.Size = new System.Drawing.Size(64, 16);
            this.twoPtsRadioButton.TabIndex = 7;
            this.twoPtsRadioButton.Text = "&2 points";
            // 
            // fivePtsRadioButton
            // 
            this.fivePtsRadioButton.Location = new System.Drawing.Point(120, 48);
            this.fivePtsRadioButton.Name = "fivePtsRadioButton";
            this.fivePtsRadioButton.Size = new System.Drawing.Size(64, 16);
            this.fivePtsRadioButton.TabIndex = 6;
            this.fivePtsRadioButton.Text = "&5 points";
            // 
            // ninePtsRadioButton
            // 
            this.ninePtsRadioButton.Checked = true;
            this.ninePtsRadioButton.Location = new System.Drawing.Point(120, 32);
            this.ninePtsRadioButton.Name = "ninePtsRadioButton";
            this.ninePtsRadioButton.Size = new System.Drawing.Size(64, 16);
            this.ninePtsRadioButton.TabIndex = 5;
            this.ninePtsRadioButton.TabStop = true;
            this.ninePtsRadioButton.Text = "&9 points";
            // 
            // recalibrateButton
            // 
            this.recalibrateButton.Enabled = false;
            this.recalibrateButton.Location = new System.Drawing.Point(16, 64);
            this.recalibrateButton.Name = "recalibrateButton";
            this.recalibrateButton.Size = new System.Drawing.Size(75, 23);
            this.recalibrateButton.TabIndex = 3;
            this.recalibrateButton.Text = "&Recalibrate";
            this.recalibrateButton.Click += new System.EventHandler(this.recalibrateButton_Click);
            // 
            // calibrateButton
            // 
            this.calibrateButton.Enabled = false;
            this.calibrateButton.Location = new System.Drawing.Point(16, 24);
            this.calibrateButton.Name = "calibrateButton";
            this.calibrateButton.Size = new System.Drawing.Size(75, 23);
            this.calibrateButton.TabIndex = 2;
            this.calibrateButton.Text = "&Calibrate";
            this.calibrateButton.Click += new System.EventHandler(this.calibrateButton_Click);
            // 
            // axTetCalibPlot
            // 
            this.axTetCalibPlot.Enabled = true;
            this.axTetCalibPlot.Location = new System.Drawing.Point(208, 16);
            this.axTetCalibPlot.Name = "axTetCalibPlot";
            this.axTetCalibPlot.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axTetCalibPlot.OcxState")));
            this.axTetCalibPlot.Size = new System.Drawing.Size(192, 192);
            this.axTetCalibPlot.TabIndex = 0;
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(16, 96);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(192, 16);
            this.label2.TabIndex = 4;
            this.label2.Text = "A key press interrupts the calibration.";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.trackStopButton);
            this.groupBox3.Controls.Add(this.trackStartButton);
            this.groupBox3.Location = new System.Drawing.Point(188, 663);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(112, 104);
            this.groupBox3.TabIndex = 4;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Track";
            // 
            // trackStopButton
            // 
            this.trackStopButton.Enabled = false;
            this.trackStopButton.Location = new System.Drawing.Point(16, 64);
            this.trackStopButton.Name = "trackStopButton";
            this.trackStopButton.Size = new System.Drawing.Size(75, 23);
            this.trackStopButton.TabIndex = 5;
            this.trackStopButton.Text = "St&op";
            this.trackStopButton.Click += new System.EventHandler(this.trackStopButton_Click);
            // 
            // trackStartButton
            // 
            this.trackStartButton.Enabled = false;
            this.trackStartButton.Location = new System.Drawing.Point(16, 24);
            this.trackStartButton.Name = "trackStartButton";
            this.trackStartButton.Size = new System.Drawing.Size(75, 23);
            this.trackStartButton.TabIndex = 4;
            this.trackStartButton.Text = "St&art";
            this.trackStartButton.Click += new System.EventHandler(this.trackStartButton_Click);
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(20, 92);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(152, 112);
            this.label3.TabIndex = 5;
            this.label3.Text = resources.GetString("label3.Text");
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(20, 317);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(152, 112);
            this.label4.TabIndex = 6;
            this.label4.Text = "Use calibration to calibrate the eye tracker for your eyes. Hit recalibrate to im" +
                "prove inadequate points.";
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(20, 543);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(152, 96);
            this.label5.TabIndex = 7;
            this.label5.Text = "After calibrating, you are ready to start tracking gaze data. A simple square is " +
                "displayed where you gaze at, changing color and size depending on your distance " +
                "to the eye tracker.";
            // 
            // eyetrackers
            // 
            this.eyetrackers.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.eyetrackers.FormattingEnabled = true;
            this.eyetrackers.Location = new System.Drawing.Point(294, 15);
            this.eyetrackers.Name = "eyetrackers";
            this.eyetrackers.Size = new System.Drawing.Size(305, 21);
            this.eyetrackers.TabIndex = 8;
            // 
            // useManualIp
            // 
            this.useManualIp.AutoSize = true;
            this.useManualIp.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.useManualIp.Location = new System.Drawing.Point(357, 46);
            this.useManualIp.Name = "useManualIp";
            this.useManualIp.Size = new System.Drawing.Size(136, 17);
            this.useManualIp.TabIndex = 9;
            this.useManualIp.Text = "Use Manual IP-address";
            this.useManualIp.UseVisualStyleBackColor = true;
            this.useManualIp.CheckedChanged += new System.EventHandler(this.useManualIp_CheckedChanged);
            // 
            // label6
            // 
            this.label6.Location = new System.Drawing.Point(20, 15);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(259, 32);
            this.label6.TabIndex = 10;
            this.label6.Text = "Choose an eyetracker from the list or specify a manual IP address:";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(204, 559);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(105, 23);
            this.button1.TabIndex = 17;
            this.button1.Text = "CostumeCalibration";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(204, 606);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(100, 23);
            this.button2.TabIndex = 18;
            this.button2.Text = "socket";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(409, 564);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(213, 65);
            this.label1.TabIndex = 19;
            this.label1.Text = "Notice: \r\n(1) track status-> start; find both eyes\r\n(2) costume Calibration; 5 po" +
                "ints calibration\r\n(3) socket; first from python, second camera\r\n(4) track -> sta" +
                "rt; waiting for start signal";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(391, 695);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(35, 13);
            this.label7.TabIndex = 20;
            this.label7.Text = "label7";
            // 
            // MainForm
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(714, 812);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.useManualIp);
            this.Controls.Add(this.eyetrackers);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.serverAddressTextBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "MainForm";
            this.Text = "Tobii Eye Tracking C# Sample";
            this.Closing += new System.ComponentModel.CancelEventHandler(this.MainForm_Closing);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.axTetTrackStatus)).EndInit();
            this.groupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.axTetCalibPlot)).EndInit();
            this.groupBox3.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

		}
		#endregion

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main() 
		{
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());

		}

		/// <summary>
		/// Updates the calibration plot object with data from last calibration.
		/// </summary>
		void UpdateCalibPlot() 
		{
			if (!tetCalibPlot.IsConnected) 
			{
				tetCalibPlot.Connect(GetConnectionString(), (int)TetConstants.TetConstants_DefaultServerPort);
				tetCalibPlot.SetData(null); // Will use the currently stored calibration data
			}
			tetCalibPlot.UpdateData();
		}

		/// <summary>
		/// Starts calibration in either calibration or recalibration mode.
		/// </summary>
		/// <param name="isRecalibrating">whether to use recalibration or not.</param>
		void Calibrate(bool isRecalibrating) 
		{
			// Connect the calibration procedure if necessary
			if (!tetCalibProc.IsConnected) tetCalibProc.Connect(GetConnectionString(), (int)TetConstants.TetConstants_DefaultServerPort);

			// Initiate number of points to be calibrated
			SetNumberOfCalibrationPoints();

            // Initiate window properties and start calibration
            tetCalibProc.PointColor = 255;
            tetCalibProc.WindowTopmost = false;
			tetCalibProc.WindowVisible = true;
			tetCalibProc.StartCalibration(isRecalibrating ? TetCalibType.TetCalibType_Recalib : TetCalibType.TetCalibType_Calib, false);
		}

		/// <summary>
		/// Sets the number of points to use when calibrating.
		/// </summary>
		void SetNumberOfCalibrationPoints()
		{
			if (ninePtsRadioButton.Checked) tetCalibProc.NumPoints = TetNumCalibPoints.TetNumCalibPoints_9;
			else if (fivePtsRadioButton.Checked) tetCalibProc.NumPoints = TetNumCalibPoints.TetNumCalibPoints_5;
			else if (twoPtsRadioButton.Checked) tetCalibProc.NumPoints = TetNumCalibPoints.TetNumCalibPoints_2;
		}

        private string GetConnectionString()
        {
            if (useManualIp.Checked)
            {
                return serverAddressTextBox.Text;
            }
            else
            {
                return ((TetServiceEntryWrapper)eyetrackers.SelectedItem).Hostname;
            }
        }

        private void UpdateButtons()
        {
            bool valid = false;
            if (useManualIp.Checked)
                valid = serverAddressTextBox.Text.Length != 0;
            else
                valid = eyetrackers.SelectedIndex != -1;

            trackStartButton.Enabled = valid;
            trackStopButton.Enabled = valid;
            trackStatusStartButton.Enabled = valid;
            trackStatusStopButton.Enabled = valid;
            calibrateButton.Enabled = valid;
            recalibrateButton.Enabled = valid;
        }

		#region Form events

		/// <summary>
		/// Creates and initializes members.
		/// </summary>
		private void MainForm_Load(object sender, System.EventArgs e)
		{
			gazeForm = new GazeForm();

			// Retreive underlying references to ActiveX controls
			tetTrackStatus = (ITetTrackStatus)axTetTrackStatus.GetOcx();
			tetCalibPlot = (ITetCalibPlot)axTetCalibPlot.GetOcx();

			// Set up the calibration procedure object and it's events
			tetCalibProc = new TetCalibProcClass();
			_ITetCalibProcEvents_Event tetCalibProcEvents = (_ITetCalibProcEvents_Event)tetCalibProc;
			tetCalibProcEvents.OnCalibrationEnd += new _ITetCalibProcEvents_OnCalibrationEndEventHandler(tetCalibProcEvents_OnCalibrationEnd);
			tetCalibProcEvents.OnKeyDown += new _ITetCalibProcEvents_OnKeyDownEventHandler(tetCalibProcEvents_OnKeyDown);

			// Set up the TET client object and it's events
			tetClient = new TetClientClass();
			_ITetClientEvents_Event tetClientEvents = (_ITetClientEvents_Event)tetClient;
			tetClientEvents.OnTrackingStarted += new _ITetClientEvents_OnTrackingStartedEventHandler(tetClientEvents_OnTrackingStarted);
			tetClientEvents.OnTrackingStopped += new _ITetClientEvents_OnTrackingStoppedEventHandler(tetClientEvents_OnTrackingStopped);
			tetClientEvents.OnGazeData += new _ITetClientEvents_OnGazeDataEventHandler(tetClientEvents_OnGazeData);
            //tetClientEvents.OnCalibrationGazeData += new _ITetClientEvents_OnCalibrationGazeDataEventHandler(tetClientEvents_OnCalibrationGazeData);
            tetClientEvents.OnAddCalibrationPointStarted +=new _ITetClientEvents_OnAddCalibrationPointStartedEventHandler(tetClientEvents_OnAddCalibrationPointStarted);
            tetClientEvents.OnAddCalibrationPointEnded +=new _ITetClientEvents_OnAddCalibrationPointEndedEventHandler(tetClientEvents_OnAddCalibrationPointEnded);

            serviceBrowser = new TetServiceBrowserClass();
            serviceBrowser.OnServiceAdded += new _ITetServiceBrowserEvents_OnServiceAddedEventHandler(serviceBrowser_OnServiceAdded);
            serviceBrowser.OnServiceUpdated += new _ITetServiceBrowserEvents_OnServiceUpdatedEventHandler(serviceBrowser_OnServiceUpdated);
            serviceBrowser.OnServiceRemoved += new _ITetServiceBrowserEvents_OnServiceRemovedEventHandler(serviceBrowser_OnServiceRemoved);
            serviceBrowser.Start();
		}

        private void UpdateEyetrackerCombo()
        {
            eyetrackers.Items.Clear();
            TetServiceEntryWrapper selected = eyetrackers.SelectedItem as TetServiceEntryWrapper;

            foreach (TetServiceEntryWrapper eyetracker in services) {
                if (eyetracker.IsRunning)
                    eyetrackers.Items.Add(eyetracker);
            }

            if (eyetrackers.Items.Count == 0)
            {
                UpdateButtons();
                return;
            }

            if (selected != null)
            {
                int index = eyetrackers.Items.IndexOf(selected);
                if (index != -1)
                    eyetrackers.SelectedIndex = index;
                else
                    eyetrackers.SelectedIndex = 0;
            }
            else
            {
                eyetrackers.SelectedIndex = 0;
            }

            UpdateButtons();
        }

        void serviceBrowser_OnServiceRemoved(string servicename)
        {
            services.Remove(new TetServiceEntryWrapper(servicename));
            UpdateEyetrackerCombo();
        }

        void serviceBrowser_OnServiceUpdated(ref TetServiceEntry serviceEntry)
        {
            TetServiceEntryWrapper wrapper = new TetServiceEntryWrapper(serviceEntry);
            if (!services.Contains(wrapper))
            {
                services.Add(wrapper);
            }
            else
            {
                services.Remove(wrapper);
                services.Add(wrapper);
            }
            UpdateEyetrackerCombo();
        }

        void serviceBrowser_OnServiceAdded(ref TetServiceEntry serviceEntry)
        {
            serviceBrowser_OnServiceUpdated(ref serviceEntry);
        }

		private void MainForm_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			// Clean up objects
			try 
			{
				if (tetTrackStatus.IsConnected) 
				{
					if (tetTrackStatus.IsTracking) tetTrackStatus.Stop();
					tetTrackStatus.Disconnect();
				}

				if (tetCalibProc.IsConnected) 
				{
					if (tetCalibProc.IsCalibrating)	tetCalibProc.InterruptCalibration();
					tetCalibProc.Disconnect();
				}

				// TODO: TetCalibPlot.Disconnect() is always failing
				try 
				{
					if (tetCalibPlot.IsConnected) tetCalibPlot.Disconnect();
				} 
				catch {}

				if (tetClient.IsConnected) 
				{
					if (tetClient.IsTracking) tetClient.StopTracking();
					tetClient.Disconnect();
				}

                serviceBrowser.Stop();
			} 
			catch (Exception ex) 
			{
				MessageBox.Show(ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		#endregion

		#region Button events

		private void trackStatusStartButton_Click(object sender, System.EventArgs e)
		{
			try 
			{
				// Connect to the TET server if necessary
				if (!tetTrackStatus.IsConnected) tetTrackStatus.Connect(GetConnectionString(), (int)TetConstants.TetConstants_DefaultServerPort);
				
				// Start the track status meter
				if (!tetTrackStatus.IsTracking) tetTrackStatus.Start();
			} 
			catch (Exception ex) 
			{
				MessageBox.Show(ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		private void trackStatusStopButton_Click(object sender, System.EventArgs e)
		{
			try 
			{
				if (tetTrackStatus.IsTracking) tetTrackStatus.Stop();
			} 
			catch (Exception ex) 
			{
				MessageBox.Show(ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		private void calibrateButton_Click(object sender, System.EventArgs e)
		{
			try 
			{
				Calibrate(false);
			}
			catch (Exception ex) 
			{
				MessageBox.Show(ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		private void recalibrateButton_Click(object sender, System.EventArgs e)
		{
			try 
			{
				Calibrate(true);
			}
			catch (Exception ex) 
			{
				MessageBox.Show(ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		private void trackStartButton_Click(object sender, System.EventArgs e)
		{
			try 
			{
				// Connect to the TET server if necessary
				if (!tetClient.IsConnected) tetClient.Connect(GetConnectionString(), (int)TetConstants.TetConstants_DefaultServerPort, TetSynchronizationMode.TetSynchronizationMode_Local);
				
				// Start tracking gaze data
				tetClient.StartTracking();
			}
			catch (Exception ex) 
			{
				MessageBox.Show(ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
            _starttime = DateTime.UtcNow;
            _stopwatch = Stopwatch.StartNew();
            DateTime highresDT = _starttime.AddTicks(_stopwatch.Elapsed.Ticks);
            DateTime cstTime = highresDT.ToLocalTime();

            string timeStamp = cstTime.ToString("MM-dd-yyyy_HH-mm-ss-ffff");
            logger.set_refTime(timeStamp.Split('_')[1]);
        }

		private void trackStopButton_Click(object sender, System.EventArgs e)
		{
			try 
			{
				if (tetClient.IsTracking) tetClient.StopTracking();
			}
			catch (Exception ex) 
			{
				MessageBox.Show(ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
            logger.log();
            logger.close();
		}

		#endregion

		#region Tracking events

		private void tetClientEvents_OnTrackingStarted()
		{
			// Show the square form when tracking starts
			gazeForm.Show();
		}

		private void tetClientEvents_OnTrackingStopped(int hr)
		{
			// Hide the square form when tracking stops
			gazeForm.Hide();
			if (hr != (int)TetHResults.ITF_S_OK) MessageBox.Show(string.Format("Error {0} occured while tracking.", hr), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
		}

		private void tetClientEvents_OnGazeData(ref TetGazeData gazeData)
		{
            // System.Diagnostics.Trace.WriteLine(" LeftX: " + gazeData.x_camerapos_lefteye + " LeftGX: " + gazeData.x_gazepos_lefteye);
            // As soon as we get the data, get the time stamp
            DateTime highresDT = _starttime.AddTicks(_stopwatch.Elapsed.Ticks);
            DateTime cstTime = highresDT.ToLocalTime();
            string timeStamp = cstTime.ToString("MM/dd/yyyy HH:mm:ss:ffff");

            float distance = 0f;
            bool valid = true;
			float x, y;
            float gazeXRight, gazeYRight;
            x = 0;
            y = 0;

            System.Diagnostics.Trace.WriteLine(" LeftX: " + gazeData.x_camerapos_lefteye + " LeftGX: " + gazeData.x_gazepos_lefteye);

            //// STARTING ADDITIONS HERE
            float lpd = gazeData.diameter_pupil_lefteye;
            float rpd = gazeData.diameter_pupil_righteye;
            float pd = (lpd + rpd)/2.0f;
            Rectangle screenSize = Screen.PrimaryScreen.Bounds;
            int pixel_x = screenSize.Width; //1680; // Depends on monitor size, change to an easily located constant later
            int pixel_y = screenSize.Height;//1050; // same as above

            double d = 0; // this will be the distance the eye has traveled across the screen in 8.3 milliseconds. unit of pixels. 


			// Use data only if both left and right eye was found by the eye tracker
            if (gazeData.validity_lefteye == 0 || gazeData.validity_righteye == 0)
            {
                gazeXRight = gazeData.x_gazepos_righteye;
                gazeYRight = gazeData.y_gazepos_righteye;

                if (calibrationFlag == true&&pointNum>0)
                {
                    if (posMaxX == 0 && posMaxY == 0 && posMinX == 0 && posMinY == 0)
                    {
                        posMaxX = gazeXRight;
                        posMaxY = gazeYRight;
                        posMinX = gazeXRight;
                        posMinY = gazeYRight;
                    }
                    else
                    {
                        if (gazeXRight > posMaxX)
                        {
                            posMaxX = gazeXRight;
                        }
                        else if (gazeXRight < posMinX)
                        {
                            posMinX = gazeXRight;
                        }

                        if (gazeYRight > posMaxY)
                        {
                            posMaxY = gazeYRight;
                        }
                        else if (gazeYRight < posMinY)
                        {
                            posMinY = gazeYRight;
                        }
                    }
                }
                else if(calibrationFlag == false)
                {

                }

                // Let the x, y and distance be the right and left eye average
                if (gazeData.validity_lefteye == 0 && gazeData.validity_righteye != 0)
                {
                    x = gazeData.x_gazepos_lefteye;
                    y = gazeData.y_gazepos_lefteye;
                    distance = gazeData.distance_lefteye;
                }
                else if (gazeData.validity_lefteye != 0 && gazeData.validity_righteye == 0)
                {
                    x = gazeData.x_gazepos_righteye;
                    y = gazeData.y_gazepos_righteye;
                    distance = gazeData.distance_righteye;
                }
                else if (gazeData.validity_lefteye == 0 && gazeData.validity_righteye == 0)
                {
                    x = (gazeData.x_gazepos_lefteye + gazeData.x_gazepos_righteye) / 2;
                    y = (gazeData.y_gazepos_lefteye + gazeData.y_gazepos_righteye) / 2;
                    distance = (gazeData.distance_lefteye + gazeData.distance_righteye) / 2;
                }
                else
                {
                    valid = false;
                    x = y = 0.0F;
                    distance = 400.0F;
                }

                // Most of the changes are here
                now += 1;

                // to distinguish an actual blink from other noise
                if (blink > 4)
                {
                    blink_now += 1;
                    blink = 0;
                }


                gaze_x_valid.Add(x * pixel_x);
                gaze_y_valid.Add(y * pixel_y);


                if (gaze_x_valid.Count > 1 && gaze_y_valid.Count > 1)
                {
                    d = Math.Pow((Math.Pow((gaze_x_valid[now - 1] - gaze_x_valid[now - 2]), 2) + Math.Pow((gaze_y_valid[now - 1] - gaze_y_valid[now - 2]), 2)), 0.5);
                }

                if (d <= 35)
                {
                    fixation_duration += 8.3;
                    fixation_point_x_now = gaze_x_valid.GetRange(0, now);
                    fixation_point_y_now = gaze_y_valid.GetRange(0, now);
                }
                else
                {
                    fixation_duration = 0.0;
                    if (fixation_point_x_now.Last() % 2 == 0)
                    {
                        fixation_point_x = fixation_point_x_now[(int)(fixation_point_x_now.Count() - 1) / 2] / pixel_x;
                        fixation_point_y = fixation_point_y_now[(int)(fixation_point_y_now.Count() - 1) / 2] / pixel_y;
                    }
                    else if (fixation_point_x_now.Count() > 1)
                    {
                        fixation_point_x = ((fixation_point_x_now[(int)(fixation_point_x_now.Count() - 1) / 2] + fixation_point_x_now[1 + (int)(fixation_point_x_now.Count() - 1) / 2]) / 2.0) / pixel_x;
                        fixation_point_y = ((fixation_point_y_now[(int)(fixation_point_y_now.Count() - 1) / 2] + fixation_point_y_now[1 + (int)(fixation_point_y_now.Count() - 1) / 2]) / 2.0) / pixel_y;
                    }
                }

                // Set position, size and color of gaze form
                if (valid)
                {
                    gazeForm.Width = gazeForm.Height = (int)((distance - 400) * 0.3);
                    gazeForm.Left = (int)(x * Screen.PrimaryScreen.Bounds.Width) - gazeForm.Width / 2;
                    gazeForm.Top = (int)(y * Screen.PrimaryScreen.Bounds.Height) - gazeForm.Height / 2;
                    gazeForm.BackColor = Color.FromArgb((int)distance % 255, 255 - ((int)distance % 255), 50);
                }
                else
                {
                    gazeForm.Left = (int)(Screen.PrimaryScreen.Bounds.Width / 2 - gazeForm.Width / 2);
                    gazeForm.Top = (int)(Screen.PrimaryScreen.Bounds.Height / 2 - gazeForm.Height / 2);
                    gazeForm.BackColor = Color.Blue;
                }
            }

            logger.setETData(gazeData);
		}

		#endregion


		#region Calibration events

		private void tetCalibProcEvents_OnCalibrationEnd(int result)
		{
			// Calibration ended, hide the calibration window and update the calibration plot
			tetCalibProc.WindowVisible = false;
			UpdateCalibPlot();
		}

		private void tetCalibProcEvents_OnKeyDown(int virtualKeyCode)
		{
			// Interrupt the calibration on key events
			if (tetCalibProc.IsCalibrating) tetCalibProc.InterruptCalibration(); // Will trigger OnCalibrationEnd
		}

		#endregion

        #region CostumeCalibration

        private void calibrationPointAdd(float calibationPointX, float calibationPointY)
        {
            try
            {
                tetClient.AddCalibrationPoint(calibationPointX, calibationPointY, 6, true);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        //calibrate on the object with two points
        private void button1_Click(object sender, EventArgs e)
        {
            if (!tetClient.IsConnected) tetClient.Connect(GetConnectionString(), (int)TetConstants.TetConstants_DefaultServerPort, TetSynchronizationMode.TetSynchronizationMode_Local);
            tetClient.ClearCalibration();
            calibPintNum = 0;
            MessageBox.Show("start calibration?");
            calibrationPointAdd(0.5f, 0.5f);
        }


        private void tetClientEvents_OnAddCalibrationPointStarted()
        {
            //label7.Text = "Calibration started";
        }

        private void tetClientEvents_OnAddCalibrationPointEnded(int result)
        {

            calibPintNum += 1;

            if (calibPintNum >=5)
            {
                if (!tetClient.IsAddingCalibrationPoint)
                {
                    tetClient.CalculateAndSetCalibration();
                    tetClient.SaveCalibrationToFile("calibration2");
                    tetClient.GetCalibrationResult("calibration2");
                }
                MessageBox.Show("calibration finished");
            }
            else
            {
                DialogResult calibResult = MessageBox.Show("continue to the next point?", "continue?", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (calibResult == DialogResult.Yes)
                {
                    if (calibPintNum == 1)
                    {
                        calibrationPointAdd(0.1f, 1/14f);
                    }
                    else if (calibPintNum == 2)
                    {
                        calibrationPointAdd(0.9f, 1/14f);
                    }
                    else if (calibPintNum == 3)
                    {
                        calibrationPointAdd(0.1f, 13/14f);
                    }
                    //else if (calibPintNum == 4)
                    //{
                    //    calibrationPointAdd(0.6f, 0.4f);
                    //}
                    //else if (calibPintNum == 5)
                    //{
                    //    calibrationPointAdd(0.6f, 0.8f);
                    //}
                    //else if (calibPintNum == 6)
                    //{
                    //    calibrationPointAdd(0.8f, 0.2f);
                    //}
                    //else if (calibPintNum == 7)
                    //{
                    //    calibrationPointAdd(0.8f, 0.6f);
                    //}
                    else
                    {
                        calibrationPointAdd(0.9f, 13/14f);
                    }
                }
                else
                {
                    MessageBox.Show("calibration ended.");
                }
            }
        }

        #endregion

        private void useManualIp_CheckedChanged(object sender, EventArgs e)
        {
            eyetrackers.Enabled = !useManualIp.Checked;
            serverAddressTextBox.Enabled = useManualIp.Checked;
            UpdateButtons();
        }

        private void serverAddressTextBox_TextChanged(object sender, EventArgs e)
        {
            UpdateButtons();
        }

        private static void TimedEvent(object source, ElapsedEventArgs e)
        {
        }

        private void button2_Click(object sender, EventArgs e)
        {
            socketser = new SocketDLL.ServerSocket(port, 1, "");
            clientData = new SocketDLL.TrialData(true, true, true, true, "", 0);
            socketser.Server.Listen(10);
            //clienterPha = 
            socketser.acceptConnection(clienterPha);
            clienterPha = socketser.Client;
            socketser.acceptConnection(clienterCamera);
            clienterCamera = socketser.Client;
        }

    }
}
