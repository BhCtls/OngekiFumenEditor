﻿using OngekiFumenEditor.Base.Attributes;
using OngekiFumenEditor.Modules.FumenVisualEditor.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OngekiFumenEditor.Base
{
    public interface ITimelineObject : IComparable<ITimelineObject>
    {
        public TGrid TGrid { get; set; }
    }
}
