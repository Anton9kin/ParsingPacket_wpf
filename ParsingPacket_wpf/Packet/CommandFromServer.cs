using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParsingPacket_wpf.Packet
{
    class CommandFromServer
    {
        public enum Type {
            NULL = 0x00,    ///< No commands

            Get_TRANSPORT_Statistic = 0x01, ///< Get UDP statistic
            Get_TRANSPORT_Error = 0x02, ///< Get TRANSPORT_Error statistic
            Switch_OFF_Device = 0x03,   ///< Command on Switch off device
            Request_ActLoc = 0x04,  ///< Command for send current activity and Location
            Request_Update = 0x05,  ///< Command for Request update
            SetSOS = 0x06,  ///< Set mode of SOS
            RequestOptions = 0x07,  ///< Command for request options
            RequestDeviceError = 0x08,  //!< Command for Request Device Error (not release)


            /** \details
            * kal_uint32 RX_packets = porog_low_activity;
            *
            * kal_uint32 TX_packets = porog_high_activity;
            *
            * kal_uint32 out_bytes = time gps find;
            *
            * kal_uint32 in_bytes = time switch off (to deep sleep);
            */
            GetPorogAndTime = 0x09, ///< Get porog and time of device (not release)


            /** \details
            * kal_uint32 RX_packets = ECP_VoltageDischarge;
            *
            * kal_uint32 TX_packets = ECP_VoltageUpdate;
            *
            * kal_uint32 out_bytes = ECP_VoltageSendUDP;
            *
            * kal_uint32 in_bytes = ECP_VoltageSwitchOFF
            */
            GetParamVoltage = 0x0A, ///< Get param of voltage (not release)


            GetLog = 0x0B, ///< Get Log-file to FTP
            SetSOSParam = 0x0C, ///< Set parameters for SOS mode
            SetUPDParam = 0x0D, ///< Set parameters for Activity mode
            Switch2G3 = 0x0E, //!< Switch to 2G/3G (not release)
            RebootDevice = 0x0F, ///< Command for Reboot device


            /**
            * \details
            * kal_uint32 RX_packets = ActivityRate;
            * kal_uint32 TX_packets = LocationRate;
            * kal_uint32 out_bytes = reserved;
            * kal_uint32 in_bytes = reserved
            *
            */
            Get_ActLocRate = 0x10, ///< Get timeout for Activity/Location Rate (not release)

            SetActLocRate = 0x11, //!< Set Activity/Location Period
            DeleteLog = 0x12, ///< Command for delete Log
            GetLogPart = 0x13, ///< Get part of Log
            UpdateModem = 0x14, ///< Command for update modem


            /** \details kal_uint32 RX_packets = Mishiko.bootStatus
            */
            GETStatusDevice = 0x15, ///< Get status device


            /** \details CommandFrom.Data = porogAccelerometr
            */
            PorogAcelerometr = 0x16,  ///< Set Porog Acelerometr


            /** \details
            * kal_uint32 RX_packets = Algorithm Version / Version Modem;
            * kal_uint32 TX_packets = Firmware Version / Revision number;
            * kal_uint32 out_bytes = reserved
            * kal_uint32 in_bytes = reserved
            */
            DeviceVers = 0x17, ///< Get Device Version


            /** \details
            * CommandFrom.Data = number of corners
            * If geofence is enable then send four pointer
            */
            GeoFence = 0x18, ///< Enable/Disable GeoFence


            /** \details
            * Data = turn 0-off, 1-on for lights
            */
            SwitchLight = 0x19, ///< Enable/Disable Indication in Activity mode
            MAX
        };

        public Type command { get; set; }
        public UInt32 Data { get; set; }
        public List<Parameter> list { get; set; } = new List<Parameter>();

        public CommandFromServer(string[] str, int i) {
            Parameter param = new Parameter { Param = "", Value = "" };
            list.Add(param);

            command = CheckCommand(Byte.Parse(str[i], System.Globalization.NumberStyles.HexNumber));
            Data = UInt32.Parse(str[i + 4] + str[i + 3] + str[i + 2] + str[i + 1], System.Globalization.NumberStyles.HexNumber);

            param = new Parameter { Param = "Command server", Value = command.ToString() };

            if (command != Type.GeoFence)
                list.Add(param);

            if (command != Type.NULL)
            {
                switch (command)
                {
                    case Type.GeoFence:
                        if (Data != 0)
                        {
                            param.Value += "_ON";
                            list.Add(param);

                            string s;
                            float Lat, Long;
                            Byte index;

                            for (int k = 0; k < Data; k++)
                            {
                                s = str[5 * k + 8] + str[5 * k + 7] + str[5 * k + 6] + str[5 * k + 5];
                                Lat = ConvertHexStr(s);
                                index = Byte.Parse(str[5 * k + 9], System.Globalization.NumberStyles.HexNumber);
                                s = str[5 * k + 13] + str[5 * k + 12] + str[5 * k + 11] + str[5 * k + 10];
                                Long = ConvertHexStr(s);
                                param = new Parameter { Param = "Corner " + index.ToString(), Value = Lat.ToString() + " " + Long.ToString() };
                                list.Add(param);
                            }

                        }
                        else
                        {
                            param.Value += "_OFF";
                            list.Add(param);
                        }
                        break;

                    case Type.GetLogPart:
                        param = new Parameter { Param = "Size get log", Value = Data.ToString() + " bytes" } ;
                        list.Add(param);
                        break;

                    case Type.SetActLocRate:
                        param = new Parameter { Param = "Rate", Value = Data.ToString() };
                        list.Add(param);
                        break;

                    case Type.SetSOS: case Type.SetSOSParam:
                        param = new Parameter { Param = "Rate", Value = (Data & 0xFFFF).ToString() };
                        list.Add(param);

                        param = new Parameter { Param = "Period", Value = (Data >> 16).ToString() };
                        list.Add(param);
                        break;

                    case Type.SetUPDParam:
                        param = new Parameter { Param = "Rate", Value = Data.ToString() };
                        list.Add(param);
                        break;

                    case Type.SwitchLight:
                        param = new Parameter { Param = "", Value = "Indication_" + (Data == 0 ? "OFF" : "ON") };
                        list.Add(param);
                        break;
                }
            }
        }

        private Type CheckCommand(Byte comm) {
            Type c = (Type)comm;
            
            foreach (Type tp in Enum.GetValues(typeof(Type)))
            {
                if (tp == c)
                    return c;
            }

            return Type.NULL;
        }

        public float ConvertHexStr(string s)
        {
            Int32 d = Convert.ToInt32(s, 16);
            double f = Convert.ToDouble(d);

            byte[] bytes = BitConverter.GetBytes(d);
            return BitConverter.ToSingle(bytes, 0);
        }
    }
}
