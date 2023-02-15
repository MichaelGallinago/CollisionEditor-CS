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

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            int width = 128;
            int height = width;
            int stride = width / 8;
            byte[] pixels = new byte[height * stride];

            // Try creating a new image with a custom palette.
            List<System.Windows.Media.Color> colors = new List<System.Windows.Media.Color>();
            colors.Add(System.Windows.Media.Colors.Red);
            colors.Add(System.Windows.Media.Colors.Blue);
            colors.Add(System.Windows.Media.Colors.Green);
            BitmapPalette myPalette = new BitmapPalette(colors);

            ImageOfTile.Source = BitmapSource.Create(width,height,96,96,PixelFormats.Indexed1,myPalette,pixels,stride);
        }
    }
}
