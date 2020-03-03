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
    public partial class SP_SuppliesTransaction : Form
    {
        CheeseEntities cheese = new CheeseEntities();
        DataTable tableCart = new DataTable();
        int cartRow = 0, selectedCartRow = -1;
        int flag = 0;

        public SP_SuppliesTransaction()
        {
            InitializeComponent();

            insertComboBox();
            loadData();
            clear();
            EnabledStatus(false);
            ButtonStatus(true);
        }

        private void loadData()
        {
            var suppliesTransaction = from x in cheese.SuppliesTransactions
                                      select new { x.SuppliesTransactionID, x.SuppliesTransactionDate, x.SuppliesTransactionStatus };
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
            button4.Enabled = !status;
            button5.Enabled = !status;
            button6.Enabled = !status;
        }

        private void EnabledStatus(bool status)
        {
            textBox1.Enabled = false;
            textBox2.Enabled = false;
            textBox2.Visible = false;
            label6.Visible = false;
            comboBox1.Enabled = status;
            comboBox2.Enabled = status;
            numericUpDown1.Enabled = status;
            numericUpDown2.Enabled = status;
        }

        private void clear()
        {
            comboBox1.SelectedIndex = 0;
            comboBox2.SelectedIndex = 0;
            numericUpDown1.Value = 0;
            numericUpDown2.Value = 0;
        }

        public string generateSuppliesTransactionID()
        {
            string newID = "";
            var suppliesTransaction = from temp in cheese.SuppliesTransactions select temp;
            var countOfRows = suppliesTransaction.Count();

            if (countOfRows != 0)
            {
                var data = suppliesTransaction.ToList();
                var lastRow = data.ElementAt(countOfRows - 1);
                string curr = lastRow.SuppliesTransactionID;
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
            if (comboBox1.SelectedIndex < 0)
            {
                label10.Text = "Vendor Must be Choosen";
                label10.Visible = true;
            }
            else if (comboBox2.SelectedIndex < 0)
            {
                label10.Text = "Milk Variation Must be Choosen";
                label10.Visible = true;
            }
            else if (numericUpDown1.Value < 1)
            {
                label10.Text = "Quantity Must be More Than 0";
                label10.Visible = true;
            }
            else if (numericUpDown2.Value < 1)
            {
                label10.Text = "Price Must Be Inpputed";
                label10.Visible = true;
            }
            else
            {
                var milk = from x in cheese.Milk select x;//all milk
                string mName = milk.ToArray()[comboBox2.SelectedIndex].MilkName; // milkname
                var milkInput = (from x in cheese.Milk where x.MilkName.Equals(mName) select x).First();//milk data
                var vendor = from x in cheese.Vendors select x;//all vendor
                string vName = vendor.ToArray()[comboBox1.SelectedIndex].VendorName;//vendor name
                var vendorInput = (from x in cheese.Vendors where x.VendorName.Equals(vName) select x).First();//vendor data

                tableCart.Rows.Add(milkInput.MilkID, (int)numericUpDown1.Value, (int)numericUpDown2.Value, vendorInput.VendorID);
                cartRow++;
                clear();
            }
        }

        private void insertComboBox()
        {
            var vendor = from x in cheese.Vendors select x;
            var milk = from x in cheese.Milk select x;

            comboBox1.DataSource = vendor.ToList();
            comboBox1.DisplayMember = "VendorName";

            comboBox2.DataSource = milk.ToList();
            comboBox2.DisplayMember = "MilkName";

            tableCart.Columns.Add("MilkID");
            tableCart.Columns.Add("Quantity");
            tableCart.Columns.Add("Price");
            tableCart.Columns.Add("VendorID");
            dataGridView2.DataSource = tableCart;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            DataRow[] foundRows = tableCart.Select();
            if (selectedCartRow < 0)
            {
                label10.Text = "You Must Select an item in the cart";
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
                    clear();
                    selectedCartRow = -1;
                    cartRow--;
                }
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (selectedCartRow >= 0)
            {
                DataRow currRow;
                var milk = from x in cheese.Milk select x;//all milk
                string mName = milk.ToArray()[comboBox2.SelectedIndex].MilkName; // milkname
                var milkInput = (from x in cheese.Milk where x.MilkName.Equals(mName) select x).First();//milk data
                var vendor = from x in cheese.Vendors select x;//all vendor
                string vName = vendor.ToArray()[comboBox1.SelectedIndex].VendorName;//vendor name
                var vendorInput = (from x in cheese.Vendors where x.VendorName.Equals(vName) select x).First();//vendor data
                if (selectedCartRow >= 0)
                {
                    currRow = tableCart.Rows[selectedCartRow];
                    currRow.BeginEdit();
                    currRow["MilkID"] = milkInput.MilkID.ToString();
                    currRow["Quantity"] = (int)numericUpDown1.Value;
                    currRow["Price"] = (int)numericUpDown2.Value;
                    currRow["VendorID"] = vendorInput.VendorID.ToString();
                    currRow.EndEdit();
                    MessageBox.Show("Record edited successfully");
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            clear();
            EnabledStatus(true);
            ButtonStatus(false);
            textBox1.Text = generateSuppliesTransactionID();
            dataGridView2.DataSource = tableCart;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            clear();
            textBox1.Text = "";
            EnabledStatus(false);
            ButtonStatus(true);
            label10.Visible = false;
            tableCart.Clear();
            flag = 0;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (cartRow > 0)
            {
                SuppliesTransaction sT = new SuppliesTransaction();
                sT.SuppliesTransactionID = textBox1.Text;
                sT.SuppliesTransactionDate = DateTime.Now;
                sT.RequestBy = _MainForm.userID;
                sT.SuppliesTransactionDescription = "No Description";
                sT.SuppliesTransactionStatus = "Unchecked";
                sT.UpdatedBy = "";
                cheese.SuppliesTransactions.Add(sT);
                //cheese.SaveChanges();
                foreach (DataRow row in tableCart.Rows)
                {
                    DetailSuppliesTransaction dST = new DetailSuppliesTransaction();
                    dST.SuppliesTransactionID = textBox1.Text;
                    dST.MilkID = row.Field<string>(0);
                    dST.Quantity = Int32.Parse(row.Field<string>(1));//Int32.(row["Quantity"].ToString());
                    dST.Price = Int32.Parse(row.Field<string>(2));
                    dST.VendorID = row.Field<string>(3);//row["VendorID"].ToString();
                    cheese.DetailSuppliesTransactions.Add(dST);
                    //cheese.SaveChanges();
                }
                cheese.SaveChanges();
                loadData();
                clear();
                EnabledStatus(false);
                ButtonStatus(true);
                textBox1.Text = "";
                label10.Visible = false;
                tableCart.Clear();
                flag = 0;

            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0) { 
            textBox1.Text = dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString();
            }
            if (flag == 0 && textBox1.Text!="")
            {
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
                clear();
            }
        }

        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            var milk = from x in cheese.Milk select x;//all milk
            string mName = milk.ToArray()[comboBox2.SelectedIndex].MilkName; // milkname
            var milkInput = (from x in cheese.Milk where x.MilkName.Equals(mName) select x).First();//milk data
            var vendor = from x in cheese.Vendors select x;//all vendor
            string vName = vendor.ToArray()[comboBox1.SelectedIndex].VendorName;//vendor name
            var vendorInput = (from x in cheese.Vendors where x.VendorName.Equals(vName) select x).First();//vendor data

            selectedCartRow = e.RowIndex;
            int tempmilk = 0, tempvendor = 0;
            while (tempmilk < milk.Count())
            {
                if (milk.ToArray()[tempmilk].MilkID.Equals(dataGridView2.Rows[selectedCartRow].Cells[0].Value.ToString()))
                {
                    break;
                }
                else
                {
                    tempmilk++;
                }
            }
            comboBox2.SelectedIndex = tempmilk;
            while (tempvendor < vendor.Count())
            {
                if (vendor.ToArray()[tempvendor].VendorID.Equals(dataGridView2.Rows[selectedCartRow].Cells[3].Value.ToString()))
                {
                    break;
                }
                else
                {
                    tempvendor++;
                }
            }
            comboBox1.SelectedIndex = tempvendor;

            numericUpDown1.Value = Int32.Parse(dataGridView2.Rows[selectedCartRow].Cells[1].Value.ToString());
            numericUpDown2.Value = Int32.Parse(dataGridView2.Rows[selectedCartRow].Cells[2].Value.ToString());

        }

    }
}
