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

    [SerializeField] private AudioSource source;

    [SerializeField] private AudioClip StartMenuMusic;
    [SerializeField] private AudioClip StartGameMusic;
    [SerializeField] private AudioClip GameMusic;
    [SerializeField] private AudioClip YouWinMusic;
    [SerializeField] private AudioClip TheEndMusic;

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
        source.clip = StartMenuMusic;
        source.time = 0;
        source.loop = true;
        source.Play();
        screenFader?.CrossFadeAlpha(0f, fadeDelay, true);
        yield return new WaitForSeconds(fadeDelay);
        gameState = GameState.Menu;
    }

    private IEnumerator LoadMainMenuRoutine()
    {
        if (source.isPlaying)
        {
            StartCoroutine(AudioFadeOut.FadeOut(source, fadeDelay - 0.5f));
        }
        screenFader?.CrossFadeAlpha(1f, fadeDelay, true);
        yield return new WaitForSeconds(fadeDelay);
        DeleteAllEntities();
        SceneManager.LoadScene(0);
        gameState = GameState.Loading;
        source.clip = StartMenuMusic;
        source.time = 0;
        source.loop = true;
        source.Play();
        screenFader?.CrossFadeAlpha(0f, fadeDelay, true);
        yield return new WaitForSeconds(fadeDelay);
        gameState = GameState.Menu;
    }


    private IEnumerator LoadMainGameRoutine()
    {
        // Show setupscreen
        gameState = GameState.Loading;
        if (source.isPlaying)
        {
            StartCoroutine(AudioFadeOut.FadeOut(source, fadeDelay - 0.5f));
        }
        screenFader?.CrossFadeAlpha(1f, fadeDelay, true);
        yield return new WaitForSeconds(fadeDelay);
        SceneManager.LoadScene(1);
        source.clip = StartGameMusic;
        source.time = 0;
        source.Play();
        screenFader?.CrossFadeAlpha(0f, fadeDelay, true);
        yield return new WaitForSeconds(gameEndDelay);


        // load game
        if (source.isPlaying)
        {
            StartCoroutine(AudioFadeOut.FadeOut(source, fadeDelay - 0.5f));
        }
        screenFader?.CrossFadeAlpha(1f, fadeDelay, true);
        yield return new WaitForSeconds(fadeDelay);
        DeleteAllEntities();
        SceneManager.LoadScene(2);
        source.clip = GameMusic;
        source.time = 0;
        source.loop = true;
        source.Play();
        IsGameOver = false;
        screenFader?.CrossFadeAlpha(0f, fadeDelay, true);
        yield return new WaitForSeconds(fadeDelay);
        gameState = GameState.Playing;
    }

    private IEnumerator LoadGameOverRoutine()
    {
        // Load win screen
        gameState = GameState.Loading;
        if (source.isPlaying)
        {
            StartCoroutine(AudioFadeOut.FadeOut(source, fadeDelay - 0.5f));
        }
        screenFader?.CrossFadeAlpha(1f, fadeDelay, true);
        yield return new WaitForSeconds(fadeDelay);
        DeleteAllEntities();
        SceneManager.LoadScene(3);
        screenFader?.CrossFadeAlpha(0f, fadeDelay, true);
        yield return new WaitForSeconds(fadeDelay);
        source.clip = YouWinMusic;
        source.time = 0;
        source.loop = false;
        source.Play();
        yield return new WaitForSeconds(fadeDelay);
        if (source.isPlaying)
        {
            StartCoroutine(AudioFadeOut.FadeOut(source, fadeDelay - 0.5f));
        }
        screenFader?.CrossFadeAlpha(1f, fadeDelay, true);
        yield return new WaitForSeconds(fadeDelay);
        SceneManager.LoadScene(4);
        screenFader?.CrossFadeAlpha(0f, fadeDelay, true);
        yield return new WaitForSeconds(fadeDelay);
        source.clip = TheEndMusic;
        source.time = 0;
        source.loop = false;
        source.Play();
        gameState = GameState.Over;
        yield return new WaitForSeconds(fadeDelay);
        gameState = GameState.Loading;
        if (source.isPlaying)
        {
            StartCoroutine(AudioFadeOut.FadeOut(source, fadeDelay - 0.5f));
        }
        screenFader?.CrossFadeAlpha(1f, fadeDelay, true);
        yield return new WaitForSeconds(fadeDelay);
        SceneManager.LoadScene(0);
        screenFader?.CrossFadeAlpha(0f, fadeDelay, true);
        yield return new WaitForSeconds(fadeDelay);
        gameState = GameState.Menu;
        source.clip = StartMenuMusic;
        source.time = 0;
        source.loop = true;
        source.Play();
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
                        StopAllCoroutines();
                        StartCoroutine(LoadMainMenuRoutine());
                        break;
                }
            }
        }
    }
}
