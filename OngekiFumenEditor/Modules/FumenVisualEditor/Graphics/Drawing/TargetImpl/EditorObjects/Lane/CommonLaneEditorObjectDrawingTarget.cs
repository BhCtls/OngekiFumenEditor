﻿using Caliburn.Micro;
using OngekiFumenEditor.Base.OngekiObjects.ConnectableObject;
using OngekiFumenEditor.Kernel.Graphics;
using OngekiFumenEditor.Kernel.Graphics.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace OngekiFumenEditor.Modules.FumenVisualEditor.Graphics.Drawing.TargetImpl.EditorObjects.Lane
{
    public abstract class CommonLaneEditorObjectDrawingTarget : CommonBatchDrawTargetBase<ConnectableStartObject>
    {
        public override int DefaultRenderOrder => 2000;
        public override DrawingVisible DefaultVisible => DrawingVisible.Design;

        private IBatchTextureDrawing textureDrawing;
        private IHighlightBatchTextureDrawing highlightDrawing;

        public abstract Texture StartEditorTexture { get; }
        public abstract Texture NextEditorTexture { get; }
        public abstract Texture EndEditorTexture { get; }

        private Vector2 editorSize = new(16, 16);
        private List<(Vector2, Vector2, float)> selectList = new();
        private List<(Vector2, Vector2, float)> drawList = new();

        public CommonLaneEditorObjectDrawingTarget()
        {
            textureDrawing = IoC.Get<IBatchTextureDrawing>();
            highlightDrawing = IoC.Get<IHighlightBatchTextureDrawing>();
        }

        public override void DrawBatch(IFumenEditorDrawingContext target, IEnumerable<ConnectableStartObject> objs)
        {
            target.PerfomenceMonitor.OnBeginTargetDrawing(this);
            {
                var previewMinTGrid = target.TGridRange.VisiableMinTGrid;
                var previewMaxTGrid = target.TGridRange.VisiableMaxTGrid;

                void drawEditorTap(Texture texture, Vector2 size, IEnumerable<ConnectableObjectBase> o)
                {
                    foreach (var item in o)
                    {
                        if (item.TGrid < previewMinTGrid || item.TGrid > previewMaxTGrid)
                            continue;

                        var x = (float)XGridCalculator.ConvertXGridToX(item.XGrid, target.Editor);
                        var y = (float)target.ConvertToY(item.TGrid);

                        var pos = new Vector2(x, y);
                        drawList.Add((size, pos, 0f));
                        target.RegisterSelectableObject(item, pos, size);
                        if (item.IsSelected)
                            selectList.Add((size * 1.5f, pos, 0f));
                    }

                    highlightDrawing.Draw(target, texture, selectList);
                    textureDrawing.Draw(target, texture, drawList);

                    selectList.Clear();
                    drawList.Clear();
                }
                drawEditorTap(StartEditorTexture, editorSize, objs);
                drawEditorTap(NextEditorTexture, editorSize, objs.SelectMany(x => x.Children.OfType<ConnectableNextObject>()));
                drawEditorTap(EndEditorTexture, editorSize, objs.Select(x => x.Children.LastOrDefault()).OfType<ConnectableEndObject>());
            }
            target.PerfomenceMonitor.OnAfterTargetDrawing(this);
        }
    }
}
