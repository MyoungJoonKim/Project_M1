using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectTest : MonoBehaviour
{
    public Transform player;      // 플레이어 위치
    public Transform monster;     // 타겟 몬스터 위치

    void Update()
    {
        if (player == null || monster == null) return;

        // 1. 플레이어 위치로 이펙트 이동
        transform.position = player.position;

        // 2. 몬스터와의 방향 벡터 계산
        Vector3 direction = monster.position - player.position;

        // 3. 2D 각도 계산 (Atan2 사용)
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // 4. 이펙트 회전 (Z축 기준 회전)
        // 만약 이펙트가 기본적으로 '오른쪽'을 향해 뿜어지는 구조라면 아래 코드가 맞습니다.
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }
}
