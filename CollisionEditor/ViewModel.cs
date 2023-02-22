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
        private Tilemap Tilemap { get; }

        public MainViewModel() => this.Tilemap = null;

        // Since 'destinationFilePath' was picked using a file dialog, 
        // this method can't fail.

        public void OpenAngleMapFile(string filePath)
        {
            Anglemap anglemap = new Anglemap(filePath);
            MainWindow.ShowAnglemap(anglemap);
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
