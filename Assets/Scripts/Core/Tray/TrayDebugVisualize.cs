using System.Text;
using TMPro;
using UnityEngine;

public class TrayDebugVisualize : MonoBehaviour
{
    public TextMeshPro TextMeshPro;

    private void Awake()
    {
        TextMeshPro.text = "";
    }

    public void Refresh(TrayMergeCandidate trayMergeCandidate)
    {
        if (trayMergeCandidate == null)
        {
            TextMeshPro.text = "";
            return;
        }
        StringBuilder stringBuilder = new("Debug:");
        stringBuilder.AppendLine($"Current Item Checking: {trayMergeCandidate.ItemId}");
        stringBuilder.AppendLine($"Main Level: {trayMergeCandidate.MainLevel}");
        stringBuilder.AppendLine($"Sub Level: {trayMergeCandidate.SubLevel}");
        stringBuilder.AppendLine($"Is Placed by Player: {trayMergeCandidate.isOriginTray}");
        TextMeshPro.text = stringBuilder.ToString();
    }
}