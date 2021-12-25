#version 330 core

in vec4 vertexColor;
out vec4 FragColor; // input from the vertex shader

void main() {
	FragColor = vertexColor;
}