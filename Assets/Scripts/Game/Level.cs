using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Threading;

public class Level : MonoBehaviour
{
    const float CAMERA_ORTHO_SIZE = 50f;
    const float PIPE_WIDTH = 10.5f;
    const float PIPE_HEAD_HEIGHT = 4f;
    static float PIPE_SPEED = 30f;
    const float PIPE_DESTROY_X_POSITION = -105f;
    const float PIPE_SPAWN_X_POSITION = 100f;
    const float GROUND_DESTROY_X_POSITION = -100f;
    const float GROUND_SPAWN_X_POSITION = 100f;
    const float BIRD_X_POSITION = 0f;
    List<Pipe> pipeList;
    List<Transform> groundList;
    float pipeSpawnTimer;
    float pipeSpawnTimerMax;
    float gapSize;
    int pipesSpawned;

    static Level instance;
    int score;
    State state;

    enum State
    {
        WaittingToPlay,
        Playing,
        BirdIsDead
    }

    public enum Difficulty
    {
        Easy,
        Medium,
        Hard,
        Impossible
    }
    void Awake()
    {
        pipeList = new List<Pipe>();

        pipeSpawnTimerMax = 1.4f;
        pipesSpawned = 0;
        SetDifficulty(Difficulty.Easy);
        SpawnInitialGround();
        instance = this;
        score = 0;

        state = State.WaittingToPlay;
    }
    void Start()
    {
        Bird.GetInstance().OnDied += Bird_OnDied;
        Bird.GetInstance().OnStartedPlaying += On_Started_Playing;
    }

    private void Bird_OnDied(object sender, System.EventArgs e)
    {
        Debug.Log("Dead Event");
        state = State.BirdIsDead;

        if (PlayerPrefs.GetInt("highscore") < score)
        {

            PlayerPrefs.SetInt("highscore", score);
            PlayerPrefs.Save();

        }


    }


    private void On_Started_Playing(object sender, System.EventArgs e)
    {
        Debug.Log("Game Start");
        state = State.Playing;

    }

    void Update()
    {
        if (state == State.Playing)
        {
            HandlePipeMovement();
            HandlePipeSpawning();
            HandleGround();
        }
    }

    void SpawnInitialGround()
    {
        groundList = new List<Transform>();
        float groundY = -48f;
        float groundWidth = 100f;
        Transform groundTransform;
        groundTransform = Instantiate(GameAssets.GetInstance().pfGround, new Vector3(0, groundY, 0), Quaternion.identity);
        groundList.Add(groundTransform);
        groundTransform = Instantiate(GameAssets.GetInstance().pfGround, new Vector3(groundWidth, groundY, 0), Quaternion.identity);
        groundList.Add(groundTransform);
        groundTransform = Instantiate(GameAssets.GetInstance().pfGround, new Vector3(groundWidth * 2f, groundY, 0), Quaternion.identity);
        groundList.Add(groundTransform);

    }
    void HandleGround()
    {
        float groundY = -48f;
        float groundWidth = 100f;
        foreach (Transform groundTransform in groundList)
        {
            groundTransform.position += new Vector3(-1, 0, 0) * PIPE_SPEED * Time.deltaTime;
            if (groundTransform.position.x < GROUND_DESTROY_X_POSITION)
            {
                // ground passed to left side relocate to the rightside
                groundTransform.position = new Vector3(groundWidth * 2f, groundY, 0);
            }
        }
    }

    public static Level GetInstance()
    {
        return instance;
    }

    public int GetScore()
    {
        return score;
    }
    Difficulty GetDifficulty()
    {
        if (pipesSpawned >= 200) return Difficulty.Impossible;
        if (pipesSpawned >= 100) return Difficulty.Hard;
        if (pipesSpawned >= 50) return Difficulty.Medium;
        return Difficulty.Easy;

    }

    void SetDifficulty(Difficulty difficulty)
    {
        switch (difficulty)
        {
            case Difficulty.Easy:
                gapSize = 50f;
                break;
            case Difficulty.Medium:
                gapSize = 45f;
                break;
            case Difficulty.Hard:
                gapSize = 40f;
                break;
            case Difficulty.Impossible:
                gapSize = 30f;
                break;
            default:
                gapSize = 50f;
                break;
        }
    }

    public int GetPipesSpawned()
    {
        return pipesSpawned;
    }
    void HandlePipeSpawning()
    {
        pipeSpawnTimer -= Time.deltaTime;
        if (pipeSpawnTimer < 0)
        {
            pipeSpawnTimer += pipeSpawnTimerMax;

            float heightHedgeLimit = 10f;
            float minHeight = gapSize * .5f + heightHedgeLimit;
            float totalHeight = CAMERA_ORTHO_SIZE * 2f;
            float maxHeight = totalHeight - gapSize * .5f - heightHedgeLimit;

            float height = Random.Range(minHeight, maxHeight);

            CreateGap(height, gapSize, PIPE_SPAWN_X_POSITION);
            pipesSpawned++;
            SetDifficulty(GetDifficulty());
        }
    }
    void HandlePipeMovement()
    {
        for (int i = 0; i < pipeList.Count; i++)
        {
            Pipe pipe = pipeList[i];

            bool isRightToTheBird = pipe.GetXPosition() > BIRD_X_POSITION;
            pipe.Move();
            if (isRightToTheBird && pipe.GetXPosition() <= BIRD_X_POSITION && pipe.IsBottom())
            {
                score++;
                SoundManager.PlaySound(SoundManager.Sound.Coin);
            }

            if (pipe.GetXPosition() < PIPE_DESTROY_X_POSITION)
            {
                pipe.DestroySelf();
                pipeList.Remove(pipe);
                i--;
            }
        }
    }

    void CreateGap(float gapY, float gapSize, float xPosition)
    {
        CreatePipe(gapY - gapSize * .5f, xPosition, true);
        CreatePipe(CAMERA_ORTHO_SIZE * 2f - gapY - gapSize * 0.5f, xPosition, false);
    }
    void CreatePipe(float height, float xPosition, bool createBottom)
    {

        //SET PIPE HEAD
        Transform pipeHead = Instantiate(GameAssets.GetInstance().pfPipeHead);
        SpriteRenderer pipeHeadSpriteRenderer = pipeHead.GetComponent<SpriteRenderer>();
        // SET PIPE BODY
        Transform pipeBody = Instantiate(GameAssets.GetInstance().pfPipeBody);
        SpriteRenderer pipeBodySpriteRenderer = pipeBody.GetComponent<SpriteRenderer>();
        BoxCollider2D pipeBodyBoxCollider2d = pipeBody.GetComponent<BoxCollider2D>();

        float pipeHeadYPosition;
        float pipeBodyYPosition;
        if (createBottom)
        {
            pipeHeadYPosition = -CAMERA_ORTHO_SIZE + height - PIPE_HEAD_HEIGHT / 2;
            pipeBodyYPosition = -CAMERA_ORTHO_SIZE;
        }
        else
        {
            pipeHeadYPosition = CAMERA_ORTHO_SIZE - height + PIPE_HEAD_HEIGHT / 2;
            pipeBodyYPosition = CAMERA_ORTHO_SIZE;
            pipeBody.localScale = new Vector3(1, -1, 1);
        }
        // pipe head y position
        pipeHead.position = new Vector3(xPosition, pipeHeadYPosition, 0f);

        //pipe body y position
        pipeBody.position = new Vector3(xPosition, pipeBodyYPosition, 0f);
        pipeBodySpriteRenderer.size = new Vector2(PIPE_WIDTH, height);
        pipeBodyBoxCollider2d.size = new Vector2(PIPE_WIDTH - 1f, height);

        Pipe pipe = new Pipe(pipeHead, pipeBody, createBottom);
        // pipeList.Add(pipeBody);
        // pipeList.Add(pipeHead);
        pipeList.Add(pipe);
    }

    private class Pipe
    {

        private Transform pipeHeadTransform;
        private Transform pipeBodyTransform;
        private bool createBottom;
        public Pipe(Transform pipeHeadTransform, Transform pipeBodyTransform, bool createBottom)
        {
            this.pipeBodyTransform = pipeBodyTransform;
            this.pipeHeadTransform = pipeHeadTransform;
            this.createBottom = createBottom;
        }

        public void Move()
        {
            pipeBodyTransform.position += new Vector3(-1, 0, 0) * PIPE_SPEED * Time.deltaTime;
            pipeHeadTransform.position += new Vector3(-1, 0, 0) * PIPE_SPEED * Time.deltaTime;
        }

        public float GetXPosition()
        {
            return pipeBodyTransform.position.x;
        }

        public void DestroySelf()
        {
            Destroy(pipeBodyTransform.gameObject);
            Destroy(pipeHeadTransform.gameObject);
        }

        public bool IsBottom()
        {
            return createBottom;
        }
    }
}
