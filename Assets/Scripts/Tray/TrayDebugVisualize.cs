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

    public void Refresh(PriorityTray priorityTray)
    {
        if (priorityTray == null)
        {
            TextMeshPro.text = "";
            return;
        }
        StringBuilder stringBuilder = new("Debug:");
        stringBuilder.AppendLine($"Current Item Checking: {priorityTray.MainItemID}");
        stringBuilder.AppendLine($"Main Level: {priorityTray.MainLevel}");
        stringBuilder.AppendLine($"Sub Level: {priorityTray.SubLevel}");
        stringBuilder.AppendLine($"Is Placed by Player: {priorityTray.isPlacedSlot}");
        TextMeshPro.text = stringBuilder.ToString();
    }
}