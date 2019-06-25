using System;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.IO;
using System.Windows.Forms;
using System.Collections;
using System.Collections.Specialized;
namespace SPGen
{
	public enum TextType
	{
		Text,
		HtmlClassic,
        ServerControl
	}
	public class ResourcesTesxtsBuilder
	{
		
		//private static Hashtable		AdminMenuTexts = new Hashtable();
        //-------------------------------------
        private static Hashtable TablesResources = new Hashtable();

        
        #region Create
		public static void Create()
		{
			//CreateAdminLanguageArXmlConfig();
			//CreateAdminLanguageEnXmlConfig();
			CreateUserLanguageArXmlConfig();
			CreateUserLanguageEnXmlConfig();
		}
        

        #endregion
        //---------------------------------------------------
        /*#region CreateAdminMenueArXmlConfig
        public static void CreateAdminMenueArXmlConfig()
        {
            //-----------------------------------
            string configFile = "ProjectFiles/AdminMenu.ar.resx";
            StreamReader _reader = null;
            string lineOfText;
            StringBuilder sb = new StringBuilder();
            if (false == System.IO.File.Exists(configFile))
            {
                throw new Exception("File " + configFile + " does not exists");
            }
            string xmlData = "";
            string recourcesfile ;
            string xmlTags ;
            string path = Globals.AdminLanguagesDirectory + "AdminMenu.ar.resx";
             using (Stream stream = System.IO.File.OpenRead(configFile))
            {
                _reader = new StreamReader(stream);
                while (true)
                {
                    lineOfText = _reader.ReadLine();
                    if (lineOfText == null)
                    {
                        xmlData = sb.ToString();
                        //--------------------------------
                        xmlTags = CreateAdminMenuTextsXmlTags();
                        recourcesfile = xmlData.Replace("{R}", xmlTags);
                        //Ar File
                        // Create a file to write to.
                        using (StreamWriter sw = File.CreateText(path))
                        {
                            sw.WriteLine(recourcesfile);
                        }
                        return;
                        //-----------------------------------
                    }
                    else
                        sb.Append(lineOfText + Environment.NewLine);
                }
            }
           
        }
        #endregion*/
        //---------------------------------------------------
        //---------------------------------------------------
      /* /* #region CreateAdminMenueEnXmlConfig
        public static void CreateAdminMenueEnXmlConfig()
        {
            //-----------------------------------
            string configFile = "ProjectFiles/AdminMenu.resx";
            StreamReader _reader = null;
            string lineOfText;
            StringBuilder sb = new StringBuilder();
            if (false == System.IO.File.Exists(configFile))
            {
                throw new Exception("File " + configFile + " does not exists");
            }
            string xmlData = "";
            string recourcesfile;
            string xmlTags;
            string enPath = Globals.AdminLanguagesDirectory + "AdminMenu.resx";
            using (Stream stream = System.IO.File.OpenRead(configFile))
            {
                _reader = new StreamReader(stream);
                while (true)
                {
                    lineOfText = _reader.ReadLine();
                    if (lineOfText == null)
                    {
                        xmlData = sb.ToString();
                        //--------------------------------
                        xmlTags = CreateAdminMenuTextsXmlTags();
                        recourcesfile = xmlData.Replace("{R}", xmlTags);
                        //En File
                        // Create a file to write to.
                        using (StreamWriter sw = File.CreateText(enPath))
                        {
                            sw.WriteLine(recourcesfile);
                        }
                        return;
                        //-----------------------------------
                    }
                    else
                        sb.Append(lineOfText + Environment.NewLine);
                }
            }

        }
        #endregion*/
        //---------------------------------------------------
       /* #region CreateAdminLanguageArXmlConfig
        public static void CreateAdminLanguageArXmlConfig()
		{
			//-----------------------------------
			string configFile = "ProjectFiles/AdminTextsAr.resx";
			StreamReader _reader = null;
			string lineOfText;
			StringBuilder sb = new StringBuilder();
			if (false == System.IO.File.Exists(configFile))
			{
				throw new Exception("File " + configFile + " does not exists");
			}
            string xmlData = "";
            string recourcesfile = "";
            string directoryPath = "";
            string path = "";
			using (Stream stream = System.IO.File.OpenRead(configFile))
			{
				_reader = new StreamReader(stream);
				while (true)
				{
					lineOfText = _reader.ReadLine();
					if (lineOfText == null)
					{
                        xmlData = sb.ToString();
                        TableResource resource;
                        string xmlTags;
                        foreach (DictionaryEntry key in TablesResources)
                        {
                            resource = (TableResource)key.Value;
                            xmlTags = resource.GetAdminTags();
                            recourcesfile = xmlData.Replace("{R}", xmlTags);
                            //Ar File
                            //Directory Check
                            directoryPath = Globals.ModulesGlobalResources + resource.Key + "\\";
                            if (!Directory.Exists(directoryPath))
                                Directory.CreateDirectory(directoryPath);
                            //------------------------
                            // Create a file to write to.
                            path = directoryPath + resource.Key + "Admin.ar.resx";
                            //------------------------
                           
                            // Create a file to write to.
                            using (StreamWriter sw = File.CreateText(path))
                            {
                                sw.WriteLine(recourcesfile);
                            }
                        }
						return;
						//-----------------------------------
					}
					else
						sb.Append(lineOfText + Environment.NewLine);
				}
               
			}
		}
        #endregion*/
     /*   #region CreateAdminLanguageEnXmlConfig
        //-------------------------------------
        public static void CreateAdminLanguageEnXmlConfig()
        {
            //-----------------------------------
            string configFile = "ProjectFiles/AdminTextsEn.resx";
            StreamReader _reader = null;
            string lineOfText;
            StringBuilder sb = new StringBuilder();
            if (false == System.IO.File.Exists(configFile))
            {
                throw new Exception("File " + configFile + " does not exists");
            }
            string xmlData = "";
            string recourcesfile = "";
            string directoryPath = "";
            string path = "";
            using (Stream stream = System.IO.File.OpenRead(configFile))
            {
                _reader = new StreamReader(stream);
                while (true)
                {
                    lineOfText = _reader.ReadLine();
                    if (lineOfText == null)
                    {
                        xmlData = sb.ToString();
                        TableResource resource;
                        string xmlTags;
                        foreach (DictionaryEntry key in TablesResources)
                        {
                            resource = (TableResource)key.Value;
                            xmlTags = resource.GetAdminTags();
                            recourcesfile = xmlData.Replace("{R}", xmlTags);
                            //Directory Check
                            directoryPath = Globals.ModulesGlobalResources + resource.Key + "\\";
                            if (!Directory.Exists(directoryPath))
                                Directory.CreateDirectory(directoryPath);
                            //------------------------
                            // Create a file to write to.
                            path = directoryPath + resource.Key + "Admin.resx";
                            //------------------------

                            using (StreamWriter sw = File.CreateText(path))
                            {
                                sw.WriteLine(recourcesfile);
                            }
                        }
                        return;
                        //-----------------------------------
                    }
                    else
                        sb.Append(lineOfText + Environment.NewLine);
                }
               
            }
        }
#endregion*/
        #region CreateUserLanguageArXmlConfig

        //-------------------------------------
		public static void CreateUserLanguageArXmlConfig()
		{
            //-----------------------------------
            string configFile = "ProjectFiles/UserTextsAr.resx";
            StreamReader _reader = null;
            string lineOfText;
            StringBuilder sb = new StringBuilder();
            if (false == System.IO.File.Exists(configFile))
            {
                throw new Exception("File " + configFile + " does not exists");
            }
            string xmlData = "";
            string recourcesfile = "";
            string directoryPath = "";
            string path = "";
            using (Stream stream = System.IO.File.OpenRead(configFile))
            {
                _reader = new StreamReader(stream);
                while (true)
                {
                    lineOfText = _reader.ReadLine();
                    if (lineOfText == null)
                    {
                        xmlData = sb.ToString();
                        TableResource resource;
                        string xmlTags;
                        foreach (DictionaryEntry key in TablesResources)
                        {
                            resource = (TableResource)key.Value;
                            xmlTags = resource.GetUserTags();
                            recourcesfile = xmlData.Replace("{R}", xmlTags);
                            //Directory Check
                            directoryPath = Globals.ModulesGlobalResources + resource.Key + "\\";
                            if (!Directory.Exists(directoryPath))
                                Directory.CreateDirectory(directoryPath);
                            //------------------------
                            // Create a file to write to.
                            path = directoryPath + resource.Key + ".ar.resx";
                            //------------------------
                            using (StreamWriter sw = File.CreateText(path))
                            {
                                sw.WriteLine(recourcesfile);
                            }
                        }
                        return;
                        //-----------------------------------
                    }
                    else
                        sb.Append(lineOfText + Environment.NewLine);
                }
            }
		}
        //-------------------------------------
#endregion
        #region CreateUserLanguageEnXmlConfig
        public static void CreateUserLanguageEnXmlConfig()
        {
            //-----------------------------------
            string configFile = "ProjectFiles/UserTextsEn.resx";
            StreamReader _reader = null;
            string lineOfText;
            StringBuilder sb = new StringBuilder();
            if (false == System.IO.File.Exists(configFile))
            {
                throw new Exception("File " + configFile + " does not exists");
            }
            string xmlData = "";
            string recourcesfile = "";
            string directoryPath = "";
            string path = "";
            using (Stream stream = System.IO.File.OpenRead(configFile))
            {
                _reader = new StreamReader(stream);
                while (true)
                {
                    lineOfText = _reader.ReadLine();
                    if (lineOfText == null)
                    {
                        xmlData = sb.ToString();
                        TableResource resource;
                        string xmlTags;
                        foreach (DictionaryEntry key in TablesResources)
                        {
                            resource = (TableResource)key.Value;
                            xmlTags = resource.GetUserTags();
                            recourcesfile = xmlData.Replace("{R}", xmlTags);
                            //Directory Check
                            directoryPath = Globals.ModulesGlobalResources + resource.Key + "\\";
                            if (!Directory.Exists(directoryPath))
                                Directory.CreateDirectory(directoryPath);
                            //------------------------
                            // Create a file to write to.
                            path = directoryPath + resource.Key + ".resx";
                            //------------------------
                            using (StreamWriter sw = File.CreateText(path))
                            {
                                sw.WriteLine(recourcesfile);
                            }
                        }
                        return;
                        //-----------------------------------
                    }
                    else
                        sb.Append(lineOfText + Environment.NewLine);
                }
                
            }
        }
        #endregion
       /* #region AddAdminText
        //-------------------------------------
        public static string AddAdminText(string text, TextType type)
        {
            return AddAdminText(SqlProvider.obj.TableName, text, type);
        }
        //-------------------------------------
        public static string AddAdminText(string tableId, string text, TextType type)
		{
            string key = Globals.GetProgramatlyName(text);
            TableResource recource ;
            if (!TablesResources.Contains(tableId))
            {
                recource = new TableResource(tableId);
                recource.Key = tableId;
                TablesResources.Add(tableId,recource);
            }
            else
            {
                recource = (TableResource) TablesResources[tableId];
            }
            recource.AddUserText(key, text);
            //-------------------------
            if (type == TextType.HtmlClassic)
            {
                if (ProjectBuilder.IsLabelText)
                    return "<asp:Label id=\"lbl" + key + "\" Text=\"<%$ Resources:" + tableId + "Admin," + key + " %>\" runat=\"server\" />";
                else
                    return "<%= Resources." + tableId + "Admin." + key + " %>";
            }
            else if (type == TextType.ServerControl)
            {

                return "<%$ Resources:" + tableId + "Admin," + key + " %>";
            }
            else
            {
                return "Resources." + tableId + "Admin." + key;
            }
		}
        #endregion*/
        #region AddAdminGlobalText
        //-------------------------------------
		public static string AddAdminGlobalText(string text, TextType type)
		{
			if (type == TextType.HtmlClassic)
			{
                return "<%= Resources.Admin." + text + " %>";
			}
            else if (type == TextType.ServerControl)
            {
                return "<%$ Resources:Admin," + text + " %>";
            }
			else
			{
                return "Resources.Admin." + text;
			}

		}
		//-------------------------------------
        #endregion
       /* #region AddAdminMenuText
        public static string AddAdminMenuText(string text, TextType type)
		{
            string key = Globals.GetProgramatlyName(text);

            if (!AdminMenuTexts.Contains(key))
			{
				AdminMenuTexts.Add(key, text);
			}
			if (type == TextType.HtmlClassic)
			{
                return "<%= Resources.AdminMenu." + key + " %>";
			}
			else
			{
                return "Resources.AdminMenu." + key;
			}
		}
        #endregion*/
        #region AddUserText
        //-------------------------------------
        public static string AddUserText(string text, TextType type)
        {
            return AddUserText(SqlProvider.obj.TableName, text, type);
        }
        //-------------------------------------
        public static string AddUserText(string tableId, string text, TextType type)
		{
            string key=Globals.GetProgramatlyName(text);
            TableResource recource;
            if (!TablesResources.Contains(tableId))
            {
                recource = new TableResource(tableId);
                TablesResources.Add(tableId, recource);
            }
            else
            {
                recource = (TableResource) TablesResources[tableId];
            }
            recource.AddUserText(key, text);
            //-------------------------
            if (type == TextType.HtmlClassic)
            {
                if (ProjectBuilder.IsLabelText)
                    return "<asp:Label Text=\"<%$ Resources:" + tableId + "," + key + " %>\" runat=\"server\" />";
                return "<%= Resources." + tableId + "." + key + " %>";
            }
            else if (type == TextType.ServerControl)
            {
                return "<%$ Resources:" + tableId + "," + text + " %>";
            }
            else
            {
                return "Resources." + tableId + "." + key;
            }

		}
        #endregion
        /*#region CreateAdminMenuTextsXmlTags
        //-------------------------------------
        
		private static string CreateAdminMenuTextsXmlTags()
		{
			StringBuilder textsxml = new StringBuilder();
			
			foreach (DictionaryEntry key in AdminMenuTexts)
			{
                textsxml.Append(TableResource.CreateTextXmlNode(key.Key.ToString(), key.Value.ToString()));
			}
			
			return textsxml.ToString();
		}
		#endregion
		*/
		

    }
}

