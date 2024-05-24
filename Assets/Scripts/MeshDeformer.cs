using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshDeformer :MonoBehaviour
{
    public MeshFilter sphereMeshFilter; 
    public float displacementAmount = 0.7f; 
    public uint numCellPoints = 2;

    List<int> connectedTriangles = new List<int>();

    public void MeshDeform()
    {
        List<int> randomVertex = new List<int>();

        Mesh sphereMesh = sphereMeshFilter.sharedMesh;
        Mesh copyMesh = sphereMesh;
        Vector3[] sphereVertices = sphereMesh.vertices;
        Vector3 vertexNormal;

        if(sphereMeshFilter != null)
        {

            for(int i = 0;i < numCellPoints;i++)
            {
                int randomIndex;

                do
                {
                    randomIndex = Random.Range(0, sphereMesh.vertices.Length);
                } while(randomVertex.Contains(randomIndex));

                randomVertex.Add(randomIndex);
            }

            foreach(int vertexIndex in randomVertex)
            {

                FindTriangles(sphereMesh.triangles[vertexIndex]);

                vertexNormal = AverageNormal(sphereMesh, sphereMesh.triangles);

                sphereVertices[sphereMesh.triangles[vertexIndex * 3]] += vertexNormal * displacementAmount;

                Debug.Log(vertexNormal + " vertexnormal");
            }
            randomVertex.Clear();

            for(int y = 0;y< sphereVertices.Length;y++)
            {
                Vector3 vertex = sphereVertices[y];
                float minDistance = float.MaxValue;
                int closestPointIndex = 0;

                float distance = minDistance;

                for(int i = 0;i < randomVertex.Count;i++)
                {
                    Vector3 point = sphereVertices[sphereMesh.triangles[randomVertex[i] * 3]];

                    distance = Vector3.Distance(vertex, point);


                    if(distance < minDistance)
                    {
                        minDistance = distance;
                        closestPointIndex = i;
                        
                    }
                }
                if(distance == 0) { continue; }
                FindTriangles(sphereMesh.triangles[y]);

                vertexNormal = AverageNormal(sphereMesh, sphereMesh.triangles);

                sphereVertices[sphereMesh.triangles[y * 3]] += vertexNormal * (1f/distance);


            }

            sphereMesh.vertices = sphereVertices;
            sphereMesh.RecalculateNormals();
            sphereMesh.RecalculateBounds();
        }

    }
    public void FindTriangles(int vertexIndex)
    {
        connectedTriangles.Clear();
        int centerVertexIndex = vertexIndex;
        Mesh squareMesh = sphereMeshFilter.sharedMesh;
        int[] triangles = squareMesh.triangles;

        for(int i = 0;i < triangles.Length;i += 3)
        {
            if(triangles[i] == centerVertexIndex || triangles[i + 1] == centerVertexIndex || triangles[i + 2] == centerVertexIndex)
            {
                connectedTriangles.Add(i);
            }
        }
    }

    public Vector3 AverageNormal(Mesh mesh, int[] triangles)
    {
        Vector3 averageNormal = Vector3.zero;

        foreach(int triangleIndex in connectedTriangles)
        {
            Vector3 normal = mesh.normals[triangles[triangleIndex]];
            averageNormal += normal;
        }

        averageNormal /= connectedTriangles.Count;
        averageNormal.Normalize();
        return averageNormal;
    }
}
