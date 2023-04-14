Shader "Private/Portal Collision Shader"
{
    Properties
    {
        _PortalColor("Portal Color (RGBA)", Color) = (1, 1, 1, 0.1)
    }

    SubShader
    {
        //
        // see https://docs.unity3d.com/2019.3/Documentation/Manual/SL-SubShaderTags.html
        //
        Tags
        {
            // You can determine in which order your objects are drawn using the Queue tag. A Shader
            // decides which render queue its objects belong to, this way any Transparent shaders make sure they are drawn after all opaque objects and so on.
            // There are four pre - defined render queues, but there can be more queues in between the predefined ones.The predefined queues are :
            // Background - this render queue is rendered before any others. You’d typically use this for things that really need to be in the background.
            // Geometry (default) - this is used for most objects. Opaque geometry uses this queue.
            // AlphaTest - alpha tested geometry uses this queue. It’s a separate queue from Geometry one since it’s more efficient to render alpha-tested objects after all solid ones are drawn.
            // Transparent - this render queue is rendered after Geometry and AlphaTest, in back-to-front order. Anything alpha-blended (i.e. shaders that don’t write to depth buffer) should go here (glass, particle effects).
            // Overlay - this render queue is meant for overlay effects. Anything rendered last should go here (e.g. lens flares).
            //
            // For special uses in-between queues can be used.
            // Tags { "Queue" = "Geometry+1" }
            //
            "Queue" = "Transparent"
            //
            // RenderType tag categorizes shaders into several predefined groups, e.g. is is an opaque shader, or an alpha-tested shader etc. 
            // This is used by Shader Replacement and in some cases used to produce camera’s depth texture.
            //
            "RenderType" = "Transparent"
            //
            // If ForceNoShadowCasting tag is given and has a value of “True”, then an object that is rendered using this subshader will never cast shadows. 
            // This is mostly useful when you are using shader replacement on transparent objects and you do not wont to inherit a shadow pass from another subshader.
            //
            "ForceNoShadowCasting" = "True"
            //
            // If IgnoreProjector tag is given and has a value of “True”, then an object that uses this shader will not be affected by Projectors. 
            // This is mostly useful on semitransparent objects, because there is no good way for Projectors to affect them.
            //
            "IgnoreProjector" = "True"
        }

        Pass
        {
            ZWrite Off
            Blend SrcAlpha OneMinusSrcAlpha
            Cull Back            // (Off/Back/Front)
            LOD 100

            CGPROGRAM

            #pragma vertex vert alpha
            #pragma fragment frag alpha

            #include "UnityCG.cginc"

            struct VertexInput
            {
                float4 vertex : POSITION;
            };

            struct VertexOutput
            {
                float4 vertex : SV_POSITION;
            };

            float4 _PortalColor;

            VertexOutput vert(VertexInput i)
            {
                VertexOutput o;

                o.vertex = UnityObjectToClipPos(i.vertex);

                return o;
            }

            fixed4 frag(VertexOutput i) : SV_Target
            {
                return _PortalColor;
            }

            ENDCG
        }
    }
}