using TMPro;
using UnityEngine;

public class ScoreUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreText;

    private void OnEnable()
    {
        StaticEventHandler.OnScoreChanged += StaticEventHandler_OnScoreChanged;
    }

    private void OnDisable()
    {
        StaticEventHandler.OnScoreChanged -= StaticEventHandler_OnScoreChanged;
    }

    private void Start()
    {
        UpdateScoreText(0, 1);
    }

    private void StaticEventHandler_OnScoreChanged(ScoreChangedArgs scoreChangedArgs)
    {
        UpdateScoreText(scoreChangedArgs.score, scoreChangedArgs.multiplier);
    }

    private void UpdateScoreText(long score, int multiplier)
    {
        scoreText.text = $"SCORE: {score.ToString("###,###0")}\nMULTIPLIER: x{multiplier}";
    }
}