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
    public partial class IS_ViewPurchaseOrder : Form
    {
        CheeseEntities cheese = new CheeseEntities();

        public IS_ViewPurchaseOrder()
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
            var suppliesTransaction = from x in cheese.SuppliesTransactions
                                      select new { x.SuppliesTransactionID, x.SuppliesTransactionDate, x.SuppliesTransactionDescription, x.SuppliesTransactionStatus };
            dataGridView1.DataSource = suppliesTransaction.ToList();
            if (suppliesTransaction != null)
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
            button4.Enabled = status;
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
            button4.Enabled = false;
            textBox1.Enabled = false;
            textBox2.Enabled = false;
        }


        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridView1.RowCount>0)
            {
                textBox1.Text = dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString();
                textBox2.Text = dataGridView1.Rows[e.RowIndex].Cells[2].Value.ToString();
            }
            if (textBox1.Text != "") { 
            var detailSuppliesTransaction = from x in cheese.DetailSuppliesTransactions 
                                            where x.SuppliesTransactionID == textBox1.Text
                                            select new { x.MilkID, x.Price, x.Quantity, x.VendorID };
            dataGridView2.DataSource = detailSuppliesTransaction.ToList();
            if (detailSuppliesTransaction != null)
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


        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "")
            {
                label10.Text = "You Must Choose Transaction First";
                label10.Visible = true;
            }
            else
            {
                clear();
                EnabledStatus(true);
                ButtonStatus(false);
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

        private void button4_Click(object sender, EventArgs e)
        {
            if (textBox1.Text != "")
            {
                DialogResult dr = MessageBox.Show("Delete " + textBox1.Text, "Confirm", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);

                if (dr == System.Windows.Forms.DialogResult.OK)
                {
                    SuppliesTransaction deleteST = (from x in cheese.SuppliesTransactions
                                        where x.SuppliesTransactionID.Equals(textBox1.Text)
                                        select x).First();
                    var deletedST = (from x in cheese.DetailSuppliesTransactions
                                    where x.SuppliesTransactionID.Equals(textBox1.Text)
                                    select x);
                    cheese.SuppliesTransactions.Remove(deleteST);
                    foreach (DetailSuppliesTransaction detailSuppliesTransaction in deletedST)
                    {
                        cheese.DetailSuppliesTransactions.Remove(detailSuppliesTransaction);
                    }
                    cheese.SaveChanges();
                    clear();
                    textBox1.Text = "";
                    loadData();
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (textBox1.Text != "")
            {
                SuppliesTransaction sT = (from x in cheese.SuppliesTransactions
                                          where x.SuppliesTransactionID.Equals(textBox1.Text)
                                          select x).First();
                var dST = from x in cheese.DetailSuppliesTransactions
                          where x.SuppliesTransactionID.Equals(textBox1.Text)
                          select x;
                if (sT != null)
                {
                    sT.SuppliesTransactionDescription = textBox2.Text;
                    sT.SuppliesTransactionStatus = "Checked";
                    sT.UpdatedBy = _MainForm.userID;
                    cheese.SaveChanges();
                    loadData();
                    clear();
                    EnabledStatus(false);
                    ButtonStatus(true);
                    label10.Visible = false;

                    GoodsReceipt gR = new GoodsReceipt();
                    string newID = generateGoodsReceiptID();
                    gR.GoodsReceiptID = newID;
                    gR.SuppliesTransactionID = textBox1.Text;
                    gR.CreatedBy = _MainForm.userID;
                    gR.GoodsReceiptDate = DateTime.Now;
                    gR.GoodsReceiptDescription = "No Description";
                    //gR.GoodsReceiptStatus = "Unchecked";
                    //gR.UpdatedBy = "";
                    cheese.GoodsReceipts.Add(gR);
                    //cheese.SaveChanges();
                    foreach (DetailSuppliesTransaction detail in dST)
                    {
                        DetailGoodsReceipt dGR = new DetailGoodsReceipt();
                        
                        dGR.GoodsReceiptID = newID;
                        dGR.MilkID = detail.MilkID.ToString();
                        dGR.Quantity = (Int32)detail.Quantity;
                        //dGR.Price = (Int32)detail.Price;
                        //dGR.VendorID = detail.VendorID;
                        cheese.DetailGoodsReceipts.Add(dGR);
                        //cheese.SaveChanges();
                    }
                    cheese.SaveChanges();
                    loadData();
                    clear();
                    textBox1.Text = "";
                    MessageBox.Show("Data Updated");
                }
            }
        }

        public string generateGoodsReceiptID()
        {
            string newID = "";
            var goodsReceipt = from temp in cheese.GoodsReceipts select temp;
            var countOfRows = goodsReceipt.Count();

            if (countOfRows != 0)
            {
                var data = goodsReceipt.ToList();
                var lastRow = data.ElementAt(countOfRows - 1);
                string curr = lastRow.GoodsReceiptID;
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
                return "GRD0001";
            }
        }

    }
}
