using BehComponents;
using HooshBartarAccountingApp.DatabaseModel;
using HooshBartarAccountingApp.DataShow;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.Remoting.Channels;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HooshBartarAccountingApp
{
    class Sanad
    {
        AccountDBEntities db = new AccountDBEntities();

        public string HesabName { get; set; }
        public string ShamsiDate { get; set; }
        public DateTime MiladiDate { get; set; }
        public string SanadDescription { get; set; }
        public string SanadDebit { get; set; }
        public string SanadCredit { get; set; }

        public int? BuyVolume { get; set; }
        public int? SellVolume { get; set; }
        public int? BuyPrice { get; set; }
        public int? SellPrice { get; set; }
        public DataGridView MyDgv { get; set; }
        public List<tblRooznameh> TblRooznamehList { get; set; }

        public static string GetBetween(string strSource, string strStart, string strEnd)
        {
            try
            {
                string output = "";
                if (strSource.Contains(strStart) && strSource.Contains(strEnd))
                {
                    int Start, End;
                    Start = strSource.IndexOf(strStart, 0) + strStart.Length;
                    End = strSource.IndexOf(strEnd, Start);
                    //برای حذف کاما از رشته جهت سهولت تبدیل به اینتجر
                    string pre = strSource.Substring(Start, End - Start);
                    if (pre.Contains(","))
                    {
                        output = pre.Replace(",", "");
                    }
                    else
                    {
                        output = pre;
                    }
                    return output;
                }
                return "";
            }
            catch (Exception)
            {
                return "0";
            }
        }

        public bool SabtSanad(List<Sanad> buyList, List<Sanad> SellList, NumericUpDown numBakhshPardakhti, NumericUpDown numArzeshAfzoodeh, NumericUpDown numEnteghalTax, NumericUpDown BuyFee, NumericUpDown SellFee, ComboBox cmbNoeTasviyeh, ComboBox cmbTarafMoameleh, ComboBox cmbCodeTaraf, ComboBox cmbHesabBanki, ComboBox cmbCodeHesabBanki, CheckBox chkBuyFactor, CheckBox chkSellFactor, DataGridView dgvSanad, object sender, EventArgs e)
        {
            try
            {
                List<tblRooznameh> sL = new List<tblRooznameh>();
                foreach (var item in buyList)
                {
                    string HesabNameInDb = db.tblRooznamehs.Where(a => a.NameHesab == item.HesabName).Select(a => a.NameHesab).FirstOrDefault();
                    Int64 CodeHesabNameInDb = db.tblRooznamehs.Where(a => a.NameHesab == item.HesabName).Select(a => a.IdHesab).FirstOrDefault();
                    if (HesabNameInDb == null)
                    {
                        MessageBoxFarsi.Show($"حساب {item.HesabName} در پایگاه داده وجود ندارد، و در حال معرفی شدن به پایگاه داده است!");
                        frmHesabha frm = new frmHesabha();
                        frm.txtNameHesab.Text = item.HesabName;
                        frm.frmHesabha_Load(null, null);
                        frm.cmbGrouheHesab.SelectedIndex = 0;
                        frm.cmbDaftarKol.SelectedIndex = 1;
                        frm.btnSave_Click(null, null);
                    }
                }

                if (chkBuyFactor.Checked)
                {
                    frmBuySellFactor frm = new frmBuySellFactor();
                    frm.chkBuyFactor.Checked = true;
                    foreach (var item in buyList)
                    {
                        Int64 ArzeshAfzoodehTax = 0;
                        ArzeshAfzoodehTax = Convert.ToInt64(Convert.ToInt64(item.BuyVolume) * Convert.ToInt64(item.BuyPrice) * Convert.ToDouble(numArzeshAfzoodeh.Value));
                        Int64? ArzeshKolMoameleh = 0;
                        Int64 EnteghalTax = 0;
                        Int64 Karmozd = 0;
                        Karmozd = Convert.ToInt64(Convert.ToInt64(item.BuyVolume) * Convert.ToInt64(item.BuyPrice) * Convert.ToDouble(BuyFee.Value));
                        ArzeshKolMoameleh =Convert.ToInt64(item.SanadDebit);
                        string HesabNameInDb = db.tblRooznamehs.Where(a => a.NameHesab == item.HesabName).Select(a => a.NameHesab).FirstOrDefault();
                        Int64 CodeHesabNameInDb = db.tblRooznamehs.Where(a => a.NameHesab == item.HesabName).Select(a => a.IdHesab).FirstOrDefault();
                        if (cmbNoeTasviyeh.SelectedIndex == 0)//معامله نقد
                        {
                            if (cmbHesabBanki.Text == "" || cmbCodeHesabBanki.Text == "")
                            {
                                MessageBoxFarsi.Show(" با توجه به نوع معامله، باید حسابی جهت تسویه معرفی شود!", "خطا در تنظیم سند");
                                return false;
                            }
                            dgvSanad.Rows.Add(CodeHesabNameInDb, HesabNameInDb, ArzeshAfzoodehTax, EnteghalTax, Karmozd, item.BuyVolume, item.SanadDescription, ArzeshKolMoameleh, 0, item.ShamsiDate);
                            dgvSanad.Rows.Add(cmbCodeTaraf.Text, cmbTarafMoameleh.Text, 0, 0, 0, 0, item.SanadDescription, 0, ArzeshKolMoameleh, item.ShamsiDate);
                            dgvSanad.Rows.Add(cmbCodeTaraf.Text, cmbTarafMoameleh.Text, 0, 0, 0, 0, item.SanadDescription, ArzeshKolMoameleh, 0, item.ShamsiDate);
                            dgvSanad.Rows.Add(cmbCodeHesabBanki.Text, cmbHesabBanki.Text, 0, 0, 0, 0, item.SanadDescription, 0, ArzeshKolMoameleh, item.ShamsiDate);
                            frm.SabtExcell(dgvSanad);
                            TblRooznamehList = frm.TblRooznamehsList;

                        }
                        else if (cmbNoeTasviyeh.SelectedIndex == 1)//معامله نسیه
                        {
                            dgvSanad.Rows.Add(CodeHesabNameInDb, HesabNameInDb, ArzeshAfzoodehTax, EnteghalTax, Karmozd, item.BuyVolume, item.SanadDescription, ArzeshKolMoameleh, 0, item.ShamsiDate);
                            dgvSanad.Rows.Add(cmbCodeTaraf.Text, cmbTarafMoameleh.Text, 0, 0, 0, 0, item.SanadDescription, 0, ArzeshKolMoameleh, item.ShamsiDate);
                            frm.SabtExcell(dgvSanad);
                            TblRooznamehList = frm.TblRooznamehsList;

                        }
                        else if (cmbNoeTasviyeh.SelectedIndex == 2)//معامله نقد و نسیه
                        {
                            if (cmbHesabBanki.Text == "" || cmbCodeHesabBanki.Text == "")
                            {
                                MessageBoxFarsi.Show(" با توجه به نوع معامله، باید حسابی جهت تسویه معرفی شود!", "خطا در تنظیم سند");
                                return false;
                            }
                            if (Convert.ToDouble(numBakhshPardakhti.Value) >= ArzeshKolMoameleh || Convert.ToDouble(numBakhshPardakhti.Value) == 0)
                            {
                                MessageBoxFarsi.Show("شرایط پرداخت با نوع قرارداد نقد/نسیه مطابقت ندارد، قرارداد را عوض نمایید یا بخشی از مبلغ پرداختی را وارد کنید");
                                numBakhshPardakhti.BackColor = Color.Yellow;
                                return false;
                            }
                            dgvSanad.Rows.Add(CodeHesabNameInDb, HesabNameInDb, ArzeshAfzoodehTax, EnteghalTax, Karmozd, item.BuyVolume, item.SanadDescription, ArzeshKolMoameleh, 0, item.ShamsiDate);
                            dgvSanad.Rows.Add(cmbCodeTaraf.Text, cmbTarafMoameleh.Text, 0, 0, 0, 0, item.SanadDescription, 0, ArzeshKolMoameleh - Convert.ToDouble(numBakhshPardakhti.Value), item.ShamsiDate);
                            dgvSanad.Rows.Add(cmbCodeHesabBanki.Text, cmbHesabBanki.Text, 0, 0, 0, 0, item.SanadDescription, 0, Convert.ToDouble(numBakhshPardakhti.Value), item.ShamsiDate);
                            frm.SabtExcell(dgvSanad);
                            TblRooznamehList = frm.TblRooznamehsList;
                        }
                    }
                }

                if (chkSellFactor.Checked)
                {
                    frmBuySellFactor frm = new frmBuySellFactor();
                    frm.chkSellFactor.Checked = true;

                    DateConvert dateConvert = new DateConvert();
                    DateTime BeforeHistoryDate = new DateTime();
                    foreach (var item in SellList)
                    {
                        var shamsiTarikh = item.ShamsiDate;
                        var result = dateConvert.ConvertToMiladi(shamsiTarikh);
                        if (result==true)
                        {
                            BeforeHistoryDate = dateConvert.TarikhMiladi;
                        }
                        else
                        {
                            dateConvert.ShortConvertToMiladi(shamsiTarikh);
                            BeforeHistoryDate = dateConvert.TarikhMiladi;
                        }

                        Int64? volMojud = db.tblRooznamehs.Where(a => a.NameHesab == item.HesabName && a.SanadTarikhMiladi <= BeforeHistoryDate).Select(a => a.BuySellVol).Sum();
                        if (Convert.ToInt64(item.SellVolume) > volMojud)
                        {
                            MessageBoxFarsi.Show($"حداکثر موجودی مورد معامله {item.HesabName} تعداد {volMojud} می‌باشد، تعداد فروش را کاهش دهید", "خطاء موجودی");
                        }
                        if (volMojud == null || volMojud == 0)
                        {
                            MessageBoxFarsi.Show($"مورد معامله {item.HesabName} موجودی در دفاتر ندارد، لطفا کنترل نمایید", "نبود موجودی");

                        }
                        if (volMojud != null && volMojud != 0 && Convert.ToInt64(item.SellVolume) <= volMojud)
                        {
                            Int64 ArzeshAfzoodehTax = Convert.ToInt64(Convert.ToInt64(item.SellVolume) * Convert.ToInt64(item.SellPrice) * Convert.ToDouble(numArzeshAfzoodeh.Value));
                            Int64? ArzeshKolMoameleh = 0;
                            Int64 EnteghalTax = 0;
                            Int64 Karmozd = 0;

                            var NamehesabForush = db.tblRooznamehs.Where(a => a.IdHesab == 4001001).Select(a => a.NameHesab).FirstOrDefault();
                            var CodeHesabForush = db.tblRooznamehs.Where(a => a.IdHesab == 4001001).Select(a => a.IdHesab).FirstOrDefault();
                            var NamehesabBahayeTamam = db.tblRooznamehs.Where(a => a.IdHesab == 5001001).Select(a => a.NameHesab).FirstOrDefault();
                            var CodeHesabBahayeTamam = db.tblRooznamehs.Where(a => a.IdHesab == 5001001).Select(a => a.IdHesab).FirstOrDefault();
                            Karmozd = Convert.ToInt64(Convert.ToInt64(item.SellVolume) * Convert.ToInt64(item.SellPrice) * Convert.ToDouble(SellFee.Value));
                            EnteghalTax = Convert.ToInt64(Convert.ToInt64(item.SellVolume) * Convert.ToInt64(item.SellPrice) * Convert.ToDouble(numEnteghalTax.Value));
                            ArzeshKolMoameleh =Convert.ToInt64(item.SanadCredit);
                            string HesabNameInDb = db.tblRooznamehs.Where(a => a.NameHesab == item.HesabName).Select(a => a.NameHesab).FirstOrDefault();
                            Int64 CodeHesabNameInDb = db.tblRooznamehs.Where(a => a.NameHesab == item.HesabName).Select(a => a.IdHesab).First();
                            if (HesabNameInDb == null)
                            {
                                MessageBoxFarsi.Show($"حساب {item.HesabName} در پایگاه داده وجود ندارد، از بخش معرفی حساب معین اقدام نمایید");
                            }

                            Int64? sumHesab = db.tblRooznamehs.Where(a => a.IdHesab == CodeHesabNameInDb && a.SanadTarikhMiladi <= BeforeHistoryDate).Select(a => a.SanadBedehkar - a.SabadBestankar).Sum();
                            Int64? vol = db.tblRooznamehs.Where(a => a.IdHesab == CodeHesabNameInDb && a.SanadTarikhMiladi <= BeforeHistoryDate).Select(a => a.BuySellVol).Sum();
                            double? avg = Convert.ToDouble(sumHesab) / vol;
                            Int64? avgArzeshKharid = 0;
                            if (item.SellVolume == vol)
                            {
                                avgArzeshKharid = sumHesab;
                            }
                            else if (item.SellVolume < vol)
                            {
                                avgArzeshKharid = Convert.ToInt64(avg * Convert.ToInt64(item.SellVolume));
                            }

                            if (cmbNoeTasviyeh.SelectedIndex == 0)//معامله نقد
                            {
                                if (cmbHesabBanki.Text == "" || cmbCodeHesabBanki.Text == "")
                                {
                                    MessageBoxFarsi.Show(" با توجه به نوع معامله، باید حسابی جهت تسویه معرفی شود!", "خطا در تنظیم سند");
                                    return false;
                                }
                                dgvSanad.Rows.Add(CodeHesabNameInDb, HesabNameInDb, ArzeshAfzoodehTax, EnteghalTax, Karmozd, item.SellVolume, item.SanadDescription, 0, avgArzeshKharid, item.ShamsiDate);
                                dgvSanad.Rows.Add(cmbCodeTaraf.Text, cmbTarafMoameleh.Text, 0, 0, 0, 0, item.SanadDescription, ArzeshKolMoameleh, 0, item.ShamsiDate);
                                dgvSanad.Rows.Add(cmbCodeTaraf.Text, cmbTarafMoameleh.Text, 0, 0, 0, 0, item.SanadDescription, 0, ArzeshKolMoameleh, item.ShamsiDate);
                                dgvSanad.Rows.Add(cmbCodeHesabBanki.Text, cmbHesabBanki.Text, 0, 0, 0, 0, item.SanadDescription, ArzeshKolMoameleh, 0, item.ShamsiDate);
                                dgvSanad.Rows.Add(CodeHesabForush, NamehesabForush, 0, 0, 0, 0, item.SanadDescription, 0, ArzeshKolMoameleh, item.ShamsiDate);
                                dgvSanad.Rows.Add(CodeHesabBahayeTamam, NamehesabBahayeTamam, 0, 0, 0, 0, item.SanadDescription, avgArzeshKharid, 0, item.ShamsiDate);
                                frm.SabtExcell(dgvSanad);
                                TblRooznamehList = frm.TblRooznamehsList;

                            }
                            else if (cmbNoeTasviyeh.SelectedIndex == 1)//معامله نسیه
                            {
                                dgvSanad.Rows.Add(CodeHesabNameInDb, HesabNameInDb, ArzeshAfzoodehTax, EnteghalTax, Karmozd, item.SellVolume, item.SanadDescription, 0, avgArzeshKharid, item.ShamsiDate);
                                dgvSanad.Rows.Add(cmbCodeTaraf.Text, cmbTarafMoameleh.Text, 0, 0, 0, 0, item.SanadDescription, ArzeshKolMoameleh, 0, item.ShamsiDate);
                                dgvSanad.Rows.Add(CodeHesabForush, NamehesabForush, 0, 0, 0, 0, item.SanadDescription, 0, ArzeshKolMoameleh, item.ShamsiDate);
                                dgvSanad.Rows.Add(CodeHesabBahayeTamam, NamehesabBahayeTamam, 0, 0, 0, 0, item.SanadDescription, avgArzeshKharid, 0, item.ShamsiDate);
                                frm.SabtExcell(dgvSanad);
                                TblRooznamehList = frm.TblRooznamehsList;

                            }
                            else if (cmbNoeTasviyeh.SelectedIndex == 2)//معامله نقد و نسیه
                            {
                                if (cmbHesabBanki.Text == "" || cmbCodeHesabBanki.Text == "")
                                {
                                    MessageBoxFarsi.Show(" با توجه به نوع معامله، باید حسابی جهت تسویه معرفی شود!", "خطا در تنظیم سند");
                                    return false;
                                }
                                if (Convert.ToDouble(numBakhshPardakhti.Value) >= ArzeshKolMoameleh || Convert.ToDouble(numBakhshPardakhti.Value) == 0)
                                {
                                    MessageBoxFarsi.Show("شرایط پرداخت با نوع قرارداد نقد/نسیه مطابقت ندارد، قرارداد را عوض نمایید یا بخشی از مبلغ پرداختی را وارد کنید");
                                    numBakhshPardakhti.BackColor = Color.Yellow;
                                    return false;
                                }
                                dgvSanad.Rows.Add(CodeHesabNameInDb, HesabNameInDb, ArzeshAfzoodehTax, EnteghalTax, Karmozd, item.SellVolume, item.SanadDescription, 0, avgArzeshKharid, item.ShamsiDate);
                                dgvSanad.Rows.Add(cmbCodeTaraf.Text, cmbTarafMoameleh.Text, 0, 0, 0, 0, item.SanadDescription, ArzeshKolMoameleh - Convert.ToDouble(numBakhshPardakhti.Value), 0, item.ShamsiDate);
                                dgvSanad.Rows.Add(cmbCodeHesabBanki.Text, cmbHesabBanki.Text, 0, 0, 0, 0, item.SanadDescription, Convert.ToDouble(numBakhshPardakhti.Value), 0, item.ShamsiDate);
                                dgvSanad.Rows.Add(CodeHesabForush, NamehesabForush, 0, 0, 0, 0, item.SanadDescription, 0, ArzeshKolMoameleh, item.ShamsiDate);
                                dgvSanad.Rows.Add(CodeHesabBahayeTamam, NamehesabBahayeTamam, 0, 0, 0, 0, item.SanadDescription, avgArzeshKharid, 0, item.ShamsiDate);
                                frm.SabtExcell(dgvSanad);
                                TblRooznamehList = frm.TblRooznamehsList;
                            }
                        }
                    }
                }
                return true;

            }
            catch (Exception ex)
            {
                MessageBoxFarsi.Show(ex.Message.ToString(), "خطا در تنظیم سند");
                return false;
            }
        }
    }
}
