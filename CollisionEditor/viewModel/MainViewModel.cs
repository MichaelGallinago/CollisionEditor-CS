using CollisionEditor.Model;
using System.Collections.Generic;
using System.Collections;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.IO;
using System;

namespace CollisionEditor.ViewModel
{
    public class MainViewModel : INotifyPropertyChanged, INotifyDataErrorInfo
    {
        public AngleMap AngleMap { get; private set; }
        public TileSet TileSet { get; private set; }
        
        public ICommand MenuOpenAngleMapCommand { get; }
        public ICommand MenuOpenTileMapCommand { get; }
        public ICommand MenuSaveTileMapCommand { get; }
        public ICommand MenuSaveWidthMapCommand { get; }
        public ICommand MenuSaveHeightMapCommand { get; }
        public ICommand MenuSaveAngleMapCommand { get; }
        public ICommand MenuSaveAllCommand { get; }
        public ICommand MenuUnloadTileMapCommand { get; }
        public ICommand MenuUnloadAngleMapCommand { get; }
        public ICommand MenuUnloadAllCommand { get; }
        public ICommand SelectTileCommand { get; }
        public ICommand AngleIncrementCommand { get; }
        public ICommand AngleDecrementCommand { get; }
        public ICommand ExitAppCommand { get; }

        public byte ByteAngle
        {
            get => _byteAngle;
            set
            {   
                _byteAngle = value;
                (byte byteAngle, string hexAngle, double fullAngle) angles = ViewModelAngleService.GetAngles(_byteAngle);
                ShowAngles(angles.byteAngle, angles.hexAngle, angles.fullAngle);

                _window.DrawRedLine();
            }
        }

        public string HexAngle
        {
            get => _hexAngle;
            set
            {
                _hexAngle = value;

                if (_hexAngle.Length != 4 || _hexAngle[0] != '0' || _hexAngle[1] != 'x'
                    || !_hexadecimalAlphabet.Contains(_hexAngle[2]) || !_hexadecimalAlphabet.Contains(_hexAngle[3]))
                {
                    AddError(nameof(HexAngle), "Error! Wrong hexadecimal number");
                    return;
                }

                ClearErrors(nameof(HexAngle));

                (byte byteAngle, string hexAngle, double fullAngle) angles = ViewModelAngleService.GetAngles(_hexAngle);
                ByteAngle = angles.byteAngle;
                AngleMap.SetAngle((int)ChosenTile, angles.byteAngle);
                _window.DrawRedLine();
            }
        }

        public uint ChosenTile
        {
            get => _chosenTile;
            set
            {   
                _chosenTile = value;
            }
        }


        private const string _hexadecimalAlphabet = "0123456789ABCDEF";

        private MainWindow _window;
        private byte _byteAngle;
        private string _hexAngle;
        private uint _chosenTile;

        public MainViewModel(MainWindow window)
        {
            AngleMap = new AngleMap(0);
            TileSet  = new TileSet(0);
            _chosenTile = 0;
            _byteAngle = 0;
            _hexAngle = "0x00";
            _window = window;
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
            TileGridUpdate(TileSet, (int)_chosenTile, window);
        }

        private void MenuOpenAngleMap()
        {
            string filePath = ViewModelFileService.GetFileOpenPath(ViewModelFileService.Filters.AngleMap);
            if (filePath != string.Empty)
            {   
                AngleMap = new AngleMap(filePath);
                if (TileSet is null)
                {
                    TileSet = new TileSet(AngleMap.Values.Count);
                }
                
                ViewModelAssistant.SupplementElements(AngleMap, TileSet);

                (byte byteAngle, string hexAngle, double fullAngle) angles = ViewModelAssistant.GetAngles(AngleMap, _chosenTile);
                ShowAngles(angles.byteAngle, angles.hexAngle, angles.fullAngle);
                _window.SelectTileTextBox.IsEnabled = true;
                _window.SelectTileButton.IsEnabled = true;
                _window.TextBoxByteAngle.IsEnabled = true;
                _window.TextBoxHexAngle.IsEnabled = true;

                TileMapGridUpdate(TileSet.Tiles.Count);
                _window.DrawRedLine();
            }
        }

        public void ShowAngles(byte byteAngle, string hexAngle, double fullAngle)
        {
            _byteAngle = byteAngle;
            OnPropertyChanged(nameof(ByteAngle));
            _window.ByteAngleIncrimentButton.IsEnabled = true;
            _window.ByteAngleDecrementButton.IsEnabled = true;

            _hexAngle = hexAngle;
            OnPropertyChanged(nameof(HexAngle));
            _window.HexAngleIncrimentButton.IsEnabled = true;
            _window.HexAngleDecrementButton.IsEnabled = true;

            _window.TextBlockFullAngle.Text = fullAngle.ToString() + "°";
        }

        private void MenuOpenTileMap()
        {
            string filePath = ViewModelFileService.GetFileOpenPath(ViewModelFileService.Filters.TileMap);
            if (filePath != string.Empty)
            {
                TileSet = new TileSet(filePath);
                if (AngleMap is null)
                {
                    AngleMap = new AngleMap(TileSet.Tiles.Count);
                }

                ViewModelAssistant.SupplementElements(AngleMap,TileSet);

                ViewModelAssistant.BitmapConvert(TileSet.Tiles[(int)_chosenTile]);
                TileGridUpdate(TileSet, (int)ChosenTile, _window);
                RectanglesGridUpdate();
                _window.Heights.Text = ViewModelAssistant.GetCollisionValues(TileSet.HeightMap[(int)_chosenTile]);
                _window.Widths.Text = ViewModelAssistant.GetCollisionValues(TileSet.WidthMap[(int)_chosenTile]);

                (byte byteAngle, string hexAngle, double fullAngle) angles = ViewModelAssistant.GetAngles(AngleMap, _chosenTile);
                ShowAngles(angles.byteAngle, angles.hexAngle, angles.fullAngle);
                _window.TextBoxByteAngle.IsEnabled = true;
                _window.TextBoxHexAngle.IsEnabled = true;

                _window.TileMapGrid.Children.Clear();

                foreach (Bitmap tile in TileSet.Tiles)
                {
                    var image = new System.Windows.Controls.Image()
                    {
                        Width = TileSet.TileSize.Width * 2,
                        Height = TileSet.TileSize.Height * 2
                    };
                    image.Source = ViewModelAssistant.BitmapConvert(tile);
                    _window.TileMapGrid.Children.Add(image);
                }

                _window.SelectTileTextBox.IsEnabled = true;
                _window.SelectTileButton.IsEnabled = true;

                TileMapGridUpdate(TileSet.Tiles.Count);
                _window.DrawRedLine();
            }
        }

        public void TileMapGridUpdate(int tileCount)
        {
            _window.TileMapGrid.Height = (int)Math.Ceiling((double)tileCount / _window.TileMapGrid.Columns) * (16 * 2 + 4);
        }

        private void MenuSaveTileMap()
        {
            if (TileSet.Tiles.Count == 0)
            {
                System.Windows.Forms.MessageBox.Show("Error: You haven't chosen TileMap to save");
                return;
            }

            string filePath = ViewModelFileService.GetFileSavePath(ViewModelFileService.Filters.TileMap);
            if (filePath != string.Empty)
            {
                TileSet.Save(Path.GetFullPath(filePath), 16);
            }
        }

        private void MenuSaveWidthMap()
        {
            if (TileSet.Tiles.Count == 0)
            {
                System.Windows.Forms.MessageBox.Show("Error: The WidthMap isn't generated!");
                return;
            }

            string filePath = ViewModelFileService.GetFileSavePath(ViewModelFileService.Filters.WidthMap);
            if (filePath != string.Empty)
            {
                TileSet.SaveCollisionMap(Path.GetFullPath(filePath), TileSet.WidthMap);
            }
        }

        private void MenuSaveHeightMap()
        {
            if (TileSet.Tiles.Count == 0)
            {
                System.Windows.Forms.MessageBox.Show("Error: The HeightMap isn't generated!");
                return;
            }

            string filePath = ViewModelFileService.GetFileSavePath(ViewModelFileService.Filters.HeightMap);
            if (filePath != string.Empty)
            {
                TileSet.SaveCollisionMap(Path.GetFullPath(filePath), TileSet.HeightMap);
            }
        }

        private void MenuSaveAngleMap()
        {
            if (AngleMap.Values.Count == 0)
            {
                System.Windows.Forms.MessageBox.Show("Error: You haven't chosen AngleMap to save");
                return;
            }

            string filePath = ViewModelFileService.GetFileSavePath(ViewModelFileService.Filters.AngleMap);
            if (filePath != string.Empty)
            {
                AngleMap.Save(Path.GetFullPath(filePath));
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
            _window.TileMapGrid.Children.Clear();
            TileSet = new TileSet(AngleMap.Values.Count);

            TileGridUpdate(TileSet, (int)ChosenTile, _window);
            _window.Heights.Text = ViewModelAssistant.GetCollisionValues(TileSet.HeightMap[(int)_chosenTile]);
            _window.Widths.Text = ViewModelAssistant.GetCollisionValues(TileSet.WidthMap[(int)_chosenTile]);

            (byte byteAngle, string hexAngle, double fullAngle) angles = ViewModelAssistant.GetAngles(AngleMap, _chosenTile);
            ShowAngles(angles.byteAngle, angles.hexAngle, angles.fullAngle);
        }

        private void MenuUnloadAngleMap()
        {
            AngleMap = new AngleMap(TileSet.Tiles.Count);

            (byte byteAngle, string hexAngle, double fullAngle) angles = ViewModelAssistant.GetAngles(AngleMap, _chosenTile);
            ShowAngles(angles.byteAngle, angles.hexAngle, angles.fullAngle);
        }

        private void MenuUnloadAll()
        {
            _window.TileMapGrid.Children.Clear();
            TileSet = new TileSet(0);
            AngleMap = new AngleMap(0);

            _window.Heights.Text = null;
            _window.Widths.Text = null;
            ShowAngles(0, "0x00", 0);
            _window.SelectTileTextBox.Text = "0";
            _window.ByteAngleIncrimentButton.IsEnabled = false;
            _window.ByteAngleDecrementButton.IsEnabled = false;
            _window.HexAngleIncrimentButton.IsEnabled = false;
            _window.HexAngleDecrementButton.IsEnabled = false;
            _window.SelectTileTextBox.IsEnabled = false;
            _window.SelectTileButton.IsEnabled = false;
            _window.TextBoxByteAngle.IsEnabled = false;
            _window.TextBoxHexAngle.IsEnabled = false;
            _window.canvasForLine.Children.Clear();
            _window.RectanglesGrid.Children.Clear();
        }

        private void AngleIncrement()
        {
            byte byteAngle = AngleMap.ChangeAngle((int)_chosenTile, 1);

            (byte byteAngle, string hexAngle, double fullAngle) angles = ViewModelAngleService.GetAngles(byteAngle);
            ShowAngles(byteAngle, angles.hexAngle, angles.fullAngle);

            _window.DrawRedLine();
        }

        private void AngleDecrement()
        {
            byte byteAngle = AngleMap.ChangeAngle((int)_chosenTile, -1);

            (byte byteAngle, string hexAngle, double fullAngle) angles = ViewModelAngleService.GetAngles(byteAngle);
            ShowAngles(byteAngle, angles.hexAngle, angles.fullAngle);

            _window.DrawRedLine();
        }

        public void SelectTile()
        {
            if (_chosenTile > TileSet.Tiles.Count - 1)
            {
                _chosenTile = (uint)TileSet.Tiles.Count - 1;
                OnPropertyChanged(nameof(ChosenTile));
            }

            System.Windows.Controls.Image lastTile = GetTile(_window.LastChozenTile);

            _window.TileMapGrid.Children.RemoveAt(_window.LastChozenTile);
            _window.TileMapGrid.Children.Insert(_window.LastChozenTile, lastTile);

            System.Windows.Controls.Image newTile = GetTile((int)_chosenTile);

            Border border = new Border();
            border.BorderBrush = new SolidColorBrush(Colors.Red);
            border.BorderThickness = new Thickness(2);
            border.Width = 36;
            border.Height = 36;
            border.Child = newTile;

            _window.TileMapGrid.Children.RemoveAt((int)_chosenTile);
            _window.TileMapGrid.Children.Insert((int)_chosenTile, border);

            _window.LastChozenTile = (int)_chosenTile;
            TileGridUpdate(TileSet, (int)ChosenTile, _window);
            _window.Heights.Text = ViewModelAssistant.GetCollisionValues(TileSet.HeightMap[(int)_chosenTile]);
            _window.Widths.Text  = ViewModelAssistant.GetCollisionValues(TileSet.WidthMap[(int)_chosenTile]);
            
            (byte byteAngle, string hexAngle, double fullAngle) angles = ViewModelAssistant.GetAngles(AngleMap, _chosenTile);
            ShowAngles(angles.byteAngle, angles.hexAngle, angles.fullAngle);

            _window.DrawRedLine();
            _window.RectanglesGrid.Children.Clear();
        }

        public void SelectTileFromTileMap()
        {
            OnPropertyChanged(nameof(ChosenTile));
            TileGridUpdate(TileSet, (int)ChosenTile, _window);
            _window.Heights.Text = ViewModelAssistant.GetCollisionValues(TileSet.HeightMap[(int)_chosenTile]);
            _window.Widths.Text = ViewModelAssistant.GetCollisionValues(TileSet.WidthMap[(int)_chosenTile]);

            (byte byteAngle, string hexAngle, double fullAngle) angles = ViewModelAssistant.GetAngles(AngleMap, _chosenTile);
            ShowAngles(angles.byteAngle, angles.hexAngle, angles.fullAngle);

            _window.DrawRedLine();
            _window.RectanglesGrid.Children.Clear();
        }

        internal System.Windows.Controls.Image GetTile(int index)
        {
            Bitmap tile = TileSet.Tiles[index];
            var image = new System.Windows.Controls.Image()
            {
                Width = TileSet.TileSize.Width * 2,
                Height = TileSet.TileSize.Height * 2
            };
            image.Source = ViewModelAssistant.BitmapConvert(tile);
            return image;
        }

        private void ExitApp()
        {
            _window.Close();
        }

        public void AngleUpdator(Vector2<int> vectorGreen, Vector2<int> vectorBlue)
        {
            if (AngleMap.Values.Count == 0)
                return;

            byte byteAngle = AngleMap.SetAngleWithLine((int)_chosenTile, vectorGreen, vectorBlue);

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
            _window.RectanglesGrid.ColumnDefinitions.Clear();
            _window.RectanglesGrid.RowDefinitions.Clear();

            var size = TileSet.TileSize;

            for (int x = 0; x < size.Width; x++)
                _window.RectanglesGrid.ColumnDefinitions.Add(new ColumnDefinition());

            for (int y = 0; y < size.Height; y++)
                _window.RectanglesGrid.RowDefinitions.Add(new RowDefinition());
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
