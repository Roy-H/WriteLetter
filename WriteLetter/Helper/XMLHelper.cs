using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using System.IO;
using XmlReader = System.Xml.XmlReader;
using XmlWriter = System.Xml.XmlWriter;
using StreamWriter = System.IO.StreamWriter;
using StreamReader = System.IO.StreamReader;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;

using Newtonsoft.Json;
using WriteLetter.ViewModels;
using System.Runtime.Serialization;
using System.Xml;
using System.Runtime.Serialization.Json;

namespace WriteLetter.Helper
{
    static class DataHelper
    {        
        static public async Task<object> Load(Type type,string fileName)
        {
            var fileExist = await ApplicationData.Current.LocalFolder.TryGetItemAsync(fileName) as StorageFile;
            if (fileExist == null)
            {                
                return null;
            }
            try
            {
                var file = await ApplicationData.Current.LocalFolder.TryGetItemAsync(fileName);
                if (file == null)
                    return file;
                var file2 = await ApplicationData.Current.LocalFolder.GetFileAsync(fileName);                          
                DataContractJsonSerializer ser = new DataContractJsonSerializer(type);
                
                var jsonbuffer = await FileIO.ReadTextAsync(file2);
                var ms = new MemoryStream(UnicodeEncoding.UTF8.GetBytes(jsonbuffer));
                var data = ser.ReadObject(ms);                
                return data;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            
        }

        static public async Task Save(Type type,object obj, string fileName)
        {            
            try
            {
                StorageFolder storageFolder = ApplicationData.Current.LocalFolder;
                var file = await storageFolder.CreateFileAsync(fileName, CreationCollisionOption.ReplaceExisting);
                var ser = new DataContractJsonSerializer(type);      
                MemoryStream stream = new MemoryStream();
                ser.WriteObject(stream, obj);
                string szJson = Encoding.UTF8.GetString(stream.ToArray());

                StorageFile sampleFile = await storageFolder.GetFileAsync(fileName);
                await FileIO.WriteTextAsync(sampleFile, szJson);          
            }
            catch (Exception ex)
            {

                throw ex;
            }   
        }

        private static async void TransferData()
        {
            // Initialize the in-memory stream where data will be stored.
            using (var stream = new Windows.Storage.Streams.InMemoryRandomAccessStream())
            {
                // Create the data writer object backed by the in-memory stream.
                using (var dataWriter = new Windows.Storage.Streams.DataWriter(stream))
                {
                    dataWriter.UnicodeEncoding = Windows.Storage.Streams.UnicodeEncoding.Utf8;
                    dataWriter.ByteOrder = Windows.Storage.Streams.ByteOrder.LittleEndian;

                    // Write each element separately.
                    //foreach (string inputElement in inputElements)
                    //{
                    //    uint inputElementSize = dataWriter.MeasureString(inputElement);
                    //    dataWriter.WriteUInt32(inputElementSize);
                    //    dataWriter.WriteString(inputElement);
                    //}
                    

                    // Send the contents of the writer to the backing stream.
                    await dataWriter.StoreAsync();

                    // For the in-memory stream implementation we are using, the flushAsync call is superfluous,
                    // but other types of streams may require it.
                    await dataWriter.FlushAsync();

                    // In order to prolong the lifetime of the stream, detach it from the DataWriter so that it 
                    // will not be closed when Dispose() is called on dataWriter. Were we to fail to detach the 
                    // stream, the call to dataWriter.Dispose() would close the underlying stream, preventing 
                    // its subsequent use by the DataReader below.
                    dataWriter.DetachStream();
                }

                // Create the input stream at position 0 so that the stream can be read from the beginning.
                stream.Seek(0);
                using (var dataReader = new Windows.Storage.Streams.DataReader(stream))
                {
                    // The encoding and byte order need to match the settings of the writer we previously used.
                    dataReader.UnicodeEncoding = Windows.Storage.Streams.UnicodeEncoding.Utf8;
                    dataReader.ByteOrder = Windows.Storage.Streams.ByteOrder.LittleEndian;

                    // Once we have written the contents successfully we load the stream.
                    await dataReader.LoadAsync((uint)stream.Size);

                    var receivedStrings = "";

                    // Keep reading until we consume the complete stream.
                    while (dataReader.UnconsumedBufferLength > 0)
                    {
                        // Note that the call to readString requires a length of "code units" to read. This
                        // is the reason each string is preceded by its length when "on the wire".
                        uint bytesToRead = dataReader.ReadUInt32();
                        receivedStrings += dataReader.ReadString(bytesToRead) + "\n";
                    }

                    // Populate the ElementsRead text block with the items we read from the stream.
                    //ElementsRead.Text = receivedStrings;
                }
            }
        }
    }

    
}
