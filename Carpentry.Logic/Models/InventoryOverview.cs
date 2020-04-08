﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Carpentry.Logic.Models
{
    public class InventoryOverview // TODO : Rename to CardOverviewDto
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Type { get; set; }

        public string Cost { get; set; }

        public int? Cmc { get; set; }

        public string Img { get; set; }

        public int Count { get; set; }

        //category / status / group
        public string Description { get; set; }
    }
}
