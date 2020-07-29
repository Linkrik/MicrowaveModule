using MicrowaveModule.UserControl;
using System;
using System.Collections.Generic;
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
    /// Логика взаимодействия для UserControlControl.xaml
    /// </summary>
    public partial class UserControlControl  //: UserControl
    {
        public UserControlControl()
        {
            InitializeComponent();

            int[] DacStep = { 0, 16, 32, 64, 128, 240 };
            comboBoxDacStep.Items.Clear();
            foreach (var item in DacStep)
            {
                comboBoxDacStep.Items.Add(item);
            }
            comboBoxDacStep.Text = "0";
        }


        private byte codeDac = 0;   //значение затворного напряжения cod DAC
        private byte dacStep = 0;   //значение затворного напряжения DAC Step
        private byte valueAtt = 0;  //значение аттенюатора
        private byte valuePow = 0;  //значение питания
        private bool flagEvent = true;


        /// <summary>
        /// управление затворным напряжением
        /// </summary>
        private void NumericUpDownCodeDac_Value()
        {
            int number;
            if (int.TryParse(textBoxCodeDac12Bit.Text, out number))
            {
                if (number >=0 && number <= 255)
                {
                    textBoxCodeDac12Bit.Text = Convert.ToString(number);
                    codeDac = Convert.ToByte(number);
                }
                else if (number < 0)
                {
                    number = 0;
                    textBoxCodeDac12Bit.Text = Convert.ToString(number);
                    codeDac = Convert.ToByte(number);
                }
                else
                {
                    number = 255;
                    textBoxCodeDac12Bit.Text = Convert.ToString(number);
                    codeDac = Convert.ToByte(number);
                }
            }
            else
            {
                textBoxCodeDac12Bit.Text = Convert.ToString(codeDac);
            }

            if (UserControlConnect.ComPort.IsOpen)
            {
                InterfacingPCWithGene2.sendControlGateVoltage(UserControlConnect.ComPort, codeDac, dacStep);
            }
        }

        /// <summary>
        /// перевод из двоичной сис.сч. методом сдвига битов влева (для управления питанием)
        /// </summary>
        private void GenerationNumericUpDownPower_Value()
        {
            valuePow = Convert.ToByte(Convert.ToInt32(checkBoxGeneralPowerSupple.IsChecked) |
                       Convert.ToInt32(checkBoxStdnPower.IsChecked) << 1);
            
            if (UserControlConnect.ComPort.IsOpen)
            {
                InterfacingPCWithGene2.sendControlPower(UserControlConnect.ComPort, valuePow);
            }

        }

        /// <summary>
        /// перевод из двоичной сис.сч. методом сдвига битов влева (для аттенюатором)
        /// </summary>
        private void GenerationNumericUpDownAtt1_Value()
        {

            valueAtt = Convert.ToByte(Convert.ToInt32(checkBoxAtt1bit0.IsChecked) |                 //if (checkBoxAtt1bit0.IsChecked == true) newValue += 1;
                Convert.ToInt32(checkBoxAtt1bit1.IsChecked) << 1 |                        //if (checkBoxAtt1bit1.IsChecked == true) newValue += 2;
                Convert.ToInt32(checkBoxAtt1bit2.IsChecked) << 2 |                        //if (checkBoxAtt1bit2.IsChecked == true) newValue += 4;
                Convert.ToInt32(checkBoxAtt1bit3.IsChecked) << 3 |                        //if (checkBoxAtt1bit3.IsChecked == true) newValue += 8;
                Convert.ToInt32(checkBoxAtt1bit4.IsChecked) << 4 |                        //if (checkBoxAtt1bit4.IsChecked == true) newValue += 16;
                Convert.ToInt32(checkBoxAtt1bit5.IsChecked) << 5);                        //if (checkBoxAtt1bit5.IsChecked == true) newValue += 32;
                textBoxAttenuatorUpDown.Text = Convert.ToString(valueAtt);


            //------------------------------------------------------//
            if (UserControlConnect.ComPort.IsOpen)
            {
                InterfacingPCWithGene2.sendControlAttDAC(UserControlConnect.ComPort, valueAtt);
            }
        }

        /// <summary>
        /// метод распределенния флагов в чек боксах
        /// </summary>
        /// <param name="value"> число для перевода в двоичную сис.сч. </param>
        private void DistributionOfCheckboxes(int value) //метод распределенния флагов в чек боксах
        {
            flagEvent = false;
            if (value / 32 != 0) checkBoxAtt1bit5.IsChecked = true; else checkBoxAtt1bit5.IsChecked = false;
            if (value % 32 / 16 != 0) checkBoxAtt1bit4.IsChecked = true; else checkBoxAtt1bit4.IsChecked = false;
            if (value % 32 % 16 / 8 != 0) checkBoxAtt1bit3.IsChecked = true; else checkBoxAtt1bit3.IsChecked = false;
            if (value % 32 % 16 % 8 / 4 != 0) checkBoxAtt1bit2.IsChecked = true; else checkBoxAtt1bit2.IsChecked = false;
            if (value % 32 % 16 % 8 % 4 / 2 != 0) checkBoxAtt1bit1.IsChecked = true; else checkBoxAtt1bit1.IsChecked = false;
            if (value % 32 % 16 % 8 % 4 % 2 / 1 != 0) checkBoxAtt1bit0.IsChecked = true; else checkBoxAtt1bit0.IsChecked = false;
            flagEvent = true;
        }

        #region Event checkBox Attenuator
        private void checkBoxAtt1bit5_Checked(object sender, RoutedEventArgs e)
        {
            if (flagEvent)
            {
                GenerationNumericUpDownAtt1_Value();
            }
        }

        private void checkBoxAtt1bit4_Checked(object sender, RoutedEventArgs e)
        {
            if (flagEvent)
            {
                GenerationNumericUpDownAtt1_Value();
            }
        }

        private void checkBoxAtt1bit3_Checked(object sender, RoutedEventArgs e)
        {
            if (flagEvent)
            {
                GenerationNumericUpDownAtt1_Value();
            }
        }

        private void checkBoxAtt1bit2_Checked(object sender, RoutedEventArgs e)
        {
            if (flagEvent)
            {
                GenerationNumericUpDownAtt1_Value();
            }
        }

        private void checkBoxAtt1bit1_Checked(object sender, RoutedEventArgs e)
        {
            if (flagEvent)
            {
                GenerationNumericUpDownAtt1_Value();
            }
        }

        private void checkBoxAtt1bit0_Checked(object sender, RoutedEventArgs e)
        {
            if (flagEvent)
            {
                GenerationNumericUpDownAtt1_Value();
            }
        }

        private void checkBoxAtt1bit0_Unchecked(object sender, RoutedEventArgs e)
        {
            if (flagEvent)
            {
                GenerationNumericUpDownAtt1_Value();
            }
        }

        private void checkBoxAtt1bit1_Unchecked(object sender, RoutedEventArgs e)
        {
            if (flagEvent)
            {
                GenerationNumericUpDownAtt1_Value();
            }
        }

        private void checkBoxAtt1bit2_Unchecked(object sender, RoutedEventArgs e)
        {
            if (flagEvent)
            {
                GenerationNumericUpDownAtt1_Value();
            }
        }

        private void checkBoxAtt1bit3_Unchecked(object sender, RoutedEventArgs e)
        {
            if (flagEvent)
            {
                GenerationNumericUpDownAtt1_Value();
            }
        }

        private void checkBoxAtt1bit4_Unchecked(object sender, RoutedEventArgs e)
        {
            if (flagEvent)
            {
                GenerationNumericUpDownAtt1_Value();
            }
        }

        private void checkBoxAtt1bit5_Unchecked(object sender, RoutedEventArgs e)
        {
            if (flagEvent)
            {
                GenerationNumericUpDownAtt1_Value();
            }
        }

        #endregion

        #region Event Up/Down textBox Attenuation
        private void buttonUpTextBoxAtt_Click(object sender, RoutedEventArgs e)
        {

            if (valueAtt != 63)
            {
                valueAtt++;
            }
            DistributionOfCheckboxes(valueAtt);
            GenerationNumericUpDownAtt1_Value();
        }

        private void buttonDownTextBoxAtt_Click(object sender, RoutedEventArgs e)
        {
           
            if (valueAtt != 0)
            {
                valueAtt--;
            }
            DistributionOfCheckboxes(valueAtt);
            GenerationNumericUpDownAtt1_Value();
        }

        private void textBoxAttenuatorUpDown_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (textBoxAttenuatorUpDown.Text!="")
            {
                int number;
                if (Int32.TryParse(textBoxAttenuatorUpDown.Text, out number))
                {
                    if (number >= 0 && number <= 63)
                    {
                        DistributionOfCheckboxes(number);
                        GenerationNumericUpDownAtt1_Value();
                    }
                    else if (number < 0)
                    {
                        DistributionOfCheckboxes(0);
                        GenerationNumericUpDownAtt1_Value();
                    }
                    else
                    {
                        DistributionOfCheckboxes(63);
                        GenerationNumericUpDownAtt1_Value();
                    }
                }
                else
                {
                    textBoxAttenuatorUpDown.Text = Convert.ToString(valueAtt);
                }
            }
        }
        #endregion

        #region Event checkBox Power
        private void checkBoxStdnPower_Checked(object sender, RoutedEventArgs e)
        {
            GenerationNumericUpDownPower_Value();
        }

        private void checkBoxGeneralPowerSupple_Checked(object sender, RoutedEventArgs e)
        {
            GenerationNumericUpDownPower_Value();
        }

        private void checkBoxGeneralPowerSupple_Unchecked(object sender, RoutedEventArgs e)
        {
            GenerationNumericUpDownPower_Value();
        }

        private void checkBoxStdnPower_Unchecked(object sender, RoutedEventArgs e)
        {
            GenerationNumericUpDownPower_Value();
        }

        #endregion

        #region Event Up/Down textBox comboBox управление затворным напряжением
        private void buttonUpTextBoxCodeDac_Click(object sender, RoutedEventArgs e)
        {
            if (codeDac != 255)
            {
                 codeDac++;
            }
            textBoxCodeDac12Bit.Text = Convert.ToString(codeDac);

        }

        private void buttonDownTextBoxCodeDac_Click(object sender, RoutedEventArgs e)
        {
            if (codeDac != 0)
            {
                codeDac--;
            }
            textBoxCodeDac12Bit.Text = Convert.ToString(codeDac);
        }

        private void textBoxCodeDac12Bit_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (textBoxCodeDac12Bit.Text != "")
            {
                    NumericUpDownCodeDac_Value();
            }
        }

        private void comboBoxDacStep_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            dacStep = Convert.ToByte(comboBoxDacStep.SelectedItem);
            NumericUpDownCodeDac_Value();
        }
        #endregion
    }
}
