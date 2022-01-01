﻿using OngekiFumenEditor.Modules.FumenVisualEditor.ViewModels.OngekiObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OngekiFumenEditor.Base.OngekiObjects.Wall
{
    public class WallRightNext : WallNext
    {
        public override string IDShortName => "WRN";
        public override Type ModelViewType => typeof(WallNextViewModel<WallRightNext>);
    }
}
