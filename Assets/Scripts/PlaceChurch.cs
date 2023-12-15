using UnityEngine;

public class PlaceChurch : MonoBehaviour
{
    public GameObject churchPrefab;
    public Transform churchParent;
    public GameObject treePrefab;
    public Transform treeParent;
    public VoronoiGenerator voronoiGenerator;
    public MeshFilter meshFilter;
    public int treeCount = 10;
    public void PlaceChurchesOnMainCells()
    {
        for(int i = churchParent.childCount - 1;i >= 0;i--)
        {
            Object churchChild = churchParent.GetChild(i).gameObject;
            DestroyImmediate(churchChild);
        }

        Vector2Int[] cellPointsArray = Noise.generatorPointsArray;
        Vector3Int[] cellPointsArray3D = Noise.generatorPointsArray3D;

        if(!voronoiGenerator.is3DPoints)
        {
            if(cellPointsArray == null || churchPrefab is null)
            {            
                Debug.LogError("Generate a map first: cellPointsArray or churchPrefab is null");
            }
            else
            {
                foreach(var cellPoint in cellPointsArray)
                {
                    float xCoord = (cellPoint.x * 10) - 995;
                    float zCoord = ((cellPoint.y * 10) - 995) * -1;

                    float yHeightCoord = GetHeightAtCoordinates(xCoord, zCoord);

                    Vector3 mainCell = new(xCoord, yHeightCoord - 15, zCoord);
                    GameObject church = Instantiate(churchPrefab, mainCell, Quaternion.identity);
                    church.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);

                    if(churchParent != null)
                        church.transform.parent = churchParent;
                }
            }
        }
        else
        {
            if(cellPointsArray3D is null || churchPrefab is null)
            {
                Debug.LogError("Generate a map first: cellPointsArray is null");
            }
            else
            {
                foreach(var cellPoint in cellPointsArray3D)
                {
                    Vector3 mainCell = new((cellPoint.x * 10) - 995, cellPoint.y * ((int)voronoiGenerator.meshHeightMultiplier), ((cellPoint.z * 10) - 995) * -1);
                    GameObject church = Instantiate(churchPrefab, mainCell, Quaternion.identity);
                    church.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);

                    if(churchParent != null)
                        church.transform.parent = churchParent;
                }
            }
        }

    }

    public void PlaceTrees()
    {
        for(int i = treeParent.childCount - 1;i >= 0;i--)
        {
            Object treeChild = treeParent.GetChild(i).gameObject;
            DestroyImmediate(treeChild);

        }

        Vector2Int[] cellPointsArray = Noise.generatorPointsArray;

        if(!voronoiGenerator.is3DPoints)
        {
            if(cellPointsArray is null || treePrefab is null)
            {
                Debug.LogError("Generate a map first: cellPointsArray is null");
            }
            else
            {
                for(int i = 0;i < treeCount;i++)
                {
                    float xCoord = (Random.Range(0, voronoiGenerator.mapWidth) * 9.9f) - 1000;
                    float zCoord = (Random.Range(0, voronoiGenerator.mapHeight) * 9.9f) - 1000;
                    float yHeightCoord = GetHeightAtCoordinates(xCoord, zCoord);
                    Vector3 treeSpawnPoint = new Vector3(xCoord, yHeightCoord, zCoord);
                    GameObject tree = Instantiate(treePrefab, treeSpawnPoint, Quaternion.identity);
                    tree.transform.localScale = new Vector3(1000, 1000, 1000);
                    tree.transform.Rotate(-90, 0, 0);

                    if(treeParent != null)
                        tree.transform.parent = treeParent;
                }
            }
        }
    }

    public float GetHeightAtCoordinates(float x, float z)
    {
        if(meshFilter == null || meshFilter.sharedMesh == null)
        {
            Debug.LogError("MeshFilter or Mesh not assigned.");
            return 0f;
        }

        Mesh mesh = meshFilter.sharedMesh;
        Vector3[] vertices = mesh.vertices;

        float closestDistance = float.MaxValue;
        float height = 0f;

        for(int i = 0;i < vertices.Length;i++)
        {
            Vector3 vertex = meshFilter.transform.TransformPoint(vertices[i]);
            float distance = Vector2.Distance(new Vector2(x, z), new Vector2(vertex.x, vertex.z));

            if(distance < closestDistance)
            {
                closestDistance = distance;
                height = vertex.y;
            }
        }

        return height;
    }
}
