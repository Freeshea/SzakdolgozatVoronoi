using UnityEngine;

public class VoronoiGenerator : MonoBehaviour
{
    public enum DrawMode { CellMap, NoiseMap, ColorMap, MeshWithCell, MeshWithNoise, MeshWithColor }
    public DrawMode drawMode;

    public int mapWidth;
    public int mapHeight;

    public uint generatorPoints;

    public float meshHeightMultiplier;

    public AnimationCurve meshHeightCurve;
    public bool isCurveEnabled;

    public bool isInverse;

    public bool isAutoUpdate;

    public bool isManhattanDistance;

    public TerrainType[] colorRegions;

    public bool isBoundCells;

    public bool is3DPoints;

    public void GenerateMap()
    {
        MeshData meshData;
        float[,] noiseMap;

        if(isBoundCells)
        {
            noiseMap = Noise.GenerateNoiseMapBound(mapWidth, mapHeight, generatorPoints, isInverse, isManhattanDistance);
        }
        else if(is3DPoints)
        {
            noiseMap = Noise.GenerateNoiseMap3DPoints(mapWidth, mapHeight, generatorPoints, isInverse, isManhattanDistance);
        }
        else
        {
            noiseMap = Noise.GenerateNoiseMap(mapWidth, mapHeight, generatorPoints, isInverse, isManhattanDistance);
        }

        Color[] colourMap = ColorMapWithRegions(noiseMap);

        MapDisplay display = FindObjectOfType<MapDisplay>();

        if(drawMode == DrawMode.CellMap)
        {
            ColorMapWithCells(out noiseMap, out Color[] colorCellMap);
            display.DrawTexture(TextureGenerator.TextureFromColorMap(colorCellMap, mapWidth, mapHeight));
        }
        else if(drawMode == DrawMode.NoiseMap)
        {
            display.DrawTexture(TextureGenerator.TextureFromHeightMap(noiseMap));
        }
        else if(drawMode == DrawMode.ColorMap)
        {
            display.DrawTexture(TextureGenerator.TextureFromColorMap(colourMap, mapWidth, mapHeight));
        }
        else if(drawMode == DrawMode.MeshWithCell)
        {
            noiseMap = Noise.GenerateCellMap(mapWidth, mapHeight, generatorPoints, isManhattanDistance);
            meshData = MeshGenerator.GenerateTerrainMesh(noiseMap, meshHeightMultiplier, meshHeightCurve, isCurveEnabled);
            display.DrawMesh(meshData, TextureGenerator.TextureFromHeightMap(noiseMap));
        }
        else if(drawMode == DrawMode.MeshWithNoise)
        {
            meshData = MeshGenerator.GenerateTerrainMesh(noiseMap, meshHeightMultiplier, meshHeightCurve, isCurveEnabled);
            display.DrawMesh(meshData, TextureGenerator.TextureFromHeightMap(noiseMap));
        }

        else if(drawMode == DrawMode.MeshWithColor)
        {
            meshData = MeshGenerator.GenerateTerrainMesh(noiseMap, meshHeightMultiplier, meshHeightCurve, isCurveEnabled);
            display.DrawMesh(meshData, TextureGenerator.TextureFromColorMap(colourMap, mapWidth, mapHeight));
        }
    }

    private void ColorMapWithCells(out float[,] noiseMap, out Color[] colorCellMap)
    {
        colorCellMap = new Color[mapWidth * mapHeight];
        noiseMap = Noise.GenerateCellMap(mapWidth, mapHeight, generatorPoints, isManhattanDistance);

        for(int y = 0;y < mapHeight;y++)
        {
            for(int x = 0;x < mapWidth;x++)
            {
                float uniqueNumber = noiseMap[x, y];
                colorCellMap[y * mapWidth + x] = Color.HSVToRGB(uniqueNumber, 1f, 1f);
            }
        }
    }

    private Color[] ColorMapWithRegions(float[,] noiseMap)
    {
        Color[] colorMap = new Color[mapWidth * mapHeight];

        for(int y = 0;y < mapHeight;y++)
        {
            for(int x = 0;x < mapWidth;x++)
            {
                float currentHeight = noiseMap[x, y];
                for(int i = 0;i < colorRegions.Length;i++)
                {
                    if(currentHeight <= colorRegions[i].height)
                    {
                        colorMap[y * mapWidth + x] = colorRegions[i].color;
                        break;
                    }
                }
            }
        }
        return colorMap;
    }

    [System.Serializable]
    public struct TerrainType
    {
        public string name;
        public float height;
        public Color color;
    }
}