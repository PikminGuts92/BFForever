﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Win32; // OpenFileDialog
using System.ComponentModel; // SortDescription
using IO = System.IO;
using BFForever;
using BFForever.Audio;
using BFForever.Riff2;
using BFForever.Texture;

namespace RiffExplorer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private FEnvironment manager = new FEnvironment();

        public MainWindow()
        {
            InitializeComponent();
        }

        private void UnregisterNode(TreeViewItem parent)
        {
            foreach (TreeViewItem child in parent.Items)
            {
                UnregisterNode(child);
            }

            // Unregisters name
            TreeView_Archive.UnregisterName(parent.Name);
        }

        private void SortNode(TreeViewItem parent)
        {
            foreach (TreeViewItem child in parent.Items)
            {
                SortNode(child);
            }

            // Sorts nodes
            parent.Items.SortDescriptions.Clear();
            parent.Items.SortDescriptions.Add(new SortDescription("Name", System.ComponentModel.ListSortDirection.Ascending));
        }

        private string ToFilePath(Index2Entry entry)
        {
            // i.e. songs.dragonforce.ttfaf.fused.rif -> songs/dragonforce/ttfaf/fused.rif
            int lastIdx = entry.FilePath.Value.LastIndexOf(".");
            if (lastIdx == -1) return entry.FilePath.Value.ToLower();

            if (entry.IsZObject())
                lastIdx++;

            char[] chars = entry.FilePath.Value.ToCharArray();
            for (int i = 0; i < lastIdx; i++)
            {
                if (chars[i] == '.') chars[i] = '/';
            }

            return new string(chars).ToLower();
        }

        private void RefreshFileTree()
        {
            // Unregisters nodes
            foreach (TreeViewItem node in TreeView_Archive.Items)
                UnregisterNode(node);

            TreeView_Archive.Items.Clear();

            if (manager.Index == null || manager.Definition == null) return;

            TreeViewItem root = new TreeViewItem();
            root.Header = manager.Definition.PackageName;
            root.Tag = manager;
            root.Name = "_";
            TreeView_Archive.RegisterName("_", root);
            //root.ContextMenu = TreeView_Archive.Resources["CM_Directory"] as ContextMenu;

            TreeViewItem tn = root;
            foreach (Index2Entry entry in manager.Index.Entries)
            {
                tn = root;
                string currentPath = "";
                string[] splitNames = ToFilePath(entry).Split('/');

                for (int i = 0; i < splitNames.Length; i++)
                {
                    currentPath += splitNames[i];
                    if (i == (splitNames.Length - 1))
                        // File entry
                        tn = AddNode(tn, currentPath, splitNames[i], false, entry);
                    else
                    {
                        // Folder entry
                        currentPath += "/";
                        tn = AddNode(tn, currentPath, splitNames[i], true, entry);
                    }
                }
            }

            root.Items.SortDescriptions.Clear();
            root.Items.SortDescriptions.Add(new SortDescription("Name", System.ComponentModel.ListSortDirection.Ascending));
            //root.Items.Refresh();

            // Sorts nodes
            foreach (TreeViewItem node in root.Items)
                SortNode(node);

            TreeView_Archive.Items.Add(root);
        }

        private string CreateKey(string path, bool folder)
        {
            if (path == "") return "_";

            List<char> newKey = new List<char>();

            newKey.Add((folder) ? 'a' : 'b'); // So folders sort first

            foreach (char c in path)
            {
                if ((c >= '0' && c <= '9') || (c >= 'a' && c <= 'z') || (c >= 'A' && c <= 'Z'))
                    newKey.Add(c);
                else if (c == '.')
                {
                    // So that periods sort before slashes
                    newKey.Add('_');
                    newKey.Add('a');
                }
                else if (c == '_')
                {
                    newKey.Add('_');
                    newKey.Add('b');
                }
                else
                    newKey.Add('_');
            }

            // Returns unique key
            return string.Concat(newKey);
        }

        private TreeViewItem AddNode(TreeViewItem parent, string currentPath, string text, bool folder, Index2Entry entry)
        {
            //node.Items.Cast<TreeViewItem>().
            string key = CreateKey(currentPath, folder);
            object needle = TreeView_Archive.FindName(key);

            if (needle != null)
                return needle as TreeViewItem;
            else
            {
                //TreeFileEntry temp = new TreeFileEntry();
                TreeViewItem child = new TreeViewItem();
                child.Header = text;
                child.Name = key;
                //temp.Path = currentPath;
                TreeView_Archive.RegisterName(key, child);

                int returnIdx = parent.Items.Add(child);
                if (!folder) child.Tag = entry;
                SetNodeProperties(child);

                return parent.Items[returnIdx] as TreeViewItem;
            }
        }

        private void SetNodeProperties(TreeViewItem node)
        {
            // TODO: Implement
        }

        private void ToolBar_Loaded(object sender, RoutedEventArgs e)
        {
            // Hides the stupid overflow arrow
            // Source: http://stackoverflow.com/questions/4662428/how-to-hide-arrow-on-right-side-of-a-toolbar

            ToolBar toolBar = sender as ToolBar;
            var overflowGrid = toolBar.Template.FindName("OverflowGrid", toolBar) as FrameworkElement;
            if (overflowGrid != null)
            {
                overflowGrid.Visibility = Visibility.Collapsed;
            }

            var mainPanelBorder = toolBar.Template.FindName("MainPanelBorder", toolBar) as FrameworkElement;
            if (mainPanelBorder != null)
            {
                mainPanelBorder.Margin = new Thickness(0);
            }
        }

        private void Menu_File_Open_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Index2|index2.rif";
            if (ofd.ShowDialog() == false) return;

            manager.LoadPackage(IO.Path.GetDirectoryName(ofd.FileName));
            RefreshFileTree();
        }

        private void Menu_File_Exit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
