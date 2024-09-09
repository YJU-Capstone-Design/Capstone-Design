/*using System.Collections;
using System.Collections.Generic;
using System.Xml;
using TMPro;
using UnityEngine;

public class UserLogin : MonoBehaviour
{
    [SerializeField] GameObject loginBox;//로그인 창
    [SerializeField] TextMeshProUGUI player_Name;
    [SerializeField] TextMeshProUGUI player_No;

    public void SaveUserData()
    {
        if (!string.IsNullOrEmpty(player_Name.text) && !string.IsNullOrEmpty(player_No.text))
        {
            Debug.Log(player_Name.text + "    " + player_Name.text);
            // 데이터베이스 입력
            XmlNodeList selectedData = DBConnect.Select("ranking", $"WHERE id = {int.Parse(player_No.text)}");

            // player_No 값이 int 형인지 확인 후, 데이터 입력
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

                // 로비 복귀 로직 추가 필요
            }
            else
            {
                Debug.Log("학번이 잘못되었습니다.");
            }
        }
        else
        {
            Debug.Log("입력값이 비어있습니다.");
        }
    }

}
*/