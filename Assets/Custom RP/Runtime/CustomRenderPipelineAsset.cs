using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

[CreateAssetMenu(menuName = "Rendering/Custom Render Pipeline")]
public class CustomRenderPipelineAsset : RenderPipelineAsset
{
   [SerializeField] private bool useSRPBatcher = true;
   
   protected override RenderPipeline CreatePipeline()
   {
      return new CustomRenderPipeline(useSRPBatcher);
   }
}