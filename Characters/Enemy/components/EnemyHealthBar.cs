using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthBar : MonoBehaviour
{
    [SerializeField] GameObject PlateIcon = null;

    private bool HealthBarEnabled;

    private Transform HealthBarTrans;
    private Image HealthBar;
    private Image HealthBarGhost;

    private RectTransform BackgroundBar;

    private bool alreadySetupOnce = false;
    private float original_background_height;

    public void SetupSpecial(int Plating, int Armor)
    {
        if (!alreadySetupOnce)
        {
            HealthBarTrans = transform.Find("HealthBar");
            HealthBar = HealthBarTrans.GetComponent<Image>();
            HealthBarGhost = transform.Find("HealthBarGhost").GetComponent<Image>();
            BackgroundBar = transform.Find("Background").GetComponent<RectTransform>();
            alreadySetupOnce = true;

            original_background_height = BackgroundBar.sizeDelta.y; 
        }

        for (int i = 0; i < Plating; ++i)
        {
            Instantiate(PlateIcon, HealthBarTrans);
        }

        BackgroundBar.sizeDelta = new Vector2(BackgroundBar.sizeDelta.x, BackgroundBar.sizeDelta.y * (1 + (float)Armor / 100));
    }

    public void Enable()
    {
        gameObject.SetActive(true);
        HealthBarEnabled = true;
    }

    public void Disable()
    {
        gameObject.SetActive(false);
    }

    private void Update()
    {
        if (HealthBarEnabled)
        {
            rotate();
            UpdateGhost();
        }
        else
        {
            Disable();
        }
    }

    // Update is called once per frame
    private void rotate()
    {
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, 45f, transform.eulerAngles.z);
    }

    private void UpdateGhost()
    {
        if (HealthBarGhost.fillAmount > HealthBar.fillAmount)
        {
            HealthBarGhost.fillAmount -= .2f * Time.deltaTime;
        }
        else
        {
            HealthBarGhost.fillAmount = HealthBar.fillAmount;
        }
    }
}
