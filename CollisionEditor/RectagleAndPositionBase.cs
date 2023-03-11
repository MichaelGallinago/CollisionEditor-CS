using CollisionEditor.model;
using System;
using System.Windows.Media;
using System.Windows.Shapes;
namespace CollisionEditor
{
    internal class SquareAndPosition
    {
        public Rectangle Square { get; set; } = new Rectangle();
        public Vector2<int> Position { get; set; } = new Vector2<int>();

        public static bool Equal (SquareAndPosition squareAndPosition1, SquareAndPosition squareAndPosition2)
        {
            System.Windows.Forms.MessageBox.Show("ГАНДОНИЩЕ ТОТ КТО ЭТУ ХРЕНЬ СДЕЛАЛ");
            if (squareAndPosition1.Position.Equals(squareAndPosition2.Position)) 
            {
                System.Windows.Forms.MessageBox.Show("ГАНДОН");
                return true;
            }
            return false;
            //&& squareAndPosition2.Position.Equals(squareAndPosition1.Position)
        }
    }
}