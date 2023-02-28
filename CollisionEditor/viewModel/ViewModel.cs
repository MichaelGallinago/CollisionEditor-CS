using CollisionEditor.model;
using System.ComponentModel;
using System.IO;
using System.Windows;
using System.Windows.Input;

namespace CollisionEditor.viewModel
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private MainWindow window;  
        private Anglemap Anglemap { get; set; }
        private Tilemap Tilemap { get; set;}
        public ICommand TestCommand { get; set; }

        public MainViewModel(MainWindow window)
        {
            this.window = window;
            TestCommand = new RelayCommand(Test);
        }

        private void Test()
        {
            MessageBox.Show("Оно работает");
        }

        public void OpenAngleMapFile(string filePath)
        {   
            Anglemap = new Anglemap(filePath);
            byte HardCOREAngle= Anglemap.Values[35];

            string hexAngle = Convertor.GetHexAngle(HardCOREAngle);
            double angle360like = Convertor.Get360Angle(HardCOREAngle);
            int angle256like = Convertor.Get256Angle(hexAngle);
            MainWindow.ShowAnglemap(angle256like, hexAngle, angle360like);
        }
        public void AngleUpdator(Vector2<int> vectorGreen, Vector2<int> vectorBlue)
        {
            if (Anglemap is not null)
            {
                int angle256like = Anglemap.UpdateWithLine(15, vectorGreen, vectorBlue); 
                string hexAngle = Convertor.GetHexAngle((byte)angle256like);
                double angle360like = Convertor.Get360Angle((byte)angle256like);
                MainWindow.ShowAnglemap(angle256like, hexAngle, angle360like);
            }
            else
            {
                return;
            }
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
