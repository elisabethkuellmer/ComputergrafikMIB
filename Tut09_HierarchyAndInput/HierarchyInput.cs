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
    public class HierarchyInput : RenderCanvas
    {

        private SceneContainer _scene;
        private SceneRenderer _sceneRenderer;
        private float _camAngle = 0;
        private float _camSpeed = 0;
        private TransformComponent _baseTransform;
        private TransformComponent _bodyTransform;
        private TransformComponent _upperArmTransform;
        private TransformComponent _foreArmTransform;
        private TransformComponent _RightHandTransform;
        private TransformComponent _LeftHandTransform;




        SceneContainer CreateScene()
        {
            // Initialize transform components that need to be changed inside "RenderAFrame"
            _baseTransform = new TransformComponent
            {
                Rotation = new float3(0, 0, 0),
                Scale = new float3(1, 1, 1),
                Translation = new float3(0, 0, 0)
            };

            _bodyTransform = new TransformComponent
            {
                Rotation = new float3(0,0,0),
                Scale = new float3(1,1,1),
                Translation = new float3(0,6,0)
            };

            _upperArmTransform = new TransformComponent
            {
                Rotation = new float3(0,0,0),
                Scale = new float3(1,1,1),
                Translation = new float3(2,4,0)
            };

            _foreArmTransform = new TransformComponent
            {
                Rotation = new float3(0,0,0),
                Scale = new float3(1,1,1),
                Translation = new float3(-2,4,0)
            };
            _RightHandTransform = new TransformComponent
            {
                Rotation = new float3(0,0,0),
                Scale = new float3(1,1,1),
                Translation = new float3(0.5f,4.5f,0)
            };
            _LeftHandTransform = new TransformComponent
            {
                Rotation = new float3(0,0,0),
                Scale = new float3(1,1,1),
                Translation = new float3(-0.5f,4.5f,0)
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
                                Effect = SimpleMeshes.MakeShaderEffect(new float3(0.7f, 0.7f, 0.7f), new float3(0.7f, 0.7f, 0.7f), 5)
                               //Diffuse = new MatChannelContainer {Color = new float3(0.7f,0.7f,0.7f)},
                               //Specular = new SpecularChannelContainer {Color = new float3(0,0,0), Shininess = 5}
                            },

                            // MESH COMPONENT
                            SimpleMeshes.CreateCuboid(new float3(10, 2, 10))
                        }
                    },
                    //Red Body
                    new SceneNodeContainer 
                    {
                        Components = new List<SceneComponentContainer>
                        {
                            _bodyTransform,
                            new ShaderEffectComponent
                            {
                                Effect = SimpleMeshes.MakeShaderEffect(new float3(1f, 0f, 0f), new float3(0.7f, 0.7f, 0.7f), 5)
                               // Diffuse = new MatChannelContainer {Color = new float3(1,0,0)},
                               // Specular = new SpecularChannelContainer {Color = new float3(1,1,1), Shininess = 5}
                            },
                            SimpleMeshes.CreateCuboid(new float3(2,10,2))
                        },
                        Children = new List<SceneNodeContainer>
                        {
                            //Green Upperarm
                            new SceneNodeContainer
                            {
                                Components = new List<SceneComponentContainer>
                                {
                                _upperArmTransform 
                                },
                                Children = new List<SceneNodeContainer>
                                {
                                    new SceneNodeContainer
                                    {
                                        Components = new List<SceneComponentContainer>
                                        {
                                            new TransformComponent
                                            {
                                                Rotation = new float3(0,0,0),
                                                Scale = new float3(1,1,1),
                                                Translation = new float3(0,4,0)
                                            },
                                            new ShaderEffectComponent
                                            {
                                                Effect = SimpleMeshes.MakeShaderEffect(new float3(0,1,0), new float3(0.7f,0.7f,0.7f),5)
                                            },
                                            SimpleMeshes.CreateCuboid(new float3(2,10,2))
                                        },
                                    Children = new List<SceneNodeContainer>
                                        {
                                        //Blue Forearm
                                        new SceneNodeContainer
                                            {
                                            Components = new List<SceneComponentContainer>
                                                {
                                                    _foreArmTransform
                                                },
                                            Children = new List<SceneNodeContainer>
                                                {
                                                    new SceneNodeContainer
                                                    {
                                                        Components = new List<SceneComponentContainer>
                                                        {
                                                            new TransformComponent
                                                            {
                                                                Rotation = new float3(0,0,0),
                                                                Scale = new float3(1,1,1),
                                                                Translation = new float3(0,4,0)
                                                            },
                                                            new ShaderEffectComponent
                                                            {
                                                                Effect = SimpleMeshes.MakeShaderEffect(new float3(0,0,1), new float3(0.7f,0.7f,0.7f),5)
                                                            },
                                                            SimpleMeshes.CreateCuboid(new float3(2,10,2))   
                                                         },
                                                    Children = new List<SceneNodeContainer>
                                                    {
                                                        //RightHand
                                                        new SceneNodeContainer
                                                            {
                                                                Components = new List<SceneComponentContainer>
                                                                {
                                                                     _RightHandTransform
                                                                },
                                                                 
                                                                Children = new List<SceneNodeContainer>
                                                                {
                                                                     new SceneNodeContainer
                                                                        {
                                                                            Components = new List<SceneComponentContainer>
                                                                            {
                                                                                new TransformComponent
                                                                                {
                                                                                    Rotation = new float3(0,0,0),
                                                                                    Scale = new float3(1,1,1),
                                                                                    Translation = new float3(0,2.5f,0)
                                                                                },
                                                        
                                                                                new ShaderEffectComponent
                                                                                {
                                                                                    Effect = SimpleMeshes.MakeShaderEffect(new float3(1,1,0), new float3(0.7f,0.7f,0.7f),5)
                                                                                },
                                                                                 SimpleMeshes.CreateCuboid(new float3(1,5,2))
                                                                             }
                                                                         }
                                                                    }
                                                                },
                                                                     //LeftHand
                                                                        new SceneNodeContainer
                                                                            {
                                                                                Components = new List<SceneComponentContainer>
                                                                                {
                                                                                     _LeftHandTransform
                                                                                 },
                                                                                 Children = new List<SceneNodeContainer>
                                                                                {
                                                                                    new SceneNodeContainer
                                                                                    {
                                                                                        Components = new List<SceneComponentContainer>
                                                                                        {
                                                                                             new TransformComponent
                                                                                            {
                                                                                                Rotation = new float3(0,0,0),
                                                                                                Scale = new float3(1,1,1),
                                                                                                Translation = new float3(0,2.5f,0)
                                                                                            },
                                                                
                                                                                            new ShaderEffectComponent
                                                                                            {
                                                                                                Effect = SimpleMeshes.MakeShaderEffect(new float3(1,1,0), new float3(0.7f,0.7f,0.7f),5)
                                                                                            },
                                                                                        SimpleMeshes.CreateCuboid(new float3(1,5,2))
                                                                                        }
                                                                                    }
                                                                                }
                                                                            }
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                             };
                         }

        // Init is called on startup. 
        public override void Init()
        {
            // Set the clear color for the backbuffer to white (100% intensity in all color channels R, G, B, A).
            RC.ClearColor = new float4(0.8f, 0.9f, 0.7f, 1);

            _scene = CreateScene();

            // Create a scene renderer holding the scene above
            _sceneRenderer = new SceneRenderer(_scene);
        }

        // RenderAFrame is called once a frame
        public override void RenderAFrame()
        {
              // Clear the backbuffer
            RC.Clear(ClearFlags.Color | ClearFlags.Depth);

            // Setup the camera 
            RC.View = float4x4.CreateTranslation(0, -10, 50) * float4x4.CreateRotationY(_camAngle);

            //Bodyrotation
            float bodyrot = _bodyTransform.Rotation.y;
            bodyrot += 0.1f*Keyboard.LeftRightAxis;
            _bodyTransform.Rotation = new float3(0,bodyrot,0);

            float upperarmchange =_upperArmTransform.Rotation.x;
            upperarmchange += 0.1f*Keyboard.UpDownAxis;
            _upperArmTransform.Rotation = new float3(upperarmchange,0,0);

            float foreArmchange = _foreArmTransform.Rotation.x;
            foreArmchange += 0.1f*Keyboard.WSAxis;
            _foreArmTransform.Rotation = new float3(foreArmchange,0,0);

           if( Mouse.LeftButton){
                _camSpeed = Mouse.Velocity.x;
            }
            else {
                _camSpeed = _camSpeed * 0.9f;
            }

            _camAngle = _camAngle + 0.005f * _camSpeed * Time.DeltaTime;
            
            float left_hand_rotation_z = _LeftHandTransform.Rotation.z;  
            left_hand_rotation_z += 2f * Keyboard.ADAxis * Time.DeltaTime;
            if( left_hand_rotation_z > 1.5f) {
                left_hand_rotation_z = 1.5f;
            }
            if( left_hand_rotation_z < 0) {
                left_hand_rotation_z = 0;
            }
            _LeftHandTransform.Rotation = new float3(0, 0, left_hand_rotation_z);
            
            float right_hand_rotation_z = _RightHandTransform.Rotation.z;  
            right_hand_rotation_z -= 2f * Keyboard.ADAxis * Time.DeltaTime;
            if( right_hand_rotation_z < -1.5f) {
                right_hand_rotation_z = -1.5f;
            }
            if( right_hand_rotation_z > 0) {
                right_hand_rotation_z = 0;
            }
            _RightHandTransform.Rotation = new float3(0, 0, right_hand_rotation_z);




            // Render the scene on the current render context
            _sceneRenderer.Render(RC);

            // Swap buffers: Show the contents of the backbuffer (containing the currently rendered farame) on the front buffer.
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