using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DeathCounter : MonoBehaviour
{
    private TextMeshProUGUI death_label;
    private int deaths = 0;

    // Start is called before the first frame update
    void Start()
    {
        death_label = GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void IncrementDeaths()
    {
        deaths++;
        death_label.text = deaths.ToString();
    }

}
