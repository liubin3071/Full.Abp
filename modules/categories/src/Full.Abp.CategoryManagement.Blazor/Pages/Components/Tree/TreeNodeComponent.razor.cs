#region Using directives

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

#endregion

namespace Full.Abp.CategoryManagement.Blazor.Pages.Components.Tree
{
    public partial class TreeNodeComponent<TNode> : ComponentBase
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
            Children = await GetChildNodes(Node);
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
                Children = Enumerable.Empty<TNode>();
            }

            IsExpanded = newExpand;
        }


        protected void OnToggleNode(TNode node, bool expand)
        {
            bool expanded = ExpandedNodes.Contains(node);

            if (expanded && !expand)
            {
                ExpandedNodes.Remove(node);
                ExpandedNodesChanged.InvokeAsync(ExpandedNodes);
            }
            else if (!expanded && expand)
            {
                ExpandedNodes.Add(node);
                ExpandedNodesChanged.InvokeAsync(ExpandedNodes);
            }

            StateHasChanged();
        }

        #endregion

        #region Properties

        [Parameter] public int DefaultExpandedDeepin { get; set; }

        [Parameter] public IEnumerable<TNode> Children { get; set; }

        [Parameter] public int Deepin { get; set; } = 0;

        [Parameter] public TNode Node { get; set; }

        [Parameter] public RenderFragment<TreeViewNodeContext<TNode>> NodeContent { get; set; }

        [Parameter] public IList<TNode> ExpandedNodes { get; set; } = new List<TNode>();

        [Parameter] public EventCallback<IList<TNode>> ExpandedNodesChanged { get; set; }

        [Parameter] public Func<TNode, Task<IEnumerable<TNode>>> GetChildNodes { get; set; }

        [Parameter] public Func<TNode, bool> HasChildNodes { get; set; } = node => true;

        public bool IsExpanded { get; private set; } = false;

        [CascadingParameter] public TreeNodeComponent<TNode> ParentNodeComponent { get; set; }

        [Parameter] public RenderFragment ChildContent { get; set; }

        #endregion
    }
}