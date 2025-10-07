using System.Collections.Generic;
using UnityEngine;

using Sirenix.OdinInspector;
public class DecoManager : MonoBehaviour
{
    [Header("Param")]
    [SerializeField] private Mesh grassMesh;
    [SerializeField] private Material grassMat;

    [Header("Storage")]
    [SerializeField] private DecoGrass[] decoArray;
    [SerializeField] private Matrix4x4[] matrixArray;

    [Button]
    private void GetAllMatrixChilds()
    {
        decoArray = GetComponentsInChildren<DecoGrass>();

        List<Matrix4x4> grassList = new();

        int length = decoArray.Length;
        for (int i = 0; i < length; i++)
        {
            grassList.AddRange(decoArray[i].GrassMatrix);
        }

        matrixArray = grassList.ToArray();
    }

    void Update()
    {
        if (!grassMesh || !grassMat) return;

        Graphics.DrawMeshInstanced(grassMesh, 0, grassMat, matrixArray);
    }
}
