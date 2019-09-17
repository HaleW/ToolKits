using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.Storage;
using static Tools.Utils.IOutils;

namespace Tools.Utils
{
    public class ReadDataUtils
    {
        private readonly DataResult Result = new DataResult();

        public async Task<DataResult> StartCollectAsync(StorageFolder storageFolder, ConditionsResult conditions)
        {
            Result.FolderStructure += "根目录：" + storageFolder.Path + "\n";

            await QueryFolderAsync(storageFolder, conditions, 2);

            return Result;
        }

        private async Task QueryFileAsync(StorageFolder storageFolder, ConditionsResult conditions, int indent)
        {
            foreach (StorageFile file in await storageFolder.GetFilesAsync())
            {
                PringFile(indent, file);

                await FileConditionsAsync(conditions, file);
            }
        }

        private async Task QueryFolderAsync(StorageFolder storageFolder, ConditionsResult conditions, int indent)
        {
            await QueryFileAsync(storageFolder, conditions, indent);

            foreach (StorageFolder folder in await storageFolder.GetFoldersAsync())
            {
                PrintFolder(indent, folder);

                await FolderConditionsAsync(folder, conditions, indent);
            }
        }

        private async Task FileConditionsAsync(ConditionsResult conditionsResult, StorageFile file)
        {
            foreach (KeyValuePair<Conditions, string[]> pair in conditionsResult.ExcludeFile)
            {
                switch (pair.Key)
                {
                    case Conditions.FileFullName:
                        foreach (string condition in pair.Value)
                        {
                            if (file.Name.Equals(condition)) return;
                        }
                        break;
                    case Conditions.FilePartName:
                        foreach (string condition in pair.Value)
                        {
                            if (file.Name.Contains(condition)) return;
                        }
                        break;
                    case Conditions.FileNamePrefix:
                        foreach (string condition in pair.Value)
                        {
                            if (file.Name.StartsWith(condition)) return;
                        }
                        break;
                    case Conditions.FileNameSuffix:
                        foreach (string condition in pair.Value)
                        {
                            if (file.Name.EndsWith(condition)) return;
                        }
                        break;
                }
            }

            Dictionary<Conditions, string[]> fileConditions = conditionsResult.IncludeFile;

            if (fileConditions.Count == 0)
            {
                await ResultDataAsync(file);

                return;
            }

            bool fileFlag = false;

            foreach (KeyValuePair<Conditions, string[]> pair in fileConditions)
            {
                switch (pair.Key)
                {
                    case Conditions.FileFullName:
                        foreach (string condition in pair.Value)
                        {
                            if (file.Name.Equals(condition)) fileFlag = true;
                        }
                        break;
                    case Conditions.FilePartName:
                        foreach (string condition in pair.Value)
                        {
                            if (file.Name.Contains(condition)) fileFlag = true;
                        }
                        break;
                    case Conditions.FileNamePrefix:
                        foreach (string condition in pair.Value)
                        {
                            if (file.Name.StartsWith(condition)) fileFlag = true;
                        }
                        break;
                    case Conditions.FileNameSuffix:
                        foreach (string condition in pair.Value)
                        {
                            if (file.Name.EndsWith(condition)) fileFlag = true;
                        }
                        break;
                }

                if (fileFlag) await ResultDataAsync(file);
            }
        }

        private async Task ResultDataAsync(StorageFile file)
        {
            DataResult result = await ReadFileAsync(file);
            Result.FileContent += result.FileContent;
            Result.ErrorFileName += result.ErrorFileName;
            Result.FileList += result.FileList;
        }

        private async Task FolderConditionsAsync(StorageFolder folder, ConditionsResult conditionsResult, int indent)
        {
            bool includeFlag = false;

            Dictionary<Conditions, string[]> includeDic = conditionsResult.IncludeFolder;
            Dictionary<Conditions, string[]> excludeDic = conditionsResult.ExcludeFolder;

            foreach (KeyValuePair<Conditions, string[]> pair in excludeDic)
            {
                switch (pair.Key)
                {
                    case Conditions.FolderFullName:
                        foreach (string condition in pair.Value)
                        {
                            if (folder.Name.Equals(condition)) return;
                        }
                        break;
                    case Conditions.FolderPartName:
                        foreach (string condition in pair.Value)
                        {
                            if (folder.Name.Contains(condition)) return;
                        }
                        break;
                    case Conditions.FolderNamePrefix:
                        foreach (string condition in pair.Value)
                        {
                            if (folder.Name.StartsWith(condition)) return;
                        }
                        break;
                    case Conditions.FolderNameSuffix:
                        foreach (string condition in pair.Value)
                        {
                            if (folder.Name.EndsWith(condition)) return;
                        }
                        break;
                }
            }

            foreach (KeyValuePair<Conditions, string[]> pair in includeDic)
            {
                switch (pair.Key)
                {
                    case Conditions.FolderFullName:
                        foreach (string condition in pair.Value)
                        {
                            if (folder.Name.Equals(condition)) includeFlag = true;
                        }
                        break;
                    case Conditions.FolderPartName:
                        foreach (string condition in pair.Value)
                        {
                            if (folder.Name.Contains(condition)) includeFlag = true;
                        }
                        break;
                    case Conditions.FolderNamePrefix:
                        foreach (string condition in pair.Value)
                        {
                            if (folder.Name.StartsWith(condition)) includeFlag = true;
                        }
                        break;
                    case Conditions.FolderNameSuffix:
                        foreach (string condition in pair.Value)
                        {
                            if (folder.Name.EndsWith(condition)) includeFlag = true;
                        }
                        break;
                }

                if (includeFlag) break;
            }

            if (includeFlag || (includeDic.Count == 0))
            {
                await QueryFolderAsync(folder, conditionsResult, indent + 2);
            }
        }

        private void PringFile(int indent, StorageFile file)
        {
            for (int i = 0; i < indent; i++)
            {
                Result.FolderStructure += "  ";
            }
            Result.FolderStructure += file.Name + "\n";
        }

        private void PrintFolder(int indent, StorageFolder folder)
        {
            for (int i = 0; i < indent; i++)
            {
                Result.FolderStructure += "  ";
            }
            Result.FolderStructure += "文件夹：" + folder.Name + "\n";
        }
    }
}
