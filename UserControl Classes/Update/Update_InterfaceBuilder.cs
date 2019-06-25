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

		//
		private string photoUploadreID = "fuPhoto";
		private string logoUploadreID = "fuLogo";
		private string fileUploadreID = "fuFile";

		private bool hasPhoto = false;
		private bool hasLogo = false;
		//private bool hasFile = false;

		private string imageName = "imgPhoto";
		private string imageNameTr = "trImgPhoto";
		private string imageNameTrHeader = "trImgPhotoHeader";
		private string logoName = "imgLogo";
		private string logoNameTr= "trImgLogo";
		private string logoNameTrHeader = "trImgLogoHeader";
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
				if (IshasFreeTextBoxControl)
				{
					return "<%@ Page language=\"c#\" ValidateRequest=\"false\" MasterPageFile=\"/App_Design/Admin/Admin.master\" CodeFile=\"Edit.aspx.cs\" Inherits=\"Admin" + ClassName + "\"  %>";

				}
				else
				{
					return "<%@ Page language=\"c#\" MasterPageFile=\"/App_Design/Admin/Admin.master\" CodeFile=\"Edit.aspx.cs\" Inherits=\"Admin" + ClassName + "\"  %>";
				}
			}
		}
        //
        protected  string GenerateControls()
        {
			string propertyName;
			string trPropertyName;
			string datatype;
            bool hasPasswordField = false;
			StringBuilder controls = new StringBuilder();
            controls.Append("\n\t\t\t\t<tr>");
            controls.Append("\n\t\t\t\t\t<td class=\"Result\" align=\"center\" colspan=\"2\">");
            controls.Append("\n\t\t\t\t\t\t<asp:Label id=\"lblResult\" runat=\"server\"></asp:Label>");
            controls.Append("\n\t\t\t\t\t</td>");
            controls.Append("\n\t\t\t\t</tr>");
            foreach (SQLDMO.Column column in Fields)
            {
				propertyName = Globals.GetProgramatlyName(column.Name);
				trPropertyName = "tr" + propertyName;
                //if (ID == null || column.Name != ID.Name)//||!Globals.CheckIsAddedBySql(ID))
				if ((ID == null || column.Name != ID.Name) && (column.Name.IndexOf("_") < 0) && column.Name.ToLower() != ProjectBuilder.LangID)
				{
                    TableConstraint cnstr = SqlProvider.obj.GetParentColumn(column.Name);
                    datatype = Globals.GetAliasDataType(column.Datatype);
                    if (datatype == "bool")
                    {
						if (ProjectBuilder.HasConfiguration)
						{
							controls.Append("\n\t\t\t\t<tr id=\"" + trPropertyName + "\" runat=\"server\" >");
						}
						else
						{
							controls.Append("\n\t\t\t\t<tr>");

						}
						controls.Append("\n\t\t\t\t\t<td class=\"Text\">" + ResourcesTesxtsBuilder.AddUserText(Globals.GetProgramatlyName(column.Name),TextType.HtmlClassic) );
                        if (!column.AllowNulls)
                            controls.Append("<span class=\"RequiredField\"><asp:Label runat=\"server\" Text=\"*\" /></span>");
                        controls.Append("</td>");
                        controls.Append("\n\t\t\t\t\t<td class=\"Control\">");
                        controls.Append("\n\t\t\t\t\t\t<asp:CheckBox id=\"cb" + Globals.GetProgramatlyName(column.Name) + "\" CssClass=\"Controls\" runat=\"server\" ValidationGroup=\"" + Table + "\"></asp:CheckBox>");
                        controls.Append("\n\t\t\t\t\t</td>");
                        controls.Append("\n\t\t\t\t</tr>");
                    }
					else if (Globals.GetSqlDataType(column.Datatype) == SqlDbType.NText)
					{
						if (column.Name.ToLower().IndexOf("details") > -1)
						{
							if (ProjectBuilder.HasConfiguration)
							{
								controls.Append("\n\t\t\t\t<tr id=\"" + trPropertyName + "\" runat=\"server\" >");
							}
							else
							{
								controls.Append("\n\t\t\t\t<tr>");

							}
							controls.Append("\n\t\t\t\t\t<td class=\"Text\">" + ResourcesTesxtsBuilder.AddUserText(Globals.GetProgramatlyName(column.Name), TextType.HtmlClassic));
                            if (!column.AllowNulls)
                                controls.Append("<span class=\"RequiredField\"><asp:Label runat=\"server\" Text=\"*\" /></span>");
                            controls.Append("</td>");
							controls.Append("\n\t\t\t\t\t<td class=\"Control\">");
							//FREETEXTBOX
                            if (ProjectBuilder.IsFreeTextBoxEditor)
                            {
                                controls.Append("\n\t\t\t\t\t\t<FTB:FREETEXTBOX id=\"txt" + Globals.GetProgramatlyName(column.Name) + "\"   runat=\"server\"  TextDirection=\"" + ResourcesTesxtsBuilder.AddAdminGlobalText("EditorDirection", TextType.ServerControl) + "\" ");
                                controls.Append("\n\t\t\t\t\t\tToolbarLayout=\"Bold,Italic,Underline,Strikethrough,Superscript,Subscript;");
                                controls.Append("\n\t\t\t\t\t\tJustifyLeft,JustifyRight,JustifyCenter,JustifyFull;");
                                controls.Append("\n\t\t\t\t\t\tCut,Copy,Paste,Delete,Undo,Redo,Print,Save,ieSpellCheck|");
                                controls.Append("\n\t\t\t\t\t\tParagraphMenu,FontFacesMenu,FontSizesMenu,FontForeColorsMenu,FontBackColorsMenu,FontForeColorPicker,FontBackColorPicker|StyleMenu,SymbolsMenu,InsertHtmlMenu|InsertRule,InsertDate,InsertTime|WordClean|");
                                controls.Append("\n\t\t\t\t\t\tCreateLink,Unlink;RemoveFormat,BulletedList,NumberedList,Indent,Outdent;InsertTable,EditTable,InsertTableRowBefore,InsertTableRowAfter,DeleteTableRow,InsertTableColumnBefore,InsertTableColumnAfter,DeleteTableColumn|\"");
                                controls.Append("\n\t\t\t\t\t\tSupportFolder=\"/phyEditorImages/FreeTextBox/\" ButtonSet=\"NotSet\"  Width=\"450px\" ButtonWidth=\"21\"></FTB:FREETEXTBOX>");						//
                                //-----------
                            }
                            else
                            { 
                                controls.Append("\n\t\t\t\t\t\t<fckeditorv2:fckeditor id=\"txt" + Globals.GetProgramatlyName(column.Name) + "\" runat=\"server\"></fckeditorv2:fckeditor>");
                            }
							controls.Append("\n\t\t\t\t\t</td>");
							controls.Append("\n\t\t\t\t</tr>");

							IshasFreeTextBoxControl = true;
						}
						else
						{
							if (ProjectBuilder.HasConfiguration)
							{
								controls.Append("\n\t\t\t\t<tr id=\"" + trPropertyName + "\" runat=\"server\" >");
							}
							else
							{
								controls.Append("\n\t\t\t\t<tr>");

							}
							controls.Append("\n\t\t\t\t\t<td class=\"Text\">" + ResourcesTesxtsBuilder.AddUserText(Globals.GetProgramatlyName(column.Name), TextType.HtmlClassic) );
                            if (!column.AllowNulls)
                                controls.Append("<span class=\"RequiredField\"><asp:Label runat=\"server\" Text=\"*\" /></span>");
                            controls.Append("</td>");
							controls.Append("\n\t\t\t\t\t<td class=\"Control\">");
							controls.Append("\n\t\t\t\t\t\t<asp:TextBox id=\"txt" + Globals.GetProgramatlyName(column.Name) + "\" runat=\"server\" TextMode=\"MultiLine\" CssClass=\"Controls\" ValidationGroup=\"" + Table + "\" ></asp:TextBox>");
                            
                            //controls.Append("\n\t\t\t\t\t\t<input type=\"text\"  class=\"Controls\"  name=\"txt" + Globals.GetProgramatlyName(column.Name) + "LengthControler\" id=\"txt" + Globals.GetProgramatlyName(column.Name) + "LengthControler\" style=\"width: 40px;\"  disabled>");
							//-----------
							controls.Append("\n\t\t\t\t\t</td>");
							controls.Append("\n\t\t\t\t</tr>");
						
						}
					}
                    else if (datatype != "byte[]" && datatype != "Object" && datatype != "Guid")
                    {
						if (ProjectBuilder.HasConfiguration)
						{
							controls.Append("\n\t\t\t\t<tr id=\"" + trPropertyName + "\" runat=\"server\" >");
						}
						else
						{
							controls.Append("\n\t\t\t\t<tr>");

						}
						controls.Append("\n\t\t\t\t\t<td class=\"Text\">" + ResourcesTesxtsBuilder.AddUserText(Globals.GetProgramatlyName(column.Name), TextType.HtmlClassic) );
                        if (!column.AllowNulls)
                            controls.Append("<span class=\"RequiredField\"><asp:Label runat=\"server\" Text=\"*\" /></span>");
                        controls.Append("</td>");
						controls.Append("\n\t\t\t\t\t<td class=\"Control\">");
                       //XXXXXXXXXXXXXXXXXXXXXXXXXXXXXDownLoadExistFile
						if (cnstr == null)
						{
							if (column.Name.IndexOf(ProjectBuilder.ExtensionInColumnName) > -1)
							{
								if (column.Name == ProjectBuilder.PhotoExtensionColumnName)
								{
									hasPhoto = true;
									
									controls.Append("\n\t\t\t\t\t\t<asp:FileUpload ID=\"" + photoUploadreID + "\" runat=\"server\" CssClass=\"Controls\"  />");
									//if (!column.AllowNulls) 
									//    controls.Append("\n\t\t\t\t\t\t<asp:RequiredFieldValidator Display=\"Dynamic\" id=\"rfv" + Globals.GetProgramatlyName(column.Name) + "\" runat=\"server\" ErrorMessage=\"*\" ControlToValidate=\"" + photoUploadreID + "\" ValidationGroup=\"" + Table + "\" Text=\"*\"></asp:RequiredFieldValidator>");

								}
								else if (column.Name == "LogoExtension")
								{
									hasLogo = true;
									controls.Append("\n\t\t\t\t\t\t<asp:FileUpload ID=\"" + logoUploadreID + "\" runat=\"server\" CssClass=\"Controls\"  />");
									//if (!column.AllowNulls) 
									//    controls.Append("\n\t\t\t\t\t\t<asp:RequiredFieldValidator Display=\"Dynamic\" id=\"rfv" + Globals.GetProgramatlyName(column.Name) + "\" runat=\"server\" ErrorMessage=\"*\" ControlToValidate=\"" + logoUploadreID + "\" ValidationGroup=\"" + Table + "\" Text=\"*\"></asp:RequiredFieldValidator>");
								}
								else if (column.Name == "FileExtension")
								{
									
									controls.Append("\n\t\t\t\t\t\t<asp:FileUpload ID=\"" + fileUploadreID + "\" runat=\"server\" CssClass=\"Controls\"  />");
									controls.Append("\n\t\t\t\t\t\t<asp:HyperLink ID=\"" + fileName + "\" runat=\"server\"   />");
									//if (!column.AllowNulls) 
									//    controls.Append("\n\t\t\t\t\t\t<asp:RequiredFieldValidator Display=\"Dynamic\" id=\"rfv" + Globals.GetProgramatlyName(column.Name) + "\" runat=\"server\" ErrorMessage=\"*\" ControlToValidate=\"" + fileUploadreID + "\" ValidationGroup=\"" + Table + "\" Text=\"*\"></asp:RequiredFieldValidator>");

								}
								else if (column.Name.IndexOf("Extension") > -1)
								{
									string[] stringSeparators = new string[] { "Extension" };
									string[] separatingResult = column.Name.Split(stringSeparators, StringSplitOptions.None);
									propertyName = separatingResult[0];
									string uploaderID = "fu" + propertyName;
									
									controls.Append("\n\t\t\t\t\t\t<asp:FileUpload ID=\"" + uploaderID + "\" runat=\"server\" CssClass=\"Controls\"  />");
									controls.Append("\n\t\t\t\t\t\t<asp:HyperLink ID=\"hl" + propertyName + "\" runat=\"server\"   />");
									//if (!column.AllowNulls) 
									//    controls.Append("\n\t\t\t\t\t\t<asp:RequiredFieldValidator Display=\"Dynamic\" id=\"rfv" + Globals.GetProgramatlyName(column.Name) + "\" runat=\"server\" ErrorMessage=\"*\" ControlToValidate=\"" + uploaderID + "\" ValidationGroup=\"" + Table + "\" Text=\"*\"></asp:RequiredFieldValidator>");
								
								}

								
							}
                            //Check Priority
                            else if (column.Name.ToLower() == ProjectBuilder.PriorityColumnName.ToLower())
                            {
                                controls.Append("\n\t\t\t\t\t\t<asp:DropDownList id=\"" + ProjectBuilder.PriorityDropDownList + "\" runat=\"server\" CssClass=\"Controls\" ValidationGroup=\"" + Table + "\"></asp:DropDownList>");
                            }
                            else
                            {

                                if (column.Name.ToLower().IndexOf("password") > -1)
                                {
                                    hasPasswordField = true;
                                    controls.Append("\n\t\t\t\t\t\t<asp:TextBox MaxLength=\"" + Globals.GetTextBoxMaxLength(column) + "\" id=\"txt" + Globals.GetProgramatlyName(column.Name) + "\" runat=\"server\" TextMode=\"Password\" CssClass=\"Controls\" ValidationGroup=\"" + Table + "\"></asp:TextBox>");
                                    if (datatype == "int" || datatype == "long" || datatype == "short")
                                    {
                                        controls.Append("\n\t\t\t\t\t\t<asp:RegularExpressionValidator Display=\"Dynamic\" ID=\"rev" + Globals.GetProgramatlyName(column.Name) + "\" runat=\"server\" ControlToValidate=\"txt" + Globals.GetProgramatlyName(column.Name) + "\" ErrorMessage=\"\" " + ResourcesTesxtsBuilder.AddAdminGlobalText("InvalidNumericalData", TextType.ServerControl) + " ValidationGroup=\"" + Table + "\" ValidationExpression=\"\\d*\"></asp:RegularExpressionValidator>");
                                    }
                                    else if (column.Name.ToLower().IndexOf("email") > -1)
                                    {
                                        controls.Append("\n\t\t\t\t\t\t<asp:RegularExpressionValidator Display=\"Dynamic\" ID=\"rev" + Globals.GetProgramatlyName(column.Name) + "\" runat=\"server\" ControlToValidate=\"txt" + Globals.GetProgramatlyName(column.Name) + "\" ErrorMessage=\"\" Text=\"" + ResourcesTesxtsBuilder.AddAdminGlobalText("InvalidEmail", TextType.ServerControl) + "\" ValidationGroup=\"" + Table + "\" ValidationExpression=\"\\w+([-+.']\\w+)*@\\w+([-.]\\w+)*\\.\\w+([-.]\\w+)*\"></asp:RegularExpressionValidator>");

                                    }

                                }
                                else if (column.Length > 128)
                                {
                                    controls.Append("\n\t\t\t\t\t\t<asp:TextBox MaxLength=\"" + Globals.GetTextBoxMaxLength(column) + "\" id=\"txt" + Globals.GetProgramatlyName(column.Name) + "\" runat=\"server\" TextMode=\"MultiLine\" CssClass=\"Controls\" ValidationGroup=\"" + Table + "\"  maxlengthS=\"" + Globals.GetTextBoxMaxLength(column) + "\" onkeyup=\"return ismaxlength(this,document.forms[0].txt" + Globals.GetProgramatlyName(column.Name) + "LengthControler)\"  onfocus=\"return ismaxlength(this,document.forms[0].txt" + Globals.GetProgramatlyName(column.Name) + "LengthControler)\"></asp:TextBox>");
                                    controls.Append("\n\t\t\t\t\t\t<input type=\"text\"  class=\"Controls\"  name=\"txt" + Globals.GetProgramatlyName(column.Name) + "LengthControler\" id=\"txt" + Globals.GetProgramatlyName(column.Name) + "LengthControler\" style=\"width: 40px;\"  disabled>");
                                    if (datatype == "int" || datatype == "long" || datatype == "short")
                                    {
                                        controls.Append("\n\t\t\t\t\t\t<asp:RegularExpressionValidator Display=\"Dynamic\" ID=\"rev" + Globals.GetProgramatlyName(column.Name) + "\" runat=\"server\" ControlToValidate=\"txt" + Globals.GetProgramatlyName(column.Name) + "\" ErrorMessage=\"\" ValidationGroup=\"" + Table + "\" ValidationExpression=\"\\d*\"></asp:RegularExpressionValidator>");
                                    }
                                    else if (column.Name.ToLower().IndexOf("email") > -1)
                                    {
                                        controls.Append("\n\t\t\t\t\t\t<asp:RegularExpressionValidator Display=\"Dynamic\" ID=\"rev" + Globals.GetProgramatlyName(column.Name) + "\" runat=\"server\" ControlToValidate=\"txt" + Globals.GetProgramatlyName(column.Name) + "\" ErrorMessage=\"\" ValidationGroup=\"" + Table + "\" ValidationExpression=\"\\w+([-+.']\\w+)*@\\w+([-.]\\w+)*\\.\\w+([-.]\\w+)*\"></asp:RegularExpressionValidator>");

                                    }
                                }
                                else
                                {
                                    controls.Append("\n\t\t\t\t\t\t<asp:TextBox MaxLength=\"" + Globals.GetTextBoxMaxLength(column) + "\" id=\"txt" + Globals.GetProgramatlyName(column.Name) + "\" runat=\"server\" CssClass=\"Controls\" ValidationGroup=\"" + Table + "\"></asp:TextBox>");
                                    if (datatype == "int" || datatype == "long" || datatype == "short")
                                    {
                                        controls.Append("\n\t\t\t\t\t\t<asp:RegularExpressionValidator Display=\"Dynamic\" ID=\"rev" + Globals.GetProgramatlyName(column.Name) + "\" runat=\"server\" ControlToValidate=\"txt" + Globals.GetProgramatlyName(column.Name) + "\" ErrorMessage=\"\" ValidationGroup=\"" + Table + "\" ValidationExpression=\"\\d*\"></asp:RegularExpressionValidator>");
                                    }
                                    else if (column.Name.ToLower().IndexOf("email") > -1)
                                    {
                                        controls.Append("\n\t\t\t\t\t\t<asp:RegularExpressionValidator Display=\"Dynamic\" ID=\"rev" + Globals.GetProgramatlyName(column.Name) + "\" runat=\"server\" ControlToValidate=\"txt" + Globals.GetProgramatlyName(column.Name) + "\" ErrorMessage=\"\" ValidationGroup=\"" + Table + "\" ValidationExpression=\"\\w+([-+.']\\w+)*@\\w+([-.]\\w+)*\\.\\w+([-.]\\w+)*\"></asp:RegularExpressionValidator>");

                                    }
                                }

                                if (!column.AllowNulls)
                                    controls.Append("\n\t\t\t\t\t\t<asp:RequiredFieldValidator Display=\"Dynamic\" id=\"rfv" + Globals.GetProgramatlyName(column.Name) + "\" runat=\"server\" ErrorMessage=\"*\" ControlToValidate=\"txt" + Globals.GetProgramatlyName(column.Name) + "\" ValidationGroup=\"" + Table + "\" Text=\"*\"></asp:RequiredFieldValidator>");

                            }
						}
						else
						{
							
							controls.Append("\n\t\t\t\t\t\t<asp:DropDownList id=\"ddl" + Globals.GetProgramatlyName(cnstr.ParentTable) + "\" runat=\"server\" CssClass=\"Controls\" ValidationGroup=\"" + Table + "\"></asp:DropDownList>");
							if (!column.AllowNulls)
								controls.Append("\n\t\t\t\t\t\t<asp:RequiredFieldValidator Display=\"Dynamic\" id=\"rfv" + Globals.GetProgramatlyName(column.Name) + "\" runat=\"server\" ErrorMessage=\"*\" ControlToValidate=\"ddl" + Globals.GetProgramatlyName(cnstr.ParentTable) + "\" InitialValue=\"-1\" ValidationGroup=\"" + Table + "\" Text=\"*\"></asp:RequiredFieldValidator>");
						}
                        controls.Append("\n\t\t\t\t\t</td>");
                        controls.Append("\n\t\t\t\t</tr>");
                        //--------------------------------------------
                        //Confirm Password
                        //--------------------------------------------
                        if (hasPasswordField)
                        {
                            string propertyConfirm = propertyName + "Confirm";
                            controls.Append("\n\t\t\t\t\t<td class=\"Text\">" + ResourcesTesxtsBuilder.AddUserText(propertyConfirm, TextType.HtmlClassic) );
                            if (!column.AllowNulls)
                                controls.Append("<span class=\"RequiredField\"><asp:Label runat=\"server\" Text=\"*\" /></span>");
                            controls.Append("</td>");
                            controls.Append("\n\t\t\t\t\t<td class=\"Control\">");
                            controls.Append("\n\t\t\t\t\t\t<asp:TextBox MaxLength=\"" + Globals.GetTextBoxMaxLength(column) + "\"  id=\"txt" + propertyConfirm + "\" runat=\"server\" TextMode=\"Password\" CssClass=\"Controls\" ValidationGroup=\"" + Table + "\"></asp:TextBox>");
                            if (!column.AllowNulls)
                                controls.Append("\n\t\t\t\t\t\t<asp:RequiredFieldValidator Display=\"Dynamic\" id=\"rfv" + propertyConfirm + "\" runat=\"server\" ErrorMessage=\"*\" ControlToValidate=\"txt" + propertyName + "\" ValidationGroup=\"" + Table + "\" Text=\"*\"></asp:RequiredFieldValidator>");
                            controls.Append("\n\t\t\t\t\t\t <asp:CompareValidator ID=\"cv" + propertyConfirm + "\" runat=\"server\" ControlToCompare=\"txt" + propertyName + "\" ControlToValidate=\"txt" + propertyConfirm + "\" ErrorMessage=\"" + ResourcesTesxtsBuilder.AddUserText("InvalidConfirmPassord", TextType.ServerControl) + "\" Text=\"" + ResourcesTesxtsBuilder.AddUserText("InvalidConfirmPassord", TextType.ServerControl) + "\" ValidationGroup=\"" + Table + "\"></asp:CompareValidator>");
                            controls.Append("\n\t\t\t\t\t</td>");
                            controls.Append("\n\t\t\t\t</tr>");
                            hasPasswordField = false;
                        }
                    }
                }
            }
            controls.Append("\n\t\t\t\t<tr>");
            controls.Append("\n\t\t\t\t\t<td class=\"Result\" align=\"center\" colspan=\"2\">");
            //New
            controls.Append("\n\t\t\t\t\t\t<asp:HyperLink ID=\"hlCancel\" ImageUrl=\"/App_Design/Admin/Icons/Buttons/Cancel.jpg\" runat=\"server\" Text=\" " + ResourcesTesxtsBuilder.AddAdminGlobalText("Cancel", TextType.ServerControl) + "\" NavigateUrl=\"default.aspx\" ></asp:HyperLink>");
            controls.Append("\n\t\t\t\t\t\t<asp:ImageButton ID=\"ibtnUpdate\" ImageUrl=\"/App_Design/Admin/Icons/Buttons/Update.gif\" runat=\"server\" AlternateText=\"" + ResourcesTesxtsBuilder.AddAdminGlobalText("Update", TextType.ServerControl) + "\" OnClick=\"ibtnUpdate_Click\" ValidationGroup=\"" + Table + "\" />");
            //OLD
            //controls.Append("\n\t\t\t\t\t\t<asp:Button id=\"btnUpdate\" runat=\"server\" Width=\"100px\" Text=\"" + ResourcesTesxtsBuilder.AddAdminGlobalText("Update", TextType.ServerControl) + "\" CssClass=\"Button\" OnClick=\"btnUpdate_Click\" ValidationGroup=\"" + Table + "\"></asp:Button>");
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
			tableStart.Append("\n\t\t\t\t\t<td align=\"center\" class=\"Title\" colspan=\"" + mainTableColumsCount + "\">" + ResourcesTesxtsBuilder.AddUserText(Globals.GetProgramatlyName(HeaderTitle), TextType.HtmlClassic) + "</td>");
            tableStart.Append("\n\t\t\t\t</tr>");
			tableStart.Append("\n\t\t\t\t<tr>");
			
			tableStart.Append("\n\t\t\t\t\t<td>");
            tableStart.Append("\n\t\t\t<table class=\"SubTable\" cellspacing=\"0\" cellpadding=\"0\" width=\"100%\" border=\"0\" >");
   
            StringBuilder tableEnd = new StringBuilder(); 
            //---------------------------
            //End of sub table
            tableEnd.Append("\n\t\t\t\t</table>");
            //---------------------------
			tableEnd.Append("\n\t\t\t</td>");
			if (hasPhoto && hasLogo)
			{
				tableEnd.Append("\n\t\t\t<td valign=\"top\">");
				 tableEnd.Append("\n\t\t\t<table class=\"SubTable\" cellspacing=\"0\" cellpadding=\"0\" width=\"100%\" border=\"0\">");
				 tableEnd.Append("\n\t\t\t\t<tr id=\"" + imageNameTrHeader + "\" runat=\"server\" >");
				 tableEnd.Append("\n\t\t\t\t\t<td class=\"Imagetext\">" + ResourcesTesxtsBuilder.AddAdminGlobalText("Photo", TextType.HtmlClassic) + "</td>");
				 tableEnd.Append("\n\t\t\t\t</tr>");
				 tableEnd.Append("\n\t\t\t\t<tr id=\"" + imageNameTr + "\" runat=\"server\" >");
				 tableEnd.Append("\n\t\t\t\t\t<td class=\"ControlCenter\"><asp:Image ID=\"" + imageName + "\" runat=\"server\" /></td>");
				 tableEnd.Append("\n\t\t\t\t</tr>");
				 tableEnd.Append("\n\t\t\t\t<tr id=\"" + logoNameTrHeader + "\" runat=\"server\" >");
				 tableEnd.Append("\n\t\t\t\t\t<td class=\"Imagetext\">" + ResourcesTesxtsBuilder.AddAdminGlobalText("Logo", TextType.HtmlClassic) + "</td>");
				 tableEnd.Append("\n\t\t\t\t</tr>");
				 tableEnd.Append("\n\t\t\t\t<tr id=\"" + logoNameTr + "\" runat=\"server\" >");
				 tableEnd.Append("\n\t\t\t\t\t<td class=\"ControlCenter\"> <asp:Image ID=\"" + logoName + "\" runat=\"server\" /></td>");
				 tableEnd.Append("\n\t\t\t\t</tr>");
                 tableEnd.Append("\n\t\t\t\t</table>");
				tableEnd.Append("\n\t\t\t</td>");
			}
			else if (hasPhoto)
			{

				tableEnd.Append("\n\t\t\t<td valign=\"top\">");
				tableEnd.Append("\n\t\t\t<table class=\"SubTable\" cellspacing=\"0\" cellpadding=\"0\" width=\"100%\" border=\"0\">");
				tableEnd.Append("\n\t\t\t\t<tr id=\"" + imageNameTrHeader + "\" runat=\"server\" >");
				tableEnd.Append("\n\t\t\t\t\t<td class=\"Imagetext\">" + ResourcesTesxtsBuilder.AddAdminGlobalText("Photo", TextType.HtmlClassic) + "</td>");
				tableEnd.Append("\n\t\t\t\t</tr>");
				tableEnd.Append("\n\t\t\t\t<tr id=\"" + imageNameTr + "\" runat=\"server\" >");
				tableEnd.Append("\n\t\t\t\t\t<td class=\"ControlCenter\"><asp:Image ID=\"" + imageName + "\" runat=\"server\" /></td>");
				tableEnd.Append("\n\t\t\t\t</tr>");
				tableEnd.Append("\n\t\t\t\t</table>");
				tableEnd.Append("\n\t\t\t</td>");
			}
			else if (hasLogo)
			{
				tableEnd.Append("\n\t\t\t<td valign=\"top\">");
				tableEnd.Append("\n\t\t\t<table class=\"SubTable\" cellspacing=\"0\" cellpadding=\"0\" width=\"100%\" border=\"0\">");
				tableEnd.Append("\n\t\t\t\t<tr id=\"" + logoNameTrHeader + "\" runat=\"server\" >");
				tableEnd.Append("\n\t\t\t\t\t<td class=\"Imagetext\">" + ResourcesTesxtsBuilder.AddAdminGlobalText("Logo", TextType.HtmlClassic) + "</td>");
				tableEnd.Append("\n\t\t\t\t</tr>");
				tableEnd.Append("\n\t\t\t\t<tr id=\"" + logoNameTr + "\" runat=\"server\" >");
				tableEnd.Append("\n\t\t\t\t\t<td class=\"ControlCenter\"> <asp:Image ID=\"" + logoName + "\" runat=\"server\" /></td>");
				tableEnd.Append("\n\t\t\t\t</tr>");
				tableEnd.Append("\n\t\t\t\t</table>");
				tableEnd.Append("\n\t\t\t</td>");
			}
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
			string contentsTable = GenerateContentsTable();
			StringBuilder userControl = new StringBuilder();
			userControl.Append(GenerateDirective());
			userControl.Append("\n" + ControlRegisters);
			userControl.Append("\n" + contentsTable);
			//
			DirectoryInfo dInfo = Directory.CreateDirectory(Globals.UserControlsDirectory + global.TableProgramatlyName);
			string path = dInfo.FullName + "\\" + ClassName + ".ascx";
			//
			FileManager.CreateFile(path, userControl.ToString());
			
		}
		//
		private void GeneratePageInterface()
		{
			string contentsTable =GenerateContentsTable();
			StringBuilder userControl = new StringBuilder();
			userControl.Append(GenerateDirective());
			userControl.Append("\n" + ControlRegisters);
			userControl.Append("\n <asp:Content id=\"Content1\" ContentPlaceHolderID=\"BasicContents\" runat=\"server\">");
			userControl.Append("\n" + contentsTable);
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
