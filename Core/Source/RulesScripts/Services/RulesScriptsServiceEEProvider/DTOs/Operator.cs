using System;
using System.CodeDom;
using Sistran.Core.Framework.Data;

namespace Sistran.Core.Application.RulesScriptsServices.EEProvider.DTOs
{
	/// <summary>
	/// Un operador es un símbolo que especifica una acción que se realiza en una o más expresiones.
	/// </summary>
	[Serializable]
	public class Operator : IDescriptionable
	{
		private string	_description;
		private CodeBinaryOperatorType _code;
		private string	_smallDescription;

		public Operator(CodeBinaryOperatorType code, string description, string smallDescription)
		{
			_code=code;
			_description=description;
			_smallDescription=smallDescription;
		}

		
		/// <summary>
		/// Obtiene la descripcion que se utilizara en la interfaz de usuario.
		/// </summary>
		public string Description
		{
			get { return _description; }
		}

		/// <summary>
		/// Obtiene el codigo que representa el operador.
		/// </summary>
		public int Code
		{
			get { return (int)_code; }
		}
		
		/// <summary>
		/// Obtiene la descripcion corta que se utilizara en la interfaz de usuario.
		/// </summary>
		public string SmallDescription
		{
			get { return _smallDescription; }
		}
		
		/// <summary>
		/// Obtiene el tipo de operador binario.
		/// </summary>
		public CodeBinaryOperatorType GetOperatorType
		{
			get { return _code; }
		}
	}
}