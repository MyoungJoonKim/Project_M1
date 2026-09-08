using UnityEngine;

public partial class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    [Header("Audio Source")]
    [SerializeField] private AudioSource bgmSource;
    [SerializeField] private AudioSource sfxSource;

    [Header("BGM Clips")]
    [SerializeField] private AudioClip TitleBgm;
    [SerializeField] private AudioClip LobbyBgm;
    [SerializeField] private AudioClip BattleBgm;

    [Header("UI SFX Clips")]
    [SerializeField] private AudioClip buttonClick;

    [Header("Battle SFX Clips")]
    [SerializeField] private AudioClip warning;
    [SerializeField] private AudioClip expPickup;
    [SerializeField] private AudioClip playerHit;
    [SerializeField] private AudioClip playerDead;
    [SerializeField] private AudioClip monsterDead;
    [SerializeField] private AudioClip pillarBroken;

    [Header("Player Skill SFX Clips")]
    [SerializeField] private AudioClip sunArea;
    [SerializeField] private AudioClip explosion;
    [SerializeField] private AudioClip crystalWave;
    [SerializeField] private AudioClip poisonCloud;
    [SerializeField] private AudioClip electricBall;
    [SerializeField] private AudioClip lightningStrike;

    [Header("Boss Skill SFX Clips")]
    [SerializeField] private AudioClip dustPuff;
    [SerializeField] private AudioClip slashBall;
    [SerializeField] private AudioClip energyExplosion;

    private float sfxInterval = 0.03f;
    private float lastSfxTime = -999f;


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);
    }

    
    public void PlaySceneBGM(SceneType sceneType)
    {
        switch (sceneType)
        {
            case SceneType.TITLE:
                PlayBGM(TitleBgm);
                break;
            case SceneType.LOBBY:
                PlayBGM(LobbyBgm);
                break;
            case SceneType.BATTLE:
                PlayBGM(BattleBgm);
                break;
            case SceneType.LOADING:
                bgmSource.Stop();
                break;
            case SceneType.END:
                break;
        }
    }

    public void PlayButtonClick()
    {
        PlaySFX(buttonClick);
    }
    public void PlayPlayerHit()
    {
        PlaySFX(playerHit);
    }
    public void PlayPlayerDead()
    {
        PlaySFX(playerDead);
    }
    public void PlayMonsterDead()
    {
        PlaySFX(monsterDead);
    }
    public void PlayPickup()
    {
        PlaySFX(expPickup);
    }
    public void PlayWarning()
    {
        PlaySFX(warning);
    }
    public void PlayPillarBroken()
    {
        PlaySFX(pillarBroken);
    }
    public void PlaySunAreaSkill()
    {
        PlaySFX(sunArea);
    }
    public void PlayElectricSkill()
    {
        PlaySFX(electricBall);
    }
    public void PlayExplosionSkill()
    {
        PlaySFX(explosion);
    }
    public void PlayCrystalWave()
    {
        PlaySFX(crystalWave);
    }
    public void PlayPoisonCloudSkill()
    {
        PlaySFX(poisonCloud);
    }
    public void PlayLightningStrikeSkill()
    {
        PlaySFX(lightningStrike);
    }
    public void PlayDustPuffSkill()
    {
        PlaySFX(dustPuff);
    }
    public void PlaySlashBallSkill()
    {
        PlaySFX(slashBall);
    }
    public void PlayEnergyExplosionSkill()
    {
        PlaySFX(energyExplosion);
    }

    private void PlayBGM(AudioClip bgm)
    {
        if (bgm == null)
            return;

        bgmSource.clip = bgm;
        bgmSource.loop = true;
        bgmSource.Play();
    }

    private void PlaySFX(AudioClip sfx)
    {
        if (sfx == null) 
            return;

        if (Time.unscaledTime < lastSfxTime + sfxInterval)
            return;

        lastSfxTime = Time.unscaledTime;

        sfxSource.PlayOneShot(sfx);
    }


}
