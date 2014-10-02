/**
 * Copyright (c) David-John Miller AKA Anoyomouse 2014
 *
 * See LICENCE in the project directory for licence information
 **/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSIGoal
{
	public class CrowdFundStats
	{
		public int success { get; set; }
		public CrowdFundStatsData data { get; set; }
		public string code { get; set; }
		public string msg { get; set; }
	}

	public class CrowdFundStatsData
	{
		public long fans { get; set; }
		public decimal funds { get; set; }
        public decimal fleet { get; set; }
		public CrowdFundStatsDataNextGoal next_goal { get; set; }
		public long alpha_slots_left { get; set; }
	}

	public class CrowdFundStatsDataNextGoal
	{
		public string title { get; set; }
		public float percentage { get; set; }
		public string goal { get; set; }
	}
}
