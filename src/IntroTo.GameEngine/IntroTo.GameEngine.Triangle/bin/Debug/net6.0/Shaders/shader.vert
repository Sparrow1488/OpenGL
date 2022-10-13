#version 330 core
layout (location = 5) in vec3 aPosition;

uniform mat4 Rotation;

void main()
{
    gl_Position = Rotation * vec4(aPosition, 1.0);
}