using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    static UIManager m_instance;
    AudioSource audiosrc;
    public AudioClip winClip, loseClip;
    public static UIManager instance {
        get {
            if (m_instance == null) {
                m_instance = FindObjectOfType<UIManager>();
            }
            return m_instance;
        }
    }
    public Text ammoText, scoreText, waveText;
    public GameObject gameoverUI,  WinUI, LoseUI;

    public void UpdateAmmoText(int magAmmo, int remainAmmo) {
        ammoText.text = magAmmo + "/" + remainAmmo;
    }

    public void UpdateScoreText(int newScore) {
        scoreText.text = "Score : " + newScore;
    }

    public void UpdateWaveText(int waves, int count) {
        waveText.text = "Wave L " + waves + "\nEnemy Left : " + count;
    }

    public void SetActiveGameoverUI(bool active) {
        gameoverUI.SetActive(active);
        if (Enemy.currentKill >= 6) {
            WinUI.SetActive(active);
            audiosrc.PlayOneShot(winClip);
        } else {
            LoseUI.SetActive(active);
            audiosrc.PlayOneShot(loseClip);
        }
    }

    public void GameRestart() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        Enemy.currentKill = 0;
    }
    // Start is called before the first frame update
    void Start()
    {
        audiosrc = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
