using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public enum GameMode {
    idle,
    playing,
    levelEnd
}

public class MissionDemolition : MonoBehaviour
{
    static public MissionDemolition S; 

    [Header("Inscribed in Inspector")]
    public Text uitLevel; // The UIText_Level Text
    public Text uitShots; // The UIText_Shots Text

    public int maxShots = 10;
    public TextMeshProUGUI youDiedText;
    public TextMeshProUGUI youWinText;
    public TextMeshProUGUI menuBtnTxt;
    public Image menuBtnImg;
    public Image fadeImage;
    public TextMeshProUGUI shotsRemainingText;

    public Vector3 castlePos; // The place to put castles
    public GameObject[] castles; // An array of the castles

    [Header("Dynamic")]
    public int level; // The current level
    public int levelMax;
    public int shotsTaken;
    public GameObject castle; // The current castle
    public GameMode mode = GameMode.idle;
    public string showing = "Show Slingshot"; // FollowCam mode
    public bool isStart = false;
    

    void Awake()
    {
        youDiedText.gameObject.SetActive(false);
        youWinText.gameObject.SetActive(false);
        shotsRemainingText.gameObject.SetActive(false);
    }

    void Start()
    {
        S = this;

        level = 0;
        shotsTaken = 0;
        levelMax = castles.Length;
        StartLevel();
    }

    public void StartLevel()
    {
        if (castle != null) {
            Destroy(castle);
        }
        Time.timeScale = 1f;
        
        Projectile.DESTROY_PROJECTILES();

        castle = Instantiate<GameObject>(castles[level]);
        castle.transform.position = castlePos;

        Goal.goalMet = false;
                
        UpdateGUI();

        mode = GameMode.playing;

        FollowCam.SWITCH_VIEW(FollowCam.eView.both);
        isStart = true;
    }

    void UpdateGUI()
    {
        uitLevel.text = "Level: " + (level+1) + " of " + levelMax;
        uitShots.text = "Shots Taken: " + shotsTaken + " / " + maxShots;
    }

    void Update()
    {
        UpdateGUI();

        if ( (mode == GameMode.playing) && Goal.goalMet )
        {
            mode = GameMode.levelEnd;

            FollowCam.SWITCH_VIEW(FollowCam.eView.both);

            Invoke("NextLevel", 2f);
        }        

        if ( (mode == GameMode.playing) && shotsTaken >= maxShots )
        {
            mode = GameMode.levelEnd;

            FollowCam.SWITCH_VIEW(FollowCam.eView.both);

            StartCoroutine(FadeToBlack(false));
        }

        if ( (mode == GameMode.playing) && shotsTaken == 0 )
        {
            shotsRemainingText.text = "Shots Remaining: " + (maxShots);
            shotsRemainingText.gameObject.SetActive(true);
        }
        else if (isStart)
        {
            StartCoroutine(ShotsRemainingFade());
        }

    }

IEnumerator ShotsRemainingFade()
{
    float fadeDuration = 1.5f; 
    float time = 0f;

    Color shotsRemainingTextColor = shotsRemainingText.color;
    shotsRemainingTextColor.a = 1;
    shotsRemainingText.color = shotsRemainingTextColor;

    while (time < fadeDuration)
    {
        float alpha = 1 - (time / fadeDuration);
        shotsRemainingTextColor.a = alpha;
        shotsRemainingText.color = shotsRemainingTextColor;

        time += Time.deltaTime;
        yield return null;
    }

    shotsRemainingTextColor.a = 0; 
    shotsRemainingText.color = shotsRemainingTextColor;
    shotsRemainingText.gameObject.SetActive(false);
}


IEnumerator FadeToBlack(bool isWin)
{
    float fadeDuration = 3f; 
    float textFadeDelay = 1f; 
    float time = 0f;
    TextMeshProUGUI selectedText;

    if (isWin)
    {
        selectedText = youWinText;
    }
    else
    {
        selectedText = youDiedText;
    }

    Color textColor = selectedText.color;
    textColor.a = 0;
    selectedText.color = textColor;
   
    Color menuBtnTxtColor = menuBtnTxt.color;
    menuBtnTxtColor.a = 0;
    menuBtnTxt.color = menuBtnTxtColor;
   
    Color menuBtnImgColor = menuBtnImg.color;
    menuBtnImgColor.a = 0;
    menuBtnImg.color = menuBtnImgColor;

    selectedText.gameObject.SetActive(true);



    while (time <= fadeDuration)
    {
        float alpha = time / fadeDuration;
        fadeImage.color = new Color(0, 0, 0, alpha); 

        if (time > textFadeDelay)
        {
            float textAlpha = (time - textFadeDelay) / (fadeDuration - textFadeDelay);
            textColor.a = textAlpha;
            selectedText.color = textColor; 

            menuBtnTxtColor.a = textAlpha;
            menuBtnTxt.color = menuBtnTxtColor;

            menuBtnImgColor.a = textAlpha;
            menuBtnImg.color = menuBtnImgColor;
        }

        time += Time.deltaTime;
        yield return null;
    }

    fadeImage.color = new Color(0, 0, 0, 1); 
    textColor.a = 1; 
    selectedText.color = textColor;
    menuBtnTxtColor.a = 1;
    menuBtnTxt.color = menuBtnTxtColor;
    menuBtnImgColor.a = 1;
    menuBtnImg.color = menuBtnImgColor;
}



    void NextLevel()
    {
        level++;
        shotsTaken = 0;
        if (level == levelMax)
        {
            StartCoroutine(FadeToBlack(true));
        }
        StartLevel();
    }

    static public void SHOT_FIRED()
    {
        S.shotsTaken++;
    }

    static public GameObject GET_CASTLE()
    {
        return S.castle;
    }
}
