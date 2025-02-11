using LitMotion;
using LitMotion.Extensions;
using Sirenix.OdinInspector;
using TMPro;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class PuzzleQuestEffectUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI popupTextPrefab;
    [SerializeField] private GameObject trailPrefab;
    [SerializeField] private Camera mainCam;
    [SerializeField] private QuestStageUI questStageUI;
    private Vector3 starPosition;

    private void Awake()
    {
        mainCam = Camera.main;
        popupTextPrefab.gameObject.SetActive(false);
        trailPrefab.gameObject.SetActive(false);
    }

    private void QuestStageUIOnOnStarChanged(Vector3 newPosition)
    {
        starPosition = newPosition;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            Test();
        }
    }

    [Button]
    private void Test()
    {
        CreateEffect(Vector3.zero);
    }

    public void CreateEffect(Vector3 worldPosition)
    {
        var screenPosition = mainCam.WorldToScreenPoint(worldPosition);
        Creates(screenPosition);
        CreateTrailToStar(worldPosition);
    }

    private void Creates(Vector3 screenPos)
    {
        var text = Instantiate(popupTextPrefab, screenPos, Quaternion.identity, transform);
        text.gameObject.SetActive(true);

        LSequence.Create()
            .Append(LMotion.Create(text.transform.position.y, text.transform.position.y + 50, 0.35f)
                .BindToPositionY(text.transform))
            .AppendInterval(0.4f)
            .Append(LMotion.Create(text.transform.localScale, Vector3.zero, 0.3f).BindToLocalScale(text.transform))
            .Join(LMotion.Create(1, 0, 0.31f).WithOnComplete(() => { Destroy(text.gameObject); })
                .Bind((x) => { text.alpha = x; }))
            .Run();
    }

    public void CreateTrailToStar(Vector3 worldPos)
    {
        // create trail
        
        
    }

    public RectTransform test;
    [Button]
    private void Tesrt()
    {
        Vector3 startWorldPos = test.transform.TransformPoint(test.anchoredPosition);
        Vector3 targetLocalPos = transform.InverseTransformPoint(startWorldPos);
        Vector3 worldGameObject = Camera.main.ScreenToWorldPoint(startWorldPos);
        Debug.Log(startWorldPos);
        Debug.Log(targetLocalPos);
        Debug.Log(worldGameObject);
    }
}