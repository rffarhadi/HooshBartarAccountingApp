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
    public partial class frmAfzayeshSarmayeh : Form
    {
        AccountDBEntities db;

        public frmAfzayeshSarmayeh()
        {
            InitializeComponent();
            db = new AccountDBEntities();
        }

        private void frmAfzayeshSarmayeh_Load(object sender, EventArgs e)
        {
            try
            {
                /////////////////////////////
                System.Globalization.PersianCalendar pc = new System.Globalization.PersianCalendar();
                DateTime now = DateTime.Now;
                mskTarikh.Text = pc.GetYear(now).ToString() + pc.GetMonth(now).ToString("0#") + pc.GetDayOfMonth(now).ToString("0#") + pc.GetHour(now).ToString("0#") + pc.GetMinute(now).ToString("0#") + pc.GetSecond(now).ToString("0#");
                ///
                //
                if (rbnVotingRight.Enabled)
                {
                    numPricePazireh.Enabled = true;
                }
                else
                {
                    numPricePazireh.Enabled = false;
                }
                //

                //شروع برای طرف معامله
                cmbMoredMoamele.Items.Clear();
                cmbCodeMoredehMoameleh.Items.Clear();
                var MoredehMoamelehArray = db.tblRooznamehs.Where(a => a.IdHesab >= 1002001 && a.IdHesab <= 1002999 || a.IdHesab >= 10010001 && a.IdHesab <= 10019999).OrderBy(a => a.NameHesab).Select(a => a.NameHesab).Distinct().OrderBy(a=>a).ToArray();
                if (MoredehMoamelehArray != null)
                {
                    foreach (var item in MoredehMoamelehArray)
                    {
                        cmbMoredMoamele.Items.Add(item);
                    }
                }
                var CodeMoredehMoamelehArray = db.tblRooznamehs.Where(a => a.IdHesab >= 1002001 && a.IdHesab <= 1002999 || a.IdHesab >= 10010001 && a.IdHesab <= 10019999).OrderBy(a => a.NameHesab).Select(a => a.IdHesab).Distinct().OrderBy(a => a).ToArray();
                if (CodeMoredehMoamelehArray != null)
                {
                    foreach (var item in CodeMoredehMoamelehArray)
                    {
                        cmbCodeMoredehMoameleh.Items.Add(item);
                    }
                }
                //پایان
                //شروع حساب بانکی
                var NameHesabBankiList = db.tblRooznamehs.Where(a => a.IdHesab >= 1001001 && a.IdHesab <= 1001999).OrderBy(a => a.NameHesab).Select(a => a.NameHesab).Distinct().OrderBy(a=>a).ToArray();
                cmbHesabBanki.Items.Clear();
                if (NameHesabBankiList != null)
                {
                    foreach (var item in NameHesabBankiList)
                    {
                        cmbHesabBanki.Items.Add(item);
                    }
                }
                var codeHesabBankiList = db.tblRooznamehs.Where(a => a.IdHesab >= 1001001 && a.IdHesab <= 1001999).OrderBy(a => a.NameHesab).Select(a => a.IdHesab).Distinct().OrderBy(a => a).ToArray();
                cmbCodeHesabBanki.Items.Clear();
                if (codeHesabBankiList != null)
                {
                    foreach (var item in codeHesabBankiList)
                    {
                        cmbCodeHesabBanki.Items.Add(item);
                    }
                }
                //پایان
            }
            catch (Exception ex)
            {
                MessageBoxFarsi.Show(ex.Message.ToString(), "خطا");
            }
        }



        private void rbnStockDividend_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                numPricePazireh.Enabled = false;
                cmbCodeHesabBanki.Enabled = false;
                cmbHesabBanki.Enabled = false;
                txtSearchCodeBank.Enabled = false;
                txtSearchNameBank.Enabled = false;
            }
            catch (Exception ex)
            {
                MessageBoxFarsi.Show(ex.Message.ToString(), "خطا");
            }
        }

        private void cmbMoredMoamele_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                string nameHesabMored = cmbMoredMoamele.SelectedItem.ToString();
                int codeHesabMored = Convert.ToInt32(db.tblRooznamehs.Where(a => a.NameHesab == nameHesabMored).Select(a => a.IdHesab).Distinct().FirstOrDefault());
                cmbCodeMoredehMoameleh.Text = codeHesabMored.ToString();
            }
            catch (Exception ex)
            {
                MessageBoxFarsi.Show(ex.Message.ToString(), "خطا");
            }
        }

        private void cmbCodeMoredehMoameleh_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                int codeHesabMored = Convert.ToInt32(cmbCodeMoredehMoameleh.SelectedItem);
                string nameHesab = db.tblRooznamehs.Where(a => a.IdHesab == codeHesabMored).Select(a => a.NameHesab).Distinct().FirstOrDefault();
                cmbMoredMoamele.Text = nameHesab;
            }
            catch (Exception ex)
            {
                MessageBoxFarsi.Show(ex.Message.ToString(), "خطا");
            }
        }


        private void btnSabt_Click(object sender, EventArgs e)
        {
            try
            {


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
                    if (LastIdsanadGhablAz.Count == 0)
                    {
                        lastSanad = Convert.ToDouble(LastIdsanadBaedAz.Min()) / 2;
                    }
                    else
                    {
                        lastSanad = Convert.ToDouble(LastIdsanadGhablAz.Min()) / 2;
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

                if (cmbMoredMoamele.SelectedItem ==null)
                {
                    txtSearchMoredMoameleh.ForeColor= Color.Red;
                    MessageBoxFarsi.Show("هیچ مورد معامله‌ای انتخاب نشده است", "عدم انتخاب سهم");
                    return;
                }
                if (cmbCodeMoredehMoameleh.SelectedItem == null)
                {
                    txtSearchCodeMoameleh.ForeColor = Color.Red;
                    MessageBoxFarsi.Show("هیچ مورد معامله‌ای انتخاب نشده است", "عدم انتخاب سهم");
                    return;
                }
                string nameHesabTaraf = cmbMoredMoamele.SelectedItem.ToString();
                long idHesabTaraf = Convert.ToInt64(cmbCodeMoredehMoameleh.SelectedItem);
                string nameGrouhHesab = db.tblRooznamehs.Where(a => a.IdHesab == idHesabTaraf).Select(a => a.NameGroupHesab).FirstOrDefault();
                string nameZirGrouh = db.tblRooznamehs.Where(a => a.IdHesab == idHesabTaraf).Select(a => a.NameSumGroupHesab).FirstOrDefault();

               

                List<tblRooznameh> sanadList = new List<tblRooznameh>();
                if (rbnVotingRight.Checked)
                {
                    if (cmbHesabBanki.SelectedItem == null)
                    {
                        txtSearchNameBank.ForeColor = Color.Red;
                        MessageBoxFarsi.Show("هیچ حساب بانکی انتخاب نشده است", "عدم انتخاب بانک");
                        return;
                    }
                    if (cmbCodeHesabBanki.SelectedItem == null)
                    {
                        txtSearchCodeBank.ForeColor = Color.Red;
                        MessageBoxFarsi.Show("هیچ بانکی انتخاب نشده است", "عدم انتخاب بانک");
                        return;
                    }
                    //حساب بانکی
                    string nameHesabBankiName = cmbHesabBanki.SelectedItem.ToString();
                    long idHesabBanki = Convert.ToInt64(cmbCodeHesabBanki.SelectedItem);
                    var mandehHesabBanki = db.tblRooznamehs.Where(a => a.IdHesab == idHesabBanki).Select(a => a.SanadBedehkar - a.SabadBestankar).Sum();
                    string nameGrouhHesabBanki = db.tblRooznamehs.Where(a => a.IdHesab == idHesabBanki).Select(a => a.NameGroupHesab).FirstOrDefault();
                    string nameZirGrouhHesabBanki = db.tblRooznamehs.Where(a => a.IdHesab == idHesabBanki).Select(a => a.NameSumGroupHesab).FirstOrDefault();

                    var vol = Convert.ToInt64(numVol.Value);
                    if (vol==0)
                    {
                        numVol.BackColor = Color.Yellow;
                        MessageBoxFarsi.Show("تعداد سهام افزایش سرمایه وارد نشده است", "عدم ثبت تعداد");
                        return;
                    }
                    var arzeshMoameleh = Convert.ToInt64(numVol.Value) * Convert.ToInt64(numPricePazireh.Value);
                    tblRooznameh tblSahm = new tblRooznameh()
                    {
                        SanadId = (decimal)lastSanad,
                        SanadTarikhShamsi = tarikhShamsi,
                        SanadTarikhMiladi = tarikhMiladi,
                        IdHesab = idHesabTaraf,
                        NameHesab = nameHesabTaraf,
                        NameGroupHesab = nameGrouhHesab,
                        NameSumGroupHesab = nameZirGrouh,
                        SanadTozih = $"ثبت افزایش سرمایه از محل آورده سهم {nameHesabTaraf} به تعداد {numVol.Value}",
                        BuySellVol = Convert.ToInt32(numVol.Value),
                        SanadBedehkar = arzeshMoameleh,
                        SabadBestankar = 0
                    };
                    sanadList.Add(tblSahm);

                    if (mandehHesabBanki <= 0)
                    {
                        MessageBoxFarsi.Show($"حساب بانکی {nameHesabBankiName} مانده ندارد", "عدم موجودی بانک");
                        return;
                    }
                    tblRooznameh tblhesabBanki = new tblRooznameh()
                    {
                        SanadId = (decimal)lastSanad,
                        SanadTarikhShamsi = tarikhShamsi,
                        SanadTarikhMiladi = tarikhMiladi,
                        IdHesab = idHesabBanki,
                        NameHesab = nameHesabBankiName,
                        NameGroupHesab = nameGrouhHesabBanki,
                        NameSumGroupHesab = nameZirGrouhHesabBanki,
                        SanadTozih = $"ثبت افزایش سرمایه از محل آورده سهم {nameHesabTaraf} به تعداد {numVol.Value}",
                        BuySellVol = 0,
                        SanadBedehkar = 0,
                        SabadBestankar = arzeshMoameleh
                    };
                    sanadList.Add(tblhesabBanki);
                }
                else if (rbnStockDividend.Checked)
                {
                    tblRooznameh tblSahm = new tblRooznameh()
                    {
                        SanadId = (decimal)lastSanad,
                        SanadTarikhShamsi = tarikhShamsi,
                        SanadTarikhMiladi = tarikhMiladi,
                        IdHesab = idHesabTaraf,
                        NameHesab = nameHesabTaraf,
                        NameGroupHesab = nameGrouhHesab,
                        NameSumGroupHesab = nameZirGrouh,
                        SanadTozih = $"ثبت افزایش سرمایه (سهام جایزه) سهم {nameHesabTaraf} به تعداد {numVol.Value}",
                        BuySellVol = Convert.ToInt32(numVol.Value),
                        SanadBedehkar = 0,
                        SabadBestankar = 0
                    };
                    sanadList.Add(tblSahm);
                    tblRooznameh tblSahm2 = new tblRooznameh()
                    {
                        SanadId = (decimal)lastSanad,
                        SanadTarikhShamsi = tarikhShamsi,
                        SanadTarikhMiladi = tarikhMiladi,
                        IdHesab = idHesabTaraf,
                        NameHesab = nameHesabTaraf,
                        NameGroupHesab = nameGrouhHesab,
                        NameSumGroupHesab = nameZirGrouh,
                        SanadTozih = $"ثبت افزایش سرمایه (سهام جایزه) سهم {nameHesabTaraf} به تعداد {numVol.Value}",
                        BuySellVol = 0,
                        SanadBedehkar = 0,
                        SabadBestankar = 0
                    };
                    sanadList.Add(tblSahm2);
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
                string message = "";
                if (rbnStockDividend.Checked)
                {
                     message = "افزایش سرمایه از محل سود انباشته (سهام جایزه)";
                }
                else if (rbnVotingRight.Checked)
                {
                    message = "افزایش سرمایه از محل آورده";
                }
                MessageBoxFarsi.Show($"{message} {nameHesabTaraf} با موفقیت ثبت شد", "ثبت موفق");
            }
            catch (Exception ex)
            {
                MessageBoxFarsi.Show(ex.Message.ToString(), "خطا در ثبت سند");
            }
        }

        private void cmbHesabBanki_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                string nameHesabBank = cmbHesabBanki.SelectedItem.ToString();
                int codeHesabBank = Convert.ToInt32(db.tblRooznamehs.Where(a => a.NameHesab == nameHesabBank).Select(a => a.IdHesab).Distinct().FirstOrDefault());
                cmbCodeHesabBanki.Text = codeHesabBank.ToString();
            }
            catch (Exception ex)
            {
                MessageBoxFarsi.Show(ex.Message.ToString(), "خطا");
            }
        }

        private void cmbCodeHesabBanki_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                int codeHesabBanki = Convert.ToInt32(cmbCodeHesabBanki.SelectedItem);
                string nameHesab = db.tblRooznamehs.Where(a => a.IdHesab == codeHesabBanki).Select(a => a.NameHesab).Distinct().FirstOrDefault();
                cmbHesabBanki.SelectedItem = nameHesab;
            }
            catch (Exception ex)
            {
                MessageBoxFarsi.Show(ex.Message.ToString(), "خطا");
            }
        }

        private void txtSearchMoredMoameleh_TextChanged(object sender, EventArgs e)
        {
            try
            {
                string NameHesab = db.tblRooznamehs.Where(a => a.IdHesab >= 1002001 && a.IdHesab <= 1002999 || a.IdHesab >= 10010001 && a.IdHesab <= 10019999).Where(a => a.NameHesab.Contains(txtSearchMoredMoameleh.Text)).Select(a => a.NameHesab).Distinct().FirstOrDefault();
                if (NameHesab == null)
                {
                    MessageBoxFarsi.Show("حسابی با این مشخصات وجود ندارد");
                }
                else
                {
                    cmbMoredMoamele.Text = NameHesab;
                }
            }
            catch (Exception ex)
            {
                MessageBoxFarsi.Show(ex.Message.ToString(), "خطا در جستجو");
            }
        }

        private void txtSearchMoredMoameleh_MouseClick(object sender, MouseEventArgs e)
        {
            try
            {
                txtSearchMoredMoameleh.Text = "";

            }
            catch (Exception ex)
            {
                MessageBoxFarsi.Show(ex.Message.ToString(), "خطا");
            }
        }

        private void txtSearchCodeMoameleh_TextChanged(object sender, EventArgs e)
        {
            try
            {
                string mySelectedCode = txtSearchCodeMoameleh.Text;
                int idHesab = (int)db.tblRooznamehs.Where(a => a.IdHesab.ToString().Contains(mySelectedCode)).Select(a => a.IdHesab).Distinct().OrderBy(a => a).FirstOrDefault();
                if (idHesab == 0)
                {
                    MessageBoxFarsi.Show("حسابی با این مشخصات وجود ندارد");
                }
                else
                {
                    cmbCodeMoredehMoameleh.Text = idHesab.ToString();
                }
            }
            catch (Exception ex)
            {
                MessageBoxFarsi.Show(ex.Message.ToString(), "خطا در جستجو");
            }
        }

        private void txtSearchCodeBank_MouseClick(object sender, MouseEventArgs e)
        {
            try
            {
                txtSearchCodeBank.Text = "";

            }
            catch (Exception ex)
            {
                MessageBoxFarsi.Show(ex.Message.ToString(), "خطا");
            }
        }

        private void txtSearchNameBank_MouseClick(object sender, MouseEventArgs e)
        {
            try
            {
                txtSearchNameBank.Text = "";

            }
            catch (Exception ex)
            {
                MessageBoxFarsi.Show(ex.Message.ToString(), "خطا");
            }
        }

        private void txtSearchNameBank_TextChanged(object sender, EventArgs e)
        {
            try
            {
                string NameHesabBanki = db.tblRooznamehs.Where(a => a.IdHesab >= 1001001 && a.IdHesab <= 1001999).Where(a => a.NameHesab.Contains(txtSearchNameBank.Text)).Select(a => a.NameHesab).Distinct().FirstOrDefault();
                if (NameHesabBanki == null)
                {
                    MessageBoxFarsi.Show("حساب بانکی با این مشخصات وجود ندارد");
                }
                else
                {
                    cmbHesabBanki.Text = NameHesabBanki;
                }
            }
            catch (Exception ex)
            {
                MessageBoxFarsi.Show(ex.Message.ToString(), "خطا در جستجو");
            }
        }

        private void txtSearchCodeBank_TextChanged(object sender, EventArgs e)
        {
            try
            {
                string mySelectedCode = txtSearchCodeBank.Text;
                int idHesab = (int)db.tblRooznamehs.Where(a => a.IdHesab.ToString().Contains(mySelectedCode)).Select(a => a.IdHesab).Distinct().OrderBy(a => a).FirstOrDefault();
                if (idHesab == 0)
                {
                    MessageBoxFarsi.Show("حسابی با این مشخصات وجود ندارد");
                }
                else
                {
                    cmbCodeHesabBanki.Text = idHesab.ToString();
                }
            }
            catch (Exception ex)
            {
                MessageBoxFarsi.Show(ex.Message.ToString(), "خطا در جستجو");
            }
        }

        private void txtSearchCodeMoameleh_MouseClick(object sender, MouseEventArgs e)
        {
            try
            {
                txtSearchCodeMoameleh.Text = "";

            }
            catch (Exception ex)
            {
                MessageBoxFarsi.Show(ex.Message.ToString(), "خطا");
            }
        }

        private void rbnVotingRight_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                numPricePazireh.Enabled = true;
                cmbCodeHesabBanki.Enabled = true;
                cmbHesabBanki.Enabled = true;
                txtSearchCodeBank.Enabled = true;
                txtSearchNameBank.Enabled = true;
            }
            catch (Exception ex)
            {
                MessageBoxFarsi.Show(ex.Message.ToString(), "خطا");
            }
        }
    }
}
