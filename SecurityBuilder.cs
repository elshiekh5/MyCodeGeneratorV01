using System;
using System.Text;
using System.IO;
using System.Windows.Forms;
namespace SPGen
{
	/// <summary>
	/// Summary description for SecurityBuilder.
	/// </summary>
	public class SecurityBuilder
	{
		public static void Create()
		{
			Globals global=new Globals();
			SecurityBuilder sb=new SecurityBuilder();
			sb.ExecuteSqlInFile(global.SecuritySQLFile);
		}

		public  bool ExecuteSqlInFile(string pathToScriptFile ) 
		{
			try 
			{
				StreamReader _reader= null;

				string sql	= "";

				if( false == System.IO.File.Exists( pathToScriptFile )) 
				{
					throw new Exception("File " + pathToScriptFile + " does not exists");
				}
				using( Stream stream = System.IO.File.OpenRead( pathToScriptFile ) ) 
				{
					_reader = new StreamReader( stream );

					while( null != (sql = ReadNextStatementFromStream( _reader ) )) 
					{
						SqlProvider.obj.ExecuteNonQuery(sql);
					}

					_reader.Close();
				}

				return true;
			}
			catch(Exception ex) 
			{
				MessageBox.Show(ex.Message);
				return false;
			}


		}
		private  string ReadNextStatementFromStream( StreamReader _reader ) 
		{
			try 
			{
				StringBuilder sb = new StringBuilder();

				string lineOfText;
	
				while(true) 
				{
					lineOfText = _reader.ReadLine();
					if( lineOfText == null ) 
					{

						if( sb.Length > 0 ) 
						{
							return sb.ToString();
						}
						else 
						{
							return null;
						}
					}

					if( lineOfText.TrimEnd().ToUpper() == "GO" ) 
					{
						break;
					}
				
					sb.Append(lineOfText + Environment.NewLine);
				}

				return sb.ToString();
			}
			catch( Exception ex ) 
			{
				MessageBox.Show(ex.Message);
				return null;
			}
		}

	}
}
