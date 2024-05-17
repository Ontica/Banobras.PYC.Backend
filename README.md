# BANOBRAS - Sistema PYC

[![Codacy Badge](https://app.codacy.com/project/badge/Grade/8b7ef90cedbb4ec5b80e8758bbc99b6b)](https://app.codacy.com/gh/Ontica/Banobras.PYC/dashboard?utm_source=gh&utm_medium=referral&utm_content=&utm_campaign=Badge_grade)
&nbsp; &nbsp;
[![Maintainability](https://api.codeclimate.com/v1/badges/be691fba4b81410d4b98/maintainability)](https://codeclimate.com/github/Ontica/Banobras.PYC/maintainability)

La actualización del **Sistema PYC (Presupuesto y Control)** está siendo desarrollada por
nuestra organización, a la medida de las necesidades del Banco Nacional de Obras y Servicios
Públicos S.N.C (BANOBRAS).

[BANOBRAS](https://www.gob.mx/banobras) es una institución de banca de desarrollo mexicana cuya labor
es financiar obras para la creación de servicios públicos. Por el tamaño de su cartera de crédito directo,
es el cuarto Banco más grande del sistema bancario mexicano y el primero de la Banca de Desarrollo.

El Sistema PYC de BANOBRAS está conformado por tres sistemas integrados que conviven entre sí:

1.  Sistema de control presupuestal (basado en [Empiria Budgeting](https://github.com/Ontica/Empiria.Budgeting)).
2.  Sistema de administración del flujo de efectivo (basado en [Empiria Cashflow](https://github.com/Ontica/Empiria.Cashflow))
3.  Sistema de pago a proveedores (basado en [Empiria Payments](https://github.com/Ontica/Empiria.Payments)).

Los tres sistemas operan sobre el presupuesto del gasto corriente y sobre el presupuesto del programa financiero del Banco.

Todos los servicios, procesos y reglas del negocio que ofrece este sistema son proporcionados mediante
una capa de servicios web.

## Contenido

1.  **Core**
    Contiene el código con los casos de uso específicos del Sistema PYC de
    BANOBRAS, los cuales a su vez integran y utilizan los casos de uso y
    componentes de Empiria Budgeting, Empiria Cashflow y Empiria Payments.

    Así mismo, se enlaza con **External Interfaces** para obtener información
    proveniente de otros sistemas del Banco, como es el caso de los sistemas
    SICOFIN, SIC, SIMEFIN y CFDI.


2.  **External Interfaces**
    Este módulo contiene las interfaces externas cuyo propósito es conectar el
    Sistema PYC de BANOBRAS con otros sistemas del Banco, como es el caso de los
    sistemas SICOFIN, SIC, SIMEFIN y CFDI.

3.  **Web Api**

    A través de estos servicios es que se comunica el backend del Sistema PYC
    con otros sistemas, incluyendo la aplicación frontend del propio sistema.

    Además, sirve como integrador de otros módulos con servicios web
    necesarios para el sistema.

    Este módulo es el que se instala en el servidor de aplicaciones IIS donde
    se ejecuta el backend del Sistema PYC.

## Documentación

De acuerdo a las prácticas de desarrollo ágil, se escribirá e incluirá en este
repositorio en las últimas semanas del proyecto.

## Licencia

Este producto y sus partes se distribuyen mediante una licencia GNU AFFERO
GENERAL PUBLIC LICENSE, para uso exclusivo de BANOBRAS y de su personal, y
también para su uso por cualquier otro organismo en México perteneciente a
la Administración Pública Federal.

Para cualquier otro uso (con excepción a lo estipulado en los Términos de
Servicio de GitHub), es indispensable obtener con nuestra organización una
licencia distinta a esta.

Lo anterior restringe la distribución, copia, modificación, almacenamiento,
instalación, compilación o cualquier otro uso del producto o de sus partes,
a terceros, empresas privadas o a su personal, sean o no proveedores de
servicios de las entidades públicas mencionadas.

El desarrollo, evolución y mantenimiento de este producto está siendo pagado
en su totalidad con recursos públicos, y está protegido por las leyes nacionales
e internacionales de derechos de autor.

## Copyright

Copyright © 2024-2025. La Vía Óntica SC, Ontica LLC y autores.
Todos los derechos reservados.
