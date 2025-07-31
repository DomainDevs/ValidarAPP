using Sistran.Company.ExternalPrinterServices;
using Sistran.Company.ExternalPrinterServices.Models;
using System;
using System.ServiceModel;

namespace Sistran.Company.ExternalPrinterServicesEEProvider
{
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, InstanceContextMode = InstanceContextMode.PerCall)]
    public class ExternalPrinterServicesEEProvider : IExternalPrinterService
    {
        public ConsulBenefPolicyResponse ConsulBenefPolicy(PolicyPrinterClass consulBenefPolicyRequest)
        {
            return new ConsulBenefPolicyResponse
            {
                ListBenefPolicyClass = new System.Collections.Generic.List<ListBenefPolicyClass>
               {
                   new ListBenefPolicyClass
                   {
                       Apellido1 = "CUERVO ",
                       Apellido2 = "CUERVO",
                       Cod_Ramo= "7",
                       Cod_Suc = "9",
                       Desc_Ramo="AUTOMOVILES",
                       Desc_Suc="MANIZALES",
                       Direccion_Beneficiario = "CL 51 NO. 7 - 50, SANTAFE DE BOGOTA DC",
                       Nombre= "CARLOS MANUEL",
                       Nro_Endoso="1",
                       Nro_Identificacion="1.704.680",
                       Nro_Poliza="3009284",
                       Tipo_Identificacion="CC"

                   }
               }
            };
        }

        public ConsultCoveragePolicyResponse ConsultCoveragePolicy(PolicyPrinterClass consultCoveragePolicyRequest)
        {
            return new ConsultCoveragePolicyResponse
            {
                ListCoveragePolicyClass = new System.Collections.Generic.List<ListCoveragePolicyClass>
              {
                  new ListCoveragePolicyClass
                  {
                       Cod_cobertura = "",
                       Cod_ramo = "",
                       Cod_suc="9",
                       Deducible_cobertura="",
                       Descripcion_Cobertura="",
                       Descripcion_Riesgo = "",
                       Desc_ramo= "AUTOMOVILES",
                       Desc_suc ="MANIZALES",
                       Estado_cobertura="",
                       Nro_pol="3009284",
                       Nro_riesgo="",
                       Suma_asegurada="450.486,00",
                       Suma_asegurada_acum="466.797,92"

                  }
              }
            };
        }

        public ConsultDataBasicPolicyResponse ConsultDataBasicPolicy(PolicyPrinterClass consultDataBasicPolicyRequest)
        {
            return new ConsultDataBasicPolicyResponse
            {
                Nombre_producto = "AU PREVICOLECTIVAS",
                Nombre_cli = "FISCALIA",
                Nro_pol = "3009284",
                Anio_emi_endo = "2018",
                Apellido1_cli = "GENERAL",
                Apellido2_cli = "DE LA NACION",
                Clave_int = "2",
                Cod_cli = "",
                Cod_ramo = "7",
                Cod_suc = "9",
                Desc_ramo = "AUTOMOVILES",
                Desc_suc = "MANIZALES",
                Fec_fin_vig = "2018",
                Fec_ini_vig = "2017",
                Marca = "RENAULT",
                Model = "2004",
                Nombre_agente = "CUERVO CARLOS MANUEL",
                Nro_doc = "1.704.680",
                Nro_doc_agente = "80.015.278",
                Nro_endo = "",
                Periodicidad_prima = "",
                Placa = "ABB0066",
                ProcessMessage = "",
                Tipo_doc = "NIT",
                Tipo_doc_agente = "1.704.680",
                Tipo_pol = "100% COMPAÑIA",
                Valor_prima = "466.797,92",
                Val_ahorro = "24.999,96",
                Val_aseg = "8.000.000"
            };
        }

        public ConsultDataClientResponse ConsultDataClient(ConsultDataClientRequest consultDataClientRequest)
        {
            return new ConsultDataClientResponse
            {
                Ciudad = "MANIZALES",
                Codigo_Cliente = "4568",
                Correo_Electronico = "www.axacolpatria.co,",
                Departamento = "CALDAS",
                Direccion = "CALLE 10 N. 63 46",
                Estado_Civil = "SOLTERO",
                Nombre = "CARLOS MANUEL",
                Nro_Doc = "1.704.680",
                Primer_Apellido = "CUERVO",
                Segundo_Apellido = "CUERVO",
                Sexo = "M",
                Tipo_Doc = "CC",
                Tipo_Persona = "NATURAL"
            };
        }

        public ConsultMultiriskHomePolicyResponse ConsultMultiriskHomePolicy(PolicyPrinterClass policyPrinterClass)
        {
            return new ConsultMultiriskHomePolicyResponse
            {
                MultiriskHomeClass = new System.Collections.Generic.List<MultiriskHomeClass>
                {
                    new MultiriskHomeClass
                    {
                        Cod_ciudad_vivienda = "2",
                        Cod_departamento_vivienda= "30",
                        Cod_pais_vivienda="1",
                        Desc_ciudad_vivienda="LA PEDRERA",
                        Desc_departamento_vivienda="AMAZONAS",
                        Desc_pais_vivienda="COLOMBIA",
                        Direccion_vivienda="CALLE 10 N. 63 46",
                        Id_vivienda="",
                        Tipo_Vivienda="RURAL",
                        Valor_contenido="106.656,00",
                        Valor_vivienda="773.256,00"
                    }
                }
            };
        }

        public PrinterClass GenerateWSPolicyFormatCollect(GenerateWSPolicyFormatCollectRequest generateWSPolicyFormatCollectRequest)
        {
            return new PrinterClass
            {
                Message = "",
                PrinterBinary = "",
                ProcessMessage = "",
                XMLFTP = ""
            };
        }

        public GenerateWSPolicyPrinterResponse GenerateWSPolicyPrinter(GenerateWSPolicyPrinterRequest generateWSPolicyPrinterRequest)
        {
            return new GenerateWSPolicyPrinterResponse
            {
                ProcessMessage = "",
                AllCertificate = 0,
                AllInformation = 0,
                AllInsured = 0,
                Annexes = 0,
                BranchId = 30,
                Certificate = 0,
                CertificateSince = 0,
                CertificateUntil = 0,
                Clauses = 0,
                Coverages = 0,
                DocumentNumber = 0,
                emailUser = 1,
                EndorsementId = 0,
                Fee = 0,
                FrontPage = 1,
                InsuranceObject = 1,
                LetterInstruction = 1,
                Payment = 1,
                PaymentFormat = 1,
                PrefixNum = 1,
                printBinary = 1,
                YearEndorsement = 2018

            };
        }

        public PrinterClass GenerateWSQuotePrinter(GenerateWSQuotePrinterRequest generateWSQuotePrinterRequest)
        {
            return new PrinterClass
            {
                Message = "",
                PrinterBinary = "",
                ProcessMessage = "",
                XMLFTP = ""
            };
        }

        public GetConsulEndorsementPolicyResponse GetConsulEndorsementPolicy(GetConsulEndorsementPolicyRequest getConsulEndorsementPolicyRequest)
        {
            return new GetConsulEndorsementPolicyResponse
            {
                ListEndorsementPolicyClass = new System.Collections.Generic.List<ListEndorsementPolicyClass>
                {
                    new ListEndorsementPolicyClass
                    {
                        Cod_Ramo = "7",
                        Cod_Suc="9",
                        Desc_Endoso="",
                        Desc_Ramo="AUTOMOVILES",
                        Desc_Suc="MANIZALES",
                        Fec_emi="2018",
                        Fec_vig_desde="2017",
                        Fec_vig_hasta="2018",
                        Nro_Endoso="3009284",
                        Nro_Poliza="3009284",
                        Vlr_Aseg_Endoso="8.000.000",
                        Vlr_Prima_Endoso="466.797,92"
                    }
                }
            };
        }

        public GetConsulRecoveriesPolicyResponse GetConsulRecoveriesPolicy(GetConsulRecoveriesPolicyRequest getConsulRecoveriesPolicyRequest)
        {
            return new GetConsulRecoveriesPolicyResponse
            {
                ListRecoveriesPolicyClass = new System.Collections.Generic.List<ListRecoveriesPolicyClass>
               {
                   new ListRecoveriesPolicyClass
                   {
                       Nro_Poliza = "3009284",
                       Nro_Endoso="3009284",
                       Cod_Ramo="7",
                       Cod_Suc="9",
                       Desc_Ramo="AUTOMOVILES",
                       Desc_Grupo_Endoso = "",
                       Desc_Sucursal="MANIZALES",
                       Fecha_Desde_Mora="2017",
                       Fecha_Proximo_Pago="2018",
                       Fecha_Ultimo_Pago="2018",
                       Saldo_en_Mora="24.999,96",
                       Valor_Total_a_Pagar="50.999,96",
                       Valor_Ultimo_Pago="74.999,96"

                   }
               }
            };
        }

        public GetConsulSinisterPolicyResponse GetConsulSinisterPolicy(GetConsulSinisterPolicyRequest getConsulSinisterPolicyrequest)
        {
            return new GetConsulSinisterPolicyResponse
            {
                ListSinisterPolicyClass = new System.Collections.Generic.List<ListSinisterPolicyClass>
                {
                    new ListSinisterPolicyClass
                    {
                       Clase_pago = "MENSUAL",
                       Cod_Ramo ="7",
                       Cod_Ramo_tec="7",
                       Cod_suc="9",
                       Desc_Amparo="24.999,96",
                       Desc_Ramo_Com="AUTOMOVILES",
                       Desc_Ramo_Tec="AUTOMOVILES",
                       Fecha_Aviso="2018",
                       Fecha_pago="2018",
                       Fec_siniestro="2018",
                       Nom_suc ="MANIZALES",
                       Nro_Endoso="3009284",
                       Nro_pol="3009284",
                       Nro_Reclamo="",
                       Sucursal_reclamo="MANIZALES",
                       Nro_riesgo_sin="",
                       Txt_desc_endo="",
                       Vlr_pagado="466.797,92"

                    }
                }
            };
        }
    }
}
