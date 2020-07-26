using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Text;
using System.Windows;

namespace MicrowaveModule.UserControl
{
    class InterfacingPCWithGene2
    {
        public static string[] testConnection(SerialPort ComPort)
        {
            byte[] bytesToWrite = new byte[20];
            byte[] bytesToRead = new byte[20];

            bytesToWrite[0] = 73;
            bytesToWrite[1] = 88;
            bytesToWrite[2] = 50;
            bytesToWrite[3] = 48;
            bytesToWrite[4] = 48;
            bytesToWrite[5] = 49;
            bytesToWrite[6] = 83;
            bytesToWrite[7] = 78;
            bytesToWrite[8] = 50;
            bytesToWrite[9] = 48;
            bytesToWrite[10] = 50;
            bytesToWrite[11] = 48;
            bytesToWrite[12] = 48;
            bytesToWrite[13] = 48;
            bytesToWrite[14] = 49;
            bytesToWrite[15] = 70;
            bytesToWrite[16] = 87;
            bytesToWrite[17] = 48;
            bytesToWrite[18] = 48;
            bytesToWrite[19] = 49;


            string name = "";
            string serialNumber = "";
            string versionFW = "";

            string[] response = new string[5];


            for (int i = 0; i < 20; i++)
            {
                try
                {
                    ComPort.Write(bytesToWrite, i, 1);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка записи данных в порт:\n Номер байта " + (i).ToString() + "\n" + ex.Message);
                    response[0] = "Ошибка записи данных в порт:";
                    response[1] = "Номер байта " + (i).ToString();
                    response[2] = ex.Message;
                    response[3] = "";

                    return response;
                }
                try
                {
                    ComPort.Read(bytesToRead, i, 1);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка чтения данных с порта:\n Номер байта " + (i).ToString() + "\n" + ex.Message);
                    response[0] = "Ошибка чтения данных с порта:";
                    response[1] = "Номер байта " + (i).ToString();
                    response[2] = ex.Message;
                    response[3] = "";

                    return response;
                }

                if (i < 20 - 1)
                {
                    //bytesToWrite[i + 1] = bytesToRead[i];
                }
                if ((0 < i) & (i < 6))
                {
                    //name += System.Text.Encoding.ASCII.GetString(bytesToRead, i, 1);
                    name += System.Text.Encoding.GetEncoding(1251).GetString(bytesToRead, i, 1);
                }

                if ((5 < i) & (i < 15))
                {
                    serialNumber += System.Text.Encoding.ASCII.GetString(bytesToRead, i, 1);
                }

                if ((14 < i) & (i < 20))
                {
                    versionFW += System.Text.Encoding.ASCII.GetString(bytesToRead, i, 1);
                }
                response[0] = "Связь успешно установленна";
                response[1] = "с устройством";
                response[2] = name;
                response[3] = serialNumber;
                response[4] = versionFW;


            }
            return response;
        }









    }
}
