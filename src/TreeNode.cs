using System;
using System.Collections.Generic;

namespace DrSearch
{
    /// <summary>
    /// Represents one item in tree, used for Boolean expression parsing.
    /// </summary>
    public class TreeNode
    {
        public String nodeValue { get; set; } //values assigned to the node
        public TreeNode leftNode { get; set; } //items present on the left side of expr tree
        public TreeNode rightNode { get; set; } //items present on the right side of expr tree
        public List<Int32> matchingArticles { get; set; } //indexes of articles which are matching for the given node
    }
}
