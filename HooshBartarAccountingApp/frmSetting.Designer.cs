namespace HooshBartarAccountingApp
{
    partial class frmSetting
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmSetting));
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.dgvSetting = new System.Windows.Forms.DataGridView();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.numBuyKarmozd = new System.Windows.Forms.NumericUpDown();
            this.numSellKarmozd = new System.Windows.Forms.NumericUpDown();
            this.numArzeshAfzudehTax = new System.Windows.Forms.NumericUpDown();
            this.namEnteghaLTax = new System.Windows.Forms.NumericUpDown();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.btnEdit = new System.Windows.Forms.Button();
            this.btnSabt = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSetting)).BeginInit();
            this.tableLayoutPanel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numBuyKarmozd)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numSellKarmozd)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numArzeshAfzudehTax)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.namEnteghaLTax)).BeginInit();
            this.tableLayoutPanel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.dgvSetting, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel2, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel3, 0, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 28.03738F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 71.96262F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 252F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(907, 360);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // dgvSetting
            // 
            this.dgvSetting.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvSetting.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvSetting.Location = new System.Drawing.Point(3, 110);
            this.dgvSetting.Name = "dgvSetting";
            this.dgvSetting.RowHeadersWidth = 62;
            this.dgvSetting.RowTemplate.Height = 28;
            this.dgvSetting.Size = new System.Drawing.Size(901, 247);
            this.dgvSetting.TabIndex = 0;
            this.dgvSetting.MouseUp += new System.Windows.Forms.MouseEventHandler(this.dgvSetting_MouseUp);
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 8;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 48.92704F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 51.07296F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 116F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 94F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 156F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 93F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 148F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 95F));
            this.tableLayoutPanel2.Controls.Add(this.label1, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.label2, 2, 0);
            this.tableLayoutPanel2.Controls.Add(this.label3, 4, 0);
            this.tableLayoutPanel2.Controls.Add(this.label4, 6, 0);
            this.tableLayoutPanel2.Controls.Add(this.numBuyKarmozd, 1, 0);
            this.tableLayoutPanel2.Controls.Add(this.numSellKarmozd, 3, 0);
            this.tableLayoutPanel2.Controls.Add(this.numArzeshAfzudehTax, 5, 0);
            this.tableLayoutPanel2.Controls.Add(this.namEnteghaLTax, 7, 0);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 1;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(901, 24);
            this.tableLayoutPanel2.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label1.Location = new System.Drawing.Point(807, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(91, 24);
            this.label1.TabIndex = 4;
            this.label1.Text = "کارمزد خرید:";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label2.Location = new System.Drawing.Point(590, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(110, 24);
            this.label2.TabIndex = 5;
            this.label2.Text = "کارمزد فروش:";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label3.Location = new System.Drawing.Point(340, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(150, 24);
            this.label3.TabIndex = 6;
            this.label3.Text = "مالیات ارزش افزوده:";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label4.Location = new System.Drawing.Point(99, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(142, 24);
            this.label4.TabIndex = 7;
            this.label4.Text = "مالیات نقل و انتقال:";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // numBuyKarmozd
            // 
            this.numBuyKarmozd.DecimalPlaces = 5;
            this.numBuyKarmozd.Dock = System.Windows.Forms.DockStyle.Fill;
            this.numBuyKarmozd.Increment = new decimal(new int[] {
            1,
            0,
            0,
            262144});
            this.numBuyKarmozd.Location = new System.Drawing.Point(706, 3);
            this.numBuyKarmozd.Maximum = new decimal(new int[] {
            90,
            0,
            0,
            131072});
            this.numBuyKarmozd.Name = "numBuyKarmozd";
            this.numBuyKarmozd.Size = new System.Drawing.Size(95, 27);
            this.numBuyKarmozd.TabIndex = 8;
            // 
            // numSellKarmozd
            // 
            this.numSellKarmozd.DecimalPlaces = 5;
            this.numSellKarmozd.Dock = System.Windows.Forms.DockStyle.Fill;
            this.numSellKarmozd.Increment = new decimal(new int[] {
            1,
            0,
            0,
            262144});
            this.numSellKarmozd.Location = new System.Drawing.Point(496, 3);
            this.numSellKarmozd.Maximum = new decimal(new int[] {
            90,
            0,
            0,
            131072});
            this.numSellKarmozd.Name = "numSellKarmozd";
            this.numSellKarmozd.Size = new System.Drawing.Size(88, 27);
            this.numSellKarmozd.TabIndex = 9;
            // 
            // numArzeshAfzudehTax
            // 
            this.numArzeshAfzudehTax.DecimalPlaces = 5;
            this.numArzeshAfzudehTax.Dock = System.Windows.Forms.DockStyle.Fill;
            this.numArzeshAfzudehTax.Increment = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.numArzeshAfzudehTax.Location = new System.Drawing.Point(247, 3);
            this.numArzeshAfzudehTax.Maximum = new decimal(new int[] {
            90,
            0,
            0,
            131072});
            this.numArzeshAfzudehTax.Name = "numArzeshAfzudehTax";
            this.numArzeshAfzudehTax.Size = new System.Drawing.Size(87, 27);
            this.numArzeshAfzudehTax.TabIndex = 10;
            // 
            // namEnteghaLTax
            // 
            this.namEnteghaLTax.DecimalPlaces = 5;
            this.namEnteghaLTax.Dock = System.Windows.Forms.DockStyle.Fill;
            this.namEnteghaLTax.Increment = new decimal(new int[] {
            1,
            0,
            0,
            262144});
            this.namEnteghaLTax.Location = new System.Drawing.Point(3, 3);
            this.namEnteghaLTax.Maximum = new decimal(new int[] {
            90,
            0,
            0,
            131072});
            this.namEnteghaLTax.Name = "namEnteghaLTax";
            this.namEnteghaLTax.Size = new System.Drawing.Size(90, 27);
            this.namEnteghaLTax.TabIndex = 11;
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.ColumnCount = 4;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 46.76923F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 53.23077F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 209F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 294F));
            this.tableLayoutPanel3.Controls.Add(this.btnEdit, 0, 0);
            this.tableLayoutPanel3.Controls.Add(this.btnSabt, 0, 0);
            this.tableLayoutPanel3.Controls.Add(this.btnDelete, 1, 0);
            this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel3.Location = new System.Drawing.Point(3, 33);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 1;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(901, 71);
            this.tableLayoutPanel3.TabIndex = 2;
            // 
            // btnEdit
            // 
            this.btnEdit.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnEdit.Location = new System.Drawing.Point(507, 3);
            this.btnEdit.Name = "btnEdit";
            this.btnEdit.Size = new System.Drawing.Size(205, 65);
            this.btnEdit.TabIndex = 2;
            this.btnEdit.Text = "ویرایش";
            this.btnEdit.UseVisualStyleBackColor = true;
            this.btnEdit.Click += new System.EventHandler(this.btnEdit_Click);
            // 
            // btnSabt
            // 
            this.btnSabt.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnSabt.Location = new System.Drawing.Point(718, 3);
            this.btnSabt.Name = "btnSabt";
            this.btnSabt.Size = new System.Drawing.Size(180, 65);
            this.btnSabt.TabIndex = 0;
            this.btnSabt.Text = "ثبت";
            this.btnSabt.UseVisualStyleBackColor = true;
            this.btnSabt.Click += new System.EventHandler(this.btnSabt_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnDelete.Location = new System.Drawing.Point(298, 3);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(203, 65);
            this.btnDelete.TabIndex = 1;
            this.btnDelete.Text = "حذف";
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // frmSetting
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 19F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(907, 360);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmSetting";
            this.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "تنظیمات";
            this.Load += new System.EventHandler(this.frmSetting_Load);
            this.tableLayoutPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvSetting)).EndInit();
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numBuyKarmozd)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numSellKarmozd)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numArzeshAfzudehTax)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.namEnteghaLTax)).EndInit();
            this.tableLayoutPanel3.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.DataGridView dgvSetting;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.NumericUpDown numBuyKarmozd;
        private System.Windows.Forms.NumericUpDown numSellKarmozd;
        private System.Windows.Forms.NumericUpDown numArzeshAfzudehTax;
        private System.Windows.Forms.NumericUpDown namEnteghaLTax;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private System.Windows.Forms.Button btnSabt;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnEdit;
    }
}