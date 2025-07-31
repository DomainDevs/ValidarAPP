// -----------------------------------------------------------------------
// <copyright file="ParamLegalRepresentativeSing.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Andres F. Gonzalez R.</author>
// -----------------------------------------------------------------------
namespace Sistran.Company.Application.UniquePersonServices.Models
{
    using Sistran.Core.Application.Utilities.Enums;
    using Sistran.Core.Application.Utilities.Error;
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    /// <summary>
    /// Contiene las propiedades de Firma Representante Legal
    /// </summary>
    [DataContract]
    public class ParamLegalRepresentativeSing
    {
        /// <summary>
        /// paramCompanyType de Firma Representante Legal.
        /// </summary>
        [DataMember]
        private readonly ParamCompanyType paramCompanyType;

        /// <summary>
        /// paramBranchType de Firma Representante Legal.
        /// </summary>
        [DataMember]
        private readonly ParamBranchType paramBranchType;

        /// <summary>
        /// currentFrom de Firma Representante Legal.
        /// </summary>
        [DataMember]
        private readonly DateTime currentFrom;

        /// <summary>
        /// legalRepresentative de Firma Representante Legal.
        /// </summary>
        [DataMember]
        private readonly string legalRepresentative;

        /// <summary>
        /// pathSignatureImg de Firma Representante Legal.
        /// </summary>
        [DataMember]
        private readonly string pathSignatureImg;

        /// <summary>
        /// signatureImg de Firma Representante Legal.
        /// </summary>
        [DataMember]
        private readonly byte[] signatureImg;

        /// <summary>
        /// userId de Firma Representante Legal.
        /// </summary>
        [DataMember]
        private readonly string userId;

        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="ParamLegalRepresentativeSing"/>.
        /// </summary>
        /// <param name="paramCompanyType">Compañia de Firma Representante Legal.</param>
        /// <param name="paramBranchType">Sucursal de Firma Representante Legal.</param>
        /// <param name="currentFrom">Fecha de Firma Representante Legal.</param>
        /// <param name="legalRepresentative">Representante de Firma Representante Legal.</param>
        /// <param name="pathSignatureImg">Ruta de la imagen de Firma Representante Legal.</param>
        /// <param name="signatureImg">Byte de la imagen de Firma Representante Legal.</param>
        /// <param name="userId">Usuario de Firma Representante Legal.</param>
        private ParamLegalRepresentativeSing(ParamCompanyType paramCompanyType, ParamBranchType paramBranchType, DateTime currentFrom, string legalRepresentative, string pathSignatureImg, byte[] signatureImg, string userId)
        {
            this.paramCompanyType = paramCompanyType;
            this.paramBranchType = paramBranchType;
            this.currentFrom = currentFrom;
            this.legalRepresentative = legalRepresentative;
            this.pathSignatureImg = pathSignatureImg;
            this.signatureImg = signatureImg;
            this.userId = userId;
        }

        /// <summary>
        /// Obtiene el ParamCompanyType de Firma Representante Legal.
        /// </summary>
        [DataMember]
        public ParamCompanyType ParamCompanyType
        {
            get
            {
                return this.paramCompanyType;
            }
        }

        /// <summary>
        /// Obtiene el ParamBranchType de Firma Representante Legal.
        /// </summary>
        [DataMember]
        public ParamBranchType ParamBranchType
        {
            get
            {
                return this.paramBranchType;
            }
        }

        /// <summary>
        /// Obtiene el CurrentFrom de Firma Representante Legal.
        /// </summary>
        [DataMember]
        public DateTime CurrentFrom
        {
            get
            {
                return this.currentFrom;
            }
        }

        /// <summary>
        /// Obtiene el LegalRepresentative de Firma Representante Legal.
        /// </summary>
        [DataMember]
        public string LegalRepresentative
        {
            get
            {
                return this.legalRepresentative;
            }
        }

        /// <summary>
        /// Obtiene el PathSignatureImg de Firma Representante Legal.
        /// </summary>
        [DataMember]
        public string PathSignatureImg
        {
            get
            {
                return this.pathSignatureImg;
            }
        }

        /// <summary>
        /// Obtiene el SignatureImg de Firma Representante Legal.
        /// </summary>
        [DataMember]
        public byte[] SignatureImg
        {
            get
            {
                return this.signatureImg;
            }
        }

        /// <summary>
        /// Obtiene el UserId de Firma Representante Legal.
        /// </summary>
        [DataMember]
        public string UserId
        {
            get
            {
                return this.userId;
            }
        }

        /// <summary>
        /// Objeto que obtiene la Firma Representante Legal.
        /// </summary>
        /// <param name="paramCompanyType">Compañia de Firma Representante Legal.</param>
        /// <param name="paramBranchType">Sucursal de Firma Representante Legal.</param>
        /// <param name="currentFrom">Fecha de Firma Representante Legal.</param>
        /// <param name="legalRepresentative">Representante de Firma Representante Legal.</param>
        /// <param name="pathSignatureImg">Ruta de la imagen de Firma Representante Legal.</param>
        /// <param name="signatureImg">Byte de la imagen de Firma Representante Legal.</param>
        /// <param name="userId">Usuario de Firma Representante Legal.</param>
        /// <returns>Retorna el modelo de negocio o un error.</returns>
        public static Result<ParamLegalRepresentativeSing, ErrorModel> GetParamLegalRepresentativeSing(ParamCompanyType paramCompanyType, ParamBranchType paramBranchType, DateTime currentFrom, string legalRepresentative, string pathSignatureImg, byte[] signatureImg, string userId)
        {
            return new ResultValue<ParamLegalRepresentativeSing, ErrorModel>(new ParamLegalRepresentativeSing(paramCompanyType, paramBranchType, currentFrom, legalRepresentative, pathSignatureImg, signatureImg, userId));
        }

        /// <summary>
        /// Objeto que crea la Firma Representante Legal.
        /// </summary>
        /// <param name="paramCompanyType">Compañia de Firma Representante Legal.</param>
        /// <param name="paramBranchType">Sucursal de Firma Representante Legal.</param>
        /// <param name="currentFrom">Fecha de Firma Representante Legal.</param>
        /// <param name="legalRepresentative">Representante de Firma Representante Legal.</param>
        /// <param name="pathSignatureImg">Ruta de la imagen de Firma Representante Legal.</param>
        /// <param name="signatureImg">Byte de la imagen de Firma Representante Legal.</param>
        /// <param name="userId">Usuario de Firma Representante Legal.</param>
        /// <returns>Retorna el modelo de negocio o un error.</returns>
        public static Result<ParamLegalRepresentativeSing, ErrorModel> CreateParamLegalRepresentativeSing(ParamCompanyType paramCompanyType, ParamBranchType paramBranchType, DateTime currentFrom, string legalRepresentative, string pathSignatureImg, byte[] signatureImg, string userId)
        {
            //// escribir todas las validaciones requeridas para la creación del modelo. Ejemplo
            List<string> error = new List<string>();

            if (paramCompanyType.Id <= 0)
            {
                error.Add("El identificador no puede ser un valor negativo");
                return new ResultError<ParamLegalRepresentativeSing, ErrorModel>(ErrorModel.CreateErrorModel(error, ErrorType.BusinessFault, null));
            }
            else
            {
                return new ResultValue<ParamLegalRepresentativeSing, ErrorModel>(new ParamLegalRepresentativeSing(paramCompanyType, paramBranchType, currentFrom, legalRepresentative, pathSignatureImg, signatureImg, userId));
            }
        }
    }
}
