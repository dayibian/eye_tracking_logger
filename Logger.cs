using System;
using System.IO;
using System.Collections.Generic;
//using System.Linq;
using System.Text;
using SocketDLL;
using TetComp;
using System.Diagnostics;
using System.Windows.Forms;
using System.Drawing;

namespace Eye_Tracker_Component_C_Sharp_NET
{
    public class Logger
    {
        private List<TetGazeData> raw_data = new List<TetGazeData>();
        private string Data_base_dir = 
            "C:/Users/biand/Documents/Affective_touch/Eye_tracking/EyetrackerComponents.Net_log/data/";
        private string rawDataFileName = "RawLog.dat";
        private StreamWriter raw_writer = null;
        DateTime _starttime = new DateTime();
        Stopwatch _stopwatch = null;
        private string ref_time;
        private int screen_width = 1920;
        private int screen_height = 1080;
        private int number = 0;

        public Logger(string _rawLogFileName = "")
        {
            if (_rawLogFileName != "")
            {
                rawDataFileName = _rawLogFileName;
            }

            // Initialize the log file here
            string raw = Data_base_dir + rawDataFileName;

            raw_writer = File.CreateText(raw);

            // Start a timer here
            _starttime = DateTime.UtcNow;
            _stopwatch = Stopwatch.StartNew();
            // Get the screen resolution for logging
            Rectangle screenSize = Screen.PrimaryScreen.Bounds;
            screen_width = screenSize.Width; //1680; // Depends on monitor size, change to an easily located constant later
            screen_height = screenSize.Height;//1050; // same as above
        }

        public void setETData(TetGazeData _raw)
        {
            raw_data.Add(_raw);
        }

        public void log()
        {
            // Now, log the raw Tobii eye tracker data in the Tobii Studio format
            string dat = "";
            DateTime highresDT = _starttime.AddTicks(_stopwatch.Elapsed.Ticks);
            DateTime cstTime = highresDT.ToLocalTime();

            string timeStamp = cstTime.ToString("MM-dd-yyyy_HH-mm-ss-ffff");    //the time when the whole experiment is finished
            char[] delim = { '_' };
            string[] parts = timeStamp.Split(delim);
            string date = parts[0];
            string time = parts[1];
            // First, log Header, just to confirm to Tobii Studio standard
            dat = "System Properties:";
            raw_writer.WriteLine(dat);
            dat = "Operating System:  Microsoft Windows";
            raw_writer.WriteLine(dat);
            dat = "System User Name:  biand";
            raw_writer.WriteLine(dat);
            dat = "Machine Name:  RASL-2";
            raw_writer.WriteLine(dat);
            dat = " ";
            raw_writer.WriteLine(dat);
            dat = "Data properties:";
            raw_writer.WriteLine(dat);
            dat = " ";
            raw_writer.WriteLine(dat);
            dat = "Recording name:  " + rawDataFileName;
            raw_writer.WriteLine(dat);
            dat = "Recording date:  " + date;
            raw_writer.WriteLine(dat);
            dat = "Recording time:  " + time;
            raw_writer.WriteLine(dat);
            dat = "Recording resolution:  " + screen_width + " x " + screen_height;
            raw_writer.WriteLine(dat);
            dat = " ";
            raw_writer.WriteLine(dat);
            dat = " ";
            raw_writer.WriteLine(dat);
            dat = "Reference time:  " + ref_time;
            raw_writer.WriteLine(dat);
            dat = " ";
            raw_writer.WriteLine(dat);
            dat = "Participant:";
            raw_writer.WriteLine(dat);

            dat = " ";
            raw_writer.WriteLine(dat);
            dat = "Timestamp TimestampSec TimestampUSec Number GazePointXLeft GazePointYLeft CamXLeft CamYLeft DistanceLeft PupilLeft ValidityLeft GazePointXRight GazePointYRight CamXRight CamYRight DistanceRight PupilRight ValidityRight";
            raw_writer.WriteLine(dat);

            foreach (TetGazeData data in raw_data)
            {
                // Now, start logging the actual data
                dat = "";
                // Log the time stamp in msec
                int time_stamp = (int)(100 * (data.timestamp_sec + (data.timestamp_microsec / (1e6))));
                dat += (time_stamp + " ");
                dat += (data.timestamp_sec + " ");
                dat += (data.timestamp_microsec + " ");
                // Log the number
                dat += (number + " ");
                number++;
                // Log the left eye data
                dat += (data.x_gazepos_lefteye + " ");
                dat += (data.y_gazepos_lefteye + " ");
                dat += (data.x_camerapos_lefteye + " ");
                dat += (data.y_camerapos_lefteye + " ");
                dat += (data.distance_lefteye + " ");
                dat += (data.diameter_pupil_lefteye + " ");
                dat += (data.validity_lefteye + " ");
                // Log the right eye data
                dat += (data.x_gazepos_righteye + " ");
                dat += (data.y_gazepos_righteye + " ");
                dat += (data.x_camerapos_righteye + " ");
                dat += (data.y_camerapos_righteye + " ");
                dat += (data.distance_righteye + " ");
                dat += (data.diameter_pupil_righteye + " ");
                dat += (data.validity_righteye + " ");

                // finally write this line
                raw_writer.WriteLine(dat);
            }
            raw_data.Clear();
        }

        public void set_refTime(string _ref_time)
        {
            ref_time = _ref_time;
        }

        public void close()
        {
            raw_writer.Close();
        }
    }
}
