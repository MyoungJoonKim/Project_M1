using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIBase : MonoBehaviour 
{
    protected AI AI = AI.AI_CREATE;

    protected Character Character;

    public void Init(Character character)
    {
        Character = character;
    }

    public void State() //fsm( ���� ����
    {
        switch (AI)
        {
            case AI.AI_CREATE: 
                Create();
                break;
            case AI.AI_SEARCH:
                Search();
                break;
            case AI.AI_MOVE:
                Move();
                break;
            case AI.AI_RESET:
                Reset();
                break;
        }
    }

    protected virtual void Create()
    {
        AI = AI.AI_SEARCH; // ����ȿ��
    }
    protected virtual void Search()
    {
        AI = AI.AI_MOVE; // ��ã��,��ã��,��Ȳ�ϱ�
    }
    protected virtual void Move()
    {
        AI = AI.AI_SEARCH; // ��ǥ���� �̵�, �����ϸ� ����
    }
    protected virtual void Reset()
    {
        AI = AI.AI_CREATE;
    }
}
