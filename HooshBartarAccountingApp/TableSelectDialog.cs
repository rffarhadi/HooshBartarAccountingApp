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
    public partial class TableSelectDialog : Form
    {
        public TableSelectDialog()
        {
            InitializeComponent();
        }
        public TableSelectDialog(string[] tables)
        {
            InitializeComponent();

            this.listBox1.DataSource = tables;
        }

        public string Selection
        {
            get { return this.listBox1.SelectedItem as string; }
        }
    }
}
