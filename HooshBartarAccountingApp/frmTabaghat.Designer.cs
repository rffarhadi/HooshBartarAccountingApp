namespace HooshBartarAccountingApp
{
    partial class frmTabaghat
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmTabaghat));
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.label1 = new System.Windows.Forms.Label();
            this.cmbTabaghehHesab = new System.Windows.Forms.ComboBox();
            this.cmbZirTabagheh = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.btnDisplay = new System.Windows.Forms.Button();
            this.btnInsert = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnEdit = new System.Windows.Forms.Button();
            this.ChkSubDel = new System.Windows.Forms.CheckBox();
            this.chkSubEdit = new System.Windows.Forms.CheckBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.dgvTabaghat = new System.Windows.Forms.DataGridView();
            this.dgvZirTabaghat = new System.Windows.Forms.DataGridView();
            this.tableLayoutPanel1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvTabaghat)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvZirTabaghat)).BeginInit();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.groupBox1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.groupBox2, 0, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 49.27536F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.72464F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(800, 406);
            this.tableLayoutPanel1.TabIndex = 2;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.tableLayoutPanel2);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(3, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(794, 194);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "عملیات درج، ویرایش و حذف";
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 3;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 41.62521F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 58.37479F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 265F));
            this.tableLayoutPanel2.Controls.Add(this.label1, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.cmbTabaghehHesab, 0, 1);
            this.tableLayoutPanel2.Controls.Add(this.cmbZirTabagheh, 1, 1);
            this.tableLayoutPanel2.Controls.Add(this.label3, 1, 0);
            this.tableLayoutPanel2.Controls.Add(this.btnDisplay, 2, 0);
            this.tableLayoutPanel2.Controls.Add(this.btnInsert, 0, 2);
            this.tableLayoutPanel2.Controls.Add(this.btnDelete, 2, 2);
            this.tableLayoutPanel2.Controls.Add(this.btnEdit, 1, 2);
            this.tableLayoutPanel2.Controls.Add(this.ChkSubDel, 2, 3);
            this.tableLayoutPanel2.Controls.Add(this.chkSubEdit, 1, 3);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(3, 23);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 4;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 54.28571F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 45.71429F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 48F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 32F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(788, 168);
            this.tableLayoutPanel2.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label1.Location = new System.Drawing.Point(574, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(211, 47);
            this.label1.TabIndex = 0;
            this.label1.Text = "طبقه حساب دفتر کل:";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // cmbTabaghehHesab
            // 
            this.cmbTabaghehHesab.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cmbTabaghehHesab.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbTabaghehHesab.FormattingEnabled = true;
            this.cmbTabaghehHesab.Items.AddRange(new object[] {
            "دارایی",
            "بدهی",
            "حقوق مالکانه",
            "درآمد",
            "هزینه"});
            this.cmbTabaghehHesab.Location = new System.Drawing.Point(574, 50);
            this.cmbTabaghehHesab.Name = "cmbTabaghehHesab";
            this.cmbTabaghehHesab.Size = new System.Drawing.Size(211, 27);
            this.cmbTabaghehHesab.TabIndex = 3;
            this.cmbTabaghehHesab.SelectedIndexChanged += new System.EventHandler(this.cmbTabaghehHesab_SelectedIndexChanged);
            // 
            // cmbZirTabagheh
            // 
            this.cmbZirTabagheh.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cmbZirTabagheh.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbZirTabagheh.FormattingEnabled = true;
            this.cmbZirTabagheh.Location = new System.Drawing.Point(269, 50);
            this.cmbZirTabagheh.Name = "cmbZirTabagheh";
            this.cmbZirTabagheh.Size = new System.Drawing.Size(299, 27);
            this.cmbZirTabagheh.TabIndex = 4;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label3.Location = new System.Drawing.Point(269, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(299, 47);
            this.label3.TabIndex = 2;
            this.label3.Text = "زیر طبقه حساب دفتر کل:";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnDisplay
            // 
            this.btnDisplay.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnDisplay.Location = new System.Drawing.Point(3, 3);
            this.btnDisplay.Name = "btnDisplay";
            this.btnDisplay.Size = new System.Drawing.Size(260, 41);
            this.btnDisplay.TabIndex = 8;
            this.btnDisplay.Text = "نمایش";
            this.btnDisplay.UseVisualStyleBackColor = true;
            this.btnDisplay.Click += new System.EventHandler(this.btnDisplay_Click);
            // 
            // btnInsert
            // 
            this.btnInsert.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnInsert.Location = new System.Drawing.Point(574, 90);
            this.btnInsert.Name = "btnInsert";
            this.btnInsert.Size = new System.Drawing.Size(211, 42);
            this.btnInsert.TabIndex = 5;
            this.btnInsert.Text = "درج اطلاعات";
            this.btnInsert.UseVisualStyleBackColor = true;
            this.btnInsert.Click += new System.EventHandler(this.btnInsert_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnDelete.Location = new System.Drawing.Point(3, 90);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(260, 42);
            this.btnDelete.TabIndex = 6;
            this.btnDelete.Text = "حذف اطلاعات";
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnEdit
            // 
            this.btnEdit.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnEdit.Location = new System.Drawing.Point(269, 90);
            this.btnEdit.Name = "btnEdit";
            this.btnEdit.Size = new System.Drawing.Size(299, 42);
            this.btnEdit.TabIndex = 7;
            this.btnEdit.Text = "ویرایش اطلاعات";
            this.btnEdit.UseVisualStyleBackColor = true;
            this.btnEdit.Click += new System.EventHandler(this.btnEdit_Click);
            // 
            // ChkSubDel
            // 
            this.ChkSubDel.AutoSize = true;
            this.ChkSubDel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ChkSubDel.Location = new System.Drawing.Point(3, 138);
            this.ChkSubDel.Name = "ChkSubDel";
            this.ChkSubDel.Size = new System.Drawing.Size(260, 27);
            this.ChkSubDel.TabIndex = 9;
            this.ChkSubDel.Text = "حذف زیر طبقه";
            this.ChkSubDel.UseVisualStyleBackColor = true;
            // 
            // chkSubEdit
            // 
            this.chkSubEdit.AutoSize = true;
            this.chkSubEdit.Dock = System.Windows.Forms.DockStyle.Fill;
            this.chkSubEdit.Location = new System.Drawing.Point(269, 138);
            this.chkSubEdit.Name = "chkSubEdit";
            this.chkSubEdit.Size = new System.Drawing.Size(299, 27);
            this.chkSubEdit.TabIndex = 10;
            this.chkSubEdit.Text = "ویرایش زیر طبقه";
            this.chkSubEdit.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.tableLayoutPanel3);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox2.Location = new System.Drawing.Point(3, 203);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(794, 200);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "نمایش اطلاعات";
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.ColumnCount = 2;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 44.54315F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 55.45685F));
            this.tableLayoutPanel3.Controls.Add(this.dgvTabaghat, 0, 0);
            this.tableLayoutPanel3.Controls.Add(this.dgvZirTabaghat, 1, 0);
            this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel3.Location = new System.Drawing.Point(3, 23);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 1;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(788, 174);
            this.tableLayoutPanel3.TabIndex = 0;
            // 
            // dgvTabaghat
            // 
            this.dgvTabaghat.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvTabaghat.Location = new System.Drawing.Point(440, 3);
            this.dgvTabaghat.Name = "dgvTabaghat";
            this.dgvTabaghat.ReadOnly = true;
            this.dgvTabaghat.RowHeadersWidth = 62;
            this.dgvTabaghat.RowTemplate.Height = 28;
            this.dgvTabaghat.Size = new System.Drawing.Size(345, 168);
            this.dgvTabaghat.TabIndex = 0;
            this.dgvTabaghat.MouseUp += new System.Windows.Forms.MouseEventHandler(this.dgvTabaghat_MouseUp);
            // 
            // dgvZirTabaghat
            // 
            this.dgvZirTabaghat.AllowUserToAddRows = false;
            this.dgvZirTabaghat.AllowUserToDeleteRows = false;
            this.dgvZirTabaghat.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvZirTabaghat.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvZirTabaghat.Location = new System.Drawing.Point(3, 3);
            this.dgvZirTabaghat.Name = "dgvZirTabaghat";
            this.dgvZirTabaghat.ReadOnly = true;
            this.dgvZirTabaghat.RowHeadersWidth = 62;
            this.dgvZirTabaghat.RowTemplate.Height = 28;
            this.dgvZirTabaghat.Size = new System.Drawing.Size(431, 168);
            this.dgvZirTabaghat.TabIndex = 1;
            this.dgvZirTabaghat.MouseUp += new System.Windows.Forms.MouseEventHandler(this.dgvZirTabaghat_MouseUp);
            // 
            // frmTabaghat
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 19F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 406);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmTabaghat";
            this.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.Text = "معرفی طبقات حساب‌ها";
            this.Load += new System.EventHandler(this.frmTabaghat_Load);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.tableLayoutPanel3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvTabaghat)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvZirTabaghat)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cmbTabaghehHesab;
        private System.Windows.Forms.ComboBox cmbZirTabagheh;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private System.Windows.Forms.Button btnDisplay;
        private System.Windows.Forms.Button btnInsert;
        private System.Windows.Forms.Button btnEdit;
        public System.Windows.Forms.DataGridView dgvZirTabaghat;
        private System.Windows.Forms.DataGridView dgvTabaghat;
        private System.Windows.Forms.CheckBox ChkSubDel;
        private System.Windows.Forms.CheckBox chkSubEdit;
    }
}