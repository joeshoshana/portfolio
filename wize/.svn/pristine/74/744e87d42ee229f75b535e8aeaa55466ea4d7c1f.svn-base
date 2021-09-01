var items;
var companyforms;
var companysettings;
var companyformsfields;
var companytables;
var companyscales;
var current;
var current_form;
var filter;


$(document).ready(function () {
    Shared();

    CreateDatatable();
    CreateTablesDatatable();
    CreateFormsDatatable();
    CreateSettingsDatatable();

    CreateFormsFieldsDatatable();
    CreateScalesDatatable();

    Language();

    CreateFilterEvents();
    AllowedRows(items.rows().data().length);
    $("#tabs").tabs();

    $('#items tbody').on('click', 'tr', function (e) {
        current = items.row(this).data();
        if (current === undefined)
            return;        

        GetModalData();
        $("#current").modal();
        companytables.draw();
        companyforms.draw();
        companyscales.draw();
        companysettings.draw();
    });

    $('#isowner').on('change', function () {
        ToggleCompanyLimit($(this).is(':checked'));
    });

    $('#companytables tbody').on('click', 'input[type=checkbox]', function (e) {
        var row = $(this).parents('tr');
        var dt = $('#companytables').DataTable();
        var obj = dt.row(row).data();        
        dt.row(row).data().CompanyID = $(this)[0].checked?1:0;
    });

    $('#companytables tbody').on('change', 'input[type=text]', function (e) {
        var row = $(this).parents('tr');
        var dt = $('#companytables').DataTable();
        var obj = dt.row(row).data();
        var val = $(this).val();
        var limit = dt.row(row).data().OwnerLimit;
        if (limit != -1 && limit < val) {
            alert(dictionary.Limit);
            val = 0;
            $(this).val(0);
        }

        dt.row(row).data().AllowedRows = val;
    });

    $('#companyforms tbody').on('click', 'input[type=checkbox]', function (e) {
        var row = $(this).parents('tr');
        var dt = $('#companyforms').DataTable();
        var obj = dt.row(row).data();
        dt.row(row).data().CompanyID = $(this)[0].checked ? 1 : 0;
    });

    $('#companyforms tbody').on('change', 'input[type=text]', function (e) {
        var row = $(this).parents('tr');
        var dt = $('#companyforms').DataTable();
        var obj = dt.row(row).data();
        var val = $(this).val();
        var limit = dt.row(row).data().OwnerLimit;
        if (limit != -1 && limit < val) {
            alert(dictionary.Limit);
            val = 0;
            $(this).val(0);
        }

        dt.row(row).data().AllowedRows = val;

    });
    $('#companyscales tbody').on('click', 'input', function (e) {
        var row = $(this).parents('tr');
        var dt = $('#companyscales').DataTable();
        var obj = dt.row(row).data();
        dt.row(row).data().CompanyID = $(this)[0].checked ? 1 : 0;
    });

    $('#companysettings tbody').on('click', 'input[type=checkbox]', function (e) {
        var row = $(this).parents('tr');
        var dt = $('#companysettings').DataTable();
        var obj = dt.row(row).data();
        dt.row(row).data().CompanyID = $(this)[0].checked ? 1 : 0;
    });

    $('#companysettings tbody').on('change', 'input[type=text]', function (e) {
        var row = $(this).parents('tr');
        var dt = $('#companysettings').DataTable();
        var obj = dt.row(row).data();
        var val = $(this).val();
        var limit = dt.row(row).data().OwnerLimit;
        if (limit != -1 && limit < val) {
            alert(dictionary.Limit);
            val = 0;
            $(this).val(0);
        }

        dt.row(row).data().AllowedRows = val;
    });

    $('#btnNew').on('click', function (e) {

        current = new Object();
        current.GUID = -1;
        current.Active = true;
        GetModalData();        
        ToggleCompanyLimit(false);
        companytables.draw();
        companyforms.draw();
        companyscales.draw();
        companysettings.draw();
        $("#current").modal();
    });

    $('#btnSave').on('click', function (e) {
        if (current === undefined)
            return;
        SetModalData();
        Save(current);
    });
    $('#btnSaveFields').on('click', function (e) {
        if (current === undefined)
            return;
        SetModalData();
        SaveFields(current);
    });
    
    $('#companyforms tbody').on('click', 'button', function (e) {
        e.stopPropagation();
        var row = $(this).parents('tr');
        var dt = $('#companyforms').DataTable();
        current_form = dt.row(row).data();
        
        
        companyformsfields.draw();
        $("#formsFields_current").modal();
    });

    $('#companyformsfields tbody').on('click', 'input', function (e) {
        
        var row = $(this).parents('tr');
        var dt = $('#companyformsfields').DataTable();
        var obj = dt.row(row).data();
        if ($(this).parents('td')[0].cellIndex == 1)          
            obj.ValidationRequired = $(this)[0].checked ? 1 : 0;
        else if ($(this).parents('td')[0].cellIndex == 2)
            obj.IsShowing = $(this)[0].checked ? 1 : 0;
    });

    if (is_super == false) {
        items.columns([3]).visible(false, false);
    }
    else {
        items.columns([3]).visible(true, false);
    }
});

function ToggleCompanyLimit(val)
{
    if (val == false)
        $(".company_limit").hide();
    else
        $(".company_limit").show();
}

function CreateTablesDatatable() {
    companytables = $('#companytables').DataTable({
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
            "url": "/Companies/GetTables",
            "data": function (data) {
                data.data = JSON.stringify(current);
            },
            "type": "POST",
            "dataType": "json",
            error: function (xhr, textStatus, errorThrown) {
                console.log(xhr.responseText);
            }
        },
        "columns": [
           { "data": "TabelName" },
           { "data": "CompanyID" },
           { "data": "AllowedRows" },
           { "data": "OwnerLimit"},
           { "data": "TableID" },
           { "data": "GUID" }
        ],
        "columnDefs": [
            {
                "targets": [0, 1,2,3,4,5],
                "className": "dt-center",
                "orderable": true
            },
            {
                "targets": [1],
                "render": function (data, type, row, meta) {
                    if (data != 0) {
                        return '<input type=\"checkbox\" checked value="' + data + '">';
                    } else {
                        return '<input type=\"checkbox\" value="' + data + '">';
                    }
                }
            },
            {
                "targets": [2],
                "render": function (data, type, row, meta) {
                        return '<input class="text-right" type=\"text\" value="' + data + '" dir="ltr">';
                }
            },
            {
                "targets": [3],
                "render": function (data, type, row, meta) {
                    return '<lable class="text-right" dir="ltr">' + data + '</label>';
                }
            },
            {
                "targets": [4,5],
                "visible": false,
                "searchable": false
            }
        ],
    });
}

function CreateSettingsDatatable() {
    companysettings = $('#companysettings').DataTable({
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
            "url": "/Companies/GetSettings",
            "data": function (data) {
                data.data = JSON.stringify(current);
            },
            "type": "POST",
            "dataType": "json",
            error: function (xhr, textStatus, errorThrown) {
                console.log(xhr.responseText);
            }
        },
        "columns": [
           { "data": "SettingName" },
           { "data": "CompanyID" },
           { "data": "AllowedRows" },
           { "data": "OwnerLimit" },
           { "data": "SettingID" },
           { "data": "GUID" }
        ],
        "columnDefs": [
            {
                "targets": [0, 1, 2, 3, 4, 5],
                "className": "dt-center",
                "orderable": true
            },
            {
                "targets": [1],
                "render": function (data, type, row, meta) {
                    if (data != 0) {
                        return '<input type=\"checkbox\" checked value="' + data + '">';
                    } else {
                        return '<input type=\"checkbox\" value="' + data + '">';
                    }
                }
            },
            {
                "targets": [2],
                "render": function (data, type, row, meta) {
                    return '<input class="text-right" type=\"text\" value="' + data + '" dir="ltr">';
                }
            },
            {
                "targets": [3],
                "render": function (data, type, row, meta) {
                    return '<lable class="text-right" dir="ltr">' + data + '</label>';
                }
            },
            {
                "targets": [4, 5],
                "visible": false,
                "searchable": false
            }
        ],
    });
}

function CreateFormsDatatable() {
    companyforms = $('#companyforms').DataTable({
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
            "url": "/Companies/GetForms",
            "data": function (data) {
                data.data = JSON.stringify(current);
            },
            "type": "POST",
            "dataType": "json",
            error: function (xhr, textStatus, errorThrown) {
                console.log(xhr.responseText);
            }
        },
        "columns": [
           { "data": "TabelName" },
           { "data": "CompanyID" },
           { "data": "Forms" },
           { "data": "AllowedRows" },
           { "data": "OwnerLimit" },
           { "data": "FormID" },
           { "data": "GUID" }
        ],
        "columnDefs": [
            {
                "targets": [0, 1, 2, 3, 4, 5, 6],
                "className": "dt-center",
                "orderable": true
            },
            {
                "targets": [1],
                "render": function (data, type, row, meta) {
                    if (data != 0) {
                        return '<input type=\"checkbox\" checked value="' + data + '">';
                    } else {
                        return '<input type=\"checkbox\" value="' + data + '">';
                    }
                }
            },
            {
                "targets": [2],
                "data": null,
                "defaultContent": "<button>" + dictionary.Fields + "</button>"
            },
            {
                "targets": [3],
                "render": function (data, type, row, meta) {
                    return '<input class="text-right" type=\"text\" value="' + data + '" dir="ltr">';
                }
            },
            {
                "targets": [4],
                "render": function (data, type, row, meta) {
                    return '<lable class="text-right" dir="ltr">' + data + '</label>';
                }
            },
            {
                "targets": [ 5, 6],
                "visible": false,
                "searchable": false
            }
        ],
    });
}


function CreateFormsFieldsDatatable() {
    companyformsfields = $('#companyformsfields').DataTable({
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
            "url": "/Companies/GetFormsFields",
            "data": function (data) {
                data.form = JSON.stringify(current_form);
                data.data = JSON.stringify(current);
            },
            "type": "POST",
            "dataType": "json",
            error: function (xhr, textStatus, errorThrown) {
                console.log(xhr.responseText);
            }
        },
        "columns": [
           { "data": "FormsFieldName" },
           { "data": "ValidationRequired" },
           { "data": "IsShowing" },
           { "data": "FormsFieldID" },
           { "data": "CompanyID" },
           { "data": "FormID" },
           { "data": "GUID" }
        ],
        "columnDefs": [
            {
                "targets": [0, 1, 2],
                "className": "dt-center",
                "orderable": true
            },
            {
                "targets": [1,2],
                "render": function (data, type, row, meta) {
                    if (data != 0) {
                        return '<input type=\"checkbox\" checked value="' + data + '">';
                    } else {
                        return '<input type=\"checkbox\" value="' + data + '">';
                    }
                }
            },
            {
                "targets": [3, 4, 5, 6],
                "visible": false,
                "searchable": false
            }
        ],
    });
}

function CreateScalesDatatable() {
    companyscales = $('#companyscales').DataTable({
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
            "url": "/Companies/GetScales",
            "data": function (data) {
                data.data = JSON.stringify(current);
            },
            "type": "POST",
            "dataType": "json",
            error: function (xhr, textStatus, errorThrown) {
                console.log(xhr.responseText);
            }
        },
        "columns": [
           { "data": "Name" },
           { "data": "MAC" },
           { "data": "Status" },
           { "data": "ScalesTypeName" },
           { "data": "GUID" },
           { "data": "CompanyID" },
           { "data": "Weight" },
           { "data": "WeightDate" },
           { "data": "Active" }
        ],
        "columnDefs": [
            {
                "targets": [0, 1, 2, 3],
                "className": "dt-center",
                "orderable": true
            },
            {
                "targets": [4, 5, 6, 7, 8],
                "visible": false,
                "searchable": false
            }
        ],
        "createdRow": function (row, data, dataIndex) {
            if (data.Status == true) {
                $(row).css('background-color', '#38c449');
                //startWorker();
            }
            if (data.Status == false) {
                $(row).css('background-color', '#ef3b3b');
                //stopWorker();
            }
        }
    });
}


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
            "url": "/Companies/Get",
            "async": false,
            "data": function (data) {
                data.data = JSON.stringify(new Filter());
            },
            "type": "POST",
            "dataType": "json",
            error: function (xhr, textStatus, errorThrown) {
                console.log(xhr.responseText);
            }
        },
        "columns": [
           { "data": "Name" },
           { "data": "Address" },
           { "data": "ID" },
           { "data": "IsOwner" },
           { "data": "Active" },
           { "data": "GUID" }
        ],
        "columnDefs": [
            {
                "targets": [0, 1, 2, 3, 4, 5],
                "className": "dt-center",
                "orderable": true
            },
            {
                "targets": [4, 5],
                "visible": false,
                "searchable": false
            },
            {
                "targets": [3],
                "render": function (data, type, row, meta) {
                    if (data != 0) {
                        return '<input type=\"checkbox\" checked value="' + data + '" disabled>';
                    } else {
                        return '<input type=\"checkbox\" value="' + data + '"  disabled>';
                    }
                }
            },
        ],
    });
}

function GetModalData() {
    if (current !== undefined) {
        $("#name").val(current.Name);
        $("#address").val(current.Address);
        $("#id").val(current.ID);
        $("#active").prop("checked", current.Active);
        $("#isowner").prop("checked", current.IsOwner);
        $("#language").val((current.LanguageID == null ? 0 : current.LanguageID));
        $('#company_limit').val(current.CompaniesLimit);
        ToggleCompanyLimit(current.IsOwner);
    }
}

function SetModalData() {
    if (current !== undefined) {
        current.Name = $("#name").val();
        current.Address = $("#address").val();
        current.ID = $("#id").val();
        current.Active = $("#active").prop("checked");
        current.IsOwner = $("#isowner").prop("checked");
        current.LanguageID = $("#language").val();
        current.CompaniesLimit = $('#company_limit').val();
    }
}

function AllowedRows(len) {
    $.ajax({
        type: 'POST',
        url: '/Sites/AllowedRows',
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify({ data: len }),
        dataType: 'json',
        success: function (data) {
            if (data.isSucceded == false) {
                alert(data.message);
            }
            else {
                if (data.data == false)
                    $("#btnNew").hide();
                else
                    $("#btnNew").show();
            }
        },
        error: function (xhr, ajaxOptions, thrownError)
        { console.log(xhr.responseText) }

    });
}

function SaveFields(obj) {
    var f = companyformsfields.rows().data().toArray();

    $.ajax({
        type: 'POST',
        url: '/Companies/SaveFields',
        contentType: 'application/json; charset=utf-8',

        data: JSON.stringify({ data: JSON.stringify(obj), formsfields: JSON.stringify(f) }),
        dataType: 'json',
        success: function (data) {
            if (data.isSucceded == false) {
                alert(data.message);
            }
            else {
                $("#formsFields_current").modal('hide');
            }
        },
        error: function (xhr, ajaxOptions, thrownError)
        { console.log(xhr.responseText) }

    });
}

function Save(obj) {
    var t = companytables.rows().data().toArray();
    var f = companyforms.rows().data().toArray();
    var s = companysettings.rows().data().toArray();
    
    if (obj.CompanyID == undefined)
        obj.CompanyID = 0;

    if (obj.IsOwner == undefined || obj.IsOwner == false)
        obj.CompaniesLimit = 0;
    else if (obj.IsOwner == true && (obj.CompaniesLimit == undefined || obj.CompaniesLimit <= 0)) {
        alert(dictionary.InvalidCompaniesLimit);
        return;
    }


    obj.Tables = t;
    obj.Forms = f;
    obj.Settings = s;

    $.ajax({
        type: 'POST',
        url: '/Companies/Save',
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify({ data: JSON.stringify(obj)}),
        //data: JSON.stringify({ data: JSON.stringify(obj), tables: JSON.stringify(t), forms: JSON.stringify(f), settings: JSON.stringify(s) }),
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

function Filter() {
    this.Name = $("#f_name").val();
    this.Address = $("#f_address").val();
    this.ID = $("#f_id").val();
    this.Active = $("#f_active").is(":checked");
    if (this.Active === undefined || !this.Active)
        this.Active = false;
}

function CreateFilterEvents() {
    $("#f_name").on('input', function () {
        items.draw();
    });

    $("#f_address").on('input', function () {
        items.draw();
    });

    $("#f_id").on('input', function () {
        items.draw();
    });

    $("#f_active").on('input', function () {
        items.draw();
    });

    $("#f_isowner").on('input', function () {
        items.draw();
    });



}
