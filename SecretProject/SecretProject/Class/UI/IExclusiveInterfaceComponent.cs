﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecretProject.Class.UI
{
    public interface IExclusiveInterfaceComponent
    {
         bool IsActive { get; set; }
        bool FreezesGame { get; set; }
    }
}