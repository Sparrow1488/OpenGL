using OpenTK.Graphics.OpenGL4;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using System.Drawing;

namespace IntroTo.GameEngine.Triangle;

public class WindowTK : GameWindow
{
    public WindowTK(
        int width,
        int height,
        string title) : base(GameWindowSettings.Default, NativeWindowSettings.Default)
    {
        Context.SwapInterval = 2; // 2-3 is normal values
        Size = (width, height); 
        Title = title;
        TriangleShader = new("./Shaders/shader.vert", "./Shaders/shader.frag");
    }

    public Shader TriangleShader { get; private set; }
    public int VertexArrayObject { get; private set; }

    protected override void OnLoad()
    {
        base.OnLoad();
        GL.ClearColor(0.2f, 0.3f, 0.3f, 1.0f);

        ConfigureTriangleVertexes();
    }

    protected override void OnRenderFrame(FrameEventArgs e)
    {
        base.OnRenderFrame(e);

        GL.Clear(ClearBufferMask.ColorBufferBit);

        TriangleShader.Use();
        DrawTriangleVertexes();

        SwapBuffers();
    }

    private void ConfigureTriangleVertexes()
    {
        VertexBuffer vertexBuffer = new(new[] {
                -0.5f,-0.5f, 0.0f,
                 0.0f, 0.5f, 0.0f,
                 0.5f,-0.5f, 0.0f
        });

        // 1. Создаем объект вершинного буфера, в котором будут храниться наши координаты точек.
        //    Привязываем и загружаем значения наших вершинных точек.
        //    Примечание: в VBO мы сохраняем вершины, а в VAO мы их передаем для обработки самим OpenGL
        int vertexBufferObject = GL.GenBuffer();
        GL.BindBuffer(BufferTarget.ArrayBuffer, vertexBufferObject);
        GL.BufferData(
            BufferTarget.ArrayBuffer,
            vertexBuffer.Vertices.Length * sizeof(float),
            vertexBuffer.Vertices,
            BufferUsageHint.StaticDraw);

        // 2. Установим указатели на вершинные буферы (для наложения шейдеров).
        //    Включаем атрибут, используя EnableVertexAttribArray, чтобы OpenGL мог интерпретировать вершинные данные
        int sizeOfVector3 = 3;
        int shaderLocation = TriangleShader.GetAttribLocation("aPosition");

        // 3. Для хранения и повторного использования вершин мы используем VAO, чтобы OpenGL смог их отрисовать
        VertexArrayObject = GL.GenVertexArray();
        GL.BindVertexArray(VertexArrayObject);

        // 4. Установим указатели на вершины
        GL.BindBuffer(BufferTarget.ArrayBuffer, vertexBufferObject);
        SetVertexAttribPointer(shaderLocation, sizeOfVector3);

        /* Все что мы делали на протяжении миллионов страниц подводило нас к этому моменту.
         * VAO, хранящее вершинные атрибуты и требуемый VBO. Зачастую, когда у нас есть множественные объекты 
         * для отрисовки мы в начале генерируем и конфигурируем VAO и сохраняем их для последующего использования.
         * И когда надо будет отрисовать один из наших объектов мы просто используем сохраненный VAO. */
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

    private void DrawTriangleVertexes()
    {
        GL.BindVertexArray(VertexArrayObject);
        GL.DrawArrays(PrimitiveType.Triangles, 0, 3);
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
