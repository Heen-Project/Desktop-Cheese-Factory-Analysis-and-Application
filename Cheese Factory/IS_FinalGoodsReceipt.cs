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
    public partial class IS_FinalGoodsReceipt : Form
    {
        CheeseEntities cheese = new CheeseEntities();
        DataTable tableCart = new DataTable();
        int cartRow = 0, selectedCartRow = -1;
        int flag = 0;
        public IS_FinalGoodsReceipt()
        {
            InitializeComponent();
            loadData();
            clear();
            EnabledStatus(false);
            btnDisabled();
            insertTableColoumn();
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
            dataGridView2.DataSource = finalGoodsReceipt.ToList();
            if (finalGoodsReceipt != null)
            {
                dataGridView1.Enabled = true;
            }
            else
            {
                dataGridView1.Enabled = false;
            }
            if (flag == 0 && dataGridView2.RowCount > 0)
            {
                FinalGoodsReceipt fGR = (from x in cheese.FinalGoodsReceipts where x.GoodsReceiptCheckID.Equals(textBox3.Text) select x).First();
                var detailFinalGoodsReceipt = from x in cheese.DetailFinalGoodsReceipts where x.FinalGoodsReceiptID.Equals(fGR.FinalGoodsReceiptID) select x;
                dataGridView3.DataSource = detailFinalGoodsReceipt.ToList();
            }
        }

        private void ButtonStatus(bool status)
        {
            button1.Enabled = status;
            button2.Enabled = !status;
            button3.Enabled = !status;
            button4.Enabled = !status;
            button5.Enabled = !status;
            button6.Enabled = !status;
        }

        private void btnDisabled()
        {
            button1.Enabled = false;
            button2.Enabled = false;
            button3.Enabled = false;
            button4.Enabled = false;
            button5.Enabled = false;
            button6.Enabled = false;
        }

        private void EnabledStatus(bool status)
        {
            textBox1.Enabled = false;
            textBox2.Enabled = false;
            textBox2.Visible = false;
            label2.Visible = false;
            textBox3.Enabled = false;
            textBox3.Visible = false;
            label3.Visible = false;
            textBox5.ReadOnly = true;
            textBox7.Enabled = false;
            numericUpDown1.Enabled = status;
        }

        private void clear()
        {
            textBox1.Text = "";
            textBox2.Text = "";
            textBox3.Text = "";
            textBox5.Text = "";
            textBox7.Text = "";
            label10.Visible = false;
            numericUpDown1.Value = 0;
        }

        public string generateFinalGoodsReceiptID()
        {
            string newID = "";
            var finalGoodsReceipt = from temp in cheese.FinalGoodsReceipts select temp;
            var countOfRows = finalGoodsReceipt.Count();

            if (countOfRows != 0)
            {
                var data = finalGoodsReceipt.ToList();
                var lastRow = data.ElementAt(countOfRows - 1);
                string curr = lastRow.FinalGoodsReceiptID;
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
                return "STR0001";
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "")
            {
                label10.Text = "There is no Trasaction";
                label10.Visible = true;
            }
            else if (numericUpDown1.Value < 1)
            {
                label10.Text = "Updated Quantity Must be More Than 0";
                label10.Visible = true;
            }
            else
            {
                tableCart.Rows.Add(textBox7.Text, (int)numericUpDown1.Value);
                cartRow++;
                textBox7.Text = "";
                numericUpDown1.Value = 0;
                label10.Visible = false;
            }
        }

        private void insertTableColoumn()
        {
            var vendor = from x in cheese.Vendors select x;
            var milk = from x in cheese.Milk select x;
            tableCart.Columns.Add("MilkID");
            tableCart.Columns.Add("Quantity");
            dataGridView3.DataSource = tableCart;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            DataRow[] foundRows = tableCart.Select();
            if (selectedCartRow < 0)
            {
                label10.Text = "You Must Select an item in the detail";
                label10.Visible = true;
                return;
            }
            if (textBox1.Text != "" && foundRows.Length > 0)
            {
                DialogResult dr = MessageBox.Show("Delete " + tableCart.Rows[selectedCartRow].Field<string>(0), "Confirm", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);

                if (dr == System.Windows.Forms.DialogResult.OK)
                {
                    tableCart.Rows[selectedCartRow].Delete();
                    loadData();
                    textBox7.Text = "";
                    numericUpDown1.Value = 0;
                    selectedCartRow = -1;
                    cartRow--;
                    label10.Visible = false;
                }
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (selectedCartRow >= 0)
            {
                DataRow currRow;
                if (selectedCartRow >= 0)
                {
                    currRow = tableCart.Rows[selectedCartRow];
                    currRow.BeginEdit();
                    currRow["MilkID"] = textBox7.Text;
                    currRow["Quantity"] = (int)numericUpDown1.Value;
                    currRow.EndEdit();
                    MessageBox.Show("Record edited successfully");
                    label10.Visible = false;
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text != "")
            {
                EnabledStatus(true);
                ButtonStatus(false);
                dataGridView3.DataSource = tableCart;
                dataGridView1.Enabled = false;
                label10.Visible = false;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            clear();
            EnabledStatus(false);
            ButtonStatus(true);
            label10.Visible = false;
            tableCart.Clear();
            flag = 0;
            dataGridView1.Enabled = true;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (cartRow > 0)
            {
                if (cartRow == dataGridView3.RowCount-1)
                {
                    string newID = generateFinalGoodsReceiptID();
                    FinalGoodsReceipt fGR = new FinalGoodsReceipt();
                    fGR.FinalGoodsReceiptID = newID;
                    fGR.GoodsReceiptCheckID = textBox3.Text;
                    fGR.FinalGoodsReceiptDate = DateTime.Now;
                    fGR.CreatedBy = _MainForm.userID;
                    cheese.FinalGoodsReceipts.Add(fGR);
                    foreach (DataRow row in tableCart.Rows)
                    {
                        DetailFinalGoodsReceipt dFGR = new DetailFinalGoodsReceipt();
                        dFGR.FinalGoodsReceiptID = newID;
                        dFGR.MilkID = row.Field<string>(0);
                        dFGR.Quantity = Int32.Parse(row.Field<string>(1));
                        cheese.DetailFinalGoodsReceipts.Add(dFGR);
                    }
                    cheese.SaveChanges();
                    loadData();
                    loadDetail();
                    clear();
                    EnabledStatus(false);
                    ButtonStatus(true);
                    label10.Visible = false;
                    tableCart.Clear();
                    flag = 0;
                    dataGridView1.Enabled = true;
                }
                else
                {
                    label10.Text = "You Must Insert All Items";
                    label10.Visible = true;
                }
            }
        }

        private void dataGridView3_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && dataGridView3.DataSource == tableCart)
            {
                selectedCartRow = e.RowIndex;
                textBox7.Text = dataGridView3.Rows[selectedCartRow].Cells[0].Value.ToString();
                numericUpDown1.Value = Int32.Parse(dataGridView3.Rows[selectedCartRow].Cells[1].Value.ToString());
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                textBox1.Text = dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString();
                var goodsReceipt = (from x in cheese.GoodsReceipts where x.SuppliesTransactionID.Equals(textBox1.Text) select x).First();
                textBox2.Text = goodsReceipt.GoodsReceiptID;
                var goodsReceiptCheck = (from x in cheese.GoodsReceiptChecks where x.GoodsReceiptID.Equals(textBox2.Text) select x).First();
                textBox3.Text = goodsReceiptCheck.GoodsReceiptCheckID;

                textBox5.Text = "IS Description : " + dataGridView1.Rows[e.RowIndex].Cells[2].Value.ToString() + ", QM Description : " + goodsReceipt.GoodsReceiptDescription;
                loadDetail();
                ButtonStatus(true);
                if (textBox3.Text != "")
                {
                    var finalGoodsReceipt = from x in cheese.FinalGoodsReceipts select x;
                    if (finalGoodsReceipt.Count() > 0)
                    {
                        FinalGoodsReceipt fGR = (from x in cheese.FinalGoodsReceipts where x.GoodsReceiptCheckID.Equals(textBox3.Text) select x).First();
                        if (fGR != null)
                        {
                            btnDisabled();
                        }
                    }
                }

                var detailSuppliesTransaction = from x in cheese.DetailSuppliesTransactions
                                                where x.SuppliesTransactionID == textBox1.Text
                                                select new { x.MilkID, x.Price, x.Quantity, x.VendorID };
                dataGridView4.DataSource = detailSuppliesTransaction.ToList();
                if (detailSuppliesTransaction != null)
                {
                    dataGridView2.Enabled = true;
                }
                else
                {
                    dataGridView2.Enabled = false;
                }
                textBox7.Text = "";
                numericUpDown1.Value = 0;
            }
        }

        private void dataGridView4_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                textBox7.Text = dataGridView4.Rows[e.RowIndex].Cells[0].Value.ToString();
                numericUpDown1.Value = Int32.Parse(dataGridView4.Rows[e.RowIndex].Cells[2].Value.ToString());
            }
        }

    }
}
