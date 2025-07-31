function EncryptData() {
    var input_pass = document.getElementById("Password");
    if (input_pass.value != "") {
        var key = CryptoJS.enc.Utf8.parse('8080808080808080');
        var iv = CryptoJS.enc.Utf8.parse('8080808080808080');
        var encryptedpassword = CryptoJS.AES.encrypt(CryptoJS.enc.Utf8.parse(input_pass.value), key,
            {
                keySize: 128 / 8,
                iv: iv,
                mode: CryptoJS.mode.CBC,
                padding: CryptoJS.pad.Pkcs7
            });
        input_pass.value = encryptedpassword;
    }
}