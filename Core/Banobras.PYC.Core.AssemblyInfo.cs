/* BANOBRAS **************************************************************************************************
*                                                                                                            *
*  System   : Sistema PYC (Planeación y control)                 Module  : BANOBRAS PYC Core                 *
*  Assembly : Banobras.PYC.Core.dll                              Pattern : Assembly Attributes File          *
*                                                                License : Please read LICENSE.txt file      *
*                                                                                                            *
*  Summary  : Este módulo contiene el código con los casos de uso específicos del Sistema PYC de BANOBRAS,   *
*             así como código para integrar los casos de uso y componentes de Empiria Budgeting, Empiria     *
*             CashFlow y Empiria Payments.                                                                   *
*                                                                                                            *
*             Así mismo, se enlaza con Banobras.PYC.ExternalInterfaces para obtener información proveniente  *
*             de otros sistemas del Banco, como es el caso de los sistemas SICOFIN, SIC, SIMEFIN y CFDI.     *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;
using System.Reflection;
using System.Runtime.CompilerServices;

/*************************************************************************************************************
* Assembly configuration attributes.                                                                         *
*************************************************************************************************************/
[assembly: AssemblyTrademark("Empiria and Ontica are either trademarks of La Vía Óntica SC or Ontica LLC.")]
[assembly: AssemblyCulture("")]
[assembly: CLSCompliant(true)]
[assembly: InternalsVisibleTo("Banobras.PYC.Core.Tests")]
