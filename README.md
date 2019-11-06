# xamarin-forms-notifications
## Notificaciones agrupadas

Si tu app usa notificaciones locales o remotas y deseas agruparlas para organizar un poco mejor la barra de estado del dispositivo móvil, quédate unos minutos a leer esta entrada, seguro que te interesa.

Haciendo un poco de historia a partir de Android 7.0 (API nivel 24) y versiones posteriores, cuando se reciben más de cuatro notificaciones de un mismo origen, el sistema las agrupa de manera automática, pero que ocurre si queremos ir un paso más allá y realizar agrupaciones en base a un criterio determinado. Por ejemplo, imagínate una aplicación de mensajería que da soporte a un comercio electrónico y esta recibe notificaciones de cada cliente potencial, para una mejor experiencia de usuario lo más idóneo es agrupar los mensajes en base a cada cliente, de manera que el usuario pueda ver de un vistazo los diferentes mensajes de cada comprador.

Vamos a ver los pasos necesarios para realizar en la plataforma Android un agrupamiento de notificaciones.

## Crear canal de notificación

Mediante el siguiente código (que tendremos que incluir dentro de nuestra clase MainActivity en el método OnCreate), creamos un nuevo canal para la recepción de las notificaciones

    var channel = new NotificationChannel(CHANNEL_ID, "FCM Notifications", NotificationImportance.Default)
            {
                Description = "Firebase Cloud Messages appear in this channel"
            };

    var notificationManager = (NotificationManager)GetSystemService(Android.Content.Context.NotificationService);
            notificationManager.CreateNotificationChannel(channel);
            
## Crear la notificación

Una vez creado el canal de notificación, lo siguiente que vamos a realizar es la creación propia de la notificación, lo más importante en este punto es el establecimiento de la propiedad SetGroup(), aquí deberemos indicar un nombre identificativo por el que queramos realizar la agrupación.

    //create the notification
    var notification = builder
                        .SetContentIntent(contentIntent)
                        .SetSmallIcon(Android.Resource.Drawable.SymActionChat)
                        .SetTicker(title)
                        .SetContentTitle(title)
                        .SetContentText(text)
                        .SetSound(RingtoneManager.GetDefaultUri(RingtoneType.Notification))
                        .SetPriority(NotificationCompat.PriorityHigh)
                        .SetGroup(GROUP_NAME)
                        .SetAutoCancel(true)
                        .Build();


    //show the notification
    notificationManager.Notify(id, notification);
    
## Crear la notificación de resumen

Bien, llegado a este punto, y si lanzaramos nuestro código tal cual, tendríamos en nuestra StatusBar del dispositivo móvil un conjunto de notificaciones una debajo de otra, pero sin agrupar, para ello necesitaremos crear otra notificación que agrupe todas las del mismo grupo. Como detalle cabe mencionar que solo la crearemos si existe más de una notificación del mismo grupo, tiene sentido verdad, pues vamos a ello.

    int numberNotifications = getNumberNotifications(GROUP_NAME);
    if (numberNotifications > 1)
    {
                text = GROUP_NAME + " (" + numberNotifications + ")";

                //create the summary notification
                var notificationSummary = builderMuted
                        .SetContentIntent(contentIntent)
                        .SetSmallIcon(Android.Resource.Drawable.SymActionChat)
                        .SetStyle(new NotificationCompat.BigTextStyle().SetSummaryText(text))
                        .SetSound(null)
                        .SetPriority(NotificationCompat.PriorityLow)
                        .SetGroupSummary(true)
                        .SetGroup(GROUP_NAME)
                        .SetShortcutId("summary_" + GROUP_NAME)
                        .SetAutoCancel(true)
                        .Build();

                //show the notification
                int idSumm = getIdSummaryNotification(GROUP_NAME);
                notificationManager.Notify(idSumm, notificationSummary);
    }
    
Mediante la llamada a la función getNumberNotifications(GROUP_NAME), obtenemos el número de notificaciones que existen en la StatusBar con un mismo código de grupo, la particularidad del código anterior es la existencia de asignación de las propiedades SetGroupSummary(true) para indicar que se trata de la notificación padre o notificación de resumen, así como SetShortcutID que lo usamos para poder localizar la notificación de tipo Summary en el caso de que existieran otras agrupaciones.

Otro detalle a tener en cuenta del código es el valor de la variable idSumm, es referida al identificador de la notificación de resumen, necesitamos conocer este valor para actualizar su contenido en la recepción de una nueva notificación del mismo grupo.

## Ejemplo de funcionamiento
Aquí os dejo un ejemplo de cómo nos quedarían las notificaciones agrupadas en nuestra StatusBar o barra de notificaciones.

![Captura 1](https://1.bp.blogspot.com/-02mECYaKsIs/XbiwscskepI/AAAAAAAAoHE/VquZWeDsmqYZSTpA-X5vluiLXTNm5ZzTACLcBGAsYHQ/s1600/Screenshot_1572384868.png)

![Captura 2](https://1.bp.blogspot.com/-gXZfVM1PYn4/XbiwsWcBwTI/AAAAAAAAoHA/VWCMHeKtdQg-3Vd23ylZ4RRNlZm2ICXxQCLcBGAsYHQ/s1600/Screenshot_1572384891.png)

## Conclusiones

El ejemplo que hemos visto está basado en notificaciones locales pero igualmente podría ser aplicado a notificaciones push remotas, al fin y al cabo la recepción y posteriormente representación en pantalla de la notificación se haría del mismo modo.

Si echáis un ojo al proyecto podéis observar que he creado dos canales de notificaciones, uno con alta prioridad (se reproduce sonido de alerta en la notificación) y otro de baja prioridad (se omite sonido de alerta en la notificación), esto es debido a que en la notificación de resumen no quiero que emita un sonido, delego esta característica solo a las notificaciones hijas que se agrupan dentro de ella, esta personalización es a gusto del consumidor y dependiendo de las necesidades de vuestro proyecto.
