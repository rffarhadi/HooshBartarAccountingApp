using BehComponents;
using HooshBartarAccountingApp.DatabaseModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HooshBartarAccountingApp.DataShow
{
    class Display
    {
        AccountDBEntities db = new AccountDBEntities();//ایجاد یک شی جدید از پایگاه داده
        public DataGridView dgvFromDisplay { get; set; }
        public string[] NameSotunha { get; set; }
        public object DataList { get; set; }

        public DataGridView ShowDgv(DataGridView datagrid, string[] Names,object data)
        {
            try
            {
                dgvFromDisplay = datagrid;
                NameSotunha = Names;
                DataList = data;                
                dgvFromDisplay.DataSource = DataList;
                //شروع تنظیمات گرید
                for (int i = 0; i < NameSotunha.Count(); i++)
                {
                    dgvFromDisplay.Columns[i].HeaderText = NameSotunha.ElementAt(i);
                    dgvFromDisplay.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                }
                dgvFromDisplay.AllowUserToAddRows = false;
                dgvFromDisplay.AllowUserToDeleteRows = false;
                //پایان تنظیمات گرید
            }
            catch (Exception ex)
            {

                MessageBoxFarsi.Show(ex.Message.ToString(), "وجود خطا در نمایش اطلاعات");
            }
            return dgvFromDisplay;
        }
    }
}
