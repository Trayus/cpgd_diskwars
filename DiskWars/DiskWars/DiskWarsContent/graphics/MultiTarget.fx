float screenWidth;
float screenHeight;
float4 ambientColor;

float lightStrength;
float lightDecay;
float3 lightPosition;
float3 lightEndPosition;
float3 coneDirection;
float coneAngle;
float radius;
float minsize;
float maxsize;
int coneHardEdge;
float4 lightColor;
float lightRadius;
float specularStrength;

Texture NormalMap;
sampler NormalMapSampler = sampler_state {
	texture = <NormalMap>;
	magfilter = LINEAR;
	minfilter = LINEAR;
	mipfilter = LINEAR;
	AddressU = mirror;
	AddressV = mirror;
};

Texture ColorMap;
sampler ColorMapSampler = sampler_state {
	texture = <ColorMap>;
	magfilter = LINEAR;
	minfilter = LINEAR;
	mipfilter = LINEAR;
	AddressU = mirror;
	AddressV = mirror;
};

struct VertexToPixel
{
	float4 Position : POSITION;
	float2 TexCoord : TEXCOORD0;
	float4 Color : COLOR0;
};

struct PixelToFrame
{
	float4 Color : COLOR0;
};

VertexToPixel MyVertexShader(float4 inPos: POSITION0, float2 texCoord: TEXCOORD0, float4 color: COLOR0)
{
	VertexToPixel Output = (VertexToPixel)0;
	
	Output.Position = inPos;
	Output.TexCoord = texCoord;
	Output.Color = color;
	
	return Output;
}

PixelToFrame PointLightShader(VertexToPixel PSIn) : COLOR0
{	
	PixelToFrame Output = (PixelToFrame)0;
	
	float4 colorMap = tex2D(ColorMapSampler, PSIn.TexCoord);
	float3 normal = (2.0f * (tex2D(NormalMapSampler, PSIn.TexCoord))) - 1.0f;
		
	float3 pixelPosition;
	pixelPosition.x = screenWidth * PSIn.TexCoord.x;
	pixelPosition.y = screenHeight * PSIn.TexCoord.y;
	pixelPosition.z = 0;

	float3 lightDirection = lightPosition - pixelPosition;
	float3 lightDirNorm = normalize(lightDirection);
	float3 halfVec = float3(0, 0, 1);
		
	float amount = max(dot(normal, lightDirNorm), 0);
	float coneAttenuation = saturate(1.0f - length(lightDirection) / lightDecay); 
				
	float3 reflect = normalize(2 * amount * normal - lightDirNorm);
	float specular = min(pow(saturate(dot(reflect, halfVec)), 10), amount);
				
	Output.Color = colorMap * coneAttenuation * lightColor * lightStrength + (specular * coneAttenuation * specularStrength);

	return Output;
}

PixelToFrame CircleLightShader(VertexToPixel PSIn) : COLOR0
{	
	PixelToFrame Output = (PixelToFrame)0;
	
	float4 colorMap = tex2D(ColorMapSampler, PSIn.TexCoord);
	float3 normal = (2.0f * (tex2D(NormalMapSampler, PSIn.TexCoord))) - 1.0f;
		
	float3 pixelPosition;
	pixelPosition.x = screenWidth * PSIn.TexCoord.x;
	pixelPosition.y = screenHeight * PSIn.TexCoord.y;
	pixelPosition.z = 0;

	float3 lightDirection = lightPosition - pixelPosition;
	float3 lightDirNorm = normalize(lightDirection);
	float3 halfVec = float3(0, 0, 1);
		
	float amount = max(dot(normal, lightDirNorm), 0);
	float coneAttenuation = saturate(1.0f - length(lightDirection) / lightDecay);
	if (length(lightDirection) > maxsize || length(lightDirection) < minsize)
	{
	   coneAttenuation = 0;
	}
	else
	{
	   coneAttenuation = 1.0 - ((abs(length(lightDirection) - ((minsize + maxsize) / 2))) / ((maxsize - minsize) / 2));
	}
				
	float3 reflect = normalize(2 * amount * normal - lightDirNorm);
	float specular = min(pow(saturate(dot(reflect, halfVec)), 10), amount);
				
	Output.Color = colorMap * coneAttenuation * lightColor * lightStrength + (specular * coneAttenuation * specularStrength);

	return Output;
}

PixelToFrame ConeLightShader(VertexToPixel PSIn) : COLOR0
{	
	PixelToFrame Output = (PixelToFrame)0;
	
	float4 colorMap = tex2D(ColorMapSampler, PSIn.TexCoord);
	float3 normal = (2.0f * (tex2D(NormalMapSampler, PSIn.TexCoord))) - 1.0f;
		
	float3 pixelPosition;
	pixelPosition.x = screenWidth * PSIn.TexCoord.x;
	pixelPosition.y = screenHeight * PSIn.TexCoord.y;
	pixelPosition.z = 0;

	float3 lightDirection = lightPosition - pixelPosition;
	float3 lightDirNorm = normalize(lightDirection);
	float3 halfVec = float3(0, 0, 1);
		
	float amount = max(dot(normal, lightDirNorm), 0);
	float coneAttenuation = saturate(1.0f - length(lightDirection) / lightDecay); 
	if (dot(lightDirNorm, normalize(coneDirection)) < coneAngle)
	{
		coneAttenuation = 0;
	}
	else if (coneHardEdge == 1)
	{
		float temp = acos(clamp(coneAngle, 0, 0.9999));
		coneAttenuation *= (1 - (acos(dot(lightDirNorm, normalize(coneDirection))) / temp));
	}
				
	float3 reflect = normalize(2 * amount * normal - lightDirNorm);
	float specular = min(pow(saturate(dot(reflect, halfVec)), 10), amount);
				
	Output.Color = colorMap * coneAttenuation * lightColor * lightStrength + (specular * coneAttenuation * specularStrength);

	return Output;
}

PixelToFrame LineLightShader(VertexToPixel PSIn) : COLOR0
{	
	PixelToFrame Output = (PixelToFrame)0;
	
	float4 colorMap = tex2D(ColorMapSampler, PSIn.TexCoord);
	float3 normal = (2.0f * (tex2D(NormalMapSampler, PSIn.TexCoord))) - 1.0f;
		
	float3 pixelPosition;
	pixelPosition.x = screenWidth * PSIn.TexCoord.x;
	pixelPosition.y = screenHeight * PSIn.TexCoord.y;
	pixelPosition.z = 0;

	float3 lightDirection = lightPosition - pixelPosition;
	float3 lightDirNorm = normalize(lightDirection);
	float3 halfVec = float3(0, 0, 1);
		
	float amount = max(dot(normal, lightDirNorm), 0);
	float coneAttenuation = 1;//saturate(1.0f - length(lightDirection) / lightDecay); 

	//float dx = lightPosition[0] - lightEndPosition[0];
	//float dy = lightPosition[1] - lightEndPosition[1];
	//float hypot = sqrt(dx * dx + dy * dy);
	//float dist = abs((pixelPosition[0] - lightEndPosition[0]) * dy - (pixelPosition[1] - lightEndPosition[1]) * dx) / hypot;
	
	float dist = distance(lightPosition, pixelPosition) + distance(lightEndPosition, pixelPosition);
	float totaldist = distance(lightPosition, lightEndPosition) + radius;
	
	if (dist > totaldist)
	{
		coneAttenuation = 0;
	}
	else if (coneHardEdge == 1)
	{
		coneAttenuation *= ((totaldist - dist) / radius);
	}

				
	float3 reflect = normalize(2 * amount * normal - lightDirNorm);
	float specular = min(pow(saturate(dot(reflect, halfVec)), 10), amount);
				
	Output.Color = colorMap * coneAttenuation * lightColor * lightStrength + (specular * coneAttenuation * specularStrength);

	return Output;
}

technique DeferredPointLight
{
    pass Pass1
    {
		VertexShader = compile vs_2_0 MyVertexShader();
        PixelShader = compile ps_2_0 PointLightShader();
    }
}

technique DeferredConeLight
{
    pass Pass1
    {
		VertexShader = compile vs_2_0 MyVertexShader();
        PixelShader = compile ps_2_0 ConeLightShader();
    }
}

technique DeferredLineLight
{
    pass Pass1
    {
		VertexShader = compile vs_2_0 MyVertexShader();
        PixelShader = compile ps_2_0 LineLightShader();
    }
}

technique DeferredCircleLight
{
    pass Pass1
    {
		VertexShader = compile vs_2_0 MyVertexShader();
        PixelShader = compile ps_2_0 CircleLightShader();
    }
}