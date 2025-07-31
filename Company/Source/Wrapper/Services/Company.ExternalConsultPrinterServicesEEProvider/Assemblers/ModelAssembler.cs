using System;
using System.Collections.Generic;
using Sistran.Company.ExternalConsultPrinterServices.Models;
using Sistran.Company.ExternalConsultPrinterServicesEEProvider.WSPolicyPrinter;

namespace Sistran.Company.ExternalPrinterServicesEEProvider.Assemblers
{
    public class ModelAssembler
    {
        public static ConsulBenefPolicyResponse GenerateBenefPolicy(BenefPolicy benefPolicy)
        {
            ConsulBenefPolicyResponse consulBenefPolicyResponse = new ConsulBenefPolicyResponse();
            List<ListBenefPolicyClass> listBenefPolicyClass = new List<ListBenefPolicyClass>();
            foreach (var item in benefPolicy.ListBenefPolicyClass)
            {
                listBenefPolicyClass.Add(BeneficiaryPolicy(item));
            }
            consulBenefPolicyResponse.ListBenefPolicyClass = listBenefPolicyClass;
            consulBenefPolicyResponse.ProcessMessage = benefPolicy.ProcessMessage;

            return consulBenefPolicyResponse;
        }

        private static ListBenefPolicyClass BeneficiaryPolicy(BenefPolicyClass item) => new ListBenefPolicyClass
        {
            Apellido1 = item.Apellido1,
            Apellido2 = item.Apellido2,
            Cod_Ramo = item.Cod_Ramo,
            Cod_Suc = item.Cod_Suc,
            Desc_Ramo = item.Desc_Ramo,
            Desc_Suc = item.Desc_Suc,
            Direccion_Beneficiario = item.Direccion_Beneficiario,
            Nombre = item.Nombre,
            Nro_Endoso = item.Nro_Endoso,
            Nro_Identificacion = item.Nro_Identificacion,
            Nro_Poliza = item.Nro_Poliza,
            Tipo_Identificacion = item.Tipo_Identificacion
        };

        public static ConsultCoveragePolicyResponse ConsultCoveragesPolicy(CoveragePolicy coveragePolicy)
        {
            ConsultCoveragePolicyResponse consultCoveragePolicyResponse = new ConsultCoveragePolicyResponse();
            List<ListCoveragePolicyClass> listCoveragePolicyClass = new List<ListCoveragePolicyClass>();

            foreach (var item in coveragePolicy.ListCoveragePolicyClass)
            {
                listCoveragePolicyClass.Add(CoveragePolicy(item));
            }
            consultCoveragePolicyResponse.ListCoveragePolicyClass = listCoveragePolicyClass;
            consultCoveragePolicyResponse.ProcessMessage = coveragePolicy.ProcessMessage;

            return consultCoveragePolicyResponse;
        }

        public static ConsultMultiriskHomePolicyResponse ConsultMultiriskHomePolicies(ExternalConsultPrinterServicesEEProvider.WSPolicyPrinter.MultiriskHomeClass[] multiriskHome, string message)
        {
            ConsultMultiriskHomePolicyResponse consultMultiriskHomePolicyResponse = new ConsultMultiriskHomePolicyResponse();
            List<ExternalConsultPrinterServices.Models.MultiriskHomeClass> multiriskHomeClass = new List<ExternalConsultPrinterServices.Models.MultiriskHomeClass>();
            foreach (var item in multiriskHome)
            {
                multiriskHomeClass.Add(ConsultMultiriskHomePolicy(item));
            }
            consultMultiriskHomePolicyResponse.MultiriskHomeClass = multiriskHomeClass;
            consultMultiriskHomePolicyResponse.ProcessMessage = message;

            return consultMultiriskHomePolicyResponse;
        }
        
        private static ExternalConsultPrinterServices.Models.MultiriskHomeClass ConsultMultiriskHomePolicy(ExternalConsultPrinterServicesEEProvider.WSPolicyPrinter.MultiriskHomeClass item) => new ExternalConsultPrinterServices.Models.MultiriskHomeClass
        {
            Cod_ciudad_vivienda = item.Cod_ciudad_vivienda,
            Cod_departamento_vivienda = item.Cod_departamento_vivienda,
            Cod_pais_vivienda = item.Cod_pais_vivienda,
            Desc_ciudad_vivienda = item.Desc_ciudad_vivienda,
            Desc_departamento_vivienda = item.Desc_departamento_vivienda,
            Desc_pais_vivienda = item.Desc_pais_vivienda,
            Direccion_vivienda = item.Desc_pais_vivienda,
            Id_vivienda = item.Desc_pais_vivienda,
            Tipo_Vivienda = item.Desc_pais_vivienda,
            Valor_contenido = item.Desc_pais_vivienda,
            Valor_vivienda = item.Desc_pais_vivienda
        };

        public static ConsultDataClientResponse ConsultDataClients(DatesBasicClientClass client) => new ConsultDataClientResponse
        {
            Ciudad = client.Ciudad,
            Codigo_Cliente = client.Codigo_Cliente,
            Correo_Electronico = client.Correo_Electronico,
            Departamento = client.Departamento,
            Direccion = client.Direccion,
            Estado_Civil = client.Estado_Civil,
            Nombre = client.Nombre,
            Nro_Doc = client.Nro_Doc,
            Primer_Apellido = client.Primer_Apellido,
            ProcessMessage = "",
            Segundo_Apellido = client.Segundo_Apellido,
            Sexo = client.Sexo,
            Tipo_Doc = client.Tipo_Doc,
            Tipo_Persona = client.Tipo_Persona
        };
        
        public static GetConsulEndorsementPolicyResponse GetConsulEndorsementPolicies(EndorsementPolicyClass[] listEndorsementPolicy, string message)
        {
            GetConsulEndorsementPolicyResponse getConsulEndorsementPolicyResponse = new GetConsulEndorsementPolicyResponse();
            List<ListEndorsementPolicyClass> listEndorsementPolicyClass = new List<ListEndorsementPolicyClass>();
            foreach (var item in listEndorsementPolicy)
            {
                listEndorsementPolicyClass.Add(GetConsulEndorsementPolicy(item));
            }
            getConsulEndorsementPolicyResponse.ListEndorsementPolicyClass = listEndorsementPolicyClass;
            getConsulEndorsementPolicyResponse.ProcessMessage = message;

            return getConsulEndorsementPolicyResponse;
        }

        public static GetConsulRecoveriesPolicyResponse GetConsulRecoveriesPolicies(RecoveriesPolicyClass[] listRecoveriesPolicy, string message)
        {
            GetConsulRecoveriesPolicyResponse getConsulRecoveriesPolicyResponse = new GetConsulRecoveriesPolicyResponse();
            List<ListRecoveriesPolicyClass> listRecoveriesPolicyClass = new List<ListRecoveriesPolicyClass>();

            foreach (var item in listRecoveriesPolicy)
            {
                listRecoveriesPolicyClass.Add(GetConsulRecoveriesPolicy(item));
            }
            getConsulRecoveriesPolicyResponse.ListRecoveriesPolicyClass = listRecoveriesPolicyClass;
            getConsulRecoveriesPolicyResponse.ProcessMessage = message;

            return getConsulRecoveriesPolicyResponse;
        }

        public static GetConsulSinisterPolicyResponse GetConsulSinisterPolicies(SinisterPolicyClass[] listSinisterPolicy, string message)
        {
            GetConsulSinisterPolicyResponse getConsulSinisterPolicyResponse = new GetConsulSinisterPolicyResponse();
            List<ListSinisterPolicyClass> listSinisterPolicyClass = new List<ListSinisterPolicyClass>();
            foreach (var item in listSinisterPolicy)
            {
                listSinisterPolicyClass.Add(GetConsulSinisterPolicy(item));
            }
            getConsulSinisterPolicyResponse.ListSinisterPolicyClass = listSinisterPolicyClass;
            getConsulSinisterPolicyResponse.ProcessMessage = message;

            return getConsulSinisterPolicyResponse;
        }

        private static ListSinisterPolicyClass GetConsulSinisterPolicy(SinisterPolicyClass item) => new ListSinisterPolicyClass
        {
            Clase_pago = item.clase_pago,
            Cod_Ramo = item.Cod_Ramo,
            Cod_Ramo_tec = item.Cod_Ramo_tec,
            Cod_suc = item.cod_suc,
            Desc_Amparo = item.Desc_Amparo,
            Desc_Ramo_Com = item.Desc_Ramo_Com,
            Desc_Ramo_Tec = item.Desc_Ramo_Tec,
            Fecha_Aviso = item.Fecha_Aviso,
            Fecha_pago = item.Fecha_pago,
            Fec_siniestro = item.fec_siniestro,
            Nom_suc = item.nom_suc,
            Nro_Endoso = item.Nro_Endoso,
            Nro_pol = item.Nro_pol,
            Nro_Reclamo = item.Nro_Reclamo,
            Nro_riesgo_sin = item.nro_riesgo_sin,
            Sucursal_reclamo = item.Sucursal_reclamo,
            Txt_desc_endo = item.txt_desc_endo,
            Vlr_pagado = item.vlr_pagado
        };

        private static ListRecoveriesPolicyClass GetConsulRecoveriesPolicy(RecoveriesPolicyClass item) => new ListRecoveriesPolicyClass
        {
            Cod_Ramo = item.Cod_Ramo,
            Cod_Suc = item.Cod_Suc,
            Desc_Grupo_Endoso = item.Desc_Grupo_Endoso,
            Desc_Ramo = item.Desc_Ramo,
            Desc_Sucursal = item.Desc_Sucursal,
            Fecha_Desde_Mora = item.Fecha_Desde_Mora,
            Fecha_Proximo_Pago = item.Fecha_Proximo_Pago,
            Fecha_Ultimo_Pago = item.Fecha_Ultimo_Pago,
            Nro_Endoso = item.Nro_Endoso,
            Nro_Poliza = item.Nro_Poliza,
            Saldo_en_Mora = item.Saldo_en_Mora,
            Valor_Total_a_Pagar = item.Valor_Total_a_Pagar,
            Valor_Ultimo_Pago = item.Valor_Ultimo_Pago
        };

        private static ListEndorsementPolicyClass GetConsulEndorsementPolicy(EndorsementPolicyClass item) => new ListEndorsementPolicyClass
        {
            Cod_Ramo = item.Cod_Ramo,
            Cod_Suc = item.Cod_Suc,
            Desc_Endoso = item.Desc_Endoso,
            Desc_Ramo = item.Desc_Ramo,
            Desc_Suc = item.Desc_Suc,
            Fec_emi = item.fec_emi,
            Fec_vig_desde = item.fec_vig_desde,
            Fec_vig_hasta = item.fec_vig_hasta,
            Nro_Endoso = item.Nro_Endoso,
            Nro_Poliza = item.Nro_Poliza,
            Vlr_Aseg_Endoso = item.Vlr_Aseg_Endoso,
            Vlr_Prima_Endoso = item.Vlr_Prima_Endoso
        };

        public static ConsultDataBasicPolicyResponse ConsultDataBasicPolicies(DatesBasicsPolicyClass dates) => new ConsultDataBasicPolicyResponse
        {
            Nombre_producto = dates.nombre_producto,
            Nombre_cli = dates.nombre_cli,
            Nro_pol = dates.nro_pol,
            Anio_emi_endo = dates.anio_emi_endo,
            Apellido1_cli = dates.apellido1_cli,
            Apellido2_cli = dates.apellido2_cli,
            Clave_int = dates.clave_int,
            Cod_cli = dates.cod_cli,
            Cod_ramo = dates.cod_ramo,
            Cod_suc = dates.cod_suc,
            Desc_ramo = dates.desc_ramo,
            Desc_suc = dates.cod_suc,
            Fec_fin_vig = dates.fec_fin_vig,
            Fec_ini_vig = dates.fec_ini_vig,
            Marca = dates.marca,
            Model = dates.model,
            Nombre_agente = dates.nombre_agente,
            Nro_doc = dates.nro_doc,
            Nro_doc_agente = dates.nro_doc_agente,
            Nro_endo = dates.nro_endo,
            Periodicidad_prima = dates.periodicidad_prima,
            Placa = dates.placa,
            ProcessMessage = "",
            Tipo_doc = dates.tipo_doc,
            Tipo_doc_agente = dates.tipo_doc_agente,
            Tipo_pol = dates.tipo_pol,
            Valor_prima = dates.valor_prima,
            Val_ahorro = dates.val_ahorro,
            Val_aseg = dates.val_aseg
        };

        private static ListCoveragePolicyClass CoveragePolicy(CoveragePolicyClass item) => new ListCoveragePolicyClass
        {
            Cod_cobertura = item.Cod_cobertura,
            Cod_ramo = item.cod_ramo,
            Cod_suc = item.cod_suc,
            Deducible_cobertura = item.Cod_cobertura,
            Descripcion_Cobertura = item.Cod_cobertura,
            Descripcion_Riesgo = item.descripcion_Riesgo,
            Desc_ramo = item.desc_ramo,
            Desc_suc = item.desc_suc,
            Estado_cobertura = item.Estado_cobertura,
            Nro_pol = item.nro_pol,
            Nro_riesgo = item.nro_riesgo,
            Suma_asegurada = item.Suma_asegurada,
            Suma_asegurada_acum = item.Suma_asegurada_acum
        };
    }
}
