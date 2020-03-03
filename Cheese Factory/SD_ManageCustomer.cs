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
    public partial class SD_ManageCustomer : Form
    {
        CheeseEntities cheese = new CheeseEntities();
        int flag=0;
        public SD_ManageCustomer()
        {
            InitializeComponent();
            loadData();
            EnabledStatus(false);
            ButtonStatus(true);
        }

        private void loadData()
        {
            var customer = from x in cheese.Customers
                           select new { x.CustomerID, x.CustomerName, x.CustomerGender, x.CustomerAddress, x.CustomerContactNumber, x.CustomerEmail };
            dataGridView1.DataSource = customer.ToList();
            if (customer != null)
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
            button2.Enabled = status;
            button3.Enabled = status;
            button4.Enabled = !status;
            button5.Enabled = !status;
            button6.Enabled = !status;
        }

        private void EnabledStatus(bool status)
        {
            textBox1.Enabled = false;
            textBox2.Enabled = status;
            textBox3.Enabled = status;
            textBox4.Enabled = status;
            textBox5.Enabled = status;
            radioButton1.Enabled = status;
            radioButton2.Enabled = status;
        }

        private void clear()
        {
            /*foreach (Control item in this.Controls)
            {
                if (item.GetType() == typeof(TextBox))
                    ((TextBox)item).Text = "";
                else if (item.GetType() == typeof(RadioButton))
                    ((RadioButton)item).Checked = false;
                else if (item.GetType() == typeof(ComboBox))
                    ((ComboBox)item).SelectedIndex = 0;
                else if (item.GetType() == typeof(CheckBox))
                    ((CheckBox)item).Checked = false;
                else if (item.GetType() == typeof(NumericUpDown))
                    ((NumericUpDown)item).Value = 0;
                //else if(item.GetType()==typeof(DateTimePicker))
                //  ((DateTimePicker)item).
            }*/
            textBox1.Text = "";
            textBox2.Text = "";
            textBox3.Text = "";
            textBox4.Text = "";
            textBox5.Text = "";
            radioButton1.Checked = false;
            radioButton2.Checked = false;
        }
        public string generateCustomerID()
        {
            string newID = "";
            var customer = from temp in cheese.Customers select temp;
            var countOfRows = customer.Count();

            if (countOfRows != 0)
            {
                var data = customer.ToList();
                var lastRow = data.ElementAt(countOfRows - 1);
                string curr = lastRow.CustomerID;
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
                return "CST0001";
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            clear();
            EnabledStatus(true);
            ButtonStatus(false);
            flag = 1;
            textBox1.Text = generateCustomerID();
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
                    Customer deleteC = (from x in cheese.Customers
                                        where x.CustomerID.Equals(textBox1.Text)
                                        select x).First();
                    var deleteDC = (from x in cheese.DetailCustomers
                                    where x.CustomerID.Equals(textBox1.Text)
                                    select x);
                    cheese.Customers.Remove(deleteC);
                    foreach (DetailCustomer detailCustomer in deleteDC)
                    {
                        cheese.DetailCustomers.Remove(detailCustomer);
                    }
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
                label7.Visible = true;
                label7.Text = "Name Must be Filled";
            }
            else if (radioButton1.Checked == false && radioButton2.Checked == false)
            {
                label7.Visible = true;
                label7.Text = "Gender Must be Choose";
            }
            else if (textBox3.Text == "")
            {
                label7.Visible = true;
                label7.Text = "Address Can't be Empty";
            }
            else if (textBox4.Text=="")
            {
                label7.Visible = true;
                label7.Text = "Contact Number Must be Filled";
            }
            else if (textBox5.Text == "")
            {
                label7.Visible = true;
                label7.Text = "Email Can Not Be Empty";
            }
            else
            {
                
                Customer c = new Customer();
                DetailCustomer dC = new DetailCustomer();
                c.CustomerID = textBox1.Text;
                c.CustomerName = textBox2.Text;
                if (radioButton1.Checked)
                {
                    c.CustomerGender = "Male";
                }
                else if (radioButton2.Checked)
                {
                    c.CustomerGender = "Female";
                }
                c.CustomerAddress = textBox3.Text;
                c.CustomerContactNumber = textBox4.Text;
                c.CustomerEmail = textBox5.Text;
                c.RegisterBy = _MainForm.userID;
                c.RegisterDate = DateTime.Now;
                dC.CustomerID = textBox1.Text;
                dC.LastUpdatedBy = _MainForm.userID;
                dC.LastUpdatedDate = DateTime.Now;
                cheese.Customers.Add(c);
                cheese.DetailCustomers.Add(dC);
                cheese.SaveChanges();
                loadData();
                clear();
                EnabledStatus(false);
                ButtonStatus(true);
                label7.Visible = false;
                flag = 0;
            }
        }

        private void updateData()
        {
            if (textBox1.Text != "")
            {
                Customer c = (from x in cheese.Customers where x.CustomerID.Equals(textBox1.Text) select x).First();
                if (c != null)
                {
                    c.CustomerName = textBox2.Text;
                    if (radioButton1.Checked)
                    {
                        c.CustomerGender = "Male";
                    }
                    else if (radioButton2.Checked)
                    {
                        c.CustomerGender = "Female";
                    }
                    c.CustomerAddress = textBox3.Text;
                    c.CustomerContactNumber = textBox4.Text;
                    c.CustomerEmail = textBox5.Text;

                    DetailCustomer dC = new DetailCustomer();
                    dC.CustomerID = textBox1.Text;
                    dC.LastUpdatedBy = _MainForm.userID;
                    dC.LastUpdatedDate = DateTime.Now;
                    cheese.DetailCustomers.Add(dC);
                    cheese.SaveChanges();
                    loadData();
                    clear();
                    EnabledStatus(false);
                    ButtonStatus(true);
                    label7.Visible = false;
                    flag = 0;
                }
            }
        }


        private void button6_Click(object sender, EventArgs e)
        {
            clear();
            textBox1.Text = "";
            EnabledStatus(false);
            ButtonStatus(true);
            label7.Visible = false;
            flag = 0;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            clear();
            label7.Visible = false;
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (flag != 1)
            {
                if (e.RowIndex >= 0)
                {
                    textBox1.Text = dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString();
                    textBox2.Text = dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString();
                    if (dataGridView1.Rows[e.RowIndex].Cells[2].Value.ToString() == "Male")
                    {
                        radioButton1.Checked = true;
                    }
                    else if (dataGridView1.Rows[e.RowIndex].Cells[2].Value.ToString() == "Female")
                    {
                        radioButton2.Checked = true;
                    }
                    textBox3.Text = dataGridView1.Rows[e.RowIndex].Cells[3].Value.ToString();
                    textBox4.Text = dataGridView1.Rows[e.RowIndex].Cells[4].Value.ToString();
                    textBox5.Text = dataGridView1.Rows[e.RowIndex].Cells[5].Value.ToString();
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
    }
}
