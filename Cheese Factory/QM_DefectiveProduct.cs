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
    public partial class QM_DefectiveProduct : Form
    {
        CheeseEntities cheese = new CheeseEntities();
        DataTable tableCart = new DataTable();
        int cartRow = 0, selectedCartRow = -1;
        int flag = 0;
        public QM_DefectiveProduct()
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
            var warehouse = from x in cheese.Warehouses
                                      select new { x.WarehouseID, x.UpdatedDate};
            dataGridView1.DataSource = warehouse.ToList();
            if (warehouse != null)
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
            comboBox2.Enabled = status;
            numericUpDown1.Enabled = status;
            textBox2.Enabled = status;
        }

        private void clear()
        {
            comboBox2.SelectedIndex = 0;
            numericUpDown1.Value = 0;
            textBox2.Text = "";
        }

        public string generateWarehouseID()
        {
            string newID = "";
            var warehouse = from temp in cheese.Warehouses select temp;
            var countOfRows = warehouse.Count();

            if (countOfRows != 0)
            {
                var data = warehouse.ToList();
                var lastRow = data.ElementAt(countOfRows - 1);
                string curr = lastRow.WarehouseID;
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
                return "WRH0001";
            }
        }

        private void insertComboBox()
        {
            var keju = from x in cheese.Cheese select x;
            comboBox2.DataSource = keju.ToList();
            comboBox2.DisplayMember = "CheeseName";

            tableCart.Columns.Add("CheeseID");
            tableCart.Columns.Add("Quantity");
            tableCart.Columns.Add("Description");
            dataGridView2.DataSource = tableCart;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            clear();
            EnabledStatus(true);
            ButtonStatus(false);
            textBox1.Text = generateWarehouseID();
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
                Warehouse w = new Warehouse();
                w.WarehouseID = textBox1.Text;
                w.UpdatedDate = DateTime.Now;
                w.UpdatedBy = _MainForm.userID;

                cheese.Warehouses.Add(w);
                //cheese.SaveChanges();
                foreach (DataRow row in tableCart.Rows)
                {
                    DetailWarehouse dW = new DetailWarehouse();
                    dW.WarehouseID = textBox1.Text;
                    dW.ProductID = row.Field<string>(0);
                    dW.Quantity = Int32.Parse(row.Field<string>(1));
                    dW.Status = row.Field<string>(3);
                    cheese.DetailWarehouses.Add(dW);
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

        private void button4_Click(object sender, EventArgs e)
        {
            if (comboBox2.SelectedIndex < 0)
            {
                label10.Text = "Product Must be Choosen";
                label10.Visible = true;
            }
            else if (numericUpDown1.Value < 1)
            {
                label10.Text = "Quantity Must be More Than 0";
                label10.Visible = true;
            }
            else if (textBox2.Text == "")
            {
                label10.Text = "Reason Must be Stated";
                label10.Visible = true;
            }
            else
            {
                var keju = from x in cheese.Cheese select x;
                string kName = keju.ToArray()[comboBox2.SelectedIndex].CheeseName; 
                var cheeseInput = (from x in cheese.Milk where x.MilkName.Equals(kName) select x).First();

                tableCart.Rows.Add(cheeseInput.MilkID, (int)numericUpDown1.Value, textBox2.Text);
                cartRow++;
                clear();
            }
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
                var keju = from x in cheese.Cheese select x;
                string kName = keju.ToArray()[comboBox2.SelectedIndex].CheeseName; 
                var cheeseInput = (from x in cheese.Milk where x.MilkName.Equals(kName) select x).First();
                if (selectedCartRow >= 0)
                {
                    currRow = tableCart.Rows[selectedCartRow];
                    currRow.BeginEdit();
                    currRow["CheeseID"] = cheeseInput.MilkID.ToString();
                    currRow["Quantity"] = (int)numericUpDown1.Value;
                    currRow["Status"] = textBox2.Text;
                    currRow.EndEdit();
                    MessageBox.Show("Record edited successfully");
                }
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                textBox1.Text = dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString();
            }
            if (flag == 0 && textBox1.Text != "")
            {
                var detailWarehouse = from x in cheese.DetailWarehouses
                                                where x.WarehouseID == textBox1.Text
                                                select new { x.WarehouseID, x.ProductID, x.Quantity, x.Status };
                dataGridView2.DataSource = detailWarehouse.ToList();
                if (detailWarehouse != null)
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
            var keju = from x in cheese.Cheese select x;
            string kName = keju.ToArray()[comboBox2.SelectedIndex].CheeseName;
            var cheeseInput = (from x in cheese.Milk where x.MilkName.Equals(kName) select x).First();
            
            selectedCartRow = e.RowIndex;
            int tempcheese = 0;
            while (tempcheese < keju.Count())
            {
                if (keju.ToArray()[tempcheese].CheeseID.Equals(dataGridView2.Rows[selectedCartRow].Cells[0].Value.ToString()))
                {
                    break;
                }
                else
                {
                    tempcheese++;
                }
            }
            comboBox2.SelectedIndex = tempcheese;

            numericUpDown1.Value = Int32.Parse(dataGridView2.Rows[selectedCartRow].Cells[1].Value.ToString());
            textBox2.Text = dataGridView2.Rows[selectedCartRow].Cells[2].Value.ToString();

        }


    }
}
