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
    public partial class SP_Cheese : Form
    {
        CheeseEntities cheese = new CheeseEntities();
        int flag = 0;
        public SP_Cheese()
        {
            InitializeComponent();
            loadData();
            EnabledStatus(false);
            ButtonStatus(true);
        }

        private void loadData()
        {
            var keju = from temp in cheese.Cheese
                         select new { temp.CheeseID, temp.CheeseName, temp.CheeseLifeTime };
            dataGridView1.DataSource = keju.ToList();
            if (keju != null)
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
            dateTimePicker1.Enabled = status;
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
            dateTimePicker1.Value = DateTime.Today;
            label10.Visible = false;
        }

        public string generateCheeseID()
        {
            string newID = "";
            var keju = from temp in cheese.Cheese select temp;
            var countOfRows = keju.Count();

            if (countOfRows != 0)
            {
                var data = keju.ToList();
                var lastRow = data.ElementAt(countOfRows - 1);
                string curr = lastRow.CheeseID;
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
                return "CHS0001";
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            clear();
            EnabledStatus(true);
            ButtonStatus(false);
            flag = 1;
            textBox1.Text = generateCheeseID();
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
                    Cheese deleteC = (from x in cheese.Cheese
                                      where x.CheeseID.Equals(textBox1.Text)
                                      select x).First();
                    cheese.Cheese.Remove(deleteC);
                    cheese.SaveChanges();
                    loadData();
                    clear();
                    textBox1.Text = "";
                }
            }
            flag = 0;
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

        private void button5_Click(object sender, EventArgs e)
        {
            clear();
            label10.Visible = false;
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
        private void insertData()
        {
            if (textBox2.Text == "")
            {
                label10.Visible = true;
                label10.Text = "Cheese Variation Must be Filled";
            }
            else
            {
                Cheese c = new Cheese();
                c.CheeseID = textBox1.Text;
                c.CheeseName = textBox2.Text;
                c.CheeseLifeTime = dateTimePicker1.Value;
                c.InputedBy = _MainForm.userID;
                c.CheeseInputDate = DateTime.Now;
                cheese.Cheese.Add(c);
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
                Cheese c = (from x in cheese.Cheese where x.CheeseID.Equals(textBox1.Text) select x).First();
                if (c != null)
                {
                    c.CheeseName = textBox2.Text;
                    c.CheeseLifeTime = dateTimePicker1.Value;
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

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (flag != 1)
            {
                if (e.RowIndex >= 0)
                {
                    textBox1.Text = dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString();
                    textBox2.Text = dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString();
                    dateTimePicker1.Value = DateTime.Parse(dataGridView1.Rows[e.RowIndex].Cells[2].Value.ToString());
                }
            }
        }

    }
}
