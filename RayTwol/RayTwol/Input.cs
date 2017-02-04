using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RayTwol
{
    public static class Input
    {
        public static bool A, B, C, D, E, F, G, H, I, J, K, L, M, N, O, P, Q, R, S, T, U, V, W, X, Y, Z,
            CTRL, SHIFT, ALT, SPACE, TAB;

        public static void Changed()
        {
            if (Z)
                Editor.selectMode = selectModes.Delete;
            else
                Editor.selectMode = selectModes.Create;

            if (Editor.selecting)
                switch (Editor.selectMode)
                {
                    case selectModes.Create:
                        Editor.selSquare.Stroke = Editor.brush_create;
                        break;
                    case selectModes.Delete:
                        Editor.selSquare.Stroke = Editor.brush_delete;
                        break;
                }
        }
    }
}
