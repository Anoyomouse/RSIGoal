using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using RSIGoal.Properties;

namespace RSIGoal
{
    public partial class frmMain : Form
    {
        public frmMain()
        {
            InitializeComponent();

			this.DoubleBuffered = true;
        }

		private void frmMain_Paint(object sender, PaintEventArgs e)
		{
			e.Graphics.DrawImage(Resources.seriescarouselbg, 0, 0);

			var bg_image = Resources.bg_large_lines;
			var tiles_x = this.ClientSize.Width / bg_image.Width;
			var tiles_y = this.ClientSize.Height / bg_image.Height;

			for (int x = 0; x < tiles_x; x++)
				for (int y = 0; y < tiles_y; y++)
				{
					e.Graphics.DrawImage(bg_image, bg_image.Width * x, bg_image.Height * y);
				}
		}

		private void DrawTextShadow()
		{
			//TextRenderer.DrawText(Graphics, text, font, new Point(20,20), Color.FromArgb(0x0000f0ff);
		}
    }
}
