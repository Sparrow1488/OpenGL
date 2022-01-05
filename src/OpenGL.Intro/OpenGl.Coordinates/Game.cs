using GraphicEngine.V1;
using GraphicEngine.V1.Entities;
using OpenGl.Coordinates.Tools;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;

internal class Game : GameWindow
{
    private readonly List<int> _cells = new();
    private readonly List<GameObject> _gameObjects = new();
    private readonly Engine _engine = new();
    private readonly Logger _logger = new();

    private Shader _coordShader;

    public Game() : base(GameWindowSettings.Default, NativeWindowSettings.Default) { 
        CenterWindow(new Vector2i(500));
    }

    protected override void OnLoad()
    {
        _coordShader = new Shader("vertex1.glsl", "fragment1.glsl", "Coordinates").Create();
        var texture = new Texture("awesomeface.png").Create();
        var quadre = new Quadre().CreateTextured(0.25f)
                                 .Use(_coordShader)
                                 .Add(texture)
                                 .SetName("Testing_game_object");
        OnSelectedInit(quadre);

        _gameObjects.Add(quadre);

        base.OnLoad();
    }

    protected override void OnRenderFrame(FrameEventArgs args)
    {
        GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
        GL.LoadIdentity();
        GL.ClearColor(new Color4(53, 95, 115, 1));

        var model = Matrix4.CreateRotationX(MathHelper.DegreesToRadians(-55.0f));
        var view = Matrix4.CreateTranslation(0.0f, 0.0f, -3.0f);
        Matrix4 projection = Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(45.0f), 500 / 500, 0.1f, 100.0f); // вид в проекции
        _coordShader.SetMatrix4("model", model)
                    .SetMatrix4("view", view)
                    .SetMatrix4("projection", projection);

        foreach (var gameObject in _gameObjects)
        {
            gameObject.Draw();
        }

        SwapBuffers();
        base.OnRenderFrame(args);
    }

    protected override void OnMouseDown(MouseButtonEventArgs e)
    {
        base.OnMouseDown(e);
        int width = 1;
        int height = 1;
        unsafe
        {
            GLFW.GetWindowSize(WindowPtr, out width, out height);
        }
        float mouseX = (float)(-1.0 + 2.0 * MousePosition.X / width);
        float mouseY = (float)(1.0 - 2.0 * MousePosition.Y / height);
        _logger.Log($"Mouse Down → ({mouseX}; {mouseY})");

        foreach (var obj in _gameObjects)
        {
            if (obj.IsSelected(mouseX, mouseY))
            {
                _logger.Success(obj.Name + " selected");
            }
        }
    }

    protected override void OnResize(ResizeEventArgs e)
    {
        GL.Viewport(0, 0, e.Width, e.Height);
        base.OnResize(e);
    }

    private void OnSelectedInit(GameObject obj)
    {
        obj.OnSelected += () =>
        {
            var rnd = new Random();
            var r = (float)rnd.NextDouble();
            var g = (float)rnd.NextDouble();
            var b = (float)rnd.NextDouble();
            obj.ChangeShaderColor(new Color4(r, g, b, 1f), "UniColor");
        };
    }
}