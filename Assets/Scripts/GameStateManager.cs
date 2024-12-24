using UnityEngine;
using UnityEngine.SceneManagement;

public class GameStateManager : MonoBehaviour
{
    public Health playerHealth;
    public Health bossHealth;

    public string onBossDieScene;

    private void Start()
    {
        playerHealth.OnHealthOut += OnPlayerDie;
        bossHealth.OnHealthOut += OnBossDie;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
            OnPlayerDie(0);
    }

    private void OnDisable()
    {
        playerHealth.OnHealthOut -= OnPlayerDie;
        bossHealth.OnHealthOut -= OnBossDie;
    }

    void OnPlayerDie(int i)
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    void OnBossDie(int i)
    {
        SceneManager.LoadScene(onBossDieScene);
    }
}
