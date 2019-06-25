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
    public enum SettingType
    {
        MaxLength,
        Extension
    }
	/// <summary>
	/// Summary description for SiteSettingsBuilder.
	/// </summary>
	public class SiteSettingsBuilder 
	{
        /**************************************************************/
        private static Hashtable TablesSettings = new Hashtable();
        /**************************************************************/
        #region Create
        public static void Create()
        {
            CreateTablesSettingFile();
            CreateTablesSettingClass();
        }
        #endregion
        /**************************************************************/
        #region CreateTablesSettingFile
        public static void CreateTablesSettingFile()
        {
            //-----------------------------------
            string configFile = "ProjectFiles/TableSettings.resx";
            StreamReader _reader = null;
            string lineOfText;
            StringBuilder sb = new StringBuilder();
            if (!System.IO.File.Exists(configFile))
            {
                throw new Exception("File " + configFile + " does not exists");
            }
            string xmlData = "";
            string settingsFile = "";
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
                        TableSettings setting;
                        string xmlTags;
                        foreach (DictionaryEntry key in TablesSettings)
                        {
                            setting = (TableSettings)key.Value;
                            xmlTags = setting.Tags.ToString();
                            settingsFile = xmlData.Replace("{R}", xmlTags);
                            //Directory Check
                            directoryPath = Globals.ModulesGlobalResources + setting.Key + "\\";
                            if (!Directory.Exists(directoryPath))
                                Directory.CreateDirectory(directoryPath);
                            //------------------------
                            // Create a file to write to.
                            path = directoryPath + setting.Key + "Settings.resx";
                            //------------------------
                           
                            // Create a file to write to.
                            using (StreamWriter sw = File.CreateText(path))
                            {
                                sw.WriteLine(settingsFile);
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
        #region CreateTablesSettingClass
        public static void CreateTablesSettingClass()
        {
            //-----------------------------------
            string settingClass = "ProjectFiles/TableSettings.cs";
            StreamReader _reader = null;
            string lineOfText;
            StringBuilder sb = new StringBuilder();
            if (!System.IO.File.Exists(settingClass))
            {
                throw new Exception("File " + settingClass + " does not exists");
            }
            string classData = "";
            string settingsFile = "";
            string path = "";
            using (Stream stream = System.IO.File.OpenRead(settingClass))
            {
                _reader = new StreamReader(stream);
                while (true)
                {
                    lineOfText = _reader.ReadLine();
                    if (lineOfText == null)
                    {
                        classData = sb.ToString();
                        TableSettings setting;
                        string classProperties;
                        foreach (DictionaryEntry key in TablesSettings)
                        {
                            setting = (TableSettings)key.Value;
                            classProperties = setting.Propreties.ToString();
                            settingsFile = classData.Replace("{R}", classProperties);
                            settingsFile = settingsFile.Replace("{T}", setting.Key);
                            //Ar File
                            path = Globals.ClassesDirectory + "\\" + setting.Key + "\\" + setting.Key + "Settings.cs";
                            // Create a file to write to.
                            using (StreamWriter sw = File.CreateText(path))
                            {
                                sw.WriteLine(settingsFile);
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
        #region AddTableSetting
        //-------------------------------------
        public static string AddTableSetting(string key,SettingType type)
        {
            return AddTableSetting(SqlProvider.obj.TableName, key, type);
        }
        //-------------------------------------
        public static string AddTableSetting(string tableId, string key, SettingType type)
        {
            key = Globals.GetProgramatlyName(key);
            TableSettings tableSettings;
            if (!TablesSettings.Contains(tableId))
            {
                tableSettings = new TableSettings(tableId);
                tableSettings.Key = tableId;
                TablesSettings.Add(tableId, tableSettings);
            }
            else
            {
                tableSettings = (TableSettings)TablesSettings[tableId];
            }
            tableSettings.AddSetting(key, type);
            
            //-------------------------
            return tableId+"Settings.Instance." + key; ;
        }
        #endregion
        //-------------------------------------
        public static string AddFileMaxLength(string key)
        {
            key += "MaxSize";
            return AddTableSetting(key, SettingType.MaxLength);
        }
        //-------------------------------------
        public static string AddFileExtensions(string key)
        {
            key = key + "AvailableExtension";
            return AddTableSetting(key, SettingType.Extension);
        }
        //-------------------------------------
	}
}

