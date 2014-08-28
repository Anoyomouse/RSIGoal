using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;
using RSIGoal.Properties;

namespace RSIGoal
{
    public partial class frmMain : Form
    {
        public frmMain()
        {
            InitializeComponent();

			this.DoubleBuffered = true;
			AddFontFromMemory();

			shadowBrush = new SolidBrush(Color.Black);
			textBrush = new SolidBrush(Color.FromArgb(0x00, 0xf0, 0xff));
        }

		private void AddFontFromMemory()
		{
			PrivateFontCollection pfc = new PrivateFontCollection();

			var managedFontData = Resources.Electrolize_Regular;

			var fontData = Marshal.AllocCoTaskMem(managedFontData.Length);
			Marshal.Copy(managedFontData, 0, fontData, managedFontData.Length);
			pfc.AddMemoryFont(fontData, managedFontData.Length);
			Marshal.FreeCoTaskMem(fontData);

			var ff = pfc.Families.First();
			myFontFamily = ff;
			myFont = new Font(ff, 32);
		}

		Brush shadowBrush;
		Brush textBrush;
		private string FundStats = "-";
		private FontFamily myFontFamily;
		private Font myFont;

		private void frmMain_Paint(object sender, PaintEventArgs e)
		{
			e.Graphics.DrawImage(Resources.seriescarouselbg, 0, 0);

			var bg_image = Resources.bg_large_lines;
			var tiles_x = this.ClientSize.Width / bg_image.Width;
			var tiles_y = this.ClientSize.Height / bg_image.Height;

			for (int x = 0; x <= tiles_x; x++)
				for (int y = 0; y <= tiles_y; y++)
				{
					e.Graphics.DrawImage(bg_image, bg_image.Width * x, bg_image.Height * y);
				}

			DrawTextShadow(e.Graphics);
		}

		private void DrawTextShadow(Graphics graphics)
		{
			var len = graphics.MeasureString(FundStats, myFont);

			var top = (int)(this.ClientSize.Height / 2 - len.Height / 2);

			if (FundStats.Length > 5)
			{
				var left = (int)(this.ClientSize.Width / 2 - len.Width / 2);

				graphics.DrawString(FundStats, myFont, shadowBrush, new Point(left - 5, top + 5));
				graphics.DrawString(FundStats, myFont, textBrush, new Point(left, top));
			}
			else
			{
				var measureText = "$ 99 999 999";
				var pntLen = graphics.MeasureString(measureText, myFont);
				//var left = (int)((int)(this.ClientSize.Width / 2 - pntLen.Width / 2));// - len.Width);
				//graphics.DrawLine(Pens.Yellow, left, 0, left, this.ClientSize.Height);

				var right = (int)(this.ClientSize.Width / 2 + pntLen.Width / 2);
				//graphics.DrawLine(Pens.Yellow, right, 0, right, this.ClientSize.Height);

				right -= (int)len.Width;
				//graphics.DrawLine(Pens.Green, right, 0, right, this.ClientSize.Height);

				graphics.DrawString(FundStats, myFont, shadowBrush, new Point(right - 5, top + 5));
				graphics.DrawString(FundStats, myFont, textBrush, new Point(right, top));
			}
		}

		private void GetData()
		{
			HttpWebRequest hwr = HttpWebRequest.CreateHttp("https://robertsspaceindustries.com/api/stats/getCrowdfundStats");
			hwr.Method = "POST";

			var sw = new StreamWriter(hwr.GetRequestStream());
			sw.Write("{\"fans\":true,\"funds\":true,\"alpha_slots\":true}");
			sw.Flush();

			var response = hwr.GetResponse();
			JsonTextReader tr = new JsonTextReader(new StreamReader(response.GetResponseStream()));
			var data = JsonSerializer.CreateDefault().Deserialize<CrowdFundStats>(tr);

			// '{"success":1,"data":{"fans":535841,"funds":5223978141,"next_goal":{"title":"53M","percentage":23.98,"goal":"$53,000,000"},"alpha_slots_left":0},"code":"OK","msg":"OK"}'
			if (data.success == 1)
			{
				FundStats = (data.data.funds / 100.0m).ToString("$ ###,###,###");
				this.Refresh();
			}
		}

		private void cmdGrabData_Click(object sender, EventArgs e)
		{
			GetData();
		}

		private void frmMain_ResizeEnd(object sender, EventArgs e)
		{
			this.Refresh();
		}
    }
}
