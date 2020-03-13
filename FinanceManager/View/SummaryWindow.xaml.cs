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
    /// SummaryWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class SummaryWindow : Window
    {
        public SummaryWindow()
        {
            InitializeComponent();
            Binding binding = new Binding("Html");
            binding.Source = SummaryWindowVM.Instance;
            binding.Mode = BindingMode.OneWay;
            textBox.SetBinding(TextBox.TextProperty, binding);
        }
    }
}
