using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public static class Noise
{
    public static Vector2Int[] generatorPointsArray;
    public static Vector3Int[] generatorPointsArray3D;

    public static float[,] GenerateNoiseMap(int mapWidth, int mapHeight, uint numCellPoints, bool inverseEnable, bool manhattanDistance)
    {
        float[,] noiseMap = new float[mapWidth, mapHeight];
        generatorPointsArray = new Vector2Int[numCellPoints];
        float[] distanceArray = new float[mapWidth * mapHeight];
        float minimumArrayValue;
        float maximumArrayValue;

        if(mapWidth <= 0 || mapHeight <= 0)
        {
            mapHeight = 10;
            mapWidth = 10;
        }

        if(numCellPoints >= mapWidth * mapHeight || numCellPoints <= 0)
        {
            numCellPoints = 3;
        }

        for(int i = 0;i < numCellPoints;i++)
        {
            generatorPointsArray[i] = new Vector2Int(Random.Range(0, mapWidth), Random.Range(0, mapHeight));
        }

        for(int y = 0;y < mapHeight;y++)
        {
            for(int x = 0;x < mapWidth;x++)
            {
                Vector2Int cell = new(x, y);
                float minDistance = float.MaxValue;
                float distance;

                for(int i = 0;i < numCellPoints;i++)
                {
                    Vector2Int point = generatorPointsArray[i];

                    if(manhattanDistance)
                    {
                        distance = ManhattanDistance2D(cell, point);
                    }
                    else
                    {
                        distance = Vector2Int.Distance(cell, point);
                    }

                    if(distance < minDistance)
                    {
                        minDistance = distance;
                        distanceArray[x + mapWidth * y] = distance;
                    }
                }
            }
        }

        minimumArrayValue = distanceArray.Min();
        maximumArrayValue = distanceArray.Max();

        for(int y = 0;y < mapHeight;y++)
        {
            for(int x = 0;x < mapWidth;x++)
            {
                if(inverseEnable)
                {
                    distanceArray[x + mapWidth * y] = (distanceArray[x + mapWidth * y] - minimumArrayValue) / (maximumArrayValue - minimumArrayValue);
                }
                else
                {
                    distanceArray[x + mapWidth * y] = 1 - ((distanceArray[x + mapWidth * y] - minimumArrayValue) / (maximumArrayValue - minimumArrayValue));
                }

                noiseMap[x, y] = distanceArray[x + mapWidth * y];
            }
        }
        return noiseMap;
    }

    public static float[,] GenerateCellMap(int mapWidth, int mapHeight, uint numCellPoints, bool manhattanDistance)
    {
        float[,] cellMap = new float[mapWidth, mapHeight];
        generatorPointsArray = new Vector2Int[numCellPoints];

        if(mapWidth <= 0 || mapHeight <= 0)
        {
            mapHeight = 10;
            mapWidth = 10;
        }

        if(numCellPoints >= mapWidth || numCellPoints >= mapHeight || numCellPoints <= 0)
        {
            numCellPoints = 3;
        }

        for(int i = 0;i < numCellPoints;i++)
        {
            generatorPointsArray[i] = new Vector2Int(Random.Range(0, mapWidth), Random.Range(0, mapHeight));
        }

        for(int y = 0;y < mapHeight;y++)
        {
            for(int x = 0;x < mapWidth;x++)
            {
                Vector2Int cell = new(x, y);
                float minDistance = float.MaxValue;
                int closestPointIndex = 0;

                float distance;

                for(int i = 0;i < numCellPoints;i++)
                {
                    Vector2Int point = generatorPointsArray[i];

                    if(manhattanDistance)
                    {
                        distance = ManhattanDistance2D(cell, point);
                    }
                    else
                    {
                        distance = Vector2Int.Distance(cell, point);
                    }

                    if(distance < minDistance)
                    {
                        minDistance = distance;
                        closestPointIndex = i;
                    }
                }
                cellMap[y, x] = (float)closestPointIndex / (numCellPoints - 1);
            }
        }
        return cellMap;
    }

    public static float[,] GenerateNoiseMapBound(int mapWidth, int mapHeight, uint numCellPoints, bool inverseEnable, bool manhattanDistance)
    {
        float[,] noiseMap = new float[mapWidth, mapHeight];
        generatorPointsArray = new Vector2Int[numCellPoints];
        float[] distanceArray = new float[mapWidth * mapHeight];
        float minimumArrayValue;
        float maximumArrayValue;
        int gridWidth = mapWidth/10;
        int gridHeight =  mapHeight/10;
        int counterWidth = 0;
        int counterHeight = 0;

        if(mapWidth <= 0 || mapHeight <= 0)
        {
            mapHeight = 10;
            mapWidth = 10;
        }

        for(int i = 0;i < numCellPoints;i++)
        {
            if(numCellPoints <= (((mapWidth * mapHeight) / (gridWidth * gridHeight)) * 2))
            {
                if(counterHeight == 10)
                {
                    counterHeight = 0;
                }
            }

            if(counterWidth != 0 && (counterWidth % (mapWidth / gridWidth) == 0))
            {
                counterWidth = 0;
                counterHeight++;
            }
            float x = Random.Range(counterWidth * gridWidth, (counterWidth + 1) * gridWidth);
            float y = Random.Range(counterHeight * gridHeight, (counterHeight + 1) * gridHeight);
            generatorPointsArray[i] = new Vector2Int((int)x, (int)y);
            counterWidth++;
        }

        for(int y = 0;y < mapHeight;y++)
        {
            for(int x = 0;x < mapWidth;x++)
            {
                Vector2Int cell = new Vector2Int(x, y);
                float minDistance = float.MaxValue;
                float distance;

                for(int i = 0;i < numCellPoints;i++)
                {
                    Vector2Int point = generatorPointsArray[i];
                    if(manhattanDistance)
                    {
                        distance = ManhattanDistance2D(cell, point);
                    }
                    else
                    {
                        distance = Vector2Int.Distance(cell, point);
                    }

                    if(distance < minDistance)
                    {
                        minDistance = distance;
                        distanceArray[x + mapWidth * y] = distance;
                    }
                }
            }
        }

        minimumArrayValue = distanceArray.Min();
        maximumArrayValue = distanceArray.Max();

        for(int y = 0;y < mapHeight;y++)
        {
            for(int x = 0;x < mapWidth;x++)
            {
                if(inverseEnable)
                {
                    distanceArray[x + mapWidth * y] = (distanceArray[x + mapWidth * y] - minimumArrayValue) / (maximumArrayValue - minimumArrayValue);
                }
                else
                {
                    distanceArray[x + mapWidth * y] = 1 - ((distanceArray[x + mapWidth * y] - minimumArrayValue) / (maximumArrayValue - minimumArrayValue));
                }

                noiseMap[x, y] = distanceArray[x + mapWidth * y];
            }
        }
        Debug.Log(generatorPointsArray.Length);
        return noiseMap;
    }

    public static float[,] GenerateNoiseMap3DPoints(int mapWidth, int mapHeight, uint numCellPoints, bool inverseEnable, bool manhattanDistance)
    {
        float[,] noiseMap = new float[mapWidth, mapHeight];
        generatorPointsArray3D = new Vector3Int[numCellPoints];
        float[] distanceArray = new float[mapWidth * mapHeight];
        float minimumArrayValue;
        float maximumArrayValue;

        if(mapWidth <= 0 || mapHeight <= 0)
        {
            mapHeight = 10;
            mapWidth = 10;
        }

        if(numCellPoints >= mapWidth || numCellPoints >= mapHeight || numCellPoints <= 0)
        {
            numCellPoints = 3;
        }

        for(int i = 0;i < numCellPoints;i++)
        {
            generatorPointsArray3D[i] = new Vector3Int(Random.Range(0, mapWidth), Random.Range(0, 25), Random.Range(0, mapHeight));

        }

        for(int y = 0;y < mapHeight;y++)
        {
            for(int x = 0;x < mapWidth;x++)
            {
                Vector3Int cell = new Vector3Int(x, 0, y);
                float minDistance = float.MaxValue;
                float distance;

                for(int i = 0;i < numCellPoints;i++)
                {
                    Vector3Int point = generatorPointsArray3D[i];
                    if(manhattanDistance)
                    {
                        distance = ManhattanDistance3D(cell, point);
                    }
                    else
                    {
                        distance = Vector3Int.Distance(cell, point);
                    }

                    if(distance < minDistance)
                    {
                        minDistance = distance;
                        distanceArray[x + mapWidth * y] = distance;
                    }
                }
            }
        }

        minimumArrayValue = distanceArray.Min();
        maximumArrayValue = distanceArray.Max();

        for(int y = 0;y < mapHeight;y++)
        {
            for(int x = 0;x < mapWidth;x++)
            {
                if(inverseEnable)
                {
                    distanceArray[x + mapWidth * y] = (distanceArray[x + mapWidth * y] - minimumArrayValue) / (maximumArrayValue - minimumArrayValue);
                }
                else
                {
                    distanceArray[x + mapWidth * y] = 1 - ((distanceArray[x + mapWidth * y] - minimumArrayValue) / (maximumArrayValue - minimumArrayValue));
                }

                noiseMap[x, y] = distanceArray[x + mapWidth * y];
            }
        }
        return noiseMap;
    }
    public static int ManhattanDistance3D(Vector3Int a, Vector3Int b)
    {
        checked
        {
            return Mathf.Abs(a.x - b.x) + Mathf.Abs(a.y - b.y) + Mathf.Abs(a.z - b.z);
        }
    }
    public static int ManhattanDistance2D(Vector2Int a, Vector2Int b)
    {
        checked
        {
            return Mathf.Abs(a.x - b.x) + Mathf.Abs(a.y - b.y);
        }
    }
}
