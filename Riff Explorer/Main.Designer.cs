namespace Riff_Explorer
{
    partial class Main
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Main));
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.ddb_file = new System.Windows.Forms.ToolStripDropDownButton();
            this.ddb_item_open = new System.Windows.Forms.ToolStripMenuItem();
            this.ddb_item_exportstrings = new System.Windows.Forms.ToolStripMenuItem();
            this.ddb_item_save = new System.Windows.Forms.ToolStripMenuItem();
            this.exportIndex2ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exportCatalog2ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.indexTree = new System.Windows.Forms.TreeView();
            this.objectImages = new System.Windows.Forms.ImageList(this.components);
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.listView1 = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.button1 = new System.Windows.Forms.Button();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.button2 = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.lbl_idxkey = new System.Windows.Forms.Label();
            this.tb_idxkey = new System.Windows.Forms.TextBox();
            this.tb_objkey = new System.Windows.Forms.TextBox();
            this.lbl_objkey = new System.Windows.Forms.Label();
            this.cms_index2 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.writeEntriesToFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exportPackageDefToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.cms_index2.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ddb_file});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(784, 25);
            this.toolStrip1.TabIndex = 0;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // ddb_file
            // 
            this.ddb_file.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.ddb_file.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ddb_item_open,
            this.ddb_item_exportstrings,
            this.ddb_item_save,
            this.exportIndex2ToolStripMenuItem,
            this.exportCatalog2ToolStripMenuItem,
            this.exportPackageDefToolStripMenuItem});
            this.ddb_file.Image = ((System.Drawing.Image)(resources.GetObject("ddb_file.Image")));
            this.ddb_file.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ddb_file.Name = "ddb_file";
            this.ddb_file.Size = new System.Drawing.Size(38, 22);
            this.ddb_file.Text = "File";
            // 
            // ddb_item_open
            // 
            this.ddb_item_open.Name = "ddb_item_open";
            this.ddb_item_open.Size = new System.Drawing.Size(172, 22);
            this.ddb_item_open.Text = "Open";
            this.ddb_item_open.Click += new System.EventHandler(this.ddb_item_open_Click);
            // 
            // ddb_item_exportstrings
            // 
            this.ddb_item_exportstrings.Name = "ddb_item_exportstrings";
            this.ddb_item_exportstrings.Size = new System.Drawing.Size(172, 22);
            this.ddb_item_exportstrings.Text = "Export Strings";
            this.ddb_item_exportstrings.Click += new System.EventHandler(this.exportStringsToolStripMenuItem_Click);
            // 
            // ddb_item_save
            // 
            this.ddb_item_save.Name = "ddb_item_save";
            this.ddb_item_save.Size = new System.Drawing.Size(172, 22);
            this.ddb_item_save.Text = "Save";
            this.ddb_item_save.Click += new System.EventHandler(this.ddb_item_save_Click);
            // 
            // exportIndex2ToolStripMenuItem
            // 
            this.exportIndex2ToolStripMenuItem.Name = "exportIndex2ToolStripMenuItem";
            this.exportIndex2ToolStripMenuItem.Size = new System.Drawing.Size(172, 22);
            this.exportIndex2ToolStripMenuItem.Text = "Export Index2";
            this.exportIndex2ToolStripMenuItem.Click += new System.EventHandler(this.exportIndex2ToolStripMenuItem_Click);
            // 
            // exportCatalog2ToolStripMenuItem
            // 
            this.exportCatalog2ToolStripMenuItem.Name = "exportCatalog2ToolStripMenuItem";
            this.exportCatalog2ToolStripMenuItem.Size = new System.Drawing.Size(172, 22);
            this.exportCatalog2ToolStripMenuItem.Text = "Export Catalog2";
            this.exportCatalog2ToolStripMenuItem.Click += new System.EventHandler(this.exportCatalog2ToolStripMenuItem_Click);
            // 
            // indexTree
            // 
            this.indexTree.Dock = System.Windows.Forms.DockStyle.Fill;
            this.indexTree.ImageKey = "page_white_text.png";
            this.indexTree.ImageList = this.objectImages;
            this.indexTree.Location = new System.Drawing.Point(3, 16);
            this.indexTree.Name = "indexTree";
            this.indexTree.SelectedImageIndex = 0;
            this.indexTree.Size = new System.Drawing.Size(312, 412);
            this.indexTree.TabIndex = 1;
            this.indexTree.BeforeCollapse += new System.Windows.Forms.TreeViewCancelEventHandler(this.treeView1_BeforeCollapse);
            this.indexTree.BeforeExpand += new System.Windows.Forms.TreeViewCancelEventHandler(this.treeView1_BeforeExpand);
            this.indexTree.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeView1_AfterSelect);
            // 
            // objectImages
            // 
            this.objectImages.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("objectImages.ImageStream")));
            this.objectImages.TransparentColor = System.Drawing.Color.Transparent;
            this.objectImages.Images.SetKeyName(0, "page_white_text.png");
            this.objectImages.Images.SetKeyName(1, "folder_closed.png");
            this.objectImages.Images.SetKeyName(2, "folder_open.png");
            // 
            // splitContainer1
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.splitContainer1, 4);
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(3, 28);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.groupBox2);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.groupBox1);
            this.splitContainer1.Size = new System.Drawing.Size(778, 431);
            this.splitContainer1.SplitterDistance = 318;
            this.splitContainer1.TabIndex = 2;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.indexTree);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox2.Location = new System.Drawing.Point(0, 0);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(318, 431);
            this.groupBox2.TabIndex = 2;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Objects";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.tableLayoutPanel2);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(456, 431);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Data";
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 4;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 65F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 35F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 77F));
            this.tableLayoutPanel2.Controls.Add(this.listView1, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.button1, 0, 1);
            this.tableLayoutPanel2.Controls.Add(this.comboBox1, 1, 1);
            this.tableLayoutPanel2.Controls.Add(this.button2, 3, 1);
            this.tableLayoutPanel2.Controls.Add(this.textBox1, 0, 2);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(3, 16);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 3;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 70F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 30F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(450, 412);
            this.tableLayoutPanel2.TabIndex = 1;
            // 
            // listView1
            // 
            this.listView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2});
            this.tableLayoutPanel2.SetColumnSpan(this.listView1, 4);
            this.listView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listView1.FullRowSelect = true;
            this.listView1.Location = new System.Drawing.Point(3, 3);
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(444, 264);
            this.listView1.TabIndex = 0;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.View = System.Windows.Forms.View.Details;
            this.listView1.ItemSelectionChanged += new System.Windows.Forms.ListViewItemSelectionChangedEventHandler(this.listView1_ItemSelectionChanged);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Item";
            this.columnHeader1.Width = 150;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "Value";
            this.columnHeader2.Width = 390;
            // 
            // button1
            // 
            this.button1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.button1.Enabled = false;
            this.button1.Location = new System.Drawing.Point(3, 273);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(34, 19);
            this.button1.TabIndex = 1;
            this.button1.Text = "<";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // comboBox1
            // 
            this.comboBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.comboBox1.Enabled = false;
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(43, 273);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(210, 21);
            this.comboBox1.TabIndex = 2;
            this.comboBox1.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            // 
            // button2
            // 
            this.button2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.button2.Enabled = false;
            this.button2.Location = new System.Drawing.Point(375, 273);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(72, 19);
            this.button2.TabIndex = 3;
            this.button2.Text = "Save";
            this.button2.UseVisualStyleBackColor = true;
            // 
            // textBox1
            // 
            this.tableLayoutPanel2.SetColumnSpan(this.textBox1, 4);
            this.textBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBox1.Enabled = false;
            this.textBox1.Location = new System.Drawing.Point(3, 298);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(444, 111);
            this.textBox1.TabIndex = 4;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 4;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 70F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 70F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.splitContainer1, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.lbl_idxkey, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.tb_idxkey, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.tb_objkey, 3, 2);
            this.tableLayoutPanel1.Controls.Add(this.lbl_objkey, 2, 2);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 25);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(784, 487);
            this.tableLayoutPanel1.TabIndex = 3;
            // 
            // lbl_idxkey
            // 
            this.lbl_idxkey.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lbl_idxkey.AutoSize = true;
            this.lbl_idxkey.Location = new System.Drawing.Point(3, 468);
            this.lbl_idxkey.Name = "lbl_idxkey";
            this.lbl_idxkey.Size = new System.Drawing.Size(57, 13);
            this.lbl_idxkey.TabIndex = 3;
            this.lbl_idxkey.Text = "Index Key:";
            this.lbl_idxkey.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // tb_idxkey
            // 
            this.tb_idxkey.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.tb_idxkey.Location = new System.Drawing.Point(73, 465);
            this.tb_idxkey.Name = "tb_idxkey";
            this.tb_idxkey.ReadOnly = true;
            this.tb_idxkey.Size = new System.Drawing.Size(316, 20);
            this.tb_idxkey.TabIndex = 5;
            // 
            // tb_objkey
            // 
            this.tb_objkey.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.tb_objkey.Location = new System.Drawing.Point(465, 465);
            this.tb_objkey.Name = "tb_objkey";
            this.tb_objkey.ReadOnly = true;
            this.tb_objkey.Size = new System.Drawing.Size(316, 20);
            this.tb_objkey.TabIndex = 6;
            // 
            // lbl_objkey
            // 
            this.lbl_objkey.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lbl_objkey.AutoSize = true;
            this.lbl_objkey.Location = new System.Drawing.Point(395, 468);
            this.lbl_objkey.Name = "lbl_objkey";
            this.lbl_objkey.Size = new System.Drawing.Size(62, 13);
            this.lbl_objkey.TabIndex = 4;
            this.lbl_objkey.Text = "Object Key:";
            this.lbl_objkey.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // cms_index2
            // 
            this.cms_index2.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.cms_index2.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.writeEntriesToFileToolStripMenuItem});
            this.cms_index2.Name = "cms_index2";
            this.cms_index2.Size = new System.Drawing.Size(178, 26);
            // 
            // writeEntriesToFileToolStripMenuItem
            // 
            this.writeEntriesToFileToolStripMenuItem.Name = "writeEntriesToFileToolStripMenuItem";
            this.writeEntriesToFileToolStripMenuItem.Size = new System.Drawing.Size(177, 22);
            this.writeEntriesToFileToolStripMenuItem.Text = "Write Entries To File";
            this.writeEntriesToFileToolStripMenuItem.Click += new System.EventHandler(this.writeEntriesToFileToolStripMenuItem_Click);
            // 
            // exportPackageDefToolStripMenuItem
            // 
            this.exportPackageDefToolStripMenuItem.Name = "exportPackageDefToolStripMenuItem";
            this.exportPackageDefToolStripMenuItem.Size = new System.Drawing.Size(172, 22);
            this.exportPackageDefToolStripMenuItem.Text = "Export PackageDef";
            this.exportPackageDefToolStripMenuItem.Click += new System.EventHandler(this.exportPackageDefToolStripMenuItem_Click);
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(784, 512);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.toolStrip1);
            this.Name = "Main";
            this.Text = "Riff Explorer";
            this.Load += new System.EventHandler(this.Main_Load);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.cms_index2.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripDropDownButton ddb_file;
        private System.Windows.Forms.ToolStripMenuItem ddb_item_open;
        private System.Windows.Forms.TreeView indexTree;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.ToolStripMenuItem ddb_item_exportstrings;
        private System.Windows.Forms.ImageList objectImages;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label lbl_idxkey;
        private System.Windows.Forms.Label lbl_objkey;
        private System.Windows.Forms.TextBox tb_idxkey;
        private System.Windows.Forms.TextBox tb_objkey;
        private System.Windows.Forms.ContextMenuStrip cms_index2;
        private System.Windows.Forms.ToolStripMenuItem writeEntriesToFileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ddb_item_save;
        private System.Windows.Forms.ToolStripMenuItem exportIndex2ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exportCatalog2ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exportPackageDefToolStripMenuItem;
    }
}

