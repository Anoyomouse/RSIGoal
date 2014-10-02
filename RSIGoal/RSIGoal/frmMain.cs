/**
 * Copyright (c) David-John Miller AKA Anoyomouse 2014
 *
 * See LICENCE in the project directory for licence information
 **/
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
        /// <summary>Initializes a new instance of the <see cref="frmMain"/> class.</summary>
        public frmMain()
        {
            InitializeComponent();

			this.DoubleBuffered = true;
			AddFontFromMemory();

			shadowBrush = new SolidBrush(Color.Black);
			textBrush = new SolidBrush(Color.FromArgb(0x00, 0xf0, 0xff));

            if (File.Exists("SavedData.json"))
            {
                LoadSavedData();
            }
            else
            {
                savedGoalList = new List<SavedGoal>();
            }
        }

        public void LoadSavedData()
        {
            if (!File.Exists("SavedData.json"))
            {
                savedGoalList = new List<SavedGoal>();
                return;
            }

            var data = File.ReadAllText("SavedData.json");

            savedGoalList = JsonConvert.DeserializeObject<List<SavedGoal>>(data);

            var goal = (from x in savedGoalList orderby x.Timestamp descending select x).First();
            FundStats = (goal.Funds / 100.0m).ToString("$ ###,###,###");
            NumberOfFans = goal.Fans.ToString("###,###");
            FleetNumber = goal.Fleet.ToString("###,###");
        }

        /// <summary>Adds the font from memory, loads it up from the resources.</summary>
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
			fontDollarAmmount = new Font(ff, 32);
            fontFans = new Font(ff, 24);
		}

		Brush shadowBrush;
		Brush textBrush;
		private string FundStats = "-";
        private string NumberOfFans = "";
        private string FleetNumber = "";
		private FontFamily myFontFamily;
		private Font fontDollarAmmount;
        private Font fontFans;

        List<SavedGoal> savedGoalList;
        decimal DeltaFunds;
        decimal DeltaFans;
        decimal DeltaFleet;

        /// <summary>Handles the Paint event of the frmMain control.</summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="PaintEventArgs"/> instance containing the event data.</param>
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

            int top = 0;

			DrawDollarAmount(e.Graphics, ref top);

            DrawFans(e.Graphics, ref top);
		}

        /// <summary>Draws the fund amount.</summary>
        /// <param name="graphics">The graphics.</param>
        /// <param name="top">The top.</param>
        private void DrawDollarAmount(Graphics graphics, ref int top)
		{
			var len = graphics.MeasureString(FundStats, fontDollarAmmount);

            top += 20;

			if (FundStats.Length > 5)
			{
				var left = (int)(this.ClientSize.Width / 2 - len.Width / 2);

				graphics.DrawString(FundStats, fontDollarAmmount, shadowBrush, new Point(left - 5, top + 5));
				graphics.DrawString(FundStats, fontDollarAmmount, textBrush, new Point(left, top));
			}
			else
			{
				var measureText = "$ 99 999 999";
				var pntLen = graphics.MeasureString(measureText, fontDollarAmmount);
				//var left = (int)((int)(this.ClientSize.Width / 2 - pntLen.Width / 2));// - len.Width);
				//graphics.DrawLine(Pens.Yellow, left, 0, left, this.ClientSize.Height);

				var right = (int)(this.ClientSize.Width / 2 + pntLen.Width / 2);
				//graphics.DrawLine(Pens.Yellow, right, 0, right, this.ClientSize.Height);

				right -= (int)len.Width;
				//graphics.DrawLine(Pens.Green, right, 0, right, this.ClientSize.Height);

				graphics.DrawString(FundStats, fontDollarAmmount, shadowBrush, new Point(right - 5, top + 5));
				graphics.DrawString(FundStats, fontDollarAmmount, textBrush, new Point(right, top));
			}

            top += (int)(len.Height);
        }

        /// <summary>Draws the fans.</summary>
        /// <param name="graphics">The graphics.</param>
        /// <param name="top">The top.</param>
        private void DrawFans(Graphics graphics, ref int top)
        {
            top += 5;

            var fansText = string.Format("Fans: {0}", NumberOfFans);
			var len = graphics.MeasureString(fansText, fontFans);

            graphics.DrawString(fansText, fontFans, shadowBrush, new Point(20, top + 5));
            graphics.DrawString(fansText, fontFans, textBrush, new Point(20, top));

            top += (int)(5 + len.Height);

            var fleetText = string.Format("Fleet: {0}", FleetNumber);

            graphics.DrawString(fleetText, fontFans, shadowBrush, new Point(20, top + 5));
            graphics.DrawString(fleetText, fontFans, textBrush, new Point(20, top));
        }

		private void cmdGrabData_Click(object sender, EventArgs e)
		{
            if (bgwLoadData.IsBusy)
            {
                return;
            }

            bgwLoadData.RunWorkerAsync(null);
		}

		private void frmMain_ResizeEnd(object sender, EventArgs e)
		{
			this.Refresh();
		}

        private void bgwLoadData_DoWork(object sender, DoWorkEventArgs e)
        {
            bgwLoadData.ReportProgress(0);
            HttpWebRequest hwr = HttpWebRequest.CreateHttp("https://robertsspaceindustries.com/api/stats/getCrowdfundStats");
            hwr.Method = "POST";

            var sw = new StreamWriter(hwr.GetRequestStream());
            sw.Write("{\"fans\":true,\"funds\":true,\"alpha_slots\":true,\"fleet\":true}");
            sw.Flush();

            var response = hwr.GetResponse();
            bgwLoadData.ReportProgress(50);

            JsonTextReader tr = new JsonTextReader(new StreamReader(response.GetResponseStream()));
            var data = JsonSerializer.CreateDefault().Deserialize<CrowdFundStats>(tr);
            bgwLoadData.ReportProgress(75);

            // '{"success":1,"data":{"fans":535841,"funds":5223978141,"fleet":520000,"next_goal":{"title":"53M","percentage":23.98,"goal":"$53,000,000"},"alpha_slots_left":0},"code":"OK","msg":"OK"}'
            if (data.success == 1)
            {
                FundStats = (data.data.funds / 100.0m).ToString("$ ###,###,###");
                NumberOfFans = data.data.fans.ToString("###,###");
                FleetNumber = data.data.fleet.ToString("###,###");
            }
            bgwLoadData.ReportProgress(90);

            SavedGoal sg = new SavedGoal();
            sg.Fans = data.data.fans;
            sg.Funds = data.data.funds;
            sg.Fleet = (long)data.data.fleet;
            sg.Timestamp = DateTime.Now;

            savedGoalList.Add(sg);

            File.WriteAllText("SavedData.json", JsonConvert.SerializeObject(savedGoalList));

            bgwLoadData.ReportProgress(100);
        }

        private void bgwLoadData_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            this.Text = "Loaded: " + e.ProgressPercentage + " %";
            this.Refresh();
        }
    }
}
