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
        private enum E_Device_Error
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

        enum E_Device_Warning
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

        enum E_Device_Info
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


        private Int64 Packet_Time { get; set; }         // Time create packet
        private Byte Num { get; set; }                  // Number of Error/Warning/Info (now it always equals 1)
        private E_Device_Error Error { get; set; }      // Type Error
        private E_Device_Warning Warning { get; set; }  // Type Warning
        private E_Device_Info Info { get; set; }		// Type Info

        public PacketDeviceInfo(string[] dataPack)
        {
            Parameter p;
            int length = dataPack.Length;

            if (parsing(dataPack) == false)
            {
                MessageBox.Show("Not correct data", "Warning", MessageBoxButton.OK);
                return;
            }

            data = new string[length - 4 - 5];
            for (int i = 5; i < length - 4; i++)
                data[i - 5] = dataPack[i];

            string s = data[7] + data[6] + data[5] + data[4] + data[3] + data[2] + data[1] + data[0];
            Packet_Time = Convert.ToInt64(s, 16);
            p = TimestampToDate(Packet_Time);
            list.Add(p);

            Num = Convert.ToByte(data[8], 16);
            p = new Parameter { Param = "Num of ", Value = Num.ToString() };
            list.Add(p);

            Error = (E_Device_Error)Convert.ToByte(data[9], 16);
            p = new Parameter { Param = "Error", Value = Error.ToString() };
            list.Add(p);

            Warning = (E_Device_Warning)Convert.ToByte(data[10], 16);
            p = new Parameter { Param = "Warning", Value = Warning.ToString() };
            list.Add(p);

            Info = (E_Device_Info)Convert.ToByte(data[11], 16);
            p = new Parameter { Param = "Info", Value = Info.ToString() };
            list.Add(p);

            p = getCCID(data, 12);
            list.Add(p);
        }
    }
}
