using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

using Windows.Security.Cryptography;
using Windows.Security.Cryptography.Core;
using Windows.Storage.Streams;
using System.Threading.Tasks;




// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace Hash
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }

        private async void PickAFileButton_Click(object sender, RoutedEventArgs e)
        {
            // Clear previous returned file name, if it exists, between iterations of this scenario
            OutputTextBlock.Text = "";

            FileOpenPicker openPicker = new FileOpenPicker();
            openPicker.ViewMode = PickerViewMode.List;
            openPicker.SuggestedStartLocation = PickerLocationId.PicturesLibrary;
            openPicker.FileTypeFilter.Add(".iso");
            openPicker.FileTypeFilter.Add(".jpeg");
            openPicker.FileTypeFilter.Add(".png");
            openPicker.FileTypeFilter.Add(".jpg");
            openPicker.FileTypeFilter.Add(".mp4");
            StorageFile file = await openPicker.PickSingleFileAsync();
            OutputTextBlock.Text = "请稍后……";
            if (file != null)
            {
                // Hash a message.
                String strAlgName = HashAlgorithmNames.Sha1;
                String strEncodedHash = await FileHashMsg(strAlgName, file);

                // Application now has read/write access to the picked file
                OutputTextBlock.Text = strEncodedHash;

           
            }
            else
            {
                OutputTextBlock.Text = "操作取消";
            }
        }


        //字符串hash
        public String StringHashMsg(String strAlgName, String strMsg)
        {  
            // Convert the message string to binary data.
            IBuffer buffUtf8Msg = CryptographicBuffer.ConvertStringToBinary(strMsg, BinaryStringEncoding.Utf8);

            // Create a HashAlgorithmProvider object.
            HashAlgorithmProvider objAlgProv = HashAlgorithmProvider.OpenAlgorithm(strAlgName);
            // Demonstrate how to retrieve the name of the hashing algorithm.
            String strAlgNameUsed = objAlgProv.AlgorithmName;
            // Hash the message.
       
            IBuffer buffHash = objAlgProv.HashData(buffUtf8Msg);
            // Verify that the hash length equals the length specified for the algorithm.
            if (buffHash.Length != objAlgProv.HashLength)
            {
                throw new Exception("There was an error creating the hash");
            }
            // Convert the hash to a string (for display).
            //String strHashBase64 = CryptographicBuffer.EncodeToBase64String(buffHash);
            String strHashHex = CryptographicBuffer.EncodeToHexString(buffHash);
            // Return the encoded string
            return strHashHex;
        }

        //文件hash
        public async Task<String> FileHashMsg(String strAlgName,StorageFile file)
        {
            IBuffer bufferMsg = await FileIO.ReadBufferAsync(file);

          
            // Create a HashAlgorithmProvider object.
            HashAlgorithmProvider objAlgProv = HashAlgorithmProvider.OpenAlgorithm(strAlgName);
            // Demonstrate how to retrieve the name of the hashing algorithm.
            String strAlgNameUsed = objAlgProv.AlgorithmName;
            // Hash the message.

            IBuffer buffHash = objAlgProv.HashData(bufferMsg);
            // Verify that the hash length equals the length specified for the algorithm.
            if (buffHash.Length != objAlgProv.HashLength)
            {
                throw new Exception("There was an error creating the hash");
            }
            // Convert the hash to a string (for display).
            //String strHashBase64 = CryptographicBuffer.EncodeToBase64String(buffHash);
            String strHashHex= CryptographicBuffer.EncodeToHexString(buffHash);

            // Return the encoded string
            return strHashHex;
        }
    }
}
