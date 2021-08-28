using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace Universal_Fixer
{
	public class MainForm : Form
	{
		public string DirectoryName = "";

		private IContainer components = null;

		private CheckBox checkBox14;

		private CheckBox checkBox13;

		private CheckBox checkBox12;

		private CheckBox fixnetspider;

		private CheckBox checkBox11;

		private TransparentPanel net_down;

		private TransparentPanel pe_up;

		private TransparentPanel net_up;

		private TransparentPanel netpe_up;

		private TransparentPanel netpe_down;

		private TransparentPanel pe_down;

		private TextBox textBox2;

		private CheckBox fiximporttable;

		private CheckBox checkBox6;

		private CheckBox fixnetdirectory;

		private CheckBox fixBSJB;

		private CheckBox checkBox1;

		private GroupBox groupBox3;

		private CheckBox fixnetmetadata;

		private CheckBox fixrelocations;

		private CheckBox fixnumberofrvaandsizes;

		private GroupBox groupBox2;

		private CheckBox fixdatadirectories;

		private CheckBox fixsizeofstackheap;

		private CheckBox fixversioninfo;

		private CheckBox fixsizeofinitdata;

		private CheckBox fixoptionaheader1;

		private CheckBox fixsizeofimage;

		private CheckBox fixrawallignment;

		private CheckBox characteristics;

		private GroupBox groupBox1;

		private CheckBox checkBox19;

		private CheckBox checkBox10;

		private CheckBox checkBox9;

		private CheckBox checkBox8;

		private CheckBox checkBox7;

		private CheckBox checkBox5;

		private CheckBox checkBox4;

		private CheckBox checkBox3;

		private CheckBox checkBox2;

		private Button button1;

		private TextBox textBox1;

		private Label label1;

		private Button button2;

		private Button button3;

		public MainForm()
		{
			InitializeComponent();
		}

		private void Button1Click(object sender, EventArgs e)
		{
			OpenFileDialog openFileDialog = new OpenFileDialog();
			openFileDialog.Title = "Browse for target assembly";
			openFileDialog.InitialDirectory = "c:\\";
			if (DirectoryName != "")
			{
				openFileDialog.InitialDirectory = DirectoryName;
			}
			openFileDialog.Filter = "All files (*.exe,*.dll)|*.exe;*.dll";
			openFileDialog.FilterIndex = 2;
			openFileDialog.RestoreDirectory = true;
			if (openFileDialog.ShowDialog() == DialogResult.OK)
			{
				string fileName = openFileDialog.FileName;
				textBox1.Text = fileName;
				int num = fileName.LastIndexOf("\\");
				if (num != -1)
				{
					DirectoryName = fileName.Remove(num, fileName.Length - num);
				}
				if (DirectoryName.Length == 2)
				{
					DirectoryName += "\\";
				}
			}
		}

		private void Button3Click(object sender, EventArgs e)
		{
			Application.Exit();
		}

		private void SetPEChecked(bool state)
		{
			characteristics.Checked = state;
			fixrawallignment.Checked = state;
			fixsizeofimage.Checked = state;
			fixoptionaheader1.Checked = state;
			fixsizeofinitdata.Checked = state;
			fixversioninfo.Checked = state;
			fixsizeofstackheap.Checked = state;
			fixdatadirectories.Checked = state;
			fixnetspider.Checked = state;
		}

		private void SetNETPEChecked(bool state)
		{
			fixnumberofrvaandsizes.Checked = state;
			fixrelocations.Checked = state;
			fiximporttable.Checked = state;
			fixnetmetadata.Checked = state;
			fixnetdirectory.Checked = state;
			fixBSJB.Checked = state;
		}

		private void SetNETChecked(bool state)
		{
			checkBox2.Checked = state;
			checkBox3.Checked = state;
			checkBox4.Checked = state;
			checkBox9.Checked = state;
			checkBox8.Checked = state;
			checkBox1.Checked = state;
			checkBox7.Checked = state;
			checkBox19.Checked = state;
			checkBox6.Checked = state;
			checkBox5.Checked = state;
			checkBox10.Checked = state;
			checkBox11.Checked = state;
			checkBox12.Checked = state;
			checkBox13.Checked = state;
			checkBox14.Checked = state;
		}

		private void Button2Click(object sender, EventArgs e)
		{
			string text = textBox1.Text;
			byte[] array = new byte[0];
			MetadataReader metadataReader = new MetadataReader();
			BinaryReader binaryReader = null;
			MemoryStream memoryStream = null;
			if (!(text != ""))
			{
				return;
			}
			string text2 = Path.Combine(Path.GetDirectoryName(text), Path.GetFileNameWithoutExtension(text) + "_fix" + Path.GetExtension(text));
			textBox2.Text = "";
			array = File.ReadAllBytes(text);
			if (array.Length < 512)
			{
				textBox2.Text = textBox2.Text + "Invalid file: " + text2 + "\r\n";
				textBox2.Text += "File size should be at least 512 bytes!\r\n";
				return;
			}
			memoryStream = new MemoryStream(array);
			binaryReader = new BinaryReader(memoryStream);
			metadataReader = new MetadataReader();
			if (metadataReader.FixPE(text, binaryReader, array, textBox2, characteristics.Checked, fixrawallignment.Checked, fixsizeofimage.Checked, fixoptionaheader1.Checked, fixsizeofinitdata.Checked, fixversioninfo.Checked, fixsizeofstackheap.Checked, fixdatadirectories.Checked, fixnetspider.Checked))
			{
				File.WriteAllBytes(text2, array);
				textBox2.Text = textBox2.Text + "Stage 1 completed & file saved on: " + text2 + "\r\n";
				if (metadataReader.FixNetPE(text, binaryReader, array, textBox2, fixnumberofrvaandsizes.Checked, fixrelocations.Checked, fixnetmetadata.Checked, fixBSJB.Checked, fixnetdirectory.Checked, fiximporttable.Checked))
				{
					File.WriteAllBytes(text2, array);
					textBox2.Text = textBox2.Text + "Stage 2 completed & file saved on: " + text2 + "\r\n";
					if (checkBox2.Checked && metadataReader.RemoveInvalids(text, binaryReader, array, textBox2, checkBox2.Checked))
					{
						File.WriteAllBytes(text2, array);
						textBox2.Text = textBox2.Text + "Stage 3 completed & file saved on: " + text2 + "\r\n";
					}
					if (checkBox3.Checked && metadataReader.RemoveMultiple(text, binaryReader, array, textBox2, checkBox3.Checked))
					{
						File.WriteAllBytes(text2, array);
						textBox2.Text = textBox2.Text + "Stage 4 completed & file saved on: " + text2 + "\r\n";
					}
					if (metadataReader.FixAssembly(text, binaryReader, array, textBox2, checkBox4.Checked, checkBox5.Checked, checkBox7.Checked, checkBox8.Checked, checkBox9.Checked, checkBox10.Checked, checkBox19.Checked, checkBox1.Checked, checkBox6.Checked, checkBox11.Checked, checkBox12.Checked, checkBox13.Checked, checkBox14.Checked))
					{
						File.WriteAllBytes(text2, array);
						textBox2.Text = textBox2.Text + "Stage 5 completed & file saved on: " + text2 + "\r\n";
					}
				}
			}
			else
			{
				textBox2.Text = textBox2.Text + "Invalid PE file: " + text2 + "\r\n";
				textBox2.Text += "Fixing aborted!\r\n";
			}
			binaryReader.Close();
			memoryStream.Close();
		}

		private void TextBox1DragEnter(object sender, DragEventArgs e)
		{
			if (e.Data.GetDataPresent(DataFormats.FileDrop))
			{
				e.Effect = DragDropEffects.Copy;
			}
			else
			{
				e.Effect = DragDropEffects.None;
			}
		}

		private void TextBox1DragDrop(object sender, DragEventArgs e)
		{
			try
			{
				Array array = (Array)e.Data.GetData(DataFormats.FileDrop);
				if (array == null)
				{
					return;
				}
				string text = array.GetValue(0).ToString();
				int num = text.LastIndexOf(".");
				if (num == -1)
				{
					return;
				}
				string text2 = text.Substring(num);
				text2 = text2.ToLower();
				if (text2 == ".exe" || text2 == ".dll")
				{
					Activate();
					textBox1.Text = text;
					int num2 = text.LastIndexOf("\\");
					if (num2 != -1)
					{
						DirectoryName = text.Remove(num2, text.Length - num2);
					}
					if (DirectoryName.Length == 2)
					{
						DirectoryName += "\\";
					}
				}
			}
			catch
			{
			}
		}

		private void Pe_upClick(object sender, EventArgs e)
		{
			SetPEChecked(state: true);
		}

		private void Pe_downClick(object sender, EventArgs e)
		{
			SetPEChecked(state: false);
		}

		private void Netpe_upClick(object sender, EventArgs e)
		{
			SetNETPEChecked(state: true);
		}

		private void Netpe_downClick(object sender, EventArgs e)
		{
			SetNETPEChecked(state: false);
		}

		private void Net_upClick(object sender, EventArgs e)
		{
			SetNETChecked(state: true);
		}

		private void Net_downClick(object sender, EventArgs e)
		{
			SetNETChecked(state: false);
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing && components != null)
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		private void InitializeComponent()
		{
			button3 = new System.Windows.Forms.Button();
			button2 = new System.Windows.Forms.Button();
			label1 = new System.Windows.Forms.Label();
			textBox1 = new System.Windows.Forms.TextBox();
			button1 = new System.Windows.Forms.Button();
			checkBox2 = new System.Windows.Forms.CheckBox();
			checkBox3 = new System.Windows.Forms.CheckBox();
			checkBox4 = new System.Windows.Forms.CheckBox();
			fixnetspider = new System.Windows.Forms.CheckBox();
			checkBox5 = new System.Windows.Forms.CheckBox();
			checkBox7 = new System.Windows.Forms.CheckBox();
			checkBox8 = new System.Windows.Forms.CheckBox();
			checkBox9 = new System.Windows.Forms.CheckBox();
			checkBox10 = new System.Windows.Forms.CheckBox();
			fixnetmetadata = new System.Windows.Forms.CheckBox();
			checkBox19 = new System.Windows.Forms.CheckBox();
			fixversioninfo = new System.Windows.Forms.CheckBox();
			fixsizeofinitdata = new System.Windows.Forms.CheckBox();
			fixsizeofstackheap = new System.Windows.Forms.CheckBox();
			fixoptionaheader1 = new System.Windows.Forms.CheckBox();
			fixsizeofimage = new System.Windows.Forms.CheckBox();
			characteristics = new System.Windows.Forms.CheckBox();
			fixrawallignment = new System.Windows.Forms.CheckBox();
			groupBox1 = new System.Windows.Forms.GroupBox();
			pe_down = new Universal_Fixer.TransparentPanel();
			pe_up = new Universal_Fixer.TransparentPanel();
			fixdatadirectories = new System.Windows.Forms.CheckBox();
			groupBox2 = new System.Windows.Forms.GroupBox();
			netpe_down = new Universal_Fixer.TransparentPanel();
			netpe_up = new Universal_Fixer.TransparentPanel();
			fiximporttable = new System.Windows.Forms.CheckBox();
			fixBSJB = new System.Windows.Forms.CheckBox();
			fixrelocations = new System.Windows.Forms.CheckBox();
			fixnetdirectory = new System.Windows.Forms.CheckBox();
			fixnumberofrvaandsizes = new System.Windows.Forms.CheckBox();
			groupBox3 = new System.Windows.Forms.GroupBox();
			checkBox14 = new System.Windows.Forms.CheckBox();
			checkBox13 = new System.Windows.Forms.CheckBox();
			checkBox12 = new System.Windows.Forms.CheckBox();
			checkBox11 = new System.Windows.Forms.CheckBox();
			net_down = new Universal_Fixer.TransparentPanel();
			net_up = new Universal_Fixer.TransparentPanel();
			checkBox6 = new System.Windows.Forms.CheckBox();
			checkBox1 = new System.Windows.Forms.CheckBox();
			textBox2 = new System.Windows.Forms.TextBox();
			groupBox1.SuspendLayout();
			groupBox2.SuspendLayout();
			groupBox3.SuspendLayout();
			SuspendLayout();
			button3.Location = new System.Drawing.Point(329, 52);
			button3.Name = "button3";
			button3.Size = new System.Drawing.Size(72, 26);
			button3.TabIndex = 13;
			button3.Text = "Exit";
			button3.UseVisualStyleBackColor = true;
			button3.Click += new System.EventHandler(Button3Click);
			button2.Location = new System.Drawing.Point(180, 52);
			button2.Name = "button2";
			button2.Size = new System.Drawing.Size(102, 26);
			button2.TabIndex = 12;
			button2.Text = "Fix assembly";
			button2.UseVisualStyleBackColor = true;
			button2.Click += new System.EventHandler(Button2Click);
			label1.BackColor = System.Drawing.Color.Transparent;
			label1.ForeColor = System.Drawing.Color.Black;
			label1.Location = new System.Drawing.Point(12, 9);
			label1.Name = "label1";
			label1.Size = new System.Drawing.Size(100, 14);
			label1.TabIndex = 15;
			label1.Text = "Name of assembly:";
			textBox1.AllowDrop = true;
			textBox1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
			textBox1.Location = new System.Drawing.Point(12, 26);
			textBox1.Name = "textBox1";
			textBox1.Size = new System.Drawing.Size(515, 20);
			textBox1.TabIndex = 14;
			textBox1.DragDrop += new System.Windows.Forms.DragEventHandler(TextBox1DragDrop);
			textBox1.DragEnter += new System.Windows.Forms.DragEventHandler(TextBox1DragEnter);
			button1.Location = new System.Drawing.Point(12, 52);
			button1.Name = "button1";
			button1.Size = new System.Drawing.Size(123, 26);
			button1.TabIndex = 11;
			button1.Text = "Browse for assembly";
			button1.UseVisualStyleBackColor = true;
			button1.Click += new System.EventHandler(Button1Click);
			checkBox2.Checked = true;
			checkBox2.CheckState = System.Windows.Forms.CheckState.Checked;
			checkBox2.Location = new System.Drawing.Point(6, 19);
			checkBox2.Name = "checkBox2";
			checkBox2.Size = new System.Drawing.Size(146, 18);
			checkBox2.TabIndex = 18;
			checkBox2.Text = "Remove invalid streams";
			checkBox2.UseVisualStyleBackColor = true;
			checkBox3.Checked = true;
			checkBox3.CheckState = System.Windows.Forms.CheckState.Checked;
			checkBox3.Location = new System.Drawing.Point(160, 19);
			checkBox3.Name = "checkBox3";
			checkBox3.Size = new System.Drawing.Size(192, 18);
			checkBox3.TabIndex = 19;
			checkBox3.Text = "Remove multiple Module/Assembly";
			checkBox3.UseVisualStyleBackColor = true;
			checkBox4.Checked = true;
			checkBox4.CheckState = System.Windows.Forms.CheckState.Checked;
			checkBox4.Location = new System.Drawing.Point(6, 43);
			checkBox4.Name = "checkBox4";
			checkBox4.Size = new System.Drawing.Size(77, 24);
			checkBox4.TabIndex = 20;
			checkBox4.Text = "Fix names";
			checkBox4.UseVisualStyleBackColor = true;
			fixnetspider.Checked = true;
			fixnetspider.CheckState = System.Windows.Forms.CheckState.Checked;
			fixnetspider.Location = new System.Drawing.Point(317, 79);
			fixnetspider.Name = "fixnetspider";
			fixnetspider.Size = new System.Drawing.Size(152, 24);
			fixnetspider.TabIndex = 21;
			fixnetspider.Text = "Fix .Net Spider Native Res";
			fixnetspider.UseVisualStyleBackColor = true;
			checkBox5.Checked = true;
			checkBox5.CheckState = System.Windows.Forms.CheckState.Checked;
			checkBox5.Location = new System.Drawing.Point(6, 103);
			checkBox5.Name = "checkBox5";
			checkBox5.Size = new System.Drawing.Size(90, 20);
			checkBox5.TabIndex = 22;
			checkBox5.Text = "Fix extends";
			checkBox5.UseVisualStyleBackColor = true;
			checkBox7.Checked = true;
			checkBox7.CheckState = System.Windows.Forms.CheckState.Checked;
			checkBox7.Location = new System.Drawing.Point(6, 73);
			checkBox7.Name = "checkBox7";
			checkBox7.Size = new System.Drawing.Size(90, 24);
			checkBox7.TabIndex = 23;
			checkBox7.Text = "Fix methods";
			checkBox7.UseVisualStyleBackColor = true;
			checkBox8.Checked = true;
			checkBox8.CheckState = System.Windows.Forms.CheckState.Checked;
			checkBox8.Location = new System.Drawing.Point(235, 43);
			checkBox8.Name = "checkBox8";
			checkBox8.Size = new System.Drawing.Size(90, 24);
			checkBox8.TabIndex = 24;
			checkBox8.Text = "Fix resources";
			checkBox8.UseVisualStyleBackColor = true;
			checkBox9.Checked = true;
			checkBox9.CheckState = System.Windows.Forms.CheckState.Checked;
			checkBox9.Location = new System.Drawing.Point(102, 43);
			checkBox9.Name = "checkBox9";
			checkBox9.Size = new System.Drawing.Size(114, 24);
			checkBox9.TabIndex = 25;
			checkBox9.Text = "Fix NestedClasses";
			checkBox9.UseVisualStyleBackColor = true;
			checkBox10.Location = new System.Drawing.Point(235, 73);
			checkBox10.Name = "checkBox10";
			checkBox10.Size = new System.Drawing.Size(178, 24);
			checkBox10.TabIndex = 26;
			checkBox10.Text = "Set invalid Method body to ret";
			checkBox10.UseVisualStyleBackColor = true;
			fixnetmetadata.Checked = true;
			fixnetmetadata.CheckState = System.Windows.Forms.CheckState.Checked;
			fixnetmetadata.Location = new System.Drawing.Point(6, 49);
			fixnetmetadata.Name = "fixnetmetadata";
			fixnetmetadata.Size = new System.Drawing.Size(162, 24);
			fixnetmetadata.TabIndex = 35;
			fixnetmetadata.Text = "Fix .NET Metadata (DataDir)";
			fixnetmetadata.UseVisualStyleBackColor = true;
			checkBox19.Checked = true;
			checkBox19.CheckState = System.Windows.Forms.CheckState.Checked;
			checkBox19.Location = new System.Drawing.Point(102, 99);
			checkBox19.Name = "checkBox19";
			checkBox19.Size = new System.Drawing.Size(104, 24);
			checkBox19.TabIndex = 36;
			checkBox19.Text = "Fix PropertyMap";
			checkBox19.UseVisualStyleBackColor = true;
			fixversioninfo.Checked = true;
			fixversioninfo.CheckState = System.Windows.Forms.CheckState.Checked;
			fixversioninfo.Location = new System.Drawing.Point(344, 49);
			fixversioninfo.Name = "fixversioninfo";
			fixversioninfo.Size = new System.Drawing.Size(132, 24);
			fixversioninfo.TabIndex = 32;
			fixversioninfo.Text = "Fix Version info";
			fixversioninfo.UseVisualStyleBackColor = true;
			fixsizeofinitdata.Location = new System.Drawing.Point(148, 49);
			fixsizeofinitdata.Name = "fixsizeofinitdata";
			fixsizeofinitdata.Size = new System.Drawing.Size(177, 24);
			fixsizeofinitdata.TabIndex = 31;
			fixsizeofinitdata.Text = "Fix SizeOfInitData (inconsistent)";
			fixsizeofinitdata.UseVisualStyleBackColor = true;
			fixsizeofstackheap.Checked = true;
			fixsizeofstackheap.CheckState = System.Windows.Forms.CheckState.Checked;
			fixsizeofstackheap.Location = new System.Drawing.Point(6, 79);
			fixsizeofstackheap.Name = "fixsizeofstackheap";
			fixsizeofstackheap.Size = new System.Drawing.Size(136, 24);
			fixsizeofstackheap.TabIndex = 33;
			fixsizeofstackheap.Text = "Fix SizeOfStack/Heap";
			fixsizeofstackheap.UseVisualStyleBackColor = true;
			fixoptionaheader1.Checked = true;
			fixoptionaheader1.CheckState = System.Windows.Forms.CheckState.Checked;
			fixoptionaheader1.Location = new System.Drawing.Point(6, 49);
			fixoptionaheader1.Name = "fixoptionaheader1";
			fixoptionaheader1.Size = new System.Drawing.Size(136, 24);
			fixoptionaheader1.TabIndex = 30;
			fixoptionaheader1.Text = "Fix OptionaHeader (1)";
			fixoptionaheader1.UseVisualStyleBackColor = true;
			fixsizeofimage.Checked = true;
			fixsizeofimage.CheckState = System.Windows.Forms.CheckState.Checked;
			fixsizeofimage.Location = new System.Drawing.Point(344, 19);
			fixsizeofimage.Name = "fixsizeofimage";
			fixsizeofimage.Size = new System.Drawing.Size(104, 24);
			fixsizeofimage.TabIndex = 29;
			fixsizeofimage.Text = "Fix SizeOfImage";
			fixsizeofimage.UseVisualStyleBackColor = true;
			characteristics.Checked = true;
			characteristics.CheckState = System.Windows.Forms.CheckState.Checked;
			characteristics.Location = new System.Drawing.Point(6, 19);
			characteristics.Name = "characteristics";
			characteristics.Size = new System.Drawing.Size(173, 24);
			characteristics.TabIndex = 28;
			characteristics.Text = "Fix Fileheader.characteristics";
			characteristics.UseVisualStyleBackColor = true;
			fixrawallignment.Checked = true;
			fixrawallignment.CheckState = System.Windows.Forms.CheckState.Checked;
			fixrawallignment.Location = new System.Drawing.Point(168, 19);
			fixrawallignment.Name = "fixrawallignment";
			fixrawallignment.Size = new System.Drawing.Size(162, 24);
			fixrawallignment.TabIndex = 27;
			fixrawallignment.Text = "Fix Sections Raw Allignment";
			fixrawallignment.UseVisualStyleBackColor = true;
			groupBox1.Controls.Add(pe_down);
			groupBox1.Controls.Add(pe_up);
			groupBox1.Controls.Add(fixdatadirectories);
			groupBox1.Controls.Add(fixrawallignment);
			groupBox1.Controls.Add(characteristics);
			groupBox1.Controls.Add(fixsizeofimage);
			groupBox1.Controls.Add(fixoptionaheader1);
			groupBox1.Controls.Add(fixsizeofstackheap);
			groupBox1.Controls.Add(fixsizeofinitdata);
			groupBox1.Controls.Add(fixversioninfo);
			groupBox1.Controls.Add(fixnetspider);
			groupBox1.Location = new System.Drawing.Point(12, 93);
			groupBox1.Name = "groupBox1";
			groupBox1.Size = new System.Drawing.Size(516, 110);
			groupBox1.TabIndex = 37;
			groupBox1.TabStop = false;
			groupBox1.Text = "Portable Executable";
			pe_down.Location = new System.Drawing.Point(475, 70);
			pe_down.Name = "pe_down";
			pe_down.Size = new System.Drawing.Size(40, 40);
			pe_down.TabIndex = 42;
			pe_down.Click += new System.EventHandler(Pe_downClick);
			pe_up.Location = new System.Drawing.Point(475, 7);
			pe_up.Name = "pe_up";
			pe_up.Size = new System.Drawing.Size(40, 40);
			pe_up.TabIndex = 41;
			pe_up.Click += new System.EventHandler(Pe_upClick);
			fixdatadirectories.Checked = true;
			fixdatadirectories.CheckState = System.Windows.Forms.CheckState.Checked;
			fixdatadirectories.Location = new System.Drawing.Point(148, 80);
			fixdatadirectories.Name = "fixdatadirectories";
			fixdatadirectories.Size = new System.Drawing.Size(122, 24);
			fixdatadirectories.TabIndex = 34;
			fixdatadirectories.Text = "Fix DataDirectories";
			fixdatadirectories.UseVisualStyleBackColor = true;
			groupBox2.Controls.Add(netpe_down);
			groupBox2.Controls.Add(netpe_up);
			groupBox2.Controls.Add(fiximporttable);
			groupBox2.Controls.Add(fixBSJB);
			groupBox2.Controls.Add(fixrelocations);
			groupBox2.Controls.Add(fixnetdirectory);
			groupBox2.Controls.Add(fixnumberofrvaandsizes);
			groupBox2.Controls.Add(fixnetmetadata);
			groupBox2.Location = new System.Drawing.Point(12, 209);
			groupBox2.Name = "groupBox2";
			groupBox2.Size = new System.Drawing.Size(516, 83);
			groupBox2.TabIndex = 38;
			groupBox2.TabStop = false;
			groupBox2.Text = ".NET PE";
			netpe_down.Location = new System.Drawing.Point(475, 52);
			netpe_down.Name = "netpe_down";
			netpe_down.Size = new System.Drawing.Size(40, 30);
			netpe_down.TabIndex = 43;
			netpe_down.Click += new System.EventHandler(Netpe_downClick);
			netpe_up.Location = new System.Drawing.Point(475, 6);
			netpe_up.Name = "netpe_up";
			netpe_up.Size = new System.Drawing.Size(40, 30);
			netpe_up.TabIndex = 42;
			netpe_up.Click += new System.EventHandler(Netpe_upClick);
			fiximporttable.Checked = true;
			fiximporttable.CheckState = System.Windows.Forms.CheckState.Checked;
			fiximporttable.Location = new System.Drawing.Point(336, 19);
			fiximporttable.Name = "fiximporttable";
			fiximporttable.Size = new System.Drawing.Size(104, 24);
			fiximporttable.TabIndex = 41;
			fiximporttable.Text = "Fix Import Table";
			fiximporttable.UseVisualStyleBackColor = true;
			fixBSJB.Checked = true;
			fixBSJB.CheckState = System.Windows.Forms.CheckState.Checked;
			fixBSJB.Location = new System.Drawing.Point(336, 49);
			fixBSJB.Name = "fixBSJB";
			fixBSJB.Size = new System.Drawing.Size(146, 24);
			fixBSJB.TabIndex = 36;
			fixBSJB.Text = "Fix Metadata (BSJB)";
			fixBSJB.UseVisualStyleBackColor = true;
			fixrelocations.Checked = true;
			fixrelocations.CheckState = System.Windows.Forms.CheckState.Checked;
			fixrelocations.Location = new System.Drawing.Point(174, 19);
			fixrelocations.Name = "fixrelocations";
			fixrelocations.Size = new System.Drawing.Size(156, 24);
			fixrelocations.TabIndex = 35;
			fixrelocations.Text = "Fix Relocations/EntryPoint";
			fixrelocations.UseVisualStyleBackColor = true;
			fixnetdirectory.Checked = true;
			fixnetdirectory.CheckState = System.Windows.Forms.CheckState.Checked;
			fixnetdirectory.Location = new System.Drawing.Point(174, 49);
			fixnetdirectory.Name = "fixnetdirectory";
			fixnetdirectory.Size = new System.Drawing.Size(125, 24);
			fixnetdirectory.TabIndex = 40;
			fixnetdirectory.Text = "Fix .NET Directory";
			fixnetdirectory.UseVisualStyleBackColor = true;
			fixnumberofrvaandsizes.Checked = true;
			fixnumberofrvaandsizes.CheckState = System.Windows.Forms.CheckState.Checked;
			fixnumberofrvaandsizes.Location = new System.Drawing.Point(6, 19);
			fixnumberofrvaandsizes.Name = "fixnumberofrvaandsizes";
			fixnumberofrvaandsizes.Size = new System.Drawing.Size(162, 24);
			fixnumberofrvaandsizes.TabIndex = 0;
			fixnumberofrvaandsizes.Text = "Fix NumberOfRvaAndSizes";
			fixnumberofrvaandsizes.UseVisualStyleBackColor = true;
			groupBox3.Controls.Add(checkBox14);
			groupBox3.Controls.Add(checkBox13);
			groupBox3.Controls.Add(checkBox12);
			groupBox3.Controls.Add(checkBox11);
			groupBox3.Controls.Add(net_down);
			groupBox3.Controls.Add(net_up);
			groupBox3.Controls.Add(checkBox6);
			groupBox3.Controls.Add(checkBox1);
			groupBox3.Controls.Add(checkBox2);
			groupBox3.Controls.Add(checkBox3);
			groupBox3.Controls.Add(checkBox5);
			groupBox3.Controls.Add(checkBox19);
			groupBox3.Controls.Add(checkBox4);
			groupBox3.Controls.Add(checkBox10);
			groupBox3.Controls.Add(checkBox8);
			groupBox3.Controls.Add(checkBox7);
			groupBox3.Controls.Add(checkBox9);
			groupBox3.Location = new System.Drawing.Point(12, 298);
			groupBox3.Name = "groupBox3";
			groupBox3.Size = new System.Drawing.Size(516, 161);
			groupBox3.TabIndex = 39;
			groupBox3.TabStop = false;
			groupBox3.Text = ".NET";
			checkBox14.Checked = true;
			checkBox14.CheckState = System.Windows.Forms.CheckState.Checked;
			checkBox14.Location = new System.Drawing.Point(6, 126);
			checkBox14.Name = "checkBox14";
			checkBox14.Size = new System.Drawing.Size(90, 29);
			checkBox14.TabIndex = 47;
			checkBox14.Text = "ClassLayout";
			checkBox14.UseVisualStyleBackColor = true;
			checkBox13.Checked = true;
			checkBox13.CheckState = System.Windows.Forms.CheckState.Checked;
			checkBox13.Location = new System.Drawing.Point(235, 126);
			checkBox13.Name = "checkBox13";
			checkBox13.Size = new System.Drawing.Size(138, 25);
			checkBox13.TabIndex = 46;
			checkBox13.Text = "Method/Field signature";
			checkBox13.UseVisualStyleBackColor = true;
			checkBox12.Checked = true;
			checkBox12.CheckState = System.Windows.Forms.CheckState.Checked;
			checkBox12.Location = new System.Drawing.Point(102, 128);
			checkBox12.Name = "checkBox12";
			checkBox12.Size = new System.Drawing.Size(104, 24);
			checkBox12.TabIndex = 45;
			checkBox12.Text = "Fix AssembyRef";
			checkBox12.UseVisualStyleBackColor = true;
			checkBox11.Checked = true;
			checkBox11.CheckState = System.Windows.Forms.CheckState.Checked;
			checkBox11.Location = new System.Drawing.Point(102, 73);
			checkBox11.Name = "checkBox11";
			checkBox11.Size = new System.Drawing.Size(127, 24);
			checkBox11.TabIndex = 44;
			checkBox11.Text = "Fix MethodBody Size";
			checkBox11.UseVisualStyleBackColor = true;
			net_down.Location = new System.Drawing.Point(475, 121);
			net_down.Name = "net_down";
			net_down.Size = new System.Drawing.Size(40, 40);
			net_down.TabIndex = 43;
			net_down.Click += new System.EventHandler(Net_downClick);
			net_up.Location = new System.Drawing.Point(475, 7);
			net_up.Name = "net_up";
			net_up.Size = new System.Drawing.Size(40, 40);
			net_up.TabIndex = 42;
			net_up.Click += new System.EventHandler(Net_upClick);
			checkBox6.Checked = true;
			checkBox6.CheckState = System.Windows.Forms.CheckState.Checked;
			checkBox6.Location = new System.Drawing.Point(235, 99);
			checkBox6.Name = "checkBox6";
			checkBox6.Size = new System.Drawing.Size(117, 24);
			checkBox6.TabIndex = 38;
			checkBox6.Text = "Fix Module member";
			checkBox6.UseVisualStyleBackColor = true;
			checkBox1.Checked = true;
			checkBox1.CheckState = System.Windows.Forms.CheckState.Checked;
			checkBox1.Location = new System.Drawing.Point(325, 43);
			checkBox1.Name = "checkBox1";
			checkBox1.Size = new System.Drawing.Size(123, 24);
			checkBox1.TabIndex = 37;
			checkBox1.Text = "Fix resources sizes";
			checkBox1.UseVisualStyleBackColor = true;
			textBox2.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
			textBox2.Location = new System.Drawing.Point(12, 476);
			textBox2.MaxLength = int.MaxValue;
			textBox2.Multiline = true;
			textBox2.Name = "textBox2";
			textBox2.ReadOnly = true;
			textBox2.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			textBox2.Size = new System.Drawing.Size(516, 167);
			textBox2.TabIndex = 40;
			base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			base.ClientSize = new System.Drawing.Size(540, 655);
			base.Controls.Add(textBox2);
			base.Controls.Add(groupBox3);
			base.Controls.Add(groupBox2);
			base.Controls.Add(groupBox1);
			base.Controls.Add(button3);
			base.Controls.Add(button2);
			base.Controls.Add(label1);
			base.Controls.Add(textBox1);
			base.Controls.Add(button1);
			base.Name = "MainForm";
			Text = "Universal Fixer 1.0 by CodeCracker / SND";
			groupBox1.ResumeLayout(false);
			groupBox2.ResumeLayout(false);
			groupBox3.ResumeLayout(false);
			ResumeLayout(false);
			PerformLayout();
		}
	}
}
