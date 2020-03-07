using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FinanceManager.Model;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace FinanceManager.ViewModel
{
    class MainWindowVM : INotifyPropertyChanged
    {
        public static MainWindowVM Instance = new MainWindowVM();

        public List<string> AllCodeList
        {
            get
            {
                return Database.Instance.AllCodeList;
            }
            set
            {
                Database.Instance.AllCodeList = value;
                OnPropertyChanged("AllCodeList");
                OnPropertyChanged("AllTypeList");
            }
        }

        public List<string> AllTypeList
        {
            get
            {
                return Database.Instance.AllTypeList;
            }
            set
            {
            }
        }
        private string _Code;
        public string Code
        {
            private get
            {
                return _Code;
            }
            set
            {
                _Code = value;
                FromDate = Database.Instance.GetFirstDay(value);
                ToDate = DateTime.Today;
                OnPropertyChanged("TradingInfos");
            }
        }
        public string Type
        {
            set
            {
                Code = Database.Instance.GetCodeFromCompanyName(value);
            }
        }
        private DateTime _FromDate;
        public DateTime FromDate
        {
            get
            {
                return _FromDate;
            }
            set
            {
                _FromDate = value;
                OnPropertyChanged("FromDate");
                OnPropertyChanged("TradingInfos");
            }
        }
        private DateTime _ToDate;
        public DateTime ToDate
        {
            get
            {
                return _ToDate;
            }
            set
            {
                _ToDate = value;
                OnPropertyChanged("ToDate");
                OnPropertyChanged("TradingInfos");
            }
        }
        public ObservableCollection<TradingInfo> TradingInfos
        {
            get
            {
                return Database.Instance.GetInfoFromCode(Code, FromDate, ToDate);
            }
            set
            {
            }
        }
        public Dictionary<string, string> TransToTitleDict = new Dictionary<string, string>()
        {
            { "DateStr", "日付"},
            { "TotalBuyPrice", "購入金額"},
            { "BuyNum", "購入株数"},
            { "TotalSellPrice", "売却金額"},
            { "SellNum", "売却株数"},
            { "FinalPrice", "最終株価"}
        };
        private string _ProgressStr;
        public string ProgressStr
        {
            get
            {
                return _ProgressStr;
            }
            set
            {
                _ProgressStr = value;
                OnPropertyChanged("ProgressStr");
            }
        }
        public event PropertyChangedEventHandler PropertyChanged = null;

        public MainWindowVM()
        {
            Database.Instance.Load("Database.xml");
            Database.Instance.ProgressChanged += UpdateProgress;
            ProgressStr = "0%";
            if (AllCodeList.Count != 0)
            {
                Code = AllCodeList[0];
                FromDate = Database.Instance.GetFirstDay(Code);
                ToDate = DateTime.Today;
            }
            else
            {
                Code = "";
                FromDate = DateTime.Today;
                ToDate = FromDate;
            }
        }

        public void GetTodayInfo()
        {
            Database.Instance.GetTodayInfo();
            OnPropertyChanged("TradingInfos");
        }

        public void Save()
        {
            Database.Instance.Save("Database.xml");
        }

        protected void OnPropertyChanged(string info)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(info));
        }

        private void UpdateProgress(object sender, EventArgs e)
        {
            ProgressStr = ((ProgressArgs)e).Progress.ToString() + "%";
            OnPropertyChanged("ProgressStr");
        }
    }
}
