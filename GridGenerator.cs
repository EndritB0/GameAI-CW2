using System.Collections.Generic;
using UnityEngine;

// Not needed for the coursework task.

//public enum CellType
//{ 
//    Water,
//    Land,
//    Sand
//}

//public class Cell
//{
//    public Vector3 Position { get; set; }
//    public CellType CellType { get; set; }
//    public int Cost { get; set; }

//    public Cell(Vector3 position, CellType cellType, int cost)
//    {
//        Position = position;
//        CellType = cellType;
//        Cost = cost;
//    }
//}

public class GridGenerator : MonoBehaviour
{
    // These are all public so that you can look at their values in the inspector.
    // This is very bad practice. Don't do this.
    // If you edit the values by accident, you can reset the script in the inspector to revert to the hardcoded values.

    // Edited: Endrit Brahimi
    public int Width = 30;
    public int Depth = 30;

    public float playerSpeed = 3f;
    public int NumberOfObstacles = 15;
    public GameObject ObstaclePrefab;
    public GameObject Player;
    public Vector3 StartPosition;
    public GameObject Destination;
    public GameObject GridMarkerPrefab;
    public GameObject GridMarkerHolder;
    public Vector3 EndPosition;
    public Transform Ground;
    public Transform PathCells;
    public GameObject PathPrefab;
    public HashSet<Vector3> Obstacles;
    public HashSet<Vector3> WalkableCells;
    public bool shouldPlayerMove = false;
    public List<Vector3> playerPath;
    public GameObject playerInstance;
    public int pathIndex = 0;

    // This hardcoded array defines the grid layout.
    // 0 is empty, 1 is an obstacle, 2 is player position, 3 is destination position.
    // Edited: Endrit Brahimi
    public int[,] scenario1 = new int[,] {
        { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 ,1 },
        { 1, 2, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0 ,1 },
        { 1, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1, 1, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0 ,1 },
        { 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1, 1, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0 ,1 },
        { 1, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 ,1 },
        { 1, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0 ,1 },
        { 1, 1, 1, 0, 1, 1, 1, 1, 1, 0, 1, 1, 1, 1, 1, 0, 1, 1, 1, 1, 1, 0, 1, 1, 1, 1, 1, 0, 1 ,1 },
        { 1, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0 ,1 },
        { 1, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 1, 0, 1, 0, 0, 0, 1, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0 ,1 },
        { 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 1, 0, 0, 0, 1, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0 ,1 },
        { 1, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 ,1 },
        { 1, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0 ,1 },
        { 1, 1, 1, 1, 1, 0, 1, 1, 1, 1, 1, 0, 1, 1, 1, 1, 1, 0, 1, 1, 1, 1, 1, 0, 1, 1, 1, 1, 1 ,1 },
        { 1, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0 ,1 },
        { 1, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 1, 0, 1, 1, 0, 0, 1, 0, 0, 0, 0 ,1 },
        { 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0 ,1 },
        { 1, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 ,1 },
        { 1, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0 ,1 },
        { 1, 1, 1, 1, 1, 1, 1, 0, 1, 1, 1, 1, 1, 0, 1, 1, 1, 1, 1, 0, 1, 1, 1, 1, 1, 0, 1, 1, 1 ,1 },
        { 1, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0 ,1 },
        { 1, 0, 0, 0, 0, 1, 1, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0 ,1 },
        { 1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0 ,1 },
        { 1, 0, 0, 0, 0, 1, 1, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 ,1 },
        { 1, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0 ,1 },
        { 1, 1, 1, 1, 0, 1, 1, 1, 1, 1, 0, 1, 1, 1, 1, 1, 0, 1, 1, 1, 1, 1, 0, 1, 1, 1, 0, 1, 1 ,1 },
        { 1, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0 ,1 },
        { 1, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 ,1 },
        { 1, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0 ,1 },
        { 1, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 1, 0, 0, 0, 3 ,1 },
        { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 ,1 }
    };

    private void Start()
    {
        Obstacles = new HashSet<Vector3>();
        WalkableCells = new HashSet<Vector3>();
        GenerateGrid(scenario1);  // Scenario 1 ///////////////////////////////////////////////////////////////////////////
    }

    public void GenerateGrid(int[,] _grid)
    {
        ClearData();
        ClearPath();

        DrawGridMarkers();

        SetGridLayoutFromIntArray(_grid);

        // This can be used instead of SetGridLayoutFromIntArray() to randomise all positions.
        //RandomiseGridLayout();

        LocateWalkableCells();

        // Position Ground based on the size of the grid
        // Default Size of Plane mesh is 10, 1, 10 so that's why we are dividing by 10
        Ground.position = new Vector3((Width / 2f) - 0.5f, 0, (Depth / 2f) - 0.5f);
        Ground.localScale = new Vector3(Width / 10f, 1, Depth / 10f);
        //Camera.main.transform.position = new Vector3(Ground.position.x, 5f * (Width / 10f) + (Width / 10f), Ground.position.z - Depth / 2f - Depth / 4f - (Depth / 10f));
    }

    private void Update()
    {
        if (shouldPlayerMove)
        {
            var nextCellToVisit = playerPath[pathIndex];
            //Debug.Log("Player move in Update: " + nextCellToVisit);

            playerInstance.transform.position = Vector3.MoveTowards(playerInstance.transform.position, nextCellToVisit, playerSpeed * Time.deltaTime);
            playerInstance.transform.LookAt(nextCellToVisit);

            if (playerInstance.transform.position == nextCellToVisit)
            {
                pathIndex--;
            }

            if (pathIndex < 0)
            {
                shouldPlayerMove = false;
                playerPath.Clear();
            }
        }
    }

    private void RandomiseGridLayout()
    {
        PlaceObstaclesAtRandomPositions();
        StartPosition = PlaceObjectAtRandomPosition(Player);
        EndPosition = PlaceObjectAtRandomPosition(Destination);
    }

    private void DrawGridMarkers()
    {
        for (int x = 0; x < Width; x++)
        {
            for (int y = 0; y < Depth; y++)
            {
                if ((x + y) % 2 == 0) // if position is even
                {
                    Vector3 position = new Vector3(x, 0, y);
                    Instantiate(GridMarkerPrefab, position, Quaternion.identity, GridMarkerHolder.transform);
                }
            }
        }
    }

    private void LocateWalkableCells()
    {
        for (int z = 0; z < Depth; z++)
        {
            for (int x = 0; x < Width; x++)
            {
                if (!IsCellOccupied(new Vector3(x, 0, z)))
                {
                    WalkableCells.Add(new Vector3(x, 0, z));
                }
            }
        }
    }

    public List<Vector3> GetNeighbours(Vector3 currentCell)
    {
        var neighbours = new List<Vector3>()
        {
            new Vector3(currentCell.x - 1, 0, currentCell.z), // Up
            new Vector3(currentCell.x + 1, 0, currentCell.z), // Down
            new Vector3(currentCell.x, 0, currentCell.z - 1), // Left
            new Vector3(currentCell.x, 0, currentCell.z + 1), // Right
        };

        var walkableNeighbours = new List<Vector3>();

        foreach (var neighbour in neighbours)
        {
            if (!IsCellOccupied(neighbour) && IsInLevelBounds(neighbour))
            {
                walkableNeighbours.Add(neighbour);
            }

        }

        return walkableNeighbours;
    }

    private bool IsInLevelBounds(Vector3 neighbour)
    {
        if (neighbour.x >= 0 && neighbour.x <= Width - 1 && neighbour.z >= 0 && neighbour.z <= Depth - 1)
        {
            return true;
        }

        return false;
    }

    public void ClearPath()
    {
        foreach (Transform pathCell in PathCells)
        {
            Destroy(pathCell.gameObject);
        }
    }

    private Vector3 PlaceObjectAtRandomPosition(GameObject gameObjectToPlace)
    {
        while (true)
        {
            var positionX = UnityEngine.Random.Range(1, Width);
            var positionZ = UnityEngine.Random.Range(1, Depth);

            // Y must be 0 otherwise even if x and z match we would still get multiple objects placed at the same location because they have different Y
            var cellPosition = new Vector3(positionX, 0, positionZ);

            if (!IsCellOccupied(cellPosition))
            {
                var objectPosition = cellPosition;
                objectPosition.y = gameObjectToPlace.transform.position.y;

                if (gameObjectToPlace.name == "PlayerHolder")
                {
                    playerInstance = Instantiate(gameObjectToPlace, objectPosition, Quaternion.identity, transform);
                }
                else
                {
                    Instantiate(gameObjectToPlace, objectPosition, Quaternion.identity, transform);
                }

                return cellPosition;
            }
        }
    }

    private void ClearData()
    {
        DestroyChildren(transform);
        DestroyChildren(GridMarkerHolder.transform);
        Obstacles.Clear();
        WalkableCells.Clear();
    }

    private void DestroyChildren(Transform parent)
    {
        foreach (Transform child in parent)
        {
            Destroy(child.gameObject);
        }
    }

    private void PlaceObstaclesAtRandomPositions()
    {
        var obstaclesToGenerate = NumberOfObstacles;

        while (obstaclesToGenerate > 0)
        {
            var positionX = UnityEngine.Random.Range(1, Width);
            var positionZ = UnityEngine.Random.Range(1, Depth);

            // Y must be 0 otherwise even if x and z match we would still get multiple objects placed at the same location because they have different Y
            var cellPosition = new Vector3(positionX, 0, positionZ);

            if (!IsCellOccupied(cellPosition))
            {
                Obstacles.Add(cellPosition);

                var objectPosition = cellPosition;
                objectPosition.y = ObstaclePrefab.transform.position.y;

                Instantiate(ObstaclePrefab, objectPosition, Quaternion.identity, transform);
                obstaclesToGenerate--;
            }
        }
    }

    private void SetGridLayoutFromIntArray(int[,] intArray)
    {
        int width = intArray.GetLength(1);
        int height = intArray.GetLength(0);

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                Vector3 cellPosition = new Vector3(x, 0, y);

                if (!IsCellOccupied(cellPosition))
                {
                    switch (intArray[x, y])
                    {
                        case 1:
                            Vector3 obstaclePosition = cellPosition;
                            Instantiate(ObstaclePrefab, obstaclePosition, Quaternion.identity, transform);
                            Obstacles.Add(cellPosition);
                            break;

                        case 2:
                            Vector3 playerPosition = cellPosition;
                            playerPosition.y = Player.transform.position.y;
                            playerInstance = Instantiate(Player, playerPosition, Quaternion.identity, transform);
                            StartPosition = playerPosition;
                            break;

                        case 3:
                            Vector3 destinationPosition = cellPosition;
                            Instantiate(Destination, destinationPosition, Quaternion.identity, transform);
                            EndPosition = destinationPosition;
                            break;

                        default:
                            break;
                    }

                }
            }
        }

        Print2DIntArray(scenario1);
        Debug.Log("Obstacles generated: " + Obstacles.Count);
    }

    public void VisualizePath(Dictionary<Vector3, Vector3> cellParents)
    {
        var path = new List<Vector3>();
        var current = cellParents[EndPosition];

        path.Add(EndPosition);

        while (current != StartPosition)
        {
            path.Add(current);
            current = cellParents[current];
        }

        // Edited: Endrit Brahimi
        Debug.Log("Path Length: " + path.Count);

        for (int i = 1; i < path.Count; i++)
        {
            var pathCellPosition = path[i];
            pathCellPosition.y = PathPrefab.transform.position.y;
            Instantiate(PathPrefab, pathCellPosition, Quaternion.identity, PathCells);
        }

        MovePlayer(path);
    }

    private void MovePlayer(List<Vector3> path)
    {
        Debug.Log("Move Player!");
        shouldPlayerMove = true;
        playerPath = path;
        pathIndex = playerPath.Count - 1;
        Debug.Log("Destination cell: " + EndPosition);
    }

    private bool IsCellOccupied(Vector3 position)
    {
        if (Obstacles.Contains(position))
        {
            return true;
        }

        return false;
    }

    void Print2DIntArray(int[,] intArray)
    {
        int width = intArray.GetLength(1);
        int height = intArray.GetLength(0);
        string gridString = "";

        for (int y = 0; y < height; y++)
        {
            string rowString = "  ";

            for (int x = 0; x < width; x++)
            {
                string value = "";

                switch (intArray[y, x])
                {
                    case 0:
                        value = "□";
                        break;
                    case 1:
                        value = "■";
                        break;
                    case 2:
                        value = "X";
                        break;
                    case 3:
                        value = "G";
                        break;
                    default:
                        break;
                }

                rowString += value + "   ";
            }

            gridString += rowString + "\n";
        }

        Debug.Log(gridString);
    }
}
