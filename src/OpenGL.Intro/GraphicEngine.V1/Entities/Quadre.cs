using System;
using System.Collections.Generic;
using System.Text;

namespace GraphicEngine.V1.Entities
{
    public class Quadre : GameObject
    {
        public override GameObject Create()
        {
            return Create(0.2f);
        }

        public GameObject Create(float size)
        {
            Vertices = new float[]
            {
                -size, -size, 0.0f,
                -size,  size, 0.0f,
                size, size, 0.0f,
                size, -size, 0.0f
            };
            Indices = new uint[]
            {
                0, 1, 2,
                0, 3, 2
            };
            base.Create();
            return this;
        }
    }
}
