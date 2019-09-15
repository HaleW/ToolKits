using System;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.AccessCache;
using Windows.Storage.Pickers;

namespace Tools.Utils
{
    public class IOutils
    {
        public static async Task<StorageFolder> OpenFolderAsync()
        {
            var folderPicker = new FolderPicker();

            folderPicker.FileTypeFilter.Add("*");

            StorageFolder folder = await folderPicker.PickSingleFolderAsync();

            if (folder != null)
            {
                StorageApplicationPermissions.FutureAccessList.AddOrReplace("PickedFolderToken", folder);
            }

            return folder;
        }

        public static async Task<DataResult> ReadFileAsync(StorageFile file)
        {
            DataResult result = new DataResult();
            try
            {
                string fileName = file.Name;
                string fileContent = await FileIO.ReadTextAsync(file);

                result.FileContent = fileName + "\n" + fileContent + "\n";
                result.FileList = fileName + "\n";
            }
            catch
            {
                result.ErrorFileName = file.Name + "\n";
            }

            return result;
        }
    }
}
