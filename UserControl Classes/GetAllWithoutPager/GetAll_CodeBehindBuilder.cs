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
	public class GetAll_CodeBehindBuilder :CodeBehindBuilder 
	{
		Hashtable allParameters = null;
		//
		public GetAll_CodeBehindBuilder(InterfaceType type)
		{
			Type = type;
			ClassName=global.GetAllCodeBehindClass;
		}
        //
		private string GeneratePageLoad()
		{
            string pageLoadBody = "\n\t\tlblResult.Text=\"\";";
            pageLoadBody += "\n\t\tif(!IsPostBack)";
			pageLoadBody += "\n\t\t{";
			string gridID="dg" + global.TableProgramatlyName;
			pageLoadBody += "\n\t\t\t"+gridID+".Columns["+gridID+".Columns.Count - 2].HeaderText = "+LanguageXmlBuilder.AddText("Update", TextType.Text)+";";
			pageLoadBody += "\n\t\t\t"+gridID+".Columns["+gridID+".Columns.Count - 1].HeaderText = "+LanguageXmlBuilder.AddText("Delete", TextType.Text)+";";
			pageLoadBody += "\n\t\t\tLoadData();";
			pageLoadBody += "\n\t\t};";
            return GeneratePageLoadHandler(pageLoadBody); 
		}
		//
        private string CreateLoadData()
		{
			StringBuilder loadData=new StringBuilder();
			loadData.Append("\n\tprivate void LoadData()");
			loadData.Append("\n\t{");
			loadData.Append("\n\t\tDataTable dtSource= "+global.TableFactoryClass+"."+StoredProcedureTypes.GetAll.ToString()+"();");
			loadData.Append("\n\t\tif(dtSource!=null&&dtSource.Rows.Count >0)");
			loadData.Append("\n\t\t{");
			loadData.Append("\n\t\t\t"+global.ViewAllDataGrid+".DataSource= dtSource;");
			if(ID!=null)
				loadData.Append("\n\t\t\t"+global.ViewAllDataGrid+".DataKeyField=\""+Globals.GetProgramatlyName(ID.Name)+"\";");
			loadData.Append("\n\t\t\tif("+global.ViewAllDataGrid+".PageSize>=dtSource.Rows.Count)");
			loadData.Append("\n\t\t\t{");
			loadData.Append("\n\t\t\t\t"+global.ViewAllDataGrid+".AllowPaging=false;");
			loadData.Append("\n\t\t\t}");
			loadData.Append("\n\t\t\t"+global.ViewAllDataGrid+".DataBind();");
			loadData.Append("\n\t\t\t" + global.ViewAllDataGrid + ".Visible = true;");
			loadData.Append("\n\t\t}");
			loadData.Append("\n\t\telse");
			loadData.Append("\n\t\t{");
			loadData.Append("\n\t\t\t" + global.ViewAllDataGrid + ".Visible=false;");
			loadData.Append("\n\t\t\tlblResult.Text = "+LanguageXmlBuilder.AddText("ThereIsNoData", TextType.Text)+";");
			loadData.Append("\n\t\t}");
			loadData.Append("\n\t}");
			return loadData.ToString();
		}
		//
        private string CreatePageIndexHandler()
		{
            StringBuilder pageIndexHandler=new StringBuilder();
			pageIndexHandler.Append("\n\tprotected void " + global.ViewAllDataGrid + "_PageIndexChanged(object source,DataGridPageChangedEventArgs e)");
			pageIndexHandler.Append("\n\t{");
			pageIndexHandler.Append("\n\t\t"+global.ViewAllDataGrid+".CurrentPageIndex=e.NewPageIndex;");
			pageIndexHandler.Append("\n\t\tLoadData();");
			pageIndexHandler.Append("\n\t}");
			return pageIndexHandler.ToString();
		}
		//
		private string CreateItemDataBoundHandler()
		{
			StringBuilder pageIndexHandler = new StringBuilder();
			pageIndexHandler.Append("\n\tprotected void " + global.ViewAllDataGrid + "_ItemDataBound(object source, DataGridItemEventArgs e)");
			pageIndexHandler.Append("\n\t{");
			pageIndexHandler.Append("\n\t\tif (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)");
			pageIndexHandler.Append("\n\t\t{");
			pageIndexHandler.Append("\n\t\t\tImageButton lbtnDelete = (ImageButton)e.Item.FindControl(\"lbtnDelete\");");
			pageIndexHandler.Append("\n\t\t\tlbtnDelete.Attributes.Add(\"onclick\", \"return confirm('\"+" + LanguageXmlBuilder.AddText("DeleteActivation", TextType.Text) + "+\"')\");");
			pageIndexHandler.Append("\n\t\t\tlbtnDelete.AlternateText = " + LanguageXmlBuilder.AddText("Delete", TextType.Text) + ";");
			
			pageIndexHandler.Append("\n\t\t}");
			pageIndexHandler.Append("\n\t}");
			return pageIndexHandler.ToString();
		}
		//
		private string CreateDeleteCommandHandler()
		{
			string dataGridID = "dg" + global.TableProgramatlyName;
			string id = Globals.GetProgramatlyName(ID.Name);
			id = Globals.ConvetStringToCamelCase(id);
			StringBuilder pageIndexHandler = new StringBuilder();
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
								pageIndexHandler.Append("\n\t\t\tSiteUrls urls = SiteUrls.Instance;");
								siteurlObject = true;
							}
							//--------------------------------------------
							if (column.Name == ProjectBuilder.PhotoExtensionColumnName)
							{		
								pageIndexHandler.Append("\n\t\t\t//Photo-----------------------------");
								pageIndexHandler.Append("\n\t\t\t//Delete old original photo");
								//Add Urls Proprety
								SiteUrlsBuilder.AddDirectoryUrl(Globals.GetProgramatlyName(Table + "OriginalPhotos"), SiteUrlsBuilder.photoOriginalUrl, Globals.GetProgramatlyName(Table), "");
								SiteUrlsBuilder.AddDirectoryUrl(Globals.GetProgramatlyName(Table + "MicroPhotoThumbs"), SiteUrlsBuilder.photoMicroUrl, Globals.GetProgramatlyName(Table), "");
								SiteUrlsBuilder.AddDirectoryUrl(Globals.GetProgramatlyName(Table + "MiniPhotoThumbs"), SiteUrlsBuilder.photoMiniUrl, Globals.GetProgramatlyName(Table), "");
								SiteUrlsBuilder.AddDirectoryUrl(Globals.GetProgramatlyName(Table + "NormalPhotoThumbs"), SiteUrlsBuilder.photoNormalUrl, Globals.GetProgramatlyName(Table), "");
								SiteUrlsBuilder.AddDirectoryUrl(Globals.GetProgramatlyName(Table + "BigPhotoThumbs"), SiteUrlsBuilder.photoBigUrl, Globals.GetProgramatlyName(Table), "");
								pageIndexHandler.Append("\n\t\t\tFile.Delete(Server.MapPath(urls." + Table + "OriginalPhotos) + " + global.EntityClassObject + ".Photo" + ");");
								pageIndexHandler.Append("\n\t\t\t//Delete old Thumbnails");
								pageIndexHandler.Append("\n\t\t\tFile.Delete(Server.MapPath(urls." + Table + "MicroPhotoThumbs) + " + global.TableFactoryClass + ".Create" + Table + "PhotoName(" + id + " ) + MoversFW.Thumbs.thumbnailExetnsion);");
								pageIndexHandler.Append("\n\t\t\tFile.Delete(Server.MapPath(urls." + Table + "MiniPhotoThumbs) + " + global.TableFactoryClass + ".Create" + Table + "PhotoName(" + id + " ) + MoversFW.Thumbs.thumbnailExetnsion);");
								pageIndexHandler.Append("\n\t\t\tFile.Delete(Server.MapPath(urls." + Table + "NormalPhotoThumbs) + " + global.TableFactoryClass + ".Create" + Table + "PhotoName(" + id + " ) + MoversFW.Thumbs.thumbnailExetnsion);");
								pageIndexHandler.Append("\n\t\t\tFile.Delete(Server.MapPath(urls." + Table + "BigPhotoThumbs) + " + global.TableFactoryClass + ".Create" + Table + "PhotoName(" + id + " ) + MoversFW.Thumbs.thumbnailExetnsion);");
								pageIndexHandler.Append("\n\t\t\t//------------------------------------------------");


							}
							else if (column.Name == "LogoExtension")
							{
								pageIndexHandler.Append("\n\t\t\t//Logo-----------------------------");
								pageIndexHandler.Append("\n\t\t\t//Delete old original Logo");
								//Add Urls Proprety
								SiteUrlsBuilder.AddDirectoryUrl(Globals.GetProgramatlyName(Table + "OriginalLogos"), SiteUrlsBuilder.logoOriginalUrl, Globals.GetProgramatlyName(Table), "");
								SiteUrlsBuilder.AddDirectoryUrl(Globals.GetProgramatlyName(Table + "MicroLogoThumbs"), SiteUrlsBuilder.logoMicroUrl, Globals.GetProgramatlyName(Table), "");
								SiteUrlsBuilder.AddDirectoryUrl(Globals.GetProgramatlyName(Table + "MiniLogoThumbs"), SiteUrlsBuilder.logoMiniUrl, Globals.GetProgramatlyName(Table), "");
								SiteUrlsBuilder.AddDirectoryUrl(Globals.GetProgramatlyName(Table + "NormalLogoThumbs"), SiteUrlsBuilder.logoNormalUrl, Globals.GetProgramatlyName(Table), "");
								SiteUrlsBuilder.AddDirectoryUrl(Globals.GetProgramatlyName(Table + "BigLogoThumbs"), SiteUrlsBuilder.logoBigUrl, Globals.GetProgramatlyName(Table), "");
								pageIndexHandler.Append("\n\t\t\tFile.Delete(Server.MapPath(urls." + Table + "OriginalLogos)  + " + global.EntityClassObject + ".Logo" + ");");
								pageIndexHandler.Append("\n\t\t\t//Delete old Thumbnails");
								pageIndexHandler.Append("\n\t\t\tFile.Delete(Server.MapPath(urls." + Table + "MicroLogoThumbs) + " + global.TableFactoryClass + ".Create" + Table + "LogoName(" + id + " ) + MoversFW.Thumbs.thumbnailExetnsion);");
								pageIndexHandler.Append("\n\t\t\tFile.Delete(Server.MapPath(urls." + Table + "MiniLogoThumbs) + " + global.TableFactoryClass + ".Create" + Table + "LogoName(" + id + " ) + MoversFW.Thumbs.thumbnailExetnsion);");
								pageIndexHandler.Append("\n\t\t\tFile.Delete(Server.MapPath(urls." + Table + "NormalLogoThumbs) + " + global.TableFactoryClass + ".Create" + Table + "LogoName(" + id + " ) + MoversFW.Thumbs.thumbnailExetnsion);");
								pageIndexHandler.Append("\n\t\t\tFile.Delete(Server.MapPath(urls." + Table + "BigLogoThumbs) + " + global.TableFactoryClass + ".Create" + Table + "LogoName(" + id + " ) + MoversFW.Thumbs.thumbnailExetnsion);");

							}
							else if (column.Name == "FileExtension")
							{
								pageIndexHandler.Append("\n\t\t\t//File-----------------------------");
								pageIndexHandler.Append("\n\t\t\t//Delete old original file");
								//Add Urls Proprety
								SiteUrlsBuilder.AddDirectoryUrl(Globals.GetProgramatlyName(Table + "OriginalFiles"), SiteUrlsBuilder.filesUrl, Globals.GetProgramatlyName(Table), "");
								pageIndexHandler.Append("\n\t\t\tFile.Delete(Server.MapPath(urls." + Table + "OriginalFiles) + " + global.EntityClassObject + ".File" + ");");
								pageIndexHandler.Append("\n\t\t\t\t//------------------------------------------------");

							}
							else if (column.Name.IndexOf("Extension") > 0)
							{
								string[] stringSeparators = new string[] { "Extension" };
								string[] separatingResult = column.Name.Split(stringSeparators, StringSplitOptions.None);
								string propretyName = separatingResult[0];
								string uploaderID = "fu" + propretyName;
								pageIndexHandler.Append("\n\t\t\t//File-----------------------------");
								//Add Urls Proprety
								SiteUrlsBuilder.AddDirectoryUrl(Globals.GetProgramatlyName(Table + "Original" + propretyName), SiteUrlsBuilder.otherFilesUrl, Globals.GetProgramatlyName(Table), propretyName + "s");
								pageIndexHandler.Append("\n\t\t\t//Delete old original " + propretyName);
								pageIndexHandler.Append("\n\t\t\tFile.Delete(Server.MapPath(urls." + Table + "Original" + propretyName + ") + " + global.EntityClassObject + "." + propretyName + ");");

							}
						}
					}

				}
			}
			#endregion
			//XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX//
			pageIndexHandler.Append("\n\t\t\tlblResult.ForeColor = Color.Blue;");
			pageIndexHandler.Append("\n\t\t\tlblResult.Text = " + LanguageXmlBuilder.AddText("DeletingOprationDone", TextType.Text) + ";");
			pageIndexHandler.Append("\n\t\t\t//if one item in datagrid");
			pageIndexHandler.Append("\n\t\t\tif ("+dataGridID+".Items.Count == 1)");
			pageIndexHandler.Append("\n\t\t\t{");
			pageIndexHandler.Append("\n\t\t\t\t--"+dataGridID+".CurrentPageIndex;");
			pageIndexHandler.Append("\n\t\t\t}");
			pageIndexHandler.Append("\n\t\t\tLoadData();");
			pageIndexHandler.Append("\n\t\t}");
			pageIndexHandler.Append("\n\t\telse");
			pageIndexHandler.Append("\n\t\t{");
            pageIndexHandler.Append("\n\t\t\tlblResult.ForeColor = Color.Red;");
			pageIndexHandler.Append("\n\t\t\tlblResult.Text =" + LanguageXmlBuilder.AddText("DeletingOprationFaild", TextType.Text) + ";");

			pageIndexHandler.Append("\n\t\t}");
			pageIndexHandler.Append("\n\t}");
			return pageIndexHandler.ToString();
			//
		}
		//
		public static void Create(InterfaceType type)
		{
			GetAll_CodeBehindBuilder cr = new GetAll_CodeBehindBuilder(type);
			cr.CreateTheFile();
		}
		//
		public static void Create(InterfaceType type, Hashtable allParameters, string operation)
		{
			GetAll_CodeBehindBuilder cr = new GetAll_CodeBehindBuilder(type);
			Globals global = new Globals();
			cr.ClassName = global.TableProgramatlyName + "_" + operation;
			cr.allParameters = allParameters;
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
