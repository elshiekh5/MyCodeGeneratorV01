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
	public class AppTemplateBuilder : InterfaceBuilder
    {
		//
		private bool hasPhoto = false;
		private bool hasLogo = false;
		private bool hasDetails = false;
		private bool hasDescription = false;
		private bool hasTitle = false;
		private string photoProprety = "";
		private string logoProprety = "";

		private string titleProprety = "";
		private string detailsProprety = "";
		private string descriptionProprety = "";
		//-------------------------------------
		static StringBuilder AppTemplates = new StringBuilder();
		//-------------------------------------
		public AppTemplateBuilder()
        {
        }
		//-------------------------------------
		//
		public static void AddTemplate()
		{
			AppTemplateBuilder template = new AppTemplateBuilder();
			template.CreateTemplate();
		}
        //
		public void  CreateTemplate()
		{

			StringBuilder template = new StringBuilder();
			StringBuilder propreties = new StringBuilder();
			string repeaterID = "dl" + global.TableProgramatlyName;
			//xmlTag

			//template.Append("\n\t\t\t");

			//--------------------------
			string datatype;
			string propertyName;
			string fullOptionalPropertyName;
			string idName = Globals.GetProgramatlyName(ID.Name); ;
			string detailsPageName = "DetailsPage";
			string detailsPagePath = "App_Forms/"+Table+"/details.aspx?" + Globals.GetProgramatlyName(ID.Name) + "={0}";
			SiteUrlsBuilder.AddUrl(detailsPageName, detailsPagePath);
			
			foreach (SQLDMO.Column column in Fields)
			{
				propertyName = Globals.GetProgramatlyName(column.Name);
				fullOptionalPropertyName = SiteOptionsBuilder.GetFullHasPropertyString(propertyName);
				if ((ID == null || column.Name != ID.Name) && (column.Name.IndexOf("_") < 0)
                     && column.Name.ToLower().IndexOf("password") < 0
                  && column.Name.ToLower().IndexOf("shortdescription") < 0
                  && column.Name.ToLower().IndexOf("priority") < 0
                  && column.Name.ToLower().IndexOf("isavailable ") < 0 
                    && column.Name.ToLower() != ProjectBuilder.LangID)
				{
					TableConstraint cnstr = SqlProvider.obj.GetParentColumn(column.Name);
					datatype = Globals.GetAliasDataType(column.Datatype);
					if (datatype == "bool")
					{
					}
					else if (Globals.GetSqlDataType(column.Datatype) == SqlDbType.NText)
					{
						if (column.Name.ToLower().IndexOf("details") > -1)
						{
							hasDetails = true;
							detailsProprety = Globals.GetProgramatlyName(column.Name);
						}
						else if (column.Name.ToLower().IndexOf("description") > -1)
						{
							hasDescription = true;
							descriptionProprety = Globals.GetProgramatlyName(column.Name);
						}
					}
					else if (datatype != "byte[]" && datatype != "Object" && datatype != "Guid" && column.Name.ToLower().IndexOf("password") < 0 && column.Name.ToLower() != ProjectBuilder.LangID)
					{

						if (column.Name.IndexOf(ProjectBuilder.ExtensionInColumnName) > -1)
						{
							if (column.Name == ProjectBuilder.PhotoExtensionColumnName)
							{
								hasPhoto = true;
								photoProprety = Globals.GetProgramatlyName(column.Name);

							}
							else if (column.Name == "LogoExtension")
							{
								hasLogo = true;
								logoProprety = Globals.GetProgramatlyName(column.Name);
							}
						}
						else if (column.Name.ToLower().IndexOf("description") > -1)
						{
							hasDescription = true;
							descriptionProprety = Globals.GetProgramatlyName(column.Name);
						}
						else if (column.Name.ToLower().IndexOf("title") > -1)
						{
							hasTitle = true;
							titleProprety = Globals.GetProgramatlyName(column.Name);
						}
						else
						{
							//----------------------------------------
							if (ProjectBuilder.HasConfiguration)
								propreties.Append("\n\t\t\t\t<tr runat=\"server\" visible=\"<%# SiteOptions.CheckOption(" + SiteOptionsBuilder.GetFullPropertyPath(fullOptionalPropertyName) + ") %>\">");
							else
								propreties.Append("\n\t\t\t\t<tr>");
							propreties.Append("\n\t\t\t\t\t<td class=\"GText\">");
							propreties.Append("\n\t\t\t\t\t\t<span class=\"GText\">" + ResourcesTesxtsBuilder.AddUserText(Globals.GetProgramatlyName(column.Name), TextType.HtmlClassic) + ": </span>");
							propreties.Append("\n\t\t\t\t\t\t<span class=\"GValue\"><%#Eval(\"" + propertyName + "\") %></span>");
							propreties.Append("\n\t\t\t\t\t</td>");
							propreties.Append("\n\t\t\t\t\t</tr>");
							//----------------------------------------
						}
					}
				}
			}
			template.Append("\n<!-- ------------------------ " + repeaterID + " ------------------------ -->");
			template.Append("\n<asp:DataList Width=\"100%\" ID=\"" + repeaterID + "\" RepeatColumns=\"1\" runat=\"server\">");
			template.Append("\n\t<ItemTemplate>");
			template.Append("\n\t\t<table cellspacing=\"0\" cellpadding=\"0\" class=\"GTable\" border=\"0\">");
			template.Append("\n\t\t\t<tr>");
			template.Append("\n\t\t\t\t<td style=\"width: 100%; vertical-align:top;\">");
			template.Append("\n\t\t\t\t\t<table class=\"GData\" cellpadding=\"0\" cellspacing=\"0\" width=\"100%\" border=\"0\">");
			if (hasTitle)
			{
				string titleFullOptionalPropertyName = SiteOptionsBuilder.GetFullHasPropertyString(titleProprety);
				if (ProjectBuilder.HasConfiguration)
					template.Append("\n\t\t\t\t\t<tr runat=\"server\" visible=\"<%# SiteOptions.CheckOption(" + SiteOptionsBuilder.GetFullPropertyPath(titleFullOptionalPropertyName) + ") %>\">");
				else
					template.Append("\n\t\t\t\t\t<tr>");
				template.Append("\n\t\t\t\t\t\t<td class=\"GTitle\">");
				template.Append("\n\t\t\t\t\t\t\t<a class=\"GTitle\" href='<%# "+SiteUrlsBuilder.GetTheGetUrlMethodIdentifire()+"(\"" + detailsPageName + "\",Eval(\"" + idName + "\")) %>'>");
				template.Append("\n\t\t\t\t\t\t\t\t<%# Globals.SubStringByWords(DataBinder.Eval(Container, \"DataItem." + titleProprety + "\"),10) %>");
				template.Append("\n\t\t\t\t\t\t\t</a>");
				template.Append("\n\t\t\t\t\t\t</td>");
				template.Append("\n\t\t\t\t\t</tr>");
			}
			if (hasDetails)
			{
				string detailsFullOptionalPropertyName = SiteOptionsBuilder.GetFullHasPropertyString(detailsProprety);
				if (ProjectBuilder.HasConfiguration)
					template.Append("\n\t\t\t\t\t<tr runat=\"server\" visible=\"<%# SiteOptions.CheckOption(" + SiteOptionsBuilder.GetFullPropertyPath(detailsFullOptionalPropertyName) + ") %>\">");
				else
					template.Append("\n\t\t\t\t\t<tr>");
				template.Append("\n\t\t\t\t\t\t<td class=\"GDetails\">");
				template.Append("\n\t\t\t\t\t\t\t\t<%# Globals.SubStringByWords(DataBinder.Eval(Container, \"DataItem." + detailsProprety + "\"), 30)%>");
				template.Append("\n\t\t\t\t\t\t\t</td>");
				template.Append("\n\t\t\t\t\t\t</tr>");
			}
			if (hasDescription)
			{
				string descriptionFullOptionalPropertyName = SiteOptionsBuilder.GetFullHasPropertyString(descriptionProprety);
				if (ProjectBuilder.HasConfiguration)
					template.Append("\n\t\t\t\t\t<tr runat=\"server\" visible=\"<%# SiteOptions.CheckOption(" + SiteOptionsBuilder.GetFullPropertyPath(descriptionFullOptionalPropertyName) + ") %>\">");
				else
					template.Append("\n\t\t\t\t\t<tr>");
				template.Append("\n\t\t\t\t\t\t<td class=\"GDetails\">");
				template.Append("\n\t\t\t\t\t\t\t\t<%# Globals.SubStringByWords(DataBinder.Eval(Container, \"DataItem." + descriptionProprety + "\"), 30)%>");
				template.Append("\n\t\t\t\t\t\t\t</td>");
				template.Append("\n\t\t\t\t\t\t</tr>");
			}
			template.Append(propreties.ToString());
			template.Append("\n\t\t\t\t\t</table>");
			template.Append("\n\t\t\t\t</td>");
			if (hasPhoto || hasLogo)
			{

				template.Append("\n\t\t\t\t<td style=\"text-align:center; vertical-align:top;\">");
				if (hasPhoto)
				{
					string photoFullOptionalPropertyName = SiteOptionsBuilder.GetFullHasPropertyString(photoProprety);
					if (ProjectBuilder.HasConfiguration)
						template.Append("\n\t\t\t\t\t<table runat=\"server\" visible=\"<%# SiteOptions.CheckOption(" + SiteOptionsBuilder.GetFullPropertyPath(photoFullOptionalPropertyName) + ") %>\" cellpadding=\"0\" cellspacing=\"0\" class=\"GImageContainer\">");
					else
						template.Append("\n\t\t\t\t\t<table  cellpadding=\"0\" cellspacing=\"0\" class=\"GImageContainer\">");

					template.Append("\n\t\t\t\t\t\t<tr>");
					template.Append("\n\t\t\t\t\t\t\t<td>");
					template.Append("\n\t\t\t\t\t\t\t\t<a href='<%# "+SiteUrlsBuilder.GetTheGetUrlMethodIdentifire()+"(\"" + detailsPageName + "\",Eval(\"" + idName + "\")) %>'>");
					template.Append("\n\t\t\t\t\t\t\t\t\t<img alt=\"<%# DataBinder.Eval(Container, \"DataItem." + titleProprety + "\") %>\"");
					template.Append("\n\t\t\t\t\t\t\t\t\t\tclass=\"GImage\" src='<%# " + global.TableFactoryClass + ".Get" + global.TableProgramatlyName + "PhotoThumbnail(Eval(\"" + idName + "\"),Eval(\"" + photoProprety + "\")) %>' /></a>");
					template.Append("\n\t\t\t\t\t\t\t</td>");
					template.Append("\n\t\t\t\t\t\t</tr>");
					template.Append("\n\t\t\t\t\t</table>");
				}
				if (hasLogo)
				{
					string logoFullOptionalPropertyName = SiteOptionsBuilder.GetFullHasPropertyString(logoProprety);
					if (ProjectBuilder.HasConfiguration)
						template.Append("\n\t\t\t\t\t<table runat=\"server\" visible=\"<%# SiteOptions.CheckOption(" + SiteOptionsBuilder.GetFullPropertyPath(logoFullOptionalPropertyName) + ") %>\" cellpadding=\"0\" cellspacing=\"0\" class=\"GImageContainer\">");
					else
						template.Append("\n\t\t\t\t\t<table  cellpadding=\"0\" cellspacing=\"0\" class=\"GImageContainer\">");

					template.Append("\n\t\t\t\t\t\t<tr>");
					template.Append("\n\t\t\t\t\t\t\t<td>");

					template.Append("\n\t\t\t\t\t\t\t\t<a href='<%# "+SiteUrlsBuilder.GetTheGetUrlMethodIdentifire()+"(\"" + detailsPageName + "\",Eval(\"" + idName + "\")) %>'>");
					template.Append("\n\t\t\t\t\t\t\t\t\t<img alt=\"<%# DataBinder.Eval(Container, \"DataItem." + titleProprety + "\") %>\"");
					template.Append("\n\t\t\t\t\t\t\t\t\t\tclass=\"GImage\" src='<%# " + global.TableFactoryClass + ".Get" + global.TableProgramatlyName + "LogoThumbnail(Eval(\"" + idName + "\"),Eval(\"" + logoProprety + "\")) %>' /></a>");
					template.Append("\n\t\t\t\t\t\t\t</td>");
					template.Append("\n\t\t\t\t\t\t</tr>");
					template.Append("\n\t\t\t\t\t</table>");
				}
				template.Append("\n\t\t\t\t</td>");
			}
			template.Append("\n\t\t\t</tr>");
			template.Append("\n\t\t</table>");
			template.Append("\n\t</ItemTemplate>");
			template.Append("\n</asp:DataList>");
			template.Append("\n<!-- ------------------------ --------------- ------------------------ -->");
			//
			AppTemplates.Append(template.ToString());

		}

        //----------------------------------------------
		//
		public static void CreateAppTemlatesFile()
		{
			//Begin create Control and check the free text box editor
			StringBuilder tmplates = new StringBuilder();
			tmplates.Append("<%@ Control Language=\"C#\" AutoEventWireup=\"true\" CodeFile=\"SiteTemplates.ascx.cs\" Inherits=\"SiteTemplates\" %>");
			tmplates.Append("\n" + AppTemplates.ToString());
			//
			string path = Globals.SiteTemplates + "SiteTemplates.ascx";
			// Create a file to write to.
			using (StreamWriter sw = File.CreateText(path))
			{
				sw.WriteLine(tmplates);
			}
			return;
			//
			//FileManager.CreateFile(path, userControl.ToString());
		}
		

		//

    }
}
