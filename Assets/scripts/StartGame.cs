using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartGame : MonoBehaviour
{
    public Button newGame;
    public Button solve;
    public Button easy;
    public Button mid;
    public Button hard;
    public Button target;
    public GameObject place;
    public Sprite mine;
    public Sprite grass;
    public Sprite flag;
    public Text won;
    public Text lose;
    private Vector3 autoLocalScale;
    private System.Random random = new System.Random();
    private Button[] grid;
    private int _gameSize;
    private static bool minesSet = false;
    private static bool end = false;
    void Start()
    {
        newGame.onClick.AddListener(starNew);
        easy.onClick.AddListener(startEasy);
        mid.onClick.AddListener(startMid);
        hard.onClick.AddListener(startHard);
        solve.onClick.AddListener(delegate { solveGame(random.Next(_gameSize), random.Next(_gameSize));});
        autoLocalScale = new Vector3(1, 1, 1);
        ToogleButtons();
    }
    private void solveGame(int x, int y)
    {
            for (int yy = y - 1; yy <= y + 1; yy++)
                if (yy > 0 && yy <= _gameSize)
                    for (int xx = x - 1; xx <= x + 1; xx++)
                        if (xx > 0 && xx <= _gameSize)
                            if (GetField(xx, yy) != -1)
                            {
                                for (int c = 0; c < _gameSize * _gameSize; c++)
                                    if (grid[c].GetComponent<Position>().x == xx && grid[c].GetComponent<Position>().y == yy)
                                    {
                                        grid[c].onClick.Invoke();
                                          solveGame(random.Next(_gameSize), random.Next(_gameSize));
                                }
                            }
                            else
                                for (int q = 0; q < _gameSize * _gameSize; q++)
                                    if (grid[q].GetComponent<Position>().x == xx && grid[q].GetComponent<Position>().y == yy)
                                    { grid[q].image.sprite = flag; solveGame(random.Next(_gameSize), random.Next(_gameSize)); }
    }
    private void starNew()
    {
        SceneManager.LoadScene(0);
        Start();
    }
    private void startEasy()
    {
        _gameSize = 9;
        MoveAwayButtons();
        for (int y = 1; y <= _gameSize; y++)
        {
            for (int x = 1; x <= _gameSize; x++)
            {
                CreateObj(x, y);
            }
        }

        EditGrid(_gameSize);
    }
    private void startMid()
    {
        _gameSize = 12;
        MoveAwayButtons();
        for (int y = 1; y <= _gameSize; y++)
        {
            for (int x = 1; x <= _gameSize; x++)
            {
                CreateObj(x, y);
            }
        }
        EditGrid(_gameSize);
    }
    private void startHard()
    {
        _gameSize = 15;
        MoveAwayButtons();
        for (int y = 1; y <= _gameSize; y++)
        {
            for (int x = 1; x <= _gameSize; x++)
            {
                CreateObj(x, y);
            }
        }

        EditGrid(_gameSize);
    }
    public int GetField(int x, int y)
    {
        for (int i = 0; i < _gameSize * _gameSize; i++)
            if (grid[i].GetComponent<Position>().x == x && grid[i].GetComponent<Position>().y == y)
                return grid[i].GetComponent<Position>().isMine;
        return 0;
    }
    public void SetField(int x, int y, int value)
    {
        for (int i = 0; i < _gameSize * _gameSize; i++)
        {
            if (grid[i].GetComponent<Position>().x == x && grid[i].GetComponent<Position>().y == y)
                grid[i].GetComponent<Position>().isMine = value;
        }

    }
    void SetMines(int count)
    {
        while (count > 0)
        {
            int mineX = random.Next(count);
            int mineY = random.Next(count);

            if (GetField(mineX, mineY) == 0)
            {
                SetField(mineX, mineY, -1); //Add a mine
                count--;
            }
        }
    }
    int countMinesOnField()
    {
        int mines = 0;
        for (int i = 0; i < _gameSize * _gameSize; i++)
            if (grid[i].GetComponent<Position>().isMine == -1)
                mines++;
        return mines;
    }
    void ToogleButtons()
    {
        newGame.enabled = !newGame.enabled;
        solve.enabled = !solve.enabled;

        newGame.interactable = !newGame.interactable;
        solve.interactable = !solve.interactable;
    }
    void MoveAwayButtons()
    {
        easy.GetComponent<GoAway>().enabled = true;
        mid.GetComponent<GoAway>().enabled = true;
        hard.GetComponent<GoAway>().enabled = true;
        Destroy(easy);
        Destroy(mid);
        Destroy(hard);
        ToogleButtons();
        place.SetActive(true);
    }
    void EditGrid(int size)
    {
        place.AddComponent<DG>();
        place.GetComponent<DG>().col = size;
        place.GetComponent<DG>().row = size;
        grid = place.GetComponentsInChildren<Button>();
    }
    void CreateObj(int x, int y)
    {
        var newObj = Instantiate(target);
        newObj.GetComponent<Position>().x = x;
        newObj.GetComponent<Position>().y = y;
        newObj.GetComponent<Position>().isMine = 0;
        newObj.transform.SetParent(GameObject.FindGameObjectWithTag("Game").transform);
        newObj.transform.localScale = autoLocalScale;
        newObj.transform.localPosition = Vector3.zero;
        newObj.name = x + "_" + y;
        newObj.onClick.AddListener(delegate { playAction(newObj.GetComponent<Position>().x, newObj.GetComponent<Position>().y, newObj); });
    }
    int FindMines(int x, int y)
    {
        int mineCount = 0;
        for (int yy = y - 1; yy <= y + 1; yy++)
        {
            if (yy > 0 && yy <= _gameSize)
                for (int xx = x - 1; xx <= x + 1; xx++)
                {
                    if (xx > 0 && xx <= _gameSize)
                    {
                        if (GetField(xx, yy) == -1)
                            mineCount++;
                    }

                }
        }
        return mineCount;
    }
    public void SetGrass(int x, int y, int mines)
    {
        for (int i = 0; i < _gameSize * _gameSize; i++)
        {
            if (grid[i].GetComponent<Position>().x == x && grid[i].GetComponent<Position>().y == y)
            {
                if (mines > 0) grid[i].GetComponentInChildren<Text>().text = mines.ToString();
                grid[i].image.sprite = grass;
                grid[i].interactable = !grid[i].interactable;
                grid[i].enabled = !grid[i].enabled;
            }
        }
    }
    private void playAction(int x, int y, Button current)
    {
        if (!minesSet) { SetMines(_gameSize); minesSet = true; }
        int mines = FindMines(x, y);
        if (current.GetComponent<Position>().isMine == -1)
        {
            current.GetComponent<Image>().sprite = mine;
            lose.enabled = true;
            lose.transform.SetAsLastSibling();
            for (int i = 0; i < _gameSize * _gameSize; i++)
                grid[i].enabled = false;
            solve.enabled = false;
        }
        else
        {
            SetGrass(x, y, mines);
        }
        //EndGame();
    }
    public void EndGame()
    {
        int mines = 0;
        int flagmines = 0;

        for (int i = 0; i < _gameSize * _gameSize; i++)
            if (grid[i].GetComponent<Position>().isMine == -1)
                mines++;
        for (int i = 0; i < _gameSize * _gameSize; i++)
            if (grid[i].image.sprite == flag && grid[i].GetComponent<Position>().isMine == -1)
                flagmines++;
        if (flagmines == mines && flagmines>0 && mines >0)
        {
            won.enabled = true;
            won.transform.SetAsLastSibling();
            for (int i = 0; i < _gameSize * _gameSize; i++)
                grid[i].enabled = false;
            solve.enabled = false;
            end = true;
        }
    }
    private void Update()
    {
        EndGame();
    }
}