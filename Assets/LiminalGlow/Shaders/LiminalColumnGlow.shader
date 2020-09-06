Shader "Liminal/Columns/Glow"
{
	Properties
	{
		_MainTex("Texture", 2D) = "white" {}
		_Color("Color", Color) = (0.5, 0.5, 0.5, 0.5)
		_ColorCore("Core Color", Color) = (0.5, 0.5, 0.5, 0.5)
		_CoreCutoff("Core Cutoff", Range(0, 1)) = 0.5
		_Strength("Strength", Float) = 1
		_PulseStrength("Pulse Strength", Float) = 0.5
		_PulseRate("Pulse Rate", Float) = 1
		_Offset("Offset", Float) = 0
	}
	SubShader
	{
		Tags
		{
			"Queue" = "Transparent-1"
			"IgnoreProjector" = "True"
			"RenderType" = "Transparent"
		}

		LOD 100
		Blend SrcAlpha One
		ZWrite Off
		Cull Off
		Offset [_Offset], [_Offset]

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma fragmentoption ARB_precision_hint_fastest
			
			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float4 vertex : SV_POSITION;
				float2 uv : TEXCOORD0;
				half time : TEXCOORD1;
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;

			fixed4 _Color;
			fixed4 _ColorCore;
			fixed _CoreCutoff;
			fixed _PulseRate;
			fixed _PulseStrength;
			fixed _Strength;

			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);

				fixed3 worldPos = mul(unity_ObjectToWorld, v.vertex);
				o.time = (sin(_Time.x * _PulseRate + worldPos.x * 0.2) + 1) / 2;
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				fixed4 col = tex2D(_MainTex, i.uv);
				col.rgb *= lerp(_ColorCore, _Color, 1 - pow(saturate(col.r / _CoreCutoff), 2));
				col.a = lerp(col.a * (1 - _PulseStrength), col.a * (1 + _PulseStrength), i.time);
				col *= _Strength;

				return col;
			}
			ENDCG
		}
	}
}
