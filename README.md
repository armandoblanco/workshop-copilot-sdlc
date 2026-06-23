# Workshop: GitHub Copilot SDLC E2E — Contoso Seguros CR

## De la Historia de Usuario al Código en Producción con Agentes Especializados

[![GitHub Copilot](https://img.shields.io/badge/GitHub%20Copilot-Enabled-green)](https://github.com/features/copilot)
[![.NET](https://img.shields.io/badge/.NET-8.0%20LTS-purple)](https://dotnet.microsoft.com)
[![Duración](https://img.shields.io/badge/Duración-90%20minutos-red)](.)
[![SDLC](https://img.shields.io/badge/SDLC-E2E-blue)](.)

---

## 📋 Tabla de Contenidos

- [Introducción](#-introducción)
- [Arquitectura de Agentes](#-arquitectura-de-agentes)
- [Escenario](#-escenario-contoso-seguros-cr)
- [Pre-requisitos](#-pre-requisitos)
- [Agenda del Workshop](#-agenda-del-workshop)
- [Módulo 1: Historias de Usuario](#-módulo-1-creación-de-historia-de-usuario-10-min)
- [Módulo 2: Revisión de Historia](#-módulo-2-revisión-de-historia-10-min)
- [Módulo 3: Revisión Arquitectónica](#-módulo-3-revisión-arquitectónica-15-min)
- [Módulo 4: Implementación](#-módulo-4-implementación-25-min)
- [Módulo 5: Casos de Prueba](#-módulo-5-casos-de-prueba-15-min)
- [Módulo 6: Auditoría de Seguridad](#-módulo-6-auditoría-de-seguridad-10-min)
- [Cierre](#-cierre-5-min)

---

## 🎯 Introducción

Este workshop de **90 minutos** cubre el ciclo completo de desarrollo de software usando **GitHub Copilot como orquestador de roles especializados**. Cada etapa del SDLC tiene un agente configurado con el contexto y las responsabilidades de ese rol específico.

La diferencia con un workshop de codificación tradicional: aquí Copilot no solo escribe código. Redacta requisitos, evalúa arquitectura, genera pruebas y audita seguridad, todo dentro del mismo repositorio.

### Lo que vas a aprender

- Cómo configurar agentes especializados por rol del SDLC en `.github/agents/`
- La diferencia práctica entre usar Copilot genérico vs. un agente con contexto de dominio
- Cómo encadenar los artefactos de cada etapa como entrada de la siguiente
- Que el valor de Copilot en equipos no es solo velocidad sino **consistencia** entre roles

---

## 🤖 Arquitectura de Agentes

Este workshop usa **6 agentes especializados**, cada uno mapeado a un rol del SDLC:

| Agente | Archivo | Rol | Etapa |
|--------|---------|-----|-------|
| `product-owner` | `.github/agents/product-owner.agent.md` | Redacta y refina Historias de Usuario | Planificación |
| `story-reviewer` | `.github/agents/story-reviewer.agent.md` | Valida la Historia contra estándares de calidad | Planificación |
| `architect` | `.github/agents/architect.agent.md` | Evalúa viabilidad técnica y propone diseño | Diseño |
| `implementer` | `.github/agents/implementer.agent.md` | Implementa el feature con .NET 8 Minimal APIs | Desarrollo |
| `qa-engineer` | `.github/agents/qa-engineer.agent.md` | Genera casos y código de pruebas | QA |
| `security-auditor` | `.github/agents/security-auditor.agent.md` | Revisa vulnerabilidades y riesgos de seguridad | Seguridad |

> **Nota sobre compatibilidad:** Los agentes con formato `.agent.md` funcionan en GitHub Copilot Cloud Agent (GitHub.com) y VS Code Agent Mode. En VS Code, el selector de agentes aparece en el panel de Copilot Chat. Si tu versión no muestra el selector, usa el archivo como referencia de prompt y cópialo manualmente en el chat.

---

## 🏢 Escenario: Contoso Seguros CR

**Contoso Seguros CR** es una aseguradora costarricense que gestiona pólizas de seguro de salud, vida y vehículos. Necesitan una API para que sus agentes comerciales puedan consultar y administrar pólizas de clientes desde una aplicación móvil.

El sistema ya tiene una base de código existente con modelos básicos. Durante el workshop vas a agregar el feature de **gestión de reclamaciones** (claims) siguiendo el flujo completo del SDLC.

### Por qué este escenario

- Es un dominio con **reglas de negocio no triviales** (montos máximos, estados de póliza, validaciones)
- Tiene **implicaciones de seguridad reales** (datos de salud, montos financieros)
- Es familiar para audiencias de servicios financieros en LATAM
- La complejidad es suficiente para que los agentes tengan algo real que analizar, sin ser abrumadora en 90 minutos

---

## 🛠️ Pre-requisitos

```bash
dotnet --version   # 8.x
code --version     # VS Code
git --version
```

**Extensiones VS Code:**
- GitHub Copilot
- GitHub Copilot Chat  
- C# Dev Kit

**Acceso:**
- Licencia de GitHub Copilot activa (Individual, Business o Enterprise)
- Para usar los agentes cloud: acceso a github.com/copilot con el repositorio disponible

---

## 📅 Agenda del Workshop

| Tiempo | Módulo | Agente | Artefacto producido |
|--------|--------|--------|---------------------|
| 0:00 – 0:05 | Setup | — | Proyecto corriendo localmente |
| 0:05 – 0:15 | Módulo 1: Historia de Usuario | `product-owner` | `docs/historia-reclamaciones.md` |
| 0:15 – 0:25 | Módulo 2: Revisión de Historia | `story-reviewer` | `docs/revision-historia.md` |
| 0:25 – 0:40 | Módulo 3: Revisión Arquitectónica | `architect` | `docs/propuesta-arquitectura.md` |
| 0:40 – 1:05 | Módulo 4: Implementación | `implementer` | Código fuente del feature |
| 1:05 – 1:20 | Módulo 5: Casos de Prueba | `qa-engineer` | Tests xUnit |
| 1:20 – 1:30 | Módulo 6: Auditoría de Seguridad | `security-auditor` | `docs/reporte-seguridad.md` |
| 1:30 – 1:35 | Cierre | — | — |

---

## 🔬 Setup: Ejecutar el Proyecto Base (5 min)

```bash
# Clonar el repositorio
git clone https://github.com/armandoblanco/workshop-copilot-sdlc-tico.git
cd workshop-copilot-sdlc-tico

# Restaurar dependencias y compilar
dotnet build

# Ejecutar la API
cd ContosoSegurosCR
dotnet run
```

Verifica que funcione:
- API en `http://localhost:5000`
- Swagger en `http://localhost:5000/swagger`

El proyecto base tiene los endpoints de **Clientes** y **Pólizas** ya implementados. Tu tarea es agregar el módulo de **Reclamaciones**.

---

## 🔬 Módulo 1: Creación de Historia de Usuario (10 min)

**Objetivo:** Usar el agente `product-owner` para redactar una Historia de Usuario bien formada para el feature de reclamaciones.

### Activar el agente

En Copilot Chat, selecciona el agente **product-owner** del selector de agentes.

### Prompt inicial

```
Como Product Owner de Contoso Seguros CR necesito una Historia de Usuario para que los agentes comerciales puedan registrar y gestionar reclamaciones de clientes.

Contexto del negocio:
- Los clientes tienen pólizas activas de salud, vida o vehículos
- Una reclamación (claim) se activa cuando el cliente reporta un siniestro
- Los agentes deben poder crear, consultar y actualizar el estado de una reclamación
- Una reclamación tiene un monto máximo según el tipo de póliza

Genera la Historia de Usuario completa en formato estándar y guárdala en docs/historia-reclamaciones.md
```

### Qué esperar

El agente genera una HU con criterios de aceptación en formato BDD (Given/When/Then), definición de done, y las restricciones de negocio relevantes. Si la salida no incluye criterios de aceptación suficientemente específicos, pídele que los expanda con casos de borde.

---

## 🔬 Módulo 2: Revisión de Historia (10 min)

**Objetivo:** Usar el agente `story-reviewer` para auditar la calidad de la HU antes de que llegue al equipo técnico.

### Activar el agente

Selecciona el agente **story-reviewer**.

### Prompt

```
Revisa la Historia de Usuario en docs/historia-reclamaciones.md y evalúa su calidad.

Analiza:
1. ¿Cumple el formato INVEST (Independent, Negotiable, Valuable, Estimable, Small, Testable)?
2. ¿Los criterios de aceptación son verificables y no ambiguos?
3. ¿Hay dependencias técnicas no documentadas?
4. ¿El alcance es adecuado para un sprint?
5. ¿Faltan reglas de negocio importantes?

Genera el reporte en docs/revision-historia.md con una calificación por cada criterio y las correcciones sugeridas.
```

### Punto de discusión

El revisor puede identificar ambigüedades que el PO no vio. Este es el momento para que los participantes comparen: ¿el agente reviewer encontró algo que el agente PO dejó incompleto?

---

## 🔬 Módulo 3: Revisión Arquitectónica (15 min)

**Objetivo:** Usar el agente `architect` para evaluar el feature propuesto contra la arquitectura existente y generar una propuesta técnica.

### Activar el agente

Selecciona el agente **architect**.

### Prompt

```
Actúa como arquitecto senior. Revisa:
1. La Historia de Usuario en docs/historia-reclamaciones.md
2. La revisión de calidad en docs/revision-historia.md
3. La arquitectura actual del proyecto en ContosoSegurosCR/

Evalúa:
- ¿Cómo encaja el módulo de reclamaciones en la arquitectura existente de Minimal APIs?
- ¿Qué modelos, servicios y endpoints son necesarios?
- ¿Hay riesgos de integridad referencial entre Reclamaciones y Pólizas?
- ¿Qué validaciones de negocio deben ser parte del servicio vs. la capa de presentación?
- ¿Hay implicaciones de rendimiento con datos de reclamaciones en memoria?

Genera la propuesta técnica en docs/propuesta-arquitectura.md incluyendo el diagrama de modelos en texto ASCII.
```

---

## 🔬 Módulo 4: Implementación (25 min)

**Objetivo:** Usar el agente `implementer` para construir el feature según los artefactos de las etapas anteriores.

### Activar el agente

Selecciona el agente **implementer**.

### Prompt

```
Implementa el módulo de reclamaciones para Contoso Seguros CR basándote en:
- Historia de Usuario: docs/historia-reclamaciones.md
- Propuesta de arquitectura: docs/propuesta-arquitectura.md
- Estándares del proyecto: .github/copilot-instructions.md

Crea:
1. ContosoSegurosCR/Models/Reclamacion.cs — modelo y enumeraciones de estado
2. ContosoSegurosCR/DTOs/ReclamacionDto.cs — DTOs para crear y actualizar
3. ContosoSegurosCR/Services/ReclamacionServicio.cs — lógica de negocio con validaciones
4. Endpoints en ContosoSegurosCR/Program.cs — grupo /api/reclamaciones con documentación OpenAPI

Sigue exactamente el mismo patrón que Poliza.cs y PolizaServicio.cs.
Todo en español. Datos de ejemplo precargados.
```

### Iteración esperada

Es normal que el primer output requiera ajustes. Si el agente implementa algo que no alinea con la propuesta arquitectónica, señálalo explícitamente en el siguiente prompt. Esta iteración es parte del demo.

---

## 🔬 Módulo 5: Casos de Prueba (15 min)

**Objetivo:** Usar el agente `qa-engineer` para generar una suite de pruebas completa.

### Activar el agente

Selecciona el agente **qa-engineer**.

### Prompt

```
Genera una suite de pruebas completa para el módulo de reclamaciones basándote en:
- Historia de Usuario con criterios de aceptación: docs/historia-reclamaciones.md
- Implementación: ContosoSegurosCR/Services/ReclamacionServicio.cs

Crea en ContosoSegurosCR.Tests/:
1. ReclamacionServicioTests.cs — pruebas unitarias de la lógica de negocio
   - Todos los criterios de aceptación de la HU cubiertos
   - Casos positivos Y casos de borde/error
2. ReclamacionIntegrationTests.cs — pruebas de integración con WebApplicationFactory
   - Validar códigos HTTP correctos por escenario
   - Validar que las validaciones de negocio retornan 422 (no 500)

Usa xUnit y el mismo patrón que los tests existentes en el proyecto.
```

---

## 🔬 Módulo 6: Auditoría de Seguridad (10 min)

**Objetivo:** Usar el agente `security-auditor` para identificar riesgos de seguridad en el feature implementado.

### Activar el agente

Selecciona el agente **security-auditor**.

### Prompt

```
Realiza una auditoría de seguridad del módulo de reclamaciones implementado.

Revisa:
- ContosoSegurosCR/Services/ReclamacionServicio.cs
- ContosoSegurosCR/Models/Reclamacion.cs  
- Los endpoints de reclamaciones en ContosoSegurosCR/Program.cs

Evalúa específicamente:
1. Validación de inputs — ¿hay riesgo de datos malformados que rompen la lógica?
2. Autorización — ¿los endpoints verifican que el agente puede acceder a esa póliza?
3. Exposición de datos — ¿los DTOs exponen campos que no deberían ser públicos?
4. Reglas de negocio como controles de seguridad — ¿el monto máximo está validado server-side?
5. Logging — ¿hay trazabilidad suficiente para auditoría regulatoria?

Genera el reporte en docs/reporte-seguridad.md con severidad por hallazgo (Alta/Media/Baja) y recomendaciones concretas con código.
```

---

## ✅ Cierre (5 min)

Al terminar el workshop debes tener:

**Documentación generada por agentes:**
- `docs/historia-reclamaciones.md` — HU con criterios de aceptación BDD
- `docs/revision-historia.md` — Reporte de calidad INVEST
- `docs/propuesta-arquitectura.md` — Diseño técnico con modelos
- `docs/reporte-seguridad.md` — Auditoría con hallazgos y severidades

**Código implementado:**
- `ContosoSegurosCR/Models/Reclamacion.cs`
- `ContosoSegurosCR/DTOs/ReclamacionDto.cs`
- `ContosoSegurosCR/Services/ReclamacionServicio.cs`
- Endpoints registrados en `Program.cs`

**Pruebas:**
- `ContosoSegurosCR.Tests/ReclamacionServicioTests.cs`
- `ContosoSegurosCR.Tests/ReclamacionIntegrationTests.cs`
- Todos los tests pasando con `dotnet test`

---

## 📚 Recursos

- [GitHub Copilot Custom Agents](https://docs.github.com/en/copilot/how-tos/copilot-on-github/customize-copilot/customize-cloud-agent/create-custom-agents)
- [Custom Agents Configuration Reference](https://docs.github.com/en/copilot/reference/custom-agents-configuration)
- [Awesome Copilot — ejemplos de la comunidad](https://github.com/github/awesome-copilot)
- [Minimal APIs .NET 8](https://learn.microsoft.com/aspnet/core/fundamentals/minimal-apis/overview)
