using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WinCounter : MonoBehaviour
{
    private TextMeshProUGUI win_label;
    private int wins = 0;

    // Start is called before the first frame update
    void Start()
    {
        win_label = GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void IncrementWins()
    {
        wins++;
        win_label.text = wins.ToString();
    }
}
