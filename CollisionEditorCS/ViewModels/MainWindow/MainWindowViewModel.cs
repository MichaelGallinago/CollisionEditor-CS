using CollisionEditor.model;
using CollisionEditor.viewModel;
using CollisionEditorCS.Views;
using System.ComponentModel;
using Avalonia.Media.Imaging;
using ReactiveUI;
using System.Reactive;
using Avalonia.Controls;
using MessageBoxSlim.Avalonia.DTO;
using MessageBoxSlim.Avalonia.Enums;
using MessageBoxSlim.Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia;

namespace CollisionEditorCS.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        public string Greeting => "Welcome to Avalonia!";
        private MainWindow window;
        private AngleMap AngleMap { get; set; }
        private TileSet TileSet { get; set; }
        public ReactiveCommand<Unit, Unit> MenuOpenAngleMapCommand { get; }
        public ReactiveCommand<Unit, Unit> MenuOpenTileMapCommand { get; }
        public ReactiveCommand<Unit, Unit> MenuSaveTileMapCommand { get; }
        public ReactiveCommand<Unit, Unit> SelectTileCommand { get; }
        public ReactiveCommand<Unit, Unit> AngleIncrementCommand { get; }
        public ReactiveCommand<Unit, Unit> AngleDecrementCommand { get; }
        public ReactiveCommand<Unit, Unit> ExitAppCommand { get; }
        public int ChosenTile { get; set; }

        public MainWindowViewModel(MainWindow window)
        {
            this.window = window;
            MenuOpenAngleMapCommand = ReactiveCommand.Create(MenuOpenAngleMap);
            MenuOpenTileMapCommand = ReactiveCommand.Create(MenuOpenTileMap);
            MenuSaveTileMapCommand = ReactiveCommand.Create(MenuSaveTileMap);

            AngleIncrementCommand = ReactiveCommand.Create(AngleIncrement);
            AngleDecrementCommand = ReactiveCommand.Create(AngleDecrement);
            SelectTileCommand = ReactiveCommand.Create(SelectTile);

            ExitAppCommand = ReactiveCommand.Create(ExitApp);
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
            MainWindow mainWindow = (MainWindow)((IClassicDesktopStyleApplicationLifetime)Application.Current.ApplicationLifetime).Windows;

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

        private static void ShowTile(Bitmap TileStrip)
        {
            MainWindow mainWindow = (MainWindow)((IClassicDesktopStyleApplicationLifetime)Application.Current.ApplicationLifetime).Windows;
            mainWindow.ImageOfTile.Source = TileStrip;
        }

        private void MenuSaveTileMap()
        {
            if (TileSet is null)
            {
                MainWindow mainWindow = (MainWindow)((IClassicDesktopStyleApplicationLifetime)Application.Current.ApplicationLifetime).Windows;
                BoxedMessage.Create(new MessageBoxParams
                {
                    Buttons = ButtonEnum.Ok,
                    ContentTitle = "Error",
                    ContentMessage = "Error: You didn't choose TileMap to save it",
                    Location = WindowStartupLocation.CenterScreen,
                }).ShowDialogAsync(mainWindow);
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
            return (ViewModelAssistant.GetCorrectDotPosition(new Vector2<double>(x, y), 8));
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}