using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Shapes;

public class ShapeDesignerWindow : EditorWindow
{

    //Icons
    private Rect shapeIconSection;
    private Texture2D shapeIconTexture;

    private Texture2D currentShape;

    private Texture2D cubeIcone;
    private Texture2D sphereIcone;
    private Texture2D capsuleIcone;
    private Texture2D cylinderIcone;


    //Basic parameters
    private ShapeTypes shapeType;
    private Vector3 shapePosition;
    private Vector3 shapeRotation;
    private float shapeScale;
    private Color shapeColor;

    //it must be initialized, otherwise there will be "NullReferenceException" error
    //while calculating the length, because it is empty == not initialized
    private string shapeName = "Test shape";

    //Random options section
    private bool ShowRandomOptions;
    private float minPosition;
    private float maxPosition;
    private float maxRotation;
    private float maxScale = 1f;

    //Multiple shapes section
    private bool ShowMultipleShapes;
    private int numberOfShapes;
    private float spaceBetweenShapes;
    private bool offsetIsEnabled = true;
    private bool[] offsetTransform = new bool[3] { true, true, true };



    [MenuItem("Window/Javad/Shape Designer")]
    static void OpenWindow() {
        ShapeDesignerWindow window = (ShapeDesignerWindow)GetWindow(typeof(ShapeDesignerWindow));

        //the window can't get smaller than this size
        window.minSize = new Vector2(200f, 200f);
        
        window.titleContent.text = "Shape Designer";

        window.Show();
    }

    //when the window is started
    private void OnEnable()
    {
        InitializeTextures();
    }

    //Load the textures from "resources/icons"
    private void InitializeTextures() {
        shapeIconTexture = new Texture2D(1, 1);
        shapeIconTexture.SetPixel(0, 0, Color.blue);
        shapeIconTexture.Apply();

        cubeIcone = Resources.Load<Texture2D>("Icons/cube");
        sphereIcone = Resources.Load<Texture2D>("Icons/sphere");
        capsuleIcone = Resources.Load<Texture2D>("Icons/capsule");
        cylinderIcone = Resources.Load<Texture2D>("Icons/cylinder");
    }

    private void OnGUI()
    {
        GUILayout.Space(5);

        ChangeShape();
        DrawLayouts(position.width);
        DrawShapeSettings();
    }


    private void ChangeShape() {
        switch (shapeType)
        {
            case ShapeTypes.CUBE:
                {
                    currentShape = cubeIcone;
                    break;
                }
            case ShapeTypes.SPHERE:
                {
                    currentShape = sphereIcone;
                    break;
                }
            case ShapeTypes.CAPSULE:
                {
                    currentShape = capsuleIcone;
                    break;
                }
            case ShapeTypes.CYLINDER:
                {
                    currentShape = cylinderIcone;
                    break;
                }
        }
    }


    private void DrawLayouts(float screenWidth) {
        shapeIconSection.x = (screenWidth - shapeIconSection.width)/ 2;
        shapeIconSection.y = 350;
        shapeIconSection.width = 200;
        shapeIconSection.height = 200;

        GUI.DrawTexture(shapeIconSection, currentShape);
    }

    private void DrawShapeSettings() {
        EditorGUILayout.Space();

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Type:", GUILayout.Width(60));
        shapeType = (ShapeTypes)EditorGUILayout.EnumPopup(shapeType);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        shapePosition = EditorGUILayout.Vector3Field("Position:", shapePosition);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        shapeRotation = EditorGUILayout.Vector3Field("Rotation:",shapeRotation);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Size:", GUILayout.Width(60));
        shapeScale = EditorGUILayout.Slider(shapeScale, 1f, 10f);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Color:", GUILayout.Width(60));
        shapeColor = EditorGUILayout.ColorField(shapeColor);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Name:", GUILayout.Width(60));
        shapeName = EditorGUILayout.TextField(shapeName);
        EditorGUILayout.EndHorizontal();

        if (shapeName.Length == 0)
        {
            EditorGUILayout.HelpBox("The shape's name can not be empty", MessageType.Error);

        }
        else if (shapeName.StartsWith(" "))
        {
            EditorGUILayout.HelpBox("The shape's name can not start with a space", MessageType.Error);
        }


        //Random shapes
        ShowRandomOptions = EditorGUILayout.Foldout(ShowRandomOptions, "Create Randomized Shape");
        if (ShowRandomOptions)
        {
            minPosition = EditorGUILayout.FloatField("Minimum Position:", minPosition);
            maxPosition = EditorGUILayout.FloatField("Maximum Position:", maxPosition);
            maxRotation = EditorGUILayout.FloatField("Maximum Rotation:", maxRotation);
            maxScale = EditorGUILayout.FloatField("Maximum Scale:", maxScale);

            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Create"))
            {
                CreateShape(true);
            }
            if (GUILayout.Button("Reset Values"))
            {
                minPosition = 0;
                maxPosition = 0;
                maxRotation = 0;
                maxScale = 1;
            }
            EditorGUILayout.EndHorizontal();

            if (maxScale < 0.11f)
            {
                maxScale = 0.1f;
                EditorGUILayout.HelpBox("The scale can not be less than 0.1", MessageType.Warning);
            }

        }


        
        //Multiple shapes
        ShowMultipleShapes = EditorGUILayout.Foldout(ShowMultipleShapes, "Create Multiple Shapes");
        if (ShowMultipleShapes)
        {
            numberOfShapes = EditorGUILayout.IntField("Count:", numberOfShapes);
            if (numberOfShapes < 2)
            {
                numberOfShapes = 1;
                EditorGUILayout.HelpBox("The count can not be lower than 1", MessageType.Warning);
            }
            else if (numberOfShapes > 50)
            {
                EditorGUILayout.HelpBox("Large number of shapes, can make the process so long or crash", MessageType.Warning);
            }




            offsetIsEnabled = EditorGUILayout.BeginToggleGroup("Affect offset over:", offsetIsEnabled);
            spaceBetweenShapes = EditorGUILayout.FloatField("Offset:", spaceBetweenShapes);
            offsetTransform[0] = EditorGUILayout.Toggle("X:", offsetTransform[0]);
            offsetTransform[1] = EditorGUILayout.Toggle("Y:", offsetTransform[1]);
            offsetTransform[2] = EditorGUILayout.Toggle("Z:", offsetTransform[2]);
            EditorGUILayout.EndToggleGroup();


            if(!(offsetTransform[0] || offsetTransform[1] || offsetTransform[2]) && offsetIsEnabled)
            {
                EditorGUILayout.HelpBox("At least one must be active.\nIf you don't need offset, you can disable it.", MessageType.Error);
            }



            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Create Multiple Shapes"))
            {
                CreateMultipleShapes();
            }
            EditorGUILayout.EndHorizontal();
        }


        


        EditorGUILayout.BeginHorizontal();
        if(GUILayout.Button("Create Single Shape"))
        {
            CreateShape(false);
        }
        EditorGUILayout.EndHorizontal();
    }


    private void CreateShape(bool randomize)
    {

        if (shapeName.Length == 0 || shapeName.StartsWith(" "))
            return;

        switch (shapeType)
        {
            case ShapeTypes.CUBE:
                {
                    GameObject shape = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    if (!randomize)
                    {
                        SetTransforms(shape);
                    }
                    else
                    {
                        SetRandomTransforms(shape);
                    }
                    break;
                }
            case ShapeTypes.SPHERE:
                {
                    GameObject shape = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                    if (!randomize)
                    {
                        SetTransforms(shape);
                    }
                    else
                    {
                        SetRandomTransforms(shape);
                    }
                    break;
                }
            case ShapeTypes.CAPSULE:
                {
                    GameObject shape = GameObject.CreatePrimitive(PrimitiveType.Capsule);
                    if (!randomize)
                    {
                        SetTransforms(shape);
                    }
                    else
                    {
                        SetRandomTransforms(shape);
                    }
                    break;
                }
            case ShapeTypes.CYLINDER:
                {
                    GameObject shape = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
                    if (!randomize)
                    {
                        SetTransforms(shape);
                    }
                    else
                    {
                        SetRandomTransforms(shape);
                    }
                    break;
                }
        }
    }



    private void CreateMultipleShapes() {
        if (shapeName.Length == 0 || shapeName.StartsWith(" "))
            return;

        switch (shapeType)
        {
            case ShapeTypes.CUBE:
                {

                    for(int i =0; i< numberOfShapes; i++)
                    {
                        GameObject shape = GameObject.CreatePrimitive(PrimitiveType.Cube);
                        SetTransformsMultiple(shape, i);
                    }
                    
                    break;
                }
            case ShapeTypes.SPHERE:
                {
                    for (int i = 0; i < numberOfShapes; i++)
                    {
                        GameObject shape = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                        SetTransformsMultiple(shape, i);
                    }
                    break;
                }
            case ShapeTypes.CAPSULE:
                {
                    for (int i = 0; i < numberOfShapes; i++)
                    {
                        GameObject shape = GameObject.CreatePrimitive(PrimitiveType.Capsule);
                        SetTransformsMultiple(shape, i);
                    }
                    break;
                }
            case ShapeTypes.CYLINDER:
                {
                    
                    for (int i = 0; i < numberOfShapes; i++)
                    {
                        GameObject shape = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
                        SetTransformsMultiple(shape, i);
                    }
                    break;
                }
        }
    }

    private void SetTransforms(GameObject shape) {
        shape.transform.position = shapePosition;
        shape.transform.eulerAngles = shapeRotation;
        shape.transform.localScale = new Vector3(shapeScale, shapeScale, shapeScale);

        var newMaterial = new Material(shape.GetComponent<Renderer>().sharedMaterial);
        newMaterial.color = shapeColor;
        shape.GetComponent<Renderer>().sharedMaterial = newMaterial;

        shape.name = shapeName;
    }


    private void SetRandomTransforms(GameObject shape)
    {
        float newPosition = Random.Range(minPosition, maxPosition);
        float newRotation = Random.Range(0, maxRotation);
        float newScale = Random.Range(0.1f, maxScale);

        shape.transform.position = Vector3.one * newPosition;
        shape.transform.eulerAngles = Vector3.one * newRotation;
        shape.transform.localScale = Vector3.one * newScale;

        var newMaterial = new Material(shape.GetComponent<Renderer>().sharedMaterial);
        newMaterial.color = shapeColor;
        shape.GetComponent<Renderer>().sharedMaterial = newMaterial;

        shape.name = shapeName;
    }

    private void SetTransformsMultiple(GameObject shape, int i)
    {
        if (offsetIsEnabled)
        {
            //x
            if(offsetTransform[0])
                shape.transform.position += shapePosition + (Vector3.right * i * spaceBetweenShapes);
            //y
            if (offsetTransform[1])
                shape.transform.position += shapePosition + (Vector3.up * i * spaceBetweenShapes);
            //z
            if (offsetTransform[2])
                shape.transform.position += shapePosition + (Vector3.forward * i * spaceBetweenShapes);
        } else
        {
            shape.transform.position = shapePosition + (Vector3.one * i * spaceBetweenShapes);
        }
        shape.transform.eulerAngles = shapeRotation;
        shape.transform.localScale = new Vector3(shapeScale, shapeScale, shapeScale);

        var newMaterial = new Material(shape.GetComponent<Renderer>().sharedMaterial);
        newMaterial.color = shapeColor;
        shape.GetComponent<Renderer>().sharedMaterial = newMaterial;

        shape.name = shapeName + " " + (i+1);
    }
}
