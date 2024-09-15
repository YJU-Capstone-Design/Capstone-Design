using TheBackend.ToolKit.InvalidFilter;
using UnityEngine;

public class NewBehaviourScript : Singleton<NewBehaviourScript>
{
    private TheBackend.ToolKit.InvalidFilter.FilterManager _filterManager = new FilterManager();

    void Start()
    {
        if (_filterManager.LoadInvalidString() == false)
        {
            Debug.LogError("���͸� �ʱ�ȭ�� �����߽��ϴ�");
        }
    }

    public bool CheckWord(string word)
    {
        if (_filterManager.IsFilteredString(word))
        {
            Debug.Log("����� �� ���� �̸��Դϴ�.");
            return true;
        }
        else
        {
            Debug.Log("����� �� �ִ� �̸��Դϴ�.");
            return false;
        }
    }
}