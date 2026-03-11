# Seeds iniciales

Archivos:

- `001_seed_core_iam.sql`
- `002_seed_iam_catalogos_auth.sql`

Objetivo:

- Cargar un set minimo e idempotente para pruebas de endpoints (`tenant`, `empresa`, `usuario`, `rol`, `permisos`, `recursos_ui`, `sesion`).

Datos de prueba creados/actualizados:

- Tenant: `SEED`
- Empresa: `SEED-ERP`
- Usuario: `admin.seed` (`login_normalizado = ADMIN.SEED`)
- MFA habilitado para `admin.seed` (factor OTP base)
- Rol: `ADMIN`
- Permisos:
  - `SEG.USUARIO.LISTAR`
  - `SEG.USUARIO.CREAR`
  - `ORG.EMPRESA.LISTAR`
- Recursos UI:
  - `NAV.HOME`
  - `NAV.USUARIOS`
  - `NAV.EMPRESAS`
- Catalogos IAM para autenticacion:
  - `catalogo.canal_notificacion` (`EMAIL`, `SMS`)
  - `catalogo.proposito_desafio_mfa` (`LOGIN`)
  - `catalogo.tipo_verificacion_restablecimiento` (`EMAIL`)
  - `catalogo.tipo_factor_mfa` (`OTP`)

Notas:

- El script es idempotente: se puede ejecutar multiples veces sin duplicar el set seed.
- Ejecutar en orden: `001` y luego `002`.
- La credencial de `admin.seed` se guarda como hash/salt de prueba (`algoritmo_clave = SHA2_512`, `iteraciones_clave = 100000`).
