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
    public partial class frmSabtSanad : Form
    {
        public frmSabtSanad()
        {
            InitializeComponent();
        }

        AccountDBEntities db = new AccountDBEntities();
        public void frmSabtSanad_Load(object sender, EventArgs e)
        {
            try
            {
                /////////////////////////////
                System.Globalization.PersianCalendar pc = new System.Globalization.PersianCalendar();
                DateTime now = DateTime.Now;
                mskTarikh.Text = pc.GetYear(now).ToString() + pc.GetMonth(now).ToString("0#") + pc.GetDayOfMonth(now).ToString("0#") + pc.GetHour(now).ToString("0#") + pc.GetMinute(now).ToString("0#") + pc.GetSecond(now).ToString("0#");

                DateTime yesterday = DateTime.Now.AddDays(-1);
                mskAzTarikh.Text = pc.GetYear(yesterday).ToString() + pc.GetMonth(yesterday).ToString("0#") + pc.GetDayOfMonth(yesterday).ToString("0#") + pc.GetHour(yesterday).ToString("0#") + pc.GetMinute(yesterday).ToString("0#") + pc.GetSecond(yesterday).ToString("0#");

                DateTime tomorrow = DateTime.Now.AddDays(1);
                mskTaTarikh.Text = pc.GetYear(tomorrow).ToString() + pc.GetMonth(tomorrow).ToString("0#") + pc.GetDayOfMonth(tomorrow).ToString("0#") + pc.GetHour(tomorrow).ToString("0#") + pc.GetMinute(tomorrow).ToString("0#") + pc.GetSecond(tomorrow).ToString("0#");
                ////////////////////////////
                var HesabhaList = db.tblRooznamehs.OrderBy(a => a.IdHesab).ToList();
                var IdHesabhaList = HesabhaList.Select(a => a.IdHesab).Distinct().ToArray();
                var NameHesabhaList = HesabhaList.Select(a => a.NameHesab).Distinct().ToArray();
                foreach (var item in IdHesabhaList)
                {
                    cmbCode.Items.Add(item);
                }
                foreach (var item in NameHesabhaList)
                {
                    cmbName.Items.Add(item);
                }

                cmbReport.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                MessageBoxFarsi.Show(ex.Message.ToString(), "خطا");
            }
        }

        private void btnInsert_Click(object sender, EventArgs e)
        {
            try
            {
                if (cmbCode.Text == "" || cmbName.Text == "")
                {
                    MessageBoxFarsi.Show("نام یا کد حساب انتخاب نشده است", "خطا در تنظیم سند");
                    return;
                }
                if (numBedehkar.Value == 0 && numBestankar.Value == 0)
                {
                    MessageBoxFarsi.Show("یا مبلغ بدهکار یا مبلغ بستانکار می‌تواند غیرصفر باشد", "خطا در تنظیم سند");
                    return;
                }
                dgvSanad.Rows.Add(cmbCode.Text, cmbName.Text, txtSharh.Text, numBedehkar.Value, numBestankar.Value);
                dgvSanad.Columns[3].DefaultCellStyle.Format = "#,0.###";
                dgvSanad.Columns[4].DefaultCellStyle.Format = "#,0.###";
                numBedehkar.Value = 0;
                numBestankar.Value = 0;
            }
            catch (Exception ex)
            {
                MessageBoxFarsi.Show(ex.Message.ToString(), "خطا");
            }
        }

        private void cmbCode_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                int codeHesab = Convert.ToInt32(cmbCode.SelectedItem);
                string nameHesab = db.tblRooznamehs.Where(a => a.IdHesab == codeHesab).Select(a => a.NameHesab).Distinct().FirstOrDefault();
                cmbName.SelectedItem = nameHesab;
            }
            catch (Exception ex)
            {
                MessageBoxFarsi.Show(ex.Message.ToString(), "خطا");
            }
        }

        private void cmbName_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                string nameHesab = cmbName.SelectedItem.ToString();
                var codeHesabList = db.tblRooznamehs.Where(a => a.NameHesab == nameHesab).Select(a => a.IdHesab).Distinct().ToList();
                cmbCode.Items.Clear();
                foreach (var item in codeHesabList)
                {
                    cmbCode.Items.Add(item);
                }
                int codeHesab = Convert.ToInt32(db.tblRooznamehs.Where(a => a.NameHesab == nameHesab).Select(a => a.IdHesab).Distinct().FirstOrDefault());
                if (codeHesabList.Count == 1)
                {
                    cmbCode.Text = codeHesab.ToString();
                }
                //else if (codeHesabList.Count > 1)
                // {
                //     MessageBoxFarsi.Show("با توجه به اینکه حساب مورد نظر، می‌تواند ماهیت بدهکار و بستانکار داشته باشد، در انتخاب کد حساب دقت نمایید");
                // }

            }
            catch (Exception ex)
            {
                MessageBoxFarsi.Show(ex.Message.ToString(), "خطا");
            }
        }

        private void txtSearchName_TextChanged(object sender, EventArgs e)
        {
            try
            {
                var NameHesab = db.tblRooznamehs.Where(a => a.NameHesab.Contains(txtSearchName.Text)).Select(a => a.NameHesab).Distinct().FirstOrDefault();
   
                if (NameHesab == null)
                {
                    MessageBoxFarsi.Show("حسابی با این مشخصات وجود ندارد");
                }
                else
                {
                    cmbName.Text = NameHesab;
                }

            }
            catch (Exception ex)
            {
                MessageBoxFarsi.Show(ex.Message.ToString(), "خطا در جستجو");
            }
        }

        private void txtSearchCode_TextChanged(object sender, EventArgs e)
        {
            try
            {
                string mySelectedCode = txtSearchCode.Text;
                int idHesab = (int)db.tblRooznamehs.Where(a => a.IdHesab.ToString().Contains(mySelectedCode)).Select(a => a.IdHesab).Distinct().FirstOrDefault();
                if (idHesab == 0)
                {
                    MessageBoxFarsi.Show("حسابی با این مشخصات وجود ندارد");
                }
                else
                {
                    cmbCode.Text = idHesab.ToString();
                }
            }
            catch (Exception ex)
            {
                MessageBoxFarsi.Show(ex.Message.ToString(), "خطا در جستجو");
            }
        }

        private void txtSearchCode_MouseClick(object sender, MouseEventArgs e)
        {
            txtSearchCode.Text = "";
        }

        private void txtSearchName_MouseClick(object sender, MouseEventArgs e)
        {
            txtSearchName.Text = "";
        }

        private void btnDel_Click(object sender, EventArgs e)
        {
            try
            {
                dgvSanad.Rows.Remove(dgvSanad.CurrentRow);
            }
            catch (Exception ex)
            {
                MessageBoxFarsi.Show(ex.Message.ToString(), "خطا در حذف سطر مورد نظر");
            }
        }

        private void btnSabt_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvSanad.Rows.Count == 0 || dgvSanad.Rows == null)
                {
                    MessageBoxFarsi.Show("ابتدا باید سند را تنظیم نمایید", "تکمیل اطلاعات سند");
                    return;
                }
                string tarikhShamsi = mskTarikh.Text;

                DateConvert miladi = new DateConvert();
                DateTime tarikhMiladi = new DateTime();
                //برای اینکه دو نوع فرمت تاریخ (با دقیقه و ساعت یا بدون آنها) را از سند چک کنیم
                var result = miladi.ConvertToMiladi(tarikhShamsi);
                if (result == true)
                {
                    tarikhMiladi = miladi.TarikhMiladi;
                }
                if (result == false)
                {
                    miladi.ShortConvertToMiladi(tarikhShamsi);
                    tarikhMiladi = miladi.TarikhMiladi;
                }

                double lastSanad = 0;
                var LastIdsanadGhablAz = db.tblRooznamehs.Where(a => a.SanadTarikhMiladi < tarikhMiladi && a.SanadId > 0).Select(a => a.SanadId).ToList();
                var LastIdsanadBaedAz = db.tblRooznamehs.Where(a => a.SanadTarikhMiladi >= tarikhMiladi && a.SanadId > 0).Select(a => a.SanadId).ToList();
                if (LastIdsanadGhablAz.Count == 0 && LastIdsanadBaedAz.Count == 0)
                {
                    lastSanad = 1;
                }
                else if (LastIdsanadGhablAz.Count == 0 && LastIdsanadBaedAz.Count > 0)
                {
                    LastIdsanadGhablAz = db.tblRooznamehs.Where(a => a.SanadTarikhMiladi <= tarikhMiladi && a.SanadId > 0).Select(a => a.SanadId).ToList();
                    LastIdsanadBaedAz = db.tblRooznamehs.Where(a => a.SanadTarikhMiladi >= tarikhMiladi && a.SanadId > 0).Select(a => a.SanadId).ToList();
                    if (LastIdsanadGhablAz.Count==0)
                    {
                        lastSanad = Convert.ToDouble(LastIdsanadBaedAz.Min())/2;
                    }
                    else
                    {
                        lastSanad = Convert.ToDouble(LastIdsanadGhablAz.Min())/2;
                    }
                }
                else if (LastIdsanadGhablAz.Count > 0 && LastIdsanadBaedAz.Count == 0)
                {
                    LastIdsanadGhablAz = db.tblRooznamehs.Where(a => a.SanadTarikhMiladi <= tarikhMiladi && a.SanadId > 0).Select(a => a.SanadId).ToList();
                    lastSanad = Convert.ToInt64(LastIdsanadGhablAz.Max() + 1);
                }
                else if (LastIdsanadGhablAz.Count > 0 && LastIdsanadBaedAz.Count > 0)
                {
                    var MyBeforMaxId = db.tblRooznamehs.Where(a => a.SanadTarikhMiladi < tarikhMiladi && a.SanadId > 0).Select(a => a.SanadId).Max();
                    var MyAfterMinId = db.tblRooznamehs.Where(a => a.SanadTarikhMiladi >= tarikhMiladi && a.SanadId > 0).Select(a => a.SanadId).Min();
                    lastSanad = Convert.ToDouble((MyBeforMaxId + MyAfterMinId) / 2);
                }

                List<tblRooznameh> sanadList = new List<tblRooznameh>();
                foreach (DataGridViewRow row in dgvSanad.Rows)
                {
                    long idHesab = Convert.ToInt64(row.Cells[0].Value);
                    string nameGrouhHesab = db.tblRooznamehs.Where(a => a.IdHesab == idHesab).Select(a => a.NameGroupHesab).FirstOrDefault();
                    string nameZirGrouh = db.tblRooznamehs.Where(a => a.IdHesab == idHesab).Select(a => a.NameSumGroupHesab).FirstOrDefault();
                    tblRooznameh tbl = new tblRooznameh()
                    {
                        SanadId = (decimal)lastSanad,
                        SanadTarikhShamsi = tarikhShamsi,
                        SanadTarikhMiladi = tarikhMiladi,
                        IdHesab = Convert.ToInt64(row.Cells[0].Value),
                        NameHesab = row.Cells[1].Value.ToString(),
                        NameGroupHesab = nameGrouhHesab,
                        NameSumGroupHesab = nameZirGrouh,
                        SanadTozih = row.Cells[2].Value.ToString(),
                        SanadBedehkar = Convert.ToInt64(row.Cells[3].Value),
                        SabadBestankar = Convert.ToInt64(row.Cells[4].Value)
                    };
                    sanadList.Add(tbl);
                }
                var sumBed = sanadList.Select(a => a.SanadBedehkar).Sum();
                var sumBes = sanadList.Select(a => a.SabadBestankar).Sum();

                if (sumBed != sumBes)
                {
                    MessageBoxFarsi.Show($"جمع ستون بدهکار با جمع ستون بستانکار مبلغ {Math.Abs((long)(sumBed - sumBes))} اختلاف دارد", "سند ناتراز");
                    return;
                }
                db.tblRooznamehs.AddRange(sanadList);
                db.SaveChanges();
                DisplayNewItem(sanadList.Select(a => a.SanadId).FirstOrDefault());
                MessageBoxFarsi.Show("سند با موفقیت به ثبت رسید", "ثبت موفق");
                dgvSanad.Rows.Clear();
            }
            catch (Exception ex)
            {
                MessageBoxFarsi.Show(ex.Message.ToString(), "خطا در ثبت سند");
            }
        }

        public void btnDisplay_Click(object sender, EventArgs e)
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

                var hesabhaInfoList = db.tblRooznamehs.OrderBy(a => a.SanadId).Where(a => a.SanadTarikhMiladi >= tarikhMiladiAz && a.SanadTarikhMiladi <= tarikhMiladiTa && a.SanadId > 0).Select(a => new { a.SanadId, a.SanadTarikhShamsi, a.IdHesab, a.NameHesab, a.NameGroupHesab, a.NameSumGroupHesab, a.SanadTozih, a.SanadBedehkar, a.SabadBestankar }).Distinct().ToList();
                //برای نامگذاری سرستون‌ها در کلاس مربوطه
                string[] NameSotunha = { "شماره سند", "تاریخ", "کد حساب", "نام حساب", "طبقه حساب", "زیرطبقه حساب", "شرح سند", "بدهکار", "بستانکار" };
                Display calledDgv = new Display();
                calledDgv.ShowDgv(dgvDisplay, NameSotunha, hesabhaInfoList);
                dgvDisplay = calledDgv.dgvFromDisplay;
                dgvDisplay.Columns[0].DefaultCellStyle.Format = "N4";
                dgvDisplay.Columns[7].DefaultCellStyle.Format = "N0";
                dgvDisplay.Columns[8].DefaultCellStyle.Format = "N0";
            }
            catch (Exception ex)
            {
                MessageBoxFarsi.Show(ex.Message.ToString(), "خطا در نمایش");
            }
        }

        public void DisplayNewItem(decimal sanadId)
        {
            try
            {
                var hesabhaInfoList = db.tblRooznamehs.OrderBy(a => a.SanadId).Where(a => a.SanadId == sanadId).Select(a => new { a.SanadId, a.SanadTarikhShamsi, a.IdHesab, a.NameHesab, a.NameGroupHesab, a.NameSumGroupHesab, a.SanadTozih, a.SanadBedehkar, a.SabadBestankar }).Distinct().ToList();
                //برای نامگذاری سرستون‌ها در کلاس مربوطه
                string[] NameSotunha = { "شماره سند", "تاریخ", "کد حساب", "نام حساب", "طبقه حساب", "زیرطبقه حساب", "شرح سند", "بدهکار", "بستانکار" };
                Display calledDgv = new Display();
                calledDgv.ShowDgv(dgvDisplay, NameSotunha, hesabhaInfoList);
                dgvDisplay = calledDgv.dgvFromDisplay;
                dgvDisplay.Columns[0].DefaultCellStyle.Format = "N4";
                dgvDisplay.Columns[7].DefaultCellStyle.Format = "N0";
                dgvDisplay.Columns[8].DefaultCellStyle.Format = "N0";
            }
            catch (Exception ex)
            {
                MessageBoxFarsi.Show(ex.Message.ToString(), "خطا در نمایش");
            }
        }

        public void txtSearchIdSanad_TextChanged(object sender, EventArgs e)
        {
            try
            {
                decimal myIdSanad = Convert.ToDecimal(txtSearchIdSanad.Text);

                var hesabhaInfoList = db.tblRooznamehs.OrderBy(a => a.SanadId).Where(a => a.SanadId == myIdSanad && a.SanadId > 0).Select(a => new { a.SanadId, a.SanadTarikhShamsi, a.IdHesab, a.NameHesab, a.NameGroupHesab, a.NameSumGroupHesab, a.SanadTozih, a.SanadBedehkar, a.SabadBestankar }).ToList();
                //برای نامگذاری سرستون‌ها در کلاس مربوطه
                string[] NameSotunha = { "شماره سند", "تاریخ", "کد حساب", "نام حساب", "طبقه حساب", "زیرطبقه حساب", "شرح سند", "بدهکار", "بستانکار" };
                Display calledDgv = new Display();
                calledDgv.ShowDgv(dgvDisplay, NameSotunha, hesabhaInfoList);
                dgvDisplay = calledDgv.dgvFromDisplay;
                dgvDisplay.Columns[0].DefaultCellStyle.Format = "N4";
                dgvDisplay.Columns[7].DefaultCellStyle.Format = "N0";
                dgvDisplay.Columns[8].DefaultCellStyle.Format = "N0";
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
                string myHesab = txtSearchNam.Text;
                var hesabhaInfoList = db.tblRooznamehs.OrderBy(a => a.SanadId).Where(a => a.NameHesab.Contains(myHesab) && a.SanadId > 0).Select(a => new { a.SanadId, a.SanadTarikhShamsi, a.IdHesab, a.NameHesab, a.NameGroupHesab, a.NameSumGroupHesab, a.SanadTozih, a.BuySellVol, a.ArzeshAfzudehTax, a.EnteghaTax, a.SanadBedehkar, a.SabadBestankar }).Distinct().ToList();
                //برای نامگذاری سرستون‌ها در کلاس مربوطه
                string[] NameSotunha = { "شماره سند", "تاریخ", "کد حساب", "نام حساب", "طبقه حساب", "زیرطبقه حساب", "شرح سند", "حجم معاملات", "مالیات ارزش افزوده", "مالیات نقل و انتقال", "بدهکار", "بستانکار" };
                Display calledDgv = new Display();
                calledDgv.ShowDgv(dgvDisplay, NameSotunha, hesabhaInfoList);
                dgvDisplay = calledDgv.dgvFromDisplay;
                dgvDisplay.Columns[0].DefaultCellStyle.Format = "N4";
                dgvDisplay.Columns[7].DefaultCellStyle.Format = "N0";
                dgvDisplay.Columns[8].DefaultCellStyle.Format = "N0";
                dgvDisplay.Columns[9].DefaultCellStyle.Format = "N0";
                dgvDisplay.Columns[10].DefaultCellStyle.Format = "N0";
                dgvDisplay.Columns[11].DefaultCellStyle.Format = "N0";
            }
            catch (Exception ex)
            {
                MessageBoxFarsi.Show(ex.Message.ToString(), "خطا در جستجو");
            }
        }

        public void btnEdit_Click(object sender, EventArgs e)
        {
            try
            {
                int RowsCount = dgvDisplay.Rows.Count;
                if (RowsCount < 2)
                {
                    MessageBoxFarsi.Show("سندی انتخاب نشده یا اطلاعات سند انتخاب نشده است، از بخش جستجوی سند، سند را مشخص نمایید", "انتخاب ناقص سند");
                    return;
                }

                List<decimal> idSanadList = new List<decimal>();
                for (int i = 0; i < RowsCount; i++)
                {
                    idSanadList.Add(Convert.ToInt32(dgvDisplay.Rows[i].Cells[0].Value));
                }
                int TedadSanad = idSanadList.Distinct().Count();//تعداد سند مجزا
                if (TedadSanad > 1)
                {
                    MessageBoxFarsi.Show("صرفاً مجاز به انتخاب یک سند جهت ویراش می‌باشید", "انتخاب بیش از یک سند");
                    return;
                }
                DataTable dt = new DataTable();
                foreach (DataGridViewColumn col in dgvDisplay.Columns)
                {
                    dt.Columns.Add(col.HeaderText, typeof(string));
                }
                dt.Columns[0].DataType = typeof(decimal);
                dt.Columns[7].DataType = typeof(double);
                dt.Columns[8].DataType = typeof(double);
                foreach (DataGridViewRow row in dgvDisplay.Rows)
                {
                    DataRow dRow = dt.NewRow();
                    foreach (DataGridViewCell cell in row.Cells)
                    {
                        if (cell.Value == null)
                        {
                            dRow[cell.ColumnIndex] = 0;
                        }
                        else
                        {
                            dRow[cell.ColumnIndex] = cell.Value;
                        }
                    }
                    dt.Rows.Add(dRow);
                }
                frmEditSanad frmEdit = new frmEditSanad();
                frmEdit.dgvEdit.DataSource = dt;
                for (int i = 0; i < frmEdit.dgvEdit.ColumnCount; i++)
                {
                    frmEdit.dgvEdit.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                }
                frmEdit.dgvEdit.Columns[0].ReadOnly = true;
                frmEdit.dgvEdit.Columns[2].ReadOnly = true;
                frmEdit.dgvEdit.Columns[3].ReadOnly = true;
                frmEdit.dgvEdit.Columns[4].ReadOnly = true;
                frmEdit.dgvEdit.Columns[5].ReadOnly = true;
                frmEdit.dgvEdit.Columns[0].DefaultCellStyle.Format = "N4";
                frmEdit.dgvEdit.Columns[7].DefaultCellStyle.Format = "N0";
                frmEdit.dgvEdit.Columns[8].DefaultCellStyle.Format = "N0";
                frmEdit.ShowDialog();
                DisplayNewItem(idSanadList.FirstOrDefault());
            }
            catch (Exception ex)
            {
                MessageBoxFarsi.Show(ex.Message.ToString(), "وجود خطا در ویرایش سند");
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                decimal IdSanadHazfi = Convert.ToDecimal(dgvDisplay.CurrentRow.Cells[0].Value);
                var del = db.tblRooznamehs.Where(a => a.SanadId == IdSanadHazfi);
                db.tblRooznamehs.RemoveRange(del);
                db.SaveChanges();
                DisplayNewItem(IdSanadHazfi);
                MessageBoxFarsi.Show($"حذف سند {IdSanadHazfi} با موفقیت انجام شد", "حذف موفق سند");
            }
            catch (Exception ex)
            {
                MessageBoxFarsi.Show(ex.Message.ToString(), "خطا در حذف");
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
                if (cmbReport.SelectedIndex == 1)
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

                if (cmbReport.SelectedIndex == 2)
                {
                    if (dgvDisplay.DataSource == null)
                    {
                        MessageBoxFarsi.Show("یک گروه حساب را از جدول نمایش انتخاب کنید", "عدم انتخاب دفتر کل");
                        return;
                    }
                    string HesabGrouh = dgvDisplay.CurrentRow.Cells[5].Value.ToString();
                    var hesabhaInfoList = db.tblRooznamehs.OrderBy(a => a.SanadId).Where(a => a.NameGroupHesab == HesabGrouh && a.SanadTarikhMiladi >= tarikhMiladiAz && a.SanadTarikhMiladi <= tarikhMiladiTa && a.SanadId > 0).ToList();
                    if (hesabhaInfoList.Count == 0 || hesabhaInfoList == null)
                    {
                        MessageBoxFarsi.Show("هیچ سابقه‌ای برای گروه حساب انتخاب شده در بازه زمانی مورد نظر وجود ندارد", "نبود سابقه اطلاعاتی در گزارش گیری");
                        return;
                    }
                    string[] NameSotunha = { "شماره سند", "تاریخ", "کد حساب", "نام حساب", "گروه حساب", "شرح سند", "بدهکار", "بستانکار", "مانده", "تشخیص" };
                    ExcelReports report = new ExcelReports();
                    report.KolReport(hesabhaInfoList, NameSotunha, tarikhShamsiAz, tarikhShamsiTa, tarikhMiladiAz, tarikhMiladiTa, HesabGrouh);

                }
            }
            catch (Exception ex)
            {
                MessageBoxFarsi.Show(ex.Message.ToString(), "خطا در گزارش گیری");
            }
        }

        private void tableLayoutPanel7_Paint(object sender, PaintEventArgs e)
        {

        }

        private void dgvDisplay_MouseUp(object sender, MouseEventArgs e)
        {
            try
            {
                txtSearchNam.Text = dgvDisplay.CurrentRow.Cells[3].Value.ToString();
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
                decimal myIdSanad = Convert.ToDecimal(txtSearchIdSanad.Value);
                var hesabhaInfoList = db.tblRooznamehs.OrderBy(a => a.SanadId).Where(a => a.SanadId == myIdSanad && a.SanadId > 0).Select(a => new { a.SanadId, a.SanadTarikhShamsi, a.IdHesab, a.NameHesab, a.NameGroupHesab, a.NameSumGroupHesab, a.SanadTozih, a.SanadBedehkar, a.SabadBestankar }).ToList();
                //برای نامگذاری سرستون‌ها در کلاس مربوطه
                string[] NameSotunha = { "شماره سند", "تاریخ", "کد حساب", "نام حساب", "طبقه حساب", "زیرطبقه حساب", "شرح سند", "بدهکار", "بستانکار" };
                Display calledDgv = new Display();
                calledDgv.ShowDgv(dgvDisplay, NameSotunha, hesabhaInfoList);
                dgvDisplay = calledDgv.dgvFromDisplay;
                dgvDisplay.Columns[0].DefaultCellStyle.Format = "N4";
                dgvDisplay.Columns[7].DefaultCellStyle.Format = "N0";
                dgvDisplay.Columns[8].DefaultCellStyle.Format = "N0";
            }
            catch (Exception ex)
            {
                MessageBoxFarsi.Show(ex.Message.ToString(), "خطا در جستجو");
            }
        }
    }
}
