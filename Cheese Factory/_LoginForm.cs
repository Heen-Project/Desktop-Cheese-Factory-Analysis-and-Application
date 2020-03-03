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
    public partial class _LoginForm : Form
    {
        CheeseEntities Cheese = new CheeseEntities();
        bool toogle = true;
        public _LoginForm()
        {
            InitializeComponent();
            label10.Visible = false;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (textBox1.Text.Length >= 7)
            {
                try
                {
                    var Data = (from Temp in Cheese.Employees where Temp.EmployeeID.Equals(textBox1.Text) select Temp).First();
                    if (Data != null)
                    {
                        _MainForm.userID = Data.EmployeeID;
                        _MainForm.userName = Data.EmployeeName;
                        _MainForm.getInstance().login(Data.Division.ToString(), Data.EmployeeName.ToString());
                        this.Dispose();
                    }
                }
                catch
                {
                    toogle = !toogle;
                    if (toogle == true)
                    {
                        label10.ForeColor = Color.Green;
                    }
                    else if (toogle==false)
                    {
                        label10.ForeColor = Color.Red;
                    }
                    label10.Visible = true;
                    label10.Text = "Login Failed";
                    textBox1.Text = "";
                }
            }
        }
    }
}
