Shader "Custom/GaussianBlur"
{
	Properties
	{
		_MainTex("_MainTex", 2D) = "white" {}
		_blurAmount ("blurAmount", Float) = 0.0075
	}
		SubShader
	{

		GrabPass{}

		//Horizontal blur pass.
		Pass
		{
			Blend SrcAlpha OneMinusSrcAlpha
			name "HorizontalBlur"

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			#include "UnityCG.cginc"

			sampler2D _GrabTexture : register(s0);
			uniform float4 _GrabTexture_TexelSize;
			float _blurAmount;
			struct v2f
			{
				float4 vertex : SV_POSITION;
				float2 uv : TEXCOORD0;
			};

			float4 _GrabTexture_ST;
			
			v2f vert (appdata_base v)
			{
				v2f o;
				o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
				o.uv = TRANSFORM_TEX(v.texcoord, _GrabTexture);
#if UNITY_UV_STARTS_AT_TOP
				if (_GrabTexture_TexelSize.y < 0)
					o.uv.y = 1 - o.uv.y;
#endif
				return o;
			}
			
			half4 frag (v2f i) : COLOR
			{
				float blurAmount = _blurAmount;

				half4 sum = half4(0.0,0.0,0.0,0.0);

				sum += tex2D(_GrabTexture, float2(i.uv.x - 5.0 * blurAmount, i.uv.y)) * 0.025;
				sum += tex2D(_GrabTexture, float2(i.uv.x - 4.0 * blurAmount, i.uv.y)) * 0.05;
				sum += tex2D(_GrabTexture, float2(i.uv.x - 3.0 * blurAmount, i.uv.y)) * 0.09;
				sum += tex2D(_GrabTexture, float2(i.uv.x - 2.0 * blurAmount, i.uv.y)) * 0.12;
				sum += tex2D(_GrabTexture, float2(i.uv.x - blurAmount, i.uv.y)) * 0.15;
				sum += tex2D(_GrabTexture, float2(i.uv.x, i.uv.y)) * 0.16;
				sum += tex2D(_GrabTexture, float2(i.uv.x + blurAmount, i.uv.y)) * 0.15;
				sum += tex2D(_GrabTexture, float2(i.uv.x + 2.0 * blurAmount, i.uv.y)) * 0.12;
				sum += tex2D(_GrabTexture, float2(i.uv.x + 3.0 * blurAmount, i.uv.y)) * 0.09;
				sum += tex2D(_GrabTexture, float2(i.uv.x + 4.0 * blurAmount, i.uv.y)) * 0.05;
				sum += tex2D(_GrabTexture, float2(i.uv.x + 5.0 * blurAmount, i.uv.y)) * 0.025;
				
				//sum = sum / 11;

				return float4(sum.rgb, 1);
			}
				ENDCG
	}


				// Vertical blur pass
				Pass
			{
				Blend SrcAlpha OneMinusSrcAlpha
				Name "VerticalBlur"

				CGPROGRAM
				#pragma vertex vert
				#pragma fragment frag            
				#include "UnityCG.cginc"

				sampler2D _GrabTexture : register(s0);
				uniform float4 _GrabTexture_TexelSize;
				float _blurAmount;

			struct v2f
			{
				float4  pos : SV_POSITION;
				float2  uv : TEXCOORD0;
			};

			float4 _GrabTexture_ST;

			v2f vert(appdata_base v)
			{
				v2f o;
				o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
				o.uv = TRANSFORM_TEX(v.texcoord, _GrabTexture);
				
#if UNITY_UV_STARTS_AT_TOP
				if (_GrabTexture_TexelSize.y < 0)
					o.uv.y = 1 - o.uv.y;
#endif

				return o;
			}

			half4 frag(v2f i) : COLOR
			{
				float blurAmount = _blurAmount;

			half4 sum = half4(0.0,0.0,0.0,0.0);

			sum += tex2D(_GrabTexture, float2(i.uv.x, i.uv.y - 5.0 * blurAmount)) * 0.025;
			sum += tex2D(_GrabTexture, float2(i.uv.x, i.uv.y - 4.0 * blurAmount)) * 0.05;
			sum += tex2D(_GrabTexture, float2(i.uv.x, i.uv.y - 3.0 * blurAmount)) * 0.09;
			sum += tex2D(_GrabTexture, float2(i.uv.x, i.uv.y - 2.0 * blurAmount)) * 0.12;
			sum += tex2D(_GrabTexture, float2(i.uv.x, i.uv.y - blurAmount)) * 0.15;
			sum += tex2D(_GrabTexture, float2(i.uv.x, i.uv.y)) * 0.16;
			sum += tex2D(_GrabTexture, float2(i.uv.x, i.uv.y + blurAmount)) * 0.15;
			sum += tex2D(_GrabTexture, float2(i.uv.x, i.uv.y + 2.0 * blurAmount)) * 0.12;
			sum += tex2D(_GrabTexture, float2(i.uv.x, i.uv.y + 3.0 * blurAmount)) * 0.09;
			sum += tex2D(_GrabTexture, float2(i.uv.x, i.uv.y + 4.0 * blurAmount)) * 0.05;
			sum += tex2D(_GrabTexture, float2(i.uv.x, i.uv.y + 5.0 * blurAmount)) * 0.025;

			//sum = sum / 11;

			return float4(sum.rgb, 1);
			}
				ENDCG
			}
	}

	Fallback "VertexLit"
}
