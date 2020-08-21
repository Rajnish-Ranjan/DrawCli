using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace DrawCli
{
    /// <summary>
    /// Interaction logic for setProperties.xaml
    /// </summary>
    public partial class setProperties : Window
    {
        #region private Members

        private Color _color;
        private int _thickness;
        /// <summary>
        /// The MainWindow from where the control has been transferred 
        /// That window is going to recieve the properties set over here
        /// </summary>
        private MainWindow Selectfrom_Window;
        private List<int> _thicknessRange = new List<int>();


        #endregion

        /// <summary>
        /// Constructor for the new window
        /// </summary>
        /// <param name="_main"></param>
        public setProperties(MainWindow _main)
        {
            Selectfrom_Window = _main;
            _thicknessRange.AddRange(Enumerable.Range(0, 100).ToArray());
            InitializeComponent();

        }

        #region Trigger Methods

        private void btn_SaveChange_Click(object sender, RoutedEventArgs e)
        {
            //update thickness and color
            Selectfrom_Window.changeProperties(_color, _thickness);
            this.Close();
        }

        private void clr_pick_ColorChange(object sender, RoutedPropertyChangedEventArgs<Color> e)
        {
            _color = ColorPicker1.SelectedColor;
        }

        private void Combo_thickness_SelChanged(object sender, SelectionChangedEventArgs e)
        {
            string str = Convert.ToString(combo_Thick.SelectedItem);
            string str_num = "";
            for(int i = str.Length - 1; i >= 0; i--)
            {
                if (str[i] <= '9' && str[i] >= '0')
                {
                    str_num = str[i] + str_num;
                }
                else
                {
                    break;
                }
            }
            try
            {
                _thickness = Convert.ToInt32(str_num);
            }
            catch(Exception)
            {
                _thickness = -1;
            }
        }

        #endregion

        /// <summary>
        /// Setting Combobox Items' values on loading
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void getThicknessRange(object sender, RoutedEventArgs e)
        {
            var combo = sender as ComboBox;
            combo.ItemsSource = _thicknessRange;
            combo.SelectedIndex = 1;
        }
    }
}