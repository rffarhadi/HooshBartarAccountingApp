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
    public partial class frmTaraznameh : Form
    {
        public frmTaraznameh()
        {
            InitializeComponent();
        }

        AccountDBEntities db = new AccountDBEntities();
        private void frmTaraznameh_Load(object sender, EventArgs e)
        {
            try
            {
                System.Globalization.PersianCalendar pc = new System.Globalization.PersianCalendar();
                DateTime now = DateTime.Now.AddDays(1);
                mskTarikh.Text = pc.GetYear(now).ToString() + pc.GetMonth(now).ToString("0#") + pc.GetDayOfMonth(now).ToString("0#") + pc.GetHour(now).ToString("0#") + pc.GetMinute(now).ToString("0#") + pc.GetSecond(now).ToString("0#");
            }
            catch (Exception ex)
            {

                MessageBoxFarsi.Show(ex.Message.ToString(), "خطا");
            }
        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void btnReport_Click(object sender, EventArgs e)
        {
            try
            {
                string tarikhShamsi = mskTarikh.Text;
                DateConvert miladi = new DateConvert();
                miladi.ConvertToMiladi(tarikhShamsi);
                DateTime tarikhMiladi = miladi.TarikhMiladi;
                var grouhList = db.tblRooznamehs.OrderByDescending(a => a.IdHesab).Where(a => a.SanadTarikhMiladi <= tarikhMiladi).Select(a => new { a.SanadBedehkar, a.SabadBestankar, a.IdHesab }).ToList();
                var grouhDaraeiJariNamesList = db.tblTabaghatHesabs.OrderByDescending(a => a.CodeZirtabagheTa).Where(a => a.CodeTabaghehHesab == 1 && a.CodeZirtabagheTa <= 7999).Select(a => new { a.ZirTzbagheyehHesab }).ToList();
                var grouhDaraeiGheyrJariNamesList = db.tblTabaghatHesabs.OrderByDescending(a => a.CodeZirtabagheTa).Where(a => a.CodeTabaghehHesab == 1 && a.CodeZirtabagheTa > 7999).Select(a => new { a.ZirTzbagheyehHesab }).ToList();
                List<FinancialStatements> RightHandList = new List<FinancialStatements>();
                /////////////////////شروع محاسبه جمع حساب‌های دارایی غیرجاری
                int i = 16001;
                int j = 16999;
                foreach (var item in grouhDaraeiGheyrJariNamesList)
                {
                    FinancialStatements t = new FinancialStatements()
                    {
                        NameGroupHesab = item.ZirTzbagheyehHesab,
                        Mandeh = grouhList.Where(a => a.IdHesab.ToString().Substring(0, 1) == "1" && Convert.ToInt32(a.IdHesab.ToString().Substring(3)) <= j && Convert.ToInt32(a.IdHesab.ToString().Substring(3)) >= i).Select(a => a.SanadBedehkar - a.SabadBestankar).Sum(),
                    };
                    RightHandList.Add(t);
                    if (i >= 8001 && j <= 8999)
                    {
                        FinancialStatements t2 = new FinancialStatements()
                        {
                            NameGroupHesab = "جمع دارایی‌های غیر جاری",
                            Mandeh = grouhList.Where(a => a.IdHesab.ToString().Substring(0, 1) == "1" && Convert.ToInt32(a.IdHesab.ToString().Substring(3)) <= 16999 && Convert.ToInt32(a.IdHesab.ToString().Substring(3)) >= 8001).Select(a => a.SanadBedehkar - a.SabadBestankar).Sum(),
                        };
                        RightHandList.Add(t2);
                    }
                    i = i - 1000;
                    j = j - 1000;
                }
                /////////////////////شروع محاسبه جمع حساب‌های دارایی جاری
                i = 7001;
                j = 7999;
                foreach (var item in grouhDaraeiJariNamesList)
                {
                    FinancialStatements t = new FinancialStatements()
                    {
                        NameGroupHesab = item.ZirTzbagheyehHesab,
                        Mandeh = grouhList.Where(a => a.IdHesab.ToString().Substring(0, 1) == "1" && Convert.ToInt32(a.IdHesab.ToString().Substring(3)) <= j && Convert.ToInt32(a.IdHesab.ToString().Substring(3)) >= i).Select(a => a.SanadBedehkar - a.SabadBestankar).Sum(),
                    };
                    RightHandList.Add(t);
                    if (i >= 1001 && j <= 1999)
                    {
                        FinancialStatements totalJari = new FinancialStatements()
                        {
                            NameGroupHesab = "جمع دارایی‌های جاری",
                            Mandeh = grouhList.Where(a => a.IdHesab.ToString().Substring(0, 1) == "1" && Convert.ToInt32(a.IdHesab.ToString().Substring(3)) <= 7999 && Convert.ToInt32(a.IdHesab.ToString().Substring(3)) >= 1001).Select(a => a.SanadBedehkar - a.SabadBestankar).Sum(),
                        };
                        RightHandList.Add(totalJari);
                        FinancialStatements TotalAsset = new FinancialStatements()
                        {
                            NameGroupHesab = "جمع دارایی‌ها",
                            Mandeh = grouhList.Where(a => a.IdHesab.ToString().Substring(0, 1) == "1" && Convert.ToInt32(a.IdHesab.ToString().Substring(3)) <= 16999 && Convert.ToInt32(a.IdHesab.ToString().Substring(3)) >= 1001).Select(a => a.SanadBedehkar - a.SabadBestankar).Sum(),
                        };
                        RightHandList.Add(TotalAsset);
                    }
                    i = i - 1000;
                    j = j - 1000;
                }
                dgvRight.DataSource = RightHandList.Select(a => new { a.NameGroupHesab, a.Mandeh }).ToList();
                dgvRight.Columns[0].HeaderText = "شرح";
                dgvRight.Columns[1].HeaderText = "مانده حساب";
                dgvRight.Columns[1].DefaultCellStyle.Format = "N0";
                dgvRight.Columns[0].Width = 230;
                dgvRight.Columns[1].Width = 120;

                /////////////////////شروع محاسبه جمع حساب‌های حقوق مالکانه
                var grouhHoghughSahebanNames = db.tblTabaghatHesabs.OrderByDescending(a => a.CodeZirtabagheTa).Where(a => a.CodeTabaghehHesab == 3 && a.CodeZirtabagheTa <= 6999).Select(a => new { a.ZirTzbagheyehHesab, a.CodeZirtabagheAz, a.CodeZirtabagheTa }).ToList();
                List<FinancialStatements> HoghughSahebanList = new List<FinancialStatements>();
                i = 6001;
                j = 6999;
                foreach (var item in grouhHoghughSahebanNames)
                {
                    FinancialStatements t = new FinancialStatements();
                    t.NameGroupHesab = item.ZirTzbagheyehHesab;
                    if (item.CodeZirtabagheAz >= 3001 && item.CodeZirtabagheTa <= 3999)
                    {
                        var TaraznamehMandeh = grouhList.Where(a => a.IdHesab.ToString().Substring(0, 1) == "3" && Convert.ToInt32(a.IdHesab.ToString().Substring(3)) <= j && Convert.ToInt32(a.IdHesab.ToString().Substring(3)) >= i).Select(a => a.SabadBestankar - a.SanadBedehkar).Sum();
                        var DaramadhaMandeh = grouhList.Where(a => a.IdHesab.ToString().Substring(0, 1) == "4").Select(a => a.SabadBestankar - a.SanadBedehkar).Sum();
                        var HazihehaMandeh = grouhList.Where(a => a.IdHesab.ToString().Substring(0, 1) == "5").Select(a => a.SanadBedehkar - a.SabadBestankar).Sum();
                        t.Mandeh = TaraznamehMandeh + DaramadhaMandeh - HazihehaMandeh;
                    }
                    else
                    {
                        t.Mandeh = grouhList.Where(a => a.IdHesab.ToString().Substring(0, 1) == "3" && Convert.ToInt32(a.IdHesab.ToString().Substring(3)) <= j && Convert.ToInt32(a.IdHesab.ToString().Substring(3)) >= i).Select(a => a.SabadBestankar - a.SanadBedehkar).Sum();
                    }
                    HoghughSahebanList.Add(t);
                    if (i >= 1001 && j <= 1999)
                    {
                        FinancialStatements TotalEquity = new FinancialStatements()
                        {
                            NameGroupHesab = "جمع حقوق مالکانه",
                            Mandeh = HoghughSahebanList.Sum(a => a.Mandeh),
                        };
                        HoghughSahebanList.Add(TotalEquity);
                    }
                    i = i - 1000;
                    j = j - 1000;
                }
                /////////////////////شروع محاسبه جمع حساب‌های بدهی غیرجاری
                var grouhBedehiJariNamesList = db.tblTabaghatHesabs.OrderByDescending(a => a.CodeZirtabagheTa).Where(a => a.CodeTabaghehHesab == 2 && a.CodeZirtabagheTa <= 3999).Select(a => new { a.ZirTzbagheyehHesab }).ToList();
                var grouhBedehiGheyrJariNamesList = db.tblTabaghatHesabs.OrderByDescending(a => a.CodeZirtabagheTa).Where(a => a.CodeTabaghehHesab == 2 && a.CodeZirtabagheTa > 3999).Select(a => new { a.ZirTzbagheyehHesab }).ToList();
                List<FinancialStatements> LeftHandList = new List<FinancialStatements>();
                i = 7001;
                j = 7999;
                foreach (var item in grouhBedehiGheyrJariNamesList)
                {
                    FinancialStatements t = new FinancialStatements()
                    {
                        NameGroupHesab = item.ZirTzbagheyehHesab,
                        Mandeh = grouhList.Where(a => a.IdHesab.ToString().Substring(0, 1) == "2" && Convert.ToInt32(a.IdHesab.ToString().Substring(3)) <= j && Convert.ToInt32(a.IdHesab.ToString().Substring(3)) >= i).Select(a => a.SabadBestankar - a.SanadBedehkar).Sum(),
                    };
                    LeftHandList.Add(t);
                    if (i >= 4001 && j <= 4999)
                    {
                        FinancialStatements t2 = new FinancialStatements()
                        {
                            NameGroupHesab = "جمع بدهی‌های غیرجاری",
                            Mandeh = grouhList.Where(a => a.IdHesab.ToString().Substring(0, 1) == "2" && Convert.ToInt32(a.IdHesab.ToString().Substring(3)) <= 7999 && Convert.ToInt32(a.IdHesab.ToString().Substring(3)) >= 4001).Select(a => a.SabadBestankar - a.SanadBedehkar).Sum(),
                        };
                        LeftHandList.Add(t2);
                    }
                    i = i - 1000;
                    j = j - 1000;
                }
                ////////////////////شروع بدهی جاری               
                i = 3001;
                j = 3999;
                foreach (var item in grouhBedehiJariNamesList)
                {
                    FinancialStatements t = new FinancialStatements()
                    {
                        NameGroupHesab = item.ZirTzbagheyehHesab,
                        Mandeh = grouhList.Where(a => a.IdHesab.ToString().Substring(0, 1) == "2" && Convert.ToInt32(a.IdHesab.ToString().Substring(3)) <= j && Convert.ToInt32(a.IdHesab.ToString().Substring(3)) >= i).Select(a => a.SabadBestankar - a.SanadBedehkar).Sum(),
                    };
                    LeftHandList.Add(t);
                    if (i >= 1001 && j <= 1999)
                    {
                        FinancialStatements t2 = new FinancialStatements()
                        {
                            NameGroupHesab = "جمع بدهی‌های جاری",
                            Mandeh = grouhList.Where(a => a.IdHesab.ToString().Substring(0, 1) == "2" && Convert.ToInt32(a.IdHesab.ToString().Substring(3)) <= 3999 && Convert.ToInt32(a.IdHesab.ToString().Substring(3)) >= 1001).Select(a => a.SabadBestankar - a.SanadBedehkar).Sum(),
                        };
                        LeftHandList.Add(t2);
                    }
                    i = i - 1000;
                    j = j - 1000;
                }
                LeftHandList.AddRange(HoghughSahebanList);
                for (int k = 0; k <= 2; k++)
                {
                    FinancialStatements Khali = new FinancialStatements()
                    {
                        NameGroupHesab = "",
                    };
                    if (k == 2)
                    {
                        FinancialStatements TotalDebtAndEquity = new FinancialStatements()
                        {
                            NameGroupHesab = "جمع بدهی‌ها و حقوق مالکانه",
                            Mandeh = grouhList.Where(a => a.IdHesab.ToString().Substring(0, 1) == "2" || a.IdHesab.ToString().Substring(0, 1) == "3" || a.IdHesab.ToString().Substring(0, 1) == "4" || a.IdHesab.ToString().Substring(0, 1) == "5").Select(a => a.SabadBestankar - a.SanadBedehkar).Sum(),
                        };
                        LeftHandList.Add(TotalDebtAndEquity);
                    }
                    LeftHandList.Add(Khali);
                }
                dgvLeft.DataSource = LeftHandList.Select(a => new { a.NameGroupHesab, a.Mandeh }).ToList();
                dgvLeft.Columns[0].HeaderText = "شرح";
                dgvLeft.Columns[1].HeaderText = "مانده حساب";
                dgvLeft.Columns[1].DefaultCellStyle.Format = "N0";
                dgvLeft.Columns[0].Width = 230;
                dgvLeft.Columns[1].Width = 120;

            }
            catch (Exception ex)
            {
                MessageBoxFarsi.Show(ex.Message.ToString(), "خطا");
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                Microsoft.Office.Interop.Excel.Application app = new Microsoft.Office.Interop.Excel.Application();
                Microsoft.Office.Interop.Excel.Workbook workbook = app.Workbooks.Add(Type.Missing);
                Microsoft.Office.Interop.Excel.Worksheet worksheet = null;
                worksheet = workbook.Sheets["Sheet1"];
                worksheet = workbook.ActiveSheet;
                worksheet.Name = "صورت وضعیت مالی (ترازنامه)";
                worksheet.DisplayRightToLeft = true;
                worksheet.Cells[1, 2] = $"صورت وضعیت مالی (ترازنامه) منتهی به: {mskTarikh.Text}";
                worksheet.Range[worksheet.Cells[1, 2], worksheet.Cells[1, 4]].Merge();
                worksheet.Rows[1].HorizontalAlignment = HorizontalAlignment.Center;
                worksheet.Rows[1].Font.Bold = true;
                worksheet.Cells[2, 2] = "شرح";
                worksheet.Cells[2, 3] = "مانده حساب";
                worksheet.Cells[2, 4] = "شرح";
                worksheet.Cells[2, 5] = "مانده حساب";
                worksheet.Rows[2].Font.Bold = true;
                for (int j = 0; j < dgvRight.Rows.Count; j++)
                {
                    worksheet.Cells[j + 3, 2] = dgvRight.Rows[j].Cells[0].Value;
                    worksheet.Cells[j + 3, 3] = dgvRight.Rows[j].Cells[1].Value;
                    worksheet.Cells[j + 3, 3].NumberFormat = "#,##0";
                    worksheet.Cells[j + 3, 4] = dgvLeft.Rows[j].Cells[0].Value;
                    worksheet.Cells[j + 3, 5] = dgvLeft.Rows[j].Cells[1].Value;
                    worksheet.Cells[j + 3, 5].NumberFormat = "#,##0";
                }
                worksheet.Rows[dgvRight.Rows.Count + 2].Font.Bold = true;
                worksheet.Cells[12, 2].Font.Bold = true;
                worksheet.Cells[12, 3].Font.Bold = true;
                worksheet.Cells[20, 2].Font.Bold = true;
                worksheet.Cells[20, 3].Font.Bold = true;
                worksheet.Cells[7, 4].Font.Bold = true;
                worksheet.Cells[7, 5].Font.Bold = true;
                worksheet.Cells[11, 4].Font.Bold = true;
                worksheet.Cells[11, 5].Font.Bold = true;
                worksheet.Cells[18, 4].Font.Bold = true;
                worksheet.Cells[18, 5].Font.Bold = true;
                worksheet.Cells[20, 4].Font.Bold = true;
                worksheet.Cells[20, 5].Font.Bold = true;
                Microsoft.Office.Interop.Excel.Range usedRange = worksheet.UsedRange;
                usedRange.Columns.AutoFit();
                usedRange.Columns.Borders.LineStyle = BorderStyle.FixedSingle;
                usedRange.Rows.AutoFit();
                var sfd = new SaveFileDialog();
                sfd.FileName = "ترازنامه";
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

        private void btnDisplay_Click(object sender, EventArgs e)
        {
            try
            {

                string tarikhShamsi = mskTarikh.Text;
                DateConvert miladi = new DateConvert();
                miladi.ConvertToMiladi(tarikhShamsi);
                DateTime tarikhMiladi = miladi.TarikhMiladi;
                var grouhList = db.tblRooznamehs.OrderByDescending(a => a.IdHesab).Where(a => a.SanadTarikhMiladi <= tarikhMiladi).Select(a => new { a.SanadBedehkar, a.SabadBestankar, a.IdHesab, a.NameHesab }).ToList();
                List<FinancialStatements> RightHandList = new List<FinancialStatements>();
                List<FinancialStatements> GheyrjariList = new List<FinancialStatements>();
                /////////////////////شروع محاسبه حساب‌های دارایی غیرجاری
                var grouhDaraeiGheyrJariNamesList = db.tblTabaghatHesabs.OrderByDescending(a => a.CodeZirtabagheTa).Where(a => a.CodeTabaghehHesab == 1 && a.CodeZirtabagheTa > 7999).Select(a => new { a.ZirTzbagheyehHesab }).ToList();

                //برای محاسبه مانده اشخاص بستانکار طوری که در صورت بستانکار بودن، در طرف چپ ظاهر شوند
                var hesabhaList = db.tblRooznamehs.Where(a => a.SanadTarikhMiladi <= tarikhMiladi).OrderBy(a => a.IdHesab).GroupBy(a => a.NameHesab).ToList();
                List<Reports> AllHesabhaList = new List<Reports>();
                foreach (var item in hesabhaList)
                {

                    Reports r = new Reports()
                    {
                        Tarikh = item.Select(a => a.SanadTarikhShamsi).Last(),
                        IdHesab = item.Select(a => a.IdHesab).First(),
                        NameHesab = item.Select(a => a.NameHesab).First(),
                        SubGroupHesab = item.Select(a => a.NameSumGroupHesab).First(),
                        TabagehHesab = item.Select(a => a.NameGroupHesab).First(),
                        BalanceTotalVolume = item.Select(a => a.BuySellVol).Sum(),
                        BedehkarTurnOver = item.Select(a => a.SanadBedehkar).Sum(),
                        BestankarTurnOver = item.Select(a => a.SabadBestankar).Sum(),
                        Balance = Convert.ToInt64(item.Select(a => a.SanadBedehkar).Sum() - item.Select(a => a.SabadBestankar).Sum()),
                    };
                    AllHesabhaList.Add(r);
                }


                ////////////////////////////اتمام محاسبات اشخاص خالص
                int i = 16001;
                int j = 16999;
                foreach (var item in grouhDaraeiGheyrJariNamesList)
                {
                    FinancialStatements t = new FinancialStatements()
                    {
                        NameGroupHesab = item.ZirTzbagheyehHesab,
                        Mandeh = grouhList.Where(a => a.IdHesab.ToString().Substring(0, 1) == "1" && Convert.ToInt32(a.IdHesab.ToString().Substring(3)) <= j && Convert.ToInt32(a.IdHesab.ToString().Substring(3)) >= i).Select(a => a.SanadBedehkar - a.SabadBestankar).Sum(),
                        IdHesab = grouhList.Where(a => a.IdHesab.ToString().Substring(0, 1) == "1" && Convert.ToInt32(a.IdHesab.ToString().Substring(3)) <= j && Convert.ToInt32(a.IdHesab.ToString().Substring(3)) >= i).Select(a => a.IdHesab).FirstOrDefault(),
                    };
                    RightHandList.Add(t);
                    GheyrjariList.Add(t);
                    if (i >= 8001 && j <= 8999)
                    {
                        FinancialStatements t2 = new FinancialStatements()
                        {
                            NameGroupHesab = "جمع دارایی‌های غیر جاری",
                            Mandeh = GheyrjariList.Select(a => a.Mandeh).Sum(),
                            IdHesab = 0,
                        };
                        RightHandList.Add(t2);
                    }
                    i = i - 1000;
                    j = j - 1000;
                }

                /////////////////////شروع محاسبه  حساب‌های دارایی جاری
                var grouhDaraeiJariNamesList = db.tblTabaghatHesabs.OrderByDescending(a => a.CodeZirtabagheTa).Where(a => a.CodeTabaghehHesab == 1 && a.CodeZirtabagheTa <= 7999).Select(a => new { a.ZirTzbagheyehHesab, a.CodeZirtabagheAz, a.CodeZirtabagheTa }).ToList();
                List<FinancialStatements> jariList = new List<FinancialStatements>();
                i = 7001;
                j = 7999;
                foreach (var item in grouhDaraeiJariNamesList)
                {
                    FinancialStatements t = new FinancialStatements();
                    if (i >= 3001 && j <= 3999)
                    {
                        t.NameGroupHesab = item.ZirTzbagheyehHesab;
                        t.Mandeh = (AllHesabhaList.Where(a => a.IdHesab.ToString().Substring(0, 1) == "1" && Convert.ToInt32(a.IdHesab.ToString().Substring(3)) <= j && Convert.ToInt32(a.IdHesab.ToString().Substring(3)) >= i).Where(a=>a.Balance>=0).Select(a => a.Balance).Sum())+ (AllHesabhaList.Where(a => a.IdHesab.ToString().Substring(0, 1) == "2" && Convert.ToInt32(a.IdHesab.ToString().Substring(3)) <= j-2000 && Convert.ToInt32(a.IdHesab.ToString().Substring(3)) >= i-2000).Where(a => a.Balance >= 0).Select(a => a.Balance).Sum());
                        t.IdHesab = AllHesabhaList.Where(a => a.IdHesab.ToString().Substring(0, 1) == "1" && Convert.ToInt32(a.IdHesab.ToString().Substring(3)) <= j && Convert.ToInt32(a.IdHesab.ToString().Substring(3)) >= i).Where(a => a.Balance >= 0).Select(a => a.IdHesab).FirstOrDefault();
                    }
                    else
                    {
                        t.NameGroupHesab = item.ZirTzbagheyehHesab;
                        t.Mandeh = AllHesabhaList.Where(a => a.IdHesab.ToString().Substring(0, 1) == "1" && Convert.ToInt32(a.IdHesab.ToString().Substring(3)) <= j && Convert.ToInt32(a.IdHesab.ToString().Substring(3)) >= i).Select(a => a.Balance).Sum();
                        t.IdHesab = AllHesabhaList.Where(a => a.IdHesab.ToString().Substring(0, 1) == "1" && Convert.ToInt32(a.IdHesab.ToString().Substring(3)) <= j && Convert.ToInt32(a.IdHesab.ToString().Substring(3)) >= i).Where(a => a.Balance >= 0).Select(a => a.IdHesab).FirstOrDefault();
                    }
                    RightHandList.Add(t);
                    jariList.Add(t);

                    if (i >= 1001 && j <= 1999)
                    {
                        FinancialStatements totalJari = new FinancialStatements()
                        {
                            NameGroupHesab = "جمع دارایی‌های جاری",
                            Mandeh = jariList.Select(a => a.Mandeh).Sum(),
                            IdHesab = 0,
                        };
                        RightHandList.Add(totalJari);
                        FinancialStatements TotalAsset = new FinancialStatements()
                        {
                            NameGroupHesab = "جمع دارایی‌ها",
                            Mandeh = GheyrjariList.Select(a => a.Mandeh).Sum()+ jariList.Select(a => a.Mandeh).Sum(),
                            IdHesab = 0,
                        };
                        RightHandList.Add(TotalAsset);
                    }
                    i = i - 1000;
                    j = j - 1000;
                }
                dgvRight.DataSource = RightHandList.Select(a => new { a.NameGroupHesab, a.Mandeh }).ToList();
                dgvRight.Columns[0].HeaderText = "شرح";
                dgvRight.Columns[1].HeaderText = "مانده حساب";
                dgvRight.Columns[1].DefaultCellStyle.Format = "N0";
                dgvRight.Columns[0].Width = 230;
                dgvRight.Columns[1].Width = 120;

                /////////////////////شروع محاسبه جمع حساب‌های حقوق مالکانه
                var grouhHoghughSahebanNames = db.tblTabaghatHesabs.OrderByDescending(a => a.CodeZirtabagheTa).Where(a => a.CodeTabaghehHesab == 3 && a.CodeZirtabagheTa <= 6999).Select(a => new { a.ZirTzbagheyehHesab, a.CodeZirtabagheAz, a.CodeZirtabagheTa }).ToList();
                List<FinancialStatements> HoghughSahebanList = new List<FinancialStatements>();
                i = 6001;
                j = 6999;
                foreach (var item in grouhHoghughSahebanNames)
                {
                    FinancialStatements t = new FinancialStatements();
                    t.NameGroupHesab = item.ZirTzbagheyehHesab;
                    if (item.CodeZirtabagheAz >= 3001 && item.CodeZirtabagheTa <= 3999)
                    {
                        var TaraznamehMandeh = grouhList.Where(a => a.IdHesab.ToString().Substring(0, 1) == "3" && Convert.ToInt32(a.IdHesab.ToString().Substring(3)) <= j && Convert.ToInt32(a.IdHesab.ToString().Substring(3)) >= i).Select(a => a.SabadBestankar - a.SanadBedehkar).Sum();
                        var DaramadhaMandeh = grouhList.Where(a => a.IdHesab.ToString().Substring(0, 1) == "4").Select(a => a.SabadBestankar - a.SanadBedehkar).Sum();
                        var HazihehaMandeh = grouhList.Where(a => a.IdHesab.ToString().Substring(0, 1) == "5").Select(a => a.SanadBedehkar - a.SabadBestankar).Sum();
                        t.Mandeh = TaraznamehMandeh + DaramadhaMandeh - HazihehaMandeh;
                        t.IdHesab = grouhList.Where(a => a.IdHesab.ToString().Substring(0, 1) == "5").Select(a => a.IdHesab).FirstOrDefault();
                    }
                    else
                    {
                        t.Mandeh = grouhList.Where(a => a.IdHesab.ToString().Substring(0, 1) == "3" && Convert.ToInt32(a.IdHesab.ToString().Substring(3)) <= j && Convert.ToInt32(a.IdHesab.ToString().Substring(3)) >= i).Select(a => a.SabadBestankar - a.SanadBedehkar).Sum();
                        t.IdHesab = grouhList.Where(a => a.IdHesab.ToString().Substring(0, 1) == "3" && Convert.ToInt32(a.IdHesab.ToString().Substring(3)) <= j && Convert.ToInt32(a.IdHesab.ToString().Substring(3)) >= i).Select(a => a.IdHesab).FirstOrDefault();
                    }
                    HoghughSahebanList.Add(t);
                    if (i >= 1001 && j <= 1999)
                    {
                        FinancialStatements TotalEquity = new FinancialStatements()
                        {
                            NameGroupHesab = "جمع حقوق مالکانه",
                            Mandeh = HoghughSahebanList.Sum(a => a.Mandeh),
                            IdHesab = 0,
                        };
                        HoghughSahebanList.Add(TotalEquity);
                    }
                    i = i - 1000;
                    j = j - 1000;
                }
                //اتمام حقوق مالکانه

                List<FinancialStatements> LeftHandList = new List<FinancialStatements>();
                List<FinancialStatements> BedehiGheryList = new List<FinancialStatements>();
                
                LeftHandList.AddRange(HoghughSahebanList);
                /////////////////////شروع محاسبه جمع حساب‌های بدهی غیرجاری
                var grouhBedehiGheyrJariNamesList = db.tblTabaghatHesabs.OrderByDescending(a => a.CodeZirtabagheTa).Where(a => a.CodeTabaghehHesab == 2 && a.CodeZirtabagheTa > 3999).Select(a => new { a.ZirTzbagheyehHesab }).ToList();
                i = 7001;
                j = 7999;
                foreach (var item in grouhBedehiGheyrJariNamesList)
                {
                    FinancialStatements t = new FinancialStatements()
                    {
                        NameGroupHesab = item.ZirTzbagheyehHesab,
                        Mandeh = -AllHesabhaList.Where(a => a.IdHesab.ToString().Substring(0, 1) == "2" && Convert.ToInt32(a.IdHesab.ToString().Substring(3)) <= j && Convert.ToInt32(a.IdHesab.ToString().Substring(3)) >= i).Select(a => a.Balance).Sum(),
                        IdHesab = AllHesabhaList.Where(a => a.IdHesab.ToString().Substring(0, 1) == "2" && Convert.ToInt32(a.IdHesab.ToString().Substring(3)) <= j && Convert.ToInt32(a.IdHesab.ToString().Substring(3)) >= i).Select(a => a.IdHesab).FirstOrDefault()
                    };
                    LeftHandList.Add(t);
                    BedehiGheryList.Add(t);
                    if (i >= 4001 && j <= 4999)
                    {
                        FinancialStatements t2 = new FinancialStatements()
                        {
                            NameGroupHesab = "جمع بدهی‌های غیرجاری",
                            Mandeh = BedehiGheryList.Select(a => a.Mandeh).Sum(),
                            IdHesab = 0,
                        };
                        LeftHandList.Add(t2);
                        BedehiGheryList.Add(t2);
                    }
                    i = i - 1000;
                    j = j - 1000;
                }
                //اتمام بدهی غیرجاری

                ////////////////////شروع بدهی جاری  
                ///
                List<FinancialStatements> BedehiJariList = new List<FinancialStatements>();
                var grouhBedehiJariNamesList = db.tblTabaghatHesabs.OrderByDescending(a => a.CodeZirtabagheTa).Where(a => a.CodeTabaghehHesab == 2 && a.CodeZirtabagheTa <= 3999).Select(a => new { a.ZirTzbagheyehHesab, a.CodeZirtabagheAz, a.CodeZirtabagheTa }).ToList();
                i = 3001;
                j = 3999;
                foreach (var item in grouhBedehiJariNamesList)
                {
                    if (i >= 1001 && j <= 1999)
                    {
                        //برای اینکه گروپ بای کردن، یک نام نگه می دارد و آیدی حساب می تواند چند شماره باشد
                        FinancialStatements t = new FinancialStatements();
                        t.NameGroupHesab = item.ZirTzbagheyehHesab;
                        t.Mandeh = (-AllHesabhaList.Where(a => a.IdHesab.ToString().Substring(0, 1) == "1" && Convert.ToInt32(a.IdHesab.ToString().Substring(3)) <= j + 2000 && Convert.ToInt32(a.IdHesab.ToString().Substring(3)) >= i + 2000).Where(a => a.Balance < 0).Select(a => a.Balance).Sum()) + (-AllHesabhaList.Where(a => a.IdHesab.ToString().Substring(0, 1) == "2" && Convert.ToInt32(a.IdHesab.ToString().Substring(3)) <= j && Convert.ToInt32(a.IdHesab.ToString().Substring(3)) >= i).Where(a => a.Balance < 0).Select(a => a.Balance).Sum());
                        t.IdHesab = 1001001;
                        BedehiJariList.Add(t);
                    }
                    else
                    {
                        FinancialStatements t = new FinancialStatements()
                        {
                            NameGroupHesab = item.ZirTzbagheyehHesab,
                            Mandeh = -AllHesabhaList.Where(a => a.IdHesab.ToString().Substring(0, 1) == "2" && Convert.ToInt32(a.IdHesab.ToString().Substring(3)) <= j && Convert.ToInt32(a.IdHesab.ToString().Substring(3)) >= i).Select(a => a.Balance).Sum(),
                            IdHesab = AllHesabhaList.Where(a => a.IdHesab.ToString().Substring(0, 1) == "2" && Convert.ToInt32(a.IdHesab.ToString().Substring(3)) <= j && Convert.ToInt32(a.IdHesab.ToString().Substring(3)) >= i).Select(a => a.IdHesab).FirstOrDefault()
                        };
                        BedehiJariList.Add(t);
                    }
                    if (i >= 1001 && j <= 1999)
                    {
                        FinancialStatements t2 = new FinancialStatements()
                        {
                            NameGroupHesab = "جمع بدهی‌های جاری",
                            Mandeh = BedehiJariList.Select(a => a.Mandeh).Sum(),
                            IdHesab = 0,
                        };
                        BedehiJariList.Add(t2);
                    }
                    i = i - 1000;
                    j = j - 1000;
                }
                LeftHandList.AddRange(BedehiJariList);
                ///////////////////////اتمام بدهی جاری

                for (int k = 0; k <= 2; k++)
                {
                    if (k == 0)
                    {
                        FinancialStatements khali = new FinancialStatements();
                        LeftHandList.Add(khali);

                    }
                    if (k ==1)
                    {
                        FinancialStatements TotalDebt = new FinancialStatements()
                        {
                            NameGroupHesab = "جمع بدهی‌ها",
                            Mandeh = BedehiJariList.Select(a=>a.Mandeh).Last()+ BedehiGheryList.Select(a => a.Mandeh).Last(),
                            IdHesab = 0,
                        };
                        LeftHandList.Add(TotalDebt);
                    }

                    if (k == 2)
                    {
                        FinancialStatements TotalDebtAndEquity = new FinancialStatements()
                        {
                            NameGroupHesab = "جمع بدهی‌ها و حقوق مالکانه",
                            Mandeh = LeftHandList.Where(a=>a.IdHesab!=0).Select(a => a.Mandeh).Sum(),
                            IdHesab = 0,
                        };
                        LeftHandList.Add(TotalDebtAndEquity);
                    }
                }
                dgvLeft.DataSource = LeftHandList.Select(a => new { a.NameGroupHesab, a.Mandeh }).ToList();
                dgvLeft.Columns[0].HeaderText = "شرح";
                dgvLeft.Columns[1].HeaderText = "مانده حساب";
                dgvLeft.Columns[1].DefaultCellStyle.Format = "N0";
                dgvLeft.Columns[0].Width = 230;
                dgvLeft.Columns[1].Width = 120;

            }
            catch (Exception ex)
            {
                MessageBoxFarsi.Show(ex.Message.ToString(), "خطاء در نمایش ترازنامه");
            }
        }

        private void btnExcel_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvRight.DataSource == null || dgvLeft.DataSource == null)
                {
                    MessageBoxFarsi.Show("ابتدا دکمه نمایش را کلیک کنید تا اطلاعات ترازنامه بارگزاری شود");
                    return;
                }

                Microsoft.Office.Interop.Excel.Application app = new Microsoft.Office.Interop.Excel.Application();
                Microsoft.Office.Interop.Excel.Workbook workbook = app.Workbooks.Add(Type.Missing);
                Microsoft.Office.Interop.Excel.Worksheet worksheet = null;
                worksheet = workbook.Sheets["Sheet1"];
                worksheet = workbook.ActiveSheet;
                worksheet.Name = "صورت وضعیت مالی (ترازنامه)";
                worksheet.DisplayRightToLeft = true;
                worksheet.Cells[1, 2] = $"صورت وضعیت مالی (ترازنامه) منتهی به: {mskTarikh.Text}";
                worksheet.Range[worksheet.Cells[1, 2], worksheet.Cells[1, 5]].Merge();
                worksheet.Rows[1].HorizontalAlignment = HorizontalAlignment.Right;
                worksheet.Rows[1].Font.Bold = true;
                worksheet.Cells[2, 2] = "شرح";
                worksheet.Cells[2, 3] = "مانده حساب";
                worksheet.Cells[2, 4] = "شرح";
                worksheet.Cells[2, 5] = "مانده حساب";
                worksheet.Rows[2].Font.Bold = true;
                for (int j = 0; j < dgvRight.Rows.Count; j++)
                {
                    worksheet.Cells[j + 3, 2] = dgvRight.Rows[j].Cells[0].Value;
                    worksheet.Cells[j + 3, 3] = dgvRight.Rows[j].Cells[1].Value;
                    worksheet.Cells[j + 3, 3].NumberFormat = "#,##0";
                    worksheet.Cells[j + 3, 4] = dgvLeft.Rows[j].Cells[0].Value;
                    worksheet.Cells[j + 3, 5] = dgvLeft.Rows[j].Cells[1].Value;
                    worksheet.Cells[j + 3, 5].NumberFormat = "#,##0";
                }
                worksheet.Rows[dgvRight.Rows.Count + 2].Font.Bold = true;
                worksheet.Cells[12, 2].Font.Bold = true;
                worksheet.Cells[12, 3].Font.Bold = true;
                worksheet.Cells[20, 2].Font.Bold = true;
                worksheet.Cells[20, 3].Font.Bold = true;
                worksheet.Cells[7, 4].Font.Bold = true;
                worksheet.Cells[7, 5].Font.Bold = true;
                worksheet.Cells[11, 4].Font.Bold = true;
                worksheet.Cells[11, 5].Font.Bold = true;
                worksheet.Cells[18, 4].Font.Bold = true;
                worksheet.Cells[18, 5].Font.Bold = true;
                worksheet.Cells[20, 4].Font.Bold = true;
                worksheet.Cells[20, 5].Font.Bold = true;
                worksheet.Cells[21, 3].Font.Underline = 5;//xlUnderlineStyleDoubleAccounting
                worksheet.Cells[21, 5].Font.Underline = 5; //xlUnderlineStyleDoubleAccounting
                Microsoft.Office.Interop.Excel.Range usedRange = worksheet.UsedRange;
                usedRange.Columns.AutoFit();
                usedRange.Columns.Borders.LineStyle = BorderStyle.FixedSingle;
                usedRange.Rows.AutoFit();
                var sfd = new SaveFileDialog();
                sfd.FileName = "صورت وضعیت مالی (ترازنامه)";
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

        private void tableLayoutPanel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void tableLayoutPanel4_Paint(object sender, PaintEventArgs e)
        {

        }

        private void dgvLeft_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dgvRight_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void mskTarikh_MaskInputRejected(object sender, MaskInputRejectedEventArgs e)
        {

        }
    }
}
