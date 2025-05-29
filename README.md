# Lista de Tareas - Aplicación Móvil Xamarin.Forms

## Descripción

Esta es una aplicación móvil desarrollada con Xamarin.Forms que permite a los usuarios gestionar una lista de tareas pendientes. Los usuarios pueden agregar nuevas tareas, editar las existentes (nombre y descripción), y eliminarlas. Las tareas se almacenan localmente utilizando una base de datos SQLite.

Este proyecto fue desarrollado como parte de una actividad de aprendizaje para demostrar el uso de C# para la lógica de negocio y Xamarin.Forms para el diseño de la interfaz de usuario en la creación de aplicaciones móviles multiplataforma.

## Requerimientos

La aplicación implementa las siguientes funcionalidades:

-   **Pantalla Principal:** Muestra la lista de todas las tareas.
-   **Agregar Tarea:** Un botón en la pantalla principal permite añadir nuevas tareas.
-   **Editar Tarea:** Al seleccionar una tarea de la lista, se abre una pantalla para modificar su nombre y descripción.
-   **Almacenamiento:** Las tareas se guardan de forma persistente en una base de datos SQLite local.
-   **Manejo de Errores:** La aplicación incluye mecanismos para manejar errores y excepciones de manera adecuada.

## Tecnologías Utilizadas

-   **Xamarin.Forms:** Framework de código abierto para construir interfaces de usuario nativas para iOS, Android, macOS, WPF y Tizen desde una única base de código .NET.
-   **C#:** Lenguaje de programación utilizado para la lógica de negocio de la aplicación.
-   **SQLite:** Motor de base de datos relacional ligero y autónomo que se utiliza para el almacenamiento local de las tareas.

## Instrucciones de Ejecución

Para ejecutar esta aplicación, necesitarás tener instalado el entorno de desarrollo de Xamarin. Puedes seguir las guías oficiales de Microsoft para configurar tu entorno para el desarrollo de Xamarin.Forms en Visual Studio o Visual Studio para Mac.

1.  Clona este repositorio (si aplica).
2.  Abre la solución en Visual Studio o Visual Studio para Mac.
3.  Selecciona un simulador o dispositivo físico como destino de la compilación.
4.  Ejecuta la aplicación.

## ¿Cómo se hizo esta aplicación?

Esta aplicación de lista de tareas se construyó utilizando el patrón de diseño Model-View-ViewModel (MVVM), que ayuda a separar la interfaz de usuario, la lógica de la vista y la lógica de negocio. Aquí te explico los pasos generales:

1.  **Creación del Proyecto Xamarin.Forms:** Se inicia creando un nuevo proyecto Xamarin.Forms en Visual Studio o Visual Studio para Mac. Esto genera la estructura básica de la aplicación, incluyendo los proyectos para las plataformas específicas (iOS, Android, etc.) y el proyecto compartido donde reside la mayor parte del código.

2.  **Definición del Modelo (Model):** Se crea una clase en C# llamada `TodoItem` que representa una tarea. Esta clase tiene propiedades como `Id`, `Title` (nombre de la tarea), `Description` (descripción detallada), y posiblemente un estado como `IsCompleted`.

    ```csharp
    public class TodoItem
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public bool IsCompleted { get; set; }
    }
    ```

3.  **Creación de la Vista (View):** Se diseñan las interfaces de usuario utilizando XAML (Extensible Application Markup Language).
    -   **Pantalla Principal:** Un `ContentPage` que contiene un `ListView` para mostrar las tareas y un botón para agregar nuevas tareas. El `ListView` utiliza un `DataTemplate` para definir cómo se muestra cada `TodoItem`.
    -   **Pantalla de Edición:** Otro `ContentPage` con campos (`Entry` para el nombre, `Editor` para la descripción) y botones para guardar o cancelar.

4.  **Implementación del ViewModel (ViewModel):** Se crean clases en C# que actúan como intermediarios entre las Vistas y el Modelo.
    -   **ViewModel de la Pantalla Principal:** Expone una colección de `TodoItem`s para mostrar en el `ListView`, comandos para agregar y eliminar tareas, y la lógica para cargar las tareas desde la base de datos.
    -   **ViewModel de la Pantalla de Edición:** Contiene las propiedades para el nombre y la descripción de la tarea que se está editando, y comandos para guardar los cambios.

5.  **Almacenamiento con SQLite:** Se utiliza la biblioteca `sqlite-net-pcl` para interactuar con una base de datos SQLite local. Se crea un servicio (como `DatabaseService`) que se encarga de inicializar la conexión a la base de datos, crear la tabla `TodoItem` si no existe, y proporcionar métodos para insertar, actualizar, eliminar y obtener tareas.

6.  **Enlace de Datos (Data Binding):** Se utiliza el enlace de datos de Xamarin.Forms para conectar las propiedades de los ViewModels con los elementos de la interfaz de usuario en las Vistas. Por ejemplo, el texto de un `Entry` se puede enlazar a una propiedad `Title` en el ViewModel.

7.  **Navegación:** Se implementa la navegación entre la pantalla principal y la pantalla de edición, generalmente utilizando `Navigation.PushAsync` y `Navigation.PopAsync`.

8.  **Manejo de Errores:** Se utilizan bloques `try-catch` para capturar posibles excepciones durante las operaciones (por ejemplo, al acceder a la base de datos) y se informa al usuario de manera adecuada, por ejemplo, mediante `DisplayAlert`.
