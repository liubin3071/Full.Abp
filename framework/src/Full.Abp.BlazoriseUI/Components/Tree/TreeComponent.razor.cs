#region Using directives

using Microsoft.AspNetCore.Components;

#endregion

namespace Full.Abp.BlazoriseUI.Components.Tree
{
    public partial class TreeComponent<TNode> : ComponentBase
    {
        #region Members

        private TreeViewStore<TNode> store = new TreeViewStore<TNode>();

        #endregion

        #region Methods

        public void SelectNode( TNode node )
        {
            SelectedNode = node;

            StateHasChanged();
        }

        #endregion

        #region Properties

        [Parameter]
        public int DefaultExpandedDeepin { get; set; }
        /// <summary>
        /// Collection of child TreeComponent items (child nodes)
        /// </summary>
        [Parameter] public IEnumerable<TNode> Nodes { get; set; }

        /// <summary>
        /// Template to display content for the nodeData
        /// </summary>
        [Parameter] public RenderFragment<TreeViewNodeContext<TNode>> NodeContent { get; set; }

        /// <summary>
        /// Currently selected TreeComponent item/nodeData.
        /// </summary>
        [Parameter]
        public TNode SelectedNode
        {
            get => store.SelectedNode;
            set
            {
                if ( EqualityComparer<TNode>.Default.Equals( store.SelectedNode, value ) )
                    return;

                store.SelectedNode = value;

                SelectedNodeChanged.InvokeAsync( store.SelectedNode );

            }
        }

        /// <summary>
        /// Occurs when the selected TreeComponent nodeData has changed.
        /// </summary>
        [Parameter] public EventCallback<TNode> SelectedNodeChanged { get; set; }

        /// <summary>
        /// List of currently expanded TreeComponent items (child nodes).
        /// </summary>
        [Parameter] public IList<TNode> ExpandedNodes { get; set; } = new List<TNode>();

        /// <summary>
        /// Occurs when the collection of expanded nodes has changed.
        /// </summary>
        [Parameter] public EventCallback<IList<TNode>> ExpandedNodesChanged { get; set; }

        /// <summary>
        /// Gets the list of child nodes for each nodeData.
        /// </summary>
        [Parameter]
        public Func<TNode, Task<IEnumerable<TNode>>> GetChildren { get; set; } =
            (node => Task.FromResult(Enumerable.Empty<TNode>())); 

        /// <summary>
        /// Indicates if the nodeData has child elements.
        /// </summary>
        [Parameter] public Func<TNode, bool> HasChildren { get; set; } = node => true;

        [Parameter] public RenderFragment ChildContent { get; set; }

        #endregion
    }


}