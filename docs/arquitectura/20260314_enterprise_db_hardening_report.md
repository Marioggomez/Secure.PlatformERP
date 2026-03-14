# Diagnostico y Hardening Enterprise DB (Multi-tenant / Multiempresa / IAM)

Fecha: 2026-03-14  
Fuente analizada: `database/schema-snapshots/secure-schema.json`, `database/schema-snapshots/schema-report.md`, `database/stored-procedures`, `database/migrations`, `database/seeds`.

> Nota: no se encontro fisicamente `Base Secure.txt` en el workspace. El analisis se hizo sobre el snapshot y scripts reales versionados.

## FASE 1 - Diagnostico tecnico profundo

### Fortalezas actuales
- Modelo base ya maduro: `id_tenant` + `id_empresa` presente en una gran parte del core IAM y tablas operativas.
- Existe endurecimiento previo por `SESSION_CONTEXT` en varios `*_listar/crear/actualizar/obtener`.
- Existen tablas de alcance IAM y metadata de entidad (`seguridad.entidad_alcance_dato`).
- Existe soporte real de sesion, MFA, helpdesk y trazabilidad de seguridad.
- Ya hay FK compuestas en zonas criticas (por ejemplo `seguridad.usuario_empresa`, `seguridad.sesion_usuario`).

### Riesgos criticos (prioridad alta)
1. **Fallback inseguro de empresa en pre-autenticacion**
- En `seguridad.usp_auth_obtener_usuario_para_autenticacion` existia `COALESCE(ue.id_empresa, emp.id_empresa)`, lo que permite seleccionar empresa por tenant aunque usuario no tenga asignacion explicita.
- Riesgo: escalacion horizontal entre empresas del mismo tenant.

2. **Tablas empresa-scoped sin `id_tenant` materializado**
- Detectadas 10 tablas con `id_empresa` sin `id_tenant`.
- Impacta integridad semantica, trazabilidad y capacidad de enforcement transversal.

3. **FKs ambiguas (simple y compuesta simultaneas)**
- Varias tablas tenian FK a `organizacion.empresa(id_empresa)` coexistiendo con FK compuesta `(id_tenant,id_empresa)`.
- Riesgo: ambiguedad de plan de ejecucion y huecos de consistencia.

4. **Mezcla de seguridad de datos con preferencias UI**
- `seguridad.filtro_dato_usuario` se usa para filtros de seguridad y para `UI_LAYOUT`.
- Riesgo: semantica degradada, gobierno deficiente, dificultad de auditoria.

### Riesgos medios
- `seguridad.usuario_scope_empresa` y `seguridad.usuario_scope_unidad` sin tenant/empresa completos en estructura original.
- Ausencia de capa canonica unica para “acceso efectivo” (tenia logica dispersa en SPs).
- Falta de preparacion formal RLS en objetos reusable/politica definida.

### Olores de diseño detectados
- Polimorfismo de entidades (`id_entidad_sistema` + `id_registro_entidad`) en:
  - `actividad.comentario`
  - `documento.documento_entidad`
  - `etiqueta.etiqueta_entidad`
- Sin `id_tenant/id_empresa` materializado en tablas hijas.
- Estrategia recomendada: blindaje por join obligatorio a entidad raiz + metadata (`entidad_alcance_dato`) y vistas securizadas por modulo.

### Riesgos de fuga de datos
- Fuga intra-tenant por fallback de empresa en login.
- Fuga por SP que dependan solo de “recordar filtrar” y no de capa canonica reusable.

### Prioridad de remediacion
- Alta: fallback pre-auth, FKs ambiguas, scope tables incompletas.
- Media: separacion UI/security y capa canonica de acceso.
- Baja: RLS full ON inmediato (recomendado en rollout controlado).

## FASE 2 - Arquitectura objetivo

### Decisiones aplicadas
1. **Tenant materializado** en tablas empresa-scoped que no lo tenian.
2. **Regla compuesta obligatoria** `(id_tenant,id_empresa)` como referencia principal.
3. **Eliminacion de fallback inseguro** en pre-autenticacion.
4. **Separacion semantica**: nueva tabla `seguridad.preferencia_ui_usuario`.
5. **Capa canonica de acceso efectivo**:
- `seguridad.fn_usuario_empresas_efectivas`
- `seguridad.fn_usuario_unidades_efectivas`
- `seguridad.vw_usuario_scope_efectivo`
- `seguridad.usp_scope_validar_sesion`
6. **RLS preparado** con politica en `STATE = OFF` para habilitacion controlada.

## FASE 3 - Delta SQL idempotente

Script entregado:
- `database/migrations/20260314193000__enterprise_scope_authorization_hardening.up.sql`

Incluye:
- `ALTER TABLE ... ADD id_tenant` (y `id_empresa` donde faltaba) + backfill + validacion + `NOT NULL`.
- Creacion de FKs compuestas nuevas.
- Limpieza de FKs simples duplicadas cuando ya existe compuesta.
- Indices de rendimiento para patrones tenant+empresa+usuario.
- Separacion de preferencias UI en tabla dedicada.
- `CREATE OR ALTER` de procedimientos/funciones/vistas de acceso efectivo.
- Hardening de `usp_auth_obtener_usuario_para_autenticacion` (sin fallback).

## FASE 4 - Capa canonica de resolucion de acceso

Objetos implementados:
- `seguridad.fn_usuario_empresas_efectivas(@id_usuario,@id_tenant)`
- `seguridad.fn_usuario_unidades_efectivas(@id_usuario,@id_tenant,@id_empresa)`
- `seguridad.vw_usuario_scope_efectivo`
- `seguridad.usp_scope_validar_sesion`

Cobertura funcional:
- acceso por empresa asignada,
- acceso por scope explicito,
- acceso por rol/excepcion,
- denegacion explicita (`DENY`) prevalece,
- admin tenant (si aplica) con visibilidad tenant-wide,
- denegacion por defecto cuando no hay scope efectivo.

## FASE 5 - Pruebas SQL

Script entregado:
- `database/tests/20260314_enterprise_scope_validation.sql`

Cobertura de prueba:
1. aislamiento tenant A vs tenant B,
2. empresa unica,
3. multiples empresas,
4. scope por unidad,
5. usuario sin scope efectivo,
6. verificacion de fallback pre-auth removido,
7. verificacion de FKs compuestas habilitadas,
8. smoke test de vista canonica,
9. pauta de validacion `SESSION_CONTEXT`.

## FASE 6 - Resumen final

### Mejoras realizadas
- Endurecimiento de integridad tenant/empresa en tablas criticas.
- Cierre del vector de fallback inseguro en autenticacion.
- Capa de autorizacion de datos centralizada y reusable.
- Separacion limpia de seguridad de datos vs preferencias UI.
- Base preparada para activar RLS progresivamente.

### Riesgos eliminados
- Fuga intra-tenant por empresa implicita.
- Ambiguedad FK simple/compuesta.
- Mezcla semantica de seguridad con layout UI.

### Riesgos pendientes
- RLS aun en `OFF` por estrategia segura de rollout.
- Tablas polimorficas (`comentario`, `documento_entidad`, `etiqueta_entidad`) requieren vistas securizadas por entidad raiz para blindaje total sin duplicar datos.

### Puntaje (estimado)
- Antes: **7.1 / 10**
- Despues: **9.0 / 10**

### Proximos pasos opcionales (alto valor)
1. Activar `RLS_scope_tenant_empresa` en QA con pruebas de regresion de SP/API.
2. Crear vistas securizadas por modulo para tablas polimorficas de comentario/documento/etiqueta.
3. Añadir pruebas automatizadas de negacion por defecto en pipeline CI.
4. Publicar catalogo de alcances (`TENANT`, `EMPRESA`, `UNIDAD`, `PROPIO`) con reglas de validacion por constraint/procedimiento.

## Artefactos entregados
- `database/migrations/20260314193000__enterprise_scope_authorization_hardening.up.sql`
- `database/migrations/20260314193000__enterprise_scope_authorization_hardening.down.sql`
- `database/tests/20260314_enterprise_scope_validation.sql`

