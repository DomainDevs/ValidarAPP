class Massive {
    static GetLoadTypes() {
        return $.ajax({
            type: 'POST',
            url: 'GetLoadTypes',
            dataType: 'json',
            contentType: 'application/json; charset=utf-8',
        });
    }

    static UploadFile(fileName) {
        return $.ajax({
            type: 'POST',
            url: 'UploadFile',
            data: JSON.stringify({ fileName: fileName }),
            dataType: 'json',
            contentType: 'application/json; charset=utf-8'
        });
    }

    static CreateLoad(massiveViewModel) {
        return $.ajax({
            type: 'POST',
            url: 'CreateLoad',
            data: JSON.stringify({ massiveViewModel: massiveViewModel }),
            dataType: 'json',
            contentType: 'application/json; charset=utf-8'
        });
    }

    static TariffedLoad(massiveLoadId) {
        return $.ajax({
            type: 'POST',
            url: 'TariffedLoad',
            data: JSON.stringify({ massiveLoadId: massiveLoadId }),
            dataType: 'json',
            contentType: 'application/json; charset=utf-8'
        });
    }

    static IssuePolicy(massiveLoadId) {
        return $.ajax({
            type: 'POST',
            url: 'IssuePolicy',
            data: JSON.stringify({ massiveLoadId: massiveLoadId }),
            dataType: 'json',
            contentType: 'application/json; charset=utf-8'
        });
    }

    static GetMassiveLoadsByDescription(description) {
        return $.ajax({
            type: 'POST',
            url: 'GetMassiveLoadsByDescription',
            data: JSON.stringify({ description: description }),
            dataType: 'json',
            contentType: 'application/json; charset=utf-8'
        });
    }

    static GenerateFileToMassiveLoad(massiveLoadProccessId) {
        return $.ajax({
            type: 'POST',
            url: 'GenerateFileToMassiveLoad',
            data: JSON.stringify({ massiveLoadProccessId: massiveLoadProccessId }),
            dataType: 'json',
            contentType: 'application/json; charset=utf-8',
        });
    }
}