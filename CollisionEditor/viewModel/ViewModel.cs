﻿using CollisionEditor.model;
using System.ComponentModel;
using System.IO;
using System.Windows;

namespace CollisionEditor.viewModel
{
    class MainViewModel : INotifyPropertyChanged
    {

        private Tilemap Tilemap { get; set;}

        public void OpenAngleMapFile(string filePath)
        {
            Anglemap anglemap = new Anglemap(filePath);
            byte HardCOREAngle= anglemap.Values[35];

            string hexAngle = Convertor.GetHexAngle(HardCOREAngle);
            double angle360like = Convertor.Get360Angle(HardCOREAngle);
            int angle256like = Convertor.Get256Angle(hexAngle);
            MainWindow.ShowAnglemap(angle256like, hexAngle, angle360like);
        }
        public void AngleUpdator(Vector2<int> vectorGreen, Vector2<int> vectorBlue)
        {
            UpdateWithLine();
        }

        public bool TileStripIsNull()
        {
            return Tilemap is null;
        }
        public void OpenTileStripFile(string filePath)
        {
            this.Tilemap = new Tilemap(filePath);
            MainWindow.ShowTileStrip(Convertor.BitmapConvert(Tilemap.Tiles[5]));                
        }
        public void SaveTileStrip(string filePath)
        {
            Tilemap.Save(Path.GetFullPath(filePath), 16);
        }

        public Vector2<int> GetCordinats(double x, double y)
        {
            return (AngleConstructor.GetCorrectDotPosition(new Vector2<double>(x, y)));
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


    }
}
