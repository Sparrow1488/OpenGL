#version 330 core

in vec2 aTexture;
out vec4 FragColor;

uniform sampler2D texture1;
uniform sampler2D texture2;

void main() 
{
	FragColor = mix(texture(texture1, aTexture), texture(texture2, aTexture), 0.6);
}