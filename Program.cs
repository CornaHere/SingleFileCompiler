// Copyright (C) 2023 Corna

using System.IO;
using System.CommandLine;
using System.Diagnostics;
using System.Threading.Tasks;

namespace SingleFileCompiler
{
	internal class Program
	{
		static async Task<int> Main(string[] args)
		{
			var rootCmd = new RootCommand("A simple helper for compiling single c sharp source file.");

			var pathArg = new Argument<string>("Path", "The path of the source file.");
            var nameOpt = new Option<string?>("--class", "The class of the Main function. You shoud input as *namespace*.*class*");
			rootCmd.AddArgument(pathArg);
			rootCmd.AddOption(nameOpt);
			rootCmd.SetHandler(
				(path, name) =>
				{
					GenerateCsproj(path, name);
					CompileCsproj();
					CleanIntermediateFile();
				},
				pathArg,
				nameOpt!
			);

			return await rootCmd.InvokeAsync(args);
		}

		static void GenerateCsproj(string path, string? name)
		{
			StreamWriter writer = new StreamWriter("temporary.csproj");
			writer.WriteLine($"<Project Sdk=\"Microsoft.NET.Sdk\"><PropertyGroup><OutputType>Exe</OutputType><TargetFramework>net7.0</TargetFramework><ImplicitUsings>disable</ImplicitUsings><EnableDefaultCompileItems>false</EnableDefaultCompileItems><GenerateFullPaths>true</GenerateFullPaths><Nullable>enable</Nullable><StartupObject>{(name == null ? Path.GetFileNameWithoutExtension(path) + ".Program" : name)}</StartupObject><AssemblyName>{Path.GetFileNameWithoutExtension(path)}</AssemblyName></PropertyGroup><ItemGroup><Compile Include=\"{path}\" /></ItemGroup></Project>");
			writer.Close();
		}

		static void CompileCsproj()
		{
			ProcessStartInfo startInfo = new ProcessStartInfo("dotnet", "build temporary.csproj");
			Process compile = new Process();
			compile.StartInfo = startInfo;
			compile.Start();
			compile.WaitForExit();
		}

		static void CleanIntermediateFile()
		{
			DirectoryInfo obj = new DirectoryInfo("obj");
			if (obj.Exists)
			{
				DirectoryInfo[] dirs = obj.GetDirectories();
				foreach (DirectoryInfo dir in dirs)
				{
					dir.Delete(true);
				}
				obj.Delete(true);
			}
		}
	}
}