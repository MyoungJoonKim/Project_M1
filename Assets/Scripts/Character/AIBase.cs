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

    public void State() //fsm( 상태 패턴
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
        AI = AI.AI_SEARCH; // 연출효과
    }
    protected virtual void Search()
    {
        AI = AI.AI_MOVE; // 길찾기,적찾기,방황하기
    }
    protected virtual void Move()
    {
        AI = AI.AI_SEARCH; // 목표지점 이동, 도착하면 공격
    }
    protected virtual void Reset()
    {
        AI = AI.AI_CREATE;
    }
}
