/* The IO class allows developers to access external files, write to
 * files, use standard conversion methods.
 * 
 * Author: Corey St-Jacques
 */

using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Gameplay.Map.ChunkMaster.Scripts.ChunkMaster.ChunkMaster.src.NET.Interfaces;

namespace Gameplay.Map.ChunkMaster.Scripts.ChunkMaster.ChunkMaster.src.Utils
{
    /// <summary>
    /// This class may be used for saving or writing files, 
    /// also conversions from bytes to objects and vice versa.
    /// </summary>
    public static class IO
    {
        /// <summary>
        /// This method converts an object type to a byte array.
        /// </summary>
        /// <param name="obj">The object you would like to convert.</param>
        /// <returns>byte[]</returns>
        public static byte[] ObjectToByteArray(object obj)
        {
            BinaryFormatter bf = new BinaryFormatter();
            using (var ms = new MemoryStream())
            {
                bf.Serialize(ms, obj);
                return ms.ToArray();
            }
        }

        /// <summary>
        /// This method converts a byte array into a readable object.
        /// </summary>
        /// <param name="arrBytes">The byte array you would like to convert.</param>
        /// <returns>object</returns>
        public static object ByteArrayToObject(byte[] arrBytes)
        {
            if (arrBytes != null)           // array is null
                if (arrBytes.Length > 0)    // array is too small
                {
                    using (var memStream = new MemoryStream())
                    {
                        var binForm = new BinaryFormatter();
                        memStream.Write(arrBytes, 0, arrBytes.Length);
                        memStream.Seek(0, SeekOrigin.Begin);
                        var obj = binForm.Deserialize(memStream);
                        return obj;
                    }
                }
                else
                    return null;
            else
                return null;
        }

        /// <summary>
        /// Saves the byte array to file.
        /// </summary>
        /// <param name="_FileName">Your filename and directory to save to.</param>
        /// <param name="_ByteArray">Your byte array content you would like to save to.</param>
        /// <returns>bool</returns>
        public static bool SaveToFile(string _FileName, byte[] _ByteArray)
        {
            try
            {
                // Open file for reading
                System.IO.FileStream _FileStream =
                   new System.IO.FileStream(_FileName, System.IO.FileMode.Create,
                                            System.IO.FileAccess.Write);
                // Writes a block of bytes to this stream using data from
                // a byte array.
                _FileStream.Write(_ByteArray, 0, _ByteArray.Length);

                // close file stream
                _FileStream.Close();

                return true;
            }
            catch (Exception _Exception)
            {
                // Error
                Console.WriteLine("Exception caught in process: {0}",
                                  _Exception.ToString());
            }

            // error occured, return false
            return false;
        }

        /// <summary>
        /// Reads a byte array from file.
        /// </summary>
        /// <param name="filePath">Provide the filepath at which to load your file.</param>
        /// <returns>byte[]</returns>
        public static byte[] ReadFile(string filePath)
        {
            byte[] buffer;
            FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            try
            {
                int length = (int)fileStream.Length;  // get file length
                buffer = new byte[length];            // create buffer
                int count;                            // actual number of bytes read
                int sum = 0;                          // total number of bytes read

                Console.WriteLine("Reading: " + length + " bytes.");

                // read until Read method returns 0 (end of the stream has been reached)
                while ((count = fileStream.Read(buffer, sum, length - sum)) > 0)
                    sum += count;  // sum is a buffer offset for next reading
            }
            finally
            {
                fileStream.Close();
                Console.WriteLine("Closing: " + filePath);
            }
            return buffer;
        }

        /// <summary>
        /// Creates a directory if the path doesn't already exist.
        /// </summary>
        /// <param name="path">The path inwhich you would like to create a path.</param>
        /// <returns>string</returns>
        public static string OpenDirectory(string path)
        {
            string directory = path;
            if (!Directory.Exists(path))
            {
                DirectoryInfo di = Directory.CreateDirectory(path);
                Console.WriteLine("Created dir: " + di.FullName);
                directory = di.FullName;
            }
            else
            {
                directory = Path.GetFullPath(path);
            }
            return directory;
        }

        /// <summary>
        /// Save the current sector to a file using matrix points as the filename.
        /// </summary>
        /// <param name="sector">The current sector you would like to save.</param>
        /// <param name="path">The path where you would like to save the sector to.</param>
        /// <returns>string</returns>
        public static string SaveSector(ISector sector, string path)
        {
            byte[] bytes = ObjectToByteArray(sector.GetBlocksArray());
            string filePath = path + "/" + sector.sectorMatrixPoint.ToString();
            SaveToFile(filePath, bytes);
            return filePath;
        }

        /// <summary>
        /// Checks to see if the file exists.
        /// </summary>
        /// <param name="path">Requires the file path.</param>
        /// <returns>bool</returns>
        public static bool FileExists(string path)
        {
            return File.Exists(path);
        }

        /// <summary>
        /// Destroys the world's directory.
        /// </summary>
        /// <param name="path">Requires world path.</param>
        public static void DestroyDirectory(string path)
        {
            Directory.Delete(path, true);
        }
    }
}
