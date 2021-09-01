var certificates;
var current;
var filter;
var contacts;
var contact_way;
var GUID = 4;

$(document).ready(function() {
    Shared();
    LoadComboboxes();

    $("#f_from_out_date").datepicker({
        dateFormat: "dd/mm/yy",
        todayBtn: "linked",
        clearBtn: true,
        language: 'he',
        autoclose: true
    }).datepicker("setDate", 'now').change();

    $("#f_to_out_date").datepicker({
        dateFormat: "dd/mm/yy",
        todayBtn: "linked",
        clearBtn: true,
        language: 'he',
        autoclose: true
    });

    $("#btnEvacuation").on("click", function() {
        Report(1);
    });

    $("#btnWeights").on("click", function() {
        Report(2);
    });

    /*writePermission = ReadWritePermissions(company_forms_data, GUID, 'w');
    LoadComboboxes();

    //$("#f_items").combobox();
    //$("#f_transports").combobox();
    //$("#f_vehicles").combobox();
    //$("#f_insites").combobox();
    //$("#f_outsites").combobox();
    //$("#f_users").combobox();
    //$("#f_customers").combobox();
    //$("#f_drivers").combobox();
    //
    //$("#items").combobox();
    //$("#transports").combobox();
    //$("#vehicles").combobox();
    //$("#insites").combobox();
    //$("#outsites").combobox();
    //$("#users").combobox();
    //$("#customers").combobox();
    //$("#drivers").combobox();

    $("#f_from_out_date").datepicker({
        dateFormat: "dd/mm/yy",
        todayBtn: "linked",
        clearBtn: true,
        language: 'he',
        autoclose: true
    }).datepicker("setDate", 'now').change();

    $("#f_to_out_date").datepicker({
        dateFormat: "dd/mm/yy",
        todayBtn: "linked",
        clearBtn: true,
        language: 'he',
        autoclose: true
    });

    $("#out_date").datepicker({
        dateFormat: "dd/mm/yy",
        todayBtn: "linked",
        clearBtn: true,
        language: 'he',
        autoclose: true
    });

    $("#in_date").datepicker({
        dateFormat: "dd/mm/yy",
        todayBtn: "linked",
        clearBtn: true,
        language: 'he',
        autoclose: true
    });

    CreateDatatable();
    var data = InvisibleFields(4);
    $.each(data, function (i, val) {
        UpdateVisibilityDataTableColumns(val.FormsFieldName);
    });

    CreateContactsTable();
    CreateFilterEvents();

    

    $('#certificates tbody').on('click', 'tr', function (e) {
        if (!writePermission)
            return;
        current = certificates.row(this).data();
        if (current === undefined)
            return;
        GetModalData();
        $("#current").modal();
    });

    $('#contacts tbody').on('click', 'tr', function (e) {
        var obj = contacts.row(this).data();
        if (obj === undefined)
            return;
        if (contact_way == 1 && (obj.Phone == undefined || obj.Phone == ""))
            alert(dictionary.MissingPhone);
        else if (contact_way == 2 && (obj.Email == undefined || obj.Email == ""))
            alert(dictionary.MissingEmail);
        else
        {
            if(contact_way == 1)
            {
                if (confirm(dictionary.SendTo + obj.FirstName + ' SMS?')) {
                    $.ajax({
                        type: 'POST',
                        url: '/Customers/SMSContact',
                        contentType: 'application/json; charset=utf-8',
                        data: JSON.stringify({ data: JSON.stringify(obj), type:1,weighingVehicleID : current.GUID }),
                        dataType: 'json',
                        success: function (data) {
                            if (data.isSucceded == false) {
                                alert(data.message);
                            }
                            else {
                                alert("SMS" + dictionary.Sent);
                            }
                        },
                        error: function (xhr, ajaxOptions, thrownError)
                        { console.log(xhr.responseText) }

                    });
                }
            }

            if (contact_way == 2) {
                if (confirm(dictionary.SendTo + obj.FirstName + ' מייל?')) {
                    $.ajax({
                        type: 'POST',
                        url: '/Customers/MailContact',
                        contentType: 'application/json; charset=utf-8',
                        data: JSON.stringify({ data: JSON.stringify(obj),type:1, weighingVehicleID: current.GUID }),
                        dataType: 'json',
                        success: function (data) {
                            if (data.isSucceded == false) {
                                alert(data.message);
                            }
                            else {
                                alert(dictionary.Sent + " " + dictionary.Email);
                            }
                        },
                        error: function (xhr, ajaxOptions, thrownError)
                        { console.log(xhr.responseText) }

                    });
                }
            }
        }
    });

    $('#certificates tbody').on('click', '.btnCertificate', function (e) {
        if (!writePermission)
            return;
        e.stopPropagation();
        var row = $(this).parents('tr');
        var dt = $('#certificates').DataTable();
        var obj = dt.row(row).data();
        Preview(obj);
    });

    $('#certificates tbody').on('click', '.btnSMS', function (e) {
        if (!writePermission)
            return;
        e.stopPropagation();
        contact_way = 1;
        var row = $(this).parents('tr');
        var dt = $('#certificates').DataTable();
        var obj = dt.row(row).data();
        current = obj;
        ContactSelection(obj);
    });

    $('#certificates tbody').on('click', '.btnMail', function (e) {
        if (!writePermission)
            return;
        e.stopPropagation();
        contact_way = 2;
        var row = $(this).parents('tr');
        var dt = $('#certificates').DataTable();
        var obj = dt.row(row).data();
        current = obj;
        ContactSelection(obj);
    });

    $('#btnSave').on('click', function (e) {
        if (!writePermission)
            return;
        if (current === undefined)
            return;
        SetModalData();

        Save(current);
    });
*/
    $("#btnBack").on("click", function(e) {
        window.location.href = "Index";
    });
});

function UpdateVisibilityDataTableColumns(fieldName) {
    $("." + fieldName).hide();
    switch (fieldName) {
        case "LicenseNumber":
            certificates.column(2).visible(false);
            break;
        case "TransportName":
            certificates.column(1).visible(false);
            break;
        case "ItemName":
            certificates.column(8).visible(false);
            break;
        case "SourceSite":
            certificates.column(9).visible(false);
            break;
        case "DestinationSite":
            certificates.column(10).visible(false);
            break;
        case "CustomerName":
            certificates.column(11).visible(false);
            break;
        case "DriverName":
            certificates.column(12).visible(false);
            break;
        case "Remarks":
            certificates.column(16).visible(false);
            break;
    }
}

function LoadComboboxes() {
    LoadItems();
    LoadTransports();
    LoadDrivers();
    LoadVehicles();
    LoadInSites();
    LoadOutSites();
    //LoadUsers();
    LoadCustomers();
}

function Report(rptNum) {
    $.ajax({
        type: "POST",
        url: "/VehiclesWeighing/Report",
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify({
            filter: JSON.stringify(new Filter()),
            rptNum: rptNum
        }),
        dataType: "json",
        success: function(data) {
            if (data.isSucceded === false) {
                console.error(data.message);
            } else {
                const dt = JSON.parse(data.data);
                if (dt.length === 0) {
                    alert("אין רשומות להצגה עבור המסננים הנתונים");
                    return;
                }
                const csvObj = GetReport(rptNum, dt);
                download(csvObj);                                               
            }
        },
        error: function(xhr, ajaxOptions, thrownError) {
            console.log(xhr.responseText);
        }
    });
}

function GetReport(rptNum, data)
{
    switch(rptNum)
    {
        case 1:
            {
                const reportData = evacuationReport(data);
                const csvObj = objectToCsv(reportData, [
                    "CustomerName",
                    "Appearances",
                    "Netto",
                    "CustomerCode"
                ],[$("#f_from_out_date").val(),$("#f_to_out_date").val()]);
                return csvObj;
            }
            break;
        case 2:
            {                
                const csvObj = objectToCsv(data, [
                    "CertificateID",
                    "InWeight",
                    "OutWeight",
                    "InDate",
                    "OutDate",                    
                    "Netto",
                    "LicenseNumber",
                    "DriverName",
                    "CustomerName",
                    "CustomerCode",
                    "ItemName",
                    "TransportName",                    
                    "InSiteName",
                    "OutSiteName",
                    "Remarks",
                    "Reference"
                ],[$("#f_from_out_date").val(),$("#f_to_out_date").val()]);
                return csvObj;
            }            
            break;
    }
}

function LoadCustomers() {
    $.ajax({
        type: "POST",
        url: "/Customers/Get",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function(data) {
            if (data.isSucceded == false) {
                alert(data.message);
            } else {
                var f_customers = $("#f_customers");
                f_customers.empty();
                f_customers.append(
                    $("<option/>")
                        .val(0)
                        .text(dictionary.SelectCustomer)
                );
                $.each(data.data, function(i, val) {
                    f_customers.append(
                        $("<option/>")
                            .val(val.GUID)
                            .text(val.Name)
                    );
                });
            }
        },
        error: function(xhr, ajaxOptions, thrownError) {
            console.log(xhr.responseText);
        }
    });
}

function LoadUsers() {
    $.ajax({
        type: "POST",
        url: "/Users/Get",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function(data) {
            if (data.isSucceded == false) {
                alert(data.message);
            } else {
                var f_Users = $("#f_users");              
                f_Users.empty();
                f_Users.append(
                    $("<option />")
                        .val(0)
                        .text(dictionary.SelectUser)
                );
                $.each(data.data, function(i, val) {
                    f_Users.append(
                        $("<option />")
                            .val(val.GUID)
                            .text(val.FirstName)
                    );
                });
            }
        },
        error: function(xhr, ajaxOptions, thrownError) {
            console.log(xhr.responseText);
        }
    });
}

function LoadInSites() {
    $.ajax({
        type: "POST",
        url: "/Sites/Get",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function(data) {
            if (data.isSucceded == false) {
                alert(data.message);
            } else {
                var f_InSites = $("#f_insites");
                f_InSites.empty();
                f_InSites.append(
                    $("<option />")
                        .val(0)
                        .text(dictionary.SelectSite)
                );
                $.each(data.data, function(i, val) {
                    f_InSites.append(
                        $("<option />")
                            .val(val.GUID)
                            .text(val.Name)
                    );
                });
            }
        },
        error: function(xhr, ajaxOptions, thrownError) {
            console.log(xhr.responseText);
        }
    });
}

function LoadOutSites() {
    $.ajax({
        type: "POST",
        url: "/Sites/Get",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function(data) {
            if (data.isSucceded == false) {
                alert(data.message);
            } else {
                var f_outSites = $("#f_outsites");
                f_outSites.empty();
                f_outSites.append(
                    $("<option />")
                        .val(0)
                        .text(dictionary.SelectSite)
                );
                $.each(data.data, function(i, val) {
                    f_outSites.append(
                        $("<option />")
                            .val(val.GUID)
                            .text(val.Name)
                    );
                });
            }
        },
        error: function(xhr, ajaxOptions, thrownError) {
            console.log(xhr.responseText);
        }
    });
}

function LoadVehicles() {
    $.ajax({
        type: "POST",
        url: "/Vehicles/Get",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function(data) {
            if (data.isSucceded == false) {
                alert(data.message);
            } else {
                var f_vehicles = $("#f_vehicles");
                f_vehicles.empty();
                f_vehicles.append(
                    $("<option />")
                        .val(0)
                        .text(dictionary.SelectVehicle)
                );
                $.each(data.data, function(i, val) {
                    f_vehicles.append(
                        $("<option />")
                            .val(val.GUID)
                            .text(val.LicenseNumber)
                            .attr("tare", val.Tare)
                    );
                });
            }
        },
        error: function(xhr, ajaxOptions, thrownError) {
            console.log(xhr.responseText);
        }
    });
}

function LoadItems() {
    $.ajax({
        type: "POST",
        url: "/Items/Get",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function(data) {
            if (data.isSucceded == false) {
                alert(data.message);
            } else {
                var f_items = $("#f_items");             
                f_items.empty();
                f_items.append(
                    $("<option />")
                        .val(0)
                        .text(dictionary.Selectitem)
                );
                $.each(data.data, function(i, val) {
                    f_items.append(
                        $("<option />")
                            .val(val.GUID)
                            .text(val.Name)
                    );
                });
            }
        },
        error: function(xhr, ajaxOptions, thrownError) {
            console.log(xhr.responseText);
        }
    });
}

function LoadTransports() {
    $.ajax({
        type: "POST",
        url: "/Transports/Get",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function(data) {
            if (data.isSucceded == false) {
                alert(data.message);
            } else {
                var f_transports = $("#f_transports");
                f_transports.empty();
                f_transports.append(
                    $("<option />")
                        .val(0)
                        .text(dictionary.SelectTransport)
                );
                $.each(data.data, function(i, val) {
                    f_transports.append(
                        $("<option />")
                            .val(val.GUID)
                            .text(val.Name)
                    );
                });
            }
        },
        error: function(xhr, ajaxOptions, thrownError) {
            console.log(xhr.responseText);
        }
    });
}

function LoadDrivers() {
    $.ajax({
        type: "POST",
        url: "/Drivers/Get",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function(data) {
            if (data.isSucceded == false) {
                alert(data.message);
            } else {
                var f_drivers = $("#f_drivers");
                f_drivers.empty();
                f_drivers.append(
                    $("<option />")
                        .val(0)
                        .text(dictionary.SelectDriver)
                );
                $.each(data.data, function(i, val) {
                    f_drivers.append(
                        $("<option />")
                            .val(val.GUID)
                            .text(val.Name)
                    );
                });
            }
        },
        error: function(xhr, ajaxOptions, thrownError) {
            console.log(xhr.responseText);
        }
    });
}

function Filter() {
    this.Customers = $("#f_customers").val();
    this.Vehicles = $("#f_vehicles").val();
    this.Transports = $("#f_transports").val();
    this.InSites = $("#f_insites").val();
    this.OutSites = $("#f_outsites").val();
    this.Drivers = $("#f_drivers").val();    
    this.Items = $("#f_items").val(); 
    this.IsCancelled = $("#f_is_cancelled").is(":checked");
    if (this.IsCancelled === undefined || !this.IsCancelled)
        this.IsCancelled = false;
    this.FromOutDate = ToServerDate($("#f_from_out_date").val(), 'DD/MM/YYYY');
    this.ToOutDate = ToServerDate($("#f_to_out_date").val(), 'DD/MM/YYYY');       
}




function evacuationReport(dataObjectsArr) {
    const dataPivotByCustomerName = {};    

    for (const record of dataObjectsArr) {
        let { CustomerName, CustomerCode, Netto, GUID } = record; //Destructuring the record object
    
        if (dataPivotByCustomerName[CustomerName] && !dataPivotByCustomerName[CustomerName].Netto) {
            continue;
        }

        dataPivotByCustomerName[CustomerName] = {
        CustomerName,
        CustomerCode,
        Netto,
        Appearances: 1};

        const currentCustomerName = CustomerName;
        const currentGuid = GUID;
        for (const item of dataObjectsArr) {
            const { CustomerName, CustomerCode, Netto, GUID } = item; //Destructuring the record object
            if (dataPivotByCustomerName[currentCustomerName].CustomerName === CustomerName && currentGuid !== GUID) {
            dataPivotByCustomerName[CustomerName].Appearances++;
            dataPivotByCustomerName[CustomerName].Netto += Netto;
        }
    }
}

const dataForCsv = [];
    for (const item in dataPivotByCustomerName) {
        dataForCsv.push(dataPivotByCustomerName[item]);
    }
    return dataForCsv;
}