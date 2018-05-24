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
        private byte verpack { get; set; }          // version of packet
        private UInt64 Packet_Time { get; set; }     // Time create packet
        private float Latitude { get; set; }        // Latitude (format DDMM.mm)
        private float Longitude { get; set; }       // Longitude (format DDMM.mm)
        private UInt16 Altitude { get; set; }       // Altitude
        private byte fix { get; set; }              // fix pos
        private byte hdop { get; set; }             // hdop
        private byte Num { get; set; }              // Number of Base station
        private UInt32 PLMN { get; set; }			// PLMN (Code country and code operator)
        List<CellInfo> cellInfo = new List<CellInfo>();

        public PacketLocation (string[] dataPack)
        {
            Parameter p;
            int n = 0;

            int length = dataPack.Length;

            if (parsing(dataPack) == false)
            {
                MessageBox.Show("Not correct data", "Warning", MessageBoxButton.OK);
                return;
            }

            data = new string[length - 4 - 5];
            for (int i = 5; i < length - 4; i++)
                data[i - 5] = dataPack[i];

            string s = getStr(ref n, sizeof(byte));
            verpack = Convert.ToByte(s, 16);
            
            s = getStr(ref n, sizeof(Int64));
            Packet_Time = Convert.ToUInt64(s, 16);

            s = getStr(ref n, sizeof(float));
            Latitude = ConvertHexStr(s);

            s = getStr(ref n, sizeof(float));
            Longitude = ConvertHexStr(s);

            s = getStr(ref n, sizeof(UInt16));
            Altitude = Convert.ToUInt16(s, 16);

            s = getStr(ref n, sizeof(byte));
            fix = Convert.ToByte(s, 16);

            s = getStr(ref n, sizeof(byte));
            hdop = Convert.ToByte(s, 16);

            s = getStr(ref n, sizeof(byte));
            Num = Convert.ToByte(s, 16);

            s = getStr(ref n, sizeof(UInt32));
            PLMN = Convert.ToUInt32(s, 16);

            p = getCCID(data, n);
            list.Add(p);
            n += 20;

            if (Num > 0)
            {
                for (int i = 0; i < (Num & 0x7F); i++)
                {
                    CellInfo cell = new CellInfo();
                    s = getStr(ref n, cell.SizeLac());
                    cell.SetLAC(s);

                    s = getStr(ref n, cell.SizeCell());
                    cell.SetCell(s);

                    s = getStr(ref n, cell.SizeRSSI());
                    cell.SetRssi(s);

                    cellInfo.Add(cell);
                }
            }

            p = TimestampToDate(Packet_Time);
            list.Add(p);

            p = new Parameter { Param = "Version packet", Value = verpack.ToString() };
            list.Add(p);

            p = new Parameter { Param = "Latitude", Value = Latitude.ToString() };
            list.Add(p);

            p = new Parameter { Param = "Longitude", Value = Longitude.ToString() };
            list.Add(p);

            p = new Parameter { Param = "Altitude", Value = Altitude.ToString() };
            list.Add(p);

            p = new Parameter { Param = "FIX", Value = String.Format("{0}", Convert.ToChar(fix)) };
            list.Add(p);

            p = new Parameter { Param = "HDOP", Value = hdop.ToString() };
            list.Add(p);

            p = new Parameter { Param = "Num", Value = (Num & 0x7F).ToString() };
            list.Add(p);

            p = new Parameter { Param = "PLMN", Value = PLMN.ToString("X") };
            list.Add(p);

            int k = 1;
            foreach (CellInfo ci in cellInfo)
            {
                p = new Parameter { Param = "Cell_" + k.ToString() };
                p.Value = "LAC = " + ci.LAC.ToString("X");
                p.Value += " CELL_ID = " + ci.CELL_ID.ToString("X");
                p.Value += " RSSI = " + ci.RSSI.ToString();
                list.Add(p);

                k++;
            }
        }
    }
}
