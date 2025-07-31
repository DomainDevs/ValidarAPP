using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

//namespace Sistran.Core.Framework.UIF.Web.Helpers
//{
//    public class DateHelper
//    {
//        private static Boolean _leapYear;
//        private static Boolean _isActive;
//        public static readonly string FormatDate = "dd/MM/yyyy";

//        public static Boolean LeapYear
//        {
//            get
//            {
//                if (_isActive == false)
//                {
//                    Services.DelegateUnderwriting delegateUnderwriting = new Services.DelegateUnderwriting();
//                    _leapYear = delegateUnderwriting.GetLeapYear();
//                    _isActive = true;
//                }
//                return _leapYear;
//            }
//            private set { _leapYear = value; }
//        }

//        static DateHelper()
//        {
//            if (_isActive == false)
//            {
//                Services.DelegateUnderwriting delegateUnderwriting = new Services.DelegateUnderwriting();
//                _leapYear = delegateUnderwriting.GetLeapYear();
//                _isActive = true;
//            }
//        }

//    }
//}