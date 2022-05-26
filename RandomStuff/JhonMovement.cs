//"Librerías" que utiliza Unity
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//"Main de Unity, clase JhonMovement porque así se llama mi script XD"
public class JhonMovement : MonoBehaviour
{

    //Las variables públicas se pueden utilizar en el Unity, para que nosotros desde U le demos valores específicos, cuando son privadas, Unity no puede acceder a ellas, solo el programa

    //Varibales globales como la fuerza de salto, la velocidad, y Grounded es un bool que determina si está en el suelo o no
    public float jumpforce;
    public float speed;
    private bool Grounded;

    //RigidBody 2D es un componente en Unity, que le asignamos a un objeto para que pueda tener propiedades de un objeto 2D, con objeto no me refiero a Objeto de Programación
    private Rigidbody2D Rigidbody2D;

    //Animator es el componente en Unity para las animaciones, al igual que RigidBody
    private Animator Animator;
    private float Horizontal;
    public GameObject BulletPrefab;
    private float LastShoot;

    //Cada que inicie el juego, haz:...
    void Start()
    {   
        //Cada que inice, que me de el componente de RigidBody2D, Animator
        Rigidbody2D = GetComponent<Rigidbody2D>();
        Animator = GetComponent<Animator>();
    }   

    //Cada que se actualice un frame, es decir, esta función se va a ejecutar n veces por segundo. Por ejemplo, si nuestro juego va a 60fps, 60 frames per second, esta f(x) se va a ejecutar 60 veces por segundo.
    void Update()
    {   
        //Horizontal = El input del teclado, obtenemos valores de -1 a 1, dependiendo de la tecla que presione
        Horizontal = Input.GetAxisRaw("Horizontal");

        //Set bool porque en Unity pusimos Bool, el nombre running es el nombre que pusimos, si horizontal es diferente de 0 devuelve true, si es igual, devuelve false
        Animator.SetBool("Running", Horizontal != 0.0f);

        //Dibujamos un rayito que se origina en la hitbox del personaje
        Debug.DrawRay(transform.position, Vector2.down * 0.1f, Color.red); 

        //Ifs pora que cuando el personaje se mueva hacia la izquierda, realmente voltee
        if(Horizontal < 0.0f) transform.localScale = new Vector3(-1.0f,1.0f,1.0f);
        else if(Horizontal > 0.0f) transform.localScale = new Vector3(1.0f,1.0f,1.0f);

        //Si el rayito de la hitbox choca en el suelo, grounded = true, si no, es false, esto es para que no salte si no está en el suelo, si no se hace esto, el jugador puede
        //dar los saltos que quiera hasta volar XD

        //Si, Physics2D.Raycast() que es una función de la clase Pyshics2D que hace un rayito no necesariamente visible en gameplay, desde nuestra posición hasta el vector Y en su origen, y si 
        //hay una distancia de 0.1, entonces grounded, que es nuestra variable para saber si está en el suelo; devuelve true, en caso contrario, devuelve false.

        if(Physics2D.Raycast(transform.position,Vector2.down,0.1f))
        {
            Grounded = true;
        }
        else
        {
            Grounded = false;
        }
        
        //Agregamos físicas para el salto del personaje, si presiona la W y Grounded = True, es decir, si está en el suelo; puede brincar con la función Jump()
        if(Input.GetKeyDown(KeyCode.W) && Grounded)
        {
            Jump();
        }

        if(Input.GetKey(KeyCode.Space) && Time.time > LastShoot + 0.25f)
        {
            Shoot();
            LastShoot = Time.time;
        }
    }

    //Esta función es para que la velocidad del personaje no cambie a pesar de cualquier variación de fps, porque si el juego aumenta o disminuye de fps dependiendo del usuario, la velocidad
    //del personaje se ve afectada, con esta función establefcemos una misma velocidad para el objeto para siempre.

    private void FixedUpdate() {

        //La velocidad del personaje = new Vector2(El valor de Horizontal, que recordemos que es el que le damos con el input del teclado; y en y se queda igual)
        
        Rigidbody2D.velocity = new Vector2(Horizontal,Rigidbody2D.velocity.y);
    }

    //f(x) para saltar, que...
    private void Jump()
    {   
        //Al muñequito, le agregamos una fuerza, que será en el vector2.up, es decir, en el eje de las y; multiplicado por la fuerza que nosotros le demos en Unity.
        Rigidbody2D.AddForce(Vector2.up * jumpforce);
    }

    private void Shoot()
    {   
        Vector3 direction;
        if(transform.localScale.x == 1) direction = Vector2.right;
        else direction = Vector2.left;

        //Lo que hace es Instanciar, que toma un prefab y lo duplica en un sitio
        //del mundo, con alguna rotación, Quaternion.identity es ninguna rotación
        GameObject bullet = Instantiate(BulletPrefab,transform.position + direction * 0.1f, Quaternion.identity);
        bullet.GetComponent<BulletScript>().SetDirection(direction);
    }
}
