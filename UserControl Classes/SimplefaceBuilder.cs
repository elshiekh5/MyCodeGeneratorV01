using System;
using System.Text ;
namespace SPGen
{
	/// <summary>
	/// Summary description for CodeBehindBuilder.
	/// </summary>
	public abstract class SimplefaceBuilder 
	{
        protected string ClassName = "";
        protected string _HeaderTitle = "";
        protected string HeaderTitle
        {
            get { return _HeaderTitle; }
            set { _HeaderTitle = value; }
        }
        //
        protected string GenerateControlDirective()
		{
             return"<%@ Control Language=\"c#\" AutoEventWireup=\"true\" CodeFile=\"" + ClassName + ".ascx.cs\" Inherits=\"" + ClassName + "\" %>";
		}
		//
        protected string GenerateControlRegisters()
		{
			return "\n <%@ Register TagPrefix=\"mp\" Namespace=\"MasterBoxes\" Assembly=\"MasterBoxes\" %>";
		}
        protected abstract string GenerateControls();
		//--------------------------------
        protected string GenerateUserControl()
        {
            //Table Header
            string tableHeader = "\n <mp:BoxContainer  Header=\"" + HeaderTitle + "\" runat=\"server\">";
            tableHeader+="\n\t<mp:Content PlaceHolderID=\"BoxContents\" runat=\"server\">";
            tableHeader+="\n\t\t\t<table class=\"MainTable\" cellspacing=\"0\" cellpadding=\"0\" width=\"100%\" border=\"0\" >";
            //TableFooter
            string tableFooter="\n\t\t\t</table>";
            tableFooter+="\n\t</mp:Content>";
            tableFooter += "\n </mp:BoxContainer>";

            StringBuilder userControl = new StringBuilder();
            userControl.Append(GenerateControlDirective());
            userControl.Append("\n" + GenerateControlRegisters());
            userControl.Append("\n" + tableHeader);
            userControl.Append("\n" + GenerateControls());
            userControl.Append("\n" + tableFooter);
            return userControl.ToString();
        }
        //--------------------------------
        public abstract void CreateControlFile();
        //


	}
}
