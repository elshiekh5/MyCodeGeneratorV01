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
	/// <summary>
	/// Summary description for SiteOptionsBuilder.
	/// </summary>
	public class SiteOptionsBuilder  
	{
        /**************************************************************/
        private static Hashtable TablesOptions = new Hashtable();
        private static Hashtable SiteGlobalOptions = new Hashtable();
        /**************************************************************/
        #region Create
        public static void Create()
        {
            CreateTablesOptionFile();
            CreateGlobalOptionsFile();
        }
        #endregion
        /**************************************************************/
        //---------------------------------------------------
        #region CreateGlobalOptionsFile
        public static void CreateGlobalOptionsFile()
        {
            //-----------------------------------
            string configFile = "ProjectFiles/SiteOptions.resx";
            StreamReader _reader = null;
            string lineOfText;
            StringBuilder sb = new StringBuilder();
            if (false == System.IO.File.Exists(configFile))
            {
                throw new Exception("File " + configFile + " does not exists");
            }
            string xmlData = "";
            string conigurationFile;
            string xmlTags;
            string path = Globals.SiteOptionsDirectory + "SiteOptions.resx";
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
                        xmlTags = CreateSiteOptionsXmlTags();
                        conigurationFile = xmlData.Replace("{R}", xmlTags);
                        //Ar File
                        // Create a file to write to.
                        using (StreamWriter sw = File.CreateText(path))
                        {
                            sw.WriteLine(conigurationFile);
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
        /**************************************************************/
        #region CreateTablesOptionFile
        public static void CreateTablesOptionFile()
        {
            //-----------------------------------
            string configFile = "ProjectFiles/TableOptions.resx";
            StreamReader _reader = null;
            string lineOfText;
            StringBuilder sb = new StringBuilder();
            if (false == System.IO.File.Exists(configFile))
            {
                throw new Exception("File " + configFile + " does not exists");
            }
            string xmlData = "";
            string conigurationFile = "";
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
                        TableOptions coniguration;
                        string xmlTags;
                        foreach (DictionaryEntry key in TablesOptions)
                        {
                            coniguration = (TableOptions)key.Value;
                            xmlTags = coniguration.GetTags();
                            conigurationFile = xmlData.Replace("{R}", xmlTags);
                            //Directory Check
                            directoryPath = Globals.ModulesGlobalResources + coniguration.Key + "\\";
                            if (!Directory.Exists(directoryPath))
                                Directory.CreateDirectory(directoryPath);
                            //------------------------
                            // Create a file to write to.
                            path = directoryPath + coniguration.Key + "Options.resx";
                            // Create a file to write to.
                            using (StreamWriter sw = File.CreateText(path))
                            {
                                sw.WriteLine(conigurationFile);
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
        /**************************************************************/
        #region AddTableOption
        //-------------------------------------
        public static string AddTableOption(string key)
        {
            return AddTableOption(SqlProvider.obj.TableName, key);
        }
        //-------------------------------------
        public static string AddTableOption(string tableId, string key)
        {
            key = Globals.GetProgramatlyName(key);
            TableOptions tableOptions;
            if (!TablesOptions.Contains(tableId))
            {
                tableOptions = new TableOptions(tableId);
                tableOptions.Key = tableId;
                TablesOptions.Add(tableId, tableOptions);
            }
            else
            {
                tableOptions = (TableOptions)TablesOptions[tableId];
            }
            tableOptions.AddTableOption(key);
            //-------------------------
            return "SiteOptions.CheckOption(" + SiteOptionsBuilder.GetFullPropertyPath(key) + ")";
        }
        #endregion

        #region AddSiteOption
        public static string AddSiteOption(string key)
        {
            key = Globals.GetProgramatlyName(key);

            if (!SiteGlobalOptions.Contains(key))
            {
                SiteGlobalOptions.Add(key, "");
            }
            return "SiteOptions.CheckOption(" + SiteOptionsBuilder.GetFullPropertyPath(key) + ")";
        }
        #endregion


        #region CreateSiteOptionsXmlTags
        //-------------------------------------
        private static string CreateSiteOptionsXmlTags()
        {
            StringBuilder configsXml = new StringBuilder();

            foreach (DictionaryEntry key in SiteGlobalOptions)
            {
                configsXml.Append(TableOptions.CreateXmlNode(key.Key.ToString()));
            }

            return configsXml.ToString();
        }
        #endregion

		//-------------------------------------
		public static string GetOptionalCondition(string hasPropertyString, bool withAnd)
		{
            string condition ="SiteOptions.CheckOption("+SiteOptionsBuilder.GetFullPropertyPath(hasPropertyString)+")";
			if (withAnd)
			{
				return " && "+condition;
			}
			else
			{
                return condition;
			}
		}
		//-------------------------------------
		public static string GetHasPropertyString(string property)
		{
			return "Has" + property;
		}
		//-------------------------------------
		public static string GetFullHasPropertyString(string property)
		{
			Globals global = new Globals();
            //return global.TableProgramatlyName + GetHasPropertyString(property);
            return GetHasPropertyString(property);
        }
		//-------------------------------------\
        //-------------------------------------
        public static string GetFullPropertyPath(string property)
        {
            return "Resources." + SqlProvider.obj.TableName + "Options." + property;
        }
        //-------------------------------------\
	}
}

