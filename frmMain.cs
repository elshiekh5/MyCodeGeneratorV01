using System;
using System.Drawing;
using System.Collections;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using System.Data.SqlClient;
//

using System.Web.Configuration;
namespace SPGen
{
	/// <summary>
	/// Main UI for SPGen
	/// </summary>
	public class frmMain : System.Windows.Forms.Form
	{		
		

		private System.Windows.Forms.StatusBar statbarMain;
		private System.Windows.Forms.StatusBarPanel statbarpnlMain;
		private System.Windows.Forms.Button cmdConnect;
		private System.Windows.Forms.TextBox txtPassword;
		private System.Windows.Forms.TextBox txtUser;
		private System.Windows.Forms.Splitter spltrMain;
		private System.Windows.Forms.Panel pnlConnectTo;
		private System.Windows.Forms.ImageList imglstMain;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.TextBox txtphysicalPath;
		private System.Windows.Forms.TextBox txtServerName;
		private System.Windows.Forms.TextBox txtSiteName;
		private TextBox txtProjectPort;
		private Label label4;
        private CheckBox cbHasMultiLanguages;
		private ComboBox cbDatabases;
		private Button btnGO;
		private Label label5;
		private Label label6;
		private Label label7;
		private Label label8;
		private Label label9;
		private CheckBox cbHasConfiguration;
        private CheckBox cbAllowSorting;
        private Label label10;
        private CheckBox cbAllowXmlDocumentation;
        private Label label11;
        private Label label12;
        private CheckBox cbIsGlobalResource;
        private Label label13;
        private CheckBox cbHasProprety;
        private Label label14;
        private CheckBox cbISExcuteScaler;
        private Label label15;
        private CheckBox cbIsLabelText;
        private TextBox txtIdentityText;
        private CheckBox cbIsFreeTextBoxEditor;
        private Label label16;
		private System.ComponentModel.IContainer components;

		public frmMain()
		{			
			InitializeComponent();			

			// List Registered Servers
			object[] objServers = (object[])SqlProvider.obj.RegisteredServers;
			//selServers.Items.AddRange(objServers);

			// Default connection details, if provided

            NameValueCollection settingsAppSettings = (NameValueCollection)WebConfigurationManager.AppSettings;			

			if (settingsAppSettings["ServerName"] != null && settingsAppSettings["ServerName"] != "")
			{
				//selServers.Text = settingsAppSettings["ServerName"];
				SqlProvider.obj.ServerName = settingsAppSettings["ServerName"];
			}
			if (settingsAppSettings["UserName"] != null && settingsAppSettings["UserName"] != "")
			{
				txtUser.Text = settingsAppSettings["UserName"];
				SqlProvider.obj.UserName = settingsAppSettings["UserName"];
			}
			if (settingsAppSettings["Password"] != null && settingsAppSettings["Password"] != "")
			{
				char chPassword = '*';
				txtPassword.PasswordChar = chPassword;
				txtPassword.Text = settingsAppSettings["Password"];
				SqlProvider.obj.Password = settingsAppSettings["Password"];
			}
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if (components != null) 
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMain));
            this.statbarMain = new System.Windows.Forms.StatusBar();
            this.statbarpnlMain = new System.Windows.Forms.StatusBarPanel();
            this.pnlConnectTo = new System.Windows.Forms.Panel();
            this.cmdConnect = new System.Windows.Forms.Button();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.txtUser = new System.Windows.Forms.TextBox();
            this.imglstMain = new System.Windows.Forms.ImageList(this.components);
            this.spltrMain = new System.Windows.Forms.Splitter();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.txtphysicalPath = new System.Windows.Forms.TextBox();
            this.txtSiteName = new System.Windows.Forms.TextBox();
            this.txtServerName = new System.Windows.Forms.TextBox();
            this.txtProjectPort = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.cbHasMultiLanguages = new System.Windows.Forms.CheckBox();
            this.cbDatabases = new System.Windows.Forms.ComboBox();
            this.btnGO = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.cbHasConfiguration = new System.Windows.Forms.CheckBox();
            this.cbAllowSorting = new System.Windows.Forms.CheckBox();
            this.label10 = new System.Windows.Forms.Label();
            this.cbAllowXmlDocumentation = new System.Windows.Forms.CheckBox();
            this.label11 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.cbIsGlobalResource = new System.Windows.Forms.CheckBox();
            this.label13 = new System.Windows.Forms.Label();
            this.cbHasProprety = new System.Windows.Forms.CheckBox();
            this.label14 = new System.Windows.Forms.Label();
            this.cbISExcuteScaler = new System.Windows.Forms.CheckBox();
            this.label15 = new System.Windows.Forms.Label();
            this.cbIsLabelText = new System.Windows.Forms.CheckBox();
            this.txtIdentityText = new System.Windows.Forms.TextBox();
            this.cbIsFreeTextBoxEditor = new System.Windows.Forms.CheckBox();
            this.label16 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.statbarpnlMain)).BeginInit();
            this.SuspendLayout();
            // 
            // statbarMain
            // 
            this.statbarMain.Location = new System.Drawing.Point(0, 507);
            this.statbarMain.Name = "statbarMain";
            this.statbarMain.Panels.AddRange(new System.Windows.Forms.StatusBarPanel[] {
            this.statbarpnlMain});
            this.statbarMain.ShowPanels = true;
            this.statbarMain.Size = new System.Drawing.Size(368, 22);
            this.statbarMain.TabIndex = 5;
            // 
            // statbarpnlMain
            // 
            this.statbarpnlMain.AutoSize = System.Windows.Forms.StatusBarPanelAutoSize.Spring;
            this.statbarpnlMain.Name = "statbarpnlMain";
            this.statbarpnlMain.Text = "Awaiting your orders...";
            this.statbarpnlMain.Width = 352;
            // 
            // pnlConnectTo
            // 
            this.pnlConnectTo.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlConnectTo.Location = new System.Drawing.Point(0, 0);
            this.pnlConnectTo.Name = "pnlConnectTo";
            this.pnlConnectTo.Size = new System.Drawing.Size(368, 42);
            this.pnlConnectTo.TabIndex = 9;
            // 
            // cmdConnect
            // 
            this.cmdConnect.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.cmdConnect.Location = new System.Drawing.Point(129, 29);
            this.cmdConnect.Name = "cmdConnect";
            this.cmdConnect.Size = new System.Drawing.Size(64, 21);
            this.cmdConnect.TabIndex = 2;
            this.cmdConnect.Text = "Connect";
            this.cmdConnect.Click += new System.EventHandler(this.cmdConnect_Click);
            // 
            // txtPassword
            // 
            this.txtPassword.Location = new System.Drawing.Point(8, 29);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.Size = new System.Drawing.Size(103, 20);
            this.txtPassword.TabIndex = 1;
            this.txtPassword.Text = "Password";
            this.txtPassword.TextChanged += new System.EventHandler(this.txtPassword_TextChanged);
            this.txtPassword.Leave += new System.EventHandler(this.txtPassword_Leave);
            this.txtPassword.Enter += new System.EventHandler(this.txtPassword_Enter);
            // 
            // txtUser
            // 
            this.txtUser.Location = new System.Drawing.Point(8, 3);
            this.txtUser.Name = "txtUser";
            this.txtUser.Size = new System.Drawing.Size(103, 20);
            this.txtUser.TabIndex = 0;
            this.txtUser.Text = "User";
            this.txtUser.TextChanged += new System.EventHandler(this.txtUser_TextChanged);
            this.txtUser.Leave += new System.EventHandler(this.txtUser_Leave);
            this.txtUser.Enter += new System.EventHandler(this.txtUser_Enter);
            // 
            // imglstMain
            // 
            this.imglstMain.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imglstMain.ImageStream")));
            this.imglstMain.TransparentColor = System.Drawing.Color.Transparent;
            this.imglstMain.Images.SetKeyName(0, "");
            this.imglstMain.Images.SetKeyName(1, "");
            this.imglstMain.Images.SetKeyName(2, "");
            // 
            // spltrMain
            // 
            this.spltrMain.Location = new System.Drawing.Point(0, 42);
            this.spltrMain.Name = "spltrMain";
            this.spltrMain.Size = new System.Drawing.Size(3, 465);
            this.spltrMain.TabIndex = 11;
            this.spltrMain.TabStop = false;
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(5, 76);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(82, 23);
            this.label1.TabIndex = 12;
            this.label1.Text = "Physical Path";
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(5, 119);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(67, 23);
            this.label2.TabIndex = 13;
            this.label2.Text = "App Name";
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(5, 99);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(69, 23);
            this.label3.TabIndex = 14;
            this.label3.Text = "Server Name";
            // 
            // txtphysicalPath
            // 
            this.txtphysicalPath.Location = new System.Drawing.Point(173, 73);
            this.txtphysicalPath.Name = "txtphysicalPath";
            this.txtphysicalPath.Size = new System.Drawing.Size(100, 20);
            this.txtphysicalPath.TabIndex = 3;
            this.txtphysicalPath.Text = "C:\\";
            // 
            // txtSiteName
            // 
            this.txtSiteName.Location = new System.Drawing.Point(173, 116);
            this.txtSiteName.Name = "txtSiteName";
            this.txtSiteName.Size = new System.Drawing.Size(100, 20);
            this.txtSiteName.TabIndex = 5;
            this.txtSiteName.Text = "ProviderTest";
            // 
            // txtServerName
            // 
            this.txtServerName.Location = new System.Drawing.Point(173, 96);
            this.txtServerName.Name = "txtServerName";
            this.txtServerName.Size = new System.Drawing.Size(100, 20);
            this.txtServerName.TabIndex = 4;
            this.txtServerName.Text = "localhost";
            // 
            // txtProjectPort
            // 
            this.txtProjectPort.Location = new System.Drawing.Point(173, 139);
            this.txtProjectPort.Name = "txtProjectPort";
            this.txtProjectPort.Size = new System.Drawing.Size(100, 20);
            this.txtProjectPort.TabIndex = 6;
            this.txtProjectPort.Text = "4001";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(7, 167);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(84, 13);
            this.label4.TabIndex = 24;
            this.label4.Text = "Multi Languages";
            // 
            // cbHasMultiLanguages
            // 
            this.cbHasMultiLanguages.AutoSize = true;
            this.cbHasMultiLanguages.Location = new System.Drawing.Point(173, 165);
            this.cbHasMultiLanguages.Name = "cbHasMultiLanguages";
            this.cbHasMultiLanguages.Size = new System.Drawing.Size(103, 17);
            this.cbHasMultiLanguages.TabIndex = 7;
            this.cbHasMultiLanguages.Text = "Multi Languages";
            this.cbHasMultiLanguages.UseVisualStyleBackColor = true;
            // 
            // cbDatabases
            // 
            this.cbDatabases.Enabled = false;
            this.cbDatabases.FormattingEnabled = true;
            this.cbDatabases.Location = new System.Drawing.Point(173, 235);
            this.cbDatabases.Name = "cbDatabases";
            this.cbDatabases.Size = new System.Drawing.Size(101, 21);
            this.cbDatabases.TabIndex = 8;
            // 
            // btnGO
            // 
            this.btnGO.Location = new System.Drawing.Point(101, 417);
            this.btnGO.Name = "btnGO";
            this.btnGO.Size = new System.Drawing.Size(75, 23);
            this.btnGO.TabIndex = 10;
            this.btnGO.Text = "GO";
            this.btnGO.UseVisualStyleBackColor = true;
            this.btnGO.Click += new System.EventHandler(this.btnGO_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(3, 237);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(58, 13);
            this.label5.TabIndex = 24;
            this.label5.Text = "Databases";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(5, 146);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(64, 13);
            this.label6.TabIndex = 24;
            this.label6.Text = "Project Port";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(38, 52);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(199, 13);
            this.label7.TabIndex = 24;
            this.label7.Text = "________________________________";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(42, 427);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(199, 13);
            this.label8.TabIndex = 24;
            this.label8.Text = "________________________________";
            this.label8.Click += new System.EventHandler(this.label8_Click);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(3, 189);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(93, 13);
            this.label9.TabIndex = 28;
            this.label9.Text = "Has Configuration";
            // 
            // cbHasConfiguration
            // 
            this.cbHasConfiguration.AutoSize = true;
            this.cbHasConfiguration.Location = new System.Drawing.Point(173, 188);
            this.cbHasConfiguration.Name = "cbHasConfiguration";
            this.cbHasConfiguration.Size = new System.Drawing.Size(112, 17);
            this.cbHasConfiguration.TabIndex = 9;
            this.cbHasConfiguration.Text = "Has Configuration";
            this.cbHasConfiguration.UseVisualStyleBackColor = true;
            // 
            // cbAllowSorting
            // 
            this.cbAllowSorting.AutoSize = true;
            this.cbAllowSorting.Checked = true;
            this.cbAllowSorting.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbAllowSorting.Location = new System.Drawing.Point(173, 213);
            this.cbAllowSorting.Name = "cbAllowSorting";
            this.cbAllowSorting.Size = new System.Drawing.Size(120, 17);
            this.cbAllowSorting.TabIndex = 9;
            this.cbAllowSorting.Text = "Allow Admin Sorting";
            this.cbAllowSorting.UseVisualStyleBackColor = true;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(3, 214);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(101, 13);
            this.label10.TabIndex = 28;
            this.label10.Text = "Allow Admin Sorting";
            // 
            // cbAllowXmlDocumentation
            // 
            this.cbAllowXmlDocumentation.AutoSize = true;
            this.cbAllowXmlDocumentation.Location = new System.Drawing.Point(173, 262);
            this.cbAllowXmlDocumentation.Name = "cbAllowXmlDocumentation";
            this.cbAllowXmlDocumentation.Size = new System.Drawing.Size(139, 17);
            this.cbAllowXmlDocumentation.TabIndex = 9;
            this.cbAllowXmlDocumentation.Text = "AllowXmlDocumentation";
            this.cbAllowXmlDocumentation.UseVisualStyleBackColor = true;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(3, 263);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(120, 13);
            this.label11.TabIndex = 28;
            this.label11.Text = "AllowXmlDocumentation";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(3, 282);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(81, 13);
            this.label12.TabIndex = 30;
            this.label12.Text = "GlobalResource";
            // 
            // cbIsGlobalResource
            // 
            this.cbIsGlobalResource.AutoSize = true;
            this.cbIsGlobalResource.Checked = true;
            this.cbIsGlobalResource.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbIsGlobalResource.Location = new System.Drawing.Point(173, 281);
            this.cbIsGlobalResource.Name = "cbIsGlobalResource";
            this.cbIsGlobalResource.Size = new System.Drawing.Size(100, 17);
            this.cbIsGlobalResource.TabIndex = 29;
            this.cbIsGlobalResource.Text = "GlobalResource";
            this.cbIsGlobalResource.UseVisualStyleBackColor = true;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(2, 327);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(67, 13);
            this.label13.TabIndex = 32;
            this.label13.Text = "HasProprety";
            // 
            // cbHasProprety
            // 
            this.cbHasProprety.AutoSize = true;
            this.cbHasProprety.Checked = true;
            this.cbHasProprety.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbHasProprety.Location = new System.Drawing.Point(172, 326);
            this.cbHasProprety.Name = "cbHasProprety";
            this.cbHasProprety.Size = new System.Drawing.Size(86, 17);
            this.cbHasProprety.TabIndex = 31;
            this.cbHasProprety.Text = "HasProprety";
            this.cbHasProprety.UseVisualStyleBackColor = true;
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(3, 359);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(168, 13);
            this.label14.TabIndex = 34;
            this.label14.Text = "ExcuteScalerForCreateAndDelete";
            // 
            // cbISExcuteScaler
            // 
            this.cbISExcuteScaler.AutoSize = true;
            this.cbISExcuteScaler.Checked = true;
            this.cbISExcuteScaler.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbISExcuteScaler.Location = new System.Drawing.Point(173, 358);
            this.cbISExcuteScaler.Name = "cbISExcuteScaler";
            this.cbISExcuteScaler.Size = new System.Drawing.Size(88, 17);
            this.cbISExcuteScaler.TabIndex = 33;
            this.cbISExcuteScaler.Text = "ExcuteScaler";
            this.cbISExcuteScaler.UseVisualStyleBackColor = true;
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(3, 304);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(57, 13);
            this.label15.TabIndex = 36;
            this.label15.Text = "Label Text";
            // 
            // cbIsLabelText
            // 
            this.cbIsLabelText.AutoSize = true;
            this.cbIsLabelText.Checked = true;
            this.cbIsLabelText.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbIsLabelText.Location = new System.Drawing.Point(173, 303);
            this.cbIsLabelText.Name = "cbIsLabelText";
            this.cbIsLabelText.Size = new System.Drawing.Size(76, 17);
            this.cbIsLabelText.TabIndex = 35;
            this.cbIsLabelText.Text = "Label Text";
            this.cbIsLabelText.UseVisualStyleBackColor = true;
            // 
            // txtIdentityText
            // 
            this.txtIdentityText.Location = new System.Drawing.Point(256, 356);
            this.txtIdentityText.Name = "txtIdentityText";
            this.txtIdentityText.Size = new System.Drawing.Size(100, 20);
            this.txtIdentityText.TabIndex = 37;
            // 
            // cbIsFreeTextBoxEditor
            // 
            this.cbIsFreeTextBoxEditor.AutoSize = true;
            this.cbIsFreeTextBoxEditor.Checked = true;
            this.cbIsFreeTextBoxEditor.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbIsFreeTextBoxEditor.Location = new System.Drawing.Point(172, 382);
            this.cbIsFreeTextBoxEditor.Name = "cbIsFreeTextBoxEditor";
            this.cbIsFreeTextBoxEditor.Size = new System.Drawing.Size(138, 17);
            this.cbIsFreeTextBoxEditor.TabIndex = 38;
            this.cbIsFreeTextBoxEditor.Text = "Is FREETEXTBOX Editor";
            this.cbIsFreeTextBoxEditor.UseVisualStyleBackColor = true;
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(3, 382);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(119, 13);
            this.label16.TabIndex = 39;
            this.label16.Text = "Is FREETEXTBOX Editor";
            // 
            // frmMain
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(368, 529);
            this.Controls.Add(this.label16);
            this.Controls.Add(this.cbIsFreeTextBoxEditor);
            this.Controls.Add(this.txtIdentityText);
            this.Controls.Add(this.label15);
            this.Controls.Add(this.cbIsLabelText);
            this.Controls.Add(this.label14);
            this.Controls.Add(this.cbISExcuteScaler);
            this.Controls.Add(this.label13);
            this.Controls.Add(this.cbHasProprety);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.cbIsGlobalResource);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.cbAllowXmlDocumentation);
            this.Controls.Add(this.cbAllowSorting);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.cbHasConfiguration);
            this.Controls.Add(this.cmdConnect);
            this.Controls.Add(this.btnGO);
            this.Controls.Add(this.txtPassword);
            this.Controls.Add(this.cbDatabases);
            this.Controls.Add(this.txtUser);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.txtProjectPort);
            this.Controls.Add(this.cbHasMultiLanguages);
            this.Controls.Add(this.txtServerName);
            this.Controls.Add(this.txtSiteName);
            this.Controls.Add(this.txtphysicalPath);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.spltrMain);
            this.Controls.Add(this.pnlConnectTo);
            this.Controls.Add(this.statbarMain);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "frmMain";
            this.ShowIcon = false;
            this.Text = "Asp.Net Provider";
            this.Load += new System.EventHandler(this.frmMain_Load);
            ((System.ComponentModel.ISupportInitialize)(this.statbarpnlMain)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

		}
		#endregion

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main() 
		{
			Application.Run(new frmMain());
		}

		private void cmdConnect_Click(object sender, System.EventArgs e)
		{						
			// First ensure connection details are valid
			if (SqlProvider.obj.ServerName == "" || SqlProvider.obj.UserName == "")
			{   
				MessageBox.Show("Please enter in valid connection details.",this.Text);				
			}
			else
			{
				this.Cursor = Cursors.WaitCursor;
				statbarpnlMain.Text = "Connecting to SQL Server...";

				//Valid connection details				
				cbDatabases.Items.Clear();
				cbDatabases.Items.Add("Select");
				// List Databases
				try
				{				
					SqlProvider.obj.Connect();
					Array aDatabases = (Array)SqlProvider.obj.Databases;
					SqlProvider.obj.DisConnect();

					for (int i = 0; i < aDatabases.Length; i++)
					{
						cbDatabases.Items.Add(aDatabases.GetValue(i).ToString());
					}
					cbDatabases.SelectedIndex = 0;
					cbDatabases.Enabled = true;
					this.Cursor = Cursors.Default;
					statbarpnlMain.Text = "Connectiong successful, databases listed...";
				}
				catch
				{				
					this.Cursor = Cursors.Default;
					statbarpnlMain.Text = "Connectiong un-successful...";
					MessageBox.Show("Connection to database failed. Please check your Server Name, User and Password.", this.Text);
				}
			}
		}

		private void txtPassword_Enter(object sender, System.EventArgs e)
		{
			if (txtPassword.Text == "Password") 
			{
				txtPassword.Text = "";
				char chPassword = '*';
				txtPassword.PasswordChar = chPassword;
			}
		}

		private void txtUser_Enter(object sender, System.EventArgs e)
		{
			if (txtUser.Text == "User") txtUser.Text = "";
		}

	
		private void txtUser_Leave(object sender, System.EventArgs e)
		{
			if (txtUser.Text == "")
				txtUser.Text = "User";
			else
				SqlProvider.obj.UserName = txtUser.Text;
			
		}

		private void txtPassword_Leave(object sender, System.EventArgs e)
		{
				SqlProvider.obj.Password = txtPassword.Text;
		}
/*
		private void selServers_Leave(object sender, System.EventArgs e)
		{			
			if (selServers.Text == "")
				selServers.Text = "Select server";
			else
				SqlProvider.obj.ServerName = selServers.Text;
		}
		*/




		private void frmMain_Load(object sender, EventArgs e)
		{
			//selProjectType.SelectedIndex = 0;
		}

		private void txtPassword_TextChanged(object sender, EventArgs e)
		{

		}

		private void txtUser_TextChanged(object sender, EventArgs e)
		{

		}

		private void btnGO_Click(object sender, EventArgs e)
		{
			try
			{
				if (cbDatabases.SelectedIndex <= 0)
				{
					MessageBox.Show("Please Choose database");
					return;
				}
				this.Cursor = Cursors.WaitCursor;
				// Set database to get tables from	
				SqlProvider.obj.Database = cbDatabases.SelectedItem.ToString();
				ProjectBuilder.ProjectName = txtSiteName.Text;
				ProjectBuilder.ProjectPort = txtProjectPort.Text;
				ProjectBuilder.ServerName = txtServerName.Text;
				ProjectBuilder.PhysicalPath = txtphysicalPath.Text;
				ProjectBuilder.ProjectName = txtSiteName.Text;
				ProjectBuilder.HasMultiLanguages = cbHasMultiLanguages.Checked;
				ProjectBuilder.ProjectType = ProjectType.Simple; //(ProjectType)selProjectType.SelectedIndex;
                ProjectBuilder.HasConfiguration = cbHasConfiguration.Checked;
                ProjectBuilder.AllowAdminSorting = cbAllowSorting.Checked;
                ProjectBuilder.AllowXmlDocumentation = cbAllowXmlDocumentation.Checked;
                ProjectBuilder.IsLabelText = cbIsLabelText.Checked;
                ProjectBuilder.HasProprety = cbHasProprety.Checked;
                ProjectBuilder.ISExcuteScaler = cbISExcuteScaler.Checked;
                ProjectBuilder.IdentityText = txtIdentityText.Text;
                ProjectBuilder.IsFreeTextBoxEditor = cbIsFreeTextBoxEditor.Checked;
                ProjectBuilder.CreateProject();
				this.Cursor = Cursors.Default;
			}
			catch (Exception ex)
			{
				this.Cursor = Cursors.Default;
				statbarpnlMain.Text = "Problem listing Tables...";

				MessageBox.Show(ex.Message);
			}
		}

        private void label8_Click(object sender, EventArgs e)
        {

        }

		
		

		


		

		
		



	}
}
