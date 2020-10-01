using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;
using System.IO;
using UnityEditor;
using UnityEngine;



public class Simplification : MonoBehaviour
{
    public Material material;
    public string assetName = "plan";

    // Start is called before the first frame update
    void Start()
    {
        gameObject.AddComponent<MeshFilter>();
        gameObject.AddComponent<MeshRenderer>();
    }

    // Update is called once per frame
    private void Update()
    {
        GenerateMesh();
    }

    void GenerateMesh()
    {

        string assetPath = "";

        //Trouve et stock le path (chemin/fichier.off du .OFF)
        string[] models = AssetDatabase.FindAssets(assetName, new[] { "Assets/Models" });
        foreach (string guid in models)
        {
            assetPath = AssetDatabase.GUIDToAssetPath(guid);
            Debug.Log(assetPath);
        }

        if (assetPath.Length != 0)
        {
            //va ouvrirr et lire ligne par ligne le fichier .off
            using (StreamReader streamReader = File.OpenText(assetPath))
            {
                if (streamReader.ReadLine() != "OFF")
                {
                    Debug.LogError("Failed to load '" + assetPath + "': Invalid file format");
                    return;
                }

                string infoLine = streamReader.ReadLine();

                //stocke les valeurs des 3 premiers chiffres du fichier .off
                int verticesCount, faceCount, edgeCount;

                //récupère uniquement les chiffres, et non les espaces!!

                string[] Firstnumbers = Regex.Split(infoLine, @"\D+");

                //Convertit la représentation sous forme de chaîne d'un nombre en son équivalent entier 32 bits signé. Une valeur de retour indique si l'opération a réussi (out)
                //le format de la ligne lue est un string, on la transforme en int pour donner une valeur aux int sommets, facettes et arêtes (dans ce cas, juste le nombre de ces élements dans la 3D)
                if (Firstnumbers.Length !=  3 
                || !int.TryParse(Firstnumbers[0], out verticesCount) 
                || !int.TryParse(Firstnumbers[1], out faceCount) 
                || !int.TryParse(Firstnumbers[2], out edgeCount))
                {
                    Debug.LogError("Failed to load '" + assetPath + "': Vertex/face/edge count are invalid");
                    Debug.Log(infoLine);
                    return;
                }

                //int.TryParse(Firstnumbers[0], out verticesCount);
                //int.TryParse(Firstnumbers[1], out faceCount);
                //int.TryParse(Firstnumbers[2], out edgeCount);


                //coordonnées contenues dans le sommet
                Vector3[] vertices = new Vector3[verticesCount];

                for (int i = 0; i < verticesCount; ++i)
                {
                    //format de lecture ( - comme moins & . comme virgule)
                    var format = new NumberFormatInfo();
                    format.NegativeSign = "-";
                    format.NumberDecimalSeparator = ".";


                    string vertexCoordLine = streamReader.ReadLine();
                    //sépare chaque valeur de vertexCoordLine par " "
                    string[] vertexCoords = vertexCoordLine.Split(' ');

                    if (vertexCoords.Length != 3
                    || !float.TryParse(vertexCoords[0], NumberStyles.Float, format, out vertices[i].x)
                    || !float.TryParse(vertexCoords[1], NumberStyles.Float, format, out vertices[i].y)
                    || !float.TryParse(vertexCoords[2], NumberStyles.Float, format, out vertices[i].z))
                    {
                        Debug.LogError("Failed to load '" + assetPath + "': Vertex " + i + " has invalid coordinates");
                        return;
                    }

                    //le format de la ligne lue est un string, on la transforme en float et on remplie les coo (x,y,z)
                    //float.TryParse(verticesCoords[0], NumberStyles.Float, format, out vertices[i].x);
                    //float.TryParse(verticesCoords[1], NumberStyles.Float, format, out vertices[i].y);
                    //float.TryParse(verticesCoords[2], NumberStyles.Float, format, out vertices[i].z);
                }


                //chaque face à 3 sommets
                int[] faces = new int[faceCount * 3];

                for (int i = 0; i < faceCount; i++)
                {
                    string indexLine = streamReader.ReadLine();

                    //récupère uniquement les chiffres, et non les espaces!!
                    string[] indexArray = Regex.Split(indexLine, @"\D+");

                    int indexCount;

                    if (indexArray.Length < 4
                    || !int.TryParse(indexArray[0], out indexCount) || indexCount != 3
                    || !int.TryParse(indexArray[1], out faces[i * 3])
                    || !int.TryParse(indexArray[2], out faces[i * 3 + 1])
                    || !int.TryParse(indexArray[3], out faces[i * 3 + 2])
                    || faces[i * 3] >= verticesCount
                    || faces[i * 3 + 1] >= verticesCount
                    || faces[i * 3 + 2] >= verticesCount)
                    {
                        Debug.LogError("Failed to load '" + assetPath + "': Indices for face " + i + " are invalid");
                        Debug.Log(indexLine);
                        return;
                    }

                    //int.TryParse(indexArray[0], out indexCount);
                    //int.TryParse(indexArray[1], out faces[i * 3]);
                    //int.TryParse(indexArray[2], out faces[i * 3 + 1]);
                    //int.TryParse(indexArray[3], out faces[i * 3 + 2]);

                    //int.Parse(indexArray[0]);
                    //int.Parse(indexArray[1]);
                    //int.Parse(indexArray[2]);
                    //int.Parse(indexArray[3]);

                }

                


                //colorit la 3D
                Mesh mesh = new Mesh();
                mesh.vertices = vertices;
                mesh.triangles = faces;

                gameObject.GetComponent<MeshFilter>().mesh = mesh;
                gameObject.GetComponent<MeshRenderer>().material = material;
            }


        }

    }
}
