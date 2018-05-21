using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ParsingPacket_wpf.Packet
{
    class PacketGenericResponse : PacketBase
    {
        private CommandFromServer comServer;

        public PacketGenericResponse(string[] dataPack)
        {
            int length = dataPack.Length;

            if (parsing(dataPack) == false)
            {
                MessageBox.Show("Not correct data", "Warning", MessageBoxButton.OK);
                return;
            }

            data = new string[length - 4 - 5];
            for (int i = 5; i < length - 4; i++)
                data[i - 5] = dataPack[i];


            comServer = new CommandFromServer(data, 0);
            list.AddRange(comServer.list);
        }
    }
}
