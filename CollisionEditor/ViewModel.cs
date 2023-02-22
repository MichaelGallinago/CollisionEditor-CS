using CollisionEditor.model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CollisionEditor;
namespace CollisionEditor.viewModel
{
    class MainViewModel : INotifyPropertyChanged
    {

        // A model class that is responsible to persist and load data
        private Tilemap Tilemap { get; set;}

        public MainViewModel() => this.Tilemap = null;

        // Since 'destinationFilePath' was picked using a file dialog, 
        // this method can't fail.

        public void OpenAngleMapFile(string filePath)
        {
            Anglemap anglemap = new Anglemap(filePath);
            MainWindow.ShowAnglemap(anglemap);
        }
        public void OpenTileStripFile(string filePath)
        {
            this.Tilemap = new Tilemap(filePath);
            MainWindow.ShowTileStrip(Convertor.BitmapConvert(Tilemap.Tiles[5]));                
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
