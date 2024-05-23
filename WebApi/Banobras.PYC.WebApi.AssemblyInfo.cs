/* BANOBRAS **************************************************************************************************
*                                                                                                            *
*  System   : Sistema PYC (Planeación y control)                 Module  : BANOBRAS PYC Web Api              *
*  Assembly : Banobras.PYC.WebApi.dll                            Pattern : Assembly Attributes File          *
*                                                                License : Please read LICENSE.txt file      *
*                                                                                                            *
*  Summary  : El propósito de este módulo es servir como un integrador de diferentes módulos con servicios   *
*             web. A través de estos servicios es que se comunica el backend del Sistema PYC de BANOBRAS     *
*             con otros sistemas, incluyendo la aplicación frontend del propio sistema.                      *
*                                                                                                            *
*             Así mismo, mediante sus componentes es posible adaptar o modificar el comportamiento           *
*             predeterminado de los servicios web, mandando llamar casos de uso y dominios específicos       *
*             a las necesidades más cambiantes del Banco.                                                    *
*                                                                                                            *
*             Este módulo es el que se instala en el servidor de aplicaciones IIS donde se ejecuta           *
*             el backend del Sistema PYC.                                                                    *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;
using System.Reflection;

/*************************************************************************************************************
* Assembly configuration attributes.                                                                         *
*************************************************************************************************************/
[assembly: AssemblyTrademark("Empiria and Ontica are either trademarks of La Vía Óntica SC or Ontica LLC.")]
[assembly: AssemblyCulture("")]
[assembly: CLSCompliant(true)]
