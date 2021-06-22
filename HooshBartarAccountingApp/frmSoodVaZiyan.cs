using BehComponents;
using HooshBartarAccountingApp.DatabaseModel;
using HooshBartarAccountingApp.DataShow;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HooshBartarAccountingApp
{
    public partial class frmSoodVaZiyan : Form
    {
        public frmSoodVaZiyan()
        {
            InitializeComponent();
        }

        AccountDBEntities db = new AccountDBEntities();
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

                var grouhList = db.tblRooznamehs.OrderByDescending(a => a.IdHesab).Where(a => a.SanadTarikhMiladi <= tarikhMiladiTa && a.SanadTarikhMiladi >= tarikhMiladiAz).Select(a => new { a.SanadBedehkar, a.SabadBestankar, a.IdHesab }).ToList();

                List<FinancialStatements> SoodVaZiyanList = new List<FinancialStatements>();
                /////////محاسبه فروش (درآمدهای عملیاتی)
                var grouhDaramadhaList = db.tblTabaghatHesabs.OrderBy(a => a.CodeZirtabagheTa).Where(a => a.CodeTabaghehHesab == 4).Select(a => new { a.ZirTzbagheyehHesab, a.CodeZirtabagheAz, a.CodeZirtabagheTa }).ToList();
                int i = 1001;
                int j = 1999;
                foreach (var item in grouhDaramadhaList)
                {
                    if (item.CodeZirtabagheAz >= i && item.CodeZirtabagheTa <= j)
                    {
                        FinancialStatements DaramadForush = new FinancialStatements()
                        {
                            NameGroupHesab = item.ZirTzbagheyehHesab,
                            Mandeh = grouhList.Where(a => a.IdHesab.ToString().Substring(0, 1) == "4" && Convert.ToInt32(a.IdHesab.ToString().Substring(3)) <= j && Convert.ToInt32(a.IdHesab.ToString().Substring(3)) >= i).Select(a => a.SabadBestankar - a.SanadBedehkar).Sum(),
                        };
                        SoodVaZiyanList.Add(DaramadForush);
                    }
                }
                //////////محاسبه بهای تمام شده فروش
                var grouhHazinehaList = db.tblTabaghatHesabs.OrderBy(a => a.CodeZirtabagheTa).Where(a => a.CodeTabaghehHesab == 5).Select(a => new { a.ZirTzbagheyehHesab, a.CodeZirtabagheAz, a.CodeZirtabagheTa }).ToList();
                i = 1001;
                j = 1999;
                foreach (var item in grouhHazinehaList)
                {
                    if (item.CodeZirtabagheAz >= i && item.CodeZirtabagheTa <= j)
                    {
                        FinancialStatements Baha = new FinancialStatements()
                        {
                            NameGroupHesab = item.ZirTzbagheyehHesab,
                            Mandeh = grouhList.Where(a => a.IdHesab.ToString().Substring(0, 1) == "5" && Convert.ToInt32(a.IdHesab.ToString().Substring(3)) <= j && Convert.ToInt32(a.IdHesab.ToString().Substring(3)) >= i).Select(a => a.SabadBestankar - a.SanadBedehkar).Sum(),
                        };
                        SoodVaZiyanList.Add(Baha);
                    }
                }
                ///////////محاسبه سود ناخالص
                FinancialStatements SoodNaKhales = new FinancialStatements()
                {
                    NameGroupHesab = "سود ناخالص",
                    Mandeh = SoodVaZiyanList.Select(a => a.Mandeh).Sum(),
                    Id = 1,
                };
                SoodVaZiyanList.Add(SoodNaKhales);
                /////////محاسبه هزینه‌های عمومی و اداری
                i = 2001;
                j = 2999;
                foreach (var item in grouhHazinehaList)
                {
                    if (item.CodeZirtabagheAz >= i && item.CodeZirtabagheTa <= j)
                    {
                        FinancialStatements Edari = new FinancialStatements()
                        {
                            NameGroupHesab = item.ZirTzbagheyehHesab,
                            Mandeh = grouhList.Where(a => a.IdHesab.ToString().Substring(0, 1) == "5" && Convert.ToInt32(a.IdHesab.ToString().Substring(3)) <= j && Convert.ToInt32(a.IdHesab.ToString().Substring(3)) >= i).Select(a => a.SabadBestankar - a.SanadBedehkar).Sum(),
                        };
                        SoodVaZiyanList.Add(Edari);
                    }
                }
                ///////////محاسبه سود (زیان) عملیاتی
                FinancialStatements SoodAmaliyati = new FinancialStatements()
                {
                    NameGroupHesab = "سود (زیان) عملیاتی",
                    Mandeh = SoodVaZiyanList.Where(a => a.Id != 1).Select(a => a.Mandeh).Sum(),
                    Id = 2,
                };
                SoodVaZiyanList.Add(SoodAmaliyati);
                /////////محاسبه سایر درآمدهای عملیاتی
                i = 2001;
                j = 2999;
                foreach (var item in grouhDaramadhaList)
                {
                    if (item.CodeZirtabagheAz >= i && item.CodeZirtabagheTa <= j)
                    {
                        FinancialStatements SayerDaramadAmaliyati = new FinancialStatements()
                        {
                            NameGroupHesab = item.ZirTzbagheyehHesab,
                            Mandeh = grouhList.Where(a => a.IdHesab.ToString().Substring(0, 1) == "4" && Convert.ToInt32(a.IdHesab.ToString().Substring(3)) <= j && Convert.ToInt32(a.IdHesab.ToString().Substring(3)) >= i).Select(a => a.SabadBestankar - a.SanadBedehkar).Sum(),
                        };
                        SoodVaZiyanList.Add(SayerDaramadAmaliyati);
                    }
                }
                /////////محاسبه سایر هزینه‌های عملیاتی
                i = 3001;
                j = 3999;
                foreach (var item in grouhHazinehaList)
                {
                    if (item.CodeZirtabagheAz >= i && item.CodeZirtabagheTa <= j)
                    {
                        FinancialStatements SayerHazinehAmaliyati = new FinancialStatements()
                        {
                            NameGroupHesab = item.ZirTzbagheyehHesab,
                            Mandeh = grouhList.Where(a => a.IdHesab.ToString().Substring(0, 1) == "5" && Convert.ToInt32(a.IdHesab.ToString().Substring(3)) <= j && Convert.ToInt32(a.IdHesab.ToString().Substring(3)) >= i).Select(a => a.SabadBestankar - a.SanadBedehkar).Sum(),
                        };
                        SoodVaZiyanList.Add(SayerHazinehAmaliyati);
                    }
                }
                ///////////محاسبه سود (زیان) قبل از بهره و مالیات
                FinancialStatements Ebit = new FinancialStatements()
                {
                    NameGroupHesab = "سود (زیان) قبل از بهره و مالیات",
                    Mandeh = SoodVaZiyanList.Where(a => a.Id != 1 && a.Id != 2).Select(a => a.Mandeh).Sum(),
                    Id = 3,
                };
                SoodVaZiyanList.Add(Ebit);
                /////////محاسبه هزینه‌های مالی
                i = 4001;
                j = 4999;
                foreach (var item in grouhHazinehaList)
                {
                    if (item.CodeZirtabagheAz >= i && item.CodeZirtabagheTa <= j)
                    {
                        FinancialStatements HazinehMali = new FinancialStatements()
                        {
                            NameGroupHesab = item.ZirTzbagheyehHesab,
                            Mandeh = grouhList.Where(a => a.IdHesab.ToString().Substring(0, 1) == "5" && Convert.ToInt32(a.IdHesab.ToString().Substring(3)) <= j && Convert.ToInt32(a.IdHesab.ToString().Substring(3)) >= i).Select(a => a.SabadBestankar - a.SanadBedehkar).Sum(),
                        };
                        SoodVaZiyanList.Add(HazinehMali);
                    }
                }
                /////////محاسبه سایر درآمدهای غیر عملیاتی
                i = 3001;
                j = 3999;
                foreach (var item in grouhDaramadhaList)
                {
                    if (item.CodeZirtabagheAz >= i && item.CodeZirtabagheTa <= j)
                    {
                        FinancialStatements SayerDaramadGheyrAmaliyati = new FinancialStatements()
                        {
                            NameGroupHesab = item.ZirTzbagheyehHesab,
                            Mandeh = grouhList.Where(a => a.IdHesab.ToString().Substring(0, 1) == "4" && Convert.ToInt32(a.IdHesab.ToString().Substring(3)) <= j && Convert.ToInt32(a.IdHesab.ToString().Substring(3)) >= i).Select(a => a.SabadBestankar - a.SanadBedehkar).Sum(),
                        };
                        SoodVaZiyanList.Add(SayerDaramadGheyrAmaliyati);
                    }
                }
                /////////محاسبه سایر هزینه‌های غیرعملیاتی
                i = 5001;
                j = 5999;
                foreach (var item in grouhHazinehaList)
                {
                    if (item.CodeZirtabagheAz >= i && item.CodeZirtabagheTa <= j)
                    {
                        FinancialStatements SayerHazinehGheyrAmaliyati = new FinancialStatements()
                        {
                            NameGroupHesab = item.ZirTzbagheyehHesab,
                            Mandeh = grouhList.Where(a => a.IdHesab.ToString().Substring(0, 1) == "5" && Convert.ToInt32(a.IdHesab.ToString().Substring(3)) <= j && Convert.ToInt32(a.IdHesab.ToString().Substring(3)) >= i).Select(a => a.SabadBestankar - a.SanadBedehkar).Sum(),
                        };
                        SoodVaZiyanList.Add(SayerHazinehGheyrAmaliyati);
                    }
                }
                ///////////محاسبه سود (زیان) قبل از مالیات
                FinancialStatements Ebt = new FinancialStatements()
                {
                    NameGroupHesab = "سود (زیان) قبل از مالیات",
                    Mandeh = SoodVaZiyanList.Where(a => a.Id != 1 && a.Id != 2 && a.Id != 3).Select(a => a.Mandeh).Sum(),
                    Id = 4,
                };
                SoodVaZiyanList.Add(Ebt);
                /////////محاسبه مالیات
                i = 6001;
                j = 6999;
                foreach (var item in grouhHazinehaList)
                {
                    if (item.CodeZirtabagheAz >= i && item.CodeZirtabagheTa <= j)
                    {
                        FinancialStatements MaliyatHazineh = new FinancialStatements()
                        {
                            NameGroupHesab = item.ZirTzbagheyehHesab,
                            Mandeh = grouhList.Where(a => a.IdHesab.ToString().Substring(0, 1) == "5" && Convert.ToInt32(a.IdHesab.ToString().Substring(3)) <= j && Convert.ToInt32(a.IdHesab.ToString().Substring(3)) >= i).Select(a => a.SabadBestankar - a.SanadBedehkar).Sum(),
                        };
                        SoodVaZiyanList.Add(MaliyatHazineh);
                    }
                }
                ///////////محاسبه سود خالص
                FinancialStatements NetIncome = new FinancialStatements()
                {
                    NameGroupHesab = "سود خالص",
                    Mandeh = SoodVaZiyanList.Where(a => a.Id != 1 && a.Id != 2 && a.Id != 3 && a.Id != 4).Select(a => a.Mandeh).Sum(),
                    Id = 5,
                };
                SoodVaZiyanList.Add(NetIncome);
                dgvSoodVaZiyan.DataSource = SoodVaZiyanList.Select(a => new { a.NameGroupHesab, a.Mandeh }).ToList();
                dgvSoodVaZiyan.Columns[0].HeaderText = "شرح نام حساب";
                dgvSoodVaZiyan.Columns[1].HeaderText = "مانده";
                dgvSoodVaZiyan.Columns[1].DefaultCellStyle.Format = "#,##0;(#,##0)";
                for (int m = 0; m < dgvSoodVaZiyan.Columns.Count; m++)
                {
                    dgvSoodVaZiyan.Columns[m].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                }
            }
            catch (Exception ex)
            {
                MessageBoxFarsi.Show(ex.Message.ToString(), "خطاء در بارگزاری صورت سود و زیان");
            }
        }

        private void frmSoodVaZiyan_Load(object sender, EventArgs e)
        {
            try
            {
                System.Globalization.PersianCalendar pc = new System.Globalization.PersianCalendar();
                DateTime AzTarikh = DateTime.Now.AddDays(-365);
                mskAzTarikh.Text = pc.GetYear(AzTarikh).ToString() + pc.GetMonth(AzTarikh).ToString("0#") + pc.GetDayOfMonth(AzTarikh).ToString("0#") + pc.GetHour(AzTarikh).ToString("0#") + pc.GetMinute(AzTarikh).ToString("0#") + pc.GetSecond(AzTarikh).ToString("0#");

                DateTime TaTarikh = DateTime.Now.AddDays(1);
                mskTaTarikh.Text = pc.GetYear(TaTarikh).ToString() + pc.GetMonth(TaTarikh).ToString("0#") + pc.GetDayOfMonth(TaTarikh).ToString("0#") + pc.GetHour(TaTarikh).ToString("0#") + pc.GetMinute(TaTarikh).ToString("0#") + pc.GetSecond(TaTarikh).ToString("0#");
            }
            catch (Exception ex)
            {
                MessageBoxFarsi.Show(ex.Message.ToString(), "خطا");
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void btnExcel_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvSoodVaZiyan.DataSource == null || dgvSoodVaZiyan.DataSource == null)
                {
                    MessageBoxFarsi.Show("ابتدا دکمه نمایش را کلیک کنید تا اطلاعات صورت سود و زیان بارگزاری شود");
                    return;
                }
                Microsoft.Office.Interop.Excel.Application app = new Microsoft.Office.Interop.Excel.Application();
                Microsoft.Office.Interop.Excel.Workbook workbook = app.Workbooks.Add(Type.Missing);
                Microsoft.Office.Interop.Excel.Worksheet worksheet = null;
                worksheet = workbook.Sheets["Sheet1"];
                worksheet = workbook.ActiveSheet;
                worksheet.Name = "صورت سود و زیان";
                worksheet.DisplayRightToLeft = true;
                worksheet.Cells[1, 1] = $"صورت سود و زیان از تاریخ: {mskAzTarikh.Text} تا تاریخ {mskTaTarikh.Text}";
                worksheet.Range[worksheet.Cells[1, 1], worksheet.Cells[1, 6]].Merge();
                worksheet.Rows[1].HorizontalAlignment = HorizontalAlignment.Right;
                worksheet.Rows[1].Font.Bold = true;
                worksheet.Cells[2, 1] = "شرح";
                worksheet.Cells[2, 6] = "مانده حساب";
                worksheet.Rows[2].Font.Bold = true;
                for (int j = 0; j < dgvSoodVaZiyan.Rows.Count; j++)
                {
                    worksheet.Cells[j + 3, 1] = dgvSoodVaZiyan.Rows[j].Cells[0].Value;
                    worksheet.Cells[j + 3, 6] = dgvSoodVaZiyan.Rows[j].Cells[1].Value;
                    worksheet.Cells[j + 3, 6].NumberFormat = "#,##0;(#,##0)";
                }
                worksheet.Cells[5, 1].Font.Bold = true;
                worksheet.Cells[5, 6].Font.Bold = true;
                worksheet.Cells[7, 1].Font.Bold = true;
                worksheet.Cells[7, 6].Font.Bold = true;
                worksheet.Cells[10, 1].Font.Bold = true;
                worksheet.Cells[10, 6].Font.Bold = true;
                worksheet.Cells[14, 1].Font.Bold = true;
                worksheet.Cells[14, 6].Font.Bold = true;
                worksheet.Cells[16, 1].Font.Bold = true;
                worksheet.Cells[16, 6].Font.Bold = true;

                Microsoft.Office.Interop.Excel.Range usedRange = worksheet.UsedRange;
                usedRange.Columns.AutoFit();
                usedRange.Columns.Borders.LineStyle = BorderStyle.FixedSingle;
                usedRange.Rows.AutoFit();
                var sfd = new SaveFileDialog();
                sfd.FileName = "صورت سود و زیان";
                sfd.DefaultExt = ".Xlsx";
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    workbook.SaveAs(sfd.FileName, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Microsoft.Office.Interop.Excel.XlSaveAsAccessMode.xlExclusive, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);
                    app.Quit();
                }
                app.Quit();
            }
            catch (Exception ex)
            {
                MessageBoxFarsi.Show(ex.Message.ToString(), "خطا در تهیه خروجی اکسل");
            }
        }
    }
}
