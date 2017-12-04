using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BFForever.Riff;
using BFForever;
using System.IO;

using System.Reflection;
using System.Runtime.InteropServices;

namespace Riff_Explorer
{
    public partial class Main : Form
    {
        private static readonly string[] NoteNames = new string[] { "C", "C#", "D", "D#", "E", "F", "F#", "G", "G#", "A", "A#", "B" };
        SongManager sm;
        string directory;

        public Main()
        {
            InitializeComponent();
        }

        OpenFileDialog ofd = new OpenFileDialog();
        private void ddb_item_open_Click(object sender, EventArgs e)
        {
            ofd.Title = "Open";
            //ofd.Filter = "Rif|*.rif";
            ofd.Filter = "Index2 Rif|index2.rif";


            if (ofd.ShowDialog() == DialogResult.OK)
            {
                directory = Path.GetDirectoryName(ofd.FileName);
                this.Text = $"Riff Explorer - {directory}";

                // Opens files from directory
                sm = new SongManager(directory);

                //rif.Import(ofd.FileName);
                GenerateNodes();
            }

            //rif = new RiffFile();

            //if (rif.Objects[0] is Song) MessageBox.Show("This is a song!");
            
        }

        private void GenerateNodes()
        {
            // Clears nodes
            indexTree.Nodes.Clear();

            TreeNode root = new TreeNode("Archive");
            root.ImageKey = "folder_closed.png";
            root.SelectedImageKey = "folder_closed.png";
            root.Name = root.Text;

            TreeNode tn = root;
            foreach (Index2Entry entry in sm.IndexEntries)
            {
                tn = root;
                string currentPath = "";
                string[] splitNames = entry.InternalPath.Value.Split('.');

                for (int i = 0; i < splitNames.Length; i++)
                {
                    currentPath += splitNames[i];
                    if (i == (splitNames.Length - 1)) tn = AddNode(tn, currentPath, splitNames[i], false);
                    else
                    {
                        currentPath += ".";
                        tn = AddNode(tn, currentPath, splitNames[i], true);
                    }
                }
            }

            root.Expand();
            indexTree.Nodes.Add(root); // It's already well sorted
            //indexTree.Sort();
        }

        private TreeNode AddNode(TreeNode node, string key, string text, bool folder)
        {
            if (node.Nodes.ContainsKey(key))
            {
                return node.Nodes[key];
            }
            else
            {
                TreeNode temp = new TreeNode();
                temp.Text = text;
                temp.Name = key;

                if (folder)
                {
                    temp.ImageKey = "folder_closed.png";
                    temp.SelectedImageKey = "folder_closed.png";
                    node.Nodes.Add(temp);
                    return node.Nodes[key];
                }
                else
                {
                    temp.ImageKey = "page_white_text.png";
                    temp.SelectedImageKey = "page_white_text.png";

                    node.Nodes.Add(temp);
                    return node.Nodes[key];
                }
            }
        }
        

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            listView1.Items.Clear();
            tb_idxkey.Clear();
            tb_objkey.Clear();

        }

        private void Main_Load(object sender, EventArgs e)
        {
            
        }

        FolderBrowserDialog fbd = new FolderBrowserDialog();
        SaveFileDialog sfd = new SaveFileDialog();

        private void exportStringsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // This exports all strings from all rif files found
            fbd.Description = "Open Folder";
            if (!(fbd.ShowDialog() == DialogResult.OK)) return;

            sfd.Title = "Save";
            sfd.Filter = "Text|*.txt";
            sfd.InitialDirectory = fbd.SelectedPath;

            if (!(sfd.ShowDialog() == DialogResult.OK)) return;

            string[] files = Directory.GetFiles(fbd.SelectedPath, "*.rif", SearchOption.AllDirectories);

            foreach(string file in files)
            {
                RiffFile rif = new RiffFile();

                try
                {
                    // Loads strings from file
                    rif.Import(file);
                }
                catch
                {

                }
            }

            // Exports all strings
            StringKey.ExportToFile(sfd.FileName);
            MessageBox.Show("Exported strings successfully!");
        }

        private string GetTypeName(object obj)
        {
            if (obj == null) return "";

            string text;

            if (obj is IEnumerable<object>)
            {
                text = ((IEnumerable<object>)obj).GetType().GetGenericArguments()[0].ToString();
            }
            else
                text = obj.GetType().ToString();

            int idx = text.LastIndexOf('.');

            if (idx != -1)
                // Removes periods
                text = text.Remove(0, idx + 1);

            return text;
        }

        private void treeView1_BeforeCollapse(object sender, TreeViewCancelEventArgs e)
        {
            if (e.Node.Tag != null) return;

            e.Node.ImageKey = "folder_closed.png";
            e.Node.SelectedImageKey = "folder_closed.png";
        }

        private void treeView1_BeforeExpand(object sender, TreeViewCancelEventArgs e)
        {
            if (e.Node.Tag != null) return;

            e.Node.ImageKey = "folder_open.png";
            e.Node.SelectedImageKey = "folder_open.png";
        }

        private void listView1_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            textBox1.Clear();
            comboBox1.Items.Clear();

            if (e.Item.Tag == null)
            {
                textBox1.Enabled = false;
                return;
            }

            if (e.Item.Tag is IEnumerable<object>)
            {
                textBox1.Enabled = false;
                comboBox1.Enabled = true;

                string name = GetTypeName(e.Item.Tag);
                int count = 0;

                foreach (object obj in e.Item.Tag as IEnumerable<object>)
                {
                    if (obj is string)
                        comboBox1.Items.Add(new ComboboxItem((string)obj, obj));
                    else
                        comboBox1.Items.Add(new ComboboxItem(name + "[" + count + "]", obj));
                        
                    count++;
                }

                if (comboBox1.Items.Count > 0) comboBox1.SelectedIndex = 0;
            }
            else
            {
                textBox1.Enabled = true;
                textBox1.Text = e.Item.Tag.ToString();
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        
        private void writeEntriesToFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (indexTree.SelectedNode.Tag == null || !(indexTree.SelectedNode.Tag is Index2)) return;
            Index2 index = indexTree.SelectedNode.Tag as Index2;

            sfd.Title = "Save Index2 Entries";
            sfd.Filter = "Text|*.txt";
            //sfd.InitialDirectory = fbd.SelectedPath;

            if (!(sfd.ShowDialog() == DialogResult.OK)) return;

            //index.WriteEntries(sfd.FileName);
            MessageBox.Show("Entries saved successfully!");

        }

        private void ddb_item_save_Click(object sender, EventArgs e)
        {
            sfd.Title = "Save Riff file";
            sfd.Filter = "Riff|*.rif";
            if (!(sfd.ShowDialog() == DialogResult.OK)) return;

            //rif.SaveToFile(sfd.FileName);
            MessageBox.Show("Riff file saved successfully!");
        }

        private void exportIndex2ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            sfd.Title = "Save Index2 Entries";
            sfd.Filter = "JSON|*.json";
            //sfd.InitialDirectory = fbd.SelectedPath;

            if (!(sfd.ShowDialog() == DialogResult.OK)) return;

            sm.ExportIndex2(sfd.FileName);
            MessageBox.Show("Exported Index2 successfully!");
        }

        private void exportCatalog2ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            sfd.Title = "Save Catalog2 Entries";
            sfd.Filter = "JSON|*.json";
            //sfd.InitialDirectory = fbd.SelectedPath;

            if (!(sfd.ShowDialog() == DialogResult.OK)) return;

            sm.ExportCatalog2(sfd.FileName);
            MessageBox.Show("Exported Catalog2 successfully!");
        }

        private void exportPackageDefToolStripMenuItem_Click(object sender, EventArgs e)
        {
            sfd.Title = "Save PackageDef Entries";
            sfd.Filter = "JSON|*.json";
            //sfd.InitialDirectory = fbd.SelectedPath;

            if (!(sfd.ShowDialog() == DialogResult.OK)) return;

            sm.ExportCatalog2(sfd.FileName);
            MessageBox.Show("Exported PackageDef successfully!");
        }
    }

    public class ObjectPair
    {
        public string Parent { get; set; }
        public string Child { get; set; }
    }

    public class ObjectComparer : IEqualityComparer<ObjectPair>
    {
        public bool Equals(ObjectPair x, ObjectPair y)
        {
            if (x.Parent == y.Parent && x.Child == y.Child) return true;
            else return false;
        }

        public int GetHashCode(ObjectPair obj)
        {
            string pair = obj.Parent + obj.Parent;
            return pair.GetHashCode();
        }
    }
    
}
