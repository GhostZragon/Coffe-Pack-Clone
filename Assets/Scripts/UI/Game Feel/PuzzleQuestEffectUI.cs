using LitMotion;
using LitMotion.Extensions;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

public class PuzzleQuestEffectUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI popupTextPrefab;

    private void Awake()
    {
        popupTextPrefab.gameObject.SetActive(false);
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
        CreateText(Vector3.zero);
    }

    public void CreateText(Vector3 worldPosition)
    {
        var screenPosition = Camera.main.WorldToScreenPoint(worldPosition);

        var text = Instantiate(popupTextPrefab, screenPosition, Quaternion.identity, transform);
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
}