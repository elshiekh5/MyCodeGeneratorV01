using System;

using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.IO;
using System.Windows.Forms;
namespace SPGen
{
    /// <summary>
    /// Summary description for Class1.
    /// </summary>
    public class ClassEntityBuilder : Generator
    {
        public ClassEntityBuilder()
        {
            ClassName = global.TableEntityClass;
        }
		//
        private string GeneateUsingBlock()
        {
            string Usingblock = "";
            Usingblock += "using System;\n";
            return Usingblock;
        }
        //
        private string GenerateClassBody()
        {
            return CreateInfoPropreties();
        }
        //

        private string GenerateClass(string classBody)
        {
            string xmlDocumentation = "\n/// <summary>\n";
            xmlDocumentation += "/// The class entity of " + SqlProvider.obj.TableName + ".\n";
            xmlDocumentation += "/// </summary>\n";
            string _class = "";
            _class += GeneateUsingBlock();
            if (ProjectBuilder.AllowXmlDocumentation)
                _class += xmlDocumentation;
            _class += "public class " + ClassName;
            _class += "\n{\n" + classBody + "\n}";
            return _class;
        }
        //
        public static void Create()
        {
            ClassEntityBuilder dp = new ClassEntityBuilder();
            dp.CreateClassFile();
        }
        //
        public void CreateClassFile()
        {
            try
            {
                string _class = GenerateClass(GenerateClassBody());
                DirectoryInfo dInfo = Directory.CreateDirectory(Globals.ClassesDirectory + global.TableProgramatlyName);
                string path = dInfo.FullName + "\\" + ClassName + ".cs";
                // Create a file to write to.
                using (StreamWriter sw = File.CreateText(path))
                {
                    sw.WriteLine(_class);
                }
                
            }
            catch (Exception ex)
            {
                MessageBox.Show("My Generated Code Exception:" + ex.Message);

            }
        }
        //
        #region ClassMember
        public string CreateInfoPropreties()
        {
            try
            {
				//Add Optional 
				if (ProjectBuilder.HasConfiguration)
				{
					string tableOptional = SiteOptionsBuilder.GetHasPropertyString(global.TableProgramatlyName);
					SiteOptionsBuilder.AddSiteOption(tableOptional);
				}
				//
                string xmlDocumentation = "";
                //
                StringBuilder EntityPropreties = new StringBuilder();
                //
                string dataType;
				string dataTypeAlias;
				string propertyName;
				string memberName;
                foreach (SQLDMO.Column colCurrent in Fields)
                {
					//Note dataType for checks & dataTypeAlias for writing
					dataType = Globals.GetAliasDataType(colCurrent.Datatype);
					dataTypeAlias = dataType;
					propertyName = Globals.GetProgramatlyName(colCurrent.Name);
					memberName = " _" + Globals.GetProgramatlyName(colCurrent.Name);
					#region Base Property
					EntityPropreties.Append("\n\t#region --------------" + propertyName + "--------------");
					if (colCurrent.Name.ToLower() == "langid")
					{
						dataTypeAlias = "Languages";
					}
                    if (dataType == "string")
                        EntityPropreties.Append("\n\tprivate " + dataTypeAlias + memberName + "= \"\";");
                    else if (dataType == "Guid")
                        EntityPropreties.Append("\n\tprivate " + dataTypeAlias + memberName + "= Guid.NewGuid();");
					else if (dataType == "DateTime")
						EntityPropreties.Append("\n\tprivate " + dataTypeAlias + memberName + "= DateTime.MinValue;");
					else
                        EntityPropreties.Append("\n\tprivate " + dataTypeAlias + memberName + ";");
                    //XML Documentaion
                    xmlDocumentation = "\n";
                    xmlDocumentation += "\n\t/// <summary>\n";
                    xmlDocumentation += "\t/// Gets or sets "+ SqlProvider.obj.TableName +" " + propertyName+". \n";
                    xmlDocumentation += "\t/// </summary>";
                    if (ProjectBuilder.AllowXmlDocumentation)
                        EntityPropreties.Append(xmlDocumentation);
                    //Propretie body
                    EntityPropreties.Append("\n\tpublic " + dataTypeAlias + " " + propertyName);
                    EntityPropreties.Append("\n\t{");
                    EntityPropreties.Append("\n\t\tget { return "+memberName + " ; }");
                    EntityPropreties.Append("\n\t\tset { "+memberName + "= value ; }");
                    EntityPropreties.Append("\n\t}");
                    EntityPropreties.Append("\n\t" + Globals.MetthodsSeparator);
					EntityPropreties.Append("\n\t#endregion");
					#endregion
                    if (ProjectBuilder.HasProprety)
                    {
                        #region ------------HasProperty---------------
                        if ((ID == null || colCurrent.Name != ID.Name))
                        {
                            string hasPropertyString = SiteOptionsBuilder.GetHasPropertyString(propertyName);
                            string fullHasPropertyString = SiteOptionsBuilder.GetFullHasPropertyString(propertyName);
                            string conditionLine = "";
                            if (ProjectBuilder.HasConfiguration)
                            {
                                SiteOptionsBuilder.AddTableOption(fullHasPropertyString);
                            }
                            if (dataType == "string" || dataType == "byte[]")
                            {
                                //--------------------------------------------------
                                if (ProjectBuilder.HasConfiguration)
                                {
                                    conditionLine = "if( " + memberName + ".Length > 0" + SiteOptionsBuilder.GetOptionalCondition(fullHasPropertyString, true) + ") ";
                                }
                                else
                                {
                                    conditionLine = "if( " + memberName + ".Length > 0) ";
                                }
                                //--------------------------------------------------
                            }

                            else if (dataType == "bool")
                            {
                                //--------------------------------------------------
                                if (ProjectBuilder.HasConfiguration)
                                {
                                    conditionLine = "if( " + memberName + SiteOptionsBuilder.GetOptionalCondition(fullHasPropertyString, true) + ") ";
                                }
                                else
                                {
                                    conditionLine = "if( " + memberName + ") ";
                                }
                                //--------------------------------------------------
                            }
                            else if (dataType == "DateTime")
                            {
                                //--------------------------------------------------
                                if (ProjectBuilder.HasConfiguration)
                                {
                                    conditionLine = "if( " + memberName + " != DateTime.MinValue" + SiteOptionsBuilder.GetOptionalCondition(fullHasPropertyString, true) + ") ";
                                }
                                else
                                {
                                    conditionLine = "if( " + memberName + " != DateTime.MinValue) ";
                                }
                                //--------------------------------------------------
                            }
                            else if (dataType == "decimal")
                            {
                                //--------------------------------------------------
                                if (ProjectBuilder.HasConfiguration)
                                {
                                    conditionLine = "if( " + memberName + " > 0.0" + SiteOptionsBuilder.GetOptionalCondition(fullHasPropertyString, true) + ") ";
                                }
                                else
                                {
                                    conditionLine = "if( " + memberName + " > 0.0) ";
                                }
                                //--------------------------------------------------
                            }
                            else if (dataType == "double")
                            {
                                //--------------------------------------------------
                                if (ProjectBuilder.HasConfiguration)
                                {
                                    conditionLine = "if( " + memberName + " > 0.0" + SiteOptionsBuilder.GetOptionalCondition(fullHasPropertyString, true) + ") ";
                                }
                                else
                                {
                                    conditionLine = "if( " + memberName + " > 0.0) ";
                                }
                                //--------------------------------------------------
                            }
                            else if (dataType == "Single")
                            {
                                //--------------------------------------------------
                                if (ProjectBuilder.HasConfiguration)
                                {
                                    conditionLine = "if( " + memberName + " != null" + SiteOptionsBuilder.GetOptionalCondition(fullHasPropertyString, true) + ") ";
                                }
                                else
                                {
                                    conditionLine = "if( " + memberName + " != null) ";
                                }
                                //--------------------------------------------------
                            }
                            else if (dataType == "short")
                            {
                                //--------------------------------------------------
                                if (ProjectBuilder.HasConfiguration)
                                {
                                    conditionLine = "if( " + memberName + " > 0" + SiteOptionsBuilder.GetOptionalCondition(fullHasPropertyString, true) + ") ";
                                }
                                else
                                {
                                    conditionLine = "if( " + memberName + " > 0) ";
                                }
                                //--------------------------------------------------
                            }
                            else if (dataType == "int")
                            {
                                //--------------------------------------------------
                                if (ProjectBuilder.HasConfiguration)
                                {
                                    conditionLine = "if( " + memberName + " > 0" + SiteOptionsBuilder.GetOptionalCondition(fullHasPropertyString, true) + ") ";
                                }
                                else
                                {
                                    conditionLine = "if( " + memberName + " > 0) ";
                                }
                                //--------------------------------------------------
                            }
                            else if (dataType == "long")
                            {
                                //--------------------------------------------------
                                if (ProjectBuilder.HasConfiguration)
                                {
                                    conditionLine = "if( " + memberName + " > 0" + SiteOptionsBuilder.GetOptionalCondition(fullHasPropertyString, true) + ") ";
                                }
                                else
                                {
                                    conditionLine = "if( " + memberName + " > 0) ";
                                }
                                //--------------------------------------------------
                            }
                            else if (dataType == "byte")
                            {
                                //--------------------------------------------------
                                if (ProjectBuilder.HasConfiguration)
                                {
                                    conditionLine = "if( " + memberName + " > 0" + SiteOptionsBuilder.GetOptionalCondition(fullHasPropertyString, true) + ") ";
                                }
                                else
                                {
                                    conditionLine = "if( " + memberName + " > 0) ";
                                }
                                //--------------------------------------------------
                            }
                            else if (dataType == "Guid")
                            {
                                //--------------------------------------------------
                                if (ProjectBuilder.HasConfiguration)
                                {
                                    conditionLine = "if( " + memberName + " != null" + SiteOptionsBuilder.GetOptionalCondition(fullHasPropertyString, true) + ") ";
                                }
                                else
                                {
                                    conditionLine = "if( " + memberName + " != null) ";
                                }
                                //--------------------------------------------------
                            }
                            else if (dataType == "Object")
                            {
                                //--------------------------------------------------
                                if (ProjectBuilder.HasConfiguration)
                                {
                                    conditionLine = "if( " + memberName + " != null" + SiteOptionsBuilder.GetOptionalCondition(fullHasPropertyString, true) + ") ";
                                }
                                else
                                {
                                    conditionLine = "if( " + memberName + " != null) ";
                                }
                                //--------------------------------------------------
                            }



                            EntityPropreties.Append("\n\t#region --------------" + hasPropertyString + "--------------");
                            EntityPropreties.Append("\n\tpublic bool " + hasPropertyString);
                            EntityPropreties.Append("\n\t{");
                            EntityPropreties.Append("\n\t\tget");
                            EntityPropreties.Append("\n\t\t{");
                            EntityPropreties.Append("\n\t\t\t" + conditionLine);
                            EntityPropreties.Append("\n\t\t\t{ ");
                            EntityPropreties.Append("\n\t\t\t\treturn true;");
                            EntityPropreties.Append("\n\t\t\t} ");
                            EntityPropreties.Append("\n\t\t\telse");
                            EntityPropreties.Append("\n\t\t\t{");
                            EntityPropreties.Append("\n\t\t\t\treturn false;");
                            EntityPropreties.Append("\n\t\t\t}");
                            EntityPropreties.Append("\n\t\t}");
                            EntityPropreties.Append("\n\t}");
                            EntityPropreties.Append("\n\t" + Globals.MetthodsSeparator);
                            EntityPropreties.Append("\n\t#endregion");
                            //------------------------------------------
                        }
                        #endregion
                    }
					#region Additional Properties for files properties
					//Additional Properties for files properties
					if (dataType == "string" && colCurrent.Name.IndexOf("Extension") > -1)
					{
						string[] stringSeparators = new string[] { "Extension" };
						string[] separatingResult = colCurrent.Name.Split(stringSeparators, StringSplitOptions.None);
						propertyName = separatingResult[0];
								
						//Property
						EntityPropreties.Append("\n\t#region --------------" + propertyName + "--------------");
						
						EntityPropreties.Append("\n\tpublic string " + propertyName);
						EntityPropreties.Append("\n\t{");
						EntityPropreties.Append("\n\t\tget");
						EntityPropreties.Append("\n\t\t{");
						EntityPropreties.Append("\n\t\t\tif( "+memberName + ".Length > 0) ");
						EntityPropreties.Append("\n\t\t\t{ ");
						EntityPropreties.Append("\n\t\t\t\treturn "+global.TableFactoryClass + ".Create" + Table + propertyName + "Name(_" + Globals.GetProgramatlyName(ID.Name) + ")+ "+memberName+";");
						EntityPropreties.Append("\n\t\t\t}");
						EntityPropreties.Append("\n\t\t\telse");
						EntityPropreties.Append("\n\t\t\t{");
						EntityPropreties.Append("\n\t\t\t\treturn \"\";");
						EntityPropreties.Append("\n\t\t\t}");
						EntityPropreties.Append("\n\t\t}");
						EntityPropreties.Append("\n\t}");
						EntityPropreties.Append("\n\t" + Globals.MetthodsSeparator);
						EntityPropreties.Append("\n\t#endregion");
						//----------------------------------------------------------
						//Additional photos Properties
						if (colCurrent.Name.ToLower().IndexOf("photo") > -1 || colCurrent.Name.ToLower().IndexOf("logo") > -1)
						{
							//Micro thumbnail
							EntityPropreties.Append("\n\t#region --------------Micro" + propertyName + "Thumbs--------------");
							EntityPropreties.Append("\n\tpublic string Micro" + propertyName + "Thumbs");
							EntityPropreties.Append("\n\t{");
							EntityPropreties.Append("\n\t\tget");
							EntityPropreties.Append("\n\t\t{");
							EntityPropreties.Append("\n\t\t\tif( "+memberName + ".Length > 0) ");
							EntityPropreties.Append("\n\t\t\t{ ");
							EntityPropreties.Append("\n\t\t\t\treturn " + global.TableFactoryClass + ".Create" + Table + propertyName + "Name(_" + Globals.GetProgramatlyName(ID.Name) + ")+  MoversFW.Thumbs.thumbnailExetnsion ;");
							EntityPropreties.Append("\n\t\t\t}");
							EntityPropreties.Append("\n\t\t\telse");
							EntityPropreties.Append("\n\t\t\t{");
							EntityPropreties.Append("\n\t\t\t\treturn \"\";");
							EntityPropreties.Append("\n\t\t\t}");
							EntityPropreties.Append("\n\t\t}");
							EntityPropreties.Append("\n\t}");
							EntityPropreties.Append("\n\t" + Globals.MetthodsSeparator);
							EntityPropreties.Append("\n\t#endregion");
							//-----------------------------------------------------------
							//Mini thumbnail
							EntityPropreties.Append("\n\t#region --------------Mini" + propertyName + "Thumbs--------------");
							EntityPropreties.Append("\n\tpublic string Mini" + propertyName + "Thumbs");
							EntityPropreties.Append("\n\t{");
							EntityPropreties.Append("\n\t\tget");
							EntityPropreties.Append("\n\t\t{");
							EntityPropreties.Append("\n\t\t\tif( "+memberName + ".Length > 0) ");
							EntityPropreties.Append("\n\t\t\t{ ");
							EntityPropreties.Append("\n\t\t\t\treturn " + global.TableFactoryClass + ".Create" + Table + propertyName + "Name(_" + Globals.GetProgramatlyName(ID.Name) + ")+  MoversFW.Thumbs.thumbnailExetnsion ;");
							EntityPropreties.Append("\n\t\t\t}");
							EntityPropreties.Append("\n\t\t\telse");
							EntityPropreties.Append("\n\t\t\t{");
							EntityPropreties.Append("\n\t\t\t\treturn \"\";");
							EntityPropreties.Append("\n\t\t\t}");
							EntityPropreties.Append("\n\t\t}");
							EntityPropreties.Append("\n\t}");
							EntityPropreties.Append("\n\t" + Globals.MetthodsSeparator);
							EntityPropreties.Append("\n\t#endregion");
							//-----------------------------------------------------------
							//Normal thumbnail
							EntityPropreties.Append("\n\t#region --------------Normal" + propertyName + "Thumbs--------------");
							EntityPropreties.Append("\n\tpublic string Normal" + propertyName + "Thumbs");
							EntityPropreties.Append("\n\t{");
							EntityPropreties.Append("\n\t\tget");
							EntityPropreties.Append("\n\t\t{");
							EntityPropreties.Append("\n\t\t\tif( "+memberName + ".Length > 0) ");
							EntityPropreties.Append("\n\t\t\t{ ");
							EntityPropreties.Append("\n\t\t\t\treturn " + global.TableFactoryClass + ".Create" + Table + propertyName + "Name(_" + Globals.GetProgramatlyName(ID.Name) + ")+  MoversFW.Thumbs.thumbnailExetnsion ;");
							EntityPropreties.Append("\n\t\t\t}");
							EntityPropreties.Append("\n\t\t\telse");
							EntityPropreties.Append("\n\t\t\t{");
							EntityPropreties.Append("\n\t\t\t\treturn \"\";");
							EntityPropreties.Append("\n\t\t\t}");
							EntityPropreties.Append("\n\t\t}");
							EntityPropreties.Append("\n\t}");
							EntityPropreties.Append("\n\t" + Globals.MetthodsSeparator);
							EntityPropreties.Append("\n\t#endregion");
							//-----------------------------------------------------------
							//Big thumbnail
							EntityPropreties.Append("\n\t#region --------------Big" + propertyName + "Thumbs--------------");
							EntityPropreties.Append("\n\tpublic string Big" + propertyName + "Thumbs");
							EntityPropreties.Append("\n\t{");
							EntityPropreties.Append("\n\t\tget");
							EntityPropreties.Append("\n\t\t{");
							EntityPropreties.Append("\n\t\t\tif( "+memberName + ".Length > 0) ");
							EntityPropreties.Append("\n\t\t\t{ ");
							EntityPropreties.Append("\n\t\t\t\treturn " + global.TableFactoryClass + ".Create" + Table + propertyName + "Name(_" + Globals.GetProgramatlyName(ID.Name) + ")+  MoversFW.Thumbs.thumbnailExetnsion ;");
							EntityPropreties.Append("\n\t\t\t}");
							EntityPropreties.Append("\n\t\t\telse");
							EntityPropreties.Append("\n\t\t\t{");
							EntityPropreties.Append("\n\t\t\t\treturn \"\";");
							EntityPropreties.Append("\n\t\t\t}");
							EntityPropreties.Append("\n\t\t}");
							EntityPropreties.Append("\n\t}");
							EntityPropreties.Append("\n\t" + Globals.MetthodsSeparator);
							EntityPropreties.Append("\n\t#endregion");
							//-----------------------------------------------------------
						}

					}
					#endregion

					
				}
                //

                EntityPropreties.Append("\n");
                return EntityPropreties.ToString();

            }
            catch (Exception ex)
            {
                MessageBox.Show("My Generated Code Exception:" + ex.Message);
                return "";
            }
        }
        #endregion
    }
}

