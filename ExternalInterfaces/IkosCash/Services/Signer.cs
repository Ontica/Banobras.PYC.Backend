/* Empiria Connector *****************************************************************************************
*                                                                                                            *
*  Module   : Transaction Management                     Component : Interface adapters                      *
*  Assembly : Ikos.Cash.Connector.dll                    Pattern   : Data Transfer Object                    *
*  Type     : TokenDto                                   License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : DTO used to get ikos cash transactionstatus.                                                   *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace Banobras.PYC.ExternalInterfaces.IkosCash {

  static public class Signer {

    #region Properties

    static private readonly string certPath = "d:\\mi_certificado.pfx";

    #endregion Properties

    #region Public methods

    static public string GetSerialNumber() {
      X509Certificate2 x509 = LoadCertificate();

      return x509.SerialNumber;
    }


    static internal byte[] Sign(string data) {

      X509Certificate2 cert = LoadCertificate();

      byte[] dataToSign = Encoding.UTF8.GetBytes(data);

      byte[] signedData = SignData(dataToSign, cert);

      return signedData;
    }

    #endregion Public methods

    #region Helpers

    static private X509Certificate2 LoadCertificate() {
      return new X509Certificate2(certPath, "Abcd234");
    }


    static private byte[] SignData(byte[] data, X509Certificate2 cert) {
      using (RSA rsa = cert.GetRSAPrivateKey()) {
        if (rsa == null) {
          throw new InvalidOperationException("El certificado no contiene una clave privada RSA.");
        }
        return rsa.SignData(data, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);
      }

    }

    #endregion Helpers

  } // class Signer 


} // namespace Banobras.PYC.ExternalInterfaces.IkosCash
