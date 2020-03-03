using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Drawing.Imaging;


namespace Cheese_Factory
{
    public partial class _BarcodeForm : Form
    {
        //link http://genieonhire.com/images/profile.png
       
        public _BarcodeForm(String barcode, String division)
        {
            InitializeComponent();

            Bitmap bitMap = new Bitmap(barcode.Length * 40, 150);
            using (Graphics graphic = Graphics.FromImage(bitMap))
            {
                //Font font = new System.Drawing.Font("IDAutomationSC128L DEMO", 20);
                Font font = new System.Drawing.Font("IDAutomationHC39M", 20);
                PointF point = new PointF(2f, 2f);
                SolidBrush black = new SolidBrush(Color.Black);
                SolidBrush white = new SolidBrush(Color.White);
                graphic.FillRectangle(white, 0,0, bitMap.Width, bitMap.Height);
                graphic.DrawString("*"+barcode+"*",font, black, point);
            }
            using (MemoryStream memoryStream = new MemoryStream())
            {
                bitMap.Save(memoryStream, ImageFormat.Png);
                pictureBox1.Image = bitMap;
                //pictureBox1.Height = bitMap.Height;
                //pictureBox1.Width = bitMap.Width;
                Bitmap bmp = new Bitmap(this.Width-20, this.Height-20);
                //this.DrawToBitmap(bmp, this.Bounds);
                label1.Text = division;
                pictureBox1.DrawToBitmap(bmp, pictureBox1.Bounds);
                pictureBox2.DrawToBitmap(bmp, pictureBox2.Bounds);
                label1.DrawToBitmap(bmp, label1.Bounds);
                //this.DrawToBitmap((Bitmap)pictureBox1.Image, pictureBox1.Bounds);
                //this.DrawToBitmap((Bitmap)pictureBox2.Image, pictureBox2.Bounds);
                bmp.Save(Application.StartupPath+"\\Barcode\\"+barcode+".Png");

            }

            
        }

    }
}
