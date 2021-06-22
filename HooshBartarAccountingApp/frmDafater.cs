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
    public partial class frmDafater : Form
    {
        public frmDafater()
        {
            InitializeComponent();
        }
        AccountDBEntities db = new AccountDBEntities();

        private void frmDafater_Load(object sender, EventArgs e)
        {

            try
            {
                /////////////////////////////
                System.Globalization.PersianCalendar pc = new System.Globalization.PersianCalendar();
                DateTime yesterday = DateTime.Now.AddDays(-2);
                mskAzTarikh.Text = pc.GetYear(yesterday).ToString() + pc.GetMonth(yesterday).ToString("0#") + pc.GetDayOfMonth(yesterday).ToString("0#") + pc.GetHour(yesterday).ToString("0#") + pc.GetMinute(yesterday).ToString("0#") + pc.GetSecond(yesterday).ToString("0#");
                DateTime tomorrow = DateTime.Now.AddDays(2);
                mskTaTarikh.Text = pc.GetYear(tomorrow).ToString() + pc.GetMonth(tomorrow).ToString("0#") + pc.GetDayOfMonth(tomorrow).ToString("0#") + pc.GetHour(tomorrow).ToString("0#") + pc.GetMinute(tomorrow).ToString("0#") + pc.GetSecond(tomorrow).ToString("0#");
                ////////////////////////////
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void btnDisplay_Click(object sender, EventArgs e)
        {
            try
            {
                string tarikhShamsiAz = mskAzTarikh.Text;
                DateConvert miladiAz = new DateConvert();
                miladiAz.ConvertToMiladi(tarikhShamsiAz);
                DateTime tarikhMiladiAz = miladiAz.TarikhMiladi;

                string tarikhShamsiTa = mskTaTarikh.Text;
                DateConvert miladiTa = new DateConvert();
                miladiTa.ConvertToMiladi(tarikhShamsiTa);
                DateTime tarikhMiladiTa = miladiTa.TarikhMiladi;

                var hesabhaInfoList = db.tblRooznamehs.OrderBy(a => a.SanadId).Where(a => a.SanadTarikhMiladi >= tarikhMiladiAz && a.SanadTarikhMiladi <= tarikhMiladiTa && a.SanadId > 0).Select(a => new { a.SanadId, a.SanadTarikhMiladi, a.SanadTarikhShamsi, a.IdHesab, a.NameHesab, a.NameGroupHesab, a.NameSumGroupHesab, a.SanadTozih, a.BuySellVol, a.ArzeshAfzudehTax, a.EnteghaTax, a.KarmozdMoameleh, a.SanadBedehkar, a.SabadBestankar }).Distinct().ToList();
                List<ReportOfDafater> repList = new List<ReportOfDafater>();
                foreach (var item in hesabhaInfoList)
                {
                    ReportOfDafater tbl = new ReportOfDafater();
                    tbl.IdSanad = item.SanadId;
                    tbl.Tarikh = item.SanadTarikhShamsi;
                    tbl.IdHesab = item.IdHesab;
                    tbl.NameHesab = item.NameHesab;
                    tbl.GroupHesab = item.NameGroupHesab;
                    tbl.SubGroupHesab = item.NameSumGroupHesab;
                    tbl.SanadTozih = item.SanadTozih;
                    tbl.BuySellVol = item.BuySellVol;
                    tbl.ArzeshAfzudehTax = item.ArzeshAfzudehTax;
                    tbl.EnteghaTax = item.EnteghaTax;
                    tbl.KarmozdMoameleh = item.KarmozdMoameleh;
                    tbl.BedehkarTurnOver = item.SanadBedehkar;
                    tbl.BestankarTurnOver = item.SabadBestankar;
                    tbl.Balance = Convert.ToInt64(Math.Abs(Convert.ToDecimal((hesabhaInfoList.Where(a => a.SanadId <= item.SanadId).Select(a => a.SanadBedehkar - a.SabadBestankar).Sum()))));
                    if (hesabhaInfoList.Where(a => a.SanadId <= item.SanadId).Select(a => a.SanadBedehkar).Sum() >= hesabhaInfoList.Where(a => a.SanadId <= item.SanadId).Select(a => a.SabadBestankar).Sum())
                    {
                        tbl.Mahiyat = "بدهکار";
                    }
                    else if (hesabhaInfoList.Where(a => a.SanadId <= item.SanadId).Select(a => a.SanadBedehkar).Sum() < hesabhaInfoList.Where(a => a.SanadId <= item.SanadId).Select(a => a.SabadBestankar).Sum())
                    {
                        tbl.Mahiyat = "بستانکار";
                    }
                    else if (hesabhaInfoList.Where(a => a.SanadId <= item.SanadId).Select(a => a.SanadBedehkar).Sum() == hesabhaInfoList.Where(a => a.SanadId <= item.SanadId).Select(a => a.SabadBestankar).Sum())
                    {
                        tbl.Mahiyat = "-";
                    }
                    repList.Add(tbl);
                }

                //برای نامگذاری سرستون‌ها در کلاس مربوطه
                string[] NameSotunha = { "شماره سند", "تاریخ", "کد حساب", "نام حساب", "طبقه حساب", "زیرطبقه حساب", "شرح سند", "حجم معاملات", "مالیات ارزش افزوده", "مالیات نقل و انتقال", "کارمزد معامله", "بدهکار", "بستانکار","مانده","ماهیت" };
                Display calledDgv = new Display();
                calledDgv.ShowDgv(dgvDisplay, NameSotunha, repList);
                dgvDisplay = calledDgv.dgvFromDisplay;
                dgvDisplay.Columns[0].DefaultCellStyle.Format = "N4";
                dgvDisplay.Columns[7].DefaultCellStyle.Format = "N0";
                dgvDisplay.Columns[8].DefaultCellStyle.Format = "N0";
                dgvDisplay.Columns[9].DefaultCellStyle.Format = "N0";
                dgvDisplay.Columns[10].DefaultCellStyle.Format = "N0";
                dgvDisplay.Columns[11].DefaultCellStyle.Format = "N0";
                dgvDisplay.Columns[12].DefaultCellStyle.Format = "N0";
            }
            catch (Exception ex)
            {
                MessageBoxFarsi.Show(ex.Message.ToString(), "خطا در نمایش");
            }
        }

        private void txtSearchIdSanad_TextChanged(object sender, EventArgs e)
        {
           
        }

        private void txtSearchDaftarKol_TextChanged(object sender, EventArgs e)
        {
            try
            {
                string daftarkolName = txtSearchDaftarKol.Text;

                var hesabhaInfoList = db.tblRooznamehs.OrderBy(a => a.SanadId).Where(a => a.NameSumGroupHesab.ToString().Contains(daftarkolName) && a.SanadId > 0).Select(a => new { a.SanadId, a.SanadTarikhMiladi, a.SanadTarikhShamsi, a.IdHesab, a.NameHesab, a.NameGroupHesab, a.NameSumGroupHesab, a.SanadTozih, a.BuySellVol, a.ArzeshAfzudehTax, a.EnteghaTax, a.KarmozdMoameleh, a.SanadBedehkar, a.SabadBestankar }).Distinct().ToList();
                List<ReportOfDafater> repList = new List<ReportOfDafater>();
                foreach (var item in hesabhaInfoList)
                {
                    ReportOfDafater tbl = new ReportOfDafater();
                    tbl.IdSanad = item.SanadId;
                    tbl.Tarikh = item.SanadTarikhShamsi;
                    tbl.IdHesab = item.IdHesab;
                    tbl.NameHesab = item.NameHesab;
                    tbl.GroupHesab = item.NameGroupHesab;
                    tbl.SubGroupHesab = item.NameSumGroupHesab;
                    tbl.SanadTozih = item.SanadTozih;
                    tbl.BuySellVol = item.BuySellVol;
                    tbl.ArzeshAfzudehTax = item.ArzeshAfzudehTax;
                    tbl.EnteghaTax = item.EnteghaTax;
                    tbl.KarmozdMoameleh = item.KarmozdMoameleh;
                    tbl.BedehkarTurnOver = item.SanadBedehkar;
                    tbl.BestankarTurnOver = item.SabadBestankar;
                    tbl.Balance = Convert.ToInt64(Math.Abs(Convert.ToDecimal((hesabhaInfoList.Where(a => a.SanadId <= item.SanadId).Select(a => a.SanadBedehkar - a.SabadBestankar).Sum()))));
                    if (item.SanadBedehkar > item.SabadBestankar)
                    {
                        tbl.Mahiyat = "بدهکار";
                    }
                    else if (item.SanadBedehkar < item.SabadBestankar)
                    {
                        tbl.Mahiyat = "بستانکار";
                    }
                    else
                    {
                        tbl.Mahiyat = "-";
                    }
                    repList.Add(tbl);
                }

                string[] NameSotunha = { "شماره سند", "تاریخ", "کد حساب", "نام حساب", "طبقه حساب", "زیرطبقه حساب", "شرح سند", "حجم معاملات", "مالیات ارزش افزوده", "مالیات نقل و انتقال", "کارمزد معامله", "بدهکار", "بستانکار", "مانده", "ماهیت" };
                Display calledDgv = new Display();
                calledDgv.ShowDgv(dgvDisplay, NameSotunha, repList);
                dgvDisplay = calledDgv.dgvFromDisplay;
                dgvDisplay.Columns[0].DefaultCellStyle.Format = "N4";
                dgvDisplay.Columns[7].DefaultCellStyle.Format = "N0";
                dgvDisplay.Columns[8].DefaultCellStyle.Format = "N0";
                dgvDisplay.Columns[9].DefaultCellStyle.Format = "N0";
                dgvDisplay.Columns[10].DefaultCellStyle.Format = "N0";
                dgvDisplay.Columns[11].DefaultCellStyle.Format = "N0";
                dgvDisplay.Columns[12].DefaultCellStyle.Format = "N0";
                dgvDisplay.Columns[13].DefaultCellStyle.Format = "N0";
            }
            catch (Exception ex)
            {
                MessageBoxFarsi.Show(ex.Message.ToString(), "خطا در جستجو");
            }
        }

        private void txtSearchNam_TextChanged(object sender, EventArgs e)
        {
            try
            {
                string tarikhShamsiAz = mskAzTarikh.Text;
                DateConvert miladiAz = new DateConvert();
                miladiAz.ConvertToMiladi(tarikhShamsiAz);
                DateTime tarikhMiladiAz = miladiAz.TarikhMiladi;

                string tarikhShamsiTa = mskTaTarikh.Text;
                DateConvert miladiTa = new DateConvert();
                miladiTa.ConvertToMiladi(tarikhShamsiTa);
                DateTime tarikhMiladiTa = miladiTa.TarikhMiladi;

                string myHesab = txtSearchNam.Text;
                var hesabhaInfoList = db.tblRooznamehs.OrderBy(a => a.SanadId).Where(a => a.NameHesab.Contains(myHesab) && a.SanadId > 0 && a.SanadTarikhMiladi>= tarikhMiladiAz && a.SanadTarikhMiladi<= tarikhMiladiTa).Select(a => new { a.SanadId, a.SanadTarikhMiladi, a.SanadTarikhShamsi, a.IdHesab, a.NameHesab, a.NameGroupHesab, a.NameSumGroupHesab, a.SanadTozih, a.BuySellVol, a.ArzeshAfzudehTax, a.EnteghaTax, a.KarmozdMoameleh, a.SanadBedehkar, a.SabadBestankar }).Distinct().ToList();
                List<ReportOfDafater> repList = new List<ReportOfDafater>();
                foreach (var item in hesabhaInfoList)
                {
                    ReportOfDafater tbl = new ReportOfDafater();
                    tbl.IdSanad = item.SanadId;
                    tbl.Tarikh = item.SanadTarikhShamsi;
                    tbl.IdHesab = item.IdHesab;
                    tbl.NameHesab = item.NameHesab;
                    tbl.GroupHesab = item.NameGroupHesab;
                    tbl.SubGroupHesab = item.NameSumGroupHesab;
                    tbl.SanadTozih = item.SanadTozih;
                    tbl.BuySellVol = item.BuySellVol;
                    tbl.ArzeshAfzudehTax = item.ArzeshAfzudehTax;
                    tbl.EnteghaTax = item.EnteghaTax;
                    tbl.KarmozdMoameleh = item.KarmozdMoameleh;
                    tbl.BedehkarTurnOver = item.SanadBedehkar;
                    tbl.BestankarTurnOver = item.SabadBestankar;
                    tbl.Balance = Convert.ToInt64(Math.Abs(Convert.ToDecimal((hesabhaInfoList.Where(a => a.SanadId <= item.SanadId).Select(a => a.SanadBedehkar - a.SabadBestankar).Sum()))));
                    if (item.SanadBedehkar > item.SabadBestankar)
                    {
                        tbl.Mahiyat = "بدهکار";
                    }
                    else if (item.SanadBedehkar < item.SabadBestankar)
                    {
                        tbl.Mahiyat = "بستانکار";
                    }
                    else
                    {
                        tbl.Mahiyat = "-";
                    }
                    repList.Add(tbl);
                }
                //برای نامگذاری سرستون‌ها در کلاس مربوطه
                string[] NameSotunha = { "شماره سند", "تاریخ", "کد حساب", "نام حساب", "طبقه حساب", "زیرطبقه حساب", "شرح سند", "حجم معاملات", "مالیات ارزش افزوده", "مالیات نقل و انتقال", "کارمزد معامله", "بدهکار", "بستانکار", "مانده", "ماهیت" };
                Display calledDgv = new Display();
                calledDgv.ShowDgv(dgvDisplay, NameSotunha, repList);
                dgvDisplay = calledDgv.dgvFromDisplay;
                dgvDisplay.Columns[0].DefaultCellStyle.Format = "N4";
                dgvDisplay.Columns[7].DefaultCellStyle.Format = "N0";
                dgvDisplay.Columns[8].DefaultCellStyle.Format = "N0";
                dgvDisplay.Columns[9].DefaultCellStyle.Format = "N0";
                dgvDisplay.Columns[10].DefaultCellStyle.Format = "N0";
                dgvDisplay.Columns[11].DefaultCellStyle.Format = "N0";
                dgvDisplay.Columns[12].DefaultCellStyle.Format = "N0";
                dgvDisplay.Columns[13].DefaultCellStyle.Format = "N0";
            }
            catch (Exception ex)
            {
                MessageBoxFarsi.Show(ex.Message.ToString(), "خطا در جستجو");
            }
        }

        private void btnExcell_Click(object sender, EventArgs e)
        {
            try
            {
                string tarikhShamsiAz = mskAzTarikh.Text;
                DateConvert miladiAz = new DateConvert();
                miladiAz.ConvertToMiladi(tarikhShamsiAz);
                DateTime tarikhMiladiAz = miladiAz.TarikhMiladi;

                string tarikhShamsiTa = mskTaTarikh.Text;
                DateConvert miladiTa = new DateConvert();
                miladiTa.ConvertToMiladi(tarikhShamsiTa);
                DateTime tarikhMiladiTa = miladiTa.TarikhMiladi;

                if (cmbReport.SelectedIndex == 0)
                {
                    if (dgvDisplay.DataSource == null)
                    {
                        MessageBoxFarsi.Show("یک حساب را از جدول نمایش انتخاب کنید", "عدم انتخاب حساب");
                        return;
                    }
                    decimal idSanad = Convert.ToDecimal(dgvDisplay.CurrentRow.Cells[0].Value);
                    string NameHesab = dgvDisplay.CurrentRow.Cells[3].Value.ToString();
                    var hesabhaInfoList = db.tblRooznamehs.OrderBy(a => a.SanadId).Where(a => a.NameHesab == NameHesab && a.SanadId == idSanad).Where(a => a.IdHesab >= 1002001 && a.IdHesab <= 1002999 || a.IdHesab >= 1005001 && a.IdHesab <= 1005999 || a.IdHesab >= 10010001 && a.IdHesab <= 10019999).ToList();
                    string TarikhFactor = dgvDisplay.CurrentRow.Cells[1].Value.ToString();
                    if (hesabhaInfoList.Count == 0 || hesabhaInfoList == null)
                    {
                        MessageBoxFarsi.Show("هیچ مورد معامله‌ای انتخاب نشده است، از جدول نمایش یک مورد را انتخاب کنید", "نبود سابقه اطلاعاتی در گزارش گیری");
                        return;
                    }
                    //برای نامگذاری سرستون‌ها در کلاس مربوطه
                    string[] NameSotunha = { "شماره سند", "تاریخ", "نام حساب", "شرح سند", "حجم معامله", "مالیات نقل و انتقال", "مالیات ارزش افزوه", "کارمزد معامله", "مبلغ فاکتور بدون مالیات ارزش افزوده", "جمع مبلغ فاکتور" };
                    ExcelReports factor = new ExcelReports();
                    factor.FactorReport(hesabhaInfoList, NameSotunha, TarikhFactor, NameHesab);
                }
                if (cmbReport.SelectedIndex == 1)
                {
                    if (dgvDisplay.DataSource == null)
                    {
                        MessageBoxFarsi.Show("یک حساب را از جدول نمایش انتخاب کنید", "عدم انتخاب حساب");
                        return;
                    }
                    string NameHesab = dgvDisplay.CurrentRow.Cells[3].Value.ToString();
                    var hesabhaInfoList = db.tblRooznamehs.OrderBy(a => a.SanadId).Where(a => a.NameHesab == NameHesab).Where(a => a.IdHesab >= 1002001 && a.IdHesab <= 1002999 || a.IdHesab >= 1005001 && a.IdHesab <= 1005999 || a.IdHesab >= 10010001 && a.IdHesab <= 10019999).Where(a => a.SanadId > 0).ToList();
                    string TarikhFactor = dgvDisplay.CurrentRow.Cells[1].Value.ToString();
                    if (hesabhaInfoList.Count == 0 || hesabhaInfoList == null)
                    {
                        MessageBoxFarsi.Show("هیچ مورد معامله‌ای انتخاب نشده است، از جدول نمایش یک مورد را انتخاب کنید", "نبود سابقه اطلاعاتی در گزارش گیری");
                        return;
                    }
                    //برای نامگذاری سرستون‌ها در کلاس مربوطه
                    string[] NameSotunha = { "شماره سند", "تاریخ", "نام حساب", "شرح سند", "حجم معامله", "مالیات نقل و انتقال", "مالیات ارزش افزوه", "کارمزد معامله", "مبلغ فاکتور بدون مالیات ارزش افزوده", "جمع مبلغ فاکتور" };
                    ExcelReports factor = new ExcelReports();
                    factor.FactorReport(hesabhaInfoList, NameSotunha, TarikhFactor, NameHesab);
                }
                if (cmbReport.SelectedIndex == 2)
                {
                    var hesabhaInfoList = db.tblRooznamehs.OrderBy(a => a.SanadId).Where(a => a.SanadTarikhMiladi >= tarikhMiladiAz && a.SanadTarikhMiladi <= tarikhMiladiTa && a.SanadId > 0).ToList();
                    if (hesabhaInfoList.Count == 0)
                    {
                        MessageBoxFarsi.Show("هیچ سابقه‌ای در زمان انتخاب شده وجود ندارد، تاریخ را تغییر دهید", "نبود سابقه اطلاعاتی در گزارش گیری");
                        return;
                    }
                    //برای نامگذاری سرستون‌ها در کلاس مربوطه
                    string[] NameSotunha = { "شماره سند", "تاریخ", "کد حساب", "نام حساب", "شرح سند", "بدهکار", "بستانکار", "تشخیص" };
                    ExcelReports report = new ExcelReports();
                    report.RooznamehReport(hesabhaInfoList, NameSotunha, tarikhShamsiAz, tarikhShamsiTa);
                }
                if (cmbReport.SelectedIndex == 3)
                {
                    string HesabName = txtSearchNam.Text;
                    if (HesabName == "")
                    {
                        MessageBoxFarsi.Show("یک حساب را از جدول نمایش انتخاب کنید", "عدم انتخاب حساب معین");
                        return;
                    }
                    var hesabhaInfoList = db.tblRooznamehs.OrderBy(a => a.SanadId).Where(a => a.NameHesab == HesabName && a.SanadTarikhMiladi >= tarikhMiladiAz && a.SanadTarikhMiladi <= tarikhMiladiTa && a.SanadId > 0).ToList();
                    if (hesabhaInfoList.Count == 0 || hesabhaInfoList == null)
                    {
                        MessageBoxFarsi.Show("هیچ سابقه‌ای برای حساب انتخاب شده در بازه زمانی مورد نظر وجود ندارد", "نبود سابقه اطلاعاتی در گزارش گیری");
                        return;
                    }
                    string[] NameSotunha = { "شماره سند", "تاریخ", "کد حساب", "نام حساب", "شرح سند", "بدهکار", "بستانکار", "مانده", "تشخیص" };
                    ExcelReports report = new ExcelReports();
                    report.MoeinReport(hesabhaInfoList, NameSotunha, tarikhShamsiAz, tarikhShamsiTa, tarikhMiladiAz, tarikhMiladiTa, HesabName);
                }

                if (cmbReport.SelectedIndex == 4)
                {
                    if (dgvDisplay.DataSource == null)
                    {
                        MessageBoxFarsi.Show("یک گروه حساب را از جدول نمایش انتخاب کنید", "عدم انتخاب دفتر کل");
                        return;
                    }
                    string HesabGrouh = dgvDisplay.CurrentRow.Cells[5].Value.ToString();
                    var hesabhaInfoList = db.tblRooznamehs.OrderBy(a => a.SanadId).Where(a => a.NameSumGroupHesab == HesabGrouh && a.SanadTarikhMiladi >= tarikhMiladiAz && a.SanadTarikhMiladi <= tarikhMiladiTa && a.SanadId > 0).ToList();
                    if (hesabhaInfoList.Count == 0 || hesabhaInfoList == null)
                    {
                        MessageBoxFarsi.Show("هیچ سابقه‌ای برای گروه حساب انتخاب شده در بازه زمانی مورد نظر وجود ندارد", "نبود سابقه اطلاعاتی در گزارش گیری");
                        return;
                    }
                    string[] NameSotunha = { "شماره سند", "تاریخ", "کد حساب", "نام حساب", "گروه حساب", "شرح سند", "بدهکار", "بستانکار", "مانده", "تشخیص" };
                    ExcelReports report = new ExcelReports();
                    report.KolReport(hesabhaInfoList, NameSotunha, tarikhShamsiAz, tarikhShamsiTa, tarikhMiladiAz, tarikhMiladiTa, HesabGrouh);
                }
                if (cmbReport.SelectedIndex == 5)
                {
                    if (dgvDisplay.DataSource == null)
                    {
                        MessageBoxFarsi.Show("یک طبقه حساب را از جدول نمایش انتخاب کنید", "عدم انتخاب طبقه حساب");
                        return;
                    }
                    string HesabGrouh = dgvDisplay.CurrentRow.Cells[4].Value.ToString();
                    var hesabhaInfoList = db.tblRooznamehs.OrderBy(a => a.SanadId).Where(a => a.NameGroupHesab == HesabGrouh && a.SanadTarikhMiladi >= tarikhMiladiAz && a.SanadTarikhMiladi <= tarikhMiladiTa && a.SanadId > 0).ToList();
                    if (hesabhaInfoList.Count == 0 || hesabhaInfoList == null)
                    {
                        MessageBoxFarsi.Show("هیچ سابقه‌ای برای گروه حساب انتخاب شده در بازه زمانی مورد نظر وجود ندارد", "نبود سابقه اطلاعاتی در گزارش گیری");
                        return;
                    }
                    string[] NameSotunha = { "شماره سند", "تاریخ", "کد حساب", "نام حساب", "طبقه حساب", "شرح سند", "بدهکار", "بستانکار", "مانده", "تشخیص" };
                    ExcelReports report = new ExcelReports();
                    report.TabagheReport(hesabhaInfoList, NameSotunha, tarikhShamsiAz, tarikhShamsiTa, tarikhMiladiAz, tarikhMiladiTa, HesabGrouh);
                }
            }
            catch (Exception ex)
            {
                MessageBoxFarsi.Show(ex.Message.ToString(), "خطا در گزارش گیری");
            }
        }

        private void txtSearchIdSanad_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                decimal myIdSanad = txtSearchIdSanad.Value;

                var hesabhaInfoList = db.tblRooznamehs.OrderBy(a => a.SanadId).Where(a => a.SanadId == myIdSanad && a.SanadId > 0).Select(a => new { a.SanadId, a.SanadTarikhMiladi, a.SanadTarikhShamsi, a.IdHesab, a.NameHesab, a.NameGroupHesab, a.NameSumGroupHesab, a.SanadTozih, a.BuySellVol, a.ArzeshAfzudehTax, a.EnteghaTax, a.KarmozdMoameleh, a.SanadBedehkar, a.SabadBestankar }).Distinct().ToList();
                List<ReportOfDafater> repList = new List<ReportOfDafater>();
                foreach (var item in hesabhaInfoList)
                {
                    ReportOfDafater tbl = new ReportOfDafater();
                    tbl.IdSanad = item.SanadId;
                    tbl.Tarikh = item.SanadTarikhShamsi;
                    tbl.IdHesab = item.IdHesab;
                    tbl.NameHesab = item.NameHesab;
                    tbl.GroupHesab = item.NameGroupHesab;
                    tbl.SubGroupHesab = item.NameSumGroupHesab;
                    tbl.SanadTozih = item.SanadTozih;
                    tbl.BuySellVol = item.BuySellVol;
                    tbl.ArzeshAfzudehTax = item.ArzeshAfzudehTax;
                    tbl.EnteghaTax = item.EnteghaTax;
                    tbl.KarmozdMoameleh = item.KarmozdMoameleh;
                    tbl.BedehkarTurnOver = item.SanadBedehkar;
                    tbl.BestankarTurnOver = item.SabadBestankar;
                    tbl.Balance = Convert.ToInt64(Math.Abs(Convert.ToDecimal((hesabhaInfoList.Where(a => a.SanadId <= item.SanadId).Select(a => a.SanadBedehkar - a.SabadBestankar).Sum()))));
                    if (item.SanadBedehkar > item.SabadBestankar)
                    {
                        tbl.Mahiyat = "بدهکار";
                    }
                    else if (item.SanadBedehkar < item.SabadBestankar)
                    {
                        tbl.Mahiyat = "بستانکار";
                    }
                    else
                    {
                        tbl.Mahiyat = "-";
                    }
                    repList.Add(tbl);
                }

                //برای نامگذاری سرستون‌ها در کلاس مربوطه
                string[] NameSotunha = { "شماره سند", "تاریخ", "کد حساب", "نام حساب", "طبقه حساب", "زیرطبقه حساب", "شرح سند", "حجم معاملات", "مالیات ارزش افزوده", "مالیات نقل و انتقال", "کارمزد معامله", "بدهکار", "بستانکار", "مانده", "ماهیت" };
                Display calledDgv = new Display();
                calledDgv.ShowDgv(dgvDisplay, NameSotunha, repList);
                dgvDisplay = calledDgv.dgvFromDisplay;
                dgvDisplay.Columns[0].DefaultCellStyle.Format = "N4";
                dgvDisplay.Columns[7].DefaultCellStyle.Format = "N0";
                dgvDisplay.Columns[8].DefaultCellStyle.Format = "N0";
                dgvDisplay.Columns[9].DefaultCellStyle.Format = "N0";
                dgvDisplay.Columns[10].DefaultCellStyle.Format = "N0";
                dgvDisplay.Columns[11].DefaultCellStyle.Format = "N0";
                dgvDisplay.Columns[12].DefaultCellStyle.Format = "N0";
                dgvDisplay.Columns[13].DefaultCellStyle.Format = "N0";
            }
            catch (Exception ex)
            {
                MessageBoxFarsi.Show(ex.Message.ToString(), "خطا در جستجو");
            }
        }
    }
}
