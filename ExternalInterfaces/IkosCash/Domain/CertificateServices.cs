/* Empiria Connector *****************************************************************************************
*                                                                                                            *
*  Module   : Transaction Management                     Component : Interface adapters                      *
*  Assembly : Ikos.Cash.Connector.dll                    Pattern   : Service provider                        *
*  Type     : CertificateServices                        License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Provides X509Certificate-related services.                                                     *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace Empiria.Payments.BanobrasIntegration.IkosCash {

  /// <summary>Provides X509Certificate-related services.</summary>
  internal class CertificateServices {

    #region Fields

    private readonly X509Certificate2 _x509Certificate;

    #endregion Fields

    internal CertificateServices(X509Certificate2 certificate) {
      Assertion.Require(certificate, nameof(certificate));

      _x509Certificate = certificate;
    }

    #region Methods

    public string GetSerialNumber() {
      return _x509Certificate.SerialNumber;
    }


    public string GetSerialNumberBase64() {
      var serial = _x509Certificate.SerialNumber;

      byte[] dataBytes = Encoding.UTF8.GetBytes(serial);

     return Convert.ToBase64String(dataBytes);
    }


    internal string Sign(string data) {
      byte[] dataBytes = Encoding.UTF8.GetBytes(data);

      using (SHA256 sha256 = SHA256.Create()) {
        byte[] hash = sha256.ComputeHash(dataBytes);

        using (RSA rsa = _x509Certificate.GetRSAPrivateKey()) {
          var hashSigned = rsa.SignHash(hash, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);

          return Convert.ToBase64String(hashSigned);
        }
      }
    }

    #endregion Methods

  } // class CertificateServices

} // namespace Banobras.PYC.ExternalInterfaces.IkosCash
