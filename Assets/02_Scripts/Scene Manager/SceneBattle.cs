using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneBattle : MonoBehaviour
{
    public void OnButtonExit()
    {
        Time.timeScale = 1f;
        
        int rewardExp = 0;

        if (Shared.battleManager != null)
            rewardExp = Shared.battleManager.GetRewardUserExp();

        if (Shared.userManager != null)
            Shared.userManager.AddUserExp(rewardExp);

        Shared.sceneLoadManager.ChangeScene(SceneType.LOBBY, false);
    }
}
