#version 440 core

layout (location = 0) in vec3 VertexPosition;
layout (location = 1) in vec3 VertexColor;

uniform mat4 RotationMatrix;
uniform double Time;

out vec3 Color;

void main()
{
	float changeColorSpeed = 0.8;
	Color = vec3(VertexColor.x, sin(float(Time) * changeColorSpeed), VertexColor.z);
	gl_Position = vec4(VertexPosition, 1.0) * RotationMatrix;
}