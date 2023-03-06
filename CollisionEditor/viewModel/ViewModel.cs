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
        private AngleMap AngleMap { get; set; }
        private TileSet TileSet { get; set;}
        public ICommand MenuOpenAngleMapCommand { get; set; }
        public ICommand MenuOpenTileMapCommand { get; set; }
        public ICommand MenuSaveTileMapCommand { get; set; }
        public ICommand SelectTileCommand { get; set; }
        public ICommand AngleIncrementCommand { get; set; }
        public ICommand AngleDecrementCommand { get; set; }

        public int ChosenTile { get; set; }

        public MainViewModel(MainWindow window)
        {
            this.window = window;
            MenuOpenAngleMapCommand = new RelayCommand(MenuOpenAngleMap);
            MenuOpenTileMapCommand = new RelayCommand(MenuOpenTileMap);
            MenuSaveTileMapCommand = new RelayCommand(MenuSaveTiletMap);

            AngleIncrementCommand = new RelayCommand(AngleIncrement);
            AngleDecrementCommand = new RelayCommand(AngleDecrement);
            SelectTileCommand = new RelayCommand(SelectTile);

        }

        private void MenuOpenAngleMap()
        {
            System.Windows.Forms.OpenFileDialog openFileDialog = new System.Windows.Forms.OpenFileDialog();
            string filePath = string.Empty;
            openFileDialog.Filter = "Binary Files(*.bin)| *.bin| All files(*.*) | *.*";
            if (openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                window.ImageOfTile.Source = null;
                filePath = openFileDialog.FileName;

                AngleMap = new AngleMap(filePath);
                byte HardCOREAngle = AngleMap.Values[ChosenTile];

                ConvertAndShowAngles(HardCOREAngle);
            }
        }

        private void ConvertAndShowAngles(byte HardCOREAngle)
        {
            string hexAngle = Convertor.GetHexAngle(HardCOREAngle);
            double angle360like = Convertor.Get360Angle(HardCOREAngle);
            int angle256like = Convertor.Get256Angle(hexAngle);
            ShowAnglemap(angle256like, hexAngle, angle360like);
        }


        private void MenuOpenTileMap()
        {
            System.Windows.Forms.OpenFileDialog openFileDialog = new System.Windows.Forms.OpenFileDialog();
            openFileDialog.Filter = "Image Files(*.png)| *.png| All files(*.*) | *.*";
            string filePath = string.Empty;
            if (openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                window.ImageOfTile.Source = null;
                filePath = openFileDialog.FileName;

                this.TileSet = new TileSet(filePath);
                AngleMap = new AngleMap(TileSet.Tiles.Count);
                Convertor.BitmapConvert(TileSet.Tiles[ChosenTile]);
                ShowTileStrip(Convertor.BitmapConvert(TileSet.Tiles[ChosenTile]));
            }
        }
        private static void ShowTileStrip(System.Windows.Media.Imaging.BitmapSource TileStrip)
        {
            MainWindow mainWindow = (MainWindow)System.Windows.Application.Current.MainWindow;
            mainWindow.ImageOfTile.Source = TileStrip;
        }

        private void MenuSaveTiletMap()
        {
            if (TileMapIsNull())
            {
                System.Windows.Forms.MessageBox.Show("Ошибка: Вы не выбрали Tilemap, чтобы её сохранить");
            }
            else
            {
                System.Windows.Forms.SaveFileDialog saveFileDialog = new System.Windows.Forms.SaveFileDialog();

                saveFileDialog.Filter = "Image Files(*.png)| *.png";

                //Спрашивать RowCount

                if (saveFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    SaveTileMap(saveFileDialog.FileName);
                }
            }
        }

        private void AngleIncrement()
        {
            byte HardCOREAngle = AngleMap.ChangeAngle(ChosenTile, 1);
            
            ConvertAndShowAngles(HardCOREAngle);
        }
        private void AngleDecrement()
        {
            byte HardCOREAngle = AngleMap.ChangeAngle(ChosenTile, -1);

            ConvertAndShowAngles(HardCOREAngle);
        }

        private void SelectTile()
        {
            ShowTileStrip(Convertor.BitmapConvert(TileSet.Tiles[ChosenTile]));
        }

        
        public void SaveTileMap(string filePath)
        {
            TileSet.Save(Path.GetFullPath(filePath), 16);
        }

        public static void ShowAnglemap(int angle256like, string hexAngle, double angle360like)
        {
            MainWindow mainWindow = (MainWindow)System.Windows.Application.Current.MainWindow;

            mainWindow.TextBlock256Angle.Text = angle256like.ToString();
            mainWindow.TextBlockHexAngle.Text = hexAngle;
            mainWindow.TextBlock360Angle.Text = angle360like.ToString() + "'";
        }


        public void OpenAngleMapFile(string filePath)
        {   
            AngleMap = new AngleMap (filePath);
            byte HardCOREAngle= AngleMap.Values[ChosenTile];

            ConvertAndShowAngles(HardCOREAngle);
        }
        public void AngleUpdator(Vector2<int> vectorGreen, Vector2<int> vectorBlue)
        {
            if (AngleMap is not null)
            {
                int angle256like = AngleMap.SetAngleWithLine(ChosenTile, vectorGreen, vectorBlue); 
                string hexAngle = Convertor.GetHexAngle((byte)angle256like);
                double angle360like = Convertor.Get360Angle((byte)angle256like);
                ShowAnglemap(angle256like, hexAngle, angle360like);
            }
            else
            {
                return;
            }
        }

        public bool TileMapIsNull()
        {
            return TileSet is null;
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
