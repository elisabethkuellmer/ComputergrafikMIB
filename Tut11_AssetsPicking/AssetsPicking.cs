using System;
using System.Collections.Generic;
using System.Linq;
using Fusee.Base.Common;
using Fusee.Base.Core;
using Fusee.Engine.Common;
using Fusee.Engine.Core;
using Fusee.Math.Core;
using Fusee.Serialization;
using Fusee.Xene;
using static System.Math;
using static Fusee.Engine.Core.Input;
using static Fusee.Engine.Core.Time;

namespace Fusee.Tutorial.Core
{
    public class AssetsPicking : RenderCanvas
    {
        private SceneContainer _scene;
        private SceneRenderer _sceneRenderer;
        private ScenePicker _scenePicker;
        private PickResult _currentPick;
        private float3 _oldColor;
        private float _direction;
        private TransformComponent _baseTransform;
        private TransformComponent _rad1Transform;
        private TransformComponent _rad2Transform;
        private TransformComponent _rad3Transform;
        private TransformComponent _rad4Transform;
        private TransformComponent _griffTransform;
        private float _camAngle = 0;
        private float _camSpeed = 0;
        

        SceneContainer CreateScene()
        {
            // Initialize transform components that need to be changed inside "RenderAFrame"
            _baseTransform = new TransformComponent
            {
                Rotation = new float3(0, 0, 0),
                Scale = new float3(1, 1, 1),
                Translation = new float3(0, 0, 0)
            };

            // Setup the scene graph
            return new SceneContainer
            {
                Children = new List<SceneNodeContainer>
                {
                    new SceneNodeContainer
                    {
                        Components = new List<SceneComponentContainer>
                        {
                            // TRANSFROM COMPONENT
                            _baseTransform,

                            // SHADER EFFECT COMPONENT
                            new ShaderEffectComponent
                            {
                                Effect = SimpleMeshes.MakeShaderEffect(new float3(0.7f, 0.7f, 0.7f), new float3(1, 1, 1), 5)
                            },

                            // MESH COMPONENT
                            // SimpleAssetsPickinges.CreateCuboid(new float3(10, 10, 10))
                            SimpleMeshes.CreateCuboid(new float3(10, 10, 10))
                        }
                    },
                }
            };
        }

        // Init is called on startup. 
        public override void Init()
        {
            // Set the clear color for the backbuffer to white (100% intensity in all color channels R, G, B, A).
            RC.ClearColor = new float4(0.8f, 0.9f, 0.7f, 1);

            _scene = AssetStorage.Get<SceneContainer>("Car1.fus");

   

            // Create a scene renderer holding the scene above
            _sceneRenderer = new SceneRenderer(_scene);
            _scenePicker = new ScenePicker(_scene);

        }

        // RenderAFrame is called once a frame
        public override void RenderAFrame()
        {
           // _baseTransform.Rotation = new float3(0, M.MinAngle(TimeSinceStart), 0);

            _rad1Transform = _scene.Children.FindNodes(node => node.Name == "Rad1")?.FirstOrDefault()?.GetTransform();
            float rad1Rotation = _rad1Transform.Rotation.y;
            rad1Rotation += 2* Keyboard.UpDownAxis * DeltaTime;
            _rad1Transform.Rotation = new float3( 0,rad1Rotation, 0);

            _rad2Transform = _scene.Children.FindNodes(node => node.Name == "Rad2")?.FirstOrDefault()?.GetTransform();
             float rad2Rotation = _rad2Transform.Rotation.y;
            rad2Rotation += 2* Keyboard.UpDownAxis * DeltaTime;
            _rad2Transform.Rotation = new float3( 0,rad2Rotation, 0);

            _rad3Transform = _scene.Children.FindNodes(node => node.Name == "Rad3")?.FirstOrDefault()?.GetTransform();
             float rad3Rotation = _rad3Transform.Rotation.y;
            rad3Rotation += 2* Keyboard.UpDownAxis * DeltaTime;
            _rad3Transform.Rotation = new float3( 0,rad3Rotation, 0);

            _rad4Transform = _scene.Children.FindNodes(node => node.Name == "Rad4")?.FirstOrDefault()?.GetTransform();
             float rad4Rotation = _rad4Transform.Rotation.y;
            rad4Rotation += 2* Keyboard.UpDownAxis * DeltaTime;
            _rad4Transform.Rotation = new float3( 0,rad4Rotation, 0);

            _griffTransform = _scene.Children.FindNodes(node => node.Name == "Griff")?.FirstOrDefault()?.GetTransform();
             float griffRotation = _griffTransform.Rotation.y;
            griffRotation += 0.1f* Keyboard.UpDownAxis;
        
            if( griffRotation > 1.5f) {
                griffRotation = 1.5f;
            }
            if( griffRotation < 0) {
                griffRotation = 0;
            }
 
            _griffTransform.Rotation = new float3(0, griffRotation,0);

            // Setup the camera 
            RC.View = float4x4.CreateTranslation(0, -1, 10) * float4x4.CreateRotationX(-(float) Atan(15.0 / 40.0)) * float4x4.CreateRotationY(_camAngle);

            if( Mouse.RightButton){
                _camSpeed = Mouse.Velocity.x;
            }
            else {
                _camSpeed = _camSpeed * 0.9f;
            }

            _camAngle = _camAngle + 0.005f * _camSpeed * Time.DeltaTime;

            
            if (Mouse.LeftButton)
            {
                float2 pickPosClip = Mouse.Position * new float2(2.0f / Width, -2.0f / Height) + new float2(-1, 1);
                _scenePicker.View = RC.View;
                _scenePicker.Projection = RC.Projection;
                List<PickResult> pickResults = _scenePicker.Pick(pickPosClip).ToList();
                
                PickResult newPick = null;
                if (pickResults.Count > 0)
                {
                    pickResults.Sort((a, b) => Sign(a.ClipPos.z - b.ClipPos.z));
                    newPick = pickResults[0];
                }
                if (newPick?.Node != _currentPick?.Node)
                {
                    if (_currentPick != null)
                    {
                        _currentPick.Node.GetComponent<ShaderEffectComponent>().Effect.SetEffectParam("DiffuseColor",_oldColor);
                    }
                    if (newPick != null)
                    { 
                        _oldColor = (float3) newPick.Node.GetComponent<ShaderEffectComponent>().Effect.GetEffectParam("DiffuseColor");
                        newPick.Node.GetComponent<ShaderEffectComponent>().Effect.SetEffectParam("DiffuseColor", new float3(_oldColor.r*0.5f,_oldColor.g*0.5f,_oldColor.b*0.5f));

                    }
                    _currentPick = newPick;
                }
                

            }

            // Clear the backbuffer
            RC.Clear(ClearFlags.Color | ClearFlags.Depth);

            // Setup the camera 
            //RC.View = float4x4.CreateTranslation(0, 0, 40) * float4x4.CreateRotationX(-(float) Atan(15.0 / 40.0));

            if (Mouse.LeftButton)
             {
                float2 pickPosClip = Mouse.Position * new float2(2.0f / Width, -2.0f / Height) + new float2(-1, 1);
                _scenePicker.View = RC.View;
                _scenePicker.Projection = RC.Projection;
                List<PickResult> pickResults = _scenePicker.Pick(pickPosClip).ToList();

                if (pickResults.Count > 0)
                {
                    pickResults.Sort((a, b) => Sign(a.ClipPos.z - b.ClipPos.z));
                    Diagnostics.Log(pickResults[0].Node.Name);
                }
            }

            // Render the scene on the current render context
            _sceneRenderer.Render(RC);

            // Swap buffers: Show the contents of the backbuffer (containing the currently rendered frame) on the front buffer.
            Present();
        }


        // Is called when the window was resized
        public override void Resize()
        {
            // Set the new rendering area to the entire new windows size
            RC.Viewport(0, 0, Width, Height);

            // Create a new projection matrix generating undistorted images on the new aspect ratio.
            var aspectRatio = Width / (float)Height;

            // 0.25*PI Rad -> 45� Opening angle along the vertical direction. Horizontal opening angle is calculated based on the aspect ratio
            // Front clipping happens at 1 (Objects nearer than 1 world unit get clipped)
            // Back clipping happens at 2000 (Anything further away from the camera than 2000 world units gets clipped, polygons will be cut)
            var projection = float4x4.CreatePerspectiveFieldOfView(M.PiOver4, aspectRatio, 1, 20000);
            RC.Projection = projection;
        }
    }
}
