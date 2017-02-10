﻿using System.Windows;
using System.Windows.Media;

namespace RayTwol
{
    public static partial class Editor
    {


        public static System.Windows.Shapes.Rectangle selSquare = new System.Windows.Shapes.Rectangle();
        public static SolidColorBrush brush_select = new SolidColorBrush();
        public static SolidColorBrush brush_create = new SolidColorBrush();
        public static SolidColorBrush brush_delete = new SolidColorBrush();
        public static SolidColorBrush brush_hidden = new SolidColorBrush();

        public static System.Drawing.Point mouse;

        public static bool selectingLeft;
        public static bool selectingRight;
        public static SelectModes selectMode = SelectModes.Create;
        public static System.Drawing.Point selOrigin = new System.Drawing.Point();
        public static System.Drawing.Point selRange = new System.Drawing.Point();
        public static System.Drawing.Point sel1 = new System.Drawing.Point();
        public static System.Drawing.Point sel2 = new System.Drawing.Point();



        public static void SelectingStandards()
        {
            if (selectingLeft || selectingRight)
            {
                selRange = new System.Drawing.Point(((mouse.X + 8) / 16) * 16, ((mouse.Y + 8) / 16) * 16);

                if (selRange.X < selOrigin.X)
                {
                    sel1.X = selRange.X;
                    sel2.X = selOrigin.X;
                }
                else
                {
                    sel1.X = selOrigin.X;
                    sel2.X = selRange.X;
                }

                if (selRange.Y < selOrigin.Y)
                {
                    sel1.Y = selRange.Y;
                    sel2.Y = selOrigin.Y;
                }
                else
                {
                    sel1.Y = selOrigin.Y;
                    sel2.Y = selRange.Y;
                }

                selSquare.Margin = new Thickness(sel1.X, sel1.Y, 0, 0);
                selSquare.Width = sel2.X - sel1.X;
                selSquare.Height = sel2.Y - sel1.Y;
            }
        }

        public static void SelectingLeft()
        {
            SelectingStandards();
            switch (editMode)
            {
                case EditMode.Graphics:
                    selSquare.Stroke = brush_select;
                    break;
                case EditMode.Collision:
                    selSquare.Stroke = brush_create;
                    break;
            }
        }

        public static void SelectingRight()
        {
            SelectingStandards();
            switch (editMode)
            {
                case EditMode.Graphics:
                    selSquare.Stroke = brush_create;
                    break;
                case EditMode.Collision:
                    selSquare.Stroke = brush_delete;
                    break;
            }

            if (editMode == EditMode.Graphics)
                Types_SetFromClipboardTemp(sel1.X / 16, sel1.Y / 16, sel2.X / 16, sel2.Y / 16);
        }

        public static void SetSelectionOrigin()
        {
            selOrigin.X = ((mouse.X + 8) / 16) * 16;
            selOrigin.Y = ((mouse.Y + 8) / 16) * 16;
        }

        public static void SelectionEndLeft()
        {
            Input.Changed();
            selectingLeft = false;

            switch (editMode)
            {
                case EditMode.Collision:
                    Types_SetCollision(selectedType, sel1.X / 16, sel1.Y / 16, sel2.X / 16, sel2.Y / 16);
                    selSquare.Stroke = brush_hidden;
                    break;

                case EditMode.Graphics:
                    hasSelection = true;
                    break;
            }

        }
        public static void SelectionEndRight()
        {
            Input.Changed();
            selectingRight = false;

            switch (editMode)
            {
                case EditMode.Collision:
                    Types_SetCollision(Collisions.none, sel1.X / 16, sel1.Y / 16, sel2.X / 16, sel2.Y / 16);
                    break;

                case EditMode.Graphics:
                    Types_SetFromClipboard(sel1.X / 16, sel1.Y / 16, sel2.X / 16, sel2.Y / 16);
                    break;
            }
            selSquare.Stroke = brush_hidden;
        }
    }
}
