﻿using OngekiFumenEditor.Base;
using OngekiFumenEditor.Base.OngekiObjects.Beam;
using OngekiFumenEditor.Kernel.Graphics.Base;
using OngekiFumenEditor.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Drawing;
using System.Linq;

namespace OngekiFumenEditor.Modules.FumenVisualEditor.Graphics.Drawing.TargetImpl.OngekiObjects.Beam
{
    [Export(typeof(IFumenEditorDrawingTarget))]
    internal class BeamLazerDrawingTarget : CommonDrawTargetBase<BeamStart>, IDisposable
    {
        private DefaultBeamLazerTextureDrawing lazerDrawing;
        private Texture textureBody;
        private Texture textureWarn;

        public override IEnumerable<string> DrawTargetID { get; } = new[] { "BMS" };
        public override DrawingVisible DefaultVisible => DrawingVisible.Preview;

        public override int DefaultRenderOrder => 300;

        public BeamLazerDrawingTarget()
        {
            lazerDrawing = new();

            void load(ref Texture t,string name) {
                var info = System.Windows.Application.GetResourceStream(new Uri(@"Modules\FumenVisualEditor\Views\OngekiObjects\" + name, UriKind.Relative));
                using var bitmap = Image.FromStream(info.Stream) as Bitmap;
                t = new Texture(bitmap);

            }

            load(ref textureBody, "beam_body.png");
            textureBody.TextureWrapT = OpenTK.Graphics.OpenGL.TextureWrapMode.Repeat;

            load(ref textureWarn, "beam_warn.png");
            textureWarn.TextureWrapS = OpenTK.Graphics.OpenGL.TextureWrapMode.Repeat;
            textureWarn.TextureWrapT = OpenTK.Graphics.OpenGL.TextureWrapMode.Repeat;
        }

        public override void Draw(IFumenEditorDrawingContext target, BeamStart obj)
        {
            //todo 宽度目测的，需要精确计算
            var xGridWidth = XGridCalculator.CalculateXUnitSize(target.Editor.Setting.XGridDisplayMaxUnit, target.ViewWidth, target.Editor.Setting.XGridUnitSpace) / target.Editor.Setting.XGridUnitSpace;
            var width = xGridWidth * 3f * obj.WidthId;

            var beginTGrid = obj.MinTGrid;
            var endTGrid = obj.MaxTGrid;

            var duration = endTGrid.TotalGrid - beginTGrid.TotalGrid;
            if (duration == 0)
                return;

            var curTGrid = target.Editor.GetCurrentTGrid();

            /* ^  -- leadOutTGrid
             * |  |
             * |  |   progress = [1,2]
             * |  |
             *    -- endTGrid
             *    |
             *    |
             *    |   progress = [0,1]
             *    |
             *    |
             *    -- beginTGrid
             *    |
             *    |   progress = [-1,0]
             *    |
             *    --leadInTGrid
             */

            double progress;
            XGrid xGrid;
            bool prepareWarn = false;

            if (curTGrid < beginTGrid)
            {
                //progress = [-1,0]
                var leadBodyInTGrid = TGridCalculator.ConvertAudioTimeToTGrid(TGridCalculator.ConvertTGridToAudioTime(beginTGrid, target.Editor) - TimeSpan.FromMilliseconds(BeamStart.LEAD_IN_BODY_DURATION), target.Editor);
                progress = MathUtils.Normalize(leadBodyInTGrid.TotalGrid, beginTGrid.TotalGrid, curTGrid.TotalGrid) - 1;
                xGrid = obj.XGrid;

                prepareWarn = true;
            }
            else if (curTGrid > endTGrid)
            {
                //progress = [1,2]
                var leadOutTGrid = TGridCalculator.ConvertAudioTimeToTGrid(TGridCalculator.ConvertTGridToAudioTime(endTGrid, target.Editor) + TimeSpan.FromMilliseconds(BeamStart.LEAD_OUT_DURATION), target.Editor);
                progress = MathUtils.Normalize(endTGrid.TotalGrid, leadOutTGrid.TotalGrid, curTGrid.TotalGrid) + 1;
                xGrid = obj.Children.LastOrDefault()?.XGrid;
            }
            else
            {
                //progress = [0,1]
                progress = MathUtils.Normalize(beginTGrid.TotalGrid, endTGrid.TotalGrid, curTGrid.TotalGrid);
                xGrid = obj.CalulateXGrid(curTGrid);
            }

            if (xGrid is null)
                return;
            var x = (float)XGridCalculator.ConvertXGridToX(xGrid, target.Editor);

            if (prepareWarn)
            {
                var leadInTGrid = TGridCalculator.ConvertAudioTimeToTGrid(TGridCalculator.ConvertTGridToAudioTime(beginTGrid, target.Editor) - TimeSpan.FromMilliseconds(BeamStart.LEAD_IN_DURATION), target.Editor);
                var warnProgress = MathUtils.Normalize(leadInTGrid.TotalGrid, beginTGrid.TotalGrid, curTGrid.TotalGrid) - 0.25;
                lazerDrawing.Draw(target, textureWarn, (int)width, x, (float)warnProgress, new(1, 215 / 255.0f, 0, 0.5f));
            }

            lazerDrawing.Draw(target, textureBody, (int)width, x, (float)progress, OpenTK.Mathematics.Vector4.One);
        }

        public void Dispose()
        {
            lazerDrawing?.Dispose();
            lazerDrawing = default;

            textureBody?.Dispose();
            textureBody = default;

            textureWarn?.Dispose();
            textureWarn = default;
        }
    }
}
