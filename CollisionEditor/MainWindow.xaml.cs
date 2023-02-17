using Microsoft.Win32;
using System.Windows;
using System.Windows.Forms;
using CollisionEditor.model;
using static CollisionEditor.MainWindow;
using System.Windows.Media.Imaging;
using System.Collections.Generic;
using System.Windows.Media;
using System.IO;

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
                Anglemap anglemap = new Anglemap(filePath);
                System.Windows.Forms.MessageBox.Show(string.Join(" ", anglemap.Values));
            }
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
                tileStrip = new Tilemap(filePath);
                ImageOfTile.Source = Convertor.BitmapConvert(tileStrip.bitmaps[5]);
            }
        }

        private void MenuSaveTiletmapClick(object sender, RoutedEventArgs e)
        {
            if (tileStrip is null)
            {
                System.Windows.Forms.MessageBox.Show("Ошибка: Вы не выбрали Tilemap, чтобы её сохранить");
            }
            
            System.Windows.Forms.SaveFileDialog saveFileDialog = new System.Windows.Forms.SaveFileDialog();
            
            saveFileDialog.Filter = "Image Files(*.png)| *.png";
            //Спрашивать RowCount
            if (saveFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                tileStrip.Save(Path.GetFullPath(saveFileDialog.FileName), 1);
            }
        }
    }
}
