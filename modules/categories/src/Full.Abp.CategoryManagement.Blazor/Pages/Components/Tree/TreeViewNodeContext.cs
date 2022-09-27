namespace Full.Abp.CategoryManagement.Blazor.Pages.Components.Tree;

public class TreeViewNodeContext<TNode>
{
    public TNode Node { get; set; }
 
    public TreeNodeComponent<TNode> NodeComponentRef { get; set; }
}