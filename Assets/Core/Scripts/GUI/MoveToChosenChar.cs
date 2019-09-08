using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoveToChosenChar : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> char_ui_elems = null;

    [SerializeField]
    private SODict curr_char = null;

    [SerializeField]
    private Image image = null;

    private void Start()
    {
        Move();
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    [ContextMenu("Move")]
    public void Move()
    {
        for (int i = 0; i < char_ui_elems.Count; i++)
        {
            CharInfoHolder char_info_holder = char_ui_elems[i].GetComponent<CharInfoHolder>();

            if(char_info_holder == null)
            {
                throw new System.Exception(char_ui_elems[i].name + " UI element does not have CharInfoHolder component defined");
            }

            SODict curr_char_info = GetSingleElem.Get(curr_char) as SODict;

            if (curr_char_info == char_info_holder.char_info)
            {
                // Enable / Show the Image
                image.enabled = true;

                // This RectTransform
                RectTransform curr_rect_transform = GetComponent<RectTransform>();

                // Move to chosen char RectTransform
                curr_rect_transform.position = char_info_holder.gameObject.GetComponent<RectTransform>().position;
            }
        }
    }
}
