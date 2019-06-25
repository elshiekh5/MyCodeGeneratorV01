using System;
using System.IO;
using System.Text;
namespace SPGen
{
	/// <summary>
	/// Summary description for FileManager.
	/// </summary>
	public class FileManager
	{
		//Copy File and set the Namespace or / Project name
		public static void CopyProjectFile(DirectoryInfo DestinationDirectory,FileInfo SourceFile)
		{
			
			StreamReader _reader			= null;
			
			string lineOfText;
			StringBuilder sb = new StringBuilder();
			using( Stream stream = SourceFile.OpenRead() ) 
			{
				Globals global=new Globals();
				_reader = new StreamReader( stream ,Encoding.GetEncoding(global.ProjectEncoding));
				while(true) 
				{
					lineOfText = _reader.ReadLine();
					if( lineOfText == null ) 
					{
						string _class=sb.ToString();

						_class=_class.Replace("{0}",ProjectBuilder.ProjectName);
						//-----------------------------------
						string path = DestinationDirectory.FullName + @"\" + SourceFile.Name;
						// Create a file to write to.
						
						FileStream fs = new FileStream(path,FileMode.CreateNew, FileAccess.Write);

						using (StreamWriter sw = new StreamWriter(fs,Encoding.GetEncoding(global.ProjectEncoding)) )
						{
							sw.WriteLine(_class);				
						}     
						return;
						//-----------------------------------
					}
					else
						sb.Append(lineOfText + Environment.NewLine);
				}
				
				
			}
		}

		//------------------------------------------------------------
		public static void CopyFile(string filePath,string newFilePath)
		{
			
			StreamReader _reader			= null;
			string lineOfText;
			StringBuilder sb = new StringBuilder();
			if( false == System.IO.File.Exists( filePath )) 
			{
				throw new Exception("File " + filePath + " does not exists");
			}
			using( Stream stream = System.IO.File.OpenRead( filePath ) ) 
			{
				_reader = new StreamReader( stream );
				while(true) 
				{
					lineOfText = _reader.ReadLine();
					if( lineOfText == null ) 
					{
						string _class=sb.ToString();
						_class=_class.Replace("{0}",ProjectBuilder.NameSpace);
						//-----------------------------------
						// Create a file to write to.
						using (StreamWriter sw = File.CreateText(newFilePath)) 
						{
							
							sw.WriteLine(_class);				
						}    
						return;
						//-----------------------------------
					}
					else
						sb.Append(lineOfText + Environment.NewLine);
				}
				
				
			}
		
		}
		//-------------------------------------------------------------
		public static void CreateFile(string filePath,string fileContent)
		{
			Globals global=new Globals();
			// Create a file to write to.
			FileStream fs = new FileStream(filePath,FileMode.CreateNew, FileAccess.Write);

			using (StreamWriter sw = new StreamWriter(fs,Encoding.GetEncoding(global.ProjectEncoding)) )
			{
				sw.WriteLine(fileContent);				
			} 	
		}
		//-------------------------------------------------------------
		
	}
}
