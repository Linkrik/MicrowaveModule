using MicrowaveModule.UserControl;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading;
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

        /// <summary>
        /// Запрос температуры
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void requestTemperature_Click(object sender, RoutedEventArgs e)
        {
            UserControlControl.timer.Stop();
            while(UserControlControl.sensorDataBackgroundWorker.IsBusy)
            { }
            
            textBlockTemperature.Text = Convert.ToString(InterfacingPCWithGene2.requestTemperCode(UserControlConnect.ComPort));
            
            UserControlControl.timer.Start();
        }

        /// <summary>
        /// Запрос АЦП
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonRequestAdcCode_Click(object sender, RoutedEventArgs e)
        {
            UserControlControl.timer.Stop();
            while (UserControlControl.sensorDataBackgroundWorker.IsBusy)
            { }
            
            byte[] Adc = new byte[3];
            Adc=InterfacingPCWithGene2.requestAdcCode(UserControlConnect.ComPort);
            textBlockAdcHigh.Text = Convert.ToString(Adc[1]); //старший байт
            textBlockAdcLow.Text = Convert.ToString(Adc[2]);  //младший байт
            
            UserControlControl.timer.Start();
        }


        /// <summary>
        /// метод для выставления коректного адреса чтения/записи
        /// </summary>
        private void ReadWriteOneCoefficient()
        {
            textBoxRequestReadingOneCoefficient.TextChanged -= new System.Windows.Controls.TextChangedEventHandler(this.textBoxRequestReadingOneCoefficient_TextChanged);  // отключаем событие изменения textbox
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
                    adressCoef = number;
                }
                else
                {
                    number = 65535;
                    textBoxRequestReadingOneCoefficient.Text = Convert.ToString(number);
                    adressCoef = number;
                }
            }
            else
            {
                textBoxRequestReadingOneCoefficient.Text = Convert.ToString(adressCoef);
            }
            textBoxRequestReadingOneCoefficient.TextChanged += new System.Windows.Controls.TextChangedEventHandler(this.textBoxRequestReadingOneCoefficient_TextChanged);  // включает событие изменения textbox
            if (UserControlConnect.ComPort.IsOpen)
            {
                UserControlControl.timer.Stop();
                while (UserControlControl.sensorDataBackgroundWorker.IsBusy)
                { }

                InterfacingPCWithGene2.requestSingleCode(UserControlConnect.ComPort, adressCoef);

                UserControlControl.timer.Start();
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
                UserControlControl.timer.Stop();
                while (UserControlControl.sensorDataBackgroundWorker.IsBusy)
                { }

                InterfacingPCWithGene2.recordOfOneCoefficient(UserControlConnect.ComPort, adressCoef, Coef);

                UserControlControl.timer.Start();
            }
        }

        /// <summary>
        /// нажатие кнопки увиличения адреса коэффициента
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonUpRequestReadingOneCoefficient_Click(object sender, RoutedEventArgs e)
        {
            if (adressCoef != 65535)
            {
                adressCoef++;
            }
            textBoxRequestReadingOneCoefficient.Text = Convert.ToString(adressCoef);

            if (UserControlConnect.ComPort.IsOpen)
            {
                UserControlControl.timer.Stop();
                while (UserControlControl.sensorDataBackgroundWorker.IsBusy)
                { }

                InterfacingPCWithGene2.requestSingleCode(UserControlConnect.ComPort, adressCoef);

                UserControlControl.timer.Start();
            }
        }

        /// <summary>
        ///  нажатие кнопки уменьшения адреса коэффициента
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonDownRequestReadingOneCoefficient_Click(object sender, RoutedEventArgs e)
        {
            if (adressCoef != 0)
            {
                adressCoef--;
            }
            textBoxRequestReadingOneCoefficient.Text = Convert.ToString(adressCoef);

            if (UserControlConnect.ComPort.IsOpen)
            {
                UserControlControl.timer.Stop();
                while (UserControlControl.sensorDataBackgroundWorker.IsBusy)
                { }

                InterfacingPCWithGene2.requestSingleCode(UserControlConnect.ComPort, adressCoef);

                UserControlControl.timer.Start();
            }
        }

        /// <summary>
        /// событие изменения  текст бокса адреса
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void textBoxRequestReadingOneCoefficient_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (textBoxRequestReadingOneCoefficient.Text != "")
            {
                ReadWriteOneCoefficient();
            }
        }

        /// <summary>
        /// нажатие на кнопку чтения полной таблицы коэффициентов
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonRequestReadingOddsTable_Click(object sender, RoutedEventArgs e)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();

            UserControlControl.timer.Stop();
            while (UserControlControl.sensorDataBackgroundWorker.IsBusy)
            { }

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

            UserControlControl.timer.Start();
        }

        /// <summary>
        /// нажатие кнопки запроса одного коэффициента 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonRecordOfOneCoefficient_Click(object sender, RoutedEventArgs e)
        {
            UserControlControl.timer.Stop();
            while (UserControlControl.sensorDataBackgroundWorker.IsBusy)
            { }

            InterfacingPCWithGene2.recordOfOneCoefficient(UserControlConnect.ComPort, adressCoef, Coef);

            UserControlControl.timer.Start();
        }

        /// <summary>
        /// нажатие кнопки увеличения адреса
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonUpRequestWriteOneCoefficient_Click(object sender, RoutedEventArgs e)
        {
            if (Coef != 255)
            {
                Coef++;
            }
            textBoxRequestWriteOneCoefficient.Text = Convert.ToString(Coef);

            if (UserControlConnect.ComPort.IsOpen)
            {
                UserControlControl.timer.Stop();
                while (UserControlControl.sensorDataBackgroundWorker.IsBusy)
                { }

                InterfacingPCWithGene2.recordOfOneCoefficient(UserControlConnect.ComPort, adressCoef, Coef);

                UserControlControl.timer.Start();
            }
        }

        /// <summary>
        ///  нажатие кнопки уменьшения адреса
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonDownRequestWriteOneCoefficient_Click(object sender, RoutedEventArgs e)
        {
            if (Coef != 0)
            {
                Coef--;
            }
            textBoxRequestWriteOneCoefficient.Text = Convert.ToString(Coef);

            if (UserControlConnect.ComPort.IsOpen)
            {
                UserControlControl.timer.Stop();
                while (UserControlControl.sensorDataBackgroundWorker.IsBusy)
                { }

                InterfacingPCWithGene2.recordOfOneCoefficient(UserControlConnect.ComPort, adressCoef, Coef);

                UserControlControl.timer.Start();
            }
        }

        /// <summary>
        /// изменение текст бокса адреса
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void textBoxRequestWriteOneCoefficient_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (textBoxRequestWriteOneCoefficient.Text != "")
            {
                OneCoefficient();
            }
        }

        /// <summary>
        /// при запросе полной таблицы коэффициентов
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonOddsTableEntry_Click(object sender, RoutedEventArgs e)
        {
            UserControlControl.timer.Stop();
            while (UserControlControl.sensorDataBackgroundWorker.IsBusy)
            { }

            byte[] CorrectCodes = new byte[262144];
            
                for (int i = 0; i < 262144; i++)
                {
                    CorrectCodes[i] = (byte)(i % 256);
                }

            string respond = InterfacingPCWithGene2.programmingWorkTable(UserControlConnect.ComPort, CorrectCodes);
            MessageBox.Show(respond);

            UserControlControl.timer.Start();
        }

        /// <summary>
        /// при запросе одного коэффициента
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonRequestReadingOneCoefficient_Click(object sender, RoutedEventArgs e)
        {
            UserControlControl.timer.Stop();
            while (UserControlControl.sensorDataBackgroundWorker.IsBusy)
            { }

            InterfacingPCWithGene2.requestSingleCode(UserControlConnect.ComPort, adressCoef);

            UserControlControl.timer.Start();
        }
    }
}
