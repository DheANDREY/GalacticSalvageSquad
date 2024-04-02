using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class TileGenerator : MonoBehaviour
{
    [SerializeField] private int width, height, wallCount=8, resourceCount=4;
    public TileGrid _tilePrefab;
    public Obstacle _obstacle;
    public GameObject _wall;
    public GameObject[] _resource;
    [SerializeField] private Transform tileParent, obsParent, resourceParent;

    public TextMeshProUGUI t_timer;
    private float minutes, seconds, curStopwatch;
    private void Start()
    {
        //GenerateGrid();
        GenerateGrid();
        GenerateObstacle(7);
    }

    public bool isGenerateNew;
    private void Update()
    {
        if (isGenerateNew)
        {
            ClearObstacles();
            GenerateObstacle(7);
            isGenerateNew = false;
        }

        curStopwatch += Time.deltaTime;
        minutes = Mathf.FloorToInt(curStopwatch / 60);
        seconds = Mathf.FloorToInt(curStopwatch % 60);

        t_timer.text = string.Format("{0:00}   {1:00}", minutes, seconds);
    }

    private void GenerateGrid()
    {
        for(int x = 0; x < width; x++)
        {
            for(int y = 0; y < height; y++)
            {
                var spawnedTile = Instantiate(_tilePrefab, new Vector3(x, y), Quaternion.identity);
                spawnedTile.name = $"Tile{x} {y}";
                spawnedTile.transform.SetParent(tileParent);
            }
        }

        //_camera.transform.position = new Vector3((float)width / 2 - 0.5f, (float)height / 2 - 0.5f, -10);
    }

    public List<Vector2Int> obstaclePositions, wallPositions, resoPositions;
    private void GenerateObstacle(int obstacleCount)
    {
        // Generate the grid as before
        //GenerateGrid();

        // List to keep track of tiles where obstacles are placed
        obstaclePositions = new List<Vector2Int>();

        // Generate obstacle positions
        for (int i = 0; i < obstacleCount; i++)
        {
            // Generate random position within the grid
            int obstacleX = Random.Range(1, width - 1);
            int obstacleY = Random.Range(0, height);

            // Ensure the obstacle doesn't overlap with existing obstacles
            while (obstaclePositions.Contains(new Vector2Int(obstacleX, obstacleY)))
            {
                obstacleX = Random.Range(1, width - 1);
                obstacleY = Random.Range(0, height);
            }

            // Instantiate obstacle prefab at the chosen position
            var obstacle = Instantiate(_obstacle, new Vector3(obstacleX, obstacleY), Quaternion.identity);
            obstacle.name = $"Obstacle{i + 1}";
            obstacle.transform.SetParent(obsParent); // Assuming you have an obstacle parent object

            // Store the obstacle position
            obstaclePositions.Add(new Vector2Int(obstacleX, obstacleY));
        }

        wallPositions = new List<Vector2Int>();
        for (int j = 0; j < wallCount; j++)
        {
            // Lakukan sesuatu untuk wall

            // Generate random position within the grid
            int wallX = Random.Range(1, width - 1);
            int wallY = Random.Range(0, height);

            // Ensure the wall doesn't overlap with existing obstacles or walls
            while (obstaclePositions.Contains(new Vector2Int(wallX, wallY)))
            {
                wallX = Random.Range(1, width - 1);
                wallY = Random.Range(0, height);
            }

            // Instantiate wall prefab at the chosen position
            var wall = Instantiate(_wall, new Vector3(wallX, wallY), Quaternion.identity);
            wall.name = $"Wall{j + 1}";
            wall.transform.SetParent(tileParent); // Assuming you have a wall parent object
            wallPositions.Add(new Vector2Int(wallX, wallY));
        }

        resoPositions = new List<Vector2Int>();
        // RESOURCE MANAGER
        for (int k = 0; k < resourceCount; k++)
        {
            // Lakukan sesuatu untuk wall

            // Generate random position within the grid
            int resX = Random.Range(1, width - 1);
            int resY = Random.Range(0, height);

            // Ensure the wall doesn't overlap with existing obstacles or walls
            while (resoPositions.Contains(new Vector2Int(resX, resY)) || wallPositions.Contains(new Vector2Int(resX, resY)) || obstaclePositions.Contains(new Vector2Int(resX, resY)))
            {
                resX = Random.Range(1, width - 1);
                resY = Random.Range(0, height);
            }

            // Instantiate wall prefab at the chosen position
            var resource = Instantiate(_resource[Random.Range(0,_resource.Length)], new Vector3(resX, resY), Quaternion.identity);
            resource.name = $"Resource{k + 1}";
            resource.transform.SetParent(resourceParent); // Assuming you have a wall parent object
            resoPositions.Add(new Vector2Int(resX, resY));
        }
    }

    private void ClearObstacles()
    {
        GameObject[] obstacles = GameObject.FindGameObjectsWithTag("obstacle");
        GameObject[] walls = GameObject.FindGameObjectsWithTag("wall");
        GameObject[] resources = GameObject.FindGameObjectsWithTag("resource");
        foreach (GameObject obstacle in obstacles)
        {
            Destroy(obstacle);
        }

        foreach (GameObject wall in walls)
        {
            Destroy(wall);
        }

        foreach (GameObject resource in resources)
        {
            Destroy(resource);
        }
    }

    public GameObject audio;
    public void RestartGame()
    {
        SceneManager.LoadScene("Gameplay");
        audio.SetActive(true);
        // Mengembalikan Time.timeScale ke 1 setelah merestart game
        Time.timeScale = 1f;

    }
}
