using System;

namespace SPGen
{
	/// <summary>
	/// Summary description for BuildWebConfig.
	/// </summary>
	public class WebConfigBuilder
	{
		public static void Create()
		{
			WebConfigBuilder wcb=new WebConfigBuilder();
			wcb.CreateWebConfig();
		}
		string configFile = "ProjectFiles/web.config";
		protected bool CreateWebConfig() 
		{
			bool returnValue = false;
			try 
			{
				System.Xml.XmlDocument doc = new System.Xml.XmlDocument();
				if( doc == null ) 
					return false;
		
				doc.PreserveWhitespace  = true;
		
				
		
				doc.Load(configFile);
				System.Xml.XmlNode connectionStrings = doc.SelectSingleNode("configuration/connectionStrings");
				foreach (System.Xml.XmlNode setting in connectionStrings) 
				{
					if( setting.Name == "add" ) 
					{
						System.Xml.XmlAttribute attrKey = setting.Attributes["name"];
						if( attrKey != null)
						{
							if(attrKey.Value == "Connectionstring" ) 
							{
								System.Xml.XmlAttribute attrSqlValue = setting.Attributes["connectionString"];
								if( attrSqlValue != null ) 
								{
									attrSqlValue.Value = SqlProvider.obj.GetTheConnectionString();
								}	
							}
						}
					}
				}
				System.Xml.XmlNode authentication = doc.SelectSingleNode("configuration/system.web/authentication");

				foreach( System.Xml.XmlNode auth in authentication ) 
				{
					if( auth.Name == "forms" ) 
					{
						System.Xml.XmlAttribute attrName = auth.Attributes["name"];
						if( attrName != null)
						{
										
							attrName.Value ="."+ProjectBuilder.ProjectName;
								
						}
					}
				}
				// Save the document to a file and auto-indent the output.
				System.Xml.XmlTextWriter writer = new System.Xml.XmlTextWriter(Globals.BaseDirectory + "web.config", System.Text.Encoding.UTF8);
				writer.Formatting = System.Xml.Formatting.Indented;
				doc.Save(writer);
				writer.Flush();
				writer.Close();

			}
			catch(Exception e ) 
			{
				Console.Write(e.Message);
			}
	
			return returnValue;
		}
	}
}
