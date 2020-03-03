using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Cheese_Factory
{
    public partial class QM_CheckIncomingSupplies : Form
    {
        CheeseEntities cheese = new CheeseEntities();
        public QM_CheckIncomingSupplies()
        {
            InitializeComponent();
            loadData();
            clear();
            EnabledStatus(false);
            ButtonStatus(true);
        }

        private void clear()
        {
            textBox2.Text = "";
            label10.Visible = false;
        }

        private void loadData()
        {
            var goodsReceipt = from x in cheese.GoodsReceipts
                               //where (x.GoodsReceiptDescription !="No Description")
                               select new { x.SuppliesTransactionID, x.GoodsReceiptID, x.GoodsReceiptDescription };
            dataGridView1.DataSource = goodsReceipt.ToList();
            if (goodsReceipt != null)
            {
                dataGridView1.Enabled = true;
            }
            else
            {
                dataGridView1.Enabled = false;
            }
        }

        private void ButtonStatus(bool status)
        {
            button1.Enabled = status;
            button2.Enabled = !status;
            button3.Enabled = !status;
        }

        private void EnabledStatus(bool status)
        {
            textBox1.Enabled = false;
            textBox2.Enabled = status;
        }

        private void BtnDisabled()
        {
            button1.Enabled = false;
            button2.Enabled = false;
            button3.Enabled = false;
            textBox1.Enabled = false;
            textBox2.Enabled = false;
        }

        public string generateGoodsReceiptCheckID()
        {
            string newID = "";
            var goodsReceiptCheck = from temp in cheese.GoodsReceiptChecks select temp;
            var countOfRows = goodsReceiptCheck.Count();

            if (countOfRows != 0)
            {
                var data = goodsReceiptCheck.ToList();
                var lastRow = data.ElementAt(countOfRows - 1);
                string curr = lastRow.GoodsReceiptCheckID;
                string prefix = curr.Substring(0, 3);
                string num = curr.Substring(3, 4);
                int id = Int32.Parse(num);
                id++;
                if (id < 10000)
                {
                    newID = prefix + "000" + id;

                }
                else if (id < 1000)
                {
                    newID = prefix + "00" + id;

                }
                else if (id < 100)
                {
                    newID = prefix + "0" + id;
                }
                else if (id < 10)
                {
                    newID = prefix + id;
                }
                return newID;
            }
            else
            {
                return "GRC0001";
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {

            if (textBox1.Text == "")
            {
                label10.Text = "You Must Choose Goods Receipt First";
                label10.Visible = true;
            }
            else
            {
                clear();
                EnabledStatus(true);
                ButtonStatus(false);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (textBox1.Text != "")
            {
                GoodsReceipt gR = (from x in cheese.GoodsReceipts
                                   where x.GoodsReceiptID.Equals(textBox1.Text)
                                   select x).First();
                if (gR != null)
                {

                    var dGR = from x in cheese.DetailGoodsReceipts
                              where x.GoodsReceiptID.Equals(textBox1.Text)
                              select x;

                    gR.GoodsReceiptDescription = textBox2.Text;
                    cheese.SaveChanges();
                    GoodsReceiptCheck gRC = new GoodsReceiptCheck();
                    string newID = generateGoodsReceiptCheckID();
                    gRC.GoodsReceiptID = textBox1.Text;
                    gRC.GoodsReceiptCheckID = newID;
                    gRC.CreatedBy = _MainForm.userID;
                    gRC.GoodsReceiptCheckDate = DateTime.Now;
                    cheese.GoodsReceiptChecks.Add(gRC);

                    //cheese.SaveChanges();
                    foreach (DetailGoodsReceipt detail in dGR)
                    {
                        DetailGoodsReceiptCheck dGRC = new DetailGoodsReceiptCheck();
                        dGRC.GoodsReceiptCheckID = newID;
                        dGRC.MilkID = detail.MilkID.ToString();
                        dGRC.Quantity = (Int32)detail.Quantity;
                        cheese.DetailGoodsReceiptChecks.Add(dGRC);
                        //cheese.SaveChanges();
                    }
                    cheese.SaveChanges();
                    textBox1.Text = "";
                    MessageBox.Show("Data Updated");
                    loadData();
                    clear();
                    EnabledStatus(false);
                    ButtonStatus(true);
                    label10.Visible = false;
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            clear();
            textBox1.Text = "";
            EnabledStatus(false);
            ButtonStatus(true);
            label10.Visible = false;
            dataGridView2.DataSource = null;
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridView1.RowCount > 0)
            {
                textBox1.Text = dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString();
                textBox2.Text = dataGridView1.Rows[e.RowIndex].Cells[2].Value.ToString();
            }
            if (textBox1.Text != "")
            {
                var detailGoodsReceipt = from x in cheese.DetailGoodsReceipts
                                                where x.GoodsReceiptID == textBox1.Text
                                                select new { x.MilkID, x.Quantity};
                dataGridView2.DataSource = detailGoodsReceipt.ToList();
                if (detailGoodsReceipt != null)
                {
                    dataGridView2.Enabled = true;
                }
                else
                {
                    dataGridView2.Enabled = false;
                }

                if (textBox2.Text != "No Description")
                {
                    BtnDisabled();
                }
                else
                {
                    label10.Visible = false;
                }
            }
        }

    }
}
