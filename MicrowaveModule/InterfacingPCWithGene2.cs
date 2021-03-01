using Microsoft.Azure.Devices;
using NuGet.Protocol.Plugins;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Ports;
using System.Text;
using System.Windows;

namespace MicrowaveModule.UserControl
{
    class InterfacingPCWithGene2
    {


        private static bool flagParcelArrived = false;             //флаг прихода сообщения в буффер
        private static bool flagMethodIsAlreadyRunning = false;    //флаг говорящий о том, запущена команда(метод) или нет (ПОКА НЕ ИСПОЛЬЗУЮ)

        static public void myPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        { flagParcelArrived = true; } //метод при срабатывании события получения посылки в буфер.

        /// <summary>
        /// I. device identification
        /// </summary>
        /// <param name="ComPort"> device </param>
        /// <returns></returns>
        public static string[] testConnection(SerialPort ComPort)
        {
            flagMethodIsAlreadyRunning = true;

            byte[] byteToWrite = { 0x49 };
            byte[] bytesToRead = new byte[19];

            string name = "";
            string serialNumber = "";
            string versionFW = "";

            string[] response = new string[5];


            try
            {
                ComPort.Write(byteToWrite,0,1);
            }
            catch (Exception ex)
            {

                MessageBox.Show("(1)Ошибка записи данных в порт:\n  байт byteToWrite" + "\n" + ex.Message);
                response[0] = "Ошибка записи данных в порт:";
                response[1] = "байт byteToWrite";
                response[2] = ex.Message;
                response[3] = "";
                flagMethodIsAlreadyRunning = false;
                return response;
            }

            for (int i = 0; i < 19; i++)
            {
                
                try
                {
                    ComPort.Read(bytesToRead, i, 1);
                }
                catch (Exception ex)
                {

                    MessageBox.Show("(2)Ошибка чтения данных с порта:\n Номер байта " + (i).ToString() + "\n" + ex.Message);
                    response[0] = "Ошибка чтения данных с порта:";
                    response[1] = "Номер байта " + (i).ToString();
                    response[2] = ex.Message;
                    response[3] = "";
                    flagMethodIsAlreadyRunning = false;
                    return response;
                }

                if ((0 < i) & (i < 6))
                {
                    //name += System.Text.Encoding.ASCII.GetString(bytesToRead, i, 1);
                    name += System.Text.Encoding.ASCII.GetString(bytesToRead, i, 1);
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
            flagMethodIsAlreadyRunning = false;
            return response;
        }

        /// <summary>
        /// II. attenuator control
        /// </summary>
        /// <param name="ComPort">device</param>
        /// <param name="commands">команда управления аттенюатором [0;63]</param>
        public static void sendControlAttDAC(SerialPort ComPort, byte commands)
        {
            flagMethodIsAlreadyRunning = true;
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
                    MessageBox.Show("(3)Ошибка записи данных в порт:\n Номер байта " + (i).ToString() + "\n" + ex.Message);
                    flagMethodIsAlreadyRunning = false;
                    return;
                }
                try
                {
                    ComPort.Read(bytesToRead, i, 1);

                }
                catch (Exception ex)
                {
                    MessageBox.Show("(4)Ошибка чтения данных в порт:\n Номер байта " + (i).ToString() + "\n" + ex.Message);
                    flagMethodIsAlreadyRunning = false;
                    return;
                }

                if (bytesToRead[0] != bytesToWrite[0])
                {
                    MessageBox.Show("(5)Ошибка передачи данных:\n Номер байта " + (i + 1).ToString() + "\n Переданный байт " + bytesToWrite[i].ToString() + "\n Принятый байт " + bytesToRead[i]);
                    flagMethodIsAlreadyRunning = false;
                    return;
                }
            }
            flagMethodIsAlreadyRunning = false;
        }

        /// <summary>
        /// III. (управление затворным напряжением)
        /// </summary>
        /// <param name="ComPort"> device </param>
        /// <param name="command"> attenuator control command </param>
        public static void sendControlGateVoltage(SerialPort ComPort, int command)
        {
            flagMethodIsAlreadyRunning = true;
            int N = 3;
            byte[] bytesToWrite = new byte[N];
            byte[] bytesToRead = new byte[N-2];
            byte[] bytesComand = BitConverter.GetBytes(command);
            bytesToWrite[0] = 0x47; // "G" (код команды) 
            bytesToWrite[1] = bytesComand[0];      // младший  байт адреса 12-ти битного кода ЦАП (0, 1, 2… 255)
            bytesToWrite[2] = bytesComand[1];      // старший байт адреса 12-ти битного кода ЦАП (0, 16, 32… 240)
            

            for (int i = 0; i < N; i++)
            {
                try
                {
                    ComPort.Write(bytesToWrite, i, 1);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("(6)Ошибка записи данных в порт:\n Номер байта " + (i).ToString() + "\n" + ex.Message);
                    flagMethodIsAlreadyRunning = false;
                    return;
                }

                if (i==0)
                {
                    try
                    {
                        ComPort.Read(bytesToRead, 0, 1);

                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("(7)Ошибка чтения данных в порт:\n Номер байта " + (i).ToString() + "\n" + ex.Message);
                        flagMethodIsAlreadyRunning = false;
                        return;
                    }
                }
            }

            flagMethodIsAlreadyRunning = false;
        }

        /// <summary>
        /// IV. (управление питанием)
        /// </summary>
        /// <param name="ComPort">device</param>
        /// <param name="commands">comands control pover [0;3]]</param>
        public static void sendControlPower(SerialPort ComPort, byte commands)
        {
            flagMethodIsAlreadyRunning = true;
            int N = 2;
            byte[] bytesToWrite = new byte[N];
            byte[] bytesToRead = new byte[N];
            bytesToWrite[0] = 0x44; // 68
            bytesToWrite[1] = commands; //0,1,2,3

            for (int i = 0; i < N; i++)
            {

                try
                {
                    ComPort.Write(bytesToWrite, i, 1);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("(9)Ошибка записи данных в порт:\n Номер байта " + (i).ToString() + "\n" + ex.Message);
                    flagMethodIsAlreadyRunning = false;
                    return;
                }

                if (i == 0)
                {
                    try
                    {
                        ComPort.Read(bytesToRead, 0, 1);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("(10)Ошибка чтения данных в порт:\n Номер байта " + (i).ToString() + "\n" + ex.Message);
                        flagMethodIsAlreadyRunning = false;
                        return;
                    }
                }

                if (bytesToRead[0] != bytesToWrite[0])
                {
                    MessageBox.Show("(11)Ошибка передачи данных:\n Номер байта " + (i + 1).ToString() + "\n Переданный байт " + bytesToWrite[i].ToString() + "\n Принятый байт " + bytesToRead[i]);
                }
            }
            flagMethodIsAlreadyRunning = false;
        }

        /// <summary>
        /// V. (запрос температуры)
        /// </summary>
        /// <param name="ComPort">device</param>
        /// <returns></returns>
        public static double requestTemperCode(SerialPort ComPort)
        {
            flagMethodIsAlreadyRunning = true;
            int N = 3;
            double constTempCoeff = 0.00390625;
            byte[] bytesToWrite = new byte[N-2];
            byte[] bytesToRead = new byte[N];
            uint temperatureByte = 0;
            bytesToWrite[0] = 0x4B; // "K" (код команды)
            //bytesToWrite[1] = 75; // 


            try
            {
                ComPort.Write(bytesToWrite, 0, 1);
            }
            catch (Exception ex)
            {
                MessageBox.Show("(12)Ошибка записи данных в порт:\n Номер байта "  + "\n" + ex.Message);
                flagMethodIsAlreadyRunning = false;
                return 0;
            }

            try
            {
                ComPort.Read(bytesToRead, 0, 1);

            }
            catch (Exception ex)
            {
                MessageBox.Show("(13)Ошибка чтения данных в порт:\n Номер байта "  + "\n" + ex.Message);
                flagMethodIsAlreadyRunning = false;
                return 0;
            }

            try
            {
                ComPort.Read(bytesToRead, 1, 1);

            }
            catch (Exception ex)
            {
                MessageBox.Show("(13)Ошибка чтения данных в порт:\n Номер байта " +  "\n" + ex.Message);
                flagMethodIsAlreadyRunning = false;
                return 0;
            }

            try
            {
                ComPort.Read(bytesToRead, 2, 1);
            }
            catch (Exception ex)
            {
                MessageBox.Show("(13)Ошибка чтения данных в порт:\n Номер байта " + "\n" + ex.Message);
                flagMethodIsAlreadyRunning = false;
                return 0;
            }

            temperatureByte = bytesToRead[1];
            temperatureByte += Convert.ToUInt32(bytesToRead[2] << 8);

            int val = Convert.ToInt32(BitOperator.ExtractNumber(temperatureByte, 0, 15));
            uint sign = BitOperator.ExtractNumber(temperatureByte, 15, 1);


            if (sign == 1)
            {
                val *= -1;
            }
 

            flagMethodIsAlreadyRunning = false;
            return val * constTempCoeff;
        }

        /// <summary>
        /// VI. (запрос данных с АЦП)
        /// </summary>
        /// <param name="ComPort">device</param>
        /// <returns></returns>
        public static byte [] requestAdcCode(SerialPort ComPort)
        {
            flagMethodIsAlreadyRunning = true;
            int N = 3;
            byte[] bytesToWrite = new byte[N];
            byte[] bytesToRead = new byte[N];

            bytesToWrite[0] = 0x41; // "A" (код команды)
            bytesToWrite[1] = 0; // Старший байт АЦП
            bytesToWrite[2] = 0; // Младший байт АЦП

            try
            {
                ComPort.Write(bytesToWrite, 0, 1);
            }
            catch (Exception ex)
            {
                MessageBox.Show("(15)Ошибка записи данных в порт:\n Номер байта " + "\n" + ex.Message);
                flagMethodIsAlreadyRunning = false;
                return bytesToRead;
            }

            try
            {
                ComPort.Read(bytesToRead, 0, 1);
            }
            catch (Exception ex)
            {
                MessageBox.Show("(16)Ошибка чтения данных в порт:\n Номер байта " + "\n" + ex.Message);
                flagMethodIsAlreadyRunning = false;
                return bytesToRead;
            }


            try
            {
                ComPort.Read(bytesToRead, 1, 1);
            }
            catch (Exception ex)
            {
                MessageBox.Show("(16)Ошибка чтения данных в порт:\n Номер байта " + "\n" + ex.Message);
                flagMethodIsAlreadyRunning = false;
                return bytesToRead;
            }


            try
            {
                ComPort.Read(bytesToRead, 2, 1);
            }
            catch (Exception ex)
            {
                MessageBox.Show("(16)Ошибка чтения данных в порт:\n Номер байта " + "\n" + ex.Message);
                flagMethodIsAlreadyRunning = false;
                return bytesToRead;
            }

           
            flagMethodIsAlreadyRunning = false;
            return bytesToRead;
        }

        /// <summary>
        /// VII. (Запрос одного коэффициента)
        /// </summary>
        /// <param name="ComPort">device</param>
        /// <param name="address">адресс коэффициента</param>
        /// <returns></returns>
        public static byte requestSingleCode(SerialPort ComPort, int address)
        {
            flagMethodIsAlreadyRunning = true;
            int N = 4;
            byte[] bytesToWrite = new byte[N];
            byte[] bytesToRead = new byte[N];
            bytesToWrite[0] = 0x72; // "s" (код команды)
            bytesToWrite[1] = (byte)(address / 256); // старший байт адреса
            bytesToWrite[2] = (byte)(address % 256); // младший байт адреса
            bytesToWrite[3] = 0x0; // 0 Код коэффициента

            for (int i = 0; i < N; i++)
            {
                try
                {
                    ComPort.Write(bytesToWrite, i, 1);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("(18)Ошибка записи данных в порт:\n Номер байта " + (i).ToString() + "\n" + ex.Message);
                    flagMethodIsAlreadyRunning = false;
                    return 0;
                }

                try
                {
                    ComPort.Read(bytesToRead, i, 1);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("(19)Ошибка чтения данных в порт:\n Номер байта " + (i).ToString() + "\n" + ex.Message);
                    flagMethodIsAlreadyRunning = false;
                    return 0;
                }

                if (i < N - 1)
                {
                    if (bytesToRead[i] != bytesToWrite[i])
                    {
                        MessageBox.Show("(20)Ошибка передачи данных:\n Номер байта " + (i + 1).ToString() + "\n Переданный байт " + bytesToWrite[i].ToString() + "\n Принятый байт " + bytesToRead[i]);
                        flagMethodIsAlreadyRunning = false;
                        return 0;
                    }
                }
            }
            flagMethodIsAlreadyRunning = false;
            return bytesToRead[3];
        }

        /// <summary>
        /// VIII. (запрос полной таблицы коэффициентов)
        /// </summary>
        /// <param name="ComPort">device</param>
        /// <returns></returns>
        public static byte[] requestWorkTable(SerialPort ComPort)
        {
            flagMethodIsAlreadyRunning = true;
            int N = 65537;
            byte[] bytesToWrite = new byte[N];
            byte[] bytesToRead = new byte[N];
            bytesToWrite[0] = 0x52; // "R" (код команды)
            //for (int i = 0; i < N; i++)
            //{
            //    bytesToWrite[i] = (byte)(i % 256);
            //}
            for (int i = 0; i < N; i++)
            {
                try
                {
                    ComPort.Write(bytesToWrite, i, 1);
                }
                catch (Exception ex)
                {
                    ComPort.Close();
                    MessageBox.Show("(21)Ошибка записи данных в порт:\n Номер байта " + (i).ToString() + "\n" + ex.Message);
                    flagMethodIsAlreadyRunning = false;
                    return bytesToRead;
                }

                try
                {
                    ComPort.Read(bytesToRead, i, 1);
                }
                catch (Exception ex)
                {
                    ComPort.Close();
                    MessageBox.Show("(22)Ошибка чтения данных в порт:\n Номер байта " + (i).ToString() + "\n" + ex.Message);
                    flagMethodIsAlreadyRunning = false;
                    return bytesToRead;
                }
            }
            flagMethodIsAlreadyRunning = false;
            return bytesToRead;
        }

        /// <summary>
        /// IX. (запись одного коэффициента)
        /// </summary>
        /// <param name="ComPort">device</param>
        /// <param name="address">адрес</param>
        /// <param name="CorrectCodes">коэффициент</param>
        /// <returns></returns>
        public static void recordOfOneCoefficient(SerialPort ComPort,int address, byte CorrectCodes)
        {
            flagMethodIsAlreadyRunning = true;
            int N = 5;
            byte[] bytesToWrite = new byte[N];
            byte[] bytesToRead = new byte[N];
            bytesToWrite[0] = 0x6D; // "m" (код команды)
            bytesToWrite[1] = 0x6D; // код подтверждения посылки записи одного коэффициента;
            bytesToWrite[2] = (byte)(address / 256); // старший байт адреса
            bytesToWrite[3] = (byte)(address % 256); // младший байт адреса
            bytesToWrite[4] = CorrectCodes;

            for (int i = 0; i < N; i++)
            {
                try
                {
                    ComPort.Write(bytesToWrite, i, 1);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("(23)Ошибка записи данных в порт:\n Номер байта " + (i).ToString() + "\n" + ex.Message);
                    flagMethodIsAlreadyRunning = false;
                    return;
                }


                try
                {
                    ComPort.Read(bytesToRead, i, 1);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("(24)Ошибка чтения данных в порт:\n Номер байта " + (i).ToString() + "\n" + ex.Message);
                    flagMethodIsAlreadyRunning = false;
                    return;
                }

                if (i < N - 1)
                {
                    if (bytesToRead[i] != bytesToWrite[i])
                    {
                        MessageBox.Show("(25)Ошибка передачи данных:\n Номер байта " + (i + 1).ToString() + "\n Переданный байт " + bytesToWrite[i].ToString() + "\n Принятый байт " + bytesToRead[i]);
                    }
                }
            }
            flagMethodIsAlreadyRunning = false;
        }

        /// <summary>
        /// X. (запись полной таблицы коэффициентов)
        /// </summary>
        /// <param name="ComPort">device</param>
        /// <param name="CorrectCodes">коэффициенты</param>
        /// <returns></returns>
        public static string programmingWorkTable(SerialPort ComPort, byte[] CorrectCodes)
        {
            flagMethodIsAlreadyRunning = true;
            int N = 65538;
            byte[] bytesToWrite = new byte[N];
            byte[] bytesToRead = new byte[N];
            bytesToWrite[0] = 0x4D; // "M" (код команды)
            bytesToWrite[1] = 0x4D; // "M" (код команды) Код подтверждения посылки $4D (M)

            for (int i = 0; i < CorrectCodes.Length; i++)
            {
                bytesToWrite[i + 2] = CorrectCodes[i];
            }

            for (int i = 0; i < N; i++)
            {
                try
                {
                    ComPort.Write(bytesToWrite, i, 1);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("(26)Ошибка записи данных в порт:\n Номер байта " + (i).ToString() + "\n" + ex.Message);
                    flagMethodIsAlreadyRunning = false;
                    return "Ошибка";
                }


                try
                {
                    ComPort.Read(bytesToRead, i, 1);

                }
                catch (Exception ex)
                {
                    MessageBox.Show("(27)Ошибка чтения данных в порт:\n Номер байта " + (i).ToString() + "\n" + ex.Message);
                    flagMethodIsAlreadyRunning = false;
                    return "Ошибка";
                }

                if (bytesToRead[i] != bytesToWrite[i])
                {
                    MessageBox.Show("(28)Ошибка передачи данных:\n Номер байта " + (i + 1).ToString() + "\n Переданный байт " + bytesToWrite[i].ToString() + "\n Принятый байт " + bytesToRead[i]);
                    return "Ошибка";
                }
            }
            flagMethodIsAlreadyRunning = false;
            return "Данные переданы успешно.";
        }
    }
}
