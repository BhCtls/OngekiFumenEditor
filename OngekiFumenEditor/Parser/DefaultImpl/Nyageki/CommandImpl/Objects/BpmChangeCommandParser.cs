﻿using OngekiFumenEditor.Base;
using OngekiFumenEditor.Base.OngekiObjects;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OngekiFumenEditor.Parser.DefaultImpl.Nyageki.CommandImpl.Objects
{
    [Export(typeof(INyagekiCommandParser))]
    public class BpmChangeCommandParser : INyagekiCommandParser
    {
        public string CommandName => "BpmChange";

        public void ParseAndApply(OngekiFumen fumen, string[] seg)
        {
            //$"BpmChange:{bpm.BPM}:T[{bpm.TGrid.Unit},{bpm.TGrid.Grid}]"
            var bpm = new BPMChange();
            var data = seg[1].Split(":");

            bpm.BPM = float.Parse(data[0]);
            bpm.TGrid = data[1].ParseToTGrid();

            fumen.AddObject(bpm);
        }
    }
}
