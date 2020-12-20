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

    [SerializeField] private Image screenFader;
    [SerializeField] private float fadeDelay = 2f;
    [SerializeField] private float gameEndDelay = 5f;

    private GameState gameState;

    private Transform player;

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

        // player = FindObjectOfType<PlayerManager>().transform;
    }

    // Start is called before the first frame update
    void Start()
    {
        gameState = GameState.Loading;
        StartCoroutine(LoadMainMenuRoutine());
    }

    private IEnumerator LoadMainMenuRoutine()
    {
        yield return StartCoroutine(LoadMenuRoutine());
    }

    private IEnumerator LoadMenuRoutine()
    {
        screenFader?.CrossFadeAlpha(0f, fadeDelay, true);
        yield return new WaitForSeconds(fadeDelay);
        gameState = GameState.Menu;
    }

    private IEnumerator LoadMainGameRoutine()
    {
        gameState = GameState.Loading;
        screenFader?.CrossFadeAlpha(1f, fadeDelay, true);
        yield return new WaitForSeconds(fadeDelay);
        SceneManager.LoadScene(1);
        screenFader?.CrossFadeAlpha(0f, fadeDelay, true);
        yield return new WaitForSeconds(fadeDelay);
        PlayerManager.EnablePlayer(true);
        gameState = GameState.Playing;
    }

    private IEnumerator LoadGameOverRoutine()
    {
        gameState = GameState.Loading;
        screenFader?.CrossFadeAlpha(1f, fadeDelay, true);
        yield return new WaitForSeconds(fadeDelay);
        DeleteAllEntities();
        SceneManager.LoadScene(3);
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
        var entityArray = entityManager.GetAllEntities();
        foreach (var entity in entityArray)
        {
            entityManager.DestroyEntity(entity);
        }
        entityArray.Dispose();
    }

    // Update is called once per frame
    void Update()
    {
        if (gameState == GameState.Loading)
        {
            return;
        }

        var gamepad = Gamepad.current;
        if (gamepad != null)
        {
            if (gamepad.buttonSouth.wasPressedThisFrame)
            {
                switch (gameState)
                {
                    case GameState.Menu:
                        StartCoroutine(LoadMainGameRoutine());
                        break;
                    case GameState.Playing:
                        StartCoroutine(LoadDeadRoutine());
                        break;
                }
            }

            if (gamepad.buttonNorth.wasPressedThisFrame)
            {
                switch (gameState)
                {
                    case GameState.Playing:
                        StartCoroutine(LoadGameOverRoutine());
                        break;
                }
            }
        }

        var keyboard = Keyboard.current;
        if (keyboard != null)
        {
            if (keyboard.aKey.wasPressedThisFrame)
            {
                switch (gameState)
                {
                    case GameState.Menu:
                        StartCoroutine(LoadMainGameRoutine());
                        break;
                    case GameState.Playing:
                        StartCoroutine(LoadDeadRoutine());
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
