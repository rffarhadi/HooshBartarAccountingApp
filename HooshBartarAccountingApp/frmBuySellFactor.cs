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
using BehComponents;

namespace HooshBartarAccountingApp
{
    public partial class frmBuySellFactor : Form
    {
        public frmBuySellFactor()
        {
            InitializeComponent();
        }
        AccountDBEntities db = new AccountDBEntities();
        public List<tblRooznameh> TblRooznamehsList { get; set; }
        private void frmBuySellFactor_Load(object sender, EventArgs e)
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

                var SettingList = db.tblSettings.ToList();
                if (SettingList != null)
                {
                    numAfzoodehTaxRate.Value = (decimal)SettingList.Select(a => a.ArzeshAfzoodehTaxRate).FirstOrDefault();
                    if (chkBuyFactor.Checked)
                    {
                        numKarmozdRate.Value = (decimal)SettingList.Select(a => a.BuyKarmozdRate).FirstOrDefault();
                        numEnteghalTaxRate.Value = 0;
                    }
                    if (chkSellFactor.Checked)
                    {
                        numKarmozdRate.Value = (decimal)SettingList.Select(a => a.SellKarmozdRate).FirstOrDefault();
                        numEnteghalTaxRate.Value = (decimal)SettingList.Select(a => a.NaghloEnteghalTaxRate).FirstOrDefault();
                    }
                }
                if (chkBuyFactor.Checked)
                {
                    cmbMoredMoamele.Items.Clear();
                    cmbCodeMoredehMoameleh.Items.Clear();
                    var MoredehMoamelehArray = db.tblRooznamehs.Where(a => a.IdHesab >= 1002001 && a.IdHesab <= 1002999 || a.IdHesab >= 1005001 && a.IdHesab <= 1005999 || a.IdHesab >= 10010001 && a.IdHesab <= 10019999).OrderBy(a => a.NameHesab).Select(a => a.NameHesab).Distinct().OrderBy(a => a).ToArray();
                    if (MoredehMoamelehArray != null)
                    {
                        foreach (var item in MoredehMoamelehArray)
                        {
                            cmbMoredMoamele.Items.Add(item);
                        }
                    }
                    var CodeMoredehMoamelehArray = db.tblRooznamehs.Where(a => a.IdHesab >= 1002001 && a.IdHesab <= 1002999 || a.IdHesab >= 1005001 && a.IdHesab <= 1005999 || a.IdHesab >= 10010001 && a.IdHesab <= 10019999).OrderBy(a => a.NameHesab).Select(a => a.IdHesab).Distinct().OrderBy(a => a).ToArray();
                    if (CodeMoredehMoamelehArray != null)
                    {
                        foreach (var item in CodeMoredehMoamelehArray)
                        {
                            cmbCodeMoredehMoameleh.Items.Add(item);
                        }
                    }
                }
                else if (chkSellFactor.Checked)
                {
                    cmbMoredMoamele.Items.Clear();
                    cmbCodeMoredehMoameleh.Items.Clear();
                    var MoredehMoamelehList = db.tblRooznamehs.Where(a => a.IdHesab >= 1002001 && a.IdHesab <= 1002999 || a.IdHesab >= 1005001 && a.IdHesab <= 1005999 || a.IdHesab >= 10010001 && a.IdHesab <= 10019999).Where(a => a.BuySellVol > 0).OrderBy(a => a.NameHesab).Select(a => a.NameHesab).OrderBy(a => a).Distinct().ToArray();
                    if (MoredehMoamelehList != null)
                    {
                        foreach (var item in MoredehMoamelehList)
                        {
                            cmbMoredMoamele.Items.Add(item);
                        }
                    }
                    var CodeMoredehMoamelehList = db.tblRooznamehs.Where(a => a.IdHesab >= 1002001 && a.IdHesab <= 1002999 || a.IdHesab >= 1005001 && a.IdHesab <= 1005999 || a.IdHesab >= 10010001 && a.IdHesab <= 10019999).Where(a => a.BuySellVol > 0).OrderBy(a => a.NameHesab).Select(a => a.IdHesab).OrderBy(a => a).Distinct().ToArray();
                    if (CodeMoredehMoamelehList != null)
                    {
                        foreach (var item in CodeMoredehMoamelehList)
                        {
                            cmbCodeMoredehMoameleh.Items.Add(item);
                        }
                    }
                }
                if (chkBuyFactor.Checked)
                {
                    cmbTarafMoameleh.Items.Clear();
                    cmbCodeTaraf.Items.Clear();
                    var NameTarafList = db.tblRooznamehs.Where(a => a.IdHesab >= 2001001 && a.IdHesab <= 2001999).OrderBy(a => a.NameHesab).Select(a => a.NameHesab).Distinct().OrderBy(a => a).ToArray();
                    if (NameTarafList != null)
                    {
                        foreach (var item in NameTarafList)
                        {
                            cmbTarafMoameleh.Items.Add(item);
                        }
                    }
                    var CodeTarafList = db.tblRooznamehs.Where(a => a.IdHesab >= 2001001 && a.IdHesab <= 2001999).OrderBy(a => a.NameHesab).Select(a => a.IdHesab).Distinct().OrderBy(a => a).ToArray();
                    if (CodeTarafList != null)
                    {
                        foreach (var item in CodeTarafList)
                        {
                            cmbCodeTaraf.Items.Add(item);
                        }
                    }
                }
                if (chkSellFactor.Checked)
                {
                    cmbTarafMoameleh.Items.Clear();
                    cmbCodeTaraf.Items.Clear();
                    var NameTarafList = db.tblRooznamehs.Where(a => a.IdHesab >= 1003001 && a.IdHesab <= 1003999).OrderBy(a => a.SanadTarikhMiladi).Select(a => a.NameHesab).Distinct().OrderBy(a => a).ToArray();
                    if (NameTarafList != null)
                    {
                        foreach (var item in NameTarafList)
                        {
                            cmbTarafMoameleh.Items.Add(item);
                        }
                    }
                    var CodeTarafList = db.tblRooznamehs.Where(a => a.IdHesab >= 1003001 && a.IdHesab <= 1003999).OrderBy(a => a.NameHesab).Select(a => a.IdHesab).Distinct().ToArray();
                    if (CodeTarafList != null)
                    {
                        foreach (var item in CodeTarafList)
                        {
                            cmbCodeTaraf.Items.Add(item);
                        }
                    }
                } ///////////
                var NameHesabBankiList = db.tblRooznamehs.Where(a => a.IdHesab >= 1001001 && a.IdHesab <= 1001999).OrderBy(a => a.NameHesab).Select(a => a.NameHesab).Distinct().OrderBy(a => a).ToArray();
                cmbHesabBanki.Items.Clear();
                if (NameHesabBankiList != null)
                {
                    foreach (var item in NameHesabBankiList)
                    {
                        cmbHesabBanki.Items.Add(item);
                    }
                    if (cmbHesabBanki.Items.Count != 0)
                    {
                        cmbHesabBanki.SelectedIndex = 0;

                    }
                }
                var CodeHesabBankiList = db.tblRooznamehs.Where(a => a.IdHesab >= 1001001 && a.IdHesab <= 1001999).OrderBy(a => a.NameHesab).Select(a => a.IdHesab).Distinct().OrderBy(a => a).ToArray();
                cmbCodeHesabBanki.Items.Clear();
                if (CodeHesabBankiList != null)
                {
                    foreach (var item in CodeHesabBankiList)
                    {
                        cmbCodeHesabBanki.Items.Add(item);
                    }
                    if (cmbCodeHesabBanki.Items.Count != 0)
                    {
                        cmbCodeHesabBanki.SelectedIndex = 0;
                    }
                }
                cmbReport.SelectedIndex = 0;
                cmbNoeTasviyeh.SelectedIndex = 1;
                btnDisplay_Click(sender, e);
            }
            catch (Exception ex)
            {
                MessageBoxFarsi.Show(ex.Message.ToString(), "خطا");
            }
        }

        private void rbnSalaf_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void btnInsert_Click(object sender, EventArgs e)
        {
            try
            {
                if (cmbMoredMoamele.Text == "" || cmbTarafMoameleh.Text == "" || cmbCodeTaraf.Text == "" || cmbCodeMoredehMoameleh.Text == "")
                {
                    MessageBoxFarsi.Show(" شخص طرف معامله یا حساب مورد معامله انتخاب نشده است", "خطا در تنظیم سند");
                    return;
                }
                if (numVol.Value <= 0 && numPrice.Value <= 0)
                {
                    MessageBoxFarsi.Show("حجم معامله یا قیمت معامله نمی‌تواند صفر یا کوچکتر از صفر باشد", "خطا در تنظیم سند");
                    return;
                }
                Int64 ArzeshAfzoodehTax = Convert.ToInt64(Convert.ToInt64(numVol.Value) * Convert.ToInt64(numPrice.Value) * Convert.ToDouble(numAfzoodehTaxRate.Value));
                Int64 ArzeshKolMoameleh = 0;
                Int64 EnteghalTax = 0;
                Int64 Karmozd = 0;


                if (chkBuyFactor.Checked)
                {
                    Karmozd = Convert.ToInt64(Convert.ToInt64(numVol.Value) * Convert.ToInt64(numPrice.Value) * Convert.ToDouble(numKarmozdRate.Value));
                    ArzeshKolMoameleh = Convert.ToInt64(numVol.Value) * Convert.ToInt64(numPrice.Value) + ArzeshAfzoodehTax + Karmozd;
                    if (cmbNoeTasviyeh.SelectedIndex == 0)//معامله نقد
                    {
                        dgvSanad.Rows.Add(cmbCodeMoredehMoameleh.Text, cmbMoredMoamele.Text, ArzeshAfzoodehTax, EnteghalTax, Karmozd, numVol.Value, txtSharh.Text, ArzeshKolMoameleh, 0, mskTarikh.Text);
                        dgvSanad.Rows.Add(cmbCodeTaraf.Text, cmbTarafMoameleh.Text, 0, 0, 0, 0, txtSharh.Text, 0, ArzeshKolMoameleh, mskTarikh.Text);
                        dgvSanad.Rows.Add(cmbCodeTaraf.Text, cmbTarafMoameleh.Text, 0, 0, 0, 0, txtSharh.Text, ArzeshKolMoameleh, 0, mskTarikh.Text);
                        dgvSanad.Rows.Add(cmbCodeHesabBanki.Text, cmbHesabBanki.Text, 0, 0, 0, 0, txtSharh.Text, 0, ArzeshKolMoameleh, mskTarikh.Text);
                    }
                    else if (cmbNoeTasviyeh.SelectedIndex == 1)//معامله نسیه
                    {
                        dgvSanad.Rows.Add(cmbCodeMoredehMoameleh.Text, cmbMoredMoamele.Text, ArzeshAfzoodehTax, EnteghalTax, Karmozd, numVol.Value, txtSharh.Text, ArzeshKolMoameleh, 0, mskTarikh.Text);
                        dgvSanad.Rows.Add(cmbCodeTaraf.Text, cmbTarafMoameleh.Text, 0, 0, 0, 0, txtSharh.Text, 0, ArzeshKolMoameleh, mskTarikh.Text);
                    }
                    else if (cmbNoeTasviyeh.SelectedIndex == 2)//معامله نقد و نسیه
                    {
                        if (Convert.ToDouble(numBakhshPardakhti.Value) >= ArzeshKolMoameleh || Convert.ToDouble(numBakhshPardakhti.Value) == 0)
                        {
                            MessageBoxFarsi.Show("شرایط پرداخت با نوع قرارداد نقد/نسیه مطابقت ندارد، قرارداد را عوض نمایید یا بخشی از مبلغ پرداختی را وارد کنید");
                            numBakhshPardakhti.BackColor = Color.Yellow;
                            return;
                        }
                        dgvSanad.Rows.Add(cmbCodeMoredehMoameleh.Text, cmbMoredMoamele.Text, ArzeshAfzoodehTax, EnteghalTax, Karmozd, numVol.Value, txtSharh.Text, ArzeshKolMoameleh, 0, mskTarikh.Text);
                        dgvSanad.Rows.Add(cmbCodeTaraf.Text, cmbTarafMoameleh.Text, 0, 0, 0, 0, txtSharh.Text, 0, ArzeshKolMoameleh - Convert.ToDouble(numBakhshPardakhti.Value), mskTarikh.Text);
                        dgvSanad.Rows.Add(cmbCodeHesabBanki.Text, cmbHesabBanki.Text, 0, 0, 0, 0, txtSharh.Text, 0, Convert.ToDouble(numBakhshPardakhti.Value), mskTarikh.Text);
                    }

                }
                if (chkSellFactor.Checked)
                {
                    Int64 id = Convert.ToInt64(cmbCodeMoredehMoameleh.Text);
                    Int64? volMojud = db.tblRooznamehs.Where(a => a.IdHesab == id).Select(a => a.BuySellVol).Sum();
                    if (Convert.ToInt64(numVol.Value) > volMojud)
                    {
                        MessageBoxFarsi.Show($"حداکثر موجودی مورد معامله {volMojud} می‌باشد، تعداد فروش را کاهش دهید", "خطاء موجودی");
                        return;
                    }
                    var NamehesabForush = db.tblRooznamehs.Where(a => a.IdHesab == 4001001).Select(a => a.NameHesab).FirstOrDefault();
                    var CodeHesabForush = db.tblRooznamehs.Where(a => a.IdHesab == 4001001).Select(a => a.IdHesab).FirstOrDefault();
                    var NamehesabBahayeTamam = db.tblRooznamehs.Where(a => a.IdHesab == 5001001).Select(a => a.NameHesab).FirstOrDefault();
                    var CodeHesabBahayeTamam = db.tblRooznamehs.Where(a => a.IdHesab == 5001001).Select(a => a.IdHesab).FirstOrDefault();
                    Karmozd = Convert.ToInt64(Convert.ToInt64(numVol.Value) * Convert.ToInt64(numPrice.Value) * Convert.ToDouble(numKarmozdRate.Value));
                    EnteghalTax = Convert.ToInt64(Convert.ToInt64(numVol.Value) * Convert.ToInt64(numPrice.Value) * Convert.ToDouble(numEnteghalTaxRate.Value));

                    ArzeshKolMoameleh = Convert.ToInt64(numVol.Value) * Convert.ToInt64(numPrice.Value) + ArzeshAfzoodehTax - EnteghalTax - Karmozd;
                    Int64? avgArzeshKharid =0;
                    if (Convert.ToInt64(numVol.Value) == volMojud)
                    {
                        long? sumHesab = db.tblRooznamehs.Where(a => a.IdHesab == id).Select(a => a.SanadBedehkar - a.SabadBestankar).Sum();
                        avgArzeshKharid = sumHesab;
                    }
                    else if(Convert.ToInt64(numVol.Value) < volMojud)
                    {
                        avgArzeshKharid = Convert.ToInt64(Convert.ToDouble(txtAvgKharid.Text) * Convert.ToInt64(numVol.Value));
                    }
                    if (cmbNoeTasviyeh.SelectedIndex == 0)//معامله نقد
                    {
                        dgvSanad.Rows.Add(cmbCodeMoredehMoameleh.Text, cmbMoredMoamele.Text, ArzeshAfzoodehTax, EnteghalTax, Karmozd, numVol.Value, txtSharh.Text, 0, avgArzeshKharid);
                        dgvSanad.Rows.Add(cmbCodeTaraf.Text, cmbTarafMoameleh.Text, 0, 0, 0, 0, txtSharh.Text, ArzeshKolMoameleh, 0, mskTarikh.Text);
                        dgvSanad.Rows.Add(cmbCodeTaraf.Text, cmbTarafMoameleh.Text, 0, 0, 0, 0, txtSharh.Text, 0, ArzeshKolMoameleh, mskTarikh.Text);
                        dgvSanad.Rows.Add(cmbCodeHesabBanki.Text, cmbHesabBanki.Text, 0, 0, 0, 0, txtSharh.Text, ArzeshKolMoameleh, 0, mskTarikh.Text);
                        dgvSanad.Rows.Add(CodeHesabForush, NamehesabForush, 0, 0, 0, 0, txtSharh.Text, 0, ArzeshKolMoameleh, mskTarikh.Text);
                        dgvSanad.Rows.Add(CodeHesabBahayeTamam, NamehesabBahayeTamam, 0, 0, 0, 0, txtSharh.Text, avgArzeshKharid, 0, mskTarikh.Text);
                    }
                    else if (cmbNoeTasviyeh.SelectedIndex == 1)//معامله نسیه
                    {
                        dgvSanad.Rows.Add(cmbCodeMoredehMoameleh.Text, cmbMoredMoamele.Text, ArzeshAfzoodehTax, EnteghalTax, Karmozd, numVol.Value, txtSharh.Text, 0, avgArzeshKharid, mskTarikh.Text);
                        dgvSanad.Rows.Add(cmbCodeTaraf.Text, cmbTarafMoameleh.Text, 0, 0, 0, 0, txtSharh.Text, ArzeshKolMoameleh, 0, mskTarikh.Text);
                        dgvSanad.Rows.Add(CodeHesabForush, NamehesabForush, 0, 0, 0, 0, txtSharh.Text, 0, ArzeshKolMoameleh, mskTarikh.Text);
                        dgvSanad.Rows.Add(CodeHesabBahayeTamam, NamehesabBahayeTamam, 0, 0, 0, 0, txtSharh.Text, avgArzeshKharid, 0, mskTarikh.Text);
                    }
                    else if (cmbNoeTasviyeh.SelectedIndex == 2)//معامله نقد و نسیه
                    {
                        if (Convert.ToDouble(numBakhshPardakhti.Value) >= ArzeshKolMoameleh || Convert.ToDouble(numBakhshPardakhti.Value) == 0)
                        {
                            MessageBoxFarsi.Show("شرایط پرداخت با نوع قرارداد نقد/نسیه مطابقت ندارد، قرارداد را عوض نمایید یا بخشی از مبلغ پرداختی را وارد کنید");
                            numBakhshPardakhti.BackColor = Color.Yellow;
                            return;
                        }
                        dgvSanad.Rows.Add(cmbCodeMoredehMoameleh.Text, cmbMoredMoamele.Text, ArzeshAfzoodehTax, EnteghalTax, Karmozd, numVol.Value, txtSharh.Text, 0, avgArzeshKharid, mskTarikh.Text);
                        dgvSanad.Rows.Add(cmbCodeTaraf.Text, cmbTarafMoameleh.Text, 0, 0, 0, 0, txtSharh.Text, ArzeshKolMoameleh - Convert.ToDouble(numBakhshPardakhti.Value), 0, mskTarikh.Text);
                        dgvSanad.Rows.Add(cmbCodeHesabBanki.Text, cmbHesabBanki.Text, 0, 0, 0, 0, txtSharh.Text, Convert.ToDouble(numBakhshPardakhti.Value), 0, mskTarikh.Text);
                        dgvSanad.Rows.Add(CodeHesabForush, NamehesabForush, 0, 0, 0, 0, txtSharh.Text, 0, ArzeshKolMoameleh, mskTarikh.Text);
                        dgvSanad.Rows.Add(CodeHesabBahayeTamam, NamehesabBahayeTamam, 0, 0, 0, 0, txtSharh.Text, avgArzeshKharid, 0, mskTarikh.Text);
                    }
                }
                dgvSanad.Columns[0].DefaultCellStyle.Format = "#,0.###";
                dgvSanad.Columns[2].DefaultCellStyle.Format = "#,0.###";
                dgvSanad.Columns[3].DefaultCellStyle.Format = "#,0.###";
                dgvSanad.Columns[4].DefaultCellStyle.Format = "#,0.###";
                dgvSanad.Columns[5].DefaultCellStyle.Format = "#,0.###";
                dgvSanad.Columns[6].DefaultCellStyle.Format = "#,0.###";
                dgvSanad.Columns[7].DefaultCellStyle.Format = "#,0.###";
                dgvSanad.Columns[8].DefaultCellStyle.Format = "#,0.###";
                numVol.Value = 0;
                numPrice.Value = 0;
            }
            catch (Exception ex)
            {
                MessageBoxFarsi.Show(ex.Message.ToString(), "خطا در تنظیم سند");
            }
        }

        private void cmbMoredMoamele_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                string nameHesabMored = cmbMoredMoamele.SelectedItem.ToString();
                int codeHesabMored = Convert.ToInt32(db.tblRooznamehs.Where(a => a.NameHesab == nameHesabMored).Select(a => a.IdHesab).Distinct().OrderBy(a => a).FirstOrDefault());
                cmbCodeMoredehMoameleh.Text = codeHesabMored.ToString();
                int? vol = db.tblRooznamehs.Where(a => a.IdHesab == codeHesabMored).Select(a => a.BuySellVol).Sum();
                if (chkSellFactor.Checked && vol == 0 && vol == null)
                {
                    MessageBoxFarsi.Show($"از مورد معامله {nameHesabMored} با کد حساب {codeHesabMored} هیچ کالایی جهت فروش وجود ندارد", "نبود موجودی");
                    btnInsert.Enabled = false;
                    txtAvgKharid.Text = "";
                }
                else if (chkSellFactor.Checked && vol > 0)
                {
                    long? sumHesab = db.tblRooznamehs.Where(a => a.IdHesab == codeHesabMored).Select(a => a.SanadBedehkar - a.SabadBestankar).Sum();
                    double avg = Convert.ToDouble(sumHesab / vol);
                    txtAvgKharid.Text = avg.ToString();
                    long? sumVol = db.tblRooznamehs.Where(a => a.IdHesab == codeHesabMored).Select(a => a.BuySellVol).Sum();
                    txtMojudi.Text = sumVol.ToString();
                    btnInsert.Enabled = true;
                }
                else if (chkBuyFactor.Checked)
                {
                    txtAvgKharid.Text = "0";
                    btnInsert.Enabled = true;
                }
            }
            catch (Exception ex)
            {
                MessageBoxFarsi.Show(ex.Message.ToString(), "خطا");
            }
        }

        private void cmbTarafMoameleh_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (chkBuyFactor.Checked)
                {
                    string nameHesabTaraf = cmbTarafMoameleh.SelectedItem.ToString();
                    int codeHesabTaraf = Convert.ToInt32(db.tblRooznamehs.Where(a => a.NameHesab == nameHesabTaraf && a.IdHesab >= 2001001 && a.IdHesab <= 2001999).Select(a => a.IdHesab).Distinct().OrderBy(a => a).FirstOrDefault());
                    cmbCodeTaraf.Text = codeHesabTaraf.ToString();
                }
                if (chkSellFactor.Checked)
                {
                    string nameHesabTaraf = cmbTarafMoameleh.SelectedItem.ToString();
                    int codeHesabTaraf = Convert.ToInt32(db.tblRooznamehs.Where(a => a.NameHesab == nameHesabTaraf && a.IdHesab >= 1003001 && a.IdHesab <= 1003999).Select(a => a.IdHesab).Distinct().FirstOrDefault());
                    cmbCodeTaraf.Text = codeHesabTaraf.ToString();
                }

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
                string NameHesab = db.tblRooznamehs.Where(a => a.IdHesab >= 1002001 && a.IdHesab <= 1002999 || a.IdHesab >= 1005001 && a.IdHesab <= 1005999 || a.IdHesab >= 10010001 && a.IdHesab <= 10019999).Where(a => a.NameHesab.Contains(txtSearchMoredMoameleh.Text)).Select(a => a.NameHesab).Distinct().OrderBy(a => a).FirstOrDefault();
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

        private void txtSearchTarafMoameleh_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (chkBuyFactor.Checked)
                {
                    var NameHesab = db.tblRooznamehs.Where(a => a.IdHesab >= 2001001 && a.IdHesab <= 2001999).OrderBy(a => a.SanadTarikhMiladi).Where(a => a.NameHesab.Contains(txtSearchTarafMoameleh.Text)).Select(a => a.NameHesab).Distinct().OrderBy(a => a).FirstOrDefault();
                    if (NameHesab == null)
                    {
                        MessageBoxFarsi.Show("حسابی با این مشخصات وجود ندارد");
                    }
                    else
                    {
                        cmbTarafMoameleh.Text = NameHesab;
                    }
                }
                else if (chkSellFactor.Checked)
                {
                    var NameHesab = db.tblRooznamehs.Where(a => a.IdHesab >= 1003001 && a.IdHesab <= 1003999).OrderBy(a => a.SanadTarikhMiladi).Where(a => a.NameHesab.Contains(txtSearchTarafMoameleh.Text)).Select(a => a.NameHesab).Distinct().OrderBy(a => a).FirstOrDefault();
                    if (NameHesab == null)
                    {
                        MessageBoxFarsi.Show("حسابی با این مشخصات وجود ندارد");
                    }
                    else
                    {
                        cmbTarafMoameleh.Text = NameHesab;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBoxFarsi.Show(ex.Message.ToString(), "خطا در جستجو");
            }
        }

        private void txtSearchTarafMoameleh_MouseClick(object sender, MouseEventArgs e)
        {
            try
            {
                txtSearchTarafMoameleh.Text = "";
            }
            catch (Exception ex)
            {
                MessageBoxFarsi.Show(ex.Message.ToString(), "خطا");
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
        public void SabtExcell(DataGridView dgv)
        {
            dgvSanad = dgv;
            try
            {
                if (dgvSanad.Rows.Count == 0 || dgvSanad.Rows == null)
                {
                    MessageBoxFarsi.Show("ابتدا باید سند را تنظیم نمایید", "تکمیل اطلاعات سند");
                    return;
                }
                List<DateTime> dateTimesList = new List<DateTime>();
                DateTime tarikhMiladi = new DateTime();
                DateConvert dtMiladi = new DateConvert();
                foreach (DataGridViewRow row in dgvSanad.Rows)
                {
                    var rowTarikhShamsi = row.Cells[9].Value.ToString();
                    var result = dtMiladi.ConvertToMiladi(rowTarikhShamsi);
                    if (result == true)
                    {
                        tarikhMiladi = dtMiladi.TarikhMiladi;
                    }
                    if (result == false)
                    {
                        dtMiladi.ShortConvertToMiladi(rowTarikhShamsi);
                        tarikhMiladi = dtMiladi.TarikhMiladi;
                    }
                    dateTimesList.Add(tarikhMiladi);
                }

                DateTime MaxtarikhMiladi = dateTimesList.Max();
                DateTime MintarikhMiladi = dateTimesList.Min();

                double lastSanad = 0;
                var LastIdsanadGhablAz = db.tblRooznamehs.Where(a => a.SanadTarikhMiladi < MaxtarikhMiladi && a.SanadId > 0).Select(a => a.SanadId).ToList();
                var LastIdsanadBaedAz = db.tblRooznamehs.Where(a => a.SanadTarikhMiladi > MaxtarikhMiladi && a.SanadId > 0).Select(a => a.SanadId).ToList();
                if (LastIdsanadGhablAz.Count == 0 && LastIdsanadBaedAz.Count == 0)
                {
                    lastSanad = 1;
                }
                else if (LastIdsanadGhablAz.Count == 0 && LastIdsanadBaedAz.Count > 0)
                {
                    LastIdsanadGhablAz = db.tblRooznamehs.Where(a => a.SanadTarikhMiladi <= MaxtarikhMiladi && a.SanadId > 0).Select(a => a.SanadId).ToList();
                    LastIdsanadBaedAz = db.tblRooznamehs.Where(a => a.SanadTarikhMiladi >= MaxtarikhMiladi && a.SanadId > 0).Select(a => a.SanadId).ToList();
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
                    LastIdsanadGhablAz = db.tblRooznamehs.Where(a => a.SanadTarikhMiladi <= MaxtarikhMiladi && a.SanadId > 0).Select(a => a.SanadId).ToList();
                    lastSanad = Convert.ToInt64(LastIdsanadGhablAz.Max() + 1);
                }
                else if (LastIdsanadGhablAz.Count > 0 && LastIdsanadBaedAz.Count > 0)
                {
                    var MyBeforMaxId = db.tblRooznamehs.Where(a => a.SanadTarikhMiladi < tarikhMiladi && a.SanadId > 0).Select(a => a.SanadId).Max();
                    var MyAfterMinId = db.tblRooznamehs.Where(a => a.SanadTarikhMiladi >= tarikhMiladi && a.SanadId > 0).Select(a => a.SanadId).Min();
                    lastSanad = Convert.ToDouble((MyBeforMaxId + MyAfterMinId) / 2);
                }

                List<tblRooznameh> sanadList = new List<tblRooznameh>();
                DateConvert dt = new DateConvert();

                foreach (DataGridViewRow row in dgvSanad.Rows)
                {
                    dt.ShortConvertToMiladi(row.Cells[9].Value.ToString());
                    var Miladi = dt.TarikhMiladi;

                    long idHesab = Convert.ToInt64(row.Cells[0].Value);
                    string nameGrouhHesab = db.tblRooznamehs.Where(a => a.IdHesab == idHesab).Select(a => a.NameGroupHesab).FirstOrDefault();
                    string nameZirGrouh = db.tblRooznamehs.Where(a => a.IdHesab == idHesab).Select(a => a.NameSumGroupHesab).FirstOrDefault();
                    tblRooznameh tbl = new tblRooznameh();
                    tbl.SanadId = (decimal)lastSanad;
                    tbl.SanadTarikhShamsi = row.Cells[9].Value.ToString();
                    tbl.SanadTarikhMiladi = Miladi;
                    tbl.IdHesab = Convert.ToInt64(row.Cells[0].Value);
                    tbl.NameHesab = row.Cells[1].Value.ToString();
                    tbl.NameGroupHesab = nameGrouhHesab;
                    tbl.NameSumGroupHesab = nameZirGrouh;
                    tbl.ArzeshAfzudehTax = Convert.ToInt32(row.Cells[2].Value);
                    tbl.EnteghaTax = Convert.ToInt32(row.Cells[3].Value);
                    tbl.KarmozdMoameleh = Convert.ToInt32(row.Cells[4].Value);
                    if (chkBuyFactor.Checked)
                    {
                        tbl.BuySellVol = Convert.ToInt32(row.Cells[5].Value);
                    }
                    if (chkSellFactor.Checked)
                    {
                        tbl.BuySellVol = -Convert.ToInt32(row.Cells[5].Value);
                    }
                    tbl.SanadTozih = row.Cells[6].Value.ToString();
                    tbl.SanadBedehkar = Convert.ToInt64(row.Cells[7].Value);
                    tbl.SabadBestankar = Convert.ToInt64(row.Cells[8].Value);
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
                dgvSanad.Rows.Clear();
                TblRooznamehsList = sanadList;
            }
            catch (Exception ex)
            {
                MessageBoxFarsi.Show(ex.Message.ToString(), "خطا در ثبت سند");
            }
        }
        public void btnSabt_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvSanad.Rows.Count == 0 || dgvSanad.Rows == null)
                {
                    MessageBoxFarsi.Show("ابتدا باید سند را تنظیم نمایید", "تکمیل اطلاعات سند");
                    return;
                }
                List<DateTime> dateTimesList = new List<DateTime>();
                DateTime miladiDate = new DateTime();
                DateConvert dtMiladi = new DateConvert();

                foreach (DataGridViewRow row in dgvSanad.Rows)
                {
                    string shamsiTarikh = row.Cells[9].Value.ToString();
                    var result = dtMiladi.ConvertToMiladi(shamsiTarikh);
                    if (result == true)
                    {
                        miladiDate = dtMiladi.TarikhMiladi;
                    }
                    if (result == false)
                    {
                        dtMiladi.ShortConvertToMiladi(shamsiTarikh);
                        miladiDate = dtMiladi.TarikhMiladi;
                    }
                    dateTimesList.Add(miladiDate);
                }

                DateTime tarikhMiladi = dateTimesList.Max();
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
                    lastSanad = Convert.ToDouble((MyBeforMaxId + MyAfterMinId) /2);
                }

                List<tblRooznameh> sanadList = new List<tblRooznameh>();
                DateConvert dt = new DateConvert();

                foreach (DataGridViewRow row in dgvSanad.Rows)
                {
                    dt.ConvertToMiladi(row.Cells[9].Value.ToString());
                    var Miladi = dt.TarikhMiladi;

                    long idHesab = Convert.ToInt64(row.Cells[0].Value);
                    string nameGrouhHesab = db.tblRooznamehs.Where(a => a.IdHesab == idHesab).Select(a => a.NameGroupHesab).FirstOrDefault();
                    string nameZirGrouh = db.tblRooznamehs.Where(a => a.IdHesab == idHesab).Select(a => a.NameSumGroupHesab).FirstOrDefault();
                    tblRooznameh tbl = new tblRooznameh();
                    tbl.SanadId = (decimal)lastSanad;
                    tbl.SanadTarikhShamsi = row.Cells[9].Value.ToString();
                    tbl.SanadTarikhMiladi = Miladi;
                    tbl.IdHesab = Convert.ToInt64(row.Cells[0].Value);
                    tbl.NameHesab = row.Cells[1].Value.ToString();
                    tbl.NameGroupHesab = nameGrouhHesab;
                    tbl.NameSumGroupHesab = nameZirGrouh;
                    tbl.ArzeshAfzudehTax = Convert.ToInt32(row.Cells[2].Value);
                    tbl.EnteghaTax = Convert.ToInt32(row.Cells[3].Value);
                    tbl.KarmozdMoameleh = Convert.ToInt32(row.Cells[4].Value);
                    if (chkBuyFactor.Checked)
                    {
                        tbl.BuySellVol = Convert.ToInt32(row.Cells[5].Value);
                    }
                    if (chkSellFactor.Checked)
                    {
                        tbl.BuySellVol = -Convert.ToInt32(row.Cells[5].Value);
                    }
                    tbl.SanadTozih = row.Cells[6].Value.ToString();
                    tbl.SanadBedehkar = Convert.ToInt64(row.Cells[7].Value);
                    tbl.SabadBestankar = Convert.ToInt64(row.Cells[8].Value);
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

                if (chkBuyFactor.Checked)
                {
                    MessageBoxFarsi.Show("سند خرید با موفقیت به ثبت رسید", "ثبت موفق");
                }
                if (chkSellFactor.Checked)
                {
                    MessageBoxFarsi.Show("سند فروش با موفقیت به ثبت رسید", "ثبت موفق");
                }
                dgvSanad.Rows.Clear();
            }
            catch (Exception ex)
            {
                MessageBoxFarsi.Show(ex.Message.ToString(), "خطا در ثبت سند");
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

                var hesabhaInfoList = db.tblRooznamehs.OrderBy(a => a.SanadId).Where(a => a.SanadTarikhMiladi >= tarikhMiladiAz && a.SanadTarikhMiladi <= tarikhMiladiTa && a.SanadId > 0).Select(a => new { a.SanadId, a.SanadTarikhShamsi, a.IdHesab, a.NameHesab, a.NameGroupHesab, a.NameSumGroupHesab, a.SanadTozih, a.BuySellVol, a.ArzeshAfzudehTax, a.EnteghaTax, a.KarmozdMoameleh, a.SanadBedehkar, a.SabadBestankar }).Distinct().ToList();
                //برای نامگذاری سرستون‌ها در کلاس مربوطه
                string[] NameSotunha = { "شماره سند", "تاریخ", "کد حساب", "نام حساب", "طبقه حساب", "زیرطبقه حساب", "شرح سند", "حجم معاملات", "مالیات ارزش افزوده", "مالیات نقل و انتقال", "کارمزد معامله", "بدهکار", "بستانکار" };
                Display calledDgv = new Display();
                calledDgv.ShowDgv(dgvDisplay, NameSotunha, hesabhaInfoList);
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

        private void DisplayNewItem(decimal idsand)
        {
            try
            {

                var hesabhaInfoList = db.tblRooznamehs.OrderBy(a => a.SanadId).Where(a => a.SanadId == idsand).Select(a => new { a.SanadId, a.SanadTarikhShamsi, a.IdHesab, a.NameHesab, a.NameGroupHesab, a.NameSumGroupHesab, a.SanadTozih, a.BuySellVol, a.ArzeshAfzudehTax, a.EnteghaTax, a.KarmozdMoameleh, a.SanadBedehkar, a.SabadBestankar }).Distinct().ToList();
                //برای نامگذاری سرستون‌ها در کلاس مربوطه
                string[] NameSotunha = { "شماره سند", "تاریخ", "کد حساب", "نام حساب", "طبقه حساب", "زیرطبقه حساب", "شرح سند", "حجم معاملات", "مالیات ارزش افزوده", "مالیات نقل و انتقال", "کارمزد معامله", "بدهکار", "بستانکار" };
                Display calledDgv = new Display();
                calledDgv.ShowDgv(dgvDisplay, NameSotunha, hesabhaInfoList);
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

        private void cmbCodeTaraf_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                int codeHesabTaraf = Convert.ToInt32(cmbCodeTaraf.SelectedItem);
                string nameHesab = db.tblRooznamehs.Where(a => a.IdHesab == codeHesabTaraf).Select(a => a.NameHesab).Distinct().FirstOrDefault();
                cmbTarafMoameleh.SelectedItem = nameHesab;
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

        private void chkBuyFactor_CheckedChanged(object sender, EventArgs e)
        {
            chkSellFactor.Checked = false;
            frmBuySellFactor_Load(sender, e);
        }

        private void chkSellFactor_CheckedChanged(object sender, EventArgs e)
        {
            chkBuyFactor.Checked = false;
            frmBuySellFactor_Load(sender, e);
        }

        private void btnEdit_Click(object sender, EventArgs e)
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
                    idSanadList.Add(Convert.ToDecimal(dgvDisplay.Rows[i].Cells[0].Value));
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
                dt.Columns[9].DataType = typeof(double);
                dt.Columns[10].DataType = typeof(double);
                dt.Columns[11].DataType = typeof(double);
                dt.Columns[12].DataType = typeof(double);
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
                frmEditFactor frmEdit = new frmEditFactor();
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
                frmEdit.dgvEdit.Columns[9].DefaultCellStyle.Format = "N0";
                frmEdit.dgvEdit.Columns[10].DefaultCellStyle.Format = "N0";
                frmEdit.dgvEdit.Columns[11].DefaultCellStyle.Format = "N0";
                frmEdit.dgvEdit.Columns[12].DefaultCellStyle.Format = "N0";
                frmEdit.ShowDialog();
                DisplayNewItem(idSanadList.FirstOrDefault());
            }
            catch (Exception ex)
            {
                MessageBoxFarsi.Show(ex.Message.ToString(), "وجود خطا در ویرایش سند");
            }
        }

        private void txtSearchNam_TextChanged(object sender, EventArgs e)
        {
            try
            {
                string myHesab = txtSearchNam.Text;
                var hesabhaInfoList = db.tblRooznamehs.OrderBy(a => a.SanadId).Where(a => a.NameHesab.Contains(myHesab) && a.SanadId > 0).Select(a => new { a.SanadId, a.SanadTarikhShamsi, a.IdHesab, a.NameHesab, a.NameGroupHesab, a.NameSumGroupHesab, a.SanadTozih, a.BuySellVol, a.ArzeshAfzudehTax, a.EnteghaTax, a.KarmozdMoameleh, a.SanadBedehkar, a.SabadBestankar }).Distinct().ToList();
                //برای نامگذاری سرستون‌ها در کلاس مربوطه
                string[] NameSotunha = { "شماره سند", "تاریخ", "کد حساب", "نام حساب", "طبقه حساب", "زیرطبقه حساب", "شرح سند", "حجم معاملات", "مالیات ارزش افزوده", "مالیات نقل و انتقال", "کارمزد معامله", "بدهکار", "بستانکار" };
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


        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                decimal IdSanadHazfi = Convert.ToDecimal(dgvDisplay.CurrentRow.Cells[0].Value);
                var del = db.tblRooznamehs.Where(a => a.SanadId == IdSanadHazfi);
                db.tblRooznamehs.RemoveRange(del);
                db.SaveChanges();
                DisplayNewItem(IdSanadHazfi);
                MessageBoxFarsi.Show($"حذف سند شماره {IdSanadHazfi} با موفقیت انجام شد", "حذف موفق سند");
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
                    if (dgvDisplay.DataSource == null)
                    {
                        MessageBoxFarsi.Show("یک حساب را از جدول نمایش انتخاب کنید", "عدم انتخاب حساب");
                        return;
                    }
                    int idSanad = Convert.ToInt32(dgvDisplay.CurrentRow.Cells[0].Value);
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

        private void dgvDisplay_MouseUp(object sender, MouseEventArgs e)
        {
            //txtSearchNam.Text = dgvDisplay.CurrentRow.Cells[3].Value.ToString();
        }

        private void btnReadExcell_Click(object sender, EventArgs e)
        {
            try
            {
                if (cmbTarafMoameleh.Text == "" || cmbCodeTaraf.Text == "")
                {
                    MessageBoxFarsi.Show(" شخص طرف معامله انتخاب نشده است", "خطا در تنظیم سند");
                    return;
                }
                ExcelReports er = new ExcelReports();
                er.ExcellReader();
                var buyList = er.MyBuyList.OrderBy(a => a.ShamsiDate).ToList();
                var sellList = er.MySellList.OrderBy(a => a.ShamsiDate).ToList();
                if (chkBuyFactor.Checked)
                {
                    if (buyList.Count == 0)
                    {
                        MessageBoxFarsi.Show("تراکنش خریدی وجود ندارد ،اطمینان یابید ستون دوم اکسل، تاریخ و ستون سوم شرح سند باشد", "نبود تراکنش خرید");
                        return;
                    }
                }
                if (chkSellFactor.Checked)
                {
                    if (sellList.Count == 0)
                    {
                        MessageBoxFarsi.Show("تراکنش فروشی وجود ندارد ،اطمینان یابید ستون دوم اکسل، تاریخ و ستون سوم شرح سند باشد", "نبود تراکنش خرید");
                        return;
                    }
                }

                Sanad sabtSanad = new Sanad();
                var result = sabtSanad.SabtSanad(buyList, sellList, numBakhshPardakhti, numAfzoodehTaxRate, numEnteghalTaxRate, numKarmozdRate, numKarmozdRate, cmbNoeTasviyeh, cmbTarafMoameleh, cmbCodeTaraf, cmbHesabBanki, cmbCodeHesabBanki, chkBuyFactor, chkSellFactor, dgvSanad, sender, e);
                var AsnadList = sabtSanad.TblRooznamehList;
                if (result == true && chkBuyFactor.Checked && AsnadList != null)
                {
                        MessageBoxFarsi.Show("سند(های) خرید با موفقیت به ثبت رسید", "ثبت موفق");
                }
                if (result == true && chkSellFactor.Checked && AsnadList != null)
                {
                    MessageBoxFarsi.Show("سند(های) فروش با موفقیت به ثبت رسید", "ثبت موفق");
                }
                if (AsnadList == null || AsnadList.Count==0)
                {
                    MessageBoxFarsi.Show("با توجه به کنترل سیستم مدیریت موجودی، سندی جهت ثبت وجود ندارد", "عدم وجود سند جهت ثبت");
                }
                dgvSanad.Rows.Clear();

            }
            catch (Exception ex)
            {
                MessageBoxFarsi.Show(ex.Message.ToString()+"\n"+"احتمالا سطری وجود دارد که حاوی خرید و فروش نیست!", "خطا در دریافت اطلاعات اکسل");
            }

        }

        private void txtSearchIdSanad_TextChanged(object sender, EventArgs e)
        {
            try
            {
                decimal myIdSanad = Convert.ToDecimal(txtSearchIdSanad.Text);

                var hesabhaInfoList = db.tblRooznamehs.OrderBy(a => a.SanadId).Where(a => a.SanadId == myIdSanad && a.SanadId > 0).Select(a => new { a.SanadId, a.SanadTarikhShamsi, a.IdHesab, a.NameHesab, a.NameGroupHesab, a.NameSumGroupHesab, a.SanadTozih, a.BuySellVol, a.ArzeshAfzudehTax, a.EnteghaTax, a.KarmozdMoameleh, a.SanadBedehkar, a.SabadBestankar }).Distinct().ToList();
                //برای نامگذاری سرستون‌ها در کلاس مربوطه
                string[] NameSotunha = { "شماره سند", "تاریخ", "کد حساب", "نام حساب", "طبقه حساب", "زیرطبقه حساب", "شرح سند", "حجم معاملات", "مالیات ارزش افزوده", "مالیات نقل و انتقال", "کارمزد معامله", "بدهکار", "بستانکار" };
                Display calledDgv = new Display();
                calledDgv.ShowDgv(dgvDisplay, NameSotunha, hesabhaInfoList);
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
                MessageBoxFarsi.Show(ex.Message.ToString(), "خطا در جستجو");
            }
        }

        private void txtSearchIdSanad_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                decimal myIdSanad = Convert.ToDecimal(txtSearchIdSanad.Value);
                var hesabhaInfoList = db.tblRooznamehs.OrderBy(a => a.SanadId).Where(a => a.SanadId == myIdSanad && a.SanadId > 0).Select(a => new { a.SanadId, a.SanadTarikhShamsi, a.IdHesab, a.NameHesab, a.NameGroupHesab, a.NameSumGroupHesab, a.SanadTozih, a.BuySellVol, a.ArzeshAfzudehTax, a.EnteghaTax, a.KarmozdMoameleh, a.SanadBedehkar, a.SabadBestankar }).Distinct().ToList();
                //برای نامگذاری سرستون‌ها در کلاس مربوطه
                string[] NameSotunha = { "شماره سند", "تاریخ", "کد حساب", "نام حساب", "طبقه حساب", "زیرطبقه حساب", "شرح سند", "حجم معاملات", "مالیات ارزش افزوده", "مالیات نقل و انتقال", "کارمزد معامله", "بدهکار", "بستانکار" };
                Display calledDgv = new Display();
                calledDgv.ShowDgv(dgvDisplay, NameSotunha, hesabhaInfoList);
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
                MessageBoxFarsi.Show(ex.Message.ToString(), "خطا در جستجو");
            }
        }

        private void txtSearchDaftarKol_TextChanged(object sender, EventArgs e)
        {
            try
            {
                string daftarkolName = txtSearchDaftarKol.Text;

                var hesabhaInfoList = db.tblRooznamehs.OrderBy(a => a.SanadId).Where(a => a.NameSumGroupHesab.ToString().Contains(daftarkolName) && a.SanadId > 0).Select(a => new { a.SanadId, a.SanadTarikhShamsi, a.IdHesab, a.NameHesab, a.NameGroupHesab, a.NameSumGroupHesab, a.SanadTozih, a.BuySellVol, a.ArzeshAfzudehTax, a.EnteghaTax, a.KarmozdMoameleh, a.SanadBedehkar, a.SabadBestankar }).Distinct().ToList();
                //برای نامگذاری سرستون‌ها در کلاس مربوطه
                string[] NameSotunha = { "شماره سند", "تاریخ", "کد حساب", "نام حساب", "طبقه حساب", "زیرطبقه حساب", "شرح سند", "حجم معاملات", "مالیات ارزش افزوده", "مالیات نقل و انتقال", "کارمزد معامله", "بدهکار", "بستانکار" };
                Display calledDgv = new Display();
                calledDgv.ShowDgv(dgvDisplay, NameSotunha, hesabhaInfoList);
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
                MessageBoxFarsi.Show(ex.Message.ToString(), "خطا در جستجو");
            }
        }
    }
}
