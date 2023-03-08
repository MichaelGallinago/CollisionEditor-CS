using CollisionEditor.model;
using System.ComponentModel;
using System.IO;
using System.Windows;
using System.Windows.Forms;
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
        public ICommand MenuSaveAngleMapCommand { get; set; }
        public ICommand SelectTileCommand { get; set; }
        public ICommand AngleIncrementCommand { get; set; }
        public ICommand AngleDecrementCommand { get; set; }
        public ICommand ExitAppCommand { get; set; }
        public int ChosenTile { get; set; }

        public MainViewModel(MainWindow window)
        {
            this.window = window;
            MenuOpenAngleMapCommand = new RelayCommand(MenuOpenAngleMap);
            MenuOpenTileMapCommand = new RelayCommand(MenuOpenTileMap);
            MenuSaveTileMapCommand = new RelayCommand(MenuSaveTileMap);
            MenuSaveAngleMapCommand = new RelayCommand(MenuSaveAngleMap);

            AngleIncrementCommand = new RelayCommand(AngleIncrement);
            AngleDecrementCommand = new RelayCommand(AngleDecrement);
            SelectTileCommand = new RelayCommand(SelectTile);

            ExitAppCommand = new RelayCommand(ExitApp);
        }

        private void MenuOpenAngleMap()
        {
            string filePath = ViewModelAngleService.GetAngleMapFilePath();
            if (filePath is not null)
            {
                window.ImageOfTile.Source = null;
                AngleMap = new AngleMap(filePath);
                (int byteAngle, string hexAngle, double fullAngle) angles = ViewModelAssistant.GetAngles(AngleMap, ChosenTile);
                ShowAngles(angles.byteAngle, angles.hexAngle, angles.fullAngle);
            }
        }
        public static void ShowAngles(int byteAngle, string hexAngle, double fullAngle)
        {
            MainWindow mainWindow = (MainWindow)System.Windows.Application.Current.MainWindow;

            mainWindow.TextBlockByteAngle.Text = byteAngle.ToString();
            mainWindow.TextBlockHexAngle.Text = hexAngle;
            mainWindow.TextBlockFullAngle.Text = fullAngle.ToString() + "'";
        }

        private void MenuOpenTileMap()
        {
            string filePath = ViewModelTileService.GetTileMapFilePath();
            this.TileSet = new TileSet(filePath);
            AngleMap = new AngleMap(TileSet.Tiles.Count);
            Convertor.BitmapConvert(TileSet.Tiles[ChosenTile]);
            ShowTile(Convertor.BitmapConvert(TileSet.Tiles[ChosenTile]));
        }

        private static void ShowTile(System.Windows.Media.Imaging.BitmapSource TileStrip)
        {
            MainWindow mainWindow = (MainWindow)System.Windows.Application.Current.MainWindow;
            mainWindow.ImageOfTile.Source = TileStrip;
        }

        private void MenuSaveTileMap()
        {
            if (TileSet is null)
            {
                System.Windows.Forms.MessageBox.Show("Error: You haven't chosen TileMap to save");
            }
            else
            {
                string filePath = ViewModelTileService.GetTileMapSavePath();
                if (filePath is not null)
                {
                    TileSet.Save(Path.GetFullPath(filePath), 16);
                }
            }
        }
        private void MenuSaveAngleMap()
        {
            if (AngleMap is null)
            {
                System.Windows.Forms.MessageBox.Show("Error: You haven't chosen AngleMap to save");
            }
            else
            {
                string filePath = ViewModelAngleService.GetAngleMapSavePath();
                if (filePath is not null)
                {
                    AngleMap.Save(Path.GetFullPath(filePath));
                }
            }
        }

        private void AngleIncrement()
        {
            byte byteAngle = AngleMap.ChangeAngle(ChosenTile, 1);

            (int byteAngle, string hexAngle, double fullAngle) angles = ViewModelAngleService.GetAngles(byteAngle);
            ShowAngles(byteAngle, angles.hexAngle, angles.fullAngle);
        }
        private void AngleDecrement()
        {
            byte byteAngle = AngleMap.ChangeAngle(ChosenTile, -1);

            (int byteAngle, string hexAngle, double fullAngle) angles = ViewModelAngleService.GetAngles(byteAngle);
            ShowAngles(byteAngle, angles.hexAngle, angles.fullAngle);
        }

        private void SelectTile()
        {
            ShowTile(Convertor.BitmapConvert(TileSet.Tiles[ChosenTile]));
        }

        private void ExitApp()
        {
            window.Close();
        }

        public void AngleUpdator(Vector2<int> vectorGreen, Vector2<int> vectorBlue)
        {
            if (AngleMap is not null)
            {
                byte byteAngle = AngleMap.SetAngleWithLine(ChosenTile, vectorGreen, vectorBlue);

                (int byteAngle, string hexAngle, double fullAngle) angles = ViewModelAngleService.GetAngles(byteAngle);
                ShowAngles(byteAngle, angles.hexAngle, angles.fullAngle);   
            }
            else
            {
                return;
            }
        }      

        public Vector2<int> GetCordinats(double x, double y)
        {
            return (ViewModelAssistant.GetCorrectDotPosition(new Vector2<double>(x, y),8));
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
