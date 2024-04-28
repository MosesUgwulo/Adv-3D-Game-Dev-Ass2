using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Maze
{
    public class SideWinder : MonoBehaviour
    {
        public GameObject vWall, hWall, ground;
        public GameObject[,] GridObjectsH, GridObjectsV;
        public const int N = 1, S = 2, E = 4, W = 8;
        public int[,] Grid;
        [SerializeField]
        [Range(5, 100)]
        public int width, height, wallSize;
        public float wallHeight;

        private void Start()
        {
            Init();
            GenerateMazeBinary();
            DisplayGrid();
            // StartCoroutine(GenereateMazeAndDisplay());
        }

        // IEnumerator GenereateMazeAndDisplay()
        // {
        //     yield return StartCoroutine(GenerateMazeBinary());
        //     yield return StartCoroutine(DisplayGrid());
        // }
        
        private void Update()
        {
            // if (Input.GetKeyDown(KeyCode.G))
            // {
            //     foreach (var obj in GameObject.FindGameObjectsWithTag("Wall"))
            //     {
            //         Destroy(obj);
            //     }
            //     Init();
            //     GenerateMazeBinary();
            //     DisplayGrid();
            // }
        }

        void Init()
        {
            height = width;
            ground.transform.localScale = new Vector3(width * wallSize + width, 0.1f, height * wallSize + height);
            vWall.transform.localScale = new Vector3(0.1f, wallHeight, wallSize);
            hWall.transform.localScale = new Vector3(wallSize, wallHeight, 0.1f);
            
            Grid = new int[width, height];
            GridObjectsV = new GameObject[width + 1, height + 1];
            GridObjectsH = new GameObject[width + 1, height + 1];
            
            DrawGrid();
        }

        private void DrawGrid()
        {
            for (int i = 0; i <= height; i++)
            {
                for (int j = 0; j <= width; j++)
                {
                    if (i < height)
                    {
                        float vWallSize = vWall.transform.localScale.z;
                        float xOffsetV, zOffsetV;
                        xOffsetV = -(width * vWallSize) / 2;
                        zOffsetV = -(height * vWallSize) / 2;
                        
                        GridObjectsV[j, i] = Instantiate(vWall, 
                            new Vector3(-vWallSize / 2 + j * wallSize + xOffsetV, wallSize / 2,
                                i * vWallSize + zOffsetV), Quaternion.identity, transform);
                        
                        GridObjectsV[j, i].SetActive(true);
                        GridObjectsV[j, i].name = "v" + i + j;
                        GridObjectsV[j, i].tag = "Wall";
                    }
                    
                    if (j < width)
                    {
                        float hWallSize = hWall.transform.localScale.x;
                        float xOffsetH, zOffsetH;
                        xOffsetH = -(width * hWallSize) / 2;
                        zOffsetH = -(height * hWallSize) / 2;
                        
                        GridObjectsH[j, i] = Instantiate(hWall, 
                            new Vector3(j * hWallSize + xOffsetH, wallSize / 2,
                                -hWallSize / 2 + i * hWallSize + zOffsetH), Quaternion.identity, transform);
                        
                        GridObjectsH[j, i].SetActive(true);
                        GridObjectsH[j, i].name = "h" + i + j;
                        GridObjectsH[j, i].tag = "Wall";
                    }
                }
            }
        }

        private void GenerateMazeBinary()
        {
            float randomNumber;
            int runStart;
        
            for (int row = 0; row < height; row++)
            {
                for (int cell = 0; cell < width; cell++)
                {
                    randomNumber = Random.Range(0, 101);
                    runStart = randomNumber > 50 ? N : E;
        
                    if (cell == width - 1)
                    {
                        runStart = N;
                    }
                    else if (row == height - 1)
                    {
                        runStart = E;
                    }
                    
                    Grid[cell, row] = runStart;
                }
            }
        }
        
        // private IEnumerator GenerateMazeBinary()
        // {
        //     float randomNumber;
        //     int runStart;
        //
        //     for (int row = 0; row < height; row++)
        //     {
        //         for (int cell = 0; cell < width; cell++)
        //         {
        //             randomNumber = Random.Range(0, 101);
        //             runStart = randomNumber > 50 ? N : E;
        //
        //             if (cell == width - 1)
        //             {
        //                 runStart = N;
        //             }
        //             else if (row == height - 1)
        //             {
        //                 runStart = E;
        //             }
        //             
        //             Grid[cell, row] = runStart;
        //             yield return new WaitForSeconds(0.1f);
        //         }
        //     }
        // }

        private void DisplayGrid()
        {
            for (int row = 0; row < height; row++)
            {
                for (int cell = 0; cell < width; cell++)
                {
                    if (Grid[cell, row] == N)
                    {
                        Destroy(GridObjectsH[cell, row + 1]);
                    }
                    else if (Grid[cell, row] == E)
                    {
                        Destroy(GridObjectsV[cell + 1, row]);
                    }
                }
            }
        }
        
        // private IEnumerator DisplayGrid()
        // {
        //     for (int row = 0; row < height; row++)
        //     {
        //         for (int cell = 0; cell < width; cell++)
        //         {
        //             if (Grid[cell, row] == N)
        //             {
        //                 Destroy(GridObjectsH[cell, row + 1]);
        //             }
        //             else if (Grid[cell, row] == E)
        //             {
        //                 Destroy(GridObjectsV[cell + 1, row]);
        //             }
        //
        //             yield return new WaitForSeconds(0.1f);
        //         }
        //     }
        // }
    }
}
