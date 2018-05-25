using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParsingPacket_wpf.Packet
{
    class CommandFromServer
    {
        public enum Type : byte
        {
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

        private Type Command { get; set; }
        private UInt32 CommandData { get; set; }
        public List<Parameter> List { get; set; } = new List<Parameter>();

        public CommandFromServer(List<byte> data)
        {
            Command = CheckCommand(data[0]);
            data.RemoveAt(0);

            byte[] bytes = new byte[4] {data[0], data[1], data[2], data[3] };
            CommandData = BitConverter.ToUInt32(bytes, 0);
            data.RemoveRange(0, 4);

            Parameter p = new Parameter { Param = "Command from server", Value = Command.ToString() };

            if (Command != Type.GeoFence)
                List.Add(p);

            if (Command != Type.NULL)
            {
                switch (Command)
                {
                    case Type.GeoFence:
                        if (CommandData != 0)
                        {
                            p.Value += "_ON";
                            List.Add(p);

                            byte[] bits = new byte[4];
                            GeoFence point;

                            for (int k = 0; k < CommandData; k++)
                            {
                                point = new GeoFence(data);
                                p = new Parameter { Param = "Corner " + point.Index.ToString(), Value = point.Latitude.ToString() + " " + point.Longitude.ToString() };
                                List.Add(p);
                            }

                        }
                        else
                        {
                            p.Value += "_OFF";
                            List.Add(p);
                        }
                        break;

                    case Type.GetLogPart:
                        p = new Parameter { Param = "Size get log", Value = CommandData.ToString() + " bytes" };
                        List.Add(p);
                        break;

                    case Type.SetActLocRate:
                        p = new Parameter { Param = "Rate", Value = CommandData.ToString() };
                        List.Add(p);
                        break;

                    case Type.SetSOS:
                    case Type.SetSOSParam:
                        p = new Parameter { Param = "Rate", Value = (CommandData & 0xFFFF).ToString() };
                        List.Add(p);

                        p = new Parameter { Param = "Period", Value = (CommandData >> 16).ToString() };
                        List.Add(p);
                        break;

                    case Type.SetUPDParam:
                        p = new Parameter { Param = "Rate", Value = CommandData.ToString() };
                        List.Add(p);
                        break;

                    case Type.SwitchLight:
                        p = new Parameter { Param = "", Value = "Indication_" + (CommandData == 0 ? "OFF" : "ON") };
                        List.Add(p);
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
    }
}
