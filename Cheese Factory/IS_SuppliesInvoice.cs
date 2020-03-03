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
    public partial class IS_SuppliesInvoice : Form
    {
        CheeseEntities cheese = new CheeseEntities();
        public IS_SuppliesInvoice()
        {
            InitializeComponent();
            loadData();
            clear();
            enableStatus(false);
            loadDetail();
        }

        private void loadData()
        {
            var suppliesTransaction = from x in cheese.SuppliesTransactions
                                      select new { x.SuppliesTransactionID, x.SuppliesTransactionStatus, x.SuppliesTransactionDescription, x.SuppliesTransactionDate };
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

        private void loadDetail()
        {
            var finalGoodsReceipt = from x in cheese.FinalGoodsReceipts
                                    where x.GoodsReceiptCheckID.Equals(textBox3.Text)
                                    select new { x.FinalGoodsReceiptID, x.FinalGoodsReceiptDate };
            dataGridView3.DataSource = finalGoodsReceipt.ToList();
            if (finalGoodsReceipt != null)
            {
                dataGridView3.Enabled = true;
            }
            else
            {
                dataGridView3.Enabled = false;
            }
            if (dataGridView3.RowCount > 0)
            {
                FinalGoodsReceipt fGR = (from x in cheese.FinalGoodsReceipts where x.GoodsReceiptCheckID.Equals(textBox3.Text) select x).First();
                var detailFinalGoodsReceipt = from x in cheese.DetailFinalGoodsReceipts where x.FinalGoodsReceiptID.Equals(fGR.FinalGoodsReceiptID) select new { x.MilkID, x.Quantity};
                dataGridView4.DataSource = detailFinalGoodsReceipt.ToList();
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
            textBox6.Enabled = false;
            textBox2.Visible = false;
            textBox3.Visible = false;
            textBox4.Visible = false;
            label2.Visible = false;
            label3.Visible = false;
            label4.Visible = false;
            numericUpDown1.Enabled = false;
            numericUpDown2.Enabled = false;
            textBox7.Enabled = false;
            textBox8.Enabled = false;
        }

        private void clear()
        {
            textBox1.Text = "";
            textBox2.Text = "";
            textBox3.Text = "";
            textBox4.Text = "";
            textBox5.Text = "";
            textBox6.Text = "";
            label10.Visible = false;
            numericUpDown1.Value = 0;
            numericUpDown2.Value = 0;
            textBox7.Text = "";
            textBox8.Text = "";
            dataGridView2.DataSource = null;
            dataGridView4.DataSource = null;
            dataGridView3.DataSource = null;
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                textBox1.Text = dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString();
                GoodsReceipt goodsReceipt = (from x in cheese.GoodsReceipts where x.SuppliesTransactionID.Equals(textBox1.Text) select x).First();
                textBox2.Text = goodsReceipt.GoodsReceiptID;
                GoodsReceiptCheck goodsReceiptCheck = (from x in cheese.GoodsReceiptChecks where x.GoodsReceiptID.Equals(textBox2.Text) select x).First();
                textBox3.Text = goodsReceiptCheck.GoodsReceiptCheckID;
                FinalGoodsReceipt finalGoodsReceipt = (from x in cheese.FinalGoodsReceipts where x.GoodsReceiptCheckID.Equals(textBox3.Text) select x).First();
                textBox4.Text = finalGoodsReceipt.FinalGoodsReceiptID;
                loadDetail();
               /* if (textBox4.Text != "")
                {
                    var finalGoodsReceiptTemp = from x in cheese.FinalGoodsReceipts select x;
                    if (finalGoodsReceiptTemp.Count() > 0)
                    {
                        FinalGoodsReceipt fGR = (from x in cheese.FinalGoodsReceipts where x.GoodsReceiptCheckID.Equals(textBox3.Text) select x).First();
                        if (fGR != null)
                        {
                            enableStatus(false);
                        }
                    }
                }*/
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
                numericUpDown1.Value = 0;
            }
        }

        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                textBox5.Text = dataGridView2.Rows[e.RowIndex].Cells[3].Value.ToString();
                textBox6.Text = dataGridView2.Rows[e.RowIndex].Cells[0].Value.ToString();
                numericUpDown1.Value = Int32.Parse(dataGridView2.Rows[e.RowIndex].Cells[1].Value.ToString());
                numericUpDown2.Value = Int32.Parse(dataGridView2.Rows[e.RowIndex].Cells[2].Value.ToString());
                textBox7.Text = (numericUpDown1.Value * numericUpDown2.Value).ToString();
                int temp=0;
                foreach (DataGridViewRow row in dataGridView2.Rows)
                {
                    temp += Int32.Parse(row.Cells[1].Value.ToString()) * Int32.Parse(row.Cells[2].Value.ToString());
                }
                textBox8.Text = temp.ToString();
                enableStatus(true);
                var verifiedInvoice = (from x in cheese.VerifiedInvoices select x);
                if (verifiedInvoice.ToList().Count>0){
                    foreach (VerifiedInvoice row in verifiedInvoice.ToList())
                    {
                        if (row.FinalGoodsReceiptID.Equals(textBox4.Text))
                        {
                            enableStatus(false);
                        }
                    }
                }
               
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox4.Text != "")
            {
                DialogResult dr = MessageBox.Show("Create " + textBox1.Text + " Invoice Verification", "Confirm", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);

                if (dr == System.Windows.Forms.DialogResult.OK)
                {
                    VerifiedInvoice vI = new VerifiedInvoice();
                    vI.FinalGoodsReceiptID = textBox4.Text;
                    vI.VerifiedInvoiceID = generateVerifiedInvoiceID();
                    vI.VerifiedInvoiceStatus = "Created";
                    vI.VerifiedInvoiceDate = DateTime.Now;
                    vI.TotalPrice = Int32.Parse(textBox8.Text);
                    vI.CreatedBy = _MainForm.userID;
                    vI.SuppliesTransactionID = textBox1.Text;
                    cheese.VerifiedInvoices.Add(vI);
                    cheese.SaveChanges();
                    loadData();
                    clear();
                    enableStatus(false);
                    MessageBox.Show("Invoice Verification Created");
                }
            }
        }

        public string generateVerifiedInvoiceID()
        {
            string newID = "";
            var verifiedInvocie = from temp in cheese.VerifiedInvoices select temp;
            var countOfRows = verifiedInvocie.Count();

            if (countOfRows != 0)
            {
                var data = verifiedInvocie.ToList();
                var lastRow = data.ElementAt(countOfRows - 1);
                string curr = lastRow.VerifiedInvoiceID;
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
                return "VFI0001";
            }
        }

    }
}
