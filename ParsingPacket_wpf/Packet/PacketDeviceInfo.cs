using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ParsingPacket_wpf.Packet
{
    class PacketDeviceInfo : PacketBase
    {
        private enum E_Device_Error : byte
        {
            NONE = 0x00,              //!< No error
            GPS = 0x01,               //!< Error GPS
            LBS_2G = 0x02,            //!< NO LBS in GSM mode
            LBS_3G = 0x03,            //!< NO LBS in UMTS mode
            LBS_CUR = 0x04,           //!< NO current LBS

            BOOT_SwitchOff = 0x05,    //!< Device boot from SwitchOff state
            BOOT_SOS = 0x06,          //!< Device boot from SOS state
            BOOT_Active = 0x07,       //!< Device boot from Activity state
            BOOT_Passive = 0x08,      //!< Device boot from Passive state
            BOOT_GPRS = 0x09,         //!< Device boot with GPRS Error

            FTP_PUTLOG_ERROR = 0x0A,  //!< Can't send Log to FTP

            MODEM_UPDATE_ERROR = 0x0B,//!< Can't update modem

            ACCL_ERROR = 0x0C,        //!< Accelerometr don't work
            BOOT_REBOOT_SMS = 0x0D,   //!< Device boot from received SMS Reboot

            MAX
        };

        enum E_Device_Warning : byte
        {
            NONE = 0x00,                      //!< No warning
            Device_SwitchOff = 0x01,          //!< Device to SwitchOff
            Device_WakeUp = 0x02,             //!< Device is WakeUp
            Device_Active = 0x03,             //!< Device to Active
            Device_Passive = 0x04,            //!< Device to Passive
            Device_Start = 0x05,              //!< Device is Started
            Low_Battery = 0x06,               //!< Low Charge Battery on device

            MODEM_UPDATE_EQUAL_VERSION = 0x07,//!< Modem have last version
            MODEM_UPDATE_NOT_CHARGING = 0x08, //!< Update modem when device not charging
            MODEM_UPDATE_LOW_CHARGE = 0x09,   //!< Update modem when low charge battery
            MODEM_UPDATE_OLD_VERSION = 0x0A,  //!< Modem have newer version

            Geofence_ALARM = 0x0B,            //!< Device out geoFence

            MAX
        };

        enum E_Device_Info : byte
        {
            NONE = 0x00,      //!< No info
            SOS_Get = 0x01,      //!< Device get SMS with SOS and LIGHT_OFF
            UPD_Get = 0x02,      //!< Device get SMS with UPDATE and LIGHT_OFF

            FirstStart = 0x03,   //!< Device start is first

            FTP_PUTLOG_OK = 0x04,//!< Device send LOG to FTP successfully

            Charge_ON = 0x05,    //!< Device stand up to charge
            Charge_OFF = 0x06,   //!< Device finished charge or Device taken from charge

            FTP_PARTLOG = 0x07,  //!< Device send part og Log

            MODEM_UPDATE_SUCCESSEFUL = 0x08, //!< Modem update is successful or this flag used for LIGHT_ON for Device_Active

            SOS_LIGHT_ON = 0x09, //!< Device get SMS with SOS and LIGHT_ON
            UPD_LIGHT_ON = 0x0A, //!< Device get SMS with UPDATE and LIGHT_ON

            SET_LIGHT_ON = 0x0B,    ///< Flag - get command to switch ON leds
            SET_LIGHT_OFF = 0x0C,   ///< Flag - get command to switch OFF leds
            GEO_FENCE_ON = 0x0D,    ///< Flag - get command GeoFence enable
            GEO_FENCE_OFF = 0x0E,   ///< Flag - get command GeoFence disable
            GET_UPDATE = 0x0F,      ///< Flag - get command to Update
            GET_OPTIONS = 0x10,     ///< Flag - get command to Request Options

            Try_Connect_To_New_Operator = 0x11, ///< Device trying reconnect to new operator
            Change_Operator = 0x12, ///< Device reconnect to new operator

            MAX
        };


        private UInt64 Packet_Time;         // Time create packet
        private Byte Num;                  // Number of Error/Warning/Info (now it always equals 1)
        private E_Device_Error Error;      // Type Error
        private E_Device_Warning Warning;  // Type Warning
        private E_Device_Info Info;		// Type Info
        private byte[] CCID = new byte[20];       // CCID of device

        public PacketDeviceInfo(List<byte> data)
        {
            if (Parsing(ref data) == false)
            {
                MessageBox.Show("Not correct data", "Warning", MessageBoxButton.OK);
                return;
            }
            Packet_Time = GetUInt64(ref data);
            Num = GetByte(ref data);

            Error = (E_Device_Error)GetByte(ref data);
            Warning = (E_Device_Warning)GetByte(ref data);
            Info = (E_Device_Info)GetByte(ref data);

            for (int i = 0; i < CCID.Length; i++)
            {
                CCID[i] = GetByte(ref data);
            }

            CRC32 = GetUInt32(ref data);
        }

        public List<Parameter> GetListParam()
        {
            SetBaseParam();

            Parameter p;

            p = TimestampToDate(Packet_Time);
            list.Add(p);

            p = new Parameter { Param = "", Value = "DATA:" };
            list.Add(p);

            p = new Parameter { Param = "Num of ", Value = Num.ToString() };
            list.Add(p);

            p = new Parameter { Param = "Error", Value = Error.ToString() };
            list.Add(p);

            p = new Parameter { Param = "Warning", Value = Warning.ToString() };
            list.Add(p);

            p = new Parameter { Param = "Info", Value = Info.ToString() };
            list.Add(p);

            p = GetCCID_Byte(ref CCID);
            list.Add(p);

            return list;
        }
    }
}
