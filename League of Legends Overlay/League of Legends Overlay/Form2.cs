using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace League_of_Legends_Overlay
{
    public partial class Form2 : Form
    {
        public PictureBox overlayPictureBox;

        public Form2()
        {
            InitializeComponent();
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            overlayPictureBox.Location = new Point(0, 0);
            overlayPictureBox.Size = this.Size;
        }

    }
}
