/*using System.Collections;
using System.Collections.Generic;
using System.Xml;
using TMPro;
using UnityEngine;

public class UserLogin : MonoBehaviour
{
    [SerializeField] GameObject loginBox;//�α��� â
    [SerializeField] TextMeshProUGUI player_Name;
    [SerializeField] TextMeshProUGUI player_No;

    public void SaveUserData()
    {
        if (!string.IsNullOrEmpty(player_Name.text) && !string.IsNullOrEmpty(player_No.text))
        {
            Debug.Log(player_Name.text + "    " + player_Name.text);
            // �����ͺ��̽� �Է�
            XmlNodeList selectedData = DBConnect.Select("ranking", $"WHERE id = {int.Parse(player_No.text)}");

            // player_No ���� int ������ Ȯ�� ��, ������ �Է�
            if (int.TryParse(player_No.text, out int playerId))
            {
                if (selectedData == null)
                {
                    DBConnect.Insert("ranking", int.Parse(player_No.text), player_Name.text, (int)endTime);
                }
                else
                {
                    DBConnect.Update("ranking", "name", "time", player_Name.text, (int)endTime, $"id = {int.Parse(player_No.text)}");
                }

                // �κ� ���� ���� �߰� �ʿ�
            }
            else
            {
                Debug.Log("�й��� �߸��Ǿ����ϴ�.");
            }
        }
        else
        {
            Debug.Log("�Է°��� ����ֽ��ϴ�.");
        }
    }

}
*/