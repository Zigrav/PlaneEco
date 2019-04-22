using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ScoreCounter : MonoBehaviour
{
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
    }
}
