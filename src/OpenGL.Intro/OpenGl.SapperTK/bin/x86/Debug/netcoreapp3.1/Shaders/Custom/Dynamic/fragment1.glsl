#version 330 core

out vec4 FragColor; // input from the vertex shader
uniform vec4 ourValue;

void main() {
	FragColor = ourValue;
}