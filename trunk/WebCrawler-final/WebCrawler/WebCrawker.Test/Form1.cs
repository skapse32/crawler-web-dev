using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using WebCrawler.Core;

namespace WebCrawker.Test
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            FTPExtension ex = new FTPExtension();
            ex.UploadFile("ftp://31.170.165.24", "u131915856", "16011991", "test1", "db.xls", @"C:\Users\Thanh\Desktop\db1.xls");
        }
    }
}
