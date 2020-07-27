using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Text;
using System.Windows;

namespace MicrowaveModule.UserControl
{
    class InterfacingPCWithGene2
    {
        /// <summary>
        /// device identification
        /// </summary>
        /// <param name="ComPort"> device </param>
        /// <returns></returns>
        public static string[] testConnection(SerialPort ComPort)
        {
            byte[] bytesToWrite = new byte[20];
            byte[] bytesToRead = new byte[20];

            bytesToWrite[0] = 73;
            bytesToWrite[1] = 0;
            bytesToWrite[2] = 0;
            bytesToWrite[3] = 0;
            bytesToWrite[4] = 0;
            bytesToWrite[5] = 0;
            bytesToWrite[6] = 0;
            bytesToWrite[7] = 0;
            bytesToWrite[8] = 0;
            bytesToWrite[9] = 0;
            bytesToWrite[10] = 0;
            bytesToWrite[11] = 0;
            bytesToWrite[12] = 0;
            bytesToWrite[13] = 0;
            bytesToWrite[14] = 0;
            bytesToWrite[15] = 0;
            bytesToWrite[16] = 0;
            bytesToWrite[17] = 0;
            bytesToWrite[18] = 0;
            bytesToWrite[19] = 0;


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

        /// <summary>
        /// (управление затворным напряжением)
        /// </summary>
        /// <param name="ComPort"> device </param>
        /// <param name="command"> attenuator control command </param>
        public static void sendControlPower(SerialPort ComPort, byte command1, byte command2)
        {
            int N = 3;
            byte[] bytesToWrite = new byte[N];
            byte[] bytesToRead = new byte[N];
            bytesToWrite[0] = 0x44; // "C" (код команды)
            bytesToWrite[1] = command1; // старший байт адреса
            bytesToWrite[2] = command2; // младший байт адреса



            for (int i = 0; i < N; i++)
            {
                try
                {
                    ComPort.Write(bytesToWrite, i, 1);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка записи данных в порт:\n Номер байта " + (i).ToString() + "\n" + ex.Message);

                }
                try
                {
                    ComPort.Read(bytesToRead, i, 1);

                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка чтения данных в порт:\n Номер байта " + (i).ToString() + "\n" + ex.Message);

                }

                if (bytesToRead[i] != bytesToWrite[i])
                {
                    MessageBox.Show("Ошибка передачи данных:\n Номер байта " + (i + 1).ToString() + "\n Переданный байт " + bytesToWrite[i].ToString() + "\n Принятый байт " + bytesToRead[i]);

                }
            }
        }

        /// <summary>
        /// attenuator control
        /// </summary>
        /// <param name="ComPort"></param>
        /// <param name="commands"></param>
        public static void sendControlAttDAC(SerialPort ComPort, byte commands)
        {
            int N = 2;
            byte[] bytesToWrite = new byte[N];
            byte[] bytesToRead = new byte[N];
            bytesToWrite[0] = 0x43; // "C" (код команды)
            bytesToWrite[1] = commands;



            for (int i = 0; i < N; i++)
            {
                try
                {
                    ComPort.Write(bytesToWrite, i, 1);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка записи данных в порт:\n Номер байта " + (i).ToString() + "\n" + ex.Message);

                }
                try
                {
                    ComPort.Read(bytesToRead, i, 1);

                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка чтения данных в порт:\n Номер байта " + (i).ToString() + "\n" + ex.Message);

                }

                if (bytesToRead[i] != bytesToWrite[i])
                {
                    MessageBox.Show("Ошибка передачи данных:\n Номер байта " + (i + 1).ToString() + "\n Переданный байт " + bytesToWrite[i].ToString() + "\n Принятый байт " + bytesToRead[i]);

                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ComPort"></param>
        /// <param name="commands"></param>
        public static void send___(SerialPort ComPort, byte commands)
        {
            int N = 2;
            byte[] bytesToWrite = new byte[N];
            byte[] bytesToRead = new byte[N];
            bytesToWrite[0] = 0x43; // "C" (код команды)
            bytesToWrite[1] = commands;

            for (int i = 0; i < N; i++)
            {
                try
                {
                    ComPort.Write(bytesToWrite, i, 1);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка записи данных в порт:\n Номер байта " + (i).ToString() + "\n" + ex.Message);

                }
                try
                {
                    ComPort.Read(bytesToRead, i, 1);

                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка чтения данных в порт:\n Номер байта " + (i).ToString() + "\n" + ex.Message);

                }

                if (bytesToRead[i] != bytesToWrite[i])
                {
                    MessageBox.Show("Ошибка передачи данных:\n Номер байта " + (i + 1).ToString() + "\n Переданный байт " + bytesToWrite[i].ToString() + "\n Принятый байт " + bytesToRead[i]);

                }
            }
        }

    }
}
