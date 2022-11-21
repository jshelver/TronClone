using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Trail
{

    public class TrailGeneration : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] Material trailMaterial;
        [SerializeField] Transform trailCreationPointTransform;
        Transform trailParent;

        [Header("Trail Generation Settings")]
        [SerializeField] bool divideLine = true;
        [SerializeField] float trailWidth = 0.2f;

        [Header("Update Trail Settings")]
        [SerializeField] float updateTrailSpawnDelay = 0.1f;
        [SerializeField] int maxNumberOfVertices = 1000; // must be divisible by 4

        [Header("Divide Trail Settings")]
        [SerializeField] float divideTrailSpawnDelay = 0.5f;
        [SerializeField] float trailSectionLength = 2f;
        [SerializeField] float trailLifeTime = 5f;
        float spawnTime;
        bool firstTime, isEven;
        List<Vector3> verticesDef;
        List<int> trianglesDef;
        int numberOfVertices;
        MeshFilter meshFilter;
        MeshCollider meshCollider;

        List<GameObject> trailSegments;

        void Start()
        {
            spawnTime = 0f;
            firstTime = true;
            trailSegments = new List<GameObject>();
            trailParent = GameObject.Find("TrailParent").transform;
        }

        void Update()
        {
            if (divideLine)
            {
                if (Time.time > spawnTime)
                {
                    spawnTime = Time.time + divideTrailSpawnDelay;
                    DivideLine();
                }
            }
            else
            {
                if (Time.time > spawnTime)
                {
                    spawnTime = Time.time + updateTrailSpawnDelay;
                    GenerateTrail();
                }
            }
        }

        private void GenerateTrail()
        {
            Vector3[] vertices = null;
            int[] triangles = null;

            Vector3 backward = trailCreationPointTransform.position;

            if (firstTime)
            {
                vertices = new Vector3[]
                {
                backward + (trailCreationPointTransform.right * -trailWidth),
                backward - (trailCreationPointTransform.right * -trailWidth),
                backward - (trailCreationPointTransform.right * -trailWidth) + trailCreationPointTransform.up * trailWidth,
                backward + (trailCreationPointTransform.right * -trailWidth) + trailCreationPointTransform.up * trailWidth
                };

                triangles = new int[]
                {
                0, 2, 1,
                0, 3, 2
                };

                // Initilize new gameobject
                GameObject trail = new GameObject("Trail");
                trail.transform.parent = trailParent;
                trail.tag = "Trail";

                // Add mesh filter and mesh renderer's material
                meshFilter = trail.AddComponent<MeshFilter>();
                trail.AddComponent<MeshRenderer>().material = trailMaterial;

                // Add mesh collider
                meshCollider = trail.AddComponent<MeshCollider>();
                meshCollider.sharedMesh = meshFilter.mesh;

                // Set the mesh's vertices and triangles
                meshFilter.mesh.vertices = vertices;
                meshFilter.mesh.triangles = triangles;

                // Create lists that store references to these vertices and triangles
                verticesDef = new List<Vector3>();
                trianglesDef = new List<int>();
                foreach (Vector3 v in vertices)
                {
                    verticesDef.Add(v);
                }
                foreach (int t in triangles)
                {
                    trianglesDef.Add(t);
                }

                isEven = false;
                firstTime = false;

                numberOfVertices = 4;
                return;
            }

            if (numberOfVertices >= maxNumberOfVertices)
            {
                numberOfVertices -= 4;
                for (int i = 0; i < 4; i++)
                {
                    verticesDef.RemoveAt(0);
                }
                for (int i = 0; i < 30; i++)
                {
                    trianglesDef.RemoveAt(0);
                }
                List<int> temp = new List<int>(); ;
                for (int i = 0; i < trianglesDef.Count; i++)
                {
                    temp.Add(trianglesDef[i] - 4);
                }
                trianglesDef = temp;
            }

            if (isEven)
            {
                verticesDef.Add(backward + (trailCreationPointTransform.right * -trailWidth));
                verticesDef.Add(backward - (trailCreationPointTransform.right * -trailWidth));
                verticesDef.Add(backward - (trailCreationPointTransform.right * -trailWidth) + trailCreationPointTransform.up * trailWidth);
                verticesDef.Add(backward + (trailCreationPointTransform.right * -trailWidth) + trailCreationPointTransform.up * trailWidth);

                // Left face
                trianglesDef.Add(numberOfVertices - 4);
                trianglesDef.Add(numberOfVertices - 1);
                trianglesDef.Add(numberOfVertices);

                trianglesDef.Add(numberOfVertices - 4);
                trianglesDef.Add(numberOfVertices);
                trianglesDef.Add(numberOfVertices + 3);

                // Top face
                trianglesDef.Add(numberOfVertices - 4);
                trianglesDef.Add(numberOfVertices + 3);
                trianglesDef.Add(numberOfVertices + 2);

                trianglesDef.Add(numberOfVertices - 4);
                trianglesDef.Add(numberOfVertices + 2);
                trianglesDef.Add(numberOfVertices - 3);

                // Right face
                trianglesDef.Add(numberOfVertices - 3);
                trianglesDef.Add(numberOfVertices + 2);
                trianglesDef.Add(numberOfVertices + 1);

                trianglesDef.Add(numberOfVertices - 3);
                trianglesDef.Add(numberOfVertices + 1);
                trianglesDef.Add(numberOfVertices - 2);

                // Bottom face
                trianglesDef.Add(numberOfVertices - 2);
                trianglesDef.Add(numberOfVertices + 1);
                trianglesDef.Add(numberOfVertices);

                trianglesDef.Add(numberOfVertices - 2);
                trianglesDef.Add(numberOfVertices);
                trianglesDef.Add(numberOfVertices - 1);

                // Back face
                trianglesDef.Add(numberOfVertices);
                trianglesDef.Add(numberOfVertices + 1);
                trianglesDef.Add(numberOfVertices + 2);

                trianglesDef.Add(numberOfVertices);
                trianglesDef.Add(numberOfVertices + 2);
                trianglesDef.Add(numberOfVertices + 3);

                isEven = false;
            }
            else
            {
                verticesDef.Add(backward + (trailCreationPointTransform.right * -trailWidth) + trailCreationPointTransform.up * trailWidth);
                verticesDef.Add(backward - (trailCreationPointTransform.right * -trailWidth) + trailCreationPointTransform.up * trailWidth);
                verticesDef.Add(backward - (trailCreationPointTransform.right * -trailWidth));
                verticesDef.Add(backward + (trailCreationPointTransform.right * -trailWidth));

                // Left face
                trianglesDef.Add(numberOfVertices - 4);
                trianglesDef.Add(numberOfVertices + 3);
                trianglesDef.Add(numberOfVertices);

                trianglesDef.Add(numberOfVertices - 4);
                trianglesDef.Add(numberOfVertices);
                trianglesDef.Add(numberOfVertices - 1);

                // Top face
                trianglesDef.Add(numberOfVertices - 2);
                trianglesDef.Add(numberOfVertices - 1);
                trianglesDef.Add(numberOfVertices);

                trianglesDef.Add(numberOfVertices - 2);
                trianglesDef.Add(numberOfVertices);
                trianglesDef.Add(numberOfVertices + 1);

                // Right face
                trianglesDef.Add(numberOfVertices - 3);
                trianglesDef.Add(numberOfVertices - 2);
                trianglesDef.Add(numberOfVertices + 1);

                trianglesDef.Add(numberOfVertices - 3);
                trianglesDef.Add(numberOfVertices + 1);
                trianglesDef.Add(numberOfVertices + 2);

                // Bottom face
                trianglesDef.Add(numberOfVertices - 3);
                trianglesDef.Add(numberOfVertices + 2);
                trianglesDef.Add(numberOfVertices + 3);

                trianglesDef.Add(numberOfVertices - 3);
                trianglesDef.Add(numberOfVertices + 3);
                trianglesDef.Add(numberOfVertices - 4);

                // Back face
                trianglesDef.Add(numberOfVertices);
                trianglesDef.Add(numberOfVertices + 1);
                trianglesDef.Add(numberOfVertices + 2);

                trianglesDef.Add(numberOfVertices);
                trianglesDef.Add(numberOfVertices + 2);
                trianglesDef.Add(numberOfVertices + 3);

                isEven = true;
            }
            numberOfVertices += 4;

            meshFilter.mesh.vertices = verticesDef.ToArray();
            meshFilter.mesh.triangles = trianglesDef.ToArray();

            meshFilter.mesh.RecalculateNormals();
            meshFilter.mesh.RecalculateBounds();

            meshCollider.sharedMesh = meshFilter.mesh;
        }

        private void DivideLine()
        {
            Vector3 backward = trailCreationPointTransform.position;
            Vector3 backwardOffset = trailCreationPointTransform.position - (trailCreationPointTransform.forward * trailSectionLength);

            Vector3[] vertices = new Vector3[]
            {
            backward + (trailCreationPointTransform.right * -trailWidth),
            backward - (trailCreationPointTransform.right * -trailWidth),
            backward - (trailCreationPointTransform.right * -trailWidth) + trailCreationPointTransform.up * trailWidth,
            backward + (trailCreationPointTransform.right * -trailWidth) + trailCreationPointTransform.up * trailWidth,
            backwardOffset + (trailCreationPointTransform.right * -trailWidth) + trailCreationPointTransform.up * trailWidth,
            backwardOffset - (trailCreationPointTransform.right * -trailWidth) + trailCreationPointTransform.up * trailWidth,
            backwardOffset - (trailCreationPointTransform.right * -trailWidth),
            backwardOffset + (trailCreationPointTransform.right * -trailWidth)
            };

            int[] triangles =
            {
            0, 3, 2, // Front face
            0, 2, 1,
            2, 3, 4, // Top face
            2, 4, 5,
            1, 2, 5, // Right face
            1, 5, 6,
            0, 7, 4, // Left face
            0, 4, 3,
            5, 4, 7, // Bottom face
            5, 7, 6,
            0, 6, 7, // Back face
            0, 1, 6
            };
            // Initialize trail segment game object
            GameObject trailSegment = new GameObject($"Trail Segment{trailSegments.Count}");
            trailSegments.Add(trailSegment);
            trailSegment.transform.parent = trailParent;
            trailSegment.tag = "Trail";

            // Add mesh filter, renderer, and collider
            MeshFilter mf = trailSegment.AddComponent<MeshFilter>();
            MeshCollider mc = trailSegment.AddComponent<MeshCollider>();
            trailSegment.AddComponent<MeshRenderer>().material = trailMaterial;

            // Set mesh vertices and triangles
            mf.mesh.vertices = vertices;
            mf.mesh.triangles = triangles;

            // Set mesh collider mesh
            mc.sharedMesh = mf.mesh;

            mf.mesh.RecalculateNormals();
            mf.mesh.RecalculateBounds();

            // Destroy after certain amount of time
            Destroy(trailSegment, trailLifeTime);
        }
    }

}
