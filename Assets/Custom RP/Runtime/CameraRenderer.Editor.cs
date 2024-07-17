using System.Net;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Profiling;

partial class CameraRenderer
{
   partial void DrawUnsupportedShaders();
   partial void DrawGizmos();
   partial void PrepareBuffer();
#if UNITY_EDITOR
   private static ShaderTagId[] legacyShaderTagIds = {
      new ShaderTagId("Always"),
      new ShaderTagId("ForwardBase"),
      new ShaderTagId("PrepassBase"),
      new ShaderTagId("Vertex"),
      new ShaderTagId("VertexLMRGBM"),
      new ShaderTagId("VertexLM"),
   };

   private static Material errorMaterial;

   partial void DrawUnsupportedShaders()
   {
      { // Unsupported shaders
         if (errorMaterial == null)
         {
            errorMaterial = new Material(Shader.Find("Hidden/InternalErrorShader"));
         }

         var drawingSettings = new DrawingSettings(legacyShaderTagIds[0], new SortingSettings(camera))
         {
            overrideMaterial = errorMaterial,
         };
         for (int i = 1; i < legacyShaderTagIds.Length; i++)
         {
            drawingSettings.SetShaderPassName(i, legacyShaderTagIds[i]);
         }
         var filteringSettings = FilteringSettings.defaultValue;
         context.DrawRenderers(cullingResults, ref drawingSettings, ref filteringSettings);
      }
   }

   partial void DrawGizmos()
   {
      if (Handles.ShouldRenderGizmos())
      {
         context.DrawGizmos(camera, GizmoSubset.PreImageEffects);
         context.DrawGizmos(camera, GizmoSubset.PostImageEffects);
      }
   }

   private string SampleName;
   
   partial void PrepareBuffer()
   {
      Profiler.BeginSample("Editor Only");
      buffer.name = SampleName = camera.name;
      Profiler.EndSample();
   }
   
#else  
   const string SampleName = bufferName;   

#endif
}