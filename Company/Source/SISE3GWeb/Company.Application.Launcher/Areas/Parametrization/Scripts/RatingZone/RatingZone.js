// -----------------------------------------------------------------------
// <copyright file="RatingZone.js" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Robinson Castro Londoño</author>
// -----------------------------------------------------------------------

var glbRatingZone = { Cities: [] };
var glbCities = [];
var totalRatingZones;
var RatingZonesIndex = null;
/**
 * Clase para el manejo de Zonas de tarifacion
 */
class RatingZone extends Uif2.Page {

    /**
     * @summary
     * Obtien el componente DropDown de la busqueda avanzada
     */
    get AdvSearchdropDown() {
        this.dropDownSearch = uif2.dropDown({
            source: rootPath + "Parametrization/RatingZone/AdvancedSearch",
            element: "#btnSearchAdv",
            align: "right",
            width: 550,
            height: 551,
            loadedCallback: this.AdvSearchEvents.bind(this)
        });
    }

    /**
     * @summary
     * inicializa los componentes
     */
    getInitialState() {
        $("input[type=text]").TextTransform(ValidatorType.UpperCase);

        $.ajaxSetup({ async: true });
        this.AdvSearchdropDown;

        $("#listRatingZone").UifListView({
            displayTemplate: "#templateRatingZone",
            edit: true,
            delete: true,
            customEdit: true,
            sourceData: null,
            height: 350,
            deleteCallback: RatingZone.DeleteRatingZone
        });

        $("#listCities").UifListView({
            displayTemplate: "#template-city",
            title: Resources.Language.LabelAvailableCities,
            selectionType: "multiple",
            height: 300
        });
        $("#listviewCitiesAssing").UifListView({
            displayTemplate: "#template-city",
            title: Resources.Language.LabelSelectedCities,
            selectionType: "multiple",
            height: 300
        });

        request("Underwriting/Underwriting/GetPrefixes", {}, "POST", Resources.Language.ErrorGetPrefixes, RatingZone.SetPrefixes);
        request("Parametrization/RatingZone/GetRatingZonesByFilter", {}, "POST", Resources.Language.ErrorQueryChargingZones, RatingZone.SetRatingZone);
        request("Parametrization/RatingZone/GetCountries", {}, "POST", Resources.Language.ErrorSearchCountry, RatingZone.SetContries);

        $("#txtSearch").mask("AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA");

    }

    /**
     * @summary
     * Inicializa los eventos
     */
    bindEvents() {
        $("#listRatingZone").on("rowEdit", RatingZone.SetFormRatingZone);
        $("#btnNewRatingZone").on("click", RatingZone.ClearFormRatingZone);
        $("#btnAddRatingZone").on("click", RatingZone.AddRatingZone);
        $("#btnCities").on("click", RatingZone.OpenModalCities);
        $("#ddlCountry").on("itemSelected", RatingZone.ChangeCountry);
        $("#ddlState").on("itemSelected", RatingZone.ChangeState);
        $("#btnModalCitiesAssignAll").click(RatingZone.CopyAllCities);
        $("#btnModalCitiesAssign").click(RatingZone.CopyCitiesSelected);
        $("#btnModalCitiesDeallocateAll").click(RatingZone.DeallocateCitiesAll);
        $("#btnModalCitiesDeallocate").click(RatingZone.DeallocateCitiesSelected);
        $("#btnModalCitiesClose").on("click", RatingZone.CloseModalCities);
        $("#btnModalCitiesAccept").on("click", RatingZone.SaveModalCities);
        $("#btnSaveRatingZone").on("click", RatingZone.SaveRatingZone);
        $("#btnSearchAdv").on("click", this.OpenAdvSearch.bind(this));
        $("#txtSearch").on("search", this.BasicSearch.bind(this));
        $("#btnExit").on("click", this.Exit);
        $("#btnExport").on("click", this.Export);
    }

    /**
     * @summary
     * evento para exportar el excel
     */
    Export() {
        request("Parametrization/RatingZone/GenerateFileToExport", null, "POST", AppResources.ErrorGeneratingFile, (url) => { window.open(url) });
    }

    /**
     * @summary
     * Evento al salir de la pantalla
     */
    Exit() {
        window.location = rootPath + "Home/Index";
    }

    /**
     * @summary
     * Inicializa los eventos de la busqueda avanzada
     */
    AdvSearchEvents() {
        $("input[type=text]").TextTransform(ValidatorType.UpperCase);
        $("#listRatingZoneAdvSearch").UifListView({
            displayTemplate: "#templateRatingZone",
            sourceData: null,
            selectionType: "single",
            height: 240
        });

        $("#btnCancelAdvSearch").on("click", this.CloseAdvSearch.bind(this));
        $("#btnAcceptAdvSearch").on("click", this.AcceptAdvSearch.bind(this));

        $("#advRatingZoneCode").mask("#");
        $("#advDescription").mask("AAAAAAAAAAAAAAA");

        $("#btnSearchAdvSearch").on("click", this.OnSearchAdv.bind(this));
    }

    /**
     * @summary
     * Evento al ejecutar la busqueda avanzada
     */
    OnSearchAdv() {
        $("#listRatingZoneAdvSearch").UifListView("refresh");
        //const ratingZoneCode = $("#advRatingZoneCode").val();
        const prefixCode = $("#PrefixCodeBus").val();
        const description = $("#advDescription").val().trim();

        if (prefixCode === "" && description === "") {
            $.UifNotify("show", { type: "info", message: Resources.Language.ErrorAdvSearchRatingZone, autoclose: true });
            return;
        }
        if (description !== "" && description.length < 3) {
            $.UifNotify("show", { type: "info", message: Resources.Language.ErrorSearchCharMin, autoclose: true });
            return;
        }
        request("Parametrization/RatingZone/GetRatingZonesByFilter", JSON.stringify({ "prefixCode": prefixCode, filter: description }), "POST", "",
            (result) => {
                result.forEach(item => { $("#listRatingZoneAdvSearch").UifListView("addItem", item) });
            });
    }

    /**
     * @summary
     * Abre la busqueda avanzada
     */
    OpenAdvSearch() {
        request("Underwriting/Underwriting/GetPrefixes", {}, "POST", Resources.Language.ErrorGetPrefixes, RatingZone.SetPrefixesSub);
        this.dropDownSearch.show();
        $("#listRatingZoneAdvSearch").UifListView("refresh");
        $("#listRatingZoneAdvSearch").UifListView("clear");
        //$("#PrefixCodeBus").val(RatingZone.SetPrefixes);
        $("#advDescription").val("");
    }

    /**
     * @summary
     * Cierra la busqueda avanzada
     */
    CloseAdvSearch() {
        this.dropDownSearch.hide();
    }

    /**
     * @summary
     * Accion del boton aceptar de la busqueda avanzada
     * @param {any} e
     * Evento
     */
    AcceptAdvSearch(e) {
        const selected = $("#listRatingZoneAdvSearch").UifListView("getSelected")[0];
        if (selected) {
            this.CloseAdvSearch(this);

            const index = $("#listRatingZone").UifListView("findIndex", (x) => {
                return x.RatingZoneCode === selected.RatingZoneCode;
            });

            RatingZone.SetFormRatingZone(e, selected, index);
        }
    }

    /**
     * @summary
     * Accion al ejecutarla busqueda simple
     * @param {any} e
     * Evento
     * @param {String} value
     * Valor a buscar
     */
    BasicSearch(e, value) {
        $("#txtSearch").val("");
        value = value.trim();
        if (value === "" || value.length < 3 || value.length === 0) {
            $.UifNotify("show", { type: "info", message: Resources.Language.ErrorSearchCharMin, autoclose: true });
            return;
        }

        request("Parametrization/RatingZone/GetRatingZonesByFilter", JSON.stringify({ filter: value }), "POST", "",
            (result) => {
                if (result.length === 1) {
                    const index = $("#listRatingZone").UifListView("findIndex", (x) => {
                        return x.RatingZoneCode === result[0].RatingZoneCode;
                    });

                    RatingZone.SetFormRatingZone(e, result[0], index);
                }
                else if (result.length <= 0) {
                    $.UifNotify("show", { type: "info", message: Resources.Language.ExistRatingZone, autoclose: true });
                }
                else {
                    this.OpenAdvSearch(this);
                    result.forEach(item => { $("#listRatingZoneAdvSearch").UifListView("addItem", item) });
                }
            });
    }

    /**
     * @summary
     * Raliza el guardado global de las zonas de tarifacion
     */
    static SaveRatingZone() {

        let data = $("#listRatingZone").UifListView("getData");

        const createRatingZone = data.filter(x => { return x.StatusTypeService == ParametrizationStatus.Create });
		const updateRatingZone = data.filter(x => { return x.StatusTypeService == ParametrizationStatus.Update });
		const deleteRatingZone = data.filter(x => { return x.StatusTypeService == ParametrizationStatus.Delete });

		//data = createRatingZone.concat(updateRatingZone);
		//data = createRatingZone.concat(deleteRatingZone);
        data = createRatingZone.concat(updateRatingZone).concat(deleteRatingZone);

        if (data.length > 0) {
            request("Parametrization/RatingZone/ExecuteOperationsParametrizationRatingZone",
                JSON.stringify({ ratingZoneViewModels: data }), "POST", "",
                (result) => {
                    request("Parametrization/RatingZone/GetRatingZonesByFilter", {}, "POST", Resources.Language.ErrorQueryChargingZones, RatingZone.SetRatingZone);
                    RatingZone.ClearFormRatingZone();

                    let message = Resources.Language.Create;

                    if (result.TotalAdded > 0) {
                        message += `</br> ${Resources.Language.Aggregates}: ${result.TotalAdded}`;
                    }
                    if (result.TotalModified > 0) {
                        message += `</br> ${Resources.Language.Updated}: ${result.TotalModified}`;
                    }
                    if (result.TotalDeleted > 0) {
                        message += `</br> ${Resources.Language.Removed}: ${result.TotalDeleted}`;
                    }
                    if (result.Message) {
                        message += `</br> ${Resources.Language.Errors}: ${result.Message}`;
                    }

                    $.UifNotify("show", { 'type': "info", 'message': message, 'autoclose': true });
                });
        }
    }

    /**
     * @summary
     * Cierra la modal de ciudades
     */
    static CloseModalCities() {
        glbCities = jQuery.extend(true, [], glbRatingZone.Cities);
    }

    /**
     * @summary
     * Guarda las ciudades parametrizadas
     */
    static SaveModalCities() {

        const data = $("#listRatingZone").UifListView("getData").filter((x) => { return x.RatingZoneCode != glbRatingZone.RatingZoneCode && x.PrefixCode == glbRatingZone.PrefixCode });
        const cities = jQuery.extend(true, [], glbCities);

        let isValid = true;
        let message = "";

        for (let i = 0; i < data.length; i++) {
            const r = data[i];
            r.Cities.forEach(rc => {
                cities.forEach(c => {
                    if (c.Id === rc.Id && c.State.Id === rc.State.Id && rc.State.Country.Id === c.State.Country.Id && rc.Id > 0) {
                        const format = function () {
                            let theString = arguments[0];
                            for (let j = 1; j < arguments.length; j++) {
                                const regEx = new RegExp(`\\{${j - 1}\\}`, "gm");
                                theString = theString.replace(regEx, arguments[j]);
                            };
                            return theString;
                        }

                        message += `* ${format(Resources.Language.MessageCityIsAlreadyAssign, c.Description, r.Description)} </br>`;
                        isValid = false;
                    }
                });
            });
        }
        if (isValid === true) {
            $("#modalCities").UifModal("hide");
            glbRatingZone.Cities = cities;
            glbCities = [];
        } else {
            $.UifNotify("show", { type: "info", message: message, autoclose: true });
        }
    }

    /**
     * @summary
     * mueve todas las ciudades a la 2da lista
     */
    static CopyAllCities() {
        const cities = $("#listCities").UifListView("getData");
        $("#listCities").UifListView("refresh");

        cities.forEach(city => {
            $("#listviewCitiesAssing").UifListView("addItem", city);
            glbCities.push(city);
        });
    }

    /**
    * @summary
    * Mueve las ciudades seleccionadas a la 2da lista
    */
    static CopyCitiesSelected() {
        const cities = $("#listCities").UifListView("getSelected");
        cities.forEach(city => {
            var findCity = (element) => { return element.Id == city.Id && element.State.Id == city.State.Id && element.State.Country.Id == city.State.Country.Id };

            var index = $("#listCities").UifListView("findIndex", findCity);
            $("#listCities").UifListView("deleteItem", index);

            $("#listviewCitiesAssing").UifListView("addItem", city);
            glbCities.push(city);
        });
    }

    /**
     * @summary
     * mueve todas las ciudades a la 1da lista
     */
    static DeallocateCitiesAll() {
        const cities = $("#listviewCitiesAssing").UifListView("getData");
        $("#listviewCitiesAssing").UifListView("refresh");

        cities.forEach(city => {
            $("#listCities").UifListView("addItem", city);
            glbCities = glbCities.filter(element => { return element.Id != city.Id || element.State.Id != city.State.Id || element.State.Country.Id != city.State.Country.Id });
        });
    }

    /**
    * @summary
    * Mueve las ciudades seleccionadas a la 1da lista
    */
    static DeallocateCitiesSelected() {
        const cities = $("#listviewCitiesAssing").UifListView("getSelected");
        cities.forEach(city => {
            var findCity = (element) => { return element.Id == city.Id && element.State.Id == city.State.Id && element.State.Country.Id == city.State.Country.Id };

            var index = $("#listviewCitiesAssing").UifListView("findIndex", findCity);
            $("#listviewCitiesAssing").UifListView("deleteItem", index);

            $("#listCities").UifListView("addItem", city);
            glbCities = glbCities.filter(element => { return element.Id != city.Id || element.State.Id != city.State.Id || element.State.Country.Id != city.State.Country.Id });
        });
    }

    /**
     * @summary
     * Abre al modal de Ciudades
     */
    static OpenModalCities() {
        if ($("#PrefixCode").UifSelect("getSelectedSource") == undefined) {
            $.UifNotify("show", { type: "info", message: "Debe seleccionar un Ramo.", autoclose: true });
            return;
        }

        $("#modalCities").UifModal("showLocal", Resources.Language.ModalTitleCities);
        $("#ddlCountry").UifSelect("setSelected", 1);
        $("#listCities").UifListView("refresh");
        $("#listviewCitiesAssing").UifListView("refresh");
        $("#listCities").UifListView("clear");
        $("#listviewCitiesAssing").UifListView("clear");
        totalRatingZones.forEach(item => {
            if (item.Cities.length > 0 && item.RatingZoneCode == glbRatingZone.RatingZoneCode) {
                item.Cities.forEach(city => {
                    //const tmpCity = glbRatingZone.Cities.find(x => { return x.Id == city.Id && x.State.Id == state.Id && x.State.Country.Id == country.Id });
                    //if (tmpCity !== null && tmpCity !== undefined) {
                    $("#listviewCitiesAssing").UifListView("addItem", city);
                    //}
                });
            }
        });

        request("Parametrization/RatingZone/GetStatesByCountry", JSON.stringify({ countryId: 1 }), "POST", Resources.Language.ErrorConsultingDepartments, RatingZone.SetStates);
    }

    /**
     * @summary
     * Setea los select con los valores de los ramos
     * @param {Object<Prefix>} prefixes
     * lista de ramos
     */
    static SetPrefixes(prefixes) {

        let comboConfig = { sourceData: prefixes };
        $("#PrefixCode").UifSelect(comboConfig);    
    }

    static SetPrefixesSub(prefixes) {
        $("#PrefixCodeBus").UifSelect(
            {
                sourceData: prefixes,
                id: "Id",
                name: "Description"
            });
    }

    /**
     * @summary
     * Setea la lista de paises
     * @param {List<Country>} contries
     * Lista de paises
     */
    static SetContries(contries) {
        $("#ddlCountry").UifSelect(
            {
                sourceData: contries,
                id: "Id",
                name: "Description",
                selectedId: 1
            });
        request("Parametrization/RatingZone/GetStatesByCountry", JSON.stringify({ countryId: 1 }), "POST", Resources.Language.ErrorConsultingDepartments, RatingZone.SetStates);
    }

    /**
     * @summary
     * 
     * @param {any} states
     */
    static SetStates(states) {
        $("#ddlState").UifSelect(
            {
                sourceData: states,
                id: "Id",
                name: "Description",
                native: false,
                filter: true
            });
    }

    /**
     * @summary
     * Evento al cambiar el pais
     * @param {any} e
     * evento
     * @param {Object<Country>} country
     * Objeto seleccionado
     */
    static ChangeCountry(e, country) {
        $("#listCities").UifListView("refresh");
        $("#listviewCitiesAssing").UifListView("refresh");
        $("#ddlState").UifSelect();

        if (country.Id !== "") {
            request("Parametrization/RatingZone/GetStatesByCountry", JSON.stringify({ countryId: country.Id }), "POST", Resources.Language.ErrorConsultingDepartments, RatingZone.SetStates);
        }
    }

    /**
     * @summary
     * Evento al cambiar de Estado
     * @param {any} e
     * Evento
     * @param {Object<State>} state
     * Estado seleccionado
     */
    static ChangeState(e, state) {

        $("#listCities").UifListView("refresh");
        $("#listviewCitiesAssing").UifListView("refresh");

        if (state.Id !== "") {
            const country = $("#ddlCountry").UifSelect("getSelectedSource");
            const prefix = $("#PrefixCode").UifSelect("getSelectedSource");
            request("Parametrization/RatingZone/GetCitiesByCountryState", JSON.stringify({ countryId: country.Id, stateId: state.Id, PrefixCode: prefix.Id }), "POST", Resources.Language.ErrorQueryingCities, RatingZone.SetCities);
        }
    }

    /**
     * @summary
     * setea la lista de cuidades
     * @param {List<City>} cities
     * Lista de ciudades
     */
    static SetCities(cities) {
        const country = $("#ddlCountry").UifSelect("getSelectedSource");
        const state = $("#ddlState").UifSelect("getSelectedSource");

        cities.forEach(city => {
            const tmpCity = glbRatingZone.Cities.find(x => { return x.Id == city.Id && x.State.Id == state.Id && x.State.Country.Id == country.Id });
            if (tmpCity !== null && tmpCity !== undefined) {
                $("#listviewCitiesAssing").UifListView("addItem", city);
            } else {
                $("#listCities").UifListView("addItem", city);
            }
        });
    }

    /**
    * @summary
    * Setea el listView con las zonas de tarifacion
    * @param {Object<RatingZone>} ratingZones
    * lista zonas de tarifacion
    */
    static SetRatingZone(ratingZones) {
        $("#listRatingZone").UifListView("refresh");
        ratingZones.forEach((item) => { $("#listRatingZone").UifListView("addItem", item) });
        totalRatingZones = ratingZones;
    }

    /**
     * @summary
     * Accion al guardar el formulario
     */
    static AddRatingZone() {
        if ($("#formRatingZone").valid()) {
            let ratingZone = $("#formRatingZone").serializeObject();
            ratingZone.IsDefault = $("#IsDefault").prop("checked");
            ratingZone.PrefixDescription = $("#PrefixCode").UifSelect("getSelectedSource").Description;
            ratingZone.Cities = glbRatingZone.Cities;

            if (RatingZone.ValidateRatingZone(ratingZone)) {
                if (parseInt(ratingZone.RatingZoneCode) > 0) {
                    RatingZone.UpdateRatingZone(ratingZone);
                }
                else {
                    RatingZone.InsertRatingZone(ratingZone);
                }
                RatingZone.ClearFormRatingZone();
            }
        }
    }

    /**
     * @summary
     * Realiza las validaciones de zona predeterminada por ramo
     * @param {Object<RatingZone>} ratingZone
     * Zona de tarifacion a validar
     */
	static ValidateRatingZone(ratingZone) {
        if (ratingZone.IsDefault) {
           var data = $("#listRatingZone").UifListView("getData").filter((x) => { return x.RatingZoneCode != ratingZone.RatingZoneCode && x.IsDefault == true && x.PrefixCode == ratingZone.PrefixCode && x.Description == ratingZone.Description });

            if (RatingZonesIndex == null) {
				var ifExist = $("#listRatingZone").UifListView('getData').filter(function (item) {
					return item.Description.toUpperCase() == ratingZone.Description.toUpperCase() && item.PrefixCode == ratingZone.PrefixCode
                });
				if (ifExist.length > 0) {
					$.UifNotify("show", { type: "danger", message: Resources.Language.ErrorWhenAddingRatingZone , autoclose: true });
                    return false;
                }
            }

            if (data.length > 0) {
                $.UifDialog("confirm", { message: Resources.Language.MessageChangeRatingZoneDefault },
                    (result) => {
                        if (result) {
                            const fIndex = (x) => { return x.IsDefault == true && x.PrefixCode == ratingZone.PrefixCode };
                            const index = $("#listRatingZone").UifListView("findIndex", fIndex);
                            data[0].IsDefault = false;

                            const aIndex = $("#hdnIndex").val();
                            $("#hdnIndex").val(index);

                            if (parseInt(data[0].RatingZoneCode) > 0) {
                                RatingZone.UpdateRatingZone(data[0]);
                            }
                            else {
                                RatingZone.InsertRatingZone(data[0]);
                            }

                            $("#hdnIndex").val(aIndex);
                        } else {
                            ratingZone.IsDefault = false;
                        }

                        if (parseInt(ratingZone.RatingZoneCode) > 0) {
                            RatingZone.UpdateRatingZone(ratingZone);
                        }
                        else {
                            RatingZone.InsertRatingZone(ratingZone);
                        }
                        RatingZone.ClearFormRatingZone();
                    });

                return false;

            }
            else {
                return true;
            }

        } else {
           var data = $("#listRatingZone").UifListView("getData").filter((x) => { return x.Description == ratingZone.Description });
            if (RatingZonesIndex == null) {
				var ifExist = $("#listRatingZone").UifListView('getData').filter(function (item) {
					return item.Description.toUpperCase() == ratingZone.Description.toUpperCase() && item.PrefixCode == ratingZone.PrefixCode
                });
				if (ifExist.length > 0) {
					$.UifNotify("show", { type: "danger", message: Resources.Language.ErrorWhenAddingRatingZone, autoclose: true });
                    return false;
                }
                else {
                    return true;
                }
            }
            else {
                //$('#listRatingZone').UifListView('editItem', RatingZonesIndex, ratingZone);
                return true;
            }
        }


    }

    /**
     * @summary
     * Edita un registro de la listView
     * @param {any} e
     * Evento
     * @param  {Object<RatingZone>} formRatingZone
     * Elemento a Editar
     */
    static UpdateRatingZone(formRatingZone) {
        formRatingZone.StatusTypeService = ParametrizationStatus.Update;
        formRatingZone.Edit = true;
        delete formRatingZone.Create;
        $("#listRatingZone").UifListView("editItem", $("#hdnIndex").val(), formRatingZone);
    }

    /**
     * @summary
     * Inserta un registro de la listView
     * @param {any} e
     * Evento
     * @param  {Object<RatingZone>} formRatingZone
     * Elemento a Insertar
     */
    static InsertRatingZone(formRatingZone) {
        formRatingZone.Create = true;
        delete formRatingZone.Edit;
        if (formRatingZone.StatusTypeService === "") {
            formRatingZone.StatusTypeService = ParametrizationStatus.Create;
            $("#listRatingZone").UifListView("addItem", formRatingZone);
        } else {
            formRatingZone.StatusTypeService = ParametrizationStatus.Create;
            $("#listRatingZone").UifListView("editItem", $("#hdnIndex").val(), formRatingZone);
        }
    }

    /**
     * @summary
     * Elimina un registro de la listView
     * @param {any} e
     * Evento
     * @param {Object<RatingZone>} ratingZone
     * Elemento a eliminar
     */
    static DeleteRatingZone(e, ratingZone) {
		e.resolve();
		if (parseInt(ratingZone.StatusTypeService) !== ParametrizationStatus.Create) {
			ratingZone.StatusTypeService = ParametrizationStatus.Delete;
			ratingZone.allowEdit = false;
			ratingZone.allowDelete = false;
			$("#listRatingZone").UifListView("addItem", ratingZone);
        }
        RatingZone.ClearFormRatingZone();
    }

    /**
     * @summary
     * Setea el formulario con un objeto
     * @param {event} e
     * evento 
     * @param {RatingZone} item
     * Item a renderizar
     * @param {Object<RatingZone>} index
     * index del item en el listView
     */
    static SetFormRatingZone(e, item, index) {
        glbCities = jQuery.extend(true, [], item.Cities);
        glbRatingZone = jQuery.extend(true, {}, item);
        glbRatingZone.Cities = jQuery.extend(true, [], item.Cities);

        RatingZone.ClearValidation();
        const form = $("#formRatingZone");
        form.find("#StatusTypeService").val(item.StatusTypeService);
        form.find("#hdnIndex").val(index);
        form.find("#RatingZoneCode").val(item.RatingZoneCode);
        form.find("#PrefixCode").UifSelect('setSelected',item.PrefixCode);
        form.find("#Description").val(item.Description);
        form.find("#SmallDescription").val(item.SmallDescription);
        RatingZonesIndex = index;
        if (item.IsDefault) {
            form.find("#IsDefault").prop("checked", "checked");
        } else {
            form.find("#IsDefault").removeAttr("checked");
        }
    }

    /**
     * @summary
     * Inicializa los valores del formulario (vacio)
     */
    static ClearFormRatingZone() {
        glbCities = [];
        glbRatingZone = { Cities: [] };
        RatingZonesIndex = null;

        const form = $("#formRatingZone");
        form.find("#StatusTypeService").val("");
        form.find("#hdnIndex").val("");
        form.find("#RatingZoneCode").val("");
        form.find("#PrefixCode").UifSelect('setSelected', null);
        form.find("#Description").val("");
        form.find("#SmallDescription").val("");
        form.find("#IsDefault").removeAttr("checked");

        RatingZone.ClearValidation();
    }

    /**
     * @summary
     * Limpia el formulario validaciones
     */
    static ClearValidation() {
        const form = $("#formRatingZone");
        const validator = $(form).validate();
        $("[name]", form).each(function () {
            validator.successList.push(this);//mark as error free
            validator.showErrors();//remove error messages if present
        });
        validator.resetForm();//remove error class on name elements and clear history
        validator.reset();//remove all error and success data
    }
}