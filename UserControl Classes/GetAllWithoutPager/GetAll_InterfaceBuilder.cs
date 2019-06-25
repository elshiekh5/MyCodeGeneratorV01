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
	public class GetAll_InterfaceBuilder : InterfaceBuilder
    {
		Hashtable allParameters = null;
		//
		public GetAll_InterfaceBuilder(InterfaceType type)
		{
			ClassName = global.GetAllCodeBehindClass;
			HeaderTitle = global.HeaderTitle_GetAll;
			Type = type;
			
        }
		//----------------------------------
		private string GenerateDirective()
		{
			if (Type == InterfaceType.WEbUserControl)
			{
				return  "<%@ Control Language=\"c#\" AutoEventWireup=\"true\" CodeFile=\"" + ClassName + ".ascx.cs\" Inherits=\"" + ClassName + "\" %>";
			}
			else
			{
				if (IshasFreeTextBoxControl)
				{
					return "<%@ Page language=\"c#\" ValidateRequest=\"false\" MasterPageFile=\"~/App_Design/Ar/MasterPages/Admin.master\" CodeFile=\"Edit.aspx.cs\" Inherits=\"Admin" + ClassName + "\" Theme=\"AdminSite\" %>";

				}
				else
				{
					return "<%@ Page language=\"c#\" MasterPageFile=\"~/App_Design/Ar/MasterPages/Admin.master\" CodeFile=\"Edit.aspx.cs\" Inherits=\"Admin" + ClassName + "\" Theme=\"AdminSite\" %>";
				}
			}
		}
        //----------------------------------
        private string GenerateControls()
        {
			string dataGridID = "dg" + global.TableProgramatlyName;
            StringBuilder controls = new StringBuilder();

            controls.Append("\n\t\t\t\t<tr>");
			controls.Append("\n\t\t\t\t\t<td class=\"Result\" align=\"center\" >");
			controls.Append("\n\t\t\t\t\t\t<asp:Label ID=\"lblResult\" runat=\"server\" ForeColor=\"Red\" Text=\"\"></asp:Label>");
			controls.Append("\n\t\t\t\t\t</td>");
			controls.Append("\n\t\t\t\t</tr>");

            #region DataGrid Declaretion And Propreties
			controls.Append("\n\t\t\t\t\t<td class=\"Control\" align=\"center\" >");
			controls.Append("\n\t\t\t\t\t\t<asp:datagrid id=\"" + dataGridID + "\" runat=\"server\" SkinId=\"GridViewSkin\" ");
			controls.Append("\n\t\t\t\t\t\tOnDeleteCommand=\"" + dataGridID + "_DeleteCommand\" OnItemDataBound=\"" + dataGridID + "_ItemDataBound\" OnPageIndexChanged=\"" + dataGridID + "_PageIndexChanged\" >");
            #endregion
			//
			if (Fields.Count > 0)
			{
				controls.Append("\n\t\t\t\t\t\t<Columns>");
				int i = 0;
				foreach (SQLDMO.Column column in Fields)
				{
					if ((ID == null || column.Name != ID.Name) && i < 4)
					{
						if (allParameters != null && !allParameters.Contains(column.Name))
							continue;
						controls.Append("\n\t\t\t\t\t\t\t<asp:BoundColumn DataField=\"" + column.Name + "\" HeaderText=\"" + column.Name + "\"></asp:BoundColumn>");
					}
					++i;
				}
				if (ID != null)
				{
					controls.Append("\n\t\t\t\t\t\t\t<asp:TemplateColumn HeaderText=\"\">");
					controls.Append("\n\t\t\t\t\t\t\t<ItemTemplate>");
					controls.Append("\n\t\t\t\t\t\t\t\t<a href='<%# \"Edit.aspx?" + Globals.GetProgramatlyName(ID.Name) + "=\"+DataBinder.Eval(Container.DataItem, \"" + ID.Name + "\")+\"&iK=" + global.TableProgramatlyName + "\" %>' class='Link'>");
					controls.Append("\n\t\t\t\t\t\t\t\t\t<img src=\"/App_Design/Globals/Images/Admin/edit.gif\" border=\"0\" alt=\"<%#"+LanguageXmlBuilder.AddText("Update", TextType.Text)+"%>\" /></a>");
					controls.Append("\n\t\t\t\t\t\t\t</ItemTemplate>");
					controls.Append("\n\t\t\t\t\t\t\t</asp:TemplateColumn>");
					//Delete column.
					controls.Append("\n\t\t\t\t\t\t\t<asp:TemplateColumn HeaderText=\"\">");
					controls.Append("\n\t\t\t\t\t\t\t<ItemTemplate>");
					controls.Append("\n\t\t\t\t\t\t\t\t<asp:ImageButton ID=\"lbtnDelete\" AlternateText=\"\" ImageUrl=\"/App_Design/Globals/Images/Admin/delete.gif\" CommandName=\"Delete\" runat=\"server\"></asp:ImageButton>");
					controls.Append("\n\t\t\t\t\t\t\t</ItemTemplate>");
					controls.Append("\n\t\t\t\t\t\t\t</asp:TemplateColumn>");
				}
				controls.Append("\n\t\t\t\t\t\t</Columns>");
				controls.Append("\n\t\t\t\t\t\t</asp:datagrid>");
			}
            controls.Append("\n\t\t\t\t\t</td>");
            controls.Append("\n\t\t\t\t</tr>");
            //--------------------------------

            return controls.ToString();

        }
        //
		public static void Create(InterfaceType type)
        {
			GetAll_InterfaceBuilder cr = new GetAll_InterfaceBuilder(type);
			cr.GenerateInterface();
        }
		//
		public static void Create(InterfaceType type, Hashtable allParameters, string operation)
		{
			GetAll_InterfaceBuilder cr = new GetAll_InterfaceBuilder(type);
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
		//
		private void GenerateUserControlInterface()
		{
			//Begin create Control and check the free text box editor
			string controls = GenerateControls();
			StringBuilder userControl = new StringBuilder();
			userControl.Append(GenerateDirective());
			userControl.Append("\n" + ControlRegisters);
			userControl.Append("\n" + TableHeader);
			userControl.Append("\n" + GenerateControls());
			userControl.Append("\n" + TableFooter);

			//
			DirectoryInfo dInfo = Directory.CreateDirectory(Globals.UserControlsDirectory + global.TableProgramatlyName);
			string path = dInfo.FullName + "\\" + ClassName + ".ascx";
			//
			FileManager.CreateFile(path, userControl.ToString());

		}
		//
		private void GeneratePageInterface()
		{
			//Begin create Control and check the free text box editor
			string controls = GenerateControls();
			StringBuilder userControl = new StringBuilder();
			userControl.Append(GenerateDirective());
			userControl.Append("\n" + ControlRegisters);
			userControl.Append("\n <asp:Content id=\"Content1\" ContentPlaceHolderID=\"BasicContents\" runat=\"server\">");
			userControl.Append("\n" + TableHeader);
			userControl.Append("\n" + controls);
			userControl.Append("\n" + TableFooter);
			userControl.Append("\n</asp:Content>");
			//
			string directoryPath = Globals.AdminFolder + global.TableProgramatlyName + "\\";
			string path = directoryPath + "\\default.aspx";
			DirectoriesManager.ChechDirectory(directoryPath);
			FileManager.CreateFile(path, userControl.ToString());
		}
        //
    }
}
