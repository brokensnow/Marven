﻿using System;
using System.Collections.Generic;

namespace REAccess.Mobile.Database.Models
{
    public partial class Geo
    {
        public int Id { get; set; }
        public string AdCode { get; set; }
        public string GeoJson { get; set; }
    }
}
