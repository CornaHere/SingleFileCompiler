# Single File Compiler

A simple helper for compiling single c sharp source file.

## Usage

`sfc <path> [--class *classname*]`

path: The path of the source file that you want to compile.  
classname: The class of the Main function. You should input it as Namespace.Class.

## Example

`sfc C:\example.cs --class Example.Program`

This command will compile the example.cs and its entrance funtion is `Example.Program.Main`.