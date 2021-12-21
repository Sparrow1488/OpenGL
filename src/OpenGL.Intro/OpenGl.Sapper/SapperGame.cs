using SharpGL;

namespace OpenGl.Sapper
{
    public class SapperGame
    {
        private OpenGL _gl;
        private float _cellSize = 5f;
        private int _gameSize = 10;

        private readonly float _winH = 100;
        private readonly float _winW = 100;

        public SapperGame(float winH, float winW)
        {
            _winH = winH;
            _winW = winW;
        }

        public void Initialize(OpenGL gl)
        {
            _gl = gl;
        }

        public void CreateGame(OpenGL gl, float cellSize, int gameSize)
        {
            _gl = gl;
            _cellSize = cellSize;
            _gameSize = gameSize;
        }

        public void DrawGameField()
        {
            _gl.Clear(OpenGL.GL_COLOR_BUFFER_BIT | OpenGL.GL_DEPTH_BUFFER_BIT);
            _gl.LoadIdentity();
            _gl.Scale(_cellSize / _gameSize, _cellSize / _gameSize, 1);
            var f = -(_winW / 2 * _cellSize);
            //_gl.Translate(-(10 / 2 * _cellSize), -(10 / 2 * _cellSize), -1f);
            _gl.Translate(0, 0, -1f);

            _gl.Begin(OpenGL.GL_LINES);

            for (int column = 0; column < _gameSize; column++)
                for (int row = 0; row < _gameSize; row++)
                    DrawCell(row, column);

            _gl.End();
            _gl.Flush();
        }

        public void DrawCell(int row, int column)
        {
            float baseX = row * _cellSize;
            float baseY = column * _cellSize;
            _gl.Vertex(baseX, baseY);
            _gl.Vertex(baseX, baseY + _cellSize);

            _gl.Vertex(baseX, baseY + _cellSize);
            _gl.Vertex(baseX + _cellSize, baseY + _cellSize);

            _gl.Vertex(baseX + _cellSize, baseY + _cellSize);
            _gl.Vertex(baseX + _cellSize, baseY);

            _gl.Vertex(baseX + _cellSize, baseY);
            _gl.Vertex(baseX, baseY);
        }
    }
}
