using CollisionEditor.model;

namespace CollisionEditor.viewModel
{
    public static class ViewModelAngleService
    {
        public static string GetAngleMapFilePath()
        {
            System.Windows.Forms.OpenFileDialog openFileDialog = new System.Windows.Forms.OpenFileDialog();
            string filePath = string.Empty;
            openFileDialog.Filter = "Binary Files(*.bin)| *.bin| All files(*.*) | *.*";
            if (openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                filePath = openFileDialog.FileName;
            }
            return filePath;
        }
        public static (int byteAngle, string hexAngle, double fullAngle) GetAngles(byte byteAngle)
        {
            return (byteAngle, Convertor.GetHexAngle(byteAngle), Convertor.GetFullAngle(byteAngle));
        }
    }
}