﻿using OngekiFumenEditor.Utils.ObjectPool;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using System;
using System.Buffers;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;
using System.Windows.Documents;
using OngekiFumenEditor.Utils;
using System.ComponentModel.Composition;
using Polyline2DCSharp;
using Caliburn.Micro;
using static OngekiFumenEditor.Base.OngekiObjects.BulletPallete;
using OngekiFumenEditor.Kernel.Graphics.Drawing.DefaultDrawingImpl.LineDrawing;
using OngekiFumenEditor.Modules.FumenVisualEditor.Graphics;
using OngekiFumenEditor.Kernel.Graphics.Drawing.DefaultDrawingImpl.StringDrawing.String;
using OngekiFumenEditor.Kernel.Graphics.Base;

namespace OngekiFumenEditor.Kernel.Graphics.Drawing.DefaultDrawingImpl.PolygonDrawing
{
    [Export(typeof(IPolygonDrawing))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class DefaultPolygonDrawing : CommonDrawingBase, IPolygonDrawing, IDisposable
    {
        private readonly Base.Shader shader;
        private readonly int vbo;
        private readonly int vao;

        private float[] postData = new float[VertexCount * 6];
        const int VertexByteSize = (2 + 4) * sizeof(float);
        const int VertexCount = 300000;

        private int postVertexCount = 0;
        private IDrawingContext target;

        public int AvailablePostableVertexCount => VertexByteSize - postVertexCount;

        public DefaultPolygonDrawing()
        {
            shader = CommonLineShader.Shared;

            vbo = GL.GenBuffer();
            vao = GL.GenVertexArray();

            Init();
        }

        private void Init()
        {
            GL.BindVertexArray(vao);
            {
                GL.BindBuffer(BufferTarget.ArrayBuffer, vbo);
                {
                    GL.BufferData(BufferTarget.ArrayBuffer, VertexByteSize * VertexCount, postData, BufferUsageHint.DynamicDraw);

                    GL.EnableVertexAttribArray(0);
                    GL.VertexAttribPointer(0, 2, VertexAttribPointerType.Float, false, sizeof(float) * 6, 0);

                    GL.EnableVertexAttribArray(1);
                    GL.VertexAttribPointer(1, 4, VertexAttribPointerType.Float, false, sizeof(float) * 6, sizeof(float) * 2);
                }
                GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            }
            GL.BindVertexArray(0);
        }


        public void Dispose()
        {
            GL.DeleteVertexArray(vao);
            GL.DeleteBuffer(vbo);
        }

        public void Begin(IDrawingContext target)
        {
            target.PerfomenceMonitor.OnBeginDrawing(this);
            this.target = target;
            shader.Begin();
            shader.PassUniform("Model", GetOverrideModelMatrix());
            shader.PassUniform("ViewProjection", GetOverrideViewProjectMatrixOrDefault(target));
            GL.BindVertexArray(vao);
        }

        public void PostPoint(System.Numerics.Vector2 Point, System.Numerics.Vector4 Color)
        {
            if (postVertexCount > VertexCount)
                throw new InvalidOperationException($"postVertexCount > VertexCount({VertexCount})");

            postData[6 * postVertexCount + 0] = Point.X;
            postData[6 * postVertexCount + 1] = Point.Y;
            postData[6 * postVertexCount + 2] = Color.X;
            postData[6 * postVertexCount + 3] = Color.Y;
            postData[6 * postVertexCount + 4] = Color.Z;
            postData[6 * postVertexCount + 5] = Color.W;

            postVertexCount++;
        }

        public void End()
        {
            FlushDraw();

            GL.BindVertexArray(0);
            shader.End();
            target.PerfomenceMonitor.OnAfterDrawing(this);

            target = default;
        }

        private void FlushDraw()
        {
            GL.NamedBufferSubData(vbo, IntPtr.Zero, postVertexCount * VertexByteSize, postData);
            GL.DrawArrays(PrimitiveType.TriangleStrip, 0, postVertexCount);
            target.PerfomenceMonitor.CountDrawCall(this);
            postVertexCount = 0;
        }
    }
}
