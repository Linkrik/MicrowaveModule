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
            textBlockTemperature.Text = Convert.ToString(InterfacingPCWithGene2.requestTemperCode(UserControlConnect.ComPort));
        }

        /// <summary>
        /// Запрос АЦП
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonRequestAdcCode_Click(object sender, RoutedEventArgs e)
        {   
            byte[] Adc = new byte[3];
            Adc =InterfacingPCWithGene2.requestAdcCode(UserControlConnect.ComPort);
            textBlockAdcHigh.Text = Convert.ToString(Adc[2]); //старший байт
            textBlockAdcLow.Text = Convert.ToString(Adc[1]);  //младший байт
        }


        /// <summary>
        /// при запросе полной таблицы коэффициентов
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
