using Accord.IO;
using BehComponents;
using HooshBartarAccountingApp.DatabaseModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HooshBartarAccountingApp.DataShow
{
    class ExcelReports
    {
        public List<tblRooznameh> RawRooznamehTable { get; set; }
        public List<tblTabaghatHesab> RawGrouhTable { get; set; }
        public string[] NamesOfColumns { get; set; }
        public string StartDate { get; set; }
        public DateTime MiladiStartDate { get; set; }
        public DateTime MiladiEndDate { get; set; }
        public string EndDate { get; set; }
        public string HesabName { get; set; }
        public string GrouhName { get; set; }
        public string TarikhFactor { get; set; }

        public DataTable SourceData { get; set; }
        public List<Sanad> MyBuyList { get; set; }
        public List<Sanad> MySellList { get; set; }

        public void BalancSheet(List<tblRooznameh> rawRooznamehTbl, List<tblTabaghatHesab> rawTabaghatTbl, string tarikhFactor)
        {
            RawRooznamehTable = rawRooznamehTbl;
            RawGrouhTable = rawTabaghatTbl;
            TarikhFactor = tarikhFactor;
            Microsoft.Office.Interop.Excel.Application app = new Microsoft.Office.Interop.Excel.Application();
            Microsoft.Office.Interop.Excel.Workbook workbook = app.Workbooks.Add(Type.Missing);
            Microsoft.Office.Interop.Excel.Worksheet worksheet = null;
            worksheet = workbook.Sheets["Sheet1"];
            worksheet = workbook.ActiveSheet;
            worksheet.Name = "صورت وضعیت مالی (ترازنامه)";
            worksheet.DisplayRightToLeft = true;
            worksheet.Cells[1, 2] = $"صورت وضعیت مالی (ترازنامه) منتهی به: {TarikhFactor}";
            worksheet.Range[worksheet.Cells[1, 2], worksheet.Cells[1, 7]].Merge();
            worksheet.Rows[1].HorizontalAlignment = HorizontalAlignment.Right;
            worksheet.Rows[1].Font.Bold = true;
            worksheet.Cells[2, 2] = "شرح";
            worksheet.Cells[2, 3] = "مانده حساب";

            int tedadSatrDaraeiGheyrJari = RawGrouhTable.Where(a => a.CodeTabaghehHesab == 1 && a.CodeZirtabagheTa <= 16999 && a.CodeZirtabagheAz >= 8001).Count();

            int tedadSatrDaraeiJari = RawGrouhTable.Where(a => a.CodeTabaghehHesab == 1 && a.CodeZirtabagheTa <= 7999 && a.CodeZirtabagheAz >= 1001).Count();
            for (int j = 0; j < tedadSatrDaraeiGheyrJari; j++)
            {
                string SubGrouh = RawGrouhTable.Where(a => a.CodeTabaghehHesab == 1 && a.CodeZirtabagheTa <= 16999 && a.CodeZirtabagheAz >= 8001).Select(a => a.ZirTzbagheyehHesab).ElementAt(j);
                worksheet.Cells[j + 3, 2] = SubGrouh;
                worksheet.Cells[j + 3, 3] = RawRooznamehTable.Where(a => a.NameSumGroupHesab == SubGrouh).Select(a => a.SanadBedehkar - a.SabadBestankar).Sum();
                if (j + 1 == tedadSatrDaraeiGheyrJari)
                {
                    worksheet.Cells[j + 4, 2] = "جمع دارایی‌های غیرجاری";
                    worksheet.Cells[j + 4, 2].Font.Bold = true;
                    worksheet.Cells[j + 4, 3] = RawRooznamehTable.Where(a => a.IdHesab.ToString().Substring(0, 1) == "1" && a.IdHesab <= 10016999 && a.IdHesab >= 1008001).Select(a => a.SanadBedehkar - a.SabadBestankar).Sum();
                    worksheet.Cells[j + 4, 3].Font.Bold = true;
                }
            }
            for (int j = 0; j < tedadSatrDaraeiJari; j++)
            {
                string SubGrouh = RawGrouhTable.Where(a => a.CodeTabaghehHesab == 1 && a.CodeZirtabagheTa <= 7999 && a.CodeZirtabagheAz >= 1001).Select(a => a.ZirTzbagheyehHesab).ElementAt(j);
                worksheet.Cells[j + tedadSatrDaraeiGheyrJari + 4, 2] = SubGrouh;
                long? Mandeh = RawRooznamehTable.Where(a => a.NameSumGroupHesab == SubGrouh).Select(a => a.SanadBedehkar - a.SabadBestankar).Sum();
                worksheet.Cells[j + tedadSatrDaraeiGheyrJari + 4, 3] = Mandeh;
                if (j + 1 == tedadSatrDaraeiJari)
                {
                    worksheet.Cells[j + tedadSatrDaraeiGheyrJari + 4, 2] = "جمع دارایی‌های جاری";
                    worksheet.Cells[j + tedadSatrDaraeiGheyrJari + 4, 2].Font.Bold = true;
                    worksheet.Cells[j + tedadSatrDaraeiGheyrJari + 4, 3] = RawRooznamehTable.Where(a => a.IdHesab.ToString().Substring(0, 1) == "1" && a.IdHesab <= 1007999 && a.IdHesab >= 1001001).Select(a => a.SanadBedehkar - a.SabadBestankar).Sum();
                    worksheet.Cells[j + tedadSatrDaraeiGheyrJari + 4, 3].Font.Bold = true;
                    worksheet.Cells[j + tedadSatrDaraeiGheyrJari + 5, 2] = "جمع دارایی‌ها";
                    worksheet.Cells[j + tedadSatrDaraeiGheyrJari + 5, 2].Font.Bold = true;
                    worksheet.Cells[j + tedadSatrDaraeiGheyrJari + 5, 3] = RawRooznamehTable.Where(a => a.IdHesab.ToString().Substring(0, 1) == "1" && a.IdHesab <= 10016999 && a.IdHesab >= 1001001).Select(a => a.SanadBedehkar - a.SabadBestankar).Sum();
                    worksheet.Cells[j + tedadSatrDaraeiGheyrJari + 5, 3].Font.Bold = true;
                }
            }

            Microsoft.Office.Interop.Excel.Range usedRange = worksheet.UsedRange;
            usedRange.Columns.AutoFit();
            usedRange.Columns.Borders.LineStyle = BorderStyle.FixedSingle;
            usedRange.Columns[3].NumberFormat = "#,##0";
            usedRange.Columns[5].NumberFormat = "#,##0";

            usedRange.Rows.AutoFit();
            var sfd = new SaveFileDialog();
            sfd.FileName = "ترازنامه";
            sfd.DefaultExt = ".Xlsx";
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                workbook.SaveAs(sfd.FileName, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Microsoft.Office.Interop.Excel.XlSaveAsAccessMode.xlExclusive, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);
            }
            app.Quit();



        }
        public void FactorReport(List<tblRooznameh> rawTbl, string[] ColumnsName, string tarikhFactor, string nameHesab)
        {
            RawRooznamehTable = rawTbl;
            NamesOfColumns = ColumnsName;
            TarikhFactor = tarikhFactor;
            HesabName = nameHesab;
            Microsoft.Office.Interop.Excel.Application app = new Microsoft.Office.Interop.Excel.Application();
            Microsoft.Office.Interop.Excel.Workbook workbook = app.Workbooks.Add(Type.Missing);
            Microsoft.Office.Interop.Excel.Worksheet worksheet = null;
            worksheet = workbook.Sheets["Sheet1"];
            worksheet = workbook.ActiveSheet;
            worksheet.Name = "Factor";
            worksheet.DisplayRightToLeft = true;
            if (RawRooznamehTable.Count == 1)
            {
                if (RawRooznamehTable.Select(a => a.SanadBedehkar).Sum() >= RawRooznamehTable.Select(a => a.SabadBestankar).Sum())
                {
                    worksheet.Cells[1, 1] = $"فاکتور خرید: {HesabName}";
                }
                if (RawRooznamehTable.Select(a => a.SanadBedehkar).Sum() < RawRooznamehTable.Select(a => a.SabadBestankar).Sum())
                {
                    worksheet.Cells[1, 1] = $"فاکتور فروش: {HesabName}";
                }
            }
            else if (RawRooznamehTable.Count > 1)
            {
                worksheet.Cells[1, 1] = $"فاکتورهای خرید و فروش:  {HesabName}";
            }

            worksheet.Range[worksheet.Cells[1, 1], worksheet.Cells[1, 3]].Merge();
            worksheet.Cells[1, 9] = "تاریخ فاکتور:";
            worksheet.Cells[1, 10] = TarikhFactor.ToString();
            worksheet.Rows[1].HorizontalAlignment = HorizontalAlignment.Right;
            worksheet.Rows[1].Font.Bold = true;
            worksheet.Columns[1].Font.Bold = true;
            for (int i = 0; i < NamesOfColumns.Count(); i++)
            {
                worksheet.Cells[2, i + 1] = NamesOfColumns[i];
            }
            int tedadaSatrha = RawRooznamehTable.Count;

            for (int i = 1; i <= NamesOfColumns.Count(); i++)
            {
                for (int j = 1; j <= tedadaSatrha + 3; j++)
                {
                    worksheet.Cells[j, i].Borders.LineStyle = BorderStyle.FixedSingle;
                }
            }

            for (int j = 0; j < tedadaSatrha; j++)
            {
                int i = 1;
                worksheet.Cells[j + 3, i++] = RawRooznamehTable.Select(a => a.SanadId).ElementAt(j);
                worksheet.Cells[j + 3, i++] = RawRooznamehTable.Select(a => a.SanadTarikhShamsi).ElementAt(j);
                worksheet.Cells[j + 3, i++] = RawRooznamehTable.Select(a => a.NameHesab).ElementAt(j);
                worksheet.Cells[j + 3, i++] = RawRooznamehTable.Select(a => a.SanadTozih).ElementAt(j);
                worksheet.Cells[j + 3, i++] = RawRooznamehTable.Select(a => a.BuySellVol).ElementAt(j);
                worksheet.Cells[j + 3, i - 1].NumberFormat = "#,##0";
                worksheet.Cells[j + 3, i++] = RawRooznamehTable.Select(a => a.EnteghaTax).ElementAt(j);
                worksheet.Cells[j + 3, i - 1].NumberFormat = "#,##0";
                worksheet.Cells[j + 3, i++] = RawRooznamehTable.Select(a => a.ArzeshAfzudehTax).ElementAt(j);
                worksheet.Cells[j + 3, i - 1].NumberFormat = "#,##0";
                worksheet.Cells[j + 3, i++] = RawRooznamehTable.Select(a => a.KarmozdMoameleh).ElementAt(j);
                worksheet.Cells[j + 3, i - 1].NumberFormat = "#,##0";
                if (RawRooznamehTable.Select(a => a.SanadBedehkar).ElementAt(j) >= RawRooznamehTable.Select(a => a.SabadBestankar).ElementAt(j))
                {
                    worksheet.Cells[j + 3, i++] = RawRooznamehTable.Select(a => a.SanadBedehkar + a.SabadBestankar - a.ArzeshAfzudehTax - a.KarmozdMoameleh).ElementAt(j);
                }
                if (RawRooznamehTable.Select(a => a.SanadBedehkar).ElementAt(j) < RawRooznamehTable.Select(a => a.SabadBestankar).ElementAt(j))
                {
                    worksheet.Cells[j + 3, i++] = RawRooznamehTable.Select(a => a.SabadBestankar + a.SanadBedehkar - a.ArzeshAfzudehTax - a.KarmozdMoameleh).ElementAt(j);
                }
                worksheet.Cells[j + 3, i - 1].NumberFormat = "#,##0";
                worksheet.Cells[j + 3, i++] = RawRooznamehTable.Select(a => a.SanadBedehkar + a.SabadBestankar).ElementAt(j);
                worksheet.Cells[j + 3, i - 1].NumberFormat = "#,##0";
                if (tedadaSatrha - j == 1)
                {
                    worksheet.Cells[j + 4, 4] = "جمع";
                    worksheet.Cells[j + 4, 4].Font.Bold = true;
                    worksheet.Cells[j + 4, 5] = RawRooznamehTable.Select(a => a.BuySellVol).Sum();
                    worksheet.Cells[j + 4, 5].NumberFormat = "#,##0";
                    worksheet.Cells[j + 4, 5].Font.Bold = true;
                    worksheet.Cells[j + 4, 6] = RawRooznamehTable.Select(a => a.EnteghaTax).Sum();
                    worksheet.Cells[j + 4, 6].NumberFormat = "#,##0";
                    worksheet.Cells[j + 4, 6].Font.Bold = true;
                    worksheet.Cells[j + 4, 7] = RawRooznamehTable.Select(a => a.ArzeshAfzudehTax).Sum();
                    worksheet.Cells[j + 4, 7].NumberFormat = "#,##0";
                    worksheet.Cells[j + 4, 7].Font.Bold = true;
                    worksheet.Cells[j + 4, 8] = RawRooznamehTable.Select(a => a.KarmozdMoameleh).Sum();
                    worksheet.Cells[j + 4, 8].NumberFormat = "#,##0";
                    worksheet.Cells[j + 4, 8].Font.Bold = true;
                    worksheet.Cells[j + 4, 9] = RawRooznamehTable.Select(a => a.SanadBedehkar + a.SabadBestankar - a.ArzeshAfzudehTax - a.KarmozdMoameleh).Sum();
                    worksheet.Cells[j + 4, 9].NumberFormat = "#,##0";
                    worksheet.Cells[j + 4, 9].Font.Bold = true;
                    worksheet.Cells[j + 4, 10] = RawRooznamehTable.Select(a => a.SanadBedehkar + a.SabadBestankar).Sum();
                    worksheet.Cells[j + 4, 10].NumberFormat = "#,##0";
                    worksheet.Cells[j + 4, 10].Font.Bold = true;
                }
            }
            Microsoft.Office.Interop.Excel.Range usedRange = worksheet.UsedRange;
            usedRange.Columns.AutoFit();
            usedRange.Rows.AutoFit();
            var sfd = new SaveFileDialog();
            sfd.FileName = HesabName;
            sfd.DefaultExt = ".Xlsx";
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                workbook.SaveAs(sfd.FileName, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Microsoft.Office.Interop.Excel.XlSaveAsAccessMode.xlExclusive, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);
            }
            app.Quit();
        }

        public void RooznamehReport(List<tblRooznameh> rawTbl, string[] ColumnsName, string az, string ta)
        {
            RawRooznamehTable = rawTbl;
            NamesOfColumns = ColumnsName;
            StartDate = az;
            EndDate = ta;
            Microsoft.Office.Interop.Excel.Application app = new Microsoft.Office.Interop.Excel.Application();
            Microsoft.Office.Interop.Excel.Workbook workbook = app.Workbooks.Add(Type.Missing);
            Microsoft.Office.Interop.Excel.Worksheet worksheet = null;
            worksheet = workbook.Sheets["Sheet1"];
            worksheet = workbook.ActiveSheet;
            worksheet.Name = "Report";
            worksheet.DisplayRightToLeft = true;
            worksheet.Cells[1, 1] = "دفتر روزنامه ...........";
            worksheet.Range[worksheet.Cells[1, 1], worksheet.Cells[1, 3]].Merge();
            worksheet.Cells[1, 4] = "از تاریخ:";
            worksheet.Cells[1, 5] = StartDate.ToString();
            worksheet.Cells[1, 6] = "تا تاریخ:";
            worksheet.Cells[1, 7] = EndDate.ToString();
            worksheet.Rows[1].HorizontalAlignment = HorizontalAlignment.Right;
            worksheet.Rows[1].Font.Bold = true;
            worksheet.Columns[1].Font.Bold = true;
            for (int i = 0; i < NamesOfColumns.Count(); i++)
            {
                worksheet.Cells[2, i + 1] = NamesOfColumns[i];
            }
            int tedadaSatrha = RawRooznamehTable.Count;

            for (int i = 1; i <= NamesOfColumns.Count(); i++)
            {
                for (int j = 1; j <= tedadaSatrha + 3; j++)
                {
                    worksheet.Cells[j, i].Borders.LineStyle = BorderStyle.FixedSingle;
                }
            }

            for (int j = 0; j < tedadaSatrha; j++)
            {
                int i = 1;
                worksheet.Cells[j + 3, i++] = RawRooznamehTable.Select(a => a.SanadId).ElementAt(j);
                worksheet.Cells[j + 3, i++] = RawRooznamehTable.Select(a => a.SanadTarikhShamsi).ElementAt(j);
                worksheet.Cells[j + 3, i++] = RawRooznamehTable.Select(a => a.IdHesab).ElementAt(j);
                worksheet.Cells[j + 3, i++] = RawRooznamehTable.Select(a => a.NameHesab).ElementAt(j);
                worksheet.Cells[j + 3, i++] = RawRooznamehTable.Select(a => a.SanadTozih).ElementAt(j);
                worksheet.Cells[j + 3, i++] = RawRooznamehTable.Select(a => a.SanadBedehkar).ElementAt(j);
                worksheet.Cells[j + 3, i - 1].NumberFormat = "#,##0";
                worksheet.Cells[j + 3, i++] = RawRooznamehTable.Select(a => a.SabadBestankar).ElementAt(j);
                worksheet.Cells[j + 3, i - 1].NumberFormat = "#,##0";
                if (RawRooznamehTable.Select(a => a.SanadBedehkar).ElementAt(j) > RawRooznamehTable.Select(a => a.SabadBestankar).ElementAt(j))
                {
                    worksheet.Cells[j + 3, i++] = "بدهکار";
                }
                else if (RawRooznamehTable.Select(a => a.SanadBedehkar).ElementAt(j) < RawRooznamehTable.Select(a => a.SabadBestankar).ElementAt(j))
                {
                    worksheet.Cells[j + 3, i++] = "بستانکار";
                }
                else
                {
                    worksheet.Cells[j + 3, i++] = "-";
                }
                if (tedadaSatrha - j == 1)
                {
                    worksheet.Cells[j + 4, 5] = "جمع گردش";
                    worksheet.Cells[j + 4, 5].Font.Bold = true;
                    worksheet.Cells[j + 4, 6] = RawRooznamehTable.Select(a => a.SanadBedehkar).Sum();
                    worksheet.Cells[j + 4, 6].NumberFormat = "#,##0";
                    worksheet.Cells[j + 4, 6].Font.Bold = true;
                    worksheet.Cells[j + 4, 7] = RawRooznamehTable.Select(a => a.SabadBestankar).Sum();
                    worksheet.Cells[j + 4, 7].NumberFormat = "#,##0";
                    worksheet.Cells[j + 4, 7].Font.Bold = true;
                }
            }
            Microsoft.Office.Interop.Excel.Range usedRange = worksheet.UsedRange;
            usedRange.Columns.AutoFit();
            usedRange.Rows.AutoFit();

            var sfd = new SaveFileDialog();
            sfd.FileName = RawRooznamehTable.Select(a => a.SanadId).FirstOrDefault().ToString("#,##0");
            sfd.DefaultExt = ".Xlsx";
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                workbook.SaveAs(sfd.FileName, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Microsoft.Office.Interop.Excel.XlSaveAsAccessMode.xlExclusive, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);
            }
            app.Quit();
        }

        public void MoeinReport(List<tblRooznameh> rawTbl, string[] ColumnsName, string azShamsi, string taShamsi, DateTime azMiladi, DateTime taMiladi, string hesabName)
        {
            RawRooznamehTable = rawTbl;
            NamesOfColumns = ColumnsName;
            HesabName = hesabName;
            StartDate = azShamsi;
            EndDate = taShamsi;
            MiladiStartDate = azMiladi;
            MiladiEndDate = taMiladi;
            Microsoft.Office.Interop.Excel.Application app = new Microsoft.Office.Interop.Excel.Application();
            Microsoft.Office.Interop.Excel.Workbook workbook = app.Workbooks.Add(Type.Missing);
            Microsoft.Office.Interop.Excel.Worksheet worksheet = null;
            worksheet = workbook.Sheets["Sheet1"];
            worksheet = workbook.ActiveSheet;
            worksheet.Name = "Report";
            worksheet.DisplayRightToLeft = true;
            worksheet.Cells[1, 1] = $"دفتر معین: {HesabName}";
            worksheet.Range[worksheet.Cells[1, 1], worksheet.Cells[1, 3]].Merge();
            worksheet.Cells[1, 4] = "از تاریخ:";
            worksheet.Cells[1, 5] = StartDate.ToString();
            worksheet.Cells[1, 6] = "تا تاریخ:";
            worksheet.Cells[1, 7] = EndDate.ToString();
            worksheet.Rows[1].HorizontalAlignment = HorizontalAlignment.Right;
            worksheet.Rows[1].Font.Bold = true;
            worksheet.Columns[1].Font.Bold = true;
            for (int i = 0; i < NamesOfColumns.Count(); i++)
            {
                worksheet.Cells[2, i + 1] = NamesOfColumns[i];
            }
            int tedadaSatrha = RawRooznamehTable.Count;

            for (int i = 1; i <= NamesOfColumns.Count(); i++)
            {
                for (int j = 1; j <= tedadaSatrha + 3; j++)
                {
                    worksheet.Cells[j, i].Borders.LineStyle = BorderStyle.FixedSingle;
                }
            }

            for (int j = 0; j < tedadaSatrha; j++)
            {
                int i = 1;
                worksheet.Cells[j + 3, i++] = RawRooznamehTable.Select(a => a.SanadId).ElementAt(j);
                worksheet.Cells[j + 3, i++] = RawRooznamehTable.Select(a => a.SanadTarikhShamsi).ElementAt(j);
                worksheet.Cells[j + 3, i++] = RawRooznamehTable.Select(a => a.IdHesab).ElementAt(j);
                worksheet.Cells[j + 3, i++] = RawRooznamehTable.Select(a => a.NameHesab).ElementAt(j);
                worksheet.Cells[j + 3, i++] = RawRooznamehTable.Select(a => a.SanadTozih).ElementAt(j);
                worksheet.Cells[j + 3, i++] = RawRooznamehTable.Select(a => a.SanadBedehkar).ElementAt(j);
                worksheet.Cells[j + 3, i - 1].NumberFormat = "#,##0";
                worksheet.Cells[j + 3, i++] = RawRooznamehTable.Select(a => a.SabadBestankar).ElementAt(j);
                worksheet.Cells[j + 3, i - 1].NumberFormat = "#,##0";
                worksheet.Cells[j + 3, i++] = Math.Abs((long)(RawRooznamehTable.Where(a => a.SanadId <= RawRooznamehTable.Select(b => b.SanadId).ElementAt(j)).Select(a => a.SanadBedehkar - a.SabadBestankar).Sum()));
                worksheet.Cells[j + 3, i - 1].NumberFormat = "#,##0";

                if (RawRooznamehTable.Select(a => a.SanadBedehkar).ElementAt(j) > RawRooznamehTable.Select(a => a.SabadBestankar).ElementAt(j))
                {
                    worksheet.Cells[j + 3, i++] = "بدهکار";
                }
                else if (RawRooznamehTable.Select(a => a.SanadBedehkar).ElementAt(j) < RawRooznamehTable.Select(a => a.SabadBestankar).ElementAt(j))
                {
                    worksheet.Cells[j + 3, i++] = "بستانکار";
                }
                else
                {
                    worksheet.Cells[j + 3, i++] = "-";
                }
                if (tedadaSatrha - j == 1)
                {
                    worksheet.Cells[j + 4, 5] = "جمع گردش";
                    worksheet.Cells[j + 4, 5].Font.Bold = true;
                    worksheet.Cells[j + 4, 6] = RawRooznamehTable.Select(a => a.SanadBedehkar).Sum();
                    worksheet.Cells[j + 4, 6].NumberFormat = "#,##0";
                    worksheet.Cells[j + 4, 6].Font.Bold = true;
                    worksheet.Cells[j + 4, 7] = RawRooznamehTable.Select(a => a.SabadBestankar).Sum();
                    worksheet.Cells[j + 4, 7].NumberFormat = "#,##0";
                    worksheet.Cells[j + 4, 7].Font.Bold = true;
                    worksheet.Cells[j + 4, 8] = Math.Abs((long)(RawRooznamehTable.Select(a => a.SanadBedehkar - a.SabadBestankar).Sum()));
                    worksheet.Cells[j + 4, 8].NumberFormat = "#,##0";
                    worksheet.Cells[j + 4, 8].Font.Bold = true;
                    if (RawRooznamehTable.Select(a => a.SanadBedehkar - a.SabadBestankar).Sum() > 0)
                    {
                        worksheet.Cells[j + 4, 9] = "بدهکار";
                    }
                    else if (RawRooznamehTable.Select(a => a.SanadBedehkar - a.SabadBestankar).Sum() < 0)
                    {
                        worksheet.Cells[j + 4, 9] = "بستانکار";
                    }
                    else
                    {
                        worksheet.Cells[j + 4, 9] = "-";
                    }
                }
            }
            Microsoft.Office.Interop.Excel.Range usedRange = worksheet.UsedRange;
            usedRange.Columns.AutoFit();
            usedRange.Rows.AutoFit();

            var sfd = new SaveFileDialog();
            sfd.FileName = HesabName;
            sfd.DefaultExt = ".Xlsx";
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                workbook.SaveAs(sfd.FileName, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Microsoft.Office.Interop.Excel.XlSaveAsAccessMode.xlExclusive, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);
            }
            app.Quit();
        }

        public void KolReport(List<tblRooznameh> rawTbl, string[] ColumnsName, string azShamsi, string taShamsi, DateTime azMiladi, DateTime taMiladi, string grouhName)
        {
            RawRooznamehTable = rawTbl;
            NamesOfColumns = ColumnsName;
            GrouhName = grouhName;
            StartDate = azShamsi;
            EndDate = taShamsi;
            MiladiStartDate = azMiladi;
            MiladiEndDate = taMiladi;
            Microsoft.Office.Interop.Excel.Application app = new Microsoft.Office.Interop.Excel.Application();
            Microsoft.Office.Interop.Excel.Workbook workbook = app.Workbooks.Add(Type.Missing);
            Microsoft.Office.Interop.Excel.Worksheet worksheet = null;
            worksheet = workbook.Sheets["Sheet1"];
            worksheet = workbook.ActiveSheet;
            worksheet.Name = "Report";
            worksheet.DisplayRightToLeft = true;
            worksheet.Cells[1, 1] = $"دفتر کل: {GrouhName}";
            worksheet.Range[worksheet.Cells[1, 1], worksheet.Cells[1, 3]].Merge();
            worksheet.Cells[1, 4] = "از تاریخ:";
            worksheet.Cells[1, 5] = StartDate.ToString();
            worksheet.Cells[1, 6] = "تا تاریخ:";
            worksheet.Cells[1, 7] = EndDate.ToString();
            worksheet.Rows[1].HorizontalAlignment = HorizontalAlignment.Right;
            worksheet.Rows[1].Font.Bold = true;
            worksheet.Columns[1].Font.Bold = true;
            for (int i = 0; i < NamesOfColumns.Count(); i++)
            {
                worksheet.Cells[2, i + 1] = NamesOfColumns[i];
            }
            int tedadaSatrha = RawRooznamehTable.Count;

            for (int i = 1; i <= NamesOfColumns.Count(); i++)
            {
                for (int j = 1; j <= tedadaSatrha + 3; j++)
                {
                    worksheet.Cells[j, i].Borders.LineStyle = BorderStyle.FixedSingle;
                }
            }
            for (int j = 0; j < tedadaSatrha; j++)
            {
                int i = 1;
                worksheet.Cells[j + 3, i++] = RawRooznamehTable.Select(a => a.SanadId).ElementAt(j);
                worksheet.Cells[j + 3, i++] = RawRooznamehTable.Select(a => a.SanadTarikhShamsi).ElementAt(j);
                worksheet.Cells[j + 3, i++] = RawRooznamehTable.Select(a => a.IdHesab).ElementAt(j);
                worksheet.Cells[j + 3, i++] = RawRooznamehTable.Select(a => a.NameHesab).ElementAt(j);
                worksheet.Cells[j + 3, i++] = RawRooznamehTable.Select(a => a.NameSumGroupHesab).ElementAt(j);
                worksheet.Cells[j + 3, i++] = RawRooznamehTable.Select(a => a.SanadTozih).ElementAt(j);
                worksheet.Cells[j + 3, i++] = RawRooznamehTable.Select(a => a.SanadBedehkar).ElementAt(j);
                worksheet.Cells[j + 3, i - 1].NumberFormat = "#,##0";
                worksheet.Cells[j + 3, i++] = RawRooznamehTable.Select(a => a.SabadBestankar).ElementAt(j);
                worksheet.Cells[j + 3, i - 1].NumberFormat = "#,##0";
                worksheet.Cells[j + 3, i++] = Math.Abs((long)(RawRooznamehTable.Where(a => a.SanadId <= RawRooznamehTable.Select(b => b.SanadId).ElementAt(j)).Select(a => a.SanadBedehkar - a.SabadBestankar).Sum()));
                worksheet.Cells[j + 3, i - 1].NumberFormat = "#,##0";

                if (RawRooznamehTable.Where(a => a.SanadId <= RawRooznamehTable.Select(b => b.SanadId).ElementAt(j)).Select(a => a.SanadBedehkar - a.SabadBestankar).Sum() >= 0)
                {
                    worksheet.Cells[j + 3, i++] = "بدهکار";
                }
                else
                {
                    worksheet.Cells[j + 3, i++] = "بستانکار";
                }
                if (tedadaSatrha - j == 1)
                {
                    worksheet.Cells[j + 4, 6] = "جمع گردش";
                    worksheet.Cells[j + 4, 6].Font.Bold = true;
                    worksheet.Cells[j + 4, 7] = RawRooznamehTable.Select(a => a.SanadBedehkar).Sum();
                    worksheet.Cells[j + 4, 7].NumberFormat = "#,##0";
                    worksheet.Cells[j + 4, 7].Font.Bold = true;
                    worksheet.Cells[j + 4, 8] = RawRooznamehTable.Select(a => a.SabadBestankar).Sum();
                    worksheet.Cells[j + 4, 8].NumberFormat = "#,##0";
                    worksheet.Cells[j + 4, 8].Font.Bold = true;
                    worksheet.Cells[j + 4, 9] = Math.Abs((long)(RawRooznamehTable.Select(a => a.SanadBedehkar - a.SabadBestankar).Sum()));
                    worksheet.Cells[j + 4, 9].NumberFormat = "#,##0";
                    worksheet.Cells[j + 4, 9].Font.Bold = true;
                    if (RawRooznamehTable.Select(a => a.SanadBedehkar - a.SabadBestankar).Sum() >= 0)
                    {
                        worksheet.Cells[j + 4, 10] = "بدهکار";
                    }
                    else if (RawRooznamehTable.Select(a => a.SanadBedehkar - a.SabadBestankar).Sum() < 0)
                    {
                        worksheet.Cells[j + 4, 10] = "بستانکار";
                    }
                }
            }
            Microsoft.Office.Interop.Excel.Range usedRange = worksheet.UsedRange;
            usedRange.Columns.AutoFit();
            usedRange.Rows.AutoFit();

            var sfd = new SaveFileDialog();
            sfd.FileName = GrouhName;
            sfd.DefaultExt = ".Xlsx";
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                workbook.SaveAs(sfd.FileName, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Microsoft.Office.Interop.Excel.XlSaveAsAccessMode.xlExclusive, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);
            }
            app.Quit();
        }

        public void TabagheReport(List<tblRooznameh> rawTbl, string[] ColumnsName, string azShamsi, string taShamsi, DateTime azMiladi, DateTime taMiladi, string grouhName)
        {
            RawRooznamehTable = rawTbl;
            NamesOfColumns = ColumnsName;
            GrouhName = grouhName;
            StartDate = azShamsi;
            EndDate = taShamsi;
            MiladiStartDate = azMiladi;
            MiladiEndDate = taMiladi;
            Microsoft.Office.Interop.Excel.Application app = new Microsoft.Office.Interop.Excel.Application();
            Microsoft.Office.Interop.Excel.Workbook workbook = app.Workbooks.Add(Type.Missing);
            Microsoft.Office.Interop.Excel.Worksheet worksheet = null;
            worksheet = workbook.Sheets["Sheet1"];
            worksheet = workbook.ActiveSheet;
            worksheet.Name = "Report";
            worksheet.DisplayRightToLeft = true;
            worksheet.Cells[1, 1] = $"طبقه: {GrouhName}";
            worksheet.Range[worksheet.Cells[1, 1], worksheet.Cells[1, 3]].Merge();
            worksheet.Cells[1, 4] = "از تاریخ:";
            worksheet.Cells[1, 5] = StartDate.ToString();
            worksheet.Cells[1, 6] = "تا تاریخ:";
            worksheet.Cells[1, 7] = EndDate.ToString();
            worksheet.Rows[1].HorizontalAlignment = HorizontalAlignment.Right;
            worksheet.Rows[1].Font.Bold = true;
            worksheet.Columns[1].Font.Bold = true;
            for (int i = 0; i < NamesOfColumns.Count(); i++)
            {
                worksheet.Cells[2, i + 1] = NamesOfColumns[i];
            }
            int tedadaSatrha = RawRooznamehTable.Count;

            for (int i = 1; i <= NamesOfColumns.Count(); i++)
            {
                for (int j = 1; j <= tedadaSatrha + 3; j++)
                {
                    worksheet.Cells[j, i].Borders.LineStyle = BorderStyle.FixedSingle;
                }
            }
            for (int j = 0; j < tedadaSatrha; j++)
            {
                int i = 1;
                worksheet.Cells[j + 3, i++] = RawRooznamehTable.Select(a => a.SanadId).ElementAt(j);
                worksheet.Cells[j + 3, i++] = RawRooznamehTable.Select(a => a.SanadTarikhShamsi).ElementAt(j);
                worksheet.Cells[j + 3, i++] = RawRooznamehTable.Select(a => a.IdHesab).ElementAt(j);
                worksheet.Cells[j + 3, i++] = RawRooznamehTable.Select(a => a.NameHesab).ElementAt(j);
                worksheet.Cells[j + 3, i++] = RawRooznamehTable.Select(a => a.NameGroupHesab).ElementAt(j);
                worksheet.Cells[j + 3, i++] = RawRooznamehTable.Select(a => a.SanadTozih).ElementAt(j);
                worksheet.Cells[j + 3, i++] = RawRooznamehTable.Select(a => a.SanadBedehkar).ElementAt(j);
                worksheet.Cells[j + 3, i - 1].NumberFormat = "#,##0";
                worksheet.Cells[j + 3, i++] = RawRooznamehTable.Select(a => a.SabadBestankar).ElementAt(j);
                worksheet.Cells[j + 3, i - 1].NumberFormat = "#,##0";
                worksheet.Cells[j + 3, i++] = Math.Abs((long)(RawRooznamehTable.Where(a => a.SanadTarikhMiladi <= RawRooznamehTable.Select(b => b.SanadTarikhMiladi).ElementAt(j)).Select(a => a.SanadBedehkar - a.SabadBestankar).Sum()));
                worksheet.Cells[j + 3, i - 1].NumberFormat = "#,##0";

                if (RawRooznamehTable.Where(a => a.SanadTarikhMiladi <= RawRooznamehTable.Select(b => b.SanadTarikhMiladi).ElementAt(j)).Select(a => a.SanadBedehkar - a.SabadBestankar).Sum() >= 0)
                {
                    worksheet.Cells[j + 3, i++] = "بدهکار";
                }
                else
                {
                    worksheet.Cells[j + 3, i++] = "بستانکار";
                }
                if (tedadaSatrha - j == 1)
                {
                    worksheet.Cells[j + 4, 6] = "جمع گردش";
                    worksheet.Cells[j + 4, 6].Font.Bold = true;
                    worksheet.Cells[j + 4, 7] = RawRooznamehTable.Select(a => a.SanadBedehkar).Sum();
                    worksheet.Cells[j + 4, 7].NumberFormat = "#,##0";
                    worksheet.Cells[j + 4, 7].Font.Bold = true;
                    worksheet.Cells[j + 4, 8] = RawRooznamehTable.Select(a => a.SabadBestankar).Sum();
                    worksheet.Cells[j + 4, 8].NumberFormat = "#,##0";
                    worksheet.Cells[j + 4, 8].Font.Bold = true;
                    worksheet.Cells[j + 4, 9] = Math.Abs((long)(RawRooznamehTable.Select(a => a.SanadBedehkar - a.SabadBestankar).Sum()));
                    worksheet.Cells[j + 4, 9].NumberFormat = "#,##0";
                    worksheet.Cells[j + 4, 9].Font.Bold = true;
                    if (RawRooznamehTable.Select(a => a.SanadBedehkar - a.SabadBestankar).Sum() >= 0)
                    {
                        worksheet.Cells[j + 4, 10] = "بدهکار";
                    }
                    else if (RawRooznamehTable.Select(a => a.SanadBedehkar - a.SabadBestankar).Sum() < 0)
                    {
                        worksheet.Cells[j + 4, 10] = "بستانکار";
                    }
                }
            }
            Microsoft.Office.Interop.Excel.Range usedRange = worksheet.UsedRange;
            usedRange.Columns.AutoFit();
            usedRange.Rows.AutoFit();

            var sfd = new SaveFileDialog();
            sfd.FileName = GrouhName;
            sfd.DefaultExt = ".Xlsx";
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                workbook.SaveAs(sfd.FileName, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Microsoft.Office.Interop.Excel.XlSaveAsAccessMode.xlExclusive, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);
            }
            app.Quit();
        }

        public void AbstractReport(DataGridView dgv, string azShamsi, string taShamsi)
        {
            StartDate = azShamsi;
            EndDate = taShamsi;
            Microsoft.Office.Interop.Excel.Application app = new Microsoft.Office.Interop.Excel.Application();
            Microsoft.Office.Interop.Excel.Workbook workbook = app.Workbooks.Add(Type.Missing);
            Microsoft.Office.Interop.Excel.Worksheet worksheet = null;
            worksheet = workbook.Sheets["Sheet1"];
            worksheet = workbook.ActiveSheet;
            worksheet.Name = "Report";
            worksheet.DisplayRightToLeft = true;
            worksheet.Cells[1, 1] = $"خلاصه وضعیت مالی: {HesabName}";
            worksheet.Range[worksheet.Cells[1, 1], worksheet.Cells[1, 3]].Merge();
            worksheet.Cells[1, 4] = "از تاریخ:";
            worksheet.Cells[1, 5] = StartDate.ToString();
            worksheet.Cells[1, 6] = "تا تاریخ:";
            worksheet.Cells[1, 7] = EndDate.ToString();
            worksheet.Rows[1].HorizontalAlignment = HorizontalAlignment.Right;
            worksheet.Rows[1].Font.Bold = true;
            worksheet.Columns[1].Font.Bold = true;
            for (int i = 0; i < dgv.Columns.Count; i++)
            {
                worksheet.Cells[2, i + 1] = dgv.Columns[i].HeaderText;
            }

            for (int j = 0; j < dgv.Rows.Count; j++)
            {
                for (int i = 0; i < dgv.Columns.Count; i++)
                {
                    worksheet.Cells[j + 3, i + 1] = dgv.Rows[j].Cells[i].Value;
                    worksheet.Cells[j + 3, i + 1].NumberFormat = "#,##0";
                    if (j == dgv.Rows.Count - 1 && i == dgv.Columns.Count - 1)
                    {
                        double sum = 0;
                        for (int k = 0; k < dgv.Rows.Count; k++)
                        {
                            sum += Convert.ToDouble(dgv.Rows[k].Cells[9].Value);
                        }
                        worksheet.Cells[dgv.Rows.Count + 3, 9] = "جمع";
                        worksheet.Cells[dgv.Rows.Count + 3, 9].Font.Bold = true;
                        worksheet.Cells[dgv.Rows.Count + 3, 10] = sum;
                        worksheet.Cells[dgv.Rows.Count + 3, 10].NumberFormat = "#,##0";
                        worksheet.Cells[dgv.Rows.Count + 3, 10].Font.Bold = true;

                    }
                }
            }
            for (int i = 1; i <= dgv.Rows.Count + 3; i++)
            {
                for (int j = 1; j <= dgv.Columns.Count; j++)
                {
                    worksheet.Cells[i, j].Borders.LineStyle = BorderStyle.FixedSingle;
                }
            }
            Microsoft.Office.Interop.Excel.Range usedRange = worksheet.UsedRange;
            usedRange.Columns.AutoFit();
            usedRange.Rows.AutoFit();

            var sfd = new SaveFileDialog();
            sfd.FileName = "خلاصه وضعیت مالی";
            sfd.DefaultExt = ".Xlsx";
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                workbook.SaveAs(sfd.FileName, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Microsoft.Office.Interop.Excel.XlSaveAsAccessMode.xlExclusive, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);
            }
            app.Quit();
        }

        public void ExcellReader()
        {
            try
            {
                OpenFileDialog openFileDialog = new OpenFileDialog();
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    string filename = openFileDialog.FileName;
                    string extension = Path.GetExtension(filename);
                    if (extension == ".xls" || extension == ".xlsx")
                    {
                        ExcelReader db = new ExcelReader(filename, true, false);
                        TableSelectDialog t = new TableSelectDialog(db.GetWorksheetList());

                        if (t.ShowDialog() == DialogResult.OK)
                        {
                            var data = db.GetWorksheet(t.Selection);
                            this.SourceData = data;
                            List<Sanad> sanadList = new List<Sanad>();

                            for (int i = 0; i < SourceData.Rows.Count; i++)
                            {
                                Int64 v = 0;
                                Sanad s = new Sanad
                                {
                                    ShamsiDate = SourceData.Rows[i].ItemArray.ElementAt(1).ToString(),
                                    SanadDescription = SourceData.Rows[i].ItemArray.ElementAt(2).ToString(),
                                    SanadDebit=SourceData.Rows[i].ItemArray.ElementAt(3).ToString(),
                                    SanadCredit = SourceData.Rows[i].ItemArray.ElementAt(4).ToString()
                                };
                                sanadList.Add(s);
                            }

                            List<Sanad> BuyList = new List<Sanad>();
                            List<Sanad> SellList = new List<Sanad>();
                            foreach (var item in sanadList)
                            {
                                if (item.SanadDescription.Contains("خرید ") && item.SanadDescription.Contains("به نرخ "))
                                {
                                    //برای معاملات غیر وزنی که متن توضیح سند در گزارش کارگزاری متفاوت است
                                    if (item.SanadDescription.Contains("خرید تعداد"))
                                    {
                                        Sanad s = new Sanad();
                                        s.BuyVolume = Convert.ToInt32(Sanad.GetBetween(item.SanadDescription, "تعداد ", "سهم "));
                                        s.HesabName = Sanad.GetBetween(item.SanadDescription, "(", ")");
                                        s.BuyPrice = Convert.ToInt32(Sanad.GetBetween(item.SanadDescription, "نرخ", "به"));
                                        s.SanadDescription = item.SanadDescription;
                                        s.ShamsiDate = item.ShamsiDate;
                                        s.SanadDebit = item.SanadDebit;
                                        s.SanadCredit = item.SanadCredit;
                                        BuyList.Add(s);
                                    }
                                    else
                                    {
                                        Sanad s = new Sanad();
                                        s.BuyVolume = Convert.ToInt32(Sanad.GetBetween(item.SanadDescription, "خرید ", "سهم"));
                                        s.HesabName = Sanad.GetBetween(item.SanadDescription, "(", ")");
                                        s.BuyPrice = Convert.ToInt32(Sanad.GetBetween(item.SanadDescription, "نرخ", "طی"));
                                        s.SanadDescription = item.SanadDescription;
                                        s.ShamsiDate = item.ShamsiDate;
                                        s.SanadDebit = item.SanadDebit;
                                        s.SanadCredit = item.SanadCredit;
                                        BuyList.Add(s);
                                    }
                                }
                                if (item.SanadDescription.Contains("فروش ") && item.SanadDescription.Contains("به نرخ "))
                                {
                                    //برای معاملات غیر وزنی که متن توضیح سند در گزارش کارگزاری متفاوت است
                                    if (item.SanadDescription.Contains("فروش تعداد"))
                                    {
                                        Sanad s = new Sanad();
                                        s.SellVolume = Convert.ToInt32(Sanad.GetBetween(item.SanadDescription, "تعداد", "سهم"));
                                        s.HesabName = Sanad.GetBetween(item.SanadDescription, "(", ")");
                                        s.SellPrice = Convert.ToInt32(Sanad.GetBetween(item.SanadDescription, "نرخ", "به"));
                                        s.SanadDescription = item.SanadDescription;
                                        s.ShamsiDate = item.ShamsiDate;
                                        s.SanadDebit = item.SanadDebit;
                                        s.SanadCredit = item.SanadCredit;
                                        SellList.Add(s);
                                    }
                                    else
                                    {
                                        Sanad s = new Sanad();
                                        s.SellVolume = Convert.ToInt32(Sanad.GetBetween(item.SanadDescription, "فروش", "سهم"));
                                        s.HesabName = Sanad.GetBetween(item.SanadDescription, "(", ")");
                                        s.SellPrice = Convert.ToInt32(Sanad.GetBetween(item.SanadDescription, "نرخ", "طی"));
                                        s.SanadDescription = item.SanadDescription;
                                        s.ShamsiDate = item.ShamsiDate;
                                        s.SanadDebit = item.SanadDebit;
                                        s.SanadCredit = item.SanadCredit;
                                        SellList.Add(s);
                                    }
                                }
                            }
                            MyBuyList = BuyList;
                            MySellList = SellList;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBoxFarsi.Show(ex.Message.ToString(), "خطا در جستجو");
            }
        }

    }
}
