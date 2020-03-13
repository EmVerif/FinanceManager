using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FinanceManager.Model;

namespace FinanceManager.ViewModel
{
    class SummaryWindowVM : INotifyPropertyChanged
    {
        public static SummaryWindowVM Instance = new SummaryWindowVM();
        public event PropertyChangedEventHandler PropertyChanged;

        public string Html
        {
            get
            {
                AssetsInfo tmp = Database.Instance.AssetsInfo;
                string ret = "";

                ret += "総資産：" + tmp.TotalAssets + "円\n";
                ret += "総損益：" + tmp.TotalProfit + "円\n";
                foreach (var companyName in tmp.AssetsInfoEachCodeDic.Keys)
                {
                    ret += "\t会社名：" + companyName + "\n";
                    ret += "\t\t資産：" + tmp.AssetsInfoEachCodeDic[companyName].TotalAssets + "円\n";
                    ret += "\t\t損益：" + tmp.AssetsInfoEachCodeDic[companyName].TotalProfit + "円\n";
                }

                return ret;
            }
        }

        protected void OnPropertyChanged(string info)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(info));
        }
    }
}
