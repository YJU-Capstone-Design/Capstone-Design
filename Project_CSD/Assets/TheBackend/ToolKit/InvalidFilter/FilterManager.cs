using TheBackend.ToolKit.InvalidFilter;
using UnityEngine;

public class NewBehaviourScript : Singleton<NewBehaviourScript>
{
    private TheBackend.ToolKit.InvalidFilter.FilterManager _filterManager = new FilterManager();

    void Start()
    {
        if (_filterManager.LoadInvalidString() == false)
        {
            Debug.LogError("필터링 초기화에 실패했습니다");
        }
    }

    public bool CheckWord(string word)
    {
        if (_filterManager.IsFilteredString(word))
        {
            Debug.Log("사용할 수 없는 이름입니다.");
            return true;
        }
        else
        {
            Debug.Log("사용할 수 있는 이름입니다.");
            return false;
        }
    }
}