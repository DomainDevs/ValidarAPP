using Sistran.Core.Application.UniquePersonService.Assemblers;
using Sistran.Core.Application.UniquePersonService.Models;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Framework.DAF;
using System;
using System.Collections.Generic;
using System.Linq;
using COMMEN = Sistran.Core.Application.Common.Entities;

namespace Sistran.Core.Application.UniquePersonService.DAOs
{
    public class NomenclatureDAO
    {
        /// <summary>
        /// Normalizar Dirección
        /// </summary>
        /// <param name="address">Dirección</param>
        /// <returns>Dirección</returns>
        public string NormalizeAddress(string address)
        {
            List<Nomenclature> nomenclatures = GetNomenclatures();

            address = address.Replace('-', ' ').Replace('.', ' ').Replace('#', ' ');

            String[] arrayAddress = address.ToUpper().Split();
            arrayAddress = arrayAddress.Where(x => !string.IsNullOrEmpty(x)).ToArray();

            string newAddress = "";
            string newstring = "";

            for (int i = 0; i < arrayAddress.Length; i++)
            {
                if (arrayAddress[i] != "")
                {
                    if (i + 1 >= arrayAddress.Length)
                    {
                        newstring = arrayAddress[i];
                    }
                    else
                    {
                        newstring = arrayAddress[i] + " " + arrayAddress[i + 1];
                    }

                    if (nomenclatures.Exists(x => x.Description == newstring))
                    {
                        newAddress = newAddress + nomenclatures.First(x => x.Description == newstring).SmallDescription;
                        i++;
                    }
                    else
                    {
                        newAddress = newAddress + arrayAddress[i];
                    }

                    newAddress = newAddress + " ";
                }
            }

            arrayAddress = newAddress.Split(' ');
            newAddress = "";

            for (int i = 0; i < arrayAddress.Length; i++)
            {
                if (arrayAddress[i] != "")
                {
                    int temp;

                    if (int.TryParse(arrayAddress[i], out temp))
                    {
                        newAddress = newAddress + arrayAddress[i];
                    }
                    else
                    {
                        if (nomenclatures.Exists(x => x.Description == arrayAddress[i]))
                        {
                            newAddress = newAddress + nomenclatures.First(x => x.Description == arrayAddress[i]).SmallDescription;
                        }
                        else
                        {
                            newAddress = newAddress + arrayAddress[i];
                        }
                    }

                    newAddress = newAddress + " ";
                }
            }

            return newAddress.Trim();
        }

        /// <summary>
        /// Obtener Nomenclaturas
        /// </summary>
        /// <returns>Nomenclaturas</returns>
        public List<Nomenclature> GetNomenclatures()
        {
            BusinessCollection businessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(COMMEN.CoNomenclatures)));
            return ModelAssembler.CreateNomenclatures(businessCollection);
        }
    }
}