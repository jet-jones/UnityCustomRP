using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;
using Random = UnityEngine.Random;

public class MeshBall : MonoBehaviour
{
    [SerializeField] private Mesh mesh;
    [SerializeField] private Material material;

    private static int _baseColorId = Shader.PropertyToID("_BaseColor");
    private Matrix4x4[] _matrices = new Matrix4x4[1023];
    private Vector4[] _baseColors = new Vector4[1023];
    private MaterialPropertyBlock _block;

    private void Awake()
    {
        for (int i = 0; i < _matrices.Length; i++)
        {
            _matrices[i] = Matrix4x4.TRS(Random.insideUnitSphere * 10f, Quaternion.identity, Vector3.one);
            _baseColors[i] = Random.ColorHSV(0, 1);
        }
    }

    private void Update()
    {
        if (_block == null)
        {
            _block = new MaterialPropertyBlock();
            _block.SetVectorArray(_baseColorId, _baseColors);
        }
        Graphics.DrawMeshInstanced(mesh, 0, material, _matrices, 1023, _block);
    }
}
