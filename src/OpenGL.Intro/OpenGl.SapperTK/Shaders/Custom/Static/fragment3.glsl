#version 330 core

in vec3 vertexColor;
in vec2 texCoord;

out vec4 FragColor;
out vec2 TexCoord; 

void main() {
	FragColor = vec4(vertexColor, 1.0);
	TexCoord = texCoord;
}