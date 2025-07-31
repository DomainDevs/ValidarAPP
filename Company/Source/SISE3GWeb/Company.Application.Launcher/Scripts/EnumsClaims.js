TypeVictim = {
    Insured: 1,
    Third: 0
};

EstimateType = {
    Compensation: 1,    //Indemnización
    Salvages: 2,        //Salvamentos
    FeeAndExpenses: 4,  //Honorarios y Gastos
    Recoveries: 7,      //Recobros
}; 

EstimationTypeStatuses =
{
    Closed: 3,          //Cerrado
    Open: 1,           //Abierto
};

Prefix =
{
    Fidelity: 1,        //Manejo
    Marine: 6,          //Casco Barco
    };

// Enum a partir de la tabla PARAM.PERSON_TYPE
ClaimPersonType =
{
    Insured: 7, // Asegudaro
    Participant: 8, // Tercero
    SuretyPerson: 17, //Afianzado 
    Holder: 18, // Tomador
};