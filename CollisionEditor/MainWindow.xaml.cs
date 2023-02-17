using Microsoft.Win32;
using System.Windows;
using System.Windows.Forms;
using CollisionEditor.model;
using static CollisionEditor.MainWindow;
using System.Windows.Media.Imaging;
using System.Collections.Generic;
using System.Windows.Media;

namespace CollisionEditor
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            
        }
        private void MenuOpenAngleMapClick(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.OpenFileDialog openFileDialog = new System.Windows.Forms.OpenFileDialog();
            string filePath = string.Empty;
            if (openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                filePath = openFileDialog.FileName;  
            }
            Anglemap anglemap = new Anglemap(filePath);
            System.Windows.Forms.MessageBox.Show(string.Join(" ",anglemap.Values));
        }

        private void MenuOpenTileStripClick(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.OpenFileDialog openFileDialog = new System.Windows.Forms.OpenFileDialog();
            string filePath = string.Empty;
            if (openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                filePath = openFileDialog.FileName;
            }
            Tilemap tileStrip = new Tilemap(filePath);
            ImageOfTile.Source = Convertor.Convert(tileStrip.bitmaps[5]);
        }
    }
}
