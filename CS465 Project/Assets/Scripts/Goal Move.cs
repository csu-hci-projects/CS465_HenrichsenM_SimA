using UnityEngine;

public class GoalMove : MonoBehaviour
{
    public enum Shape {Simple, Intermediate, Advanced};

    public Shape shape = Shape.Simple;

    public float speed;

    
    public Vector3[] squareVertices;
    public Vector3[] cubeVertices;
    public Vector3[] advancedVertices;
    public int currentIndex;

    // Update is called once per frame
    void Update()
    {
        if(shape == Shape.Simple)
        {
            SquareTrace();
        }
        if(shape == Shape.Intermediate)
        {
            CubeTrace();
        }
        if(shape == Shape.Advanced)
        {
            AdvancedTrace();
        }
    }

    void SquareTrace()
    {
        if(currentIndex >= squareVertices.Length)
        {
            currentIndex = 0;
        }
        
        transform.position = Vector3.MoveTowards(transform.position, squareVertices[currentIndex], speed * Time.deltaTime);

        if(Vector3.Distance(transform.position, squareVertices[currentIndex]) < 0.1f)
        {
            currentIndex++;

            if(currentIndex >= squareVertices.Length)
            {
                currentIndex = 0;
            }
        }
    }

    void CubeTrace()
    {
        if(currentIndex >= cubeVertices.Length)
        {
            currentIndex = 0;
        }

        transform.position = Vector3.MoveTowards(transform.position, cubeVertices[currentIndex], speed * Time.deltaTime);

        if(Vector3.Distance(transform.position, cubeVertices[currentIndex]) < 0.1f)
        {
            currentIndex++;

            if(currentIndex >= cubeVertices.Length)
            {
                currentIndex = 0;
            }
        }
    }
    
    void AdvancedTrace()
    {
        if(currentIndex >= advancedVertices.Length)
        {
            currentIndex = 0;
        }

        transform.position = Vector3.MoveTowards(transform.position, advancedVertices[currentIndex], speed * Time.deltaTime);

        if(Vector3.Distance(transform.position, advancedVertices[currentIndex]) < 0.1f)
        {
            currentIndex++;

            if(currentIndex >= advancedVertices.Length)
            {
                currentIndex = 0;
            }
        }
    }
}
