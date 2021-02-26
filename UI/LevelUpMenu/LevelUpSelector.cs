using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class LevelUpSelector : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private float timeToDisplayStats = 0.1f;
    [SerializeField] private int skillNum = 0;

    private LevelUpMenu levelUpMenu;
    private GameObject hoverSelectors;
    private bool cursor_over = false;
    private bool stats_enabled = false;
    private float cursor_over_time = 0f;

    // Start is called before the first frame update
    void Start()
    {
        levelUpMenu = GetComponentInParent<LevelUpMenu>();
        hoverSelectors = transform.Find("HoverSelectors").gameObject;
        hoverSelectors.SetActive(false);
    }

    void Update()
    {
        if (cursor_over)
        {
            cursor_over_time += Time.unscaledDeltaTime;
        }

        if (cursor_over_time >= timeToDisplayStats)
        {
            if (!stats_enabled)
            {
                stats_enabled = true;
                levelUpMenu.EnableStatPanel(skillNum);
            }
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        cursor_over = true;
        hoverSelectors.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        cursor_over = false;
        stats_enabled = false;
        levelUpMenu.DisableStatPanel();
        cursor_over_time = 0;
        hoverSelectors.SetActive(false);
    }

    public int ReturnSkillNum()
    {
        return skillNum;
    }
}
