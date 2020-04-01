using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
//! Sample terrain animator/generator
public class terrainAnim : MonoBehaviour
{
    private Terrain _myTerr;
    private TerrainData _myTerrData;
    private int _xRes;
    private int _yRes;
    float[,] _terrHeights;
    float[,] originalTerrainSectionHeight;
    public int numberOfPasses = 5;
    public int radiusOfAnimation = 50;

    public float persistence = 0.2f;
    public float lacunarity = 2;
    public int numberOfOctaves = 3;
    public float RANDOM_PARTICLE= 1;
    private List<float> _octaves= new List<float>();
    public float ocataveStep = 0.05f;

    public bool useJPG = true;
    public Texture2D texture;
    public bool Gen = true;
    // Use this for initialization
    void Start()
    {
        if (Gen)
        {
            // Get terrain and terrain data handles
            _myTerr = GetComponent<Terrain>();
            _myTerrData = _myTerr.terrainData;
            // Get terrain dimensions in tiles (X tiles x Y tiles)
            _xRes = _myTerrData.heightmapResolution;
            _yRes = _myTerrData.heightmapResolution;
            float tmp = 0.0f;
            for (int i = 1; i <= numberOfOctaves; i++)
            {
                _octaves.Add(tmp);
                tmp += ocataveStep;
            }
            // Set heightmap
            if (!useJPG)
                RandomizeTerrain();
            else
                GenerateFromJPG();
        }
    }
    // Update is called once per frame
    void Update()
    {
        // Call animation function
        //AnimTerrain();
    }
    // Set the terrain using noise pattern
    private void RandomizeTerrain()
    {
        // Extract entire heightmap (expensive!)
        _terrHeights = _myTerrData.GetHeights(0, 0, _xRes, _yRes);
        // STUDENT'S CODE //
        // ...
        float[] scale = new float[numberOfPasses];
        for (int k = 0; k < numberOfPasses; k++)
        {
            scale[k] = UnityEngine.Random.Range(4.0f, 12.0f);
        }
        for (int i = 0; i < _xRes; i++)
        {
            for (int j = 0; j < _yRes; j++)
            {
               
                float xCoeff = (float)i / _xRes;
                float yCoeff = (float)j / _yRes;
                _terrHeights[i, j] = 0;
                for (int l = 0; l < numberOfPasses; l++)
                {
                    for (int k = 0; k < _octaves.Count; k++)
                    {
                        float p = Mathf.Pow(persistence, k);
                        _terrHeights[i, j] += GetH(_octaves[k], p, xCoeff * scale[l], yCoeff * scale[l]);
                    }
                }
                //for (int k = 0; k < numberOfPasses; k++)
                //{
                //    _terrHeights[i, j] +=
                //    Mathf.PerlinNoise(xCoeff * scale[k], yCoeff * scale[k]);
                //}

                _terrHeights[i, j] /= (float)(numberOfPasses+numberOfOctaves);
            }
        }
        //
        // Set terrain heights (_terrHeights[coordX, coordY] = heightValue) in a loop
        //
        // You can sample perlin's noise (Mathf.PerlinNoise (xCoeff, yCoeff)) using
        //coefficients
        // between 0.0f and 1.0f
        //
        // You can combine 2-3 layers of noise with different resolutions and amplitudes for
        //a better effect
    // ...
    // END OF STUDENT'S CODE //
    // Set entire heightmap (expensive!)
        _myTerrData.SetHeights(0, 0, _terrHeights);
        originalTerrainSectionHeight = _myTerrData.GetHeights(147, 168, radiusOfAnimation * 2,
        radiusOfAnimation * 2);
    }
    private float GetH(float randomParticleParameter, float persistence, float x, float y)
    {
        float h = 0.0f, r, p;
        r = UnityEngine.Random.Range(-RANDOM_PARTICLE,RANDOM_PARTICLE)*randomParticleParameter;
        p = Mathf.PerlinNoise(x, y)*(1-randomParticleParameter);
        h = r + p;
        return h*persistence;
    }

    // Animate part of the terrain
    private void AnimTerrain()
    {
        // STUDENT'S CODE //
        // ...
        // Extract PART of the terrain e.g. 40x40 tiles (select corner (x, y) and extracted
        //patch size)
// GetHeights(5, 5, 10, 10) will extract 10x10 tiles at position (5, 5)
//
// Animate it using Time.time and trigonometric functions
//
// 3d generalizaton of sinc(x) function can be used to create the teardrop effect
//(sinc(x) = sin(x) / x)
    //
// It is reasonable to store animated part of the terrain in temporary variable e.g.
//in RandomizeTerrain()
    // function. Later, in AnimTerrain() this temporary area can be combined with
//calculated Z(height) value.
// Make sure you make a deep copy instead of shallow one (Clone(), assign operator).
//
// Set PART of the terrain (use extraction parameters)
//
// END OF STUDENT'S CODE //
_terrHeights = _myTerrData.GetHeights(147, 168, radiusOfAnimation * 2,
radiusOfAnimation * 2);
        Vector2 middle = new Vector2(radiusOfAnimation, radiusOfAnimation);
        for (int i = 0; i < radiusOfAnimation * 2; i++)
        {
            for (int j = 0; j < radiusOfAnimation * 2; j++)
            {
                Vector2 point = new Vector2(i, j);
                double distance = Vector2.Distance(point, middle);
                double difference = (radiusOfAnimation - distance) /
                radiusOfAnimation;
                if (difference < 0) difference = 0;
                _terrHeights[i, j] = (float)(originalTerrainSectionHeight[i, j] *
                (Math.Sin(Time.time + distance / 10) / 2f) * difference) + originalTerrainSectionHeight[i, j];
            }
        }
        _myTerrData.SetHeights(147, 168, _terrHeights);
    }
    private void GenerateFromJPG()
    {
        _terrHeights = _myTerrData.GetHeights(0, 0, _xRes, _yRes);
        try
        {
            int sizeX = texture.height, sizeY = texture.width;
            Color notText = texture.GetPixel(0, 0);
            bool wasTextNearby = false;
            for( int i = 0;i < sizeX; i++)
            {
                for(int j = 0;j<sizeY;j++)
                {
                    if(texture.GetPixel(i,j).Equals(notText))
                    {
                        if (wasTextNearby)
                        {
                            //_terrHeights[_xRes - j -1, _yRes-i-1] = 0.5f;
                            wasTextNearby = false;
                        }
                        else
                        {
                            _terrHeights[_xRes - j - 1, _yRes - i - 1] = 0;
                        }
                    }
                    else
                    {
                        _terrHeights[_xRes - j - 1, _yRes - i - 1] = 1;
                        wasTextNearby = true;
                    }
                }
            }
            _myTerrData.SetHeights(0, 0, _terrHeights);
        }
        catch(Exception e)
        {
            Debug.Log(e.Message);
        }
    }
}