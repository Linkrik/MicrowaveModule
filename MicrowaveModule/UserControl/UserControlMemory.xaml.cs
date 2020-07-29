using MicrowaveModule.UserControl;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MicrowaveModule
{
    /// <summary>
    /// Логика взаимодействия для UserControlMemory.xaml
    /// </summary>
    public partial class UserControlMemory //: UserControl
    {
        public UserControlMemory()
        {
            InitializeComponent();
        }

        private int adressCoef = 0;  //адрес для чтения и записи коэффициента
        private byte Coef = 0;        //коэффициент для записи

        private void requestTemperature_Click(object sender, RoutedEventArgs e)
        {
            textBlockTemperature.Text = Convert.ToString(InterfacingPCWithGene2.requestTemperCode(UserControlConnect.ComPort));
        }

        private void buttonRequestAdcCode_Click(object sender, RoutedEventArgs e)
        {
            byte [] Adc = InterfacingPCWithGene2.requestAdcCode(UserControlConnect.ComPort);
            textBlockAdcHigh.Text = Convert.ToString(Adc[0]); //старший байт
            textBlockAdcLow.Text = Convert.ToString(Adc[1]);  //младший байт
        }

        /// <summary>
        /// метод для выставления коректного адреса чтения/записи
        /// </summary>
        private void ReadWriteOneCoefficient()
        {
            int number;
            if (int.TryParse(textBoxRequestReadingOneCoefficient.Text, out number))
            {
                if (number >= 0 && number <= 65535)
                {
                    textBoxRequestReadingOneCoefficient.Text = Convert.ToString(number);
                    adressCoef = number;
                }
                else if (number < 0)
                {
                    number = 0;
                    textBoxRequestReadingOneCoefficient.Text = Convert.ToString(number);
                    adressCoef = Convert.ToInt32(number);
                }
                else
                {
                    number = 65535;
                    textBoxRequestReadingOneCoefficient.Text = Convert.ToString(number);
                    adressCoef = Convert.ToInt32(number);
                }
            }
            else
            {
                textBoxRequestReadingOneCoefficient.Text = Convert.ToString(adressCoef);
            }

            if (UserControlConnect.ComPort.IsOpen)
            {
                InterfacingPCWithGene2.requestSingleCode(UserControlConnect.ComPort, adressCoef);
            }
        }

        /// <summary>
        /// метод для выставления коректного коэффициента
        /// </summary>
        private void OneCoefficient()
        {
            int number;
            if (int.TryParse(textBoxRequestWriteOneCoefficient.Text, out number))
            {
                if (number >= 0 && number <= 255)
                {
                    textBoxRequestWriteOneCoefficient.Text = Convert.ToString(number);
                    Coef = Convert.ToByte(number);
                }
                else if (number < 0)
                {
                    number = 0;
                    textBoxRequestWriteOneCoefficient.Text = Convert.ToString(number);
                    Coef = Convert.ToByte(number);
                }
                else
                {
                    number = 255;
                    textBoxRequestWriteOneCoefficient.Text = Convert.ToString(number);
                    Coef = Convert.ToByte(number);
                }
            }
            else
            {
                textBoxRequestWriteOneCoefficient.Text = Convert.ToString(Coef);
            }

            if (UserControlConnect.ComPort.IsOpen)
            {
                InterfacingPCWithGene2.recordOfOneCoefficient(UserControlConnect.ComPort, adressCoef, Coef);
            }
        }

        private void buttonUpRequestReadingOneCoefficient_Click(object sender, RoutedEventArgs e)
        {
            if (adressCoef != 65535)
            {
                adressCoef++;
            }
            textBoxRequestReadingOneCoefficient.Text = Convert.ToString(adressCoef);

            if (UserControlConnect.ComPort.IsOpen)
            {
                InterfacingPCWithGene2.requestSingleCode(UserControlConnect.ComPort, adressCoef);
            }
        }

        private void buttonDownRequestReadingOneCoefficient_Click(object sender, RoutedEventArgs e)
        {
            if (adressCoef != 0)
            {
                adressCoef--;
            }
            textBoxRequestReadingOneCoefficient.Text = Convert.ToString(adressCoef);

            if (UserControlConnect.ComPort.IsOpen)
            {
                InterfacingPCWithGene2.requestSingleCode(UserControlConnect.ComPort, adressCoef);
            }
        }

        private void textBoxRequestReadingOneCoefficient_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (textBoxRequestReadingOneCoefficient.Text != "")
            {
                ReadWriteOneCoefficient();
            }
        }

        private void buttonRequestReadingOddsTable_Click(object sender, RoutedEventArgs e)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            int N = 65537;
            byte[] returnFullCode = new byte[N];
            returnFullCode = InterfacingPCWithGene2.requestWorkTable(UserControlConnect.ComPort);
            string respond = "";
            for (int i = 0; i < 1024; i++)
            {
                respond = respond + returnFullCode[i] + " ";
            }
            respond = respond + "                                                                                      ";
            for (int i = N - 1024; i < N; i++)
            {
                respond = respond + returnFullCode[i] + " ";
            }
            sw.Stop();
            MessageBox.Show(sw.Elapsed.ToString());
            MessageBox.Show(respond);
        }

        private void buttonRecordOfOneCoefficient_Click(object sender, RoutedEventArgs e)
        {
            InterfacingPCWithGene2.recordOfOneCoefficient(UserControlConnect.ComPort, adressCoef, Coef);
        }

        private void buttonUpRequestWriteOneCoefficient_Click(object sender, RoutedEventArgs e)
        {
            if (Coef != 255)
            {
                Coef++;
            }
            textBoxRequestWriteOneCoefficient.Text = Convert.ToString(Coef);

            if (UserControlConnect.ComPort.IsOpen)
            {
                InterfacingPCWithGene2.recordOfOneCoefficient(UserControlConnect.ComPort, adressCoef, Coef);
            }
        }

        private void buttonDownRequestWriteOneCoefficient_Click(object sender, RoutedEventArgs e)
        {
            if (Coef != 0)
            {
                Coef--;
            }
            textBoxRequestWriteOneCoefficient.Text = Convert.ToString(Coef);

            if (UserControlConnect.ComPort.IsOpen)
            {
                InterfacingPCWithGene2.recordOfOneCoefficient(UserControlConnect.ComPort, adressCoef, Coef);
            }
        }

        private void textBoxRequestWriteOneCoefficient_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (textBoxRequestWriteOneCoefficient.Text != "")
            {
                OneCoefficient();
            }
        }

        private void buttonOddsTableEntry_Click(object sender, RoutedEventArgs e)
        {
            byte[] CorrectCodes = new byte[262144];
            
                for (int i = 0; i < 262144; i++)
                {
                    CorrectCodes[i] = (byte)(i % 256);
                }

            string respond = InterfacingPCWithGene2.programmingWorkTable(UserControlConnect.ComPort, CorrectCodes);
            MessageBox.Show(respond);
        }
    }
}
