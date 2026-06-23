# Instrucciones para GitHub Copilot — Proyecto Contoso Seguros CR

## Idioma y comunicación

- Todo el código, comentarios, mensajes de error y documentación en **español**
- Nombres de variables, propiedades y métodos en español salvo términos técnicos estándar (Get, Post, Id, DTO, API)
- Commits y pull requests en español

## Stack tecnológico

| Capa | Tecnología |
|------|-----------|
| Runtime | .NET 8 LTS |
| Estilo API | Minimal APIs (nunca Controllers) |
| Documentación API | Swashbuckle con OpenAPI annotations |
| Persistencia | En memoria (List<T> y Dictionary<K,V>) |
| Pruebas | xUnit + WebApplicationFactory |
| Frontend | HTML + JavaScript vanilla + Bootstrap 5 CDN |

## Convenciones de nomenclatura

- Clases y records: PascalCase en español (`Reclamacion`, `PolizaServicio`)
- Propiedades públicas: PascalCase (`FechaCreacion`, `MontoReclamado`)
- Variables locales: camelCase (`polizaId`, `montoMaximo`)
- Interfaces: prefijo I + PascalCase (`IReclamacionServicio`)
- Constantes: PascalCase (`MontoMaximoSalud`, `EstadoPendiente`)
- Enumeraciones: PascalCase con valores en español (`EstadoReclamacion.Pendiente`)

## Estándares de código

- Usa `decimal` para todos los montos financieros, nunca `double` ni `float`
- Usa `DateTime.UtcNow` para timestamps, nunca `DateTime.Now`
- Valida inputs al inicio del método antes de procesar lógica de negocio
- Lanza `ArgumentException` para validaciones de negocio, `KeyNotFoundException` para entidades no encontradas
- Usa records para DTOs de entrada/salida
- Documenta todos los métodos públicos con comentarios XML `///` en español
- Los servicios se registran como `Singleton` en el contenedor DI

## Estructura de endpoints

- Agrupa endpoints por entidad con `MapGroup("/api/{entidad}").WithTags("{Entidad}")`
- Siempre incluye `.WithName()`, `.WithOpenApi()`, y `.Produces<T>()` en cada endpoint
- Retorna `Results.NotFound()` cuando una entidad no existe, nunca null
- Retorna `Results.Created()` con Location header en POST
- Retorna `Results.UnprocessableEntity()` para errores de validación de negocio

## Dominio de negocio

Contoso Seguros CR es una aseguradora costarricense. Las entidades principales son:

- **Cliente**: persona asegurada con datos de contacto
- **Poliza**: contrato de seguro de tipo Salud, Vida o Vehiculo, con estado y monto máximo de cobertura
- **Reclamacion**: solicitud de cobertura vinculada a una póliza activa, con estado de ciclo de vida

Reglas de negocio críticas:
- Solo se puede crear una reclamación sobre una póliza con estado `Activa`
- El monto reclamado no puede superar el `MontoMaximoCoberturaColones` de la póliza
- Una póliza no puede tener más de una reclamación en estado `Pendiente` simultáneamente
- El estado de una reclamación sigue el flujo: `Pendiente → EnRevision → Aprobada | Rechazada`
- No se puede actualizar una reclamación con estado `Aprobada` o `Rechazada`
