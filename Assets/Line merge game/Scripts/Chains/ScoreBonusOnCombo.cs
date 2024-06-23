using UnityEngine;

public class ScoreBonusOnCombo : MonoBehaviour, IChainAction
{
    [Header("Chain Action Data")]
    [SerializeField] ScoreOnComboSO scoreOnComboSO;
    [SerializeField] ScoreBootsPopup scoreBoostPrefab;
    [SerializeField] Transform scoreBoostPopupParent;

    ChainManager chainManager;

    private void Start()
    {
        chainManager = GetComponent<ChainManager>(); // this will force all chain logic to sit on chain manager object... FLAG

        if (chainManager)
            chainManager.AddObserver(this);
    }

    private void OnDestroy()
    {
        if (chainManager)
            chainManager.RemoveObserver(this);
    }


    public void NotifyObserver(BallBase ball, int currentComboReached)
    {
        if (currentComboReached >= scoreOnComboSO.scoreComboDataArray.Length)
            currentComboReached = scoreOnComboSO.scoreComboDataArray.Length - 1;

        int scoreToAdd = scoreOnComboSO.scoreComboDataArray[currentComboReached].scoreBonus;

        if (scoreToAdd > 0)
        {
            ScoreManager.instance.AddScore(scoreToAdd);

            ScoreBootsPopup popup = Instantiate(scoreBoostPrefab, scoreBoostPopupParent);
            popup.InitPopup(scoreToAdd);
        }

        //spawn score display somewhere on screen
    }

    public void NotifyObserverReset()
    {
        // some effect on reset?
    }
}
