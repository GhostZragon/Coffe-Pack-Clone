using LitMotion;
using LitMotion.Extensions;
using Sirenix.OdinInspector;
using TMPro;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class PuzzleQuestEffectUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI popupTextPrefab;

    [SerializeField] private bool debugView = false;
    [SerializeField] private QuestCollectEffectBase collectEffectStarProgress;
    [SerializeField] private QuestCollectEffectBase collectEffectCoinCollector;

    [SerializeField] private Camera mainCam;

    private void Awake()
    {
        popupTextPrefab.gameObject.SetActive(false);
    }

    [Button]
    private void Test()
    {
        CreateEffectToStar(Vector3.zero);
    }
    
    public void CreateEffectToStar(Vector3 worldPosition)
    {
        // Debug.Log("World Position: " + worldPosition);
        // var screenPosition = mainCam.WorldToScreenPoint(worldPosition);
        var screenPosition = worldPosition;
        screenPosition.z = 0;
        Debug.Log("Screen Position: " + screenPosition);
        Creates(screenPosition);
        
        collectEffectStarProgress.CreateTrail(screenPosition);
    }
    
    public void CreateEffectToCollector(Vector3 worldPosition)
    {
        // Debug.Log("World Position: " + worldPosition);
        // var screenPosition = mainCam.WorldToScreenPoint(worldPosition);
        var screenPosition = worldPosition;
        screenPosition.z = 0;
        Debug.Log("Screen Position: " + screenPosition);
       
        Creates(screenPosition);
        
        collectEffectCoinCollector.CreateTrail(screenPosition);
    }

    private void Creates(Vector3 screenPos)
    {
        
        var text = Instantiate(popupTextPrefab, screenPos, Quaternion.identity, transform);
        text.gameObject.SetActive(true);

        LSequence.Create()
            .Append(LMotion.Create(text.transform.position.y, text.transform.position.y + 1.5f, 0.35f)
                .BindToPositionY(text.transform))
            .AppendInterval(0.4f)
            .Append(LMotion.Create(text.transform.localScale, Vector3.zero, 0.3f).BindToLocalScale(text.transform))
            .Join(LMotion.Create(1, 0, 0.31f).WithOnComplete(() => { Destroy(text.gameObject); })
                .Bind((x) => { text.alpha = x; }))
            .Run();
    }
}