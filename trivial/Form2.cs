using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace trivial {
    public partial class Form2 : Form {
        public Form2() {
            InitializeComponent();
        }

        private void Form2_Load(object sender, EventArgs e) {
            updatePictureBox();
        }

        private void updatePictureBox() {
            int l = trackBar1.Value;
            int h = trackBar2.Maximum - trackBar2.Value;

            Bitmap bmp = new Bitmap(pictureBox2.Width, pictureBox2.Height);
            Graphics g = Graphics.FromImage(bmp);

            double posx = pictureBox2.Width / 100.0;
            double posy = pictureBox2.Height / 100.0;

            int length = 500;

            float posAx = 0;
            float posAy = (float)(pictureBox2.Height - posy * 10);
            float posBx = (float)(posx * l);
            float posBy = (float)(posy * h);

            g.FillRectangle(new SolidBrush(Color.Red), posAx, posAy, 5, pictureBox2.Height);
            g.FillRectangle(new SolidBrush(Color.Blue), posBx, posBy, 5, pictureBox2.Height);

            var subX = posBx - posAx;
            var subY = posBy - posAy;

            var _newX = length / Math.Sqrt(1 + subY * subY / subX / subX);
            var _newY = _newX * subY / subX;

            var newX = (float)(_newX + posAx);
            var newY = (float)(_newY + posAy);

            g.DrawLine(new Pen(Color.Black), posAx, posAy, newX, newY);
            g.FillRectangle(Brushes.Black, posAx, pictureBox2.Height - 5,(float) _newX, 5);

            g.Dispose();
            pictureBox2.Image = bmp;
        }

        private void trackBar1_Scroll(object sender, EventArgs e) {
            updatePictureBox();
        }

        private void trackBar2_Scroll(object sender, EventArgs e) {
            updatePictureBox();
        }
    }
}
