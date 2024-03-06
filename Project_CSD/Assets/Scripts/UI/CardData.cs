using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Card", menuName = "Scriptble Object/CardData")] // CreateAssetMenu : 커스텀 메뉴를 생성하는 속성
public class CardData : ScriptableObject
{
    // 카드 타입
    // 패시브 : 공격력 증가 + 10%, 공격력 증가 + 20%, 공격속도 증가 + 10%, 공격속도 증가 증가 + 20%, 캐릭터 이동속도 증가  + 20, 방어력 증가, 크리티컬 확률
    // 액티브 : 범위 공격, 대인강공격(피격 1명), 마법 공격, 유닛들 최대 체력 증가, 체력 회복
    public enum CardType { AttackDamage, AttackSpeed, UnitSpeed, Defense, Critical, rangeAttack, BossAttack, UnitMaxHealth, Heal } // enum 열거형 데이터는 정수 형태로도 사용 가능

    [Header("# Main Info")]
    public CardType cardType; // 카드 타입
    public int cardId; // 카드 id
    public string cardName; // 카드 이름
    [TextArea] // 인스펙터에 텍스트를 여러 줄 넣을 수 있게 해주는 속성
    public string itemDes; // 카드 설명
    public Sprite cardSprite; // 카드 아이콘


    [Header("# Passive Card Data")]
    public float upValue; // 증가 수치

    [Header("# Active Card Data")]
    public float damagePercent; // 데미지 수치
    public float healPercent; // 체력 수치

}