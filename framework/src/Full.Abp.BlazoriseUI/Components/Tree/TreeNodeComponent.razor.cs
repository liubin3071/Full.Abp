#region Using directives

using Microsoft.AspNetCore.Components;

#endregion

namespace Full.Abp.BlazoriseUI.Components.Tree
{
    public partial class TreeNodeComponent<TNodeData> : ComponentBase
    {
        #region Members

        private bool _childrenFixed = false;

        #endregion

        #region Methods

        public override async Task SetParametersAsync(ParameterView parameters)
        {
            await base.SetParametersAsync(parameters);
            if (IsExpanded)
            {
                await UpdateChildren();
            }
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await base.OnAfterRenderAsync(firstRender);
            if (firstRender && DefaultExpandedDeepin > Deepin)
            {
                IsExpanded = true;
                await UpdateChildren();
            }
        }

        public async Task UpdateChildren()
        {
            Children = await GetChildren(NodeData);
            await InvokeAsync(StateHasChanged);
        }

        public async Task Toggle(bool? expand = null)
        {
            var newExpand = expand ?? !IsExpanded;
            if (newExpand)
            {
                await UpdateChildren();
            }
            else
            {
                Children = Enumerable.Empty<TNodeData>();
            }

            IsExpanded = newExpand;
        }


        protected void OnToggleNode(TNodeData nodeData, bool expand)
        {
            bool expanded = ExpandedNodes.Contains(nodeData);

            if (expanded && !expand)
            {
                ExpandedNodes.Remove(nodeData);
                ExpandedNodesChanged.InvokeAsync(ExpandedNodes);
            }
            else if (!expanded && expand)
            {
                ExpandedNodes.Add(nodeData);
                ExpandedNodesChanged.InvokeAsync(ExpandedNodes);
            }

            StateHasChanged();
        }

        #endregion

        #region Properties

        [Parameter] public int DefaultExpandedDeepin { get; set; }

        [Parameter] public IEnumerable<TNodeData> Children { get; set; }

        [Parameter] public int Deepin { get; set; } = 0;

        [Parameter] public TNodeData NodeData { get; set; }

        [Parameter] public RenderFragment<TreeViewNodeContext<TNodeData>> NodeContent { get; set; }

        [Parameter] public IList<TNodeData> ExpandedNodes { get; set; } = new List<TNodeData>();

        [Parameter] public EventCallback<IList<TNodeData>> ExpandedNodesChanged { get; set; }

        [Parameter] public Func<TNodeData, Task<IEnumerable<TNodeData>>> GetChildren { get; set; }

        [Parameter] public Func<TNodeData, bool> HasChildren { get; set; } = node => true;

        public bool IsExpanded { get; private set; } = false;

        [CascadingParameter] public TreeNodeComponent<TNodeData> ParentNodeComponent { get; set; }

        [Parameter] public RenderFragment ChildContent { get; set; }

        #endregion
    }
}