#version 330 core

out vec4 FragColor;

uniform vec4 ColorId;

void main() 
{
	FragColor = ColorId;
}