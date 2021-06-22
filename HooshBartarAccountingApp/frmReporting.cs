using Accord;
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
    public partial class frmReporting : Form
    {
        public frmReporting()
        {
            InitializeComponent();
        }
        AccountDBEntities db = new AccountDBEntities();

        private void txtSearch_MouseClick(object sender, MouseEventArgs e)
        {
            txtSearchMoein.Text = "";
        }

        private void frmReporting_Load(object sender, EventArgs e)
        {

            try
            {
                /////////////////////////////
                System.Globalization.PersianCalendar pc = new System.Globalization.PersianCalendar();
                DateTime now = DateTime.Now;
                DateTime yesterday = DateTime.Now.AddDays(-365);
                mskAzTarikh.Text = pc.GetYear(yesterday).ToString() + pc.GetMonth(yesterday).ToString("0#") + pc.GetDayOfMonth(yesterday).ToString("0#") + pc.GetHour(yesterday).ToString("0#") + pc.GetMinute(yesterday).ToString("0#") + pc.GetSecond(yesterday).ToString("0#");
                DateTime tomorrow = DateTime.Now.AddDays(1);
                mskTaTarikh.Text = pc.GetYear(tomorrow).ToString() + pc.GetMonth(tomorrow).ToString("0#") + pc.GetDayOfMonth(tomorrow).ToString("0#") + pc.GetHour(tomorrow).ToString("0#") + pc.GetMinute(tomorrow).ToString("0#") + pc.GetSecond(tomorrow).ToString("0#");
                ////////////////////////////


                var hesabhaList = db.tblRooznamehs.Select(a => new { a.NameHesab, a.NameSumGroupHesab }).GroupBy(a => a.NameSumGroupHesab).ToList();
                if (hesabhaList != null)
                {
                    foreach (var item in hesabhaList)
                    {
                        cmbDaftarKol.Items.Add(item.Select(a => a.NameSumGroupHesab).FirstOrDefault());
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBoxFarsi.Show(ex.Message.ToString(), "خطا در نمایش صفحه");
            }

        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            try
            {
                string tarikhShamsiAz = mskAzTarikh.Text;
                DateConvert miladiAz = new DateConvert();
                var res1 = miladiAz.ConvertToMiladi(tarikhShamsiAz);
                DateTime tarikhMiladiAz = miladiAz.TarikhMiladi;
                string tarikhShamsiTa = mskTaTarikh.Text;
                DateConvert miladiTa = new DateConvert();
                var res2 = miladiTa.ConvertToMiladi(tarikhShamsiTa);
                DateTime tarikhMiladiTa = miladiTa.TarikhMiladi;
                if (res1 == false || res2 == false)
                {
                    MessageBoxFarsi.Show("تاریخ به درستی وارد نشده است", "فرمت تاریخ نادرست");
                    return;
                }

                List<IGrouping<string, tblRooznameh>> hesabhaList = new List<IGrouping<string, tblRooznameh>>();
                if (cmbDaftarKol.Text == "")
                {
                    string search = txtSearchMoein.Text;
                    hesabhaList = db.tblRooznamehs.Where(a => a.SanadTarikhMiladi >= tarikhMiladiAz && a.SanadTarikhMiladi <= tarikhMiladiTa).Where(a => a.NameHesab.Contains(search)).GroupBy(a => a.NameHesab).ToList();
                }
                else
                {
                    string search = txtSearchMoein.Text;
                    hesabhaList = db.tblRooznamehs.Where(a => a.SanadTarikhMiladi >= tarikhMiladiAz && a.SanadTarikhMiladi <= tarikhMiladiTa).Where(a => a.NameHesab.Contains(search)).Where(a => a.NameSumGroupHesab == cmbDaftarKol.Text).GroupBy(a => a.NameHesab).ToList();
                }


                List<Reports> reportsList = new List<Reports>();
                foreach (var item in hesabhaList)
                {
                    var shart = false;
                    if (rbnAll.Checked)
                    {
                        shart = true;
                    }
                    else if (rbnMandehdar.Checked)
                    {
                        shart = item.Select(a => a.SanadBedehkar).Sum() - item.Select(a => a.SabadBestankar).Sum() != 0;
                    }

                    if (shart)
                    {
                        string mahiyat = "";
                        if (item.Select(a => a.SanadBedehkar).Sum() > item.Select(a => a.SabadBestankar).Sum())
                        {
                            mahiyat = "بدهکار";
                        }
                        else if (item.Select(a => a.SanadBedehkar).Sum() < item.Select(a => a.SabadBestankar).Sum())
                        {
                            mahiyat = "بستانکار";
                        }
                        else if (item.Select(a => a.SanadBedehkar).Sum() == item.Select(a => a.SabadBestankar).Sum())
                        {
                            mahiyat = "-";
                        }

                        int? tedadKol = 0;
                        if (item.Select(a => a.BuySellVol).Sum() == 0)
                        {
                            tedadKol = 1;
                        }
                        else if (item.Select(a => a.BuySellVol).Sum() != 0)
                        {
                            tedadKol = item.Select(a => a.BuySellVol).Sum();
                        }
                        Reports r = new Reports()
                        {
                            Tarikh = item.Select(a => a.SanadTarikhShamsi).Last(),
                            IdHesab = item.Select(a => a.IdHesab).First(),
                            NameHesab = item.Select(a => a.NameHesab).First(),
                            SubGroupHesab = item.Select(a => a.NameSumGroupHesab).First(),
                            TabagehHesab = item.Select(a => a.NameGroupHesab).First(),
                            BalanceTotalVolume = item.Select(a => a.BuySellVol).Sum(),
                            TodayBalanceAvg = Convert.ToInt64(Math.Abs(Convert.ToDecimal(item.Select(a => a.SanadBedehkar).Sum() - item.Select(a => a.SabadBestankar).Sum()))) / tedadKol,
                            BedehkarTurnOver = item.Select(a => a.SanadBedehkar).Sum(),
                            BestankarTurnOver = item.Select(a => a.SabadBestankar).Sum(),
                            Balance = Convert.ToInt64(Math.Abs(Convert.ToDecimal(item.Select(a => a.SanadBedehkar).Sum() - item.Select(a => a.SabadBestankar).Sum()))),
                            Mahiyat = mahiyat,
                        };
                        reportsList.Add(r);
                    }
                }


                txtBalanceSum.Value = Convert.ToDecimal(reportsList.Select(a => a.Balance).Sum());

                //برای نامگذاری سرستون‌ها در کلاس مربوطه
                string[] NameSotunha = { "آخرین تاریخ", "کد حساب", "نام حساب", "دفتر کل", "طبقه حساب", "موجودی", "میانگین موزون", "گردش بدهکار", "گردش بستانکار", "مانده حساب", "ماهیت" };
                Display calledDgv = new Display();
                calledDgv.ShowDgv(dgvDisplay, NameSotunha, reportsList.OrderByDescending(a => a.Balance).ToList());
                dgvDisplay = calledDgv.dgvFromDisplay;
                dgvDisplay.Columns[5].DefaultCellStyle.Format = "N0";
                dgvDisplay.Columns[6].DefaultCellStyle.Format = "N2";
                dgvDisplay.Columns[7].DefaultCellStyle.Format = "N0";
                dgvDisplay.Columns[8].DefaultCellStyle.Format = "N0";
                dgvDisplay.Columns[9].DefaultCellStyle.Format = "N0";
            }
            catch (Exception ex)
            {
                MessageBoxFarsi.Show(ex.Message.ToString(), "خطا در جستجو");
            }
        }

        private void txtSearchMoein_MouseClick(object sender, MouseEventArgs e)
        {
            txtSearchMoein.Text = "";
        }

        private void txtSearchKol_MouseClick(object sender, MouseEventArgs e)
        {
            txtSearchKol.Text = "";
        }

        private void txtSearchTabagheh_MouseClick(object sender, MouseEventArgs e)
        {
            txtSearchTabagheh.Text = "";
        }

        private void txtSearchKol_TextChanged(object sender, EventArgs e)
        {
            try
            {
                string tarikhShamsiAz = mskAzTarikh.Text;
                DateConvert miladiAz = new DateConvert();
                var res1 = miladiAz.ConvertToMiladi(tarikhShamsiAz);
                DateTime tarikhMiladiAz = miladiAz.TarikhMiladi;
                string tarikhShamsiTa = mskTaTarikh.Text;
                DateConvert miladiTa = new DateConvert();
                var res2 = miladiTa.ConvertToMiladi(tarikhShamsiTa);
                DateTime tarikhMiladiTa = miladiTa.TarikhMiladi;
                if (res1 == false || res2 == false)
                {
                    MessageBoxFarsi.Show("تاریخ به درستی وارد نشده است", "فرمت تاریخ نادرست");
                    return;
                }

                string search = txtSearchKol.Text;
                var hesabhaList = db.tblRooznamehs.Where(a => a.SanadTarikhMiladi >= tarikhMiladiAz && a.SanadTarikhMiladi <= tarikhMiladiTa).Where(a => a.NameSumGroupHesab.Contains(search)).GroupBy(a => a.NameSumGroupHesab).ToList();

                List<Reports> reportsList = new List<Reports>();
                foreach (var item in hesabhaList)
                {
                    var shart = false;
                    if (rbnAll.Checked)
                    {
                        shart = true;
                    }
                    else if (rbnMandehdar.Checked)
                    {
                        shart = item.Select(a => a.SanadBedehkar).Sum() - item.Select(a => a.SabadBestankar).Sum() != 0;
                    }

                    if (shart)
                    {
                        string mahiyat = "";
                        if (item.Select(a => a.SanadBedehkar).Sum() > item.Select(a => a.SabadBestankar).Sum())
                        {
                            mahiyat = "بدهکار";
                        }
                        else if (item.Select(a => a.SanadBedehkar).Sum() < item.Select(a => a.SabadBestankar).Sum())
                        {
                            mahiyat = "بستانکار";
                        }
                        else if (item.Select(a => a.SanadBedehkar).Sum() == item.Select(a => a.SabadBestankar).Sum())
                        {
                            mahiyat = "-";
                        }

                        int? tedadKol = 0;
                        if (item.Select(a => a.BuySellVol).Sum() == 0)
                        {
                            tedadKol = 1;
                        }
                        else if (item.Select(a => a.BuySellVol).Sum() != 0)
                        {
                            tedadKol = item.Select(a => a.BuySellVol).Sum();
                        }
                        Reports r = new Reports()
                        {
                            Tarikh = item.Select(a => a.SanadTarikhShamsi).Last(),
                            IdHesab = Convert.ToInt64("0"),
                            NameHesab = "-",
                            SubGroupHesab = item.Select(a => a.NameSumGroupHesab).First(),
                            TabagehHesab = "-",
                            BalanceTotalVolume = Convert.ToInt64("0"),
                            TodayBalanceAvg = Convert.ToInt64("0"),
                            BedehkarTurnOver = item.Select(a => a.SanadBedehkar).Sum(),
                            BestankarTurnOver = item.Select(a => a.SabadBestankar).Sum(),
                            Balance = Convert.ToInt64(Math.Abs(Convert.ToDecimal(item.Select(a => a.SanadBedehkar).Sum() - item.Select(a => a.SabadBestankar).Sum()))),
                            Mahiyat = mahiyat,
                        };
                        reportsList.Add(r);
                    }
                }

                txtBalanceSum.Value = Convert.ToDecimal(reportsList.Select(a => a.Balance).Sum());

                //برای نامگذاری سرستون‌ها در کلاس مربوطه
                string[] NameSotunha = { "تاریخ آخرین تراکنش", "کد حساب", "نام حساب", "دفتر کل", "طبقه حساب", "موجودی", "میانگین موزون", "گردش بدهکار", "گردش بستانکار", "مانده حساب", "ماهیت" };
                Display calledDgv = new Display();
                calledDgv.ShowDgv(dgvDisplay, NameSotunha, reportsList.OrderByDescending(a => a.Balance).ToList());
                dgvDisplay = calledDgv.dgvFromDisplay;
                dgvDisplay.Columns[5].DefaultCellStyle.Format = "N0";
                dgvDisplay.Columns[6].DefaultCellStyle.Format = "N2";
                dgvDisplay.Columns[7].DefaultCellStyle.Format = "N0";
                dgvDisplay.Columns[8].DefaultCellStyle.Format = "N0";
                dgvDisplay.Columns[9].DefaultCellStyle.Format = "N0";
            }
            catch (Exception ex)
            {
                MessageBoxFarsi.Show(ex.Message.ToString(), "خطا در جستجو");
            }
        }

        private void txtSearchTabagheh_TextChanged(object sender, EventArgs e)
        {
            try
            {
                string tarikhShamsiAz = mskAzTarikh.Text;
                DateConvert miladiAz = new DateConvert();
                var res1 = miladiAz.ConvertToMiladi(tarikhShamsiAz);
                DateTime tarikhMiladiAz = miladiAz.TarikhMiladi;
                string tarikhShamsiTa = mskTaTarikh.Text;
                DateConvert miladiTa = new DateConvert();
                var res2 = miladiTa.ConvertToMiladi(tarikhShamsiTa);
                DateTime tarikhMiladiTa = miladiTa.TarikhMiladi;
                if (res1 == false || res2 == false)
                {
                    MessageBoxFarsi.Show("تاریخ به درستی وارد نشده است", "فرمت تاریخ نادرست");
                    return;
                }

                string search = txtSearchTabagheh.Text;
                var hesabhaList = db.tblRooznamehs.Where(a => a.SanadTarikhMiladi >= tarikhMiladiAz && a.SanadTarikhMiladi <= tarikhMiladiTa).Where(a => a.NameGroupHesab.Contains(search)).GroupBy(a => a.NameGroupHesab).ToList();

                List<Reports> reportsList = new List<Reports>();
                foreach (var item in hesabhaList)
                {
                    var shart = false;
                    if (rbnAll.Checked)
                    {
                        shart = true;
                    }
                    else if (rbnMandehdar.Checked)
                    {
                        shart = item.Select(a => a.SanadBedehkar).Sum() - item.Select(a => a.SabadBestankar).Sum() != 0;
                    }

                    if (shart)
                    {
                        string mahiyat = "";
                        if (item.Select(a => a.SanadBedehkar).Sum() > item.Select(a => a.SabadBestankar).Sum())
                        {
                            mahiyat = "بدهکار";
                        }
                        else if (item.Select(a => a.SanadBedehkar).Sum() < item.Select(a => a.SabadBestankar).Sum())
                        {
                            mahiyat = "بستانکار";
                        }
                        else if (item.Select(a => a.SanadBedehkar).Sum() == item.Select(a => a.SabadBestankar).Sum())
                        {
                            mahiyat = "-";
                        }

                        int? tedadKol = 0;
                        if (item.Select(a => a.BuySellVol).Sum() == 0)
                        {
                            tedadKol = 1;
                        }
                        else if (item.Select(a => a.BuySellVol).Sum() != 0)
                        {
                            tedadKol = item.Select(a => a.BuySellVol).Sum();
                        }
                        Reports r = new Reports()
                        {
                            Tarikh = item.Select(a => a.SanadTarikhShamsi).Last(),
                            IdHesab = Convert.ToInt64("0"),
                            NameHesab = "-",
                            SubGroupHesab = "-",
                            TabagehHesab = item.Select(a => a.NameGroupHesab).First(),
                            BalanceTotalVolume = Convert.ToInt64("0"),
                            TodayBalanceAvg = Convert.ToInt64("0"),
                            BedehkarTurnOver = item.Select(a => a.SanadBedehkar).Sum(),
                            BestankarTurnOver = item.Select(a => a.SabadBestankar).Sum(),
                            Balance = Convert.ToInt64(Math.Abs(Convert.ToDecimal(item.Select(a => a.SanadBedehkar).Sum() - item.Select(a => a.SabadBestankar).Sum()))),
                            Mahiyat = mahiyat,
                        };
                        reportsList.Add(r);
                    }
                }


                txtBalanceSum.Value = Convert.ToDecimal(reportsList.Select(a => a.Balance).Sum());


                //برای نامگذاری سرستون‌ها در کلاس مربوطه
                string[] NameSotunha = { "آخرین تاریخ", "کد حساب", "نام حساب", "دفتر کل", "طبقه حساب", "موجودی", "میانگین موزون", "گردش بدهکار", "گردش بستانکار", "مانده حساب", "ماهیت" };
                Display calledDgv = new Display();
                calledDgv.ShowDgv(dgvDisplay, NameSotunha, reportsList.OrderByDescending(a => a.Balance).ToList());
                dgvDisplay = calledDgv.dgvFromDisplay;
                dgvDisplay.Columns[5].DefaultCellStyle.Format = "N0";
                dgvDisplay.Columns[6].DefaultCellStyle.Format = "N2";
                dgvDisplay.Columns[7].DefaultCellStyle.Format = "N0";
                dgvDisplay.Columns[8].DefaultCellStyle.Format = "N0";
                dgvDisplay.Columns[9].DefaultCellStyle.Format = "N0";
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
                string tarikhShamsiTa = mskTaTarikh.Text;
                ExcelReports excel = new ExcelReports();
                excel.AbstractReport(dgvDisplay, tarikhShamsiAz, tarikhShamsiTa);
            }
            catch (Exception ex)
            {
                MessageBoxFarsi.Show(ex.Message.ToString(), "خطا در تهیه خروجی اکسل");
            }
        }

        private void cmbDaftarKol_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                txtSearch_TextChanged(sender, e);

            }
            catch (Exception ex)
            {
                MessageBoxFarsi.Show(ex.Message.ToString(), "خطا");
            }
        }

        private void txtSearchSoodVaZiyan_TextChanged(object sender, EventArgs e)
        {
            try
            {
                List<IGrouping<string, tblRooznameh>> hesabhaListGroupByNameHesab = new List<IGrouping<string, tblRooznameh>>();

                string search = txtSearchSoodVaZiyan.Text;
                string NameSubGroupForush = "حساب فروش (درآمدهای عملیاتی)";
                string NameSubGroupBaha = "حساب بهای تمام شده فروش";

                var firstList = db.tblRooznamehs.GroupBy(a =>a.SanadId).ToList();


               List<tblRooznameh> finalList = new List<tblRooznameh>();
                foreach (var item in firstList)
                {
                    var num = item.Where(a => a.NameHesab.Contains(search)).ToList().Count;
                    if (num>0)
                    {
                        tblRooznameh tbl = new tblRooznameh()
                        {
                            Id = item.Select(a => a.Id).FirstOrDefault(),
                            IdHesab = item.Where(a => a.NameHesab.Contains(search)).Select(a => a.IdHesab).FirstOrDefault(),
                            NameHesab = item.Where(a => a.NameHesab.Contains(search)).Select(a => a.NameHesab).FirstOrDefault(),
                            NameGroupHesab = item.Where(a => a.NameHesab.Contains(search)).Select(a => a.NameGroupHesab).FirstOrDefault(),
                            NameSumGroupHesab = item.Where(a => a.NameHesab.Contains(search)).Select(a => a.NameSumGroupHesab).FirstOrDefault(),
                            ArzeshAfzudehTax = item.Where(a => a.NameHesab.Contains(search)).Select(a => a.ArzeshAfzudehTax).Sum(),
                            BedOrBes = item.Where(a => a.NameHesab.Contains(search)).Select(a => a.BedOrBes).FirstOrDefault(),
                            BuySellVol = item.Where(a => a.NameHesab.Contains(search)).Select(a => a.BuySellVol).Sum(),
                            EnteghaTax = item.Where(a => a.NameHesab.Contains(search)).Select(a => a.EnteghaTax).Sum(),
                            KarmozdMoameleh = item.Where(a => a.NameHesab.Contains(search)).Select(a => a.KarmozdMoameleh).Sum(),
                            SabadBestankar = item.Where(a => a.NameSumGroupHesab == NameSubGroupForush || a.NameSumGroupHesab == NameSubGroupBaha).Select(a => a.SabadBestankar).Sum(),
                            SanadBedehkar = item.Where(a => a.NameSumGroupHesab == NameSubGroupForush || a.NameSumGroupHesab == NameSubGroupBaha).Select(a => a.SanadBedehkar).Sum(),
                            SanadId = item.Where(a => a.NameHesab.Contains(search)).Select(a => a.SanadId).FirstOrDefault(),
                            SanadTarikhShamsi = item.Where(a => a.NameHesab.Contains(search)).Select(a => a.SanadTarikhShamsi).FirstOrDefault(),
                            SanadTozih = item.Where(a => a.NameHesab.Contains(search)).Select(a => a.SanadTozih).FirstOrDefault(),
                        };
                        finalList.Add(tbl);
                    }                       
                                     
                }

                hesabhaListGroupByNameHesab = finalList.OrderBy(a => a.SanadTarikhMiladi).GroupBy(a => a.NameHesab).ToList();
                //////
                List<Reports> reportsList = new List<Reports>();
                foreach (var item in hesabhaListGroupByNameHesab)
                {
                    string mahiyat = "";
                    if (item.Select(a => a.SanadBedehkar).Sum() > item.Select(a => a.SabadBestankar).Sum())
                    {
                        mahiyat = "بدهکار";
                    }
                    else if (item.Select(a => a.SanadBedehkar).Sum() < item.Select(a => a.SabadBestankar).Sum())
                    {
                        mahiyat = "بستانکار";
                    }
                    else if (item.Select(a => a.SanadBedehkar).Sum() == item.Select(a => a.SabadBestankar).Sum())
                    {
                        mahiyat = "-";
                    }

                    int? tedadKol = 0;
                    if (item.Select(a => a.BuySellVol).Sum() == 0)
                    {
                        tedadKol = 1;
                    }
                    else if (item.Select(a => a.BuySellVol).Sum() != 0)
                    {
                        tedadKol = item.Select(a => a.BuySellVol).Sum();
                    }
                    Reports r = new Reports()
                    {
                        Tarikh = item.Select(a => a.SanadTarikhShamsi).Last(),
                        IdHesab = item.Select(a => a.IdHesab).First(),
                        NameHesab = item.Select(a => a.NameHesab).First(),
                        SubGroupHesab = item.Select(a => a.NameSumGroupHesab).First(),
                        TabagehHesab = item.Select(a => a.NameGroupHesab).First(),
                        BalanceTotalVolume = item.Select(a => a.BuySellVol).Sum(),
                        TodayBalanceAvg =null,
                        BedehkarTurnOver = item.Select(a => a.SanadBedehkar).Sum(),
                        BestankarTurnOver = item.Select(a => a.SabadBestankar).Sum(),
                        Balance = Convert.ToInt64(Math.Abs(Convert.ToDecimal(item.Select(a => a.SanadBedehkar).Sum() - item.Select(a => a.SabadBestankar).Sum()))),
                        Mahiyat = mahiyat,
                    };
                    reportsList.Add(r);
                }


                txtBalanceSum.Value = Convert.ToDecimal(reportsList.Select(a => a.Balance).Sum());

                //برای نامگذاری سرستون‌ها در کلاس مربوطه
                string[] NameSotunha = { "آخرین تاریخ", "کد حساب", "نام حساب", "دفتر کل", "طبقه حساب", "موجودی", "میانگین موزون", "گردش بدهکار", "گردش بستانکار", "مانده حساب", "ماهیت" };
                Display calledDgv = new Display();
                calledDgv.ShowDgv(dgvDisplay, NameSotunha, reportsList.OrderByDescending(a => a.Balance).ToList());
                dgvDisplay = calledDgv.dgvFromDisplay;
                dgvDisplay.Columns[5].DefaultCellStyle.Format = "N0";
                dgvDisplay.Columns[6].DefaultCellStyle.Format = "N2";
                dgvDisplay.Columns[7].DefaultCellStyle.Format = "N0";
                dgvDisplay.Columns[8].DefaultCellStyle.Format = "N0";
                dgvDisplay.Columns[9].DefaultCellStyle.Format = "N0";
            }
            catch (Exception ex)
            {
                MessageBoxFarsi.Show(ex.Message.ToString(), "خطا در جستجو");
            }
        }

        private void txtSearchSoodVaZiyan_MouseClick(object sender, MouseEventArgs e)
        {
            try
            {
                txtSearchSoodVaZiyan.Text = "";
            }
            catch (Exception ex)
            {
                MessageBoxFarsi.Show(ex.Message.ToString(), "خطا در جستجو");
            }
        }
    }
}
