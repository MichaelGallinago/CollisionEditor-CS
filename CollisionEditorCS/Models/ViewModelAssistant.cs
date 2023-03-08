using System;

namespace CollisionEditor.model
{
    internal static class ViewModelAssistant
    {
        public static (int byteAngle, string hexAngle, double fullAngle) GetAngles(AngleMap angleMap, int ChosenTile)
        {
            byte angle = angleMap.Values[ChosenTile];
            return (angle, Convertor.GetHexAngle(angle), Convertor.GetFullAngle(angle));
        }

        public static Vector2<int> GetCorrectDotPosition(Vector2<double> position, int cellSize)
        {
            return new Vector2<int>(
                (int)Math.Floor(position.X) & -cellSize,
                (int)Math.Floor(position.Y) & -cellSize);
        }
    }
}
