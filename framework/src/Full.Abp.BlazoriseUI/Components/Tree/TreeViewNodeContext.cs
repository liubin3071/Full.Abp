namespace Full.Abp.BlazoriseUI.Components.Tree;

public class TreeViewNodeContext<TNodeData>
{
    public TNodeData Nodedata { get; set; }
 
    public TreeNodeComponent<TNodeData> NodeComponent { get; set; }
}