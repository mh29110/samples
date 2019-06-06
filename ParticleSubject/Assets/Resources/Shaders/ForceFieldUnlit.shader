Shader "Phantom/ForceFieldUnlit"
{
	Properties
	{
		_MainTex("Texture", 2D) = "white" {}

		_ForceFieldRadius("Force Field Radius", Float) = 4.0
		_ForceFieldPosition("Force Field Position", Vector) = (0.0, 0.0, 0.0, 0.0)

		[HDR] _ColourA("Color A", Color) = (0.0, 0.0, 0.0, 0.0)
		[HDR] _ColourB("Color B", Color) = (1.0, 1.0, 1.0, 1.0)
	}
		SubShader
		{
			Tags { "Queue" = "Transparent" "RenderType" = "Opaque" }
			LOD 100

			Blend One One // 加法混合

			ZWrite Off //关闭深度测试
			Pass
			{
				CGPROGRAM
				#pragma vertex vert
				#pragma fragment frag
				//实现模糊效果
				#pragma multi_compile_fog
				#include "UnityCG.cginc"

				struct appdata
				{
					float4 vertex : POSITION;
					fixed4 color : COLOR;
					float4 tc0 : TEXCOORD0;
					float4 tc1 : TEXCOORD1;
				};

				struct v2f
				{
					float4 tc0 : TEXCOORD0;
					float4 tc1 : TEXCOORD1;
					UNITY_FOG_COORDS(1)
					float4 vertex : SV_POSITION;
					fixed4 color : COLOR;
				};
				sampler2D _MainTex;
				float4 _MainTex_ST;

				float _ForceFieldRadius;
				float3 _ForceFieldPosition;

				float4 _ColourA;
				float4 _ColourB;

				float4 GetParticleOffset(float3 particleCenter)
				{
					/*float distanceToParticle = distance(particleCenter, _ForceFieldPosition);
					if (distanceToParticle < _ForceFieldRadius)
					{
						float distanceToForceFieldRadius = _ForceFieldRadius - distanceToParticle;
						float3 directionToParticle = normalize(particleCenter - _ForceFieldPosition);
						return float4(directionToParticle,1.0f) * distanceToForceFieldRadius;
					}
					return 0;*/

					//单个粒子中心点与立场中心点的距离
					float distanceToParticle = distance(particleCenter, _ForceFieldPosition);
					float forceFieldRadiusAbs = abs(_ForceFieldRadius);

					//从立场中心指向粒子的向量。
					float3 directionToParticle = normalize(particleCenter - _ForceFieldPosition);

					float distanceToForceFieldRadius = forceFieldRadiusAbs - distanceToParticle;
					distanceToForceFieldRadius = max(distanceToForceFieldRadius, 0.0);//负数转0则超出控制范围

					distanceToForceFieldRadius *= sign(_ForceFieldRadius);//sign函数  转-1或1；
					float4 particleOffset;

					particleOffset.xyz = directionToParticle * distanceToForceFieldRadius;
					particleOffset.w = distanceToForceFieldRadius / (_ForceFieldRadius + 0.0001); //添加小数来避免除数为0，以及在r=0.0时出现未定义的颜色或行为。

					return particleOffset;
				}

				v2f vert(appdata v)
				{
					v2f o;

					float3 particleCenter = float3(v.tc0.zw, v.tc1.x);
					float3 vertexOffset = GetParticleOffset(particleCenter);
					v.vertex.xyz += vertexOffset;
					o.vertex = UnityObjectToClipPos(v.vertex);

					// 从保存在颜色顶点输入的粒子系统接收数据，并将该数据用于初始化颜色。
					o.color = v.color;

					o.tc0.xy = TRANSFORM_TEX(v.tc0, _MainTex);
					//初始化tex coord变量
					o.tc0.zw = v.tc0.zw;
					o.tc1 = v.tc1;

					UNITY_TRANSFER_FOG(o,o.vertex);
					return o;
				}

				fixed4 frag(v2f i) : SV_Target
				{
					//采样纹理
					fixed4 col = tex2D(_MainTex, i.tc0);

				// 让纹理颜色和粒子系统的顶点颜色输入相乘
				col *= i.color;

				float3 particleCenter = float3(i.tc0.zw, i.tc1.x);
				float particleOffsetNormalizedLength = GetParticleOffset(particleCenter).w;

				col = lerp(col * _ColourA, col * _ColourB, particleOffsetNormalizedLength);
				col *= col.a;
				// 应用模糊效果
				UNITY_APPLY_FOG(i.fogCoord, col);
				return col;
				}
			ENDCG
		}
		}
}