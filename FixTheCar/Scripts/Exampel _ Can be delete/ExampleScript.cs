using UnityEngine;

/// <summary>
/// This class contain example how to use RequireInterface attribute.
/// </summary>
public class ExampleScript : MonoBehaviour
{
    /// <summary>
    /// Working example of the RequireInterface attribute.
    /// </summary>
    [SerializeField]
    [RequireInterface(typeof(IExampleInterface))]
    private Object _referenceExample;
    public IExampleInterface ReferenceExample => _referenceExample as IExampleInterface;

    /// <summary>
    /// Working example of the RequireInterface attribute.
    /// </summary>
    [SerializeField]
    [RequireInterface(typeof(IExampleInterface))]
    private Object _scriptableReferenceExample;
    public IExampleInterface ScriptableReferenceExample => _scriptableReferenceExample as IExampleInterface;

    /// <summary>
    /// Working example of the RequireInterface attribute.
    /// </summary>
    [RequireInterface(typeof(IExampleInterface))]
    public Object emptyFieldExample;

    /// <summary>
    /// Not working example of the RequireInterface attribute.
    /// </summary>
    [RequireInterface(typeof(IExampleInterface))]
    public Vector2 nonReferenceExample;
}
