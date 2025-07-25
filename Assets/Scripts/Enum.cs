using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SCENE
{
    TITLE,
    LOBBY,
    BATTLE,
    LOADING,
    END
}
public enum MENU
{
    OPTION
}

public enum AI
{
    AI_CREATE,
    AI_SEARCH,
    AI_MOVE,
    AI_RESET
}

public enum AnimationLayer
{
    MoveLayer = 0,
    AttackLayer = 1
}

public enum StatType
{
    Hp, Mp, Atk, Matk, Def, Mdef, Level, Exp
}

public enum BaseStatType
{
    Str, Dex, Int, Luk, StatPoint
}

public enum MaxStatType
{
    MaxHp, MaxMp, MaxExp
}
