﻿using System;
using System.Collections.Generic;

namespace REAccess_Mobile_Database.Models
{
    public partial class AppLimitFunctionid
    {
        public int Id { get; set; }
        public int RoleId { get; set; }
        public long LimitFunctionId { get; set; }
        public int? LimitCount { get; set; }
    }
}
