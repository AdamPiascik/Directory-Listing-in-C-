using System.IO;
using System;
using Dir = DirectoryMethods;

namespace ListApp
{
    // This class is just to store the Main() method
    class MainProgram
    {
        /*  Executes the program with a set of optional command-line arguments.
            The program exports the contents of a given directory in a given
            order by calling the Contents.export() method within the
            DirectoryFunctions namespace. This is contained in functions.cs */
        static int Main(string[] args) 
        {
            /*  Create a new text file for the listing results. If the file
                already exists, it overwrites the contents. */
            File.WriteAllText(@".\directory contents.txt", String.Empty);
            /*  If no arguments are supplied, default to listing the drive
                contents in order of decreasing size. */
            if (args.Length == 0)
                Dir.Contents.export(@"C:\", "-big-first");
            /*  If only one argument is supplied, check whether to interpret it
                as a directory location or an ordering request. Use the default
                value for the non-specified argument. */
            else if (args.Length == 1){
                if (args[0].Substring(0, 1) == "-")
                    Dir.Contents.export(@"C:\", args[0]);
                else
                    Dir.Contents.export(args[0], "-big-first");
            }
            /*  If two or more arguments are supplied, assume the first is a
                directory location, and the second is an ordering request.
                Ignore the remaining arguments. */
            else
                Dir.Contents.export(args[0], args[1]);
            return 0;
        }
    }
}


