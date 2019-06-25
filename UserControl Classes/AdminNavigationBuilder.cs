using System;
using System.IO;
using System.Text ;
namespace SPGen
{
	/// <summary>
	/// Summary description for AdminNavigationBuilder.
	/// </summary>
	public class AdminNavigationBuilder 
	{
		private static  StringBuilder _NavigationItems=new StringBuilder();
		public static void CreatePartialClass()
		{
			//-----------------------------------
			string configFile = "ProjectFiles/AdminMenu.ascx.cs";
			StreamReader _reader = null;

			string lineOfText;
			StringBuilder sb = new StringBuilder();
			if (false == System.IO.File.Exists(configFile))
			{
				throw new Exception("File " + configFile + " does not exists");
			}
			using (Stream stream = System.IO.File.OpenRead(configFile))
			{
				_reader = new StreamReader(stream);
				while (true)
				{
					lineOfText = _reader.ReadLine();
					if (lineOfText == null)
					{
						string _class = sb.ToString();
                        _class = _class.Replace("{0}", _NavigationItems.ToString());
						//-----------------------------------
						//Ar File
                        string path = Globals.AdminNavigationCodeBehind + "AdminMenu.ascx.cs";
						// Create a file to write to.
						using (StreamWriter sw = File.CreateText(path))
						{
                            sw.WriteLine(_class);
						}
						//
						//-----------------------------------
						
						return;
						//-----------------------------------
					}
					else
						sb.Append(lineOfText + Environment.NewLine);
				}
			}
		}
        //
		public static  void AddItems()
		{
			Globals global=new Globals();
	 		//depend on the current table
			_NavigationItems.Append("\n\t\t\t#region --------------"+global.TableProgramatlyName+"--------------");
            string tabforIf = "";
            if (ProjectBuilder.HasConfiguration)
            {
                _NavigationItems.Append("\n\t\t\tif (SiteOptions.CheckModule(Resources.SiteOptions." + SiteOptionsBuilder.GetHasPropertyString(global.TableProgramatlyName) + ", ViewerType.Admin))");
                _NavigationItems.Append("\n\t\t\t{");
                tabforIf = "\t";
            }
            _NavigationItems.Append("\n\t\t\t" + tabforIf + "//---------------------------------------");
            _NavigationItems.Append("\n\t\t\t" + tabforIf + "//" + global.TableProgramatlyName);
            _NavigationItems.Append("\n\t\t\t" + tabforIf + "NavItem nav" + global.TableProgramatlyName + " = new NavItem(\"" + global.TableProgramatlyName + "\", " + ResourcesTesxtsBuilder.AddUserText(global.TableProgramatlyName+"ModuleTitle", TextType.Text) + ");");
            //_NavigationItems.Append("\n\t\t\t" + tabforIf + "nav" + global.TableProgramatlyName + ".AddLink(" + ResourcesTesxtsBuilder.AddUserText("AddNew" + global.TableProgramatlyName, TextType.Text) + ", \"/Admin/" + global.TableProgramatlyName + "/add.aspx\");");
            //_NavigationItems.Append("\n\t\t\t" + tabforIf + "nav" + global.TableProgramatlyName + ".AddLink(" + ResourcesTesxtsBuilder.AddUserText("ShowAll" + global.TableProgramatlyName, TextType.Text) + ", \"/Admin/" + global.TableProgramatlyName + "/default.aspx\");");
            _NavigationItems.Append("\n\t\t\t" + tabforIf + "nav" + global.TableProgramatlyName + ".AddLink(Resources.Admin.Add ,  \"/Admin/" + global.TableProgramatlyName + "/add.aspx\");");
            _NavigationItems.Append("\n\t\t\t" + tabforIf + "nav" + global.TableProgramatlyName + ".AddLink(Resources.Admin.ViewAll , \"/Admin/" + global.TableProgramatlyName + "/default.aspx\");");

            _NavigationItems.Append("\n\t\t\t" + tabforIf + "menu.Controls.Add(nav" + global.TableProgramatlyName + ");");
            _NavigationItems.Append("\n\t\t\t" + tabforIf + "//--------------------------------------");
            if (ProjectBuilder.HasConfiguration)
            {
                _NavigationItems.Append("\n\t\t\t}");
            }
            _NavigationItems.Append("\n\t\t\t#endregion");
			
		}
	}
}
