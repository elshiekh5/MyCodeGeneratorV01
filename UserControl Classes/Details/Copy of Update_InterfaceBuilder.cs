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
	public class Update_InterfaceBuilder : InterfaceBuilder
    {
		Hashtable allParameters = null;
		//
		private string photoUploadreID = "fuPhoto";
		private string logoUploadreID = "fuLogo";
		private string fileUploadreID = "fuFile";

		private bool hasPhoto = false;
		private bool hasLogo = false;
		//private bool hasFile = false;

		private string imageName = "imgPhoto";
		private string logoName = "imgLogo";
		private string fileName = "hlFile";
        //
		public Update_InterfaceBuilder(InterfaceType type)
        {
			ClassName = global.UpdateCodeBehindClass;
			HeaderTitle = global.HeaderTitle_Update;
			Type = type;
        }
		//
		private string GenerateDirective()
		{
			if (Type == InterfaceType.WEbUserControl)
			{
				return "<%@ Control Language=\"c#\" AutoEventWireup=\"true\" CodeFile=\"" + ClassName + ".ascx.cs\" Inherits=\"" + ClassName + "\" %>";
			}
			else
			{
				return "<%@ Page language=\"c#\" MasterPageFile=\"~/Masters/AdminMasterPage.master\" CodeFile=\"Edit.aspx.cs\" Inherits=\"Admin" + ClassName + "\" Theme=\"AdminSite\" %>";
			}
		}
        //
        protected  string GenerateControls()
        {

            StringBuilder controls = new StringBuilder();
            controls.Append("\n\t\t\t\t<tr>");
            controls.Append("\n\t\t\t\t\t<td class=\"Result\" align=\"center\" colspan=\"2\">");
            controls.Append("\n\t\t\t\t\t\t<asp:Label id=\"lblResult\" runat=\"server\"></asp:Label>");
            controls.Append("\n\t\t\t\t\t</td>");
            controls.Append("\n\t\t\t\t</tr>");
            string datatype;
            foreach (SQLDMO.Column column in Fields)
            {
				if (allParameters != null && !allParameters.Contains(column.Name))
					continue;
                //if (ID == null || column.Name != ID.Name)//||!Globals.CheckIsAddedBySql(ID))
				if ((ID == null || column.Name != ID.Name) && (column.Name.IndexOf("_") < 0) && column.Name.ToLower() != ProjectBuilder.LangID)
				{
                    TableConstraint cnstr = SqlProvider.obj.GetParentColumn(column.Name);
                    datatype = Globals.GetAliasDataType(column.Datatype);
                    if (datatype == "bool")
                    {
                        controls.Append("\n\t\t\t\t<tr>");
						controls.Append("\n\t\t\t\t\t<td class=\"Text\"><%=LanguageAdminResource.GetString(\"" + Globals.GetProgramatlyName(column.Name) + "\")%></td>");
                        controls.Append("\n\t\t\t\t\t<td class=\"Control\">");
						controls.Append("\n\t\t\t\t\t\t<asp:CheckBox id=\"cb" + Globals.GetProgramatlyName(column.Name) + "\" runat=\"server\" ValidationGroup=\"" + ClassName + "\"></asp:CheckBox>");
                        controls.Append("\n\t\t\t\t\t</td>");
                        controls.Append("\n\t\t\t\t</tr>");
                    }
					else if (Globals.GetSqlDataType(column.Datatype) == SqlDbType.NText)
					{
						controls.Append("\n\t\t\t\t<tr>");
						controls.Append("\n\t\t\t\t\t<td class=\"Text\"><%=LanguageAdminResource.GetString(\"" + Globals.GetProgramatlyName(column.Name) + "\")%></td>");
						controls.Append("\n\t\t\t\t\t<td class=\"Control\">");
						//FREETEXTBOX
						controls.Append("\n\t\t\t\t\t\t<FTB:FREETEXTBOX id=\"ftb" + Globals.GetProgramatlyName(column.Name) + "\"   runat=\"server\"  TextDirection=\"RightToLeft\" ");
						controls.Append("\n\t\t\t\t\t\tToolbarLayout=\"Bold,Italic,Underline,Strikethrough,Superscript,Subscript;");
						controls.Append("\n\t\t\t\t\t\tJustifyLeft,JustifyRight,JustifyCenter,JustifyFull;");
						controls.Append("\n\t\t\t\t\t\tCut,Copy,Paste,Delete,Undo,Redo,Print,Save,ieSpellCheck|");
						controls.Append("\n\t\t\t\t\t\tParagraphMenu,FontFacesMenu,FontSizesMenu,FontForeColorsMenu,FontBackColorsMenu,FontForeColorPicker,FontBackColorPicker|StyleMenu,SymbolsMenu,InsertHtmlMenu|InsertRule,InsertDate,InsertTime|WordClean|");
						controls.Append("\n\t\t\t\t\t\tCreateLink,Unlink;RemoveFormat,BulletedList,NumberedList,Indent,Outdent;InsertTable,EditTable,InsertTableRowBefore,InsertTableRowAfter,DeleteTableRow,InsertTableColumnBefore,InsertTableColumnAfter,DeleteTableColumn|\"");
                        controls.Append("\n\t\t\t\t\t\tSupportFolder=\"/phyEditorImages/FreeTextBox/\" ButtonSet=\"NotSet\"  Width=\"450px\" ButtonWidth=\"21\"></FTB:FREETEXTBOX>");						//
						//-----------
                        controls.Append("\n\t\t\t\t\t</td>");
						controls.Append("\n\t\t\t\t</tr>");
						
						IshasFreeTextBoxControl = true;
					}
                    else if (datatype != "byte[]" && datatype != "Object" && datatype != "Guid")
                    {
                       //XXXXXXXXXXXXXXXXXXXXXXXXXXXXX
						if (cnstr == null)
						{
							if (column.Name.IndexOf(ProjectBuilder.ExtensionInColumnName) > -1)
							{
								if (column.Name == ProjectBuilder.PhotoExtensionColumnName)
								{
									controls.Append("\n\t\t\t\t<tr>");
									controls.Append("\n\t\t\t\t\t<td class=\"Text\"><%=LanguageAdminResource.GetString(\"" + Globals.GetProgramatlyName(column.Name) + "\")%></td>");
									controls.Append("\n\t\t\t\t\t<td class=\"Control\">");
									controls.Append("\n\t\t\t\t\t\t<asp:FileUpload ID=\"" + photoUploadreID + "\" runat=\"server\" CssClass=\"Controls\"  />");

								}
								else if (column.Name == "LogoExtension")
								{
									controls.Append("\n\t\t\t\t<tr>");
									controls.Append("\n\t\t\t\t\t<td class=\"Text\"><%=LanguageAdminResource.GetString(\"" + Globals.GetProgramatlyName(column.Name) + "\")%></td>");
									controls.Append("\n\t\t\t\t\t<td class=\"Control\">");
									controls.Append("\n\t\t\t\t\t\t<asp:FileUpload ID=\"" + logoUploadreID + "\" runat=\"server\" CssClass=\"Controls\"  />");

								}
								else if (column.Name == "FileExtension")
								{
									controls.Append("\n\t\t\t\t<tr runat=\"server\" id=\"trFile\">");
									controls.Append("\n\t\t\t\t\t<td class=\"Text\"><%=LanguageAdminResource.GetString(\"" + Globals.GetProgramatlyName(column.Name) + "\")%></td>");
									controls.Append("\n\t\t\t\t\t<td class=\"Control\">");
									controls.Append("\n\t\t\t\t\t\t<asp:FileUpload ID=\"" + fileUploadreID + "\" runat=\"server\" CssClass=\"Controls\"  />");

								}
								else if (column.Name.IndexOf("Extension") > -1)
								{
									string[] stringSeparators = new string[] { "Extension" };
									string[] separatingResult = column.Name.Split(stringSeparators, StringSplitOptions.None);
									string propretyName = separatingResult[0];
									string uploaderID = "fu" + propretyName;
									controls.Append("\n\t\t\t\t<tr runat=\"server\" id=\"tr" + propretyName + ">");
									controls.Append("\n\t\t\t\t\t<td class=\"Text\"><%=LanguageAdminResource.GetString(\"" + Globals.GetProgramatlyName(column.Name) + "\")%></td>");
									controls.Append("\n\t\t\t\t\t<td class=\"Control\">");
									controls.Append("\n\t\t\t\t\t\t<asp:FileUpload ID=\"" + uploaderID + "\" runat=\"server\" CssClass=\"Controls\"  />");

								}

								if (!column.AllowNulls)
								{
									controls.Append("\n\t\t\t\t\t\t<asp:RequiredFieldValidator id=\"rfv" + Globals.GetProgramatlyName(column.Name) + "\" runat=\"server\" ErrorMessage=\"*\" ControlToValidate=\"txt" + Globals.GetProgramatlyName(column.Name) + "\" ValidationGroup=\"" + ClassName + "\">*</asp:RequiredFieldValidator>");
								}
							}
							else
							{
								controls.Append("\n\t\t\t\t<tr>");
								controls.Append("\n\t\t\t\t\t<td class=\"Text\"><%=LanguageAdminResource.GetString(\"" + Globals.GetProgramatlyName(column.Name) + "\")%></td>");
								controls.Append("\n\t\t\t\t\t<td class=\"Control\">");
								if (column.Name.ToLower().IndexOf("password") > -1)
								{
									controls.Append("\n\t\t\t\t\t\t<asp:TextBox id=\"txt" + Globals.GetProgramatlyName(column.Name) + "\" runat=\"server\" TextMode=\"Password\" CssClass=\"Control\" ValidationGroup=\"" + ClassName + "\"></asp:TextBox>");

								}
								else if (column.Length > 128)
								{
									controls.Append("\n\t\t\t\t\t\t<asp:TextBox id=\"txt" + Globals.GetProgramatlyName(column.Name) + "\" runat=\"server\" TextMode=\"MultiLine\" CssClass=\"Control\" ValidationGroup=\"" + ClassName + "\"></asp:TextBox>");
								}
								else
								{
									controls.Append("\n\t\t\t\t\t\t<asp:TextBox id=\"txt" + Globals.GetProgramatlyName(column.Name) + "\" runat=\"server\" CssClass=\"Control\" ValidationGroup=\"" + ClassName + "\"></asp:TextBox>");

								}

								if (!column.AllowNulls)
									controls.Append("\n\t\t\t\t\t\t<asp:RequiredFieldValidator id=\"rfv" + Globals.GetProgramatlyName(column.Name) + "\" runat=\"server\" ErrorMessage=\"*\" ControlToValidate=\"txt" + Globals.GetProgramatlyName(column.Name) + "\" ValidationGroup=\"" + ClassName + "\">*</asp:RequiredFieldValidator>");

							}
						}
						else
						{
							controls.Append("\n\t\t\t\t<tr>");
							controls.Append("\n\t\t\t\t\t<td class=\"Text\"><%=LanguageAdminResource.GetString(\"" + Globals.GetProgramatlyName(column.Name) + "\")%></td>");
							controls.Append("\n\t\t\t\t\t<td class=\"Control\">");
							controls.Append("\n\t\t\t\t\t\t<asp:DropDownList id=\"ddl" + Globals.GetProgramatlyName(cnstr.ParentTable) + "\" runat=\"server\" CssClass=\"Control\" ValidationGroup=\"" + ClassName + "\"></asp:DropDownList>");
						}
                        controls.Append("\n\t\t\t\t\t</td>");
                        controls.Append("\n\t\t\t\t</tr>");
                    }
                }
            }
            controls.Append("\n\t\t\t\t<tr>");
            controls.Append("\n\t\t\t\t\t<td class=\"Result\" align=\"center\" colspan=\"2\">");
			controls.Append("\n\t\t\t\t\t\t<asp:Button id=\"btnUpdate\" runat=\"server\" Width=\"100px\" Text=\"\" CssClass=\"Button\" OnClick=\"btnUpdate_Click\" ValidationGroup=\"" + ClassName + "\"></asp:Button>");
            controls.Append("\n\t\t\t\t\t</td>");
            controls.Append("\n\t\t\t\t</tr>");

            //

            return controls.ToString();

        }

        //----------------------------------------------
		public static void Create(InterfaceType type)
        {
			Update_InterfaceBuilder cr = new Update_InterfaceBuilder(type);
			cr.GenerateInterface();
        }
		//
		public static void Create(InterfaceType type, Hashtable allParameters, string operation)
		{
			Update_InterfaceBuilder cr = new Update_InterfaceBuilder(type);
			Globals global = new Globals();
			cr.ClassName = global.TableProgramatlyName + "_" + operation;
			cr.allParameters = allParameters;
			cr.GenerateInterface();
		}
		//
		private void GenerateInterface()
		{
			if (Type == InterfaceType.WEbUserControl)
			{
				GenerateUserControlInterface();
			}
			else
			{
				GeneratePageInterface();
			}
		}
        private string GenerateContentsTable()
        {
            string controlsRows = GenerateControls();
            StringBuilder contentsTable = new StringBuilder(); 

            int mainTableColumsCount = 1;
            if (hasPhoto||hasLogo)
            {
                mainTableColumsCount = 2;
            }
            StringBuilder tableStart = new StringBuilder(); 
            tableStart.Append("\n\t\t\t<table class=\"MainTable\" cellspacing=\"0\" cellpadding=\"0\" width=\"100%\" border=\"0\" >");
            tableStart.Append("\n\t\t\t\t<tr>");
            tableStart.Append("\n\t\t\t\t\t  <td align=\"center\" class=\"Title\" colspan=\"" + mainTableColumsCount + "\"><%=LanguageAdminResource.GetString(\"" + Globals.GetProgramatlyName(HeaderTitle) + "\")%></td>");
            tableStart.Append("\n\t\t\t\t</tr>");
			tableStart.Append("\n\t\t\t\t<tr>");
			if (hasPhoto&&hasLogo)
			{
				
				tableStart.Append("\n\t\t\t<td valign=\"top\">");
				tableStart.Append("\n\t\t\t\t<asp:Image ID=\"" + imageName + "\" runat=\"server\" />");
				tableStart.Append("\n\t\t\t\t<asp:Image ID=\"" + logoName + "\" runat=\"server\" />");
				tableStart.Append("\n\t\t\t</td>");
			}
			else if (hasPhoto)
			{

				tableStart.Append("\n\t\t\t<td valign=\"top\">");
				tableStart.Append("\n\t\t\t\t<asp:Image ID=\"" + imageName + "\" runat=\"server\" />");
				tableStart.Append("\n\t\t\t</td>");
			}
			else if (hasLogo)
			{
				tableStart.Append("\n\t\t\t<td valign=\"top\">");
				tableStart.Append("\n\t\t\t\t<asp:Image ID=\"" + logoName + "\" runat=\"server\" />");
				tableStart.Append("\n\t\t\t</td>");
			}
			tableStart.Append("\n\t\t\t\t\t<td>");
            tableStart.Append("\n\t\t\t<table class=\"SubTable\" cellspacing=\"0\" cellpadding=\"0\" width=\"100%\" border=\"0\" >");
   
            StringBuilder tableEnd = new StringBuilder(); 
            //---------------------------
            //End of sub table
            tableEnd.Append("\n\t\t\t\t</table>");
            //---------------------------
			tableEnd.Append("\n\t\t\t</td>");
             tableEnd.Append("\n\t\t</tr>");
             tableEnd.Append("\n\t</table>");
             //---------------------------
             contentsTable.Append(tableStart.ToString());
             contentsTable.Append(controlsRows);
             contentsTable.Append(tableEnd.ToString());
             return contentsTable.ToString();
         }
		//
		private void GenerateUserControlInterface()
		{
			//Begin create Control and check the free text box editor
			StringBuilder userControl = new StringBuilder();
			userControl.Append(GenerateDirective());
			userControl.Append("\n" + ControlRegisters);
            userControl.Append("\n" + GenerateContentsTable());


			//
			DirectoryInfo dInfo = Directory.CreateDirectory(Globals.UserControlsDirectory + global.TableProgramatlyName);
			string path = dInfo.FullName + "\\" + ClassName + ".ascx";
			//
			FileManager.CreateFile(path, userControl.ToString());

		}
		//
		private void GeneratePageInterface()
		{
			StringBuilder userControl = new StringBuilder();
			userControl.Append(GenerateDirective());
			userControl.Append("\n" + ControlRegisters);
			userControl.Append("\n <asp:Content id=\"Content1\" ContentPlaceHolderID=\"BasicContent\" runat=\"server\">");
			userControl.Append("\n" + GenerateContentsTable());
			userControl.Append("\n</asp:Content>");
			//
			string directoryPath = Globals.AdminFolder + global.TableProgramatlyName + "\\";
			string path = directoryPath + "\\Edit.aspx";
			DirectoriesManager.ChechDirectory(directoryPath);
			FileManager.CreateFile(path, userControl.ToString());
		}
		//

    }
}
