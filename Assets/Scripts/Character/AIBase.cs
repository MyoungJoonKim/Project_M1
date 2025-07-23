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

//// ���� 1. �̵��ε��� ����ó��, 2. �̵��� ��ǥ���� �ٶ󺸱�(ȸ��), 3. �̵��ӵ� ����
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

//public void State() //fsm( ���� ����
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
//    AI = AI.AI_SEARCH; // ����ȿ��
//}
//protected virtual void Search()
//{
//    if (Monster == null)
//    {
//        Monster = GetComponent<Monster>();
//    }
//    float dis = Vector3.Distance(Monster.transform.position, TRPATH[Index].position); // ��ã��
//    if (dis < 1f)
//    {
//        if (TRPATH.Length - 1 > Index)
//        {
//            Index++;
//        }
//        else Index = 0;
//    }

//    AI = AI.AI_MOVE; // ��ã��,��ã��,��Ȳ�ϱ�
//}
//protected virtual void Move()
//{
//    if (monsterController == null)
//    {
//        monsterController = GetComponent<MonsterController>();
//    }
//    monsterController.Move(TRPATH[Index].position);
//    AI = AI.AI_SEARCH; // ��ǥ���� �̵�, �����ϸ� ����
//}
//protected virtual void Reset()
//{
//    AI = AI.AI_CREATE;
//}
