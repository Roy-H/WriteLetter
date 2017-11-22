using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Pickers;

namespace WriteLetter.SDK.Helper
{
    public sealed class FolderHelper
    {
        private static FolderHelper instance;
        private static object asyncRoot = new object();
        public static FolderHelper Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (asyncRoot)
                    {
                        if (instance == null)
                            instance = new FolderHelper();
                    }
                }
                return instance;
            } }

        public async Task<StorageFolder> PickupFolder(PickerLocationId id = PickerLocationId.DocumentsLibrary)
        {
            var folderPicker = new FolderPicker() { SuggestedStartLocation = id };
            var picker = new FileOpenPicker { SuggestedStartLocation = PickerLocationId.DocumentsLibrary };
            picker.FileTypeFilter.Add("*");
            var folder = await folderPicker.PickSingleFolderAsync();
            return folder;
            
        }
        public async Task<StorageFile> PickupFile(PickerLocationId id = PickerLocationId.DocumentsLibrary)
        {
            var folderPicker = new FolderPicker() { };
            var picker = new FileOpenPicker { SuggestedStartLocation = id };
            picker.FileTypeFilter.Add("*");
            var file = await picker.PickSingleFileAsync();
            return file;
        }
    }
}
