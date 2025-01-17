﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OngekiFumenEditor.Base.Attributes
{
    /// <summary>
    /// 只允许单个物件被选择时显示此属性
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, Inherited = true, AllowMultiple = false)]
    public class ObjectPropertyBrowserSingleSelectedOnly : Attribute
    {
    }
}
