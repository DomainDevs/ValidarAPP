var DateNowPerson;
class Shared {
    static GetCurrentDate() {
        var now = null;
        if (DateNowPerson != null) {
            return DateNowPerson;
        }
        else {
            PersonRequest.GetCurrentDate().done(function (data) {
                if (data.success) {
                    DateNowPerson = FormatDate(data.result, 1);
                }
            });
        }
    }

    static CalculateDigitVerify(documentNumber) {
        var vpri, x, y, z, i, nit1, dv1;
        nit1 = documentNumber;
        if (!isNaN(nit1)) {
            vpri = new Array(16);
            x = 0; y = 0; z = nit1.length;
            vpri[1] = 3;
            vpri[2] = 7;
            vpri[3] = 13;
            vpri[4] = 17;
            vpri[5] = 19;
            vpri[6] = 23;
            vpri[7] = 29;
            vpri[8] = 37;
            vpri[9] = 41;
            vpri[10] = 43;
            vpri[11] = 47;
            vpri[12] = 53;
            vpri[13] = 59;
            vpri[14] = 67;
            vpri[15] = 71;
            for (i = 0; i < z; i++) {
                y = (nit1.substr(i, 1));
                x += (y * vpri[z - i]);
            }

            y = x % 11;

            if (y > 1) {
                dv1 = 11 - y;
            } else {
                dv1 = y;
            }
            return dv1;
        }
        else
            return "";
    }

    numberWithCommas(x) {
        x = x.toString().replace(/\./g, '');
        var pattern = /(-?\d+)(\d{3})/;
        while (pattern.test(x))
            x = x.replace(pattern, "$1.$2");
        return x;
    }

    static calculateAge(control, fecha) {
        if (fecha != null) {
            fecha = this.FormatDateNow(fecha);
            var fechaActual = new Date()
            var diaActual = fechaActual.getDate();
            var mmActual = fechaActual.getMonth() + 1;
            var yyyyActual = fechaActual.getFullYear();
            var FechaNac = fecha.split(DateSplit);
            var diaCumple = FechaNac[0];
            var mmCumple = FechaNac[1];
            var yyyyCumple = FechaNac[2];
            if (mmCumple.substr(0, 1) == 0) {
                mmCumple = mmCumple.substring(1, 2);
            }
            if (diaCumple.substr(0, 1) == 0) {
                diaCumple = diaCumple.substring(1, 2);
            }
            var edad = yyyyActual - yyyyCumple;
            if ((mmActual < mmCumple) || (mmActual == mmCumple && diaActual < diaCumple)) {
                edad--;
            }
            $(control).val(edad);
            return edad;
        }
        else {
            return 0;
        }
    }

    static FormatDateNow(date) {
        if (date != null) {
            date = date.toString();
            if (date.length > 10) {
                var dateString = date.substr(6);
                var currentTime = new Date(parseInt(dateString));
                var month = currentTime.getMonth() + 1;
                var day = currentTime.getDate();
                var year = currentTime.getFullYear();
                var dayFormat = day < 10 ? '0' + day : '' + day;
                var monthFormat = month < 10 ? '0' + month : '' + month;
                var date = dayFormat + DateSplit + monthFormat + DateSplit + year;
            }
        }
        return date;
    }
}