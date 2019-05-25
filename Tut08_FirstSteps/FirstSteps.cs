//https://sftp.hs-furtwangen.de/~kuellmer/CG/Aufgabe08/Web/Tut08_FirstSteps.html

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
    public class FirstSteps : RenderCanvas
    {

        private SceneContainer _scene;
        private SceneRenderer _scenerenderer;
        private float _camAngle = 0;

        private ShaderComponent cubeShader2;
        private TransformComponent _cubeTransform;
        private TransformComponent _cubeTransform1;
        private TransformComponent _cubeTransform2;
     
        // Init is called on startup. 
        public override void Init()
        {
            // Set the clear color for the backbuffer to light green (intensities in R, G, B, A).
            RC.ClearColor = new float4(0.5f, 0.5f, 0.8f, 1.0f);

            //Creat a Scene with a Cube
            //The three components: one XForm, one Shader and the Mesh
            _cubeTransform =  new TransformComponent{Scale = new float3(1,1,1), Translation= new float3(0,0,0)};
            var cubeShader = new ShaderEffectComponent
            {
                Effect =  SimpleMeshes.MakeShaderEffect(new float3 (1,1,0), new float3 (1,1,1), 4)
            };
            var cubeMesh = SimpleMeshes.CreateCuboid(new float3(10,10,10));

            //___________
            _cubeTransform1 =  new TransformComponent{Scale = new float3(1,1,1), Translation= new float3(-30,0,50)};
            var cubeShader1 = new ShaderEffectComponent
            {
                Effect =  SimpleMeshes.MakeShaderEffect(new float3 (0,1,0), new float3 (1,1,1), 4)
            };
            var cubeMesh1 = SimpleMeshes.CreateCuboid(new float3(10,10,10));

            //___________
            _cubeTransform2 =  new TransformComponent{Scale = new float3(1,1,1), Translation= new float3(30,0,50)};
            var cubeShader2 = new ShaderEffectComponent
            {
                Effect =  SimpleMeshes.MakeShaderEffect(new float3 (0,0,1), new float3 (1,1,1), 4)
            };
            var cubeMesh2 = SimpleMeshes.CreateCuboid(new float3(10,10,10));



            //Assemble the cube node containing in the three components
            var cubeNode = new SceneNodeContainer();
            cubeNode.Components = new List<SceneComponentContainer>();
            cubeNode.Components.Add(_cubeTransform);
            cubeNode.Components.Add(cubeShader);
            cubeNode.Components.Add(cubeMesh);

            var cubeNode1 = new SceneNodeContainer();
            cubeNode1.Components = new List<SceneComponentContainer>();
            cubeNode1.Components.Add(_cubeTransform1);
            cubeNode1.Components.Add(cubeShader1);
            cubeNode1.Components.Add(cubeMesh1);

            var cubeNode2 = new SceneNodeContainer();
            cubeNode2.Components = new List<SceneComponentContainer>();
            cubeNode2.Components.Add(_cubeTransform2);
            cubeNode2.Components.Add(cubeShader2);
            cubeNode2.Components.Add(cubeMesh2);

            //Create the Scene containing the cube als the only object
            _scene = new SceneContainer();
            _scene.Children = new List<SceneNodeContainer>();
            _scene.Children.Add(cubeNode);
            _scene.Children.Add(cubeNode1);
            _scene.Children.Add(cubeNode2);

            //Create a scene renderer holding the scene above
            _scenerenderer = new SceneRenderer(_scene);
        }

        

        // RenderAFrame is called once a frame
        public override void RenderAFrame()
        {
            // Clear the backbuffer
            RC.Clear(ClearFlags.Color | ClearFlags.Depth);

             //Animation
            // _camAngle = _camAngle + 90.0f*M.Pi/180.0f*DeltaTime;
             _cubeTransform.Translation = new float3(1,M.Sin(3*TimeSinceStart),0);
             _cubeTransform.Rotation = new float3(0,6,0);
            // _cubeTransform.Scale = new float3(M.Sin(5*TimeSinceStart),1,1);
            _cubeTransform1.Rotation = new float3(-1, 45.0f *M.Pi/180.0f * DeltaTime,-1);
            _cubeTransform2.Rotation = new float3(1,1,M.Cos(4*TimeSinceStart));
            _cubeTransform2.Translation = new float3(20,0,0);
            _cubeTransform1.Translation = new float3(x: -20+M.Sin(3*TimeSinceStart),1,M.Sin(6*TimeSinceStart));
          
            //Setup Camera
            RC.View = float4x4.CreateTranslation(0,0,50)*float4x4.CreateRotationY(_camAngle);

            //Render the scene on the current context
            _scenerenderer.Render(RC);

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

            // 0.25*PI Rad -> 45° Opening angle along the vertical direction. Horizontal opening angle is calculated based on the aspect ratio
            // Front clipping happens at 1 (Objects nearer than 1 world unit get clipped)
            // Back clipping happens at 2000 (Anything further away from the camera than 2000 world units gets clipped, polygons will be cut)
            var projection = float4x4.CreatePerspectiveFieldOfView(M.PiOver4, aspectRatio, 1, 20000);
            RC.Projection = projection;
        }
    }
}
