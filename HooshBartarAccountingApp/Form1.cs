using BehComponents;
using HooshBartarAccountingApp.DatabaseModel;
using Microsoft.SqlServer.Management.Common;
using Microsoft.SqlServer.Management.Smo;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HooshBartarAccountingApp
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void TsmItem_Click(object sender, EventArgs e)
        {
            try
            {
                this.Hide();
                frmTabaghat frm = new frmTabaghat();
                frm.ShowDialog();
                this.Show();
            }
            catch (Exception ex)
            {
                MessageBoxFarsi.Show("خطائی رخ داده است" + "\n" + "\n" + ex.Message.ToString(), "پیغام خطا", MessageBoxFarsiButtons.OK, MessageBoxFarsiIcon.Error, MessageBoxFarsiDefaultButton.Button1);
            }
        }

        private void tsmHesabha_Click(object sender, EventArgs e)
        {
            try
            {
                this.Hide();
                frmHesabha frm = new frmHesabha();
                frm.ShowDialog();
                this.Show();
            }
            catch (Exception ex)
            {
                MessageBoxFarsi.Show("خطائی رخ داده است" + "\n" + "\n" + ex.Message.ToString(), "پیغام خطا", MessageBoxFarsiButtons.OK, MessageBoxFarsiIcon.Error, MessageBoxFarsiDefaultButton.Button1);
            }

        }

        private void سندمالیسادهToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                this.Hide();
                frmSabtSanad frm = new frmSabtSanad();
                frm.ShowDialog();
                this.Show();
            }
            catch (Exception ex)
            {
                MessageBoxFarsi.Show("خطائی رخ داده است" + "\n" + "\n" + ex.Message.ToString(), "پیغام خطا", MessageBoxFarsiButtons.OK, MessageBoxFarsiIcon.Error, MessageBoxFarsiDefaultButton.Button1);
            }
        }

        private void فاکتورخریدToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                this.Hide();
                frmBuySellFactor frm = new frmBuySellFactor();
                frm.chkBuyFactor.Checked = true;
                frm.ShowDialog();
                this.Show();
            }
            catch (Exception ex)
            {
                MessageBoxFarsi.Show("خطائی رخ داده است" + "\n" + "\n" + ex.Message.ToString(), "پیغام خطا", MessageBoxFarsiButtons.OK, MessageBoxFarsiIcon.Error, MessageBoxFarsiDefaultButton.Button1);
            }
        }

        private void فاکتورفروشToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                this.Hide();
                frmBuySellFactor frm = new frmBuySellFactor();
                frm.chkSellFactor.Checked = true;
                frm.ShowDialog();
                this.Show();
            }
            catch (Exception ex)
            {
                MessageBoxFarsi.Show("خطائی رخ داده است" + "\n" + "\n" + ex.Message.ToString(), "پیغام خطا", MessageBoxFarsiButtons.OK, MessageBoxFarsiIcon.Error, MessageBoxFarsiDefaultButton.Button1);
            }
        }

        private void BackUp(string filename)
        {
            SqlConnection con = null;
            try
            {
                string cmd = @"Backup DataBase [" + Application.StartupPath + "\\AccountDB.mdf] To Disk=N'" + filename + "'";
                SqlCommand sqlCmd = null;
                var conStr = db.Database.Connection.ConnectionString;

                con = new SqlConnection(conStr);
                if (con.State != ConnectionState.Open)
                    con.Open();
                sqlCmd = new SqlCommand(cmd, con);
                sqlCmd.CommandTimeout = 300000;

                sqlCmd.ExecuteNonQuery();
                MessageBoxFarsi.Show("عملیات پشتیبان‌گیری با موفقیت انجام شد", "عملیات BackUp", MessageBoxFarsiButtons.OK, MessageBoxFarsiIcon.Information, MessageBoxFarsiDefaultButton.Button1);

            }
            catch (Exception ex)
            {
                MessageBoxFarsi.Show("خطائی رخ داده است" + "\n" + "\n" + ex.Message.ToString(), "پیغام خطا", MessageBoxFarsiButtons.OK, MessageBoxFarsiIcon.Information, MessageBoxFarsiDefaultButton.Button1);
            }
            finally
            {
                con.Close();
            }
        }
        private void Restore(string filename)
        {
            SqlConnection con = null;
            try
            {
                string cmd = @"ALTER DATABASE [" + Application.StartupPath + "\\AccountDB.mdf] SET SINGLE_USER with ROLLBACK IMMEDIATE " + "USE master" + @" RESTORE DATABASE [" + Application.StartupPath + "\\AccountDB.mdf] FROM DISK =N'" + filename + "'WITH RECOVERY,REPLACE";
                SqlCommand sqlCmd = null;
                var conStr = db.Database.Connection.ConnectionString;
                con = new SqlConnection(conStr);
                if (con.State != ConnectionState.Open)
                    con.Open();
                sqlCmd = new SqlCommand(cmd, con);
                sqlCmd.CommandTimeout = 300000;

                sqlCmd.ExecuteNonQuery();
                MessageBoxFarsi.Show("عملیات بازیابی با موفقیت انجام شد", "عملیات BackUp", MessageBoxFarsiButtons.OK, MessageBoxFarsiIcon.Information, MessageBoxFarsiDefaultButton.Button1);

            }
            catch (Exception ex)
            {
                MessageBoxFarsi.Show("خطائی رخ داده است" + "\n" + "\n" + ex.Message.ToString(), "پیغام خطا", MessageBoxFarsiButtons.OK, MessageBoxFarsiIcon.Information, MessageBoxFarsiDefaultButton.Button1);
            }
            finally
            {
                con.Close();
            }
        }

        AccountDBEntities db = new AccountDBEntities();

        private void بازیابیToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                string filename = string.Empty;

                OpenFileDialog ofd = new OpenFileDialog();
                ofd.Filter = @"SQL Backup Files(*.*) |*.*| (*.Bak) |*.Bak";
                ofd.DefaultExt = "Bak";
                ofd.FilterIndex = 1;
                ofd.FileName = DateTime.Now.ToString("dd-MM-yyyy_HH-mm-ss");
                ofd.Title = "Restore DataBase of Application";
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    filename = ofd.FileName;
                    Restore(filename);
                }
            }
            catch (Exception ex)
            {
                MessageBoxFarsi.Show("خطائی رخ داده است" + "\n" + "\n" + ex.Message.ToString(), "پیغام خطا", MessageBoxFarsiButtons.OK, MessageBoxFarsiIcon.Information, MessageBoxFarsiDefaultButton.Button1);
            }

            //try
            //{
            //برای کار با غیر لوکال
            //OpenFileDialog ofd = new OpenFileDialog();
            //ofd.Filter = @"SQL Backup Files(*.*) |*.*| (*.Bak) |*.Bak";
            //ofd.DefaultExt = "Bak";
            //ofd.FilterIndex = 1;
            //ofd.Title = "بازیابی";
            //if (ofd.ShowDialog() == DialogResult.OK)
            //{
            //    string mdfLocation = Application.StartupPath + "\\AccountDB.mdf";
            //    string ldfLocation = Application.StartupPath + "\\AccountDB_log.ldf";
            //    string filename = ofd.FileName;
            //    //if (MessageBoxFarsi.Show("آیا برای اولین بار است که می‌خواهید بازیابی انجام دهید؟", "", MessageBoxButtons.YesNo) == DialogResult.Yes)
            //    //{
            //    //    //برای sql معمولی
            //    //    string conS = "data source=.;initial catalog=master;integrated security=True";


            //    //    //string conS = @"data source=.\SQLExpress;initial catalog=master;integrated security=True";
            //    //    string commandText2 = @"CREATE DATABASE [AccountDB] ON ( FILENAME = N'" + mdfLocation + "' ), ( FILENAME = N'" + ldfLocation + "' ) FOR ATTACH";
            //    //    db.Database.CommandTimeout = 5000;
            //    //    SqlConnection con2 = new SqlConnection(conS);
            //    //    if (con2.State != ConnectionState.Open)
            //    //    {
            //    //        con2.Open();
            //    //        SqlCommand sqlCmd2 = new SqlCommand(commandText2, con2);
            //    //        sqlCmd2.ExecuteNonQuery();
            //    //        con2.Close();
            //    //    }
            //    //}

            //    string commandText = @"alter database [AccountDB] set offline with rollback immediate RESTORE DATABASE [AccountDB] FILE = N'AccountDB' FROM  DISK = N'" + filename + "' WITH  FILE = 1,  NOUNLOAD,  REPLACE,  STATS = 10 alter database [AccountDB] set online";
            //    string conString = db.Database.Connection.ConnectionString;
            //    db.Database.CommandTimeout = 5000;
            //    SqlConnection con = new SqlConnection(conString);
            //    if (con.State != ConnectionState.Open)
            //    {
            //        con.Open();
            //        SqlCommand sqlCmd3 = new SqlCommand(commandText, con);
            //        sqlCmd3.ExecuteNonQuery();
            //        con.Close();
            //    }
            //    MessageBoxFarsi.Show("عملیات بازیابی با موفقیت انجام شد", "عملیات بازیابی");
            //}
            //}
            //catch (Exception ex)
            //{
            //    MessageBoxFarsi.Show(ex.Message.ToString(), "خطاء در بازیابی بانک اطلاعاتی");
            //}
        }

        private void تنظیماتToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                this.Hide();
                frmSetting frm = new frmSetting();
                frm.ShowDialog();
                this.Show();
            }
            catch (Exception ex)
            {
                MessageBoxFarsi.Show("خطائی رخ داده است" + "\n" + "\n" + ex.Message.ToString(), "پیغام خطا", MessageBoxFarsiButtons.OK, MessageBoxFarsiIcon.Error, MessageBoxFarsiDefaultButton.Button1);
            }
        }

        private void ترازنامهToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                this.Hide();
                frmTaraznameh frm = new frmTaraznameh();
                frm.ShowDialog();
                this.Show();
            }
            catch (Exception ex)
            {
                MessageBoxFarsi.Show("خطائی رخ داده است" + "\n" + "\n" + ex.Message.ToString(), "پیغام خطا", MessageBoxFarsiButtons.OK, MessageBoxFarsiIcon.Error, MessageBoxFarsiDefaultButton.Button1);
            }
        }

        private void صورتسودوزیانToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                this.Hide();
                frmSoodVaZiyan frm = new frmSoodVaZiyan();
                frm.ShowDialog();
                this.Show();
            }
            catch (Exception ex)
            {
                MessageBoxFarsi.Show("خطائی رخ داده است" + "\n" + "\n" + ex.Message.ToString(), "پیغام خطا", MessageBoxFarsiButtons.OK, MessageBoxFarsiIcon.Error, MessageBoxFarsiDefaultButton.Button1);
            }
        }

        private void بارگزاریبانکاطلاعاتیToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                string mdfLocation = Application.StartupPath + "\\AccountDB.mdf";
                string ldfLocation = Application.StartupPath + "\\AccountDB.ldf";
                string conString = @"Data Source=.;AttachDbFilename=|DataDirectory|\AccountDB.mdf;integrated security=true";
                string commandText = "CREATE DATABASE [AccountDB] ON ( FILENAME = N'" + mdfLocation + "'),( FILENAME = N'" + ldfLocation + "') FOR ATTACH";
                db.Database.CommandTimeout = 5000;
                SqlConnection con = new SqlConnection(conString);
                if (con.State != ConnectionState.Open)
                {
                    con.Open();
                    SqlCommand sqlCmd = new SqlCommand(commandText, con);
                    sqlCmd.ExecuteNonQuery();
                    con.Close();
                }
                MessageBoxFarsi.Show("عملیات بارگزاری بانک اطلاعاتی با موفقیت انجام شد", "عملیات بارگزاری");
            }

            catch (Exception ex)
            {

                MessageBoxFarsi.Show(ex.Message.ToString(), "خطاء در بازیابی بانک اطلاعاتی");
            }
        }

        private void بارگزاریToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                string mdfLocation = Application.StartupPath + "\\AccountDB.mdf";
                string ldfLocation = Application.StartupPath + "\\AccountDB_log.ldf";
                string conString = db.Database.Connection.ConnectionString;
                string commandText = @"USE [master] GO CREATE DATABASE[G:\BOOKS\C#.NETACCOUNTIN\ACCOUNTINGAPP\ACCOUNTINGAPP\BIN\DEBUG\ACCOUNTDB.MDF] ON (FILENAME = N'G:\books\C#.NetAccountin\AccountingApp\AccountingApp\bin\Debug\AccountDB.mdf'),(FILENAME = N'G:\books\C#.NetAccountin\AccountingApp\AccountingApp\bin\Debug\AccountDB_log.ldf' ) FOR ATTACH GO";
                db.Database.CommandTimeout = 5000;
                db.Database.Connection.Open();
                var x = db.Database.ExecuteSqlCommand(commandText);

                //SqlConnection con = new SqlConnection(conString);
                //if (con.State != ConnectionState.Open)
                //{
                //    con.Open();
                //    SqlCommand sqlCmd = new SqlCommand(commandText, con);
                //    sqlCmd.ExecuteNonQuery();
                //    con.Close();
                //}
                MessageBoxFarsi.Show("عملیات بارگزاری بانک اطلاعاتی با موفقیت انجام شد", "عملیات بارگزاری");
            }

            catch (Exception ex)
            {
                MessageBoxFarsi.Show("خطا در بازیابی رخ داده است" + "\n" + "\n" + ex.Message.ToString(), "پیغام خطا", MessageBoxFarsiButtons.OK, MessageBoxFarsiIcon.Error, MessageBoxFarsiDefaultButton.Button1);
            }
        }

        private void tsmMoein_Click(object sender, EventArgs e)
        {
            try
            {
                this.Hide();
                frmReporting frm = new frmReporting();
                frm.ShowDialog();
                this.Show();
            }
            catch (Exception ex)
            {
                MessageBoxFarsi.Show("خطائی رخ داده است" + "\n" + "\n" + ex.Message.ToString(), "پیغام خطا", MessageBoxFarsiButtons.OK, MessageBoxFarsiIcon.Information, MessageBoxFarsiDefaultButton.Button1);
            }
        }

        private void tsmDafater_Click(object sender, EventArgs e)
        {
            try
            {
                this.Hide();
                frmDafater frm = new frmDafater();
                frm.ShowDialog();
                this.Show();
            }
            catch (Exception ex)
            {
                MessageBoxFarsi.Show("خطائی رخ داده است" + "\n" + "\n" + ex.Message.ToString(), "پیغام خطا", MessageBoxFarsiButtons.OK, MessageBoxFarsiIcon.Information, MessageBoxFarsiDefaultButton.Button1);
            }
        }

        private void tsmBakup_Click(object sender, EventArgs e)
        {
            try
            {
                SaveFileDialog sfd = new SaveFileDialog();
                string filename = string.Empty;
                sfd.Filter = @"SQL Backup Files(*.*) |*.*| (*.Bak) |*.Bak";
                sfd.DefaultExt = "Bak";
                sfd.FilterIndex = 1;
                sfd.FileName = DateTime.Now.ToString("dd-MM-yyyy_HH-mm-ss");
                sfd.Title = "Backup DataBase of Application";
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    filename = sfd.FileName;
                    BackUp(filename);
                }
            }
            catch (Exception ex)
            {
                MessageBoxFarsi.Show("خطائی رخ داده است" + "\n" + "\n" + ex.Message.ToString(), "پیغام خطا", MessageBoxFarsiButtons.OK, MessageBoxFarsiIcon.Information, MessageBoxFarsiDefaultButton.Button1);
            }
            //try
            //{
            //برای کار با غیر لوکال
            ////SaveFileDialog sfd = new SaveFileDialog();
            ////sfd.Filter = @"SQL Backup Files(*.*) |*.*| (*.Bak) |*.Bak";
            ////sfd.DefaultExt = "Bak";
            ////sfd.FilterIndex = 1;
            ////sfd.FileName = "AccountDB_" + DateTime.Now.ToString("dd-MM-yyyy_HH-mm-ss");
            ////sfd.Title = "پشتبان‌گیری";
            ////if (sfd.ShowDialog() == DialogResult.OK)
            ////{
            ////    string filename = sfd.FileName;
            ////    string conString = db.Database.Connection.ConnectionString;
            ////    string commandText = "BACKUP DATABASE [AccountDB] TO  DISK = N'" + filename + "' WITH NOFORMAT, INIT,  NAME = N'AccountDB-Full Database Backup', SKIP, NOREWIND, NOUNLOAD,  STATS = 10";
            ////    db.Database.CommandTimeout = 5000;
            ////    SqlConnection con = new SqlConnection(conString);
            ////    if (con.State != ConnectionState.Open)
            ////    {
            ////        con.Open();
            ////        SqlCommand sqlCmd = new SqlCommand(commandText, con);
            ////        sqlCmd.ExecuteNonQuery();
            ////        con.Close();
            ////    }
            ////    MessageBoxFarsi.Show("عملیات پشتیبان‌گیری با موفقیت انجام شد", "عملیات بکاپ");
            ////}

            //}
            //catch (Exception ex)
            //{

            //    MessageBoxFarsi.Show(ex.Message.ToString(), "خطاء در پشتیبان‌گیری");
            //}
        }


        private void خروجToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBoxFarsi.Show(ex.Message.ToString(), "خطاء");
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                var res = MessageBoxFarsi.Show("آیا می‌خواهید از پایگاه داده خود فایل بکاپ تهیه کنید؟", "پشتیبان‌گیری", MessageBoxFarsiButtons.YesNoCancel, MessageBoxFarsiIcon.Question);
                if (res == DialogResult.Yes)
                {
                    tsmBakup_Click(sender, e);
                }
                if (res == DialogResult.Cancel)
                {
                    e.Cancel = true;
                    return;
                }
            }
            catch (Exception ex)
            {
                MessageBoxFarsi.Show(ex.Message.ToString(), "خطاء");
            }
        }

        private void tsmAfzayeshSarmayeh_Click(object sender, EventArgs e)
        {
            try
            {
                this.Hide();
                frmAfzayeshSarmayeh frm = new frmAfzayeshSarmayeh();
                frm.rbnVotingRight.Checked = true;
                frm.ShowDialog();
                this.Show();
            }
            catch (Exception ex)
            {
                MessageBoxFarsi.Show("خطائی رخ داده است" + "\n" + "\n" + ex.Message.ToString(), "پیغام خطا", MessageBoxFarsiButtons.OK, MessageBoxFarsiIcon.Error, MessageBoxFarsiDefaultButton.Button1);
            }
        }
    }
}
