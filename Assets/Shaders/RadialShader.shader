Shader "UI/CircleReveal"
{
    Properties
    {
        _MainTex   ("Texture", 2D) = "white" {}
        _Progress  ("Progress (0..1)", Range(0,1)) = 0
        _Softness  ("Edge Softness", Range(0.001,0.2)) = 0.06
        _Aspect    ("Aspect (w/h)", Float) = 0.462   // set to screen width/height to keep circle round
        _Tint      ("Tint", Color) = (1,1,1,1)
    }
    SubShader
    {
        Tags { "Queue"="Transparent" "RenderType"="Transparent" }
        ZWrite Off
        Blend SrcAlpha OneMinusSrcAlpha
        Cull Off

        Pass
        {
            HLSLPROGRAM
            #pragma vertex   vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            sampler2D _MainTex; float4 _MainTex_ST;
            float _Progress, _Softness, _Aspect;
            float4 _Tint;

            struct VIn  { float4 vertex:POSITION; float2 uv:TEXCOORD0; };
            struct VOut { float4 pos:SV_POSITION; float2 uv:TEXCOORD0; float2 puv:TEXCOORD1; };

            VOut vert(VIn v){
                VOut o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv  = TRANSFORM_TEX(v.uv, _MainTex);
                float2 p = v.uv * 2 - 1;   // -1..1
                p.x *= _Aspect;           // keep circle round on tall portrait
                o.puv = p;
                return o;
            }

            fixed4 frag(VOut i):SV_Target
            {
                // radius grows with progress; 1.2 goes beyond corners so we fully reveal
                float radius = lerp(0.0, 1.2, _Progress);

                // signed distance to circle edge (negative inside the circle)
                float d = length(i.puv) - radius;

                // soft edge mask (1=visible, 0=hidden)
                float m = saturate(0.5 - d / _Softness);

                fixed4 col = tex2D(_MainTex, i.uv) * _Tint;
                col.a *= m;   // apply circular reveal
                return col;
            }
            ENDHLSL
        }
    }
}