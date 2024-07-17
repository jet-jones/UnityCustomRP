using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;

public partial class CameraRenderer
{
   private ScriptableRenderContext context;
   private Camera camera;

   private static ShaderTagId unlitShaderTagId = new ShaderTagId("SRPDefaultUnlit");
   
   private const string bufferName = "Render Camera";
   private CommandBuffer buffer = new CommandBuffer { 
      name = bufferName
   };

   private CullingResults cullingResults;
   
   public void Render(ScriptableRenderContext context, Camera camera)
   {
      this.context = context;
      this.camera = camera;

      PrepareBuffer();
      if (camera.cameraType == CameraType.SceneView)
      {
         ScriptableRenderContext.EmitWorldGeometryForSceneView(camera);
      }
      
      if (camera.TryGetCullingParameters(out ScriptableCullingParameters p))
      {
         cullingResults = context.Cull(ref p);
      }
      else return;
      
      context.SetupCameraProperties(camera);
      CameraClearFlags flags = camera.clearFlags;
      buffer.ClearRenderTarget(
         flags <= CameraClearFlags.Depth, 
         flags <= CameraClearFlags.Color, 
         flags == CameraClearFlags.Color ? camera.backgroundColor.linear : Color.clear);
      buffer.BeginSample(SampleName);
      ExecuteBuffer(); // Which buffer is this executing?
     
      { // Draw
         var sortingSettings = new SortingSettings(camera)
         {
            criteria = SortingCriteria.CommonOpaque
         };
         var drawingSettings = new DrawingSettings(unlitShaderTagId, sortingSettings);
         var filteringSettings = new FilteringSettings(RenderQueueRange.opaque);
         
         context.DrawRenderers(cullingResults, ref drawingSettings, ref filteringSettings);
         context.DrawSkybox(camera);

         sortingSettings.criteria = SortingCriteria.CommonTransparent;
         drawingSettings.sortingSettings = sortingSettings;
         filteringSettings.renderQueueRange = RenderQueueRange.transparent;
         
         context.DrawRenderers(cullingResults, ref drawingSettings, ref filteringSettings);
        
         DrawUnsupportedShaders();  
         DrawGizmos();
      }
      buffer.EndSample(SampleName);
      ExecuteBuffer();
      context.Submit();
   }
   
   void ExecuteBuffer()
   {
      context.ExecuteCommandBuffer(buffer);
      buffer.Clear();
   }
}