public struct Triangle {
    public readonly int vertexIndexA, vertexIndexB, vertexIndexC;
    int[] vertices;

    public Triangle(int a, int b, int c) {
        vertexIndexA = a; vertexIndexB = b; vertexIndexC = c;

        vertices = new int[3];
        vertices[0] = a; vertices[1] = b; vertices[2] = c;
    }

    public int this[int i] {
        get => vertices[i];
    }

    public bool Contains(int vertexIndex) {
        return (vertexIndex == vertexIndexA ||
                vertexIndex == vertexIndexB ||
                vertexIndex == vertexIndexC);
    }
}
