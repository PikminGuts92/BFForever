using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Riff_Explorer
{
    public class NodeSorter : IComparer<TreeNode>
    {
        /// <summary>
        /// Custom sorter - Tagged nodes take precedence
        /// </summary>
        /// <param name="x">Node X</param>
        /// <param name="y">Node Y</param>
        /// <returns>Result</returns>
        public int Compare(TreeNode x, TreeNode y)
        {
            if ((x.Tag != null && y.Tag != null) || x.Tag == null && y.Tag == null)
                // Compares strings if both have tags
                return string.Compare(x.Text, y.Text);
            else if (x.Tag != null && y.Tag == null) return 1;
            else return -1;
        }
    }
}
