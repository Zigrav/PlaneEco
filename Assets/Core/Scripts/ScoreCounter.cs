using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;

public class ScoreCounter : MonoBehaviour
{
    public UnityEvent victory;

    private TextMeshProUGUI score_label;
    private int score = 0;

    // Start is called before the first frame update
    void Start()
    {
        score_label = GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void IncrementScore()
    {
        score++;
        score_label.text = score.ToString();

        if(score >= 9)
        {
            victory.Invoke();
            score = 0;
            score_label.text = score.ToString();
        }
    }

    public void ResetScore()
    {
        score = 0;
        score_label.text = score.ToString();
    }
}
