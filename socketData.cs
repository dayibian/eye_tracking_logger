using System;
using System.Collections.Generic;
using System.Text;

namespace Eye_Tracker_Component_C_Sharp_NET
{
    class socketData: SocketDLL.StreamData
    {
        private float point_X;
        private float point_Y;
        private float point_distance;
        private string time_stamp;

        public socketData(float _point_X,
            float _point_y,
            float _point_distance,
            string _timeStamp):base(_timeStamp)
        {
            point_X = _point_X;
            point_Y = _point_y;
            point_distance = _point_distance;
            time_stamp = _timeStamp;
        }

        //////////////////////////
        public override SocketDLL.StreamData data()
        {
            return this;
        }

        public override SocketDLL.StreamData decode(byte[] byteStream)
        {
            int index_ = 0;
            //String fixationDuration, blinkRate, xPosition, yPosition, timeStamp;
            System.Text.ASCIIEncoding encoding = new System.Text.ASCIIEncoding();
            byte[] space = encoding.GetBytes(" ");
            byte[] newline = encoding.GetBytes("\n");

            point_X = float.Parse(encoding.GetString(Framer.nextToken(byteStream, space, ref index_)));
            point_Y = float.Parse(encoding.GetString(Framer.nextToken(byteStream, space, ref index_)));
            point_distance = float.Parse(encoding.GetString(Framer.nextToken(byteStream, space, ref index_)));
            time_stamp = encoding.GetString(Framer.nextToken(byteStream, newline, ref index_));
            return this;
        }

        public override byte[] encode()
        {
            System.Text.ASCIIEncoding encoding = new System.Text.ASCIIEncoding();
            byte[] buf = null;
            String encodedString = this.point_X.ToString() + " ";
            encodedString = encodedString + this.point_Y.ToString() + " ";
            encodedString = encodedString + this.point_distance.ToString() + " ";
            encodedString = encodedString + this.timeStamp;
            encodedString = encodedString + "\n";

            buf = encoding.GetBytes(encodedString);
            return buf;
        }
    }
}
