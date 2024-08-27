using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class IconShop : MonoBehaviour
{
    public Image[] icon;
    public Image selectIcon;
    public TextMeshProUGUI gold;
    public GameObject popup;
    public Image playerIcon;
    public Image lobbyIcon;
    private void Start()
    {
        ClearFilter();
    }

    public void ClearFilter()
    {
        if (PlayerData.instance != null)
        {
            for (int i = 0; i < icon.Length; i++)
            {
                Color filter = icon[i].GetComponent<Image>().color;
                filter.a = 0.5f; // 기본적으로 투명도를 0.5로 설정
                icon[i].GetComponent<Image>().color = filter;

                if (PlayerData.instance.icon != null && icon[i].sprite == PlayerData.instance.icon)
                {
                    filter.a = 1f;
                    icon[i].GetComponent<Image>().color = filter;
                }

                foreach (Sprite addedIcon in PlayerData.instance.addicon)
                {
                    if (icon[i].sprite == addedIcon)
                    {
                        filter.a = 1f;
                        icon[i].GetComponent<Image>().color = filter;
                    }
                }
            }
        }
    }

    public void SelectIcon(Image img)
    {
        if (AudioManager.instance != null) { AudioManager.instance.ButtonSound(); }
        selectIcon.sprite = img.sprite;
        gold.text = "300 Gold";
    }

    public void IconBuy()
    {
        if (AudioManager.instance != null) { AudioManager.instance.ButtonSound(); }
        if (CashManager.instance != null)
        {
            if (CashManager.instance.player_Gold >= 300 && selectIcon != null)
            {
                CashManager.instance.player_Gold -= 300;

                for (int i = 0; i < icon.Length; i++)
                {
                    if (icon[i].sprite == selectIcon.sprite)
                    {
                        Color color = icon[i].GetComponent<Image>().color;
                        color.a = 1f;
                        icon[i].GetComponent<Image>().color = color;

                        PlayerData.instance.AddIcon(icon[i].GetComponent<Image>().sprite);
                        Debug.Log(icon[i].name + " 구매");
                    }
                }
            }
        }
    }

    public void IconChange()
    {
        if (AudioManager.instance != null) { AudioManager.instance.ButtonSound(); }
        if (PlayerData.instance != null && selectIcon.sprite != null)
        {
            if (PlayerData.instance.icon != null && PlayerData.instance.icon.name == selectIcon.sprite.name)
            {
                playerIcon.sprite = selectIcon.sprite;
            }
            else
            {
                foreach (Sprite addedIcon in PlayerData.instance.addicon)
                {
                    if (selectIcon.sprite.name == addedIcon.name)
                    {
                        playerIcon.sprite = selectIcon.sprite;
                        PlayerData.instance.icon = selectIcon.sprite; // 선택된 아이콘을 현재 아이콘으로 설정
                        lobbyIcon.sprite = selectIcon.sprite;
                        break;
                    }
                }
            }
        }
        Cancel();
    }

    public void OpenShop()
    {
        if (AudioManager.instance != null) { AudioManager.instance.ButtonSound(); }
        popup.SetActive(true);
    }

    public void Cancel()
    {
        if (AudioManager.instance != null) { AudioManager.instance.ButtonSound(); }
        popup.SetActive(false);
    }

   
}
