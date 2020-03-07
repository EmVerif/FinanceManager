using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Text.RegularExpressions;

namespace FinanceManager.Model
{
    class InfoGetter
    {
        public static InfoGetter Instance = new InfoGetter();

        private string _BaseAddr = @"https://www.nikkei.com/nkd/company/?scode=";
        private string _FinanceInfoRegex = @"<dd [^>]+>(?<price>[\d,\.]+)<";
        private string _CompanyNameRegex = @"<h1[^>]+>(?<name>[^<]+)</h1>";
        private string _DateRegex = @"<div[^>]+>(?<date>\d+\/\d+\/\d+)</div>";

        public async Task<FinanceInfoToday> Get(string inCode)
        {
            string hpContent;

            using (var client = new HttpClient())
            {
                HttpResponseMessage result = await client.GetAsync(_BaseAddr + inCode).ConfigureAwait(false);
                hpContent = await result.Content.ReadAsStringAsync().ConfigureAwait(false);
            }
            return Analyze(hpContent, inCode);
        }

        private FinanceInfoToday Analyze(string inHpContent, string inCode)
        {
            Match matchFinanceInfo = Regex.Match(inHpContent, _FinanceInfoRegex);
            Match matchCompanyName = Regex.Match(inHpContent, _CompanyNameRegex);
            Match matchDate = Regex.Match(inHpContent, _DateRegex);
            FinanceInfoToday ret = null;

            if (matchCompanyName.Success && matchFinanceInfo.Success && matchDate.Success)
            {
                ret = new FinanceInfoToday(
                    inDate: matchDate.Groups["date"].Value,
                    inCode: inCode,
                    inCompanyName: matchCompanyName.Groups["name"].Value,
                    inPrice: Convert.ToDecimal(matchFinanceInfo.Groups["price"].Value.Replace(",", ""))
                );
            }

            return ret;
        }
    }

    class FinanceInfoToday
    {
        public string Date;
        public string Code;
        public string CompanyName;
        public Decimal Price;

        public FinanceInfoToday(string inDate, string inCode, string inCompanyName, Decimal inPrice)
        {
            Date = inDate;
            Code = inCode;
            CompanyName = inCompanyName;
            Price = inPrice;
        }
    }
}
