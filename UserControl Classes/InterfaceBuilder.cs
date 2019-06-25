using System;
using System.Text ;
namespace SPGen
{
	public enum InterfaceType
	{
		WEbUserControl,
		WebForm
	}
	public class InterfaceBuilder : Generator 
	{
		private InterfaceType _Type;
		public InterfaceType Type
		{
			get
			{
				return _Type;
			}
			set
			{
				_Type = value;
			}
		}
		//--------------------------------
		private StringBuilder _ControlRegisters=null;
		public string ControlRegisters
		{
			get
			{
                if (_ControlRegisters == null)
                {
                    _ControlRegisters = new StringBuilder();
                    //if (ProjectBuilder.HasMasterBox)
                    //    _ControlRegisters.Append("\n <%@ Register TagPrefix=\"mp\" Namespace=\"MasterBoxes\" Assembly=\"MasterBoxes\" %>");
                    if (IshasFreeTextBoxControl)
                    {
                        if (ProjectBuilder.IsFreeTextBoxEditor)
                        {
                            _ControlRegisters.Append("\n <%@ Register Assembly=\"FreeTextBox\" Namespace=\"FreeTextBoxControls\" TagPrefix=\"FTB\" %>");
                        }
                        else
                        {

                            _ControlRegisters.Append("\n <%@ Register Assembly=\"FredCK.FCKeditorV2\" Namespace=\"FredCK.FCKeditorV2\" TagPrefix=\"FCKeditorV2\" %>");
                        }
                    }
                }
				return _ControlRegisters.ToString();
			}
		}
		//--------------------------------
		private bool ishasFreeTextBoxControl = false;
		public bool IshasFreeTextBoxControl
		{
			get { return ishasFreeTextBoxControl; }
			set { ishasFreeTextBoxControl = value; }
		}
		//--------------------------------
		private string _HeaderTitle="";
		public string HeaderTitle
		{
			get{return _HeaderTitle;}
			set{_HeaderTitle=value;}
		}
		//--------------------------------
		private StringBuilder _TableHeader=null;
		public StringBuilder TableHeader
		{
			get
			{
				if(_TableHeader==null)
				{
					_TableHeader=new StringBuilder();
                    //if (ProjectBuilder.HasMasterBox)
                    //{
                    //    _TableHeader.Append("\n <mp:BoxContainer  Header=\"" + HeaderTitle + "\" runat=\"server\">");
                    //    _TableHeader.Append("\n\t<mp:Content PlaceHolderID=\"BoxContents\" runat=\"server\">");
                    //    _TableHeader.Append("\n\t\t\t<table class=\"MainTable\" cellspacing=\"0\" cellpadding=\"0\" width=\"100%\" border=\"0\" >");
                    //}
                    //else
                    //{
						_TableHeader.Append("\n\t\t\t<table class=\"MainTable\" cellspacing=\"0\" cellpadding=\"0\" width=\"100%\" border=\"0\" >");
						 _TableHeader.Append("\n\t\t\t\t<tr>");
                         _TableHeader.Append("\n\t\t\t\t\t<td align=\"center\" class=\"Title\" colspan=\"2\">" + ResourcesTesxtsBuilder.AddUserText(HeaderTitle, TextType.HtmlClassic) + "</td>");
						_TableHeader.Append("\n\t\t\t\t</tr>");
						_TableHeader.Append("\n\t\t\t\t<tr>");
						_TableHeader.Append("\n\t\t\t\t<td>");
                        _TableHeader.Append("\n\t\t\t<table class=\"SubTable\" cellspacing=\"0\" cellpadding=\"0\" width=\"100%\" border=\"0\" >");
                        //_TableHeader.Append("\n\t\t\t\t<tr>");
        
                    //}
					
				}
				return _TableHeader;
			}
		}
		//--------------------------------
		private StringBuilder _TableFooter=null;
		public StringBuilder TableFooter
		{
			get
			{
				if(_TableFooter==null)
				{
					_TableFooter=new StringBuilder();
					_TableFooter.Append("\n\t\t\t</table>");
					_TableFooter.Append("\n\t\t\t</td>");
					_TableFooter.Append("\n\t\t\t</tr>");
					_TableFooter.Append("\n\t\t\t</table>");
                    //if (ProjectBuilder.HasMasterBox)
                    //{
                    //    _TableFooter.Append("\n\t</mp:Content>");
                    //    _TableFooter.Append("\n </mp:BoxContainer>");
                    //}
	

				}
				return _TableFooter;
			}
		}
		//--------------------------------
	}
}
