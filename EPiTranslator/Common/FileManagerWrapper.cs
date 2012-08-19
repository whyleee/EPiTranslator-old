using System.IO;
using System.Text;

namespace EPiTranslator.Common
{
    /// <summary>
    /// The wrapper for common .NET IO API.
    /// </summary>
    public class FileManagerWrapper
    {
        #region File Operations

        /// <summary>
        /// Gets child files of specified directory.
        /// </summary>
        /// <param name="directoryPath">The directory path.</param>
        /// <param name="searchPattern">The search pattern to limit the search.
        /// Can contain <c>?</c> and <c>*</c> wildcard symbols.</param>
        /// <returns>
        /// Collection of child file paths.
        /// </returns>
        public virtual string[] GetFiles(string directoryPath, string searchPattern)
        {
            return Directory.GetFiles(directoryPath, searchPattern);
        }

        /// <summary>
        /// Determines whether the specified file exists. 
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns></returns>
        public virtual bool Exists(string path)
        {
            return File.Exists(path);
        }

        public virtual void Move(string path, string newPath)
        {
            File.Move(path, newPath);
        }

        /// <summary>
        /// Deletes the file by the specified path.
        /// </summary>
        /// <param name="path">The path to the file.</param>
        public virtual void Delete(string path)
        {
            File.Delete(path);
        }

        #endregion

        #region Directory Operations

        /// <summary>
        /// Gets child directories of specified directory.
        /// </summary>
        /// <param name="directoryPath">The directory path.</param>
        /// <returns>
        /// Collection of child directory paths.
        /// </returns>
        public virtual string[] GetDirectories(string directoryPath)
        {
            return Directory.GetDirectories(directoryPath);
        }

        /// <summary>
        /// Determines whether the given path refers to an existing directory on disk.
        /// </summary>
        /// <param name="path">The path to test.</param>
        /// <returns>
        /// <c>true</c> if path refers to an exising directory; otherwise, <c>false</c>
        /// </returns>
        public virtual bool DirectoryExists(string path)
        {
            return Directory.Exists(path);
        }

        /// <summary>
        /// Creates all directories and subdirectories in the specified path.
        /// </summary>
        /// <param name="path">The directory path to create.</param>
        public virtual void CreateDirectory(string path)
        {
            Directory.CreateDirectory(path);
        }

        /// <summary>
        /// Ensures that parent directory of the specified file exists. If not - creates all
        /// directories and subdirectories in the specified path.
        /// </summary>
        /// <param name="filePath">The file path to test.</param>
        public virtual void EnsureDirectoryExists(string filePath)
        {
            var parentDirectory = Path.GetDirectoryName(filePath);

            if (!DirectoryExists(parentDirectory))
            {
                CreateDirectory(parentDirectory);
            }
        }

        #endregion

        #region Reading/Writing

        /// <summary>
        /// Reads entire file text storing each line in the separate string.
        /// </summary>
        /// <param name="path">The path to file for reading.</param>
        /// <param name="encoding">The encoding.</param>
        /// <returns>
        /// Collection of strings that represents file text lines.
        /// </returns>
        public virtual string[] ReadAllLines(string path, Encoding encoding = null)
        {
            if (encoding == null)
            {
                encoding = Encoding.Default;
            }

            return File.ReadAllLines(path, encoding);
        }

        /// <summary>
        /// Reads entire file text into string.
        /// </summary>
        /// <param name="path">The path to file for reading.</param>
        /// <returns>
        /// String with entire file text.
        /// </returns>
        public virtual string ReadAllText(string path)
        {
            return File.ReadAllText(path);
        }

        /// <summary>
        /// Opens a file, appends the specified string to the file, and then closes the file.
        /// If the file does not exist, this method creates a file, writes the specified
        /// string to the file, then closes the file.
        /// </summary>
        /// <param name="path">The path to file for writing.</param>
        /// <param name="contents">The contents to write.</param>
        public virtual void AppendAllText(string path, string contents)
        {
            File.AppendAllText(path, contents);
        }

        /// <summary>
        /// Opens a file, appends the specified string to the file, and then closes the file.
        /// If the file does not exist, this method creates a file, writes the specified
        /// string to the file, then closes the file.
        /// </summary>
        /// <param name="path">The path to file for writing.</param>
        /// <param name="contents">The contents to write.</param>
        /// <param name="encoding">Encoding for the file.</param>
        public virtual void AppendAllText(string path, string contents, Encoding encoding)
        {
            File.AppendAllText(path, contents, encoding);
        }

        #endregion
    }
}
