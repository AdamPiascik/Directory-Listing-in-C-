# Readme for listing.exe

This program will list all the contents (folder and files) in a requested directory, including listing the contents
of all subfolders. It prints the results to a text file called "directory contents.txt" within the same directory as
the program executable itself.

## Installation

To compile this program into a usable .exe file, please enter:

    csc -out:dirList.exe functions.cs listapp.cs

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