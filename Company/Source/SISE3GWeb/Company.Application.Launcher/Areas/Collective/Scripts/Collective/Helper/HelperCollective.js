helpers: {   
    isStringNullOrEmpty: function (val) {
        switch (val) {
            case "":
            case 0:
            case "0":
            case null:
            case false:
            case undefined:
            case typeof this === 'undefined':
                return true;
            default: return false;
        }
    },   
    isStringNullOrWhiteSpace: function (val) {
        return this.isStringNullOrEmpty(val) || val.replace(/\s/g, "") === '';
    },  
    nullIfStringNullOrEmpty: function (val) {
        if (this.isStringNullOrEmpty(val)) {
            return null;
        }
        return val;
    }
}