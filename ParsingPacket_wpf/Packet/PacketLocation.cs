using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ParsingPacket_wpf.Packet
{
    class PacketLocation : PacketBase
    {
        private byte VerPack { get; set; }          // version of packet
        private UInt64 Packet_Time { get; set; }     // Time create packet
        private float Latitude { get; set; }        // Latitude (format DDMM.mm)
        private float Longitude { get; set; }       // Longitude (format DDMM.mm)
        private UInt16 Altitude { get; set; }       // Altitude
        private byte Fix { get; set; }              // fix pos
        private byte Hdop { get; set; }             // hdop
        private byte Num { get; set; }              // Number of Base station
        private UInt32 PLMN { get; set; }           // PLMN (Code country and code operator)
        private byte[] CCID = new byte[20];       // CCID of device
        List<CellInfo> cellInfo = new List<CellInfo>();
        

        public PacketLocation(List<byte> data)
        {
            if (Parsing(ref data) == false)
            {
                MessageBox.Show("Not correct data", "Warning", MessageBoxButton.OK);
                return;
            }
            VerPack = WorkBuffer.GetByte(ref data);
            Packet_Time = WorkBuffer.GetUInt64(ref data);
            Latitude = WorkBuffer.GetFloat(ref data);
            Longitude = WorkBuffer.GetFloat(ref data);
            Altitude = WorkBuffer.GetUInt16(ref data);
            Fix = WorkBuffer.GetByte(ref data);
            Hdop = WorkBuffer.GetByte(ref data);
            Num = WorkBuffer.GetByte(ref data);
            Num &= 0x7F;

            PLMN = WorkBuffer.GetUInt32(ref data);
            for (int i = 0; i < CCID.Length; i++)
            {
                CCID[i] = WorkBuffer.GetByte(ref data);
            }

            for (int i = 0; i < (Num); i++)
            {
                cellInfo.Add(new CellInfo(data));
            }

            CRC32 = WorkBuffer.GetUInt32(ref data);
        }

        public List<Parameter> GetListParam()
        {
            SetBaseParam();

            Parameter p;

            p = TimestampToDate(Packet_Time);
            List.Add(p);

            p = new Parameter { Param = "", Value = "DATA:" };
            List.Add(p);

            p = new Parameter { Param = "Version packet", Value = VerPack.ToString() };
            List.Add(p);

            p = new Parameter { Param = "Latitude", Value = Latitude.ToString() };
            List.Add(p);

            p = new Parameter { Param = "Longitude", Value = Longitude.ToString() };
            List.Add(p);

            p = new Parameter { Param = "Altitude", Value = Altitude.ToString() };
            List.Add(p);

            p = new Parameter { Param = "FIX", Value = String.Format("{0}", Convert.ToChar(Fix)) };
            List.Add(p);

            p = new Parameter { Param = "HDOP", Value = Hdop.ToString() };
            List.Add(p);

            p = new Parameter { Param = "Num", Value = (Num & 0x7F).ToString() };
            List.Add(p);

            p = new Parameter { Param = "PLMN", Value = PLMN.ToString("X") };
            List.Add(p);

            int k = 1;
            foreach (CellInfo ci in cellInfo)
            {
                p = new Parameter { Param = "Cell_" + k.ToString() };
                p.Value = "LAC = " + ci.LAC.ToString("X");
                p.Value += " CELL_ID = " + ci.CELL_ID.ToString("X");
                p.Value += " RSSI = " + ci.RSSI.ToString();
                List.Add(p);

                k++;
            }

            p = GetCCID(ref CCID);
            List.Add(p);

            return List;
        }
    }
}
