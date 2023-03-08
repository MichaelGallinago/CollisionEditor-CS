using CollisionEditor.model;
using Avalonia.Controls;

namespace CollisionEditor.viewModel
{
    public static class ViewModelAngleService
    {
        public static string GetAngleMapFilePath()
        {
            OpenFileDialog dialog = new OpenFileDialog();
            string filePath = string.Empty;

            dialog.Filters.Add(new FileDialogFilter() { Name = "Binary Files(*.bin)", Extensions = { "bin" } });
            dialog.AllowMultiple = false;

            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                filePath = dialog.FileName;
            }
            return filePath;
        }
        public static (int byteAngle, string hexAngle, double fullAngle) GetAngles(byte byteAngle)
        {
            return (byteAngle, Convertor.GetHexAngle(byteAngle), Convertor.GetFullAngle(byteAngle));
        }
    }
}