Shader "Custom/Glow2DShaderWithAlphaCut"
{
    Properties
    {
        _MainTex ("Base Texture", 2D) = "white" { }
        _GlowColor ("Glow Color", Color) = (1, 0, 1, 1)  // Default to pinkish glow
        _GlowIntensity ("Glow Intensity", Range(0, 10)) = 2.0  // Glow strength
        _Cutoff ("Alpha Cutoff", Range(0, 1)) = 0.1  // Cutoff for transparency
    }
    
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        Pass
        {
            // Enable alpha testing to cut off transparent pixels
            Tags { "Queue"="Overlay" }
            ZWrite On
            ZTest LEqual
            Blend SrcAlpha OneMinusSrcAlpha
            Cull Front
            AlphaTest Greater [_Cutoff]

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float4 pos : POSITION;
                float2 uv : TEXCOORD0;
            };

            sampler2D _MainTex;
            float4 _GlowColor;
            float _GlowIntensity;
            float _Cutoff;

            v2f vert(appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            half4 frag(v2f i) : COLOR
            {
                // Get the base texture color
                half4 texColor = tex2D(_MainTex, i.uv);
                
                // If alpha is below the cutoff, discard the pixel
                if (texColor.a < _Cutoff)
                    discard;
                
                // Add glow effect: multiply the glow color and intensity based on the alpha value of the texture
                half4 glow = _GlowColor * _GlowIntensity * texColor.a;

                // Combine the texture color with the glow effect
                return texColor + glow;
            }
            ENDCG
        }
    }

    FallBack "Unlit/Texture"
}
