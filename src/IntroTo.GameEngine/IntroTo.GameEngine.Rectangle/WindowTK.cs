using OpenTK.Graphics.OpenGL4;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using System.Drawing;

namespace IntroTo.GameEngine.Rectangle;

public class WindowTK : GameWindow
{
    public WindowTK(
        int width,
        int height,
        string title) : base(GameWindowSettings.Default, NativeWindowSettings.Default)
    {
        Context.SwapInterval = 2;
        Size = (width, height); 
        Title = title;
        FigureShader = new("./Shaders/shader.vert", "./Shaders/shader.frag");
    }

    public Shader FigureShader { get; private set; }
    public int VertexArrayObject { get; private set; }
    public int FigureIndicesCount { get; private set; }

    protected override void OnLoad()
    {
        base.OnLoad();
        GL.ClearColor(0.2f, 0.3f, 0.3f, 1.0f);

        ConfigureFigureVertexes();
    }

    protected override void OnRenderFrame(FrameEventArgs e)
    {
        base.OnRenderFrame(e);

        GL.Clear(ClearBufferMask.ColorBufferBit);

        FigureShader.Use();
        DrawFigureVertices();

        SwapBuffers();
    }

    private void ConfigureFigureVertexes()
    {
        VertexBuffer vertexBuffer = new(new[] {
                0.5f,  0.5f, 0.0f,
                0.5f, -0.5f, 0.0f,
               -0.5f, -0.5f, 0.0f,
               -0.5f,  0.5f, 0.0f
        });

        int vertexBufferObject = GL.GenBuffer();
        GL.BindBuffer(BufferTarget.ArrayBuffer, vertexBufferObject);
        GL.BufferData(
            BufferTarget.ArrayBuffer,
            vertexBuffer.Vertices.Length * sizeof(float),
            vertexBuffer.Vertices,
            BufferUsageHint.StaticDraw);

        int sizeOfVector3 = 3;
        int shaderLocation = FigureShader.GetAttribLocation("aPosition");
        SetVertexAttribPointer(shaderLocation, sizeOfVector3);

        VertexArrayObject = GL.GenVertexArray();
        GL.BindVertexArray(VertexArrayObject);
        GL.BindBuffer(BufferTarget.ArrayBuffer, vertexBufferObject);
        GL.BufferData(
            BufferTarget.ArrayBuffer, 
            vertexBuffer.Vertices.Length * sizeof(float), 
            vertexBuffer.Vertices, 
            BufferUsageHint.StaticDraw);

        // -> EBO хранит указатели indices на вершины. Это позволит складывать и отрисовывать фигуры, ссылаясь на вершины
        //    без необходимости копировать данные
        // !!!EBO необходимо создавать после создания и привязки VAO!
        uint[] indices = {
            0, 1, 3, // triangle one
            1, 2, 3  // triangle two
        };

        int elementBufferObject = GL.GenBuffer();
        GL.BindBuffer(BufferTarget.ElementArrayBuffer, elementBufferObject);
        GL.BufferData(
            BufferTarget.ElementArrayBuffer,
            size: indices.Length * sizeof(uint),
            data: indices,
            BufferUsageHint.StaticDraw);
        FigureIndicesCount = indices.Length;

        SetVertexAttribPointer(shaderLocation, sizeOfVector3);
    }

    private static void SetVertexAttribPointer(int shaderLocation, int sizeOfVector)
    {
        GL.VertexAttribPointer(
            shaderLocation,
            sizeOfVector,
            VertexAttribPointerType.Float,
            normalized: false,
            sizeOfVector * sizeof(float),
            offset: 0);
        GL.EnableVertexAttribArray(shaderLocation);
    }

    private void DrawFigureVertices()
    {
        GL.DrawElements(
            BeginMode.Triangles, 
            count: FigureIndicesCount, 
            DrawElementsType.UnsignedInt, 
            offset: 0);
    }

    #region NotFamous

    protected override void OnResize(ResizeEventArgs e)
    {
        base.OnResize(e);
        var size = new Size(Size.X, Size.Y);
        GL.Viewport(size);
    }

    #endregion
}
