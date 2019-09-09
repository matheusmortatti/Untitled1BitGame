using System;
using System.Collections.Generic;
using System.Text;

namespace SharedCode.Graphics
{
    public interface IGraphics
    {
        void Draw(GameObject gameObject);
        void Update(GameObject gameObject);
    }
}
