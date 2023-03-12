﻿using CollisionEditor.model;
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
        private TileSet TileSet { get; set; }
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
        public int ChosenTile { get; set; }

        public MainViewModel(MainWindow window)
        {
            AngleMap = new AngleMap(0);
            TileSet  = new TileSet(0);

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
            mainWindow.ByteAngleIncrimentButton.IsEnabled= true;
            mainWindow.ByteAngleDecrementButton.IsEnabled = true;

            mainWindow.TextBlockHexAngle.Text = hexAngle;
            mainWindow.HexAngleIncrimentButton.IsEnabled = true;
            mainWindow.HexAngleDecrementButton.IsEnabled = true;

            mainWindow.TextBlockFullAngle.Text = fullAngle.ToString() + "'";
            mainWindow.FullAngleIncrimentButton.IsEnabled = true;
            mainWindow.FullAngleDecrementButton.IsEnabled = true;
        }

        private void MenuOpenTileMap()
        {
            string filePath = ViewModelTileService.GetTileMapFilePath();
            if (filePath is not null)
            {
                TileSet = new TileSet(filePath);
                AngleMap = new AngleMap(TileSet.Tiles.Count);
                ViewModelAssistant.BitmapConvert(TileSet.Tiles[ChosenTile]);
                ShowTile(ViewModelAssistant.BitmapConvert(TileSet.Tiles[ChosenTile]));
                window.Heights.Text = ViewModelAssistant.GetCollisionValues(TileSet.HeightMap[ChosenTile]);
                window.Widths.Text = ViewModelAssistant.GetCollisionValues(TileSet.WidthMap[ChosenTile]);

                (int byteAngle, string hexAngle, double fullAngle) angles = ViewModelAssistant.GetAngles(AngleMap, ChosenTile);
                ShowAngles(angles.byteAngle, angles.hexAngle, angles.fullAngle);
            }
        }

        private static void ShowTile(System.Windows.Media.Imaging.BitmapSource TileStrip)
        {
            MainWindow mainWindow = (MainWindow)System.Windows.Application.Current.MainWindow;
            mainWindow.ImageOfTile.Source = TileStrip;
        }

        private void MenuSaveTileMap()
        {
            if (TileSet.Tiles.Count==0)
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

        private void MenuSaveWidthMap()
        {
            if (TileSet.Tiles.Count == 0)
            {
                System.Windows.Forms.MessageBox.Show("Error: The WidthMap isn't generated!");
            }
            else
            {
                string filePath = ViewModelTileService.GetWidthMapSavePath();
                if (filePath is not null)
                {
                    TileSet.SaveCollisionMap(Path.GetFullPath(filePath), TileSet.WidthMap);
                }
            }
        }

        private void MenuSaveHeightMap()
        {
            if (TileSet.Tiles.Count == 0)
            {
                System.Windows.Forms.MessageBox.Show("Error: The HeightMap isn't generated!");
            }
            else
            {
                string filePath = ViewModelTileService.GetWidthMapSavePath();
                if (filePath is not null)
                {
                    TileSet.SaveCollisionMap(Path.GetFullPath(filePath), TileSet.HeightMap);
                }
            }
        }

        private void MenuSaveAngleMap()
        {
            if (AngleMap.Values.Count==0)
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

        private void MenuSaveAll()
        {
            MenuSaveAngleMap();
            MenuSaveHeightMap();
            MenuSaveWidthMap();
            MenuSaveTileMap();
        }

        private void MenuUnloadTileMap()
        {
            TileSet = new TileSet(AngleMap.Values.Count);
        }

        private void MenuUnloadAngleMap()
        {
            AngleMap = new AngleMap(TileSet.Tiles.Count);
        }

        private void MenuUnloadAll()
        {
            TileSet = new TileSet(0);
            AngleMap = new AngleMap(0);
        }

        private void AngleIncrement()
        {
            byte byteAngle = AngleMap.ChangeAngle(ChosenTile, 1);

            (int byteAngle, string hexAngle, double fullAngle) angles = ViewModelAngleService.GetAngles(byteAngle);
            ShowAngles(byteAngle, angles.hexAngle, angles.fullAngle);

            window.DrawRedLine();
        }

        private void AngleDecrement()
        {
            byte byteAngle = AngleMap.ChangeAngle(ChosenTile, -1);

            (int byteAngle, string hexAngle, double fullAngle) angles = ViewModelAngleService.GetAngles(byteAngle);
            ShowAngles(byteAngle, angles.hexAngle, angles.fullAngle);

            window.DrawRedLine();
        }

        private void SelectTile()
        {
            ShowTile(ViewModelAssistant.BitmapConvert(TileSet.Tiles[ChosenTile]));
            window.Heights.Text = ViewModelAssistant.GetCollisionValues(TileSet.HeightMap[ChosenTile]);
            window.Widths.Text  = ViewModelAssistant.GetCollisionValues(TileSet.WidthMap[ChosenTile]);
        }

        private void ExitApp()
        {
            window.Close();
        }

        public void AngleUpdator(Vector2<int> vectorGreen, Vector2<int> vectorBlue)
        {
            if (AngleMap.Values.Count!=0)
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
