using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MiscDisplay : MonoBehaviour
{
    private TextMeshProUGUI mainText;
    private TextMeshProUGUI secondaryText;

    private float counter; 
    void Start()
    {
        mainText = transform.Find("MainText").GetComponent<TextMeshProUGUI>();
        secondaryText = transform.Find("SecondaryText").GetComponent<TextMeshProUGUI>();
    }

    public void enableDisplay(string mtext, string stext)
    {
        mainText.text = mtext;
        secondaryText.text = stext;
        gameObject.SetActive(true);
        counter = .1f;
    }

    // Update is called once per frame
    void Update()
    {
        if(counter > 0)
        {
            counter -= Time.deltaTime;
        }
        else
        {
            gameObject.SetActive(false);
        }
    }
}
