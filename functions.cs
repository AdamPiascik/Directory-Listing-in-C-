using System;
using System.IO;
using System.Text;
using System.Collections.Generic;

namespace DirectoryMethods
{
    /*  This class is just for containing the export method, which can be
        called without having to instantiate the Contents class */
    public class Contents
    {
        /*  The export method takes a directory name and a desired ordering,
            both as strings */
        static public void export(string parentDir, string ordering)
        {
            /*  This creates a new ContentList object named thisDir, containing
                contents of the passed directory */
            ContentList thisDir = new ContentList(parentDir);
            /*  This sorts the directory contents according to the desired
                ordering */
            thisDir.sortContents(ordering);
            //  This prints the ordered list of directory contents
            // toFile.exportListToFile(thisDir);
            thisDir.exportListToFile();
            /*  This recursively calls export on all the subfolders in the
                directory, and will continue until it bottoms out at a directory
                that doesn't have any subfolders */
            foreach(DirectoryInfo subfolder in thisDir.dir.GetDirectories()){
                export(subfolder.FullName, ordering);
            }
        }
    }

    /*  This class is used for dealing with the contents of a directory. It
        contains properties relating to the directory itself, as well as file
        lists and folder lists. It also provides methods for sorting and
        printing the contents of a directory  */
    class ContentList
    {
        /*  This is a struct for representing the properties of each item in a
            directory, be they files or folders. Storing the data this way
            allows for sorting with the sortContents() method */
        public struct ItemTraits
        {
            // The item's name
            public string itemName;
            // The item's size
            public long itemSize;
            // Instantiate the struct with the properties of a given item
            public ItemTraits(string name, long size)
            {
                itemName = name;
                itemSize = size; 
            }
        }
        /*  This is a struct for representing the size attributes of a given
            item, be it a file or folder. Storing the data this way allows for
            giving each item size a corresponding unit.  */
        public struct SizeTraits
        {
            // The item size
            public float size;
            // The unit for the item size (bytes, kilobytes, megabytes)
            public string unit;
            // Instantiate the struct with a given item size
            public SizeTraits(float itemSize)
            {
                size = itemSize;
                unit = "B"; // default unit for size
            }
        }
        // Declare an object containing information about the directory
        public DirectoryInfo dir {get; set;}
        // Declare a list of files and a list of folders
        public List<ItemTraits> folderList {get; set;}
        public List<ItemTraits> fileList {get; set;}
        /*  Constructor method for the ContentList class. It initialises the
            object properties by calling either library or custom methods */
        public ContentList (string original_dir)
        {
            // Instantiates the DirectoryInfo library class
            this.dir = new DirectoryInfo(original_dir);
            /*  Calls the makeList() custom method to generate either a
                list of files or folders */
            this.folderList = this.makeList("folders");
            this.fileList = this.makeList("files");
        }
        /*  This method sorts the contents of the folderList and fileList
            properties for the directory. When passed an appropriate ordering
            option, it will sort the contents into that order using C#'s
            in-built list sorting. Prints an error message and quits the program
            if an incorrect option is passed. */
        public void sortContents(string ordering)
        {
            if (ordering == "-small-first"){
                this.folderList.Sort((folder1, folder2) => folder1.itemSize.CompareTo(folder2.itemSize));
                this.fileList.Sort((file1, file2) => file1.itemSize.CompareTo(file2.itemSize));
            }
            else if (ordering == "-big-first"){
                this.folderList.Sort((folder1, folder2) => folder2.itemSize.CompareTo(folder1.itemSize));
                this.fileList.Sort((file1, file2) => file2.itemSize.CompareTo(file1.itemSize));
            }
            else if (ordering == "-forward-alpha"){
                this.folderList.Sort((folder1, folder2) => folder1.itemName.CompareTo(folder2.itemName));
                this.fileList.Sort((file1, file2) => file1.itemName.CompareTo(file2.itemName));
            }
            else if (ordering == "-backward-alpha"){
                this.folderList.Sort((folder1, folder2) => folder2.itemName.CompareTo(folder1.itemName));
                this.fileList.Sort((file1, file2) => file2.itemName.CompareTo(file1.itemName));
            }
            else{
                Console.WriteLine("\nERROR. The ordering you requested isn't valid. " +
                                    "Please consult the readme for a list of available ordering options.");
                System.Environment.Exit(-1);
            }
        }
        /*  This method populates the fileList and folderList lists. It uses
            the in-built GetDirectories() and GetFiles() methods, and calls
            the custom getFolderSize() method to calculate the size of folders
            and subfolders in the directory. */
        List<ItemTraits> makeList(string listType)
        {
            List<ItemTraits> traitList = new List<ItemTraits>();
            if (listType == "folders"){
                foreach (DirectoryInfo folder in this.dir.GetDirectories()){
                    ItemTraits thisFolder = new ItemTraits(folder.Name, getFolderSize(folder));
                    traitList.Add(thisFolder);
                }
            }
            else if (listType == "files"){
                foreach (FileInfo file in this.dir.GetFiles()){
                    ItemTraits thisFile = new ItemTraits(file.Name, file.Length);
                    traitList.Add(thisFile);
                }
            }
            else{
                Console.WriteLine("An invalid list type was requested");
            }
            return traitList;
        }
        /*  This method calculates the size of a folder by summing the sizes
            of all files in the folder and constituent subfolders. */
        long getFolderSize(DirectoryInfo thisFolder)
        {
            FileInfo[] subfolderFiles = thisFolder.GetFiles("*", SearchOption.AllDirectories);
            long thisFolderSize = 0;
            foreach (FileInfo file in subfolderFiles)
                thisFolderSize += file.Length;
            return thisFolderSize;
        }
        public void exportListToFile()
        {
            StreamWriter outputFile = new StreamWriter(@".\directory contents.txt", true);
            string listHead = String.Format("The contents of {0} are:\r\n",this.dir.FullName);
            outputFile.WriteLine(listHead);
            StringBuilder itemListing = new StringBuilder();
            foreach (ItemTraits folder in this.folderList){
                SizeTraits folderSize = getSizeTraits((float) folder.itemSize);
                itemListing.AppendFormat(   "{0}{1}{2}{3}",
                                            "".PadLeft(4),
                                            "Folder".PadRight(10),
                                            this.sliceString(folder.itemName, 40),
                                            String.Concat(  folderSize.size.ToString("0.## ") +
                                                            folderSize.unit).PadLeft(13));
                outputFile.WriteLine(itemListing);
                itemListing.Clear();
            }
            foreach (ItemTraits file in this.fileList){
                SizeTraits fileSize = getSizeTraits((float) file.itemSize);
                itemListing.AppendFormat(   "{0}{1}{2}{3}",
                                            "".PadLeft(4),
                                            "File".PadRight(10),
                                            this.sliceString(file.itemName, 40),
                                            String.Concat(  fileSize.size.ToString("0.## ") +
                                                            fileSize.unit).PadLeft(13));
                outputFile.WriteLine(itemListing);
                itemListing.Clear();
            }
            long dirSize = getFolderSize(this.dir);
            SizeTraits dirSizeTraits = getSizeTraits((float) dirSize);
            string dirSizeListing = String.Format(  "Total directory size = {0:0.00} {1}",
                                                    dirSizeTraits.size,
                                                    dirSizeTraits.unit);
            string listTail = String.Format("\r\n{0}\r\n", dirSizeListing.PadLeft(71));
            outputFile.WriteLine(listTail);
            outputFile.Close();
        }
        string sliceString(string originalName, int fieldWidth)
        {
            if (originalName.Length <= fieldWidth){
                return originalName.PadRight(fieldWidth + 4);
            }
            else{
                return String.Format(   "{0}{1}{2}{3}",
                                        originalName.Substring(0,fieldWidth),
                                        "\r\n", "".PadLeft(14),
                                        sliceString(originalName.Substring(fieldWidth),
                                        fieldWidth).PadRight(fieldWidth + 4)
                                        );
            }
        }
        SizeTraits getSizeTraits(float originalSize)
        {
            SizeTraits result = new SizeTraits();

            result.size = originalSize;
            result.unit = "B";
            if (originalSize / 1E9 >= 1){
                result.size = originalSize / (float) 1E9;
                result.unit = "GB";
            }
            else if (originalSize / 1E6 >= 1){
                result.size = originalSize / (float) 1E6;
                result.unit = "MB";
            }
            else if (originalSize / 1E3 >= 1){
                result.size = originalSize / (float) 1E3;
                result.unit = "KB";
            }
            return result;
        }
    }
}