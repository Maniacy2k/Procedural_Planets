using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Planet : MonoBehaviour
{
    [Range(2, 256)]
    public int resolution = 10;
    private int faceCount = 6;
    public bool autoUpdate = true;

    public ShapeSettings shapeSettings;
    public ColourSettings colourSettings;

    [HideInInspector]
    public bool shapeSettingsFoldOut;
    [HideInInspector]
    public bool colourSettingsFoldOut;

    ShapeGenerator shapeGenerator;

    [SerializeField, HideInInspector]
    MeshFilter[] meshFilters;
    TerrainFace[] terrainFaces;

    private void OnValidate()
    {
        GeneratePlanet();
    }

    void Initialise()
    {
        shapeGenerator = new ShapeGenerator(shapeSettings);
        if (meshFilters == null || meshFilters.Length < 1)
            meshFilters = new MeshFilter[faceCount];

        terrainFaces = new TerrainFace[faceCount];

        Vector3[] directions = new Vector3[] { Vector3.up, Vector3.down, Vector3.left, Vector3.right, Vector3.forward, Vector3.back };

        for (int i = 0; i < faceCount; i++)
        {
            if (meshFilters[i] == null)
            {
                GameObject meshObj = new GameObject("mesh" + (i + 1).ToString("0#"));
                meshObj.transform.parent = transform;

                meshObj.AddComponent<MeshRenderer>().sharedMaterial = new Material(Shader.Find("Standard"));
                meshFilters[i] = meshObj.AddComponent<MeshFilter>();
                meshFilters[i].sharedMesh = new Mesh();
            }

            terrainFaces[i] = new TerrainFace(shapeGenerator, meshFilters[i].sharedMesh, resolution, directions[i]);
        }
    }

    public void GeneratePlanet()
    {
        Initialise();
        GenerateMesh();
        GenerateColours();
    }

    public void OnShapeSettingsUpdated() {
        if (!autoUpdate)
            return;

        Initialise();
        GenerateMesh();
    }


    public void OnColourSettingsUpdated()
    {
        if (!autoUpdate)
            return;

        Initialise();
        GenerateColours();
    }

    void GenerateMesh() {
        foreach (TerrainFace tmpTF in terrainFaces)
            tmpTF.ConstructMesh();
            
        
    }

    void GenerateColours()
    {
        foreach (MeshFilter m in meshFilters) {
            m.GetComponent<MeshRenderer>().sharedMaterial.color = colourSettings.planetColour;
        }
    }
}
