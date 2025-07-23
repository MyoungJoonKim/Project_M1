using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading;
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

//// 문제 1. 이동인덱스 예외처리, 2. 이동시 목표지점 바라보기(회전), 3. 이동속도 조절
//    public Transform[] TRPATH;
//    public MonsterController monsterController;

//    protected AI AI = AI.AI_CREATE;

//    protected Character Character;
//    protected Monster Monster;

//    int Index = 0;

//public void Init(Character character)
//{
//    Character = character;
//}

//public void State() //fsm( 상태 패턴
//{
//    switch (AI)
//    {
//        case AI.AI_CREATE:
//            Create();
//            break;
//        case AI.AI_SEARCH:
//            Search();
//            break;
//        case AI.AI_MOVE:
//            Move();
//            break;
//        case AI.AI_RESET:
//            Reset();
//            break;
//    }
//}

//protected virtual void Create()
//{
//    AI = AI.AI_SEARCH; // 연출효과
//}
//protected virtual void Search()
//{
//    if (Monster == null)
//    {
//        Monster = GetComponent<Monster>();
//    }
//    float dis = Vector3.Distance(Monster.transform.position, TRPATH[Index].position); // 길찾기
//    if (dis < 1f)
//    {
//        if (TRPATH.Length - 1 > Index)
//        {
//            Index++;
//        }
//        else Index = 0;
//    }

//    AI = AI.AI_MOVE; // 길찾기,적찾기,방황하기
//}
//protected virtual void Move()
//{
//    if (monsterController == null)
//    {
//        monsterController = GetComponent<MonsterController>();
//    }
//    monsterController.Move(TRPATH[Index].position);
//    AI = AI.AI_SEARCH; // 목표지점 이동, 도착하면 공격
//}
//protected virtual void Reset()
//{
//    AI = AI.AI_CREATE;
//}
