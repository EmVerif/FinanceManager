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
using FinanceManager.View;

namespace FinanceManager
{
    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            Binding binding = new Binding("AllCodeList");
            binding.Source = MainWindowVM.Instance;
            binding.Mode = BindingMode.TwoWay;
            combo_Code.SetBinding(ComboBox.ItemsSourceProperty, binding);
            combo_Code.SelectedIndex = 0;

            binding = new Binding("FromDate");
            binding.Source = MainWindowVM.Instance;
            binding.Mode = BindingMode.TwoWay;
            dp_From.SetBinding(DatePicker.SelectedDateProperty, binding);

            binding = new Binding("ToDate");
            binding.Source = MainWindowVM.Instance;
            binding.Mode = BindingMode.TwoWay;
            dp_To.SetBinding(DatePicker.SelectedDateProperty, binding);

            binding = new Binding("TradingInfos");
            binding.Source = MainWindowVM.Instance;
            binding.Mode = BindingMode.TwoWay;
            dataGrid.SetBinding(DataGrid.ItemsSourceProperty, binding);

            binding = new Binding("ProgressStr");
            binding.Source = MainWindowVM.Instance;
            binding.Mode = BindingMode.TwoWay;
            tb_Progress.SetBinding(TextBox.TextProperty, binding);
        }

        private void Combo_Code_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cb_DisplayType.IsChecked == true)
            {
                MainWindowVM.Instance.Type = (string)combo_Code.SelectedItem;
            }
            else
            {
                MainWindowVM.Instance.Code = (string)combo_Code.SelectedItem;
            }
        }

        private void Bu_Save_Click(object sender, RoutedEventArgs e)
        {
            MainWindowVM.Instance.Save();
            MessageBox.Show("OK");
        }

        private void Bu_Get_Click(object sender, RoutedEventArgs e)
        {
            MainWindowVM.Instance.GetTodayInfo();
            MessageBox.Show("OK");
            MainWindowVM.Instance.ProgressStr = "0%";
        }

        private void DataGrid_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            if (e.Column.Header.ToString() == @"DateStr")
            {
                e.Column.IsReadOnly = true;
            }
            e.Column.Header = MainWindowVM.Instance.TransToTitleDict[e.Column.Header.ToString()];
        }

        private void Bu_AddCode_Click(object sender, RoutedEventArgs e)
        {
            AddCodeWindow addCode = new AddCodeWindow();

            addCode.ShowDialog();
            AddCodeWindowVM.Instance.CodeList.Remove("");
            MainWindowVM.Instance.AllCodeList = AddCodeWindowVM.Instance.CodeList.Distinct().ToList();
        }

        private void Cb_DisplayType_Click(object sender, RoutedEventArgs e)
        {
            int idx = combo_Code.SelectedIndex;

            if (cb_DisplayType.IsChecked == true)
            {
                Binding binding = new Binding("AllTypeList");
                binding.Source = MainWindowVM.Instance;
                binding.Mode = BindingMode.TwoWay;
                combo_Code.SetBinding(ComboBox.ItemsSourceProperty, binding);
                tb_DisplayType.Text = "種類：";
            }
            else
            {
                Binding binding = new Binding("AllCodeList");
                binding.Source = MainWindowVM.Instance;
                binding.Mode = BindingMode.TwoWay;
                combo_Code.SetBinding(ComboBox.ItemsSourceProperty, binding);
                tb_DisplayType.Text = "証券コード：";
            }
            combo_Code.SelectedIndex = idx;
        }
    }
}
