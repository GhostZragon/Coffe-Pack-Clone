using LitMotion;
using Sirenix.OdinInspector;
using UnityEngine;

public class QuestCollectEffect : MonoBehaviour
{
    [SerializeField] private GameObject trailPrefab;
    [SerializeField] private QuestStageUI questStageUI;

    private void Awake()
    {
        trailPrefab.gameObject.SetActive(false);
    }
    [Button]
    public void CreateTrailToStar(Vector3 worldPos)
    {
        // create trail

        var currentStage = questStageUI.CurrentStage;
        var startWorldPos = questStageUI.levelStarUI.GetStarPositionByIndex(currentStage);

        var effect = Instantiate(trailPrefab, worldPos, Quaternion.identity, transform);
        effect.gameObject.SetActive(true);

        var custom = new BezierCustom();
        custom.Setup(effect.transform.position, startWorldPos);

        LMotion.Create(0f, 1f, 1)
            .WithOnComplete(() =>
            {
                questStageUI.levelStarUI.CollectPointEffect(currentStage);
                Destroy(effect.gameObject);
            })
            .Bind((x) =>
            {
                custom.Play3(x);
                effect.transform.position = custom.moveObjectPosition;
            });
    }
}