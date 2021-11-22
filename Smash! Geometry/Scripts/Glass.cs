using UnityEngine;

public class Glass : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private Vector2 moveModifier;
    [SerializeField] private float fadeSpeed;
    private GameController gameController;
    private SpriteRenderer _sprite;
    private Rigidbody2D _rigidbody;


    private void Start()
    {
        _sprite = gameObject.GetComponent<SpriteRenderer>();
        _rigidbody = gameObject.GetComponent<Rigidbody2D>();
        _rigidbody.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
        gameController = GameController.gameController;
        _rigidbody.AddForce(new Vector2(transform.up.x * moveModifier.x, transform.up.y * moveModifier.y) * 10000 * (speed + speed * 0.30f));
    }
    private void FixedUpdate()
    {
        var color = _sprite.color;
        color = new Color(color.r, color.g, color.b, color.a - fadeSpeed);
        _sprite.color = color;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if ((collision.gameObject.name == "Inside"))
        {
            Destroy(gameObject);
        }
    }
}