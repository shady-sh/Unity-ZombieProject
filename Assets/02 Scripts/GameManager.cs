using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    static GameManager m_instance;

    public static GameManager instance {
        get {
            if (m_instance == null) {
                m_instance = FindObjectOfType<GameManager>();
            }
            return m_instance;
        }
    }
    int score = 0;

    public bool isGameover { get; private set; }

    private void Awake()
    {
        if (instance != this) {
            Destroy(gameObject);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        FindObjectOfType<PlayerHeath>().onDeath += EndGame;
    }

    public void AddScore(int newScore) {
        if (!isGameover) {
            score += newScore;
            UIManager.instance.UpdateScoreText(score);
        }
    }

    public void EndGame() {
        isGameover = true;
        UIManager.instance.SetActiveGameoverUI(true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
