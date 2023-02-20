using MaraudersModManager.Controls.Base.View;

namespace MaraudersModManager.UI;

public class ViewBucket
{
    public ViewBucket(IBaseView viewRef) => ViewRef = viewRef;

    public IBaseView ViewRef { get; set; }
}