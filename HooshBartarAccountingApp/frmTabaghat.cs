using BehComponents;
using HooshBartarAccountingApp.DatabaseModel;
using HooshBartarAccountingApp.DataShow;
using System;
using System.Collections;
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
    public partial class frmTabaghat : Form
    {
        public frmTabaghat()
        {
            InitializeComponent();
        }

        private void cmbTabaghehHesab_SelectedIndexChanged(object sender, EventArgs e)
        {
            string[] subHesabDaraei = { "حساب نقد", "حساب سرمایه‌گذاری کوتاه‌‌مدت در اوراق بهادار", "حساب دریافتنی‌ها (اشخاص)", "حساب اسناد دریافتنی", "حساب موجودی مواد و کالا", "حساب پیش‌پرداخت‌ها", "حساب سایر دارایی‌های جاری", "مشهود: حساب ساختمان", "مشهود: زمین", "مشهود: سرمایه‌گذاری بلندمدت در اوراق بهادار", "مشهود: حساب تجهیزات", "مشهود: حساب وسایل نقلیه", "مشهود: حساب سایر دارایی‌های مشهود", "نامشهود: حساب سرقفلی", "نامشهود: حساب حق امتیاز", "نامشهود: حساب سایر  دارایی‌های نامشهود" };
            string[] subHesabBedehi = { "حساب پرداختنی‌ها (اشخاص)", "حساب اسناد پرداختنی", "حساب وام بانکی کوتاه‌مدت", "حساب اوراق بدهی", "حساب اوراق مشارکت", "حساب وام بانکی بلندمدت", "حساب سایر بدهی‌های غیرجاری", };
            string[] subHesabHoghugh = { "حساب سرمایه", "حساب صرف (کسر) سرمایه", "حساب سود (زیان) انباشته", "حساب اندوخته قانونی", "حساب اندوخته طرح و توسعه", "حساب سایر حقوق مالکانه" };
            string[] subHesabDaramad = { "حساب فروش (درآمدهای عملیاتی)", "حساب سایر درآمدهای عملیاتی", "حساب سایر درآمدهای غیر عملیاتی" };
            string[] subHesabHazineh = { "حساب بهای تمام شده فروش", "حساب هزینه‌های عمومی و اداری", "حساب سایر هزینه‌های عملیاتی","هزینه‌های مالی", "حساب سایر هزینه‌های غیر عملیاتی","مالیات" };

            if (cmbTabaghehHesab.SelectedItem.ToString() == "دارایی")
            {
                cmbZirTabagheh.Items.Clear();
                cmbZirTabagheh.Items.AddRange(subHesabDaraei);
            }
            else if (cmbTabaghehHesab.SelectedItem.ToString() == "بدهی")
            {
                cmbZirTabagheh.Items.Clear();
                cmbZirTabagheh.Items.AddRange(subHesabBedehi);
            }
            else if (cmbTabaghehHesab.SelectedItem.ToString() == "حقوق مالکانه")
            {
                cmbZirTabagheh.Items.Clear();
                cmbZirTabagheh.Items.AddRange(subHesabHoghugh);
            }
            else if (cmbTabaghehHesab.SelectedItem.ToString() == "درآمد")
            {
                cmbZirTabagheh.Items.Clear();
                cmbZirTabagheh.Items.AddRange(subHesabDaramad);
            }
            else if (cmbTabaghehHesab.SelectedItem.ToString() == "هزینه")
            {
                cmbZirTabagheh.Items.Clear();
                cmbZirTabagheh.Items.AddRange(subHesabHazineh);
            }
            else
            {
                cmbZirTabagheh.Items.Clear();
            }
        }

        AccountDBEntities db = new AccountDBEntities();
        private void btnInsert_Click(object sender, EventArgs e)
        {
            try
            {
                if (cmbTabaghehHesab.Text == "")
                {
                    MessageBoxFarsi.Show("لااقل باید یک طبقه حساب انتخاب شود", "انتخاب حساب دفتر کل");
                    return;
                }
                List<tblTabaghatHesab> InsertList = new List<tblTabaghatHesab>();
                var subTabaghat = cmbZirTabagheh.Items;
                var listMojud = db.tblTabaghatHesabs.Select(a => a.ZirTzbagheyehHesab).ToList();


                foreach (var item in subTabaghat)
                {
                    foreach (var itemList in listMojud)
                    {
                        if (item.ToString()== itemList)
                        {
                            MessageBoxFarsi.Show("این زیر گروه حساب قبلاً به پایگاه داده اضافه شده است!");
                            return;
                        }
                    }
                }

                if (cmbTabaghehHesab.Text == "دارایی")
                {
                    int i = 1;
                    foreach (var item in subTabaghat)
                    {
                        tblTabaghatHesab tbl = new tblTabaghatHesab()
                        {
                            TabagheyehHesab = cmbTabaghehHesab.SelectedItem.ToString(),
                            CodeTabaghehHesab = 1,
                            ZirTzbagheyehHesab = item.ToString(),
                            CodeZirtabagheAz = Convert.ToInt32($"{i}" + $"001"),
                            CodeZirtabagheTa = Convert.ToInt32($"{i}" + $"999"),
                        };
                        i++;
                        InsertList.Add(tbl);
                    }
                    db.tblTabaghatHesabs.AddRange(InsertList);
                    db.SaveChanges();
                }
                else if (cmbTabaghehHesab.Text == "بدهی")
                {
                    int i = 1;
                    foreach (var item in subTabaghat)
                    {
                        tblTabaghatHesab tbl = new tblTabaghatHesab()
                        {
                            TabagheyehHesab = cmbTabaghehHesab.SelectedItem.ToString(),
                            CodeTabaghehHesab = 2,
                            ZirTzbagheyehHesab = item.ToString(),
                            CodeZirtabagheAz = Convert.ToInt32($"{i}" + $"001"),
                            CodeZirtabagheTa = Convert.ToInt32($"{i}" + $"999"),
                        };
                        i++;
                        InsertList.Add(tbl);
                    }
                    db.tblTabaghatHesabs.AddRange(InsertList);
                    db.SaveChanges();
                }
                else if (cmbTabaghehHesab.Text == "حقوق مالکانه")
                {
                    int i = 1;
                    foreach (var item in subTabaghat)
                    {
                        tblTabaghatHesab tbl = new tblTabaghatHesab()
                        {
                            TabagheyehHesab = cmbTabaghehHesab.SelectedItem.ToString(),
                            CodeTabaghehHesab = 3,
                            ZirTzbagheyehHesab = item.ToString(),
                            CodeZirtabagheAz = Convert.ToInt32($"{i}" + $"001"),
                            CodeZirtabagheTa = Convert.ToInt32($"{i}" + $"999"),
                        };
                        i++;
                        InsertList.Add(tbl);
                    }
                    db.tblTabaghatHesabs.AddRange(InsertList);
                    db.SaveChanges();
                }
                else if (cmbTabaghehHesab.Text == "درآمد")
                {
                    int i = 1;
                    foreach (var item in subTabaghat)
                    {
                        tblTabaghatHesab tbl = new tblTabaghatHesab()
                        {
                            TabagheyehHesab = cmbTabaghehHesab.SelectedItem.ToString(),
                            CodeTabaghehHesab = 4,
                            ZirTzbagheyehHesab = item.ToString(),
                            CodeZirtabagheAz = Convert.ToInt32($"{i}" + $"001"),
                            CodeZirtabagheTa = Convert.ToInt32($"{i}" + $"999"),
                        };
                        i++;
                        InsertList.Add(tbl);
                    }
                    db.tblTabaghatHesabs.AddRange(InsertList);
                    db.SaveChanges();
                }
                else if (cmbTabaghehHesab.Text == "هزینه")
                {
                    int i = 1;
                    foreach (var item in subTabaghat)
                    {
                        tblTabaghatHesab tbl = new tblTabaghatHesab()
                        {
                            TabagheyehHesab = cmbTabaghehHesab.SelectedItem.ToString(),
                            CodeTabaghehHesab = 5,
                            ZirTzbagheyehHesab = item.ToString(),
                            CodeZirtabagheAz = Convert.ToInt32($"{i}" + $"001"),
                            CodeZirtabagheTa = Convert.ToInt32($"{i}" + $"999"),
                        };
                        i++;
                        InsertList.Add(tbl);
                    }
                    db.tblTabaghatHesabs.AddRange(InsertList);
                    db.SaveChanges();
                }
                MessageBoxFarsi.Show("ثبت اطلاعات با موفقیت انجام شد", "‌ذخیره داده‌های ارسالی");

            }
            catch (Exception ex)
            {

                MessageBoxFarsi.Show(ex.Message.ToString(), "وجود خطا در ذخیره داده");
            }
        }

        private void frmTabaghat_Load(object sender, EventArgs e)
        {

        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                if (ChkSubDel.Checked == false)
                {
                    string Hazfi = dgvTabaghat.CurrentRow.Cells[0].Value.ToString();
                    var del = db.tblTabaghatHesabs.Where(a => a.TabagheyehHesab == Hazfi);
                    db.tblTabaghatHesabs.RemoveRange(del);
                    db.SaveChanges();
                    btnDisplay_Click(sender, e);
                    MessageBoxFarsi.Show("حذف داده با موفقیت انجام شد", "حذف موفقیت‌آمیز");
                }
                else if (ChkSubDel.Checked == true)
                {
                    string Hazfi = dgvZirTabaghat.CurrentRow.Cells[0].Value.ToString();
                    var del = db.tblTabaghatHesabs.Where(a => a.ZirTzbagheyehHesab == Hazfi);
                    db.tblTabaghatHesabs.RemoveRange(del);
                    db.SaveChanges();
                    btnDisplay_Click(sender, e);
                    MessageBoxFarsi.Show("حذف داده با موفقیت انجام شد", "حذف موفقیت‌آمیز");
                }
            }
            catch (Exception ex)
            {
                MessageBoxFarsi.Show(ex.Message.ToString(), "وجود خطا در ذخیره داده");
            }
        }

        private void btnDisplay_Click(object sender, EventArgs e)
        {
            var JadvalTabaghat = db.tblTabaghatHesabs.Select(a => new { a.TabagheyehHesab, a.CodeTabaghehHesab }).Distinct().OrderBy(a => a.CodeTabaghehHesab).ToList();
            //برای نامگذاری سرستون‌ها در کلاس مربوطه
            string[] NameSotunha = { "طبقه کل حساب‌ها", "کد طبقه حساب‌ها" };
            Display calledDgv = new Display();
            calledDgv.ShowDgv(dgvTabaghat, NameSotunha, JadvalTabaghat);
            dgvTabaghat = calledDgv.dgvFromDisplay;

            // مقدار دهی گرید دوم
            var JadvalHesabha = db.tblTabaghatHesabs.Select(a => new { a.ZirTzbagheyehHesab, a.CodeZirtabagheAz, a.CodeZirtabagheTa }).Distinct().OrderBy(a => a.ZirTzbagheyehHesab).ToList();
            //برای نامگذاری سرستون‌ها در کلاس مربوطه
            string[] NameSotunha2 = { "نام دفتر کل", "از کد", "تا کد" };
            Display calledDgv2 = new Display();
            calledDgv2.ShowDgv(dgvZirTabaghat, NameSotunha2, JadvalHesabha);
            dgvZirTabaghat = calledDgv2.dgvFromDisplay;
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            try
            {
                if (cmbTabaghehHesab.DropDownStyle == ComboBoxStyle.DropDownList && cmbZirTabagheh.DropDownStyle == ComboBoxStyle.DropDownList)
                {
                    MessageBoxFarsi.Show("هیچ سطری از دیتاگرید جهت ویرایش انتخاب نشده است", "عدم انتخاب داده");
                    return;
                }
                else if (chkSubEdit.Checked == false)
                {
                    string EditItem = dgvTabaghat.CurrentRow.Cells[0].Value.ToString();
                    var editList = db.tblTabaghatHesabs.Where(a => a.TabagheyehHesab == EditItem).ToList();
                    editList.ForEach(a => { a.TabagheyehHesab = cmbTabaghehHesab.Text; });
                    db.SaveChanges();
                    btnDisplay_Click(sender, e);
                    MessageBoxFarsi.Show("ویرایش طبقه حساب با موفقیت انجام شد", "ویرایش موفقیت‌آمیز");
                }
                else if (chkSubEdit.Checked == true)
                {
                    string EditItem = dgvZirTabaghat.CurrentRow.Cells[0].Value.ToString();
                    var editList = db.tblTabaghatHesabs.Where(a => a.ZirTzbagheyehHesab == EditItem).ToList();
                    editList.ForEach(a => { a.ZirTzbagheyehHesab = cmbZirTabagheh.Text; });
                    db.SaveChanges();
                    btnDisplay_Click(sender, e);
                    MessageBoxFarsi.Show("ویرایش زیر طبقه مزبور با موفقیت انجام شد", "ویرایش موفقیت‌آمیز");
                }
                cmbTabaghehHesab.DropDownStyle = ComboBoxStyle.DropDownList;
                cmbZirTabagheh.DropDownStyle = ComboBoxStyle.DropDownList;
            }
            catch (Exception ex)
            {
                MessageBoxFarsi.Show(ex.Message.ToString(), "وجود خطا در ذخیره داده");
            }
        }

        private void dgvTabaghat_MouseUp(object sender, MouseEventArgs e)
        {
            string EditRow = dgvTabaghat.CurrentRow.Cells[0].Value.ToString();
            cmbTabaghehHesab.DropDownStyle = ComboBoxStyle.DropDown;
            cmbZirTabagheh.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbTabaghehHesab.Text = EditRow;
        }

        private void dgvZirTabaghat_MouseUp(object sender, MouseEventArgs e)
        {
            string EditRow = dgvZirTabaghat.CurrentRow.Cells[0].Value.ToString();
            cmbZirTabagheh.DropDownStyle = ComboBoxStyle.DropDown;
            cmbTabaghehHesab.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbZirTabagheh.Text = EditRow;
        }

        //void d()
        //{

        //    ArrayList myArryList = new ArrayList();
        //    myArryList.Add(100);
        //    myArryList.Add("Hello World");
        //    myArryList.Add(300);
        //    Console.WriteLine(myArryList.Contains(100));

        //}
    }
}
