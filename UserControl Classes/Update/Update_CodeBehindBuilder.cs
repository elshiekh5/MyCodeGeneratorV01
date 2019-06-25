using System;
using System.IO;
using System.Text ;
using System.Windows.Forms;
using System.Data;
using System.Collections;
namespace SPGen
{
	/// <summary>
	/// Summary description for Ceate_CodeBehindBuilder.
	/// </summary>
	public class Update_CodeBehindBuilder : CodeBehindBuilder
	{
		//
		private string photoUploadreID = "fuPhoto";
		private string logoUploadreID = "fuLogo";
		private string fileUploadreID = "fuFile";

		private bool hasPhoto = false;
		private bool hasLogo = false;
		private bool hasFile = false;

		private string imageName = "imgPhoto";
		private string imageNameTr = "trImgPhoto";
		private string imageNameTrHeader = "trImgPhotoHeader";
		private string logoName = "imgLogo";
		private string logoNameTr = "trImgLogo";
		private string logoNameTrHeader = "trImgLogoHeader";
		private string fileName = "hlFile";

		StringBuilder DropDownListsLoader = new StringBuilder();
		//ArrayList DropDownListsLoaderMethods=new ArrayList();
		//Build DropDownLists Loader Method  and return methodName

		#region ---------------CreateHandleOptionalControls---------------
		//-----------------------------------------------
		//CreateHandleOptionalControls
		//-----------------------------------------------

		private string CreateHandleOptionalControls()
		{
			StringBuilder method = new StringBuilder();
			string propertyName;
			string trPropertyName;
			string optionalPropertyName;
			string optionalFullPropertyName;
			string id = Globals.GetProgramatlyName(ID.Name);
			id = Globals.ConvetStringToCamelCase(id);
			method.Append("\n\t#region ---------------HandleOptionalControls---------------");
			method.Append("\n\t//-----------------------------------------------");
			method.Append("\n\t//HandleOptionalControls");
			method.Append("\n\t//-----------------------------------------------");
			method.Append("\n\tprotected void HandleOptionalControls()");
			method.Append("\n\t{");
			//method.Append("\n\t\tSiteOptions config = SiteOptions.Instance;");
			foreach (SQLDMO.Column column in Fields)
			{

				propertyName = Globals.GetProgramatlyName(column.Name);
				trPropertyName = "tr" + propertyName;
				optionalPropertyName = SiteOptionsBuilder.GetHasPropertyString(propertyName);
				optionalFullPropertyName = SiteOptionsBuilder.GetFullHasPropertyString(propertyName);
				//if ((ID == null || column.Name != ID.Name) && (column.Default == null || column.Default.Length != 0) && column.Name.ToLower() != ProjectBuilder.LangID)
				if ((ID == null || column.Name != ID.Name) && (column.Name.IndexOf("_") < 0) && column.Name.ToLower() != ProjectBuilder.LangID)
				{
                    if (ProjectBuilder.HasConfiguration)
                    {
                        method.Append("\n\t\t//" + optionalPropertyName);
                        method.Append("\n\t\tif (!SiteOptions.CheckOption(" + SiteOptionsBuilder.GetFullPropertyPath(optionalFullPropertyName) + "))");
                        method.Append("\n\t\t{");
                        method.Append("\n\t\t\t" + trPropertyName + ".Visible = false;");
                        if (column.Name == ProjectBuilder.PhotoExtensionColumnName)
                        {
                            method.Append("\n\t\t\t" + imageNameTrHeader + ".Visible = false;");
                            method.Append("\n\t\t\t" + imageNameTr + ".Visible = false;");
                        }
                        if (column.Name == "LogoExtension")
                        {
                            method.Append("\n\t\t\t" + logoNameTrHeader + ".Visible = false;");
                            method.Append("\n\t\t\t" + logoNameTr + ".Visible = false;");
                        }
                        method.Append("\n\t\t}");
                        method.Append("\n\t\t//-----------------------------------");
                    }
				}
			}
			method.Append("\n\t}");
			method.Append("\n\t//-----------------------------------------------");
			method.Append("\n\t#endregion");
			
			return method.ToString();
		}
		#endregion

        public void BuildDropDownListsLoaderMethod(string parentTableName, string text, string value, out string ddl, out string methodName)
		{
            string programatlyTableName = Globals.GetProgramatlyName(parentTableName);
            ddl = "ddl" + programatlyTableName;
            string listID = programatlyTableName + "List";
            string entityClassID = programatlyTableName + "Entity";
			methodName = "Load_" + ddl+"()";
			DropDownListsLoader.Append("\n\t#region --------------" + methodName + "--------------");
			DropDownListsLoader.Append("\n\t//---------------------------------------------------------");
			DropDownListsLoader.Append("\n\t//Load_" + ddl);
			DropDownListsLoader.Append("\n\t//---------------------------------------------------------");
			DropDownListsLoader.Append("\n\tprotected void " + methodName);
			DropDownListsLoader.Append("\n\t{");
            //---------------------------------------------------------
            bool hasIsAvailable = SqlProvider.CheckIsATableHasIsAvailableColumnName(parentTableName);
            if (hasIsAvailable)
            {
                DropDownListsLoader.Append("\n\t\tList<" + entityClassID + "> " + listID + " = " + programatlyTableName + "Factory.GetAllForAdmin();");
            }
            else
            {
                bool isMaultiLanguages = (ProjectBuilder.HasMultiLanguages && SqlProvider.CheckISATableIsMultiLanguage(parentTableName));
                if (isMaultiLanguages)
                {
                    DropDownListsLoader.Append("\n\t\tLanguages langID = (Languages)ResourceManager.GetCurrentLanguage();");
                    DropDownListsLoader.Append("\n\t\tList<" + entityClassID + "> " + listID + " = " + programatlyTableName + "Factory.GetAll(langID);");
                }
                else
                {
                    DropDownListsLoader.Append("\n\t\tList<" + entityClassID + "> " + listID + " = " + programatlyTableName + "Factory.GetAll();");
                }
            }
            //---------------------------------------------------------
            DropDownListsLoader.Append("\n\t\tif (" + listID + " != null && " + listID + ".Count > 0)");
			DropDownListsLoader.Append("\n\t\t{");
            DropDownListsLoader.Append("\n\t\t\t" + ddl + ".DataSource = " + listID + ";");
			DropDownListsLoader.Append("\n\t\t\t" + ddl + ".DataTextField = \"" + text + "\";");

			DropDownListsLoader.Append("\n\t\t\t" + ddl + ".DataValueField = \"" + value + "\";");
			DropDownListsLoader.Append("\n\t\t\t" + ddl + ".DataBind();");
			DropDownListsLoader.Append("\n\t\t\t" + ddl + ".Items.Insert(0, new ListItem(" + ResourcesTesxtsBuilder.AddAdminGlobalText("Choose", TextType.Text) + ", \"-1\"));");
			DropDownListsLoader.Append("\n\t\t\t" + ddl + ".Enabled = true;");
			DropDownListsLoader.Append("\n\t\t}");
			DropDownListsLoader.Append("\n\t\telse");
			DropDownListsLoader.Append("\n\t\t{");
            DropDownListsLoader.Append("\n\t\t\t" + ddl + ".Items.Clear();");
            DropDownListsLoader.Append("\n\t\t\t" + ddl + ".Items.Insert(0, new ListItem(" + ResourcesTesxtsBuilder.AddAdminGlobalText("ThereIsNoData", TextType.Text) + ", \"-1\"));");
			DropDownListsLoader.Append("\n\t\t\t" + ddl + ".Enabled = false;");
			DropDownListsLoader.Append("\n\t\t}");

			DropDownListsLoader.Append("\n\t}");
			DropDownListsLoader.Append("\n\t//--------------------------------------------------------");
			DropDownListsLoader.Append("\n\t#endregion");
			//DropDownListsLoaderMethods.Add(methodName);

		}
		//
		public Update_CodeBehindBuilder(InterfaceType type)
		{
			Type = type;
			ClassName = global.UpdateCodeBehindClass;
		}

		private string GeneratePageLoad()
		{
			string pageLoadBody = "\n\t\tlblResult.Text=\"\";";
			pageLoadBody += "\n\t\tif(!IsPostBack)";
			pageLoadBody += "\n\t\t{";
            if (ProjectBuilder.HasConfiguration)
            {
                pageLoadBody +="\n\t\t\tSiteOptions.CheckModuleWithHandling(Resources.SiteOptions." + SiteOptionsBuilder.GetHasPropertyString(Table) + ", ViewerType.Admin);";
            }
            //pageLoadBody += "\n\t\t\tbtnUpdate.Text = " + ResourcesTesxtsBuilder.AddAdminGlobalText("Update", TextType.Text) + ";";
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
			if (ProjectBuilder.HasConfiguration)
			{
				loadDataStart.Append("\n\t\tHandleOptionalControls();");
			}//Add Urls Property
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
			foreach (SQLDMO.Column column in Fields)
			{


				//if ((ID == null || column.Name != ID.Name) && (column.Default == null || column.Default.Length != 0) && column.Name.ToLower() != ProjectBuilder.LangID)
				if ((ID == null || column.Name != ID.Name) && (column.Name.IndexOf("_") < 0) && column.Name.ToLower() != ProjectBuilder.LangID)
				{
					TableConstraint cnstr = SqlProvider.obj.GetParentColumn(column.Name);
					datatype = Globals.GetAliasDataType(column.Datatype);
                    //Check Priority
                    if (column.Name.ToLower() == ProjectBuilder.PriorityColumnName.ToLower())
                    {
                        string getCountMethod = "GetCount()";
                        bool isMaultiLanguages = (ProjectBuilder.HasMultiLanguages && SqlProvider.CheckISATableIsMultiLanguage(SqlProvider.obj.TableName));
                        if (isMaultiLanguages)
                        {
                            getCountMethod = "GetCountForAdmin()";
                        }
                        if (ProjectBuilder.HasConfiguration)
                        {
                            loadDataBody.Append("\n\t\t\t\tif (tr" + Globals.GetProgramatlyName(column.Name) + ".Visible)");
                            loadDataBody.Append("\n\t\t\t\t{");
                            loadDataBody.Append("\n\t\t\t\t\tint itemsCount = " + global.TableFactoryClass + "." + getCountMethod + ";");
                            loadDataBody.Append("\n\t\t\t\t\tOurDropDownList.LoadPriorities(ddlPriority, itemsCount, false);");
                            loadDataBody.Append("\n\t\t\t\t\tddlPriority.SelectedValue = " + global.EntityClassObject + "." + Globals.GetProgramatlyName(column.Name) + ".ToString();");
                            loadDataBody.Append("\n\t\t\t\t}");
                        }
                        else
                        {
                            loadDataBody.Append("\n\t\t\t\tint itemsCount = " + global.TableFactoryClass + "." + getCountMethod + ";");
                            loadDataBody.Append("\n\t\t\t\tOurDropDownList.LoadPriorities(ddlPriority, itemsCount, false);");
                            loadDataBody.Append("\n\t\t\t\tddlPriority.SelectedValue = " + global.EntityClassObject + "." + Globals.GetProgramatlyName(column.Name) + ".ToString();");
                        }
                    }
                    else if (datatype == "string")
					{

                        if (Globals.GetSqlDataType(column.Datatype) == SqlDbType.NText && column.Name.ToLower().IndexOf("details") > -1)
                        {
                            if (ProjectBuilder.IsFreeTextBoxEditor)
                                loadDataBody.Append("\n\t\t\t\ttxt" + Globals.GetProgramatlyName(column.Name) + ".Text = " + global.EntityClassObject + "." + Globals.GetProgramatlyName(column.Name) + ";");
                            else
                                loadDataBody.Append("\n\t\t\t\ttxt" + Globals.GetProgramatlyName(column.Name) + ".Value = " + global.EntityClassObject + "." + Globals.GetProgramatlyName(column.Name) + ";");
                        }
                        else
                        {
                            if (cnstr == null)
                            {
                                if (column.Name == ProjectBuilder.PhotoExtensionColumnName)
                                {
                                    hasPhoto = true;
                                    loadDataBody.Append("\n\t\t\t\tif (" + global.EntityClassObject + ".HasPhotoExtension)");
                                    loadDataBody.Append("\n\t\t\t\t{");
                                    //Add Urls Property

                                    SiteUrlsBuilder.AddDirectoryUrl("PhotoNormalThumbs", SiteUrlsBuilder.photoNormalUrl, Globals.GetProgramatlyName(Table), "");
                                    loadDataBody.Append("\n\t\t\t\t\t" + imageName + ".ImageUrl = " + SiteUrlsBuilder.GetIdentifire() + "PhotoNormalThumbs + " + global.EntityClassObject + ".NormalPhotoThumbs;");
                                    loadDataBody.Append("\n\t\t\t\t\t//" + imageName + ".AlternateText = " + global.EntityClassObject + ".Title;");
                                    loadDataBody.Append("\n\t\t\t\t}");
                                    loadDataBody.Append("\n\t\t\t\telse");
                                    loadDataBody.Append("\n\t\t\t\t{");
                                    loadDataBody.Append("\n\t\t\t\t\t" + imageName + ".ImageUrl = " + SiteUrlsBuilder.GetGlobalIdentifire() + "NoPhoto;");
                                    loadDataBody.Append("\n\t\t\t\t\t" + imageName + ".AlternateText = " + ResourcesTesxtsBuilder.AddAdminGlobalText("NoPhoto", TextType.Text) + ";");
                                    loadDataBody.Append("\n\t\t\t\t}");
                                    loadDataBody.Append("\n\t\t\t\t//------------------------------------------");
                                    loadDataBody.Append("\n\t\t\t\tViewState[\"" + ProjectBuilder.PhotoExtensionColumnName + "\"] = " + global.EntityClassObject + "." + ProjectBuilder.PhotoExtensionColumnName + ";");
                                    loadDataBody.Append("\n\t\t\t\t//------------------------------------------");

                                }
                                else if (column.Name == "LogoExtension")
                                {
                                    hasPhoto = true;
                                    loadDataBody.Append("\n\t\t\t\tif (" + global.EntityClassObject + ".HasLogoExtension)");
                                    loadDataBody.Append("\n\t\t\t\t{");
                                    //Add Urls Property
                                    SiteUrlsBuilder.AddDirectoryUrl("LogoNormalThumbs", SiteUrlsBuilder.logoNormalUrl, Globals.GetProgramatlyName(Table), "");
                                    loadDataBody.Append("\n\t\t\t\t\t" + logoName + ".ImageUrl = " + SiteUrlsBuilder.GetIdentifire() + "LogoNormalThumbs + " + global.EntityClassObject + ".NormalLogoThumbs;");
                                    loadDataBody.Append("\n\t\t\t\t\t//" + logoName + ".AlternateText = " + global.EntityClassObject + ".Title;");
                                    loadDataBody.Append("\n\t\t\t\t}");
                                    loadDataBody.Append("\n\t\t\t\telse");
                                    loadDataBody.Append("\n\t\t\t\t{");
                                    loadDataBody.Append("\n\t\t\t\t\t" + logoName + ".ImageUrl = " + SiteUrlsBuilder.GetGlobalIdentifire() + "NoLogo;");
                                    loadDataBody.Append("\n\t\t\t\t\t" + logoName + ".AlternateText = " + ResourcesTesxtsBuilder.AddAdminGlobalText("NoLogo", TextType.Text) + ";");
                                    loadDataBody.Append("\n\t\t\t\t}");
                                    loadDataBody.Append("\n\t\t\t\t//------------------------------------------");
                                    loadDataBody.Append("\n\t\t\t\tViewState[\"" + ProjectBuilder.LogoExtensionColumnName + "\"] = " + global.EntityClassObject + "." + ProjectBuilder.LogoExtensionColumnName + ";");
                                    loadDataBody.Append("\n\t\t\t\t//------------------------------------------");

                                }
                                else if (column.Name == "FileExtension")
                                {
                                    hasFile = true;
                                    loadDataBody.Append("\n\t\t\t\tif (" + global.EntityClassObject + ".HasFileExtension)");
                                    loadDataBody.Append("\n\t\t\t\t{");
                                    //Add Urls Property
                                    SiteUrlsBuilder.AddDirectoryUrl("Files", SiteUrlsBuilder.filesUrl, Globals.GetProgramatlyName(Table), "");
                                    loadDataBody.Append("\n\t\t\t\t\t" + fileName + ".NavigateUrl = " + SiteUrlsBuilder.GetIdentifire() + "Files + " + global.EntityClassObject + ".File;");
                                    loadDataBody.Append("\n\t\t\t\t\t" + fileName + ".Text = " + ResourcesTesxtsBuilder.AddAdminGlobalText("DownLoadExistFile", TextType.Text) + ";");
                                    loadDataBody.Append("\n\t\t\t\t}");
                                    loadDataBody.Append("\n\t\t\t\telse");
                                    loadDataBody.Append("\n\t\t\t\t{");
                                    loadDataBody.Append("\n\t\t\t\t\t" + fileName + ".Visible=false ;");
                                    loadDataBody.Append("\n\t\t\t\t}");
                                    loadDataBody.Append("\n\t\t\t\t//------------------------------------------");
                                    loadDataBody.Append("\n\t\t\t\tViewState[\"" + ProjectBuilder.FileExtensionColumnName + "\"] = " + global.EntityClassObject + "." + ProjectBuilder.FileExtensionColumnName + ";");
                                    loadDataBody.Append("\n\t\t\t\t//------------------------------------------");

                                }
                                else if (column.Name.IndexOf("Extension") > -1)
                                {
                                    string[] stringSeparators = new string[] { "Extension" };
                                    string[] separatingResult = column.Name.Split(stringSeparators, StringSplitOptions.None);
                                    string propertyName = separatingResult[0];
                                    string uploaderID = "fu" + propertyName;
                                    string downloadLinkID = "hl" + propertyName;

                                    hasFile = true;
                                    loadDataBody.Append("\n\t\t\t\tif (" + global.EntityClassObject + ".Has" + propertyName + "Extension)");
                                    loadDataBody.Append("\n\t\t\t\t{");
                                    //Add Urls Property
                                    SiteUrlsBuilder.AddDirectoryUrl(propertyName + "s", SiteUrlsBuilder.otherFilesUrl, Globals.GetProgramatlyName(Table), propertyName + "s");
                                    loadDataBody.Append("\n\t\t\t\t\t" + downloadLinkID + ".NavigateUrl = " + SiteUrlsBuilder.GetIdentifire() + propertyName + "s + " + global.EntityClassObject + "." + propertyName + ";");
                                    loadDataBody.Append("\n\t\t\t\t\t" + downloadLinkID + ".Text =" + ResourcesTesxtsBuilder.AddAdminGlobalText("DownLoadExistFile", TextType.Text) + ";");
                                    loadDataBody.Append("\n\t\t\t\t}");
                                    loadDataBody.Append("\n\t\t\t\telse");
                                    loadDataBody.Append("\n\t\t\t\t{");
                                    loadDataBody.Append("\n\t\t\t\t\thl" + propertyName + ".Visible=false ;");
                                    loadDataBody.Append("\n\t\t\t\t}");
                                    loadDataBody.Append("\n\t\t\t\t//------------------------------------------");
                                    loadDataBody.Append("\n\t\t\t\tViewState[\"" + Globals.GetProgramatlyName(column.Name) + "\"] = " + global.EntityClassObject + "." + Globals.GetProgramatlyName(column.Name) + ";");
                                    loadDataBody.Append("\n\t\t\t\t//------------------------------------------");
                                }
                                else if (column.Name.IndexOf("Date") > -1)
                                {
                                    string x = column.Default;
                                }
                                else
                                {
                                    loadDataBody.Append("\n\t\t\t\ttxt" + Globals.GetProgramatlyName(column.Name) + ".Text = " + global.EntityClassObject + "." + Globals.GetProgramatlyName(column.Name) + ";");
                                }
                            }
                            else
                            {
                                string text = SqlProvider.obj.GetExpectedNameForParent(cnstr.ParentTable);
                                string _value = Globals.GetProgramatlyName(cnstr.ParentColID);
                                string ddl;
                                string methodName;
                                BuildDropDownListsLoaderMethod(cnstr.ParentTable, text, _value, out ddl, out methodName);
                                if (ProjectBuilder.HasConfiguration)
                                {
                                    loadDataBody.Append("\n\t\t\t\tif (tr" + Globals.GetProgramatlyName(column.Name) + ".Visible)");
                                    loadDataBody.Append("\n\t\t\t\t{");
                                    loadDataBody.Append("\n\t\t\t\t\t" + methodName + ";");
                                    loadDataBody.Append("\n\t\t\t\t\t" + ddl + ".SelectedValue = " + global.EntityClassObject + "." + Globals.GetProgramatlyName(column.Name) + ".ToString();");
                                    loadDataBody.Append("\n\t\t\t\t}");
                                }
                                else
                                {
                                    loadDataBody.Append("\n\t\t\t\t" + methodName + ";");
                                    loadDataBody.Append("\n\t\t\t\t" + ddl + ".SelectedValue = " + global.EntityClassObject + "." + Globals.GetProgramatlyName(column.Name) + ".ToString();");
                                }
                            }
                        }
					}
					else if (datatype == "bool")
						loadDataBody.Append("\n\t\t\t\tcb" + Globals.GetProgramatlyName(column.Name) + ".Checked = " + global.EntityClassObject + "." + Globals.GetProgramatlyName(column.Name) + ";");
					else if (datatype != "byte[]" && datatype != "Object" && datatype != "Guid")
					{
						if (cnstr == null)
							loadDataBody.Append("\n\t\t\t\ttxt" + Globals.GetProgramatlyName(column.Name) + ".Text = " + global.EntityClassObject + "." + Globals.GetProgramatlyName(column.Name) + ".ToString();");
						else
						{
							string text = SqlProvider.obj.GetExpectedNameForParent(cnstr.ParentTable);
							string _value = Globals.GetProgramatlyName(cnstr.ParentColID);
							string ddl;
							string methodName;
							BuildDropDownListsLoaderMethod(cnstr.ParentTable, text, _value, out ddl, out methodName);
                            if (ProjectBuilder.HasConfiguration)
                            {
                                loadDataBody.Append("\n\t\t\t\tif (tr" + Globals.GetProgramatlyName(column.Name) + ".Visible)");
                                loadDataBody.Append("\n\t\t\t\t{");
                                loadDataBody.Append("\n\t\t\t\t" + methodName + ";");
                                loadDataBody.Append("\n\t\t\t\t" + ddl + ".SelectedValue = " + global.EntityClassObject + "." + Globals.GetProgramatlyName(column.Name) + ".ToString();");
                                loadDataBody.Append("\n\t\t\t\t}");
                            }
                            else
                            {
                                loadDataBody.Append("\n\t\t\t\t" + methodName + ";");
                                loadDataBody.Append("\n\t\t\t\t" + ddl + ".SelectedValue = " + global.EntityClassObject + "." + Globals.GetProgramatlyName(column.Name) + ".ToString();");
                            } 
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
		private string Create_UpdateButtonHandler()
		{
			StringBuilder _UpdateButtonHandler = new StringBuilder();
			_UpdateButtonHandler.Append("\n\t#region ---------------Update---------------");
            _UpdateButtonHandler.Append("\n\t//-----------------------------------------------");
    _UpdateButtonHandler.Append("\n\t//ibtnUpdate_Click");
    _UpdateButtonHandler.Append("\n\t//-----------------------------------------------");
   _UpdateButtonHandler.Append("\n\tprotected void ibtnUpdate_Click(object sender, ImageClickEventArgs e)");
    _UpdateButtonHandler.Append("\n\t{");
        _UpdateButtonHandler.Append("\n\t\tUpdate();");
        _UpdateButtonHandler.Append("\n\t}");
			_UpdateButtonHandler.Append("\n\t//-----------------------------------------------");
			_UpdateButtonHandler.Append("\n\t//Update method");
			_UpdateButtonHandler.Append("\n\t//-----------------------------------------------");
            _UpdateButtonHandler.Append("\n\tprotected void Update()");
			_UpdateButtonHandler.Append("\n\t{");
			_UpdateButtonHandler.Append("\n\t\tif (MoversFW.Components.UrlManager.ChechIsValidIntegerParameter("+SiteUrlsBuilder.GetIdentifire() + Globals.GetProgramatlyName(ID.Name) + "))");
			_UpdateButtonHandler.Append("\n\t\t{");
			_UpdateButtonHandler.Append("\n\t\t\tif (!Page.IsValid)");
			_UpdateButtonHandler.Append("\n\t\t\t{");
			_UpdateButtonHandler.Append("\n\t\t\t\treturn;");
			_UpdateButtonHandler.Append("\n\t\t\t}");
			_UpdateButtonHandler.Append("\n\t\t\t" + global.TableEntityClass + " " + global.EntityClassObject + " = new " + global.TableEntityClass + "();");
			string datatype;
			_UpdateButtonHandler.Append("\n\t\t\t" + global.EntityClassObject + "." + Globals.GetProgramatlyName(ID.Name) + " = Convert.To" + Globals.GetDataType(ID.Datatype) + "(Request.QueryString["+SiteUrlsBuilder.GetIdentifire() + Globals.GetProgramatlyName(ID.Name) + "]);");
			foreach (SQLDMO.Column column in Fields)
			{
				//if ((ID == null || column.Name != ID.Name) && (column.Default == null || column.Default.Length != 0) && column.Name.ToLower() != ProjectBuilder.LangID)
				if ((ID == null || column.Name != ID.Name) && (column.Name.IndexOf("_") < 0) && column.Name.ToLower() != ProjectBuilder.LangID)
				{
					TableConstraint cnstr = SqlProvider.obj.GetParentColumn(column.Name);
					datatype = Globals.GetAliasDataType(column.Datatype);
                    if (column.Name.ToLower() == ProjectBuilder.PriorityColumnName.ToLower())
                    {
                        if (ProjectBuilder.HasConfiguration)
                        {
                            _UpdateButtonHandler.Append("\n\t\tif (tr" + Globals.GetProgramatlyName(column.Name) + ".Visible)");
                            _UpdateButtonHandler.Append("\n\t\t\t" + global.EntityClassObject + "." + Globals.GetProgramatlyName(column.Name) + " =Convert.To" + Globals.GetDataType(column.Datatype) + "(ddlPriority.SelectedValue);");
                        }
                        else
                        {
                            _UpdateButtonHandler.Append("\n\t\t" + global.EntityClassObject + "." + Globals.GetProgramatlyName(column.Name) + " =Convert.To" + Globals.GetDataType(column.Datatype) + "(ddlPriority.SelectedValue);");
                        } 
                    }
					else if (datatype == "string")
					{
                        if (Globals.GetSqlDataType(column.Datatype) == SqlDbType.NText && column.Name.ToLower().IndexOf("details") > -1)
                        {
                            if (ProjectBuilder.IsFreeTextBoxEditor)
                            {
                                _UpdateButtonHandler.Append("\n\t\t\t" + global.EntityClassObject + "." + Globals.GetProgramatlyName(column.Name) + " = txt" + Globals.GetProgramatlyName(column.Name) + ".Text;");
                            }
                            else 
                            {
                                _UpdateButtonHandler.Append("\n\t\t\t" + global.EntityClassObject + "." + Globals.GetProgramatlyName(column.Name) + " = txt" + Globals.GetProgramatlyName(column.Name) + ".Value;");

                            }
                        }
                        else
                        {
                            if (cnstr == null)
                            {
                                if (column.Name == ProjectBuilder.PhotoExtensionColumnName)
                                {


                                    _UpdateButtonHandler.Append("\n\t\t\t//-------------");
                                    _UpdateButtonHandler.Append("\n\t\t\tstring " + ProjectBuilder.PhotoExtensionColumnNameVariable + " = (string)ViewState[\"" + ProjectBuilder.PhotoExtensionColumnName + "\"];");
                                    _UpdateButtonHandler.Append("\n\t\t\tif (" + photoUploadreID + ".HasFile)");
                                    _UpdateButtonHandler.Append("\n\t\t\t{");
                                    _UpdateButtonHandler.Append("\n\t\t\t\tif (!MoversFW.Photos.CheckIsImage(" + photoUploadreID + ".PostedFile))");
                                    _UpdateButtonHandler.Append("\n\t\t\t\t{");
                                    _UpdateButtonHandler.Append("\n\t\t\t\t\tlblResult.ForeColor = Color.Red;");
                                    _UpdateButtonHandler.Append("\n\t\t\t\t\tlblResult.Text = " + ResourcesTesxtsBuilder.AddAdminGlobalText("InvalidPhotoFile", TextType.Text) + ";");
                                    _UpdateButtonHandler.Append("\n\t\t\t\t\treturn;");
                                    _UpdateButtonHandler.Append("\n\t\t\t\t}");
                                    _UpdateButtonHandler.Append("\n\t\t\t\t" + global.EntityClassObject + "." + Globals.GetProgramatlyName(column.Name) + " = Path.GetExtension(" + photoUploadreID + ".FileName);");
                                    _UpdateButtonHandler.Append("\n\t\t\t}");
                                    _UpdateButtonHandler.Append("\n\t\t\telse");
                                    _UpdateButtonHandler.Append("\n\t\t\t{");
                                    _UpdateButtonHandler.Append("\n\t\t\t\t" + global.EntityClassObject + "." + Globals.GetProgramatlyName(column.Name) + " = " + ProjectBuilder.PhotoExtensionColumnNameVariable + ";");
                                    _UpdateButtonHandler.Append("\n\t\t\t}");
                                    _UpdateButtonHandler.Append("\n\t\t\t//-----------------------------------------------------------------");

                                }
                                else if (column.Name == "LogoExtension")
                                {
                                    _UpdateButtonHandler.Append("\n\t\t\t//-------------");
                                    _UpdateButtonHandler.Append("\n\t\t\tstring " + ProjectBuilder.LogoExtensionColumnNameVariable + " = (string)ViewState[\"" + ProjectBuilder.LogoExtensionColumnName + "\"];");
                                    _UpdateButtonHandler.Append("\n\t\t\tif (" + logoUploadreID + ".HasFile)");
                                    _UpdateButtonHandler.Append("\n\t\t\t{");
                                    _UpdateButtonHandler.Append("\n\t\t\t\tif (!MoversFW.Photos.CheckIsImage(" + logoUploadreID + ".PostedFile))");
                                    _UpdateButtonHandler.Append("\n\t\t\t\t{");
                                    _UpdateButtonHandler.Append("\n\t\t\t\t\tlblResult.ForeColor = Color.Red;");
                                    _UpdateButtonHandler.Append("\n\t\t\t\t\tlblResult.Text = " + ResourcesTesxtsBuilder.AddAdminGlobalText("InvalidLogoFile", TextType.Text) + ";");
                                    _UpdateButtonHandler.Append("\n\t\t\t\t\treturn;");
                                    _UpdateButtonHandler.Append("\n\t\t\t\t}");
                                    _UpdateButtonHandler.Append("\n\t\t\t\t" + global.EntityClassObject + "." + Globals.GetProgramatlyName(column.Name) + " = Path.GetExtension(" + logoUploadreID + ".FileName);");
                                    _UpdateButtonHandler.Append("\n\t\t\t}");
                                    _UpdateButtonHandler.Append("\n\t\t\telse");
                                    _UpdateButtonHandler.Append("\n\t\t\t{");
                                    _UpdateButtonHandler.Append("\n\t\t\t\t" + global.EntityClassObject + "." + Globals.GetProgramatlyName(column.Name) + " = " + ProjectBuilder.LogoExtensionColumnNameVariable + ";");
                                    _UpdateButtonHandler.Append("\n\t\t\t}");
                                    _UpdateButtonHandler.Append("\n\t\t\t//-----------------------------------------------------------------");

                                }
                                else if (column.Name == "FileExtension")
                                {
                                    _UpdateButtonHandler.Append("\n\t\t\t//-------------");
                                    _UpdateButtonHandler.Append("\n\t\t\tstring " + ProjectBuilder.FileExtensionColumnNameVariable + " = (string)ViewState[\"" + ProjectBuilder.FileExtensionColumnName + "\"];");
                                    _UpdateButtonHandler.Append("\n\t\t\tif (" + fileUploadreID + ".HasFile)");
                                    _UpdateButtonHandler.Append("\n\t\t\t{");
                                    //XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX
                                    _UpdateButtonHandler.Append("\n\t\t\t\tstring uploadedFilePath = Path.GetExtension(" + fileUploadreID + ".FileName);");
                                    _UpdateButtonHandler.Append("\n\t\t\t\t//Check suported extention");
                                    _UpdateButtonHandler.Append("\n\t\t\t\tif (!SiteSettings.CheckUploadedFileExtension(uploadedFilePath, " + SiteSettingsBuilder.AddFileExtensions(Table + "File") + "))");
                                    _UpdateButtonHandler.Append("\n\t\t\t\t{");
                                    _UpdateButtonHandler.Append("\n\t\t\t\t\tlblResult.ForeColor = Color.Red;");
                                    _UpdateButtonHandler.Append("\n\t\t\t\t\tlblResult.Text = " + ResourcesTesxtsBuilder.AddAdminGlobalText("NotSuportedFileExtention", TextType.Text) + ";");
                                    _UpdateButtonHandler.Append("\n\t\t\t\t\treturn;");
                                    _UpdateButtonHandler.Append("\n\t\t\t\t}");
                                    _UpdateButtonHandler.Append("\n\t\t\t\t//Check max length");
                                    _UpdateButtonHandler.Append("\n\t\t\t\tif (!SiteSettings.CheckUploadedFileLength(" + fileUploadreID + ".PostedFile.ContentLength," + SiteSettingsBuilder.AddFileMaxLength(Table + "File") + "))");
                                    _UpdateButtonHandler.Append("\n\t\t\t\t{");
                                    _UpdateButtonHandler.Append("\n\t\t\t\t\tlblResult.ForeColor = Color.Red;");
                                    _UpdateButtonHandler.Append("\n\t\t\t\t\tlblResult.Text = " + ResourcesTesxtsBuilder.AddAdminGlobalText("UploadedFileGreaterThanMaxLength", TextType.Text) + ";");
                                    _UpdateButtonHandler.Append("\n\t\t\t\t\treturn;");
                                    _UpdateButtonHandler.Append("\n\t\t\t\t}");
                                    _UpdateButtonHandler.Append("\n\t\t\t\t" + global.EntityClassObject + "." + Globals.GetProgramatlyName(column.Name) + " = uploadedFilePath;");
                                    //XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX
                                    //_UpdateButtonHandler.Append("\n\t\t\t\t" + global.EntityClassObject + "." + Globals.GetProgramatlyName(column.Name) + " = Path.GetExtension(" + fileUploadreID + ".FileName);");
                                    _UpdateButtonHandler.Append("\n\t\t\t}");
                                    _UpdateButtonHandler.Append("\n\t\t\telse");
                                    _UpdateButtonHandler.Append("\n\t\t\t{");
                                    _UpdateButtonHandler.Append("\n\t\t\t\t" + global.EntityClassObject + "." + Globals.GetProgramatlyName(column.Name) + " = " + ProjectBuilder.FileExtensionColumnNameVariable + ";");
                                    _UpdateButtonHandler.Append("\n\t\t\t}");
                                    _UpdateButtonHandler.Append("\n\t\t\t//-----------------------------------------------------------------");
                                }
                                else if (column.Name.IndexOf("Extension") > -1)
                                {
                                    string[] stringSeparators = new string[] { "Extension" };
                                    string[] separatingResult = column.Name.Split(stringSeparators, StringSplitOptions.None);
                                    string propertyName = separatingResult[0];
                                    string uploaderID = "fu" + propertyName;
                                    string columnNamevariable = Globals.ConvetStringToCamelCase(Globals.GetProgramatlyName(column.Name));

                                    _UpdateButtonHandler.Append("\n\t\t\t//-------------");
                                    _UpdateButtonHandler.Append("\n\t\t\tstring " + columnNamevariable + " = (string)ViewState[\"" + Globals.GetProgramatlyName(column.Name) + "\"];");
                                    _UpdateButtonHandler.Append("\n\t\t\tif (" + uploaderID + ".HasFile)");
                                    _UpdateButtonHandler.Append("\n\t\t\t{");
                                    //XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX
                                    _UpdateButtonHandler.Append("\n\t\t\t\tstring uploaded" + propertyName + "Extension = Path.GetExtension(" + uploaderID + ".FileName);");
                                    _UpdateButtonHandler.Append("\n\t\t\t\t//Check suported extention");
                                    _UpdateButtonHandler.Append("\n\t\t\t\tif (!SiteSettings.CheckUploadedFileExtension(uploaded" + propertyName + "Extension, " + SiteSettingsBuilder.AddFileExtensions(Table + propertyName) + "))");
                                    _UpdateButtonHandler.Append("\n\t\t\t\t{");
                                    _UpdateButtonHandler.Append("\n\t\t\t\t\tlblResult.ForeColor = Color.Red;");
                                    _UpdateButtonHandler.Append("\n\t\t\t\t\tlblResult.Text = " + ResourcesTesxtsBuilder.AddUserText("NotSuported" + propertyName + "Extention", TextType.Text) + ";");
                                    _UpdateButtonHandler.Append("\n\t\t\t\t\treturn;");
                                    _UpdateButtonHandler.Append("\n\t\t\t\t}");
                                    _UpdateButtonHandler.Append("\n\t\t\t\t//Check max length");
                                    _UpdateButtonHandler.Append("\n\t\t\t\tif (!SiteSettings.CheckUploadedFileLength(" + uploaderID + ".PostedFile.ContentLength," + SiteSettingsBuilder.AddFileMaxLength(Table + propertyName) + "))");
                                    _UpdateButtonHandler.Append("\n\t\t\t\t{");
                                    _UpdateButtonHandler.Append("\n\t\t\t\t\tlblResult.ForeColor = Color.Red;");
                                    _UpdateButtonHandler.Append("\n\t\t\t\t\tlblResult.Text = " + ResourcesTesxtsBuilder.AddUserText("Uploaded" + propertyName + "GreaterThanMaxLength", TextType.Text) + ";");
                                    _UpdateButtonHandler.Append("\n\t\t\t\t\treturn;");
                                    _UpdateButtonHandler.Append("\n\t\t\t\t}");
                                    _UpdateButtonHandler.Append("\n\t\t\t\t" + global.EntityClassObject + "." + Globals.GetProgramatlyName(column.Name) + " = uploaded" + propertyName + "Extension;");
                                    //XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX
                                    //_UpdateButtonHandler.Append("\n\t\t\t\t" + global.EntityClassObject + "." + Globals.GetProgramatlyName(column.Name) + " = Path.GetExtension(" + uploaderID + ".FileName);");
                                    _UpdateButtonHandler.Append("\n\t\t\t}");
                                    _UpdateButtonHandler.Append("\n\t\t\telse");
                                    _UpdateButtonHandler.Append("\n\t\t\t{");
                                    _UpdateButtonHandler.Append("\n\t\t\t\t" + global.EntityClassObject + "." + Globals.GetProgramatlyName(column.Name) + " = " + columnNamevariable + ";");
                                    _UpdateButtonHandler.Append("\n\t\t\t}");
                                    _UpdateButtonHandler.Append("\n\t\t\t//-----------------------------------------------------------------");
                                }
                                else if (column.Name.IndexOf("Date") > -1)
                                {
                                    string x = column.Default;
                                }
                                else
                                {
                                    _UpdateButtonHandler.Append("\n\t\t\t" + global.EntityClassObject + "." + Globals.GetProgramatlyName(column.Name) + " = txt" + Globals.GetProgramatlyName(column.Name) + ".Text;");

                                }
                            }
                            else
                                _UpdateButtonHandler.Append("\n\t\t\t" + global.EntityClassObject + "." + Globals.GetProgramatlyName(column.Name) + " = ddl" + Globals.GetProgramatlyName(cnstr.ParentTable) + ".SelectedValue;");
                        }
					}
					else if (datatype == "bool")
						_UpdateButtonHandler.Append("\n\t\t\t" + global.EntityClassObject + "." + Globals.GetProgramatlyName(column.Name) + " = cb" + Globals.GetProgramatlyName(column.Name) + ".Checked;");
					else if (datatype == "Guid")
						_UpdateButtonHandler.Append("\n\t\t\t" + global.EntityClassObject + "." + Globals.GetProgramatlyName(column.Name) + " = Guid.NewGuid();");
					else if (datatype != "byte[]" && datatype != "Object")
					{
                        if (cnstr == null)
                        {
                            if (ProjectBuilder.HasConfiguration)
                            {
                                _UpdateButtonHandler.Append("\n\t\tif (tr" + Globals.GetProgramatlyName(column.Name) + ".Visible)");
                                _UpdateButtonHandler.Append("\n\t\t\t" + global.EntityClassObject + "." + Globals.GetProgramatlyName(column.Name) + " = Convert.To" + Globals.GetDataType(column.Datatype) + "(txt" + Globals.GetProgramatlyName(column.Name) + ".Text);");
                            }
                            else
                            {
                                _UpdateButtonHandler.Append("\n\t\t" + global.EntityClassObject + "." + Globals.GetProgramatlyName(column.Name) + " = Convert.To" + Globals.GetDataType(column.Datatype) + "(txt" + Globals.GetProgramatlyName(column.Name) + ".Text);");

                            } 
                        }

                        else
                        {
                            if (ProjectBuilder.HasConfiguration)
                            {
                                _UpdateButtonHandler.Append("\n\t\tif (tr" + Globals.GetProgramatlyName(column.Name) + ".Visible)");
                                _UpdateButtonHandler.Append("\n\t\t\t" + global.EntityClassObject + "." + Globals.GetProgramatlyName(column.Name) + " = Convert.To" + Globals.GetDataType(column.Datatype) + "(ddl" + Globals.GetProgramatlyName(cnstr.ParentTable) + ".SelectedValue);");
                            }
                            else
                            {
                                _UpdateButtonHandler.Append("\n\t\t" + global.EntityClassObject + "." + Globals.GetProgramatlyName(column.Name) + " = Convert.To" + Globals.GetDataType(column.Datatype) + "(ddl" + Globals.GetProgramatlyName(cnstr.ParentTable) + ".SelectedValue);");
                            }
                        }
					}
				}
			}
            if (!ProjectBuilder.ISExcuteScaler)
            {
                _UpdateButtonHandler.Append("\n\t\t\tbool status = " + global.TableFactoryClass + "." + MethodType.Update.ToString() + "(" + global.EntityClassObject + ");");
                _UpdateButtonHandler.Append("\n\t\t\tif(status)");
            }
            else
            {
                _UpdateButtonHandler.Append("\n\t\tExecuteCommandStatus status = " + global.TableFactoryClass + "." + MethodType.Update.ToString() + "(" + global.EntityClassObject + ");");
                _UpdateButtonHandler.Append("\n\t\tif(status == ExecuteCommandStatus.Done)");
            }
			_UpdateButtonHandler.Append("\n\t\t\t{");
			if (hasPhoto || hasLogo || hasFile)
			{
				_UpdateButtonHandler.Append("\n\t\t\t\t//--------------------------------");
				_UpdateButtonHandler.Append("\n\t\t\t\tSiteSettings settings = SiteSettings.Instance; ;");
				_UpdateButtonHandler.Append("\n\t\t\t\t"+SiteUrlsBuilder.GetFullDeclaration());
				_UpdateButtonHandler.Append("\n\t\t\t\t//");
				_UpdateButtonHandler.Append("\n\t\t\t\t//--------------------------------");
			}
			//XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX
			#region XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX
			foreach (SQLDMO.Column column in Fields)
			{
				//if ((ID == null || column.Name != ID.Name) && (column.Default == null || column.Default.Length != 0) && column.Name.ToLower() != ProjectBuilder.LangID)
				if ((ID == null || column.Name != ID.Name) && (column.Name.IndexOf("_") < 0) && column.Name.ToLower() != ProjectBuilder.LangID)
				{
					TableConstraint cnstr = SqlProvider.obj.GetParentColumn(column.Name);
					datatype = Globals.GetAliasDataType(column.Datatype);

					if (datatype == "string")
					{
							if (cnstr == null)
							{
								if (column.Name == ProjectBuilder.PhotoExtensionColumnName)
								{
									_UpdateButtonHandler.Append("\n\t\t\t\t//Photo-----------------------------");
									_UpdateButtonHandler.Append("\n\t\t\t\tif (" + photoUploadreID + ".HasFile)");
									_UpdateButtonHandler.Append("\n\t\t\t\t{");
									_UpdateButtonHandler.Append("\n\t\t\t\t\t//if has an old photo");
									_UpdateButtonHandler.Append("\n\t\t\t\t\tif (" + ProjectBuilder.PhotoExtensionColumnNameVariable + ".Length > 0)");
									_UpdateButtonHandler.Append("\n\t\t\t\t\t{");
									_UpdateButtonHandler.Append("\n\t\t\t\t\t\t//Delete old original photo");
									//Add Urls Property
									SiteUrlsBuilder.AddDirectoryUrl( "OriginalPhotos", SiteUrlsBuilder.photoOriginalUrl, Globals.GetProgramatlyName(Table), "");
									SiteUrlsBuilder.AddDirectoryUrl( "MicroPhotoThumbs", SiteUrlsBuilder.photoMicroUrl, Globals.GetProgramatlyName(Table), "");
									SiteUrlsBuilder.AddDirectoryUrl( "MiniPhotoThumbs", SiteUrlsBuilder.photoMiniUrl, Globals.GetProgramatlyName(Table), "");
									SiteUrlsBuilder.AddDirectoryUrl( "NormalPhotoThumbs", SiteUrlsBuilder.photoNormalUrl, Globals.GetProgramatlyName(Table), "");
									SiteUrlsBuilder.AddDirectoryUrl( "BigPhotoThumbs", SiteUrlsBuilder.photoBigUrl, Globals.GetProgramatlyName(Table), "");
                                    _UpdateButtonHandler.Append("\n\t\t\t\t\t\tFile.Delete(Server.MapPath(urls.OriginalPhotos) + " + global.TableFactoryClass + ".Create" + Table + "PhotoName(" + global.EntityClassObject + "." + Globals.GetProgramatlyName(ID.Name) + " )  + " + ProjectBuilder.PhotoExtensionColumnNameVariable + ");");
									_UpdateButtonHandler.Append("\n\t\t\t\t\t\t//Delete old Thumbnails");
									_UpdateButtonHandler.Append("\n\t\t\t\t\t\tFile.Delete(Server.MapPath(urls.MicroPhotoThumbs) + " + global.TableFactoryClass + ".Create" + Table + "PhotoName(" + global.EntityClassObject + "." + Globals.GetProgramatlyName(ID.Name) + " ) + MoversFW.Thumbs.thumbnailExetnsion);");
									_UpdateButtonHandler.Append("\n\t\t\t\t\t\tFile.Delete(Server.MapPath(urls.MiniPhotoThumbs) + " + global.TableFactoryClass + ".Create" + Table + "PhotoName(" + global.EntityClassObject + "." + Globals.GetProgramatlyName(ID.Name) + " ) + MoversFW.Thumbs.thumbnailExetnsion);");
									_UpdateButtonHandler.Append("\n\t\t\t\t\t\tFile.Delete(Server.MapPath(urls.NormalPhotoThumbs) + " + global.TableFactoryClass + ".Create" + Table + "PhotoName(" + global.EntityClassObject + "." + Globals.GetProgramatlyName(ID.Name) + " ) + MoversFW.Thumbs.thumbnailExetnsion);");
									_UpdateButtonHandler.Append("\n\t\t\t\t\t\tFile.Delete(Server.MapPath(urls.BigPhotoThumbs) + " + global.TableFactoryClass + ".Create" + Table + "PhotoName(" + global.EntityClassObject + "." + Globals.GetProgramatlyName(ID.Name) + " ) + MoversFW.Thumbs.thumbnailExetnsion);");
									_UpdateButtonHandler.Append("\n\t\t\t\t\t}");
									_UpdateButtonHandler.Append("\n\t\t\t\t\t//------------------------------------------------");
									_UpdateButtonHandler.Append("\n\t\t\t\t\t//Save new original photo");
									_UpdateButtonHandler.Append("\n\t\t\t\t\t" + photoUploadreID + ".PostedFile.SaveAs(Server.MapPath(urls.OriginalPhotos) + " + global.EntityClassObject + ".Photo);");
									_UpdateButtonHandler.Append("\n\t\t\t\t\t//Create new thumbnails");
									_UpdateButtonHandler.Append("\n\t\t\t\t\tMoversFW.Thumbs.CreateThumb(urls.MicroPhotoThumbs, " + global.TableFactoryClass + ".Create" + Table + "PhotoName(" + global.EntityClassObject + "." + Globals.GetProgramatlyName(ID.Name) + " ), " + photoUploadreID + ".PostedFile, settings.MicroThumnailWidth, settings.MicroThumnailHeight);");
									_UpdateButtonHandler.Append("\n\t\t\t\t\tMoversFW.Thumbs.CreateThumb(urls.MiniPhotoThumbs, " + global.TableFactoryClass + ".Create" + Table + "PhotoName(" + global.EntityClassObject + "." + Globals.GetProgramatlyName(ID.Name) + " ), " + photoUploadreID + ".PostedFile, settings.MiniThumnailWidth, settings.MiniThumnailHeight);");
									_UpdateButtonHandler.Append("\n\t\t\t\t\tMoversFW.Thumbs.CreateThumb(urls.NormalPhotoThumbs, " + global.TableFactoryClass + ".Create" + Table + "PhotoName(" + global.EntityClassObject + "." + Globals.GetProgramatlyName(ID.Name) + " ), " + photoUploadreID + ".PostedFile, settings.NormalThumnailWidth, settings.NormalThumnailHeight);");
									_UpdateButtonHandler.Append("\n\t\t\t\t\tMoversFW.Thumbs.CreateThumb(urls.BigPhotoThumbs, " + global.TableFactoryClass + ".Create" + Table + "PhotoName(" + global.EntityClassObject + "." + Globals.GetProgramatlyName(ID.Name) + " ), " + photoUploadreID + ".PostedFile, settings.BigThumnailWidth, settings.BigThumnailHeight);");
									_UpdateButtonHandler.Append("\n\t\t\t\t}");

								}
								else if (column.Name == "LogoExtension")
								{
									_UpdateButtonHandler.Append("\n\t\t\t\t//Logo-----------------------------");
									_UpdateButtonHandler.Append("\n\t\t\t\tif (" + logoUploadreID + ".HasFile)");
									_UpdateButtonHandler.Append("\n\t\t\t\t{");
									_UpdateButtonHandler.Append("\n\t\t\t\t\t//if has an old Logo");
									_UpdateButtonHandler.Append("\n\t\t\t\t\tif (" + ProjectBuilder.LogoExtensionColumnNameVariable + ".Length > 0)");
									_UpdateButtonHandler.Append("\n\t\t\t\t\t{");
									_UpdateButtonHandler.Append("\n\t\t\t\t\t\t//Delete old original Logo");
									//Add Urls Property
									SiteUrlsBuilder.AddDirectoryUrl( "OriginalLogos", SiteUrlsBuilder.logoOriginalUrl, Globals.GetProgramatlyName(Table), "");
									SiteUrlsBuilder.AddDirectoryUrl( "MicroLogoThumbs", SiteUrlsBuilder.logoMicroUrl, Globals.GetProgramatlyName(Table), "");
									SiteUrlsBuilder.AddDirectoryUrl( "MiniLogoThumbs", SiteUrlsBuilder.logoMiniUrl, Globals.GetProgramatlyName(Table), "");
									SiteUrlsBuilder.AddDirectoryUrl( "NormalLogoThumbs", SiteUrlsBuilder.logoNormalUrl, Globals.GetProgramatlyName(Table), "");
									SiteUrlsBuilder.AddDirectoryUrl( "BigLogoThumbs", SiteUrlsBuilder.logoBigUrl, Globals.GetProgramatlyName(Table), "");
									_UpdateButtonHandler.Append("\n\t\t\t\t\t\tFile.Delete(Server.MapPath(urls.OriginalLogos) + " + global.EntityClassObject + "." + Globals.GetProgramatlyName(ID.Name) + "  + " + ProjectBuilder.LogoExtensionColumnNameVariable + ");");
									_UpdateButtonHandler.Append("\n\t\t\t\t\t\t//Delete old Thumbnails");
									_UpdateButtonHandler.Append("\n\t\t\t\t\t\tFile.Delete(Server.MapPath(urls.MicroLogoThumbs) + " + global.TableFactoryClass + ".Create" + Table + "LogoName(" + global.EntityClassObject + "." + Globals.GetProgramatlyName(ID.Name) + " ) + MoversFW.Thumbs.thumbnailExetnsion);");
									_UpdateButtonHandler.Append("\n\t\t\t\t\t\tFile.Delete(Server.MapPath(urls.MiniLogoThumbs) + " + global.TableFactoryClass + ".Create" + Table + "LogoName(" + global.EntityClassObject + "." + Globals.GetProgramatlyName(ID.Name) + " ) + MoversFW.Thumbs.thumbnailExetnsion);");
									_UpdateButtonHandler.Append("\n\t\t\t\t\t\tFile.Delete(Server.MapPath(urls.NormalLogoThumbs) + " + global.TableFactoryClass + ".Create" + Table + "LogoName(" + global.EntityClassObject + "." + Globals.GetProgramatlyName(ID.Name) + " ) + MoversFW.Thumbs.thumbnailExetnsion);");
									_UpdateButtonHandler.Append("\n\t\t\t\t\t\tFile.Delete(Server.MapPath(urls.BigLogoThumbs) + " + global.TableFactoryClass + ".Create" + Table + "LogoName(" + global.EntityClassObject + "." + Globals.GetProgramatlyName(ID.Name) + " ) + MoversFW.Thumbs.thumbnailExetnsion);");
									_UpdateButtonHandler.Append("\n\t\t\t\t\t}");
									_UpdateButtonHandler.Append("\n\t\t\t\t\t//------------------------------------------------");
									_UpdateButtonHandler.Append("\n\t\t\t\t\t//Save new original Logo");
									_UpdateButtonHandler.Append("\n\t\t\t\t\t" + logoUploadreID + ".PostedFile.SaveAs(Server.MapPath(urls.OriginalLogos) + " + global.EntityClassObject + ".Logo);");
									_UpdateButtonHandler.Append("\n\t\t\t\t\t//Create new thumbnails");
									_UpdateButtonHandler.Append("\n\t\t\t\t\tMoversFW.Thumbs.CreateThumb(urls.MicroLogoThumbs, " + global.TableFactoryClass + ".Create" + Table + "LogoName(" + global.EntityClassObject + "." + Globals.GetProgramatlyName(ID.Name) + " ), " + logoUploadreID + ".PostedFile, settings.MicroThumnailWidth, settings.MicroThumnailHeight);");
									_UpdateButtonHandler.Append("\n\t\t\t\t\tMoversFW.Thumbs.CreateThumb(urls.MiniLogoThumbs, " + global.TableFactoryClass + ".Create" + Table + "LogoName(" + global.EntityClassObject + "." + Globals.GetProgramatlyName(ID.Name) + " ), " + logoUploadreID + ".PostedFile, settings.MiniThumnailWidth, settings.MiniThumnailHeight);");
									_UpdateButtonHandler.Append("\n\t\t\t\t\tMoversFW.Thumbs.CreateThumb(urls.NormalLogoThumbs, " + global.TableFactoryClass + ".Create" + Table + "LogoName(" + global.EntityClassObject + "." + Globals.GetProgramatlyName(ID.Name) + " ), " + logoUploadreID + ".PostedFile, settings.NormalThumnailWidth, settings.NormalThumnailHeight);");
									_UpdateButtonHandler.Append("\n\t\t\t\t\tMoversFW.Thumbs.CreateThumb(urls.BigLogoThumbs, " + global.TableFactoryClass + ".Create" + Table + "LogoName(" + global.EntityClassObject + "." + Globals.GetProgramatlyName(ID.Name) + " ), " + logoUploadreID + ".PostedFile, settings.BigThumnailWidth, settings.BigThumnailHeight);");
									_UpdateButtonHandler.Append("\n\t\t\t\t}");
								}
								else if (column.Name == "FileExtension")
								{
									_UpdateButtonHandler.Append("\n\t\t\t\t//File-----------------------------");
									_UpdateButtonHandler.Append("\n\t\t\t\tif (" + fileUploadreID + ".HasFile)");
									_UpdateButtonHandler.Append("\n\t\t\t\t{");
									_UpdateButtonHandler.Append("\n\t\t\t\t\t//if has an old file");
									_UpdateButtonHandler.Append("\n\t\t\t\t\tif (" + ProjectBuilder.FileExtensionColumnNameVariable + ".Length > 0)");
									_UpdateButtonHandler.Append("\n\t\t\t\t\t{");
									_UpdateButtonHandler.Append("\n\t\t\t\t\t\t//Delete old original file");
									//Add Urls Property
									SiteUrlsBuilder.AddDirectoryUrl( "OriginalFiles", SiteUrlsBuilder.filesUrl, Globals.GetProgramatlyName(Table), "");
									_UpdateButtonHandler.Append("\n\t\t\t\t\t\tFile.Delete(Server.MapPath(urls.OriginalFiles) + " + global.EntityClassObject + "." + Globals.GetProgramatlyName(ID.Name) + "  + " + ProjectBuilder.FileExtensionColumnNameVariable + ");");
									_UpdateButtonHandler.Append("\n\t\t\t\t\t}");
									_UpdateButtonHandler.Append("\n\t\t\t\t\t//------------------------------------------------");
									_UpdateButtonHandler.Append("\n\t\t\t\t\t//Save new original file");
									_UpdateButtonHandler.Append("\n\t\t\t\t\t" + fileUploadreID + ".PostedFile.SaveAs(Server.MapPath(urls.OriginalFiles) + " + global.EntityClassObject + ".File);");

									_UpdateButtonHandler.Append("\n\t\t\t\t}");
								}
								else if (column.Name.IndexOf("Extension") > 0)
								{

									string[] stringSeparators = new string[] { "Extension" };
									string[] separatingResult =column.Name.Split(stringSeparators, StringSplitOptions.None);
									string propertyName = separatingResult[0];
									string uploaderID = "fu" + propertyName;
									string columnNamevariable = Globals.ConvetStringToCamelCase(Globals.GetProgramatlyName(column.Name));
									
									_UpdateButtonHandler.Append("\n\t\t\t\t//File-----------------------------");
									_UpdateButtonHandler.Append("\n\t\t\t\tif (" + uploaderID + ".HasFile)");
									_UpdateButtonHandler.Append("\n\t\t\t\t{");
									_UpdateButtonHandler.Append("\n\t\t\t\t\t//if has an old "+propertyName);
									_UpdateButtonHandler.Append("\n\t\t\t\t\tif (" + columnNamevariable + ".Length > 0)");
									_UpdateButtonHandler.Append("\n\t\t\t\t\t{");
									//Add Urls Property
									SiteUrlsBuilder.AddDirectoryUrl( "Original" + propertyName, SiteUrlsBuilder.otherFilesUrl, Globals.GetProgramatlyName(Table), propertyName+"s");
									_UpdateButtonHandler.Append("\n\t\t\t\t\t\t//Delete old original " + propertyName);
									_UpdateButtonHandler.Append("\n\t\t\t\t\t\tFile.Delete(Server.MapPath(urls.Original" + propertyName + ") + " + global.EntityClassObject + "." + Globals.GetProgramatlyName(ID.Name) + "  + " + columnNamevariable + ");");
									_UpdateButtonHandler.Append("\n\t\t\t\t\t}");
									_UpdateButtonHandler.Append("\n\t\t\t\t\t//------------------------------------------------");
									_UpdateButtonHandler.Append("\n\t\t\t\t\t//Save new original " + propertyName);
									_UpdateButtonHandler.Append("\n\t\t\t\t\t" + uploaderID + ".PostedFile.SaveAs(Server.MapPath(urls.Original" + propertyName + ") + " + global.EntityClassObject + "." + propertyName + ");");

									_UpdateButtonHandler.Append("\n\t\t\t\t}");
								}
								

							
						}
					}
					
				}
			}
#endregion
			/*
			if (hasPhoto)
			{
				_UpdateButtonHandler.Append("\n\t\t\t\t//Photo-----------------------------");
				_UpdateButtonHandler.Append("\n\t\t\t\tif (" + photoUploadreID + ".HasFile)");
				_UpdateButtonHandler.Append("\n\t\t\t\t{");
				_UpdateButtonHandler.Append("\n\t\t\t\t\t//if has an old photo");
				_UpdateButtonHandler.Append("\n\t\t\t\t\tif (" + ProjectBuilder.PhotoExtensionColumnName + ".Length > 0)");
				_UpdateButtonHandler.Append("\n\t\t\t\t\t{");
				_UpdateButtonHandler.Append("\n\t\t\t\t\t\t//Delete old original photo");
				_UpdateButtonHandler.Append("\n\t\t\t\t\t\tFile.Delete(Server.MapPath(urls.ProductOriginalPhotos) + product." + Globals.GetProgramatlyName(ID.Name) + "  + " + ProjectBuilder.PhotoExtensionColumnName + ");");
				_UpdateButtonHandler.Append("\n\t\t\t\t\t\t//Delete old Thumbnails");
				_UpdateButtonHandler.Append("\n\t\t\t\t\t\tFile.Delete(Server.MapPath(urls.MicroPhotoThumbs) + " + global.TableFactoryClass + ".Create" + Table + "PhotoName(" + global.EntityClassObject + "." + Globals.GetProgramatlyName(ID.Name) + " ) + MoversFW.Thumbs.thumbnailExetnsion);");
				_UpdateButtonHandler.Append("\n\t\t\t\t\t\tFile.Delete(Server.MapPath(urls.MiniPhotoThumbs) + " + global.TableFactoryClass + ".Create" + Table + "PhotoName(" + global.EntityClassObject + "." + Globals.GetProgramatlyName(ID.Name) + " ) + MoversFW.Thumbs.thumbnailExetnsion);");
				_UpdateButtonHandler.Append("\n\t\t\t\t\t\tFile.Delete(Server.MapPath(urls.NormalPhotoThumbs) + " + global.TableFactoryClass + ".Create" + Table + "PhotoName(" + global.EntityClassObject + "." + Globals.GetProgramatlyName(ID.Name) + " ) + MoversFW.Thumbs.thumbnailExetnsion);");
				_UpdateButtonHandler.Append("\n\t\t\t\t\t\tFile.Delete(Server.MapPath(urls.BigPhotoThumbs) + " + global.TableFactoryClass + ".Create" + Table + "PhotoName(" + global.EntityClassObject + "." + Globals.GetProgramatlyName(ID.Name) + " ) + MoversFW.Thumbs.thumbnailExetnsion);");
				_UpdateButtonHandler.Append("\n\t\t\t\t\t}");
				_UpdateButtonHandler.Append("\n\t\t\t\t\t//------------------------------------------------");
				_UpdateButtonHandler.Append("\n\t\t\t\t\t//Save new original photo");
				_UpdateButtonHandler.Append("\n\t\t\t\t\t" + photoUploadreID + ".PostedFile.SaveAs(Server.MapPath(urls.OriginalPhotos) + " + global.EntityClassObject + ".Photo);");
				_UpdateButtonHandler.Append("\n\t\t\t\t\t//Create new thumbnails");
				_UpdateButtonHandler.Append("\n\t\t\t\t\tMoversFW.Thumbs.CreateThumb(urls.MicroPhotoThumbs, " + global.TableFactoryClass + ".Create" + Table + "PhotoName(" + global.EntityClassObject + "." + Globals.GetProgramatlyName(ID.Name) + " ), " + photoUploadreID + ".PostedFile, settings.MicroThumnailWidth, settings.MicroThumnailHeight);");
				_UpdateButtonHandler.Append("\n\t\t\t\t\tMoversFW.Thumbs.CreateThumb(urls.MiniPhotoThumbs, " + global.TableFactoryClass + ".Create" + Table + "PhotoName(" + global.EntityClassObject + "." + Globals.GetProgramatlyName(ID.Name) + " ), " + photoUploadreID + ".PostedFile, settings.MiniThumnailWidth, settings.MiniThumnailHeight);");
				_UpdateButtonHandler.Append("\n\t\t\t\t\tMoversFW.Thumbs.CreateThumb(urls.NormalPhotoThumbs, " + global.TableFactoryClass + ".Create" + Table + "PhotoName(" + global.EntityClassObject + "." + Globals.GetProgramatlyName(ID.Name) + " ), " + photoUploadreID + ".PostedFile, settings.NormalThumnailWidth, settings.NormalThumnailHeight);");
				_UpdateButtonHandler.Append("\n\t\t\t\t\tMoversFW.Thumbs.CreateThumb(urls.BigPhotoThumbs, " + global.TableFactoryClass + ".Create" + Table + "PhotoName(" + global.EntityClassObject + "." + Globals.GetProgramatlyName(ID.Name) + " ), " + photoUploadreID + ".PostedFile, settings.BigThumnailWidth, settings.BigThumnailHeight);");
				_UpdateButtonHandler.Append("\n\t\t\t\t}");

			}
			//------------------------------------------------------------------------
			if (hasLogo)
			{
				_UpdateButtonHandler.Append("\n\t\t\t\t//Logo-----------------------------");
				_UpdateButtonHandler.Append("\n\t\t\t\tif (" + logoUploadreID + ".HasFile)");
				_UpdateButtonHandler.Append("\n\t\t\t\t{");
				_UpdateButtonHandler.Append("\n\t\t\t\t\t//if has an old Logo");
				_UpdateButtonHandler.Append("\n\t\t\t\t\tif (" + ProjectBuilder.LogoExtensionColumnName + ".Length > 0)");
				_UpdateButtonHandler.Append("\n\t\t\t\t\t{");
				_UpdateButtonHandler.Append("\n\t\t\t\t\t\t//Delete old original Logo");
				_UpdateButtonHandler.Append("\n\t\t\t\t\t\tFile.Delete(Server.MapPath(urls.ProductOriginalLogos) + product." + Globals.GetProgramatlyName(ID.Name) + "  + " + ProjectBuilder.LogoExtensionColumnName + ");");
				_UpdateButtonHandler.Append("\n\t\t\t\t\t\t//Delete old Thumbnails");
				_UpdateButtonHandler.Append("\n\t\t\t\t\t\tFile.Delete(Server.MapPath(urls.MicroLogoThumbs) + " + global.TableFactoryClass + ".Create" + Table + "LogoName(" + global.EntityClassObject + "." + Globals.GetProgramatlyName(ID.Name) + " ) + MoversFW.Thumbs.thumbnailExetnsion);");
				_UpdateButtonHandler.Append("\n\t\t\t\t\t\tFile.Delete(Server.MapPath(urls.MiniLogoThumbs) + " + global.TableFactoryClass + ".Create" + Table + "LogoName(" + global.EntityClassObject + "." + Globals.GetProgramatlyName(ID.Name) + " ) + MoversFW.Thumbs.thumbnailExetnsion);");
				_UpdateButtonHandler.Append("\n\t\t\t\t\t\tFile.Delete(Server.MapPath(urls.NormalLogoThumbs) + " + global.TableFactoryClass + ".Create" + Table + "LogoName(" + global.EntityClassObject + "." + Globals.GetProgramatlyName(ID.Name) + " ) + MoversFW.Thumbs.thumbnailExetnsion);");
				_UpdateButtonHandler.Append("\n\t\t\t\t\t\tFile.Delete(Server.MapPath(urls.BigLogoThumbs) + " + global.TableFactoryClass + ".Create" + Table + "LogoName(" + global.EntityClassObject + "." + Globals.GetProgramatlyName(ID.Name) + " ) + MoversFW.Thumbs.thumbnailExetnsion);");
				_UpdateButtonHandler.Append("\n\t\t\t\t\t}");
				_UpdateButtonHandler.Append("\n\t\t\t\t\t//------------------------------------------------");
				_UpdateButtonHandler.Append("\n\t\t\t\t\t//Save new original Logo");
				_UpdateButtonHandler.Append("\n\t\t\t\t\t" + photoUploadreID + ".PostedFile.SaveAs(Server.MapPath(urls.OriginalLogos) + " + global.EntityClassObject + ".Logo);");
				_UpdateButtonHandler.Append("\n\t\t\t\t\t//Create new thumbnails");
				_UpdateButtonHandler.Append("\n\t\t\t\t\tMoversFW.Thumbs.CreateThumb(urls.MicroLogoThumbs, " + global.TableFactoryClass + ".Create" + Table + "LogoName(" + global.EntityClassObject + "." + Globals.GetProgramatlyName(ID.Name) + " ), " + logoUploadreID + ".PostedFile, settings.MicroThumnailWidth, settings.MicroThumnailHeight);");
				_UpdateButtonHandler.Append("\n\t\t\t\t\tMoversFW.Thumbs.CreateThumb(urls.MiniLogoThumbs, " + global.TableFactoryClass + ".Create" + Table + "LogoName(" + global.EntityClassObject + "." + Globals.GetProgramatlyName(ID.Name) + " ), " + logoUploadreID + ".PostedFile, settings.MiniThumnailWidth, settings.MiniThumnailHeight);");
				_UpdateButtonHandler.Append("\n\t\t\t\t\tMoversFW.Thumbs.CreateThumb(urls.NormalLogoThumbs, " + global.TableFactoryClass + ".Create" + Table + "LogoName(" + global.EntityClassObject + "." + Globals.GetProgramatlyName(ID.Name) + " ), " + logoUploadreID + ".PostedFile, settings.NormalThumnailWidth, settings.NormalThumnailHeight);");
				_UpdateButtonHandler.Append("\n\t\t\t\t\tMoversFW.Thumbs.CreateThumb(urls.BigLogoThumbs, " + global.TableFactoryClass + ".Create" + Table + "LogoName(" + global.EntityClassObject + "." + Globals.GetProgramatlyName(ID.Name) + " ), " + logoUploadreID + ".PostedFile, settings.BigThumnailWidth, settings.BigThumnailHeight);");
				_UpdateButtonHandler.Append("\n\t\t\t\t}");

			}
			*/
			//------------------------------------------------------------------------
			//_UpdateButtonHandler.Append("\n\t\t\tlblResult.ForeColor=Color.Blue ;");
			//_UpdateButtonHandler.Append("\n\t\t\tlblResult.Text=\" „ «· ⁄œÌ· »‰Ã«Õ\";");
			_UpdateButtonHandler.Append("\n\t\t\t\tResponse.Redirect(\"default.aspx\");");

			_UpdateButtonHandler.Append("\n\t\t\t}");
            if (ProjectBuilder.ISExcuteScaler)
            {
                _UpdateButtonHandler.Append("\n\t\telse if (status == ExecuteCommandStatus.AllreadyExists)");
                _UpdateButtonHandler.Append("\n\t\t{");
                _UpdateButtonHandler.Append("\n\t\t\tlblResult.ForeColor=Color.Red ;");
                _UpdateButtonHandler.Append("\n\t\t\tlblResult.Text = Resources.Admin.DuplicateItem;");
                _UpdateButtonHandler.Append("\n\t\t}");
            }
			_UpdateButtonHandler.Append("\n\t\t\telse");
			_UpdateButtonHandler.Append("\n\t\t\t{");
			_UpdateButtonHandler.Append("\n\t\t\t\tlblResult.ForeColor=Color.Red ;");
			_UpdateButtonHandler.Append("\n\t\t\t\tlblResult.Text= " + ResourcesTesxtsBuilder.AddAdminGlobalText("UpdatingOperationFaild", TextType.Text) + ";");
			_UpdateButtonHandler.Append("\n\t\t\t}");
			_UpdateButtonHandler.Append("\n\t\t}");
			_UpdateButtonHandler.Append("\n\t\telse");
			_UpdateButtonHandler.Append("\n\t\t{");
			_UpdateButtonHandler.Append("\n\t\t\tResponse.Redirect(\"default.aspx\");");
			_UpdateButtonHandler.Append("\n\t\t}");
			_UpdateButtonHandler.Append("\n\t}");
			_UpdateButtonHandler.Append("\n\t//-----------------------------------------------");
			_UpdateButtonHandler.Append("\n\t#endregion");
			return _UpdateButtonHandler.ToString();
		}
		//
		public static void Create(InterfaceType type)
		{
			Update_CodeBehindBuilder cr = new Update_CodeBehindBuilder(type);
			cr.CreateTheFile();
		}
		//

		//
		private string GenerateClassBody()
		{
			string loadData = CreateLoadData();
			StringBuilder classBody = new StringBuilder();
            classBody.Append("\n" + GeneratePageLoad());
            classBody.Append("\n" + DropDownListsLoader.ToString());
			if (ProjectBuilder.HasConfiguration)
			{
				classBody.Append("\n" + CreateHandleOptionalControls());
			} 
			classBody.Append("\n" + loadData);
			classBody.Append("\n" + Create_UpdateButtonHandler());
			return classBody.ToString();
		}
		//
		private void CreateTheFile()
		{
			DirectoryInfo dInfo;
			string path;
			try
			{
				if (Type == InterfaceType.WEbUserControl)
				{
					dInfo = Directory.CreateDirectory(Globals.UserControlsDirectory + global.TableProgramatlyName);
					path = dInfo.FullName + "\\" + ClassName + ".ascx.cs";
				}
				else
				{
					//
					string directoryPath = Globals.AdminFolder + global.TableProgramatlyName + "\\";
					path = directoryPath + "\\Edit.aspx.cs";
					DirectoriesManager.ChechDirectory(directoryPath);
				}
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
