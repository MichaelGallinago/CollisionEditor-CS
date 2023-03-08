namespace CollisionEditor.viewModel
{
    public static class ViewModelTileService
    {

        public static string GetTileMapFilePath()
        {
            System.Windows.Forms.OpenFileDialog openFileDialog = new System.Windows.Forms.OpenFileDialog();
            openFileDialog.Filter = "Image Files(*.png)| *.png| All files(*.*) | *.*";
            string filePath = string.Empty;
            if (openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                filePath = openFileDialog.FileName;
            }
            return filePath;
        }
        public static string GetTileMapSavePath()
        {
            System.Windows.Forms.SaveFileDialog saveFileDialog = new System.Windows.Forms.SaveFileDialog();
            saveFileDialog.Filter = "Image Files(*.png)| *.png";
            string filePath = string.Empty;
            if (saveFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                filePath = saveFileDialog.FileName;
            }
            return filePath;
        }
    }
}