using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HooshBartarAccountingApp.DataShow
{
    class DateConvert
    {
        public string TarikhShamsi { get; set; }
        public DateTime TarikhMiladi { get; set; }
        public bool ConvertToMiladi(string Shamsi)
        {
            try
            {
                TarikhShamsi = Shamsi;
                int yearShamsi = Convert.ToInt32(TarikhShamsi.Substring(0, 4));
                int monthShamsi = Convert.ToInt32(TarikhShamsi.Substring(5, 2));
                int dayShamsi = Convert.ToInt32(TarikhShamsi.Substring(8, 2));
                int hourShamsi = Convert.ToInt32(TarikhShamsi.Substring(11, 2));
                int minuteShamsi = Convert.ToInt32(TarikhShamsi.Substring(14, 2));
                int secondShamsi = Convert.ToInt32(TarikhShamsi.Substring(17, 2));
                DateTime miladi = new DateTime(yearShamsi, monthShamsi, dayShamsi, hourShamsi, minuteShamsi, secondShamsi, new System.Globalization.PersianCalendar());
                TarikhMiladi = miladi;
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public bool ShortConvertToMiladi(string Shamsi)
        {
            try
            {
                TarikhShamsi = Shamsi;
                int yearShamsi = Convert.ToInt32(TarikhShamsi.Substring(0, 4));
                int monthShamsi = Convert.ToInt32(TarikhShamsi.Substring(5, 2));
                int dayShamsi = Convert.ToInt32(TarikhShamsi.Substring(8, 2));
                DateTime miladi = new DateTime(yearShamsi, monthShamsi, dayShamsi,23,59,59,998, new System.Globalization.PersianCalendar());
                TarikhMiladi = miladi;
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        //private void btnSave_Click(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        if (cmbGrouheHesab.SelectedItem == null || cmbDaftarKol.SelectedItem == null)
        //        {
        //            MessageBoxFarsi.Show("طبقه یا زیر طبقه حساب انتخاب نشده است", "الزام انتخاب");
        //            return;
        //        }
        //        if (txtNameHesab.Text == "")
        //        {
        //            MessageBoxFarsi.Show("نام حسابی وارد نشده است", "الزام وارد کردن نام حساب");
        //            return;
        //        }
        //        string tarikhShamsi = mskTarikh.Text;
        //        DateConvert miladi = new DateConvert();
        //        miladi.ConvertToMiladi(tarikhShamsi);
        //        DateTime tarikhMiladi = miladi.TarikhMiladi;
        //        double lastSanad = 0;
        //        var LastIdsanadGhablAz = db.tblRooznamehs.Where(a => a.SanadTarikhMiladi <= tarikhMiladi).Select(a => a.SanadId).ToList();
        //        var LastIdsanadBaedAz = db.tblRooznamehs.Where(a => a.SanadTarikhMiladi > tarikhMiladi).Select(a => a.SanadId).ToList();
        //        if (LastIdsanadGhablAz.Count == 0 && LastIdsanadBaedAz.Count == 0)
        //        {
        //            lastSanad = 1;
        //        }
        //        else if (LastIdsanadGhablAz.Count == 0 && LastIdsanadBaedAz.Count > 0)
        //        {
        //            lastSanad = Convert.ToDouble(LastIdsanadBaedAz.Min()) - 0.01;
        //        }
        //        else if (LastIdsanadGhablAz.Count > 0 && LastIdsanadBaedAz.Count == 0)
        //        {
        //            lastSanad = Convert.ToDouble(LastIdsanadGhablAz.Max() + 1);
        //        }
        //        else if (LastIdsanadGhablAz.Count > 0 && LastIdsanadBaedAz.Count > 0)
        //        {
        //            lastSanad = Convert.ToDouble((LastIdsanadGhablAz.Max() + LastIdsanadBaedAz.Min()) / 2);
        //        }

        //        var tabagheyeHesab = cmbGrouheHesab.SelectedItem.ToString();
        //        var ZirTabgheyeHEsab = cmbDaftarKol.SelectedItem.ToString();
        //        var Grouh = db.tblRooznamehs.Where(a => a.NameGroupHesab == tabagheyeHesab).ToList();
        //        var ZirGrouh = db.tblRooznamehs.Where(a => a.NameSumGroupHesab == ZirTabgheyeHEsab).Select(a => a.IdHesab).ToList();
        //        int CodeGrouh = db.tblTabaghatHesabs.Where(a => a.TabagheyehHesab == tabagheyeHesab).Select(a => a.CodeTabaghehHesab).FirstOrDefault();
        //        int CodeZirGrouh = db.tblTabaghatHesabs.Where(a => a.ZirTzbagheyehHesab == ZirTabgheyeHEsab).Select(a => a.CodeTabaghehHesab).FirstOrDefault();
        //        int lastIdHesab;
        //        if (ZirGrouh.Count == 0)
        //        {
        //            lastIdHesab = Convert.ToInt32(CodeGrouh.ToString() + CodeZirGrouh.ToString() + "001");
        //        }
        //        else
        //        {
        //            lastIdHesab = Convert.ToInt32(ZirGrouh.Last() + 1);
        //        }

        //        tblRooznameh tbl = new tblRooznameh();
        //        tbl.SanadId = (decimal)lastSanad;
        //        tbl.IdHesab = lastIdHesab;
        //        tbl.NameHesab = txtNameHesab.Text;
        //        tbl.NameGroupHesab = cmbGrouheHesab.SelectedItem.ToString();
        //        tbl.NameSumGroupHesab = cmbDaftarKol.SelectedItem.ToString();
        //        tbl.SanadBedehkar = Convert.ToInt64(numArzeshAvl.Value);
        //        tbl.SabadBestankar = Convert.ToInt64(numArzeshAvl.Value);
        //        tbl.SanadTozih = txtSharh.Text;
        //        tbl.SanadTarikhMiladi = tarikhMiladi;
        //        tbl.SanadTarikhShamsi = mskTarikh.Text;
        //        tbl.BuySellVol = Convert.ToInt32(numVolumAval.Value);
        //        db.tblRooznamehs.Add(tbl);
        //        db.SaveChanges();
        //        MessageBoxFarsi.Show($"حساب {txtNameHesab.Text} با موفقیت ثبت شد", "معرفی موفقیت‌آمیز حساب");
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBoxFarsi.Show(ex.Message.ToString(), "خطا در معرفی حساب");
        //    }
        //}
    }
}
