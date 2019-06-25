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
	public class Details_InterfaceBuilder : InterfaceBuilder
    {
		//
		private bool hasPhoto = false;
		private bool hasLogo = false;
		private bool hasDetails = false;
		private string detailsLabel = "";

		private string imageName = "ltrPhoto";
		private string logoName = "ltrLogo";
		private string fileName = "hlFile";
        //
		private string LogoContainerTr = "trLogoContainer";
		private string PhotoContainerTr = "trPhotoContainer";
		//
		public Details_InterfaceBuilder()
        {
			ClassName = global.TableProgramatlyName+"_Details";
        }
		//
		private string GenerateDirective()
		{
			return "<%@ Control Language=\"c#\" AutoEventWireup=\"true\" CodeFile=\"" + ClassName + ".ascx.cs\" Inherits=\"" + ClassName + "\" %>";
		}
        //
        protected  string GenerateControls()
        {

            StringBuilder controls = new StringBuilder();
			string datatype;
            foreach (SQLDMO.Column column in Fields)
            {
                //if (ID == null || column.Name != ID.Name)//||!Globals.CheckIsAddedBySql(ID))
				if ((ID == null || column.Name != ID.Name) && (column.Name.IndexOf("_") < 0)
                     && column.Name.ToLower().IndexOf("password") < 0
                  && column.Name.ToLower().IndexOf("shortdescription") < 0
                  && column.Name.ToLower().IndexOf(ProjectBuilder.PriorityColumnName.ToLower()) < 0
                  && column.Name.ToLower().IndexOf(ProjectBuilder.IsAvailable.ToLower()) < 0 
                  && column.Name.ToLower() != ProjectBuilder.LangID)
				{
                    TableConstraint cnstr = SqlProvider.obj.GetParentColumn(column.Name);
                    datatype = Globals.GetAliasDataType(column.Datatype);
					if (datatype == "bool")
					{
						controls.Append("\n\t\t\t\t<tr id=\"tr" + Globals.GetProgramatlyName(column.Name) + "\" runat=\"server\">");
						controls.Append("\n\t\t\t\t\t<td class=\"text\">");
						controls.Append("\n\t\t\t\t\t\t<span class=\"text\">");
						controls.Append("\n\t\t\t\t\t\t\t" + ResourcesTesxtsBuilder.AddUserText(Globals.GetProgramatlyName(column.Name), TextType.HtmlClassic));
						controls.Append("\n\t\t\t\t\t\t</span>");
						controls.Append("\n\t\t\t\t\t\t<span class=\"value\">");
						controls.Append("\n\t\t\t\t\t\t\t<asp:Label id=\"lbl" + Globals.GetProgramatlyName(column.Name) + "\" runat=\"server\" ></asp:Label>");
						controls.Append("\n\t\t\t\t\t\t</span>");
						controls.Append("\n\t\t\t\t\t</td>");
						controls.Append("\n\t\t\t\t</tr>");
					}
					else if (Globals.GetSqlDataType(column.Datatype) == SqlDbType.NText)
					{
                        if (column.Name.ToLower().IndexOf("details") > -1 || column.Name.ToLower().IndexOf("description") > -1)
                        {
                            hasDetails = true;
                            detailsLabel = "lbl" + Globals.GetProgramatlyName(column.Name);
                        }
                        else
                        {
                            controls.Append("\n\t\t\t\t<tr id=\"tr" + Globals.GetProgramatlyName(column.Name) + "\" runat=\"server\">");
                            controls.Append("\n\t\t\t\t\t<td class=\"text\">");
                            controls.Append("\n\t\t\t\t\t\t<span class=\"text\">");
                            controls.Append("\n\t\t\t\t\t\t\t" + ResourcesTesxtsBuilder.AddUserText(Globals.GetProgramatlyName(column.Name), TextType.HtmlClassic));
                            controls.Append("\n\t\t\t\t\t\t</span>");
                            controls.Append("\n\t\t\t\t\t\t<br />");
                            controls.Append("\n\t\t\t\t\t\t<span class=\"value\">");
                            controls.Append("\n\t\t\t\t\t\t\t<asp:Label id=\"lbl" + Globals.GetProgramatlyName(column.Name) + "\" runat=\"server\" ></asp:Label>");
                            controls.Append("\n\t\t\t\t\t\t</span>");
                            controls.Append("\n\t\t\t\t\t</td>");
                            controls.Append("\n\t\t\t\t</tr>");
                        }
					}
					else if (datatype != "byte[]" && datatype != "Object" && datatype != "Guid" && column.Name.ToLower().IndexOf("password") < 0 && column.Name.ToLower() != ProjectBuilder.LangID)
					{
						
						if (column.Name.IndexOf(ProjectBuilder.ExtensionInColumnName) > -1)
							{
								if (column.Name == ProjectBuilder.PhotoExtensionColumnName)
								{
									hasPhoto = true;
								}
								else if (column.Name == "LogoExtension")
								{
									hasLogo = true;
								}
								else if (column.Name == "FileExtension")
								{
									controls.Append("\n\t\t\t\t<tr id=\"tr" + Globals.GetProgramatlyName(column.Name) + "\" runat=\"server\">");
									controls.Append("\n\t\t\t\t\t<td class=\"text\">");
									controls.Append("\n\t\t\t\t\t\t<span class=\"text\">");
									controls.Append("\n\t\t\t\t\t\t\t" + ResourcesTesxtsBuilder.AddUserText(Globals.GetProgramatlyName(column.Name), TextType.HtmlClassic));
									controls.Append("\n\t\t\t\t\t\t</span>");
									controls.Append("\n\t\t\t\t\t\t<span class=\"value\">");
									controls.Append("\n\t\t\t\t\t\t\t<asp:HyperLink ID=\"" + fileName + "\" runat=\"server\" />");
									controls.Append("\n\t\t\t\t\t\t</span>");
									controls.Append("\n\t\t\t\t\t</td>");
									controls.Append("\n\t\t\t\t</tr>");
								}
								else if (column.Name.IndexOf("Extension") > -1)
								{

									string[] stringSeparators = new string[] { "Extension" };
									string[] separatingResult = column.Name.Split(stringSeparators, StringSplitOptions.None);
									string propertyName = separatingResult[0];
									controls.Append("\n\t\t\t\t<tr id=\"tr" + Globals.GetProgramatlyName(column.Name) + "\" runat=\"server\">");
									controls.Append("\n\t\t\t\t\t<td class=\"text\">");
									controls.Append("\n\t\t\t\t\t\t<span class=\"text\">");
									controls.Append("\n\t\t\t\t\t\t\t" + ResourcesTesxtsBuilder.AddUserText(Globals.GetProgramatlyName(column.Name), TextType.HtmlClassic));
									controls.Append("\n\t\t\t\t\t\t</span>");
									controls.Append("\n\t\t\t\t\t\t<span class=\"value\">"); 
									controls.Append("\n\t\t\t\t\t\t\t<asp:HyperLink ID=\"hl" + propertyName + "\" runat=\"server\"   />");
									controls.Append("\n\t\t\t\t\t\t</span>");
									controls.Append("\n\t\t\t\t\t</td>");
									controls.Append("\n\t\t\t\t</tr>");
								}


							}
							else
							{
								controls.Append("\n\t\t\t\t<tr id=\"tr" + Globals.GetProgramatlyName(column.Name) + "\" runat=\"server\">");
								controls.Append("\n\t\t\t\t\t<td class=\"text\">");
								controls.Append("\n\t\t\t\t\t\t<span class=\"text\">");
								controls.Append("\n\t\t\t\t\t\t\t" + ResourcesTesxtsBuilder.AddUserText(Globals.GetProgramatlyName(column.Name), TextType.HtmlClassic));
								controls.Append("\n\t\t\t\t\t\t</span>");
								controls.Append("\n\t\t\t\t\t\t<span class=\"value\">");
								controls.Append("\n\t\t\t\t\t\t\t<asp:Label id=\"lbl" + Globals.GetProgramatlyName(column.Name) + "\" runat=\"server\" ></asp:Label>");
								controls.Append("\n\t\t\t\t\t\t</span>");
								controls.Append("\n\t\t\t\t\t</td>");
								controls.Append("\n\t\t\t\t</tr>");
						}
							
					}
                }
            }
			
			//controls.Append("\n\t\t\t\t\t</td>");
			//controls.Append("\n\t\t\t\t</tr>");
            //
            return controls.ToString();

        }
		private string GenerateContentsTable()
		{
			string controlsRows = GenerateControls();
			StringBuilder contentsTable = new StringBuilder();


			contentsTable.Append("\n<table cellspacing=\"0\"   cellpadding=\"0\" border=\"0\"  width=\"100%\"  class=\"DetailsTable\">");
			contentsTable.Append("\n\t<tr>");
			contentsTable.Append("\n\t\t<td>");
			#region logo or photo
			if (hasPhoto && hasLogo)
			{
				contentsTable.Append("\n\t\t\t<table cellspacing=\"0\" cellpadding=\"0\" border=\"0\"   class=\"ImageContainerTable\" align=\"left\">");
				contentsTable.Append("\n\t\t\t\t<tr id=\""+PhotoContainerTr+"\" runat=\"server\">");
				contentsTable.Append("\n\t\t\t\t\t<td align=\"center\">");
				contentsTable.Append("\n\t\t\t\t\t\t<table cellpadding=\"0\" cellspacing=\"0\" class=\"ImageContainer\">");
				contentsTable.Append("\n\t\t\t\t\t\t\t<tr>");
                contentsTable.Append("\n\t\t\t\t\t\t\t\t<td><asp:Literal ID=\"" + imageName + "\" runat=\"server\" /></td>");
				contentsTable.Append("\n\t\t\t\t\t\t\t</tr>");
				contentsTable.Append("\n\t\t\t\t\t\t</table>");
				contentsTable.Append("\n\t\t\t\t\t</td>");
				contentsTable.Append("\n\t\t\t\t</tr>");
				contentsTable.Append("\n\t\t\t\t<tr id=\""+LogoContainerTr+"\" runat=\"server\">");
				contentsTable.Append("\n\t\t\t\t\t<td align=\"center\">");
				contentsTable.Append("\n\t\t\t\t\t\t<table cellpadding=\"0\" cellspacing=\"0\" class=\"ImageContainer\">");
				contentsTable.Append("\n\t\t\t\t\t\t\t<tr>");
                contentsTable.Append("\n\t\t\t\t\t\t\t\t<td><asp:Literal ID=\"" + logoName + "\" runat=\"server\" /></td>"); contentsTable.Append("\n\t\t\t\t\t\t\t</tr>");
				contentsTable.Append("\n\t\t\t\t\t\t</table>");
				contentsTable.Append("\n\t\t\t\t\t</td>");
				contentsTable.Append("\n\t\t\t\t</tr>");
				contentsTable.Append("\n\t\t\t</table>");
			}
			else if (hasPhoto)
			{

				contentsTable.Append("\n\t\t\t<table cellspacing=\"0\" cellpadding=\"0\" border=\"0\"   class=\"ImageContainerTable\" align=\"left\">");
				contentsTable.Append("\n\t\t\t\t<tr id=\""+PhotoContainerTr+"\" runat=\"server\">");
				contentsTable.Append("\n\t\t\t\t\t<td align=\"center\">");
				contentsTable.Append("\n\t\t\t\t\t\t<table cellpadding=\"0\" cellspacing=\"0\" class=\"ImageContainer\">");
				contentsTable.Append("\n\t\t\t\t\t\t\t<tr>");
                contentsTable.Append("\n\t\t\t\t\t\t\t\t<td><asp:Literal ID=\"" + imageName + "\" runat=\"server\" /></td>");
                contentsTable.Append("\n\t\t\t\t\t\t\t</tr>");
				contentsTable.Append("\n\t\t\t\t\t\t</table>");
				contentsTable.Append("\n\t\t\t\t\t</td>");
				contentsTable.Append("\n\t\t\t\t</tr>");
				contentsTable.Append("\n\t\t\t</table>");
			}
			else if (hasLogo)
			{
				contentsTable.Append("\n\t\t\t<table cellspacing=\"0\" cellpadding=\"0\" border=\"0\"   class=\"ImageContainerTable\" align=\"left\">");
				contentsTable.Append("\n\t\t\t\t<tr id=\""+LogoContainerTr+"\" runat=\"server\">");
				contentsTable.Append("\n\t\t\t\t\t<td align=\"center\">");
				contentsTable.Append("\n\t\t\t\t\t\t<table cellpadding=\"0\" cellspacing=\"0\" class=\"ImageContainer\">");
				contentsTable.Append("\n\t\t\t\t\t\t\t<tr>");
                contentsTable.Append("\n\t\t\t\t\t\t\t\t<td><asp:Literal ID=\"" + logoName + "\" runat=\"server\" /></td>"); contentsTable.Append("\n\t\t\t\t\t\t\t</tr>");
                contentsTable.Append("\n\t\t\t\t\t\t\t</tr>");
				contentsTable.Append("\n\t\t\t\t\t\t</table>");
				contentsTable.Append("\n\t\t\t\t\t</td>");
				contentsTable.Append("\n\t\t\t\t</tr>");
				contentsTable.Append("\n\t\t\t</table>");
			}
			#endregion
			/////////////////////
            contentsTable.Append("\n\t\t\t<table  border=\"0\"  width=\"100%\" cellpadding=\"0\" cellspacing=\"0\">");
			contentsTable.Append(controlsRows);
			contentsTable.Append("\n\t\t\t</table>");
			if (hasDetails)
			{
				contentsTable.Append("\n\t\t\t<span class=\"Details\" >");
				contentsTable.Append("\n\t\t\t\t<asp:Label ID=\"" + detailsLabel + "\" runat=\"server\"></asp:Label>");
				contentsTable.Append("\n\t\t\t</span>");
			}
			contentsTable.Append("\n\t\t</td>");
			contentsTable.Append("\n\t</tr>");
			contentsTable.Append("\n</table>");
			return contentsTable.ToString();
		}
        //----------------------------------------------
		public static void Create()
        {
			Details_InterfaceBuilder cr = new Details_InterfaceBuilder();
			cr.GenerateInterface();
        }
		//
		private void GenerateInterface()
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

    }
}
