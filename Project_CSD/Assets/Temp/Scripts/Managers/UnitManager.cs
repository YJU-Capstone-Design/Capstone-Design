using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Xml;
using UnityEngine;
using UnityEngine.UI;

public class UnitManager : MonoBehaviour
{
    [SerializeField] private UnitData unitData;

    Unit unit;
    private PoolManager pool;
    public Button unitSpawnRangeButton;

    public Button reRoll;

    private void Awake()
    {
        unit = GetComponent<Unit>();

        GameObject go = GameObject.Find("PoolManager");
        pool = go.GetComponent<PoolManager>();

        unitSpawnRangeButton = BattleManager.Instance.unitSpawnRange.GetComponentInChildren<Button>();

        reRoll = BattleManager.Instance.reRoll;
    }
    public void UsingCard()
    {
        if (!BattleManager.Instance.unitSpawnRange.activeSelf)
        {
            // �ش� ī�带 ������ ī���� ��ư ������Ʈ�� ��Ȱ��ȭ ó��
            foreach (GameObject card in BattleManager.Instance.cardObj)
            {
                if (card != this.gameObject)
                {
                    card.GetComponent<Button>().enabled = false;
                }
            }
            reRoll.enabled = false;

            GameObject spawnArea = BattleManager.Instance.unitSpawnRange.transform.GetChild(1).gameObject;
            RectTransform spawnAreaAnchors = spawnArea.GetComponent<RectTransform>();

            // ���� ī�޶��� ��ġ�� ���� ���� ���� ���� ���� ����
            if (BattleManager.Instance.mainCamera.position.x >= 3)
            {
                BattleManager.Instance.mainCamera.position = new Vector3(0, 0, -10);
                spawnAreaAnchors.anchorMin = new Vector2(0.15f, 0.43f);
                spawnAreaAnchors.anchorMax = new Vector2(1, 0.66f);
            }
            else
            {
                BattleManager.Instance.mainCamera.position = new Vector3(0, 0, -10);
                spawnAreaAnchors.anchorMin = new Vector2(0.15f, 0.43f);
                spawnAreaAnchors.anchorMax = new Vector2(1, 0.66f);
            }
            BattleManager.Instance.unitSpawnRange.SetActive(true);
            unitSpawnRangeButton.onClick.RemoveAllListeners();
            Debug.Log(unit.unitID);
            /*        unitSpawnRangeButton.onClick.AddListener(() => UnitSpawn(unit.unitID));*/
            unitSpawnRangeButton.onClick.AddListener(() => Buy(unit.cost));
        }
        else
        {
            // �ش� ī���� ����� ĵ���� ��� �ٸ� ī���� ��ư ������Ʈ Ȱ��ȭ
            foreach (GameObject card in BattleManager.Instance.cardObj)
            {
                if (card != this.gameObject)
                {
                    card.GetComponent<Button>().enabled = true;
                }
            }
            reRoll.enabled = true;
            BattleManager.Instance.unitSpawnRange.SetActive(false);
        }
    }

    public void UnitSpawn(int unitID)
    {
        // ���콺 ��Ŭ�� �� ���� ��ġ��
        BattleManager.Instance.point = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x,
            Input.mousePosition.y, -Camera.main.transform.position.z));

        switch (unitID)
        {
            case 11001: // Kitchu
                pool.Get(0, 0);
                break;
            case 11002: // Ramo
                pool.Get(0, 1);
                break;
            case 11003: // Pupnut
                pool.Get(0, 2);
                break;
            case 12001: // WhiteBread
                pool.Get(0, 3);
                break;
            case 12002: // BreadCrab
                pool.Get(0, 4);
                break;
            case 12003: // PanCake
                pool.Get(0, 7);
                break;
            case 11004: // Croirang
                pool.Get(0, 5);
                break;
            case 11005: // Eggball
                pool.Get(0, 6);
                break;
            case 11006: //Turtle
                pool.Get(0, 8);
                break;
            case 11007: // Froll
                pool.Get(0, 9);
                break;
        }

        foreach (GameObject card in BattleManager.Instance.cardObj)
        {
            card.GetComponent<Button>().enabled = true;
        }
        reRoll.enabled = true;

        BattleManager.Instance.unitSpawnRange.SetActive(false);
        BattleManager.Instance.CardShuffle(false);

        // usingCount ���̺� �ش� ID �� �÷��� count ���� +1, �ش� ID �÷��� ������ ���� �߰�
        XmlNodeList cardData = DBConnect.Select("usingCount", $"WHERE cardID = {unitID}");

        if (cardData != null)
        {
            DBConnect.UpdateOriginal($"UPDATE usingCount SET count = count + 1 WHERE cardID = {unitID}");
        }
        else
        {
            Debug.Log("�ԷµǾ��ִ� ī�尪�� �����ϴ�. �׷��� ���� �߰� �մϴ�.");
            // ���� �÷� �߰�
            DBConnect.Insert("usingCount", $"{unitID}, 1");
        }
    }


    public void Buy(int unitCost)
    {
        // uiMgr.cost�� unitData.Cost�� ���Ͽ� ���� �������� Ȯ��
        if (UiManager.Instance != null && UiManager.Instance.cost >= unitCost)
        {
            // �ڽ�Ʈ�� �����ϰ� ������ ����
            UiManager.Instance.cost -= unitCost;
            UnitSpawn(unit.unitID);
        }
        else
        {
            Debug.Log("���� ����! ���̰��̾�! ");
        }
    }
}