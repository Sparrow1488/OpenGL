﻿#version 440 core

layout (location = 0) out vec4 FragColor;

in vec3 TexCoord;

layout (binding = 0) uniform BlobSettings {
	vec4 InnerColor;
	vec4 OuterColor;
	float RadiusInner;
	float RadiusOuter;
};

void main()
{
	float dx = TexCoord.x - 0.5;
	float dy = TexCoord.y - 0.5;
	float dist = sqrt(dx * dx + dy * dy);
	FragColor = mix(InnerColor, OuterColor, smoothstep(RadiusInner, RadiusOuter, dist));
}