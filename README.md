# Readme for listing.exe

This program will list all the contents (folder and files) in a requested directory, including listing the contents
of all subfolders. It prints the results to a text file called "directory contents.txt" within the same directory as
the program executable itself.

## Installation

To compile this program into a usable .exe file, please enter:

    csc -out:dirList.exe DirMethods.cs MainApp.cs

in a command line with access to the C# compiler.

## Usage

The program may be run from the command line, and takes either one, two, or no arguments. If more than two arguments are entered,
the program just ignores anything beyond the first two. The behaviour is as follows:

* If one argument is supplied, the program will check to see whether you have entered a directory location or a sorting option as
an argument. It will use a default to supply the missing argument. The default locatioin is "C:\" and the default sorting option
is "-big-first"

* If two arguments are supplied, the program will assume they have been entered with the directory location first and the sorting option
second. It will throw up an error if you don't conform to this.

* If no arguments are entered, the default directory location ("C:\") and sorting option (-big-first) are used.

The sorting options available are:

* **-big-first:** in order of descending size

* **-small-first:** in order of ascending size

* **-forward-alpha:** in normal alphabetical order

* **-backward-alpha:** in reverse alphabetical order

**Note:** folders are always listed before files.

## Known Issues

There seems to be a permissions issue when accessing some folders, even when the program is run as an admin.
This manifests itself most notably when running the program on the whole drive, in which case many of the folders
seem to be inaccessible and throw up an exception. Upon encountering this, the program will print a message to the console
telling you this has happened, but the contents of that folder won't be written to "directory contents.txt". On my PC,
this will still print the contents of the C drive to the output file, but won't print the contents of any subfolders.
Running the program on more specific directories seems to work well.

## Future Improvements

* Fix the above permissions issue
* Add printing of date information for files and folder
* Add options to sort by date information