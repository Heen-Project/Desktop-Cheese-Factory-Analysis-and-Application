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
    public partial class SP_Vendor : Form
    {
        CheeseEntities cheese = new CheeseEntities();
        int flag = 0;
        public SP_Vendor()
        {
            InitializeComponent();
            loadData();
            EnabledStatus(false);
            ButtonStatus(true);
        }
        private void loadData()
        {
            var vendor = from temp in cheese.Vendors
                           select new { temp.VendorID, temp.VendorName};
            dataGridView1.DataSource = vendor.ToList();
            if (vendor != null)
            {
                dataGridView1.Enabled = true;
            }
            else
            {
                dataGridView1.Enabled = false;
            }
        }

        private void EnabledStatus(bool status)
        {
            textBox1.Enabled = false;
            textBox2.Enabled = status;
        }

        private void ButtonStatus(bool status)
        {
            button1.Enabled = status;
            button2.Enabled = status;
            button3.Enabled = status;
            button4.Enabled = !status;
            button5.Enabled = !status;
            button6.Enabled = !status;
        }

        private void clear()
        {
            textBox2.Text = "";
            label10.Visible = false;
        }

        public string generateVendorID()
        {
            string newID = "";
            var vendor = from temp in cheese.Vendors select temp;
            var countOfRows = vendor.Count();

            if (countOfRows != 0)
            {
                var data = vendor.ToList();
                var lastRow = data.ElementAt(countOfRows - 1);
                string curr = lastRow.VendorID;
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
                return "VDR0001";
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            clear();
            EnabledStatus(true);
            ButtonStatus(false);
            flag = 1;
            textBox1.Text = generateVendorID();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            EnabledStatus(true);
            ButtonStatus(false);
            flag = 2;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (textBox1.Text != "")
            {
                DialogResult dr = MessageBox.Show("Delete " + textBox1.Text, "Confirm", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);

                if (dr == System.Windows.Forms.DialogResult.OK)
                {
                    Vendor deleteV = (from x in cheese.Vendors
                                        where x.VendorID.Equals(textBox1.Text)
                                        select x).First();
                    cheese.Vendors.Remove(deleteV);
                    
                    cheese.SaveChanges();
                    loadData();
                    clear();
                    textBox1.Text = "";
                }
            }

            flag = 0;
        }

        private void insertData()
        {
            if (textBox2.Text == "")
            {
                label10.Visible = true;
                label10.Text = "Company Name Must be Filled";
            }
            else
            {
                Vendor v = new Vendor();
                v.VendorID = textBox1.Text;
                v.VendorName = textBox2.Text;
                v.VendorRegisterBy = _MainForm.userID;
                v.VendorRegisterDate = DateTime.Now;

                cheese.Vendors.Add(v);
                cheese.SaveChanges();
                loadData();
                clear();
                EnabledStatus(false);
                ButtonStatus(true);
                label10.Visible = false;
                flag = 0;
            }
        }

        private void updateData()
        {
            if (textBox1.Text != "")
            {
                Vendor v = (from x in cheese.Vendors where x.VendorID.Equals(textBox1.Text) select x).First();
                if (v != null)
                {
                    v.VendorName = textBox2.Text;
                    cheese.SaveChanges();
                    loadData();
                    clear();
                    EnabledStatus(false);
                    ButtonStatus(true);
                    label10.Visible = false;
                    flag = 0;
                }
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (flag == 1)
            {
                insertData();
            }
            else if (flag == 2)
            {
                updateData();
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            clear();
            textBox1.Text = "";
            EnabledStatus(false);
            ButtonStatus(true);
            label10.Visible = false;
            flag = 0;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            clear();
            label10.Visible = false;
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (flag != 1)
            {
                if (e.RowIndex >= 0)
                {
                    textBox1.Text = dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString();
                    textBox2.Text = dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString();
                }
            }
        }
    }
}
