# Proceso SRI

Este repositorio corresponde al proyecto **Proceso SRI**, un Proof of Concept (POC) cuyo objetivo es **aprender y aplicar IA, agentes y automatización** de forma dirigida y gobernada.

Este proyecto **no está enfocado en construir un producto final**, sino en aprender **cómo dirigir sistemas con IA**, no solo cómo “usarla”.

---

## Objetivo del proyecto

Aprender a:

- Dirigir un agente de desarrollo (IA) como si fuera un JR
- Separar **decisión** de **ejecución**
- Gobernar IA con reglas, memoria y límites claros
- Integrar IA, automatización (RPA) y revisión humana
- Automatizar procesos reales cuando no existen APIs públicas

La IA **no gobierna el sistema**.  
La IA **ejecuta bajo control humano**.

---

## Estado actual del proyecto

**Sprint activo:** Sprint 0  
**Estado:** Gobierno y preparación del repositorio  
**Código runtime:** No existe aún  
**Arquitectura:** Definida a nivel conceptual

En Sprint 0 **no se desarrolla funcionalidad**.  
Solo se definen reglas, estructura y forma de trabajo.

---

## Flujos del sistema

- **Flow V1**  
  Flujo funcional base del sistema.  
  Está **congelado** y no se modifica.

- **Flow V2**  
  Flujo operativo actual (desarrollo + runtime).  
  Incluye usuario, agente programador, GitHub y sistema runtime.

Flow V2 es el flujo vigente de trabajo.

---

## Planos del proyecto

El proyecto se divide en dos planos claros:

### Control Plane (`/pepe`)
Gobierno del agente de desarrollo.

Aquí viven:
- Contexto del agente
- Rol y límites
- Memoria de decisiones

Este plano:
- No se ejecuta
- No es runtime
- No toca datos reales

### Product Plane (`/src`)
Aquí vivirá el sistema real:
- UI
- Lógica de negocio
- IA runtime
- Automatización
- Persistencia

---

## El agente Pepe

**Pepe** es el agente programador del proyecto.

- Es un **agente dev-time**
- Se comporta como un **JR ejecutor**
- No decide arquitectura
- No redefine flujos
- No actúa sin contexto ni aprobación

Su gobierno está definido en:

