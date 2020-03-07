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
using FinanceManager.ViewModel;

namespace FinanceManager.View
{
    /// <summary>
    /// Window1.xaml の相互作用ロジック
    /// </summary>
    public partial class AddCodeWindow : Window
    {
        public AddCodeWindow()
        {
            InitializeComponent();
            foreach (string codeStr in AddCodeWindowVM.Instance.CodeList)
            {
                DisplayCode displayCode = new DisplayCode();
                displayCode.tb_Code.Text = codeStr;
                wp_DisplayCodeRegion.Children.Add(displayCode);
            }
        }

        private void Bu_Add_Click(object sender, RoutedEventArgs e)
        {
            DisplayCode displayCode = new DisplayCode();
            wp_DisplayCodeRegion.Children.Add(displayCode);
            AddCodeWindowVM.Instance.CodeList.Add(displayCode.tb_Code.Text);
        }

        private void Bu_Delete_Click(object sender, RoutedEventArgs e)
        {
            List<DisplayCode> delList = new List<DisplayCode>();

            foreach (DisplayCode displayCode in wp_DisplayCodeRegion.Children)
            {
                if (displayCode.cb_DelFlag.IsChecked == true)
                {
                    delList.Add(displayCode);
                }
            }
            foreach (DisplayCode del in delList)
            {
                wp_DisplayCodeRegion.Children.Remove(del);
                AddCodeWindowVM.Instance.CodeList.Remove(del.tb_Code.Text);
            }
        }
    }
}
