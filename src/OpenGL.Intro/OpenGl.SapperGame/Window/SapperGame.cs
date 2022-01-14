using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;

namespace OpenGl.SapperGame.Window
{
    public class SapperGame : GameWindow
    {
        private readonly GameManager _gameManager;

        public SapperGame() : base(GameWindowSettings.Default, NativeWindowSettings.Default)
        {
            CenterWindow(new Vector2i(650));
            _gameManager = new GameManager();
            Context.SwapInterval = 2;
        }

        protected override void OnLoad()
        {
            _gameManager.Prepare2DGameMap();
            base.OnLoad();
        }

        protected override void OnRenderFrame(FrameEventArgs args)
        {
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            GL.LoadIdentity();
            GL.ClearColor(1, 1, 1, 1);

            //GL.LineWidth(3f);
            //GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Line);
            _gameManager.DrawMap();

            SwapBuffers();
            base.OnRenderFrame(args);
        }

        protected override void OnResize(ResizeEventArgs e)
        {
            GL.Viewport(0, 0, e.Width, e.Height);
            base.OnResize(e);
        }
    }
}
