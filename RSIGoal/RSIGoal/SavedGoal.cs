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
    public class SavedGoal
    {
        public DateTime Timestamp { get; set; }

        public decimal Funds { get; set; }

        public long Fans { get; set; }
    }
}
