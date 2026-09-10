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

    private float sfxInterval = 0.045f;
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
        PlaySFX(buttonClick, sfxInterval);
    }
    public void PlayPlayerHit()
    {
        PlaySFX(playerHit, sfxInterval);
    }
    public void PlayPlayerDead()
    {
        PlaySFX(playerDead, sfxInterval);
    }
    public void PlayMonsterDead()
    {
        PlaySFX(monsterDead, sfxInterval);
    }
    public void PlayPickup()
    {
        PlaySFX(expPickup, sfxInterval);
    }
    public void PlayWarning()
    {
        PlaySFX(warning, sfxInterval);
    }
    public void PlayPillarBroken()
    {
        PlaySFX(pillarBroken, sfxInterval);
    }
    public void PlaySunAreaSkill(float interval)
    {
        PlaySFX(sunArea, interval);
    }
    public void PlayElectricSkill(float interval)
    {
        PlaySFX(electricBall, interval);
    }
    public void PlayExplosionSkill()
    {
        PlaySFX(explosion, sfxInterval);
    }
    public void PlayCrystalWave()
    {
        PlaySFX(crystalWave, sfxInterval);
    }
    public void PlayPoisonCloudSkill(float interval)
    {
        PlaySFX(poisonCloud, interval);
    }
    public void PlayLightningStrikeSkill()
    {
        PlaySFX(lightningStrike, sfxInterval);
    }
    public void PlayDustPuffSkill()
    {
        PlaySFX(dustPuff, sfxInterval);
    }
    public void PlaySlashBallSkill()
    {
        PlaySFX(slashBall, sfxInterval);
    }
    public void PlayEnergyExplosionSkill()
    {
        PlaySFX(energyExplosion, sfxInterval);
    }

    private void PlayBGM(AudioClip bgm)
    {
        if (bgm == null)
            return;

        bgmSource.clip = bgm;
        bgmSource.loop = true;
        bgmSource.Play();
    }

    private void PlaySFX(AudioClip sfx, float interval)
    {
        if (sfx == null) 
            return;

        if (Time.unscaledTime < lastSfxTime + interval)
            return;

        lastSfxTime = Time.unscaledTime;

        sfxSource.PlayOneShot(sfx);
    }


}
