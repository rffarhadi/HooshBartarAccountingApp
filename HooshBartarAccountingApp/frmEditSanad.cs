using BehComponents;
using HooshBartarAccountingApp.DatabaseModel;
using HooshBartarAccountingApp.DataShow;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HooshBartarAccountingApp
{
    public partial class frmEditSanad : Form
    {
        public frmEditSanad()
        {
            InitializeComponent();
        }

        private void frmEditSanad_Load(object sender, EventArgs e)
        {
           
        }

        AccountDBEntities db = new AccountDBEntities();
        private void btnEditSanad_Click(object sender, EventArgs e)
        {
            try
            {
                string tarikhShamsi = dgvEdit.Rows[0].Cells[1].Value.ToString();
                DateConvert miladi = new DateConvert();
                DateTime tarikhMiladi = new DateTime();
                var result = miladi.ConvertToMiladi(tarikhShamsi);
                if (result == true)
                {
                    tarikhMiladi = miladi.TarikhMiladi;
                }
                if (result == false)
                {
                    miladi.ConvertToMiladi(tarikhShamsi);
                    tarikhMiladi = miladi.TarikhMiladi;
                }

                decimal IdSanad =Convert.ToDecimal(dgvEdit.Rows[0].Cells[0].Value);
                List<tblRooznameh> sanadList = new List<tblRooznameh>();
                foreach (DataGridViewRow row in dgvEdit.Rows)
                {
                    tblRooznameh tbl = new tblRooznameh()
                    {
                        SanadTarikhShamsi = tarikhShamsi,
                        SanadTarikhMiladi = tarikhMiladi,
                        IdHesab = Convert.ToInt32(row.Cells[2].Value),
                        SanadTozih = row.Cells[6].Value.ToString(),
                        SanadBedehkar = Convert.ToInt64(row.Cells[7].Value),
                        SabadBestankar = Convert.ToInt64(row.Cells[8].Value)
                    };
                    sanadList.Add(tbl);
                }
                var sanadListNahayi = sanadList.OrderBy(a => a.IdHesab).ToList();

                var sumBed = sanadListNahayi.Select(a => a.SanadBedehkar).Sum();
                var sumBes = sanadListNahayi.Select(a => a.SabadBestankar).Sum();

                if (sumBed != sumBes)
                {
                    MessageBoxFarsi.Show($"جمع ستون بدهکار با جمع ستون بستانکار مبلغ {Math.Abs((long)(sumBed - sumBes))} اختلاف دارد", "سند ناتراز");
                    return;
                }

                var editList = db.tblRooznamehs.OrderBy(a=>a.IdHesab).Where(a => a.SanadId == IdSanad).ToList();
                int j = 0;
                    editList.ForEach(a => { a.SanadTarikhShamsi = sanadListNahayi[j].SanadTarikhShamsi; a.SanadTarikhMiladi = sanadListNahayi[j].SanadTarikhMiladi; a.SanadBedehkar = sanadListNahayi[j].SanadBedehkar; a.SabadBestankar = sanadListNahayi[j].SabadBestankar; a.SanadTozih = sanadListNahayi[j].SanadTozih; j++; });
                    db.SaveChanges();
                MessageBoxFarsi.Show($"ویرایش سند {IdSanad} با موفقیت انجام شد", "ثبت موفق");
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBoxFarsi.Show(ex.Message.ToString(), "خطا در ثبت سند");
            }
        }
    }
}
