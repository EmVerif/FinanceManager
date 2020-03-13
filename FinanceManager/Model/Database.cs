using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using System.IO;
using System.Collections.ObjectModel;
using FinanceManager.Common;

namespace FinanceManager.Model
{
    class Database
    {
        public static Database Instance = new Database();
        public List<string> AllCodeList
        {
            get
            {
                string[] codeSortArray = _CodeInfoDict.Keys.ToArray();

                Array.Sort(codeSortArray);

                return codeSortArray.ToList();
            }
            set
            {
                List<string> delCodeList = _CodeInfoDict.Keys.ToList();
                foreach (var code in value)
                {
                    if (!_CodeInfoDict.Keys.Contains(code))
                    {
                        _CodeInfoDict.Add(code, new CodeInfo());
                    }
                    else
                    {
                        delCodeList.Remove(code);
                    }
                }
                foreach (var delCode in delCodeList)
                {
                    _CodeInfoDict.Remove(delCode);
                }
            }
        }
        public List<string> AllTypeList
        {
            get
            {
                List<string> ret = new List<string>();
                foreach (var code in AllCodeList)
                {
                    ret.Add(_CodeInfoDict[code].CompanyName);
                }

                return ret;
            }
        }
        public AssetsInfo AssetsInfo
        {
            get
            {
                AssetsInfo ret = new AssetsInfo();

                foreach (var code in AllCodeList)
                {
                    Decimal totalNum = 0;
                    Decimal totalDeal = 0;
                    AssetsInfoEachCode assetsInfoEachCode = new AssetsInfoEachCode();

                    foreach (var date in _CodeInfoDict[code].DayTradingInfosDict.Keys)
                    {
                        totalNum += _CodeInfoDict[code].DayTradingInfosDict[date].BuyNum;
                        totalNum -= _CodeInfoDict[code].DayTradingInfosDict[date].SellNum;
                        totalDeal -= _CodeInfoDict[code].DayTradingInfosDict[date].TotalBuyPrice;
                        totalDeal += _CodeInfoDict[code].DayTradingInfosDict[date].TotalSellPrice;
                    }
                    assetsInfoEachCode.TotalAssets = totalNum * _CodeInfoDict[code].DayTradingInfosDict[GetLastDay(code)].FinalPrice;
                    assetsInfoEachCode.TotalProfit = totalDeal + assetsInfoEachCode.TotalAssets;
                    if ((assetsInfoEachCode.TotalAssets != 0) || (assetsInfoEachCode.TotalProfit != 0))
                    {
                        ret.AssetsInfoEachCodeDic.Add(_CodeInfoDict[code].CompanyName + @"(" + code + @")", assetsInfoEachCode);
                        ret.TotalAssets += assetsInfoEachCode.TotalAssets;
                        ret.TotalProfit += assetsInfoEachCode.TotalProfit;
                    }
                }

                return ret;
            }
        }
        public event EventHandler ProgressChanged;

        private SerializableDictionary<string, CodeInfo> _CodeInfoDict = new SerializableDictionary<string, CodeInfo>();

        public Database()
        {
        }

        public void AddCode(string inCode)
        {
            if (!_CodeInfoDict.ContainsKey(inCode))
            {
                _CodeInfoDict.Add(inCode, new CodeInfo());
            }
        }

        public ObservableCollection<TradingInfo> GetInfoFromCode(string inCode, string inFromDateStr = "", string inToDateStr = "")
        {
            DateTime fromDate;
            DateTime toDate;

            try
            {
                fromDate = DateTime.Parse(inFromDateStr);
            }
            catch
            {
                fromDate = DateTime.Parse("1900/1/1");
            }
            try
            {
                toDate = DateTime.Parse(inToDateStr);
            }
            catch
            {
                toDate = DateTime.Parse("9999/12/31");
            }
            return GetInfoFromCode(inCode, fromDate, toDate);
        }

        public ObservableCollection<TradingInfo> GetInfoFromCode(string inCode, DateTime inFromDate, DateTime inToDate)
        {
            ObservableCollection<TradingInfo> tradingInfosList = new ObservableCollection<TradingInfo>();

            try
            {
                DateTime[] daysSortArray = _CodeInfoDict[inCode].DayTradingInfosDict.Keys.ToArray();

                Array.Sort(daysSortArray);
                foreach (var day in daysSortArray.ToList())
                {
                    if ((inFromDate <= day) && (day <= inToDate))
                    {
                        tradingInfosList.Add(_CodeInfoDict[inCode].DayTradingInfosDict[day]);
                    }
                }
            }
            catch
            {
            }

            return tradingInfosList;
        }

        public DateTime GetLastDay(string inCode)
        {
            DateTime dateTime;

            try
            {
                DateTime[] daysSortArray = _CodeInfoDict[inCode].DayTradingInfosDict.Keys.ToArray();
                Array.Sort(daysSortArray);
                dateTime = daysSortArray.Last();
            }
            catch
            {
                dateTime = DateTime.Today;
            }

            return dateTime;
        }

        public DateTime GetFirstDay(string inCode)
        {
            DateTime dateTime;

            try
            {
                DateTime[] daysSortArray = _CodeInfoDict[inCode].DayTradingInfosDict.Keys.ToArray();
                Array.Sort(daysSortArray);
                dateTime = daysSortArray[0];
            }
            catch
            {
                dateTime = DateTime.Today;
            }

            return dateTime;
        }

        public string GetCompanyNameFromCode(string inCode)
        {
            return _CodeInfoDict[inCode].CompanyName;
        }

        public string GetCodeFromCompanyName(string inCompanyName)
        {
            string ret = "";

            foreach (var code in _CodeInfoDict.Keys)
            {
                if (_CodeInfoDict[code].CompanyName == inCompanyName)
                {
                    ret = code;
                }
            }

            return ret;
        }

        public void SetInfo(string inCode, List<TradingInfo> inTradingInfosList)
        {
            AddCode(inCode);
            foreach (var tradingInfo in inTradingInfosList)
            {
                DateTime dateTime = DateTime.Parse(tradingInfo.DateStr);

                if (!_CodeInfoDict[inCode].DayTradingInfosDict.ContainsKey(dateTime))
                {
                    _CodeInfoDict[inCode].DayTradingInfosDict.Add(dateTime, tradingInfo);
                }
                else
                {
                    _CodeInfoDict[inCode].DayTradingInfosDict[dateTime] = tradingInfo;
                }
            }
        }

        public void Save(string inFileName)
        {
            var xmlSerializer = new XmlSerializer(typeof(SerializableDictionary<string, CodeInfo>));
            using (var sw = new StreamWriter(inFileName, false, Encoding.UTF8))
            {
                xmlSerializer.Serialize(sw, _CodeInfoDict);
                sw.Flush();
            }
        }

        public void Load(string inFileName)
        {
            try
            {
                var xmlSerializer = new XmlSerializer(typeof(SerializableDictionary<string, CodeInfo>));
                using (FileStream fs = new FileStream(inFileName, FileMode.Open))
                {
                    _CodeInfoDict = xmlSerializer.Deserialize(fs) as SerializableDictionary<string, CodeInfo>;
                }
            }
            catch
            {
                _CodeInfoDict = new SerializableDictionary<string, CodeInfo>();
            }
        }

        // 取引終了～取引開始までの間で実行されること前提のプログラム
        public async Task<List<string>> GetTodayInfo()
        {
            List<string> errMsgsList = new List<string>();
            double percentPerGet = 100.0 / _CodeInfoDict.Keys.Count;
            double percent = 0.0;

            foreach (string code in _CodeInfoDict.Keys)
            {
                try
                {
                    var financeInfo = await InfoGetter.Instance.Get(code);
                    var date = DateTime.Parse(financeInfo.Date);

                    if (!_CodeInfoDict[code].DayTradingInfosDict.ContainsKey(date))
                    {
                        var tradingInfo = new TradingInfo();

                        tradingInfo.DateStr = date.ToString("yyyy/MM/dd"); ;
                        tradingInfo.FinalPrice = financeInfo.Price;
                        _CodeInfoDict[code].DayTradingInfosDict.Add(date, tradingInfo);
                    }
                    else
                    {
                        _CodeInfoDict[code].DayTradingInfosDict[date].FinalPrice = financeInfo.Price;
                    }
                    _CodeInfoDict[code].CompanyName = financeInfo.CompanyName;
                }
                catch
                {
                    errMsgsList.Add(code + @": データ取得失敗");
                }
                percent = percent + percentPerGet;
                ProgressChanged?.Invoke(this, new ProgressArgs(Math.Min((int)percent, 100)));
            }

            return errMsgsList;
        }
    }
    
    class ProgressArgs : EventArgs
    {
        public int Progress;

        public ProgressArgs(int inProgress)
        {
            Progress = inProgress;
        }
    }
}
