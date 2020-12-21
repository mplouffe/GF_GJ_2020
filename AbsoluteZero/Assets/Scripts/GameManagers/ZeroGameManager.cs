using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public enum GameState
{
    Menu,
    Loading,
    Playing,
    Dead,
    Over
}

public class ZeroGameManager : MonoBehaviour
{
    public static ZeroGameManager Instance;

    public bool IsGameOver;

    [SerializeField] private Image screenFader;
    [SerializeField] private float fadeDelay = 2f;
    [SerializeField] private float gameEndDelay = 5f;

    private GameState gameState;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }

        DontDestroyOnLoad(gameObject);
    }

    public void GameOver()
    {
        gameState = GameState.Loading;
        StartCoroutine(LoadGameOverRoutine());
    }

    // Start is called before the first frame update
    void Start()
    {
        gameState = GameState.Loading;
        StartCoroutine(FirstLoadRoutine());
    }

    private IEnumerator FirstLoadRoutine()
    {
        screenFader?.CrossFadeAlpha(0f, fadeDelay, true);
        yield return new WaitForSeconds(fadeDelay);
        gameState = GameState.Menu;
    }

    private IEnumerator LoadMainMenuRoutine()
    {
        gameState = GameState.Loading;
        screenFader?.CrossFadeAlpha(1f, fadeDelay, true);
        yield return new WaitForSeconds(fadeDelay);
        SceneManager.LoadScene(0);
        screenFader?.CrossFadeAlpha(0f, fadeDelay, true);
        yield return new WaitForSeconds(fadeDelay);
        gameState = GameState.Menu;
    }


    private IEnumerator LoadMainGameRoutine()
    {
        gameState = GameState.Loading;
        screenFader?.CrossFadeAlpha(1f, fadeDelay, true);
        yield return new WaitForSeconds(fadeDelay);
        DeleteAllEntities();
        SceneManager.LoadScene(1);
        IsGameOver = false;
        screenFader?.CrossFadeAlpha(0f, fadeDelay, true);
        yield return new WaitForSeconds(fadeDelay);
        gameState = GameState.Playing;
    }

    private IEnumerator LoadGameOverRoutine()
    {
        gameState = GameState.Loading;
        screenFader?.CrossFadeAlpha(1f, fadeDelay, true);
        yield return new WaitForSeconds(fadeDelay);
        DeleteAllEntities();
        SceneManager.LoadScene(2);
        screenFader?.CrossFadeAlpha(0f, fadeDelay, true);
        yield return new WaitForSeconds(fadeDelay);
        gameState = GameState.Over;
        yield return new WaitForSeconds(gameEndDelay);
        gameState = GameState.Loading;
        screenFader?.CrossFadeAlpha(1f, fadeDelay, true);
        yield return new WaitForSeconds(fadeDelay);
        SceneManager.LoadScene(0);
        screenFader?.CrossFadeAlpha(0f, fadeDelay, true);
        yield return new WaitForSeconds(fadeDelay);
        gameState = GameState.Menu;
    }

    private IEnumerator LoadDeadRoutine()
    {
        gameState = GameState.Loading;
        screenFader?.CrossFadeAlpha(1f, fadeDelay, true);
        yield return new WaitForSeconds(fadeDelay);
        DeleteAllEntities();
        SceneManager.LoadScene(2);
        screenFader?.CrossFadeAlpha(0f, fadeDelay, true);
        yield return new WaitForSeconds(fadeDelay);
        gameState = GameState.Dead;
        yield return new WaitForSeconds(gameEndDelay);
        gameState = GameState.Loading;
        screenFader?.CrossFadeAlpha(1f, fadeDelay, true);
        yield return new WaitForSeconds(fadeDelay);
        SceneManager.LoadScene(0);
        screenFader?.CrossFadeAlpha(0f, fadeDelay, true);
        yield return new WaitForSeconds(fadeDelay);
        gameState = GameState.Menu;
    }

    private void DeleteAllEntities()
    {
        var entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
        entityManager.DestroyEntity(entityManager.UniversalQuery);
    }

    // Update is called once per frame
    void Update()
    {
        if (gameState == GameState.Loading)
        {
            return;
        }

        var keyboard = Keyboard.current;
        if (keyboard != null)
        {
            if (keyboard.spaceKey.wasPressedThisFrame)
            {
                switch (gameState)
                {
                    case GameState.Menu:
                        StartCoroutine(LoadMainGameRoutine());
                        break;
                }
            }

            if (keyboard.escapeKey.wasPressedThisFrame)
            {
                switch (gameState)
                {
                    case GameState.Menu:
                        Application.Quit();
                        break;
                    case GameState.Playing:
                    case GameState.Over:
                    case GameState.Dead:
                        StartCoroutine(LoadMainMenuRoutine());
                        break;
                }
            }
        }
    }
}
