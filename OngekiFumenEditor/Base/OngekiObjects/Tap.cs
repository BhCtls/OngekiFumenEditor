﻿using OngekiFumenEditor.Base.Attributes;
using OngekiFumenEditor.Base.OngekiObjects.Lane.Base;
using OngekiFumenEditor.Modules.FumenVisualEditor.ViewModels.OngekiObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace OngekiFumenEditor.Base.OngekiObjects
{
    public class Tap : OngekiMovableObjectBase, ILaneDockableChangable
    {
        private bool isCritical = false;
        public bool IsCritical
        {
            get { return isCritical; }
            set
            {
                isCritical = value;
                NotifyOfPropertyChange(() => IDShortName);
                NotifyOfPropertyChange(() => IsCritical);
            }
        }

        private LaneStartBase referenceLaneStart = default;
        public LaneStartBase ReferenceLaneStart
        {
            get { return referenceLaneStart; }
            set
            {
                referenceLaneStart = value;

                NotifyOfPropertyChange(() => ReferenceLaneStart);
                NotifyOfPropertyChange(() => ReferenceLaneStrId);
            }
        }

        [ObjectPropertyBrowserShow]
        [ObjectPropertyBrowserAlias("RefLaneId")]
        public int ReferenceLaneStrId => ReferenceLaneStart?.RecordId ?? -1;

        private int? referenceLaneStrIdManualSet = default;
        [ObjectPropertyBrowserShow]
        [ObjectPropertyBrowserTipText("改变此值可以改变此物件对应的轨道所属")]
        [ObjectPropertyBrowserAlias("SetRefLaneId")]
        public int? ReferenceLaneStrIdManualSet
        {
            get => referenceLaneStrIdManualSet;
            set
            {
                referenceLaneStrIdManualSet = value;
                NotifyOfPropertyChange(() => ReferenceLaneStrIdManualSet);
                referenceLaneStrIdManualSet = default;
            }
        }

        public override string IDShortName => IsCritical ? "CTP" : "TAP";

        public override void Copy(OngekiObjectBase fromObj)
        {
            base.Copy(fromObj);

            if (fromObj is not Tap from)
                return;

            IsCritical = from.IsCritical;
            ReferenceLaneStart = from.ReferenceLaneStart;
        }
    }
}
