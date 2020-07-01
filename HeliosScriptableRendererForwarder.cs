using UnityEngine;
using System.Collections;
using UnityEngine.Rendering.Universal;
using UnityEngine.Rendering;

namespace UtopiaWorx.Helios
{
    public class HeliosScriptableRendererForwarder : ScriptableRendererFeature {
        [System.Serializable]
        public class Settings {
            public RenderPassEvent renderPassEvent = RenderPassEvent.AfterRendering;
        }

        public Settings settings = new Settings();

        public class RenderPass : ScriptableRenderPass {
            string profilerTag;

            private UtopiaWorx.Helios.Helios MyHelios;

            private RenderTargetIdentifier source { get; set; }

            public void Setup(RenderTargetIdentifier source) {
                this.source = source;
                this.MyHelios = FindObjectOfType<UtopiaWorx.Helios.Helios>();
            }

            public RenderPass(string profilerTag) {
                this.profilerTag = profilerTag;
            }

            public override void Configure(CommandBuffer cmd, RenderTextureDescriptor cameraTextureDescriptor) {}

            public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData) {
                CommandBuffer cmd = CommandBufferPool.Get(profilerTag);

                if (UnityEditor.EditorApplication.isPlaying) {
                    RenderTexture sourceTexture = new RenderTexture(
                        renderingData.cameraData.camera.pixelWidth,
                        renderingData.cameraData.camera.pixelHeight,
                        32
                        );
                    RenderTexture destinationTexture = new RenderTexture(sourceTexture);

                    cmd.Blit(this.source, sourceTexture);

                    context.ExecuteCommandBuffer(cmd);

                    this.MyHelios.OnRenderImage(sourceTexture, destinationTexture);
                } else {
                    context.ExecuteCommandBuffer(cmd);
                }

                cmd.Clear();

                CommandBufferPool.Release(cmd);
            }

            public override void FrameCleanup(CommandBuffer cmd) {
            }
        }

        RenderPass scriptablePass;

        public override void Create() {
            this.scriptablePass = new RenderPass("Helios");

            this.scriptablePass.renderPassEvent = settings.renderPassEvent;
        }

        public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData) {
            var src = renderer.cameraColorTarget;
            this.scriptablePass.Setup(src);
            renderer.EnqueuePass(this.scriptablePass);
        }
    }
}