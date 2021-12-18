using SharpGL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace OpenGl.Sapper
{
    public class SapperGame
    {
        private OpenGL _gl;
        private int _widthCells = 10;
        private int _heightCells = 10;

        private double _windowH = 100;
        private double _windowW = 100;

        public SapperGame(double wH, double wW)
        {
            _windowH = wH;
            _windowW = wW;
        }

        public void Initialize(OpenGL gl)
        {
            _gl = gl;
        }

        public void Initialize(OpenGL gl, int width, int height)
        {
            _gl = gl;
            _widthCells = width;
            _heightCells = height;
        }

        public void CreateGame()
        {
            
            //for (int i = 0; i < _heightCells; i++)
            //    for (int j = 0; j < _widthCells; j++)
            //        DrawCell(new[]{
            //                -(float)_windowH / 2, (float)_windowW / 2,
            //        });
        }

        public void DrawCell()
        {
            float cellX = (float)_windowH / _heightCells;
            float cellY = (float)_windowW / _widthCells;

            _gl.Clear(OpenGL.GL_COLOR_BUFFER_BIT | OpenGL.GL_DEPTH_BUFFER_BIT);
            _gl.LoadIdentity();
            _gl.Translate(0f, 0f, -40f);

            _gl.Begin(OpenGL.GL_LINES);

            for (int i = 0; i < _widthCells; i++)
            {
                _gl.Vertex(_windowW - i * cellX, _windowH / 2);
                _gl.Vertex(cellX, cellY);
            }

            _gl.End();
            _gl.Flush();
        }
    }
}
