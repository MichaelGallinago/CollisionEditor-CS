﻿using CollisionEditor.model;
using Avalonia.Controls.Shapes;
namespace CollisionEditor
{
    internal class SquareAndPosition
    {
        public Rectangle Square { get; set; } = new Rectangle();
        public Vector2<int> Position { get; set; } = new Vector2<int>();
    }
}