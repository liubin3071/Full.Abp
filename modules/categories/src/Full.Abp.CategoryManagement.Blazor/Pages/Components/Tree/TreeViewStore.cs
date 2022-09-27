#region Using directives

#endregion

using System;
using System.Collections.Generic;

namespace Full.Abp.CategoryManagement.Blazor.Pages.Components.Tree
{
    public struct TreeViewStore<TNode> : IEquatable<TreeViewStore<TNode>>
    {
        #region Methods

        public override bool Equals( object obj )
            => obj is TreeViewStore<TNode> store && Equals( store );

        public bool Equals( TreeViewStore<TNode> other )
        {
            return EqualityComparer<TNode>.Default.Equals( SelectedNode, other.SelectedNode );
        }

        public override int GetHashCode()
        {
            int result = 0;

            if ( SelectedNode != null )
                result ^= SelectedNode.GetHashCode();

            return result;
        }

        public static bool operator ==( TreeViewStore<TNode> lhs, TreeViewStore<TNode> rhs )
        {
            return lhs.Equals( rhs );
        }

        public static bool operator !=( TreeViewStore<TNode> lhs, TreeViewStore<TNode> rhs )
        {
            return !lhs.Equals( rhs );
        }

        #endregion

        #region Properties

        public TNode SelectedNode { readonly get; set; }

        #endregion
    }
}
