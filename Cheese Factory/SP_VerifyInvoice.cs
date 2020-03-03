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
    public partial class SP_VerifyInvoice : Form
    {
        CheeseEntities cheese = new CheeseEntities();
        public SP_VerifyInvoice()
        {
            InitializeComponent();
            loadData();
            clear();
            enableStatus(false);
        }


        private void loadData()
        {
            var verifiedInvoice  = from x in cheese.VerifiedInvoices
                                      select new { x.VerifiedInvoiceID, x.SuppliesTransactionID, x.FinalGoodsReceiptID, x.TotalPrice , x.VerifiedInvoiceStatus, x.VerifiedInvoiceDate };
            dataGridView5.DataSource = verifiedInvoice.ToList();
            if (verifiedInvoice != null)
            {
                dataGridView5.Enabled = true;
            }
            else
            {
                dataGridView5.Enabled = false;
            }
        }

        private void enableStatus(bool status)
        {
            button1.Enabled = status;
            textBox1.Enabled = false;
            textBox2.Enabled = false;
            textBox3.Enabled = false;
            textBox4.Enabled = false;
            textBox5.Enabled = false;
            textBox2.Visible = false;
            textBox3.Visible = false;
            textBox4.Visible = false;
            label2.Visible = false;
            label3.Visible = false;
            label4.Visible = false;
        }

        private void clear()
        {
            textBox1.Text = "";
            textBox2.Text = "";
            textBox3.Text = "";
            textBox4.Text = "";
            textBox5.Text = "";
            label10.Visible = false;
            dataGridView2.DataSource = null;
            dataGridView4.DataSource = null;
            dataGridView3.DataSource = null;
            dataGridView4.DataSource = null;
        }

        private void dataGridView5_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            clear();
            if (e.RowIndex >= 0)
            {
                textBox5.Text = dataGridView5.Rows[e.RowIndex].Cells[0].Value.ToString();
                textBox1.Text = dataGridView5.Rows[e.RowIndex].Cells[1].Value.ToString();
                textBox4.Text = dataGridView5.Rows[e.RowIndex].Cells[2].Value.ToString();

                FinalGoodsReceipt finalGoodsReceipt = (from x in cheese.FinalGoodsReceipts where x.FinalGoodsReceiptID.Equals(textBox4.Text) select x).First();
                GoodsReceipt goodsReceipt = (from x in cheese.GoodsReceipts where x.SuppliesTransactionID.Equals(textBox1.Text) select x).First();
                textBox2.Text = goodsReceipt.GoodsReceiptID;
                GoodsReceiptCheck goodsReceiptCheck = (from x in cheese.GoodsReceiptChecks where x.GoodsReceiptID.Equals(textBox2.Text) select x).First();
                textBox3.Text = goodsReceiptCheck.GoodsReceiptCheckID;

                var suppliesTransaction = (from x in cheese.SuppliesTransactions where x.SuppliesTransactionID.Equals(textBox1.Text) select new { x.SuppliesTransactionID, x.SuppliesTransactionStatus, x.SuppliesTransactionDescription });
                var detailSuppliesTransaction = (from x in cheese.DetailSuppliesTransactions where x.SuppliesTransactionID.Equals(textBox1.Text) select new { x.MilkID, x.VendorID, x.Price });
                var finalGoodsReceiptTemp = (from x in cheese.FinalGoodsReceipts where x.FinalGoodsReceiptID.Equals(textBox4.Text) select new { x.FinalGoodsReceiptID, x.FinalGoodsReceiptDate });
                var detailFinalGoodsReceipt = (from x in cheese.DetailFinalGoodsReceipts where x.FinalGoodsReceiptID.Equals(textBox4.Text) select new { x.MilkID,x.Quantity });
                dataGridView1.DataSource = finalGoodsReceiptTemp.ToList();
                dataGridView2.DataSource = detailFinalGoodsReceipt.ToList();
                dataGridView3.DataSource = suppliesTransaction.ToList();
                dataGridView4.DataSource = detailSuppliesTransaction.ToList();

                if (textBox5.Text == "")
                {
                    enableStatus(false);
                }
                else
                {
                    VerifiedInvoice verifiedInvoice  = (from x in cheese.VerifiedInvoices where x.VerifiedInvoiceID.Equals(textBox5.Text) select x).First();
                    if (verifiedInvoice.VerifiedInvoiceStatus=="Created"){
                        enableStatus(true);
                    }
                    else
                    {
                        enableStatus(false);
                    }
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox5.Text != "")
            {
                DialogResult dr = MessageBox.Show("Update " + textBox1.Text + " Invoice Verification Status", "Confirm", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);

                if (dr == System.Windows.Forms.DialogResult.OK)
                {
                    VerifiedInvoice vI = (from x in cheese.VerifiedInvoices where x.VerifiedInvoiceID.Equals(textBox5.Text) select x).First();
                    vI.VerifiedInvoiceStatus = "Waiting for Confirmation";
                    vI.ApprovedBy = _MainForm.userID;
                    vI.ApprovedDate = DateTime.Now;
                    cheese.SaveChanges();
                    loadData();
                    clear();
                    enableStatus(false);
                    MessageBox.Show("Invoice Verification Updated");
                }
            }
        }



    }
}
