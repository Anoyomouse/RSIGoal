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
using System.Drawing.Text;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Windows.Forms;
using RSIGoal.Properties;

namespace RSIGoal
{
    [SupportedOSPlatform("windows")]
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

            savedGoalList = JsonSerializer.Deserialize<List<SavedGoal>>(data);

            var goal = (from x in savedGoalList orderby x.Timestamp descending select x).First();
            FundStats = (goal.Funds / 100.0m).ToString("$ ###,###,###");
            NumberOfFans = goal.Fans.ToString("###,###");
        }

        /// <summary>Adds the font from memory, loads it up from the resources.</summary>
		private void AddFontFromMemory()
		{
			var pfc = new PrivateFontCollection();

			var managedFontData = Resources.Electrolize_Regular;

			var fontData = Marshal.AllocCoTaskMem(managedFontData.Length);
			Marshal.Copy(managedFontData, 0, fontData, managedFontData.Length);
			pfc.AddMemoryFont(fontData, managedFontData.Length);
			Marshal.FreeCoTaskMem(fontData);

			var ff = pfc.Families.First();
			//myFontFamily = ff;
			fontDollarAmmount = new Font(ff, 32);
            fontFans = new Font(ff, 24);
		}

		private readonly Brush shadowBrush;
		private readonly Brush textBrush;
		private string FundStats = "-";
        private string NumberOfFans = "";
		private Font fontDollarAmmount;
        private Font fontFans;

        List<SavedGoal> savedGoalList;

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
				var measureText = "$ 9 999 999 999";
				var pntLen = graphics.MeasureString(measureText, fontDollarAmmount);

				var right = (int)(this.ClientSize.Width / 2 + pntLen.Width / 2);

				right -= (int)len.Width;

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
            graphics.DrawString(fansText, fontFans, shadowBrush, new Point(20, top + 5));
            graphics.DrawString(fansText, fontFans, textBrush, new Point(20, top));
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

        private readonly HttpClient webClient = new();

        private class RSIResultDataModel
        {
            [JsonPropertyName("success")]    
            public int Success { get; set; }
            [JsonPropertyName("code")]
            public string Code { get; set; }
            [JsonPropertyName("msg")]
            public string Message { get; set; }
            [JsonPropertyName("data")]
            public RSIResultData Data { get; set; }
        }

        private class RSIResultData
        {
            [JsonPropertyName("fans")]
            public long Fans { get; set; }
            [JsonPropertyName("funds")]
            public long Funds { get; set; }
            [JsonPropertyName("alpha_slots_left")]
            public int AlphaSlotsLeft { get; set; }
        }

        private void bgwLoadData_DoWork(object sender, DoWorkEventArgs e)
        {
            bgwLoadData.ReportProgress(0);

            var response = webClient.PostAsJsonAsync<dynamic>("https://robertsspaceindustries.com/api/stats/getCrowdfundStats",
                new { fans = true, funds = true, alpha_slots = true, fleet = true }).Result;

            bgwLoadData.ReportProgress(50);

            var data = response.Content.ReadFromJsonAsync<RSIResultDataModel>().Result;
            bgwLoadData.ReportProgress(75);

            if (data.Success == 1)
            {
                FundStats = (data.Data.Funds / 100.0m).ToString("$ ###,###,###");
                NumberOfFans = data.Data.Fans.ToString("###,###");
            }
            bgwLoadData.ReportProgress(90);

            var sg = new SavedGoal {
                Fans = data.Data.Fans,
                Funds = data.Data.Funds,
                Timestamp = DateTime.Now
            };

            savedGoalList.Add(sg);

            File.WriteAllText("SavedData.json", JsonSerializer.Serialize(savedGoalList));

            bgwLoadData.ReportProgress(100);
        }

        private void bgwLoadData_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            this.Text = "Loaded: " + e.ProgressPercentage + " %";
            this.Refresh();
        }
    }
}
