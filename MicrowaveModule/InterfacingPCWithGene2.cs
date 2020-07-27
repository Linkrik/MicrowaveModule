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
        /// I. device identification
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
                    MessageBox.Show("(1)Ошибка записи данных в порт:\n Номер байта " + (i).ToString() + "\n" + ex.Message);
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
                    MessageBox.Show("(2)Ошибка чтения данных с порта:\n Номер байта " + (i).ToString() + "\n" + ex.Message);
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
        /// II. attenuator control
        /// </summary>
        /// <param name="ComPort">device</param>
        /// <param name="commands">команда управления аттенюатором [0;63]</param>
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
                    MessageBox.Show("(3)Ошибка записи данных в порт:\n Номер байта " + (i).ToString() + "\n" + ex.Message);

                }
                try
                {
                    ComPort.Read(bytesToRead, i, 1);

                }
                catch (Exception ex)
                {
                    MessageBox.Show("(4)Ошибка чтения данных в порт:\n Номер байта " + (i).ToString() + "\n" + ex.Message);

                }

                if (bytesToRead[i] != bytesToWrite[i])
                {
                    MessageBox.Show("(5)Ошибка передачи данных:\n Номер байта " + (i + 1).ToString() + "\n Переданный байт " + bytesToWrite[i].ToString() + "\n Принятый байт " + bytesToRead[i]);

                }
            }
        }

        /// <summary>
        /// III. (управление затворным напряжением)
        /// </summary>
        /// <param name="ComPort"> device </param>
        /// <param name="command"> attenuator control command </param>
        public static void sendControlGateVoltage(SerialPort ComPort, byte command1, byte command2)
        {
            int N = 3;
            byte[] bytesToWrite = new byte[N];
            byte[] bytesToRead = new byte[N];
            bytesToWrite[0] = 0x47; // "C" (код команды) 
            bytesToWrite[1] = command1; // старший байт адреса 12-ти битного кода ЦАП (0, 1, 2… 255)
            bytesToWrite[2] = command2; // младший байт адреса 12-ти битного кода ЦАП (0, 16, 32… 240)



            for (int i = 0; i < N; i++)
            {
                try
                {
                    ComPort.Write(bytesToWrite, i, 1);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("(6)Ошибка записи данных в порт:\n Номер байта " + (i).ToString() + "\n" + ex.Message);

                }
                try
                {
                    ComPort.Read(bytesToRead, i, 1);

                }
                catch (Exception ex)
                {
                    MessageBox.Show("(7)Ошибка чтения данных в порт:\n Номер байта " + (i).ToString() + "\n" + ex.Message);

                }

                if (bytesToRead[i] != bytesToWrite[i])
                {
                    MessageBox.Show("(8)Ошибка передачи данных:\n Номер байта " + (i + 1).ToString() + "\n Переданный байт " + bytesToWrite[i].ToString() + "\n Принятый байт " + bytesToRead[i]);

                }
            }
        }

        /// <summary>
        /// IV. (управление питанием)
        /// </summary>
        /// <param name="ComPort">device</param>
        /// <param name="commands">comands control pover [0;3]]</param>
        public static void sendControlPower(SerialPort ComPort, byte commands)
        {
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

                }
                try
                {
                    ComPort.Read(bytesToRead, i, 1);

                }
                catch (Exception ex)
                {
                    MessageBox.Show("(10)Ошибка чтения данных в порт:\n Номер байта " + (i).ToString() + "\n" + ex.Message);

                }

                if (bytesToRead[i] != bytesToWrite[i])
                {
                    MessageBox.Show("(11)Ошибка передачи данных:\n Номер байта " + (i + 1).ToString() + "\n Переданный байт " + bytesToWrite[i].ToString() + "\n Принятый байт " + bytesToRead[i]);
                }
            }
        }

        /// <summary>
        /// V. (запрос температуры)
        /// </summary>
        /// <param name="ComPort">device</param>
        /// <returns></returns>
        public static byte requestTemperCode(SerialPort ComPort)
        {
            int N = 2;
            byte[] bytesToWrite = new byte[N];
            byte[] bytesToRead = new byte[N];
            bytesToWrite[0] = 0x4B; // "K" (код команды)
            bytesToWrite[1] = 75; // 

            for (int i = 0; i < N; i++)
            {
                try
                {
                    ComPort.Write(bytesToWrite, i, 1);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("(12)Ошибка записи данных в порт:\n Номер байта " + (i).ToString() + "\n" + ex.Message);

                }
                try
                {
                    ComPort.Read(bytesToRead, i, 1);

                }
                catch (Exception ex)
                {
                    MessageBox.Show("(13)Ошибка чтения данных в порт:\n Номер байта " + (i).ToString() + "\n" + ex.Message);

                }

                if (i < N - 1)
                {
                    if (bytesToRead[i] != bytesToWrite[i])
                    {
                        MessageBox.Show("(14)Ошибка передачи данных:\n Номер байта " + (i + 1).ToString() + "\n Переданный байт " + bytesToWrite[i].ToString() + "\n Принятый байт " + bytesToRead[i]);
                        return 0;
                    }
                }
            }
            return bytesToRead[1];
        }

        /// <summary>
        /// VI. (запрос данных с АЦП)
        /// </summary>
        /// <param name="ComPort">device</param>
        /// <returns></returns>
        public static byte [] requestAdcCode(SerialPort ComPort)
        {
            int N = 3;
            byte[] bytesToWrite = new byte[N];
            byte[] bytesToRead = new byte[N];
            byte[] bytesToReadADC = {0,0};
            bytesToWrite[0] = 0x41; // "A" (код команды)
            bytesToWrite[1] = 0; // Старший байт АЦП
            bytesToWrite[2] = 0; // Младший байт АЦП

            for (int i = 0; i < N; i++)
            {
                try
                {
                    ComPort.Write(bytesToWrite, i, 1);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("(15)Ошибка записи данных в порт:\n Номер байта " + (i).ToString() + "\n" + ex.Message);

                }
                try
                {
                    ComPort.Read(bytesToRead, i, 1);

                }
                catch (Exception ex)
                {
                    MessageBox.Show("(16)Ошибка чтения данных в порт:\n Номер байта " + (i).ToString() + "\n" + ex.Message);

                }

                if (i < N - 1)
                {
                    if (bytesToRead[i] != bytesToWrite[i])
                    {
                        MessageBox.Show("(17)Ошибка передачи данных:\n Номер байта " + (i + 1).ToString() + "\n Переданный байт " + bytesToWrite[i].ToString() + "\n Принятый байт " + bytesToRead[i]);
                        return bytesToReadADC;
                    }
                }

                if (i>0)
                {

                    bytesToReadADC[i] = bytesToWrite[i];
                }

            }
            return bytesToReadADC;
        }

        /// <summary>
        /// VII. (Запрос одного коэффициента)
        /// </summary>
        /// <param name="ComPort">device</param>
        /// <param name="address">адресс коэффициента</param>
        /// <returns></returns>
        public static byte requestSingleCode(SerialPort ComPort, int address)
        {
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
                    return 0;
                }
                try
                {
                    ComPort.Read(bytesToRead, i, 1);

                }
                catch (Exception ex)
                {
                    MessageBox.Show("(19)Ошибка чтения данных в порт:\n Номер байта " + (i).ToString() + "\n" + ex.Message);
                    return 0;
                }

                if (i < N - 1)
                {
                    if (bytesToRead[i] != bytesToWrite[i])
                    {
                        MessageBox.Show("(20)Ошибка передачи данных:\n Номер байта " + (i + 1).ToString() + "\n Переданный байт " + bytesToWrite[i].ToString() + "\n Принятый байт " + bytesToRead[i]);
                        return 0;
                    }
                }
            }
            return bytesToRead[3];
        }

        /// <summary>
        /// VIII. (запрос полной таблицы коэффициентов)
        /// </summary>
        /// <param name="ComPort">device</param>
        /// <returns></returns>
        public static byte[] requestWorkTable(SerialPort ComPort)
        {
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
                    return bytesToRead;
                }
            }
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

                }
                try
                {
                    ComPort.Read(bytesToRead, i, 1);

                }
                catch (Exception ex)
                {
                    MessageBox.Show("(24)Ошибка чтения данных в порт:\n Номер байта " + (i).ToString() + "\n" + ex.Message);

                }

                if (i < N - 1)
                {
                    if (bytesToRead[i] != bytesToWrite[i])
                    {
                        MessageBox.Show("(25)Ошибка передачи данных:\n Номер байта " + (i + 1).ToString() + "\n Переданный байт " + bytesToWrite[i].ToString() + "\n Принятый байт " + bytesToRead[i]);

                    }
                }
            }
        }

        /// <summary>
        /// X. (запись полной таблицы коэффициентов)
        /// </summary>
        /// <param name="ComPort">device</param>
        /// <param name="CorrectCodes">коэффициенты</param>
        /// <returns></returns>
        public static string programmingWorkTable(SerialPort ComPort, byte[] CorrectCodes)
        {
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
                    return "Ошибка";
                }
                try
                {
                    ComPort.Read(bytesToRead, i, 1);

                }
                catch (Exception ex)
                {
                    MessageBox.Show("(27)Ошибка чтения данных в порт:\n Номер байта " + (i).ToString() + "\n" + ex.Message);
                    return "Ошибка";
                }

                if (bytesToRead[i] != bytesToWrite[i])
                {
                    MessageBox.Show("(28)Ошибка передачи данных:\n Номер байта " + (i + 1).ToString() + "\n Переданный байт " + bytesToWrite[i].ToString() + "\n Принятый байт " + bytesToRead[i]);
                    return "Ошибка";
                }
            }
            return "Данные переданы успешно.";
        }
    }
}
