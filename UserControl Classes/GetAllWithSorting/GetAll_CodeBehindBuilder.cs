using System;
using System.IO;
using System.Text ;
using System.Windows.Forms;
using System.Collections;
namespace SPGen
{
	/// <summary>
	/// Summary description for Ceate_CodeBehindBuilder.
	/// </summary>
	public class GetAllWithSorting_CodeBehindBuilder :CodeBehindBuilder 
	{
		//
        public GetAllWithSorting_CodeBehindBuilder(InterfaceType type)
		{
			Type = type;
			ClassName=global.GetAllWithSortingCodeBehindClass;
		}
        //
		private string GeneratePageLoad()
		{
            string pageLoadBody = "\n\t\tlblResult.Text=\"\";";
            pageLoadBody += "\n\t\tif(!IsPostBack)";
			pageLoadBody += "\n\t\t{";
            if (ProjectBuilder.HasConfiguration)
            {
                pageLoadBody +="\n\t\t\tSiteOptions.CheckModuleWithHandling(Resources.SiteOptions." + SiteOptionsBuilder.GetHasPropertyString(Table) + ", ViewerType.Admin);";
            }
            pageLoadBody += "\n\t\t\tPagerManager.PrepareAdminPager(pager);";
            pageLoadBody += "\n\t\t\tpager.Visible = false;";
			string gridID="dg" + global.TableProgramatlyName;
            //pageLoadBody += "\n\t\t\t" + gridID + ".Columns[" + gridID + ".Columns.Count - 2].HeaderText = " + ResourcesTesxtsBuilder.AddAdminGlobalText("Update", TextType.Text) + ";";
            //pageLoadBody += "\n\t\t\t" + gridID + ".Columns[" + gridID + ".Columns.Count - 1].HeaderText = " + ResourcesTesxtsBuilder.AddAdminGlobalText("Delete", TextType.Text) + ";";

            pageLoadBody += "\n\t\t\tViewState[\"SortExpression\"] = \"\";";
            pageLoadBody += "\n\t\t\tViewState[\"SortDir\"] = SortDir.Undefined;";
            pageLoadBody += "\n\t\t\tLoadData(\"\", SortDir.Undefined);"; 
			pageLoadBody += "\n\t\t}";
            return GeneratePageLoadHandler(pageLoadBody); 
		}
		//
        private string CreateLoadData()
		{
			StringBuilder loadData=new StringBuilder();
			loadData.Append("\n\t#region --------------LoadData--------------");
			loadData.Append("\n\t//---------------------------------------------------------");
			loadData.Append("\n\t//LoadData");
			loadData.Append("\n\t//---------------------------------------------------------");


            loadData.Append("\n\tprivate void LoadData(string sortExpression,SortDir dir)");
			loadData.Append("\n\t{");
			loadData.Append("\n\t\tpager.PageSize = SiteSettings.Instance.AdminPageSize;");
			loadData.Append("\n\t\tint totalRecords = 0;");
            string methodParameters = "";
            string methodName = StoredProcedureTypes.GetAllWithPagerAndSorting.ToString();
            bool hasIsAvailable = SqlProvider.CheckIsATableHasIsAvailableColumnName(SqlProvider.obj.TableName);

            if (hasIsAvailable)
                methodName += "ForAdmin";
            bool isMaultiLanguages = (ProjectBuilder.HasMultiLanguages && SqlProvider.CheckISATableIsMultiLanguage(SqlProvider.obj.TableName));
            if (isMaultiLanguages)
            {
                if (!hasIsAvailable)
                {
                    loadData.Append("\n\t\tLanguages langID = (Languages)ResourceManager.GetCurrentLanguage();");
                    methodParameters = "langID,";
                }
            }
            methodParameters += "pager.CurrentPage, pager.PageSize, out totalRecords, sortExpression, dir";

            loadData.Append("\n\t\tList<" + global.TableEntityClass + "> " + global.EntityClassList + "= " + global.TableFactoryClass + "." + methodName + "(" + methodParameters + ");");
            loadData.Append("\n\t\tif(" + global.EntityClassList + "!=null&&" + global.EntityClassList + ".Count >0)");
			loadData.Append("\n\t\t{");
			loadData.Append("\n\t\t\t"+global.ViewAllDataGrid+".DataSource= " + global.EntityClassList +  ";");
			if(ID!=null)
				loadData.Append("\n\t\t\t"+global.ViewAllDataGrid+".DataKeyField=\""+Globals.GetProgramatlyName(ID.Name)+"\";");

			loadData.Append("\n\t\t\t"+global.ViewAllDataGrid+".AllowPaging=false;");
			//--------
			loadData.Append("\n\t\t\tpager.Visible = true;");
			loadData.Append("\n\t\t\tpager.TotalRecords = totalRecords;");
			
			//--------
			loadData.Append("\n\t\t\t"+global.ViewAllDataGrid+".DataBind();");
			loadData.Append("\n\t\t\t" + global.ViewAllDataGrid + ".Visible = true;");
			loadData.Append("\n\t\t}");
			loadData.Append("\n\t\telse");
			loadData.Append("\n\t\t{");
			loadData.Append("\n\t\t\t" + global.ViewAllDataGrid + ".Visible=false;");
			loadData.Append("\n\t\t\tpager.Visible = false;");
			loadData.Append("\n\t\t\tlblResult.ForeColor = Color.Red;");
			loadData.Append("\n\t\t\tlblResult.Text = " + ResourcesTesxtsBuilder.AddAdminGlobalText("ThereIsNoData", TextType.Text) + ";");
			loadData.Append("\n\t\t}");
			loadData.Append("\n\t}");
			loadData.Append("\n\t//--------------------------------------------------------");
			loadData.Append("\n\t#endregion");
			return loadData.ToString();
		}
		//
        private string CreatePageIndexHandler()
		{
            StringBuilder pageIndexHandler=new StringBuilder();
			pageIndexHandler.Append("\n\t#region --------------Pager_PageIndexChang--------------");
			pageIndexHandler.Append("\n\t//---------------------------------------------------------");
			pageIndexHandler.Append("\n\t//Pager_PageIndexChang");
			pageIndexHandler.Append("\n\t//---------------------------------------------------------");

			
			pageIndexHandler.Append("\n\tprotected void Pager_PageIndexChang()");
			pageIndexHandler.Append("\n\t{");
            pageIndexHandler.Append("\n\t\tLoadData((string)ViewState[\"SortExpression\"], (SortDir)ViewState[\"SortDir\"]);");
			pageIndexHandler.Append("\n\t}");
			pageIndexHandler.Append("\n\t//--------------------------------------------------------");
			pageIndexHandler.Append("\n\t#endregion"); 
			return pageIndexHandler.ToString();
		}
		//
		private string CreateItemDataBoundHandler()
		{
			StringBuilder pageIndexHandler = new StringBuilder();
			pageIndexHandler.Append("\n\t#region --------------" + global.ViewAllDataGrid + "_ItemDataBound--------------");
			pageIndexHandler.Append("\n\t//---------------------------------------------------------");
			pageIndexHandler.Append("\n\t//" + global.ViewAllDataGrid + "_ItemDataBound");
			pageIndexHandler.Append("\n\t//---------------------------------------------------------");

			
			pageIndexHandler.Append("\n\tprotected void " + global.ViewAllDataGrid + "_ItemDataBound(object source, DataGridItemEventArgs e)");
			pageIndexHandler.Append("\n\t{");
			pageIndexHandler.Append("\n\t\tif (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)");
			pageIndexHandler.Append("\n\t\t{");
			pageIndexHandler.Append("\n\t\t\tImageButton lbtnDelete = (ImageButton)e.Item.FindControl(\"lbtnDelete\");");
			pageIndexHandler.Append("\n\t\t\tlbtnDelete.Attributes.Add(\"onclick\", \"return confirm('\"+" + ResourcesTesxtsBuilder.AddAdminGlobalText("DeleteActivation", TextType.Text) + "+\"')\");");
			pageIndexHandler.Append("\n\t\t\tlbtnDelete.AlternateText = " + ResourcesTesxtsBuilder.AddAdminGlobalText("Delete", TextType.Text) + ";");
			
			pageIndexHandler.Append("\n\t\t}");
            pageIndexHandler.Append("\n\t\t\telse if(e.Item.ItemType==ListItemType.Header)");
            pageIndexHandler.Append("\n\t\t{");
            pageIndexHandler.Append("\n\t\tOurDataGrid.AddSortImage(" + global.ViewAllDataGrid + ", e.Item, (string)ViewState[\"SortExpression\"], (SortDir)ViewState[\"SortDir\"]);");
            pageIndexHandler.Append("\n\t\t}");
			pageIndexHandler.Append("\n\t}");
			pageIndexHandler.Append("\n\t//--------------------------------------------------------");
			pageIndexHandler.Append("\n\t#endregion");
			return pageIndexHandler.ToString();
		}
		//
		private string CreateDeleteCommandHandler()
		{
			string dataGridID = "dg" + global.TableProgramatlyName;
			string id = Globals.GetProgramatlyName(ID.Name);
			id = Globals.ConvetStringToCamelCase(id);
			StringBuilder pageIndexHandler = new StringBuilder();
			pageIndexHandler.Append("\n\t#region --------------" + dataGridID + "_DeleteCommand--------------");
			pageIndexHandler.Append("\n\t//---------------------------------------------------------");
			pageIndexHandler.Append("\n\t//" + dataGridID + "_DeleteCommand");
			pageIndexHandler.Append("\n\t//---------------------------------------------------------");

			
			pageIndexHandler.Append("\n\tprotected void " + dataGridID + "_DeleteCommand(object source, DataGridCommandEventArgs e)");
			pageIndexHandler.Append("\n\t{");
			pageIndexHandler.Append("\n\t\t" + Globals.GetAliasDataType(ID.Datatype) + " " + id + " = Convert.To" + Globals.GetDataType(ID.Datatype) + "("+dataGridID+".DataKeys[e.Item.ItemIndex]);");
			pageIndexHandler.Append("\n\t\t" + global.TableEntityClass + " " + global.EntityClassObject + " =" + global.TableFactoryClass + ".Get" + global.TableProgramatlyName + "Object(" + id + ");");
			pageIndexHandler.Append("\n\t\tif(" + global.TableFactoryClass + "." + MethodType.Delete.ToString() + "(" + id + "))");
			pageIndexHandler.Append("\n\t\t{");
			//XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX//
			#region XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX
			string datatype;
			bool siteurlObject = false;
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
							if (column.Name.IndexOf("Extension") > 0 && !siteurlObject)
							{
								pageIndexHandler.Append("\n\t\t\t"+SiteUrlsBuilder.GetFullDeclaration());
								siteurlObject = true;
							}
							//--------------------------------------------
							if (column.Name == ProjectBuilder.PhotoExtensionColumnName)
							{		
								pageIndexHandler.Append("\n\t\t\t//Photo-----------------------------");
								pageIndexHandler.Append("\n\t\t\t//Delete old original photo");
								//Add Urls Property
								SiteUrlsBuilder.AddDirectoryUrl( "OriginalPhotos", SiteUrlsBuilder.photoOriginalUrl, Globals.GetProgramatlyName(Table), "");
								SiteUrlsBuilder.AddDirectoryUrl( "MicroPhotoThumbs", SiteUrlsBuilder.photoMicroUrl, Globals.GetProgramatlyName(Table), "");
								SiteUrlsBuilder.AddDirectoryUrl( "MiniPhotoThumbs", SiteUrlsBuilder.photoMiniUrl, Globals.GetProgramatlyName(Table), "");
								SiteUrlsBuilder.AddDirectoryUrl( "NormalPhotoThumbs", SiteUrlsBuilder.photoNormalUrl, Globals.GetProgramatlyName(Table), "");
								SiteUrlsBuilder.AddDirectoryUrl( "BigPhotoThumbs", SiteUrlsBuilder.photoBigUrl, Globals.GetProgramatlyName(Table), "");
								pageIndexHandler.Append("\n\t\t\tFile.Delete(Server.MapPath(urls.OriginalPhotos) + " + global.EntityClassObject + ".Photo" + ");");
								pageIndexHandler.Append("\n\t\t\t//Delete old Thumbnails");
								pageIndexHandler.Append("\n\t\t\tFile.Delete(Server.MapPath(urls." +  "MicroPhotoThumbs) + " + global.TableFactoryClass + ".Create" + Table + "PhotoName(" + id + " ) + MoversFW.Thumbs.thumbnailExetnsion);");
								pageIndexHandler.Append("\n\t\t\tFile.Delete(Server.MapPath(urls." +  "MiniPhotoThumbs) + " + global.TableFactoryClass + ".Create" + Table + "PhotoName(" + id + " ) + MoversFW.Thumbs.thumbnailExetnsion);");
								pageIndexHandler.Append("\n\t\t\tFile.Delete(Server.MapPath(urls." +  "NormalPhotoThumbs) + " + global.TableFactoryClass + ".Create" + Table + "PhotoName(" + id + " ) + MoversFW.Thumbs.thumbnailExetnsion);");
								pageIndexHandler.Append("\n\t\t\tFile.Delete(Server.MapPath(urls." +  "BigPhotoThumbs) + " + global.TableFactoryClass + ".Create" + Table + "PhotoName(" + id + " ) + MoversFW.Thumbs.thumbnailExetnsion);");
								pageIndexHandler.Append("\n\t\t\t//------------------------------------------------");


							}
							else if (column.Name == "LogoExtension")
							{
								pageIndexHandler.Append("\n\t\t\t//Logo-----------------------------");
								pageIndexHandler.Append("\n\t\t\t//Delete old original Logo");
								//Add Urls Property
								SiteUrlsBuilder.AddDirectoryUrl( "OriginalLogos", SiteUrlsBuilder.logoOriginalUrl, Globals.GetProgramatlyName(Table), "");
								SiteUrlsBuilder.AddDirectoryUrl( "MicroLogoThumbs", SiteUrlsBuilder.logoMicroUrl, Globals.GetProgramatlyName(Table), "");
								SiteUrlsBuilder.AddDirectoryUrl( "MiniLogoThumbs", SiteUrlsBuilder.logoMiniUrl, Globals.GetProgramatlyName(Table), "");
								SiteUrlsBuilder.AddDirectoryUrl( "NormalLogoThumbs", SiteUrlsBuilder.logoNormalUrl, Globals.GetProgramatlyName(Table), "");
								SiteUrlsBuilder.AddDirectoryUrl( "BigLogoThumbs", SiteUrlsBuilder.logoBigUrl, Globals.GetProgramatlyName(Table), "");
								pageIndexHandler.Append("\n\t\t\tFile.Delete(Server.MapPath(urls.OriginalLogos)  + " + global.EntityClassObject + ".Logo" + ");");
								pageIndexHandler.Append("\n\t\t\t//Delete old Thumbnails");
								pageIndexHandler.Append("\n\t\t\tFile.Delete(Server.MapPath(urls.MicroLogoThumbs) + " + global.TableFactoryClass + ".Create" + Table + "LogoName(" + id + " ) + MoversFW.Thumbs.thumbnailExetnsion);");
								pageIndexHandler.Append("\n\t\t\tFile.Delete(Server.MapPath(urls.MiniLogoThumbs) + " + global.TableFactoryClass + ".Create" + Table + "LogoName(" + id + " ) + MoversFW.Thumbs.thumbnailExetnsion);");
								pageIndexHandler.Append("\n\t\t\tFile.Delete(Server.MapPath(urls.NormalLogoThumbs) + " + global.TableFactoryClass + ".Create" + Table + "LogoName(" + id + " ) + MoversFW.Thumbs.thumbnailExetnsion);");
								pageIndexHandler.Append("\n\t\t\tFile.Delete(Server.MapPath(urls.BigLogoThumbs) + " + global.TableFactoryClass + ".Create" + Table + "LogoName(" + id + " ) + MoversFW.Thumbs.thumbnailExetnsion);");

							}
							else if (column.Name == "FileExtension")
							{
								pageIndexHandler.Append("\n\t\t\t//File-----------------------------");
								pageIndexHandler.Append("\n\t\t\t//Delete old original file");
								//Add Urls Property
								SiteUrlsBuilder.AddDirectoryUrl( "OriginalFiles", SiteUrlsBuilder.filesUrl, Globals.GetProgramatlyName(Table), "");
								pageIndexHandler.Append("\n\t\t\tFile.Delete(Server.MapPath(urls.OriginalFiles) + " + global.EntityClassObject + ".File" + ");");
								pageIndexHandler.Append("\n\t\t\t\t//------------------------------------------------");

							}
							else if (column.Name.IndexOf("Extension") > 0)
							{
								string[] stringSeparators = new string[] { "Extension" };
								string[] separatingResult = column.Name.Split(stringSeparators, StringSplitOptions.None);
								string propertyName = separatingResult[0];
								string uploaderID = "fu" + propertyName;
								pageIndexHandler.Append("\n\t\t\t//File-----------------------------");
								//Add Urls Property
								SiteUrlsBuilder.AddDirectoryUrl( "Original" + propertyName, SiteUrlsBuilder.otherFilesUrl, Globals.GetProgramatlyName(Table), propertyName + "s");
								pageIndexHandler.Append("\n\t\t\t//Delete old original " + propertyName);
								pageIndexHandler.Append("\n\t\t\tFile.Delete(Server.MapPath(urls.Original" + propertyName + ") + " + global.EntityClassObject + "." + propertyName + ");");

							}
						}
					}

				}
			}
			#endregion
			//XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX//
			pageIndexHandler.Append("\n\t\t\tlblResult.ForeColor = Color.Blue;");
			pageIndexHandler.Append("\n\t\t\tlblResult.Text = " + ResourcesTesxtsBuilder.AddAdminGlobalText("DeletingOprationDone", TextType.Text) + ";");
			pageIndexHandler.Append("\n\t\t\t//if one item in datagrid");
			pageIndexHandler.Append("\n\t\t\tif ("+dataGridID+".Items.Count == 1)");
			pageIndexHandler.Append("\n\t\t\t{");
			pageIndexHandler.Append("\n\t\t\t\t--pager.CurrentPage;");
			pageIndexHandler.Append("\n\t\t\t}");
            pageIndexHandler.Append("\n\t\t\tLoadData((string)ViewState[\"SortExpression\"], (SortDir)ViewState[\"SortDir\"]);");
			pageIndexHandler.Append("\n\t\t}");
			pageIndexHandler.Append("\n\t\telse");
			pageIndexHandler.Append("\n\t\t{");
            pageIndexHandler.Append("\n\t\t\tlblResult.ForeColor = Color.Red;");
			pageIndexHandler.Append("\n\t\t\tlblResult.Text =" + ResourcesTesxtsBuilder.AddAdminGlobalText("DeletingOprationFaild", TextType.Text) + ";");
			pageIndexHandler.Append("\n\t\t}");
			pageIndexHandler.Append("\n\t}");
			pageIndexHandler.Append("\n\t//--------------------------------------------------------");
			pageIndexHandler.Append("\n\t#endregion"); 
			return pageIndexHandler.ToString();
			//
		}
		//
        private string CreateSortCommandHandler()
        {
            string dataGridID = "dg" + global.TableProgramatlyName;
            string handlerName = dataGridID + "_SortCommand";
            StringBuilder pageIndexHandler = new StringBuilder();
            pageIndexHandler.Append("\n\t#region --------------" + handlerName + "--------------");
            pageIndexHandler.Append("\n\t//---------------------------------------------------------");
            pageIndexHandler.Append("\n\t//" + handlerName);
            pageIndexHandler.Append("\n\t//---------------------------------------------------------");

            pageIndexHandler.Append("\n\tprotected void " + handlerName + "(object source, DataGridSortCommandEventArgs e)");
            pageIndexHandler.Append("\n\t{");
            pageIndexHandler.Append("\n\t\tstring sortExpression = (string)ViewState[\"SortExpression\"];");
            pageIndexHandler.Append("\n\t\tSortDir dir =(SortDir)ViewState[\"SortDir\"];");
            pageIndexHandler.Append("\n\t\tif (sortExpression != e.SortExpression)");
            pageIndexHandler.Append("\n\t\t{");
            pageIndexHandler.Append("\n\t\t\tsortExpression = e.SortExpression;");
            pageIndexHandler.Append("\n\t\t\tdir = SortDir.Asc;");
            pageIndexHandler.Append("\n\t\t}");
            pageIndexHandler.Append("\n\t\telse");
            pageIndexHandler.Append("\n\t\t{");
            pageIndexHandler.Append("\n\t\t\tif (dir == SortDir.Asc)");
            pageIndexHandler.Append("\n\t\t\t\tdir = SortDir.Desc;");
            pageIndexHandler.Append("\n\t\t\telse");
            pageIndexHandler.Append("\n\t\t\t\tdir = SortDir.Asc;");
            pageIndexHandler.Append("\n\t\t}");

            pageIndexHandler.Append("\n\t\tViewState[\"SortExpression\"] = sortExpression;");
            pageIndexHandler.Append("\n\t\tViewState[\"SortDir\"] = dir;");
            pageIndexHandler.Append("\n\t\tpager.CurrentPage = 1;");
            pageIndexHandler.Append("\n\t\tLoadData(sortExpression, dir);");
            pageIndexHandler.Append("\n\t}");
            pageIndexHandler.Append("\n\t//--------------------------------------------------------");
            pageIndexHandler.Append("\n\t#endregion");
            return pageIndexHandler.ToString();
        }

        //
		public static void Create(InterfaceType type)
		{
			GetAllWithSorting_CodeBehindBuilder cr = new GetAllWithSorting_CodeBehindBuilder(type);
			cr.CreateTheFile();
		}
        //
        private string GenerateClassBody()
        {
            StringBuilder classBody = new StringBuilder();
            classBody.Append(GeneratePageLoad());
            classBody.Append("\n" + CreateLoadData());
			classBody.Append("\n" + CreateItemDataBoundHandler());
            classBody.Append("\n" + CreatePageIndexHandler());
			classBody.Append("\n" + CreateDeleteCommandHandler());
            classBody.Append("\n" + CreateSortCommandHandler());
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
					path = directoryPath + "\\default.aspx.cs";
					DirectoriesManager.ChechDirectory(directoryPath);
				}
				// Create the file.
                string _class = GenerateClass(GenerateClassBody());
                FileManager.CreateFile(path, _class);
				//CREATE THE WEB FORM    
				
			}
			catch(Exception ex)
			{
				MessageBox.Show("My Generated Code Exception:"+ex.Message);
				
			}
		}

		
	}
}
