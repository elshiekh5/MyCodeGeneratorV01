using System;
using System.IO;
using System.Text;
using System.Windows.Forms;
using System.Data;
using System.Collections;
namespace SPGen
{
	/// <summary>
	/// Summary description for Ceate_CodeBehindBuilder.
	/// </summary>
	public class Details_CodeBehindBuilder : CodeBehindBuilder
	{
		//
		private string LogoContainerTr = "trLogoContainer";
		private string PhotoContainerTr = "trPhotoContainer";
		//
		private string imageName = "ltrPhoto";
		private string logoName = "ltrLogo";
		private string fileName = "hlFile";

		//
		public Details_CodeBehindBuilder()
		{
			ClassName = global.TableProgramatlyName + "_Details";
		}

		private string GeneratePageLoad()
		{
			string pageLoadBody = "";
			pageLoadBody += "\n\t\tif(!IsPostBack)";
			pageLoadBody += "\n\t\t{";
			pageLoadBody += "\n\t\t\tLoadData();";
			pageLoadBody += "\n\t\t}";
			return GeneratePageLoadHandler(pageLoadBody);

		}

		private string CreateLoadData()
		{
			StringBuilder loadDataStart = new StringBuilder();
			StringBuilder loadDataChecks = new StringBuilder();
			StringBuilder loadDataBody = new StringBuilder();
			StringBuilder loadDataEnd = new StringBuilder();
			string id = Globals.GetProgramatlyName(ID.Name);
			id = Globals.ConvetStringToCamelCase(id);
			loadDataStart.Append("\n\t#region ---------------LoadData---------------");
			loadDataStart.Append("\n\t//-----------------------------------------------");
			loadDataStart.Append("\n\t//LoadData");
			loadDataStart.Append("\n\t//-----------------------------------------------");
			loadDataStart.Append("\n\tprotected void LoadData()");
			loadDataStart.Append("\n\t{");
			//Add Urls Property
			SiteUrlsBuilder.AddParameter(Globals.GetProgramatlyName(ID.Name));
			loadDataStart.Append("\n\t\tif(MoversFW.Components.UrlManager.ChechIsValidIntegerParameter("+SiteUrlsBuilder.GetIdentifire() + Globals.GetProgramatlyName(ID.Name) + "))");
			loadDataStart.Append("\n\t\t{");
			loadDataStart.Append("\n\t\t\t" + Globals.GetAliasDataType(ID.Datatype) + " " + id + " = Convert.To" + Globals.GetDataType(ID.Datatype) + "(Request.QueryString["+SiteUrlsBuilder.GetIdentifire() + Globals.GetProgramatlyName(ID.Name) + "]);");

			loadDataStart.Append("\n\t\t\t" + global.TableEntityClass + " " + global.EntityClassObject + " =" + global.TableFactoryClass + ".Get" + global.TableProgramatlyName + "Object(" + id + ");");
			loadDataChecks.Append("\n\t\t\tif (" + global.EntityClassObject + " != null)");
			loadDataChecks.Append("\n\t\t\t{");
            if (ProjectBuilder.HasMultiLanguages && SqlProvider.CheckISATableIsMultiLanguage(SqlProvider.obj.TableName))
			{
				loadDataChecks.Append("\n\t\t\t\t//");
				loadDataChecks.Append("\n\t\t\t\t//Check Valid Language to avoid changing query string manualy");
				loadDataChecks.Append("\n\t\t\t\tLanguages langid = (Languages)ResourceManager.GetCurrentLanguage();");
				loadDataChecks.Append("\n\t\t\t\tif (langid != " + global.EntityClassObject + ".LangID)");
				loadDataChecks.Append("\n\t\t\t\t{");
				loadDataChecks.Append("\n\t\t\t\t\tResponse.Redirect(\"default.aspx\");");
				loadDataChecks.Append("\n\t\t\t\t}");
			}
			//----------------------------------------------------------*/
			string datatype;
			string propertyName;
			string trPropertyName;
			string lblPropertyName;
			string optionalPropertyName ;
			foreach (SQLDMO.Column column in Fields)
			{
				propertyName = Globals.GetProgramatlyName(column.Name);
				trPropertyName = "tr" + propertyName;
                lblPropertyName="lbl" + propertyName;
				optionalPropertyName = SiteOptionsBuilder.GetHasPropertyString(propertyName);
				//if ((ID == null || column.Name != ID.Name) && (column.Default == null || column.Default.Length != 0) && column.Name.ToLower() != ProjectBuilder.LangID)
				if ((ID == null || column.Name != ID.Name) && (column.Name.IndexOf("_") < 0) 
                    && column.Name.ToLower().IndexOf("password") < 0
                  && column.Name.ToLower().IndexOf("shortdescription") < 0
                  && column.Name.ToLower().IndexOf(ProjectBuilder.PriorityColumnName.ToLower()) < 0
                  && column.Name.ToLower().IndexOf(ProjectBuilder.IsAvailable.ToLower()) < 0 
                  && column.Name.ToLower() != ProjectBuilder.LangID)
				{
					TableConstraint cnstr = SqlProvider.obj.GetParentColumn(column.Name);
					datatype = Globals.GetAliasDataType(column.Datatype);
					if (datatype == "string")
					{
						if (cnstr == null)
						{
							if (column.Name == ProjectBuilder.PhotoExtensionColumnName)
							{
								//	hasPhoto = true;
								loadDataBody.Append("\n\t\t\t\tif (" + global.EntityClassObject + ".HasPhotoExtension)");
								loadDataBody.Append("\n\t\t\t\t{");
								//Add Urls Property

								SiteUrlsBuilder.AddDirectoryUrl( "PhotoNormalThumbs", SiteUrlsBuilder.photoNormalUrl, Globals.GetProgramatlyName(Table), "");

                                loadDataBody.Append("\n\t\t\t\t\t" + imageName + ".Text = \" <a href='\" + " + SiteUrlsBuilder.GetIdentifire() + "BigPhotoThumbs + " + global.EntityClassObject + ".BigPhotoThumbs+\"' rel='lightbox'>\";");
                                loadDataBody.Append("\n\t\t\t\t\t" + imageName + ".Text += \"<img border='0' src='\" +" + SiteUrlsBuilder.GetIdentifire() + "PhotoNormalThumbs + " + global.EntityClassObject + ".NormalPhotoThumbs+\"'   class='Image' alt='\" + " + global.EntityClassObject + ".Title+\"' /></a>\";");

								loadDataBody.Append("\n\t\t\t\t}");
								loadDataBody.Append("\n\t\t\t\telse");
								loadDataBody.Append("\n\t\t\t\t{");
								loadDataBody.Append("\n\t\t\t\t\t" + PhotoContainerTr + ".Visible = false;");
								loadDataBody.Append("\n\t\t\t\t}");
								//loadDataBody.Append("\n\t\t\t\t//------------------------------------------");
								//loadDataBody.Append("\n\t\t\t\tViewState[\"" + ProjectBuilder.PhotoExtensionColumnName + "\"] = " + global.EntityClassObject + "." + ProjectBuilder.PhotoExtensionColumnName + ";");
								//loadDataBody.Append("\n\t\t\t\t//------------------------------------------");
							}
							else if (column.Name == "LogoExtension")
							{
								//hasPhoto = true;
								loadDataBody.Append("\n\t\t\t\tif (" + global.EntityClassObject + ".HasLogoExtension)");
								loadDataBody.Append("\n\t\t\t\t{");
								//Add Urls Property
								SiteUrlsBuilder.AddDirectoryUrl( "LogoNormalThumbs", SiteUrlsBuilder.logoNormalUrl, Globals.GetProgramatlyName(Table), "");


                                loadDataBody.Append("\n\t\t\t\t\t" + logoName + ".Text = \" <a href='\" + " + SiteUrlsBuilder.GetIdentifire() + "BigLogoThumbs + " + global.EntityClassObject + ".BigLogoThumbs+\"' rel='lightbox'>\";");
                                loadDataBody.Append("\n\t\t\t\t\t" + logoName + ".Text += \"<img border='0' src='\" +" + SiteUrlsBuilder.GetIdentifire() + "LogoNormalThumbs + " + global.EntityClassObject + ".NormalLogoThumbs+\"'   class='Image' alt='\" + " + global.EntityClassObject + ".Title+\"' /></a>\";");


								loadDataBody.Append("\n\t\t\t\t}");
								loadDataBody.Append("\n\t\t\t\telse");
								loadDataBody.Append("\n\t\t\t\t{");
								loadDataBody.Append("\n\t\t\t\t\t" + LogoContainerTr + ".Visible = false;");
								loadDataBody.Append("\n\t\t\t\t}");
								//loadDataBody.Append("\n\t\t\t\t//------------------------------------------");
								//loadDataBody.Append("\n\t\t\t\tViewState[\"" + ProjectBuilder.LogoExtensionColumnName + "\"] = " + global.EntityClassObject + "." + ProjectBuilder.LogoExtensionColumnName + ";");
								//loadDataBody.Append("\n\t\t\t\t//------------------------------------------");
							}
							else if (column.Name == "FileExtension")
							{
								//hasFile = true;
								loadDataBody.Append("\n\t\t\t\tif (" + global.EntityClassObject + ".HasFileExtension)");
								loadDataBody.Append("\n\t\t\t\t{");
								//Add Urls Property
								SiteUrlsBuilder.AddDirectoryUrl( "Files", SiteUrlsBuilder.filesUrl, Globals.GetProgramatlyName(Table), "");
								loadDataBody.Append("\n\t\t\t\t\t" + fileName + ".NavigateUrl = "+SiteUrlsBuilder.GetIdentifire() + "Files + " + global.EntityClassObject + ".File;");
								loadDataBody.Append("\n\t\t\t\t\t" + fileName + ".Text = " + ResourcesTesxtsBuilder.AddAdminGlobalText("DownLoadExistFile", TextType.Text) + ";");
								loadDataBody.Append("\n\t\t\t\t}");
								loadDataBody.Append("\n\t\t\t\telse");
								loadDataBody.Append("\n\t\t\t\t{");
								loadDataBody.Append("\n\t\t\t\t\t" + trPropertyName + ".Visible=false ;");
								loadDataBody.Append("\n\t\t\t\t}");
								//loadDataBody.Append("\n\t\t\t\t//------------------------------------------");
								//loadDataBody.Append("\n\t\t\t\tViewState[\"" + ProjectBuilder.FileExtensionColumnName + "\"] = " + global.EntityClassObject + "." + ProjectBuilder.FileExtensionColumnName + ";");
								//loadDataBody.Append("\n\t\t\t\t//------------------------------------------");
							}
							else if (column.Name.IndexOf("Extension") > -1)
							{
								string[] stringSeparators = new string[] { "Extension" };
								string[] separatingResult = column.Name.Split(stringSeparators, StringSplitOptions.None);
								propertyName = separatingResult[0];
								string uploaderID = "fu" + propertyName;
								string downloadLinkID = "hl" + propertyName;

								//hasFile = true;
								loadDataBody.Append("\n\t\t\t\tif (" + global.EntityClassObject + ".Has" + propertyName + "Extension)");
								loadDataBody.Append("\n\t\t\t\t{");
								//Add Urls Property
								SiteUrlsBuilder.AddDirectoryUrl( propertyName + "s", SiteUrlsBuilder.otherFilesUrl, Globals.GetProgramatlyName(Table), propertyName + "s");
								loadDataBody.Append("\n\t\t\t\t\t" + downloadLinkID + ".NavigateUrl = "+SiteUrlsBuilder.GetIdentifire() + propertyName + "s + " + global.EntityClassObject + "." + propertyName + ";");
								loadDataBody.Append("\n\t\t\t\t\t" + downloadLinkID + ".Text =" + ResourcesTesxtsBuilder.AddAdminGlobalText("DownLoadExistFile", TextType.Text) + ";");
								loadDataBody.Append("\n\t\t\t\t}");
								loadDataBody.Append("\n\t\t\t\telse");
								loadDataBody.Append("\n\t\t\t\t{");
								loadDataBody.Append("\n\t\t\t\t\t" + trPropertyName + ".Visible=false ;");
								loadDataBody.Append("\n\t\t\t\t}");
								//loadDataBody.Append("\n\t\t\t\t//------------------------------------------");
								//loadDataBody.Append("\n\t\t\t\tViewState[\"" + Globals.GetProgramatlyName(column.Name) + "\"] = " + global.EntityClassObject + "." + Globals.GetProgramatlyName(column.Name) + ";");
								//loadDataBody.Append("\n\t\t\t\t//------------------------------------------");
							}
							else if (column.Name.IndexOf("Date") > -1)
							{
								
								
								loadDataBody.Append("\n\t\t\t\t//-------------------------------");
								loadDataBody.Append("\n\t\t\t\t//" + Globals.GetProgramatlyName(column.Name));
								loadDataBody.Append("\n\t\t\t\tif (" + global.EntityClassObject + "." + optionalPropertyName + ")");
								loadDataBody.Append("\n\t\t\t\t\tlbl" + Globals.GetProgramatlyName(column.Name) + ".Text = " + global.EntityClassObject + "." + Globals.GetProgramatlyName(column.Name) + ".ToString();");
								loadDataBody.Append("\n\t\t\t\telse");
								loadDataBody.Append("\n\t\t\t\t\t" + trPropertyName + ".Visible=false ;");
								loadDataBody.Append("\n\t\t\t\t//-------------------------------");

							}
                                else if (Globals.GetSqlDataType(column.Datatype) == SqlDbType.NText)
                            {
                                loadDataBody.Append("\n\t\t\t\t//-------------------------------");
                                loadDataBody.Append("\n\t\t\t\t//" + Globals.GetProgramatlyName(column.Name));
                                loadDataBody.Append("\n\t\t\t\tif (" + global.EntityClassObject + "." + optionalPropertyName + ")");
                                loadDataBody.Append("\n\t\t\t\t\tlbl" + Globals.GetProgramatlyName(column.Name) + ".Text = Globals.SetLines(" + global.EntityClassObject + "." + Globals.GetProgramatlyName(column.Name) + ");");
                                loadDataBody.Append("\n\t\t\t\telse");
                                loadDataBody.Append("\n\t\t\t\t\t" + lblPropertyName + ".Visible=false ;");
                                
                                loadDataBody.Append("\n\t\t\t\t//-------------------------------");
                            }
							else
							{
								loadDataBody.Append("\n\t\t\t\t//-------------------------------");
								loadDataBody.Append("\n\t\t\t\t//" + Globals.GetProgramatlyName(column.Name));
								loadDataBody.Append("\n\t\t\t\tif (" + global.EntityClassObject + "." + optionalPropertyName + ")");
                                loadDataBody.Append("\n\t\t\t\t\tlbl" + Globals.GetProgramatlyName(column.Name) + ".Text = Globals.SetLines(" + global.EntityClassObject + "." + Globals.GetProgramatlyName(column.Name) + ");");
								loadDataBody.Append("\n\t\t\t\telse");
								loadDataBody.Append("\n\t\t\t\t\t" + trPropertyName + ".Visible=false ;");
								loadDataBody.Append("\n\t\t\t\t//-------------------------------");
							}


						}
						else
						{
						}
					}
					else if (datatype == "bool")
					{
						//loadDataBody.Append("\n\t\t\t\tcb" + Globals.GetProgramatlyName(column.Name) + ".Checked = " + global.EntityClassObject + "." + Globals.GetProgramatlyName(column.Name) + ";");
							loadDataBody.Append("\n\t\t\t\tlbl" + Globals.GetProgramatlyName(column.Name) + ".Text = " + ResourcesTesxtsBuilder.AddUserText("True",TextType.Text)+";");
					
					}
					else if (datatype != "byte[]" && datatype != "Object" && datatype != "Guid")
					{
						if (cnstr == null)
							loadDataBody.Append("\n\t\t\t\tlbl" + Globals.GetProgramatlyName(column.Name) + ".Text = " + global.EntityClassObject + "." + Globals.GetProgramatlyName(column.Name) + ".ToString();");
						else
						{
							//string text = SqlProvider.obj.GetExpectedNameForParent(cnstr.ParentTable);
							//string _value = Globals.GetProgramatlyName(cnstr.ParentColID);
							//string ddl;
							//string methodName;
							//BuildDropDownListsLoaderMethod(cnstr.ParentTable, text, _value, out ddl, out methodName);
							//loadDataBody.Append("\n\t\t\t\t" + methodName + ";");
							//loadDataBody.Append("\n\t\t\t\t" + ddl + ".SelectedValue = " + global.EntityClassObject + "." + Globals.GetProgramatlyName(column.Name) + ".ToString();");
						}
					}

				}
			}
			//End of object null value check
			loadDataEnd.Append("\n\t\t\t}");
			loadDataEnd.Append("\n\t\t\telse");
			loadDataEnd.Append("\n\t\t\t{");
			loadDataEnd.Append("\n\t\t\t\tResponse.Redirect(\"default.aspx\");");
			loadDataEnd.Append("\n\t\t\t}");
			//-------------------------------------
			//End of envalid parammeter check
			loadDataEnd.Append("\n\t\t}");
			loadDataEnd.Append("\n\t\telse");
			loadDataEnd.Append("\n\t\t{");
			loadDataEnd.Append("\n\t\t\tResponse.Redirect(\"default.aspx\");");
			loadDataEnd.Append("\n\t\t}");
			loadDataEnd.Append("\n\t}");
			loadDataEnd.Append("\n\t//-----------------------------------------------");
			loadDataEnd.Append("\n\t#endregion");

			return loadDataStart.ToString() + loadDataChecks.ToString() + loadDataBody.ToString() + loadDataEnd.ToString();
		}

		//
		public static void Create()
		{
			Details_CodeBehindBuilder cr = new Details_CodeBehindBuilder();
			cr.CreateTheFile();
		}
		//

		//
		private string GenerateClassBody()
		{
			string loadData = CreateLoadData();
			StringBuilder classBody = new StringBuilder();
			classBody.Append(GeneratePageLoad());

			classBody.Append("\n" + loadData);

			return classBody.ToString();
		}
		//
		private void CreateTheFile()
		{
			DirectoryInfo dInfo;
			string path;
			try
			{

				dInfo = Directory.CreateDirectory(Globals.UserControlsDirectory + global.TableProgramatlyName);
				path = dInfo.FullName + "\\" + ClassName + ".ascx.cs";
				// Create the file.
				string _class = GenerateClass(GenerateClassBody());
				FileManager.CreateFile(path, _class);
			}
			catch (Exception ex)
			{
				MessageBox.Show("My Generated Code Exception:" + ex.Message);
			}
		}

	}
}