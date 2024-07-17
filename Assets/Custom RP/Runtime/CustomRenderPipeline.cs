using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class CustomRenderPipeline : RenderPipeline
{
   // Have to override old entry point even though it won't be used
   protected override void Render(ScriptableRenderContext context, Camera[] cameras) { }
   
   public CustomRenderPipeline(bool useSRPBatcher)
   {
       GraphicsSettings.useScriptableRenderPipelineBatching = useSRPBatcher;
   }
   
   private CameraRenderer renderer = new ();
   protected override void Render(ScriptableRenderContext context, List<Camera> cameras)
   {
       for (int i = 0; i < cameras.Count; i++)
       {
            renderer.Render(context, cameras[i]); 
       } 
   }
}