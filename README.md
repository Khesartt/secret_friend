# Secret Friend API

## Descripción
API para generar asignaciones de amigo secreto de forma aleatoria. Este servicio toma una lista de jugadores con sus nombres y correos electrónicos, y genera un diccionario de asignaciones donde cada persona tiene asignado un amigo secreto diferente.

## Características
- ✅ Generación aleatoria de asignaciones
- ✅ Garantiza que nadie sea su propio amigo secreto
- ✅ Previene asignaciones duplicadas
- ✅ Validación de datos de entrada
- ✅ Documentación con Swagger
- ✅ Manejo de errores robusto
- ✅ Logging detallado

## Endpoints

### 1. Generar Amigos Secretos
**POST** `/api/secretfriend/generate`

Genera las asignaciones de amigos secretos para una lista de jugadores.

#### Request Body
```json
{
  "players": [
    {
      "name": "Juan Pérez",
      "email": "juan@ejemplo.com"
    },
    {
      "name": "María García", 
      "email": "maria@ejemplo.com"
    },
    {
      "name": "Carlos López",
      "email": "carlos@ejemplo.com"
    }
  ]
}
```

#### Response
```json
{
  "assignments": [
    {
      "giver": {
        "name": "Juan Pérez",
        "email": "juan@ejemplo.com"
      },
      "receiver": {
        "name": "María García",
        "email": "maria@ejemplo.com"
      }
    },
    {
      "giver": {
        "name": "María García",
        "email": "maria@ejemplo.com"
      },
      "receiver": {
        "name": "Carlos López",
        "email": "carlos@ejemplo.com"
      }
    },
    {
      "giver": {
        "name": "Carlos López",
        "email": "carlos@ejemplo.com"
      },
      "receiver": {
        "name": "Juan Pérez",
        "email": "juan@ejemplo.com"
      }
    }
  ],
  "message": "¡Amigos secretos generados exitosamente para 3 jugadores!",
  "generatedAt": "2025-12-14T10:30:00.000Z"
}
```

### 2. Health Check
**GET** `/api/secretfriend/health`

Verifica el estado del servicio.

### 3. Información del API
**GET** `/api/secretfriend/info`

Obtiene información sobre el API y ejemplos de uso.

### 4. Bienvenida
**GET** `/`

Endpoint de bienvenida con enlaces útiles.

## Cómo ejecutar

1. Navegar al directorio del proyecto:
```bash
cd secretFriend.Api
```

2. Ejecutar la aplicación:
```bash
dotnet run
```

3. Abrir el navegador en:
- **Aplicación**: https://localhost:7xxx
- **Swagger UI**: https://localhost:7xxx/swagger

## Validaciones

- Mínimo 2 jugadores requeridos
- No se permiten nombres duplicados
- No se permiten emails duplicados  
- Nombres y emails no pueden estar vacíos
- Nadie puede ser su propio amigo secreto

## Algoritmo

El servicio utiliza el algoritmo Fisher-Yates para mezclar aleatoriamente la lista de receptores y luego verifica que nadie sea asignado como su propio amigo secreto. Si esto ocurre, regenera las asignaciones hasta obtener una combinación válida (máximo 1000 intentos para prevenir bucles infinitos).

## Tecnologías

- .NET 9
- ASP.NET Core Web API
- Swagger/OpenAPI
- Dependency Injection
- Logging