# xamarin-forms-notifications
Notificaciones agrupadas

Si tu app usa notificaciones locales o remotas y deseas agruparlas para organizar un poco mejor la barra de estado del dispositivo móvil, quédate unos minutos a leer esta entrada, seguro que te interesa.

Haciendo un poco de historia a partir de Android 7.0 (API nivel 24) y versiones posteriores, cuando se reciben más de cuatro notificaciones de un mismo origen, el sistema las agrupa de manera automática, pero que ocurre si queremos ir un paso más allá y realizar agrupaciones en base a un criterio determinado. Por ejemplo, imagínate una aplicación de mensajería que da soporte a un comercio electrónico y esta recibe notificaciones de cada cliente potencial, para una mejor experiencia de usuario lo más idóneo es agrupar los mensajes en base a cada cliente, de manera que el usuario pueda ver de un vistazo los diferentes mensajes de cada comprador.

Vamos a ver los pasos necesarios para realizar en la plataforma Android un agrupamiento de notificaciones.
