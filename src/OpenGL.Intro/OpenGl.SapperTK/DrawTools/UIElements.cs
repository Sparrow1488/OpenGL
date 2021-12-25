using OpenGl.SapperTK.Entities;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;
using System;
using System.IO;

namespace OpenGl.SapperTK.DrawTools
{
    internal static class UIElements
    {
        public static int CreateButton(float[] vertices)
        {
            return CreateElement(vertices, new uint[]
            {
                0, 1, 2,
                0, 3, 2
            });
        }

        public static void DrawElement(int elem, int shaderProg)
        {
            GL.UseProgram(shaderProg);
            GL.BindVertexArray(elem);
            GL.DrawElements(PrimitiveType.Triangles, 6, DrawElementsType.UnsignedInt, 0);
        }

        public static int CreateCenterQuadre(float size)
        {
            float half = size / 2;
            var vertices = new float[]
            {
                -half, -half, 0f,
                -half, half, 0f,
                half, half, 0f,
                half, -half, 0f
            };
            var indices = new uint[]
            {
                0, 1, 2,
                0, 2, 3
            };
            return CreateElement(vertices, indices);
        }
        
        /// <summary>
        /// Используя тип uniform в шейдерной программе, плавно анимируем затухание, появление цвета
        /// </summary>
        public static void UniformAnimate(int shaderProgram, string uniformValue)
        {
            var time = GLFW.GetTime();
            var greenValue = Math.Sin(time) / 2f + 0.5f;
            var redValue = Math.Cos(time) / 2f + 0.5f;
            var vertexColorLocation = GL.GetUniformLocation(shaderProgram, uniformValue);
            GL.Uniform4(vertexColorLocation, new Color4((float)redValue, (float)greenValue, 0f, 1.0f));
        }

        public static void UniformTransform()
        {

        }

        /// <returns>VAO</returns>
        public static int CreateElement(float[] vertices, uint[] indices)
        {
            int verticesBufferObject;
            int elementsBufferObject;
            int vertexArrayObject;

            vertexArrayObject = GL.GenVertexArray();
            GL.BindVertexArray(vertexArrayObject);
            GL.BindBuffer(BufferTarget.ArrayBuffer, vertexArrayObject);

            verticesBufferObject = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, verticesBufferObject);
            GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(float), vertices, BufferUsageHint.StaticDraw);
            GL.BindBuffer(BufferTarget.ArrayBuffer, verticesBufferObject);

            elementsBufferObject = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, elementsBufferObject);
            GL.BufferData(BufferTarget.ElementArrayBuffer, indices.Length * sizeof(uint), indices, BufferUsageHint.StaticDraw);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, elementsBufferObject);

            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);
            GL.EnableVertexAttribArray(0);

            GL.BindVertexArray(0);

            return vertexArrayObject;
        }

        public static int CreateColorElement(float[] vertices)
        {
            var vao = GL.GenVertexArray();
            GL.BindVertexArray(vao);
            GL.BindBuffer(BufferTarget.ArrayBuffer, vao);

            var vbo = GL.GenVertexArray();
            GL.BindBuffer(BufferTarget.ArrayBuffer, vbo);
            GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(float), vertices, BufferUsageHint.StaticDraw);
            GL.BindBuffer(BufferTarget.ArrayBuffer, vbo);

            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 6 * sizeof(float), 0);
            GL.EnableVertexAttribArray(0);

            GL.VertexAttribPointer(1, 3, VertexAttribPointerType.Float, false, 6 * sizeof(float), 3 * sizeof(float));
            GL.EnableVertexAttribArray(1);

            return vao;
        }

        /// <summary>
        /// Отрисовывать через GL.DrawArrays(Triangle, 0, 3)
        /// </summary>
        /// <returns>VAO</returns>
        public static int CreateRainbowElement(float[] vertAndColors)
        {
            var vertexShaderSource = "./Shaders/Custom/Static/vertex2.glsl";
            var fragmentShaderSource = "./Shaders/Custom/Static/fragment2.glsl";
            var shaderProgram = new Shader(vertexShaderSource, fragmentShaderSource).UID;
            GL.UseProgram(shaderProgram);
            return CreateColorElement(vertAndColors);
        }
    }
}
