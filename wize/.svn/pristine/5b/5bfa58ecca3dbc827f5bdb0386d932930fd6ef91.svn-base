var items;
var current;
var filter;
var GUID = 1;

$(document).ready(function () {
    Shared();
    writePermission = ReadWritePermissions(company_settings_data, GUID, 'w');
    CreateDatatable();

    CreateFilterEvents();
    AllowedRows(items.rows().data().length);

    LoadScales();
    LoadForms();
    LoadPermissions();
    LoadDrivers();

    Language();
    $("#f_from_birth_date").datepicker({
        dateFormat: "dd/mm/yy",
        todayBtn: "linked",
        clearBtn: true,
        language: 'he',
        autoclose: true
    });

    $("#f_to_birth_date").datepicker({
        dateFormat: "dd/mm/yy",
        todayBtn: "linked",
        clearBtn: true,
        language: 'he',
        autoclose: true
    });

    $("#birth_date").datepicker({
        dateFormat: "dd/mm/yy",
        todayBtn: "linked",
        clearBtn: true,
        language: 'he',
        autoclose: true
    });
    $('#eye').on('mousedown', function () {
        var x = document.getElementById("password");
            x.type = "text";
    });

    $('#eye').on('mouseup', function () {
        var x = document.getElementById("password");
            x.type = "password";
    });

    $('#items tbody').on('click', 'tr', function (e) {
        if (!writePermission)
            return;
        current = items.row(this).data();
        if (current === undefined)
            return;
        GetModalData();
        $("#current").modal();
    });

    
    $('#btnNew').on('click', function (e) {
        
        current = new Object();
        current.Active = true;
        GetModalData();

        $("#current").modal();
    });

    $('#btnSave').on('click', function (e) {
        if (current === undefined)
            return;
        SetModalData();
        
        Save(current);
    });

});

function CreateDatatable() {
    items = $('#items').DataTable({
        "orderClasses": false,
        "processing": true,
        "serverSide": true,
        "pagingType": "full_numbers",
        "searching": false,
        "colReorder": true,
        "responsive": true,
        "language": {
            "decimal": "",
            "emptyTable": dictionary.EmptyTable,
            "info": dictionary.Info,
            "infoEmpty": dictionary.InfoEmpty,
            "infoFiltered": dictionary.InfoFiltered,
            "infoPostFix": "",
            "thousands": ",",
            "lengthMenu": dictionary.LengthMenu,
            "loadingRecords": dictionary.LoadingRecords,
            "processing": dictionary.Processing,
            "search": dictionary.Search,
            "zeroRecords": dictionary.ZeroRecords,
            "paginate": {
                "first": dictionary.First,
                "last": dictionary.Last,
                "next": dictionary.Next,
                "previous": dictionary.Previous
            }
        },
        "ajax": {
            "url": "/Users/Get",
            "async": false,
            "data": function(data)
            {
                data.data = JSON.stringify(new Filter());
                },
            "type": "POST",
            "dataType": "json",
            error: function (xhr, textStatus, errorThrown) {
                console.log(xhr.responseText);
            }
        },
        "columns": [
           { "data": "FirstName" },
           { "data": "LastName" },
           { "data": "ID" },
           { "data": "BirthDate" },
           { "data": "Username" },
           { "data": "Password" },
           { "data": "Email" },
           { "data": "PermissionName" },
           { "data": "ScaleName" },
           { "data": "FormName" },
           { "data": "CompanyID" },
           { "data": "Active" },
           { "data": "GUID" }
        ],
        "columnDefs": [
            {
                "targets": [3],
                "render": function (data, type, row, meta) {
                    return moment(data).format('DD/MM/YYYY');
                }
            },
            {
                "targets": [5],
                "render": function (data, type, row, meta) {
                    str = "";
                    var i;
                    for (i = 0; i < data.length; i++) {
                        str += "*";
                    }
                    return str;
                }
            },
            {
                "targets": [0,1,2,3,4,5,6,7,8,9],
                "className": "dt-center",
                "orderable" : true
            },
            {
                "targets": [10,11,12],
                "visible": false,
                "searchable": false
            }
        ],
    });
}

function GetModalData()
{
    if(current !== undefined)
    {
        $("#first_name").val(current.FirstName);
        $("#last_name").val(current.LastName);
        $("#id").val(current.ID);
        $("#username").val(current.Username);
        $("#password").val(current.Password);
        $("#email").val(current.Email);
        $("#scales").val(current.DefaultScaleID);
        $("#drivers").val(current.DriverID);
        $("#forms").val(current.DefaultFormID);
        $("#permissions").val(current.PermissionID);
        $("#birth_date").val(moment(current.BirthDate).format('DD/MM/YYYY'));
        $("#active").prop("checked", current.Active);
        $("#language").val((current.LanguageID == null ? 0 : current.LanguageID));
    }
}

function SetModalData() {
    if (current !== undefined) {
        current.FirstName = $("#first_name").val();
        current.LastName = $("#last_name").val();
        current.ID = $("#id").val();
        current.Username = $("#username").val();
        current.Password = $("#password").val();
        current.Email = $("#email").val();
        current.DefaultScaleID = $("#scales").val();
        current.DefaultFormID = $("#forms").val();
        current.DriverID = $("#drivers").val();
        current.BirthDate = ToServerDate($("#birth_date").val(), 'DD/MM/YYYY');
        current.Active = $("#active").prop("checked");
        current.LanguageID = $("#language").val();
        current.PermissionID = $("#permissions").val();
    }
}

function Save(obj) {
    $.ajax({
        type: 'POST',
        url: '/Users/Save',
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify({ data: JSON.stringify(obj) }),
        dataType: 'json',
        success: function (data) {
            if (data.isSucceded == false) {
                alert(data.message);
            }
            else {
                $("#current").modal('hide');
                items.draw();
            }
        },
        error: function (xhr, ajaxOptions, thrownError)
        { console.log(xhr.responseText) }
    });
}

function LoadScales() {
    $.ajax({
        type: 'POST',
        url: '/Scales/Get',
        contentType: 'application/json; charset=utf-8',
        dataType: 'json',
        data: JSON.stringify({ data: JSON.stringify(new Scale()) }),
        success: function (data) {
            if (data.isSucceded == false) {
                alert(data.message);
            }
            else {
                var scales = $("#scales");
                var f_scales = $("#f_scales");
                scales.empty();
                f_scales.empty();
                scales.append($("<option />").val(0).text(dictionary.SelectScales));
                f_scales.append($("<option />").val(0).text(dictionary.SelectScales));
                $.each(data.data, function (i, val) {
                    scales.append($("<option />").val(val.GUID).text(val.Name));
                    f_scales.append($("<option />").val(val.GUID).text(val.Name));
                });
            }
        },
        error: function (xhr, ajaxOptions, thrownError)
        { console.log(xhr.responseText) }
    });
}

function LoadPermissions() {
    $.ajax({
        type: 'POST',
        url: '/Permissions/Get',
        contentType: 'application/json; charset=utf-8',
        dataType: 'json',
        data: JSON.stringify({ data: JSON.stringify(new Permission()) }),
        success: function (data) {
            if (data.isSucceded == false) {
                alert(data.message);
            }
            else {
                var permissions = $("#permissions");
                permissions.empty();
                permissions.append($("<option />").val(0).text(dictionary.SelectPermission));
                $.each(data.data, function (i, val) {
                    permissions.append($("<option />").val(val.GUID).text(val.Name));
                });
            }
        },
        error: function (xhr, ajaxOptions, thrownError)
        { console.log(xhr.responseText) }
    });
}

function LoadForms() {
    $.ajax({
        type: 'POST',
        url: '/Home/CompanyForms',
        contentType: 'application/json; charset=utf-8',
        dataType: 'json',
        data: JSON.stringify({ data: JSON.stringify(new Form()) }),
        success: function (data) {
            if (data.isSucceded == false) {
                alert(data.message);
            }
            else {
                var forms = $("#forms");
                var f_forms = $("#f_forms");
                forms.empty();
                f_forms.empty();
                forms.append($("<option />").val(0).text(dictionary.SelectForm));
                f_forms.append($("<option />").val(0).text(dictionary.SelectForm));
                $.each(data.data, function (i, val) {
                    forms.append($("<option />").val(val.GUID).text(val.Name));
                    f_forms.append($("<option />").val(val.GUID).text(val.Name));
                });
            }
        },
        error: function (xhr, ajaxOptions, thrownError)
        { console.log(xhr.responseText) }
    });
}

function Filter()
{
    this.FirstName = $("#f_first_name").val();
    this.LastName = $("#f_last_name").val();
    this.ID = $("#f_id").val();
    this.Username = $("#f_username").val();
    this.Password = $("#f_password").val();
    this.Email = $("#f_email").val();
    this.FromBirthDate = ToServerDate($("#f_from_birth_date").val(), 'DD/MM/YYYY');
    this.ToBirthDate = ToServerDate($("#f_to_birth_date").val(), 'DD/MM/YYYY');
    this.DefaultScaleID = $("#f_scales").val();
    this.DefaultFormID = $("#f_forms").val();
    this.Active = $("#f_active").is(":checked");
    if (this.Active === undefined || !this.Active)
        this.Active = false;
}

function CreateFilterEvents()
{
    $("#f_first_name").on('input', function () {
        items.draw();
    });

    $("#f_last_name").on('input', function () {
        items.draw();
    });

    $("#f_id").on('input', function () {
        items.draw();
    });

    $("#f_username").on('input', function () {
        items.draw();
    });

    $("#f_password").on('input', function () {
        items.draw();
    });

    $("#f_email").on('input', function () {
        items.draw();
    });

    $("#f_from_birth_date").on('change', function () {
        items.draw();
    });

    $("#f_to_birth_date").on('change', function () {
        items.draw();
    });

    $("#f_active").on('input', function () {
        items.draw();
    });

    $("#f_scales").on('change',function () {
        items.draw();
    });

    $("#f_forms").on('change', function () {
        items.draw();
    });
}

function Scale() {
    this.Active = true;
    //this.Status = true;
}


function Permission() {
}

function Form() {
    this.Active = true;
    //this.Status = true;
}

function Language() {
    $.ajax({
        type: 'POST',
        url: '/Home/Language',
        contentType: 'application/json; charset=utf-8',
        dataType: 'json',
        success: function (data) {
            if (data.isSucceded == false) {
                alert(data.message);
            }
            else {
                var language = $("#language");
                language.empty();
                language.append($("<option />").val(0).text(dictionary.SelectLang));
                $.each(data.data, function (i, val) {
                    language.append($("<option />").val(val.GUID).text(val.Name));
                });
            }
        },
        error: function (xhr, ajaxOptions, thrownError)
        { console.log(xhr.responseText) }
    });
}

function AllowedRows(len) {
    $.ajax({
        type: 'POST',
        url: '/Users/AllowedRows',
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify({ data: len }),
        dataType: 'json',
        success: function (data) {
            if (data.isSucceded == false) {
                alert(data.message);
            }
            else {
                if (data.data == false || !writePermission)
                    $("#btnNew").hide();
                else
                    $("#btnNew").show();
            }
        },
        error: function (xhr, ajaxOptions, thrownError)
        { console.log(xhr.responseText) }

    });
}

function LoadDrivers() {
    $.ajax({
        type: 'POST',
        url: '/Drivers/Get',
        contentType: 'application/json; charset=utf-8',
        dataType: 'json',
        success: function (data) {
            if (data.isSucceded == false) {
                alert(data.message);
            }
            else {
                var drivers = $("#drivers");
                drivers.empty();
                drivers.append($("<option />").val(0).text(dictionary.SelectDriver));
                $.each(data.data, function (i, val) {
                    drivers.append($("<option />").val(val.GUID).text(val.Name));
                });
            }
        },
        error: function (xhr, ajaxOptions, thrownError)
        { console.log(xhr.responseText) }
    });
}