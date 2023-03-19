using CollisionEditor.model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Drawing;

namespace CollisionEditor.viewModel
{
    public class MainViewModel : INotifyPropertyChanged, INotifyDataErrorInfo
    {
        private MainWindow window;
        private AngleMap angleMap;
        private TileSet tileSet;
        public ICommand MenuOpenAngleMapCommand { get; set; }
        public ICommand MenuOpenTileMapCommand { get; set; }
        public ICommand MenuSaveTileMapCommand { get; set; }
        public ICommand MenuSaveWidthMapCommand { get; set; }
        public ICommand MenuSaveHeightMapCommand { get; set; }
        public ICommand MenuSaveAngleMapCommand { get; set; }
        public ICommand MenuSaveAllCommand { get; set; }
        public ICommand MenuUnloadTileMapCommand { get; set; }
        public ICommand MenuUnloadAngleMapCommand { get; set; }
        public ICommand MenuUnloadAllCommand { get; set; }
        public ICommand SelectTileCommand { get; set; }
        public ICommand AngleIncrementCommand { get; set; }
        public ICommand AngleDecrementCommand { get; set; }
        public ICommand ExitAppCommand { get; set; }

        private byte _byteAngle;
        public byte ByteAngle
        {
            get => _byteAngle;
            set
            {   
                _byteAngle = value;
                (byte byteAngle, string hexAngle, double fullAngle) angles = ViewModelAngleService.GetAngles(_byteAngle);
                ShowAngles(angles.byteAngle, angles.hexAngle, angles.fullAngle);

                window.DrawRedLine();
            }
        }

        private const string hexademicalAlplhabet = "0123456789ABCDEF";
        private string _hexAngle;
        public string HexAngle
        {
            get => _hexAngle;
            set
            {
                _hexAngle = value;

                if (_hexAngle.Length != 4 || _hexAngle[0] != '0' || _hexAngle[1] != 'x'
                    || !hexademicalAlplhabet.Contains(_hexAngle[2]) || !hexademicalAlplhabet.Contains(_hexAngle[3]))
                {
                    AddError(nameof(HexAngle), "Error! Wrong hexadecimal number");
                    return;
                }

                ClearErrors(nameof(HexAngle));

                (byte byteAngle, string hexAngle, double fullAngle) angles = ViewModelAngleService.GetAngles(_hexAngle);
                ByteAngle = angles.byteAngle;
                angleMap.SetAngle((int)ChosenTile, angles.byteAngle);
                window.DrawRedLine();
            }
        }
        private uint _chosenTile;
        public uint ChosenTile
        {
            get => _chosenTile;
            set
            {
                _chosenTile = value;
            }
        }

        public MainViewModel(MainWindow window)
        {
            angleMap = new AngleMap(0);
            tileSet  = new TileSet(0);
            _chosenTile = 0;
            _byteAngle = 0;
            _hexAngle = "0x00";
            this.window = window;
            MenuOpenAngleMapCommand = new RelayCommand(MenuOpenAngleMap);
            MenuOpenTileMapCommand = new RelayCommand(MenuOpenTileMap);
            MenuSaveTileMapCommand = new RelayCommand(MenuSaveTileMap);
            MenuSaveWidthMapCommand = new RelayCommand(MenuSaveWidthMap);
            MenuSaveHeightMapCommand = new RelayCommand(MenuSaveHeightMap);
            MenuSaveAngleMapCommand = new RelayCommand(MenuSaveAngleMap);
            MenuSaveAllCommand = new RelayCommand(MenuSaveAll);
            MenuUnloadTileMapCommand = new RelayCommand(MenuUnloadTileMap);
            MenuUnloadAngleMapCommand = new RelayCommand(MenuUnloadAngleMap);
            MenuUnloadAllCommand = new RelayCommand(MenuUnloadAll);

            AngleIncrementCommand = new RelayCommand(AngleIncrement);
            AngleDecrementCommand = new RelayCommand(AngleDecrement);
            SelectTileCommand = new RelayCommand(SelectTile);

            ExitAppCommand = new RelayCommand(ExitApp);

            RectanglesGridUpdate();
            TileGridUpdate(tileSet, (int)_chosenTile, window);
        }

        private void MenuOpenAngleMap()
        {
            string filePath = ViewModelAngleService.GetAngleMapFilePath();
            if (filePath is not null && filePath != string.Empty)
            {
                angleMap = new AngleMap(filePath);
                tileSet = new TileSet(angleMap.Values.Count);
                (byte byteAngle, string hexAngle, double fullAngle) angles = ViewModelAssistant.GetAngles(angleMap, _chosenTile);
                ShowAngles(angles.byteAngle, angles.hexAngle, angles.fullAngle);
                window.SelectTileTextBox.IsEnabled = true;
                window.SelectTileButton.IsEnabled = true;
                window.TextBoxByteAngle.IsEnabled = true;
                window.TextBoxHexAngle.IsEnabled = true;
            }
        }
        public void ShowAngles(byte byteAngle, string hexAngle, double fullAngle)
        {
            MainWindow mainWindow = (MainWindow)System.Windows.Application.Current.MainWindow;

            _byteAngle = byteAngle;
            OnPropertyChanged(nameof(ByteAngle));
            mainWindow.ByteAngleIncrimentButton.IsEnabled= true;
            mainWindow.ByteAngleDecrementButton.IsEnabled = true;

            _hexAngle = hexAngle;
            OnPropertyChanged(nameof(HexAngle));
            mainWindow.HexAngleIncrimentButton.IsEnabled = true;
            mainWindow.HexAngleDecrementButton.IsEnabled = true;

            mainWindow.TextBlockFullAngle.Text = fullAngle.ToString() + "°";
        }

        private void MenuOpenTileMap()
        {
            string filePath = ViewModelTileService.GetTileMapFilePath();
            if (filePath is not null && filePath != string.Empty)
            {
                tileSet = new TileSet(filePath);
                angleMap = new AngleMap(tileSet.Tiles.Count);
                ViewModelAssistant.BitmapConvert(tileSet.Tiles[(int)_chosenTile]);
                TileGridUpdate(tileSet, (int)ChosenTile, window);
                RectanglesGridUpdate();
                window.Heights.Text = ViewModelAssistant.GetCollisionValues(tileSet.HeightMap[(int)_chosenTile]);
                window.Widths.Text = ViewModelAssistant.GetCollisionValues(tileSet.WidthMap[(int)_chosenTile]);

                (byte byteAngle, string hexAngle, double fullAngle) angles = ViewModelAssistant.GetAngles(angleMap, _chosenTile);
                ShowAngles(angles.byteAngle, angles.hexAngle, angles.fullAngle);
                window.TextBoxByteAngle.IsEnabled = true;
                window.TextBoxHexAngle.IsEnabled = true;

                System.Drawing.Size size = new System.Drawing.Size(4, 4);
                int imageOfTileMapWidth = (int)((window.Width/587) * 278);

                window.TileMapGrid.Height = (int)Math.Ceiling(tileSet.Tiles.Count / 8d) * (tileSet.TileSize.Height * 2 + 4) - 4;
                
                foreach (Bitmap tile in tileSet.Tiles)
                {
                    var image = new System.Windows.Controls.Image()
                    {
                        Width = tileSet.TileSize.Width * 2,
                        Height = tileSet.TileSize.Height * 2
                    };
                    image.Source = ViewModelAssistant.BitmapConvert(tile);
                    window.TileMapGrid.Children.Add(image);
                }

                ShowTileMap(ViewModelAssistant.GetTilePanel(tileSet, imageOfTileMapWidth, size));
                
                window.SelectTileTextBox.IsEnabled = true;
                window.SelectTileButton.IsEnabled = true;
                
            }
        }

        private static void ShowTileMap(System.Windows.Media.Imaging.BitmapSource TileMap)
        {
            MainWindow mainWindow = (MainWindow)System.Windows.Application.Current.MainWindow;
            //mainWindow.ImageOfTileMap.Source = TileMap;
        }

        private void MenuSaveTileMap()
        {
            if (tileSet.Tiles.Count==0)
            {
                System.Windows.Forms.MessageBox.Show("Error: You haven't chosen TileMap to save");
                return;
            }

            string filePath = ViewModelTileService.GetTileMapSavePath();
            if (filePath is not null && filePath != string.Empty)
            {
                tileSet.Save(Path.GetFullPath(filePath), 16);
            }
        }

        private void MenuSaveWidthMap()
        {
            if (tileSet.Tiles.Count == 0)
            {
                System.Windows.Forms.MessageBox.Show("Error: The WidthMap isn't generated!");
                return;
            }

            string filePath = ViewModelTileService.GetWidthMapSavePath();
            if (filePath is not null && filePath != string.Empty)
            {
                tileSet.SaveCollisionMap(Path.GetFullPath(filePath), tileSet.WidthMap);
            }
        }

        private void MenuSaveHeightMap()
        {
            if (tileSet.Tiles.Count == 0)
            {
                System.Windows.Forms.MessageBox.Show("Error: The HeightMap isn't generated!");
                return;
            }

            string filePath = ViewModelTileService.GetWidthMapSavePath();
            if (filePath is not null && filePath != string.Empty)
            {
                tileSet.SaveCollisionMap(Path.GetFullPath(filePath), tileSet.HeightMap);
            }
        }

        private void MenuSaveAngleMap()
        {
            if (angleMap.Values.Count==0)
            {
                System.Windows.Forms.MessageBox.Show("Error: You haven't chosen AngleMap to save");
                return;
            }

            string filePath = ViewModelAngleService.GetAngleMapSavePath();
            if (filePath is not null && filePath != string.Empty)
            {
                angleMap.Save(Path.GetFullPath(filePath));
            }
        }

        private void MenuSaveAll()
        {
            MenuSaveAngleMap();
            MenuSaveHeightMap();
            MenuSaveWidthMap();
            MenuSaveTileMap();
        }

        private void MenuUnloadTileMap()
        {
            tileSet = new TileSet(angleMap.Values.Count);
        }

        private void MenuUnloadAngleMap()
        {
            angleMap = new AngleMap(tileSet.Tiles.Count);
        }

        private void MenuUnloadAll()
        {
            tileSet = new TileSet(0);
            angleMap = new AngleMap(0);
        }

        private void AngleIncrement()
        {
            byte byteAngle = angleMap.ChangeAngle((int)_chosenTile, 1);

            (byte byteAngle, string hexAngle, double fullAngle) angles = ViewModelAngleService.GetAngles(byteAngle);
            ShowAngles(byteAngle, angles.hexAngle, angles.fullAngle);

            window.DrawRedLine();
        }

        private void AngleDecrement()
        {
            byte byteAngle = angleMap.ChangeAngle((int)_chosenTile, -1);

            (byte byteAngle, string hexAngle, double fullAngle) angles = ViewModelAngleService.GetAngles(byteAngle);
            ShowAngles(byteAngle, angles.hexAngle, angles.fullAngle);

            window.DrawRedLine();
        }

        public void SelectTile()
        {   
            if (_chosenTile > tileSet.Tiles.Count - 1)
            {
                _chosenTile = (uint)tileSet.Tiles.Count - 1;
                OnPropertyChanged(nameof(ChosenTile));
            }

            TileGridUpdate(tileSet, (int)ChosenTile, window);
            window.Heights.Text = ViewModelAssistant.GetCollisionValues(tileSet.HeightMap[(int)_chosenTile]);
            window.Widths.Text  = ViewModelAssistant.GetCollisionValues(tileSet.WidthMap[(int)_chosenTile]);
            
            (byte byteAngle, string hexAngle, double fullAngle) angles = ViewModelAssistant.GetAngles(angleMap, _chosenTile);
            ShowAngles(angles.byteAngle, angles.hexAngle, angles.fullAngle);
        }

        private void ExitApp()
        {
            window.Close();
        }

        public void AngleUpdator(Vector2<int> vectorGreen, Vector2<int> vectorBlue)
        {
            if (angleMap.Values.Count == 0)
                return;

            byte byteAngle = angleMap.SetAngleWithLine((int)_chosenTile, vectorGreen, vectorBlue);

            (int byteAngle, string hexAngle, double fullAngle) angles = ViewModelAngleService.GetAngles(byteAngle);
            ShowAngles(byteAngle, angles.hexAngle, angles.fullAngle);
        }      

        public Vector2<int> GetCordinats(double x, double y)
        {
            return (ViewModelAssistant.GetCorrectDotPosition(new Vector2<double>(x, y),8));
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public event EventHandler<DataErrorsChangedEventArgs>? ErrorsChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public bool HasErrors => _propertyErrors.Any();

        private readonly Dictionary<string, List<string>> _propertyErrors = new Dictionary<string, List<string>>();

        public IEnumerable GetErrors(string? propertyName)
        {
            return _propertyErrors.GetValueOrDefault(propertyName, null);
        }

        public void AddError(string propertyName, string errorMessage)
        {
            if (!_propertyErrors.ContainsKey(propertyName))
                _propertyErrors.Add(propertyName, new List<string>());

            _propertyErrors[propertyName].Add(errorMessage);
            OnErrorsChanged(propertyName);
        }

        private void OnErrorsChanged(string propertyName)
        {
            ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
        }

        public void ClearErrors(string propertyName)
        {
            if (_propertyErrors.Remove(propertyName))
                OnErrorsChanged(propertyName);
        }

        private void RectanglesGridUpdate()
        {
            window.RectanglesGrid.ColumnDefinitions.Clear();
            window.RectanglesGrid.RowDefinitions.Clear();

            var size = tileSet.TileSize;

            for (int x = 0; x < size.Width; x++)
                window.RectanglesGrid.ColumnDefinitions.Add(new ColumnDefinition());

            for (int y = 0; y < size.Height; y++)
                window.RectanglesGrid.RowDefinitions.Add(new RowDefinition());
        }

        private static void TileGridUpdate(TileSet tileSet, int ChosenTile, MainWindow window)
        {
            window.TileGrid.Children.Clear();

            var size = tileSet.TileSize;

            window.TileGrid.Rows    = size.Height;
            window.TileGrid.Columns = size.Width;
            window.TileGrid.Background = new SolidColorBrush(Colors.Transparent);

            Bitmap tile = tileSet.Tiles.Count > 0 ? tileSet.Tiles[ChosenTile] : new Bitmap(size.Height, size.Width);

            for (int y = 0; y < size.Height; y++)
            {
                for (int x = 0; x < size.Width; x++)
                {
                    Border Border = new Border()
                    {
                        BorderThickness = new Thickness(x == 0 ? 1d : 0d, y == 0 ? 1d : 0d, 1d, 1d),
                        Background = new SolidColorBrush(tile.GetPixel(x, y).A > 0 ? Colors.Black : Colors.Transparent),
                        BorderBrush = new SolidColorBrush(Colors.Gray),
                    };

                    window.TileGrid.Children.Add(Border);
                }
            }
        }

    }
}
