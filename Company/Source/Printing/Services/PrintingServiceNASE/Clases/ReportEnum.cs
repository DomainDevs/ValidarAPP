using System;

namespace Sistran.Company.PrintingService.NASE.Clases
{
    public class ReportEnum
    {
        public enum PolicyType
        {
            INDIVIDUAL = 1,
            COLLECTIVE = 2,
            CAUTION_ART_513 = 1
        };

        public enum PrefixCode
        {
            AUTOS = 10,
            CROP = 15,
            COMPLIANCE = 30,
            LOCATION = 21,
            CAUTION = 31,
            //TODO:  <<Autor: Nicolas Gonzalez R. - NGR ; Fecha: 12/11/2015; 
            //Asunto: Se agrega funcionalidad para el ramo de arrendamiento
            LEASE = 32,
            // Autor: Nicolas Gonzalez R. - NGR, Fecha: 12/11/2015 >>
            PASSENGER_CROP = 40

        };

        public enum TextLines
        {
            COMPLIANCE_TEXT_LINES = 15,
            MAX_CHAR_PER_LINE = 135,
            MAX_CHAR_PER_PRINT = 8040
        };

        public enum ReportType
        {
            COMPLETE_POLICY = 1,
            ONLY_POLICY = 2,
            PAYMENT_CONVENTION = 3,
            TEMPORARY = 4,
            QUOTATION = 5,
            COMPLETE_REQUEST = 6,
            ONLY_REQUEST = 7,
            ONLY_POLICIES_REQUEST = 8,
            FORMAT_COLLECT = 9,
            MASS_LOAD = 10,
            LICENSE = 11,
            LICENSE_BLANK = 12,
            //TODO:  <<Autor: Luisa Fernanda Ramírez; Fecha: 27/12/2010; Asunto: OT-0051 Renovacion de Autos Individuales. Compañía: 1 
            MASSIVE_RENWAL = 13
            /* Autor: Luisa Fernanda Ramírez, Fecha: 21/12/2010 >>*/
        };

        public enum RiskStatus
        {
            NULL = 0,
            ORIGINAL = 1,
            INCLUDED = 2,
            EXCLUDED = 3,
            REHABILITATED = 4,
            MODIFIED = 5,
            NOTMODIFIED = 6,
            CANCELLATED = 7,
        };

        public enum BarCode
        {
            CONSTANT = 415,
            COUNTRY_CD = 770,
            COMPANY_CD = 733688,
            SERVICE_CD = 001,
            CONVENTION = 8,
            FIELD_ID = 8020,
            PARITY = 0
        }

        public enum FileReportVersion
        {
            VERSION_1 = 1,
            VERSION_2 = 2
        }

        public enum PrefixPolicyType
        {
            RC_PASSENGER = 1
        }
    }
}
