Shader "Custom/PlayerShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Progress ("Progress", Float) = 0
    }
    SubShader
    {
        Tags 
        { 
            "RenderType" = "Transparent" 
            "Queue" = "Transparent" 
        }
        // No culling or depth
        Cull Off ZWrite Off ZTest Off

        Pass
        {
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
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            sampler2D _MainTex;
            fixed4 _Color;
            float _Progress;

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv);
                // apply alpha
                if (col.a < 0.5) {
                    clip(-1.0); // this fragment will not be rendered
                }
                // selecting hair
                if (col.r >= 0.5 && col.g < 0.3 && col.b < 0.3) {
                    fixed r = col.r, g = col.g;
                    col.r = col.r*(1-_Progress) + g*_Progress*2;
                    col.g = col.g*(1-_Progress) + r*_Progress/2;
                    col.b = col.b + 0.5*_Progress;
                }
                return col;
            }
            ENDCG
        }
    }
}
