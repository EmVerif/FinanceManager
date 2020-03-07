using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FinanceManager.Model;
using System.ComponentModel;

namespace FinanceManager.ViewModel
{
    class AddCodeWindowVM
    {
        public static AddCodeWindowVM Instance = new AddCodeWindowVM();
        public List<string> CodeList { get; set; } = new List<string>();

        public AddCodeWindowVM()
        {
            foreach (string code in Database.Instance.AllCodeList)
            {
                CodeList.Add(code);
            }
        }
    }
}
