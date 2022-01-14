using GraphicEngine.V1;
using GraphicEngine.V1.Entities;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;

namespace OpenGl.SapperGame
{
    internal class GameManager
    {
        private readonly int _mapSize = -1;
        private GameObject[] _cells;

        public GameManager() {
            _mapSize = 10;
        }

        public GameManager(int mapSize) {
            _mapSize = mapSize;
        }

        public void Prepare3DGameMap()
        {
            Create3DMapCells();
        }

        public void Prepare2DGameMap()
        {
            Create2DMapCells();
        }

        private void Create2DMapCells()
        {
            var cells = new List<GameObject>();
            float step = 0.2f;

            var indices = new uint[]
            {
                0, 1, 2,
                0, 3, 2
            };

            var vertices = new float[]
            {
                0.0f, 0.0f, 0.0f,
                0.0f, step, 0.0f,
                step, step, 0.0f,
                step, 0.0f, 0.0f
            };

            for (int column = 0; column < _mapSize; column++)
            {
                for (int row = 0; row < _mapSize; row++)
                {
                    vertices[0] = step * row;
                    vertices[3] = step * row;
                    vertices[6] = step * row + step;
                    vertices[9] = step * row + step;

                    var gameCell = new Quadre().Create(vertices, indices);
                    cells.Add(gameCell);
                }

                vertices[1] += step;
                vertices[4] += step;
                vertices[7] += step;
                vertices[10] += step;
            }

            _cells = cells.ToArray();
        }

        public void DrawMap()
        {
            foreach (var cell in _cells)
            {
                cell.Draw();
            }
        }

        private void Create3DMapCells()
        {
            var cells = new List<GameObject>();

            for (int column = 1; column <= _mapSize; column++)
            {
                for (int row = 1; row <= _mapSize; row++)
                {
                    var shader = new Shader("ver.glsl", "fra.glsl", "Textured");
                    var cell3d = new Cube();
                    var model = Matrix4.CreateTranslation(new Vector3(row * 0.1f, column * 0.1f, 0.0f));
                    var view = Matrix4.CreateTranslation(0f, 0f, -10.0f);
                    var projection = Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(45.0f), 500 / 500, 0.1f, 100.0f);
                    cell3d.Shader = shader;
                    cell3d.Create().SetName($"Cell[c:{column}-r:{row}]").SetMatrixes(model, view, projection);
                    cells.Add(cell3d);
                }
            }

            _cells = cells.ToArray();
        }
    }
}
