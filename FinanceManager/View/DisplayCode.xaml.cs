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
using System.Windows.Navigation;
using System.Windows.Shapes;
using FinanceManager.ViewModel;

namespace FinanceManager.View
{
    /// <summary>
    /// UserControl1.xaml の相互作用ロジック
    /// </summary>
    public partial class DisplayCode : UserControl
    {
        private string _PrevText;

        public DisplayCode()
        {
            InitializeComponent();
            _PrevText = tb_Code.Text;
        }

        private void Tb_Code_TextChanged(object sender, TextChangedEventArgs e)
        {
            int idx = AddCodeWindowVM.Instance.CodeList.IndexOf(_PrevText);
            if (idx >= 0)
            {
                AddCodeWindowVM.Instance.CodeList[idx] = ((TextBox)sender).Text;
            }
            _PrevText = ((TextBox)sender).Text;
        }
    }
}
