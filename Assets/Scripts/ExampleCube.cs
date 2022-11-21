using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExampleCube : MonoBehaviour
{
    [SerializeField] float startX = 0f;
    [SerializeField] float startY = 0f;
    [SerializeField] float startZ = 0f;
    [SerializeField] float xWidth = 1f;
    [SerializeField] float yWidth = 1f;
    [SerializeField] float zWidth = 1f;

    MeshFilter meshFilter;
    MeshRenderer meshRenderer;
    Mesh mesh;
    [SerializeField] Material material;

    void Start()
    {
        CreateCube();
    }

    void Update()
    {
        UpdateCube();
    }

    private void CreateCube()
    {
        meshFilter = gameObject.AddComponent<MeshFilter>();
        meshRenderer = gameObject.AddComponent<MeshRenderer>();
        meshRenderer.material = material;

        mesh = new Mesh();
        meshFilter.mesh = mesh;

        Vector3[] vertices = new Vector3[8];
        vertices[0] = new Vector3(startX, startY, startZ);
        vertices[1] = new Vector3(startX + xWidth, startY, startZ);
        vertices[2] = new Vector3(startX + xWidth, startY + yWidth, startZ);
        vertices[3] = new Vector3(startX, startY + yWidth, startZ);
        vertices[4] = new Vector3(startX, startY, startZ + zWidth);
        vertices[5] = new Vector3(startX + xWidth, startY, startZ + zWidth);
        vertices[6] = new Vector3(startX + xWidth, startY + yWidth, startZ + zWidth);
        vertices[7] = new Vector3(startX, startY + yWidth, startZ + zWidth);

        int[] triangles =
        {
            0, 3, 1, // Front face
            1, 3, 2,
            3, 7, 2, // Top face
            2, 7, 6,
            7, 4, 6, // Back face
            6, 4, 5,
            4, 0, 5, // Bottom face
            5, 0, 1,
            4, 7, 0, // Left face
            0, 7, 3,
            1, 2, 5, // Right face
            5, 2, 6
        };

        mesh.vertices = vertices;
        mesh.triangles = triangles;

        mesh.RecalculateNormals();
    }

    private void UpdateCube()
    {
        Vector3[] vertices = new Vector3[8];
        vertices[0] = new Vector3(startX, startY, startZ);
        vertices[1] = new Vector3(startX + xWidth, startY, startZ);
        vertices[2] = new Vector3(startX + xWidth, startY + yWidth, startZ);
        vertices[3] = new Vector3(startX, startY + yWidth, startZ);
        vertices[4] = new Vector3(startX, startY, startZ + zWidth);
        vertices[5] = new Vector3(startX + xWidth, startY, startZ + zWidth);
        vertices[6] = new Vector3(startX + xWidth, startY + yWidth, startZ + zWidth);
        vertices[7] = new Vector3(startX, startY + yWidth, startZ + zWidth);

        int[] triangles =
        {
            0, 3, 1, // Front face
            1, 3, 2,
            3, 7, 2, // Top face
            2, 7, 6,
            7, 4, 6, // Back face
            6, 4, 5,
            4, 0, 5, // Bottom face
            5, 0, 1,
            4, 7, 0, // Left face
            0, 7, 3,
            1, 2, 5, // Right face
            5, 2, 6
        };

        mesh.vertices = vertices;
        mesh.triangles = triangles;
    }
}
