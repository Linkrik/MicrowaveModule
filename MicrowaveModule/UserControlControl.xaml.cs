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
        }

        private int valueAtt = 0;
        private bool flagEvent = true;

        private void GenerationNumericUpDownAtt1_Value()
        {
            int value = 0;
            value = (Convert.ToInt32(checkBoxAtt1bit0.IsChecked) |                 //if (checkBoxAtt1bit0.IsChecked == true) newValue += 1;
                Convert.ToInt32(checkBoxAtt1bit1.IsChecked) << 1 |                        //if (checkBoxAtt1bit1.IsChecked == true) newValue += 2;
                Convert.ToInt32(checkBoxAtt1bit2.IsChecked) << 2 |                        //if (checkBoxAtt1bit2.IsChecked == true) newValue += 4;
                Convert.ToInt32(checkBoxAtt1bit3.IsChecked) << 3 |                        //if (checkBoxAtt1bit3.IsChecked == true) newValue += 8;
                Convert.ToInt32(checkBoxAtt1bit4.IsChecked) << 4 |                        //if (checkBoxAtt1bit4.IsChecked == true) newValue += 16;
                Convert.ToInt32(checkBoxAtt1bit5.IsChecked) << 5);                        //if (checkBoxAtt1bit5.IsChecked == true) newValue += 32;
                textBoxAttenuatorUpDown.Text = Convert.ToString(value);
            valueAtt = value;
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

        private void buttonUpTextBoxAtt_Click(object sender, RoutedEventArgs e)
        {
            flagEvent = false;
            if (valueAtt != 63)
            {
                valueAtt++;
            }
            if (valueAtt / 32 != 0) checkBoxAtt1bit5.IsChecked = true; else checkBoxAtt1bit5.IsChecked = false;
            if (valueAtt % 32 / 16 != 0) checkBoxAtt1bit4.IsChecked = true; else checkBoxAtt1bit4.IsChecked = false;
            if (valueAtt % 32 % 16 / 8 != 0) checkBoxAtt1bit3.IsChecked = true; else checkBoxAtt1bit3.IsChecked = false;
            if (valueAtt % 32 % 16 % 8 / 4 != 0) checkBoxAtt1bit2.IsChecked = true; else checkBoxAtt1bit2.IsChecked = false;
            if (valueAtt % 32 % 16 % 8 % 4 / 2 != 0) checkBoxAtt1bit1.IsChecked = true; else checkBoxAtt1bit1.IsChecked = false;
            if (valueAtt % 32 % 16 % 8 % 4 % 2 / 1 != 0) checkBoxAtt1bit0.IsChecked = true; else checkBoxAtt1bit0.IsChecked = false;
            flagEvent = true;
            GenerationNumericUpDownAtt1_Value();
        }

        private void buttonDownTextBoxAtt_Click(object sender, RoutedEventArgs e)
        {
            flagEvent = false;
            if (valueAtt != 0)
            {
                valueAtt--;
            }
            if (valueAtt / 32 != 0) checkBoxAtt1bit5.IsChecked = true; else checkBoxAtt1bit5.IsChecked = false;
            if (valueAtt % 32 / 16 != 0) checkBoxAtt1bit4.IsChecked = true; else checkBoxAtt1bit4.IsChecked = false;
            if (valueAtt % 32 % 16 / 8 != 0) checkBoxAtt1bit3.IsChecked = true; else checkBoxAtt1bit3.IsChecked = false;
            if (valueAtt % 32 % 16 % 8 / 4 != 0) checkBoxAtt1bit2.IsChecked = true; else checkBoxAtt1bit2.IsChecked = false;
            if (valueAtt % 32 % 16 % 8 % 4 / 2 != 0) checkBoxAtt1bit1.IsChecked = true; else checkBoxAtt1bit1.IsChecked = false;
            if (valueAtt % 32 % 16 % 8 % 4 % 2 / 1 != 0) checkBoxAtt1bit0.IsChecked = true; else checkBoxAtt1bit0.IsChecked = false;
            flagEvent = true;
            GenerationNumericUpDownAtt1_Value();
        }
    }
}
