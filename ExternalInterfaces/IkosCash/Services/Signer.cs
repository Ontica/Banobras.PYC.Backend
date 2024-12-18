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
using Empiria;

namespace Banobras.PYC.ExternalInterfaces.IkosCash {

  static public class Signer {

    #region Properties

    static private readonly string certPath = "d:\\1729198545.p12";

    #endregion Properties

    #region Public methods

    static public string GetSerialNumber() {
      X509Certificate2 x509 = LoadCertificate();

      return x509.SerialNumber;
    }


    static public string GetSerialNumberBase64() {
      X509Certificate2 x509 = LoadCertificate();
      var serial = x509.SerialNumber;
      
      byte[] dataBytes = Encoding.UTF8.GetBytes(serial);

     return Convert.ToBase64String(dataBytes);
    }


    static internal string Sign(string data) {
      X509Certificate2 cert = LoadCertificate();

      byte[] dataBytes = Encoding.UTF8.GetBytes(data);

      using (SHA256 sha256 = SHA256.Create()) {
        byte[] hash = sha256.ComputeHash(dataBytes);


        using (RSA rsa = cert.GetRSAPrivateKey()) {
          var hashSigned = rsa.SignHash(hash, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);

          return Convert.ToBase64String(hashSigned);
        }
      }

    }

    #endregion Public methods

    #region Helpers

    static private X509Certificate2 LoadCertificate() {
      string pwd = ConfigurationData.Get<string>("PYC.CertificatePwd");
      return new X509Certificate2(certPath, pwd);
    }
        

    #endregion Helpers

  } // class Signer 


} // namespace Banobras.PYC.ExternalInterfaces.IkosCash
