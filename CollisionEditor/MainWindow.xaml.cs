using Microsoft.Win32;
using System.Windows;
using System.Windows.Forms;
using CollisionEditor.model;
using static CollisionEditor.MainWindow;
using System.Windows.Media.Imaging;
using System.Collections.Generic;
using System.Windows.Media;
using System.IO;
using CollisionEditor.viewModel;

namespace CollisionEditor
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Tilemap tileStrip = null;
        public MainWindow()
        {
            InitializeComponent();
            MenuOpenAngleMap.Click += MenuOpenAngleMapClick;
            MenuOpenTileStrip.Click += MenuOpenTileStripClick;
            MenuSaveTiletmap.Click += MenuSaveTiletmapClick;
        }

        private void MenuSaveTiletmapClick(object sender, RoutedEventArgs e)
        {

            //if (tileStrip is null)
            //{
            //    System.Windows.Forms.MessageBox.Show("Ошибка: Вы не выбрали Tilemap, чтобы её сохранить");
            //}

            System.Windows.Forms.SaveFileDialog saveFileDialog = new System.Windows.Forms.SaveFileDialog();

            saveFileDialog.Filter = "Image Files(*.png)| *.png";
            //Спрашивать RowCount
            
            if (saveFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                (this.DataContext as MainViewModel).SaveTileStrip(saveFileDialog.FileName);
            }
        }

        private void MenuOpenAngleMapClick(object sender, RoutedEventArgs e)
        {

            System.Windows.Forms.OpenFileDialog openFileDialog = new System.Windows.Forms.OpenFileDialog();
            string filePath = string.Empty;
            openFileDialog.Filter = "Binary Files(*.bin)| *.bin| All files(*.*) | *.*";
            if (openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                ImageOfTile.Source = null;
                filePath = openFileDialog.FileName;

                (this.DataContext as MainViewModel).OpenAngleMapFile(filePath);

            }
        }
        public static void ShowAnglemap(Anglemap anglemap)
        {
            System.Windows.Forms.MessageBox.Show(string.Join(" ", anglemap.Values));
        }

        private void MenuOpenTileStripClick(object sender, RoutedEventArgs e)
        {

            System.Windows.Forms.OpenFileDialog openFileDialog = new System.Windows.Forms.OpenFileDialog();
            openFileDialog.Filter = "Image Files(*.png)| *.png| All files(*.*) | *.*";
            string filePath = string.Empty;
            if (openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                ImageOfTile.Source = null;
                filePath = openFileDialog.FileName;
                (this.DataContext as MainViewModel).OpenTileStripFile(filePath);
            }
        }
        public static void ShowTileStrip(System.Windows.Media.Imaging.BitmapSource TileStrip)
        {
            MainWindow mainWindow = (MainWindow)System.Windows.Application.Current.MainWindow;
            mainWindow.ImageOfTile.Source = TileStrip;
        }

    }
}
