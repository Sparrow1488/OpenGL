using System;
using System.Collections.Generic;
using System.Text;

namespace GraphicEngine.V1.Intefaces
{
    public interface IClonable<T>
    {
        public T Clone();
    }
}
