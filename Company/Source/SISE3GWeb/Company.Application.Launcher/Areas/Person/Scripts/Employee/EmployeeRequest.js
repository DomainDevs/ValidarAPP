class EmployeeRequest {
    static GetDeclinedType() {
        return $.ajax({
            type: "POST",
            url: rootPath + "Person/Employee/GetDeclinedType",
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            async: false
        });
    }

    static GetEmployeByIndividualId(individualId) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Person/Employee/GetEmployee',
            data: JSON.stringify({ IndividualId: individualId }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static SaveEmployee(objEmployee) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Person/Employee/SaveEmployee',
            data: JSON.stringify({ employee: objEmployee }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static UpdateEmployee(employee) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Person/Employee/SaveEmployee',
            data: JSON.stringify({ employeeDTO: employee }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }
    
}