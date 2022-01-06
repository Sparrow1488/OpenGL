using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Text;

namespace GraphicEngine.V1.Entities
{
    public class TestSelectableCube : GameObject
    {
        public TestSelectableCube()
        {
            _colorId = new byte[]{
                (byte)Id,
                (byte)Id,
                (byte)Id
            };
        }

        public void Select(int x, int y)
        {
            GL.Disable(EnableCap.Texture2D);
        }
    }
}
