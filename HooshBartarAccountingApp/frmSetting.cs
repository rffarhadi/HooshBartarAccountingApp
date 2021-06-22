using BehComponents;
using HooshBartarAccountingApp.DatabaseModel;
using HooshBartarAccountingApp.DataShow;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HooshBartarAccountingApp
{
    public partial class frmSetting : Form
    {
        public frmSetting()
        {
            InitializeComponent();
        }
        AccountDBEntities db = new AccountDBEntities();
        private void frmSetting_Load(object sender, EventArgs e)
        {
            try
            {
                var settingList = db.tblSettings.Select(a=>new { a.Id,a.BuyKarmozdRate,a.SellKarmozdRate,a.ArzeshAfzoodehTaxRate,a.NaghloEnteghalTaxRate}).ToList();
                //برای نامگذاری سرستون‌ها در کلاس مربوطه
                string[] NameSotunha = { "کد", "نرخ کارمزد خرید", "نرخ کارمزد فروش", "نرخ مالیات ارزش افزوده", "نرخ مالیات نقل و انتقال" };
                Display calledDgv = new Display();
                calledDgv.ShowDgv(dgvSetting, NameSotunha, settingList);
                dgvSetting = calledDgv.dgvFromDisplay;
                dgvSetting.Columns[0].DefaultCellStyle.Format = "N0";
                dgvSetting.Columns[1].DefaultCellStyle.Format = "N5";
                dgvSetting.Columns[2].DefaultCellStyle.Format = "N5";
                dgvSetting.Columns[3].DefaultCellStyle.Format = "N5";
                dgvSetting.Columns[4].DefaultCellStyle.Format = "N5";
            }
            catch (Exception ex)
            {
                MessageBoxFarsi.Show(ex.Message.ToString(), "خطاء در بارگزاری اطلاعات");
            }
        }

        private void btnSabt_Click(object sender, EventArgs e)
        {
            try
            {
                var settingList = db.tblSettings.ToList();
                if (settingList.Count >= 1)
                {
                    MessageBoxFarsi.Show("امکان اضافه کردن بیش از یک سابقه وجود ندارد", "عدم امکان ثبت تنظیمات جدید");
                    return;
                }
                tblSetting tbl = new tblSetting();
                tbl.BuyKarmozdRate = numBuyKarmozd.Value;
                tbl.SellKarmozdRate = numSellKarmozd.Value;
                tbl.ArzeshAfzoodehTaxRate = numArzeshAfzudehTax.Value;
                tbl.NaghloEnteghalTaxRate = namEnteghaLTax.Value;
                db.tblSettings.Add(tbl);
                db.SaveChanges();
                frmSetting_Load(sender, e);
                MessageBoxFarsi.Show("اطلاعات با موفقیت در پایگاه داده ثبت شد", "ثبت موفق");
            }
            catch (Exception ex)
            {
                MessageBoxFarsi.Show(ex.Message.ToString(), "خطاء در ثبت");
            }
        }

        private void dgvSetting_MouseUp(object sender, MouseEventArgs e)
        {
            try
            {
                numBuyKarmozd.Value =(decimal)dgvSetting.CurrentRow.Cells[1].Value;
                numSellKarmozd.Value = (decimal)dgvSetting.CurrentRow.Cells[2].Value;
                numArzeshAfzudehTax.Value = (decimal)dgvSetting.CurrentRow.Cells[3].Value;
                namEnteghaLTax.Value = (decimal)dgvSetting.CurrentRow.Cells[4].Value;
            }
            catch (Exception ex)
            {
                MessageBoxFarsi.Show(ex.Message.ToString(), "خطاء در ویرایش");
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                int IdHazfi = Convert.ToInt32(dgvSetting.CurrentRow.Cells[0].Value);
                int id = Convert.ToInt32(db.tblSettings.Where(a => a.Id == IdHazfi).Select(a => a.Id).FirstOrDefault());
                var del = db.tblSettings.Where(a => a.Id == id);
                db.tblSettings.RemoveRange(del);
                db.SaveChanges();
                frmSetting_Load(sender, e);
                MessageBoxFarsi.Show("حذف سابقه با موفقیت انجام شد", "حذف موفقیت‌آمیز");
            }
            catch (Exception ex)
            {
                MessageBoxFarsi.Show(ex.Message.ToString(), "خطا در حذف");
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            try
            {
                var settingList = db.tblSettings.ToList();
                if (settingList.Count < 1)
                {
                    MessageBoxFarsi.Show("هیچ سابقه‌ای برای ویرایش وجود ندارد", "عدم امکان ویراایش تنظیمات");
                    return;
                }

                settingList.ForEach(a => { a.BuyKarmozdRate = numBuyKarmozd.Value; a.SellKarmozdRate = numSellKarmozd.Value; a.ArzeshAfzoodehTaxRate = numArzeshAfzudehTax.Value; a.NaghloEnteghalTaxRate = namEnteghaLTax.Value; });
                db.SaveChanges();
                frmSetting_Load(sender, e);
                MessageBoxFarsi.Show("اطلاعات با موفقیت ویرایش شد", "ویرایش موفق");
            }
            catch (Exception ex)
            {
                MessageBoxFarsi.Show(ex.Message.ToString(), "خطاء در ویرایش");
            }
        }
    }

}

