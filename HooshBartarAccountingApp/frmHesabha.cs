using BehComponents;
using HooshBartarAccountingApp.DatabaseModel;
using HooshBartarAccountingApp.DataShow;
using mshtml;
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
    public partial class frmHesabha : Form
    {
        public frmHesabha()
        {
            InitializeComponent();
        }

        AccountDBEntities db = new AccountDBEntities();

        public void frmHesabha_Load(object sender, EventArgs e)
        {
            try
            {
                /////////////////////////////
                System.Globalization.PersianCalendar pc = new System.Globalization.PersianCalendar();
                DateTime now = DateTime.Now;
                mskTarikh.Text = pc.GetYear(now).ToString() + pc.GetMonth(now).ToString("0#") + pc.GetDayOfMonth(now).ToString("0#") + pc.GetHour(now).ToString("0#") + pc.GetMinute(now).ToString("0#") + pc.GetSecond(now).ToString("0#");
                ////////////////////////////
                var HesabhaList = db.tblTabaghatHesabs.OrderBy(a => a.CodeTabaghehHesab).ToList();
                var TabagheList = HesabhaList.Select(a => a.TabagheyehHesab).Distinct().ToArray();
                cmbGrouheHesab.Items.AddRange(TabagheList);
                cmbGrouheHesab.SelectedIndex = 0;
                cmbDaftarKol.SelectedIndex = 1;
                this.ActiveControl = btnSave;
            }
            catch (Exception ex)
            {
                MessageBoxFarsi.Show(ex.Message.ToString(), "خطا");
            }
        }



        private void cmbGrouheHesab_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            try
            {
                cmbDaftarKol.Items.Clear();
                var TabagheList = db.tblTabaghatHesabs.OrderBy(a => a.CodeTabaghehHesab).Select(a => a.TabagheyehHesab).Distinct().ToList();
                foreach (var item in TabagheList)
                {
                    if (cmbGrouheHesab.SelectedItem.ToString() == item)
                    {
                        var MoeinList = db.tblTabaghatHesabs.Where(a => a.TabagheyehHesab == item).Select(a => a.ZirTzbagheyehHesab).ToArray();
                        cmbDaftarKol.Items.AddRange(MoeinList);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBoxFarsi.Show(ex.Message.ToString(), "خطا");
            }
        }

        public void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (cmbGrouheHesab.SelectedItem == null || cmbDaftarKol.SelectedItem == null)
                {
                    MessageBoxFarsi.Show("طبقه یا زیر طبقه حساب انتخاب نشده است", "الزام انتخاب");
                    return;
                }
                if (txtNameHesab.Text == "")
                {
                    MessageBoxFarsi.Show("نام حسابی وارد نشده است", "الزام وارد کردن نام حساب");
                    return;
                }
                string tarikhShamsi = mskTarikh.Text;
                DateConvert miladi = new DateConvert();
                miladi.ConvertToMiladi(tarikhShamsi);
                DateTime tarikhMiladi = miladi.TarikhMiladi;
                var tabagheyeHesab = cmbGrouheHesab.SelectedItem.ToString();
                var ZirTabgheyeHEsab = cmbDaftarKol.SelectedItem.ToString();
                var ZirGrouh = db.tblRooznamehs.Where(a => a.NameSumGroupHesab == ZirTabgheyeHEsab && a.NameGroupHesab == tabagheyeHesab).Select(a => a.IdHesab).ToList();
                int CodeGrouh = db.tblTabaghatHesabs.Where(a => a.TabagheyehHesab == tabagheyeHesab).Select(a => a.CodeTabaghehHesab).FirstOrDefault();
                int CodeZirGrouh = db.tblTabaghatHesabs.Where(a => a.ZirTzbagheyehHesab == ZirTabgheyeHEsab).Select(a => a.CodeZirtabagheAz).FirstOrDefault();
                int lastIdHesab;
                if (ZirGrouh.Count == 0)
                {
                    lastIdHesab = Convert.ToInt32(CodeGrouh.ToString() + "00" + CodeZirGrouh.ToString());
                }
                else
                {
                    lastIdHesab = Convert.ToInt32(ZirGrouh.Max() + 1);
                }

                double IdSanad = 0;
                tblRooznameh tbl = new tblRooznameh();
                tbl.SanadId = (decimal)IdSanad;
                tbl.IdHesab = lastIdHesab;
                tbl.NameHesab = txtNameHesab.Text;
                tbl.NameGroupHesab = cmbGrouheHesab.SelectedItem.ToString();
                tbl.NameSumGroupHesab = cmbDaftarKol.SelectedItem.ToString();
                tbl.SanadBedehkar = Convert.ToInt64(numVol.Value);
                tbl.SabadBestankar = Convert.ToInt64(numVol.Value);
                tbl.SanadTozih = txtSharh.Text;
                tbl.SanadTarikhMiladi = tarikhMiladi;
                tbl.SanadTarikhShamsi = mskTarikh.Text;
                db.tblRooznamehs.Add(tbl);
                db.SaveChanges();
                DisplayNewItem(txtNameHesab.Text);
                MessageBoxFarsi.Show($"حساب {txtNameHesab.Text} با موفقیت ثبت شد", "معرفی موفقیت‌آمیز حساب");
                txtNameHesab.Text = "";
            }
            catch (Exception ex)
            {
                MessageBoxFarsi.Show(ex.Message.ToString(), "خطا در معرفی حساب");
            }
        }
        private void btnDisplay_Click(object sender, EventArgs e)
        {
            try
            {
                DateConvert dt = new DateConvert();
                var res = dt.ConvertToMiladi(mskTarikh.Text);
                if (res == true)
                {
                    var miladi = dt.TarikhMiladi;
                    var hesabhaInfoList = db.tblRooznamehs.OrderBy(a => a.SanadTarikhMiladi).Select(a => new { a.IdHesab, a.NameHesab, a.NameGroupHesab, a.NameSumGroupHesab }).Distinct().ToList();
                    //برای نامگذاری سرستون‌ها در کلاس مربوطه
                    string[] NameSotunha = { "کد حساب", "نام حساب", "طبقه حساب", "زیرطبقه حساب" };
                    Display calledDgv = new Display();
                    calledDgv.ShowDgv(dgvOperation, NameSotunha, hesabhaInfoList);
                    dgvOperation = calledDgv.dgvFromDisplay;

                }
                else
                {
                    MessageBoxFarsi.Show("تاریخ را به درستی وارد کنید", "فرمت نادرست تاریخ");

                }
            }
            catch (Exception ex)
            {
                MessageBoxFarsi.Show(ex.Message.ToString(), "خطا در نمایش");
            }
        }
        private void DisplayNewItem(string nameHesab)
        {
            try
            {
                DateConvert dt = new DateConvert();
                var res = dt.ConvertToMiladi(mskTarikh.Text);
                if (res == true)
                {
                    var miladi = dt.TarikhMiladi;
                    var yesteday = miladi.AddDays(-1);
                    var hesabhaInfoList = db.tblRooznamehs.OrderBy(a => a.SanadTarikhMiladi).Where(a => a.NameHesab == nameHesab).Select(a => new { a.IdHesab, a.NameHesab, a.NameGroupHesab, a.NameSumGroupHesab }).Distinct().ToList();
                    //برای نامگذاری سرستون‌ها در کلاس مربوطه
                    string[] NameSotunha = { "کد حساب", "نام حساب", "طبقه حساب", "زیرطبقه حساب" };
                    Display calledDgv = new Display();
                    calledDgv.ShowDgv(dgvOperation, NameSotunha, hesabhaInfoList);
                    dgvOperation = calledDgv.dgvFromDisplay;

                }
                else
                {
                    MessageBoxFarsi.Show("تاریخ را به درستی وارد کنید", "فرمت نادرست تاریخ");

                }
            }
            catch (Exception ex)
            {
                MessageBoxFarsi.Show(ex.Message.ToString(), "خطا در نمایش");
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                int IdHesabHazfi = Convert.ToInt32(dgvOperation.CurrentRow.Cells[0].Value);
                int id = Convert.ToInt32(db.tblRooznamehs.Where(a => a.IdHesab == IdHesabHazfi).Select(a => a.Id).FirstOrDefault());
                var del = db.tblRooznamehs.Where(a => a.Id == id);
                db.tblRooznamehs.RemoveRange(del);
                db.SaveChanges();
                DisplayNewItem(del.Select(a=>a.NameHesab).FirstOrDefault());
                MessageBoxFarsi.Show("حذف حساب با موفقیت انجام شد", "حذف موفقیت‌آمیز");
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
                int EditItem = Convert.ToInt32(dgvOperation.CurrentRow.Cells[0].Value);
                var editList = db.tblRooznamehs.Where(a => a.IdHesab == EditItem).ToList();
                string tarikhShamsi = mskTarikh.Text;
                DateConvert miladi = new DateConvert();
                miladi.ConvertToMiladi(tarikhShamsi);
                DateTime tarikhMiladi = miladi.TarikhMiladi;

                var tabagheyeHesab = cmbGrouheHesab.SelectedItem.ToString();
                var ZirTabgheyeHEsab = cmbDaftarKol.SelectedItem.ToString();
                var ZirGrouh = db.tblRooznamehs.Where(a => a.NameSumGroupHesab == ZirTabgheyeHEsab && a.NameGroupHesab == tabagheyeHesab).Select(a => a.IdHesab).ToList();
                int CodeGrouh = db.tblTabaghatHesabs.Where(a => a.TabagheyehHesab == tabagheyeHesab).Select(a => a.CodeTabaghehHesab).FirstOrDefault();
                int CodeZirGrouh = db.tblTabaghatHesabs.Where(a => a.ZirTzbagheyehHesab == ZirTabgheyeHEsab).Select(a => a.CodeZirtabagheAz).FirstOrDefault();
                int lastIdHesab;
                if (ZirGrouh.Count == 0)
                {
                    lastIdHesab = Convert.ToInt32(CodeGrouh.ToString() + "00" + CodeZirGrouh.ToString());
                }
                else
                {
                    lastIdHesab = Convert.ToInt32(ZirGrouh.Max() + 1);
                }

                editList.ForEach(a => { a.IdHesab = lastIdHesab; a.NameHesab = txtNameHesab.Text; a.NameGroupHesab = cmbGrouheHesab.Text; a.NameSumGroupHesab = cmbDaftarKol.Text; });
                db.SaveChanges();
                DisplayNewItem(txtNameHesab.Text);
                MessageBoxFarsi.Show($"حساب {editList.Select(a => a.NameHesab).FirstOrDefault()} با موفقیت ویرایش شد", "ویرایش موفقیت‌آمیز");
            }
            catch (Exception ex)
            {
                MessageBoxFarsi.Show(ex.Message.ToString(), "خطا در حذف");
            }
        }

        private void dgvOperation_MouseUp(object sender, MouseEventArgs e)
        {
            try
            {
                int EditItem = Convert.ToInt32(dgvOperation.CurrentRow.Cells[0].Value);
                var editList = db.tblRooznamehs.Where(a => a.IdHesab == EditItem).ToList();
                txtCodeHesab.Text = EditItem.ToString();
                txtNameHesab.Text = editList.Select(a => a.NameHesab).FirstOrDefault();
                cmbGrouheHesab.Text = editList.Select(a => a.NameGroupHesab).FirstOrDefault();
                cmbDaftarKol.Text = editList.Select(a => a.NameSumGroupHesab).FirstOrDefault();
                numVol.Value = Convert.ToInt32(editList.Select(a => a.BuySellVol).FirstOrDefault());
                mskTarikh.Text = editList.Select(a => a.SanadTarikhShamsi).FirstOrDefault();
                txtSharh.Text = editList.Select(a => a.SanadTozih).FirstOrDefault();
                btnEdit.Focus();
            }
            catch (Exception ex)
            {
                MessageBoxFarsi.Show(ex.Message.ToString(), "خطا در انتخاب سطر");
            }
        }

        private void txtSearchName_TextChanged(object sender, EventArgs e)
        {
            try
            {
                var hesabhaInfoList = db.tblRooznamehs.Where(a => a.NameHesab.Contains(txtSearchName.Text)).Select(a => new { a.IdHesab, a.NameHesab, a.NameGroupHesab, a.NameSumGroupHesab }).Distinct().OrderBy(a => a.IdHesab).ToList();
                //برای نامگذاری سرستون‌ها در کلاس مربوطه
                string[] NameSotunha = { "کد حساب", "نام حساب", "طبقه حساب", "زیرطبقه حساب" };
                Display calledDgv = new Display();
                calledDgv.ShowDgv(dgvOperation, NameSotunha, hesabhaInfoList);
                dgvOperation = calledDgv.dgvFromDisplay;
            }
            catch (Exception ex)
            {
                MessageBoxFarsi.Show(ex.Message.ToString(), "خطا در نمایش");
            }
        }

        private void numIdHesab_ValueChanged(object sender, EventArgs e)
        {

        }

        private void numIdHesab_KeyPress(object sender, KeyPressEventArgs e)
        {

        }

        private void numIdHesab_KeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                var hesabhaInfoList = db.tblRooznamehs.Where(a => a.IdHesab == numIdHesab.Value).Select(a => new { a.IdHesab, a.NameHesab, a.NameGroupHesab, a.NameSumGroupHesab }).Distinct().OrderBy(a => a.IdHesab).ToList();
                //برای نامگذاری سرستون‌ها در کلاس مربوطه
                string[] NameSotunha = { "کد حساب", "نام حساب", "طبقه حساب", "زیرطبقه حساب" };
                Display calledDgv = new Display();
                calledDgv.ShowDgv(dgvOperation, NameSotunha, hesabhaInfoList);
                dgvOperation = calledDgv.dgvFromDisplay;
            }
            catch (Exception ex)
            {
                MessageBoxFarsi.Show(ex.Message.ToString(), "خطا در نمایش");
            }
        }
    }
}
