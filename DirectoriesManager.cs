using System;
using System.Text;
using System.IO;

namespace SPGen
{
	/// <summary>
	/// xDirectory Contains a Method xDirectory.Copy() that takes a source folder and copies it,
	///   it's subdirectories and it's files to a destination folder. This is functionality that is
	///   not included with the .NET Framework.
	/// </summary>
	/// <remarks>Created By John Storer II</remarks>
	public class DirectoriesManager
	{
		/// <summary>
		/// Copy a Directory, All SubDirectories and All Files Given a Source and Destination Path.
		/// </summary>
		/// <param name="SourcePath">Format: "drive:\directory\subdirectory" or "\\share\folder"</param>
		/// <param name="DestinationPath">Format: "drive:\directory\subdirectory" or "\\share\folder"</param>
		/// <param name="Overwrite">Whether or not to Overwrite Copied Files in the Destination Directory</param>
		public static void Copy(string SourcePath, string DestinationPath, bool Overwrite)
		{
			Copy(new DirectoryInfo(SourcePath.Trim()), new DirectoryInfo(DestinationPath.Trim()), null, null, Overwrite);
		}

		/// <summary>
		/// Copy a Directory, All SubDirectories and Files Given a Source and Destination Path, Given a File Filter.
		/// IMPORTANT: The search strings for Files applies to every File within the Source Directory.
		/// </summary>
		/// <param name="SourcePath">Format: "drive:\directory\subdirectory" or "\\share\folder"</param>
		/// <param name="DestinationPath">Format: "drive:\directory\subdirectory" or "\\share\folder"</param>
		/// <param name="SourceFileFilter">File Filter: Standard DOS-Style Format (Examples: "*.txt" or "*.exe")</param>
		/// <param name="Overwrite">Whether or not to Overwrite Copied Files in the Destination Directory</param>
		public static void Copy(string SourcePath, string DestinationPath, string SourceFileFilter, bool Overwrite)
		{
			Copy(new DirectoryInfo(SourcePath.Trim()), new DirectoryInfo(DestinationPath.Trim()), null, SourceFileFilter, Overwrite);
		}

		/// <summary>
		/// Copy a Directory, SubDirectories and Files Given a Source and Destination Path, Given a SubDirectory Filter and a File Filter.
		/// IMPORTANT: The search strings for SubDirectories and Files applies to every Folder and File within the Source Directory.
		/// </summary>
		/// <param name="SourcePath">Format: "drive:\directory\subdirectory" or "\\share\folder"</param>
		/// <param name="DestinationPath">Format: "drive:\directory\subdirectory" or "\\share\folder"</param>
		/// <param name="SourceDirectoryFilter">Search string on SubDirectories (Example: "System*" will return all subdirectories starting with "System" or "*S*" will return all subdirectories containing an "S")</param>
		/// <param name="SourceFileFilter">File Filter: Standard DOS-Style Format (Examples: "*.txt" or "*.exe")</param>
		/// <param name="Overwrite">Whether or not to Overwrite Copied Files in the Destination Directory</param>
		public static void Copy(string SourcePath, string DestinationPath, string SourceDirectoryFilter, string SourceFileFilter, bool Overwrite)
		{
			Copy(new DirectoryInfo(SourcePath.Trim()), new DirectoryInfo(DestinationPath.Trim()), SourceDirectoryFilter, SourceFileFilter, Overwrite);
		}

		/// <summary>
		/// Copy a Directory, All SubDirectories and All Files Given a Source and Destination DirectoryInfo Object.
		/// </summary>
		/// <param name="SourceDirectory">A DirectoryInfo Object Pointing to the Source Directory</param>
		/// <param name="DestinationDirectory">A DirectoryInfo Object Pointing to the Destination Directory</param>
		/// <param name="Overwrite">Whether or not to Overwrite Copied Files in the Destination Directory</param>
		public static void Copy(DirectoryInfo SourceDirectory, DirectoryInfo DestinationDirectory, bool Overwrite)
		{
			Copy(SourceDirectory, DestinationDirectory, null, null, Overwrite);
		}

		/// <summary>
		/// Copy a Directory, All SubDirectories and Files Given a Source and Destination DirectoryInfo Object, Given a File Filter.
		/// IMPORTANT: The search strings for Files applies to every File within the Source Directory.
		/// </summary>
		/// <param name="SourceDirectory">A DirectoryInfo Object Pointing to the Source Directory</param>
		/// <param name="DestinationDirectory">A DirectoryInfo Object Pointing to the Destination Directory</param>
		/// <param name="SourceFileFilter">File Filter: Standard DOS-Style Format (Examples: "*.txt" or "*.exe")</param>
		/// <param name="Overwrite">Whether or not to Overwrite Copied Files in the Destination Directory</param>
		public static void Copy(DirectoryInfo SourceDirectory, DirectoryInfo DestinationDirectory, string SourceFileFilter, bool Overwrite)
		{
			Copy(SourceDirectory, DestinationDirectory, null, SourceFileFilter, Overwrite);
		}

		/// <summary>
		/// Copy a Directory, SubDirectories and Files Given a Source and Destination DirectoryInfo Object, Given a SubDirectory Filter and a File Filter.
		/// IMPORTANT: The search strings for SubDirectories and Files applies to every Folder and File within the Source Directory.
		/// </summary>
		/// <param name="SourceDirectory">A DirectoryInfo Object Pointing to the Source Directory</param>
		/// <param name="DestinationDirectory">A DirectoryInfo Object Pointing to the Destination Directory</param>
		/// <param name="SourceDirectoryFilter">Search string on SubDirectories (Example: "System*" will return all subdirectories starting with "System" or "*S*" will return all subdirectories containing an "S")</param>
		/// <param name="SourceFileFilter">File Filter: Standard DOS-Style Format (Examples: "*.txt" or "*.exe")</param>
		/// <param name="Overwrite">Whether or not to Overwrite Copied Files in the Destination Directory</param>
		public static void Copy(DirectoryInfo SourceDirectory, DirectoryInfo DestinationDirectory, string SourceDirectoryFilter, string SourceFileFilter, bool Overwrite)
		{
			DirectoryInfo[] SourceSubDirectories;
			FileInfo[] SourceFiles;

			//Check for File Filter
			if (SourceFileFilter != null)
				SourceFiles = SourceDirectory.GetFiles(SourceFileFilter.Trim());
			else
				SourceFiles = SourceDirectory.GetFiles();

			//Check for Folder Filter
			if (SourceDirectoryFilter != null)
				SourceSubDirectories = SourceDirectory.GetDirectories(SourceDirectoryFilter.Trim());
			else
				SourceSubDirectories = SourceDirectory.GetDirectories();

			//Create the Destination Directory
			if (!DestinationDirectory.Exists) DestinationDirectory.Create();
			
			//Recursively Copy Every SubDirectory and it's Contents (according to folder filter)
			foreach (DirectoryInfo SourceSubDirectory in SourceSubDirectories)
				Copy(SourceSubDirectory, new DirectoryInfo(DestinationDirectory.FullName + @"\" + SourceSubDirectory.Name), SourceDirectoryFilter, SourceFileFilter, Overwrite);

			//Copy Every File to Destination Directory (according to file filter)
			foreach (FileInfo SourceFile in SourceFiles)
			{
				if(SourceFile.Extension==".cs"||
					SourceFile.Extension==".aspx"||
					SourceFile.Extension==".ascx"||
					SourceFile.Extension==".aspx"||
					SourceFile.Extension==".asax"||
					SourceFile.Extension==".css"||
					//
					SourceFile.Extension==".sln"||
                    SourceFile.Extension == ".webinfo"||
                    SourceFile.Extension == ".master"
				
					)
				{
					FileManager.CopyProjectFile(DestinationDirectory,SourceFile);
				}
				else
					SourceFile.CopyTo(DestinationDirectory.FullName + @"\" + SourceFile.Name, Overwrite);
				
			}
		}	
		//--------------------------------------------
		public static void ChechDirectory(string path)
		{
			if(!Directory.Exists(path))
				Directory.CreateDirectory(path);
		}
		
	}
}
