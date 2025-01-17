﻿using OngekiFumenEditor.Base.EditorObjects.Svg;
using OngekiFumenEditor.Kernel.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using static OngekiFumenEditor.Kernel.Graphics.ILineDrawing;

namespace OngekiFumenEditor.Modules.FumenVisualEditor.Graphics.Drawing.TargetImpl.EditorObjects.SVG.Cached
{
    public interface ICachedSvgRenderDataManager
    {
        public List<LineVertex> GetRenderData(IDrawingContext target, SvgPrefabBase svgPrefab, out bool isCached, out Rect bound);
    }
}
